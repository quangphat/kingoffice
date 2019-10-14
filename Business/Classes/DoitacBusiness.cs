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
    public class DoitacBusiness : BaseBusiness,IDoitacBusiness
    {
        protected readonly IDoitacRepository _rpDoitac;
        public DoitacBusiness(CurrentProcess process,IDoitacRepository doitacRepository):base(null,process)
        {
            _rpDoitac = doitacRepository;
        }
        public async Task<List<OptionSimpleModel>> GetListSimple()
        {
            return await _rpDoitac.GetListSimple();
        }
    }
}
