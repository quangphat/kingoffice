using Dapper;
using Entity.ViewModels;
using Microsoft.Extensions.Configuration;
using Repository.Entities;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
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
        public async Task<List<OptionSimpleModel>> GetListProvinceSimple()
        {
            var result = await connection.QueryAsync<OptionSimpleModel>("sp_KHU_VUC_LayDSTinh", null, commandType: CommandType.StoredProcedure);
            return result.ToList();
        }
        public async Task<List<OptionSimpleModel>> GetListDistrictSimple(int provinceId)
        {
            var result = await connection.QueryAsync<OptionSimpleModel>("sp_KHU_VUC_LayDSHuyen", new { @MaTinh = provinceId}, commandType: CommandType.StoredProcedure);
            return result.ToList();
        }
    }
}

