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
        public DoitacRepository(IConfiguration configuration, CurrentProcess process) : base(configuration, process)
        {

        }
        public async Task<List<OptionSimpleModel>> GetListSimple()
        {
            var result = await connection.QueryAsync<OptionSimpleModel>("sp_DOI_TAC_LayDS", null, commandType: CommandType.StoredProcedure);
            return result.ToList();
        }
    }
}

