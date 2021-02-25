using AutoMapper;
using HNM.DataNC.ModelsNC.ModelsDTO;
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
    public interface IShopmanProductRepository
    {
        Task<ProductPaggingDTO> GetLstProductShopMan(string Keyword, int? page, int? pageSize, int? StatusTypeId, int? ProductTypeId, int? ProductCategoryId);
        Task<ProductPaggingDTO> GetLstProduct(string Keyword, int? page, int? pageSize, int? StatusTypeId, int? ProductTypeId, int? ProductCategoryId);
    }


    public class ShopmanProductRepository : IRepositoryBase, IShopmanProductRepository
    {
        private readonly HttpClient _client;
        private IMapper _mapper;
        private IHttpContextAccessor _httpContextAccessor;
        public ShopmanProductRepository(HttpClient client, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _client = client;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ProductPaggingDTO> GetLstProductShopMan(string Keyword, int? page, int? pageSize, int? StatusTypeId, int? ProductTypeId, int? ProductCategoryId)
        {
            ProductPaggingDTO output = new ProductPaggingDTO();
            var jwt = JsonConvert.DeserializeObject<SumProfileResponseDTO>(_httpContextAccessor.HttpContext.Request.Cookies["JWT"]);
            if (jwt != null)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt.JWT);
                string apiUrl = $"/api/v1/Product/GetLstProductForSellShopMan";
                string paramRequest = $"?Keyword={Keyword}&Page={page}&PageSize={pageSize}&StatusTypeId={StatusTypeId}&ProductTypeId={ProductTypeId}&ProductCategoryId={ProductCategoryId}&UserId={jwt.UserId}";
                var response = await _client.GetAsync(apiUrl + paramRequest);
                if (response.IsSuccessStatusCode)
                {
                    string responseStream = await response.Content.ReadAsStringAsync();
                    output = JsonConvert.DeserializeObject<ProductPaggingDTO>(responseStream);
                }
            }

            return output;
        }

        public async Task<ProductPaggingDTO> GetLstProduct(string Keyword, int? page, int? pageSize, int? StatusTypeId, int? ProductTypeId, int? ProductCategoryId)
        {
            ProductPaggingDTO output = new ProductPaggingDTO();
            var jwt = JsonConvert.DeserializeObject<SumProfileResponseDTO>(_httpContextAccessor.HttpContext.Request.Cookies["JWT"]);
            if (jwt != null)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt.JWT);
                string apiUrl = $"/api/v1/Product/GetLstProductShopMan";
                string paramRequest = $"?Keyword={Keyword}&Page={page}&PageSize={pageSize}&StatusTypeId={StatusTypeId}&ProductTypeId={ProductTypeId}&ProductCategoryId={ProductCategoryId}&UserId={jwt.UserId}";
                var response = await _client.GetAsync(apiUrl + paramRequest);
                if (response.IsSuccessStatusCode)
                {
                    string responseStream = await response.Content.ReadAsStringAsync();
                    output = JsonConvert.DeserializeObject<ProductPaggingDTO>(responseStream);
                }
            }

            return output;
        }

    }
}
