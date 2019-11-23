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

namespace Business.Classes
{
    public class HosoBusiness : BaseBusiness, IHosoBusiness
    {
        protected readonly IMapper _mapper;
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
        public async Task<long> Save(HosoRequestModel model, bool isDraft)
        {
            if (!string.IsNullOrWhiteSpace(model.Ghichu) && model.Ghichu.Length > 200)
            {
                AddError(errors.note_length_cannot_more_than_200);
                return 0;
            }
            if (model.Doitac <= 0)
            {
                AddError(errors.missing_partner);
                return 0;
            }
            if (!isDraft)
            {
                var lstLoaiTailieu = await _rpTailieu.GetLoaiTailieuList();
                if (lstLoaiTailieu != null)
                {
                    var missingNames = BusinessExtension.GetFilesMissing(lstLoaiTailieu, model.files);
                    if (missingNames.Length > 0)
                    {
                        AddError($"{errors.missing_must_have_files} {missingNames.ToString()}");
                        return 0;
                    }
                }
            }

            var hoso = _mapper.Map<HosoModel>(model);
            var validate = validateHoso(hoso, isDraft);

            if (validate.result == false)
            {
                AddError(validate.message);
                return 0;
            }

            model.HoSoCuaAi = _process.User.Id;
            model.MaNguoiTao = _process.User.Id;
            model.MaTrangThai = isDraft == true ? (int)TrangThaiHoSo.Nhap : (int)TrangThaiHoSo.NhapLieu;
            model.KetQuaHS = (int)KetQuaHoSo.Trong;
            if (model.ID > 0)
            {
                model.MaNguoiCapnhat = _process.User.Id;
                await Update(hoso);
            }
            var hosoId = model.ID > 0 ? model.ID : await Create(hoso);
            if (hosoId > 0)
            {
                await AddNote(hosoId, model.Ghichu);
            }
            if (!isDraft)
            {

            }
            return hosoId;
        }
        public async Task<int> Update(HosoModel hoso)
        {
            if (hoso.MaTrangThai == (int)TrangThaiHoSo.NhapLieu
                || hoso.MaTrangThai == (int)TrangThaiHoSo.BoSungHoSo
                || hoso.MaTrangThai == (int)TrangThaiHoSo.GiaiNgan
                || hoso.MaTrangThai == (int)TrangThaiHoSo.ThamDinh
                || hoso.MaTrangThai == (int)TrangThaiHoSo.TuChoi
                || hoso.MaTrangThai == (int)TrangThaiHoSo.Nhap)
            {
                var checkProductInUse = await _rpProduct.CheckIsInUse(hoso.ID, hoso.Sanphamvay);
                if (checkProductInUse)
                {
                    AddError(errors.product_code_inuse);
                    return hoso.ID;
                }
                else
                {
                    await _rpProduct.UpdateUse(hoso.ID, hoso.Sanphamvay);
                }
            }
            var result = await _rpHoso.Update(hoso);
            if (result)
            {

            }
            return hoso.ID;
        }
        public async Task<int> Create(HosoModel hoso)
        {
            var autoId = await _rpAuto.GetAutoId((int)AutoID.HoSo);
            if (autoId == null)
                return 0;
            hoso.MaHoSo = BusinessExtension.GenerateAutoCode(ref autoId);
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
        public async Task<bool> UploadHoso(int hosoId, List<FileUploadModelGroupByKey> fileGroups, string rootPath)
        {
            if (fileGroups == null || !fileGroups.Any())
                return true;
            if(string.IsNullOrWhiteSpace(rootPath))
            {
                AddError(errors.missing_rootpath);
                return false;
            }
            var deleteAll = await _rpTailieu.RemoveAllTailieu(hosoId);
            if (!deleteAll)
                return false;
            var result = false;
            foreach(var item in fileGroups)
            {
                if(item.files.Any())
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
            if(hosoId <=0 || key<=0)
            {
                AddError(errors.invalid_data);
                return false;
            }
            var deleted = false;
            deleted = deleteExist ? await _rpTailieu.RemoveAllTailieu(hosoId) : true;
            if (!deleted)
                return false;
            IMediaBusiness bizMedia = _serviceProvider.GetService<IMediaBusiness>();
            foreach(var file in files)
            {
                if(!BusinessExtension.IsNotValidFileSize(file.Length))
                {
                    using (var stream = new MemoryStream())
                    {
                        await file.CopyToAsync(stream);
                        var result = await bizMedia.Upload(stream, key.ToString(), file.FileName, rootPath);
                        if(!string.IsNullOrWhiteSpace(result))
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
            int userId =0,
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
            if (string.IsNullOrWhiteSpace(hoso.TenKhachHang))
            {
                return (false, errors.customername_must_not_be_empty);
            }
            if(hoso.Sanphamvay <=0)
            {
                return (false, errors.missing_product);
            }
            if (!isDraft)
            {
                if (string.IsNullOrWhiteSpace(hoso.SDT))
                {
                    return (false, errors.missing_phone);
                }
                if (hoso.NgayNhanDon == null)
                {
                    return (false, errors.missing_ngaynhandon);
                }
                if (string.IsNullOrWhiteSpace(hoso.CMND))
                {
                    return (false, errors.missing_cmnd);
                }
                if (string.IsNullOrWhiteSpace(hoso.DiaChi))
                {
                    return (false, errors.missing_diachi);
                }
                if (hoso.MaKhuVuc <= 0)
                {
                    return (false, errors.missing_location_code);
                }
                if (hoso.Sanphamvay <= 0)
                {
                    return (false, errors.missing_product);
                }
                if (hoso.SoTienVay <= 0)
                {
                    return (false, errors.missing_money);
                }
            }

            return (true, string.Empty);
        }

       
    }
}
