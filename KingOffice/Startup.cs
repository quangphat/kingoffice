using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.Classes;
using Business.Infrastructures;
using Business.Interfaces;
using Entity.Infrastructures;
using KingOffice.Infrastructures;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Repository.Classes;
using Repository.Interfaces;

namespace KingOffice
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddSession();
            services.AddSingleton<IHosoRepository, HosoRepository>();
            services.AddSingleton<IAccountRepository, AccountRepository>();
            services.AddSingleton<IUserRoleMenuRepository, UserRoleMenuRepository>();
            services.AddSingleton<ILocationRepository, LocationRepository>();
            services.AddSingleton<INhanvienRepository, NhanvienRepository>();
            services.AddSingleton<ITailieuRepository, TailieuRepository>();
            services.AddSingleton<IDoitacRepository, DoitacRepository>();
            services.AddSingleton<IProductRepository, ProductRepository>();
            services.AddSingleton<IAutoIdRepository, AutoIdRepository>();
            services.AddSingleton<INotesRepository, NotesRepository>();
            services.AddSingleton<IRoleRepository, RoleRepository>();
            //AppendMongoDI

            services.AddScoped<IHosoBusiness, HosoBusiness>();
            services.AddScoped<IAccountBusiness, AccountBusiness>();
            services.AddScoped<ILocationBusiness, LocationBusiness>();
            services.AddScoped<IDoitacBusiness, DoitacBusiness>();
            services.AddScoped<ITailieuBusiness, TailieuBusiness>();
            services.AddScoped<IProductBusiness, ProductBusiness>();
            services.AddScoped<INhanvienBusiness, NhanvienBusiness>();
            services.AddScoped<IMediaBusiness, MediaBusiness>();
            services.AddScoped<IRoleBusiness, RoleBusiness>();
            //<AppendBusinessDI>
            services.AddScoped<F88ServiceApi.F88Service>();
            services.AddScoped<CurrentProcess>();
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            services.Configure<F88Api>(Configuration.GetSection("F88Api"));
            services.Configure<RequestLocalizationOptions>(
                        opts =>
                        {
                            var supportedCultures = new[]
                            {

                        new CultureInfo("vi-VN"),
                            };

                            opts.DefaultRequestCulture = new RequestCulture("vi-VN");
                            // Formatting numbers, dates, etc.
                            opts.SupportedCultures = supportedCultures;
                            // UI strings that we have localized.
                            opts.SupportedUICultures = supportedCultures;
                        });
            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddMyAuthentication(Configuration, "kingoffices");
            services.AddMyAuthorization();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseSession();
            app.UseMiddleware<SessionHandler>();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
