using Entity.ViewModels;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface ILocationRepository
    {
        Task<List<OptionSimpleModelOld>> GetListProvinceSimple();
        Task<List<OptionSimpleModelOld>> GetListDistrictSimple(int provinceId);
    }
}

