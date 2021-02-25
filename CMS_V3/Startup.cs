using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Threading.Tasks;
using AutoMapper;
using CMS_V3.Services;
using HNM.DataNC.ModelsIdentity;
using HNM.RepositoriesWeb.Repositories;
using HNM.RepositoriesWeb.RepositoriesBase;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace CMS_V3
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
            // Add framework services.
            services
                .AddControllersWithViews()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

            // Add Kendo UI services to the services container
            services.AddKendo();

            // ===== Add Logging ================================
            //services.ConfigureLogging();

            // ===== Config AutoMaper ===========================
            services.AddAutoMapper(typeof(Startup));

            // ===== Config Identity ========
            services.ConfigureIdentity();

            // ===== Add HttpContext =====================            
            services.AddHttpContextAccessor();

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // ===== Config Repositories ========
            //services.ConfigureRepositoryWrapper();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(option => { 
                    option.LoginPath = "/Account/RequireLogin";
                    option.ExpireTimeSpan = TimeSpan.FromDays(30);
                    option.SlidingExpiration = true;
                });
            // Add Policy
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Authenticated", policy =>
                {
                    policy.AddAuthenticationSchemes(
                        CookieAuthenticationDefaults.AuthenticationScheme)
                            .RequireAuthenticatedUser();
                });
            });

            //Account LogOut
            //services.Configure<IdentityOptions>(options =>
            //{
            //    options.Lockout.MaxFailedAccessAttempts = 10;
            //    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
            //    // Default SignIn settings.
            //    options.SignIn.RequireConfirmedEmail = true;
            //    options.SignIn.RequireConfirmedPhoneNumber = true;

            //    // Lockout settings.
            //    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            //    options.Lockout.MaxFailedAccessAttempts = 5;
            //    options.Lockout.AllowedForNewUsers = true;
            //});
            //services.ConfigureApplicationCookie(options =>
            //{
            //    // Cookie settings
            //    options.Cookie.HttpOnly = true;
            //    options.ExpireTimeSpan = TimeSpan.FromDays(1);
            //    options.LoginPath = "/Account/AccessDenied";
            //    options.AccessDeniedPath = "/Account/AccessDenied";
            //    options.SlidingExpiration = true;                
            //});
            //Đồng bộ pashwwordHassh với web đang dùng mvc 
            services.Configure<PasswordHasherOptions>(options => options.CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV2);
            //HttpClient
            services.ConfigureHttpClient();
            //Caching Redis
            services.AddDistributedRedisCache(option =>
            {
                option.Configuration = "localhost";
                option.InstanceName = "Hanoma";
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            
            {
                //app.UseDeveloperExceptionPage();
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
            }
            else
            {
                //app.UseExceptionHandler("/Home/Error");
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            // Use static files
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    // Cache static files for 30 days
                    ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=2592000");
                    ctx.Context.Response.Headers.Append("Expires", DateTime.UtcNow.AddDays(30).ToString("R", System.Globalization.CultureInfo.InvariantCulture));
                }
            });


            app.UseRouting();

         
            //use response caching
            app.UseResponseCaching();

            app.UseAuthentication();
            app.UseAuthorization();
            app.Use(async (ctx, next) =>
            {
                await next();

                if (ctx.Response.StatusCode == 500 && !ctx.Response.HasStarted)
                {
                    //Re-execute the request so the user gets the error page
                    string originalPath = ctx.Request.Path.Value;
                    ctx.Items["originalPath"] = originalPath;
                    ctx.Request.Path = "/Error/404";
                    await next();
                }
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "Shopman",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");             

                // cần mua vật tư
                endpoints.MapControllerRoute(
                   name: "MaterialDetail",
                   pattern: "{url}.pro",
                   constraints: new { url = @"^can-mua-vat-tu-([_0-9a-z-]+)$" },
                   defaults: new { controller = "ManagerDemand", action = "MaterialDetail" }
                   );

                // cần mua phụ tùng
                endpoints.MapControllerRoute(
                   name: "AccessoriDetail",
                   pattern: "{url}.pro",
                   constraints: new { url = @"^can-mua-phu-tung-([_0-9a-z-]+)$" },
                   defaults: new { controller = "ManagerDemand", action = "AccessoriDetail" }
                   );

                // cần mua thiết bị
                endpoints.MapControllerRoute(
                   name: "DemandDetail",
                   pattern: "{url}.pro",
                   constraints: new { url = @"^can-mua-([_0-9a-z-]+)$" },
                   defaults: new { controller = "ManagerDemand", action = "DemandDetail" }
                   );

                //cần thuê thiết bị
                endpoints.MapControllerRoute(
                   name: "DemandRentProductDetails",
                   pattern: "{url}.pro",
                   constraints: new { url = @"^can-thue-can-thue-thiet-bi-([_0-9a-z-]+)$" },
                   defaults: new { controller = "ManagerDemand", action = "RentDetail" }
                   );

                //cần thuê dịch vụ
                endpoints.MapControllerRoute(
                   name: "EmployDetail",
                   pattern: "{url}.pro",
                   constraints: new { url = @"^can-thue-dich-vu-([_0-9a-z-]+)$" },
                   defaults: new { controller = "ManagerDemand", action = "EmployDetail" }
                   );


                endpoints.MapControllerRoute(
                   name: "Search",
                   pattern: "tim-kiem",
                   defaults: new { controller = "Search", action = "Index" }
                   );
                //bài viêt
                endpoints.MapControllerRoute(
                   name: "Article",
                   pattern: "tin-tuc",
                   defaults: new { controller = "Article", action = "Index" }
                   );

                //bài viêt
                endpoints.MapControllerRoute(
                   name: "ArticleCate",
                   pattern: "chuyen-muc/{url}",
                   defaults: new { controller = "Article", action = "ArticleCate" }
                   );

                //video
                endpoints.MapControllerRoute(
                   name: "Video",
                   pattern: "chuyen-muc-video",
                   defaults: new { controller = "Video", action = "Index" }
                   );

                //video theo chuyen muc
                endpoints.MapControllerRoute(
                   name: "VideoCate",
                   pattern: "chuyen-muc-video/{url}",
                   defaults: new { controller = "Video", action = "VideoCate" }
                   );

                //Chi tiết bài viêt
                endpoints.MapControllerRoute(
                   name: "ArticleDetail",
                   pattern: "bai-viet/{url}",
                   defaults: new { controller = "Article", action = "ArticleDetail" }
                   );

                //Chi tiết video
                endpoints.MapControllerRoute(
                   name: "VideoDetail",
                   pattern: "video/{url}",
                   defaults: new { controller = "Video", action = "VideoDetail" }
                   );

                //Kiến thức
                endpoints.MapControllerRoute(
                   name: "Library",
                   pattern: "thu-vien-kien-thuc",
                   defaults: new { controller = "Library", action = "Index" }
                   );

                //Kiến thức
                endpoints.MapControllerRoute(
                   name: "Library",
                   pattern: "thu-vien-kien-thuc/{url}",
                   defaults: new { controller = "Library", action = "LibraryCate" }
                   );

                //Chi tiết kiến thức
                endpoints.MapControllerRoute(
                   name: "LibraryDetail",
                   pattern: "kien-thuc/{url}",
                   defaults: new { controller = "Library", action = "LibraryDetail" }
                   );
                //Chuyên mục dịch vụ
                endpoints.MapControllerRoute(
                   name: "ServiceCategory",
                   pattern: "{url}.pcat",
                   constraints: new { url = @"^dich-vu-([_0-9a-z-]+)$" },
                   defaults: new { controller = "Service", action = "ServiceByCate" }
                   );

                //Dịch vụ
                endpoints.MapControllerRoute(
                   name: "Service",
                   pattern: "dich-vu",
                   defaults: new { controller = "Service", action = "Index" }
                   );

                //Dịch vụ
                endpoints.MapControllerRoute(
                   name: "ServiceDetail",
                   pattern: "{url}.pro",
                   constraints: new { url = @"^dich-vu-([_0-9a-z-]+)$" },
                   defaults: new { controller = "Service", action = "ServiceDetail" }
                   );

                //May de ban - all
                endpoints.MapControllerRoute(
                   name: "SellingProduct",
                   pattern: "may-de-ban",
                   defaults: new { controller = "Product", action = "SellingProduct" }
                   );

                endpoints.MapControllerRoute(
                   name: "DemandProduct",
                   pattern: "can-mua-thiet-bi",
                   defaults: new { controller = "Demand", action = "DemandProduct" }
                   );

                endpoints.MapControllerRoute(
                   name: "DemandMaterial",
                   pattern: "can-mua-vat-tu",
                   defaults: new { controller = "Demand", action = "DemandMaterial" }
                   );

                endpoints.MapControllerRoute(
                   name: "DemandAccessories",
                   pattern: "can-mua-phu-tung",
                   defaults: new { controller = "Demand", action = "DemandAccessories" }
                   );

                endpoints.MapControllerRoute(
                   name: "DemandRentProduct",
                   pattern: "can-thue-thiet-bi",
                   defaults: new { controller = "Demand", action = "DemandRentProduct" }
                   );

                endpoints.MapControllerRoute(
                   name: "DemandRentProduct",
                   pattern: "can-thue-dich-vu",
                   defaults: new { controller = "Demand", action = "DemandRentEmploy" }
                   );

                endpoints.MapControllerRoute(
                   name: "DemandRentEmploy",
                   pattern: "can-mua-phu-tung",
                   defaults: new { controller = "Demand", action = "DemandRentEmploy" }
                   );

                //May cho thuê
                endpoints.MapControllerRoute(
                   name: "RentProduct",
                   pattern: "cho-thue-may",
                   defaults: new { controller = "Product", action = "RentProduct" }
                   );

                //May cho thuê - chi tiet
                endpoints.MapControllerRoute(
                   name: "SellingProductDetails",
                   pattern: "{url}.pro",
                   constraints: new { url = @"^cho-thue-([_0-9a-z-]+)$" },
                   defaults: new { controller = "Product", action = "RentProductDetails" }
                   );

                //ban vật tư - tất cả
                endpoints.MapControllerRoute(
                   name: "SellingMaterial",
                   pattern: "ban-vat-tu",
                   defaults: new { controller = "Product", action = "SellingMaterial" }
                   );



                // phụ tùng
                endpoints.MapControllerRoute(
                   name: "Accessories",
                   pattern: "ban-phu-tung",
                   defaults: new { controller = "Product", action = "Accessories" }
                   );
                // phụ tùng
                endpoints.MapControllerRoute(
                   name: "Accessories",
                   pattern: "{url}.pro",
                   constraints: new { url = @"^ban-phu-tung-([_0-9a-z-]+)$" },
                   defaults: new { controller = "Product", action = "AccessoriesDetails" }
                   );
                //Máy để bán - máy cho thue - cate cấp 1
                endpoints.MapControllerRoute(
                   name: "RentProductCategory",
                   pattern: "{url}.pcat",
                   constraints: new { url = @"^cho-thue-([_0-9a-z-]+)$" },
                   defaults: new { controller = "Product", action = "RentProductCategory" }
                   );
                //Máy để bán - ban phu tung - cate cấp 1
                endpoints.MapControllerRoute(
                   name: "AccessoriesProductCategory",
                   pattern: "{url}.pcat",
                   constraints: new { url = @"^ban-phu-tung-([_0-9a-z-]+)$" },
                   defaults: new { controller = "Product", action = "AccessoriesProductCategory" }
                   );
                //Máy để bán - ban phu tung - cate cấp 1
                endpoints.MapControllerRoute(
                   name: "MaterialProductCategory",
                   pattern: "{url}.pcat",
                   constraints: new { url = @"^ban-vat-tu-([_0-9a-z-]+)$" },
                   defaults: new { controller = "Product", action = "MaterialProductCategory" }
                   );
                //Máy để bán - máy công trình - cate cấp 1
                endpoints.MapControllerRoute(
                   name: "SellingProductCategory",
                   pattern: "{url}.pcat",
                   constraints: new { url = @"^ban-([_0-9a-z-]+)$" },
                   defaults: new { controller = "Product", action = "SellingProductCategory" }
                   );

                endpoints.MapControllerRoute(
                   name: "DemandBuyProductCategory",
                   pattern: "{url}.pcat",
                   constraints: new { url = @"^can-mua-thiet-bi-([_0-9a-z-]+)$" },
                   defaults: new { controller = "Demand", action = "DemandBuyProductCategory" }
                   );

                endpoints.MapControllerRoute(
                   name: "DemandBuyMaterialCategory",
                   pattern: "{url}.pcat",
                   constraints: new { url = @"^can-mua-vat-tu-([_0-9a-z-]+)$" },
                   defaults: new { controller = "Demand", action = "DemandBuyMaterialCategory" }
                   );

                endpoints.MapControllerRoute(
                   name: "DemandBuyAccessoriesCategory",
                   pattern: "{url}.pcat",
                   constraints: new { url = @"^can-mua-phu-tung-([_0-9a-z-]+)$" },
                   defaults: new { controller = "Demand", action = "DemandBuyAccessoriesCategory" }
                   );

                endpoints.MapControllerRoute(
                   name: "DemandRentProductCategory",
                   pattern: "{url}.pcat",
                   constraints: new { url = @"^can-thue-thiet-bi-([_0-9a-z-]+)$" },
                   defaults: new { controller = "Demand", action = "DemandRentProductCategory" }
                   );
                
                endpoints.MapControllerRoute(
                   name: "DemandEmployProductCategory",
                   pattern: "{url}.pcat",
                   constraints: new { url = @"^can-thue-dich-vu-([_0-9a-z-]+)$" },
                   defaults: new { controller = "Demand", action = "DemandEmployProductCategory" }
                   );

                endpoints.MapControllerRoute(
                  name: "CurriculumVitae",
                  pattern: "ho-so",
                  defaults: new { controller = "CurriculumVitae", action = "Index" }
                  );

                endpoints.MapControllerRoute(
                  name: "CurriculumVitaeByCate",
                  pattern: "ho-so/{url}",
                  defaults: new { controller = "CurriculumVitae", action = "CurriculumVitaeByCate" }
                  );

                endpoints.MapControllerRoute(
                  name: "CurriculumVitaeDetail",
                  pattern: "{url}",
                  constraints: new { url = @"^ho-so-([_0-9a-z-]+)$" },
                  defaults: new { controller = "CurriculumVitae", action = "CurriculumVitaeDetail" }
                  );

                //Việc làm
                endpoints.MapControllerRoute(
                  name: "Recruitment",
                  pattern: "viec-lam",
                  defaults: new { controller = "Recruitment", action = "Index" }
                  );

                //Việc làm -  danh muc
                endpoints.MapControllerRoute(
                  name: "Recruitment",
                  pattern: "danh-muc-viec-lam/{url}",
                  defaults: new { controller = "Recruitment", action = "RecruitmentByCate" }
                  );

                //Việc làm - chi tiết
                endpoints.MapControllerRoute(
                  name: "Recruitment",
                  pattern: "viec-lam/{url}",
                  defaults: new { controller = "Recruitment", action = "RecruitmentDetail" }
                  );

                

                //ban vật tư - chi tiet
                endpoints.MapControllerRoute(
                   name: "SellingMaterialDetails",
                   pattern: "{url}.pro",
                   constraints: new { url = @"^ban-vat-tu-([_0-9a-z-]+)$" },
                   defaults: new { controller = "Product", action = "SellingMaterialDetails" }
                   );


                //May de ban - chi tiet
                endpoints.MapControllerRoute(
                   name: "SellingProductDetails",
                   pattern: "{url}.pro",
                   constraints: new { url = @"^ban-([_0-9a-z-]+)$" },
                   defaults: new { controller = "Product", action = "SellingProductDetails" }
                   );

                //tai-khoan
                endpoints.MapControllerRoute(
                   name: "Profile",
                   pattern: "tai-khoan",
                   defaults: new { controller = "Profile", action = "Index" }
                   );

                //gian hàng
                endpoints.MapControllerRoute(
                   name: "ProductBrandDetail",
                   pattern: "{url}.brand",
                   defaults: new { controller = "ProductBrand", action = "Index" }
                   );

                endpoints.MapControllerRoute(
                   name: "ProductBrand Service",
                   pattern: "/dich-vu/{url}.brand",
                   defaults: new { controller = "ProductBrand", action = "Service" }
                   );

                endpoints.MapControllerRoute(
                  name: "ProductBrand Lib",
                  pattern: "/thu-vien/{url}.brand",
                  defaults: new { controller = "ProductBrand", action = "Library" }
                  );

                endpoints.MapControllerRoute(
                  name: "ProductBrand About",
                  pattern: "/gioi-thieu/{url}.brand",
                  defaults: new { controller = "ProductBrand", action = "About" }
                  );

                endpoints.MapControllerRoute(
                  name: "ProductBrand About",
                  pattern: "tao-gian-hang",
                  defaults: new { controller = "Profile", action = "CreateProductBrand" }
                  );

                endpoints.MapControllerRoute(
                   name: "SearchProduct",
                   pattern: "{url}.ps",
                   defaults: new { controller = "Search", action = "IndexPs" }
                   );
            });
        }
    }
}
