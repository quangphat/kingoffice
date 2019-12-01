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
        public UserRoleMenuRepository(IConfiguration configuration) : base(configuration)
        {

        }
        public async Task<List<UserRoleMenu>> GetMenuByUserRole(string role)
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<UserRoleMenu>($"select * from UserRoleMenu where RoleCode = '{role}'");
                return result.ToList();
            }
                
        }
        public async Task<List<UserRoleMenu>> GetMenuByRoleId(int roleId)
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<UserRoleMenu>($"select * from UserRoleMenu where RoleId = {roleId}");
                return result.ToList();
            }
               
        }
    }
}

