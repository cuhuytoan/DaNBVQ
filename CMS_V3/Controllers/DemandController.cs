using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using HNM.CommonNC;
using HNM.DataNC.ModelsFilter;
using HNM.DataNC.ModelsNC.ModelsDTO;
using HNM.RepositoriesWeb.RepositoriesBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using X.PagedList;

namespace CMS_V3.Controllers
{
    public class DemandController : Controller
    {
        private readonly ILogger<DemandController> _logger;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IRepositoryWrapper _repoWrapper;
        private IMapper _mapper;

        public DemandController(ILogger<DemandController> logger, IHttpClientFactory clientFactory, IMapper mapper, IRepositoryWrapper repoWrapper)
        {
            _clientFactory = clientFactory;
            _logger = logger;
            _mapper = mapper;
            _repoWrapper = repoWrapper;
        }

        async Task<(ListProductCategoryDTO MenuMachine, ListProductCategoryDTO MenuMaterials, ListProductCategoryDTO MenuServices)> GetCategoryMenu()
        {
            var MenuMachine = await _repoWrapper.ProductCategory.GetLstMenuByParentId(654);
            var MenuMaterials = await _repoWrapper.ProductCategory.GetLstMenuByParentId(652);
            var MenuServices = await _repoWrapper.ProductCategory.GetLstMenuByParentId(651);
            return (MenuMachine, MenuMaterials, MenuServices);
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> DemandProduct()
        {
            ViewBag.CategoryMenuHeader = await GetCategoryMenu();

            ViewBag.SubMenu656 = await _repoWrapper.ProductCategory.GetLstMenuByParentId(656); // máy công trình
            ViewBag.SubMenu1902 = await _repoWrapper.ProductCategory.GetLstMenuByParentId(1902); // xe hơi
            ViewBag.SubMenu669 = await _repoWrapper.ProductCategory.GetLstMenuByParentId(669); // xe tải chở hàng
            ProductPaggingDTO output = new ProductPaggingDTO();
            ViewBag.ProductOfCate656 = await _repoWrapper.Product.GetListProductPagging(new ProductFilter { Page = 1, PageSize = 10, StatusTypeId = 4, ProductCategoryId = 656, ProductTypeId = 2 });
            ViewBag.ProductOfCate1902 = await _repoWrapper.Product.GetListProductPagging(new ProductFilter { Page = 1, PageSize = 10, StatusTypeId = 4, ProductCategoryId = 1902, ProductTypeId = 2 });
            ViewBag.ProductOfCate669 = await _repoWrapper.Product.GetListProductPagging(new ProductFilter { Page = 1, PageSize = 10, StatusTypeId = 4, ProductCategoryId = 669, ProductTypeId = 2 });

            ViewBag.CategoryMenuTakeTop = await _repoWrapper.ProductCategory.GetLstMenuByParentId(654);
            ViewBag.CateMenus = await _repoWrapper.ProductCategory.GetProdCateTwoLevel(654);

            return View();
        }

        public async Task<IActionResult> DemandMaterial()
        {
            ViewBag.CategoryMenuHeader = await GetCategoryMenu();
            ViewBag.SubMenu1567 = await _repoWrapper.ProductCategory.GetLstMenuByParentId(1567); // vật tư thiết bị
            ViewBag.SubMenu1568 = await _repoWrapper.ProductCategory.GetLstMenuByParentId(1568); // vật tư bảo hộ lao động 
            ViewBag.SubMenu1569 = await _repoWrapper.ProductCategory.GetLstMenuByParentId(1569); // vật tư ngành nước

            ViewBag.ProductOfCate1567 = await _repoWrapper.Product.GetListProductPagging(new ProductFilter { Page = 1, PageSize = 10, StatusTypeId = 4, ProductCategoryId = 1567, ProductTypeId = 8 });
            ViewBag.ProductOfCate1568 = await _repoWrapper.Product.GetListProductPagging(new ProductFilter { Page = 1, PageSize = 10, StatusTypeId = 4, ProductCategoryId = 1568, ProductTypeId = 8 });
            ViewBag.ProductOfCate1569 = await _repoWrapper.Product.GetListProductPagging(new ProductFilter { Page = 1, PageSize = 10, StatusTypeId = 4, ProductCategoryId = 1569, ProductTypeId = 8 });


            ViewBag.CategoryMenuTakeTop = await _repoWrapper.ProductCategory.GetLstMenuByParentId(652);
            ViewBag.CateMenus = await _repoWrapper.ProductCategory.GetProdCateTwoLevel(652);

            return View();
        }

        public async Task<IActionResult> DemandAccessories()
        {
            ViewBag.CategoryMenuHeader = await GetCategoryMenu();
            ViewBag.SubMenu656 = await _repoWrapper.ProductCategory.GetLstMenuByParentId(656); // máy công trình
            ViewBag.SubMenu1902 = await _repoWrapper.ProductCategory.GetLstMenuByParentId(1902); // xe hơi
            ViewBag.SubMenu669 = await _repoWrapper.ProductCategory.GetLstMenuByParentId(669); // xe tải chở hàng

            ViewBag.ProductOfCate656 = await _repoWrapper.Product.GetListProductPagging(new ProductFilter { Page = 1, PageSize = 10, StatusTypeId = 4, ProductCategoryId = 656, ProductTypeId = 6 });
            ViewBag.ProductOfCate1902 = await _repoWrapper.Product.GetListProductPagging(new ProductFilter { Page = 1, PageSize = 10, StatusTypeId = 4, ProductCategoryId = 1902, ProductTypeId = 6 });
            ViewBag.ProductOfCate669 = await _repoWrapper.Product.GetListProductPagging(new ProductFilter { Page = 1, PageSize = 10, StatusTypeId = 4, ProductCategoryId = 669, ProductTypeId = 6 });


            ViewBag.CategoryMenuTakeTop = await _repoWrapper.ProductCategory.GetLstMenuByParentId(654);
            ViewBag.CateMenus = await _repoWrapper.ProductCategory.GetProdCateTwoLevel(654);

            return View();
        }

        public async Task<IActionResult> DemandRentProduct()
        {
            ViewBag.CategoryMenuHeader = await GetCategoryMenu();
            ViewBag.SubMenu656 = await _repoWrapper.ProductCategory.GetLstMenuByParentId(656); // máy công trình
            ViewBag.SubMenu1902 = await _repoWrapper.ProductCategory.GetLstMenuByParentId(1902); // xe hơi
            ViewBag.SubMenu669 = await _repoWrapper.ProductCategory.GetLstMenuByParentId(669); // xe tải chở hàng

            ViewBag.ProductOfCate656 = await _repoWrapper.Product.GetListProductPagging(new ProductFilter { Page = 1, PageSize = 10, StatusTypeId = 4, ProductCategoryId = 656, ProductTypeId = 4 });
            ViewBag.ProductOfCate1902 = await _repoWrapper.Product.GetListProductPagging(new ProductFilter { Page = 1, PageSize = 10, StatusTypeId = 4, ProductCategoryId = 1902, ProductTypeId = 4 });
            ViewBag.ProductOfCate669 = await _repoWrapper.Product.GetListProductPagging(new ProductFilter { Page = 1, PageSize = 10, StatusTypeId = 4, ProductCategoryId = 669, ProductTypeId = 4 });


            ViewBag.CategoryMenuTakeTop = await _repoWrapper.ProductCategory.GetLstMenuByParentId(654);
            ViewBag.CateMenus = await _repoWrapper.ProductCategory.GetProdCateTwoLevel(654);

            return View();
        }

        public async Task<IActionResult> DemandRentEmploy()
        {
            //Get Menu
            ViewBag.MenuMachine = await _repoWrapper.ProductCategory.GetLstMenuByParentId(654);
            ViewBag.MenuMaterials = await _repoWrapper.ProductCategory.GetLstMenuByParentId(652);
            ViewBag.MenuServices = await _repoWrapper.ProductCategory.GetLstMenuByParentId(651);
            ViewBag.ListCategory = await _repoWrapper.Service.GetProdCateByParentID(651);

            ViewBag.ListService = await _repoWrapper.Service.GetListService(10, 15, 4, 12);
            ViewBag.ListServiceCarriage = await _repoWrapper.Service.GetListServiceByCate(1, 10, 4, 12, 1818, null, null);
            ViewBag.ListServiceRepair = await _repoWrapper.Service.GetListServiceByCate(1, 10, 4, 12, 1817, null, null);
            ViewBag.ListServiceRescue = await _repoWrapper.Service.GetListServiceByCate(1, 10, 4, 12, 1819, null, null);
            ViewBag.MetaTags = await _repoWrapper.Setting.GetSetting();
            return View();
        }


        public async Task<IActionResult> DemandBuyProductCategory()
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

        public async Task<IActionResult> DemandBuyMaterialCategory()
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

        public async Task<IActionResult> DemandBuyAccessoriesCategory()
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

        public async Task<IActionResult> DemandRentProductCategory()
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

        public async Task<IActionResult> DemandRentEmployCategory()
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

        public async Task<IActionResult> DemandEmployProductCategory(int? page = 1, int? pageSize = 15)
        {
            //Get Menu
            ViewBag.MenuMachine = await _repoWrapper.ProductCategory.GetLstMenuByParentId(654);
            ViewBag.MenuMaterials = await _repoWrapper.ProductCategory.GetLstMenuByParentId(652);
            ViewBag.MenuServices = await _repoWrapper.ProductCategory.GetLstMenuByParentId(651);
            ViewBag.ListCategory = await _repoWrapper.Service.GetProdCateByParentID(651);
            var url = RouteData.Values["url"];
            int ProductCategory_ID = Utils.RegexRouteIdFromUrl(url.ToString());
            int intPage = page ?? 1;

            if (ProductCategory_ID == 0) // Error Redirect 301
            {
                //Response.StatusCode = 301;
                //Response.Headers("Location", "https://daninhbinhvinhquang.vn/may-de-ban");
                //Response.End();
            }
            else
            {
                ViewBag.ListService = await _repoWrapper.Service.GetListServiceByCate((int)page, (int)pageSize, 4, 12, ProductCategory_ID, null, null);
                ViewBag.Page = page ?? 1;
                ViewBag.PageSize = pageSize ?? 15;
                ViewBag.TotalCount = ViewBag.ListService.TotalRecord;
                ViewBag.TotalPages = (int)Math.Ceiling(decimal.Divide(ViewBag.ListService.TotalRecord, ViewBag.PageSize ?? 15));
            }

            var data = await _repoWrapper.Service.GetListServiceByCate((int)page, (int)pageSize, 4, 12, ProductCategory_ID, null, null);
            ViewBag.ProductCategory_ID = ProductCategory_ID;
            var productCategoryDetail = await _repoWrapper.ProductCategory.GetDetailProductCategory(ProductCategory_ID);
            ViewBag.ProductCateName = productCategoryDetail.Name;
            ViewBag.ProductCateUrl = productCategoryDetail.URL;
            return View(data);
        }

        public async Task<ActionResult> GetPaggingService(int? ProductCategory_ID, int? Location_ID, int? RelatedCategoryId, int? page = 1, int? pageSize = 15)
        {
            ViewBag.Page = page ?? 1;
            ViewBag.PageSize = pageSize ?? 15;
            ViewBag.ProductCategory_ID = ProductCategory_ID;
            var productsRes = await _repoWrapper.Service.GetListServiceByCate((int)page, (int)pageSize, 4, 12, (int)ProductCategory_ID, Location_ID, RelatedCategoryId);
            var productCategoryDetail = await _repoWrapper.ProductCategory.GetDetailProductCategory((int)ProductCategory_ID);
            ViewBag.ProductCateName = productCategoryDetail.Name;
            ViewBag.TotalCount = productsRes.TotalRecord;
            ViewBag.TotalPages = (int)Math.Ceiling(decimal.Divide(productsRes.TotalRecord, pageSize ?? 15));
            ViewBag.ServicePaging =
                new StaticPagedList<ProductSearchResultDTO>
                (
                    productsRes.Data, page ?? 1, pageSize ?? 15, productsRes.TotalRecord
                    );
            return PartialView("ServicePaggingPartial", productsRes);
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
            ViewBag.ListModels = (manufactureId > 0) ? await _repoWrapper.ProductModel.SearchProductModels(productCategoryId ?? curentCategoryId, productTypeId, manufactureId, null) : new ListProductModelSearchDTO();
            ViewBag.ListLocations = await _repoWrapper.Location.GetAllLocation();

            return PartialView("FilterPartialView");
        }
    }
}
