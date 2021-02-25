using AutoMapper;
using HNM.DataNC.Models;
using HNM.DataNC.ModelsNC.ModelsDTO;
using HNM.DataNC.ModelsStore;
using HNM.RepositoriesNC.RepositoriesBase;
using HNM.WebApiNC.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace HNM.WebApiNC.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class ArticleController : ControllerBase, IDisposable
    {
        private readonly ILoggerManager _logger;
        private IRepositoryWrapper _repoWrapper;
        private IMapper _mapper;
        private readonly IDistributedCache _distributedCache;
        public ArticleController(IRepositoryWrapper repoWrapper, ILoggerManager logger, IMapper mapper, IDistributedCache distributedCache)
        {
            _repoWrapper = repoWrapper;
            _logger = logger;
            _mapper = mapper;
            _distributedCache = distributedCache;
        }
        /// <summary>
        /// Get List Related Article
        /// </summary>
        /// <param name="ArticleCategoryId"></param>
        /// <param name="ArticleId"></param>
        /// <param name="ArticleTypeId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<ArticleShortInfo>> GetRelateArticle(int? ArticleCategoryId, int ArticleId,
            int ArticleTypeId)
        {
            var output = new List<ArticleShortInfo>();
            try
            {
                var cacheKey = $"Article_GetRelateArticle{ArticleCategoryId}{ArticleId}{ArticleTypeId}";
                var redisEncode = await _distributedCache.GetStringAsync(cacheKey);
                if (redisEncode != null)
                {
                    output = JsonConvert.DeserializeObject<List<ArticleShortInfo>>(redisEncode);
                }
                else
                {
                    var result = await _repoWrapper.Article.GetRelateArticle(ArticleCategoryId, ArticleId, ArticleTypeId);
                    output = _mapper.Map<List<ArticleShortInfo>>(result);
                    await _distributedCache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(output), Utils.Util.RedisOptions());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetRelateArticle:" + ex.ToString());
            }

            return output;
        }

        /// <summary>
        /// Get List Related Video
        /// </summary>
        /// <param name="VideoCategoryId"></param>
        /// <param name="ArticleId"></param>
        /// <param name="ArticleTypeId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<ArticleShortInfo>> GetRelateVideo(int? VideoCategoryId, int ArticleId,
            int ArticleTypeId, int page, int pageSize)
        {
            var output = new List<ArticleShortInfo>();

            var result = await _repoWrapper.Article.GetRelateVideo(VideoCategoryId, ArticleId, ArticleTypeId, page, pageSize);
            output = _mapper.Map<List<ArticleShortInfo>>(result);

            return output;
        }

        /// <summary>
        /// Get List Article By Category
        /// </summary>
        /// <param name="ArticleCategoryId"></param>
        /// <param name="ArticleTypeId"></param>
        /// <param name="page"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<ArticleShortInfo>> GetArtByCate(int? ArticleCategoryId, int ArticleTypeId, int? page, int
            PageSize)
        {
            var output = new List<ArticleShortInfo>();
            try
            {
                var cacheKey = $"Article_GetArtByCate{ArticleCategoryId}{ArticleTypeId}{page}{PageSize}";
                var redisEncode = await _distributedCache.GetStringAsync(cacheKey);
                if (redisEncode != null)
                {
                    output = JsonConvert.DeserializeObject<List<ArticleShortInfo>>(redisEncode);
                }
                else
                {
                    var result = await _repoWrapper.Article.GetArtByCate(ArticleCategoryId, ArticleTypeId, page, PageSize);
                    output = _mapper.Map<List<ArticleShortInfo>>(result);
                    await _distributedCache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(output), Utils.Util.RedisOptions());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetArtByCate:" + ex.ToString());
            }
            return output;
        }

        /// <summary>
        /// Get ListVideo
        /// </summary>
        /// <param name="VideoCategoryId"></param>
        /// <param name="ArticleTypeId"></param>
        /// <param name="page"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<Video_Search_Mobile_Result>> GetVideoByCate(int? VideoCategoryId, int ArticleTypeId, int? page, int  PageSize)
        {
            var output = new List<Video_Search_Mobile_Result>();
            try
            {
                var result = await _repoWrapper.Article.GetVideoByCate(VideoCategoryId, ArticleTypeId, page, PageSize);
                output = _mapper.Map<List<Video_Search_Mobile_Result>>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetVideoByCate:" + ex.ToString());
            }
            return output;
        }

        /// <summary>
        /// Get List Article By Category
        /// </summary>
        /// <param name="page"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<VideoShortInfo>> GetListVideo(int? page, int
            PageSize)
        {
            var output = new List<VideoShortInfo>();
            try
            {
                var cacheKey = $"Article_GetListVideo{page}{PageSize}";
                var redisEncode = await _distributedCache.GetStringAsync(cacheKey);
                if (redisEncode != null)
                {
                    output = JsonConvert.DeserializeObject<List<VideoShortInfo>>(redisEncode);
                }
                else
                {
                    var result = await _repoWrapper.Article.GetArtByCate(null, 2, page, PageSize);
                    output = _mapper.Map<List<VideoShortInfo>>(result);
                    await _distributedCache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(output), Utils.Util.RedisOptions());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetArtByCate:" + ex.ToString());
            }
            return output;
        }
        /// <summary>
        /// Get Detail Article
        /// </summary>
        /// <param name="ArticleId"></param>
        /// <param name="ArticleTypeId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ArticleDTO> GetArticleDetail(int ArticleId, int ArticleTypeId)
        {
            var output = new ArticleDTO();
            try
            {
                var cacheKey = $"Article_GetArticleDetail_{ArticleId}_{ArticleTypeId}";
                var redisEncode = await _distributedCache.GetStringAsync(cacheKey);
                if (redisEncode != null)
                {
                    output = JsonConvert.DeserializeObject<ArticleDTO>(redisEncode);
                }
                else
                {
                    var result = _repoWrapper.Article.GetArticleDetail(ArticleId, ArticleTypeId);
                    output = _mapper.Map<ArticleDTO>(result);
                    output.Content = HttpUtility.HtmlDecode(output.Content);
                    await _distributedCache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(output), Utils.Util.RedisOptions());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetArticleDetail: " + ex.ToString());
            }
            return output;
        }
        /// <summary>
        /// Get Ariticle Voice by ID
        /// </summary>
        /// <param name="ArticleId"></param>
        /// <param name="VoiceId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ArticleVoiceDTO> GetArticleVoice(int ArticleId, int VoiceId)
        {
            var output = new ArticleVoiceDTO();
            try
            {
                var cacheKey = $"Article_GetArticleVoice_{ArticleId}_{VoiceId}";
                var redisEncode = await _distributedCache.GetStringAsync(cacheKey);
                if (redisEncode != null)
                {
                    output = JsonConvert.DeserializeObject<ArticleVoiceDTO>(redisEncode);
                }
                else
                {
                    var result = _repoWrapper.Article.GetArticleVoice(ArticleId, 1);
                    output = _mapper.Map<ArticleVoiceDTO>(result);
                    await _distributedCache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(output), Utils.Util.RedisOptions());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetArticleVoice: " + ex.ToString());
            }
            return output;
        }
        /// <summary>
        /// Search Article
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="ArticleCategoryId"></param>
        /// <param name="ArticleTypeId"></param>
        /// <param name="page"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ArticleSearchDTO> SearchArticle(string keyword, int? ArticleCategoryId,
            int ArticleTypeId, int? page, int? PageSize)
        {
            keyword = HttpUtility.UrlDecode(keyword);
            var output = new ArticleSearchDTO();
            try
            {
                HttpUtility.HtmlDecode(keyword);
                output = await _repoWrapper.Article.SearchArticle(keyword, ArticleCategoryId, ArticleTypeId, page, PageSize);
            }
            catch (Exception ex)
            {
                _logger.LogError($"SearchArticle: " + ex.ToString());
            }
            return output;
        }
        /// <summary>
        /// Get Verticle Block one
        /// </summary>
        /// <param name="top"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<ArticleDTO>> GetHomeVerticleBlockOneArticlePartialView(int top)
        {
            var output = new List<ArticleDTO>();
            try
            {
                var cacheKey = $"Article_GetHomeVerticleBlockOneArticlePartialView{top}";
                var redisEncode = await _distributedCache.GetStringAsync(cacheKey);
                if (redisEncode != null)
                {
                    output = JsonConvert.DeserializeObject<List<ArticleDTO>>(redisEncode);
                }
                else
                {
                    var result = await _repoWrapper.Article.GetHomeVerticleBlockOneArticlePartialView(top);
                    output = _mapper.Map<List<ArticleDTO>>(result);
                    await _distributedCache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(output), Utils.Util.RedisOptions());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetHomeVerticleBlockOneArticlePartialView: " + ex.ToString());
            }
            return output;
        }
        /// <summary>
        /// Get Vertical Block two
        /// </summary>
        /// <param name="top"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<ArticleDTO>> GetHomeVerticleBlockTwoArticlePartialView(int top)
        {
            var output = new List<ArticleDTO>();
            try
            {
                var cacheKey = $"Article_GetHomeVerticleBlockTwoArticlePartialView{top}";
                var redisEncode = await _distributedCache.GetStringAsync(cacheKey);
                if (redisEncode != null)
                {
                    output = JsonConvert.DeserializeObject<List<ArticleDTO>>(redisEncode);
                }
                else
                {
                    var result = await _repoWrapper.Article.GetHomeVerticleBlockTwoArticlePartialView(top);
                    output = _mapper.Map<List<ArticleDTO>>(result);
                    await _distributedCache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(output), Utils.Util.RedisOptions());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetHomeVerticleBlockTwoArticlePartialView: " + ex.ToString());
            }
            return output;
        }
        /// <summary>
        /// Get Home Hightlight 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ArticleDTO> GetHomeHighLightsArticlePartialView()
        {
            var output = new ArticleDTO();
            try
            {
                var cacheKey = "Article_GetHomeHighLightsArticlePartialView";
                var redisEncode = await _distributedCache.GetStringAsync(cacheKey);
                if (redisEncode != null)
                {
                    output = JsonConvert.DeserializeObject<ArticleDTO>(redisEncode);
                }
                else
                {
                    var result = await _repoWrapper.Article.GetHomeHighLightsArticlePartialView();
                    output = _mapper.Map<ArticleDTO>(result);
                    await _distributedCache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(output), Utils.Util.RedisOptions());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetHomeHighLightsArticlePartialView: ");
            }
            return output;
        }
        /// <summary>
        /// Get Home Article By Category
        /// </summary>
        /// <param name="articleCategoryId"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<ArticleDTO>> GetHomeArticleCategoryPartialView(int articleCategoryId, int top)
        {
            var output = new List<ArticleDTO>();
            try
            {
                var cacheKey = $"Article_GetHomeArticleCategoryPartialView{articleCategoryId}{top}";
                var redisEncode = await _distributedCache.GetStringAsync(cacheKey);
                if (redisEncode != null)
                {
                    output = JsonConvert.DeserializeObject<List<ArticleDTO>>(redisEncode);
                }
                else
                {
                    var result = await _repoWrapper.Article.GetHomeArticleCategoryPartialView(articleCategoryId, top);
                    output = _mapper.Map<List<ArticleDTO>>(result);
                    await _distributedCache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(output), Utils.Util.RedisOptions());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetHomeArticleCategoryPartialView:" + ex.ToString());
            }
            return output;
        }
        [HttpGet]
        public async Task<HomeArticleDTO> GetHomeArticle()
        {
            var output = new HomeArticleDTO();
            try
            {
                var cacheKey = $"GetHomeArticle";
                var redisEncode = await _distributedCache.GetStringAsync(cacheKey);
                if (redisEncode != null)
                {
                    output = JsonConvert.DeserializeObject<HomeArticleDTO>(redisEncode);
                }
                else
                {
                    //Get Hightlight Article
                    var HightLightArt = await _repoWrapper.Article.GetHomeHighLightsArticlePartialView();
                    output.ArticleHightLight = _mapper.Map<ArticleShortInfo>(HightLightArt);
                    //Get All ArticleCategory
                    var lstArticleCategory = await _repoWrapper.ArticleCategory.GetAllArticle();
                    output.ArticleCategory = _mapper.Map<List<ArticleCategoryDTO>>(lstArticleCategory);
                    //Get All Articleby Cate            
                    var lstArt = new List<HomeArticleByCateDTO>();
                    var result = await _repoWrapper.Article.GetArtByCateHome("");
                    if (result.Count > 0)
                    {
                        foreach (var p in lstArticleCategory)
                        {
                            var item = new HomeArticleByCateDTO();
                            item.ArticleCategoryId = p.ArticleCategory_ID;
                            item.ArticleCategoryName = p.Name;
                            var dataShort = result.Where(x => x.ArticleCategoryId == p.ArticleCategory_ID).ToList();
                            item.Data = _mapper.Map<List<ArticleShortInfo>>(dataShort);
                            lstArt.Add(item);
                        }
                    }

                    output.ListArticleByCate = lstArt;
                    await _distributedCache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(output), Utils.Util.RedisOptions());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetHomeArticleCategoryPartialView:" + ex.ToString());
            }
            return output;

        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}