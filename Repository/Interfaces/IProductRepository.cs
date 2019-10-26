using Entity.ViewModels;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IProductRepository
    {
        Task<List<ProductViewModel>> GetListByDoitacId(int doitacId);
        Task<List<ProductViewModel>> GetListByHosoId(int doitaId, int hosoId);
        Task<bool> CheckIsInUse(int hosoId, int productId);
        Task<bool> UpdateUse(int hosoId, int productId);
    }
}

