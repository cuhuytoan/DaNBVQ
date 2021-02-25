using AutoMapper;
using HNM.DataNC.ModelsNC.ModelsDTO;
using HNM.DataNC.ModelsPayment;
using HNM.DataNC.ModelsNC.ModelsUtility;
using HNM.RepositoriesWeb.RepositoriesBase;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace HNM.RepositoriesWeb.Repositories
{
    public interface IUpdateStoreRepository
    {
        Task<List<PaymentType>> GetLstPaymentType();
        Task<PaymentPackageDTO> GetBrandPackage(int? ProductBrandId);
        Task<List<OrderHistoryGroup>> GetHistoryUpgrade(int? ProductBrandId);
        Task<List<PaymentBanking>> GetListPaymentBanking(int? PaymentTypeId);
        Task<OrderDetailDTO> GetOrderDetailByOrderCode(string OrderCode);
        Task<ModelBase> DeleteOrderByOrderCode(string OrderCode);
        Task<ArticleDTO> GetPaymentPolicy();
        Task<OrderDetailDTO> UpgradePackageBrand(UpgrageBrandPackageDTO UpgrageBrandPackageDTO);
        Task<PostPaymentVAT> CreateExpVAT(PostPaymentVAT PostPaymentVAT);
        Task<ModelBase> PaymentUploadImage(ImageUploadDTO ImageUploadDTO, string OrderCode);
        Task<ModelBaseStatus> PostUrlVNPay(PostUrlVNPay PostUrlVNPay);

    }
    public class UpdateStoreRepository : IRepositoryBase, IUpdateStoreRepository
    {
        private readonly HttpClient _client;
        private IMapper _mapper;
        private IHttpContextAccessor _httpContextAccessor;
        public UpdateStoreRepository(HttpClient client, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _client = client;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Lấy ra các hình thức thanh toán
        /// </summary>
        /// <returns></returns>
        public async Task<List<PaymentType>> GetLstPaymentType()
        {
            List<PaymentType> output = new List<PaymentType>();
            try
            {
                var jwt = _httpContextAccessor.HttpContext.User.FindFirst(p => p.Type == "access_token").Value;
                if (jwt != null)
                {
                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
                    string apiUrl = $"/api/v1/Payment/GetLstPaymentType";
                    var userInfo = JsonConvert.DeserializeObject<SumProfileResponseDTO>(_httpContextAccessor.HttpContext.User.FindFirst(p => p.Type == "UserInfomation").Value);
                    var response = await _client.GetAsync(apiUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        string responseStream = await response.Content.ReadAsStringAsync();
                        output = JsonConvert.DeserializeObject<List<PaymentType>>(responseStream);
                    }
                }
            }
            catch (Exception e)
            {

            }

            return output;
        }

        /// <summary>
        /// Lấy ra các gói nâng cấp + time + giảm giá
        /// </summary>
        /// <returns></returns>
        public async Task<PaymentPackageDTO> GetBrandPackage(int? ProductBrandId)
        {
            PaymentPackageDTO output = new PaymentPackageDTO();
            try
            {
                var jwt = _httpContextAccessor.HttpContext.User.FindFirst(p => p.Type == "access_token").Value;
                if (jwt != null)
                {
                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
                    string apiUrl = $"/api/v1/Payment/GetBrandPackage";
                    var userInfo = JsonConvert.DeserializeObject<SumProfileResponseDTO>(_httpContextAccessor.HttpContext.User.FindFirst(p => p.Type == "UserInfomation").Value);
                    string paramRequest = $"?ProductBrandId={ProductBrandId}";
                    var response = await _client.GetAsync(apiUrl+paramRequest);
                    if (response.IsSuccessStatusCode)
                    {
                        string responseStream = await response.Content.ReadAsStringAsync();
                        output = JsonConvert.DeserializeObject<PaymentPackageDTO>(responseStream);
                    }
                }
            }
            catch (Exception e)
            {

            }

            return output;
        }

        /// <summary>
        /// Lịch sử nâng cấp gian hàng
        /// </summary>
        /// <returns></returns>
        public async Task<List<OrderHistoryGroup>> GetHistoryUpgrade(int? ProductBrandId)
        {
            List<OrderHistoryGroup> output = new List<OrderHistoryGroup>();
            try
            {
                var jwt = _httpContextAccessor.HttpContext.User.FindFirst(p => p.Type == "access_token").Value;
                if (jwt != null)
                {
                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
                    string apiUrl = $"/api/v1/Payment/GetHistoryUpgrade";
                    var userInfo = JsonConvert.DeserializeObject<SumProfileResponseDTO>(_httpContextAccessor.HttpContext.User.FindFirst(p => p.Type == "UserInfomation").Value);
                    string paramRequest = $"?ProductBrandId={ProductBrandId}";
                    var response = await _client.GetAsync(apiUrl + paramRequest);
                    if (response.IsSuccessStatusCode)
                    {
                        string responseStream = await response.Content.ReadAsStringAsync();
                        output = JsonConvert.DeserializeObject<List<OrderHistoryGroup>>(responseStream);
                    }
                }
            }
            catch (Exception e)
            {

            }

            return output;
        }

        /// <summary>
        /// Lấy ra ds ngân hàng theo hình thức thanh toán
        /// </summary>
        /// <param name="PaymentTypeId"></param>
        /// <returns></returns>
        public async Task<List<PaymentBanking>> GetListPaymentBanking(int? PaymentTypeId)
        {
            List<PaymentBanking> output = new List<PaymentBanking>();
            try
            {
                var jwt = _httpContextAccessor.HttpContext.User.FindFirst(p => p.Type == "access_token").Value;
                if (jwt != null)
                {
                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
                    string apiUrl = $"/api/v1/Payment/GetListPaymentBanking";
                    var userInfo = JsonConvert.DeserializeObject<SumProfileResponseDTO>(_httpContextAccessor.HttpContext.User.FindFirst(p => p.Type == "UserInfomation").Value);
                    string paramRequest = $"?PaymentTypeId={PaymentTypeId}";
                    var response = await _client.GetAsync(apiUrl + paramRequest);
                    if (response.IsSuccessStatusCode)
                    {
                        string responseStream = await response.Content.ReadAsStringAsync();
                        output = JsonConvert.DeserializeObject<List<PaymentBanking>>(responseStream);
                    }
                }
            }
            catch (Exception e)
            {

            }

            return output;
        }

        /// <summary>
        /// Lấy ra chi tiết đơn hàng
        /// </summary>
        /// <param name="OrderCode"></param>
        /// <returns></returns>
        public async Task<OrderDetailDTO> GetOrderDetailByOrderCode(string OrderCode)
        {
            OrderDetailDTO output = new OrderDetailDTO();
            try
            {
                var jwt = _httpContextAccessor.HttpContext.User.FindFirst(p => p.Type == "access_token").Value;
                if (jwt != null)
                {
                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
                    string apiUrl = $"/api/v1/Payment/GetOrderDetailByOrderCode";
                    string paramRequest = $"?OrderCode={OrderCode}";
                    var response = await _client.GetAsync(apiUrl+paramRequest);
                    if (response.IsSuccessStatusCode)
                    {
                        string responseStream = await response.Content.ReadAsStringAsync();
                        output = JsonConvert.DeserializeObject<OrderDetailDTO>(responseStream);
                    }
                }
            }
            catch (Exception e)
            {

            }

            return output;
        }

        /// <summary>
        /// Xóa đơn hàng theo OrderCode
        /// </summary>
        /// <param name="OrderCode"></param>
        /// <returns></returns>
        public async Task<ModelBase> DeleteOrderByOrderCode(string OrderCode)
        {
            ModelBase output = new ModelBase();
            try
            {
                var jwt = _httpContextAccessor.HttpContext.User.FindFirst(p => p.Type == "access_token").Value;
                if (jwt != null)
                {
                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
                    string apiUrl = $"/api/v1/Payment/DeleteOrderByOrderCode";
                    string paramRequest = $"?OrderCode={OrderCode}";
                    var response = await _client.GetAsync(apiUrl + paramRequest);
                    if (response.IsSuccessStatusCode)
                    {
                        string responseStream = await response.Content.ReadAsStringAsync();
                        output = JsonConvert.DeserializeObject<ModelBase>(responseStream);
                    }
                }
            }
            catch (Exception e)
            {

            }

            return output;
        }

        /// <summary>
        /// Lấy ra điều khoản
        /// </summary>
        /// <returns></returns>
        public async Task<ArticleDTO> GetPaymentPolicy()
        {
            ArticleDTO output = new ArticleDTO();
            try
            {
                var jwt = _httpContextAccessor.HttpContext.User.FindFirst(p => p.Type == "access_token").Value;
                if (jwt != null)
                {
                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
                    string apiUrl = $"/api/v1/Payment/GetPaymentPolicy";
                    var response = await _client.GetAsync(apiUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        string responseStream = await response.Content.ReadAsStringAsync();
                        output = JsonConvert.DeserializeObject<ArticleDTO>(responseStream);
                    }
                }
            }
            catch (Exception e)
            {

            }

            return output;
        }

        /// <summary>
        /// Cập nhật quy mô gian hàng
        /// </summary>
        /// <param name="UpgrageBrandPackageDTO"></param>
        /// <returns></returns>
        public async Task<OrderDetailDTO> UpgradePackageBrand(UpgrageBrandPackageDTO UpgrageBrandPackageDTO)
        {
            OrderDetailDTO output = new OrderDetailDTO();
            var jwt = _httpContextAccessor.HttpContext.User.FindFirst(p => p.Type == "access_token").Value;
            if (jwt != null)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
                string apiUrl = $"/api/v1/Payment/UpgradePackageBrand";
                var userInfo = JsonConvert.DeserializeObject<SumProfileResponseDTO>(_httpContextAccessor.HttpContext.User.FindFirst(p => p.Type == "UserInfomation").Value);
                UpgrageBrandPackageDTO.ProductBrandId = userInfo.ProductBrandId;
                UpgrageBrandPackageDTO.UserId = userInfo.UserId;
                var json = JsonConvert.SerializeObject(UpgrageBrandPackageDTO, Formatting.Indented);
                var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _client.PostAsync(apiUrl, stringContent);
                if (response.IsSuccessStatusCode)
                {
                    string responseStream = await response.Content.ReadAsStringAsync();
                    output = JsonConvert.DeserializeObject<OrderDetailDTO>(responseStream);
                }
            }

            return output;
        }

        /// <summary>
        /// Xuất hóa đơn VAT
        /// </summary>
        /// <param name="PostPaymentVAT"></param>
        /// <returns></returns>
        public async Task<PostPaymentVAT> CreateExpVAT(PostPaymentVAT PostPaymentVAT)
        {
            PostPaymentVAT output = new PostPaymentVAT();
            var jwt = _httpContextAccessor.HttpContext.User.FindFirst(p => p.Type == "access_token").Value;
            if (jwt != null)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
                string apiUrl = $"/api/v1/Payment/CreateExpVAT";
                var userInfo = JsonConvert.DeserializeObject<SumProfileResponseDTO>(_httpContextAccessor.HttpContext.User.FindFirst(p => p.Type == "UserInfomation").Value);
                PostPaymentVAT.UserId = userInfo.UserId;
                var json = JsonConvert.SerializeObject(PostPaymentVAT, Formatting.Indented);
                var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _client.PostAsync(apiUrl, stringContent);
                if (response.IsSuccessStatusCode)
                {
                    string responseStream = await response.Content.ReadAsStringAsync();
                    output = JsonConvert.DeserializeObject<PostPaymentVAT>(responseStream);
                }
            }

            return output;
        }

        /// <summary>
        /// Upload ảnh ủy nhiệm chi
        /// </summary>
        /// <param name="ImageUploadDTO"></param>
        /// <returns></returns>
        public async Task<ModelBase> PaymentUploadImage(ImageUploadDTO ImageUploadDTO, string OrderCode)
        {
            ModelBase output = new ModelBase();
            var jwt = _httpContextAccessor.HttpContext.User.FindFirst(p => p.Type == "access_token").Value;
            if (jwt != null)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
                string apiUrl = $"/api/v1/Payment/PaymentUploadImage";
                string paramRequest = $"?OrderCode={OrderCode}";
                var json = JsonConvert.SerializeObject(ImageUploadDTO, Formatting.Indented);
                var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _client.PostAsync(apiUrl + paramRequest, stringContent);
                if (response.IsSuccessStatusCode)
                {
                    string responseStream = await response.Content.ReadAsStringAsync();
                    output = JsonConvert.DeserializeObject<ModelBase>(responseStream);
                }
            }

            return output;
        }

        /// <summary>
        /// Gửi dữ liệu qua VNPAY
        /// </summary>
        /// <param name="PostUrlVNPay"></param>
        /// <returns></returns>
        public async Task<ModelBaseStatus> PostUrlVNPay(PostUrlVNPay PostUrlVNPay)
        {
            ModelBaseStatus output = new ModelBaseStatus();
            var jwt = _httpContextAccessor.HttpContext.User.FindFirst(p => p.Type == "access_token").Value;
            if (jwt != null)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
                string apiUrl = $"/api/v1/Payment/PostUrlVNPay";
                var userInfo = JsonConvert.DeserializeObject<SumProfileResponseDTO>(_httpContextAccessor.HttpContext.User.FindFirst(p => p.Type == "UserInfomation").Value);
                var json = JsonConvert.SerializeObject(PostUrlVNPay, Formatting.Indented);
                var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _client.PostAsync(apiUrl, stringContent);
                if (response.IsSuccessStatusCode)
                {
                    string responseStream = await response.Content.ReadAsStringAsync();
                    output = JsonConvert.DeserializeObject<ModelBaseStatus>(responseStream);
                }
            }

            return output;
        }
    }
}
