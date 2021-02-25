using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using HNM.DataNC.Models;
using HNM.DataNC.ModelsNC.ModelsDTO;
using HNM.DataNC.ModelsNC.ModelsUtility;
using HNM.DataNC.ModelsPayment;
using HNM.RepositoriesNC.PaymentRepositoriesBase;
using HNM.RepositoriesNC.RepositoriesBase;
using HNM.WebApiNC.Logging;
using HNM.WebApiNC.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace HNM.WebApiNC.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class PaymentController : ControllerBase, IDisposable
    {
        private readonly ILoggerManager _logger;
        private IPaymentRepositoryWrapper _repoPaymentWrapper;
        private IRepositoryWrapper _repoWrapper;
        private IMapper _mapper;
        private readonly IDistributedCache _distributedCache;
        public PaymentController(IPaymentRepositoryWrapper repoPaymentWrapper, ILoggerManager logger, IMapper mapper, IDistributedCache distributedCache, IRepositoryWrapper repoWrapper)
        {
            _repoPaymentWrapper = repoPaymentWrapper;
            _repoWrapper = repoWrapper;
            _logger = logger;
            _mapper = mapper;
            _distributedCache = distributedCache;
        }
        /// <summary>
        /// Get List Payment Type
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<PaymentType>> GetLstPaymentType()
        {
            var output = new List<PaymentType>();
            var cacheKey = $"GetLstPaymentType";
            var redisEncode = await _distributedCache.GetStringAsync(cacheKey);
            if (redisEncode != null)
            {
                output = JsonConvert.DeserializeObject<List<PaymentType>>(redisEncode);
            }
            else
            {
                output = await _repoPaymentWrapper.PaymentType.GetlstPaymentType();
                await _distributedCache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(output), Utils.Util.RedisOptions());
            }

            return output;
        }
        /// <summary>
        /// ProductBrandId
        /// </summary>
        /// <param name="ProductBrandId"></param>
        /// <param name="ServicesId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<PaymentPackageDTO> GetBrandPackage(int? ProductBrandId)
        {
            var output = new PaymentPackageDTO();
            var brand = await _repoWrapper.Brand.FirstOrDefaultAsync(p => p.ProductBrand_ID == ProductBrandId);
            if(brand!= null)
            {
                if(brand.ProductBrandType_ID == 2)
                {
                    var orderRemain = await _repoPaymentWrapper.PaymentUpgrade.GetOrderRemain(brand.ProductBrand_ID);
                    if (orderRemain != null)
                    {
                        output.MonthRemain = orderRemain.MonthRemain;
                        output.MonthPrice = orderRemain.MonthPrice;
                        output.TotalDeduct = orderRemain.TotalDeduct;
                        
                    }
                }    
                var lstServies = await _repoPaymentWrapper.PaymentServcie.GetServices();
                var lstDiscount = await _repoPaymentWrapper.PaymentDisCount.GetlstDiscountConfig();
                output.Package = _mapper.Map<List<ServicesDTO>>(lstServies);
                output.MonthDisCount = _mapper.Map<List<DiscountConfigDTO>>(lstDiscount);
                output.ProductBrandTypeId = (int)brand.ProductBrandType_ID;
            }    
            
            return output;
        }
        /// <summary>
        /// GetList Payment Banking by Type
        /// </summary>
        /// <param name="PaymentTypeId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<PaymentBanking>> GetListPaymentBanking(int PaymentTypeId)
        {
            var output = new List<PaymentBanking>();
            output = await _repoPaymentWrapper.PaymentBanking.GetlstPaymentBankingByType(PaymentTypeId);
            return output;
        }


        [HttpPost]
        [Authorize]
        public async Task<ModelBaseStatus> PostUrlVNPay(PostUrlVNPay model)
        {
            _logger.LogDebug($"PostUrlVNPay model: {JsonConvert.SerializeObject(model)}");
            var output = new ModelBaseStatus();
            var paymentSetting = await _repoPaymentWrapper.PaymentSetting.GetPaymentSetting();
            if (paymentSetting != null)
            {
                //Get Config Info
                string vnp_Returnurl = "";
                if(model.PostType == 1) // return for web
                {
                    vnp_Returnurl = paymentSetting.vnp_Returnurl_Web;//URL nhan ket qua tra ve 
                }
                else
                {
                    vnp_Returnurl = paymentSetting.vnp_Returnurl;//URL nhan ket qua tra ve 
                }                
                string vnp_Url = paymentSetting.vnp_Url; //URL thanh toan cua VNPAY 
                string vnp_TmnCode = paymentSetting.vnp_TmnCode; //Ma website
                string vnp_HashSecret = paymentSetting.vnp_HashSecret; //Chuoi bi mat
                //Update Payment TypeID
                 await _repoPaymentWrapper.PaymentUpgrade.UpgradePaymentType(model.OrderCode,model.PaymentTypeId);

                //Get OrderID
                var orderDetail = await _repoPaymentWrapper.PaymentUpgrade.GetOrderDetailByOrderCode(model.OrderCode);
                if (orderDetail == null) return output;
                //Get Order Master 
                var orderMaster = await _repoPaymentWrapper.PaymentUpgrade.GetOrderMasterByOrderCode(model.OrderCode);
                //Gửi thông báo  trong trường hợp chuyển khoản
                if(model.PaymentTypeId ==4 )
                {
                    string msgSS = "Đơn hàng #order đang chờ thanh toán và có hiệu lực trong 72 giờ tới. Chúng tôi sẽ nâng cấp gian hàng của bạn trong vòng 24h ngay sau khi nhận được thanh toán của bạn. Cảm ơn bạn đã tham gia cùng chúng tôi.";
                    var user = await _repoWrapper.AspNetUsers.FirstOrDefaultAsync(p => p.Id == orderMaster.CreateBy);
                    msgSS = msgSS.Replace("#name", user.UserName);
                    msgSS = msgSS.Replace("#order", model.OrderCode);

                    FCMMessageOutputDTO notiModel = new FCMMessageOutputDTO();
                    notiModel.Notification.Title = "Nâng cấp gian hàng bằng chuyển khoản";
                    notiModel.Notification.Body = msgSS;
                    notiModel.Topic = orderMaster?.CreateBy;
                    notiModel.Data.formId = "PAYMENTWAIT";
                    notiModel.Data.OrderCode = model.OrderCode;
                    var pushNoti = Utils.Util.SendMessageFirebase(notiModel);

                    FCMMessage fCMMessage = new FCMMessage();
                    fCMMessage.Title = notiModel.Notification.Title;
                    fCMMessage.Body = notiModel.Notification.Body;
                    fCMMessage.CreateBy = Guid.Parse(orderMaster?.CreateBy);
                    fCMMessage.CreateDate = DateTime.Now;
                    fCMMessage.LastEditBy = Guid.Parse(orderMaster?.CreateBy);
                    fCMMessage.LastEditDate = DateTime.Now;
                    fCMMessage.UserID = Guid.Parse(orderMaster?.CreateBy);
                    fCMMessage.Topic = notiModel.Topic;
                    fCMMessage.NotificationType = notiModel.Data.notifyType;
                    fCMMessage.Form_ID = notiModel.Data.formId;
                    fCMMessage.ParameterId = notiModel.Data.id;
                    fCMMessage.FullUrl = notiModel.Data.fullUrl;
                    fCMMessage.FullUrlImage = notiModel.Data.fullUrlImage;
                    fCMMessage.NotiSpecType = notiModel.Data.notiSpecType;
                    fCMMessage.IsPinTop = notiModel.Data.isPinTop;
                    fCMMessage.FormNameApp = notiModel.Data.formAppName;
                    fCMMessage.ProductTypeId = notiModel.Data.typeId;
                    fCMMessage.CategoryId = notiModel.Data.categoryId;
                    fCMMessage.NotiSpecType = 1;
                    fCMMessage.FullUrlImage = $"https://hanoma-cdn.s3.cloud.cmctelecom.vn/DataMobile/Notify/notifyDefault.png";
                    fCMMessage.Content = msgSS;
                    fCMMessage.FullUrl = "https://hanoma.vn/ShopMan/DashBoard";


                    fCMMessage.HasRead = 0;
                    await _repoWrapper.FCMMessage.AddNewFCMMessage(fCMMessage);
                }    
                

                //Get payment input
                OrderInfo order = new OrderInfo();
                //Save order to db
                //order.OrderId = model.OrderCode;
                order.OrderId = model.OrderCode;
                order.Amount = Convert.ToInt64(orderDetail.Total);
                order.OrderDescription = orderDetail.Description;
                order.CreatedDate = DateTime.Now;

                //Build URL for VNPAY
                VnPayLibrary vnpay = new VnPayLibrary();

                vnpay.AddRequestData("vnp_Version", "2.0.0");
                vnpay.AddRequestData("vnp_Command", "pay");
                vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
                vnpay.AddRequestData("vnp_Locale", "vn");
                vnpay.AddRequestData("vnp_CurrCode", "VND");
                vnpay.AddRequestData("vnp_TxnRef", order.OrderId);
                vnpay.AddRequestData("vnp_OrderInfo", order.OrderDescription);
                vnpay.AddRequestData("vnp_OrderType", "billpayment"); //default value: other
                vnpay.AddRequestData("vnp_Amount", (order.Amount *100).ToString());
                vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
                vnpay.AddRequestData("vnp_IpAddr", model.IPAddress);
                vnpay.AddRequestData("vnp_CreateDate", order.CreatedDate.ToString("yyyyMMddHHmmss"));
                if(!String.IsNullOrEmpty(model.BankCode))
                {
                    vnpay.AddRequestData("vnp_BankCode", model.BankCode);
                }    
                string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
                output.ErrorCode = "00";
                output.Message = paymentUrl;
                return output;
            }
            else
            {
                output.ErrorCode = "01";
                output.Message = "Có lỗi trong quá trình gọi URL";
                return output;
            }
            

        }
        /// <summary>
        /// get Order Detail by Order Code
        /// </summary>
        /// <param name="OrderCode"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<OrderDetailDTO> GetOrderDetailByOrderCode(string OrderCode)
        {
            var output = new OrderDetailDTO();
            try
            {
                var result = await _repoPaymentWrapper.PaymentUpgrade.GetOrderDetailByOrderCode(OrderCode);
                if(result != null)
                {
                    output = _mapper.Map<OrderDetailDTO>(result);
                }
                else
                {
                    output.ErrorCode = "01";
                    output.Message = $"Không tồn tại order {OrderCode}";
                }
            }
            catch(Exception ex)
            {

            }
            return output;
        }
        /// <summary>
        /// Get result from VNPay
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<HNM.DataNC.ModelsNC.ModelsUtility.ModelPaymentStatus> ReceivePayment()
        {
            _logger.LogDebug($"ReceivePayment QueryString: {Request.QueryString}");
            var output = new ModelPaymentStatus();
            var orderMaster = new Order();
            var orderDetail = new OrderDetail();
            var paymentSetting = await _repoPaymentWrapper.PaymentSetting.GetPaymentSetting();
            if (paymentSetting != null)
            {
                try
                {
                    //Get Config Info
                    string vnp_Returnurl = paymentSetting.vnp_Returnurl;//URL nhan ket qua tra ve 
                    string vnp_Url = paymentSetting.vnp_Url; //URL thanh toan cua VNPAY 
                    string vnp_TmnCode = paymentSetting.vnp_TmnCode; //Ma website
                    string vnp_HashSecret = paymentSetting.vnp_HashSecret; //Chuoi bi mat

                    if (Request.QueryString.HasValue)
                    {
                        var vnpayData = Request.QueryString;
                        VnPayLibrary vnpay = new VnPayLibrary();
                        if (vnpayData.HasValue)
                        {
                            var qscoll = System.Web.HttpUtility.ParseQueryString(vnpayData.ToString());

                            foreach (string s in qscoll)
                            {
                                //get all querystring data
                                if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                                {
                                    vnpay.AddResponseData(s, qscoll[s]);
                                }
                            }
                            // }

                            //vnp_TxnRef: Ma don hang merchant gui VNPAY tai command=pay    
                            string orderId = vnpay.GetResponseData("vnp_TxnRef");
                            long amount = Convert.ToInt64(vnpay.GetResponseData("vnp_Amount")) / 100;
                            orderDetail = await _repoPaymentWrapper.PaymentUpgrade.GetOrderDetailByOrderCode(orderId);
                            if (orderDetail == null)
                            {
                                output.RspCode = "01";
                                output.Message = "Lỗi! Order không tồn tại.Mã lỗi: 01";
                                return output;
                            }
                            orderMaster = await _repoPaymentWrapper.PaymentUpgrade.GetOrderMasterByOrderCode(orderDetail.OrderCode);
                            //vnp_TransactionNo: Ma GD tai he thong VNPAY
                            long vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
                            //vnp_ResponseCode:Response code from VNPAY: 00: Thanh cong, Khac 00: Xem tai lieu
                            string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
                            //Description 

                            //vnp_SecureHash: MD5 cua du lieu tra ve
                            String vnp_SecureHash = qscoll["vnp_SecureHash"];
                            bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
                            if (checkSignature)
                            {
                                if (orderMaster != null)
                                {
                                    if (orderMaster.Total == amount)
                                    {
                                        if (orderMaster.PaymentStatus_ID == 0) // Chờ thanh toán
                                        {
                                            if (vnp_ResponseCode == "00")
                                            {
                                                //Thanh toan thanh cong
                                                output.RspCode = vnp_ResponseCode;
                                                output.Message = $"Thanh toán thành công! OrderId={orderId} VNPay TranId = {vnpayTranId}";
                                                if (orderMaster.ProductBrand_ID != null && orderMaster.Service_ID != null)
                                                {
                                                    await _repoWrapper.Brand.UpgradePackageBrand(orderMaster.ProductBrand_ID ?? 0, orderMaster.Service_ID ?? 0);
                                                    await _repoPaymentWrapper.PaymentUpgrade.FinishUpgradeBrand(orderDetail.OrderCode, 1);
                                                    //Update to Payment Log
                                                    var orderInfo = await _repoPaymentWrapper.PaymentUpgrade.GetOrderMasterByOrderCode(orderDetail.OrderCode);
                                                    var paymentLog = new PaymentLog();
                                                    //vnp_Amount: Lấy ra đơn giá    
                                                    paymentLog.Amount = Convert.ToDecimal(vnpay.GetResponseData("vnp_Amount"));
                                                    paymentLog.Order_ID = orderMaster.Order_ID;
                                                    paymentLog.BankCode = vnpay.GetResponseData("vnp_BankCode");
                                                    paymentLog.BankTranNo = vnpay.GetResponseData("vnp_BankTranNo");
                                                    paymentLog.CardType = vnpay.GetResponseData("vnp_CardType");
                                                    paymentLog.OrderInfo = vnpay.GetResponseData("vnp_OrderInfo");
                                                    paymentLog.TransactionNo = vnpay.GetResponseData("vnp_TransactionNo");
                                                    paymentLog.ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
                                                    paymentLog.TxnRef = vnpay.GetResponseData("vnp_TxnRef");
                                                    paymentLog.PaymentStatus_ID = orderInfo.PaymentStatus_ID;
                                                    paymentLog.PaymentType_ID = orderInfo.PaymentType_ID;
                                                    paymentLog.ProductBrand_ID = orderInfo.ProductBrand_ID;
                                                    paymentLog.ProductBrandName = orderInfo.ProductBrandName;
                                                    paymentLog.UserName = orderInfo.SalesUserName;
                                                    paymentLog.CreateBy = orderInfo.CreateBy;
                                                    paymentLog.CreateDate = DateTime.Now;
                                                    await _repoPaymentWrapper.PaymentUpgrade.SavePaymentLog(paymentLog);
                                                }

                                                string msgSS = "Xin chào #name, Chúc mừng gian hàng của bạn đã nâng cấp lên quy mô #quymo, thời gian bắt đầu từ #startdate thời gian hết hạn #enddate. Bấm vào đây để vào quản lý gian hàng";
                                                var user = await _repoWrapper.AspNetUsers.FirstOrDefaultAsync(p => p.Id == orderMaster.CreateBy);
                                                msgSS = msgSS.Replace("#name", user.UserName);
                                                msgSS = msgSS.Replace("#quymo", orderMaster.Service_ID == 2 ? "Vừa" : "Lớn");
                                                msgSS = msgSS.Replace("#startdate", orderDetail.StartDate?.ToString("dd/MM/yyyy"));
                                                msgSS = msgSS.Replace("#enddate", orderDetail.EndDate?.ToString("dd/MM/yyyy"));
                                                FCMMessageOutputDTO notiModel = new FCMMessageOutputDTO();
                                                notiModel.Notification.Title = "Nâng cấp gian hàng thành công";
                                                notiModel.Notification.Body = msgSS;
                                                notiModel.Topic = orderMaster?.CreateBy;
                                                notiModel.Data.formId = "PAYMENTSS";
                                                notiModel.Data.OrderCode = orderId;
                                                var pushNoti = Utils.Util.SendMessageFirebase(notiModel);

                                                FCMMessage fCMMessage = new FCMMessage();
                                                fCMMessage.Title = notiModel.Notification.Title;
                                                fCMMessage.Body = notiModel.Notification.Body;
                                                fCMMessage.CreateBy = Guid.Parse(orderMaster?.CreateBy);
                                                fCMMessage.CreateDate = DateTime.Now;
                                                fCMMessage.LastEditBy = Guid.Parse(orderMaster?.CreateBy);
                                                fCMMessage.LastEditDate = DateTime.Now;
                                                fCMMessage.UserID = Guid.Parse(orderMaster?.CreateBy);
                                                fCMMessage.Topic = notiModel.Topic;
                                                fCMMessage.NotificationType = notiModel.Data.notifyType;
                                                fCMMessage.Form_ID = notiModel.Data.formId;
                                                fCMMessage.ParameterId = notiModel.Data.id;
                                                fCMMessage.FullUrl = notiModel.Data.fullUrl;
                                                fCMMessage.FullUrlImage = notiModel.Data.fullUrlImage;
                                                fCMMessage.NotiSpecType = notiModel.Data.notiSpecType;
                                                fCMMessage.IsPinTop = notiModel.Data.isPinTop;
                                                fCMMessage.FormNameApp = notiModel.Data.formAppName;
                                                fCMMessage.ProductTypeId = notiModel.Data.typeId;
                                                fCMMessage.CategoryId = notiModel.Data.categoryId;
                                                fCMMessage.NotiSpecType = 1;
                                                fCMMessage.FullUrlImage = $"https://hanoma-cdn.s3.cloud.cmctelecom.vn/DataMobile/Notify/notifyDefault.png";
                                                fCMMessage.Content = msgSS;
                                                fCMMessage.FullUrl = "https://hanoma.vn/ShopMan/DashBoard";


                                                fCMMessage.HasRead = 0;
                                                await _repoWrapper.FCMMessage.AddNewFCMMessage(fCMMessage);
                                                return output;
                                                //log.InfoFormat("Thanh toan thanh cong, OrderId={0}, VNPAY TranId={1}", orderId, vnpayTranId);
                                            }
                                            else
                                            {
                                                //Thanh toan khong thanh cong. Ma loi: vnp_ResponseCode
                                                //vnp_ResponseCode != 00, xác nhận 
                                                //giao dịch không thành công / lỗi tại VNPAY.
                                                output.RspCode = "00";
                                                output.Message = "Confirm Success";

                                                if (orderMaster.ProductBrand_ID != null && orderMaster.Service_ID != null)
                                                {
                                                    await _repoPaymentWrapper.PaymentUpgrade.FinishUpgradeBrand(orderDetail.OrderCode, -1);
                                                }
                                                FCMMessageOutputDTO notiModel = new FCMMessageOutputDTO();
                                                notiModel.Notification.Title = "Nâng cấp gian hàng không thành công";
                                                notiModel.Notification.Body = "Nâng cấp gian hàng không thành công";
                                                notiModel.Topic = orderMaster?.CreateBy;
                                                notiModel.Data.formId = "PAYMENTNSS";
                                                notiModel.Data.OrderCode = orderId;
                                                var pushNoti = Utils.Util.SendMessageFirebase(notiModel);

                                                FCMMessage fCMMessage = new FCMMessage();
                                                fCMMessage.Title = notiModel.Notification.Title;
                                                fCMMessage.Body = notiModel.Notification.Body;
                                                fCMMessage.CreateBy = Guid.Parse(orderMaster?.CreateBy);
                                                fCMMessage.CreateDate = DateTime.Now;
                                                fCMMessage.LastEditBy = Guid.Parse(orderMaster?.CreateBy);
                                                fCMMessage.LastEditDate = DateTime.Now;
                                                fCMMessage.UserID = Guid.Parse(orderMaster?.CreateBy);
                                                fCMMessage.Topic = notiModel.Topic;
                                                fCMMessage.NotificationType = notiModel.Data.notifyType;
                                                fCMMessage.Form_ID = notiModel.Data.formId;
                                                fCMMessage.ParameterId = notiModel.Data.id;
                                                fCMMessage.FullUrl = notiModel.Data.fullUrl;
                                                fCMMessage.FullUrlImage = notiModel.Data.fullUrlImage;
                                                fCMMessage.NotiSpecType = notiModel.Data.notiSpecType;
                                                fCMMessage.IsPinTop = notiModel.Data.isPinTop;
                                                fCMMessage.FormNameApp = notiModel.Data.formAppName;
                                                fCMMessage.ProductTypeId = notiModel.Data.typeId;
                                                fCMMessage.CategoryId = notiModel.Data.categoryId;
                                                fCMMessage.NotiSpecType = 1;
                                                fCMMessage.FullUrlImage = $"https://hanoma-cdn.s3.cloud.cmctelecom.vn/DataMobile/Notify/notifyDefault.png";
                                                fCMMessage.FullUrl = "https://hanoma.vn/ShopMan/DashBoard";
                                                fCMMessage.HasRead = 0;
                                                fCMMessage.Content = "Nâng cấp gian hàng không thành công";
                                                await _repoWrapper.FCMMessage.AddNewFCMMessage(fCMMessage);
                                                return output;
                                                //log.InfoFormat("Thanh toan loi, OrderId={0}, VNPAY TranId={1},ResponseCode={2}", orderId, vnpayTranId, vnp_ResponseCode);
                                            }
                                        }
                                        else
                                        {
                                            output.RspCode = "02";
                                            output.Message = "Lỗi! Đơn hàng đã được xác nhận.Mã lỗi: " + vnp_ResponseCode;
                                            if (orderMaster.ProductBrand_ID != null && orderMaster.Service_ID != null)
                                            {
                                                await _repoPaymentWrapper.PaymentUpgrade.FinishUpgradeBrand(orderDetail.OrderCode, -1);
                                            }
                                            FCMMessageOutputDTO notiModel = new FCMMessageOutputDTO();
                                            notiModel.Notification.Title = "Nâng cấp gian hàng không thành công";
                                            notiModel.Notification.Body = "Nâng cấp gian hàng không thành công";
                                            notiModel.Topic = orderMaster?.CreateBy;
                                            notiModel.Data.formId = "PAYMENTNSS";
                                            notiModel.Data.OrderCode = orderId;
                                            var pushNoti = Utils.Util.SendMessageFirebase(notiModel);

                                            FCMMessage fCMMessage = new FCMMessage();
                                            fCMMessage.Title = notiModel.Notification.Title;
                                            fCMMessage.Body = notiModel.Notification.Body;
                                            fCMMessage.CreateBy = Guid.Parse(orderMaster?.CreateBy);
                                            fCMMessage.CreateDate = DateTime.Now;
                                            fCMMessage.LastEditBy = Guid.Parse(orderMaster?.CreateBy);
                                            fCMMessage.LastEditDate = DateTime.Now;
                                            fCMMessage.UserID = Guid.Parse(orderMaster?.CreateBy);
                                            fCMMessage.Topic = notiModel.Topic;
                                            fCMMessage.NotificationType = notiModel.Data.notifyType;
                                            fCMMessage.Form_ID = notiModel.Data.formId;
                                            fCMMessage.ParameterId = notiModel.Data.id;
                                            fCMMessage.FullUrl = notiModel.Data.fullUrl;
                                            fCMMessage.FullUrlImage = notiModel.Data.fullUrlImage;
                                            fCMMessage.NotiSpecType = notiModel.Data.notiSpecType;
                                            fCMMessage.IsPinTop = notiModel.Data.isPinTop;
                                            fCMMessage.FormNameApp = notiModel.Data.formAppName;
                                            fCMMessage.ProductTypeId = notiModel.Data.typeId;
                                            fCMMessage.CategoryId = notiModel.Data.categoryId;
                                            fCMMessage.NotiSpecType = 1;
                                            fCMMessage.FullUrlImage = $"https://hanoma-cdn.s3.cloud.cmctelecom.vn/DataMobile/Notify/notifyDefault.png";
                                            fCMMessage.FullUrl = "https://hanoma.vn/ShopMan/DashBoard";
                                            fCMMessage.HasRead = 0;
                                            fCMMessage.Content = "Nâng cấp gian hàng không thành công";
                                            await _repoWrapper.FCMMessage.AddNewFCMMessage(fCMMessage);
                                            return output;
                                        }
                                    }
                                    else
                                    {
                                        output.RspCode = "04";
                                        output.Message = "Lỗi! Số tiền không hợp lệ.Mã lỗi: " + vnp_ResponseCode;

                                        FCMMessageOutputDTO notiModel = new FCMMessageOutputDTO();
                                        notiModel.Notification.Title = "Nâng cấp gian hàng không thành công";
                                        notiModel.Notification.Body = "Nâng cấp gian hàng không thành công";
                                        notiModel.Topic = orderMaster?.CreateBy;
                                        notiModel.Data.formId = "PAYMENTNSS";
                                        notiModel.Data.OrderCode = orderId;
                                        var pushNoti = Utils.Util.SendMessageFirebase(notiModel);

                                        FCMMessage fCMMessage = new FCMMessage();
                                        fCMMessage.Title = notiModel.Notification.Title;
                                        fCMMessage.Body = notiModel.Notification.Body;
                                        fCMMessage.CreateBy = Guid.Parse(orderMaster?.CreateBy);
                                        fCMMessage.CreateDate = DateTime.Now;
                                        fCMMessage.LastEditBy = Guid.Parse(orderMaster?.CreateBy);
                                        fCMMessage.LastEditDate = DateTime.Now;
                                        fCMMessage.UserID = Guid.Parse(orderMaster?.CreateBy);
                                        fCMMessage.Topic = notiModel.Topic;
                                        fCMMessage.NotificationType = notiModel.Data.notifyType;
                                        fCMMessage.Form_ID = notiModel.Data.formId;
                                        fCMMessage.ParameterId = notiModel.Data.id;
                                        fCMMessage.FullUrl = notiModel.Data.fullUrl;
                                        fCMMessage.FullUrlImage = notiModel.Data.fullUrlImage;
                                        fCMMessage.NotiSpecType = notiModel.Data.notiSpecType;
                                        fCMMessage.IsPinTop = notiModel.Data.isPinTop;
                                        fCMMessage.FormNameApp = notiModel.Data.formAppName;
                                        fCMMessage.ProductTypeId = notiModel.Data.typeId;
                                        fCMMessage.CategoryId = notiModel.Data.categoryId;
                                        fCMMessage.NotiSpecType = 1;
                                        fCMMessage.FullUrlImage = $"https://hanoma-cdn.s3.cloud.cmctelecom.vn/DataMobile/Notify/notifyDefault.png";
                                        fCMMessage.FullUrl = "https://hanoma.vn/ShopMan/DashBoard";
                                        fCMMessage.HasRead = 0;
                                        fCMMessage.Content = "Nâng cấp gian hàng không thành công";
                                        await _repoWrapper.FCMMessage.AddNewFCMMessage(fCMMessage);
                                        return output;
                                    }
                                }
                                else
                                {
                                    output.RspCode = "01";
                                    output.Message = "Lỗi! Order không tồn tại.Mã lỗi: " + vnp_ResponseCode;
                                    return output;
                                }

                            }
                            else
                            {
                                output.RspCode = "97";
                                output.Message = "Chữ kí không hợp lệ";


                                FCMMessageOutputDTO notiModel = new FCMMessageOutputDTO();
                                notiModel.Notification.Title = "Nâng cấp không thành công";
                                notiModel.Notification.Body = "Nâng cấp không thành công";
                                notiModel.Topic = orderMaster?.CreateBy;
                                notiModel.Data.formId = "PAYMENTNSS";
                                notiModel.Data.OrderCode = orderId;
                                var pushNoti = Utils.Util.SendMessageFirebase(notiModel);

                                FCMMessage fCMMessage = new FCMMessage();
                                fCMMessage.Title = notiModel.Notification.Title;
                                fCMMessage.Body = notiModel.Notification.Body;
                                fCMMessage.CreateBy = Guid.Parse(orderMaster?.CreateBy);
                                fCMMessage.CreateDate = DateTime.Now;
                                fCMMessage.LastEditBy = Guid.Parse(orderMaster?.CreateBy);
                                fCMMessage.LastEditDate = DateTime.Now;
                                fCMMessage.UserID = Guid.Parse(orderMaster?.CreateBy);
                                fCMMessage.Topic = notiModel.Topic;
                                fCMMessage.NotificationType = notiModel.Data.notifyType;
                                fCMMessage.Form_ID = notiModel.Data.formId;
                                fCMMessage.ParameterId = notiModel.Data.id;
                                fCMMessage.FullUrl = notiModel.Data.fullUrl;
                                fCMMessage.FullUrlImage = notiModel.Data.fullUrlImage;
                                fCMMessage.NotiSpecType = notiModel.Data.notiSpecType;
                                fCMMessage.IsPinTop = notiModel.Data.isPinTop;
                                fCMMessage.FormNameApp = notiModel.Data.formAppName;
                                fCMMessage.ProductTypeId = notiModel.Data.typeId;
                                fCMMessage.CategoryId = notiModel.Data.categoryId;
                                fCMMessage.NotiSpecType = 1;
                                fCMMessage.FullUrlImage = $"https://hanoma-cdn.s3.cloud.cmctelecom.vn/DataMobile/Notify/notifyDefault.png";
                                fCMMessage.FullUrl = "https://hanoma.vn/ShopMan/DashBoard";
                                fCMMessage.HasRead = 0;
                                fCMMessage.Content = "Nâng cấp gian hàng không thành công";
                                await _repoWrapper.FCMMessage.AddNewFCMMessage(fCMMessage);
                                return output;
                            }
                        }
                    }
                    else
                    {
                        output.RspCode = "99";
                        output.Message = "Có lỗi trong quá trình thanh toán";
                        return output;
                    }
                }
                catch
                {
                    output.RspCode = "99";
                    output.Message = "Có lỗi trong quá trình thanh toán";
                    return output;
                }
                
            }
            else
            {
                output.RspCode = "99";
                output.Message = "Có lỗi trong quá trình thanh toán";                
                return output;
            }
            //Sent Message
            
            return output;
        }
        /// <summary>
        /// Return Url
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<HNM.DataNC.ModelsNC.ModelsUtility.ModelPaymentStatus> PaymentReturnUrl()
        {
            _logger.LogDebug($"ReceivePayment QueryString: {Request.QueryString}");
            var output = new ModelPaymentStatus();
            var paymentSetting = await _repoPaymentWrapper.PaymentSetting.GetPaymentSetting();
            if (paymentSetting != null)
            {
                //Get Config Info
                string vnp_Returnurl = paymentSetting.vnp_Returnurl;//URL nhan ket qua tra ve 
                string vnp_Url = paymentSetting.vnp_Url; //URL thanh toan cua VNPAY 
                string vnp_TmnCode = paymentSetting.vnp_TmnCode; //Ma website
                string vnp_HashSecret = paymentSetting.vnp_HashSecret; //Chuoi bi mat

                if (Request.QueryString.HasValue)
                {
                    var vnpayData = Request.QueryString;
                    VnPayLibrary vnpay = new VnPayLibrary();
                    if (vnpayData.HasValue)
                    {
                        var qscoll = System.Web.HttpUtility.ParseQueryString(vnpayData.ToString());

                        foreach (string s in qscoll)
                        {
                            //get all querystring data
                            if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                            {
                                vnpay.AddResponseData(s, qscoll[s]);
                            }
                        }
                        // }

                        //vnp_TxnRef: Ma don hang merchant gui VNPAY tai command=pay    
                        string orderId = vnpay.GetResponseData("vnp_TxnRef");

                        var orderDetail = await _repoPaymentWrapper.PaymentUpgrade.GetOrderDetailByOrderCode(orderId);
                        if (orderDetail == null) return output;
                        var orderMaster = await _repoPaymentWrapper.PaymentUpgrade.GetOrderMasterByOrderCode(orderDetail.OrderCode);
                        //vnp_TransactionNo: Ma GD tai he thong VNPAY
                        long vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
                        //vnp_ResponseCode:Response code from VNPAY: 00: Thanh cong, Khac 00: Xem tai lieu
                        string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
                        //Description 

                        //vnp_SecureHash: MD5 cua du lieu tra ve
                        String vnp_SecureHash = qscoll["vnp_SecureHash"];
                        bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
                        if (checkSignature)
                        {
                            if (vnp_ResponseCode == "00")
                            {
                                //Thanh toan thanh cong
                                output.RspCode = "00";
                                output.Message = $"Thanh toán thành công! OrderId={orderId} VNPay TranId = {vnpayTranId}";
                             
                                return output;
                                //log.InfoFormat("Thanh toan thanh cong, OrderId={0}, VNPAY TranId={1}", orderId, vnpayTranId);
                            }
                            else
                            {
                                //Thanh toan khong thanh cong. Ma loi: vnp_ResponseCode
                                output.RspCode = vnp_ResponseCode;
                                output.Message = "Có lỗi xảy ra trong quá trình xử lý.Mã lỗi: " + vnp_ResponseCode;                                
                                return output;
                                //log.InfoFormat("Thanh toan loi, OrderId={0}, VNPAY TranId={1},ResponseCode={2}", orderId, vnpayTranId, vnp_ResponseCode);
                            }
                        }
                        else
                        {
                            output.RspCode = vnp_ResponseCode;
                            output.Message = "Có lỗi xảy ra trong quá trình xử lý";
                           
                            return output;
                        }
                    }
                }
                else
                {

                }
            }
            else
            {
                output.RspCode = "01";
                output.Message = "Có lỗi trong quá trình thanh toán";
                return output;
            }
            //Sent Message

            return output;
        }
        [HttpPost]
        //[Authorize]
        public async Task<OrderDetailDTO> UpgradePackageBrand(UpgrageBrandPackageDTO model)
        {
            _logger.LogDebug($"UpgradePackageBrand model: {JsonConvert.SerializeObject(model)}");
            var output = new OrderDetailDTO();
            //check ProductBrand
            var brand = await _repoWrapper.Brand.FirstOrDefaultAsync(p => p.ProductBrand_ID == model.ProductBrandId);
            var user = await _repoWrapper.AspNetUsers.FirstOrDefaultAsync(p => p.Id == model.UserId);
            if(brand == null)
            {
                output.ErrorCode = "01";
                output.Message = "Không tồn tại gian hàng";
                return output;
            }
            else
            {                
                if(brand.ProductBrandType_ID == 2 && model.ServiceId == 3) //Upgrade vừa => lớn
                {
                    var orderRemain = await _repoPaymentWrapper.PaymentUpgrade.GetOrderRemain(model.ProductBrandId);
                    if(orderRemain != null)
                    {                            
                        output.MonthRemain = orderRemain.MonthRemain;
                        output.MonthPrice = orderRemain.MonthPrice;
                        output.TotalDeduct = orderRemain.TotalDeduct;
                    }    
                    var result = await _repoPaymentWrapper.PaymentUpgrade.UpgradeBrandVTL(model, brand.Name, user.NormalizedUserName, orderRemain.MonthRemain, orderRemain.MonthPrice, orderRemain.TotalDeduct??0);
                    //Upgrage ProductBrand

                    output = _mapper.Map<OrderDetailDTO>(result);
                    output.MonthRemain = orderRemain.MonthRemain;
                    output.MonthPrice = orderRemain.MonthPrice;
                    output.TotalDeduct = orderRemain.TotalDeduct;
                    output.ErrorCode = "00";
                    output.Message = "Tạo đơn hàng thành công";
                    return output;
                }
                else if(brand.ProductBrandType_ID == model.ServiceId) // Tiếp tục upgrade gian hàng cùng loại
                {
                    var result = await _repoPaymentWrapper.PaymentUpgrade.UpgradeBrand(model, brand.Name, user.NormalizedUserName, true);
                    //Upgrage ProductBrand

                    output = _mapper.Map<OrderDetailDTO>(result);
                    output.ErrorCode = "00";
                    output.Message = "Tạo đơn hàng thành công";
                    return output;
                }    
                else if (brand.ProductBrandType_ID == 3 && model.ServiceId == 2)
                {
                    output.ErrorCode = "01";
                    output.Message = "Gian hàng của bạn là loại gian hàng lớn, bạn không thể nâng cấp về loại gian hàng nhỏ hơn!";
                    return output;
                }
                else
                {
                    var result = await _repoPaymentWrapper.PaymentUpgrade.UpgradeBrand(model, brand.Name, user.NormalizedUserName,false);
                    //Upgrage ProductBrand

                    output = _mapper.Map<OrderDetailDTO>(result);
                    output.ErrorCode = "00";
                    output.Message = "Tạo đơn hàng thành công";
                    return output;
                }                   
            }
        }
        [HttpGet]
        public async Task<List<OrderHistoryGroup>> GetHistoryUpgrade(int ProductBrandId)
        {
            var output = new List<OrderHistoryGroup>();
            var brand = await _repoWrapper.Brand.FirstOrDefaultAsync(p => p.ProductBrand_ID == ProductBrandId);
            if(brand == null)
            {               
                return output;
            }
            else
            {
                var item = new OrderHistoryGroup();
                var result = await _repoPaymentWrapper.PaymentUpgrade.GetListHistoryByBrandId(ProductBrandId);
                var lstA = result.GroupBy(p => p.GroupByMonthYear).Select(g => new OrderHistoryGroup
                {
                    Monthly =  g.Key,
                    data = g.ToList()
                });
                output = lstA.ToList();  
            }
            return output;
        }
        [HttpPost]
        [Authorize]
        public async Task<PostPaymentVAT> CreateExpVAT(PostPaymentVAT model)
        {
            _logger.LogDebug($"CreateExpVAT model: {JsonConvert.SerializeObject(model)}");
            var output = new PostPaymentVAT();
            try
            {
               var result = await _repoPaymentWrapper.PaymentUpgrade.PostPaymentVat(model);
                output = _mapper.Map<PostPaymentVAT>(result);
                output.ErrorCode = "00";
                output.Message = "Lưu thông tin xuất VAT thành công";

            }
            catch(Exception ex)
            {
                output.ErrorCode = "01";
                output.Message = "Lưu thông tin xuất VAT không thành công";
            }
            return output;

        }
        [HttpPost]
        [Authorize]
        public async Task<ModelBase> PaymentUploadImage( ImageUploadDTO model, string OrderCode)
        {
            var output = new ModelBase();
            var orderItem = _repoPaymentWrapper.PaymentUpgrade.FirstOrDefaultAsync(p => p.OrderCode == OrderCode);
            if(orderItem == null)
            {
                output.ErrorCode = "01";
                output.Message = $"Không tồn tại tài đơn hàng {OrderCode}";
            }
            else
            {
                await SaveMainImage(model, OrderCode);
                output.ErrorCode = "00";
                output.Message = "Upload ảnh thành công";
            }
            return output;
        }
        /// <summary>
        /// Delete Order 
        /// </summary>
        /// <param name="OrderCode"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ModelBase> DeleteOrderByOrderCode(string OrderCode)
        {            
            var output = new ModelBase();
            var orderItem = await _repoPaymentWrapper.PaymentUpgrade.GetOrderMasterByOrderCode(OrderCode);
            if (orderItem == null)
            {
                output.ErrorCode = "01";
                output.Message = $"Không tồn tại tại đơn hàng {OrderCode}";
            }
            else
            {
                var result = await _repoPaymentWrapper.PaymentUpgrade.DeleteOrderByOrderCode(OrderCode);
                output.ErrorCode = "00";
                output.Message = $"Xóa thành công đơn hàng {OrderCode}";
            }
            return output;
        }
        [HttpGet]
        public async Task<ArticleDTO> GetPaymentPolicy()
        {
            var output = new ArticleDTO();
            var result = await _repoWrapper.Article.FirstOrDefaultAsync(p => p.Article_ID == 1683);
            if(result != null)
            {
                output = _mapper.Map<ArticleDTO>(result);
            }
            return output;

        }
        private async Task SaveMainImage(ImageUploadDTO MainImage, string OrderCode)
        {
            if (MainImage.Base64 == null) return;
            var timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            MainImage.FileName = String.Format("{0}-mobile-00-{1}.{2}", OrderCode, timestamp, MainImage.ExtensionType.Replace("image/", ""));
            MainImage.PathSave = "payment/upload";

            UploadImage(MainImage, MainImage.PathSave);

            //Save db
            await _repoPaymentWrapper.PaymentUpgrade.UploadImage(MainImage.FileName, OrderCode);

        }
        [ApiExplorerSettings(IgnoreApi = true)]
        private async Task<bool> UploadImage(ImageUploadDTO model, string pathMain)
        {
            try
            {
                var imageDataByteArray = Convert.FromBase64String(model.Base64);
                var imageDataStream = new MemoryStream(imageDataByteArray);
                imageDataStream.Position = 0;
                
                var inputStreamEnd = imageDataStream;
                
                var file = File(imageDataByteArray, model.ExtensionType);
                var fileName = model.FileName;
                if (file.FileContents.Length > 0)
                {
                    Util.UploadS3(model.FileName, pathMain, inputStreamEnd, model.ExtensionType);                    
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Upload Product Image: " + ex.ToString());
                return false;
            }
        }

            public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
