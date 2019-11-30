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
        public async Task<List<OptionSimple>> GetAllTeamManageByUserId(int userId)
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<OptionSimple>("getAllTeamManageByUserId", new { userId = userId }, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }
        public async Task<List<OptionSimpleExtendForTeam>> GetChildTeamSimpleList(int teamId)
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<OptionSimpleExtendForTeam>("sp_GetChildTeamByTeamId", new { teamId }, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }
        public async Task<List<OptionSimpleModelOld>> GetMemberByTeamIncludeChild(int teamId)
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<OptionSimpleModelOld>("sp_NHAN_VIEN_NHOM_LayDSChonThanhVienNhomCaCon", new { MaNhom = teamId }, commandType: CommandType.StoredProcedure);
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
        public async Task<bool> RemoveAllMemberFromTeam(int teamId)
        {
            using (var con = GetConnection())
            {
               await con.ExecuteAsync("sp_NHAN_VIEN_NHOM_Xoa", new { MaNhom = teamId }, commandType: CommandType.StoredProcedure);
                return true;
            }
        }
        public async Task<bool> UpdateTeam(Team team)
        {
            var p = new DynamicParameters();
            p.Add("ID", team.Id);
            p.Add("MaNhomCha", team.ParentTeamId);
            p.Add("MaNguoiQL", team.ManageUserId);
            p.Add("TenVietTat", team.ShortName);
            p.Add("Ten", team.Name);
            p.Add("ChuoiMaCha", team.ParentTeamCode);
            using (var con = GetConnection())
            {
                await con.ExecuteAsync("sp_NHOM_Sua", p, commandType: CommandType.StoredProcedure);
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
        public async Task<List<TeamViewModel>> GetTeamsByParentId(int parentId, int page = 0, int limit = 10)
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<TeamViewModel>("sp_GetChildTeamByParentId",new { parentId, page,limit }, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }
        public async Task<int> CountTeamsByParentId(int parentId)
        {
            using (var con = GetConnection())
            {
                var result = await con.ExecuteScalarAsync<int>("sp_CountChildTeamByParentId", new { parentId }, commandType: CommandType.StoredProcedure);
                return result;
            }
        }
        public async Task<Team> GetTeamById(int teamId, bool isGetForDetail = false)
        {
            using (var con = GetConnection())
            {
                var p = new DynamicParameters();
                p.Add("id", teamId);
                p.Add("isGetForDetail", isGetForDetail);
                var result = await _connection.QueryFirstOrDefaultAsync<Team>("sp_GetTeamById", p,
               commandType: CommandType.StoredProcedure);
                return result;
            }
        }
        public async Task<List<OptionSimple>> GetTeamMember(int teamId)
        {
            using (var con = GetConnection())
            {
                var p = new DynamicParameters();
                p.Add("id", teamId);
                var result = await _connection.QueryAsync<OptionSimple>("sp_GetAllUserInTeam", p,
               commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }
        public async Task<List<OptionSimple>> GetUserNotInTeam(int teamId)
        {
            using (var con = GetConnection())
            {
                var p = new DynamicParameters();
                p.Add("id", teamId);
                var result = await _connection.QueryAsync<OptionSimple>("sp_AllUserNotInTeam", p,
               commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }
        public async Task<List<TeamMember>> GetTeamMemberDetail(int teamId, int page =1, int limit =10)
        {
            using (var con = GetConnection())
            {
                var p = new DynamicParameters();
                p.Add("id", teamId);
                p.Add("page", page);
                p.Add("limit", limit);
                var result = await _connection.QueryAsync<TeamMember>("sp_GetAllMemberByTeam", p,
               commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }
        public async Task<int>CountTeamMemberDetail(int teamId)
        {
            using (var con = GetConnection())
            {
                var p = new DynamicParameters();
                p.Add("id", teamId);
                var result = await _connection.ExecuteScalarAsync<int>("sp_CountAllMemberByTeam", p,
               commandType: CommandType.StoredProcedure);
                return result;
            }
        }
        public async Task<bool> RemoveNhanvienCF(int userId)
        {
            using (var con = GetConnection())
            {
                await con.ExecuteAsync("sp_NHAN_VIEN_CF_Xoa", new { MaNhanVien = userId }, commandType: CommandType.StoredProcedure);
                return true;
            }
        }
        public async Task<bool> AddUserToNhanvienCF(int userId, List<int> teamIds)
        {

            using (var con = GetConnection())
            {
                await _connection.ExecuteAsync("sp_NHAN_VIEN_CF_Them", teamIds.Select(p => new {
                    MaNhom = p,
                    MaNhanVien = userId
                }),
               commandType: CommandType.StoredProcedure);
                return true;
            }
        }
    }
}

