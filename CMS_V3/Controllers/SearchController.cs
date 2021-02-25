using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using HNM.DataNC.ModelsNC.ModelsDTO;
using HNM.DataNC.ModelsFilter;
using HNM.RepositoriesWeb.RepositoriesBase;
using AutoMapper;
using HNM.CommonNC;
using System;
using X.PagedList;

namespace CMS_V3.Controllers
{
    public class SearchController : Controller
    {
        private readonly ILogger<SearchController> _logger;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IRepositoryWrapper _repoWrapper;
        private IMapper _mapper;

        public SearchController(ILogger<SearchController> logger, IHttpClientFactory clientFactory, IMapper mapper, IRepositoryWrapper repoWrapper)
        {
            _clientFactory = clientFactory;
            _logger = logger;
            _mapper = mapper;
            _repoWrapper = repoWrapper;
        }
        async Task<(ListProductCategoryDTO MenuMachine, ListProductCategoryDTO MenuMaterials, ListProductCategoryDTO MenuServices)> GetCategoryMenu()
        {
            var MenuMachine = await _repoWrapper.ProductCategory.GetLstMenuByParentId(654);
            var MenuMaterials = await _repoWrapper.ProductCategory.GetLstMenuByParentId(652);
            var MenuServices = await _repoWrapper.ProductCategory.GetLstMenuByParentId(651);
            return (MenuMachine, MenuMaterials, MenuServices);
        }
        public async Task<IActionResult> Index(string search = "", int? typeSearch = 1, int? page = 1, int? pageSize = 20)
        {
            ViewBag.CategoryMenuHeader = await GetCategoryMenu();
            ViewBag.TypeSearch = typeSearch ?? 1;
            ViewBag.PageCurrent = page ?? 1;
            ViewBag.SearchString = search;
            ViewBag.PageSize = pageSize ?? 20;

            return View();
        }

        public async Task<IActionResult> IndexPs(string search = "", int? typeSearch = 1, int? page = 1, int? pageSize = 20)
        {
            var url = RouteData.Values["url"];
            string text = Utils.RegexRouteStringFromUrl(url.ToString());
            search = text;
            ViewBag.CategoryMenuHeader = await GetCategoryMenu();
            ViewBag.TypeSearch = typeSearch ?? 1;
            ViewBag.PageCurrent = page ?? 1;
            ViewBag.SearchString = search;
            ViewBag.PageSize = pageSize ?? 20;

            return View();
        }

        public async Task<ActionResult> SearchProductPartialView(string search = "", int? typeSearch = 1, int? page = 1, int? pageSize = 20)
        {
            ViewBag.SearchString = search;
            ViewBag.TypeSearch = typeSearch ?? 1;
            ViewBag.PageCurrent = page ?? 1;
            ViewBag.PageSize = pageSize ?? 20;
            var productsRes = await _repoWrapper.Elastic.SearchProducts(page ?? 1, pageSize ?? 20, search);
            ViewBag.ProductPaging =
                new StaticPagedList<ProductSearchResultDTO>
                (
                    productsRes.Data, page ?? 1, pageSize ?? 20, productsRes.TotalRecord
                    );
            return PartialView("SearchProductPartialView", productsRes);
        }
        public async Task<ActionResult> SearchProductBrandPartialView(string search = "", int? typeSearch = 1, int? page = 1, int? pageSize = 20)
        {
            ViewBag.SearchString = search;
            ViewBag.TypeSearch = typeSearch ?? 1;
            ViewBag.PageCurrent = page ?? 1;
            ViewBag.PageSize = pageSize ?? 20;
            var productsBrandRes = await _repoWrapper.Elastic.SearchProductBrand(page ?? 1, pageSize ?? 20, search);
            ViewBag.ProductBrandPaging =
                new StaticPagedList<ProductBrandSearchDTO>
                (
                    productsBrandRes.Data, page ?? 1, pageSize ?? 20, productsBrandRes.TotalRecord
                    );
            return PartialView("SearchProductBrandPartialView", productsBrandRes);
        }
        public async Task<ActionResult> SearchProductServicePartialView(string search = "", int? typeSearch = 1, int? page = 1, int? pageSize = 20)
        {
            ViewBag.SearchString = search;
            ViewBag.TypeSearch = typeSearch ?? 1;
            ViewBag.PageCurrent = page ?? 1;
            ViewBag.PageSize = pageSize ?? 20;
            var productsRes = await _repoWrapper.Elastic.SearchProducts(page ?? 1, pageSize ?? 20, search);
            ViewBag.ProductPaging =
                new StaticPagedList<ProductSearchResultDTO>
                (
                    productsRes.Data, page ?? 1, pageSize ?? 20, productsRes.TotalRecord
                    );
            return PartialView("SearchProductServicePartialView", productsRes);
        }
        public async Task<ActionResult> SearchRecruitmentPartialView(string search = "", int? typeSearch = 1, int? page = 1, int? pageSize = 20)
        {
            ViewBag.SearchString = search;
            ViewBag.TypeSearch = typeSearch ?? 1;
            ViewBag.PageCurrent = page ?? 1;
            ViewBag.PageSize = pageSize ?? 20;
            var productsRes = await _repoWrapper.Recruitment.SearchRecruitment(search, page ?? 1, pageSize ?? 20, 4);
            ViewBag.ProductPaging =
                new StaticPagedList<RecruitmentSearchResultDTO>
                (
                    productsRes.Data, page ?? 1, pageSize ?? 20, productsRes.TotalRecord
                    );
            return PartialView("SearchRecruitmentPartialView", productsRes);
        }
        public async Task<ActionResult> SearchCVPartialView(string search = "", int? typeSearch = 1, int? page = 1, int? pageSize = 20)
        {
            ViewBag.SearchString = search;
            ViewBag.TypeSearch = typeSearch ?? 1;
            ViewBag.PageCurrent = page ?? 1;
            ViewBag.PageSize = pageSize ?? 20;
            var productsRes = await _repoWrapper.Elastic.SearchProducts(page ?? 1, pageSize ?? 20, search);
            ViewBag.ProductPaging =
                new StaticPagedList<ProductSearchResultDTO>
                (
                    productsRes.Data, page ?? 1, pageSize ?? 20, productsRes.TotalRecord
                    );
            return PartialView("SearchCVPartialView", productsRes);
        }
        public async Task<ActionResult> SearchArticlePartialView(string search = "", int? typeSearch = 1, int? page = 1, int? pageSize = 20)
        {
            ViewBag.SearchString = search;
            ViewBag.TypeSearch = typeSearch ?? 1;
            ViewBag.PageCurrent = page ?? 1;
            ViewBag.PageSize = pageSize ?? 20;
            var productsRes = await _repoWrapper.Article.SearchArticle(search, page ?? 1, pageSize ?? 20);
            ViewBag.ProductPaging =
                new StaticPagedList<HNM.DataNC.ModelsStore.Article_Search_Result>
                (
                    productsRes.Data, page ?? 1, pageSize ?? 20, productsRes.TotalRecord
                    );
            return PartialView("SearchArticlePartialView", productsRes);
        }
        public async Task<ActionResult> SearchAllPartialView(string search = "", int? typeSearch = 1, int? page = 1, int? pageSize = 20)
        {
            ViewBag.SearchString = search;
            ViewBag.TypeSearch = typeSearch ?? 1;
            ViewBag.PageCurrent = page ?? 1;
            ViewBag.PageSize = pageSize ?? 20;
            return PartialView("SearchAllPartialView");
        }
    }
}
