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
        public async Task<List<OptionSimpleExtendForProduct>> GetAllList(int typeId =0)
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<OptionSimpleExtendForProduct>("sp_GetAllProduct", new
                {
                    type = typeId
                }, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }
        public async Task<bool> Delete(int id)
        {
            using (var con = GetConnection())
            {
                var result = await con.ExecuteAsync("sp_SAN_PHAM_VAY_Xoa", new
                {
                    ID = id
                }, commandType: CommandType.StoredProcedure);
                return true;
            }
        }
        public async Task<List<ProductDetailViewModel>> GetListByDate(int partnerId, DateTime createdDate)
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<ProductDetailViewModel>("sp_SAN_PHAM_VAY_LayDSThongTinByID", new
                {
                    MaDoiTac = partnerId,
                    NgayTao = createdDate
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
            using (var con = GetConnection())
            {
                await con.ExecuteScalarAsync<int>("sp_SAN_PHAM_VAY_CheckExist", p, commandType: CommandType.StoredProcedure);
                var result = p.Get<int>("Exist");
                if (result > 0)
                    return true;
                return false;
            }
                
        }
        public async Task<bool> UpdateUse(int hosoId,int productId)
        {
            var p = new DynamicParameters();
            p.Add("SanPhamVay", productId);
            p.Add("ID", hosoId);
            using (var con = GetConnection())
            {
                await _connection.ExecuteAsync("sp_SAN_PHAM_VAY_CapNhatSuDung", p, commandType: CommandType.StoredProcedure);
                return true;
            }
                
        }
        public async Task<int> Insert(Product product)
        {
            var p = base.AddOutputParam("ID");
            p.Add("MaDoiTac", product.PartnerId);
            p.Add("Ma", product.Code);
            p.Add("Ten", product.Name);
            p.Add("NgayTao", product.CreatedDate);
            p.Add("MaNguoiTao", product.CreatedBy);
            p.Add("Loai", product.Type);
            using (var con = GetConnection())
            {
                await _connection.ExecuteAsync("sp_SAN_PHAM_VAY_Them", p, commandType: CommandType.StoredProcedure);
                var result = p.Get<int>("ID");
                return result;
            }

        }
    }
}

