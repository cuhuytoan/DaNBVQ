using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HNM.DataNC.ModelsNC.ModelsDTO;
using HNM.DataNC.ModelsNC.ModelsUtility;
using HNM.RepositoriesWeb.RepositoriesBase;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace HNM.RepositoriesWeb.Repositories
{
    public interface IAccountRepository
    {
        Task<SumProfileResponseDTO> Login(LoginDto model);
        Task<RegisterDto> Register(RegisterDto model);
        Task<ConfirmVerify> VerifyCode(string EmailOrPhone, string code);
        Task<ModelBase> ResendCode(ForgotPassworDTO model);
        Task<SetPassDTO> SetPassword(SetPassDTO model);
        Task<ForgotPassworDTO> ForgotPassword(ForgotPassworDTO model);
    }
    public class AccountRepository : IRepositoryBase, IAccountRepository
    {
        private readonly HttpClient _client;
        private IMapper _mapper;
        public AccountRepository(HttpClient client, IMapper mapper)
        {
            _client = client;
            _mapper = mapper;
        }

        public async Task<ForgotPassworDTO> ForgotPassword(ForgotPassworDTO model)
        {
            var output = new ForgotPassworDTO();
            string apiUrl = "/api/v1/Account/ForgotPassword";
            var json = JsonConvert.SerializeObject(model, Formatting.Indented);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(apiUrl, stringContent);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<ForgotPassworDTO>(responseStream);
            }
            return output;
        }

        public async Task<SumProfileResponseDTO> Login(LoginDto model)
        {
            var output = new SumProfileResponseDTO();
            string apiUrl = "/api/v1/Account/Login";
            var json = JsonConvert.SerializeObject(model, Formatting.Indented);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(apiUrl, stringContent);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<SumProfileResponseDTO>(responseStream);
            }
            return output;
        }

        public async Task<RegisterDto> Register(RegisterDto model)
        {
            var output = new RegisterDto();
            string apiUrl = "/api/v1/Account/Register";
            model.Password = "123456"; //todo
            var json = JsonConvert.SerializeObject(model, Formatting.Indented);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(apiUrl, stringContent);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<RegisterDto>(responseStream);
            }
            return output;
        }

        public async Task<ModelBase> ResendCode(ForgotPassworDTO model)
        {
            var output = new ModelBase();
            string apiUrl = "/api/v1/Account/ResendCode";            
            var json = JsonConvert.SerializeObject(model, Formatting.Indented);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(apiUrl, stringContent);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<ModelBase>(responseStream);
            }
            return output;
        }

        public async Task<SetPassDTO> SetPassword(SetPassDTO model)
        {
            var output = new SetPassDTO();
            string apiUrl = "/api/v1/Account/SetPassword";
            var json = JsonConvert.SerializeObject(model, Formatting.Indented);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(apiUrl, stringContent);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<SetPassDTO>(responseStream);
            }
            return output;
        }

        public async Task<ConfirmVerify> VerifyCode(string EmailOrPhone, string code)
        {
            var output = new ConfirmVerify();
            string apiUrl = $"/api/v1/Account/VerifyCode";
            string paramRequest = $"?EmailOrPhone={EmailOrPhone}&code={code}";
            var response = await _client.GetAsync(apiUrl + paramRequest);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<ConfirmVerify>(responseStream);
            }
            return output; 
        }
    }
}
