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
    public class LocationController : ControllerBase, IDisposable
    {
        private readonly ILoggerManager _logger;
        private IRepositoryWrapper _repoWrapper;
        private IMapper _mapper;
        public LocationController(IRepositoryWrapper repoWrapper, ILoggerManager logger, IMapper mapper)
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
        public async Task<ListLocationDTO> GetAllLocation(string keyword)
        {
            var result = await _repoWrapper.Location.GetAllLocation(keyword);
            var output = new ListLocationDTO();
            output.Data = _mapper.Map<IEnumerable<LocationDTO>>(result);

            return output;
        }
        [HttpGet]
        public async Task<ListDistrictDTO> GetDistrictByLocation(string keyword, int? LocationId)
        {
            var result = await _repoWrapper.Location.GetDistrictByLocation(keyword, LocationId);
            var output = new ListDistrictDTO();
            output.Data = _mapper.Map<IEnumerable<DistrictDTO>>(result);
            return output;
        }
        [HttpGet]
        public async Task<ListLocationDTO> GetLocationByCountry(string keyword, int CountryId = 7)
        {
            var result = await _repoWrapper.Location.GetAllLocation(keyword);
            var output = new ListLocationDTO();
            output.Data = _mapper.Map<IEnumerable<LocationDTO>>(result);
            return output;
        }
    }
}