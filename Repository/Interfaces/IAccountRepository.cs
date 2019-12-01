using Entity.DatabaseModels;
using Entity.DatanbaseModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IAccountRepository
    {
        Task<bool> InserUserRoleMenu(UserRoleMenu model);
        Task<Nhanvien> Login(string userName);
        Task<List<string>> GetPermissionByUserId(int roleId);
        Task<bool> ResetPassword(int id, string newPass);
        Task<bool> CheckIsTeamLead(int userId);
        Task<bool> CheckIsAdmin(int userId);
        Task<bool> UpdateRoleForUser(int userId, int roleId);
    }
}

