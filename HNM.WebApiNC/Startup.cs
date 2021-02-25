using AutoMapper;
using Elastic.Apm.AspNetCore;
using HNM.DataNC.ModelsIdentity;
using HNM.DataNC.ModelsNC.ModelsUtility;
using HNM.WebApiNC.Logging;
using HNM.WebApiNC.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using NLog;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;

namespace HNM.WebApiNC
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // ===== Add Logging ================================
            services.ConfigureLogging();

            // ===== Add Database ===============================
            services.ConfigureConnectDB(Configuration.GetConnectionString("hanomaEntities"));

            // ===== Add Database Auth===========================
            services.ConfigureConnectDBAuth(Configuration.GetConnectionString("hanomaEntities"));

            // ===== Add Database Payment========================
            services.ConfigureConnectDBPayment(Configuration.GetConnectionString("paymentEntities"));

            //Config Service Log Request
            services.AddTransient<ApiLogService>();

            // ===== Add DI Services Wraper =====================
            services.ConfigureRepositoryWrapper();

            // ===== Add DI Payment Services Wraper =============
            services.ConfigurePaymentRepositoryWrapper();

            // ===== Add FCMMessage Services ====================
            //services.ConfigureFCMMessageServices();            
            // ===== Config AutoMaper ===========================
            services.AddAutoMapper(typeof(Startup));

            // ===== Add Controller =============================
            services.AddControllers();

            // ===== Add Identity ========
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // ===== Config Identity ========
            services.ConfigureIdentity();

            // ===== Add Jwt Authentication =====================
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims            
            services.ConfigureJWT(Configuration["JwtKey"]);

            // ===== Add MVC =====================
            services.AddMvc();
            //services.AddMvc(option => option.EnableEndpointRouting = false);
            // ===== Add HttpContext =====================
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            // ===== Add application services.====
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
            //Config TokenLifeSpan
            services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromDays(1); // .FromDays(1) ...
            });
            //Account LogOut
            services.Configure<IdentityOptions>(options =>
            {
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                // Default SignIn settings.
                options.SignIn.RequireConfirmedEmail = true;
                options.SignIn.RequireConfirmedPhoneNumber = true;
            });
            //Config Swagger
            services.AddSwaggerGen(p =>
            {
                p.SwaggerDoc("v1",
                        new OpenApiInfo() { Title = "Hanoma API", Description = "Update GetListProduct, GetListRecruitment, Remove GetListAccesories, Remove GetListMaterials" });
                var xmlPatch = System.AppDomain.CurrentDomain.BaseDirectory + @"HNM.WebApiNC.xml";
                p.IncludeXmlComments(xmlPatch);
            });
            //add memory cache
            //services.AddMemoryCache();
            //add use Redis cache
            services.AddStackExchangeRedisCache(option =>
            {
                option.Configuration = "localhost";
                option.InstanceName = "Hanoma";
            });
            //Đồng bộ pashwwordHassh với web đang dùng mvc 
            services.Configure<PasswordHasherOptions>(options => options.CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV2);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<GCMiddleware>();
            app.UseExceptionHandler(a => a.Run(async context =>
            {
                var feature = context.Features.Get<IExceptionHandlerPathFeature>();
                var exception = feature.Error;

                var result = JsonConvert.SerializeObject(new ModelBase { Message = exception.Message });
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(result);
            }));
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //IdentityModelEventSource.ShowPII = true;
            }
            else
            {
                app.UseHsts();
            }
            //Add our new middleware to the pipeline
            app.UseMiddleware<ApiLoggingMiddleware>();

            app.UseRouting();
            // ===== Use Authentication ======
            app.UseAuthentication();

            app.UseAuthorization();

            //Accept All HTTP Request Methods from all origins
            app.UseCors(builder =>
                builder.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseStaticFiles();
            //Add use MVC
            //app.UseMvc();
            //Swagger
            app.UseSwagger();
            app.UseSwaggerUI(p =>
            {
                p.SwaggerEndpoint("/swagger/v1/swagger.json", "Hanoma API");

            });

            // add elastic
            app.UseElasticApm(Configuration);
        }
    }
}
