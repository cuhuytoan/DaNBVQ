using AutoMapper;
using HNM.DataNC.ModelsNC.ModelsDTO;
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
    public class LibraryCategoryController : ControllerBase, IDisposable
    {
        private readonly ILoggerManager _logger;
        private IRepositoryWrapper _repoWrapper;
        private IMapper _mapper;
        private readonly IDistributedCache _distributedCache;
        public LibraryCategoryController(IRepositoryWrapper repoWrapper, ILoggerManager logger, IMapper mapper, IDistributedCache distributedCache)
        {
            _repoWrapper = repoWrapper;
            _logger = logger;
            _mapper = mapper;
            _distributedCache = distributedCache;
        }
        /// <summary>
        /// Get List Library Category
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<LibraryCategoryDTO>> GetListLibraryCategory()
        {
            var output = new List<LibraryCategoryDTO>();
            try
            {
                var cacheKey = "LibraryCategory_GetListLibraryCategory";
                var redisEncode = await _distributedCache.GetStringAsync(cacheKey);
                if (redisEncode != null)
                {
                    output = JsonConvert.DeserializeObject<List<LibraryCategoryDTO>>(redisEncode);
                }
                else
                {
                    var result = await _repoWrapper.LibraryCategory.GetLibCategory();
                    output = _mapper.Map<List<LibraryCategoryDTO>>(result);
                    await _distributedCache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(output), Utils.Util.RedisOptions());
                }
            }
            catch(Exception ex)
            {
                _logger.LogError($"GetListLibraryCategory: " + ex.ToString());
            }
            return output;
        }
        /// <summary>
        /// Get library by Id
        /// </summary>
        /// <param name="LibraryCategoryId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<LibraryCategoryDTO> GetLibraryCategoryById(int?LibraryCategoryId)
        {
            var output = new LibraryCategoryDTO();
            try
            {
                var result = await _repoWrapper.LibraryCategory.GetLibCategoryById(LibraryCategoryId);
                output = _mapper.Map<LibraryCategoryDTO>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetLibraryCategoryById: " + ex.ToString());
            }
            return output;
        }

        /// <summary>
        /// Get library by URL
        /// </summary>
        /// <param name="Url"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<LibraryCategoryDTO> GetLibraryCategoryByUrl(string Url)
        {
            var output = new LibraryCategoryDTO();
            try
            {
                var result = await _repoWrapper.LibraryCategory.GetLibCategoryByUrl(Url);
                output = _mapper.Map<LibraryCategoryDTO>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetLibraryCategoryByUrl: " + ex.ToString());
            }
            return output;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}