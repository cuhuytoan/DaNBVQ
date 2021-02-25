using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HNM.DataNC.ModelsNC.ModelsDTO;
using HNM.DataNC.ModelsStore;
using HNM.RepositoriesWeb.RepositoriesBase;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HNM.RepositoriesWeb.Repositories
{
    public interface IRecruitmentRepository
    {
        Task<ListRecruitmentCategoryDTO> GetAllRecruitmentCategory();
        Task<RecDetailDTO> GetRecruitmentDetail(int Recruitment_ID);
        Task<RecruitmentPaggingDTO> GetListRecruitmentByCate(int recruitmentCategoryId, int page, int? pageSize, int statusTypeId);
        Task<RecruitmentCategoryDTO> GetRecruitmentById(int ID);
        Task<RecruitmentPaggingDTO> SearchRecruitment(string keyword, int page, int pageSize, int statusTypeId);
    }
    public class RecruitmentRepository: IRepositoryBase, IRecruitmentRepository
    {
        private readonly HttpClient _client;
        private IMapper _mapper;
        public RecruitmentRepository(HttpClient client, IMapper mapper)
        {
            _client = client;
            _mapper = mapper;
        }

        public async Task<ListRecruitmentCategoryDTO> GetAllRecruitmentCategory()
        {
            ListRecruitmentCategoryDTO output = new ListRecruitmentCategoryDTO();
            string apiUrl = $"/api/v1/RecruitmentCategory/GetAll";
            var response = await _client.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<ListRecruitmentCategoryDTO>(responseStream);
            }
            return output;
        }

        public async Task<RecDetailDTO> GetRecruitmentDetail(int Recruitment_ID)
        {
            RecDetailDTO output = new RecDetailDTO();
            string apiUrl = $"/api/v1/Recruitment/GetRecruitmentDetail";
            string paramRequest = $"?Recruitment_ID={Recruitment_ID}";
            var response = await _client.GetAsync(apiUrl + paramRequest);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<RecDetailDTO>(responseStream);
            }
            return output;
        }

        public async Task<RecruitmentCategoryDTO> GetRecruitmentById(int ID)
        {
            RecruitmentCategoryDTO output = new RecruitmentCategoryDTO();
            string apiUrl = $"/api/v1/RecruitmentCategory/GetRecruitmentById";
            string paramRequest = $"?recruitmentId={ID}";
            var response = await _client.GetAsync(apiUrl + paramRequest);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<RecruitmentCategoryDTO>(responseStream);
            }
            return output;
        }

        public async Task<RecruitmentPaggingDTO> GetListRecruitmentByCate(int recruitmentCategoryId, int page, int? pageSize, int statusTypeId)
        {
            RecruitmentPaggingDTO output = new RecruitmentPaggingDTO();
            string apiUrl = $"/api/v1/Recruitment/GetListRecruitment";
            string paramRequest = $"?RecruitmentCategoryId={recruitmentCategoryId}&Page={page}&PageSize={pageSize}&StatusTypeId={statusTypeId}";
            var response = await _client.GetAsync(apiUrl + paramRequest);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<RecruitmentPaggingDTO>(responseStream);
            }
            return output;
        }
        public async Task<RecruitmentPaggingDTO> SearchRecruitment(string keyword, int page, int pageSize, int statusTypeId)
        {
            RecruitmentPaggingDTO output = new RecruitmentPaggingDTO();
            string apiUrl = $"/api/v1/Recruitment/SearchRecruitment";
            string paramRequest = $"?Keyword={keyword}&Page={page}&PageSize={pageSize}&StatusTypeId={statusTypeId}";
            var response = await _client.GetAsync(apiUrl + paramRequest);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<RecruitmentPaggingDTO>(responseStream);
            }
            return output;
        }
    }
}
