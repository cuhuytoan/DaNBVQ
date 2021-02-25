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
    public class IntroController : ControllerBase, IDisposable
    {
        private readonly ILoggerManager _logger;
        private IRepositoryWrapper _repoWrapper;
        private IMapper _mapper;
        private readonly IDistributedCache _distributedCache;
        public IntroController(IRepositoryWrapper repoWrapper, ILoggerManager logger, IMapper mapper, IDistributedCache distributedCache)
        {
            _repoWrapper = repoWrapper;
            _logger = logger;
            _mapper = mapper;
            _distributedCache = distributedCache;
        }
        [HttpGet]
        public async Task<ListIntroScreenDTO> GetIntroScreens()
        {
            var output = new ListIntroScreenDTO();
            try
            {
                var cacheKey = "Intro_GetIntroScreens";
                var redisEncode = await _distributedCache.GetStringAsync(cacheKey);
                if (redisEncode != null)
                {
                    output = JsonConvert.DeserializeObject<ListIntroScreenDTO>(redisEncode);
                }
                else
                {
                    var result = await _repoWrapper.IntroScreen.GetIntroScreens();
                    output.Data = _mapper.Map<IEnumerable<IntroScreenDTO>>(result);
                    await _distributedCache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(output), Utils.Util.RedisOptions());
                }
            }
            catch(Exception ex)
            {
                _logger.LogError($"GetIntroScreens: " + ex.ToString());
            }
            return output;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}