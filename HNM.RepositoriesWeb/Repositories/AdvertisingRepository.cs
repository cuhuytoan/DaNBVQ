using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HNM.DataNC.ModelsNC.ModelsDTO;
using HNM.RepositoriesWeb.RepositoriesBase;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HNM.RepositoriesWeb.Repositories
{
    public interface IAdvertisingRepository
    {
        Task<ListAdvertisingCarouselDTO> GetHomeCarousel();
    }
    public class AdvertisingRepository :IRepositoryBase,IAdvertisingRepository
    {
        private readonly HttpClient _client;
        private IMapper _mapper;
        public AdvertisingRepository(HttpClient client, IMapper mapper)
        {
            _client = client;
            _mapper = mapper;
        }

        public async Task<ListAdvertisingCarouselDTO> GetHomeCarousel()
        {
            ListAdvertisingCarouselDTO lstCarousel = new ListAdvertisingCarouselDTO();            
            string apiUrl = "/api/v1/Advertising/GetCarouselList";
            var response = await _client.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                lstCarousel = JsonConvert.DeserializeObject<ListAdvertisingCarouselDTO>(responseStream);

            }
            return lstCarousel;
        }
    }
}
