using HNM.DataNC.Models;
using HNM.DataNC.ModelsFilter;
using HNM.DataNC.ModelsNC.ModelsDTO;
using HNM.DataNC.ModelsNC.ModelsUtility;
using HNM.DataNC.ModelsStore;
using HNM.RepositoriesNC.RepositoriesBase;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace HNM.RepositoriesNC.Repositories
{
    public interface IProductRepository : IRepositoryBase<Product>
    {
        Task<IEnumerable<Product>> GetMostViewResult();
        Task<IEnumerable<HomeProductSlide_Result>> GetHomeProductSlide(string lstAdvertistingID);
        Task<IEnumerable<Product_Search_Result>> GetTopProductMarket(int type);
        Task<IEnumerable<Product_PreProcess_ByCateId_Result>> GetTopProductByCategoryId(int productCategoryID);
        Task<IEnumerable<Product_Search_Result>> GetListProductPagging(ProductFilter filter);
        Task<IEnumerable<ProductCategory>> GetMainSystem();
        Task<IEnumerable<PatchNumber_SearchByCategory_Result>> GetListPartNumber(int? ProductCategoryId, int? ProductTypeId);
        Task<IEnumerable<Accesories_Search_Result>> GetListAccesories(ProductFilter filter);
        Task<IEnumerable<Materials_Search_Result>> GetListMaterials(ProductFilter filter);
        Task<IEnumerable<Service_Search_Result>> GetListServices(ProductFilter filter);
        Task<IEnumerable<Product_Search_Result>> GetLstProductShopMan(ProductShopManFilter filter);
        Task<ProductIdDTO> ActionProductShopman(ProductIdDTO model, string Action);
        Task<ProductIdDTO> CheckRenewProductByBrandId(ProductIdDTO model);
        int PostProductShopman(Product model, ImageUploadDTO MainImage, List<ImageUploadDTO> SubImage, string UserId);   
        Task UpdateMainImageProduct(string FileName, int ProductId);
        Task UpdateIllustrationImages(string FileName, int ProductId);
        string CreateImageURL(int Product_ID);
        bool CanEditProduct(int Product_ID, string userId);
        Task<int> ProcessPhoneNumber();
        Task DeleteIllustrationImages(List<DeleteImageProductPicture> model);
        //Task<IEnumerable<AccessoriesFit>> GetLstAccessoriesFit(int Product_ID);
        Task<int> CountProductByBrand(int ProductBrandId);
        Task PushAWSImageToRabbit(ImageUploadAWSDTO model,string queue);
        Task<List<PatchNumber_SearchByCategory_Result>> GetPatchNumberByCategory(int? ProductCategory_Id, int? ProductType_ID, string text);
        Task<IEnumerable<MetaKeyword>> GetListMetaKeyword(int ProductId);
        Task<List<Product>> GetListDemandByProductBrand(int productBrand_ID);
        Task<List<TimeLinePost_Result>> GetLstTimeLinePost(string userId, int? Page = 1, int? PageSize = 10);
        Task<IEnumerable<Product_Search_Result>> GetLstProductForSellShopMan(ProductShopManFilter filter);
    }
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(HanomaContext HanomaContext) : base(HanomaContext)
        {
        }

        public int PostProductShopman(Product model, ImageUploadDTO MainImage, List<ImageUploadDTO> SubImage, string UserId)
        {
            try
            {
                var pUserId = Guid.Parse(UserId);
                var userInfo = HanomaContext.AspNetUserProfiles.FirstOrDefault(p => p.UserId == UserId);
                var productBrand = HanomaContext.ProductBrand.FirstOrDefault(p => p.CreateBy.ToString() == UserId);
                var metaDescription = String.IsNullOrEmpty(model.MetaDescription) ? CutText(HttpUtility.HtmlDecode(model.Content), 160) : model.MetaDescription;
                if (model != null)
                {
                    var productSave = HanomaContext.Product.Find(model.Product_ID);
                    if (productSave == null)
                    {
                        //Add Product
                        model.Store_ID = 0;
                        if (model.Country_ID == 0 || model.Country_ID == null)
                        {
                            model.Country_ID = model.MadeCountryId ?? 0;
                        }
                        model.ProductBrand_ID = productBrand == null ? 0 : productBrand.ProductBrand_ID;
                        if (productBrand == null)
                        {
                            model.SaleName = userInfo?.FullName;
                            model.SaleContactName = userInfo?.FullName;
                            model.SalePhone = userInfo?.Phone;
                            model.SaleAddress = userInfo?.Address;
                            model.SaleEmail = userInfo?.Email;
                        }
                        else
                        {
                            model.SaleName = productBrand?.Name;
                            model.SaleContactName = productBrand?.Name;
                            model.SalePhone = productBrand?.Mobile;
                            model.SaleAddress = productBrand?.Address;
                            model.SaleEmail = productBrand?.Email;
                        }

                        model.AllowComment = false;
                        model.YearManufacture = model.YearManufacture ?? -1;
                        model.StartDate = DateTime.Now;
                        model.EndDate = DateTime.Now.AddYears(10);
                        model.LastEditDate = DateTime.Now;
                        model.CreateDate = DateTime.Now;
                        model.LastEditBy = pUserId;
                        model.CreateBy = pUserId;
                        model.Active = false;
                        model.Counter = 0;
                        model.ViewCount = 0;
                        model.StatusType_ID = 4;
                        if (model.Country_ID == 0 || model.Country_ID == null)
                        {
                            model.Country_ID = model.MadeCountryId ?? 0;
                        }
                        HanomaContext.Product.Add(model);
                        HanomaContext.SaveChanges();
                        

                        //CreateProduct URL
                        CreateProductURL(model.Product_ID);
                        //Update SKU
                        UpdateSkuProduct(model.Product_ID);

                        // Write to sitemap
                        //AppendProductXml(productNew.Product_ID, Util.FullDomain);
                        //AppendProductImageXml(productNew.Product_ID, Util.FullDomain);
                        InsertMetaKeywordIds(model.Product_ID);
                        //Save Image
                    }
                    else
                    {

                        var prodName = model.Name;
                        productSave.Name = model.ProductType_ID == 1 ? prodName : model.Name;//model.Name;
                        productSave.Description = model.Description;
                        productSave.Content = model.Content;
                        productSave.MetaDescription = metaDescription;
                        productSave.Tag = model.Tag;
                        productSave.HoursOfWork = model.HoursOfWork;
                        productSave.SaleName = productBrand?.Name;
                        productSave.SaleContactName = productBrand?.Name;
                        productSave.SalePhone = productBrand?.Mobile;
                        productSave.SaleAddress = productBrand?.Address;
                        productSave.SaleEmail = productBrand?.Email;
                        productSave.YearManufacture = model.YearManufacture ?? -1;
                        productSave.StatusMachine = model.StatusMachine;
                        productSave.LastEditDate = DateTime.Now;
                        productSave.LastEditBy = pUserId;
                        productSave.Active = false;
                        productSave.ProductManufacture_ID = model.ProductManufacture_ID;
                        productSave.ProductCategory_ID = model.ProductCategory_ID;
                        productSave.ProductModel_ID = model.ProductModel_ID;
                        productSave.SerialNumber = model.SerialNumber;
                        productSave.SaleLocation_ID = 37; //Default
                        productSave.Price = model.Price;
                        productSave.StatusType_ID = 4;
                        productSave.HasNewModel = model.HasNewModel;
                        productSave.ReferralCode = model.ReferralCode;
                        productSave.MadeCountryId = model.MadeCountryId;
                        productSave.HoursOfWork = model.HoursOfWork;
                        productSave.HourOfWorkType = model.HourOfWorkType;
                        productSave.Label = model.Label;
                        productSave.PartNumber = model.PartNumber;
                        productSave.RelatedCategoryId = model.RelatedCategoryId;
                        productSave.AccessoriesCategoryId = model.AccessoriesCategoryId;
                        productSave.AccessoriesModelName = model.AccessoriesModelName;
                        productSave.AccessoriesManufactureName = model.AccessoriesManufactureName;
                        productSave.SellCount = model.SellCount;
                        productSave.Unit = model.Unit;
                        if (model.Country_ID == 0 || model.Country_ID == null)
                        {
                            productSave.Country_ID = model.MadeCountryId ?? 0;
                        }

                        HanomaContext.SaveChanges();
                        //Update Meta Keyword
                        if (model.ProductCategory_ID == productSave.ProductCategory_ID)
                        {
                            UpdateMetaKeyWords(model.Product_ID);
                        }
                
                    }
                }
                //Add Tags
                //ProcessTagsProduct(model.Product_ID, model.Tag);


            }
            catch (Exception ex)
            {
                //
                model.Product_ID = 0;
                throw ex;
            }
            return model.Product_ID;
        }
        public async Task<IEnumerable<Product>> GetMostViewResult()
        {
            return await HanomaContext.Product
                .FromSqlRaw($"SELECT TOP 3 B.* " +
                $"FROM ProductBlockProduct A " +
                $"JOIN Product B ON A.Product_ID = B.Product_ID WHERE A.ProductBlock_ID = 1 ORDER BY B.LastEditDate")
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<IEnumerable<HomeProductSlide_Result>> GetHomeProductSlide(string lstAdvertistingID)
        {
            var AdvertisingBlock_ID = new SqlParameter("AdvertisingBlock_ID", lstAdvertistingID);
            return await HanomaContext.Set<HomeProductSlide_Result>()
                .FromSqlRaw($"EXECUTE HomeProductSlide @AdvertisingBlock_ID", AdvertisingBlock_ID)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<IEnumerable<Product_Search_Result>> GetTopProductMarket(int productTypeId)
        {
            var ProductType_ID = new SqlParameter("@ProductType_ID", productTypeId);
            var StatusType_ID = new SqlParameter("@StatusType_ID", 4);
            var PageSize = new SqlParameter("@PageSize", 5);
            var CurrentPage = new SqlParameter("@CurrentPage", 1);

            var output = await HanomaContext.Set<Product_Search_Result>()
                .FromSqlRaw($"EXECUTE dbo.Product_Search @ProductType_ID = @ProductType_ID, @StatusType_ID = @StatusType_ID, @PageSize = @PageSize, @CurrentPage = @CurrentPage", ProductType_ID, StatusType_ID, PageSize, CurrentPage)
                .AsNoTracking()
                .ToListAsync();
            return output;
        }
        public async Task<IEnumerable<Product_PreProcess_ByCateId_Result>> GetTopProductByCategoryId(int productCategoryID)
        {
            var ProductCategory_ID = new SqlParameter("@ProductCategory_ID", productCategoryID);
            var StatusType_ID = new SqlParameter("@StatusType_ID", 4);
            var PageSize = new SqlParameter("@PageSize", 12);
            var CurrentPage = new SqlParameter("@CurrentPage", 1);

            var output = await HanomaContext.Set<Product_PreProcess_ByCateId_Result>()
                .FromSqlRaw($"EXECUTE dbo.Product_PreProcess_ByCateId @ProductCategory_ID = @ProductCategory_ID, @StatusType_ID = @StatusType_ID, @PageSize = @PageSize, @CurrentPage = @CurrentPage", ProductCategory_ID, StatusType_ID, PageSize, CurrentPage)
                .AsNoTracking()
                .ToListAsync();
            return output;
        }
        public async Task<IEnumerable<Product_Search_Result>> GetListProductPagging(ProductFilter filter)
        {
            var Keyword = new SqlParameter("@Keyword", filter.Keyword ?? (object)DBNull.Value);
            var ProductType_ID = new SqlParameter("@ProductType_ID", filter.ProductTypeId ?? (object)DBNull.Value);
            var ProductCategory_ID = new SqlParameter("@ProductCategory_ID", filter.ProductCategoryId ?? (object)DBNull.Value);
            var Location_ID = new SqlParameter("@SaleLocation_ID", filter.LocationId ?? (object)DBNull.Value);
            var ProductModel_ID = new SqlParameter("@ProductModel_ID", filter.ProductModelId ?? (object)DBNull.Value);
            var ProductManufacture_ID = new SqlParameter("@ProductManufacture_ID", filter.ProductManufactureId ?? (object)DBNull.Value);
            var ProductBrand_ID = new SqlParameter("@ProductBrand_ID", filter.ProductBrandId ?? (object)DBNull.Value);
            var StatusType_ID = new SqlParameter("@StatusType_ID", filter.StatusTypeId);
            var PageSize = new SqlParameter("@PageSize", filter.PageSize);
            var CurrentPage = new SqlParameter("@CurrentPage", filter.Page);
            var StatusMachine = new SqlParameter("@StatusMachine", filter.StatusMachine ?? (object)DBNull.Value);

            var output = await HanomaContext.Set<Product_Search_Result>()
                .FromSqlRaw($"EXECUTE dbo.Product_Search " +
                $"@Keyword = @Keyword, " +
                $"@ProductType_ID = @ProductType_ID, " +
                $"@ProductCategory_ID = @ProductCategory_ID, " +
                $"@SaleLocation_ID = @SaleLocation_ID, " +
                $"@ProductModel_ID = @ProductModel_ID, " +
                $"@ProductManufacture_ID = @ProductManufacture_ID, " +
                $"@ProductBrand_ID = @ProductBrand_ID, " +
                $"@StatusType_ID = @StatusType_ID, " +
                $"@PageSize = @PageSize, " +
                $"@CurrentPage = @CurrentPage, " +
                $"@StatusMachine = @StatusMachine " 
                ,
                Keyword, ProductType_ID, ProductCategory_ID, Location_ID, ProductModel_ID, ProductManufacture_ID, ProductBrand_ID, StatusType_ID, PageSize, CurrentPage,StatusMachine)
                .AsNoTracking()
                .ToListAsync();
            return output;
        }

        public async Task<IEnumerable<PatchNumber_SearchByCategory_Result>> GetListPartNumber(int? ProductCategoryId, int? ProductTypeId)
        {
            var ProductCategory_ID = new SqlParameter("@ProductCategory_ID", ProductCategoryId);
            var ProductType_ID = new SqlParameter("@ProductType_ID", ProductTypeId);
            var ItemCount = new SqlParameter("@ItemCount", "")
            {
                Direction = System.Data.ParameterDirection.Output,
                DbType = System.Data.DbType.Int32
            };
            try
            {
                var output = await HanomaContext.Set<PatchNumber_SearchByCategory_Result>()
                    .FromSqlRaw($"EXECUTE dbo.PatchNumber_SearchByCategory "
                    + $"@ProductCategory_ID = @ProductCategory_ID, "
                    + $"@ProductType_ID = @ProductType_ID, "
                    + $"@ItemCount = @ItemCount OUTPUT"
                    ,
                    ProductCategory_ID, ProductType_ID, ItemCount
                    )
                     .AsNoTracking()
                    .ToListAsync();

                return output;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Product>> GetListDemandByProductBrand(int productBrand_ID)
        {
            var result = await HanomaContext.Product.Where(p => p.ProductBrand_ID == productBrand_ID && (p.ProductType_ID == 2 || p.ProductType_ID == 4 || p.ProductType_ID == 6 || p.ProductType_ID == 8))
                .OrderByDescending(x => x.LastEditDate)
                .ToListAsync();
            return result;
        }

        public async Task<IEnumerable<ProductCategory>> GetMainSystem()
        {
            var result = await HanomaContext.ProductCategory.Where(p => p.ProductCategory_ID == 653)
                .OrderBy(x => x.Sort)
                .ToListAsync();
            return result;
        }
        /// <summary>
        /// Get List Accessories
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Accesories_Search_Result>> GetListAccesories(ProductFilter filter)
        {
            var Keyword = new SqlParameter("@Keyword", filter.Keyword ?? (object)DBNull.Value);
            var ProductBrand_ID = new SqlParameter("@ProductBrand_ID", filter.ProductBrandId ?? (object)DBNull.Value);
            var ProductCategory_ID = new SqlParameter("@ProductCategory_ID", filter.ProductCategoryId ?? (object)DBNull.Value);
            var ProductManufactory_ID = new SqlParameter("@ProductManufacture_ID", filter.ProductManufactureId ?? (object)DBNull.Value);
            var ProductModel_ID = new SqlParameter("@ProductModel_ID", filter.ProductModelId ?? (object)DBNull.Value);
            var ProductType_ID = new SqlParameter("@ProductType_ID", filter.ProductTypeId ?? (object)DBNull.Value);
            var MainSystem_ID = new SqlParameter("@MainSystem_ID", filter.MainSystemId ?? (object)DBNull.Value);
            var AccDetail_ID = new SqlParameter("@AccDetail_ID", filter.AccDetailId ?? (object)DBNull.Value);
            var PatchNum = new SqlParameter("@PatchNum", filter.PatchNum ?? (object)DBNull.Value);
            var StatusType_ID = new SqlParameter("@StatusType_ID", filter.StatusTypeId);
            var PageSize = new SqlParameter("@PageSize", filter.PageSize);
            var CurrentPage = new SqlParameter("@CurrentPage", filter.Page);
            var LocationID = new SqlParameter("@Location_ID", filter.LocationId ?? (object)DBNull.Value);
            var CreateBy = new SqlParameter("@CreateBy", filter.UserId ?? (object)DBNull.Value);
            var StatusMachine = new SqlParameter("@StatusMachine", filter.StatusMachine ?? (object)DBNull.Value);
            try
            {
                var output = await HanomaContext.Set<Accesories_Search_Result>()
                        .FromSqlRaw($"EXECUTE dbo.Accesories_Search "
                         + $"@Keyword = @Keyword, "
                         + $"@ProductBrand_ID = @ProductBrand_ID, "
                         + $"@ProductCategory_ID = @ProductCategory_ID, "
                         + $"@ProductManufacture_ID = @ProductManufacture_ID, "
                         + $"@ProductModel_ID = @ProductModel_ID, "
                         + $"@ProductType_ID = @ProductType_ID, "
                         + $"@MainSystem_ID = @MainSystem_ID, "
                         + $"@AccDetail_ID = @AccDetail_ID, "
                         + $"@PatchNum = @PatchNum, "
                         + $"@Location_ID = @Location_ID, "
                         + $"@StatusType_ID = @StatusType_ID, "
                         + $"@PageSize = @PageSize, "
                         + $"@CurrentPage = @CurrentPage, "
                         + $"@CreateBy = @CreateBy, "
                         + $"@StatusMachine = @StatusMachine "
                         ,
                         Keyword, ProductBrand_ID, ProductCategory_ID, ProductManufactory_ID, ProductModel_ID, ProductType_ID,
                         MainSystem_ID, AccDetail_ID, PatchNum, LocationID, StatusType_ID, PageSize, CurrentPage,CreateBy,StatusMachine
                         )
                        .AsNoTracking()
                        .ToListAsync();
                return output;
            }
            catch (Exception ex)
            {
                return new List<Accesories_Search_Result>();
            }


        }
        /// <summary>
        /// Get List Material
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Materials_Search_Result>> GetListMaterials(ProductFilter filter)
        {
            var Keyword = new SqlParameter("@Keyword", filter.Keyword ?? (object)DBNull.Value);
            var ProductBrand_ID = new SqlParameter("@ProductBrand_ID", filter.ProductBrandId ?? (object)DBNull.Value);
            var StatusType_ID = new SqlParameter("@StatusType_ID", filter.StatusTypeId);
            var PageSize = new SqlParameter("@PageSize", filter.PageSize);
            var CurrentPage = new SqlParameter("@CurrentPage", filter.Page);
            var LocationID = new SqlParameter("@Location_ID", filter.LocationId ?? (object)DBNull.Value);
            var ProductTypeID = new SqlParameter("@ProductType_ID", filter.ProductTypeId ?? (object)DBNull.Value);
            var ProductCategory_ID = new SqlParameter("@ProductCategory_ID", filter.ProductCategoryId ?? (object)DBNull.Value);
            var MaterialManufactory = new SqlParameter("@ProductManufactory", filter.MaterialManufactory ?? (object)DBNull.Value);
            var ProductLabel = new SqlParameter("@Model", filter.ProductLabel ?? (object)DBNull.Value);
            var StatusMachine = new SqlParameter("@StatusMachine", filter.StatusMachine ?? (object)DBNull.Value);
            var PatchNum = new SqlParameter("@PatchNum", filter.PatchNum ?? (object)DBNull.Value);

            try
            {
                var output = await HanomaContext.Set<Materials_Search_Result>()
                    .FromSqlRaw($"EXECUTE dbo.Materials_Search "
                     + $"@Keyword = @Keyword, "
                     + $"@ProductBrand_ID = @ProductBrand_ID, "
                    + $"@ProductCategory_ID = @ProductCategory_ID, "
                    + $"@Location_ID = @Location_ID, "
                    + $"@ProductType_ID = @ProductType_ID, "
                    + $"@PatchNum = @PatchNum, "
                    + $"@StatusMachine = @StatusMachine, "
                    + $"@ProductManufactory = @ProductManufactory, "
                    + $"@Model = @Model, "
                     + $"@StatusType_ID = @StatusType_ID, "
                     + $"@PageSize = @PageSize, "
                     + $"@CurrentPage = @CurrentPage ",
                     Keyword, ProductBrand_ID, ProductCategory_ID, LocationID, ProductTypeID, PatchNum, StatusMachine, MaterialManufactory,
                     ProductLabel, StatusType_ID, PageSize, CurrentPage

                    )
                    .AsNoTracking()
                    .ToListAsync();

                return output;
            }
            catch (Exception ex)
            {

            }
            return new List<Materials_Search_Result>();

        }

        public async Task<int> ProcessPhoneNumber()
        {
            try
            {
                var products = await HanomaContext.Product
               .FromSqlRaw($"SELECT * FROM dbo.Product WITH (NOLOCK) WHERE  LEN(SalePhone) >= 16")
               .AsNoTracking()
               .ToListAsync();
                foreach (var item in products)
                {
                    var _urlRegex = new Regex(@"([+0-9()])+([0-9 .]{8,20})\b", RegexOptions.Compiled);
                    Match match = _urlRegex.Match(item.SalePhone);
                    if (match.Success)
                    {
                        var phoneOne = match.Value;
                        var product = HanomaContext.Product.FirstOrDefault(x => x.Product_ID == item.Product_ID);
                        product.SalePhone = phoneOne;
                        HanomaContext.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return 1;
        }

        public async Task<IEnumerable<Product_Search_Result>> GetLstProductShopMan(ProductShopManFilter filter)
        {
            var Userid = Guid.Parse(filter.UserId);
            var Keyword = new SqlParameter("@Keyword", filter.Keyword ?? (object)DBNull.Value);
            var StatusType_ID = new SqlParameter("@StatusType_ID", filter.StatusTypeId);
            var PageSize = new SqlParameter("@PageSize", filter.PageSize);
            var CurrentPage = new SqlParameter("@CurrentPage", filter.Page);
            var ProductCategory_ID = new SqlParameter("@ProductCategory_ID", filter.ProductCategoryId ?? (object)DBNull.Value);
            var ProductType_ID = new SqlParameter("@ProductType_ID", filter.ProductTypeId ?? (object)DBNull.Value);
            var CreateBy = new SqlParameter("@CreateBy", filter.UserId != null ? Userid : (object)DBNull.Value);
            try
            {
                var products = await HanomaContext.Set<Product_Search_Result>()
                    .FromSqlRaw($"EXECUTE dbo.Product_Search " +
                    $"@Keyword = @Keyword, " +
                    $"@ProductType_ID = @ProductType_ID, " +
                    $"@ProductCategory_ID = @ProductCategory_ID, " +
                    $"@CreateBy = @CreateBy, " +
                    $"@StatusType_ID = @StatusType_ID, " +
                    $"@PageSize = @PageSize, " +
                    $"@CurrentPage = @CurrentPage",
                    Keyword, ProductType_ID, ProductCategory_ID, CreateBy, StatusType_ID, PageSize, CurrentPage)
                    .AsNoTracking()
                    .ToListAsync();

                return products;

            }
            catch (Exception ex)
            {

            }
            return new List<Product_Search_Result>();
        }

        public async Task<IEnumerable<Product_Search_Result>> GetLstProductForSellShopMan(ProductShopManFilter filter)
        {
            var Userid = Guid.Parse(filter.UserId);
            var Keyword = new SqlParameter("@Keyword", filter.Keyword ?? (object)DBNull.Value);
            var StatusType_ID = new SqlParameter("@StatusType_ID", filter.StatusTypeId);
            var PageSize = new SqlParameter("@PageSize", filter.PageSize);
            var CurrentPage = new SqlParameter("@CurrentPage", filter.Page);
            var ProductCategory_ID = new SqlParameter("@ProductCategory_ID", filter.ProductCategoryId ?? (object)DBNull.Value);
            var ProductType_ID = new SqlParameter("@ProductType_ID", filter.ProductTypeId ?? (object)DBNull.Value);
            var CreateBy = new SqlParameter("@CreateBy", filter.UserId != null ? Userid : (object)DBNull.Value);
            try
            {
                var products = await HanomaContext.Set<Product_Search_Result>()
                    .FromSqlRaw($"EXECUTE dbo.ProductForSell_Search " +
                    $"@Keyword = @Keyword, " +
                    $"@ProductType_ID = @ProductType_ID, " +
                    $"@ProductCategory_ID = @ProductCategory_ID, " +
                    $"@CreateBy = @CreateBy, " +
                    $"@StatusType_ID = @StatusType_ID, " +
                    $"@PageSize = @PageSize, " +
                    $"@CurrentPage = @CurrentPage",
                    Keyword, ProductType_ID, ProductCategory_ID, CreateBy, StatusType_ID, PageSize, CurrentPage)
                    .AsNoTracking()
                    .ToListAsync();

                return products;

            }
            catch (Exception ex)
            {

            }
            return new List<Product_Search_Result>();
        }


        public async Task<ProductIdDTO> ActionProductShopman(ProductIdDTO model, string Action)
        {
            var output = new ProductIdDTO();
            var UserId = Guid.Parse(model.UserId);
            if (Action == "R") //Renew
            {
                
                
                var product = HanomaContext.Product.FirstOrDefault(p => p.Product_ID == model.ProductId && p.CreateBy == UserId);
                if (product != null)
                {
                    product.LastEditDate = DateTime.Now;
                    product.LastEditBy = Guid.Parse(model.UserId);
                    await HanomaContext.SaveChangesAsync();

                    //Update Product renew
                    model.NumberRenewInOneDay =  await UpdateRenewProduct(model);
                }
            }
            else if (Action == "H") //HUY DANG
            {
                try
                {
                    var product = HanomaContext.Product.FirstOrDefault(p => p.Product_ID == model.ProductId && p.CreateBy == UserId);
                    if (product != null)
                    {
                        product.StatusType_ID = 5;
                        product.LastEditDate = DateTime.Now;
                        product.LastEditBy = Guid.Parse(model.UserId);
                        await HanomaContext.SaveChangesAsync();
                        output.ErrorCode = "00";
                        output.Message = "Hủy đăng thành công";
                    }
                    else
                    {
                        output.ErrorCode = "001";
                        output.Message = "Có lỗi trong quá trình hủy đăng";
                    }
                }
                catch(Exception ex)
                {
                    output.ErrorCode = "001";
                    output.Message = "Có lỗi trong quá trình hủy đăng";
                }
            }
            else if (Action == "D") // Xóa 
            {
                try
                {
                    var product = HanomaContext.Product.FirstOrDefault(p => p.Product_ID == model.ProductId && p.CreateBy == UserId);
                    if (product != null)
                    {
                        product.StatusType_ID = -1;
                        product.LastEditDate = DateTime.Now;
                        product.LastEditBy = Guid.Parse(model.UserId);
                        await HanomaContext.SaveChangesAsync();
                        output.ErrorCode = "00";
                        output.Message = "Xóa thành công";
                    }
                    else
                    {
                        output.ErrorCode = "001";
                        output.Message = "Có lỗi trong quá trình hủy đăng";
                    }
                }
                catch(Exception ex)
                {
                    output.ErrorCode = "001";
                    output.Message = "Có lỗi trong quá trình hủy đăng";
                }
            }
            else if (Action == "K") // Khôi phục
            {
                try
                {
                    var product = HanomaContext.Product.FirstOrDefault(p => p.Product_ID == model.ProductId && p.CreateBy == UserId);
                    if (product != null)
                    {
                        product.StatusType_ID = 2;
                        product.LastEditDate = DateTime.Now;
                        product.LastEditBy = Guid.Parse(model.UserId);
                        await HanomaContext.SaveChangesAsync();
                        output.ErrorCode = "00";
                        output.Message = "Khôi phục thành công";
                    }
                    else
                    {
                        output.ErrorCode = "001";
                        output.Message = "Có lỗi trong quá trình hủy đăng";
                    }
                }
                catch(Exception ex)
                {
                    output.ErrorCode = "001";
                    output.Message = "Có lỗi trong quá trình hủy đăng";
                }
            }
            return output;
        }
        public async Task<int> UpdateRenewProduct(ProductIdDTO model)
        {
            int RenewCount = 1;
            var UserId = Guid.Parse(model.UserId);
            //Save ProductRenew
            var brand = await HanomaContext.AspNetUserProfiles.FirstOrDefaultAsync(p => p.UserId == model.UserId);
            var checkRenew = await HanomaContext.ProductRenew
                .FromSqlRaw($"Select * from dbo.ProductRenew where ProductBrand_ID = {brand.ProductBrand_ID} AND CONVERT(varchar, DateRenew, 112) = '{DateTime.Now.ToString("yyyyMMdd")}'")
                .FirstOrDefaultAsync();

            if (checkRenew == null)
            {
                ProductRenew prod = new ProductRenew();
                prod.ProductBrand_ID = brand.ProductBrand_ID ?? 0;
                prod.Product_Id = model.ProductId;
                prod.DateRenew = DateTime.Now;
                prod.RenewCount = 1; // 
                prod.CreateBy = UserId;
                prod.CreateDate = DateTime.Now;
                prod.LastEditBy = UserId;
                prod.LastEditDate = DateTime.Now;
                HanomaContext.ProductRenew.Add(prod);
                await HanomaContext.SaveChangesAsync();
                RenewCount = 1;
            }
            else
            {
                checkRenew.RenewCount += 1;
                checkRenew.LastEditBy = UserId;
                checkRenew.LastEditDate = DateTime.Now;
                await HanomaContext.SaveChangesAsync();
                RenewCount = checkRenew.RenewCount??1;
            }
            
            return RenewCount;
        }

        
        private void CreateProductURL(int Product_ID)
        {
            string[] P1 = new string[20];
            P1[1] = "ban";
            P1[2] = "can-mua";
            P1[3] = "cho-thue";
            P1[4] = "can-thue";
            P1[5] = "ban-phu-tung";
            P1[6] = "can-mua-phu-tung";
            P1[7] = "ban-vat-tu";
            P1[8] = "can-mua-vat-tu";
            P1[9] = "tuyen-dung";
            P1[10] = "tim-viec";
            P1[11] = "dich-vu";
            P1[12] = "can-thue-dich-vu";

            try
            {

                string P2 = "";
                var currentProduct = (from a in HanomaContext.Product
                                      where a.Product_ID == Product_ID
                                      select a).First();
                switch (currentProduct.ProductType_ID)
                {
                    case 1:
                    case 3:
                        var cateName = HanomaContext.ProductCategory.FirstOrDefault(x => x.ProductCategory_ID == currentProduct.ProductCategory_ID)?.Name;
                        var manuName = HanomaContext.ProductManufacture.FirstOrDefault(x => x.ProductManufacture_ID == currentProduct.ProductManufacture_ID)?.Name;
                        var modelName = HanomaContext.ProductModel.FirstOrDefault(x => x.ProductModel_ID == currentProduct.ProductModel_ID)?.Name;
                        P2 = String.Format("{0} {1} {2}", cateName, manuName, modelName);
                        break;
                    case 2:
                    case 4:
                    case 5:
                    case 6:
                    case 7:
                    case 11:
                        P2 = currentProduct.Name;
                        break;
                    default:
                        break;
                }

                currentProduct.URL = FormatURL(P2) + "-" + Product_ID.ToString();
                currentProduct.Alias = currentProduct.URL;
                currentProduct.FullURL = String.Format("{0}-{1}.pro", P1[(int)currentProduct.ProductType_ID], currentProduct.URL);

                HanomaContext.SaveChanges();

            }
            catch
            {

            }
        }
        private void UpdateSkuProduct(int ProductId)
        {
            string[] P1 = new string[20];
            P1[1] = "BAN"; // máy để bán
            P1[2] = "CMM"; // cần mua máy
            P1[3] = "MCT"; // cho thuê máy
            P1[4] = "CTM"; // cần thuê máy
            P1[5] = "PHT"; // bán phụ tùng
            P1[6] = "PHT"; // cần mua phụ tùng
            P1[7] = "VTU"; //bán vật tư
            P1[8] = "VTU"; // mua vật tư
            P1[9] = "TDG"; // tuyển dụng
            P1[10] = "TMV"; // tìm việc
            P1[11] = "DVU"; // dịch vụ
            P1[12] = "CTDVU"; // dịch vụ
            try
            {
                var currentProduct = HanomaContext.Product.FirstOrDefault(x => x.Product_ID == ProductId);
                currentProduct.SKU = String.Format("{0}{1}", P1[(int)currentProduct.ProductType_ID], currentProduct.Product_ID);
                HanomaContext.SaveChanges();

            }
            catch (Exception ex)
            {

            }
        }
        private void InsertMetaKeywordIds(int productId)
        {
            try
            {

                var product = HanomaContext.Product.FirstOrDefault(x => x.Product_ID == productId);
                // create alias main key word

                var cateName = HanomaContext.ProductCategory.FirstOrDefault(x => x.ProductCategory_ID == product.ProductCategory_ID)?.Name;
                var manuName = HanomaContext.ProductManufacture.FirstOrDefault(x => x.ProductManufacture_ID == product.ProductManufacture_ID)?.Name;
                var modelName = HanomaContext.ProductModel.FirstOrDefault(x => x.ProductModel_ID == product.ProductModel_ID)?.Name;
                var nameProduct = String.Format("{0} {1} {2}", cateName, manuName, modelName);
                if (String.IsNullOrWhiteSpace(nameProduct)) return;
                nameProduct = nameProduct.Trim();
                var mainKeywordUrl = FormatURL(nameProduct);

                // insert main metakeyword
                var newMainMetakeyWord = new MetaKeyword
                {
                    ProductCategory_ID = product.ProductCategory_ID,
                    Name = nameProduct,
                    URL = mainKeywordUrl,
                    Type = "main"
                };
                HanomaContext.MetaKeyword.Add(newMainMetakeyWord);
                HanomaContext.SaveChanges();
                var res = HanomaContext.MetaKeyword.Find(newMainMetakeyWord.MetaKeyword_ID);
                res.URL = String.Format("{0}-{1}", res.URL, res.MetaKeyword_ID.ToString());
                HanomaContext.SaveChanges();
                var submetakeyIds = HanomaContext.MetaKeyword.Where(x => x.ProductCategory_ID == product.ProductCategory_ID && x.Type != "main").Select(x => x.MetaKeyword_ID).OrderBy(x => Guid.NewGuid()).Take(15).ToList();

                var metaKeyForTitle = submetakeyIds.OrderBy(x => Guid.NewGuid()).Take(3).ToList();
                var metaTitle = nameProduct;
                foreach (var item in metaKeyForTitle)
                {
                    var metaItem = HanomaContext.MetaKeyword.FirstOrDefault(x => x.MetaKeyword_ID == item);
                    metaTitle = metaTitle + ", " + metaItem.Name;
                }
                product.MetaTitle = metaTitle;
                HanomaContext.SaveChanges();


                submetakeyIds.Add(res.MetaKeyword_ID);
                AddMapProductMetaKeyords(product.Product_ID, submetakeyIds);
                UpdateMetakeyWordIdsOfProduct(product.Product_ID, String.Join(",", submetakeyIds.Select(x => x.ToString()).ToList()));


            }
            catch (Exception ex)
            {

            }
        }
        private void AddMapProductMetaKeyords(int productId, List<int> metakeyIds)
        {

            foreach (var metakeyId in metakeyIds)
            {
                var newMetakeywordMap = new MetaKeywordMap
                {
                    MetaKeyword_ID = metakeyId,
                    Product_ID = productId
                };
                HanomaContext.MetaKeywordMap.Add(newMetakeywordMap);
                HanomaContext.SaveChanges();
            }

        }
        private void UpdateMetakeyWordIdsOfProduct(int productId, string ids)
        {

            var product = HanomaContext.Product.FirstOrDefault(x => x.Product_ID == productId);
            product.MetaKeywordIds = ids;

            var metakeywordModel = from metakey in HanomaContext.MetaKeyword
                                   join metakeymap in HanomaContext.MetaKeywordMap on metakey.MetaKeyword_ID equals metakeymap.MetaKeyword_ID
                                   where metakeymap.Product_ID == productId
                                   select metakey.Name;
            var metakeywords = String.Join(",", metakeywordModel.Select(x => x.ToString()).ToList());
            product.MetaKeywords = metakeywords;

            HanomaContext.SaveChanges();

        }
        private void ProcessTagsProduct(int productId, string stringTags)
        {
            if (String.IsNullOrEmpty(stringTags)) return;
            var tags = stringTags.Split(',').ToList();
            foreach (var tag in tags)
            {
                var tagExist = HanomaContext.Tag.FirstOrDefault(x => x.Name.Equals(tag));
                if (tagExist == null)
                {
                    var newTag = new Tag
                    {
                        Name = tag
                    };
                    HanomaContext.Tag.Add(newTag);

                    HanomaContext.TagMap.Add(new TagMap
                    {
                        Tag_ID = newTag.Tag_ID,
                        TagType_ID = 2,
                        Value_ID = productId
                    });
                    HanomaContext.SaveChanges();
                }
                else
                {
                    var tagMap = HanomaContext.TagMap.FirstOrDefault(x => x.Value_ID == productId && x.TagType_ID == 2 && x.Tag_ID == tagExist.Tag_ID);
                    if (tagMap == null)
                    {
                        HanomaContext.TagMap.Add(new TagMap
                        {
                            Tag_ID = tagExist.Tag_ID,
                            TagType_ID = 2,
                            Value_ID = productId
                        });
                        HanomaContext.SaveChanges();
                    }
                }
            }
        }


        public string CreateImageURL(int Product_ID)
        {
            string[] P1 = new string[11];
            P1[1] = "ban";    
            try
            {

                var currentProduct = (from a in HanomaContext.Product
                                      where a.Product_ID == Product_ID
                                      select a).First();                
                return FormatURL(currentProduct.Name) + "-" + Product_ID.ToString();

            }
            catch
            {

            }
            return "nourl";
        }


        public bool CanEditProduct(int Product_ID, string userId)
        {
            if (String.IsNullOrEmpty(userId)) return false;
            var product = HanomaContext.Product.FirstOrDefault(x => x.Product_ID == Product_ID);

            if (product.CreateBy == new Guid(userId) || userId == "06dfe050-4407-4492-b1fd-656ccb8a82b6" || userId == "a4243721-e234-4efc-aec7-048117444572")
            {
                return true;
            }

            return false;
        }

        public void UpdateMetaKeyWords(int productId)
        {
            try
            {

                var keywordMap = HanomaContext.MetaKeywordMap.Where(x => x.Product_ID == productId).ToList();
                HanomaContext.MetaKeywordMap.RemoveRange(keywordMap);
                HanomaContext.SaveChanges();

                var product = HanomaContext.Product.FirstOrDefault(x => x.Product_ID == productId);
                // create alias main key word

                var cateName = HanomaContext.ProductCategory.FirstOrDefault(x => x.ProductCategory_ID == product.ProductCategory_ID)?.Name;
                var manuName = HanomaContext.ProductManufacture.FirstOrDefault(x => x.ProductManufacture_ID == product.ProductManufacture_ID)?.Name;
                var modelName = HanomaContext.ProductModel.FirstOrDefault(x => x.ProductModel_ID == product.ProductModel_ID)?.Name;
                var nameProduct = String.Format("{0} {1} {2}", cateName, manuName, modelName);
                if (String.IsNullOrWhiteSpace(nameProduct)) return;
                nameProduct = nameProduct.Trim();
                var mainKeywordUrl = FormatURL(nameProduct);

                // insert main metakeyword
                var newMainMetakeyWord = new MetaKeyword
                {
                    ProductCategory_ID = product.ProductCategory_ID,
                    Name = nameProduct,
                    URL = mainKeywordUrl,
                    Type = "main"
                };
                HanomaContext.MetaKeyword.Add(newMainMetakeyWord);
                HanomaContext.SaveChanges();
                var res = HanomaContext.MetaKeyword.Find(newMainMetakeyWord.MetaKeyword_ID);
                res.URL = String.Format("{0}-{1}", res.URL, res.MetaKeyword_ID.ToString());
                HanomaContext.SaveChanges();
                var submetakeyIds = HanomaContext.MetaKeyword.Where(x => x.ProductCategory_ID == product.ProductCategory_ID && x.Type != "main").Select(x => x.MetaKeyword_ID).OrderBy(x => Guid.NewGuid()).Take(15).ToList();
                submetakeyIds.Add(res.MetaKeyword_ID);

                AddMapProductMetaKeyords(product.Product_ID, submetakeyIds);
                UpdateMetakeyWordIdsOfProduct(product.Product_ID, String.Join(",", submetakeyIds.Select(x => x.ToString()).ToList()));


            }
            catch (Exception ex)
            {

            }
        }

        public async Task UpdateMainImageProduct(string FileName, int ProductId)
        {
            var productSave = HanomaContext.Product.Find(ProductId);
            if (productSave != null)
            {
                productSave.Image = FileName;
                HanomaContext.SaveChanges();
            }
        }

        public async Task UpdateIllustrationImages(string FileName, int ProductId)
        {
            
            var productPicture = new ProductPicture
            {
                ProductPicture_ID = Guid.NewGuid(),
                Job_ID = Guid.NewGuid(),
                Product_ID = ProductId,
                Image = FileName,
                Active = true,
                CreateDate = DateTime.Now
            };
            HanomaContext.ProductPicture.Add(productPicture);
            HanomaContext.SaveChanges();
        }

        public async Task DeleteIllustrationImages(List<DeleteImageProductPicture> model)
        {
            foreach(var p in model)
            {
                var itemPicture = await HanomaContext.ProductPicture.FirstOrDefaultAsync(x => x.ProductPicture_ID.ToString() == p.ProductPicture_ID);
                if(itemPicture !=null)
                {
                    HanomaContext.ProductPicture.Remove(itemPicture);
                    await HanomaContext.SaveChangesAsync();
                }                    
            }    
           
        }

        

        public async Task<int> CountProductByBrand(int ProductBrandId)
        {
            string strContains = "~~1~~3~~5~~7~~11~~";
            var count = HanomaContext.Product.Count(p => p.ProductBrand_ID == ProductBrandId && p.StatusType_ID == 4 && strContains.Contains("~~" + p.ProductType_ID + "~~"));
            return count;
        }
        public IConnection GetConnection()
        {
            ConnectionFactory connectionFactory = new ConnectionFactory
            {
                HostName = "202.134.18.185",
                Port = 5672,
                UserName = "hanoma",
                Password = "hanoma",
                VirtualHost = "/",
            };
            return connectionFactory.CreateConnection();
        }
        public void Send(string queue, string data)
        {
            using (IConnection connection = GetConnection())
            {
                using (IModel channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue, true, false, false, null);
                    channel.BasicPublish(string.Empty, queue, null, Encoding.UTF8.GetBytes(data));
                }
            }
        }

        public Task PushAWSImageToRabbit(ImageUploadAWSDTO model,string queue)
        {            
            Send(queue, JsonConvert.SerializeObject(model));
            return Task.CompletedTask;
        }

        public async Task<List<PatchNumber_SearchByCategory_Result>> GetPatchNumberByCategory(int? ProductCategory_Id, int? ProductType_ID, string text)
        {

            var output = new List<PatchNumber_SearchByCategory_Result>();
            var pProductCategory_ID = new SqlParameter("@ProductCategory_ID", ProductCategory_Id ?? (object)DBNull.Value);
            var pProductType_ID = new SqlParameter("@ProductType_ID", ProductType_ID ?? (object)DBNull.Value);
            var pItemCount = new SqlParameter("@ItemCount", SqlDbType.Int) { Direction = ParameterDirection.Output };
            try
            {
                 output = await HanomaContext.Set<PatchNumber_SearchByCategory_Result>()
                    .FromSqlRaw($"EXECUTE dbo.PatchNumber_SearchByCategory " +
                    $"@ProductCategory_ID = @ProductCategory_ID, " +
                    $"@ProductType_ID = @ProductType_ID, " +
                    $"@ItemCount = @ItemCount out " ,
                    pProductCategory_ID, pProductType_ID, pItemCount)
                    .AsNoTracking()
                    .ToListAsync();

                return output;

            }
            catch (Exception ex)
            {

            }
            return output;
        }


        public async Task<ProductIdDTO> CheckRenewProductByBrandId(ProductIdDTO model)
        {
            var output = new ProductIdDTO();
            var brand = await HanomaContext.AspNetUserProfiles.FirstOrDefaultAsync(p => p.UserId == model.UserId);
            try
            {
                var checkRenew = await HanomaContext.ProductRenew
                    .FromSqlRaw($"Select * from dbo.ProductRenew where ProductBrand_ID = {brand.ProductBrand_ID} AND CONVERT(varchar, DateRenew, 112) = '{DateTime.Now.ToString("yyyyMMdd")}'")
                    .FirstOrDefaultAsync();
           
            if(checkRenew != null)
            {
                output.NumberRenewInOneDay = checkRenew.RenewCount ?? 1;
            }
            else
            {
                output.NumberRenewInOneDay = 1;
            }
            }
            catch (Exception ex)
            {

            }
            return output;
        }

        public async Task<IEnumerable<Service_Search_Result>> GetListServices(ProductFilter filter)
        {
            var Keyword = new SqlParameter("@Keyword", filter.Keyword ?? (object)DBNull.Value);
            var ProductBrand_ID = new SqlParameter("@ProductBrand_ID", filter.ProductBrandId ?? (object)DBNull.Value);
            var StatusType_ID = new SqlParameter("@StatusType_ID", filter.StatusTypeId);
            var PageSize = new SqlParameter("@PageSize", filter.PageSize);
            var CurrentPage = new SqlParameter("@CurrentPage", filter.Page);
            var LocationID = new SqlParameter("@Location_ID", filter.LocationId ?? (object)DBNull.Value);
            var ProductTypeID = new SqlParameter("@ProductType_ID", filter.ProductTypeId ?? (object)DBNull.Value);
            var ProductCategory_ID = new SqlParameter("@ProductCategory_ID", filter.ProductCategoryId ?? (object)DBNull.Value);
            //var MaterialManufactory = new SqlParameter("@ProductManufactory", filter.MaterialManufactory ?? (object)DBNull.Value);
            //var ProductLabel = new SqlParameter("@Model", filter.ProductLabel ?? (object)DBNull.Value);
            //var StatusMachine = new SqlParameter("@StatusMachine", filter.StatusMachine ?? (object)DBNull.Value);            
            var RelatedCategoryId = new SqlParameter("@RelatedCategoryId", filter.RelatedCategoryId ?? (object)DBNull.Value);

            try
            {
                var output = await HanomaContext.Set<Service_Search_Result>()
                    .FromSqlRaw($"EXECUTE dbo.Service_Search "
                     + $"@Keyword = @Keyword, "
                     + $"@ProductBrand_ID = @ProductBrand_ID, "
                    + $"@ProductCategory_ID = @ProductCategory_ID, "
                    + $"@Location_ID = @Location_ID, "
                    + $"@ProductType_ID = @ProductType_ID, "                                                            
                    + $"@RelatedCategoryId = @RelatedCategoryId, "
                     + $"@StatusType_ID = @StatusType_ID, "
                     + $"@PageSize = @PageSize, "
                     + $"@CurrentPage = @CurrentPage ",
                     Keyword, ProductBrand_ID, ProductCategory_ID, LocationID, ProductTypeID,
                     RelatedCategoryId, StatusType_ID, PageSize, CurrentPage

                    )
                    .AsNoTracking()
                    .ToListAsync();

                return output;
            }
            catch (Exception ex)
            {

            }
            return new List<Service_Search_Result>();
        }

        public async Task<IEnumerable<MetaKeyword>> GetListMetaKeyword(int ProductId)
        {
            var result = from metakey in HanomaContext.MetaKeyword
                         join metakeymap in HanomaContext.MetaKeywordMap on metakey.MetaKeyword_ID equals metakeymap.MetaKeyword_ID
                         where metakeymap.Product_ID == ProductId
                         select metakey;
            return result;
        }

        public async Task<List<TimeLinePost_Result>> GetLstTimeLinePost(string userId, int? Page = 1, int? PageSize = 10)
        {
            var pUserId = new SqlParameter("@UserId", userId);
            var pPageSize = new SqlParameter("@PageSize", PageSize);
            var pCurrentPage = new SqlParameter("@CurrentPage", Page);
            try
            {
                var output = await HanomaContext.Set<TimeLinePost_Result>()
                    .FromSqlRaw($"EXECUTE dbo.TimeLinePost "
                     + $"@UserId = @UserId, "                     
                     + $"@PageSize = @PageSize, "
                     + $"@CurrentPage = @CurrentPage ",
                     pUserId, pPageSize, pCurrentPage
                    )
                    .AsNoTracking()
                    .ToListAsync();

                return output;
            }
            catch (Exception ex)
            {

            }
            return new List<TimeLinePost_Result>();
        }
    }
}
