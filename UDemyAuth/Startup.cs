using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UDemyAuth.CustomValidation;
using UDemyAuth.Models;

namespace UDemyAuth
{
    public class Startup
    {

        public IConfiguration configuration { get; }

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppIdentityDbContext>(opt =>
            {
                opt.UseSqlServer(configuration.GetConnectionString("DefaultConnectionString"));

            });



            services.AddIdentity<AppUser, AppRole>(opt =>
            {


                opt.User.RequireUniqueEmail = true;  //error var həll ele
                opt.User.AllowedUserNameCharacters = "abcçdeəfgğhiıjklmnoöpqrsştuüvwxyzABCÇDEƏFGHIİJKLMNOÖPQRSŞTUÜVWXYZ0123456789-._";
                opt.Password.RequiredLength = 4;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireDigit = false;

            }).AddErrorDescriber<CustomIdentityErrorDescriber>()
            .AddUserValidator<CustomUserValidator>()
            .AddPasswordValidator<CustomPasswordValidator>()
            .AddEntityFrameworkStores<AppIdentityDbContext>()
            .AddDefaultTokenProviders();


           



            CookieBuilder cookieBuilder = new CookieBuilder();



            cookieBuilder.Name = "MyBlog";
            cookieBuilder.HttpOnly = false; // client terefde cookie erisemesinler.
            //cookieBuilder.Expiration = TimeSpan.FromDays(30);
            cookieBuilder.SameSite = SameSiteMode.Lax;    //(default (Lax) gelir) başqa saytdan senin uzerinen başqa sayta sorgu gede bilər,
                                                          //eger web sehife odenisle baglı deilse bele qalsa olar
                                                          //(Strict) yazilsa onu engelir

            cookieBuilder.SecurePolicy = CookieSecurePolicy.SameAsRequest;  // (Always)Istek HTTPS uzerinen gelibse cookini gonderir
                                                                            // en salamati (SameAsRequest)                




            services.ConfigureApplicationCookie(opt =>
            {
                opt.LoginPath = new PathString("/Home/Login");

                opt.LogoutPath = new PathString("/Member/LogOut");

                opt.Cookie = cookieBuilder;

                opt.SlidingExpiration = true;  //  istifadeci her girdiyinde cookinin mudetine 5 deq elave olunacaq

                opt.ExpireTimeSpan = TimeSpan.FromDays(20);

                opt.AccessDeniedPath = new PathString("/Member/AccessDenied");

            });

            services.AddMvc();

        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            app.UseStatusCodePages();   //

            app.UseStaticFiles();

            app.UseCookiePolicy();

            app.UseAuthentication();


            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });




        }
    }
}
