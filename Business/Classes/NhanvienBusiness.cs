using AutoMapper;
using Business.Infrastructures;
using Business.Interfaces;
using Entity;
using Entity.DatanbaseModels;
using Entity.Infrastructures;
using Entity.ViewModels;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Classes
{
    public class NhanvienBusiness : BaseBusiness, INhanvienBusiness
    {
        INhanvienRepository _rpNhanvien;
        public NhanvienBusiness(CurrentProcess process, INhanvienRepository nhanvienRepository, IMapper mapper) : base(mapper, process)
        {
            _rpNhanvien = nhanvienRepository;
        }
        public async Task<bool> ApproveConfig(int userId, List<int> teamIds)
        {
            if(userId<=0)
            {
                AddError(errors.missing_user_id);
                return false;
            }
            if (teamIds==null || !teamIds.Any())
            {
                AddError(errors.missing_team);
                return false;
            }
            await _rpNhanvien.AddUserToNhanvienCF(userId, teamIds);
            return true;
        }
        public async Task<Team> GetTeamById(int id, bool isGetForDetail = false)
        {
            if (id <= 0)
            {
                AddError(errors.invalid_data);
                return null;
            }
            var result = await _rpNhanvien.GetTeamById(id, isGetForDetail);
            return result;
        }
        public async Task<List<OptionSimple>> GetTeamMember(int id)
        {
            if (id <= 0)
            {
                AddError(errors.invalid_data);
                return null;
            }
            var result = await _rpNhanvien.GetTeamMember(id);
            return result;
        }
        public async Task<List<OptionSimple>> GetUserNotInTeam(int id)
        {
            if (id <= 0)
            {
                AddError(errors.invalid_data);
                return null;
            }
            var result = await _rpNhanvien.GetUserNotInTeam(id);
            return result;
        }
        public async Task<(List<TeamViewModel> datas, int totalRecord)> GetTeamsByParentId(int parentId = 0, int page =1, int limit = 10)
        {
            var totalRecord = await _rpNhanvien.CountTeamsByParentId(parentId);
            if(totalRecord<=0)
            {
                return (null, 0);
            }
            BusinessExtension.ProcessPaging(page, ref limit);    
            var result = await _rpNhanvien.GetTeamsByParentId(parentId,page, limit);
            return (result,totalRecord);
        }
        public async Task<(List<TeamMember> datas, int totalRecord)> GetTeamMembers(int teamId, int page = 1, int limit = 10)
        {
            var totalRecord = await _rpNhanvien.CountTeamMemberDetail(teamId);
            if (totalRecord <= 0)
            {
                return (null, 0);
            }
            BusinessExtension.ProcessPaging(page, ref limit);
            var result = await _rpNhanvien.GetTeamMemberDetail(teamId, page, limit);
            return (result, totalRecord);
        }
        public async Task<bool> UpdateTeam(Team model)
        {
            if (model == null|| model.Id <=0)
            {
                AddError(errors.invalid_data);
                return false;
            }
            if (string.IsNullOrWhiteSpace(model.Name) || string.IsNullOrWhiteSpace(model.ShortName))
            {
                AddError(errors.missing_team_name);
                return false;
            }
            if (model.ManageUserId <= 0)
            {
                AddError(errors.missing_manage_team);
                return false;
            }
            if (model.MemberIds == null || !model.MemberIds.Any())
            {
                AddError(errors.missing_team_member);
                return false;
            }
            if (model.ParentTeamId != 0)
                model.ParentTeamCode = await _rpNhanvien.GetParentCodeByTeamId(model.ParentTeamId) + "." + model.ParentTeamId;
            else
                model.ParentTeamCode = "0";
            var isUpdateSuccess = await _rpNhanvien.UpdateTeam(model);
            if (isUpdateSuccess == false)
                return false;
            var isRemoveSuccess = await _rpNhanvien.RemoveAllMemberFromTeam(model.Id);
            if (isRemoveSuccess == false)
                return false;
            var result = await _rpNhanvien.AddMembersToTeam(model.Id, model.MemberIds);
            return result;
        }
        public async Task<bool> CreateTeam(Team model)
        {
            if(model==null)
            {
                AddError(errors.invalid_data);
                return false;
            }
            if(string.IsNullOrWhiteSpace(model.Name) || string.IsNullOrWhiteSpace(model.ShortName))
            {
                AddError(errors.missing_team_name);
                return false;
            }
            if(model.ManageUserId <= 0)
            {
                AddError(errors.missing_manage_team);
                return false;
            }
            if(model.MemberIds == null || !model.MemberIds.Any())
            {
                AddError(errors.missing_team_member);
                return false;
            }
            if (model.ParentTeamId != 0)
                model.ParentTeamCode = await _rpNhanvien.GetParentCodeByTeamId(model.ParentTeamId) + "." + model.ParentTeamId;
            else
                model.ParentTeamCode = "0";
            var id = await _rpNhanvien.CreateTeam(model);
            if(id > 0)
            {
                await _rpNhanvien.AddMembersToTeam(id, model.MemberIds);
                return true;
            }
            return false;
        }
        public async Task<List<OptionSimple>> GetAllEmployeeSimpleList()
        {
            if (_process.User == null)
                return null;
            var result = await _rpNhanvien.GetAllEmployeeSimpleList();
            return result;
        }
        public async Task<List<OptionSimple>> GetAllTeamSimpleList()
        {
            if (_process.User == null)
                return null;
            var teams = await _rpNhanvien.GetAllTeamSimpleList();
            if(teams==null || !teams.Any())
            {
                return teams;
            }
            var result = CreateTeamTree(teams, "0");
            return result;
        }
        public async Task<bool> Update(EmployeeEditModel model)
        {
            if (model == null || model.Id <= 0)
            {
                AddError(errors.invalid_data);
                return false;
            }
            var user = _mapper.Map<Nhanvien>(model);
            user.UpdatedBy = _process.User.Id;
            return await _rpNhanvien.Update(user);
        }
        public async Task<EmployeeEditModel> GetById(int id)
        {
            if (id <= 0)
            {
                AddError(errors.invalid_data);
                return null;
            }
            var user = await _rpNhanvien.GetById(id);
            if (user == null)
            {
                AddError(errors.not_found_user);
                return null;
            }
            var result = _mapper.Map<EmployeeEditModel>(user);
            return result;
        }
        public async Task<(int totalRecord, List<EmployeeViewModel> datas)> Gets(DateTime? fromDate, DateTime? toDate,
            string freetext = "",
            int roleId = 0,
            int page = 1, int limit = 10)
        {
            if (!string.IsNullOrWhiteSpace(freetext) && freetext.Length > 30)
            {
                AddError(errors.freetext_length_cannot_lagger_30);
                return (0, null);
            }
            var fDate = fromDate == null ? DateTime.Now : fromDate.Value;
            var tDate = toDate == null ? DateTime.Now : toDate.Value;
            BusinessExtension.ProcessPaging(page, ref limit);
            freetext = string.IsNullOrWhiteSpace(freetext) ? string.Empty : freetext.Trim();
            var totalRecord = await _rpNhanvien.Count(fDate, tDate, roleId, freetext);
            if (totalRecord == 0)
            {
                return (0, null);
            }
            var nhanviens = await _rpNhanvien.Gets(fDate, tDate, roleId, freetext, page, limit);

            return (totalRecord, nhanviens);
        }
        public async Task<int> Create(UserModel entity)
        {
            if (entity == null)
            {
                AddError(errors.invalid_data);
                return 0;
            }
            if (string.IsNullOrWhiteSpace(entity.UserName))
            {
                AddError(errors.username_or_password_must_not_be_empty);
                return 0;
            }
            if (string.IsNullOrWhiteSpace(entity.Password))
            {
                AddError(errors.username_or_password_must_not_be_empty);
                return 0;
            }
            if (entity.Password.Trim().Length < Constanst.PasswordMinLengthRequire)
            {
                AddError(errors.password_not_match_min_length);
                return 0;
            }
            if (string.IsNullOrWhiteSpace(entity.PasswordConfirm))
            {
                AddError(errors.password_not_match);
                return 0;
            }
            if (entity.Password != entity.PasswordConfirm)
            {
                AddError(errors.password_not_match);
                return 0;
            }
            if (string.IsNullOrWhiteSpace(entity.Email))
            {
                AddError(errors.email_cannot_be_null);
                return 0;
            }
            if (!BusinessExtension.IsValidEmail(entity.Email, 50))
            {
                AddError(errors.invalid_email);
                return 0;
            }
            if (entity.ProvinceId <= 0)
            {
                AddError(errors.missing_province);
                return 0;
            }
            if (entity.DistrictId <= 0)
            {
                AddError(errors.missing_district);
                return 0;
            }
            var existUserName = await _rpNhanvien.GetByUserName(entity.UserName.Trim(), 0);
            if (existUserName != null)
            {
                AddError(errors.username_has_exist);
                return 0;
            }
            entity.UserName = entity.UserName.Trim();
            entity.Password = entity.Password.Trim();
            var user = _mapper.Map<Nhanvien>(entity);
            user.CreatedBy = _process.User.Id;
            user.Mat_Khau = Utils.getMD5(entity.Password);
            var result = await _rpNhanvien.Create(user);
            return result;
        }
        
        public async Task<List<OptionSimpleModelOld>> GetCourierList()
        {
            return await _rpNhanvien.GetListCourierSimple();
        }
        public async Task<List<OptionSimpleModelV2>> GetListByUserId(int userId)
        {
            return await _rpNhanvien.GetListByUserId(userId);
        }
        public async Task<List<OptionSimple>> GetSaleList()
        {
            if (_process.User == null)
                return null;
            var result = new List<OptionSimple>();
            result.Add(new OptionSimple {
                Id = _process.User.Id,
                Name = _process.User.FullName,
                Code = _process.User.UserName
            });
            return result;
        }
        private List<OptionSimple> CreateTeamTree(List<OptionSimple> lstData, string origin)
        {
            try
            {
                if (lstData == null)
                    return null;
                List<OptionSimple> lstResult = new List<OptionSimple>();
                Stack<OptionSimple> stack = new Stack<OptionSimple>();
                List<OptionSimple> lstFind = new List<OptionSimple>();
                do
                {
                    if (stack.Count > 0)
                    {
                        OptionSimple team = stack.Pop();
                        if (team != null)
                        {
                            string[] tempArray = team.Code.Split('.');
                            if (tempArray.Length > 1)
                            {
                                for (int i = 0; i < tempArray.Length - 1; i++)
                                {
                                    team.Name = "-" + team.Name;
                                }
                            }
                            lstResult.Add(team);
                            origin = team.Code + "." + team.Id;
                        }
                    }
                    lstFind = lstData.FindAll(x => x.Code.Equals(origin));
                    if (lstFind != null)
                    {

                        for (int i = lstFind.Count - 1; i >= 0; i--)
                        {
                            stack.Push(lstFind[i]);
                            lstData.Remove(lstFind[i]);
                        }
                    }
                } while (stack.Count > 0);
                return lstResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
