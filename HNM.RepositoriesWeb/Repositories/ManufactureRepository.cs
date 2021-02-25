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
    public interface IManufactureRepository
    {
        Task<ListManufactureSearchDTO> SearchManufactures(int productCategoryId, int? productTypeId, string search = "");
        Task<List<ProductManufacture_SearchByCategory_Result_DTO>> GetShopmanManuFactory();
    }
    public class ManufactureRepository : IRepositoryBase, IManufactureRepository
    {
        private readonly HttpClient _client;
        private IMapper _mapper;
        public ManufactureRepository(HttpClient client, IMapper mapper)
        {
            _client = client;
            _mapper = mapper;
        }
        public async Task<ListManufactureSearchDTO> SearchManufactures(int productCategoryId, int? productTypeId, string search = "")
        {
            ListManufactureSearchDTO output = new ListManufactureSearchDTO();
            string apiUrl = $"/api/v1/Manufacture/Search";
            string paramRequest = $"?productCategoryId={productCategoryId}&productTypeId={productTypeId}&search={search}";
            var response = await _client.GetAsync(apiUrl + paramRequest);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<ListManufactureSearchDTO>(responseStream);
            }
            return output;
        }

        public async Task<List<ProductManufacture_SearchByCategory_Result_DTO>> GetShopmanManuFactory()
        {
            List<ProductManufacture_SearchByCategory_Result_DTO> output = new List<ProductManufacture_SearchByCategory_Result_DTO>();
            string apiUrl = $"/api/v1/Manufacture/GetShopmanManuFactory";
            var response = await _client.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<List<ProductManufacture_SearchByCategory_Result_DTO>>(responseStream);
            }
            return output;
        }
    }

}
