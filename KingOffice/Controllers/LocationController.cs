using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Interfaces;
using Entity.Infrastructures;
using Microsoft.AspNetCore.Mvc;

namespace KingOffice.Controllers
{
    public class LocationController : BaseController
    {
        protected readonly ILocationBusiness _bizLocation;
        public LocationController(CurrentProcess process,ILocationBusiness locationBusiness) : base(process)
        {
            _bizLocation = locationBusiness;
        }
        public async Task<IActionResult> GetProvinceSimpleList()
        {
            var result = await _bizLocation.GetProvinceSimpleList();
            return ToResponse(result);
        }
        public async Task<IActionResult> GetDistrictSimpleList(int provinceId)
        {
            var result = await _bizLocation.GetDistrictSimpleListByProvinceId(provinceId);
            return ToResponse(result);
        }
    }
}