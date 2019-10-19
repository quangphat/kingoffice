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
    public class ProductRepository : BaseRepository, IProductRepository
    {
        public ProductRepository(IConfiguration configuration) : base(configuration)
        {

        }
        public async Task<List<ProductViewModel>> GetListByDoitacId(int doitacId)
        {
            var result = await connection.QueryAsync<ProductViewModel>("sp_SAN_PHAM_VAY_LayDSByID", new {
                @MaDoiTac = doitacId
            }, commandType: CommandType.StoredProcedure);
            return result.ToList();
        }
        public async Task<List<ProductViewModel>> GetListByHosoId(int doitaId, int hosoId)
        {
            var result = await connection.QueryAsync<ProductViewModel>("sp_SAN_PHAM_VAY_LayDSByID", new
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
            var result = await connection.ExecuteScalarAsync<int>("sp_SAN_PHAM_VAY_CheckExist", p, commandType: CommandType.StoredProcedure);
            if (result > 0)
                return true;
            return false;
        }
    }
}

