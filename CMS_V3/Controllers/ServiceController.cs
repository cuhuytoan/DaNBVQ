using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using CMS_V3.ViewModel;
using HNM.DataNC.Models;
using HNM.DataNC.ModelsNC.ModelsDTO;
using HNM.RepositoriesWeb.RepositoriesBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using HNM.CommonNC;
using X.PagedList;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CMS_V3.Controllers
{
    public class ServiceController : Controller
    {
        private readonly ILogger<LibraryController> _logger;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IRepositoryWrapper _repoWrapper;
        private IMapper _mapper;

        public ServiceController(ILogger<LibraryController> logger, IHttpClientFactory clientFactory, IMapper mapper, IRepositoryWrapper repoWrapper)
        {
            _clientFactory = clientFactory;
            _logger = logger;
            _mapper = mapper;
            _repoWrapper = repoWrapper;
        }

        public async Task<IActionResult> Index()
        {
            //Get Menu
            ViewBag.MenuMachine = await _repoWrapper.ProductCategory.GetLstMenuByParentId(654);
            ViewBag.MenuMaterials = await _repoWrapper.ProductCategory.GetLstMenuByParentId(652);
            ViewBag.MenuServices = await _repoWrapper.ProductCategory.GetLstMenuByParentId(651);
            ViewBag.ListCategory = await _repoWrapper.Service.GetProdCateByParentID(651);

            ViewBag.ListService = await _repoWrapper.Service.GetListService(10, 10, 4, 11);
            ViewBag.ListServiceCarriage = await _repoWrapper.Service.GetListServiceByCate(1, 10, 4, 11, 1818, null,null);
            ViewBag.ListServiceRepair = await _repoWrapper.Service.GetListServiceByCate(1, 10, 4, 11, 1817, null, null);
            ViewBag.ListServiceRescue = await _repoWrapper.Service.GetListServiceByCate(1, 10, 4, 11, 1819, null, null);
            ViewBag.MetaTags = await _repoWrapper.Setting.GetSetting();
            return View();
        }

        public async Task<IActionResult> ServiceByCate(int? page = 1, int? pageSize = 10)
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
                //Response.Headers("Location", "https://hanoma.vn/may-de-ban");
                //Response.End();
            }
            else
            {
                ViewBag.ListService = await _repoWrapper.Service.GetListServiceByCate((int)page, (int)pageSize, 4, 11, ProductCategory_ID, null, null);
                ViewBag.Page = page ?? 1;
                ViewBag.PageSize = pageSize ?? 10;
                ViewBag.TotalCount = ViewBag.ListService.TotalRecord;
                ViewBag.TotalPages = (int)Math.Ceiling(decimal.Divide(ViewBag.ListService.TotalRecord, ViewBag.PageSize ?? 10));
            }

            var data = await _repoWrapper.Service.GetListServiceByCate((int)page, (int)pageSize, 4, 11, ProductCategory_ID, null, null);
            ViewBag.ProductCategory_ID = ProductCategory_ID;
            var productCategoryDetail = await _repoWrapper.ProductCategory.GetDetailProductCategory(ProductCategory_ID);
            ViewBag.ProductCateName = productCategoryDetail.Name;
            ViewBag.ProductCateUrl = productCategoryDetail.URL;
            return View(data);
        }

        public async Task<ActionResult> GetPaggingService(int? ProductCategory_ID, int? Location_ID, int? RelatedCategoryId, int? page = 1, int? pageSize = 10)
        {
            ViewBag.Page = page ?? 1;
            ViewBag.PageSize = pageSize ?? 10;
            ViewBag.ProductCategory_ID = ProductCategory_ID;
            var productsRes = await _repoWrapper.Service.GetListServiceByCate((int)page, (int)pageSize, 4, 11, (int)ProductCategory_ID, Location_ID, RelatedCategoryId);
            var productCategoryDetail = await _repoWrapper.ProductCategory.GetDetailProductCategory((int)ProductCategory_ID);
            ViewBag.ProductCateName = productCategoryDetail.Name;
            ViewBag.TotalCount = productsRes.TotalRecord;
            ViewBag.TotalPages = (int)Math.Ceiling(decimal.Divide(productsRes.TotalRecord, pageSize ?? 10));
            ViewBag.ServicePaging =
                new StaticPagedList<ProductSearchResultDTO>
                (
                    productsRes.Data, page ?? 1, pageSize ?? 10, productsRes.TotalRecord
                    );
            return PartialView("ServicePaggingPartial", productsRes);
        }

        public async Task<IActionResult> LoadDataByCate(int ProductCategory_ID, int page, int pageSize = 10)
        {
            var dataResult = await _repoWrapper.Service.GetListServiceByCate(page, pageSize, 4, 11, ProductCategory_ID, null, null);

            int totalRow = dataResult.TotalRecord;
            return Json(new
            {
                data = dataResult,
                total = totalRow,
                status = true
            });
        }

        public async Task<IActionResult> ServiceDetail()
        {
            var url = RouteData.Values["url"];
            int Service_ID = Utils.RegexRouteIdFromUrl(url.ToString());
            if (Service_ID == 0) // Error Redirect 301
            {
                //Response.StatusCode = 301;
                //Response.Headers("Location", "https://hanoma.vn/may-de-ban");
                //Response.End();
            }
            ViewBag.MenuMachine = await _repoWrapper.ProductCategory.GetLstMenuByParentId(654);
            ViewBag.MenuMaterials = await _repoWrapper.ProductCategory.GetLstMenuByParentId(652);
            ViewBag.MenuServices = await _repoWrapper.ProductCategory.GetLstMenuByParentId(651);
            ViewBag.ListCategory = await _repoWrapper.Service.GetProdCateByParentID(651);

            ViewBag.MetaTags = await _repoWrapper.Setting.GetSetting();

            //Lấy ra ds dịch vụ được tài trợ và banner

            ProductDetailsDTO model = await _repoWrapper.Service.GetProductDetails(Service_ID);
            if (model.ErrorCode == "00" || model.ErrorCode == "0")
            {
                ViewBag.ListProductAds = await _repoWrapper.Service.GetSponsorBannerProduct((int)model.ProductDetails.ProductCategory_ID, 11, 5);
                ViewBag.ListProductByCate = await _repoWrapper.Service.GetListServiceByCate(1, 10, 4, 11, (int)model.ProductDetails.ProductCategory_ID, null, null);
                ViewBag.ListProductByProductBrand = await _repoWrapper.Service.GetListServiceByProductBrand(1, 10, 4, 11, (int)model.ProductDetails.ProductBrand_ID);
                var productCategoryDetail = await _repoWrapper.ProductCategory.GetDetailProductCategory((int)model.ProductDetails.ProductCategory_ID);
                ViewBag.ProductCateName = productCategoryDetail.Name;
                ViewBag.ProductCateUrl = productCategoryDetail.URL;
                return View(model);
            }
            else
            {
                return Redirect("/Error/404");
            }
        }

        //Filter DS CateID cấp 1  (ProductCategoryId)
        public async Task<JsonResult> GetLstProductCateLv1(string text)
        {
            var result = await _repoWrapper.Product.GetProdCateByParentID(text, 654);
            return Json(result.Data);
        }

        //Filter DS CateID cấp 2
        public async Task<JsonResult> GetLstProductCateLv2(string text, int? ProductCategoryId)
        {
            var result = await _repoWrapper.Product.GetProdCateByParentID(text, ProductCategoryId);
            return Json(result.Data);
        }

        public async Task<JsonResult> GetLstProductCateRe(string text, int? ProductCategoryId)
        {
            var result = await _repoWrapper.Product.GetProdCateByParentID(text, 656);
            return Json(result.Data);
        }

        //Chọn hãng sản xuất
        public async Task<JsonResult> GetLstProductCateLv3(string text, int? ProductCategoryId)
        {
            var result = await _repoWrapper.Product.GetProdCateByParentID(text, ProductCategoryId);
            return Json(result.Data);
        }

        public async Task<JsonResult> GetAllLocation(string text)
        {
            var result = await _repoWrapper.Profile.GetAllLocation(text);
            return Json(result.Data);
        }

        public async Task<JsonResult> GetAllManual(string text)
        {
            var result = await _repoWrapper.Manufacture.GetShopmanManuFactory();
            return Json(result);
        }

    }
}
