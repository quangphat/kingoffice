using Entity.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace KingOffice.Infrastructures
{
    public static class WebExtensions
    {
        public static void AddMyAuthentication(this IServiceCollection services, IConfiguration configuration, string applicationName, string cookieName = "my8ProgramingBlog")
        {

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            //DataProtectionBuilderExtensions.PersistKeysToFileSystem(DataProtectionBuilderExtensions.SetDefaultKeyLifetime(DataProtectionBuilderExtensions.SetApplicationName(DataProtectionServiceCollectionExtensions.AddDataProtection(services), applicationName), TimeSpan.FromDays(36500.0)), new DirectoryInfo("/keys"));
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(option => {
                    option.ExpireTimeSpan = TimeSpan.FromDays(30);
                    option.SlidingExpiration = true;
                    option.Cookie.Name = applicationName;
                    option.LoginPath = "/Account/Login";
                   
                });
        }
        public static void AddMyAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(
                options =>
                {
                    options.AddPolicy(
                        "admin",
                        p => { p.RequireAssertion(context => context.User.HasClaim("Scope", "admin")); });
                    options.AddPolicy(
                        "duyethoso",
                        p =>
                        {
                            p.RequireAssertion(
                                context =>
                                {
                                    return context.User?.Identity.IsAuthenticated == true &&
                                           (context.User.HasClaim("Scope", "admin") ||
                                            context.User.HasClaim("Scope", "duyethoso"));
                                });
                        });
                });
        }
        public static Account GetUserInfo(this HttpContext context)
        {
            IIdentity identity = (context != null) ? context.User?.Identity : null;
            if (identity != null && identity.IsAuthenticated)
            {
                List<Claim> list = context.User.Claims?.ToList();
                if (list != null && list.Count != 0)
                {
                    int id = 0;
                    string orgId = string.Empty;
                    Account account = new Account();
                    account.Id = id;
                    account.UserName = list.FirstOrDefault((Claim a) => a.Type == "UserName")?.Value;
                    account.Code = list.FirstOrDefault((Claim a) => a.Type == "Code")?.Value;
                    account.Id = Convert.ToInt32(list.FirstOrDefault((Claim a) => a.Type == "Id")?.Value);
                    account.Email = list.FirstOrDefault((Claim a) => a.Type == "Email")?.Value;
                    account.Role = list.FirstOrDefault((Claim a) => a.Type == "Role")?.Value;
                    account.FullName = list.FirstOrDefault((Claim a) => a.Type == "FullName")?.Value;
                    string scopeStr = list.FirstOrDefault((Claim a) => a.Type == "Scope")?.Value;
                    if (!string.IsNullOrWhiteSpace(scopeStr))
                    {
                        account.Scope = scopeStr;
                    }
                    string menuStr = list.FirstOrDefault((Claim a) => a.Type == "MenuIds")?.Value;
                    if (!string.IsNullOrWhiteSpace(menuStr))
                    {
                        account.MenuIds = menuStr.Split(',').Select(p=>Convert.ToInt32(p)).ToList();
                    }
                    return account;
                }
                return null;
            }
            return null;
        }
    }
}
