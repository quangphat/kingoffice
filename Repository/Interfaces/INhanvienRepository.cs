using Entity.ViewModels;
using Repository.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface INhanvienRepository
    {
        Task<List<OptionSimpleModel>> GetListCourierSimple();
        Task<List<OptionSimpleModelV2>> GetListByUserId(int userId);
    }
}
