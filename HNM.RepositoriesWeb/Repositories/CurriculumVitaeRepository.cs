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
    public interface ICurriculumVitaeRepository
    {
        Task<List<CareerCategoryDTO>> GetAllCareerCategory();
        Task<List<CVSearchDTO>> GetListCVPagging(int careerCategoryId, int? productCategoryId, string provinceName, int? YearOfExprience, decimal? MinSalary, decimal? MaxSalary, int page, int? pageSize, int statusTypeId);
        Task<CVDetailDTO> GetDetailsCV(int CVId);
        Task<List<CurriculumVitaeDTO>> GetRelateCurriculumVitae(int CareerCategoryId, int CurriculumVitaeId);
        Task<CareerCategoryDTO> GetDetailCareerCategory(int CareerCategoryId);
    }
    public class CurriculumVitaeRepository: IRepositoryBase, ICurriculumVitaeRepository
    {
        private readonly HttpClient _client;
        private IMapper _mapper;
        public CurriculumVitaeRepository(HttpClient client, IMapper mapper)
        {
            _client = client;
            _mapper = mapper;
        }

        public async Task<List<CareerCategoryDTO>> GetAllCareerCategory()
        {
            List<CareerCategoryDTO> output = new List<CareerCategoryDTO>();
            string apiUrl = $"/api/v1/CareerCategory/GetAllCareerCategory";
            var response = await _client.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<List<CareerCategoryDTO>>(responseStream);
            }
            return output;
        }

        public async Task<List<CVSearchDTO>> GetListCVPagging(int careerCategoryId, int? productCategoryId, string provinceName, int ? YearOfExprience, decimal? MinSalary, decimal? MaxSalary, int page, int? pageSize, int statusTypeId)
        {
            List<CVSearchDTO> output = new List<CVSearchDTO>();
            string apiUrl = $"/api/v1/CurriculumVitae/GetListCVPagging";
            string paramRequest = $"?CareerCategoryId={careerCategoryId}&MinSalary={MinSalary}&MaxSalary={MaxSalary}&Page={page}&PageSize={pageSize}&StatusTypeId={statusTypeId}";
            if(productCategoryId != null && provinceName == "null" && YearOfExprience == null)
            {
                paramRequest = $"?CareerCategoryId={careerCategoryId}&ProductCategoryId={productCategoryId}&MinSalary={MinSalary}&MaxSalary={MaxSalary}&Page={page}&PageSize={pageSize}&StatusTypeId={statusTypeId}";
            }

            if (productCategoryId == null && provinceName != "null" && YearOfExprience == null)
            {
                paramRequest = $"?CareerCategoryId={careerCategoryId}&ProvinceName={provinceName}&MinSalary={MinSalary}&MaxSalary={MaxSalary}&Page={page}&PageSize={pageSize}&StatusTypeId={statusTypeId}";
            }
            if (productCategoryId == null && provinceName == "null" && YearOfExprience != null)
            {
                paramRequest = $"?CareerCategoryId={careerCategoryId}&YearOfExprience={YearOfExprience}&MinSalary={MinSalary}&MaxSalary={MaxSalary}&Page={page}&PageSize={pageSize}&StatusTypeId={statusTypeId}";
            }

            if (productCategoryId != null && provinceName != "null" && YearOfExprience == null)
            {
                paramRequest = $"?CareerCategoryId={careerCategoryId}&ProductCategoryId={productCategoryId}&ProvinceName={provinceName}&MinSalary={MinSalary}&MaxSalary={MaxSalary}&Page={page}&PageSize={pageSize}&StatusTypeId={statusTypeId}";
            }
            if (productCategoryId != null && provinceName == "null" && YearOfExprience != null)
            {
                paramRequest = $"?CareerCategoryId={careerCategoryId}&ProductCategoryId={productCategoryId}&YearOfExprience={YearOfExprience}&MinSalary={MinSalary}&MaxSalary={MaxSalary}&Page={page}&PageSize={pageSize}&StatusTypeId={statusTypeId}";
            }

            if (productCategoryId == null && provinceName != "null" && YearOfExprience != null)
            {
                paramRequest = $"?CareerCategoryId={careerCategoryId}&ProvinceName={provinceName}&YearOfExprience={YearOfExprience}&MinSalary={MinSalary}&MaxSalary={MaxSalary}&Page={page}&PageSize={pageSize}&StatusTypeId={statusTypeId}";
            }

            if (productCategoryId != null && provinceName != "null" && YearOfExprience != null)
            {
                paramRequest = $"?CareerCategoryId={careerCategoryId}&ProductCategoryId={productCategoryId}&ProvinceName={provinceName}&YearOfExprience={YearOfExprience}" +
                $"&MinSalary={MinSalary}&MaxSalary={MaxSalary}&Page={page}&PageSize={pageSize}&StatusTypeId={statusTypeId}";
            }

            var response = await _client.GetAsync(apiUrl + paramRequest);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<List<CVSearchDTO>>(responseStream);
            }
            return output;
        }
        
        public async Task<CVDetailDTO> GetDetailsCV(int CVId)
        {
            CVDetailDTO output = new CVDetailDTO();
            string apiUrl = $"/api/v1/CurriculumVitae/GetDetailsCV";
            string paramRequest = $"?CVId={CVId}";
            var response = await _client.GetAsync(apiUrl + paramRequest);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<CVDetailDTO>(responseStream);
            }
            return output;
        }
        
        public async Task<CareerCategoryDTO> GetDetailCareerCategory(int CareerCategoryId)
        {
            CareerCategoryDTO output = new CareerCategoryDTO();
            string apiUrl = $"/api/v1/CareerCategory/GetDetailCareerCategory";
            string paramRequest = $"?CareerCategoryId={CareerCategoryId}";
            var response = await _client.GetAsync(apiUrl + paramRequest);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<CareerCategoryDTO>(responseStream);
            }
            return output;
        }
        
        public async Task<List<CurriculumVitaeDTO>> GetRelateCurriculumVitae(int CareerCategoryId, int CurriculumVitaeId)
        {
            List<CurriculumVitaeDTO> output = new List<CurriculumVitaeDTO>();
            string apiUrl = $"/api/v1/CurriculumVitae/GetRelateCurriculumVitae";
            string paramRequest = $"?CareerCategoryId={CareerCategoryId}&CurriculumVitaeId={CurriculumVitaeId}";
            var response = await _client.GetAsync(apiUrl + paramRequest);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<List<CurriculumVitaeDTO>>(responseStream);
            }
            return output;
        }
    }
}
