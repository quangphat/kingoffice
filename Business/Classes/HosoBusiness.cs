using AutoMapper;
using Business.Infrastructures;
using Business.Interfaces;
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
    public class HosoBusiness :BaseBusiness, IHosoBusiness
    {
        protected readonly IMapper _mapper;
        protected readonly IHosoRepository _rpHoso;
        public HosoBusiness(CurrentProcess process,
            IHosoRepository hosoRepository,
            IMapper mapper):base(mapper,process)
        {
            _mapper = mapper;
            _rpHoso = hosoRepository;
        }
        public async Task<(List<HosoDuyet> datas, int TotalRecord)> GetHosoDuyet(string fromDate,
            string toDate,
            string maHS= "",
            string cmnd ="",
            int loaiNgay = 1,
            int maNhom = 0,
            string freetext = "",
            int page = 1, int limit = 10,
            int maThanhVien = 0)
        {
            if(!string.IsNullOrWhiteSpace(freetext) && freetext.Length>50)
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
                maThanhVien, dtFromDate, dtToDate, maHS,cmnd, loaiNgay, status, freetext);
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
    }
}
