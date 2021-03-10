using HNM.DataNC.Models;
using HNM.DataNC.ModelsFilter;
using HNM.DataNC.ModelsNC.ModelsDTO;
using HNM.DataNC.ModelsStore;
using HNM.RepositoriesNC.RepositoriesBase;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace HNM.RepositoriesNC.Repositories
{
    public interface IBrandRepository : IRepositoryBase<ProductBrand>
    {
        Task<List<ProductBrand>> GetTopBrand(int topX);
        Task<Tuple<int, IEnumerable<ProductBrand_Search_Result>>> GetListBrandPagging(int page, int pageSize, int? LocationId, int active, int? productCategoryId, string keyword);
        Task<SentContactBrand> SentContactBrand(int BrandId, string Name, string Phone, string Email, string Content);
        Task<IEnumerable<ProductCategory>> GetProductCateByBrand(int BrandId);
        Task<IEnumerable<ProductBrand_Search_Brand_V2_Result>> GetListProductBrand(ProductBrandFilter filter);
        Task<int> PostProductBrand(ProductBrand model, ImageUploadDTO imgLogo, ImageUploadDTO imgBanner, string UserId);
        int UpdateImgProductBrand(ProductBrand model, ImageUploadDTO imgLogo, ImageUploadDTO imgBanner, int ProductBrandId, string UserId);
        Task UpdateImgLogoProductBrand(string fileName, int ProductBrandId);
        Task UpdateImgBannerProductBrand(string fileName, int ProductBrandId);
        Task UpgradePackageBrand(int ProductBrandId, int ProductBrandTypeId);
        Task<List<ProductBrand>> GetAllBrand();
        Task<bool> UpdateBrandReferralCode(int ProductBrandId, string ReferralCode);
    }
    public class BrandRepository : RepositoryBase<ProductBrand>, IBrandRepository
    {
        public BrandRepository(HanomaContext HanomaContext) : base(HanomaContext)
        {
        }

        public async Task<List<ProductBrand>> GetTopBrand(int topX)
        {
            var BlockName = new SqlParameter("@BlockName", "enterprise_reputation");

            var output = await HanomaContext.Set<ProductBrand>()
                .FromSqlRaw($" SELECT TOP {topX} P.* FROM dbo.ProductBrand AS P WITH (NOLOCK)" +
                $" INNER JOIN dbo.ProductBrandBlockProductBrand AS BBP WITH(NOLOCK) ON BBP.ProductBrand_id = P.ProductBrand_ID" +
                $" INNER JOIN dbo.ProductBrandBlock AS BB WITH(NOLOCK) ON BBP.ProductBrandBlock_id = BB.ProductBrandBlock_id" +
                $" WHERE BB.Name = @BlockName", BlockName)
                .AsNoTracking()
                .ToListAsync();
            return output;
        }
        public async Task<Tuple<int, IEnumerable<ProductBrand_Search_Result>>> GetListBrandPagging(int page, int pageSize, int? LocationId, int active, int? productCategoryId, string keyword)
        {
            var Keyword = new SqlParameter("@Keyword", String.IsNullOrEmpty(keyword) ? (object)DBNull.Value : keyword);
            var Active = new SqlParameter("@Active", active);
            var PageSize = new SqlParameter("@PageSize", pageSize);
            var CurrentPage = new SqlParameter("@CurrentPage", page);
            var ProductCategory_ID = new SqlParameter("@ProductCategory_ID", productCategoryId ?? (object)DBNull.Value);
            var Location_Id =  new SqlParameter("@Location_ID", LocationId ?? (object)DBNull.Value);
            var ItemCount = new SqlParameter("@ItemCount", "")
            {
                Direction = System.Data.ParameterDirection.Output,
                DbType = System.Data.DbType.Int32
            };

            var output = await HanomaContext.Set<ProductBrand_Search_Result>()
                .FromSqlRaw($"EXECUTE dbo.ProductBrand_Search " +
                 $"@Keyword = @Keyword, " +
                $"@Active = @Active, @PageSize = @PageSize,@Location_ID = @Location_ID, @ProductCategory_ID = @ProductCategory_ID, @CurrentPage = @CurrentPage, @ItemCount =  @ItemCount OUTPUT", Keyword, Active, PageSize, Location_Id, ProductCategory_ID, CurrentPage, ItemCount)
                .AsNoTracking()
                .ToListAsync();
            var ItemCountRes = Convert.ToInt32(ItemCount.Value);
            return new Tuple<int, IEnumerable<ProductBrand_Search_Result>>(ItemCountRes, output);
        }



        public async Task<SentContactBrand> SentContactBrand(int BrandId, string Name, string Phone, string Email, string Content)
        {
            var result = new SentContactBrand();
            var brand = HanomaContext.ProductBrand.FirstOrDefault(p => p.ProductBrand_ID == BrandId);
            string MailBody = "";
            WebClient client = new WebClient();
            Stream stream = client.OpenRead("https://daninhbinhvinhquang.vn/Content/Emailtemplate/contact-to-productbrand.html");
            using (StreamReader reader =
                new StreamReader(stream))
            {
                MailBody = reader.ReadToEnd();
            }

            MailBody = MailBody.Replace("{BrandName}", brand.Name);
            MailBody = MailBody.Replace("{BrandURL}", brand.URL);
            MailBody = MailBody.Replace("{CustName}", Name);
            MailBody = MailBody.Replace("{CustPhone}", Phone);
            MailBody = MailBody.Replace("{CustEmail}", Email);
            MailBody = MailBody.Replace("{CustContent}", Content);

            result.MailBody = MailBody;
            result.ContactEmail = brand.Email;
            result.ContactName = brand.Name;

            return result;
        }

        public async Task<IEnumerable<ProductCategory>> GetProductCateByBrand(int BrandId)
        {
            var result = HanomaContext.Product.Where(p => p.ProductBrand_ID == BrandId)
               .Select(x => x.ProductCategory_ID)
               .Distinct()
               .ToList();
            return HanomaContext.ProductCategory.Where(p => result.Contains(p.ProductCategory_ID)).ToList();
        }

        public async Task<IEnumerable<ProductBrand_Search_Brand_V2_Result>> GetListProductBrand(ProductBrandFilter filter)
        {
            var Keyword = new SqlParameter("@Keyword", filter.Keyword);
            var StatusType_ID = new SqlParameter("@StatusType_ID", filter.StatusTypeId);
            var PageSize = new SqlParameter("@PageSize", filter.PageSize);
            var CurrentPage = new SqlParameter("@CurrentPage", filter.Page);
            var ProductType_ID = new SqlParameter("@ProductType_ID", filter.ProductTypeID);
            var ProductCategory_ID = new SqlParameter("@ProductCategory_ID", filter.ProductCategoryID);
            var AccesoryCategory_ID = new SqlParameter("@AccesoryCategory_ID", filter.AccesoryCategoryID);
            var Location_ID = new SqlParameter("@Location_ID", filter.LocationID);

            var result = await HanomaContext.Set<ProductBrand_Search_Brand_V2_Result>()
                .FromSqlRaw($"EXECUTE dbo.ProductBrand_Search_Brand_V2 "
                + $"@Keyword = @Keyword, "
                + $"@ProductType_ID = @ProductType_ID, "
                + $"@ProductCategory_ID = @ProductCategory_ID, "
                + $"@AccesoryCategory_ID = @AccesoryCategory_ID, "
                + $"@Location_ID = @Location_ID, "
                + $"@StatusType_ID = @StatusType_ID, "
                     + $"@PageSize = @PageSize, "
                     + $"@CurrentPage = @CurrentPage ",
                     Keyword, ProductType_ID, ProductCategory_ID, AccesoryCategory_ID, Location_ID, StatusType_ID,
                     PageSize, CurrentPage
                     )
                .AsNoTracking()
                .ToListAsync();

            return result;



        }

        public async Task<int> PostProductBrand(ProductBrand model, ImageUploadDTO imgLogo, ImageUploadDTO imgBanner, string UserId)
        {

            var pUserId = Guid.Parse(UserId);
            if (model != null)
            {
                var prodBrandSave = HanomaContext.ProductBrand.Find(model.ProductBrand_ID);
                if (prodBrandSave == null)
                {
                    var productBrandNew = new ProductBrand
                    {
                        Location_ID = model.Location_ID,
                        Name = model.Name,
                        TradingName = model.TradingName,
                        BrandName = model.BrandName,
                        BusinessArea = model.BusinessArea,
                        Address = model.Address,
                        Telephone = model.Telephone,
                        Mobile = model.Mobile,
                        Website = model.Website,
                        Description = model.Description,
                        Email = model.Email,
                        LastEditDate = DateTime.Now,
                        CreateDate = DateTime.Now,
                        Verified = true,
                        Active = true,
                        URL = null,
                        CreateBy = new Guid(UserId),
                        LastEditBy = new Guid(UserId),
                        District = model.District,
                        PostalCode = model.PostalCode,
                        Country_ID = model.Country_ID,
                        EmailDisplay = model.EmailDisplay,
                        MapCode = model.MapCode,
                        ReferralCode = model.ReferralCode,
                        ProductBrandType_ID = 1 // Gian hàng quy mô nhỏ

                    };
                    try
                    {
                        HanomaContext.ProductBrand.Add(productBrandNew);
                        await HanomaContext.SaveChangesAsync();

                        UpdateUrlProductBrand(productBrandNew.ProductBrand_ID);
                        UpdateProductBrandIdOfProduct(UserId, productBrandNew.ProductBrand_ID);
                        UpgradeAccountAfterRegister(UserId);
                        UpdateUserProfiler(UserId, productBrandNew.ProductBrand_ID);
                        return productBrandNew.ProductBrand_ID;
                    }
                    catch (Exception ex)
                    {
                        productBrandNew.ProductBrand_ID = 0;
                        return productBrandNew.ProductBrand_ID;
                    }
                }
                else
                {
                    try
                    {
                        var urlNew = FormatURL(model.Name) + "-" + model.ProductBrand_ID.ToString();

                        prodBrandSave.Location_ID = model.Location_ID;
                        prodBrandSave.Name = model.Name;
                        prodBrandSave.TradingName = model.TradingName;
                        prodBrandSave.BrandName = model.BrandName;
                        prodBrandSave.BusinessArea = model.BusinessArea;
                        prodBrandSave.Address = model.Address;
                        prodBrandSave.Telephone = model.Telephone;
                        prodBrandSave.Mobile = model.Mobile;
                        prodBrandSave.Website = model.Website;
                        prodBrandSave.Description = model.Description;
                        prodBrandSave.Email = model.Email;
                        prodBrandSave.LastEditDate = DateTime.Now;
                        prodBrandSave.Verified = true;
                        prodBrandSave.Active = true;
                        prodBrandSave.URL = urlNew;
                        prodBrandSave.LastEditBy = pUserId;
                        prodBrandSave.District = model.District;
                        prodBrandSave.PostalCode = model.PostalCode;
                        prodBrandSave.Country_ID = model.Country_ID;
                        prodBrandSave.EmailDisplay = model.EmailDisplay;
                        prodBrandSave.MapCode = model.MapCode;
                        prodBrandSave.ReferralCode = model.ReferralCode;
                        await HanomaContext.SaveChangesAsync();

                        UpdateUserProfiler(UserId, model.ProductBrand_ID);

                        return model.ProductBrand_ID;
                    }
                    catch (Exception ex)
                    {
                        return 0;
                    }
                }

            }
            return model.ProductBrand_ID;

        }
        public void UpdateUrlProductBrand(int productBrandId)
        {
            try
            {
                var productBrand = HanomaContext.ProductBrand.FirstOrDefault(x => x.ProductBrand_ID == productBrandId);
                var urlNew = FormatURL(productBrand.Name) + "-" + productBrand.ProductBrand_ID.ToString();
                productBrand.URL = urlNew;
                HanomaContext.SaveChanges();
            }
            catch (Exception exception)
            {
                //log.Error(exception);

            }

        }
        public void UpdateProductBrandIdOfProduct(string userId, int productBrandId)
        {
            try
            {

                var userIdGuid = new Guid(userId);
                var products = HanomaContext.Product.Where(x => x.CreateBy == userIdGuid).ToList();
                if (products == null || products.Count < 1) return;
                foreach (var product in products)
                {
                    product.ProductBrand_ID = productBrandId;
                    HanomaContext.SaveChanges();
                }


            }
            catch (Exception ex)
            {
                //   log.Error(ex);
            }
        }
        public void UpgradeAccountAfterRegister(string userId)
        {
            try
            {

                var profilers = HanomaContext.AspNetUserProfiles.FirstOrDefault(x => x.UserId.Equals(userId));
                var productBrand = HanomaContext.ProductBrand.FirstOrDefault(x => x.ProductBrand_ID == profilers.ProductBrand_ID);
                profilers.AccountType = 2;

                HanomaContext.AspNetUserRoles.Add(new AspNetUserRoles
                {
                    UserId = userId,
                    RoleId = HanomaContext.AspNetRoles.FirstOrDefault(x => x.Name.Equals("Tài khoản Doanh nghiệp")).Id
                });
                HanomaContext.SaveChanges();
            }
            catch (Exception exception)
            {
                //log.Error(exception);

            }
        }
        public void UpdateUserProfiler(string userId, int productBrandId)
        {
            try
            {
                var profiler = HanomaContext.AspNetUserProfiles.FirstOrDefault(x => x.UserId.Equals(userId));
                profiler.ProductBrand_ID = productBrandId;
                HanomaContext.SaveChanges();
            }
            catch (Exception exception)
            {
                //   log.Error(exception);

            }
        }

        public int UpdateImgProductBrand(ProductBrand model, ImageUploadDTO imgLogo, ImageUploadDTO imgBanner, int ProductBrandId, string UserId)
        {
            var pUserId = Guid.Parse(UserId);
            if (model != null)
            {
                var prodBrandSave = HanomaContext.ProductBrand.Find(ProductBrandId);
                if (prodBrandSave != null)
                {
                    try
                    {
                        if (imgLogo.Base64 != null)
                        {
                            prodBrandSave.Logo = String.Format("{0}-mobile-logo.{1}", ProductBrandId, imgLogo.ExtensionType.Replace("image/", ""));
                        }

                        if (imgBanner.Base64 != null)
                        {
                            prodBrandSave.Banner = String.Format("{0}-mobile-banner.{1}", ProductBrandId, imgBanner.ExtensionType.Replace("image/", ""));
                        }

                        HanomaContext.SaveChanges();
                        return model.ProductBrand_ID;
                    }
                    catch (Exception ex)
                    {
                        return 0;
                    }
                }
            }
            return ProductBrandId;
        }

        public async Task UpdateImgLogoProductBrand(string fileName, int ProductBrandId)
        {
            var prodBrand = HanomaContext.ProductBrand.Find(ProductBrandId);
            if(prodBrand !=null)
            {
                prodBrand.Logo = fileName;
                HanomaContext.SaveChanges();
            }
        }

        public async Task UpdateImgBannerProductBrand(string fileName, int ProductBrandId)
        {
            var prodBrand = HanomaContext.ProductBrand.Find(ProductBrandId);
            if (prodBrand != null)
            {
                prodBrand.Banner = fileName;
                HanomaContext.SaveChanges();
            }
        }

        public async Task UpgradePackageBrand(int ProductBrandId, int ProductBrandTypeId)
        {
            var prodBrand = HanomaContext.ProductBrand.Find(ProductBrandId);
            if(prodBrand != null)
            {
                prodBrand.ProductBrandType_ID = ProductBrandTypeId;
                HanomaContext.SaveChanges();
            }    
        }

        public async Task<List<ProductBrand>> GetAllBrand()
        {
            return await HanomaContext.ProductBrand.ToListAsync();
        }

        public async Task<bool> UpdateBrandReferralCode(int ProductBrandId, string ReferralCode)
        {
            var prodBrand = HanomaContext.ProductBrand.Find(ProductBrandId);
            if (prodBrand != null)
            {
                prodBrand.ReferralCode = ReferralCode;
                prodBrand.ReferralCodeDate = DateTime.Now;
                HanomaContext.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
