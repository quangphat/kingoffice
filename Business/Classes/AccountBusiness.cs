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

namespace Business.Classes
{
    public class AccountBusiness : BaseBusiness, IAccountBusiness
    {
        protected readonly IAccountRepository _rpAccount;
        protected readonly IUserRoleMenuRepository _rpUserRoleMenu;
        private readonly AppSettings _appSettings;
        public AccountBusiness(IAccountRepository accountRepository,
            IUserRoleMenuRepository userRoleMenuRepository,
            IOptions<AppSettings> appSettings,
            CurrentProcess process, IMapper mapper) : base(mapper, process)
        {
            _rpAccount = accountRepository;
            _rpUserRoleMenu = userRoleMenuRepository;
            _appSettings = appSettings.Value;
        }
        public async Task<Account> Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                AddError(errors.username_or_password_must_not_be_empty);
                return null;
            }

            var nhanvien = await _rpAccount.Login(username);

            if (nhanvien == null)
            {
                AddError(nameof(errors.invalid_username_or_pass));
                return null;
            }

            if (nhanvien.Mat_Khau != Utils.getMD5(password))
                return null;
            var scope = await _rpAccount.GetPermissionByUserId(nhanvien.ID);
            var account = _mapper.Map<Account>(nhanvien);
            account.Permissions = scope.ToArray();
            var menus = await _rpUserRoleMenu.GetMenuByRoleId(account.RoleId);
            if (menus != null)
            {
                account.MenuIds = menus.Select(p => p.MenuId).ToList();
            }
            return account;
        }
    }
}
