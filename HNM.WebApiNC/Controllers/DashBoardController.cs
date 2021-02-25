using AutoMapper;
using HNM.DataNC.ModelsNC.ModelsDTO;
using HNM.RepositoriesNC.RepositoriesBase;
using HNM.WebApiNC.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace HNM.WebApiNC.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class DashBoardController : ControllerBase, IDisposable
    {
        private readonly ILoggerManager _logger;
        private IRepositoryWrapper _repoWrapper;
        private IMapper _mapper;
        private readonly IDistributedCache _distributedCache;
        public DashBoardController(IRepositoryWrapper repoWrapper, ILoggerManager logger, IMapper mapper, IDistributedCache distributedCache)
        {
            _repoWrapper = repoWrapper;
            _logger = logger;
            _mapper = mapper;
            _distributedCache = distributedCache;
        }
        /// <summary>
        /// Get Information DashBoard
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<DashBoashProductBrandDTO> GetDashBoard(string UserId, int? CustomerFilter, int? PostFilter, int? OrderFilter, int? FinanceFilter)
        {
            var output = new DashBoashProductBrandDTO();
            if (String.IsNullOrEmpty(UserId))
            {
                //output.Add(new DashBoashProductBrandDTO { ErrorCode = "004", Message = Utils.ConstMessage.GetMsgConst("004") });
                output.ErrorCode = "004";
                output.Message = Utils.ConstMessage.GetMsgConst("004");
                return output;
            }
            else
            {
                try
                {
                    //var cacheKey = $"DashBoard_GetCarouselList{UserId}{CustomerFilter}{PostFilter}{OrderFilter}{FinanceFilter}";
                    //var redisEncode = await _distributedCache.GetStringAsync(cacheKey);
                    //if (redisEncode != null)
                    //{
                    //    output = JsonConvert.DeserializeObject<DashBoashProductBrandDTO>(redisEncode);
                    //}
                    //else
                    //{
                        var result = await _repoWrapper.DashBoard.GetDashBoardPharse2(UserId, 0, 0, 0, 0);
                        if (result.Count > 0)
                        {
                            output.Brand = _mapper.Map<DashBoardBrand>(result[0]);
                            output.Customer = _mapper.Map<DashBoardCustomer>(result[0]);
                            output.Post = _mapper.Map<DashBoardPost>(result[0]);
                            output.Order = _mapper.Map<DashBoardOrder>(result[0]);
                            output.Finance = _mapper.Map<DashBoardFinance>(result[0]);
                        }
                    //    await _distributedCache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(output), Utils.Util.RedisOptions());
                    //}
                }
                catch(Exception ex)
                {
                    _logger.LogError($"GetDashBoard:" + ex.ToString());
                }

            }
            return output;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}