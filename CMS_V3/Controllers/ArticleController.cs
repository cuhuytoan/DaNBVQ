using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using CMS_V3.ViewModel;
using HNM.DataNC.Models;
using HNM.DataNC.ModelsNC.ModelsDTO;
using HNM.RepositoriesWeb.RepositoriesBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using HNM.CommonNC;

namespace CMS_V3.Controllers
{
    public class ArticleController : Controller
    {
        private readonly ILogger<ArticleController> _logger;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IRepositoryWrapper _repoWrapper;
        private IMapper _mapper;

        public ArticleController(ILogger<ArticleController> logger, IHttpClientFactory clientFactory, IMapper mapper, IRepositoryWrapper repoWrapper)
        {
            _clientFactory = clientFactory;
            _logger = logger;
            _mapper = mapper;
            _repoWrapper = repoWrapper;
        }
        public async Task<IActionResult> Index()
        {            
            //Get Menu
            ViewBag.ProductCategory = await GetAllProductCategory();
            ViewBag.MenuHeader = await _repoWrapper.Article.GetLstArticleCate();
            ViewBag.MetaTags = await _repoWrapper.Setting.GetSetting();

            ArticleViewModel articleViewModel = new ArticleViewModel();
            //lấy ra tin nổi bật đầu trang
            articleViewModel.highlightArticle = await _repoWrapper.Article.GetHomeHighLightsArticle();
            articleViewModel.lstBlock1 = await _repoWrapper.Article.GetHomeVerticleBlockOneArticle();
            articleViewModel.lstBlock2 = await _repoWrapper.Article.GetHomeVerticleBlockTwoArticle();

            //Khối tin nóng
            articleViewModel.lstHotArticle = await _repoWrapper.Article.GetArtByCate(88,1,1,7);
            articleViewModel.ArticleCategoryHotArticle = await _repoWrapper.Article.ArticleCategoryName(88);

            //Khối tin nóng
            articleViewModel.lstNews = await _repoWrapper.Article.GetArtByCate(87,1,1,7);
            articleViewModel.ArticleCategoryNew = await _repoWrapper.Article.ArticleCategoryName(87);

            //Khối tin đấu thầu
            articleViewModel.lstBidding = await _repoWrapper.Article.GetArtByCate(97, 1, 1, 7);
            articleViewModel.ArticleCategoryBidding = await _repoWrapper.Article.ArticleCategoryName(97);

            return View(articleViewModel);
        }
        public async Task<List<ListAllProductCategoryDTO>> GetAllProductCategory()
        {
            List<ListAllProductCategoryDTO> output = new List<ListAllProductCategoryDTO>();
            try
            {

                output = await _repoWrapper.ProductCategory.GetAllProductCategory();
            }
            catch (Exception ex)
            {
                _logger.LogError($"HomeController- GetAllProductCategory {ex.ToString()}");
            }

            return output;
        }
        public async Task<IActionResult> ArticleCate()
        {
            //Get Menu
            ViewBag.MenuMachine = await _repoWrapper.ProductCategory.GetLstMenuByParentId(654);
            ViewBag.MenuMaterials = await _repoWrapper.ProductCategory.GetLstMenuByParentId(652);
            ViewBag.MenuServices = await _repoWrapper.ProductCategory.GetLstMenuByParentId(651);
            ViewBag.MenuHeader = await _repoWrapper.Article.GetLstArticleCate();
            ViewBag.ListCategory = await _repoWrapper.Service.GetProdCateByParentID(651);

            var url = RouteData.Values["url"];

            ArticleCategoryDTO result = await _repoWrapper.Article.ArticleCategoryByUrl((string)url);
            if(result != null)
            {
                ViewBag.CateName = result.Name;
                ViewBag.CateUrl = result.URL;
                ViewBag.CateId = result.ArticleCategory_ID;
                ViewBag.ListArtByCate = await _repoWrapper.Article.GetArtByCate(result.ArticleCategory_ID, 1, 1, 28);
            }
            
            return View();
        }

        public async Task<IActionResult> ArticleDetail()
        {
            var url = RouteData.Values["url"];
            int Article_ID = Utils.RegexRouteIdFromUrl(url.ToString());
            if(Article_ID == 0) // Error Redirect 301
            {
                //Response.StatusCode = 301;
                //Response.Headers("Location", "https://daninhbinhvinhquang.vn/may-de-ban");
                //Response.End();
            }    
            //Get Menu
            ViewBag.MenuMachine = await _repoWrapper.ProductCategory.GetLstMenuByParentId(654);
            ViewBag.MenuMaterials = await _repoWrapper.ProductCategory.GetLstMenuByParentId(652);
            ViewBag.MenuServices = await _repoWrapper.ProductCategory.GetLstMenuByParentId(651);
            ViewBag.MenuHeader = await _repoWrapper.Article.GetLstArticleCate();
            ViewBag.MetaTags = await _repoWrapper.Setting.GetSetting();

            ArticleDTO model = await _repoWrapper.Article.DetailArticle(Article_ID, 1);
            if(model != null)
            {
                //lấy ra tên chuyên mục
                var name = await _repoWrapper.Article.ArticleCategoryName(model.ArticleCategory_ID);
                ViewBag.NameCate = name[0].Name;
                ViewBag.CateUrl = name[0].URL;
                ViewBag.CateId = model.ArticleCategory_ID;

                ViewBag.SameArticle = await _repoWrapper.Article.ArticleSameCate(model.ArticleCategory_ID, Article_ID, 1);
            }

            return View(model);
        }
    }
}
