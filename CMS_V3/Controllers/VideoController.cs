using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using HNM.CommonNC;
using HNM.DataNC.ModelsNC.ModelsDTO;
using HNM.DataNC.ModelsStore;
using HNM.RepositoriesWeb.RepositoriesBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using X.PagedList;

namespace CMS_V3.Controllers
{
    public class VideoController : Controller
    {
        private readonly ILogger<VideoController> _logger;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IRepositoryWrapper _repoWrapper;
        private IMapper _mapper;

        public VideoController(ILogger<VideoController> logger, IHttpClientFactory clientFactory, IMapper mapper, IRepositoryWrapper repoWrapper)
        {
            _clientFactory = clientFactory;
            _logger = logger;
            _mapper = mapper;
            _repoWrapper = repoWrapper;
        }
        public async Task<IActionResult> Index(int? page = 1, int? pageSize = 8)
        {
            //Get Menu
            ViewBag.MenuMachine = await _repoWrapper.ProductCategory.GetLstMenuByParentId(654);
            ViewBag.MenuMaterials = await _repoWrapper.ProductCategory.GetLstMenuByParentId(652);
            ViewBag.MenuServices = await _repoWrapper.ProductCategory.GetLstMenuByParentId(651);
            ViewBag.MenuHeader = await _repoWrapper.Video.GetLstVideoCate();
            ViewBag.ListVideo = await _repoWrapper.Video.GetListVideo((int)page, (int)pageSize);
            ViewBag.Page = page ?? 1;
            ViewBag.PageSize = pageSize ?? 8;
            ViewBag.TotalCount = ViewBag.ListVideo[0]?.TotalCount;
            ViewBag.TotalPages = (int)Math.Ceiling(decimal.Divide(ViewBag.ListVideo[0]?.TotalCount, ViewBag.PageSize ?? 8));
            //await GetPaggingVideo(1, 8);
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> GetPaggingVideo(int? page = 1, int? pageSize=8)
        {
            ViewBag.Page = page ?? 1;
            ViewBag.PageSize = pageSize ?? 8;
            ViewBag.ListVideo = await _repoWrapper.Video.GetListVideo(page ?? 1, pageSize ?? 8);
            ViewBag.TotalCount = ViewBag.ListVideo[0]?.TotalCount;
            ViewBag.TotalPages = (int)Math.Ceiling(decimal.Divide(ViewBag.ListVideo[0]?.TotalCount, pageSize ?? 8));
            ViewBag.VideoPaging =
                new StaticPagedList<VideoShortInfo>
                (
                    ViewBag.ListVideo, page ?? 1, pageSize ?? 8, ViewBag.TotalCount
                    );
            return PartialView("VideoPaggingPartial", ViewBag.ListVideo);
        }

        public async Task<ActionResult> GetPaggingRelateVideo(int ? VideoCategory_ID = 0, int? Article_ID = 0, int? page = 1, int? pageSize = 12)
        {
            ViewBag.VideoCategory_ID = VideoCategory_ID;
            ViewBag.Article_ID = Article_ID;
            ViewBag.Page = page ?? 1;
            ViewBag.PageSize = pageSize ?? 12;
            //lấy ra 12 video liên quan
            ViewBag.LstRelateVideo = await _repoWrapper.Video.GetRelateVideo((int)VideoCategory_ID, (int)Article_ID, 2, (int)page, (int)pageSize);
            var LstRelateVideo = await _repoWrapper.Video.GetRelateVideo((int)VideoCategory_ID, (int)Article_ID, 2, (int)page, 100);

            if (ViewBag.LstRelateVideo.Count == 0)
            {
                ViewBag.TotalCount = 0;
                ViewBag.TotalPages = 0;
                ViewBag.VideoPaging =
                    new StaticPagedList<VideoShortInfo>
                    (
                        ViewBag.LstRelateVideo, page ?? 1, pageSize ?? 12, 0
                        );
            }
            else
            {
                ViewBag.TotalCount = LstRelateVideo.Count;
                ViewBag.TotalPages = (int)Math.Ceiling(decimal.Divide(LstRelateVideo.Count, pageSize ?? 8));
                ViewBag.VideoPaging =
                    new StaticPagedList<VideoShortInfo>
                    (
                        ViewBag.LstRelateVideo, page ?? 1, pageSize ?? 12, ViewBag.TotalCount
                        );
            }
            
            return PartialView("VideoRelatePartial", LstRelateVideo.ToPagedList((int)page, (int)pageSize));
        }

        public async Task<ActionResult> GetPaggingVideoByCate(int? CateVideoId, int? page = 1, int? pageSize= 8)
        {
            ViewBag.Page = page ?? 1;
            ViewBag.PageSize = pageSize ?? 8;
            ViewBag.ListVideoByCate = await _repoWrapper.Article.GetVideoByCate((int)CateVideoId, 2, (int)page, (int)pageSize);
            ViewBag.CateVideoId = CateVideoId;
            if (ViewBag.ListVideoByCate.Count > 0)
            {
                ViewBag.TotalCount = ViewBag.ListVideoByCate[0]?.TotalCount;
                ViewBag.TotalPages = (int)Math.Ceiling(decimal.Divide(ViewBag.TotalCount, pageSize ?? 8));
            }
            ViewBag.VideoPaging =
                new StaticPagedList<Video_Search_Mobile_Result>
                (
                    ViewBag.ListVideoByCate, page ?? 1, pageSize ?? 8, ViewBag.TotalCount
                    );
            return PartialView("VideoByCatePaggingPartial", ViewBag.ListVideoByCate);
        }

        public async Task<IActionResult> VideoCate(int? page = 1, int? pageSize = 8)
        {
            //Get Menu
            ViewBag.MenuMachine = await _repoWrapper.ProductCategory.GetLstMenuByParentId(654);
            ViewBag.MenuMaterials = await _repoWrapper.ProductCategory.GetLstMenuByParentId(652);
            ViewBag.MenuServices = await _repoWrapper.ProductCategory.GetLstMenuByParentId(651);
            ViewBag.MenuHeader = await _repoWrapper.Video.GetLstVideoCate();
            ViewBag.MetaTags = await _repoWrapper.Setting.GetSetting();

            var url = RouteData.Values["url"];
            LibraryCategoryDTO result = await _repoWrapper.Library.LibraryCategoryByUrl((string)url);
            if (result != null)
            {
                ViewBag.CateVideoId = result.LibraryCategory_ID;
                ViewBag.UrlVideoCate = url;
                ViewBag.CatName = result.Name;
                ViewBag.ListVideoByCate = await _repoWrapper.Article.GetVideoByCate(result.LibraryCategory_ID, 2, (int)page, (int)pageSize);
                ViewBag.Page = page ?? 1;
                ViewBag.PageSize = pageSize ?? 8;
                if (ViewBag.ListVideoByCate.Count > 0)
                {
                    ViewBag.TotalCount = ViewBag.ListVideoByCate[0]?.TotalCount;
                    ViewBag.TotalPages = (int)Math.Ceiling(decimal.Divide(ViewBag.TotalCount, pageSize ?? 8));
                }
            }
            return View();
        }


        public async Task<IActionResult> VideoDetail(int? page = 1, int? pageSize = 12)
        {
            var url = RouteData.Values["url"];
            ViewBag.UrlVideo = url;
            int Article_ID = Utils.RegexRouteIdFromUrl(url.ToString());
            if (Article_ID == 0) // Error Redirect 301
            {
                //Response.StatusCode = 301;
                //Response.Headers("Location", "https://hanoma.vn/may-de-ban");
                //Response.End();
            }
            //Get Menu
            ViewBag.MenuMachine = await _repoWrapper.ProductCategory.GetLstMenuByParentId(654);
            ViewBag.MenuMaterials = await _repoWrapper.ProductCategory.GetLstMenuByParentId(652);
            ViewBag.MenuServices = await _repoWrapper.ProductCategory.GetLstMenuByParentId(651);
            ViewBag.MenuHeader = await _repoWrapper.Video.GetLstVideoCate();
            

            ArticleDTO model = await _repoWrapper.Article.DetailArticle(Article_ID, 2);
            if (model != null)
            {
                //lấy ra tên chuyên mục
                if(model.ArticleCategory_ID != null)
                {
                    var name = await _repoWrapper.Article.ArticleCategoryName(model.ArticleCategory_ID);
                    ViewBag.NameCate = name[0].Name;
                }

                var dataCate = await _repoWrapper.Library.LibraryCategoryById(model.VideoCategory_ID);
                if (dataCate != null)
                {
                    ViewBag.NameCate = dataCate.Name;
                    ViewBag.CateUrl = dataCate.URL;
                }

                ViewBag.VideoCategory_ID = model.VideoCategory_ID;
                ViewBag.CateVideoId = model.VideoCategory_ID;
                ViewBag.Article_ID = Article_ID;

                ViewBag.Page = page ?? 1;
                ViewBag.PageSize = pageSize ?? 12;
                //lấy ra 12 video liên quan
                ViewBag.LstRelateVideo = await _repoWrapper.Video.GetRelateVideo(model.VideoCategory_ID, Article_ID, 2, (int)page, (int)pageSize);
                if(ViewBag.LstRelateVideo.Count != 0)
                {
                    ViewBag.TotalCount = ViewBag.LstRelateVideo[0]?.TotalCount;
                    ViewBag.TotalPages = (int)Math.Ceiling(decimal.Divide(ViewBag.LstRelateVideo[0]?.TotalCount, pageSize ?? 8));
                }
                else
                {
                    ViewBag.TotalCount = 0;
                    ViewBag.TotalPages = 0;
                }
                
            }

            return View(model);
        }
    }
}
