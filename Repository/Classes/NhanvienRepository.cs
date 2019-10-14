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
    public class NhanvienRepository : BaseRepository, INhanvienRepository
    {
        public NhanvienRepository(IConfiguration configuration) : base(configuration)
        {

        }
        public async Task<List<OptionSimpleModel>> GetListCourierSimple()
        {
            var result = await connection.QueryAsync<OptionSimpleModel>("sp_NHAN_VIEN_LayDSCourierCode", null, commandType: CommandType.StoredProcedure);
            return result.ToList();
        }
        public async Task<List<OptionSimpleModelV2>> GetListByUserId(int userId)
        {
            var result = await connection.QueryAsync<OptionSimpleModelV2>("sp_NHOM_LayDSChonTheoNhanVien", new { @UserID =userId  }, commandType: CommandType.StoredProcedure);
            return result.ToList();
        }
    }
}

