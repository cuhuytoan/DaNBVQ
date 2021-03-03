using AutoMapper;
using HNM.DataNC.ModelsNC.ModelsDTO;
using HNM.RepositoriesNC.RepositoriesBase;
using HNM.WebApiNC.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HNM.WebApiNC.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    //    [Authorize]
    [ApiController]
    public class ProductCategoryController : ControllerBase, IDisposable
    {
        private readonly ILoggerManager _logger;
        private IRepositoryWrapper _repoWrapper;
        private IMapper _mapper;
        private readonly IDistributedCache _distributedCache;
        public ProductCategoryController(IRepositoryWrapper repoWrapper, ILoggerManager logger, IMapper mapper, IDistributedCache distributedCache)
        {
            _repoWrapper = repoWrapper;
            _logger = logger;
            _mapper = mapper;
            _distributedCache = distributedCache;
        }
        [HttpGet]
        public async Task<ProductCategoryDTO> GetDetailProductCategory(int ProductCategoryId)
        {
            var output = new ProductCategoryDTO();
            //var cacheKey = $"ProductCategory_GetDetailProductCategory{ProductCategoryId}";
            //var redisEncode = await _distributedCache.GetStringAsync(cacheKey);
            //if (redisEncode != null)
            //{
            //    output = JsonConvert.DeserializeObject<ProductCategoryDTO>(redisEncode);
            //}
            //else
            //{
                var result = await _repoWrapper.ProductCategory.SingleOrDefaultAsync(p => p.ProductCategory_ID == ProductCategoryId);
                output = _mapper.Map<ProductCategoryDTO>(result);
            //    await _distributedCache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(output), Utils.Util.RedisOptions());
            //}

            return output;
        }
        [HttpGet]
        public async Task<ListProductCategoryDTO> GetProdCateByParentID(int? parentId,string keyword = "")
        {
            var output = new ListProductCategoryDTO();
            //var cacheKey = $"ProductCategory_GetProdCateByParentID{keyword}{parentId}";
            //var redisEncode = await _distributedCache.GetStringAsync(cacheKey);
            //if (redisEncode != null)
            //{
            //    output = JsonConvert.DeserializeObject<ListProductCategoryDTO>(redisEncode);
            //}
            //else
            //{                
                var result = await _repoWrapper.ProductCategory.GetTopCateByParentId(keyword,parentId);
                output.Data = _mapper.Map<IEnumerable<ProductCategoryDTO>>(result);
            //    await _distributedCache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(output), Utils.Util.RedisOptions());
            //}
            return output;
        }

        [HttpGet]
        public async Task<List<ListAllProductCategoryDTO>> GetAllProductcategory()
        {
            var output = new List<ListAllProductCategoryDTO>();
            var result = await _repoWrapper.ProductCategory.GetAllProductcategory();
            if(result !=null)
            {
                
                foreach ( var p in result)
                {
                    ListAllProductCategoryDTO listItem = new ListAllProductCategoryDTO();
                    listItem.Parent = _mapper.Map<ProductCategoryDTO>(p); 
                    var lstChild = await _repoWrapper.ProductCategory.GetTopCateByParentId("", (int)p.ProductCategory_ID);
                    listItem.Child = _mapper.Map<IEnumerable<ProductCategoryDTO>>(lstChild);
                    output.Add(listItem);
                }
            }
            return output;
        }
        /// <summary>
        /// Get ProdCate From Menu
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        //[Authorize]
        [HttpGet]
        public async Task<ListProductCategoryDTO> GetProdCateByParentIDFromMenu(string UserId,int? productTypeId, int? statusTypeId, int parentId = 656)
        {
            var output = new ListProductCategoryDTO();           
            var result = await _repoWrapper.ProductCategory.GetMenuProdCateByUser(UserId, productTypeId, statusTypeId, parentId);
            output.Data = _mapper.Map<IEnumerable<ProductCategoryDTO>>(result);           
            return output;
        }
        [HttpGet]
        public async Task<IEnumerable<ProductCategoryDTO>> GetProdCateByParentIDTakeTop(int parentId, int top)
        {
            var output = new List<ProductCategoryDTO>();
            //var cacheKey = $"ProductCategory_GetProdCateByParentIDTakeTop{parentId}{top}";
            //var redisEncode = await _distributedCache.GetStringAsync(cacheKey);
            //if (redisEncode != null)
            //{
            //    output = JsonConvert.DeserializeObject<List<ProductCategoryDTO>>(redisEncode);
            //}
            //else
            //{
               var result = await _repoWrapper.ProductCategory.GetTopCateByParentIdTakeTop(parentId, top);
                output = _mapper.Map<List<ProductCategoryDTO>>(result);
            //    await _distributedCache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(output), Utils.Util.RedisOptions());
            //}

            return output;
        }
        [HttpGet]
        public async Task<List<ProductCategoryTwoLevelDTO>> GetProdCateTwoLevel(string keyword, int? ProductCategoryId)
        {
            var output = new List<ProductCategoryTwoLevelDTO>();
            //var cacheKey = $"ProductCategory_GetProdCateTwoLevel{keyword}{ProductCategoryId}";
            //var redisEncode = await _distributedCache.GetStringAsync(cacheKey);
            //if (redisEncode != null)
            //{
            //    output = JsonConvert.DeserializeObject<List<ProductCategoryTwoLevelDTO>>(redisEncode);
            //}
            //else
            //{
                var result = await _repoWrapper.ProductCategory.GetListProductCategoryTwoLevelDTO(keyword, ProductCategoryId);
                output = _mapper.Map<List<ProductCategoryTwoLevelDTO>>(result);
              //  await _distributedCache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(output), Utils.Util.RedisOptions());
            //}

            return output;
        }
        /// <summary>
        /// Get ListHighLightCategory in home
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ListHighLightCategoryDTO> GetListHighLightCategory()
        {
            _logger.LogInfo("GetListHighLightCategory");
            var output = new ListHighLightCategoryDTO();
            //var cacheKey = "ProductCategory_GetListHighLightCategory";
            //var redisEncode = await _distributedCache.GetStringAsync(cacheKey);
            //if (redisEncode != null)
            //{
            //    output = JsonConvert.DeserializeObject<ListHighLightCategoryDTO>(redisEncode);
            //}
            //else
            //{
                var result = await _repoWrapper.PageLayout.GetPageLayout();
                output.Data = _mapper.Map<IEnumerable<ProductCategoryHighLightDTO>>(result);
            //    await _distributedCache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(output), Utils.Util.RedisOptions());
            //}

            return output;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}