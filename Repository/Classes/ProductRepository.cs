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
    public class ProductRepository : BaseRepository, IProductRepository
    {
        public ProductRepository(IConfiguration configuration) : base(configuration)
        {

        }
        public async Task<List<OptionSimpleModelOld>> GetListByDoitacId(int doitacId)
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<OptionSimpleModelOld>("sp_SAN_PHAM_VAY_LayDSByID", new
                {
                    @MaDoiTac = doitacId
                }, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }
        public async Task<List<ProductViewModel>> GetListByHosoId(int doitaId, int hosoId)
        {
            NewConnection();
            var result = await _connection.QueryAsync<ProductViewModel>("sp_SAN_PHAM_VAY_LayDSByID", new
            {
                @MaHS = hosoId,
                @MaDoiTac = doitaId
            }, commandType: CommandType.StoredProcedure);
            return result.ToList();
        }
        public async Task<bool> CheckIsInUse(int hosoId, int productId)
        {
            var p = new DynamicParameters();
            p.Add("SanPhamVay", productId);
            p.Add("ID", hosoId);
            p.Add("Exist", dbType: DbType.Int32, direction: ParameterDirection.Output);
            await _connection.ExecuteScalarAsync<int>("sp_SAN_PHAM_VAY_CheckExist", p, commandType: CommandType.StoredProcedure);
            var result = p.Get<int>("Exist");
            if (result > 0)
                return true;
            return false;
        }
        public async Task<bool> UpdateUse(int hosoId,int productId)
        {
            var p = new DynamicParameters();
            p.Add("SanPhamVay", productId);
            p.Add("ID", hosoId);
            await _connection.ExecuteAsync("sp_SAN_PHAM_VAY_CapNhatSuDung", p, commandType: CommandType.StoredProcedure);
            return true;
        }
    }
}

