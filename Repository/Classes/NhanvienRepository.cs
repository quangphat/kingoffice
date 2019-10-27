using Dapper;
using Entity.DatanbaseModels;
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
    public class NhanvienRepository : BaseRepository, INhanvienRepository
    {
        public NhanvienRepository(IConfiguration configuration) : base(configuration)
        {

        }
        public async Task<int> Create(Nhanvien entity)
        {
            var p = AddOutputParam("id");
            p.Add("Ma", entity.Ma);
            p.Add("Ten_Dang_Nhap", entity.Ten_Dang_Nhap);
            p.Add("Mat_Khau", entity.Mat_Khau);
            p.Add("Ho_Ten", entity.Ho_Ten);
            p.Add("Dien_Thoai", entity.Dien_Thoai);
            p.Add("Email", entity.Email);
            p.Add("Role", entity.Role);
            p.Add("CreatedTime", DateTime.Now);
            p.Add("CreatedBy", entity.CreatedBy);
            p.Add("UpdatedTime", DateTime.Now);
            p.Add("UpdatedBy",entity.UpdatedBy);
            await connection.ExecuteAsync("sp_InsertUser",p,commandType:CommandType.StoredProcedure);
            return  p.Get<int>("id"); ;
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

