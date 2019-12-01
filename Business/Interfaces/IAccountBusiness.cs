using Entity.DatabaseModels;
using Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IAccountBusiness
    {
        Task<bool> InserUserRoleMenuForMultipleMenu(UserRoleMenuForMultipleMenu model);
        Task<bool> InserUserRoleMenu(UserRoleMenu model);
        Task<bool> ResetPassword(int id, string oldPass, string newPass, string confirmPass);
        Task<Account> Login(string username, string password);
        Task<bool> UpdateRoleForUserByQuangPhat(int userId, int roleId, string updateBy = "");
        Task<bool> UpdateRoleForMultipleUserByQuangPhat(List<int> userIds, int roleId, string updateBy = "");
    }
}
