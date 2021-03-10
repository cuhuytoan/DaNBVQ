using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using HNM.CommonNC;
using HNM.DataNC.ModelsNC.ModelsDTO;
using HNM.RepositoriesWeb.RepositoriesBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using X.PagedList;

namespace CMS_V3.Controllers
{
    public class CurriculumVitaeController : Controller
    {
        private readonly ILogger<CurriculumVitaeController> _logger;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IRepositoryWrapper _repoWrapper;
        private IMapper _mapper;

        public CurriculumVitaeController(ILogger<CurriculumVitaeController> logger, IHttpClientFactory clientFactory, IMapper mapper, IRepositoryWrapper repoWrapper)
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

            //get CategoryCurriculum
            ViewBag.LstCareerCategory = await _repoWrapper.CurriculumVitae.GetAllCareerCategory();

            //lái xe
            ViewBag.Driver = await _repoWrapper.CurriculumVitae.GetListCVPagging(1, null, null, null, null, null, 1, 9, 4);

            //Lái máy
            ViewBag.DriverEngine = await _repoWrapper.CurriculumVitae.GetListCVPagging(2, null, null, null, null, null, 1, 9,4);

            //Thợ sửa chữa
            ViewBag.Repairer = await _repoWrapper.CurriculumVitae.GetListCVPagging(6, null, null, null, null, null, 1, 9, 4);

            return View();
        }

        public async Task<IActionResult> CurriculumVitaeByCate(int? page = 1, int? pageSize = 9)
        {
            ViewBag.MenuMachine = await _repoWrapper.ProductCategory.GetLstMenuByParentId(654);
            ViewBag.MenuMaterials = await _repoWrapper.ProductCategory.GetLstMenuByParentId(652);
            ViewBag.MenuServices = await _repoWrapper.ProductCategory.GetLstMenuByParentId(651);

            //get CategoryCurriculum
            ViewBag.LstCareerCategory = await _repoWrapper.CurriculumVitae.GetAllCareerCategory();
            var url = RouteData.Values["url"];
            int ID = Utils.RegexRouteIdFromUrl(url.ToString());
            ViewBag.Id = ID;
            if (ID == 0) // Error Redirect 301
            {
                //Response.StatusCode = 301;
                //Response.Headers("Location", "https://daninhbinhvinhquang.vn/may-de-ban");
                //Response.End();
            }
            else
            {
                ViewBag.LstCvByCate = await _repoWrapper.CurriculumVitae.GetListCVPagging(ID, null, null, null, null, null, (int)page, pageSize, 4);

                var CareeCateInfo = await _repoWrapper.CurriculumVitae.GetDetailCareerCategory(ID);
                ViewBag.CareeName = CareeCateInfo.Name;
                ViewBag.UrlCv = url;
                ViewBag.Page = page ?? 1;
                ViewBag.PageSize = pageSize ?? 9;
                if (ViewBag.LstCvByCate.Count == 0)
                {
                    ViewBag.TotalCount = 0;
                    ViewBag.TotalPages = 0;
                }
                else
                {
                    ViewBag.TotalCount = ViewBag.LstCvByCate[0]?.TotalRows;
                    ViewBag.TotalPages = (int)Math.Ceiling(decimal.Divide(ViewBag.LstCvByCate[0]?.TotalRows, ViewBag.PageSize ?? 9));
                }
            }

            return View();
        }

        public async Task<JsonResult> GetAllCareerCategory()
        {
            var result = await _repoWrapper.CurriculumVitae.GetAllCareerCategory();
            return Json(result);
        }

        public async Task<ActionResult> GetPaggingCurriculumVitae(int? id, int? productCategoryId, string provinceName = null, int?YearOfExprience = null, int? Salary = null, int? page = 1, int? pageSize = 9)
        {
            var CareeCateInfo = await _repoWrapper.CurriculumVitae.GetDetailCareerCategory((int)id);
            ViewBag.CareeName = CareeCateInfo.Name;
            ViewBag.Id = id;
            ViewBag.Page = page ?? 1;
            ViewBag.PageSize = pageSize ?? 9;
            var minSalary = 0;
            var maxSalary = 1000000000;
            switch (Salary)
            {
                case null:
                case -1:
                    minSalary = 0; maxSalary = 1000000000; break;
                case 1:
                    minSalary = 0; maxSalary = 10000000; break;
                case 2:
                    minSalary = 10000000; maxSalary = 20000000; break;
                case 3:
                    minSalary = 20000000; maxSalary = 30000000; break;
                case 4:
                    minSalary = 30000000; maxSalary = 1000000000; break;
                default:
                    break;
            }
            ViewBag.ListCV = ViewBag.LstCvByCate = await _repoWrapper.CurriculumVitae.GetListCVPagging((int)id, productCategoryId, provinceName, YearOfExprience, minSalary, maxSalary, (int)page, pageSize, 4);
            
            if(ViewBag.ListCV.Count != 0)
            {
                ViewBag.TotalCount = ViewBag.ListCV[0]?.TotalRows;
                ViewBag.TotalPages = (int)Math.Ceiling(decimal.Divide(ViewBag.ListCV[0]?.TotalRows, pageSize ?? 9));
                ViewBag.CurriculumVitaePaging =
                    new StaticPagedList<CVSearchDTO>
                    (
                        ViewBag.ListCV, page ?? 1, pageSize ?? 9, ViewBag.TotalCount
                        );
            }
            else
            {
                ViewBag.TotalCount = 0;
                ViewBag.TotalPages = 0;
                ViewBag.CurriculumVitaePaging =
                    new StaticPagedList<CVSearchDTO>
                    (
                        ViewBag.ListCV, page ?? 1, pageSize ?? 9, ViewBag.TotalCount
                        );
            }
           
            return PartialView("CurriculumVitaePaggingPartial", ViewBag.ListCV);
        }


        public async Task<IActionResult> CurriculumVitaeDetail()
        {
            //Get Menu
            ViewBag.MenuMachine = await _repoWrapper.ProductCategory.GetLstMenuByParentId(654);
            ViewBag.MenuMaterials = await _repoWrapper.ProductCategory.GetLstMenuByParentId(652);
            ViewBag.MenuServices = await _repoWrapper.ProductCategory.GetLstMenuByParentId(651);

            //get CategoryCurriculum
            ViewBag.LstCareerCategory = await _repoWrapper.CurriculumVitae.GetAllCareerCategory();

            CVDetailDTO model = new CVDetailDTO();
            var url = RouteData.Values["url"];
            int ID = Utils.RegexRouteIdFromUrl(url.ToString());
            if (ID == 0) // Error Redirect 301
            {
                //Response.StatusCode = 301;
                //Response.Headers("Location", "https://daninhbinhvinhquang.vn/may-de-ban");
                //Response.End();
            }
            else
            {
                model = await _repoWrapper.CurriculumVitae.GetDetailsCV(ID);

                ViewBag.RelateCV = await _repoWrapper.CurriculumVitae.GetRelateCurriculumVitae((int)model.CVDetails.CareerCategoryId, model.CVDetails.Id);

                var CareeCateInfo = await _repoWrapper.CurriculumVitae.GetDetailCareerCategory((int)model.CVDetails.CareerCategoryId);
                ViewBag.CareeName = CareeCateInfo.Name;
                ViewBag.CareeUrl = CareeCateInfo.URL;
                ViewBag.CurriculumVitaeId = ID;
            }
            return View(model);
        }
    }
}
