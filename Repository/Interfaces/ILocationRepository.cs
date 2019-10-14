using Entity.ViewModels;
using Repository.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface ILocationRepository
    {
        Task<List<OptionSimpleModel>> GetListProvinceSimple();
        Task<List<OptionSimpleModel>> GetListDistrictSimple(int provinceId);
    }
}

