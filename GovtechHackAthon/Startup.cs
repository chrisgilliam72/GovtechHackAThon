using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GovtechDBLib.Models;
using GovtechHackAthon.Models;
using GovtechHackAthon.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GovtechHackAthon
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
            services.AddDbContextPool<GovtechHackathonContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("GovtechDBConnection")));
            services.AddDbContext<GovtechUsersAuthContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("IdentityDBConnection")));
            services.AddIdentity<IdentityUser, IdentityRole>(opt=>
            {
                opt.SignIn.RequireConfirmedEmail = true;
            }).AddEntityFrameworkStores<GovtechUsersAuthContext>()
            .AddDefaultTokenProviders();
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddTransient<DBLookupService>();
            services.AddTransient<DBAPI>();
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddMvc();
            services.AddDistributedMemoryCache(); // Adds a default in-memory implementation of IDistributedCache
            services.AddSession();
            services.AddHttpContextAccessor();

        
            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                options.LoginPath = "/Registration/Login";
                options.AccessDeniedPath = "/Registration/Login";
                options.SlidingExpiration = true;
            });
            services.AddApplicationInsightsTelemetry();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
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

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            CreateRoles(serviceProvider);
        }

        private void CreateRoles(IServiceProvider serviceProvider)
        {
            Task<IdentityResult> roleResult;
            Task<IdentityUser> userResult;
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager= serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            Task<bool> taskResult = roleManager.RoleExistsAsync("Admin");
            taskResult.Wait();
            if (!taskResult.Result)
            {
                IdentityRole identityRole = new IdentityRole("Admin");
                roleResult=roleManager.CreateAsync(identityRole);
                roleResult.Wait();
            }

            taskResult = roleManager.RoleExistsAsync("Applicant");
            if (!taskResult.Result)
            {
                IdentityRole identityRole = new IdentityRole("Applicant");
                roleResult = roleManager.CreateAsync(identityRole);
                roleResult.Wait();
            }

            taskResult = roleManager.RoleExistsAsync("Adjudicator");
            if (!taskResult.Result)
            {
                IdentityRole identityRole = new IdentityRole("Adjudicator");
                roleResult = roleManager.CreateAsync(identityRole);
                roleResult.Wait();
            }

            taskResult = roleManager.RoleExistsAsync("Investor");
            if (!taskResult.Result)
            {
                IdentityRole identityRole = new IdentityRole("Investor");
                roleResult = roleManager.CreateAsync(identityRole);
                roleResult.Wait();
            }

            taskResult = roleManager.RoleExistsAsync("Auditor");
            if (!taskResult.Result)
            {
                IdentityRole identityRole = new IdentityRole("Auditor");
                roleResult = roleManager.CreateAsync(identityRole);
                roleResult.Wait();
            }

            taskResult = roleManager.RoleExistsAsync("Mentor");
            if (!taskResult.Result)
            {
                IdentityRole identityRole = new IdentityRole("Mentor");
                roleResult = roleManager.CreateAsync(identityRole);
                roleResult.Wait();
            }

            userResult = userManager.FindByEmailAsync("admin@admin.com");
            userResult.Wait();
            if (userResult.Result==null)
            {
                var identityUser = new IdentityUser { UserName = "admin@admin.com", Email = "admin@admin.com" };
                roleResult = userManager.CreateAsync(identityUser, "Password01+");
                roleResult.Wait();
                roleResult = userManager.AddToRoleAsync(identityUser, "Admin");
                roleResult.Wait();

                Task<string> tokenResult = userManager.GenerateChangeEmailTokenAsync(identityUser, "admin@admin.com");
                tokenResult.Wait();
                roleResult = userManager.ConfirmEmailAsync(identityUser, tokenResult.Result);
                roleResult.Wait();
            }

        }
    }
}
