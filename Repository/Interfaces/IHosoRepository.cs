using Entity.DatabaseModels;
using Entity.PostModel;
using Entity.ViewModels;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IHosoRepository
    {
        
        Task<bool> UpdateStatus(int hosoId, int userId, int status, int result, string comment);
        Task<bool> DuyetHoso(HosoModel model);
        Task<List<OptionSimpleModelOld>> GetResultList();
        Task<List<OptionSimpleModelOld>> GetStatusList(bool isTeamlead);
        Task<List<GhichuViewModel>> GetComments(int hosoId);
        Task<bool> Update(HosoModel model);
        Task<HoSoInfoModel> GetHosoById(int hosoId);
        Task<int> Create(HosoModel model);
        Task<int> CountHosoDuyet(int maNVDangNhap,
            int maNhom,
            int maThanhVien,
            DateTime tuNgay,
            DateTime denNgay,
            string maHS,
            string cmnd,
            int loaiNgay,
            string trangThai,
            string freeText = null);
        Task<List<HoSoQuanLyModel>> GetHosoDuyet(int maNVDangNhap,
            int maNhom,
            int maThanhVien,
            DateTime tuNgay,
            DateTime denNgay,
            string maHS,
            string cmnd,
            int loaiNgay,
            string trangThai,
            string freeText,
            int offset,
            int limit,
            bool isDowload = false);
        Task<List<HoSoQuanLyModel>> GetHosoNotApprove(int userId,
            int maNhom,
            int maThanhvien,
            DateTime fromDate,
            DateTime toDate,
            string maHs,
            string cmnd,
            int dateType,
            string status
            );
        Task<bool> CreateHosoDuyetXem(int hosoId);
        Task<int> CountDanhsachHoso(int loginUserId,
            int maNhom,
            int userId,
            DateTime fromDate,
            DateTime toDate,
            string maHs,
            string cmnd,
            string trangthai,
            int loaiNgay,
            string freeText = null);
        Task<List<HoSoQuanLyModel>> GetDanhsachHoso(int loginUserId,
                int maNhom,
                int userId,
                DateTime fromDate,
                DateTime toDate,
                string maHs,
                string cmnd,
                string trangthai,
                int loaiNgay,
                string freeText = null,
                int offset = 0,
                int limit = 10,
                bool isDownload = false);
        Task Daxem(int hosoId);
        Task AddHosoDaxem(int hosoId);
    }
}
