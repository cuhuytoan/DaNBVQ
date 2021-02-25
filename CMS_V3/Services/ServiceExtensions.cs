using HNM.DataNC.Models;
using HNM.DataNC.ModelsIdentity;
using HNM.DataNC.ModelsPayment;
using HNM.RepositoriesWeb.RepositoriesBase;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CMS_V3.Services
{
    public static class ServiceExtensions
    {
        //public static void ConfigureRepositoryWrapper(this IServiceCollection services)
        //{
        //    services.AddTransient<IRepositoryWrapper, RepositoryWrapper>();
        //}

        //public static void ConfigurePaymentRepositoryWrapper(this IServiceCollection services)
        //{
        //    services.AddScoped<IPaymentRepositoryWrapper, PaymentRepositoryWrapper>();
        //}

        public static void ConfigureConnectDB(this IServiceCollection services, string connectStrings)
        {
            services.AddDbContext<HanomaContext>(options =>
                options.UseSqlServer(connectStrings), ServiceLifetime.Transient);
        }
        public static void ConfigureConnectDBPayment(this IServiceCollection services, string connectStrings)
        {
            services.AddDbContext<PaymentContext>(options =>
                options.UseSqlServer(connectStrings), ServiceLifetime.Transient);
        }
        public static void ConfigureConnectDBAuth(this IServiceCollection services, string connectStrings)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectStrings), ServiceLifetime.Transient);
        }
        public static void ConfigureIdentity(this IServiceCollection services)
        {
            services.Configure<IdentityOptions>(options =>
            {
                // Default Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;
            });
            services.Configure<IdentityOptions>(options =>
            {
                // Default SignIn settings.
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            });

            services.Configure<IdentityOptions>(options =>
            {
                // Default User settings.
                options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;

            });
            //Cookie
            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.Cookie.Name = "JWT";
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.LoginPath = "/Identity/Account/Login";
                // ReturnUrlParameter requires 
                //using Microsoft.AspNetCore.Authentication.Cookies;
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                options.SlidingExpiration = true;
            });
        }

        public static void ConfigureHttpClient(this IServiceCollection services)
        {
            services.AddHttpClient<IRepositoryWrapper, RepositoryWrapper>(client =>
            {
                //client.BaseAddress = new Uri("http://api.hanoma.vn/");
                client.BaseAddress = new Uri("http://apitest.hanoma.vn/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json")); 
            });            
        }
       
    }
}
