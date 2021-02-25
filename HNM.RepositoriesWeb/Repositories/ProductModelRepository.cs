using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using HNM.DataNC.Models;
using HNM.DataNC.ModelsNC.ModelsDTO;
using HNM.RepositoriesWeb.RepositoriesBase;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HNM.RepositoriesWeb.Repositories
{
    public interface IProductModelRepository
    {
        Task<ListProductModelSearchDTO> SearchProductModels(int? productCategoryId, int? productTypeId, int? manufactoryId, string search); 
    }
    public class ProductModelRepository : IRepositoryBase, IProductModelRepository
    {
        private readonly HttpClient _client;
        private IMapper _mapper;
        public ProductModelRepository(HttpClient client, IMapper mapper)
        {
            _client = client;
            _mapper = mapper;
        }
        public async Task<ListProductModelSearchDTO> SearchProductModels(int? productCategoryId, int? productTypeId, int? manufactoryId, string search)
        {
            var output = new ListProductModelSearchDTO();
            string apiUrl = $"/api/v1/ProductModel/Search";
            string paramRequest = $"?productCategoryId={productCategoryId}&productTypeId={productTypeId}&manufactoryId={manufactoryId}&search={search}";
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
