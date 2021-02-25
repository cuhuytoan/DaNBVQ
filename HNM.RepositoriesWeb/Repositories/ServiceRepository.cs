using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HNM.DataNC.ModelsNC.ModelsDTO;
using HNM.RepositoriesWeb.RepositoriesBase;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HNM.RepositoriesWeb.Repositories
{
    public interface IServiceRepository
    {
        Task<ProductPaggingDTO> GetListService(int page, int pageSize, int statusTypeId, int productTypeId);
        Task<ListProductCategoryDTO> GetProdCateByParentID(int parentId);
        Task<ProductDetailsDTO> GetProductDetails(int productId);
        Task<SponsorBanner> GetSponsorBannerProduct(int productCategoryId, int productTypeId, int top);
        Task<ProductPaggingDTO> GetListServiceByCate(int page, int pageSize, int statusTypeId, int productTypeId, int productCategoryId, int? locationId, int ? relatedCategoryId);
        Task<ProductPaggingDTO> GetListServiceByProductBrand(int page, int pageSize, int statusTypeId, int productTypeId, int productBrandId);
        Task<List<ProductManufacture_SearchByCategory_Result_DTO>> GetListManuByCate(string keyword);
        Task<ListProductModelSearchDTO> GetListModelByManu(int? manufactoryId, int ? ProductCategoryId,string keyword);
    }
    public class ServiceRepository : IRepositoryBase, IServiceRepository
    {
        private readonly HttpClient _client;
        private IMapper _mapper;
        private IHttpContextAccessor _httpContextAccessor;
        public ServiceRepository(HttpClient client, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _client = client;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        // lấy ra tất cả dv
        public async Task<ProductPaggingDTO> GetListService(int page, int pageSize, int statusTypeId, int productTypeId)
        {
            ProductPaggingDTO output = new ProductPaggingDTO();
            string apiUrl = $"/api/v1/Product/GetListProduct";
            string paramRequest = $"?Page={page}&PageSize={pageSize}&StatusTypeId={statusTypeId}&ProductTypeId={productTypeId}";
            var response = await _client.GetAsync(apiUrl + paramRequest);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<ProductPaggingDTO>(responseStream);
            }
            return output;
        }

        //lấy ra ds dv theo chuyên mục
        public async Task<ProductPaggingDTO> GetListServiceByCate(int page, int pageSize, int statusTypeId, int productTypeId, int productCategoryId, int? locationId, int? relatedCategoryId)
        {
            ProductPaggingDTO output = new ProductPaggingDTO();
            string apiUrl = $"/api/v1/Product/GetListProduct";
            string paramRequest = $"?Page={page}&PageSize={pageSize}&StatusTypeId={statusTypeId}&ProductTypeId={productTypeId}&ProductCategoryId={productCategoryId}&LocationId={locationId}&RelatedCategoryId={relatedCategoryId}";
            var response = await _client.GetAsync(apiUrl + paramRequest);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<ProductPaggingDTO>(responseStream);
            }
            return output;
        }

        public async Task<ProductPaggingDTO> GetListServiceByProductBrand(int page, int pageSize, int statusTypeId, int productTypeId, int productBrandId)
        {
            ProductPaggingDTO output = new ProductPaggingDTO();
            string apiUrl = $"/api/v1/Product/GetListProduct";
            string paramRequest = $"?Page={page}&PageSize={pageSize}&StatusTypeId={statusTypeId}&ProductTypeId={productTypeId}&ProductBrandId={productBrandId}";
            var response = await _client.GetAsync(apiUrl + paramRequest);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<ProductPaggingDTO>(responseStream);
            }
            return output;
        }

        public async Task<ListProductCategoryDTO> GetProdCateByParentID(int parentId)
        {
            ListProductCategoryDTO output = new ListProductCategoryDTO();
            string apiUrl = $"/api/v1/ProductCategory/GetProdCateByParentID";
            string paramRequest = $"?parentId={parentId}";
            var response = await _client.GetAsync(apiUrl + paramRequest);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<ListProductCategoryDTO>(responseStream);
            }
            return output;
        }
        
        public async Task<ProductDetailsDTO> GetProductDetails(int productId)
        {
            ProductDetailsDTO output = new ProductDetailsDTO();
            string apiUrl = $"/api/v1/Product/GetProductDetails";
            string paramRequest = $"?productId={productId}";
            var response = await _client.GetAsync(apiUrl + paramRequest);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<ProductDetailsDTO>(responseStream);
            }
            return output;
        }

        public async Task<SponsorBanner> GetSponsorBannerProduct(int productCategoryId, int productTypeId, int top)
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

        public async Task<List<ProductManufacture_SearchByCategory_Result_DTO>> GetListManuByCate(string keyword)
        {
            List<ProductManufacture_SearchByCategory_Result_DTO> output = new List<ProductManufacture_SearchByCategory_Result_DTO>();
            
            var jwt = JsonConvert.DeserializeObject<SumProfileResponseDTO>(_httpContextAccessor.HttpContext.Request.Cookies["JWT"]);
            if (jwt != null)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt.JWT);
                string apiUrl = $"/api/v1/Manufacture/GetShopmanManuFactory?keyword={keyword}";
                var response = await _client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    string responseStream = await response.Content.ReadAsStringAsync();
                    output = JsonConvert.DeserializeObject<List<ProductManufacture_SearchByCategory_Result_DTO>>(responseStream);
                }
            }

            return output;
        }

        public async Task<ListProductModelSearchDTO> GetListModelByManu(int? manufactoryId, int? ProductCategoryId,string keyword ="")
        {
            ListProductModelSearchDTO output = new ListProductModelSearchDTO();
            string apiUrl = $"/api/v1/ProductModel/ShopmanSearchModel";
            string paramRequest = "";
            if (String.IsNullOrEmpty(keyword))
            {
                paramRequest = $"?manufactoryId={manufactoryId}&productCategoryId={ProductCategoryId}";
            }
            else
            {
                paramRequest = $"?manufactoryId={manufactoryId}&productCategoryId={ProductCategoryId}&search={keyword}";
            }             
            var response = await _client.GetAsync(apiUrl + paramRequest);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<ListProductModelSearchDTO>(responseStream);
            }
            return output;
        }
    }
}
