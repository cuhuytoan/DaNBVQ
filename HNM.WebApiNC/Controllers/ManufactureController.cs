using AutoMapper;
using HNM.DataNC.Models;
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
    public class ManufactureController : ControllerBase, IDisposable
    {
        private readonly ILoggerManager _logger;
        private IRepositoryWrapper _repoWrapper;
        private IMapper _mapper;
        public ManufactureController(IRepositoryWrapper repoWrapper, ILoggerManager logger, IMapper mapper)
        {
            _repoWrapper = repoWrapper;
            _logger = logger;
            _mapper = mapper;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// using for filter
        /// </summary>
        /// <param name="productCategoryId"></param>
        /// <param name="productTypeId"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ListManufactureSearchDTO> Search(int productCategoryId, int? productTypeId, string search = "")
        {

            var result = await _repoWrapper.Manufacture.SearchManufacture(productCategoryId, productTypeId, search);
            var output = new ListManufactureSearchDTO();
            output.Data = _mapper.Map<IEnumerable<ProductManufacture_SearchByCategory_Result_DTO>>(result);
            return output;
        }
        /// <summary>
        /// Get product manufactory in shopman
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<ProductManufacture_SearchByCategory_Result_DTO>> GetShopmanManuFactory(string keyword)
        {
            var result= await _repoWrapper.Manufacture.GetShopmanListManufacture(keyword ?? "");
            var output = _mapper.Map<List<ProductManufacture_SearchByCategory_Result_DTO>>(result);
            return output;
        }

    }
}