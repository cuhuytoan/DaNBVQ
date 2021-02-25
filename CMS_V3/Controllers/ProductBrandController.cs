using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HNM.CommonNC;
using HNM.DataNC.ModelsNC.ModelsDTO;
using HNM.DataNC.ModelsStore;
using HNM.RepositoriesWeb.RepositoriesBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using X.PagedList;

namespace CMS_V3.Controllers
{
    public class ProductBrandController : Controller
    {
        private readonly ILogger<ProductBrandController> _logger;
        private readonly IRepositoryWrapper _repoWrapper;
        private IMapper _mapper;


        public ProductBrandController(ILogger<ProductBrandController> logger, IMapper mapper, IRepositoryWrapper repoWrapper)
        {
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

        public async Task<IActionResult> Index(int? page = 1, int? pageSize = 20)
        {
            var url = RouteData.Values["url"];
            int ProductBrandId = Utils.RegexRouteIdFromUrl(url.ToString());
            if (ProductBrandId == 0) // Error Redirect 301
            {
                //Response.StatusCode = 301;
                //Response.Headers("Location", "https://hanoma.vn/may-de-ban");
                //Response.End();
            }
            ViewBag.CategoryMenuHeader = await GetCategoryMenu();
            var result = await _repoWrapper.ProductBrand.GetBrandDetails(ProductBrandId);
            ViewBag.LstProduct = await _repoWrapper.ProductBrand.GetListProductByProductBrand((int)page, (int)pageSize, 4, ProductBrandId);
            if (ViewBag.LstProduct != null)
            {
                ViewBag.Page = page ?? 1;
                ViewBag.PageSize = pageSize ?? 20;
                ViewBag.TotalCount = ViewBag.LstProduct.TotalRecord;
                ViewBag.TotalPages = ViewBag.LstProduct.TotalPage;
            }
            ViewBag.ProductBrandUrl = url;
            ViewBag.ProductBrandId = ProductBrandId;
            return View(result);
        }

        public async Task<ActionResult> GetPaggingProduct(int? ProductBrandId, int? page = 1, int? pageSize = 20)
        {
            ViewBag.Page = page ?? 1;
            ViewBag.PageSize = pageSize ?? 20;
            ViewBag.ProductBrandId = ProductBrandId;
            var productsRes = await _repoWrapper.ProductBrand.GetListProductByProductBrand((int)page, (int)pageSize, 4, (int)ProductBrandId);
            ViewBag.TotalCount = productsRes.TotalRecord;
            ViewBag.TotalPages = (int)Math.Ceiling(decimal.Divide(productsRes.TotalRecord, pageSize ?? 20));
            ViewBag.ProductPaging =
                new StaticPagedList<ProductSearchResultDTO>
                (
                    productsRes.Data, page ?? 1, pageSize ?? 20, productsRes.TotalRecord
                    );
            return PartialView("ProductPaggingPartial", productsRes);
        }

        public async Task<IActionResult> Service(int? page = 1, int? pageSize = 20)
        {
            var url = RouteData.Values["url"];
            int ProductBrandId = Utils.RegexRouteIdFromUrl(url.ToString());
            if (ProductBrandId == 0) // Error Redirect 301
            {
                //Response.StatusCode = 301;
                //Response.Headers("Location", "https://hanoma.vn/may-de-ban");
                //Response.End();
            }
            ViewBag.CategoryMenuHeader = await GetCategoryMenu();
            var result = await _repoWrapper.ProductBrand.GetBrandDetails(ProductBrandId);
            ViewBag.LstProduct = await _repoWrapper.Service.GetListServiceByProductBrand((int)page, (int)pageSize , 4, 11, ProductBrandId);
            if (ViewBag.LstProduct != null)
            {
                ViewBag.Page = page ?? 1;
                ViewBag.PageSize = pageSize ?? 20;
                ViewBag.TotalCount = ViewBag.LstProduct.TotalRecord;
                ViewBag.TotalPages = ViewBag.LstProduct.TotalPage;
            }
            ViewBag.ProductBrandUrl = url;
            ViewBag.ProductBrandId = ProductBrandId;
            return View(result);
        }

        public async Task<ActionResult> GetPaggingService(int? ProductBrandId, int? page = 1, int? pageSize = 20)
        {
            ViewBag.Page = page ?? 1;
            ViewBag.PageSize = pageSize ?? 20;
            ViewBag.ProductBrandId = ProductBrandId;
            var productsRes = await _repoWrapper.Service.GetListServiceByProductBrand((int)page, (int)pageSize, 4, 11, (int)ProductBrandId);
            ViewBag.TotalCount = productsRes.TotalRecord;
            ViewBag.TotalPages = (int)Math.Ceiling(decimal.Divide(productsRes.TotalRecord, pageSize ?? 20));
            ViewBag.ProductPaging =
                new StaticPagedList<ProductSearchResultDTO>
                (
                    productsRes.Data, page ?? 1, pageSize ?? 20, productsRes.TotalRecord
                    );
            return PartialView("ServicePaggingPartial", productsRes);
        }

        public async Task<ActionResult> GetPaggingNeedBuyDemand(int? ProductBrandId, int? page = 1, int? pageSize = 20)
        {
            ViewBag.Page = page ?? 1;
            ViewBag.PageSize = pageSize ?? 20;
            ViewBag.ProductBrandId = ProductBrandId;
            var productsRes = await _repoWrapper.Service.GetListServiceByProductBrand((int)page, (int)pageSize, 4, 4, (int)ProductBrandId);
            ViewBag.TotalCount = productsRes.TotalRecord;
            ViewBag.TotalPages = (int)Math.Ceiling(decimal.Divide(productsRes.TotalRecord, pageSize ?? 20));
            ViewBag.ProductPaging =
                new StaticPagedList<ProductSearchResultDTO>
                (
                    productsRes.Data, page ?? 1, pageSize ?? 20, productsRes.TotalRecord
                    );
            return PartialView("ServicePaggingPartial", productsRes);
        }

        public async Task<IActionResult> About()
        {
            var url = RouteData.Values["url"];
            int ProductBrandId = Utils.RegexRouteIdFromUrl(url.ToString());
            if (ProductBrandId == 0) // Error Redirect 301
            {
                //Response.StatusCode = 301;
                //Response.Headers("Location", "https://hanoma.vn/may-de-ban");
                //Response.End();
            }
            ViewBag.CategoryMenuHeader = await GetCategoryMenu();
            var result = await _repoWrapper.ProductBrand.GetBrandDetails(ProductBrandId);
            ViewBag.ProductBrandUrl = url;
            ViewBag.ProductBrandId = ProductBrandId;
            return View(result);
        }

        public async Task<IActionResult> Library(int? page = 1, int? pageSize = 20)
        {
            var url = RouteData.Values["url"];
            int ProductBrandId = Utils.RegexRouteIdFromUrl(url.ToString());
            if (ProductBrandId == 0) // Error Redirect 301
            {
                //Response.StatusCode = 301;
                //Response.Headers("Location", "https://hanoma.vn/may-de-ban");
                //Response.End();
            }
            ViewBag.CategoryMenuHeader = await GetCategoryMenu();
            var result = await _repoWrapper.ProductBrand.GetBrandDetails(ProductBrandId);
            ViewBag.ListLib = await _repoWrapper.ProductBrand.GetListLibByProductBrand(null, null , 4 , result.CreateBy);
            ViewBag.Page = page ?? 1;
            ViewBag.PageSize = pageSize ?? 20;
            ViewBag.TotalCount = ViewBag.ListLib.Count;
            ViewBag.TotalPages = (int)Math.Ceiling(decimal.Divide(ViewBag.ListLib.Count, pageSize ?? 20));
            ViewBag.ProductBrandUrl = url;
            ViewBag.ProductBrandId = ProductBrandId;
            return View(result);
        }

        public async Task<ActionResult> GetPaggingLibrary(int? ProductBrandId, int? page = 1, int? pageSize = 20)
        {
            ViewBag.Page = page ?? 1;
            ViewBag.PageSize = pageSize ?? 20;
            ViewBag.ProductBrandId = ProductBrandId;
            var result = await _repoWrapper.ProductBrand.GetBrandDetails((int)ProductBrandId);
            ViewBag.ListLib = await _repoWrapper.ProductBrand.GetListLibByProductBrand(null, null, 4, result.CreateBy);
            ViewBag.TotalCount = ViewBag.ListLib.Count;
            ViewBag.TotalPages = (int)Math.Ceiling(decimal.Divide(ViewBag.ListLib.Count, pageSize ?? 20));
            ViewBag.LibPaging =
                new StaticPagedList<Library_Search_By_Cate_Result>
                (
                    ViewBag.ListLib, page ?? 1, pageSize ?? 20, ViewBag.TotalCount
                    );
            return PartialView("LibraryPaggingPartial", ViewBag.ListLib);
        }

        public async Task<ActionResult> GetBrandDetails()
        {
            var result = await _repoWrapper.Profile.GetBrandDetailByUserId();
            return Json(result);
        }
    }
}
