using Dapper;
using Entity.DatanbaseModels;
using Microsoft.Extensions.Configuration;
using Repository.Entities;
using Repository.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository.Classes
{
    public class AccountRepository : BaseRepository, IAccountRepository
    {
        public AccountRepository(IConfiguration configuration) : base(configuration)
        {

        }

        public async Task<List<string>> GetScopesByRole(string role)
        {
            var result = await connection.QueryAsync<string>($"select Scope from ScopeRole " +
                $"where role = '{role}'");
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

