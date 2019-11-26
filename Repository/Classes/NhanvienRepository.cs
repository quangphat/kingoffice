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
        public async Task<bool> Update(Nhanvien entity)
        {
            var p =new DynamicParameters();
            p.Add("id", entity.ID);
            p.Add("provinceId", entity.ProvinceId);
            p.Add("districtId", entity.DistrictId);
            p.Add("fullName", entity.Ho_Ten);
            p.Add("phone", entity.Dien_Thoai);
            p.Add("roleId", entity.RoleId);
            p.Add("email", entity.Email);
            p.Add("workDate", entity.WorkDate);
            p.Add("UpdatedTime", DateTime.Now);
            p.Add("UpdatedBy", entity.UpdatedBy);
            await _connection.ExecuteAsync("sp_UpdateUser", p, commandType: CommandType.StoredProcedure);
            return true;
        }
        public async Task<int> Count(
            DateTime workFromDate,
            DateTime workToDate,
            int roleId,
            string freeText)
        {
            var p = new DynamicParameters();
            p.Add("workFromDate",workFromDate);
            p.Add("workToDate", workToDate);
            p.Add("roleId", roleId);
            p.Add("freeText", freeText);
            var total = await _connection.ExecuteScalarAsync<int>("sp_CountNhanvien", p, commandType: CommandType.StoredProcedure);
            return total;
        }
        public async Task<List<EmployeeViewModel>> Gets(
            DateTime workFromDate,
            DateTime workToDate,
            int roleId,
            string freeText,
            int page,
            int limit)
        {
            var p = new DynamicParameters();
            p.Add("workFromDate", workFromDate);
            p.Add("workToDate", workToDate);
            p.Add("freeText", freeText);
            p.Add("page", page);
            p.Add("roleId", roleId);
            p.Add("limit", limit);
            var results = await _connection.QueryAsync<EmployeeViewModel>("sp_GetNhanvien", p, commandType: CommandType.StoredProcedure);
            return results.ToList();
        }
        public async Task<Nhanvien> GetByUserName(string userName, int id = 0)
        {
            string query = "select * from Nhan_Vien where Ten_Dang_Nhap = @userName";
            if(id>0)
            {
                query += " and ID <> @id";
            }
            var result = await _connection.QueryFirstOrDefaultAsync<Nhanvien>(query, new { @userName = userName, @id = id }, commandType: CommandType.Text);
            return result;
        }
        public async Task<Nhanvien> GetById( int id )
        {
            string query = "select * from Nhan_Vien where ID = @id";
            var result = await _connection.QueryFirstOrDefaultAsync<Nhanvien>(query, new { @id = id }, commandType: CommandType.Text);
            return result;
        }
        public async Task<int> Create(Nhanvien entity)
        {
            var p = AddOutputParam("id");
            p.Add("ProvinceId", entity.ProvinceId);
            p.Add("DistrictId", entity.DistrictId);
            p.Add("userName", entity.Ten_Dang_Nhap);
            p.Add("password", entity.Mat_Khau);
            p.Add("fullName", entity.Ho_Ten);
            p.Add("phone", entity.Dien_Thoai);
            p.Add("roleId", entity.RoleId);
            p.Add("email", entity.Email);
            p.Add("workDate", entity.WorkDate);
            p.Add("createdtime", DateTime.Now);
            p.Add("createdby", entity.CreatedBy);
            await _connection.ExecuteAsync("sp_InsertUser",p,commandType:CommandType.StoredProcedure);
            return  p.Get<int>("id"); 
        }
        public async Task<List<OptionSimpleModelOld>> GetListCourierSimple()
        {
            var result = await _connection.QueryAsync<OptionSimpleModelOld>("sp_NHAN_VIEN_LayDSCourierCode", null, commandType: CommandType.StoredProcedure);
            return result.ToList();
        }
        public async Task<List<OptionSimpleModelV2>> GetListByUserId(int userId)
        {
            var result = await _connection.QueryAsync<OptionSimpleModelV2>("sp_NHOM_LayDSChonTheoNhanVien", new { @UserID =userId  }, commandType: CommandType.StoredProcedure);
            return result.ToList();
        }
        public async Task<List<OptionSimple>> GetAllEmployeeSimpleList()
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<OptionSimple>("sp_getAllEmployeeSimpleList", null, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }
        public async Task<List<OptionSimple>> GetAllTeamSimpleList()
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<OptionSimple>("sp_getAllNhomSimpleList", null, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }
        public async Task<int> CreateTeam(Team team)
        {
            //sp_NHOM_Them
            using (var con = GetConnection())
            {
                var p = base.AddOutputParam("Id");
                p.Add("name", team.Name);
                p.Add("shortName", team.ShortName);
                p.Add("parentId", team.ParentTeamId);
                p.Add("parentCode", team.ParentTeamCode);
                p.Add("manageUserId", team.ManageUserId);
                await _connection.ExecuteAsync("sp_CreateTeam", p,
               commandType: CommandType.StoredProcedure);
                var result = p.Get<int>("Id");
                return result;
            }
        }
        public async Task<bool> AddMembersToTeam(int teamId, List<int> memberIds)
        {
            //sp_NHAN_VIEN_NHOM_Them
            using (var con = GetConnection())
            {
                await _connection.ExecuteAsync("sp_AddMemberToTeam", memberIds.Select(p=> new {
                    teamId = teamId, userId = p
                }),
               commandType: CommandType.StoredProcedure);
                return true;
            }
        }
        public async Task<string> GetParentCodeByTeamId (int teamId)
        {
            using (var con = GetConnection())
            {
                var p = new DynamicParameters();
                p.Add("MaNhom", teamId);
                var result = await _connection.ExecuteScalarAsync<string>("sp_NHOM_LayChuoiMaChaCuaMaNhom", p,
               commandType: CommandType.StoredProcedure);
                return result;
            }
        }
    }
}

