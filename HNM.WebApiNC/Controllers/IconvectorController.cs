using AutoMapper;
using HNM.DataNC.ModelsNC.ModelsDTO;
using HNM.RepositoriesNC.RepositoriesBase;
using HNM.WebApiNC.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;

namespace HNM.WebApiNC.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class IconvectorController : ControllerBase, IDisposable
    {
        private readonly ILoggerManager _logger;
        private IRepositoryWrapper _repoWrapper;
        private IMapper _mapper;
        private readonly IDistributedCache _distributedCache;
        public IconvectorController(IRepositoryWrapper repoWrapper, ILoggerManager logger, IMapper mapper, IDistributedCache distributedCache)
        {
            _repoWrapper = repoWrapper;
            _logger = logger;
            _mapper = mapper;
            _distributedCache = distributedCache;
        }
        [HttpGet]
        public IconVectorDTO GetAllIconvector()
        {
            var output = new IconVectorDTO();
            var pNormal = new List<IconVector>();
            var pActive = new List<IconVector>();


            pNormal.Add(new IconVector() { FileName = "home.svg" });
            pNormal.Add(new IconVector() { FileName = "new.svg" });
            pNormal.Add(new IconVector() { FileName = "notify.svg" });
            pNormal.Add(new IconVector() { FileName = "them.svg" });
            pNormal.Add(new IconVector() { FileName = "filter.svg" });
            pNormal.Add(new IconVector() { FileName = "text.svg" });
            pNormal.Add(new IconVector() { FileName = "thanh toolbar.svg" });


            pActive.Add(new IconVector() { FileName = "home_enable.svg" });
            pActive.Add(new IconVector() { FileName = "new_enable.svg" });
            pActive.Add(new IconVector() { FileName = "notify_enable.svg" });
            pActive.Add(new IconVector() { FileName = "them_enable.svg" });
            pActive.Add(new IconVector() { FileName = "filterActive.svg" });

            output.IconNormal = pNormal;
            output.IconActive = pActive;

            return output;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}