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
        Task<List<OptionSimple>> GetTeamSimpleListForDsHoso(int userId);
        Task<List<OptionSimpleModelOld>> GetMemberByTeamIncludeChild(int teamId);
        Task<List<OptionSimpleExtendForTeam>> GetChildTeamSimpleList(int teamId);
        Task<List<OptionSimple>> GetAllTeamManageByUserId(int userId);
        Task<bool> AddUserToNhanvienCF(int userId, List<int> teamIds);
        Task<bool> RemoveNhanvienCF(int userId);
        Task<int> CountTeamMemberDetail(int teamId);
        Task<List<TeamMember>> GetTeamMemberDetail(int teamId, int page = 1, int limit = 10);
        Task<bool> RemoveAllMemberFromTeam(int teamId);
        Task<bool> UpdateTeam(Team team);
        Task<List<OptionSimple>> GetUserNotInTeam(int teamId);
        Task<List<OptionSimple>> GetTeamMember(int teamId);
        Task<Team> GetTeamById(int teamId, bool isGetForDetail = false);
        Task<List<TeamViewModel>> GetTeamsByParentId(int parentId, int page = 0, int limit = 10);
        Task<int> CountTeamsByParentId(int parentId);
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

