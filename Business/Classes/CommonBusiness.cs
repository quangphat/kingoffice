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
    public class CommonBusiness :BaseBusiness, ICommonBusiness
    {
        protected readonly IHosoRepository _rpHoso;
        public CommonBusiness(CurrentProcess process,
            IHosoRepository hosoRepository):base(null,process)
        {
            _rpHoso = hosoRepository;
        }
        public  async Task<bool> SendEmailHoso(int hosoId)
        {
            if (hosoId <= 0)
                return false;
            var hoso = await _rpHoso.GetHosoById(hosoId);
            if (hoso == null)
                return false;

            return true;
        }
    }
}
