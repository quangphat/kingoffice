﻿using System;
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
                return ToFailResponse();
            if (!isValidAccount(account)) return ToFailResponse();

            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim("Id", account.Id.ToString()));
            claims.Add(new Claim("UserName", account.UserName));
            if (!string.IsNullOrWhiteSpace(account.Email))
                claims.Add(new Claim("Email", account.Email));
            if (!string.IsNullOrWhiteSpace(account.Code))
                claims.Add(new Claim("Code", account.Code));
            if (!string.IsNullOrWhiteSpace(account.Role))
                claims.Add(new Claim("Role", account.Role));
            if (!string.IsNullOrWhiteSpace(account.Scope))
            {
                claims.Add(new Claim("Scope", String.Join(",", account.Scope)));
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
        private bool isValidAccount(Account account)
        {
            if (account == null || account.Id <= 0
                || string.IsNullOrWhiteSpace(account.UserName))
                return false;
            return true;
        }
    }
}