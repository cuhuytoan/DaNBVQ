using AutoMapper;
using HNM.DataNC.ModelsNC.ModelsDTO;
using HNM.DataNC.ModelsStore;
using HNM.RepositoriesWeb.RepositoriesBase;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HNM.RepositoriesWeb.Repositories
{
    public interface IProductBrandRepository
    {
        Task<ProductBrandSearchDetailDTO> GetBrandDetails(int ProductBrandId);
        Task<ProductPaggingDTO> GetListProductByProductBrand(int page, int pageSize, int statusTypeId, int productBrandId);
        Task<List<Library_Search_By_Cate_Result>> GetListLibByProductBrand(int? page, int? pageSize, int statusTypeId, string createBy);
    }
    public class ProductBrandRepository : IRepositoryBase, IProductBrandRepository
    {
        private readonly HttpClient _client;
        private IMapper _maper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ProductBrandRepository(HttpClient client, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _client = client;
            _maper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ProductBrandSearchDetailDTO> GetBrandDetails(int ProductBrandId)
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

        public async Task<ProductPaggingDTO> GetListProductByProductBrand(int page, int pageSize, int statusTypeId, int productBrandId)
        {
            ProductPaggingDTO output = new ProductPaggingDTO();
            string apiUrl = $"/api/v1/Product/GetListProduct";
            string paramRequest = $"?Page={page}&PageSize={pageSize}&StatusTypeId={statusTypeId}&ProductBrandId={productBrandId}";
            var response = await _client.GetAsync(apiUrl + paramRequest);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<ProductPaggingDTO>(responseStream);
            }
            return output;
        }

        public async Task<List<Library_Search_By_Cate_Result>> GetListLibByProductBrand(int? page, int? pageSize, int statusTypeId, string createBy)
        {
            List<Library_Search_By_Cate_Result> output = new List<Library_Search_By_Cate_Result>();
            string apiUrl = $"/api/v1/Library/GetListLibraryPagging";
            string paramRequest = $"?Page={page}&PageSize={pageSize}&StatusTypeId={statusTypeId}&CreateBy={createBy}";
            var response = await _client.GetAsync(apiUrl + paramRequest);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<List<Library_Search_By_Cate_Result>>(responseStream);
            }
            return output;
        }
    }
}
