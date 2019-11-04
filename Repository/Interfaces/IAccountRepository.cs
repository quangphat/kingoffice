using Entity.DatanbaseModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IAccountRepository
    {
        Task<Nhanvien> Login(string userName);
        Task<List<string>> GetPermissionByUserId(int userId);
        Task<bool> ResetPassword(int id, string newPass);
    }
}

