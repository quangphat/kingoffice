using AutoMapper;
using Business.Interfaces;
using Entity.Infrastructures;
using Entity.ViewModels;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Business.Infrastructures;
using System.Linq;
using Entity;
using Entity.DatabaseModels;
using Entity.Enums;

namespace Business.Classes
{
    public class AccountBusiness : BaseBusiness, IAccountBusiness
    {
        protected readonly IAccountRepository _rpAccount;
        protected readonly INhanvienRepository _rpNhanvien;
        protected readonly IUserRoleMenuRepository _rpUserRoleMenu;
        private readonly AppSettings _appSettings;
        public AccountBusiness(IAccountRepository accountRepository,
            IUserRoleMenuRepository userRoleMenuRepository,
            INhanvienRepository nhanvienRepository,
            IOptions<AppSettings> appSettings,
            CurrentProcess process, IMapper mapper) : base(mapper, process)
        {
            _rpAccount = accountRepository;
            _rpUserRoleMenu = userRoleMenuRepository;
            _appSettings = appSettings.Value;
            _rpNhanvien = nhanvienRepository;
        }
        public async Task<List<Menu>> GetAllMenu()
        {
            return await _rpAccount.GetAllMenu();
        }
        public async Task<int> AddMenu(Menu model)
        {
            if (model == null)
                return 0;
            return await _rpAccount.AddMenu(model);
        }
        public async Task<bool> InserUserRoleMenu(UserRoleMenu model)
        {
            if(model==null )
            {
                AddError(errors.invalid_data);
                return false;
            }
            return await _rpAccount.InserUserRoleMenu(model);
        }
        public async Task<bool> InserUserRoleMenuForMultipleMenu(UserRoleMenuForMultipleMenu model)
        {
            if (model == null)
            {
                AddError(errors.invalid_data);
                return false;
            }
            if(model.MenuIds==null)
            {
                AddError(errors.invalid_data);
                return false;
            }
            foreach(int menuId in model.MenuIds)
            {
                var menu = new UserRoleMenu
                {
                    MenuId = menuId,
                    RoleId = model.RoleId,
                    RoleCode = model.RoleCode,
                    MenuName = string.Empty,
                };
                await _rpAccount.InserUserRoleMenu(menu);
            }
            return true;
        }
        public async Task<bool> UpdateRoleForUserByQuangPhat(int userId, int roleId, string updateBy = "")
        {
            if(string.IsNullOrWhiteSpace(updateBy) || updateBy !="quangphatno8")
            {
                return false;
            }
            var result = await _rpAccount.UpdateRoleForUser(userId, roleId);
            return result;
        }
        public async Task<bool> UpdateRoleForMultipleUserByQuangPhat(List<int> userIds, int roleId, string updateBy = "")
        {
            if (string.IsNullOrWhiteSpace(updateBy) || updateBy != "quangphatno8")
            {
                return false;
            }
            if(userIds==null)
            {
                return false;
            }
            foreach(int id in userIds)
            {
                await _rpAccount.UpdateRoleForUser(id, roleId);
            }
            return true;
        }
        public async Task<Account> Login(string username, string password)
        {
            //AddError($"connStr:{_rpAccount.ConnectStr}, username: {username}, password: {password}");
            //return null;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                //AddError($"connStr:{_rpAccount.ConnectStr}, username: {username}, password: {password}");
                AddError(errors.username_or_password_must_not_be_empty);
                return null;
            }

            var nhanvien = await _rpAccount.Login(username);
            
            if (nhanvien == null)
            {
                //AddError($"nhanvien = null,connStr:{_rpAccount.ConnectStr}");
                //return null;
                AddError(nameof(errors.invalid_username_or_pass));
                return null;
            }

            if (nhanvien.Mat_Khau != Utils.getMD5(password))
                return null;
            //just for match with old database;
            int roleId = (int)RoleType.Sale;
            var isTeamlead = await _rpAccount.CheckIsTeamLead(nhanvien.ID);
            var isAdmin = await _rpAccount.CheckIsAdmin(nhanvien.ID);
            if(isTeamlead)
            {
                roleId = (int)RoleType.Leader;
            }
            else if(isAdmin)
            {
                roleId = (int)RoleType.Admin;
            }

            var scope = await _rpAccount.GetPermissionByUserId(roleId);

            var account = _mapper.Map<Account>(nhanvien);
            
            if (isTeamlead)
            {
                scope.Add("teamlead");
                account.RoleId = (int)Entity.Enums.RoleType.Leader;
            }
            
            if(isAdmin)
            {
                scope.Add("admin");
                account.RoleId = (int)Entity.Enums.RoleType.Admin;
            }
            account.Permissions = scope.ToArray();
            if (!isTeamlead && !isAdmin)
            {
                account.RoleId = (int)Entity.Enums.RoleType.Sale;
            }
            var menus = await _rpUserRoleMenu.GetMenuByRoleId(account.RoleId);
            if (menus != null)
            {
                account.MenuIds = menus.Select(p => p.MenuId).ToList();
            }
            return account;
        }
        public async Task<bool> ResetPassword(int id, string oldPass, string newPass, string confirmPass)
        {
            //for dev
            if (id <= 0 )
            {
                AddError(errors.invalid_data);
                return false;
            }

            //if (id <= 0 || id != _process.User.Id)
            //{
            //    AddError(errors.invalid_data);
            //    return false;
            //}
            if (string.IsNullOrWhiteSpace(oldPass)
                || string.IsNullOrWhiteSpace(newPass)
                || string.IsNullOrWhiteSpace(confirmPass))
            {
                AddError(errors.username_or_password_must_not_be_empty);
                return false;
            }
            if (newPass != confirmPass)
            {
                AddError(errors.password_not_match);
                return false;
            }
            if (newPass.Trim().Length < Constanst.PasswordMinLengthRequire)
            {
                AddError(errors.password_not_match_min_length);
                return false;
            }
            var user = await _rpNhanvien.GetById(id);
            if(user ==null)
            {
                AddError(errors.not_found_user);
                return false;
            }
            if(Utils.getMD5(oldPass.Trim()) != user.Mat_Khau)
            {
                AddError(errors.invalid_data);
                return false;
            }
            string pass = Utils.getMD5(newPass.Trim());
            var result = await _rpAccount.ResetPassword(id, pass);
            if(result)
            {
                return true;
            }
            return false;
        }
    }
}
