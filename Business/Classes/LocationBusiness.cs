using Business.Interfaces;
using Entity.Infrastructures;
using Entity.ViewModels;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Classes
{
    public class LocationBusiness : BaseBusiness,ILocationBusiness
    {
        protected readonly ILocationRepository _rpLocation;
        public LocationBusiness(CurrentProcess process,ILocationRepository locationRepository):base(null,process)
        {
            _rpLocation = locationRepository;
        }
        public async Task<List<OptionSimpleModelOld>> GetProvinceSimpleList()
        {
            return await _rpLocation.GetListProvinceSimple();
        }
        public async Task<List<OptionSimpleModelOld>> GetDistrictSimpleListByProvinceId(int provinceId)
        {
            return await _rpLocation.GetListDistrictSimple(provinceId);
        }
    }
}
