using HNM.DataNC.Models;
using HNM.DataNC.ModelsIdentity;
using HNM.DataNC.ModelsPayment;
using HNM.RepositoriesNC.PaymentRepositoriesBase;
using HNM.RepositoriesNC.RepositoriesBase;
using HNM.WebApiNC.Logging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using System.Threading.Tasks;

namespace HNM.WebApiNC.Services
{
    public static class ServiceExtensions
    {
        public static void ConfigureRepositoryWrapper(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
            services.AddSingleton<IHttpContextAccessor,
            HttpContextAccessor>();
        }
        public static void ConfigurePaymentRepositoryWrapper(this IServiceCollection services)
        {
            services.AddScoped<IPaymentRepositoryWrapper, PaymentRepositoryWrapper>();
        }
        public static void ConfigureLogging(this IServiceCollection services)
        {
            services.AddScoped<ILoggerManager, LoggerManager>();
        }

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

        public static void ConfigureJWT(this IServiceCollection services, string JWTKey)
        {
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        //ValidIssuer = Configuration["JwtIssuer"],
                        //ValidAudience = Configuration["JwtIssuer"],
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTKey)),
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero // remove delay of token when expire

                    };
                    cfg.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            {
                                context.Response.Headers.Add("Token-Expired", "true");
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

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
            //services.ConfigureApplicationCookie(options =>
            //{
            //    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
            //    options.Cookie.Name = "YourAppCookieName";
            //    options.Cookie.HttpOnly = true;
            //    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
            //    options.LoginPath = "/Identity/Account/Login";
            //    // ReturnUrlParameter requires 
            //    //using Microsoft.AspNetCore.Authentication.Cookies;
            //    options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
            //    options.SlidingExpiration = true;
            //});
        }


    }
}
