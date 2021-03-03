using AutoMapper;
using HNM.DataNC.ModelsFilter;
using HNM.DataNC.ModelsNC.ModelsDTO;
using HNM.RepositoriesNC.RepositoriesBase;
using HNM.WebApiNC.Logging;
using HNM.WebApiNC.Utils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace HNM.WebApiNC.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class SearchController : ControllerBase, IDisposable
    {
        private readonly ILoggerManager _logger;
        private IRepositoryWrapper _repoWrapper;
        private IMapper _mapper;
        public SearchController(IRepositoryWrapper repoWrapper, ILoggerManager logger, IMapper mapper)
        {
            _repoWrapper = repoWrapper;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ProductPaggingDTO> ElasticProductSearch(int page , int pageSize , string search = "")
        {
            var output = new ProductPaggingDTO();
            //try
            //{
            //    var outSearchElastic = await _repoWrapper.Elastic.SearchProducts(page , pageSize , search);
            //    output.PageSize = pageSize;
            //    output.CurrentPage = page;
            //    output.TotalRecord = (int)outSearchElastic.TotalRecord;
            //    output.TotalPage = (output.TotalRecord - 1) / pageSize + 1;
            //    output.Data = _mapper.Map<IEnumerable<ProductSearchResultDTO>>(outSearchElastic.Products);
            //}
            //catch (Exception ex)
            //{
                var parameters = new ProductFilter
                {
                    Keyword = search,
                    StatusTypeId = 4
                };
                var result = await _repoWrapper.Product.GetListProductPagging(parameters);
                var firstItem = result.FirstOrDefault();
                int totalRow = (int)(firstItem?.TotalRows ?? 0);
                output.PageSize = pageSize;
                output.CurrentPage = page;
                output.TotalRecord = totalRow;
                output.TotalPage = (totalRow - 1) / pageSize + 1;
                output.Data = _mapper.Map<IEnumerable<ProductSearchResultDTO>>(result);
                return output;
            //}
            return output;
        }
        [HttpGet]
        public async Task<BrandPaggingDTO> ElasticBrandSearch(int page, int pageSize, string search = "")
        {
            var output = new BrandPaggingDTO();
            try
            {
                var outSearchElastic = await _repoWrapper.Elastic.SearchProductBrandPagination(page, pageSize, search);
                output.PageSize = pageSize;
                output.CurrentPage = page;
                output.TotalRecord = (int)outSearchElastic.TotalRecord;
                output.TotalPage = (output.TotalRecord - 1) / pageSize + 1;
                output.Data = _mapper.Map<IEnumerable<ProductBrandSearchDTO>>(outSearchElastic.Brands);
                output.Data.ToList().ForEach(x => x.Description = HttpUtility.HtmlDecode(Common.CommonUtil.StripHTML(HttpUtility.HtmlDecode(x.Description))));
                output.Data.Select(c => { c.LocationName = Util.LocationDictionary[(int)c.LocationId]; return c; }).ToList();
            }
            catch (Exception ex)
            {
                var result = await _repoWrapper.Brand.GetListBrandPagging(page, pageSize,null, 1, productCategoryId: null, keyword: search);
                int totalRow = result.Item1;
                output.PageSize = pageSize;
                output.CurrentPage = page;
                output.TotalRecord = totalRow;
                output.TotalPage = (totalRow - 1) / pageSize + 1;
                output.Data = _mapper.Map<IEnumerable<ProductBrandSearchDTO>>(result.Item2);
                output.Data.ToList().ForEach(x => x.Description = HttpUtility.HtmlDecode(Common.CommonUtil.StripHTML(HttpUtility.HtmlDecode(x.Description))));
                output.Data.Select(c => { c.LocationName = Util.LocationDictionary[(int)c.LocationId]; return c; }).ToList();
                return output;
            }
            return output;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}