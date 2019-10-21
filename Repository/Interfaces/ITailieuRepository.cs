using Entity.ViewModels;
using Repository.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface ITailieuRepository
    {
        Task<List<LoaiTaiLieuModel>> GetLoaiTailieuList();
        Task<bool> Add(TaiLieu model);
        Task<bool> RemoveAllTailieu(int hosoId);
    }
}

