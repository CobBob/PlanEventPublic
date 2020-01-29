using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PlanEvent.Data;

namespace PlanEvent
{
    public class Startup
    {
        public  Startup(IConfiguration configuration)
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
                // options.CheckConsentNeeded = context => true; 
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            //string connectionstring;

            services.AddTransient<IRepository, Repository>();

            services.AddDbContext<EventDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DeffaultConnection")));

            //services.AddDbContext<EventDbContext>(options =>
            //    options.UseSqlServer(Secrets.connectionstring));

            //TODO: figure out what exactly this piece of code does
            services.AddDefaultIdentity<IdentityUser>()
                .AddDefaultUI(Microsoft.AspNetCore.Identity.UI.UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<EventDbContext>();

            //services.AddDefaultIdentity<IdentityUser>(options =>
            //{
            //    options.Lockout.AllowedForNewUsers = true;
            //    options.Lockout.MaxFailedAccessAttempts = 3;
            //    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);

            //    options.User.AllowedUserNameCharacters = 
            //    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 -._@+";
            //});

            //services.ConfigureApplicationCookie(options =>
            //{
            //    options.Cookie.Name = "AuthCookiePlanEvent";
            //    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
            //    options.SlidingExpiration = false;
            //});

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
                app.UseExceptionHandler("/Home/CustomError");
                //app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseAuthentication();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}");
            });

            //app.Run(async (context) => await context.Response.WriteAsync(""));
        }
    }
}
