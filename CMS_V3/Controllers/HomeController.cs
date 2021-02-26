using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CMS_V3.Models;
using CMS_V3.Helper;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using HNM.DataNC.ModelsNC.ModelsDTO;
using System.Text.Json;
using Newtonsoft.Json;
using HNM.DataNC.ModelsFilter;
using HNM.RepositoriesWeb.RepositoriesBase;
using HNM.RepositoriesWeb.Repositories;
using CMS_V3.ViewModel;
using HNM.CommonNC;
using Microsoft.Extensions.Caching.Distributed;

namespace CMS_V3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IDistributedCache _distributedCache;
        public HomeController(ILogger<HomeController> logger, IHttpClientFactory clientFactory, IRepositoryWrapper repoWrapper, IDistributedCache distributedCache)
        {
            _clientFactory = clientFactory;
            _logger = logger;
            _repoWrapper = repoWrapper;
            _distributedCache = distributedCache;
        }
        
        public async Task<IActionResult> Index()
        {
            //Get Home Carousel
            await GetHomeCarousel();
            //Get Menu
            ViewBag.ProductCategory = await GetAllProductCategory();
            return View();
        }

        public async Task<ActionResult> GetHomeProductCategory(string BlockName,string Url , int ProductCategoryId)
        {
            ViewBag.MenuMachine = await GetLstMenuByParentId(ProductCategoryId);
            var jsonResult = await GetHomeBlockProduct(1, ProductCategoryId);
            ViewBag.BlockName = BlockName;
            ViewBag.Url = Url;
            return PartialView("HomeSellProduct", jsonResult);
        }
 

        public async Task<ActionResult> GetHomeLibrary()
        {
            ArticleViewModel articleViewModel = new ArticleViewModel();
            //lấy ra tin nổi bật đầu trang
            articleViewModel.highlightArticle = await _repoWrapper.Article.GetHomeHighLightsArticle();
            articleViewModel.lstBlock1 = await _repoWrapper.Article.GetHomeVerticleBlockOneArticle();
            articleViewModel.lstBlock2 = await _repoWrapper.Article.GetHomeVerticleBlockTwoArticle();
            ViewBag.ListArticle = articleViewModel;
            return PartialView("HomeLibrary", articleViewModel);
        }
        /// <summary>
        /// Get Home Carousel
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> GetHomeCarousel()
        {
            ListAdvertisingCarouselDTO lstCarousel = new ListAdvertisingCarouselDTO();
            try
            {
                var cacheKey = $"GetHomeCarousel";
                var redisEncode = await _distributedCache.GetStringAsync(cacheKey);
                if (redisEncode != null)
                {
                    lstCarousel = JsonConvert.DeserializeObject<ListAdvertisingCarouselDTO>(redisEncode);
                }
                else
                {
                    lstCarousel = await _repoWrapper.Advertising.GetHomeCarousel();
                    await _distributedCache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(lstCarousel), CommonHelper.RedisOptions());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"HomeController- GetHomeCarousel {ex.ToString()}");
            }

            return PartialView("HomeBanner", lstCarousel);
        }

        public async Task<ListProductCategoryDTO> GetLstMenuByParentId(int parentId)
        {
            ListProductCategoryDTO output = new ListProductCategoryDTO();
            try
            {
                var cacheKey = $"GetLstMenuByParentId-{parentId}";
                var redisEncode = await _distributedCache.GetStringAsync(cacheKey);
                if (redisEncode != null)
                {
                    output = JsonConvert.DeserializeObject<ListProductCategoryDTO>(redisEncode);
                }
                else
                {
                    output = await _repoWrapper.ProductCategory.GetLstMenuByParentId(parentId);
                    await _distributedCache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(output), CommonHelper.RedisOptions());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"HomeController- GetLstMenuByParentId- {parentId} {ex.ToString()}");
            }

            return output;
        }

        public async Task<List<ListAllProductCategoryDTO>> GetAllProductCategory()
        {
            List<ListAllProductCategoryDTO> output = new List<ListAllProductCategoryDTO>();
            try
            {                
               
                 output = await _repoWrapper.ProductCategory.GetAllProductCategory();
            }
            catch (Exception ex)
            {
                _logger.LogError($"HomeController- GetAllProductCategory {ex.ToString()}");
            }

            return output;
        }
        /// <summary>
        /// Get HomeBock Product
        /// </summary>
        /// <param name="productTypeId"></param>
        /// <param name="productCategoryId"></param>
        /// <returns></returns>
        public async Task<ProductPaggingDTO> GetHomeBlockProduct(int productTypeId, int productCategoryId)
        {
            ProductPaggingDTO output = new ProductPaggingDTO();
            ProductFilter parameter = new ProductFilter();
            parameter.Page = 1;
            parameter.PageSize = 10;
            parameter.StatusTypeId = 4;
            parameter.ProductTypeId = productTypeId;
            parameter.ProductCategoryId = productCategoryId;
            try
            {
                var cacheKey = $"GetHomeBlockProduct-{productTypeId}-{productCategoryId}";
                var redisEncode = await _distributedCache.GetStringAsync(cacheKey);
                if (redisEncode != null)
                {
                    output = JsonConvert.DeserializeObject<ProductPaggingDTO>(redisEncode);
                }
                else
                {
                    output = await _repoWrapper.Product.GetListProductPagging(parameter);
                    await _distributedCache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(output), CommonHelper.RedisOptions());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"HomeController- GetHomeBlockProduct {ex.ToString()}");
            }

            return output;
        }
        public async Task<IActionResult> Subscription(string email)
        {
            string msg = "";
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(email.Trim()))
            {
                msg = "Định dạng email không hợp lệ!";
            }
            else
            if (!Utils.IsEmail(email))
            {
                msg = "Định dạng email không hợp lệ!";
            }
            else
            {

                var SubItems = await _repoWrapper.Subscription.GetByEmail(email.Trim());

                if (SubItems == null)
                {
                    await _repoWrapper.Subscription.SubscriptionEmail(email);
                    msg = "Cảm ơn bạn đã đăng ký nhận tin từ HANOMA.VN!";

                }
                else
                {
                    if (SubItems.Subscription_ID == 0)
                        msg = "Đăng ký nhận tin chưa thành công";
                    else
                        msg = "Email đã được đăng ký nhận tin từ HANOMA.VN!";
                }


            }
            return Ok(msg);
        }

        public async Task<IActionResult> Notification()
        {
            var result = await _repoWrapper.Profile.GetListMsgByUser(3, 0);
            return PartialView("NotificationHeader", result);
        }

        public async Task<JsonResult> CountNotification()
        {
            var result = await _repoWrapper.Profile.CountFCMHasRead();
            return Json(result.NumberUnread.TotalUnRead);
        }
    }
}
