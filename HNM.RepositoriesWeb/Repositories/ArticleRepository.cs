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
    public interface IArticleRepository
    {
        Task<ArticleDTO> GetHomeHighLightsArticle();
        Task<List<ArticleDTO>> GetHomeVerticleBlockOneArticle();
        Task<List<ArticleDTO>> GetHomeVerticleBlockTwoArticle();
        Task<ArticleDTO> DetailArticle(int? Article_ID, int ArticleTypeId);
        Task<List<ArticleCategoryDTO>> ArticleCategoryName(int? ArticleCategory_ID);
        Task<List<ArticleShortInfo>> ArticleSameCate(int? ArticleCategory_ID, int ArticleId, int ArticleTypeId);
        Task<List<ArticleCategoryDTO>> GetLstArticleCate();
        Task<List<ArticleShortInfo>> GetArtByCate(int categoryId, int typeId, int page, int pageSize);
        Task<ArticleCategoryDTO> ArticleCategoryByUrl(string url);
        Task<IEnumerable<Video_Search_Mobile_Result>> GetVideoByCate(int videoCategoryId, int typeId, int page, int pageSize);
        Task<ArticleSearchDTO> SearchArticle(string keyword, int page, int pageSize);
    }
    public class ArticleRepository :IRepositoryBase,IArticleRepository
    {
        private readonly HttpClient _client;
        private IMapper _mapper;
        public ArticleRepository(HttpClient client, IMapper mapper)
        {
            _client = client;
            _mapper = mapper;
        }

        //tin chính
        public async Task<ArticleDTO> GetHomeHighLightsArticle()
        {
            ArticleDTO output = new ArticleDTO();
            string apiUrl = $"/api/v1/Article/GetHomeHighLightsArticlePartialView";
            var response = await _client.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<ArticleDTO>(responseStream);
            }
            return output;
        }

        //khối 1
        public async Task<List<ArticleDTO>> GetHomeVerticleBlockOneArticle()
        {
            List<ArticleDTO> output = new List<ArticleDTO>();
            string apiUrl = $"/api/v1/Article/GetHomeVerticleBlockOneArticlePartialView";
            string paramRequest = $"?top=2";
            var response = await _client.GetAsync(apiUrl + paramRequest);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<List<ArticleDTO>>(responseStream);
            }
            return output;
        }

        //khối 2
        public async Task<List<ArticleDTO>> GetHomeVerticleBlockTwoArticle()
        {
            List<ArticleDTO> output = new List<ArticleDTO>();
            string apiUrl = $"/api/v1/Article/GetHomeVerticleBlockTwoArticlePartialView";
            string paramRequest = $"?top=4";
            var response = await _client.GetAsync(apiUrl + paramRequest);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<List<ArticleDTO>>(responseStream);
            }
            return output;
        }

        //Lấy ra ds tin theo Chuyên mục
        public async Task<List<ArticleShortInfo>> GetArtByCate(int categoryId, int typeId, int page, int pageSize)
        {
            List<ArticleShortInfo> output = new List<ArticleShortInfo>();
            string apiUrl = $"/api/v1/Article/GetArtByCate";
            string paramRequest = $"?ArticleCategoryId={categoryId}&ArticleTypeId={typeId}&page={page}&PageSize={pageSize}";
            var response = await _client.GetAsync(apiUrl + paramRequest);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<List<ArticleShortInfo>>(responseStream);
            }
            return output;
        }

        //Lấy ra ds tin theo Chuyên mục
        public async Task<IEnumerable<Video_Search_Mobile_Result>> GetVideoByCate(int videoCategoryId, int typeId, int page, int pageSize)
        {
            List<Video_Search_Mobile_Result> output = new List<Video_Search_Mobile_Result>();
            string apiUrl = $"/api/v1/Article/GetVideoByCate";
            string paramRequest = $"?VideoCategoryId={videoCategoryId}&ArticleTypeId={typeId}&page={page}&PageSize={pageSize}";
            var response = await _client.GetAsync(apiUrl + paramRequest);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<List<Video_Search_Mobile_Result>>(responseStream);
            }
            return output;
        }

        public async Task<ArticleDTO> DetailArticle(int? Article_ID, int ArticleTypeId)
        {
            ArticleDTO output = new ArticleDTO();
            string apiUrl = $"/api/v1/Article/GetArticleDetail";
            string paramRequest = $"?ArticleId={Article_ID}&ArticleTypeId={ArticleTypeId}";
            var response = await _client.GetAsync(apiUrl + paramRequest);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<ArticleDTO>(responseStream);
            }
            return output;
        }

        public async Task<List<ArticleCategoryDTO>> ArticleCategoryName(int? ArticleCategory_ID)
        {
            List<ArticleCategoryDTO> output = new List<ArticleCategoryDTO>();
            string apiUrl = $"/api/v1/ArticleCategory/GetListArticleByID";
            string paramRequest = $"?ArticleCategoryId={ArticleCategory_ID}";
            var response = await _client.GetAsync(apiUrl + paramRequest);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<List<ArticleCategoryDTO>>(responseStream);
            }
            return output;
        }

        public async Task<ArticleCategoryDTO> ArticleCategoryByUrl(string url)
        {
            ArticleCategoryDTO output = new ArticleCategoryDTO();
            string apiUrl = $"/api/v1/ArticleCategory/GetArticleCategoryByUrl";
            string paramRequest = $"?Url={url}";
            var response = await _client.GetAsync(apiUrl + paramRequest);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<ArticleCategoryDTO>(responseStream);
            }
            return output;
        }

        //Tin tức liên quan
        public async Task<List<ArticleShortInfo>> ArticleSameCate(int? ArticleCategory_ID, int ArticleId, int ArticleTypeId)
        {
            List<ArticleShortInfo> output = new List<ArticleShortInfo>();
            string apiUrl = $"/api/v1/Article/GetRelateArticle";
            string paramRequest = $"?ArticleCategoryId={ArticleCategory_ID}&ArticleId={ArticleId}&ArticleTypeId={ArticleTypeId}";
            var response = await _client.GetAsync(apiUrl + paramRequest);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<List<ArticleShortInfo>>(responseStream);
            }
            return output;
        }

        public async Task<List<ArticleCategoryDTO>> GetLstArticleCate()
        {
            List<ArticleCategoryDTO> output = new List<ArticleCategoryDTO>();
            string apiUrl = $"/api/v1/ArticleCategory/GetListArticle";
            var response = await _client.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<List<ArticleCategoryDTO>>(responseStream);
            }
            return output;
        }
        public async Task<ArticleSearchDTO> SearchArticle(string keyword, int page, int pageSize)
        {
            ArticleSearchDTO output = new ArticleSearchDTO();
            string apiUrl = $"/api/v1/Article/SearchArticle";
            string paramRequest = $"?keyword={keyword}&page={page}&PageSize={pageSize}";
            var response = await _client.GetAsync(apiUrl + paramRequest);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<ArticleSearchDTO>(responseStream);
            }
            return output;
        }
    }
}
