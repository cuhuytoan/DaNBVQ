using AutoMapper;
using HNM.DataNC.ModelsNC.ModelsDTO;
using HNM.RepositoriesWeb.RepositoriesBase;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HNM.RepositoriesWeb.Repositories
{
    public interface IVideoRepository
    {
        Task<List<VideoShortInfo>> GetListVideo(int page, int pageSize);
        Task<List<LibraryCategoryDTO>> GetLstVideoCate();
        Task<List<VideoShortInfo>> GetRelateVideo(int VideoCategoryId, int ArticleId, int ArticleTypeId, int page, int pageSize);
    }

    public class VideoRepository : IRepositoryBase, IVideoRepository
    {
        private readonly HttpClient _client;
        private IMapper _mapper;

        public VideoRepository(HttpClient client, IMapper mapper)
        {
            _client = client;
            _mapper = mapper;
        }

        public async Task<List<VideoShortInfo>> GetListVideo(int page, int pageSize)
        {
            List<VideoShortInfo> output = new List<VideoShortInfo>();
            string apiUrl = $"/api/v1/Article/GetListVideo";
            string paramRequest = $"?page={page}&PageSize={pageSize}";
            var response = await _client.GetAsync(apiUrl + paramRequest);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<List<VideoShortInfo>>(responseStream);
            }
            return output;
        }


        public async Task<List<VideoShortInfo>> GetRelateVideo(int VideoCategoryId, int ArticleId, int ArticleTypeId, int page, int pageSize)
        {
            List<VideoShortInfo> output = new List<VideoShortInfo>();
            string apiUrl = $"/api/v1/Article/GetRelateVideo";
            string paramRequest = $"?VideoCategoryId={VideoCategoryId}&ArticleId={ArticleId}&ArticleTypeId={ArticleTypeId}&page={page}&pageSize={pageSize}";
            var response = await _client.GetAsync(apiUrl + paramRequest);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<List<VideoShortInfo>>(responseStream);
            }
            return output;
        }

        //đang làm
        public async Task<List<LibraryCategoryDTO>> GetLstVideoCate()
        {
            List<LibraryCategoryDTO> output = new List<LibraryCategoryDTO>();
            string apiUrl = $"/api/v1/LibraryCategory/GetListLibraryCategory";
            var response = await _client.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<List<LibraryCategoryDTO>>(responseStream);
            }
            return output;
        }
    }
}
