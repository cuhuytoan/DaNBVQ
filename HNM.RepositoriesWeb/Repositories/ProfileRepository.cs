using AutoMapper;
using HNM.CommonNC;
using HNM.DataNC.Models;
using HNM.DataNC.ModelsNC.ModelsDTO;
using HNM.DataNC.ModelsNC.ModelsUtility;
using HNM.RepositoriesWeb.RepositoriesBase;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace HNM.RepositoriesWeb.Repositories
{
    public interface IProfileRepository
    {
        Task<ProfilersDTO> Profilers();
        Task<LstFCMMessageUser> GetListMsgByUser(int? top, int notiSpecType);
        Task<ContactAdsDTO> ContactAdsDTO(ContactAdsDTO contactAds);
        Task<List<QuoteAds>> GetListQuoteAds();
        Task<FCMHasReadDTO> FCMHasRead(int HasRead, int FCMMessageID, int NotiSpecType);
        Task<ProfilersDTO> UpdateUser(ProfilersDTO profilersDTO);
        Task<ChangePassDTO> ChangePassword(ChangePassDTO changePassDTO);
        Task<PostProductBrandDTO> CreateProductBrand(PostProductBrandDTO postProductBrandDTO);
        Task<PostProductBrandDTO> PutProductBrand(PostProductBrandDTO postProductBrandDTO);
        Task<ProductBrandSearchDetailDTO> GetBrandDetails(int? ProductBrandId);
        Task<LstCountryDTO> GetLstCountry(string text);
        Task<ListLocationDTO> GetLocationByCountry(int? countryId, string text);
        Task<ListDistrictDTO> GetDistrictByLocation(int? locationId, string text);
        Task<ListLocationDTO> GetAllLocation(string text);
        Task<ProductBrandSearchDetailDTO> GetBrandDetailByUserId();
        Task<List<ProductDTO>> GetDemandByProductBrand(int productBrandId);
        Task<FCMHasReadDTO> CountFCMHasRead();
        Task<ProfilersDTO> ProfilersByUserId(string userId);
        Task<UpdateAvatarDTO> UpdateAvatar(ImageUploadAvatarDTO updateAvatar);
        Task<ModelBaseStatus> PostReferralCode(PostReferralCode PostReferralCode);
        Task<ListLocationDTO> GetAllLocation2();
    }
    class ProfileRepository: IRepositoryBase, IProfileRepository
    {
        private readonly HttpClient _client;
        private IMapper _maper;
        private IHttpContextAccessor _httpContextAccessor;
        public ProfileRepository(HttpClient client, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _client = client;
            _maper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Lấy ra thông tin tài khoản
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public async Task<ProfilersDTO> Profilers()
        {
            ProfilersDTO output = new ProfilersDTO();
            var jwt = JsonConvert.DeserializeObject<SumProfileResponseDTO>(_httpContextAccessor.HttpContext.Request.Cookies["JWT"]);
            if (jwt != null)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt.JWT);

                string apiUrl = $"/api/v1/Manage/Profilers";
                string paramRequest = $"?UserId={jwt.UserId}";
                var response = await _client.GetAsync(apiUrl + paramRequest);
                if (response.IsSuccessStatusCode)
                {
                    string responseStream = await response.Content.ReadAsStringAsync();
                    output = JsonConvert.DeserializeObject<ProfilersDTO>(responseStream);
                }
            }            
            return output;
        }

        public async Task<ProfilersDTO> ProfilersByUserId(string userId)
        {
            ProfilersDTO output = new ProfilersDTO();
            var jwt = JsonConvert.DeserializeObject<SumProfileResponseDTO>(_httpContextAccessor.HttpContext.Request.Cookies["JWT"]);
            if (jwt != null)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt.JWT);

                string apiUrl = $"/api/v1/Manage/Profilers";
                string paramRequest = $"?UserId={userId}";
                var response = await _client.GetAsync(apiUrl + paramRequest);
                if (response.IsSuccessStatusCode)
                {
                    string responseStream = await response.Content.ReadAsStringAsync();
                    output = JsonConvert.DeserializeObject<ProfilersDTO>(responseStream);
                }
            }
            return output;
        }

        /// <summary>
        /// Lấy ra danh sách thông báo
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="top"></param>
        /// <param name="notiSpecType"></param>
        /// <returns></returns>
        public async Task<LstFCMMessageUser> GetListMsgByUser(int? top, int notiSpecType)
        {
            LstFCMMessageUser output = new LstFCMMessageUser();
            try
            {
                var jwt = JsonConvert.DeserializeObject<SumProfileResponseDTO>(_httpContextAccessor.HttpContext.Request.Cookies["JWT"]);
                if (jwt != null)
                {
                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt.JWT);
                    string apiUrl = $"/api/v1/FCMMessage/GetListMsgByUser";
                    string paramRequest = $"?UserID={jwt.UserId}&Top={top}&NotiSpecType={notiSpecType}";
                    var response = await _client.GetAsync(apiUrl + paramRequest);
                    if (response.IsSuccessStatusCode)
                    {
                        string responseStream = await response.Content.ReadAsStringAsync();
                        output = JsonConvert.DeserializeObject<LstFCMMessageUser>(responseStream);
                    }
                }
            }
            catch(Exception e)
            {

            }
            
            return output;
        }

        /// <summary>
        /// Đánh dấu đã đọc
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="HasRead"></param>
        /// <param name="FCMMessageID"></param>
        /// <param name="NotiSpecType"></param>
        /// <returns></returns>
        public async Task<FCMHasReadDTO> FCMHasRead(int HasRead, int FCMMessageID, int NotiSpecType)
        {
            FCMHasReadDTO output = new FCMHasReadDTO();
            var jwt = JsonConvert.DeserializeObject<SumProfileResponseDTO>(_httpContextAccessor.HttpContext.Request.Cookies["JWT"]);
            if (jwt != null)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt.JWT);
                string apiUrl = $"/api/v1/FCMMessage/FCMHasRead";
                string paramRequest = $"?UserId={jwt.UserId}&HasRead={HasRead}&FCMMessageID={FCMMessageID}&NotiSpecType={NotiSpecType}";
                var response = await _client.GetAsync(apiUrl + paramRequest);
                if (response.IsSuccessStatusCode)
                {
                    string responseStream = await response.Content.ReadAsStringAsync();
                    output = JsonConvert.DeserializeObject<FCMHasReadDTO>(responseStream);
                }
            }
            return output;
        }

        public async Task<FCMHasReadDTO> CountFCMHasRead()
        {
            FCMHasReadDTO output = new FCMHasReadDTO();
            var jwt = JsonConvert.DeserializeObject<SumProfileResponseDTO>(_httpContextAccessor.HttpContext.Request.Cookies["JWT"]);
            if (jwt != null)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt.JWT);
                string apiUrl = $"/api/v1/FCMMessage/FCMHasRead";
                string paramRequest = $"?UserId={jwt.UserId}";
                var response = await _client.GetAsync(apiUrl + paramRequest);
                if (response.IsSuccessStatusCode)
                {
                    string responseStream = await response.Content.ReadAsStringAsync();
                    output = JsonConvert.DeserializeObject<FCMHasReadDTO>(responseStream);
                }
            }
            return output;
        }

        /// <summary>
        /// Lấy ra ds bảng giá qc
        /// </summary>
        /// <returns></returns>
        public async Task<List<QuoteAds>> GetListQuoteAds()
        {
            List<QuoteAds> output = new List<QuoteAds>();
            string apiUrl = $"/api/v1/Advertising/GetListQuoteAds";
            var response = await _client.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<List<QuoteAds>>(responseStream);
            }
            return output;
        }

        public async Task<ContactAdsDTO> ContactAdsDTO(ContactAdsDTO contactAds)
        {
            ContactAdsDTO output = new ContactAdsDTO();
            var jwt = JsonConvert.DeserializeObject<SumProfileResponseDTO>(_httpContextAccessor.HttpContext.Request.Cookies["JWT"]);
            if (jwt != null)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt.JWT);
                contactAds.UserId = jwt.UserId;
                string apiUrl = $"/api/v1/Advertising/PostContactAds";
                var json = JsonConvert.SerializeObject(contactAds, Formatting.Indented);
                var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _client.PostAsync(apiUrl, stringContent);
                if (response.IsSuccessStatusCode)
                {
                    string responseStream = await response.Content.ReadAsStringAsync();
                    output = JsonConvert.DeserializeObject<ContactAdsDTO>(responseStream);
                }
            }
            return output;
        }
        public async Task<ChangePassDTO> ChangePassword(ChangePassDTO changePassDTO)
        {
            ChangePassDTO output = new ChangePassDTO();
            var jwt = JsonConvert.DeserializeObject<SumProfileResponseDTO>(_httpContextAccessor.HttpContext.Request.Cookies["JWT"]);
            if (jwt != null)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt.JWT);
                string apiUrl = $"/api/v1/Account/ChangePassword";
                changePassDTO.UserId = jwt.UserId;
                var json = JsonConvert.SerializeObject(changePassDTO, Formatting.Indented);
                var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _client.PostAsync(apiUrl, stringContent);
                if (response.IsSuccessStatusCode)
                {
                    string responseStream = await response.Content.ReadAsStringAsync();
                    output = JsonConvert.DeserializeObject<ChangePassDTO>(responseStream);
                }
            }

            return output;
        }

        public async Task<ModelBaseStatus> PostReferralCode(PostReferralCode PostReferralCode)
        {
            ModelBaseStatus output = new ModelBaseStatus();
            var jwt = _httpContextAccessor.HttpContext.User.FindFirst(p => p.Type == "access_token").Value;
            if (jwt != null)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
                string apiUrl = $"/api/v1/Brand/PostReferralCode";
                var userInfo = JsonConvert.DeserializeObject<SumProfileResponseDTO>(_httpContextAccessor.HttpContext.User.FindFirst(p => p.Type == "UserInfomation").Value);
                PostReferralCode.UserId = userInfo.UserId;
                PostReferralCode.ProductBrandId = userInfo.ProductBrandId;
                var json = JsonConvert.SerializeObject(PostReferralCode, Formatting.Indented);
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

        public async Task<ProfilersDTO> UpdateUser(ProfilersDTO profilersDTO)
        {
            ProfilersDTO output = new ProfilersDTO();
            var jwt = JsonConvert.DeserializeObject<SumProfileResponseDTO>(_httpContextAccessor.HttpContext.Request.Cookies["JWT"]);
            if (jwt != null)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt.JWT);
                string apiUrl = $"/api/v1/Manage/UpdateProfilers?UserId={jwt.UserId}";
                var json = JsonConvert.SerializeObject(profilersDTO, Formatting.Indented);
                var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _client.PutAsync(apiUrl, stringContent);
                if (response.IsSuccessStatusCode)
                {
                    string responseStream = await response.Content.ReadAsStringAsync();
                    output = JsonConvert.DeserializeObject<ProfilersDTO>(responseStream);
                }
            }
            return output;
        }

        public async Task<PostProductBrandDTO> CreateProductBrand(PostProductBrandDTO postProductBrandDTO)
        {
            PostProductBrandDTO output = new PostProductBrandDTO();
            var jwt = JsonConvert.DeserializeObject<SumProfileResponseDTO>(_httpContextAccessor.HttpContext.Request.Cookies["JWT"]);
            if (jwt != null)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt.JWT);
                postProductBrandDTO.UserId = jwt.UserId;
                string apiUrl = $"/api/v1/Brand/PostProductBrand";
                var json = JsonConvert.SerializeObject(postProductBrandDTO, Formatting.Indented);
                var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _client.PostAsync(apiUrl, stringContent);
                if (response.IsSuccessStatusCode)
                {
                    string responseStream = await response.Content.ReadAsStringAsync();
                    output = JsonConvert.DeserializeObject<PostProductBrandDTO>(responseStream);
                }
            }
            
            return output;
        }

        public async Task<PostProductBrandDTO> PutProductBrand(PostProductBrandDTO postProductBrandDTO)
        {
            PostProductBrandDTO output = new PostProductBrandDTO();
            var jwt = JsonConvert.DeserializeObject<SumProfileResponseDTO>(_httpContextAccessor.HttpContext.Request.Cookies["JWT"]);
            if (jwt != null)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt.JWT);
                string apiUrl = $"/api/v1/Brand/PutProductBrand";
                string paramRequest = $"?ProductBrandId={postProductBrandDTO.Data.ProductBrand_ID}";
                postProductBrandDTO.UserId = jwt.UserId;
                var json = JsonConvert.SerializeObject(postProductBrandDTO, Formatting.Indented);
                var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _client.PutAsync(apiUrl + paramRequest, stringContent);
                if (response.IsSuccessStatusCode)
                {
                    string responseStream = await response.Content.ReadAsStringAsync();
                    output = JsonConvert.DeserializeObject<PostProductBrandDTO>(responseStream);
                }
            }
            return output;
        }
        
        public async Task<UpdateAvatarDTO> UpdateAvatar(ImageUploadAvatarDTO updateAvatar)
        {
            UpdateAvatarDTO output = new UpdateAvatarDTO();
            var token = _httpContextAccessor.HttpContext.User.FindFirst(p => p.Type == "access_token").Value;
            var user = _httpContextAccessor.HttpContext.User.FindFirst(p => p.Type == "UserInfomation").Value;
            var jwt = JsonConvert.DeserializeObject<SumProfileResponseDTO>(_httpContextAccessor.HttpContext.Request.Cookies["JWT"]);
            var jwtUser = JsonConvert.DeserializeObject<SumProfileResponseDTO>(user);
            if (jwt != null)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt.JWT);
                string apiUrl = $"/api/v1/ImageUpload/UpdateAvatar";
                updateAvatar.UserId = jwt.UserId;
                var json = JsonConvert.SerializeObject(updateAvatar, Formatting.Indented);
                var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
                
                var response = await _client.PostAsync(apiUrl, stringContent);
                if (response.IsSuccessStatusCode)
                {
                    string responseStream = await response.Content.ReadAsStringAsync();
                    output = JsonConvert.DeserializeObject<UpdateAvatarDTO>(responseStream);
                }
            }
            return output;
        }

        public async Task<ProductBrandSearchDetailDTO> GetBrandDetails(int? ProductBrandId)
        {
            ProductBrandSearchDetailDTO output = new ProductBrandSearchDetailDTO();
            string apiUrl = $"/api/v1/Brand/GetBrandDetails";
            string paramRequest = $"?ProductBrandId={ProductBrandId}";
            var response = await _client.GetAsync(apiUrl + paramRequest);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<ProductBrandSearchDetailDTO>(responseStream);
            }
            return output;
        }

        public async Task<LstCountryDTO> GetLstCountry(string text)
        {
            LstCountryDTO output = new LstCountryDTO();
            string apiUrl = $"/api/v1/Country/GetLstCountry";
            string paramRequest = $"?keyword={text}";
            var response = await _client.GetAsync(apiUrl + paramRequest);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<LstCountryDTO>(responseStream);
            }
            return output;
        }

        public async Task<ListLocationDTO> GetLocationByCountry(int? countryId, string text)
        {
            ListLocationDTO output = new ListLocationDTO();
            string apiUrl = $"/api/v1/Location/GetLocationByCountry";
            string paramRequest = $"?CountryId={countryId}&keyword={text}";
            var response = await _client.GetAsync(apiUrl +paramRequest);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<ListLocationDTO>(responseStream);
            }
            return output;
        }

        public async Task<ListDistrictDTO> GetDistrictByLocation(int? locationId, string text)
        {
            ListDistrictDTO output = new ListDistrictDTO();
            string apiUrl = $"/api/v1/Location/GetDistrictByLocation";
            string paramRequest = $"?LocationId={locationId}&keyword={text}";
            var response = await _client.GetAsync(apiUrl + paramRequest);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<ListDistrictDTO>(responseStream);
            }
            return output;
        }
        
        public async Task<ProductBrandSearchDetailDTO> GetBrandDetailByUserId()
        {
            ProductBrandSearchDetailDTO output = new ProductBrandSearchDetailDTO();
            var jwt = JsonConvert.DeserializeObject<SumProfileResponseDTO>(_httpContextAccessor.HttpContext.Request.Cookies["JWT"]);
            if (jwt != null)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt.JWT);
                string apiUrl = $"/api/v1/Brand/GetBrandDetailByUserId";
                string paramRequest = $"?userId={jwt.UserId}";
                var response = await _client.GetAsync(apiUrl + paramRequest);
                if (response.IsSuccessStatusCode)
                {
                    string responseStream = await response.Content.ReadAsStringAsync();
                    output = JsonConvert.DeserializeObject<ProductBrandSearchDetailDTO>(responseStream);
                }
            }
            return output;
        }

        public async Task<List<ProductDTO>> GetDemandByProductBrand(int productBrandId)
        {
            List<ProductDTO> output = new List<ProductDTO>();
            var jwt = JsonConvert.DeserializeObject<SumProfileResponseDTO>(_httpContextAccessor.HttpContext.Request.Cookies["JWT"]);
            if (jwt != null)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt.JWT);
                string apiUrl = $"/api/v1/Product/GetListProductDemand";
                string paramRequest = $"?ProductBrand_ID={productBrandId}";
                var response = await _client.GetAsync(apiUrl + paramRequest);
                if (response.IsSuccessStatusCode)
                {
                    string responseStream = await response.Content.ReadAsStringAsync();
                    output = JsonConvert.DeserializeObject<List<ProductDTO>>(responseStream);
                }
            }
            return output;
        }

        public async Task<ListLocationDTO> GetAllLocation(string text)
        {
            ListLocationDTO output = new ListLocationDTO();
            string apiUrl = $"/api/v1/Location/GetAllLocation";
            string paramRequest = $"?keyword={text}";
            var response = await _client.GetAsync(apiUrl + paramRequest);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<ListLocationDTO>(responseStream);
            }
            return output;
        }

        public async Task<ListLocationDTO> GetAllLocation2()
        {
            ListLocationDTO output = new ListLocationDTO();
            string apiUrl = $"/api/v1/Location/GetAllLocation";
            var response = await _client.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<ListLocationDTO>(responseStream);
            }
            return output;
        }

    }
}
