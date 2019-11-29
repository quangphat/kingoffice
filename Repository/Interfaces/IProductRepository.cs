using Entity.DatabaseModels;
using Entity.ViewModels;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IProductRepository
    {
        Task<bool> Delete(int id);
        Task<List<ProductDetailViewModel>> GetListByDate(int partnerId, DateTime createdDate);
        Task<int> Insert(Product product);
        Task<List<OptionSimpleExtendForProduct>> GetAllList(int doitacId = 0);
        Task<List<OptionSimpleModelOld>> GetListByDoitacId(int doitacId);
        Task<List<ProductViewModel>> GetListByHosoId(int doitaId, int hosoId);
        Task<bool> CheckIsInUse(int hosoId, int productId);
        Task<bool> UpdateUse(int hosoId, int productId);
    }
}

