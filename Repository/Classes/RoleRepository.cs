using Dapper;
using Entity.DatabaseModels;
using Entity.Infrastructures;
using Entity.ViewModels;
using Microsoft.Extensions.Configuration;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Classes
{
    public class RoleRepository : BaseRepository, IRoleRepository
    {
        public RoleRepository(IConfiguration configuration) : base(configuration)
        {

        }
        public async Task<List<OptionSimple>> GetList()
        {
            string query = "select Id, Name from Role where Deleted <> 1";
            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<OptionSimple>(query, commandType: CommandType.Text);
                return result.ToList();
            }
                

        }
    }
}

