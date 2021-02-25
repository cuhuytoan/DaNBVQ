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
    public class LibraryController : Controller
    {
        private readonly ILogger<LibraryController> _logger;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IRepositoryWrapper _repoWrapper;
        private IMapper _mapper;

        public LibraryController(ILogger<LibraryController> logger, IHttpClientFactory clientFactory, IMapper mapper, IRepositoryWrapper repoWrapper)
        {
            _clientFactory = clientFactory;
            _logger = logger;
            _mapper = mapper;
            _repoWrapper = repoWrapper;
        }
        public async Task<IActionResult> Index()
        {
            //Get Menu
            ViewBag.MenuMachine = await _repoWrapper.ProductCategory.GetLstMenuByParentId(654);
            ViewBag.MenuMaterials = await _repoWrapper.ProductCategory.GetLstMenuByParentId(652);
            ViewBag.MenuServices = await _repoWrapper.ProductCategory.GetLstMenuByParentId(651);
            ViewBag.MenuHeader = await _repoWrapper.Video.GetLstVideoCate();
            ViewBag.MetaTags = await _repoWrapper.Setting.GetSetting();

            LibraryViewModel libraryViewModel = new LibraryViewModel();
            //Khối kiến thức vận hành
            libraryViewModel.lstLib1 = await _repoWrapper.Library.GetLstLibByCate(2, 1, 7);
            libraryViewModel.CateName1 = await _repoWrapper.Library.LibraryCategoryById(2);

            //Khối kiến thức bảo dưỡng
            libraryViewModel.lstLib2 = await _repoWrapper.Library.GetLstLibByCate(3, 1, 7);
            libraryViewModel.CateName2 = await _repoWrapper.Library.LibraryCategoryById(3);

            //Khối tin sửa chữa
            libraryViewModel.lstLib3 = await _repoWrapper.Library.GetLstLibByCate(4, 1, 7);
            libraryViewModel.CateName3 = await _repoWrapper.Library.LibraryCategoryById(4);
            return View(libraryViewModel);
        }


        public async Task<IActionResult> LibraryCate()
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
                ViewBag.ListLibByCate = await _repoWrapper.Library.GetLstLibByCate(result.LibraryCategory_ID, 1, 28);
                ViewBag.CateUrl = result.URL;
                ViewBag.CateName = result.Name;
                ViewBag.CateId = result.LibraryCategory_ID;
            }

            return View();
        }


        public async Task<IActionResult> LibraryDetail()
        {
            var url = RouteData.Values["url"];
            int Library_ID = Utils.RegexRouteIdFromUrl(url.ToString());
            if (Library_ID == 0) // Error Redirect 301
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
            ViewBag.MetaTags = await _repoWrapper.Setting.GetSetting();

            LibraryDTO model = await _repoWrapper.Library.DetailLibrary(Library_ID);
            if (model.Library_ID != 0)
            {
                var libCateInfo = await _repoWrapper.Library.LibraryCategoryById((int)model.LibraryCategory_ID);
                ViewBag.NameCate = libCateInfo.Name;
                //lấy ra kiến thức liên quan liên quan
                ViewBag.SameArticle = await _repoWrapper.Library.LstRelatelLibrary((int)model.LibraryCategory_ID, model.Library_ID);
                ViewBag.CateId = model.LibraryCategory_ID;
                return View(model);
            }
            else
            {
                return Redirect("/Error/404");
            }

        }
    }
}
