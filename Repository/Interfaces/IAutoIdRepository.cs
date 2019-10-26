using Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IAutoIdRepository
    {
        Task<AutoIDModel> GetAutoId(int type);
        Task<bool> Update(AutoIDModel model);
    }
}

