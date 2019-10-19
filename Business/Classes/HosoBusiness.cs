using AutoMapper;
using Business.Infrastructures;
using Business.Interfaces;
using Entity;
using Entity.DatabaseModels;
using Entity.Enums;
using Entity.Infrastructures;
using Entity.ViewModels;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Classes
{
    public class HosoBusiness : BaseBusiness, IHosoBusiness
    {
        protected readonly IMapper _mapper;
        protected readonly IHosoRepository _rpHoso;
        protected readonly IProductRepository _rpProduct;
        protected readonly IAutoIdRepository _rpAuto;
        public HosoBusiness(CurrentProcess process,
            IHosoRepository hosoRepository,
            IProductRepository productRepository,
            IAutoIdRepository autoIdRepository,
            IMapper mapper) : base(mapper, process)
        {
            _mapper = mapper;
            _rpHoso = hosoRepository;
            _rpProduct = productRepository;
            _rpAuto = autoIdRepository;
        }
        public async Task<long> Save(HosoRequestModel model, bool isDraft)
        {
            if (!string.IsNullOrWhiteSpace(model.Ghichu) && model.Ghichu.Length > 200)
            {
                AddError(errors.note_length_cannot_more_than_200);
                return 0;
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
                return await Update(hoso);
            }
            return await Create(hoso);
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
                await _rpHoso.RemoveAllTailieu(hoso.ID);
            }
            return hoso.ID;
        }
        public async Task<long> Create(HosoModel hoso)
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
            string status = BusinessExtension.JoinTrangThai();
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
            if(!isDraft)
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
