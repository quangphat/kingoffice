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
    public class RoleBusiness :BaseBusiness, IRoleBusiness
    {
        protected readonly IRoleRepository _rpRole;
        public RoleBusiness(CurrentProcess process, IRoleRepository roleRepository):base(null,process)
        {
            _rpRole = roleRepository;
        }
        public async Task<List<OptionSimple>> GetList()
        {
            return await _rpRole.GetList();
        }
    }
}
