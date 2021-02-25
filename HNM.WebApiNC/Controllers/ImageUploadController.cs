using AutoMapper;
using HNM.Common;
using HNM.DataNC.Models;
using HNM.DataNC.ModelsIdentity;
using HNM.DataNC.ModelsNC.ModelsDTO;
using HNM.RepositoriesNC.RepositoriesBase;
using HNM.WebApiNC.Logging;
using HNM.WebApiNC.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HNM.WebApiNC.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    //[Authorize]
    [ApiController]
    public class ImageUploadController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private IMapper _mapper;
        public static IWebHostEnvironment _environment;

        public ImageUploadController(UserManager<ApplicationUser> userManager,
            IRepositoryWrapper repositoryWrapper,
            ILoggerManager logger,
            IMapper mapper,
            IWebHostEnvironment environment)
        {
            _userManager = userManager;
            _repositoryWrapper = repositoryWrapper;
            _logger = logger;
            _mapper = mapper;
            _environment = environment;
        }
        public class FileUploadApi
        {
            public IFormFile files { get; set; }
        }
        /// <summary>
        /// {
        /// "userId": "25d43545-a81a-4434-88d1-6bbfc33071b1",
        /// "FileName" : "toantest1", 
        /// "ExtensionType": "image/jpeg",
        /// "Base64": "gen ảnh base64"
        ///}
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<UpdateAvatarDTO> UpdateAvatar(ImageUploadAvatarDTO model)
        {
            model.ExtensionType = "image/jpeg";
            var response = new UpdateAvatarDTO();
            var profile = new AspNetUserProfiles();
            profile = _repositoryWrapper.AspNetUserProfiles.FirstOrDefault(p => p.UserId == model.UserId);
            if (profile == null)
            {
                _logger.LogError($"[ManageController] {ConstMessage.GetMsgConst("ACC008")}");
                response.ErrorCode = "ACC008";
                response.Message = ConstMessage.GetMsgConst("ACC008");
                return response;
            }
            try
            {
                if (model.Base64.Length > 0)
                {

                    using (var client = new HttpClient())
                    {
                        //client.BaseAddress = new Uri("https://cdn.hanoma.vn/api/UploadFile/UploadSingleImage");
                        //client.DefaultRequestHeaders.Accept.Clear();
                        //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        model.PathSave = "user/avatar/original";
                        var stringContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
                        //var buffer = System.Text.Encoding.UTF8.GetBytes(content);
                        //var byteContent = new ByteArrayContent(buffer);
                        // HTTP POST
                        //HttpResponseMessage responseSent = await client.PostAsync("https://cdn.hanoma.vn/api/UploadFile/UploadSingleImage", stringContent);
                        //File Extension Type

                        model.FileName = profile.UserId + "-" + DateTime.Now.ToString("dd-MM-yyyy") + "-" + DateTime.Now.ToString("HH-mm-ss") + "." + model.ExtensionType.Replace("image/", "");
                        var responseUpload = await UploadSingleImage(model);
                        if (responseUpload)
                        {
                            response.UserId = model.UserId;
                            response.AvatarUrl = model.FileName;

                            _mapper.Map(response, profile);
                            _repositoryWrapper.AspNetUserProfiles.UpdateProfilers(profile);
                            _repositoryWrapper.Save();
                            response.ErrorCode = "00";
                            response.Message = "Upload thành công";
                            return response;
                        }
                        else
                        {
                            response.ErrorCode = "002";
                            response.Message = ConstMessage.GetMsgConst("002");
                            return response;
                        }

                    }

                }
                else
                {
                    response.ErrorCode = "ACC014";
                    response.Message = ConstMessage.GetMsgConst("ACC014");
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.ErrorCode = "002";
                response.Message = ConstMessage.GetMsgConst("002") + " " + ex.Message.ToString();
                return response;
            }

        }
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<bool> UploadSingleImage(ImageUploadAvatarDTO model)
        {
            try
            {
                var imageDataByteArray = Convert.FromBase64String(model.Base64);
                var imageDataStream = new MemoryStream(imageDataByteArray);
                imageDataStream.Position = 0;
                var file = File(imageDataByteArray, model.ExtensionType);
                var fileName = model.FileName;
                if (file.FileContents.Length > 0)
                {
                    await CommonUtil.UploadS3(model.FileName, model.PathSave, imageDataStream, model.ExtensionType);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"UploadSingleImage: " + ex.ToString());
                return false;
            }
        }     

    }
}