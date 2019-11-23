using Dapper;
using Entity.DatanbaseModels;
using Entity.Infrastructures;
using Microsoft.Extensions.Configuration;
using Repository.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Repository.Classes
{
    public class AccountRepository : BaseRepository, IAccountRepository
    {
        public AccountRepository(IConfiguration configuration) : base(configuration)
        {

        }

        public async Task<List<string>> GetPermissionByUserId(int userId)
        {
            var result = await _connection.QueryAsync<string>("sp_getPermissionByUserId",new { @userId = userId}, commandType: CommandType.StoredProcedure);
            return result.ToList();
        }

        public async Task<Nhanvien> Login(string userName)
        {
            var result = await _connection.QueryFirstOrDefaultAsync<Nhanvien>($"select * from NHAN_VIEN " +
                $"where Ten_Dang_Nhap = '{userName}' and Trang_Thai = 1 and Xoa <> 1");
            return result;
        }
        public async Task<bool> ResetPassword(int id, string newPass)
        {
            await _connection.ExecuteAsync($"update NHAN_VIEN set Mat_Khau = @pass where Id = @id  and Trang_Thai = 1 and Xoa <> 1",
                new {
                    pass = new DbString() { Value = newPass, IsAnsi = false,Length = 50 },
                    id = id

                },commandType: CommandType.Text);
            return true;
        }
        public async Task<bool> CheckIsTeamLead(int userId)
        {
            var result = await _connection.ExecuteScalarAsync<bool>("sp_CheckIsTeamlead",
                new
                {
                   userId

                }, commandType: CommandType.StoredProcedure);
            return result;
        }
    }
}

