using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using HNM.DataNC.ModelsNC.ModelsDTO;
using HNM.RepositoriesWeb.RepositoriesBase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace CMS_V3.Areas.Shopman.Controllers
{
    [Area("Shopman")]
    [Authorize(policy: "Authenticated")]
    public class UpdateStoreController : Controller
    {
        private readonly ILogger<UpdateStoreController> _logger;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IRepositoryWrapper _repoWrapper;
        private IMapper _mapper;
        private IDistributedCache _cache;
        public UpdateStoreController(ILogger<UpdateStoreController> logger, IHttpClientFactory clientFactory, IMapper mapper, IRepositoryWrapper repoWrapper, IDistributedCache cache)
        {
            _clientFactory = clientFactory;
            _logger = logger;
            _mapper = mapper;
            _repoWrapper = repoWrapper;
            _cache = cache;
        }
        public async Task<IActionResult> Index(string vnp_ResponseCode = null, string vnp_TxnRef = null)
        {
            var productbrand = await _repoWrapper.Profile.GetBrandDetailByUserId();
            ViewBag.BrandPackage = await _repoWrapper.UpdateStore.GetBrandPackage(productbrand.ProductBrandId);
            ViewBag.LstPaymentType = await _repoWrapper.UpdateStore.GetLstPaymentType();
            ViewBag.LstBankATM = await _repoWrapper.UpdateStore.GetListPaymentBanking(1);
            ViewBag.LstBankQT = await _repoWrapper.UpdateStore.GetListPaymentBanking(2);
            ViewBag.VNPAY = await _repoWrapper.UpdateStore.GetListPaymentBanking(3);
            ViewBag.LstBankCk = await _repoWrapper.UpdateStore.GetListPaymentBanking(4);
            ViewBag.ResponseCodeReturn = vnp_ResponseCode;
            ViewBag.OrderCodeReturn = vnp_TxnRef;
            return View();
        }
        
        public async Task<IActionResult> History()
        {
            var productbrand = await _repoWrapper.Profile.GetBrandDetailByUserId();
            var result = await _repoWrapper.UpdateStore.GetHistoryUpgrade(productbrand.ProductBrandId);
            return View(result);
        }

        public async Task<IActionResult> Policy()
        {
            var data = await _repoWrapper.UpdateStore.GetPaymentPolicy();
            return PartialView("Policy", data);
        }

        public async Task<IActionResult> GetLstBankATM()
        {
            var data = await _repoWrapper.UpdateStore.GetListPaymentBanking(1);
            return Json(data);
        }

        public async Task<IActionResult> GetLstBankQT()
        {
            var data = await _repoWrapper.UpdateStore.GetListPaymentBanking(2);
            return Json(data);
        }

        public async Task<IActionResult> GetLstBankCK()
        {
            var data = await _repoWrapper.UpdateStore.GetListPaymentBanking(4);
            return Json(data);
        }

        public async Task<IActionResult> PostUrlVNPay(PostUrlVNPay PostUrlVNPay)
        {
            PostUrlVNPay.PostType = 1;
            var data = await _repoWrapper.UpdateStore.PostUrlVNPay(PostUrlVNPay);
            if(PostUrlVNPay.PaymentTypeId == 4)
            {
                if(data.ErrorCode == "00")
                {
                    var result = await _repoWrapper.UpdateStore.GetOrderDetailByOrderCode(PostUrlVNPay.OrderCode);
                    return PartialView("ReturnBanking", result);
                }
                else
                {
                    return Json("Lỗi");
                }
            }
            else
            {
                return Json(data);
            }
        }

        public async Task<IActionResult> ReturnResult(string OrderCode, string ResponseCode)
        {
            var result = await _repoWrapper.UpdateStore.GetOrderDetailByOrderCode(OrderCode);
            ViewBag.ResponseCode = ResponseCode;
            return PartialView("ReturnVnpay", result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateExpVAT(PostPaymentVAT PostPaymentVAT)
        {
            var data = await _repoWrapper.UpdateStore.CreateExpVAT(PostPaymentVAT);
            return Json(data);
        }

        [HttpPost]
        public async Task<IActionResult> PaymentUploadImage(ImageUploadDTO ImageUploadDTO, string OrderCode)
        {
            var data = await _repoWrapper.UpdateStore.PaymentUploadImage(ImageUploadDTO, OrderCode);
            return Json(data);
        }

        public async Task<IActionResult> UpgradePackageBrand(UpgrageBrandPackageDTO UpgrageBrandPackageDTO)
        {
            var data = await _repoWrapper.UpdateStore.UpgradePackageBrand(UpgrageBrandPackageDTO);
            return Json(data);
        }

        public async Task<IActionResult> GetOrderDetailByOrderCode(string OrderCode)
        {
            var data = await _repoWrapper.UpdateStore.GetOrderDetailByOrderCode(OrderCode);
            return PartialView("DetailPayment", data);
        }
    }
}
