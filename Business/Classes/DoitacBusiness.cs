using AutoMapper;
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
        public DoitacBusiness(CurrentProcess process,IDoitacRepository doitacRepository, IMapper mapper):base(mapper,process)
        {
            _rpDoitac = doitacRepository;
        }
        public async Task<List<OptionSimple>> GetListSimple()
        {
            var datas = await _rpDoitac.GetListSimple();
            return _mapper.Map<List<OptionSimple>>(datas);
        }
    }
}
