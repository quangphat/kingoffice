using Dapper;
using Entity.DatanbaseModels;
using Microsoft.Extensions.Configuration;
using Repository.Entities;
using Repository.Interfaces;
using System.Threading.Tasks;

namespace Repository.Classes
{
    public class AccountRepository : BaseRepository, IAccountRepository
    {
        public AccountRepository(IConfiguration configuration) : base(configuration)
        {

        }
        public async Task<Nhanvien> Login(string userName)
        {
            var result = await connection.QueryFirstOrDefaultAsync<Nhanvien>($"select * from NHAN_VIEN " +
                $"where Ten_Dang_Nhap = '{userName}' and Trang_Thai = 1 and Xoa <> 1");
            return result;
        }
    }
}

