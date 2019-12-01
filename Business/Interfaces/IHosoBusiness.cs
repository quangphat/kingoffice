using Entity.DatabaseModels;
using Entity.PostModel;
using Entity.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IHosoBusiness
    {
        Task<string> Download(int maNhom,
           int maThanhVien,
           DateTime? tuNgay,
           DateTime? denNgay,
           string maHS,
           string cmnd,
           int loaiNgay,
           string trangThai,
           string freeText,
           string webRoothPath, int exportType = 0);
        Task<bool> DuyetHoso(int hosoId, HosoModel model);
        Task<bool> RemoveTailieu(int hosoId, int tailieuId);
        Task<List<HosoTailieu>> GetTailieuByHosoId(int hosoId);
        Task<List<OptionSimple>> GetResultList();
        Task<List<OptionSimple>> GetStatusList();
        Task<List<GhichuViewModel>> GetComments(int hosoId);
        Task<(List<HoSoQuanLyModel> datas, int TotalRecord)> GetHosoDuyet(DateTime? fromDate,
            DateTime? toDate,
            string maHS = "",
            string cmnd = "",
            int loaiNgay = 1,
            int maNhom = 0,
            string status = null,
            string freetext = "",
            int page = 1, int limit = 10,
            int maThanhVien = 0);
        Task<List<HoSoQuanLyModel>> GetHosoNotApprove();
        Task<long> Save(HosoModel model, bool isDraft);
        Task<bool> UploadHoso(int hosoId, List<FileUploadModel> files, string rootPath);
        Task<bool> UploadHoso(int hosoId, List<FileUploadModelGroupByKey> fileGroups, string rootPath, bool isReset = false);
        //Task<bool> UploadHoso(int hosoId, int key, List<IFormFile> files, string rootPath);
        Task<bool> UploadHoso(int hosoId, int key, List<IFormFile> files, string rootPath, bool deleteExist = false);
        Task<(List<HoSoQuanLyModel> datas, int totalRecord)> GetDanhsachHoso(string maHs,
            string cmnd,
            DateTime? fromDate,
            DateTime? toDate,
            int loaiNgay = 1,
            int nhomId = 0,
            int userId = 0,
            string freetext = null,
            string status = null,
            int page = 1, int limit = 10);
        Task<HoSoInfoModel> GetById(int hosoId);
        //Task UpdateDaxem(int hosoId)
    }
}
