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
        public AccountRepository(IConfiguration configuration, CurrentProcess process) : base(configuration, process)
        {

        }

        public async Task<List<string>> GetPermissionByUserId(int userId)
        {
            var result = await connection.QueryAsync<string>("sp_getPermissionByUserId",new { @userId = userId}, commandType: CommandType.StoredProcedure);
            return result.ToList();
        }

        public async Task<Nhanvien> Login(string userName)
        {
            var result = await connection.QueryFirstOrDefaultAsync<Nhanvien>($"select * from NHAN_VIEN " +
                $"where Ten_Dang_Nhap = '{userName}' and Trang_Thai = 1 and Xoa <> 1");
            return result;
        }
    }
}

