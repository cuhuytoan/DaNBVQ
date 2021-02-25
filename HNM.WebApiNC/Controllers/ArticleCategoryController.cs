using AutoMapper;
using HNM.DataNC.ModelsNC.ModelsDTO;
using HNM.RepositoriesNC.RepositoriesBase;
using HNM.WebApiNC.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HNM.WebApiNC.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class ArticleCategoryController : ControllerBase, IDisposable
    {
        private readonly ILoggerManager _logger;
        private IRepositoryWrapper _repoWrapper;
        private IMapper _mapper;
        private readonly IDistributedCache _distributedCache;
        public ArticleCategoryController(IRepositoryWrapper repoWrapper, ILoggerManager logger, IMapper mapper, IDistributedCache distributedCache)
        {
            _repoWrapper = repoWrapper;
            _logger = logger;
            _mapper = mapper;
            _distributedCache = distributedCache;
        }
        /// <summary>
        /// Get All Article
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<ArticleCategoryDTO>> GetListArticle()
        {
            var output = new List<ArticleCategoryDTO>();
            var cacheKey = "ArticleCategory_GetListArticle";
            var redisEncode = await _distributedCache.GetStringAsync(cacheKey);
            if (redisEncode != null)
            {
                output = JsonConvert.DeserializeObject<List<ArticleCategoryDTO>>(redisEncode);
            }
            else
            {
                var result = await _repoWrapper.ArticleCategory.GetAllArticle();
                output = _mapper.Map<List<ArticleCategoryDTO>>(result);
                await _distributedCache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(output), Utils.Util.RedisOptions());

            }

            return output;
        }
        /// <summary>
        /// Get Article By ArticleID
        /// </summary>
        /// <param name="ArticleCategoryId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<ArticleCategoryDTO>> GetListArticleByID(int? ArticleCategoryId)
        {
            var output = new List<ArticleCategoryDTO>();
            var cacheKey = $"ArticleCategory_GetListArticleByID{ArticleCategoryId}";
            var redisEncode = await _distributedCache.GetStringAsync(cacheKey);
            if (redisEncode != null)
            {
                output = JsonConvert.DeserializeObject<List<ArticleCategoryDTO>>(redisEncode);
            }
            else
            {
                var result = await _repoWrapper.ArticleCategory.FindByCondition(p => p.ArticleCategory_ID == ArticleCategoryId).ToListAsync();
                output = _mapper.Map<List<ArticleCategoryDTO>>(result);
                await _distributedCache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(output), Utils.Util.RedisOptions());
            }
            return output;
        }
        [HttpGet]
        public async Task<List<VideoCategoryDTO>> GetListVideoCategory()
        {
            var output = new List<VideoCategoryDTO>();
            var result = await _repoWrapper.ArticleCategory.GetVideoCategory();
            output = _mapper.Map<List<VideoCategoryDTO>>(result);
            return output;
        }

        /// <summary>
        /// Get ArticleCategory by URL
        /// </summary>
        /// <param name="Url"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ArticleCategoryDTO> GetArticleCategoryByUrl(string Url)
        {
            var output = new ArticleCategoryDTO();
            try
            {
                var result = await _repoWrapper.ArticleCategory.GetArtCategoryByUrl(Url);
                output = _mapper.Map<ArticleCategoryDTO>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetArticleCategoryByUrl: " + ex.ToString());
            }
            return output;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }

}