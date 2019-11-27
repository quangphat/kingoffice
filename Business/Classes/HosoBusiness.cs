using AutoMapper;
using Business.Infrastructures;
using Business.Interfaces;
using Entity;
using Entity.DatabaseModels;
using Entity.Enums;
using Entity.Infrastructures;
using Entity.ViewModels;
using Microsoft.AspNetCore.Http;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using Entity.PostModel;

namespace Business.Classes
{
    public class HosoBusiness : BaseBusiness, IHosoBusiness
    {
        protected readonly IHosoRepository _rpHoso;
        protected readonly IProductRepository _rpProduct;
        protected readonly IAutoIdRepository _rpAuto;
        protected readonly INotesRepository _rpNotes;
        protected readonly ITailieuRepository _rpTailieu;
        protected IServiceProvider _serviceProvider;
        public HosoBusiness(CurrentProcess process,
            IHosoRepository hosoRepository,
            IProductRepository productRepository,
            IAutoIdRepository autoIdRepository,
            INotesRepository notesRepository,
            IServiceProvider serviceProvider,
            ITailieuRepository tailieuRepository,
            IMapper mapper) : base(mapper, process)
        {
            _mapper = mapper;
            _rpHoso = hosoRepository;
            _rpProduct = productRepository;
            _rpAuto = autoIdRepository;
            _rpTailieu = tailieuRepository;
            _rpNotes = notesRepository;
            _serviceProvider = serviceProvider;
        }
        public async Task<bool> DuyetHoso(int hosoId, HosoModel model)
        {
            if (_process.User == null)
            {
                AddError(errors.error_login_expected);
                return false;
            }
            if (model == null || model.Id <= 0)
            {
                AddError(errors.invalid_data);
                return false;
            }
            if (string.IsNullOrWhiteSpace(model.CustomerName))
            {
                AddError(errors.missing_name);
                return false;
            }
            if (string.IsNullOrWhiteSpace(model.Phone))
            {
                AddError(errors.missing_phone);
                return false;
            }

            if (string.IsNullOrWhiteSpace(model.Cmnd))
            {
                AddError(errors.missing_cmnd);
                return false;
            }
            if (string.IsNullOrWhiteSpace(model.Address))
            {
                AddError(errors.missing_diachi);
                return false;
            }
            if (model.DistrictId <= 0)
            {
                AddError(errors.missing_district);
                return false;
            }
            if (model.ProductId <= 0)
            {
                AddError(errors.missing_product);
                return false;
            }
            if (model.BorrowAmount <= 0)
            {
                AddError(errors.missing_money);
                return false;
            }
            var lstLoaiTailieu = await _rpTailieu.GetLoaiTailieuList();
            if (lstLoaiTailieu != null)
            {
                var missingNames = BusinessExtension.GetFilesMissingV2(lstLoaiTailieu, model.FileRequireIds);
                if (!string.IsNullOrWhiteSpace(missingNames))
                {
                    AddError($"{errors.missing_must_have_files} {missingNames}");
                    return false;
                }
            }
            var old = _rpHoso.GetHosoById(model.Id);
            if (old != null)
            {
                await _rpHoso.AddHosoDaxem(model.Id);
            }
            var checkProductInUse = await _rpProduct.CheckIsInUse(model.Id, model.ProductId);
            if (checkProductInUse)
            {
                AddError(errors.product_code_inuse);
                return false;
            }
            else
            {
                await _rpProduct.UpdateUse(model.Id, model.ProductId);
            }

            model.UpdateBy = _process.User.Id;

            var result = await _rpHoso.DuyetHoso(model);
            if (result == true)
            {
                await _rpHoso.UpdateStatus(model.Id, _process.User.Id, model.Status, model.Result, "");
                await AddNote(model.Id, model.Comment);
                return true;
            }
            return true;
        }
        public async Task<bool> RemoveTailieu(int hosoId, int tailieuId)
        {
            if (tailieuId <= 0)
            {
                AddError(errors.invalid_data);
                return false;
            }
            return await _rpTailieu.RemoveTailieu(hosoId, tailieuId);
        }
        public async Task<List<HosoTailieu>> GetTailieuByHosoId(int hosoId)
        {
            if (hosoId <= 0)
            {
                AddError(errors.invalid_data);
                return null;
            }
            var lstLoaiTailieu = await _rpTailieu.GetLoaiTailieuList();
            if (lstLoaiTailieu == null || !lstLoaiTailieu.Any())
                return null;
            var tailieuByHoso = await _rpTailieu.GetTailieuByHosoId(hosoId);
            var result = new List<HosoTailieu>();

            foreach (var loai in lstLoaiTailieu)
            {
                var tailieus = tailieuByHoso.Where(p => p.Key == loai.ID);

                var item = new HosoTailieu
                {
                    ID = loai.ID,
                    Ten = loai.Ten,
                    BatBuoc = loai.BatBuoc,
                    Tailieus = tailieus != null ? tailieus.ToList() : new List<FileUploadModel>()
                };
                result.Add(item);

            }

            return result;
        }
        public async Task<List<GhichuViewModel>> GetComments(int hosoId)
        {
            if (hosoId <= 0)
            {
                AddError(errors.invalid_data);
                return null;
            }
            return await _rpHoso.GetComments(hosoId);
        }
        public async Task<List<OptionSimple>> GetStatusList()
        {
            if (_process.User == null || _process.User.Permissions == null || !_process.User.Permissions.Any())
            {
                AddError(errors.invalid_data);
                return null;
            }
            var isTeamlead = _process.User.Permissions.Contains("teamlead") && !_process.User.Permissions.Contains("admin") ? true : false;
            var statusList = await _rpHoso.GetStatusList(isTeamlead);
            var result = _mapper.Map<List<OptionSimple>>(statusList);
            return result;
        }
        public async Task<List<OptionSimple>> GetResultList()
        {
            if (_process.User == null || _process.User.Permissions == null || !_process.User.Permissions.Any())
            {
                AddError(errors.invalid_data);
                return null;
            }
            var resultList = await _rpHoso.GetResultList();
            var result = _mapper.Map<List<OptionSimple>>(resultList);
            return result;
        }
        public async Task<HoSoInfoModel> GetById(int hosoId)
        {
            if (hosoId <= 0)
            {
                AddError(errors.invalid_data);
                return null;
            }
            var result = await _rpHoso.GetHosoById(hosoId);
            if (result != null)
            {
                await _rpHoso.Daxem(hosoId);
            }
            return result;
        }
        public async Task<long> Save(HosoModel model, bool isDraft)
        {
            if(model == null)
            {
                AddError(errors.invalid_data);
                return 0;
            }
            if (!string.IsNullOrWhiteSpace(model.Comment) && model.Comment.Length > 200)
            {
                AddError(errors.note_length_cannot_more_than_200);
                return 0;
            }
            if (model.PartnerId <= 0)
            {
                AddError(errors.missing_partner);
                return 0;
            }
            if (!isDraft)
            {
                var lstLoaiTailieu = await _rpTailieu.GetLoaiTailieuList();
                if (lstLoaiTailieu != null)
                {
                    var missingNames = BusinessExtension.GetFilesMissingV2(lstLoaiTailieu, model.FileRequireIds);
                    if (!string.IsNullOrWhiteSpace(missingNames))
                    {
                        AddError(missingNames);
                        return 0;
                    }
                }
            }
            
            model.Status = isDraft == true ? (int)TrangThaiHoSo.Nhap : (int)TrangThaiHoSo.NhapLieu;
            model.Result = (int)KetQuaHoSo.Trong;
            var hoso = _mapper.Map<HosoModel>(model);
            var validate = validateHoso(hoso, isDraft);

            if (validate.result == false)
            {
                AddError(validate.message);
                return 0;
            }

            //update
            if (model.Id > 0)
            {
                model.UpdateBy = _process.User.Id;
                await Update(hoso);
                await AddNote(hoso.Id, model.Comment);
                return hoso.Id;
            }
            //create
            var hosoId =  await Create(hoso);
            if (hosoId > 0)
            {
                await AddNote(hosoId, model.Comment);
            }
            return hosoId;
        }
        public async Task<int> Update(HosoModel hoso)
        {
            if (hoso.Status == (int)TrangThaiHoSo.NhapLieu
                || hoso.Status == (int)TrangThaiHoSo.BoSungHoSo
                || hoso.Status == (int)TrangThaiHoSo.GiaiNgan
                || hoso.Status == (int)TrangThaiHoSo.ThamDinh
                || hoso.Status == (int)TrangThaiHoSo.TuChoi
                || hoso.Status == (int)TrangThaiHoSo.Nhap)
            {
                var checkProductInUse = await _rpProduct.CheckIsInUse(hoso.Id, hoso.ProductId);
                if (checkProductInUse)
                {
                    AddError(errors.product_code_inuse);
                    return hoso.Id;
                }
                else
                {
                    await _rpProduct.UpdateUse(hoso.Id, hoso.ProductId);
                }
            }
            hoso.UpdateBy = _process.User.Id;
            var result = await _rpHoso.Update(hoso);
            if (result)
            {
                await _rpHoso.CreateHosoDuyetXem(hoso.Id);
            }
            return hoso.Id;
        }
        public async Task<int> Create(HosoModel hoso)
        {
            var autoId = await _rpAuto.GetAutoId((int)AutoID.HoSo);
            if (autoId == null)
                return 0;
            hoso.HoSoCuaAi = _process.User.Id;
            hoso.CreatedBy = _process.User.Id;
            hoso.Code = BusinessExtension.GenerateAutoCode(ref autoId);
            var hosoId = await _rpHoso.Create(hoso);
            if (hosoId > 0)
            {
                await _rpAuto.Update(autoId);
            }
            return hosoId;
        }
        public async Task<(List<HosoDuyet> datas, int TotalRecord)> GetHosoDuyet(string fromDate,
            string toDate,
            string maHS = "",
            string cmnd = "",
            int loaiNgay = 1,
            int maNhom = 0,
            string freetext = "",
            int page = 1, int limit = 10,
            int maThanhVien = 0)
        {
            if (!string.IsNullOrWhiteSpace(freetext) && freetext.Length > 50)
            {
                AddError("Từ khóa tìm kiếm không được nhiều hơn 50 ký tự");
                return (null, 0);
            }
            int totalRecord = 0;
            DateTime dtFromDate = DateTime.MinValue, dtToDate = DateTime.MinValue;
            if (fromDate != "")
                dtFromDate = fromDate.ConvertddMMyyyyToDateTime();
            if (toDate != "")
                dtToDate = toDate.ConvertddMMyyyyToDateTime();
            maHS = string.IsNullOrWhiteSpace(maHS) ? "" : maHS;
            cmnd = string.IsNullOrWhiteSpace(cmnd) ? "" : cmnd;
            string status = BusinessExtension.JoinHosoStatus();
            totalRecord = await CountHosoDuyet(_process.User.Id, maNhom,
                maThanhVien, dtFromDate, dtToDate, maHS, cmnd, loaiNgay, status, freetext);
            var datas = await GetHosoDuyet(_process.User.Id, maNhom, maThanhVien,
                dtFromDate, dtToDate, maHS, cmnd, loaiNgay, status, page, limit, freetext);
            return (datas, totalRecord);
        }
        public async Task<List<HosoDuyet>> GetHosoNotApprove()
        {
            DateTime tuNgay = DateTime.Now.AddDays(-50);
            DateTime denNgay = DateTime.Now.AddDays(10);
            string trangthai = "";
            trangthai += ((int)TrangThaiHoSo.TuChoi).ToString() + ","
                + ((int)TrangThaiHoSo.NhapLieu).ToString() + ","
                + ((int)TrangThaiHoSo.ThamDinh).ToString() + ","
                + ((int)TrangThaiHoSo.BoSungHoSo).ToString() + ","
                + ((int)TrangThaiHoSo.GiaiNgan).ToString();
            var result = await _rpHoso.GetHosoNotApprove(_process.User.Id,
                0,
                0,
                tuNgay,
                denNgay, string.Empty, string.Empty, 1, trangthai);
            return result;
        }

        private async Task<int> CountHosoDuyet(int maNVDangNhap,
            int maNhom,
            int maThanhVien,
            DateTime tuNgay,
            DateTime denNgay,
            string maHS,
            string cmnd,
            int loaiNgay,
            string trangThai,
            string freeText)
        {
            var result = 0;
            string query = string.IsNullOrWhiteSpace(freeText) ? string.Empty : freeText;
            result = await _rpHoso.CountHosoDuyet(maNVDangNhap,
                maNhom,
                maThanhVien,
                tuNgay,
                denNgay,
                maHS, cmnd, loaiNgay, trangThai, freeText);
            return result;
        }
        public async Task<bool> UploadHoso(int hosoId, List<FileUploadModel> files, string rootPath)
        {
            if (hosoId <= 0 || files == null || !files.Any())
            {
                AddError(errors.invalid_data);
                return false;
            }

            foreach (var item in files)
            {
                var tailieu = new TaiLieu
                {
                    FileName = item.FileName,
                    FilePath = item.FileUrl,
                    HosoId = hosoId,
                    TypeId = Convert.ToInt32(item.Key)
                };
                await _rpTailieu.Add(tailieu);
            }
            return true;
        }
        public async Task<bool> UploadHoso(int hosoId, List<FileUploadModelGroupByKey> fileGroups, string rootPath, bool deleteAllExist = false)
        {
            if (fileGroups == null || !fileGroups.Any())
                return true;
            if (string.IsNullOrWhiteSpace(rootPath))
            {
                AddError(errors.missing_rootpath);
                return false;
            }
            if(deleteAllExist)
            {
                var deleteAll = await _rpTailieu.RemoveAllTailieu(hosoId);
                if (!deleteAll)
                    return false;
            }
            
            var result = false;
            foreach (var item in fileGroups)
            {
                if (item.files.Any())
                {
                    result = await UploadHoso(hosoId, item.files, rootPath);
                }
            }
            return result;
        }
        public async Task<bool> UploadHoso(int hosoId, int key, List<IFormFile> files, string rootPath, bool deleteExist = false)
        {
            if (files == null || !files.Any())
            {
                return true;
            }
            if (hosoId <= 0 || key <= 0)
            {
                AddError(errors.invalid_data);
                return false;
            }
            var deleted = false;
            deleted = deleteExist ? await _rpTailieu.RemoveAllTailieu(hosoId) : true;
            if (!deleted)
                return false;
            IMediaBusiness bizMedia = _serviceProvider.GetService<IMediaBusiness>();
            foreach (var file in files)
            {
                if (!BusinessExtension.IsNotValidFileSize(file.Length))
                {
                    using (var stream = new MemoryStream())
                    {
                        await file.CopyToAsync(stream);
                        var result = await bizMedia.Upload(stream, key.ToString(), file.FileName, rootPath);
                        if (!string.IsNullOrWhiteSpace(result))
                        {
                            var tailieu = new TaiLieu
                            {
                                FileName = file.FileName,
                                FilePath = result,
                                HosoId = hosoId,
                                TypeId = key
                            };
                            await _rpTailieu.Add(tailieu);
                        }
                    }
                }
            }
            return true;
        }
        public async Task AddNote(int hosoId, string ghiChu)
        {

            if (string.IsNullOrWhiteSpace(ghiChu))
                return;
            GhichuModel ghichu = new GhichuModel
            {
                UserId = _process.User.Id,
                HosoId = hosoId,
                Noidung = ghiChu,
                CommentTime = DateTime.Now
            };
            await _rpNotes.Add(ghichu);
        }
        public async Task<(List<HoSoQuanLyModel> datas, int totalRecord)> GetDanhsachHoso(
            string maHs,
            string cmnd,
            DateTime? fromDate,
            DateTime? toDate,
            int loaiNgay = 1,
            int nhomId = 0,
            int userId = 0,
            string freetext = null,
            string status = null,
            int page = 1, int limit = 10)
        {
            if (!string.IsNullOrWhiteSpace(freetext) && freetext.Length > 30)
            {
                AddError(errors.freetext_length_cannot_lagger_30);
                return (null, 0);
            }
            var fDate = fromDate == null ? DateTime.Now : fromDate.Value;
            var tDate = toDate == null ? DateTime.Now : toDate.Value;
            BusinessExtension.ProcessPaging(page, ref limit);
            freetext = string.IsNullOrWhiteSpace(freetext) ? string.Empty : freetext.Trim();
            string trangthai = string.IsNullOrWhiteSpace(status) ? BusinessExtension.GetLimitStatusString() : status;
            userId = userId <= 0 ? _process.User.Id : userId;
            var totalRecord = await _rpHoso.CountDanhsachHoso(_process.User.Id, nhomId, userId, fDate, tDate, maHs, cmnd, trangthai, loaiNgay, freetext);
            var datas = await _rpHoso.GetDanhsachHoso(_process.User.Id, nhomId, userId, fDate, tDate, maHs, cmnd, trangthai, loaiNgay, freetext, page, limit);
            return (datas, totalRecord);
        }
        private async Task<List<HosoDuyet>> GetHosoDuyet(int maNVDangNhap,
            int maNhom,
            int maThanhVien,
            DateTime tuNgay,
            DateTime denNgay,
            string maHS,
            string cmnd,
            int loaiNgay,
            string trangThai,
            int page = 1, int limit = 10,
            string freeText = null, bool isDowload = false)
        {
            page = page <= 0 ? 1 : page;
            if (!isDowload)
                limit = (limit <= 0 || limit >= Constanst.Limit_Max_Page) ? Constanst.Limit_Max_Page : limit;
            int offset = (page - 1) * limit;
            string query = string.IsNullOrWhiteSpace(freeText) ? string.Empty : freeText;
            var result = await _rpHoso.GetHosoDuyet(maNVDangNhap,
                maNhom
                , maThanhVien,
                tuNgay,
                denNgay,
                maHS,
                cmnd,
                loaiNgay,
                trangThai, freeText, offset, limit, isDowload);
            return result;
        }
        private (bool result, string message) validateHoso(HosoModel hoso, bool isDraft)
        {
            if (hoso == null)
            {
                return (false, errors.invalid_data);
            }
            if (string.IsNullOrWhiteSpace(hoso.CustomerName))
            {
                return (false, errors.customername_must_not_be_empty);
            }
            if (hoso.ProductId <= 0)
            {
                return (false, errors.missing_product);
            }
            if (!isDraft)
            {
                if (string.IsNullOrWhiteSpace(hoso.Phone))
                {
                    return (false, errors.missing_phone);
                }
                if (hoso.NgayNhandon == null)
                {
                    return (false, errors.missing_ngaynhandon);
                }
                if (string.IsNullOrWhiteSpace(hoso.Cmnd))
                {
                    return (false, errors.missing_cmnd);
                }
                if (hoso.DistrictId <= 0)
                {
                    return (false, errors.missing_district);
                }
                if (string.IsNullOrWhiteSpace(hoso.Address))
                {
                    return (false, errors.missing_diachi);
                }
               
                if (hoso.ProductId <= 0)
                {
                    return (false, errors.missing_product);
                }
                if (hoso.BorrowAmount <= 0)
                {
                    return (false, errors.missing_money);
                }
                if (hoso.BirthDay == null)
                {
                    return (false, errors.missing_birthday);
                }
                if (hoso.CmndDay == null)
                {
                    return (false, errors.missing_cmnd_day);
                }
            }

            return (true, string.Empty);
        }


    }
}
