using AutoMapper;
using HNM.CommonNC;
using HNM.DataNC.ModelsNC.ModelsDTO;
using HNM.RepositoriesWeb.RepositoriesBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using X.PagedList;

namespace CMS_V3.Controllers
{
    public class RecruitmentController : Controller
    {
        private readonly ILogger<RecruitmentController> _logger;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IRepositoryWrapper _repoWrapper;
        private IMapper _mapper;

        public RecruitmentController(ILogger<RecruitmentController> logger, IHttpClientFactory clientFactory, IMapper mapper, IRepositoryWrapper repoWrapper)
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

            ViewBag.LstRecruitmentCategory = await _repoWrapper.Recruitment.GetAllRecruitmentCategory();
            ViewBag.LstDirector = await _repoWrapper.Recruitment.GetListRecruitmentByCate(1,1,8,4);
            ViewBag.LstManager = await _repoWrapper.Recruitment.GetListRecruitmentByCate(2,1,8,4);
            ViewBag.LstAccountant = await _repoWrapper.Recruitment.GetListRecruitmentByCate(3,1,8,4);
            return View();
        }

        public async Task<IActionResult> RecruitmentByCate(int? page = 1, int? pageSize = 12)
        {
            //Get Menu
            ViewBag.MenuMachine = await _repoWrapper.ProductCategory.GetLstMenuByParentId(654);
            ViewBag.MenuMaterials = await _repoWrapper.ProductCategory.GetLstMenuByParentId(652);
            ViewBag.MenuServices = await _repoWrapper.ProductCategory.GetLstMenuByParentId(651);

            ViewBag.LstRecruitmentCategory = await _repoWrapper.Recruitment.GetAllRecruitmentCategory();

            var url = RouteData.Values["url"];
            int ID = Utils.RegexRouteIdFromUrl(url.ToString());

            ViewBag.RecruitmentId = ID;
            if (ID == 0) // Error Redirect 301
            {
                //Response.StatusCode = 301;
                //Response.Headers("Location", "https://hanoma.vn/may-de-ban");
                //Response.End();
            }
            else
            {
                var model = await _repoWrapper.Recruitment.GetRecruitmentById(ID);

                ViewBag.LstRecruitmentBycate = await _repoWrapper.Recruitment.GetListRecruitmentByCate((int)model.RecruitmentCategoryId, (int)page, pageSize, 4);
                var RecruitmentCategory = await _repoWrapper.Recruitment.GetRecruitmentById(ID);
                ViewBag.RecruitmentCateName = RecruitmentCategory.Name;
                ViewBag.RecruitmentUrl = $"{RecruitmentCategory.URL}-{ID}";
                ViewBag.Page = page ?? 1;
                ViewBag.PageSize = pageSize ?? 12;
                ViewBag.TotalCount = ViewBag.LstRecruitmentBycate.TotalRecord;
                ViewBag.TotalPages = (int)Math.Ceiling(decimal.Divide(ViewBag.LstRecruitmentBycate.TotalRecord, ViewBag.PageSize ?? 12));
            }
            return View();
        }

        public async Task<ActionResult> GetPaggingRecruitment(int? RecruitmentId, int? page = 1, int? pageSize = 12)
        {
            ViewBag.Page = page ?? 1;
            ViewBag.PageSize = pageSize ?? 12;
            ViewBag.RecruitmentId = RecruitmentId;
            var model = await _repoWrapper.Recruitment.GetRecruitmentById((int)RecruitmentId);

            var LstRecruitmentBycate = await _repoWrapper.Recruitment.GetListRecruitmentByCate((int)model.RecruitmentCategoryId, (int)page, pageSize, 4);
            var RecruitmentCategory = await _repoWrapper.Recruitment.GetRecruitmentById((int)RecruitmentId);
            ViewBag.RecruitmentCateName = RecruitmentCategory.Name;
            ViewBag.RecruitmentUrl = $"{RecruitmentCategory.URL}-{RecruitmentId}";
        
            ViewBag.TotalCount = LstRecruitmentBycate.TotalRecord;
            ViewBag.TotalPages = (int)Math.Ceiling(decimal.Divide(LstRecruitmentBycate.TotalRecord, pageSize ?? 12));
            ViewBag.RecruitmentPaging =
                new StaticPagedList<RecruitmentSearchResultDTO>
                (
                    LstRecruitmentBycate.Data, page ?? 1, pageSize ?? 12, LstRecruitmentBycate.TotalRecord
                    );
            return PartialView("RecruitmentPaggingPartial", LstRecruitmentBycate);
        }

        public async Task<IActionResult> RecruitmentDetail()
        {
            //Get Menu
            ViewBag.MenuMachine = await _repoWrapper.ProductCategory.GetLstMenuByParentId(654);
            ViewBag.MenuMaterials = await _repoWrapper.ProductCategory.GetLstMenuByParentId(652);
            ViewBag.MenuServices = await _repoWrapper.ProductCategory.GetLstMenuByParentId(651);

            ViewBag.LstRecruitmentCategory = await _repoWrapper.Recruitment.GetAllRecruitmentCategory();

            RecDetailDTO model = new RecDetailDTO();
            var url = RouteData.Values["url"];
            int ID = Utils.RegexRouteIdFromUrl(url.ToString());
            if (ID == 0) // Error Redirect 301
            {
                //Response.StatusCode = 301;
                //Response.Headers("Location", "https://hanoma.vn/may-de-ban");
                //Response.End();
            }
            else
            {
                model = await _repoWrapper.Recruitment.GetRecruitmentDetail(ID);
                ViewBag.ListRelateRec = await _repoWrapper.Recruitment.GetListRecruitmentByCate((int)model.RecDetail.RecruitmentCategoryId, 1, 9, 4);
                var RecruitmentCategory = await _repoWrapper.Recruitment.GetRecruitmentById((int)model.RecDetail.RecruitmentCategoryId);
                ViewBag.RecruitmentCateName = RecruitmentCategory.Name;
                ViewBag.RecruitmentUrl = $"{RecruitmentCategory.URL}-{RecruitmentCategory.RecruitmentCategoryId}";
                ViewBag.RecruitmentId2 = ID;
            }
            return View(model);
        }
    }
}
