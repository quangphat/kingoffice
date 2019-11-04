using Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IAccountBusiness
    {
        Task<bool> ResetPassword(int id, string oldPass, string newPass, string confirmPass);
        Task<Account> Login(string username, string password);
    }
}
