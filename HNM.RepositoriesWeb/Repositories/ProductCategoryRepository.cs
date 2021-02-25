using HNM.RepositoriesWeb.RepositoriesBase;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HNM.DataNC.ModelsNC.ModelsDTO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HNM.RepositoriesWeb.Repositories
{
    public interface IProductCategoryRepository
    {
        Task<ListProductCategoryDTO> GetLstMenuByParentId(int parentID);
        Task<List<ProductCategoryTwoLevelDTO>> GetProdCateTwoLevel(int ProductCategoryId);
        Task<ProductCategoryDTO> GetDetailProductCategory(int ProductCategoryId);

        Task<List<ListAllProductCategoryDTO>> GetAllProductCategory();
    }
    public class ProductCategoryRepository :IRepositoryBase, IProductCategoryRepository
    {
        private readonly HttpClient _client;
        private IMapper _mapper;
        public ProductCategoryRepository(HttpClient client, IMapper mapper)
        {
            _client = client;
            _mapper = mapper;
        }

        public async Task<ListProductCategoryDTO> GetLstMenuByParentId(int parentID)
        {
            ListProductCategoryDTO output = new ListProductCategoryDTO();            
            string apiUrl = $"/api/v1/ProductCategory/GetProdCateByParentID";
            string paramRequest = $"?parentId={parentID}";
            var response = await _client.GetAsync(apiUrl + paramRequest);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<ListProductCategoryDTO>(responseStream);
            }
            return output;
        }
        public async Task<List<ProductCategoryTwoLevelDTO>> GetProdCateTwoLevel(int parentID)
        {
            List<ProductCategoryTwoLevelDTO> output = new List<ProductCategoryTwoLevelDTO>();
            string apiUrl = $"/api/v1/ProductCategory/GetProdCateTwoLevel";
            string paramRequest = $"?ProductCategoryId={parentID}";
            var response = await _client.GetAsync(apiUrl + paramRequest);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<List<ProductCategoryTwoLevelDTO>>(responseStream);
            }
            return output;
        }

        public async Task<ProductCategoryDTO> GetDetailProductCategory(int ProductCategoryId)
        {
            ProductCategoryDTO output = new ProductCategoryDTO();
            string apiUrl = $"/api/v1/ProductCategory/GetDetailProductCategory";
            string paramRequest = $"?ProductCategoryId={ProductCategoryId}";
            var response = await _client.GetAsync(apiUrl + paramRequest);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<ProductCategoryDTO>(responseStream);
            }
            return output;
        }

        public async Task<List<ListAllProductCategoryDTO>> GetAllProductCategory()
        {
            List<ListAllProductCategoryDTO> output = new List<ListAllProductCategoryDTO>();
            string apiUrl = $"/api/v1/ProductCategory/GetAllProductcategory";            
            var response = await _client.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<List<ListAllProductCategoryDTO>>(responseStream);
            }
            return output;
        }
    }
}
