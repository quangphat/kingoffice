using Dapper;
using Entity.DatabaseModels;
using Entity.Infrastructures;
using Microsoft.Extensions.Configuration;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Classes
{
    public class UserRoleMenuRepository : BaseRepository, IUserRoleMenuRepository
    {
        public UserRoleMenuRepository(IConfiguration configuration, CurrentProcess process) : base(configuration, process)
        {

        }
        public async Task<List<UserRoleMenu>> GetMenuByUserRole(string role)
        {
            var result = await connection.QueryAsync<UserRoleMenu>($"select * from UserRoleMenu where UserRole = '{role}'");
            return result.ToList();
        }
    }
}

