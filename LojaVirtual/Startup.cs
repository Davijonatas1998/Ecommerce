using LojaVirtual.Context;
using LojaVirtual.Models.Config_Arquivos;
using LojaVirtual.Models.ModelsUsuario;
using LojaVirtual.Repository;
using LojaVirtual.Repository.Interface;
using LojaVirtual.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;
using System;
using System.Net;

namespace LojaVirtual
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
            string SqlConnection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<Contexto>(options => options.UseMySql(SqlConnection, ServerVersion.AutoDetect(SqlConnection)));
            services.AddControllersWithViews();
            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<Contexto>().AddDefaultTokenProviders();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin",
                  politica =>
                  {
                      politica.RequireRole("Admin");
                  });
            });

            services.AddScoped<ISeedUserRoleInitial, SeedUserRoleInitial>();
            services.AddTransient<ICheckout, RepositoryBase>();
            services.AddTransient<IProfileUser, RepositoryBase>();
            services.AddTransient<IProduto, RepositoryBase>();
            services.Configure<ConfigurationImagens>(Configuration.GetSection("ConfigurationPastaImagensProdutos"));

            services.Configure<IdentityOptions>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZáàãâéêíóôõúüçÁÀÃÂÉÊÍÓÔÕÚÜÇ0123456789!@#$%^&*()_+-=[]{}|;':\",./<>? ";
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
            });

            services.AddPaging(options =>
            {
                options.ViewName = "Bootstrap5";
                options.PageParameterName = "pageindex";
            });

            services.AddMemoryCache();
            services.AddHttpClient();
            services.AddRazorPages();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(1440);
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ISeedUserRoleInitial seedUserRoleInitial)
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

            seedUserRoleInitial.SeedRoles();
            seedUserRoleInitial.SeedUsers();

            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                 name: "areas",
                 pattern: "{area:exists}/{controller=Produtos}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapRazorPages();
            });
        }
    }
}