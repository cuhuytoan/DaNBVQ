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
    public interface ISubscriptionRepository
    {
        Task<Subscription> GetByEmail(string email);
        Task<SubscriptionResultDTO> SubscriptionEmail(string email);
    }
    public class SubscriptionRepository : IRepositoryBase, ISubscriptionRepository
    {
        private readonly HttpClient _client;
        private IMapper _mapper;
        public SubscriptionRepository(HttpClient client, IMapper mapper)
        {
            _client = client;
            _mapper = mapper;
        }
        public async Task<Subscription> GetByEmail(string email)
        {
            Subscription output = new Subscription();
            string apiUrl = $"/api/v1/Subscription/GetByEmail";
            string paramRequest = $"?email={email}";
            var response = await _client.GetAsync(apiUrl + paramRequest);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<Subscription>(responseStream);
            }
            return output;
        }
        public async Task<SubscriptionResultDTO> SubscriptionEmail(string email)
        {
            SubscriptionResultDTO output = new SubscriptionResultDTO();
            string apiUrl = $"/api/v1/Subscription/SubscriptionEmail";
            var jsonData = JsonConvert.SerializeObject(new SubscriptionRequestDTO { Email = email });
            var stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(apiUrl, stringContent);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<SubscriptionResultDTO>(responseStream);
            }
            return output;
        }
    }
}
