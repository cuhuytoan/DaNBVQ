using AutoMapper;
using HNM.DataNC.Models;
using HNM.DataNC.ModelsFilter;
using HNM.DataNC.ModelsNC.ModelsDTO;
using HNM.DataNC.ModelsNC.ModelsUtility;
using HNM.DataNC.ModelsStore;
using HNM.RepositoriesNC.RepositoriesBase;
using HNM.WebApiNC.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HNM.WebApiNC.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase, IDisposable
    {
        private readonly ILoggerManager _logger;
        private IRepositoryWrapper _repoWrapper;
        private IMapper _mapper;
        private readonly IDistributedCache _distributedCache;
        private readonly IConfiguration _configuration;

        public ShoppingCartController(IRepositoryWrapper repoWrapper, ILoggerManager logger, IMapper mapper, IDistributedCache distributedCache, IConfiguration configuration)
        {   
            _repoWrapper = repoWrapper;
            _logger = logger;
            _mapper = mapper;
            _distributedCache = distributedCache;
            _configuration = configuration;
        }

        /// <summary>
        /// Get List Delivery Address
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<DeliveryAddressDTO>> GetLstDeliveryAddress(string UserId)
        {
            var output = new List<DeliveryAddressDTO>();
            output = await _repoWrapper.ShoppingCart.GetLstDeliveryAddress(UserId);
            return output;
        }
        
        /// <summary>
        /// post delivery address
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<DeliveryAddressDTO> PostDeliveryAddress(DeliveryAddressDTO model)
        {
            var output = new DeliveryAddressDTO();
            var result = await _repoWrapper.ShoppingCart.PostDeliveryAddress(model);
            if(result)
            {
                output.ErrorCode = "00";
                output.Message = "Thêm mới địa chỉ thành công";
            }
            else
            {
                output.ErrorCode = "01";
                output.Message = "Có lỗi trong quá trình cập nhật";
            }
            return output;
        }
        
        /// <summary>
        /// Xóa địa chỉ giao hàng
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete]
        public async Task<DeliveryAddressDTO> DeleteDeliveryAddress(int Id,string UserId)
        {
            var output = new DeliveryAddressDTO();
            var result = await _repoWrapper.ShoppingCart.DeleteDeliveryAddress(Id,UserId);
            if(result)
            {
                output.ErrorCode = "00";
                output.Message = "Xóa thành công";
            }
            else
            {
                output.ErrorCode = "01";
                output.Message = "Có lỗi trong quá trình xóa";
            }
            return output;
        }
        /// <summary>
        /// Tạo đơn hàng
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        //[Authorize]
        public async Task<ModelBaseStatus> PostShoppingCart(List<PostShoppingCart> model)
        {
            var output = new ModelBaseStatus();            
            var result = await _repoWrapper.ShoppingCart.PostShopingCart(model);
            if(result.Count != 0)
            {
                output.ErrorCode = "00";
                output.Message = "Đặt hàng thành công";     
                foreach(var p in result)
                {
                    var product = await _repoWrapper.Product.FirstOrDefaultAsync(x => x.Product_ID == p.ProductId);
                    FCMMessageOutputDTO notiModel = new FCMMessageOutputDTO();
                    notiModel.Notification.Title = $"Đơn hàng được tạo mới";
                    notiModel.Notification.Body = $"{DateTime.Now.ToString("dd/MM/yyyy")} Đơn hàng {p.ShopingCartCode} vừa được tạo mới";
                    notiModel.Topic = product?.CreateBy.ToString(); // Thông báo tới gian hàng
                    notiModel.Data.formId = "CART_BUY";
                    notiModel.Data.id = p.Id; // ShoppingCartDetail ID
                    notiModel.Data.notiSpecType = 0;
                    var pushNoti = Utils.Util.SendMessageFirebase(notiModel);

                    FCMMessage fCMMessage = new FCMMessage();
                    fCMMessage.Title = notiModel.Notification.Title;
                    fCMMessage.Body = notiModel.Notification.Body;
                    fCMMessage.CreateBy = product?.CreateBy;
                    fCMMessage.CreateDate = DateTime.Now;
                    fCMMessage.LastEditBy = product?.CreateBy;
                    fCMMessage.LastEditDate = DateTime.Now;
                    fCMMessage.UserID = product?.CreateBy;
                    fCMMessage.Topic = product?.CreateBy.ToString(); // Thông báo tới gian hàng
                    fCMMessage.NotificationType = notiModel.Data.notifyType;
                    fCMMessage.Form_ID = notiModel.Data.formId;
                    fCMMessage.ParameterId = notiModel.Data.id;
                    fCMMessage.FullUrl = notiModel.Data.fullUrl;
                    fCMMessage.FullUrlImage = notiModel.Data.fullUrlImage;
                    fCMMessage.NotiSpecType = 1;
                    fCMMessage.IsPinTop = notiModel.Data.isPinTop;
                    fCMMessage.FormNameApp = notiModel.Data.formAppName;
                    fCMMessage.ProductTypeId = notiModel.Data.typeId;
                    fCMMessage.CategoryId = notiModel.Data.categoryId;                    
                    fCMMessage.FullUrlImage = $"https://hanoma-cdn.s3.cloud.cmctelecom.vn/DataMobile/Notify/notifyDefault.png";
                    fCMMessage.Content = $"{DateTime.Now.ToString("dd/MM/yyyy")} Đơn hàng {p.ShopingCartCode} vừa được tạo mới";
                    fCMMessage.FullUrl = "";


                    fCMMessage.HasRead = 0;
                    await _repoWrapper.FCMMessage.AddNewFCMMessage(fCMMessage);
                }    
                
            }
            else
            {
                output.ErrorCode = "01";
                output.Message = "Có lỗi trong quá trình đặt hàng";
                FCMMessageOutputDTO notiModel = new FCMMessageOutputDTO();
                notiModel.Notification.Title = "Có lỗi trong quá trình đặt hàng";
                notiModel.Notification.Body = "Có lỗi trong quá trình đặt hàng";
                notiModel.Topic = model[0].UserId; // Thông báo tới chính người dùng
                notiModel.Data.formId = "SHOPPINGCARTNSS";
                notiModel.Data.notiSpecType = 0;
                var pushNoti = Utils.Util.SendMessageFirebase(notiModel);

                FCMMessage fCMMessage = new FCMMessage();
                fCMMessage.Title = notiModel.Notification.Title;
                fCMMessage.Body = notiModel.Notification.Body;
                fCMMessage.CreateBy = Guid.Parse(model[0].UserId);
                fCMMessage.CreateDate = DateTime.Now;
                fCMMessage.LastEditBy = Guid.Parse(model[0].UserId);
                fCMMessage.LastEditDate = DateTime.Now;
                fCMMessage.UserID = Guid.Parse(model[0].UserId);
                fCMMessage.Topic = model[0].UserId; // Thông báo tới chính người dùng
                fCMMessage.NotificationType = notiModel.Data.notifyType;
                fCMMessage.Form_ID = notiModel.Data.formId;
                fCMMessage.ParameterId = notiModel.Data.id;
                fCMMessage.FullUrl = notiModel.Data.fullUrl;
                fCMMessage.FullUrlImage = notiModel.Data.fullUrlImage;
                fCMMessage.NotiSpecType = 1;
                fCMMessage.IsPinTop = notiModel.Data.isPinTop;
                fCMMessage.FormNameApp = notiModel.Data.formAppName;
                fCMMessage.ProductTypeId = notiModel.Data.typeId;
                fCMMessage.CategoryId = notiModel.Data.categoryId;                
                fCMMessage.FullUrlImage = $"https://hanoma-cdn.s3.cloud.cmctelecom.vn/DataMobile/Notify/notifyDefault.png";
                fCMMessage.Content = "Có lỗi trong quá trình đặt hàng";
                fCMMessage.FullUrl = "";


                fCMMessage.HasRead = 0;
                await _repoWrapper.FCMMessage.AddNewFCMMessage(fCMMessage);
            }
            return output;
        }
        /// <summary>
        /// Thao tác xử lý đơn hàng StatusCart 1 - Chờ xác nhận 2 Đang xử lý 3 Thành công 4 Đã hủy
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        
        public async Task<ShoppingCartDetailDTO> PutShopingCartAction(ShoppingCartDetailDTO model)
        {
            var output = new ShoppingCartDetailDTO();
            var result = await _repoWrapper.ShoppingCart.PostShopingCartAction(model);
            if(result)
            {
                output.ErrorCode = "00";
                output.Message = "Xử lý thành công";
                var shopingCartDetail = await _repoWrapper.ShoppingCart.FirstOrDefaultAsync(x => x.Id == model.Id);
                var product = await _repoWrapper.Product.FirstOrDefaultAsync(x => x.Product_ID == shopingCartDetail.ProductId);
                string message = "";
                string title = "";
                FCMMessageOutputDTO notiModel = new FCMMessageOutputDTO();
                if (model.StatusCart == 2) // Chờ xác nhận
                {
                    title = "Xác nhận đơn hàng";
                    message = $"Đơn hàng {shopingCartDetail.ShopingCartCode}, đã được cửa hàng xác nhận";
                    notiModel.Data.formId = "CART_SELL";
                    notiModel.Topic = shopingCartDetail.CreateBy.ToString(); // Noti cho người mua
                    notiModel.Data.id = shopingCartDetail.Id;
                    notiModel.Data.notiSpecType = 0;
                }
                else if(model.StatusCart == 3) //Hoàn thành
                {
                    title = "Xác nhận đơn hàng hoàn tất";
                    message = $"Đơn hàng {shopingCartDetail.ShopingCartCode}, đã được hoàn tất";
                    notiModel.Data.formId = "CART_SELL";
                    notiModel.Topic = shopingCartDetail.CreateBy.ToString(); // Noti cho người mua
                    notiModel.Data.id = shopingCartDetail.Id;
                    notiModel.Data.notiSpecType = 0;
                }    
                else if(model.StatusCart == 4) // Đã hủy
                {
                    
                    if(model.IsCancelByBuyer == 1)
                    {
                        title = "Đơn hàng bị hủy";
                        message = $"Đơn hàng {shopingCartDetail.ShopingCartCode}, đã hủy bởi người mua - lý do: {model.ReasonCancel}";
                        notiModel.Data.formId = "CART_BUY";
                        notiModel.Topic = product?.CreateBy.ToString(); // Thông báo tới gian hàng
                        notiModel.Data.id = shopingCartDetail.Id;
                        notiModel.Data.notiSpecType = 0;
                    }    
                    else if (model.IsCancelByBuyer == 0)
                    {
                        title = "Đơn hàng bị hủy";
                        message = $"Đơn hàng {shopingCartDetail.ShopingCartCode}, đã hủy bởi người bán - lý do: {model.ReasonCancel}";
                        notiModel.Data.formId = "CART_SELL";
                        notiModel.Topic = shopingCartDetail.CreateBy.ToString(); // Noti cho người mua
                        notiModel.Data.id = shopingCartDetail.Id;
                        notiModel.Data.notiSpecType = 0;
                    }    
                }    
                
                
                notiModel.Notification.Title = title;
                notiModel.Notification.Body = message;
                //notiModel.Topic = product?.CreateBy.ToString(); // Thông báo tới gian hàng                
                var pushNoti = Utils.Util.SendMessageFirebase(notiModel);

                FCMMessage fCMMessage = new FCMMessage();
                fCMMessage.Title = notiModel.Notification.Title;
                fCMMessage.Body = notiModel.Notification.Body;
                fCMMessage.CreateBy = product?.CreateBy;
                fCMMessage.CreateDate = DateTime.Now;
                fCMMessage.LastEditBy = product?.CreateBy;
                fCMMessage.LastEditDate = DateTime.Now;
                fCMMessage.UserID = product?.CreateBy;
                fCMMessage.Topic = notiModel.Topic;
                fCMMessage.NotificationType = notiModel.Data.notifyType;
                fCMMessage.Form_ID = notiModel.Data.formId;
                fCMMessage.ParameterId = notiModel.Data.id;
                fCMMessage.FullUrl = notiModel.Data.fullUrl;
                fCMMessage.FullUrlImage = notiModel.Data.fullUrlImage;
                fCMMessage.NotiSpecType = 1;
                fCMMessage.IsPinTop = notiModel.Data.isPinTop;
                fCMMessage.FormNameApp = notiModel.Data.formAppName;
                fCMMessage.ProductTypeId = notiModel.Data.typeId;
                fCMMessage.CategoryId = notiModel.Data.categoryId;                
                fCMMessage.FullUrlImage = $"https://hanoma-cdn.s3.cloud.cmctelecom.vn/DataMobile/Notify/notifyDefault.png";
                fCMMessage.Content = message;
                fCMMessage.FullUrl = "";


                fCMMessage.HasRead = 0;
                await _repoWrapper.FCMMessage.AddNewFCMMessage(fCMMessage);
            }
            else
            {
                output.ErrorCode = "01";
                output.Message = "Có lỗi trong quá trình xử lý đơn hàng";
            }
            return output;
        }

        /// <summary>
        /// Get Lịch sử bên mua và bên bán
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<HistoryShopCart> GetLstHistoryShoppingCart([FromQuery]HistoryShoppingCart model)
        {
            var output = new HistoryShopCart();
            output.Data = await _repoWrapper.ShoppingCart.GetHistoryShopingCart(model);
            CountHistoryShoppingCart countModel = new CountHistoryShoppingCart();
            countModel.UserId = model.UserId;
            countModel.ProductBrandId = model.ProductBrandId;
            countModel.ShopingCartCode = model.ShopingCartCode;
            countModel.FromDate = model.FromDate;
            countModel.ToDate = model.ToDate;
            output.HistoryCount = await _repoWrapper.ShoppingCart.GetCountHistoryShopingCart(countModel);
            return output;
        }

        /// <summary>
        /// Get Detail shopping Cart
        /// </summary>
        /// <param name="ShopingCartCode"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<HistoryShopingCart_Result> GetDetailShoppingCart(string ShopingCartCode,int? Id)
        {
            var output = new HistoryShopingCart_Result();
            if(!String.IsNullOrEmpty(ShopingCartCode))
            {
                output = await _repoWrapper.ShoppingCart.GetDetailShoppingCart(ShopingCartCode);
            }
            else
            {
                if(Id != null)
                {
                    var spCode = await _repoWrapper.ShoppingCart.FirstOrDefaultAsync(p=>p.Id == Id);
                    if(spCode !=null)
                    {
                        output = await _repoWrapper.ShoppingCart.GetDetailShoppingCart(spCode.ShopingCartCode);
                    }    
                }    
            }
            
            return output;
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
