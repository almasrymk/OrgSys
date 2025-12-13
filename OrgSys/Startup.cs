using Application.Commands.Org.City.Commands;
using AutoMapper;
using Domain.Abstraction;
using Entity;
using Infrastructure.Persistence.Data;
using Infrastructure.Persistence.UnitOfWork;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Repository;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace OrgSys
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
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;

            });

            services.AddOptions();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddMvc(options => options.EnableEndpointRouting = false);            
            
            services.AddLocalization(options => options.ResourcesPath = "Resource");
            services.AddMvc().AddViewLocalization(options => options.ResourcesPath = "Resource").
                AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix).AddDataAnnotationsLocalization();

            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.AddSupportedUICultures("en-gb", "ar-eg");
                options.FallBackToParentUICultures = true;                
            });
            services.AddRazorPages().AddRazorRuntimeCompilation();
            services.AddRazorPages().AddViewLocalization();

            string assemblyName = typeof(Repository.OrgContext).Namespace;
            services.AddDbContext<Repository.AdminContext>(options => options.UseSqlServer(Configuration.GetConnectionString("OrgConnection"), x => x.MigrationsHistoryTable("__AdminMigrationsHistory", "admin")));
            services.AddDbContext<Repository.OrgContext>(options => options.UseSqlServer(Configuration.GetConnectionString("OrgConnection"), x => x.MigrationsHistoryTable("__MigrationsHistory", "org")).ReplaceService<IModelCacheKeyFactory, Repository.DbSchemaAwareModelCacheKeyFactory>().ReplaceService<IMigrationsAssembly, Repository.DbSchemaAwareMigrationAssembly>());
            //services.AddDbContext<OrgContext>(options => options.UseSqlServer(Configuration.GetConnectionString("OrgConnection"), x => x.MigrationsAssembly(assemblyName)).ReplaceService<IModelCacheKeyFactory, DbSchemaAwareModelCacheKeyFactory>().ReplaceService<IMigrationsAssembly, DbSchemaAwareMigrationAssembly>());
            services.AddDbContext<Infrastructure.Persistence.Data.OrgContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("Default")));

            services.AddScoped<IOrgContext>(provider => provider.GetRequiredService<Infrastructure.Persistence.Data.OrgContext>());
            services.ConfigureApplicationCookie(options =>
            {
                //options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromDays(1000);
                options.LoginPath = "/Home/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                //options.SlidingExpiration = true;
            });

            services.AddMediatR(cfg =>
            {
                //cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);                      // Web layer
                cfg.RegisterServicesFromAssembly(typeof(CreateCommand).Assembly); // Application layer
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            //services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.LoginPath = "/Home/login";
                options.LogoutPath = "/Home/logout";
            });

            services.AddMvc(options => { 
                var p = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                options.Filters.Add(new AuthorizeFilter(p));
            }).AddXmlSerializerFormatters();
            services.AddAutoMapper(cfg => { cfg.AddProfile<MappingProfile>(); });
            services.AddAutoMapper(cfg => { cfg.AddProfile<MapperConfig>(); });
            services.AddControllersWithViews();

            services.AddSession();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
            
            var supportedCultures = new[] { "en-gb", "ar-eg" };
            var localizationOptions = new RequestLocalizationOptions().SetDefaultCulture(supportedCultures[0])
                .AddSupportedCultures(supportedCultures)
                .AddSupportedUICultures(supportedCultures);

            app.UseSession();
            app.UseRequestLocalization(localizationOptions);

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            var cookiePolicyOptions = new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.Strict,
            };
            app.UseCookiePolicy(cookiePolicyOptions);
            app.UseAuthentication();
            app.UseAuthorization();
          
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapRazorPages();

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapAreaControllerRoute(
                    name: "Setting",
                    areaName: "Setting",
                    pattern: "Setting/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapAreaControllerRoute(
                    name: "Invoices",
                    areaName: "Invoices",
                    pattern: "Invoices/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapAreaControllerRoute(
                 name: "Transactions",
                 areaName: "Transactions",
                 pattern: "Transactions/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapAreaControllerRoute(
                 name: "Orders",
                 areaName: "Orders",
                 pattern: "Orders/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapAreaControllerRoute(
                 name: "Financials",
                 areaName: "Financials",
                 pattern: "Financials/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapAreaControllerRoute(
                name: "Reports",
                areaName: "Reports",
                pattern: "Reports/{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}