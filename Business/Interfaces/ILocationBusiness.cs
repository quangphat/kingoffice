using Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface ILocationBusiness
    {
        Task<List<OptionSimpleModelOld>> GetProvinceSimpleList();
        Task<List<OptionSimpleModelOld>> GetDistrictSimpleListByProvinceId(int provinceId);

    }
}
