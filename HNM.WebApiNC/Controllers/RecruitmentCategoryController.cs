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
    public class RecruitmentCategoryController : ControllerBase, IDisposable
    {
        private readonly ILoggerManager _logger;
        private IRepositoryWrapper _repoWrapper;
        private IMapper _mapper;
        private readonly IDistributedCache _distributedCache;
        public RecruitmentCategoryController(IRepositoryWrapper repoWrapper, ILoggerManager logger, IMapper mapper, IDistributedCache distributedCache)
        {
            _repoWrapper = repoWrapper;
            _logger = logger;
            _mapper = mapper;
            _distributedCache = distributedCache;
        }
        [HttpGet]
        public async Task<ListRecruitmentCategoryDTO> GetAll()
        {
            var output = new ListRecruitmentCategoryDTO();
            var cacheKey = "RecruitmentCategory_GetAll";
            var redisEncode = await _distributedCache.GetStringAsync(cacheKey);
            if (redisEncode != null)
            {
                output = JsonConvert.DeserializeObject<ListRecruitmentCategoryDTO>(redisEncode);
            }
            else
            {
                var result = await _repoWrapper.RecruitmentCategory.GetAllRecruitmentCate();
                output.Data = _mapper.Map<IEnumerable<RecruitmentCategoryDTO>>(result);
                await _distributedCache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(output), Utils.Util.RedisOptions());
            }

            return output;
        }

        [HttpGet]
        public async Task<RecruitmentCategoryDTO> GetRecruitmentById(int recruitmentId)
        {
            var output = new RecruitmentCategoryDTO();
            var result = await _repoWrapper.RecruitmentCategory.GetRecruitmentById(recruitmentId);
            output = _mapper.Map<RecruitmentCategoryDTO>(result);
            return output;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}