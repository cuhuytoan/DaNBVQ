using AutoMapper;
using HNM.DataNC.Models;
using HNM.DataNC.ModelsNC.ModelsDTO;
using HNM.DataNC.ModelsStore;
using HNM.RepositoriesNC.RepositoriesBase;
using HNM.WebApiNC.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HNM.WebApiNC.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class AdvertisingController : ControllerBase, IDisposable
    {
        private readonly ILoggerManager _logger;
        private IRepositoryWrapper _repoWrapper;
        private IMapper _mapper;
        private readonly IDistributedCache _distributedCache;
        private readonly IConfiguration _configuration;
        public AdvertisingController(IRepositoryWrapper repoWrapper, ILoggerManager logger, IMapper mapper, IDistributedCache distributedCache, IConfiguration configuration)
        {
            _repoWrapper = repoWrapper;
            _logger = logger;
            _mapper = mapper;
            _distributedCache = distributedCache;
            _configuration = configuration;
        }
        /// <summary>
        /// GetCarousel Homepage
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ListAdvertisingCarouselDTO> GetCarouselList()
        {
            var output = new ListAdvertisingCarouselDTO();
            //var cacheKey = "Advertising_GetCarouselList";
            //var redisEncode = await _distributedCache.GetStringAsync(cacheKey);
            //if (redisEncode != null)
            //{
            //    output = JsonConvert.DeserializeObject<ListAdvertisingCarouselDTO>(redisEncode);
            //}
            //else
            //{
                var result = await _repoWrapper.Advertising.GetCarouselList();
                output.Data = _mapper.Map<IEnumerable<AdvertisingCarouselDTO>>(result);
            //    await _distributedCache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(output), Utils.Util.RedisOptions());
            //}
            return output;
        }
       
        /// <summary>
        /// Get tất cả quảng cáo các khổi banner
        /// 20 -- banner
        /// 21 -- tin tức
        /// 24 -- Home top 1
        /// 25 -- Home top 2
        /// AdMobileType = 0 --> Display Web View ...AdMobileType = 1 --> Display In app ...
        /// FormID
        /// 0 -- Máy để bán
        /// 1 -- Máy cho thuê
        /// 2 -- Việc làm
        /// 3 -- Hồ sơ
        /// 4 -- Tin tức
        /// 5 -- Video
        /// ParamterID Id của việc làm sản phẩm tương ứng
        /// 
        /// </summary>
        /// <param name="lstBlockId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ListAdvertisingMobileDTO> GetAllAdBanner()
        {
            _logger.LogInfo("GetAllAdBanner");
            var output = new ListAdvertisingMobileDTO();
            var cacheKey = "Advertising_GetAllAdBanner";
            var redisEncode = await _distributedCache.GetStringAsync(cacheKey);
            if (redisEncode != null)
            {
                output = JsonConvert.DeserializeObject<ListAdvertisingMobileDTO>(redisEncode);
            }
            else
            {
                var advertisingItems = await _repoWrapper.Advertising.GetAdvertisingsByListBlockId("20,21,24,25");
                output.Data = _mapper.Map<IEnumerable<AdvertisingMobileDTO>>(advertisingItems);
                await _distributedCache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(output), Utils.Util.RedisOptions());
            }

            return output;
        }
        /// <summary>
        /// Get Quảng cáo sản phẩm đề xuất khối tin tức
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ListAdvertisingMobileDTO> GetAdProductInNews()
        {
            var output = new ListAdvertisingMobileDTO();
            var cacheKey = "Advertising_GetAdProductInNews";
            var redisEncode = await _distributedCache.GetStringAsync(cacheKey);
            if (redisEncode != null)
            {
                output = JsonConvert.DeserializeObject<ListAdvertisingMobileDTO>(redisEncode);
            }
            else
            {
                var advertisingItems = await _repoWrapper.Advertising.GetAdvertisingsByBlockId(22);
                output.Data = _mapper.Map<IEnumerable<AdvertisingMobileDTO>>(advertisingItems);
                await _distributedCache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(output), Utils.Util.RedisOptions());
            }

            return output;
        }
        /// <summary>
        /// Get Quảng cáo sản phẩm đề xuất khối video
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ListAdvertisingMobileDTO> GetAdProductIVideo()
        {
            _logger.LogInfo("GetAdBannerVideo");
            var output = new ListAdvertisingMobileDTO();
            var cacheKey = "Advertising_GetAdProductIVideo";
            var redisEncode = await _distributedCache.GetStringAsync(cacheKey);
            if (redisEncode != null)
            {
                output = JsonConvert.DeserializeObject<ListAdvertisingMobileDTO>(redisEncode);
            }
            else
            {
                var advertisingItems = await _repoWrapper.Advertising.GetAdvertisingsByBlockId(23);
                output.Data = _mapper.Map<IEnumerable<AdvertisingMobileDTO>>(advertisingItems);
                await _distributedCache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(output), Utils.Util.RedisOptions());
            }

            return output;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ProductCategoryId"></param>
        /// <param name="ProductTypeId"></param>
        /// <param name="Top"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<SponsorBanner> GetSponsorBannerProduct(int ProductCategoryId, int ProductTypeId, int? Top )
        {
            var output = new SponsorBanner();
            
            //Data Sponsor
            var lstSponsor = await _repoWrapper.Advertising.GetSponsorProduct(ProductCategoryId, ProductTypeId, Top ?? 10);
            output.DataSponsor = _mapper.Map<List<ProductSearchResultDTO>>(lstSponsor);


            //Data Banner
            int ParentCateId = -1;
            var productCate = await _repoWrapper.ProductCategory.FirstOrDefaultAsync(p => p.ProductCategory_ID == ProductCategoryId);
            if (productCate != null)
            {
                ParentCateId = productCate.Parent_ID ?? -1;
            }

            var lstBanner = await _repoWrapper.Advertising.GetBannerAdvertisingInChildPage(ProductTypeId, ParentCateId, ProductCategoryId, 12);
            if (lstBanner == null || lstBanner.Count == 0) // If null get default
            {
                lstBanner = await _repoWrapper.Advertising.GetBannerAdvertisingInChildPage(-1, -1, -1, 12);
            }
            output.DataBanner = _mapper.Map<List<AdvertisingCarouselDTO>>(lstBanner);
            return output;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<QuoteAds>> GetListQuoteAds()
        {
            return await _repoWrapper.Advertising.GetLstQuoteAds();
        }

        [HttpPost]
        [Authorize]
        public async Task<ContactAdsDTO> PostContactAds(ContactAdsDTO model)
        {
            var output = new ContactAdsDTO();
            output = model;
            var result = await _repoWrapper.Advertising.PostContactAds(model);
            if(result)
            {
                //Sent Email
                Utils.Util.SendMail("Quảng cáo Hanoma", "quangcaohanoma@gmail.com", "Quảng cáo hanoma", "Đăng ký quảng cáo hanoma",
                    $"{model.Name} - {model.Mobile} - {model.Address}  " + Environment.NewLine + $"{model.Content}", _repoWrapper.AspNetUsers.setting());
                output.ErrorCode = "00";
                output.Message = "Yêu cầu quảng cáo của bạn đã gửi tới Hanoma, chúng tôi sẽ sớm liên hệ với bạn!";
            }
            else
            {
                output.ErrorCode = "01";
                output.Message = "Có lỗi trong quá trình xử lý";
            }
            return output;
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}