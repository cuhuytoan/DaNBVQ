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
    public interface ISettingRepository
    {
        Task<Setting> GetSetting();
    }
    public class SettingRepository : IRepositoryBase, ISettingRepository
    {
        private readonly HttpClient _client;
        private IMapper _mapper;
        public SettingRepository(HttpClient client, IMapper mapper)
        {
            _client = client;
            _mapper = mapper;
        }
        public async Task<Setting> GetSetting()
        {
            Setting output = new Setting();
            string apiUrl = $"/api/v1/Setting/GetAllSetting";
            var response = await _client.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<Setting>(responseStream);
                output.MetaDescriptionDefault = HttpUtility.HtmlDecode(output.MetaDescriptionDefault);
                output.MetaKeywordsDefault = HttpUtility.HtmlDecode(output.MetaKeywordsDefault);
                output.MetaTitleDefault = HttpUtility.HtmlDecode(output.MetaTitleDefault);
            }
            return output;
        }
    }
}
