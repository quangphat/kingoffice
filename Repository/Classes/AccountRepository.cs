using Dapper;
using Entity.DatabaseModels;
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
        public string connect = "";

        public string ConnectStr => connect;

        public AccountRepository(IConfiguration configuration) : base(configuration)
        {
            connect = GetConnection().ConnectionString;
        }
        public async Task<List<Menu>> GetAllMenu()
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<Menu>("sp_GetAllMenu", commandType: CommandType.StoredProcedure);
                return result.ToList();
            }

        }
        public async Task<int> AddMenu(Menu model)
        {
            using (var con = GetConnection())
            {
                var p = AddOutputParam("id");
                p.Add("name", model.Name);
                p.Add("code", model.Code);
                p.Add("value", model.Value);
                p.Add("parentId", model.ParentId);
                await con.ExecuteAsync("sp_InsertMenu",
                    p, commandType: CommandType.StoredProcedure);
                var result = p.Get<int>("id");
                return result;
            }
        }
        public async Task<bool> InserUserRoleMenu(UserRoleMenu model)
        {
            using (var con = GetConnection())
            {
                var result = await con.ExecuteAsync("insertRoleMenu",
                    new
                    {
                        roleId = model.RoleId,
                        menuId = model.MenuId,
                        roleCode = model.RoleCode,
                        menuName = model.MenuName
                    }, commandType: CommandType.StoredProcedure);
                return true;
            }
        }
        public async Task<bool> UpdateRoleForUser(int userId, int roleId)
        {
            using (var con = GetConnection())
            {
                var result = await con.ExecuteAsync("setRoleForUser",
                    new
                    {
                        userId,
                        roleId
                    }, commandType: CommandType.StoredProcedure);
                return true;
            }
        }
        public async Task<List<string>> GetPermissionByUserId(int roleId)
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<string>("sp_getPermissionByUserId", new { @roleId = roleId }, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
                
        }

        public async Task<Nhanvien> Login(string userName)
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryFirstOrDefaultAsync<Nhanvien>($"select * from NHAN_VIEN " +
                   $"where Ten_Dang_Nhap = '{userName}' and Trang_Thai = 1 and Xoa <> 1");
                return result;
            }
               
        }
        public async Task<bool> ResetPassword(int id, string newPass)
        {
            using (var con = GetConnection())
            {
                await con.ExecuteAsync($"update NHAN_VIEN set Mat_Khau = @pass where Id = @id  and Trang_Thai = 1 and Xoa <> 1",
                   new
                   {
                       pass = new DbString() { Value = newPass, IsAnsi = false, Length = 50 },
                       id = id

                   }, commandType: CommandType.Text);
                return true;
            }
               
        }
        public async Task<bool> CheckIsTeamLead(int userId)
        {
            using (var con = GetConnection())
            {
                var result = await con.ExecuteScalarAsync<bool>("sp_CheckIsTeamlead",
                    new
                    {
                        userId

                    }, commandType: CommandType.StoredProcedure);
                return result;
            }
                
        }
        public async Task<bool> CheckIsAdmin(int userId)
        {
            using (var con = GetConnection())
            {
                var result = await con.ExecuteScalarAsync<bool>("sp_CheckIsAdmin",
                    new
                    {
                        userId

                    }, commandType: CommandType.StoredProcedure);
                return result;
            }

        }
    }
}

