using Entity.ViewModels;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface ITailieuRepository
    {
        Task<bool> UpdateExistingFile(int fileId, string name, string url);
        Task<List<LoaiTaiLieuModel>> GetLoaiTailieuList();
        Task<bool> Add(TaiLieu model);
        Task<bool> RemoveAllTailieu(int hosoId);
        Task<List<FileUploadModel>> GetTailieuByHosoId(int hosoId);
        Task<bool> RemoveTailieu(int hosoId, int tailieuId);
    }
}

