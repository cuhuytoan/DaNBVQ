using AutoMapper;
using HNM.DataNC.ModelsNC.ModelsDTO;
using HNM.RepositoriesNC.RepositoriesBase;
using HNM.WebApiNC.Logging;
using HNM.WebApiNC.Utils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HNM.WebApiNC.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class SubscriptionController : ControllerBase, IDisposable
    {
        private readonly ILoggerManager _logger;
        private IRepositoryWrapper _repoWrapper;
        private IMapper _mapper;
        public SubscriptionController(IRepositoryWrapper repoWrapper, ILoggerManager logger, IMapper mapper)
        {
            _repoWrapper = repoWrapper;
            _logger = logger;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<SubscriptionDTO> GetByEmail(string email)
        {
            var result = await _repoWrapper.Subscription.GetByEmail(email);
            var outputData = _mapper.Map<SubscriptionDTO>(result);
            return outputData;

        }
        [HttpPost]
        public async Task<SubscriptionResultDTO> SubscriptionEmail([FromBody] SubscriptionRequestDTO model)
        {
            var subscriptionId = await _repoWrapper.Subscription.SubscriptionEmail(model.Email);
            var result = new SubscriptionResultDTO();
            result.ErrorCode = "00";
            result.Message = "Đăng ký thành công!";
            return result;

        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
