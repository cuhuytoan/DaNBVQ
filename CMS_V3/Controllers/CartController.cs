using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using HNM.CommonNC;
using HNM.DataNC.ModelsFilter;
using HNM.DataNC.ModelsNC.ModelsDTO;
using HNM.RepositoriesWeb.RepositoriesBase;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using X.PagedList;

namespace CMS_V3.Controllers
{
    public class CartController : Controller
    {
        private readonly ILogger<CartController> _logger;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IRepositoryWrapper _repoWrapper;
        private IMapper _mapper;

        public CartController(ILogger<CartController> logger, IHttpClientFactory clientFactory, IMapper mapper, IRepositoryWrapper repoWrapper)
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
        public IActionResult Index()
        {
            return View();
        }

        // Key lưu chuỗi json của Cart
        public const string CARTKEY = "cart";

        // Lấy cart từ Session (danh sách CartItem)
        //public List<CartItemDTO> GetCartItems()
        //{
        //    var cookie = Request.Cookies["cart"];
        //    //string jsoncart = cookie.GetString(CARTKEY);
        //    //if (jsoncart != null)
        //    //{
        //    //    return JsonConvert.DeserializeObject<List<CartItem>>(jsoncart);
        //    //}
        //    return new List<CartItem>();
        //}

        // Xóa cart khỏi session
        void ClearCart()
        {
            var session = HttpContext.Session;
            session.Remove(CARTKEY);
        }

        // Lưu Cart (Danh sách CartItem) vào session
        void SaveCartSession(List<CartItemDTO> ls)
        {
            var session = HttpContext.Session;
            //string jsoncart = JsonConvert.SerializeObject(ls);
            //session.SetString(CARTKEY, jsoncart);
        }


        public async Task<JsonResult> AddToCart(int ProductId)
        {
            var result = await _repoWrapper.Product.GetProductDetails(ProductId);
            if (result == null)
            {
                return Json("Không có sản phẩm");
            }
            else
            {
                //var cart = GetCartItems();
                //var cartitem = cart.Find(p => p.product.ProductId == productid);
                //if (cartitem != null)
                //{
                //    // Đã tồn tại, tăng thêm 1
                //    cartitem.quantity++;
                //}
                //else
                //{
                //    //  Thêm mới
                //    cart.Add(new CartItem() { quantity = 1, product = product });
                //}

                //// Lưu cart vào Session
                //SaveCartSession(cart);
                return Json("Đã thêm vào giỏ hàng");
            }
        }

        public IActionResult RemoveCart(int productid)
        {
            return RedirectToAction();
        }

        public IActionResult Cart()
        {
            return View();
        }

        public IActionResult CheckOut()
        {
            // Xử lý khi đặt hàng
            return View();
        }
    }
}
