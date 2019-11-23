using Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface ITailieuBusiness
    {
        Task<List<LoaiTaiLieuModel>> GetList();
        Task<List<FileUploadModel>> GetTailieuByHosoId(int hosoId);
    }
}
