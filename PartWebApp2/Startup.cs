using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PartWebApp2.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using PartWebApp2.Services;

namespace PartWebApp2
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<PartiesService>();
            services.AddControllersWithViews();

            services.AddDbContext<PartyWebAppContext>(options =>
                            options.UseSqlServer(Configuration.GetConnectionString("PartyWebAppContext")));

            services.AddTransient<ISpotifyClientService, SpotifyClientService>();
            services.AddTransient<IManageService, ManagerService>();

            //services.AddTransient<IFacebookService, FacebookService>();

            services.AddSession(options => 
            {
                options.IdleTimeout = TimeSpan.FromMinutes(10); 

            }
            );

            // אימות מבוסס מנגנון קוקי
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(
                options =>
                {
                    options.LoginPath = new PathString("/Users/Login");
                    options.AccessDeniedPath = new PathString("/Users/AccessDenied");
                });
            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseSession(); 
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Users}/{action=Login}/{id?}");
            });
        }
    }
}
