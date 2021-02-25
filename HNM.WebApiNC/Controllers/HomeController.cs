using AutoMapper;
using HNM.DataNC.ModelsNC.ModelsDTO;
using HNM.RepositoriesNC.RepositoriesBase;
using HNM.WebApiNC.Logging;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HNM.WebApiNC.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    //[Authorize]
    [ApiController]
    public class HomeController : ControllerBase, IDisposable
    {
        private readonly ILoggerManager _logger;
        private IRepositoryWrapper _repoWrapper;
        private IMapper _mapper;
        public HomeController(IRepositoryWrapper repoWrapper, ILoggerManager logger, IMapper mapper)
        {
            _repoWrapper = repoWrapper;
            _logger = logger;
            _mapper = mapper;
        }
        [HttpGet]
        private async Task<IEnumerable<AdvertisingDTO>> GetAdvertisingMarketMovement()
        {
            _logger.LogInfo("GetAdvertisingMarketMovement");
            var advertisingItems = await _repoWrapper.Advertising.GetAdvertisingsByBlockId(18);
            var result = _mapper.Map<IEnumerable<AdvertisingDTO>>(advertisingItems);
            return result;
        }
        [HttpGet]
        private async Task<IEnumerable<AdvertisingDTO>> GetCarouseResult()
        {
            _logger.LogInfo("GetCarouseResult");
            var advertisingItems = await _repoWrapper.Advertising.GetCarouselList();
            var result = _mapper.Map<IEnumerable<AdvertisingDTO>>(advertisingItems);
            return result;
        }
        [HttpGet]
        private async Task<IEnumerable<AdvertisingDTO>> GetAdHeaderSlider()
        {
            _logger.LogInfo("GetAdHeaderSlider");
            var advertisingItems = await _repoWrapper.Advertising.GetAdvertisingsByBlockId(13);
            var result = _mapper.Map<IEnumerable<AdvertisingDTO>>(advertisingItems);
            return result;
        }
        [HttpGet]
        private async Task<IEnumerable<AdvertisingDTO>> GetAdvertisingBrandBlock10()
        {
            _logger.LogInfo("GetAdvertisingBrand");
            var advertisingItems = await _repoWrapper.Advertising.GetAdvertisingsByBlockId(10);
            var result = _mapper.Map<IEnumerable<AdvertisingDTO>>(advertisingItems);
            return result;
        }
        [HttpGet]
        private async Task<IEnumerable<AdvertisingDTO>> GetAdvertisingBrandBlock11()
        {
            _logger.LogInfo("GetAdvertisingBrand");
            var advertisingItems = await _repoWrapper.Advertising.GetAdvertisingsByBlockId(11);
            var result = _mapper.Map<IEnumerable<AdvertisingDTO>>(advertisingItems);
            return result;
        }

        /// <summary>
        /// Get Slide all Product slide block in HomePage
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        private async Task<IEnumerable<HomeProductSlide_ResultDTO>> GetHomeProductSlide()
        {
            _logger.LogInfo("GetAdvertisingBrand");
            var result = await _repoWrapper.Product.GetHomeProductSlide("15,16,17,19");
            return _mapper.Map<IEnumerable<HomeProductSlide_ResultDTO>>(result);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }

}