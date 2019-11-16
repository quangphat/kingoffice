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
        Task<(List<HosoDuyet> datas, int TotalRecord)> GetHosoDuyet(string fromDate,
            string toDate,
            string maHS = "",
            string cmnd = "",
            int loaiNgay = 1,
            int maNhom = 0,
            string freetext = "",
            int page = 1, int limit = 10,
            int maThanhVien = 0);
        Task<List<HosoDuyet>> GetHosoNotApprove();
        Task<long> Save(HosoRequestModel model, bool isDraft);
        Task<bool> UploadHoso(int hosoId, List<FileUploadModel> files, string rootPath);
        //Task<bool> UploadHoso(int hosoId, int key, List<IFormFile> files, string rootPath);
        Task<bool> UploadHoso(int hosoId, int key, List<IFormFile> files, string rootPath, bool deleteExist = false);
    }
}
