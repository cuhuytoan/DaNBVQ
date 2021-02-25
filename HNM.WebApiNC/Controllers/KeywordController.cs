using AutoMapper;
using HNM.DataNC.ModelsNC.ModelsDTO;
using HNM.RepositoriesNC.RepositoriesBase;
using HNM.WebApiNC.Logging;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HNM.WebApiNC.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class KeywordController : ControllerBase, IDisposable
    {
        private readonly ILoggerManager _logger;
        private IRepositoryWrapper _repoWrapper;
        private IMapper _mapper;
        public KeywordController(IRepositoryWrapper repoWrapper, ILoggerManager logger, IMapper mapper)
        {
            _repoWrapper = repoWrapper;
            _logger = logger;
            _mapper = mapper;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        [HttpGet]
        public async Task<ListKeywordDTO> GetSuggestKeywords(int topX = 10, string key = "")
        {
            var output = new ListKeywordDTO();
            try
            {
                var result = await _repoWrapper.Keyword.GetSuggestKeyword(topX, key);
                output.Data = _mapper.Map<IEnumerable<KeywordDTO>>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetSuggestKeywords: " + ex.ToString());
            }
            return output;
        }
    }
}