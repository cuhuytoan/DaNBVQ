using AutoMapper;
using HNM.RepositoriesNC.RepositoriesBase;
using HNM.WebApiNC.Logging;
using Microsoft.AspNetCore.Mvc;
using System;

namespace HNM.WebApiNC.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class ResumeController : ControllerBase, IDisposable
    {
        private readonly ILoggerManager _logger;
        private IRepositoryWrapper _repoWrapper;
        private IMapper _mapper;
        public ResumeController(IRepositoryWrapper repoWrapper, ILoggerManager logger, IMapper mapper)
        {
            _repoWrapper = repoWrapper;
            _logger = logger;
            _mapper = mapper;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        //[HttpGet]
        //public async Task<TopResumeDTO> GetTopResume(int topX = 5)
        //{
        //    var result = await _repoWrapper.Resume.GetTopResume(topX);
        //    var output = new TopResumeDTO();
        //    output.Data = _mapper.Map<IEnumerable<ResumeDTO>>(result);

        //    return output;
        //}
        //[HttpGet]
        //public async Task<ResumePaggingDTO> GetListResume(int page = 1, int pageSize = 20, int statusTypeId = 4)
        //{

        //    var result = await _repoWrapper.Resume.GetListResumePagging(page, pageSize, statusTypeId);
        //    var firstItem = result.FirstOrDefault();
        //    int totalRow = (int)(firstItem?.TotalRows ?? 0);
        //    var output = new ResumePaggingDTO();
        //    output.PageSize = pageSize;
        //    output.CurrentPage = page;
        //    output.TotalRecord = totalRow;
        //    output.TotalPage = (totalRow - 1) / pageSize + 1;
        //    output.Data = _mapper.Map<IEnumerable<ResumeSearchResultDTO>>(result);
        //    return output;
        //}
    }
}