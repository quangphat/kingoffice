using Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IRoleRepository
    {
        Task<List<OptionSimple>> GetList();
    }
}

