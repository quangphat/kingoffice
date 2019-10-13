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

namespace Business.Classes
{
    public class AccountBusiness :BaseBusiness, IAccountBusiness
    {
        protected readonly IAccountRepository _rpAccount;
        protected readonly IUserRoleMenuRepository _rpUserRoleMenu;
        private readonly AppSettings _appSettings;
        public AccountBusiness(IAccountRepository accountRepository,
            IUserRoleMenuRepository userRoleMenuRepository,
            IOptions<AppSettings> appSettings,
            CurrentProcess process, IMapper mapper):base(mapper,process)
        {
            _rpAccount = accountRepository;
            _rpUserRoleMenu = userRoleMenuRepository;
            _appSettings = appSettings.Value;
        }
        public async Task<Account> Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return null;
            var nhanvien = await _rpAccount.Login(username);
            if (nhanvien == null)
                return null;
            if (nhanvien.Mat_Khau != Utils.getMD5(password))
                return null;

            var account = _mapper.Map<Account>(nhanvien);
            var menus = await _rpUserRoleMenu.GetMenuByUserRole(account.Role);
            if(menus!=null)
            {
                account.MenuIds = menus.Select(p => p.MenuId).ToList();
            }
            //var tokenHandler = new JwtSecurityTokenHandler();
            //var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            //var tokenDescriptor = new SecurityTokenDescriptor
            //{
            //    Subject = new ClaimsIdentity(new Claim[]
            //    {
            //        new Claim(ClaimTypes.Name, account.UserName),
            //        new Claim(ClaimTypes.Role, account.Role)
            //    }),
            //    Expires = DateTime.UtcNow.AddDays(7),
            //    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            //};
            //var token = tokenHandler.CreateToken(tokenDescriptor);
            //account.Token = tokenHandler.WriteToken(token);
            return account;
        }
    }
}
