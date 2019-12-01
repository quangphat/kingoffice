using Dapper;
using Entity.Infrastructures;
using Entity.ViewModels;
using Microsoft.Extensions.Configuration;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Classes
{
    public class LocationRepository : BaseRepository, ILocationRepository
    {
        public LocationRepository(IConfiguration configuration) : base(configuration)
        {

        }
        public async Task<List<OptionSimpleModelOld>> GetListProvinceSimple()
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<OptionSimpleModelOld>("sp_KHU_VUC_LayDSTinh", null, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
                
        }
        public async Task<List<OptionSimpleModelOld>> GetListDistrictSimple(int provinceId)
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<OptionSimpleModelOld>("sp_KHU_VUC_LayDSHuyen", new { @MaTinh = provinceId }, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
                

        }
    }
}

