using Dapper;
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
    public class DoitacRepository : BaseRepository, IDoitacRepository
    {
        public DoitacRepository(IConfiguration configuration) : base(configuration)
        {

        }
        public async Task<List<OptionSimpleModelOld>> GetListSimple()
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<OptionSimpleModelOld>("sp_DOI_TAC_LayDS", null, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }
    }
}

