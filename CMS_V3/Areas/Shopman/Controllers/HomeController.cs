using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using HNM.DataNC.ModelsFilter;
using HNM.DataNC.ModelsNC.ModelsDTO;
using HNM.RepositoriesWeb.RepositoriesBase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using X.PagedList;

namespace CMS_V3.Areas.Shopman.Controllers
{
    [Area("Shopman")]
    [Authorize(policy: "Authenticated")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IRepositoryWrapper _repoWrapper;
        private IMapper _mapper;
        private IDistributedCache _cache;
        public HomeController(ILogger<HomeController> logger, IHttpClientFactory clientFactory, IMapper mapper, IRepositoryWrapper repoWrapper, IDistributedCache cache)
        {
            _clientFactory = clientFactory;
            _logger = logger;
            _mapper = mapper;
            _repoWrapper = repoWrapper;
            _cache = cache;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.ListProduct = await _repoWrapper.ShopmanProduct.GetLstProductShopMan(null, 1, 5, -1, null, null);
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> EditProductBrand()
        {
            var productbrand = await _repoWrapper.Profile.GetBrandDetailByUserId();
            //Cần lấy 
            var model = await _repoWrapper.Profile.GetBrandDetails(productbrand.ProductBrandId);
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> EditProductBrand([FromBody] PostProductBrandDTO model)
        {
            //logo
            if (model.ImgLogo != null)
            {
                String[] substrings = model.ImgLogo.Base64.Split(',');
                model.ImgLogo.Base64 = substrings[1];
            }


            //banner
            if (model.ImgBanner != null)
            {
                String[] substrings = model.ImgBanner.Base64.Split(',');
                model.ImgBanner.Base64 = substrings[1];
            }

            var result = await _repoWrapper.Profile.PutProductBrand(model);
            return Json(result);
        }


        public async Task<JsonResult> GetLstCountry(string text)
        {
            var result = await _repoWrapper.Profile.GetLstCountry(text);
            return Json(result.data);
        }

        public async Task<JsonResult> GetLocationByCountry(int? CountryId, string text)
        {
            var result = await _repoWrapper.Profile.GetLocationByCountry(CountryId, text);
            return Json(result.Data);
        }

        public async Task<JsonResult> GetDistrictByLocation(int? LocationId, string text)
        {
            var result = await _repoWrapper.Profile.GetDistrictByLocation(LocationId, text);
            return Json(result.Data);
        }
    }

}
