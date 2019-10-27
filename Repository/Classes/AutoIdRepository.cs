using Dapper;
using Entity.Enums;
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
    public class AutoIdRepository : BaseRepository, IAutoIdRepository
    {
        public AutoIdRepository(IConfiguration configuration) : base(configuration)
        {

        }
        public async Task<AutoIDModel> GetAutoId(int type)
        {
            var result = await connection.QueryFirstOrDefaultAsync<AutoIDModel>("sp_AUTOID_GetID", new { ID = type }, commandType: CommandType.StoredProcedure);
            return result;
        }
        public async Task<bool> Update(AutoIDModel model)
        {
            var p = new DynamicParameters();
            p.Add("ID", model.ID);
            p.Add("Prefix", model.Prefix);
            p.Add("Suffixes", model.Suffixes);
            p.Add("Value", model.Value);
            await connection.ExecuteAsync("sp_AUTOID_Update",
                p, commandType: CommandType.StoredProcedure);
            return true;
        }
    }
}

