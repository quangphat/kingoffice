using Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface INhanvienBusiness
    {
        Task<bool> CreateTeam(Team model);
        Task<List<OptionSimple>> GetAllTeamSimpleList();
        Task<List<OptionSimple>> GetAllEmployeeSimpleList();
        Task<bool> Update(EmployeeEditModel model);
        Task<EmployeeEditModel> GetById(int id);
        Task<(int totalRecord, List<EmployeeViewModel> datas)> Gets(DateTime? fromDate, DateTime? toDate,
            string freetext = "",
            int roleId =0,
            int page = 1, int limit = 10);
        Task<int> Create(UserModel entity);
        Task<List<OptionSimpleModelOld>> GetCourierList();
        Task<List<OptionSimple>> GetSaleList();
        Task<List<OptionSimpleModelV2>> GetListByUserId(int userId);
    }
}
