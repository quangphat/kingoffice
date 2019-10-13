using Entity.DatabaseModels;
using Repository.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IUserRoleMenuRepository
    {
        Task<List<UserRoleMenu>> GetMenuByUserRole(string role);
    }
}

