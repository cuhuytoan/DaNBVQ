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
    public interface IManagerDemandRepository
    {
        Task<List<TimeLinePostDTO>> GetLstTimeLinePost(int? Page, int? PageSize);
    }

    class ManagerDemandRepository : IRepositoryBase, IManagerDemandRepository
    {
        private readonly HttpClient _client;
        private IMapper _mapper;
        private IHttpContextAccessor _httpContextAccessor;
        public ManagerDemandRepository(HttpClient client, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _client = client;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<TimeLinePostDTO>> GetLstTimeLinePost(int? Page, int? PageSize)
        {
            List<TimeLinePostDTO> output = new List<TimeLinePostDTO>();
            var jwt = JsonConvert.DeserializeObject<SumProfileResponseDTO>(_httpContextAccessor.HttpContext.Request.Cookies["JWT"]);
            if (jwt != null)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt.JWT);

                string apiUrl = $"/api/v1/Product/GetLstTimeLinePost";
                string paramRequest = $"?UserId={jwt.UserId}&Page={Page}&PageSize={PageSize}";
                var response = await _client.GetAsync(apiUrl + paramRequest);
                if (response.IsSuccessStatusCode)
                {
                    string responseStream = await response.Content.ReadAsStringAsync();
                    output = JsonConvert.DeserializeObject<List<TimeLinePostDTO>>(responseStream);
                }
            }
            return output;
        }
    }
}
