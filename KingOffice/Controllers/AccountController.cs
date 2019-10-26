using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Business.Interfaces;
using Entity.Infrastructures;
using Entity.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace KingOffice.Controllers
{
    public class AccountController : BaseController
    {
        protected readonly IAccountBusiness _bizAccount;
        public AccountController(CurrentProcess currentProcess, IAccountBusiness accountBusiness)
            : base(currentProcess)
        {
            _bizAccount = accountBusiness;
        }
        public IActionResult GetAccount()
        {
            if(_process==null || _process.User==null)
            {
                return null;
            }
            var account = new AccountJson(_process.User);
            return ToResponse(account);
        }
        public ActionResult Login()
        {
            if (_process.User != null)
                return RedirectToAction("Index", "Home");
            ViewBag.SaveAccount = true;
            ViewBag.UserName = "";
            ViewBag.Password = "";
            return View();
        }
        public async Task<IActionResult> DangNhap(string userName, string password, string rememberMe)
        {
            var account = await _bizAccount.Login(userName,password);
            if (account == null)
                return ToResponse();
            if (!isValidAccount(account)) return ToResponse();

            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim("Id", account.Id.ToString()));
            claims.Add(new Claim("UserName", account.UserName));
            if (!string.IsNullOrWhiteSpace(account.FullName))
                claims.Add(new Claim("FullName", account.FullName));
            if (!string.IsNullOrWhiteSpace(account.Email))
                claims.Add(new Claim("Email", account.Email));
            if (!string.IsNullOrWhiteSpace(account.Code))
                claims.Add(new Claim("Code", account.Code));
            if (!string.IsNullOrWhiteSpace(account.Role))
                claims.Add(new Claim("Role", account.Role));
            
            if (account.MenuIds!=null && account.MenuIds.Any())
            {
                var menusStr = string.Join(",", account.MenuIds.ToArray());
                claims.Add(new Claim("MenuIds", menusStr));
            }
            if (account.Scopes.Any())
            {
                claims.Add(new Claim("Scopes", string.Join(",", account.Scopes.ToArray())));
            }
            var userIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            
            ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true
            };
            await HttpContext.SignInAsync(principal, authProperties);
            ViewBag.Account = account;
            return ToResponse(account);
        }
        public async Task<IActionResult> DangXuat()
        {
            HttpContext.Session.Clear();
            return new SignOutResult(new[] { "Cookies" });
        }
        private bool isValidAccount(Account account)
        {
            if (account == null || account.Id <= 0
                || string.IsNullOrWhiteSpace(account.UserName))
                return false;
            return true;
        }
    }
}