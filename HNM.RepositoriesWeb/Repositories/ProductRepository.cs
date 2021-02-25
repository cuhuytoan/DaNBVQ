using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HNM.DataNC.ModelsFilter;
using HNM.DataNC.ModelsNC.ModelsDTO;
using HNM.DataNC.Models;
using HNM.DataNC.ModelsNC.ModelsUtility;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HNM.RepositoriesWeb.Repositories
{
    public interface IProductRepository
    {
        Task<ProductPaggingDTO> GetListProductPagging(ProductFilter model);
        Task<TopProductByCateIdDTO> GetTopProductByCategoryId(int cateId);
        Task<ProductDetailsDTO> GetProductDetails(int productId);
        Task<RecDetailDTO> GetRecruitmentDetails(int RecruitmentId);
        Task<SponsorBanner> GetSponsorBannerProduct(int productCategoryId, int? productTypeId, int top);
        Task<ListProductCategoryDTO> GetProdCateByParentID(string keyword, int? productCategoryId);
        Task<List<PatchNumberDTO>> GetPartNumber(int? ProductCategoryId, int? ProductTypeId);
        Task<PostProductShopManResultDTO> CreateProduct(PostProductShopManDTO postProductBrandDTO);
        Task<ReturnPostRecDTO> UpdateRecruitment(SumPostRecDTO SumPostRecDTO, int RecruitmentId, int IsRepair);
        Task<List<UnitDTO>> GetAllUnit();
        Task<List<CareerCategoryDTO>> GetAllCareerCategory();
        Task<List<Minor>> GetAllUnitService();
        Task<PostProductShopManDTO> UpdateProduct(PostProductShopManDTO postProductBrandDTO, int ProductId, int IsRepair);
        Task<ProductIdDTO> ActionProductShopMan(ProductIdDTO productIdDTO, string Action);
        Task<ProductModelDTO> CreateModel(ProductModelDTO productModelDTO);
        Task<ProductPaggingDTO> GetLstProductShopMan(ProductShopManFilter model);
        Task<ModelBaseStatus> CheckPostProduct(int?ProductBrandId);
        Task<ModelBaseStatus> CheckRenewProduct();
        Task<ReturnPostRecDTO> CreateRecruitment(SumPostRecDTO SumPostRecDTO);
        Task<ReturnPostCVDTO> CreateCV(SumPostCurriculumnViateDTO SumPostCurriculumnViateDTO);
        Task<ModelBaseStatus> ActionInCV(int? Id, string Action);
        Task<ModelBaseStatus> ActionInRec(int? Id, string Action);
        Task<ReturnPostCVDTO> UpdateCV(SumPostCurriculumnViateDTO SumPostCurriculumnViateDTO, int CVId, int IsRepair);

        
    }
    public class ProductRepository : IProductRepository
    {
        private readonly HttpClient _client;
        private IMapper _mapper;
        private IHttpContextAccessor _httpContextAccessor;
        public ProductRepository(HttpClient client, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _client = client;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ProductPaggingDTO> GetListProductPagging(ProductFilter model)
        {
            var output = new ProductPaggingDTO();            
            string apiUrl = $"/api/v1/Product/GetListProduct";
            string paramRequest = $"?Page={model.Page}&PageSize={model.PageSize}&ProductTypeId={model.ProductTypeId}&ProductCategoryId={model.ProductCategoryId}&ProductBrandId={model.ProductBrandId}" +
                $"&StatusTypeId={model.StatusTypeId}&ProductManufactureId={model.ProductManufactureId}&ProductModelId={model.ProductModelId}&LocationId={model.LocationId}&PatchNum={model.PatchNum}" +
                $"&MainSystemId={model.MainSystemId}&StatusMachine={model.StatusMachine}";
            var response = await _client.GetAsync(apiUrl + paramRequest);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<ProductPaggingDTO>(responseStream);
            }
            return output;
        }
        public async Task<TopProductByCateIdDTO> GetTopProductByCategoryId(int cateId)
        {
            var output = new TopProductByCateIdDTO();
            string apiUrl = $"/api/v1/Product/GetTopProductByCategoryId";
            string paramRequest = $"?productCategoryID={cateId}";
            var response = await _client.GetAsync(apiUrl + paramRequest);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<TopProductByCateIdDTO>(responseStream);
            }
            return output;
        }



        public async Task<ProductDetailsDTO> GetProductDetails(int productId)
        {
            ProductDetailsDTO output = new ProductDetailsDTO();
            string apiUrl = $"/api/v1/Product/GetShopmanProductDetails";
            string paramRequest = $"?productId={productId}";
            var response = await _client.GetAsync(apiUrl + paramRequest);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<ProductDetailsDTO>(responseStream);
            }
            return output;
        }

        public async Task<RecDetailDTO> GetRecruitmentDetails(int RecruitmentId)
        {
            RecDetailDTO output = new RecDetailDTO();
            string apiUrl = $"/api/v1/Recruitment/GetRecruitmentDetail";
            string paramRequest = $"?Recruitment_ID={RecruitmentId}";
            var response = await _client.GetAsync(apiUrl + paramRequest);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<RecDetailDTO>(responseStream);
            }
            return output;
        }

        public async Task<SponsorBanner> GetSponsorBannerProduct(int productCategoryId, int? productTypeId, int top)
        {
            SponsorBanner output = new SponsorBanner();
            string apiUrl = $"/api/v1/Advertising/GetSponsorBannerProduct";
            string paramRequest = $"?ProductCategoryId={productCategoryId}&ProductTypeId={productTypeId}&Top={top}";
            var response = await _client.GetAsync(apiUrl + paramRequest);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<SponsorBanner>(responseStream);
            }
            return output;
        }
        
        public async Task<ListProductCategoryDTO> GetSponsorBannerProduct(int? productCategoryId)
        {
            ListProductCategoryDTO output = new ListProductCategoryDTO();
            string apiUrl = $"/api/v1/ProductCategory/GetProdCateByParentID";
            string paramRequest = $"?parentId={productCategoryId}";
            var response = await _client.GetAsync(apiUrl + paramRequest);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<ListProductCategoryDTO>(responseStream);
            }
            return output;
        }

        public async Task<ListProductCategoryDTO> GetProdCateByParentID(string keyword, int? productCategoryId)
        {
            ListProductCategoryDTO output = new ListProductCategoryDTO();
            string apiUrl = $"/api/v1/ProductCategory/GetProdCateByParentID";
            string paramRequest = $"?keyword={keyword}&parentId={productCategoryId}";
            var response = await _client.GetAsync(apiUrl + paramRequest);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<ListProductCategoryDTO>(responseStream);
            }
            return output;
        }
        public async Task<List<PatchNumberDTO>> GetPartNumber(int? ProductCategoryId, int? ProductTypeId)
        {
            List<PatchNumberDTO> output = new List<PatchNumberDTO>();
            string apiUrl = $"/api/v1/Product/GetPartNumber";
            string paramRequest = $"?ProductCategoryId={ProductCategoryId}&ProductTypeId={ProductTypeId}";
            var response = await _client.GetAsync(apiUrl + paramRequest);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<List<PatchNumberDTO>>(responseStream);
            }
            return output;
        }

        public async Task<List<UnitDTO>> GetAllUnit()
        {
            List<UnitDTO> output = new List<UnitDTO>();
            string apiUrl = $"/api/v1/Unit/GetAllUnit";
            var response = await _client.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<List<UnitDTO>>(responseStream);
            }
            return output;
        }

        public async Task<List<CareerCategoryDTO>> GetAllCareerCategory()
        {
            List<CareerCategoryDTO> output = new List<CareerCategoryDTO>();
            string apiUrl = $"/api/v1/CareerCategory/GetAllCareerCategory";
            var response = await _client.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<List<CareerCategoryDTO>>(responseStream);
            }
            return output;
        }

        public async Task<List<Minor>> GetAllUnitService()
        {
            List<Minor> output = new List<Minor>();
            string apiUrl = $"/api/v1/Unit/GetListMinor";
            string paramRequest = $"?MajorId=3";
            var response = await _client.GetAsync(apiUrl+ paramRequest);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<List<Minor>>(responseStream);
            }
            return output;
        }

        public async Task<PostProductShopManResultDTO> CreateProduct(PostProductShopManDTO postProductBrandDTO)
        {
            PostProductShopManResultDTO output = new PostProductShopManResultDTO();
            var jwt = JsonConvert.DeserializeObject<SumProfileResponseDTO>(_httpContextAccessor.HttpContext.Request.Cookies["JWT"]);
            if (jwt != null)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt.JWT);
                postProductBrandDTO.UserId = jwt.UserId;
                postProductBrandDTO.Product.SalePhone = jwt.Profile.PhoneNumber;
                postProductBrandDTO.Product.SaleEmail = jwt.Profile.Email;

                string apiUrl = $"/api/v1/Product/PostProductShopMan";
                var json = JsonConvert.SerializeObject(postProductBrandDTO, Formatting.Indented);
                var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _client.PostAsync(apiUrl, stringContent);
                if (response.IsSuccessStatusCode)
                {
                    string responseStream = await response.Content.ReadAsStringAsync();
                    output = JsonConvert.DeserializeObject<PostProductShopManResultDTO>(responseStream);
                }
            }

            return output;
        }

        public async Task<PostProductShopManDTO> UpdateProduct(PostProductShopManDTO postProductBrandDTO, int ProductId, int IsRepair)
        {
            PostProductShopManDTO output = new PostProductShopManDTO();
            var jwt = JsonConvert.DeserializeObject<SumProfileResponseDTO>(_httpContextAccessor.HttpContext.Request.Cookies["JWT"]);
            if (jwt != null)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt.JWT);
                postProductBrandDTO.UserId = jwt.UserId;
                postProductBrandDTO.Product.SalePhone = jwt.Profile.PhoneNumber;
                postProductBrandDTO.Product.SaleEmail = jwt.Profile.Email;
                string apiUrl = $"/api/v1/Product/PutProductShopMan";
                string paramRequest = $"?ProductID={ProductId}&IsRepair={IsRepair}";
                var json = JsonConvert.SerializeObject(postProductBrandDTO, Formatting.Indented);
                var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _client.PutAsync(apiUrl + paramRequest, stringContent);
                if (response.IsSuccessStatusCode)
                {
                    string responseStream = await response.Content.ReadAsStringAsync();
                    output = JsonConvert.DeserializeObject<PostProductShopManDTO>(responseStream);
                }
            }

            return output;
        }

        public async Task<ReturnPostRecDTO> CreateRecruitment(SumPostRecDTO SumPostRecDTO)
        {
            ReturnPostRecDTO output = new ReturnPostRecDTO();
            var jwt = JsonConvert.DeserializeObject<SumProfileResponseDTO>(_httpContextAccessor.HttpContext.Request.Cookies["JWT"]);
            if (jwt != null)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt.JWT);
                SumPostRecDTO.UserId = jwt.UserId;
                string apiUrl = $"/api/v1/Recruitment/PostRec";
                var json = JsonConvert.SerializeObject(SumPostRecDTO, Formatting.Indented);
                var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _client.PostAsync(apiUrl, stringContent);
                if (response.IsSuccessStatusCode)
                {
                    string responseStream = await response.Content.ReadAsStringAsync();
                    output = JsonConvert.DeserializeObject<ReturnPostRecDTO>(responseStream);
                }
            }

            return output;
        }

        public async Task<ReturnPostRecDTO> UpdateRecruitment(SumPostRecDTO SumPostRecDTO, int RecruitmentId, int IsRepair)
        {
            ReturnPostRecDTO output = new ReturnPostRecDTO();
            var jwt = JsonConvert.DeserializeObject<SumProfileResponseDTO>(_httpContextAccessor.HttpContext.Request.Cookies["JWT"]);
            if (jwt != null)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt.JWT);
                SumPostRecDTO.UserId = jwt.UserId;
                string apiUrl = $"/api/v1/Recruitment/PutRec";
                string paramRequest = $"?Id={RecruitmentId}&IsRepair={IsRepair}";
                var json = JsonConvert.SerializeObject(SumPostRecDTO, Formatting.Indented);
                var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _client.PutAsync(apiUrl + paramRequest, stringContent);
                if (response.IsSuccessStatusCode)
                {
                    string responseStream = await response.Content.ReadAsStringAsync();
                    output = JsonConvert.DeserializeObject<ReturnPostRecDTO>(responseStream);
                }
            }

            return output;
        }

        public async Task<ReturnPostCVDTO> CreateCV(SumPostCurriculumnViateDTO SumPostCurriculumnViateDTO)
        {
            ReturnPostCVDTO output = new ReturnPostCVDTO();
            var jwt = JsonConvert.DeserializeObject<SumProfileResponseDTO>(_httpContextAccessor.HttpContext.Request.Cookies["JWT"]);
            if (jwt != null)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt.JWT);
                SumPostCurriculumnViateDTO.UserId = jwt.UserId;
                string apiUrl = $"/api/v1/CurriculumVitae/PostCV";
                var json = JsonConvert.SerializeObject(SumPostCurriculumnViateDTO, Formatting.Indented);
                var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _client.PostAsync(apiUrl, stringContent);
                if (response.IsSuccessStatusCode)
                {
                    string responseStream = await response.Content.ReadAsStringAsync();
                    output = JsonConvert.DeserializeObject<ReturnPostCVDTO>(responseStream);
                }
            }

            return output;
        }

        public async Task<ReturnPostCVDTO> UpdateCV(SumPostCurriculumnViateDTO SumPostCurriculumnViateDTO, int CVId, int IsRepair)
        {
            ReturnPostCVDTO output = new ReturnPostCVDTO();
            var jwt = JsonConvert.DeserializeObject<SumProfileResponseDTO>(_httpContextAccessor.HttpContext.Request.Cookies["JWT"]);
            if (jwt != null)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt.JWT);
                SumPostCurriculumnViateDTO.UserId = jwt.UserId;
                string apiUrl = $"/api/v1/CurriculumVitae/PutCV";
                string paramRequest = $"?Id={CVId}&IsRepair={IsRepair}";
                var json = JsonConvert.SerializeObject(SumPostCurriculumnViateDTO, Formatting.Indented);
                var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _client.PutAsync(apiUrl + paramRequest, stringContent);
                if (response.IsSuccessStatusCode)
                {
                    string responseStream = await response.Content.ReadAsStringAsync();
                    output = JsonConvert.DeserializeObject<ReturnPostCVDTO>(responseStream);
                }
            }

            return output;
        }

        public async Task<ProductIdDTO> ActionProductShopMan(ProductIdDTO productIdDTO, string Action)
        {
            ProductIdDTO output = new ProductIdDTO();
            var jwt = JsonConvert.DeserializeObject<SumProfileResponseDTO>(_httpContextAccessor.HttpContext.Request.Cookies["JWT"]);
            if (jwt != null)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt.JWT);
                productIdDTO.UserId = jwt.UserId;
                productIdDTO.ProductBrandId = jwt.ProductBrandId;
                string apiUrl = $"/api/v1/Product/ActionProductShopMan";
                string paramRequest = $"?Action={Action}";
                var json = JsonConvert.SerializeObject(productIdDTO, Formatting.Indented);
                var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _client.PostAsync(apiUrl + paramRequest, stringContent);
                if (response.IsSuccessStatusCode)
                {
                    string responseStream = await response.Content.ReadAsStringAsync();
                    output = JsonConvert.DeserializeObject<ProductIdDTO>(responseStream);
                }
            }

            return output;
        }

        public async Task<ModelBaseStatus> ActionInCV(int? Id, string Action)
        {
            ModelBaseStatus output = new ModelBaseStatus();
            var jwt = JsonConvert.DeserializeObject<SumProfileResponseDTO>(_httpContextAccessor.HttpContext.Request.Cookies["JWT"]);
            if (jwt != null)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt.JWT);
                string apiUrl = $"/api/v1/CurriculumVitae/ActionInCV";
                string paramRequest = $"?Id={Id}&Action={Action}";
                var response = await _client.GetAsync(apiUrl + paramRequest);
                if (response.IsSuccessStatusCode)
                {
                    string responseStream = await response.Content.ReadAsStringAsync();
                    output = JsonConvert.DeserializeObject<ModelBaseStatus>(responseStream);
                }
            }

            return output;
        }
        
        public async Task<ModelBaseStatus> ActionInRec(int? Id, string Action)
        {
            ModelBaseStatus output = new ModelBaseStatus();
            var jwt = JsonConvert.DeserializeObject<SumProfileResponseDTO>(_httpContextAccessor.HttpContext.Request.Cookies["JWT"]);
            if (jwt != null)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt.JWT);
                string apiUrl = $"/api/v1/Recruitment/ActionInRec";
                string paramRequest = $"?Id={Id}&Action={Action}";
                var response = await _client.GetAsync(apiUrl + paramRequest);
                if (response.IsSuccessStatusCode)
                {
                    string responseStream = await response.Content.ReadAsStringAsync();
                    output = JsonConvert.DeserializeObject<ModelBaseStatus>(responseStream);
                }
            }

            return output;
        }

        public async Task<ProductModelDTO> CreateModel(ProductModelDTO productModelDTO)
        {
            ProductModelDTO output = new ProductModelDTO();
            var jwt = JsonConvert.DeserializeObject<SumProfileResponseDTO>(_httpContextAccessor.HttpContext.Request.Cookies["JWT"]);
            if (jwt != null)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt.JWT);
                productModelDTO.UserId = jwt.UserId;
                
                string apiUrl = $"/api/v1/ProductModel/AddNewModel";
                var json = JsonConvert.SerializeObject(productModelDTO, Formatting.Indented);
                var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _client.PostAsync(apiUrl, stringContent);
                if (response.IsSuccessStatusCode)
                {
                    string responseStream = await response.Content.ReadAsStringAsync();
                    output = JsonConvert.DeserializeObject<ProductModelDTO>(responseStream);
                }
            }

            return output;
        }

        public async Task<ProductPaggingDTO> GetLstProductShopMan(ProductShopManFilter model)
        {
            ProductPaggingDTO output = new ProductPaggingDTO();
            var jwt = JsonConvert.DeserializeObject<SumProfileResponseDTO>(_httpContextAccessor.HttpContext.Request.Cookies["JWT"]);
            if (jwt != null)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt.JWT);
                
                string apiUrl = $"/api/v1/Product/GetLstProductShopMan";
                apiUrl = apiUrl + $"?Keyword={model.Keyword}&StatusTypeId={model.StatusTypeId}&ProductTypeId={model.ProductTypeId}&ProductCategoryId={model.ProductCategoryId}&" +
                    $"UserId={model.UserId}&Page={model.Page}&PageSize={model.PageSize}";                
                var response = await _client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    string responseStream = await response.Content.ReadAsStringAsync();
                    output = JsonConvert.DeserializeObject<ProductPaggingDTO>(responseStream);
                }
            }

            return output;
        }

        public async Task<ModelBaseStatus> CheckPostProduct(int? ProductBrandId)
        {
            ModelBaseStatus output = new ModelBaseStatus();
            var jwt = JsonConvert.DeserializeObject<SumProfileResponseDTO>(_httpContextAccessor.HttpContext.Request.Cookies["JWT"]);
            if (jwt != null)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt.JWT);

                string apiUrl = $"/api/v1/Product/CheckPostProduct";
                apiUrl = apiUrl + $"?ProductBrandId={ProductBrandId}";
                var response = await _client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    string responseStream = await response.Content.ReadAsStringAsync();
                    output = JsonConvert.DeserializeObject<ModelBaseStatus>(responseStream);
                }
            }

            return output;
        }

        public async Task<ModelBaseStatus> CheckRenewProduct()
        {
            ModelBaseStatus output = new ModelBaseStatus();
            var jwt = JsonConvert.DeserializeObject<SumProfileResponseDTO>(_httpContextAccessor.HttpContext.Request.Cookies["JWT"]);
            if (jwt != null)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt.JWT);

                string apiUrl = $"/api/v1/Product/CheckRenew";
                apiUrl = apiUrl + $"?ProductBrandId={jwt.ProductBrandId}";
                var response = await _client.GetAsync(apiUrl);
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
