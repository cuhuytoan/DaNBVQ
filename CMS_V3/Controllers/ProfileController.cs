using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using HNM.DataNC.ModelsNC.ModelsDTO;
using HNM.RepositoriesWeb.RepositoriesBase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CMS_V3.Controllers
{
    [Authorize(policy: "Authenticated")]
    public class ProfileController : Controller
    {
        private readonly ILogger<ProfileController> _logger;        
        private readonly IRepositoryWrapper _repoWrapper;
        private IMapper _mapper;

        public ProfileController(ILogger<ProfileController> logger, IMapper mapper, IRepositoryWrapper repoWrapper)
        {            
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

        public async Task<IActionResult> Index(int? id)
        {
            //Get Menu
            ViewBag.CategoryMenuHeader = await GetAllProductCategory();

            //User Info
            ViewBag.UserProfile = await _repoWrapper.Profile.Profilers();

            //Notification
            ViewBag.NotificationAll = await _repoWrapper.Profile.GetListMsgByUser(1000, 0);
            ViewBag.NotificationAllUser = await _repoWrapper.Profile.GetListMsgByUser(1000, 1);
            ViewBag.NotificationSocial = await _repoWrapper.Profile.GetListMsgByUser(1000, 2);
            ViewBag.NotificationPromotion = await _repoWrapper.Profile.GetListMsgByUser(1000, 3);

            //Quote ads
            ViewBag.ListQuoteAds = await _repoWrapper.Profile.GetListQuoteAds();
            ViewBag.AboutHanoma = await _repoWrapper.Article.DetailArticle(1681, 1);

            //Active
            ViewBag.ActiveBlock = id;

            //Lấy ra chi tiết gian hàng
            var productBrand = await _repoWrapper.Profile.GetBrandDetailByUserId();
            ViewBag.ProductBrand = productBrand.ProductBrandId;
            ViewBag.ReferralCode = productBrand.ReferralCode;

            //List nhu cầu
            ViewBag.listDemand = await _repoWrapper.ManagerDemand.GetLstTimeLinePost(1,30);

            ViewBag.AllLocation = await _repoWrapper.Profile.GetAllLocation2();
            return View();
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
        public async Task<JsonResult> FCMHasRead(int FCMMessageID, int NotiSpecType)
        {
            int HasRead = 1;
            var result = await _repoWrapper.Profile.FCMHasRead(HasRead, FCMMessageID, NotiSpecType);

            return Json(result.ErrorCode);
        }

        [HttpPut]
        public async Task<JsonResult> UpdateUser(ProfilersDTO profilersDTO)
        {
            var result = await _repoWrapper.Profile.UpdateUser(profilersDTO);

            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> ContactAds(ContactAdsDTO contactAdsDTO)
        {
            var result = await _repoWrapper.Profile.ContactAdsDTO(contactAdsDTO);

            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> ChangePass(ChangePassDTO changePassDTO)
        {
            var result = await _repoWrapper.Profile.ChangePassword(changePassDTO);

            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> PostReferralCode(PostReferralCode PostReferralCode)
        {
            var result = await _repoWrapper.Profile.PostReferralCode(PostReferralCode);
            return Json(result);
        }

        [HttpGet]
        public IActionResult CreateProductBrand()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductBrand([FromBody] PostProductBrandDTO model)
        {
            //logo
            if (model.ImgLogo != null)
            {
                String[] substrings = model.ImgLogo.Base64.Split(',');
                model.ImgLogo.Base64 = substrings[1];
            }


            //banner
            if (model.ImgBanner != null)
            {
                String[] substrings = model.ImgBanner.Base64.Split(',');
                model.ImgBanner.Base64 = substrings[1];
            }

            var result = await _repoWrapper.Profile.CreateProductBrand(model);
            return Json(result);
        }

        
        public IActionResult ChangeLogo()
        {
            return PartialView("ChangeLogo");
        }

        [HttpPost]
        public async Task<IActionResult> ChangeLogo([FromBody] ImageUploadAvatarDTO model)
        {
            if (model.MainImage != null)
            {
                String[] substrings = model.MainImage.Base64.Split(',');
                model.Base64 = substrings[1];
            }
            model.ExtensionType = model.MainImage.ExtensionType;
            model.FileName = model.MainImage.FileName;

            var result = await _repoWrapper.Profile.UpdateAvatar(model);
            return Json(result.Message);
        }

        public async Task<JsonResult> GetLstCountry(string text)
        {
            var result = await _repoWrapper.Profile.GetLstCountry(text);
            return Json(result.data);
        }

        public async Task<JsonResult> GetLocationByCountry(int? CountryId, string text)
        {
            var result = await _repoWrapper.Profile.GetLocationByCountry(CountryId, text);
            return Json(result.Data);
        }

        public async Task<JsonResult> GetDistrictByLocation(int? LocationId, string text)
        {
            var result = await _repoWrapper.Profile.GetDistrictByLocation(LocationId, text);
            return Json(result.Data);
        }
    }
}
