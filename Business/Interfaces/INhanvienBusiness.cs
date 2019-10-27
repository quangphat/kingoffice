using Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface INhanvienBusiness
    {
        Task<(int totalRecord, List<Account> datas)> Gets(DateTime? fromDate, DateTime? toDate,
            string freetext = "",
            int page = 1, int limit = 10);
        Task<int> Create(UserModel entity);
        Task<List<OptionSimpleModel>> GetCourierList();
        Task<List<OptionSimpleModelV2>> GetListByUserId(int userId);
    }
}
