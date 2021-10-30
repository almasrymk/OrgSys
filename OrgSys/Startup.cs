using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Repository;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

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



            //services.AddMvc(o =>
            //{
            //    o.Filters.Add(new AuthorizeFilter(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build()));

            //});


            services.AddOptions();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddMvc(options => options.EnableEndpointRouting = false);

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
            
            services.AddLocalization(options => options.ResourcesPath = "Resource");
            services.AddMvc().AddViewLocalization(options => options.ResourcesPath = "Resource").
                AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix).AddDataAnnotationsLocalization();

            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.AddSupportedUICultures("en", "ar");
                options.FallBackToParentUICultures = true;                
            });
            services.AddRazorPages().AddRazorRuntimeCompilation();
            services.AddRazorPages().AddViewLocalization();

            services.AddDbContext<OrgContext>(options => options.UseSqlServer(Configuration.GetConnectionString("OrgConnection")));
            services.AddControllersWithViews();
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

            var supportedCultures = new[] { "en", "ar" };
            var localizationOptions = new RequestLocalizationOptions().SetDefaultCulture(supportedCultures[0])
                .AddSupportedCultures(supportedCultures)
                .AddSupportedUICultures(supportedCultures);

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
            });
        }
    }
}
