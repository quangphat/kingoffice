using Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface ILocationBusiness
    {
        Task<List<OptionSimpleModel>> GetProvinceSimpleList();
        Task<List<OptionSimpleModel>> GetDistrictSimpleListByProvinceId(int provinceId);

    }
}