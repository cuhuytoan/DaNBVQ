using AutoMapper;
using HNM.DataNC.ModelsNC.ModelsDTO;
using HNM.DataNC.ModelsStore;
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
    public interface ILibraryRepository
    {
        Task<List<Library_Search_By_Cate_Result>> GetLstLibByCate(int LibraryCategoryID, int page, int pageSize);
        Task<LibraryDTO> DetailLibrary(int? Library_ID);
        Task<List<LibraryDTO>> LstRelatelLibrary(int LibraryCategoryId, int? Library_ID);
        Task<LibraryCategoryDTO> LibraryCategoryByUrl(string url);
        Task<LibraryCategoryDTO> LibraryCategoryById(int Id);
        Task<PostLibraryDTO> CreateLibrary(PostLibraryDTO postLibraryDTO);
    }

    public class LibraryRepository: IRepositoryBase, ILibraryRepository
    {
        private readonly HttpClient _client;
        private IMapper _mapper;
        private IHttpContextAccessor _httpContextAccessor;
        public LibraryRepository(HttpClient client, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _client = client;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<Library_Search_By_Cate_Result>> GetLstLibByCate(int LibraryCategoryID, int page, int pageSize)
        {
            List<Library_Search_By_Cate_Result> output = new List<Library_Search_By_Cate_Result>();
            string apiUrl = $"/api/v1/Library/GetListLibraryPagging";
            string paramRequest = $"?LibraryCategoryID={LibraryCategoryID}&Page={page}&PageSize={pageSize}";
            var response = await _client.GetAsync(apiUrl + paramRequest);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<List<Library_Search_By_Cate_Result>>(responseStream);
            }
            return output;
        }


        public async Task<LibraryCategoryDTO> LibraryCategoryByUrl(string url)
        {
            LibraryCategoryDTO output = new LibraryCategoryDTO();
            string apiUrl = $"/api/v1/LibraryCategory/GetLibraryCategoryByUrl";
            string paramRequest = $"?Url={url}";
            var response = await _client.GetAsync(apiUrl + paramRequest);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<LibraryCategoryDTO>(responseStream);
            }
            return output;
        }

        public async Task<LibraryCategoryDTO> LibraryCategoryById(int Id)
        {
            LibraryCategoryDTO output = new LibraryCategoryDTO();
            string apiUrl = $"/api/v1/LibraryCategory/GetLibraryCategoryById";
            string paramRequest = $"?LibraryCategoryId={Id}";
            var response = await _client.GetAsync(apiUrl + paramRequest);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<LibraryCategoryDTO>(responseStream);
            }
            return output;
        }


        public async Task<LibraryDTO> DetailLibrary(int? Library_ID)
        {
            LibraryDTO output = new LibraryDTO();
            string apiUrl = $"/api/v1/Library/GetLibraryDetail";
            string paramRequest = $"?LibraryId={Library_ID}";
            var response = await _client.GetAsync(apiUrl + paramRequest);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<LibraryDTO>(responseStream);
            }
            return output;
        }
        
        public async Task<List<LibraryDTO>> LstRelatelLibrary(int LibraryCategoryId, int? Library_ID)
        {
            List<LibraryDTO> output = new List<LibraryDTO>();
            string apiUrl = $"/api/v1/Library/GetRelateLibrary";
            string paramRequest = $"?LibraryCategoryId={LibraryCategoryId}&LibraryId={Library_ID}";
            var response = await _client.GetAsync(apiUrl + paramRequest);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<List<LibraryDTO>>(responseStream);
            }
            return output;
        }

        public async Task<PostLibraryDTO> CreateLibrary(PostLibraryDTO postLibraryDTO)
        {
            PostLibraryDTO output = new PostLibraryDTO();
            var jwt = JsonConvert.DeserializeObject<SumProfileResponseDTO>(_httpContextAccessor.HttpContext.Request.Cookies["JWT"]);
            if (jwt != null)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt.JWT);
                postLibraryDTO.UserId = jwt.UserId;
                string apiUrl = $"/api/v1/Library/PostLibrary";
                var json = JsonConvert.SerializeObject(postLibraryDTO, Formatting.Indented);
                var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _client.PostAsync(apiUrl, stringContent);
                if (response.IsSuccessStatusCode)
                {
                    string responseStream = await response.Content.ReadAsStringAsync();
                    output = JsonConvert.DeserializeObject<PostLibraryDTO>(responseStream);
                }
            }

            return output;
        }
    }
}
