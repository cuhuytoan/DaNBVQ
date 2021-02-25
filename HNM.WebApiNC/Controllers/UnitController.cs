using AutoMapper;
using HNM.DataNC.Models;
using HNM.DataNC.ModelsNC.ModelsDTO;
using HNM.RepositoriesNC.RepositoriesBase;
using HNM.WebApiNC.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HNM.WebApiNC.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class UnitController : ControllerBase, IDisposable
    {
        private readonly ILoggerManager _logger;
        private IRepositoryWrapper _repoWrapper;
        private IMapper _mapper;
        private readonly IDistributedCache _distributedCache;
        private readonly IConfiguration _configuration;
        // private IFCMMessageService _fcmMessageService;
        public UnitController(IRepositoryWrapper repoWrapper, ILoggerManager logger, IMapper mapper, IDistributedCache distributedCache, IConfiguration configuration)
        {
            _repoWrapper = repoWrapper;
            _logger = logger;
            _mapper = mapper;
            _distributedCache = distributedCache;
            _configuration = configuration;
            // _fcmMessageService = fcmMessageService;
        }
        /// <summary>
        /// Get List Unit
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<UnitDTO>> GetAllUnit(string keyword)
        {
            var output = new List<UnitDTO>();
            var result = await _repoWrapper.Unit.GetListUnits(keyword);
            output = _mapper.Map<List<UnitDTO>>(result);
            return output;

        }
        /// <summary>
        /// Get List Minor
        /// 1: Job type
        /// 2: Salary Type
        /// 3: ProductServiceUnit
        /// </summary>
        /// <param name="MajorId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<Minor>> GetListMinor(int MajorId)
        {            
            var output = new List<Minor>();
            output = await _repoWrapper.Unit.GetListMinor(MajorId);
            return output;

        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }

}