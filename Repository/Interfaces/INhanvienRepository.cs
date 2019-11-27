using Entity.DatanbaseModels;
using Entity.ViewModels;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface INhanvienRepository
    {
        Task<string> GetParentCodeByTeamId(int teamId);
        Task<int> CreateTeam(Team team);
        Task<bool> AddMembersToTeam(int teamId, List<int> memberIds);
        Task<List<OptionSimple>> GetAllTeamSimpleList();
        Task<List<OptionSimple>> GetAllEmployeeSimpleList();
        Task<Nhanvien> GetById(int id);
        Task<bool> Update(Nhanvien entity);
        Task<int> Count(
            DateTime workFromDate,
            DateTime workToDate,
            int roleId,
            string freeText);
        Task<List<EmployeeViewModel>> Gets(
            DateTime workFromDate,
            DateTime workToDate,
            int roleId,
            string freeText,
            int offset,
            int limit);
        Task<Nhanvien> GetByUserName(string userName, int id = 0);
        Task<int> Create(Nhanvien entity);
        Task<List<OptionSimpleModelOld>> GetListCourierSimple();
        Task<List<OptionSimpleModelV2>> GetListByUserId(int userId);
    }
}

