using Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface INhanvienBusiness
    {
        Task<List<OptionSimpleModel>> GetCourierList();
        Task<List<OptionSimpleModelV2>> GetListByUserId(int userId);
    }
}
