using AutoMapper;
using HNM.DataNC.ModelsNC.ModelsDTO;
using HNM.DataNC.ModelsStore;
using HNM.RepositoriesNC.RepositoriesBase;
using HNM.WebApiNC.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HNM.WebApiNC.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class CareerCategoryController : ControllerBase, IDisposable
    {
        private readonly ILoggerManager _logger;
        private IRepositoryWrapper _repoWrapper;
        private IMapper _mapper;
        private readonly IDistributedCache _distributedCache;
        public CareerCategoryController(IRepositoryWrapper repoWrapper, ILoggerManager logger, IMapper mapper, IDistributedCache distributedCache)
        {
            _repoWrapper = repoWrapper;
            _logger = logger;
            _mapper = mapper;
            _distributedCache = distributedCache;
        }
        [HttpGet]
        public async Task<List<CareerCategoryDTO>> GetAllCareerCategory()
        {
            var output = new List<CareerCategoryDTO>();
            try
            {
                var cacheKey = "CareerCategory_GetAllCareerCategory";
                var redisEncode = await _distributedCache.GetStringAsync(cacheKey);
                if (redisEncode != null)
                {
                    output = JsonConvert.DeserializeObject<List<CareerCategoryDTO>>(redisEncode);
                }
                else
                {
                    var result = await _repoWrapper.CareerCategory.GetAllCareerCategory();
                    output = _mapper.Map<List<CareerCategoryDTO>>(result);
                    await _distributedCache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(output), Utils.Util.RedisOptions());
                }
            }
            catch(Exception ex)
            {
                _logger.LogError($"GetAllCareerCategory: " + ex.ToString());
            }
            return output;
        }
        [HttpGet]
        public async Task<CareerCategoryDTO> GetDetailCareerCategory(int CareerCategoryId)
        {
            var output = new CareerCategoryDTO();
            try
            {
                var cacheKey = $"CareerCategory_GetDetailCareerCategory{CareerCategoryId}";
                var redisEncode = await _distributedCache.GetStringAsync(cacheKey);
                if (redisEncode != null)
                {
                    output = JsonConvert.DeserializeObject<CareerCategoryDTO>(redisEncode);
                }
                else
                {
                    var result = await _repoWrapper.CareerCategory.SingleOrDefaultAsync(p => p.Id == CareerCategoryId);
                    output = _mapper.Map<CareerCategoryDTO>(result);
                    await _distributedCache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(output), Utils.Util.RedisOptions());
                }
            }
            catch(Exception ex)
            {
                _logger.LogError($"GetDetailCareerCategory: " + ex.ToString());
            }
            return output;
        }
        /// <summary>
        /// Get List CV Category in HomePage
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<GetCVCategory_Result>> GetListCVCategory()
        {
            var output = new List<GetCVCategory_Result>();
            try
            {
                var cacheKey = $"GetCVCategory_Result";
                var redisEncode = await _distributedCache.GetStringAsync(cacheKey);
                if (redisEncode != null)
                {
                    output = JsonConvert.DeserializeObject<List<GetCVCategory_Result>>(redisEncode);
                }
                else
                {
                    output =  await _repoWrapper.CareerCategory.GetCVCategory();                   
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetDetailCareerCategory: " + ex.ToString());
            }
            return output;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}