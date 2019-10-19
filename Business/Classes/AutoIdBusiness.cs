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
    public class AutoIdBusiness :BaseBusiness, IAutoIdBusiness
    {
        public AutoIdBusiness(CurrentProcess process):base(null,process)
        {

        }
    }
}
