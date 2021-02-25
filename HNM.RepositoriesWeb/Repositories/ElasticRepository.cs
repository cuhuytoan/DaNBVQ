using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HNM.DataNC.ElasticModels;
using HNM.DataNC.ModelsFilter;
using HNM.DataNC.ModelsNC.ModelsDTO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HNM.RepositoriesWeb.Repositories
{
    public interface IElasticRepository
    {
        Task<ProductPaggingDTO> SearchProducts(int from = 0, int size = 20, string query = "");
        Task<BrandPaggingDTO> SearchProductBrand(int page = 0, int pageSize = 20, string query = "");
    }
    public class ElasticRepository : IElasticRepository
    {
        private readonly HttpClient _client;
        private IMapper _mapper;
        public ElasticRepository(HttpClient client, IMapper mapper)
        {
            _client = client;
            _mapper = mapper;
        }
        public async Task<ProductPaggingDTO> SearchProducts(int page = 0, int pageSize = 20, string query = "")
        {
            var output = new ProductPaggingDTO();
            string apiUrl = $"/api/v1/Search/ElasticProductSearch";
            string paramRequest = $"?page={page}&pageSize={pageSize}&search={query}";
            var response = await _client.GetAsync(apiUrl + paramRequest);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<ProductPaggingDTO>(responseStream);
            }
            return output;
        }
        public async Task<BrandPaggingDTO> SearchProductBrand(int page = 0, int pageSize = 20, string query = "")
        {
            var output = new BrandPaggingDTO();
            string apiUrl = $"/api/v1/Search/ElasticBrandSearch";
            string paramRequest = $"?page={page}&pageSize={pageSize}&search={query}";
            var response = await _client.GetAsync(apiUrl + paramRequest);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<BrandPaggingDTO>(responseStream);
            }
            return output;

        }
    }
}
