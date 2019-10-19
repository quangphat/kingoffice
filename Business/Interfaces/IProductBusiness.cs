using Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IProductBusiness
    {
        Task<List<ProductViewModel>> GetListByDoitacId(int doitacId);
        Task<List<ProductViewModel>> GetListByHosoId(int doitacId, int hosoId);
    }
}
