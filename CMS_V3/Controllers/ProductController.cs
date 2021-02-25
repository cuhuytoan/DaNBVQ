using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using HNM.DataNC.ModelsNC.ModelsDTO;
using HNM.DataNC.ModelsFilter;
using HNM.RepositoriesWeb.RepositoriesBase;
using AutoMapper;
using HNM.CommonNC;
using System;
using X.PagedList;
using System.Linq;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using CMS_V3.Helper;

namespace CMS_V3.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IRepositoryWrapper _repoWrapper;
        private IMapper _mapper;
        private readonly IDistributedCache _distributedCache;

        public ProductController(ILogger<ProductController> logger, IHttpClientFactory clientFactory, IMapper mapper, IRepositoryWrapper repoWrapper, IDistributedCache distributedCache)
        {
            _clientFactory = clientFactory;
            _logger = logger;
            _mapper = mapper;
            _repoWrapper = repoWrapper;
            _distributedCache = distributedCache;
        }
        public IActionResult Index()
        {
            return View();
        }
        async Task<(ListProductCategoryDTO MenuMachine, ListProductCategoryDTO MenuMaterials, ListProductCategoryDTO MenuServices)> GetCategoryMenu()
        {
            ListProductCategoryDTO MenuMachine = new ListProductCategoryDTO();
            ListProductCategoryDTO MenuMaterials = new ListProductCategoryDTO();
            ListProductCategoryDTO MenuServices = new ListProductCategoryDTO();

            try
            {
                var cacheKey = $"GetMenu-654";
                var redisEncode = await _distributedCache.GetStringAsync(cacheKey);
                if (redisEncode != null)
                {
                    MenuMachine = JsonConvert.DeserializeObject<ListProductCategoryDTO>(redisEncode);
                }
                else
                {
                    MenuMachine = await _repoWrapper.ProductCategory.GetLstMenuByParentId(654);
                    await _distributedCache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(MenuMachine), CommonHelper.RedisOptions());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"ProductController- GetMenu654 {ex.ToString()}");
            }

            try
            {
                var cacheKey = $"GetMenu-652";
                var redisEncode = await _distributedCache.GetStringAsync(cacheKey);
                if (redisEncode != null)
                {
                    MenuMaterials = JsonConvert.DeserializeObject<ListProductCategoryDTO>(redisEncode);
                }
                else
                {
                    MenuMaterials = await _repoWrapper.ProductCategory.GetLstMenuByParentId(652);
                    await _distributedCache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(MenuMaterials), CommonHelper.RedisOptions());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"ProductController- GetMenu652 {ex.ToString()}");
            }


            try
            {
                var cacheKey = $"GetMenu-651";
                var redisEncode = await _distributedCache.GetStringAsync(cacheKey);
                if (redisEncode != null)
                {
                    MenuServices = JsonConvert.DeserializeObject<ListProductCategoryDTO>(redisEncode);
                }
                else
                {
                    MenuServices = await _repoWrapper.ProductCategory.GetLstMenuByParentId(651);
                    await _distributedCache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(MenuServices), CommonHelper.RedisOptions());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"ProductController- GetMenuService {ex.ToString()}");
            }

            return (MenuMachine, MenuMaterials, MenuServices);
        }
        public async Task<IActionResult> SellingProduct()
        {
            ViewBag.CategoryMenuHeader = await GetCategoryMenu();

            ViewBag.SubMenu656 = await _repoWrapper.ProductCategory.GetLstMenuByParentId(656); // máy công trình
            ViewBag.SubMenu1902 = await _repoWrapper.ProductCategory.GetLstMenuByParentId(1902); // xe hơi
            ViewBag.SubMenu669 = await _repoWrapper.ProductCategory.GetLstMenuByParentId(669); // xe tải chở hàng
            ProductPaggingDTO output = new ProductPaggingDTO();
            ViewBag.ProductOfCate656 = await _repoWrapper.Product.GetListProductPagging(new ProductFilter { Page = 1, PageSize = 10, StatusTypeId = 4, ProductCategoryId = 656, ProductTypeId = 1 });
            ViewBag.ProductOfCate1902 = await _repoWrapper.Product.GetListProductPagging(new ProductFilter { Page = 1, PageSize = 10, StatusTypeId = 4, ProductCategoryId = 1902, ProductTypeId = 1 });
            ViewBag.ProductOfCate669 = await _repoWrapper.Product.GetListProductPagging(new ProductFilter { Page = 1, PageSize = 10, StatusTypeId = 4, ProductCategoryId = 669, ProductTypeId = 1 });

            ViewBag.CategoryMenuTakeTop = await _repoWrapper.ProductCategory.GetLstMenuByParentId(654);
            ViewBag.CateMenus = await _repoWrapper.ProductCategory.GetProdCateTwoLevel(654);

            return View();
        }
        public async Task<IActionResult> SellingProductDetails()
        {
            ViewBag.CategoryMenuHeader = await GetCategoryMenu();

            var url = RouteData.Values["url"];
            int productId = Utils.RegexRouteIdFromUrl(url.ToString());
            ViewBag.MetaTags = await _repoWrapper.Setting.GetSetting();
            var productTypeId = 1;
            
            ProductDetailsDTO model = await _repoWrapper.Service.GetProductDetails(productId);
            
            if(model.ErrorCode == "00" || model.ErrorCode == "0")
            {
                ViewBag.ListProductAds = await _repoWrapper.Product.GetSponsorBannerProduct(model.ProductDetails.ProductCategory_ID ?? 654, productTypeId, 5);
                ViewBag.ListProductByCate = await _repoWrapper.Product.GetListProductPagging(new ProductFilter { Page = 1, PageSize = 10, StatusTypeId = 4, ProductCategoryId = model.ProductDetails.ProductCategory_ID ?? 654, ProductTypeId = productTypeId });
                ViewBag.ListProductByProductBrand = await _repoWrapper.Product.GetListProductPagging(new ProductFilter { Page = 1, PageSize = 10, StatusTypeId = 4, ProductBrandId = model.ProductDetails.ProductBrand_ID, ProductTypeId = productTypeId });

                ViewBag.ProductCategoryCurrent = await _repoWrapper.ProductCategory.GetDetailProductCategory((int)model.ProductDetails.ProductCategory_ID);

                return View(model);
            }
            else
            {
                return Redirect("/Error/404");
            }

        }

        public async Task<IActionResult> RentProduct()
        {
            ViewBag.CategoryMenuHeader = await GetCategoryMenu();
            ViewBag.SubMenu656 = await _repoWrapper.ProductCategory.GetLstMenuByParentId(656); // máy công trình
            ViewBag.SubMenu1902 = await _repoWrapper.ProductCategory.GetLstMenuByParentId(1902); // xe hơi
            ViewBag.SubMenu669 = await _repoWrapper.ProductCategory.GetLstMenuByParentId(669); // xe tải chở hàng

            ViewBag.ProductOfCate656 = await _repoWrapper.Product.GetListProductPagging(new ProductFilter { Page = 1, PageSize = 10, StatusTypeId = 4, ProductCategoryId = 656, ProductTypeId = 3 });
            ViewBag.ProductOfCate1902 = await _repoWrapper.Product.GetListProductPagging(new ProductFilter { Page = 1, PageSize = 10, StatusTypeId = 4, ProductCategoryId = 1902, ProductTypeId = 3 });
            ViewBag.ProductOfCate669 = await _repoWrapper.Product.GetListProductPagging(new ProductFilter { Page = 1, PageSize = 10, StatusTypeId = 4, ProductCategoryId = 669, ProductTypeId = 3 });


            ViewBag.CategoryMenuTakeTop = await _repoWrapper.ProductCategory.GetLstMenuByParentId(654);
            ViewBag.CateMenus = await _repoWrapper.ProductCategory.GetProdCateTwoLevel(654);

            return View();
        }

        public async Task<IActionResult> RentProductDetails()
        {
            ViewBag.CategoryMenuHeader = await GetCategoryMenu();
            var url = RouteData.Values["url"];
            int productId = Utils.RegexRouteIdFromUrl(url.ToString());
            ViewBag.MetaTags = await _repoWrapper.Setting.GetSetting();
            var productTypeId = 3;

            ProductDetailsDTO model = await _repoWrapper.Service.GetProductDetails(productId);
            if (model.ErrorCode == "00" || model.ErrorCode == "0")
            {
                ViewBag.ListProductAds = await _repoWrapper.Product.GetSponsorBannerProduct(model.ProductDetails.ProductCategory_ID ?? 654, productTypeId, 5);
                ViewBag.ListProductByCate = await _repoWrapper.Product.GetListProductPagging(new ProductFilter { Page = 1, PageSize = 10, StatusTypeId = 4, ProductCategoryId = model.ProductDetails.ProductCategory_ID ?? 654, ProductTypeId = productTypeId });
                ViewBag.ListProductByProductBrand = await _repoWrapper.Product.GetListProductPagging(new ProductFilter { Page = 1, PageSize = 10, StatusTypeId = 4, ProductBrandId = model.ProductDetails.ProductBrand_ID, ProductTypeId = productTypeId });
                ViewBag.ProductCategoryCurrent = await _repoWrapper.ProductCategory.GetDetailProductCategory((int)model.ProductDetails.ProductCategory_ID);
                return View(model);
            }
            else
            {
                return Redirect("/Error/404");
            }
        }
        public async Task<IActionResult> SellingMaterial()
        {
            ViewBag.CategoryMenuHeader = await GetCategoryMenu();
            ViewBag.SubMenu1567 = await _repoWrapper.ProductCategory.GetLstMenuByParentId(1567); // vật tư thiết bị
            ViewBag.SubMenu1568 = await _repoWrapper.ProductCategory.GetLstMenuByParentId(1568); // vật tư bảo hộ lao động 
            ViewBag.SubMenu1569 = await _repoWrapper.ProductCategory.GetLstMenuByParentId(1569); // vật tư ngành nước

            ViewBag.ProductOfCate1567 = await _repoWrapper.Product.GetListProductPagging(new ProductFilter { Page = 1, PageSize = 10, StatusTypeId = 4, ProductCategoryId = 1567, ProductTypeId = 7 });
            ViewBag.ProductOfCate1568 = await _repoWrapper.Product.GetListProductPagging(new ProductFilter { Page = 1, PageSize = 10, StatusTypeId = 4, ProductCategoryId = 1568, ProductTypeId = 7 });
            ViewBag.ProductOfCate1569 = await _repoWrapper.Product.GetListProductPagging(new ProductFilter { Page = 1, PageSize = 10, StatusTypeId = 4, ProductCategoryId = 1569, ProductTypeId = 7 });


            ViewBag.CategoryMenuTakeTop = await _repoWrapper.ProductCategory.GetLstMenuByParentId(652);
            ViewBag.CateMenus = await _repoWrapper.ProductCategory.GetProdCateTwoLevel(652);

            return View();
        }

        public async Task<IActionResult> SellingMaterialDetails()
        {
            ViewBag.CategoryMenuHeader = await GetCategoryMenu();
            var url = RouteData.Values["url"];
            int productId = Utils.RegexRouteIdFromUrl(url.ToString());
            ViewBag.MetaTags = await _repoWrapper.Setting.GetSetting();
            var productTypeId = 7;

            ProductDetailsDTO model = await _repoWrapper.Service.GetProductDetails(productId);
            if (model.ErrorCode == "00" || model.ErrorCode == "0")
            {
                ViewBag.ListProductAds = await _repoWrapper.Product.GetSponsorBannerProduct(model.ProductDetails.ProductCategory_ID ?? 654, productTypeId, 5);
                ViewBag.ListProductByCate = await _repoWrapper.Product.GetListProductPagging(new ProductFilter { Page = 1, PageSize = 10, StatusTypeId = 4, ProductCategoryId = model.ProductDetails.ProductCategory_ID ?? 654, ProductTypeId = productTypeId });
                ViewBag.ListProductByProductBrand = await _repoWrapper.Product.GetListProductPagging(new ProductFilter { Page = 1, PageSize = 10, StatusTypeId = 4, ProductBrandId = model.ProductDetails.ProductBrand_ID, ProductTypeId = productTypeId });
                ViewBag.ProductCategoryCurrent = await _repoWrapper.ProductCategory.GetDetailProductCategory((int)model.ProductDetails.ProductCategory_ID);
                return View(model);
            }
            else
            {
                return Redirect("/Error/404");
            }
        }

        public async Task<IActionResult> Accessories()
        {
            ViewBag.CategoryMenuHeader = await GetCategoryMenu();
            ViewBag.SubMenu656 = await _repoWrapper.ProductCategory.GetLstMenuByParentId(656); // máy công trình
            ViewBag.SubMenu1902 = await _repoWrapper.ProductCategory.GetLstMenuByParentId(1902); // xe hơi
            ViewBag.SubMenu669 = await _repoWrapper.ProductCategory.GetLstMenuByParentId(669); // xe tải chở hàng

            ViewBag.ProductOfCate656 = await _repoWrapper.Product.GetListProductPagging(new ProductFilter { Page = 1, PageSize = 10, StatusTypeId = 4, ProductCategoryId = 656, ProductTypeId = 5 });
            ViewBag.ProductOfCate1902 = await _repoWrapper.Product.GetListProductPagging(new ProductFilter { Page = 1, PageSize = 10, StatusTypeId = 4, ProductCategoryId = 1902, ProductTypeId = 5 });
            ViewBag.ProductOfCate669 = await _repoWrapper.Product.GetListProductPagging(new ProductFilter { Page = 1, PageSize = 10, StatusTypeId = 4, ProductCategoryId = 669, ProductTypeId = 5 });


            ViewBag.CategoryMenuTakeTop = await _repoWrapper.ProductCategory.GetLstMenuByParentId(654);
            ViewBag.CateMenus = await _repoWrapper.ProductCategory.GetProdCateTwoLevel(654);

            return View();
        }

        public async Task<IActionResult> AccessoriesDetails()
        {
            ViewBag.CategoryMenuHeader = await GetCategoryMenu();
            var url = RouteData.Values["url"];
            int productId = Utils.RegexRouteIdFromUrl(url.ToString());
            ViewBag.MetaTags = await _repoWrapper.Setting.GetSetting();
            var productTypeId = 5;

            ProductDetailsDTO model = await _repoWrapper.Service.GetProductDetails(productId);
            if (model.ErrorCode == "00" || model.ErrorCode == "0")
            {
                ViewBag.ListProductAds = await _repoWrapper.Product.GetSponsorBannerProduct(model.ProductDetails.ProductCategory_ID ?? 654, productTypeId, 5);
                ViewBag.ListProductByCate = await _repoWrapper.Product.GetListProductPagging(new ProductFilter { Page = 1, PageSize = 10, StatusTypeId = 4, ProductCategoryId = model.ProductDetails.ProductCategory_ID ?? 654, ProductTypeId = productTypeId });
                ViewBag.ListProductByProductBrand = await _repoWrapper.Product.GetListProductPagging(new ProductFilter { Page = 1, PageSize = 10, StatusTypeId = 4, ProductBrandId = model.ProductDetails.ProductBrand_ID, ProductTypeId = productTypeId });
                ViewBag.ProductCategoryCurrent = await _repoWrapper.ProductCategory.GetDetailProductCategory((int)(model.ProductDetails.ProductCategory_ID ?? model.ProductDetails.AccessoriesCategoryId ?? -1));
                return View(model);
            }
            else
            {
                return Redirect("/Error/404");
            }
        }

        public async Task<IActionResult> SellingProductCategory()
        {
            ViewBag.CategoryMenuHeader = await GetCategoryMenu();
            var url = RouteData.Values["url"];
            int cateId = Utils.RegexRouteIdFromUrl(url.ToString());

            ViewBag.CategoryMenuTakeTop = await _repoWrapper.ProductCategory.GetLstMenuByParentId(654);
            ViewBag.CateMenus = await _repoWrapper.ProductCategory.GetProdCateTwoLevel(654);
            var curentCate = await _repoWrapper.ProductCategory.GetDetailProductCategory(cateId);
            ViewBag.ProductCategoryCurrent = curentCate;
            ViewBag.ProductCategoryParent = await _repoWrapper.ProductCategory.GetDetailProductCategory((int)curentCate.ParentId);
            ViewBag.CurrentCategoryId = cateId;

            ViewBag.ListProductAds = await _repoWrapper.Product.GetSponsorBannerProduct(cateId, null, 5);

            return View();
        }
        //RentProductCategory
        public async Task<IActionResult> RentProductCategory()
        {
            ViewBag.CategoryMenuHeader = await GetCategoryMenu();
            var url = RouteData.Values["url"];
            int cateId = Utils.RegexRouteIdFromUrl(url.ToString());

            ViewBag.CategoryMenuTakeTop = await _repoWrapper.ProductCategory.GetLstMenuByParentId(654);
            ViewBag.CateMenus = await _repoWrapper.ProductCategory.GetProdCateTwoLevel(654);
            var curentCate = await _repoWrapper.ProductCategory.GetDetailProductCategory(cateId);
            ViewBag.ProductCategoryCurrent = curentCate;
            ViewBag.ProductCategoryParent = await _repoWrapper.ProductCategory.GetDetailProductCategory((int)curentCate.ParentId);
            ViewBag.CurrentCategoryId = cateId;

            ViewBag.ListProductAds = await _repoWrapper.Product.GetSponsorBannerProduct(cateId, null, 5);

            return View();
        }
        //AccessoriesProductCategory
        public async Task<IActionResult> AccessoriesProductCategory()
        {
            ViewBag.CategoryMenuHeader = await GetCategoryMenu();
            var url = RouteData.Values["url"];
            int cateId = Utils.RegexRouteIdFromUrl(url.ToString());

            ViewBag.CategoryMenuTakeTop = await _repoWrapper.ProductCategory.GetLstMenuByParentId(654);
            ViewBag.CateMenus = await _repoWrapper.ProductCategory.GetProdCateTwoLevel(654);
            ViewBag.ProductCategoryCurrent = await _repoWrapper.ProductCategory.GetDetailProductCategory(cateId);
            var curentCate = await _repoWrapper.ProductCategory.GetDetailProductCategory(cateId);
            ViewBag.ProductCategoryCurrent = curentCate;
            ViewBag.ProductCategoryParent = await _repoWrapper.ProductCategory.GetDetailProductCategory((int)curentCate.ParentId);
            ViewBag.CurrentCategoryId = cateId;

            ViewBag.ListProductAds = await _repoWrapper.Product.GetSponsorBannerProduct(cateId, null, 5);

            return View();
        }
        //MaterialProductCategory
        public async Task<IActionResult> MaterialProductCategory()
        {
            ViewBag.CategoryMenuHeader = await GetCategoryMenu();
            var url = RouteData.Values["url"];
            int cateId = Utils.RegexRouteIdFromUrl(url.ToString());

            ViewBag.CategoryMenuTakeTop = await _repoWrapper.ProductCategory.GetLstMenuByParentId(652);
            ViewBag.CateMenus = await _repoWrapper.ProductCategory.GetProdCateTwoLevel(652);
            ViewBag.ProductCategoryCurrent = await _repoWrapper.ProductCategory.GetDetailProductCategory(cateId);
            var curentCate = await _repoWrapper.ProductCategory.GetDetailProductCategory(cateId);
            ViewBag.ProductCategoryCurrent = curentCate;
            ViewBag.ProductCategoryParent = await _repoWrapper.ProductCategory.GetDetailProductCategory((int)curentCate.ParentId);
            ViewBag.CurrentCategoryId = cateId;

            ViewBag.ListProductAds = await _repoWrapper.Product.GetSponsorBannerProduct(cateId, null, 5);

            return View();
        }
        public async Task<ActionResult> FilterPartialView(int? productTypeId, int? productCategoryId, int? manufactureId, int? productModelId, int? locationId, int curentCategoryId)
        {
            ViewBag.CateCurrent = await _repoWrapper.ProductCategory.GetDetailProductCategory(curentCategoryId);
            ViewBag.ListCateChildren = await _repoWrapper.ProductCategory.GetLstMenuByParentId(curentCategoryId);

            ViewBag.ProductTypeId = productTypeId;
            ViewBag.ProductCategoryId = productCategoryId;
            ViewBag.ManufactureId = manufactureId;
            ViewBag.ProductModelId = productModelId;
            ViewBag.LocationId = locationId;
            ViewBag.CurentCategoryId = curentCategoryId;

            ViewBag.ListManufactures = await _repoWrapper.Manufacture.SearchManufactures(productCategoryId ?? curentCategoryId, productTypeId, null);
            ViewBag.ListModels = (manufactureId > 0) ?  await _repoWrapper.ProductModel.SearchProductModels(productCategoryId ?? curentCategoryId, productTypeId, manufactureId, null) : new ListProductModelSearchDTO();
            ViewBag.ListLocations = await _repoWrapper.Location.GetAllLocation();

            return PartialView("FilterPartialView");
        }
        public async Task<ActionResult> FilterMaterialCategoryPartialView(int? productTypeId, int? productCategoryId, string partNumber, int? locationId, int curentCategoryId)
        {
            ViewBag.CateCurrent = await _repoWrapper.ProductCategory.GetDetailProductCategory(curentCategoryId);
            ViewBag.ListCateChildren = await _repoWrapper.ProductCategory.GetLstMenuByParentId(curentCategoryId);

            ViewBag.ProductTypeId = productTypeId ?? 7;
            ViewBag.ProductCategoryId = productCategoryId;
            ViewBag.LocationId = locationId;
            ViewBag.CurentCategoryId = curentCategoryId;
            ViewBag.PartNumber = partNumber;
            ViewBag.ListLocations = await _repoWrapper.Location.GetAllLocation();
            ViewBag.ListPartNumber = await _repoWrapper.Product.GetPartNumber(productCategoryId ?? curentCategoryId, productTypeId ?? 7);

            return PartialView("FilterMaterialCategoryPartialView");
        }
        public async Task<ActionResult> FilterAccessoriesCategoryPartialView(int? productTypeId, int? productCategoryId, int? manufactureId, int? productModelId,int? mainSystemId, int? childMainSystemId, string statusMachine, string partNumber, int? locationId, int curentCategoryId)
        {
            ViewBag.CateCurrent = await _repoWrapper.ProductCategory.GetDetailProductCategory(curentCategoryId);
            ViewBag.ListCateChildren = await _repoWrapper.ProductCategory.GetLstMenuByParentId(curentCategoryId);

            ViewBag.ProductTypeId = productTypeId;
            ViewBag.ProductCategoryId = productCategoryId;
            ViewBag.ManufactureId = manufactureId;
            ViewBag.ProductModelId = productModelId;
            ViewBag.LocationId = locationId;
            ViewBag.CurentCategoryId = curentCategoryId;
            ViewBag.MainSystemId = mainSystemId;
            ViewBag.ChildMainSystemId = childMainSystemId;
            ViewBag.PartNumber = partNumber;
            ViewBag.StatusMachine = statusMachine;            
            ViewBag.ListManufactures = await _repoWrapper.Manufacture.SearchManufactures(productCategoryId ?? curentCategoryId, productTypeId, null);
            ViewBag.ListModels = (manufactureId > 0) ? await _repoWrapper.ProductModel.SearchProductModels(productCategoryId ?? curentCategoryId, productTypeId, manufactureId, null) : new ListProductModelSearchDTO();
            ViewBag.ListLocations = await _repoWrapper.Location.GetAllLocation();
            ViewBag.ListMainSystem = await _repoWrapper.ProductCategory.GetLstMenuByParentId(653);
            ViewBag.ListChildrenMainSystem = mainSystemId > 0 ? await _repoWrapper.ProductCategory.GetLstMenuByParentId((int)mainSystemId) : new ListProductCategoryDTO();
            ViewBag.ListPartNumber = await _repoWrapper.Product.GetPartNumber(productCategoryId ?? curentCategoryId, productTypeId ?? 5);

            return PartialView("FilterAccessoriesCategoryPartialView");
        }
        public async Task<ActionResult> FilterProductResultPartialView(ProductFilter filterModel)
        {
            ViewBag.FilterModel = filterModel;
            ViewBag.ProductTypeId = filterModel.ProductTypeId ?? 1;
            var curentCate = await _repoWrapper.ProductCategory.GetDetailProductCategory((int)filterModel.ProductCategoryId);
            ViewBag.ProductCategoryCurrent = curentCate;
            var productsRes = await _repoWrapper.Product.GetListProductPagging(filterModel);
            ViewBag.ProductPaging =
                new StaticPagedList<ProductSearchResultDTO>
                (
                    productsRes.Data, filterModel.Page, filterModel.PageSize, productsRes.TotalRecord
                    );
            return PartialView("FilterProductResultPartialView", productsRes);
        }

        public async Task<JsonResult> ActionProductShopMan(ProductIdDTO productIdDTO, string ActionString)
        {
            if(ActionString == "R")
            {
                var checkRenew = await _repoWrapper.Product.CheckRenewProduct();
                if (checkRenew.ErrorCode == "00" || checkRenew.ErrorCode == "0")
                {
                    var result = await _repoWrapper.Product.ActionProductShopMan(productIdDTO, ActionString);
                    return Json(result);
                }
                else
                {
                    return Json(checkRenew);
                }
                
            }
            else
            {
                if(ActionString == "K")
                {
                    var result = await _repoWrapper.Product.ActionProductShopMan(productIdDTO, ActionString);
                    return Json(result);
                }
                else
                {
                    var result = await _repoWrapper.Product.ActionProductShopMan(productIdDTO, ActionString);
                    return Json(result.Message);
                }
                
            }
        }
        
        public async Task<JsonResult> ActionInRec(int?Id, string ActionString)
        {
            var result = await _repoWrapper.Product.ActionInRec(Id, ActionString);
            return Json(result.Message);
        } 
        
        public async Task<JsonResult> ActionInCV(int?Id, string ActionString)
        {
            var result = await _repoWrapper.Product.ActionInRec(Id, ActionString);
            return Json(result.Message);
        }

        public async Task<JsonResult> ActionProductCanclePost(string ids, string ActionString)
        {
            ProductIdDTO ProductIdDTO = new ProductIdDTO();
            if (ActionString == "H")
            {
                if (String.IsNullOrEmpty(ids)) return Json("Hủy tin đăng thất bại");
                var idArr = ids.Split(',').ToList();
                foreach (var id in idArr)
                {
                    var productId = Int32.Parse(id);
                    ProductIdDTO.ProductId = productId;
                    var result = await _repoWrapper.Product.ActionProductShopMan(ProductIdDTO, ActionString);
                }
                return Json("00");
            }
            else
            {
                if(ActionString == "D")
                {
                    if (String.IsNullOrEmpty(ids)) return Json("Xóa tin đăng thất bại");
                    var idArr = ids.Split(',').ToList();
                    foreach (var id in idArr)
                    {
                        var productId = Int32.Parse(id);
                        ProductIdDTO.ProductId = productId;
                        var result = await _repoWrapper.Product.ActionProductShopMan(ProductIdDTO, ActionString);
                    }
                    return Json("00");
                }
                else
                {
                    return Json("Thất bại");
                }
                
            }
        }
    }
}
