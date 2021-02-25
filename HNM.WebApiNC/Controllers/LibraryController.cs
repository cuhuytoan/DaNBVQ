using AutoMapper;
using HNM.DataNC.Models;
using HNM.DataNC.ModelsFilter;
using HNM.DataNC.ModelsNC.ModelsDTO;
using HNM.DataNC.ModelsStore;
using HNM.RepositoriesNC.RepositoriesBase;
using HNM.WebApiNC.Logging;
using HNM.WebApiNC.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web;

namespace HNM.WebApiNC.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class LibraryController : ControllerBase, IDisposable
    {
        private readonly ILoggerManager _logger;
        private IRepositoryWrapper _repoWrapper;
        private IMapper _mapper;
        private readonly IDistributedCache _distributedCache;
        public LibraryController(IRepositoryWrapper repoWrapper, ILoggerManager logger, IMapper mapper, IDistributedCache distributedCache)
        {
            _repoWrapper = repoWrapper;
            _logger = logger;
            _mapper = mapper;
            _distributedCache = distributedCache;
        }
        [HttpGet]
        public async Task<IEnumerable<LibraryDTO>> GetRelateLibrary(int LibraryCategoryId, int LibraryId)
        {
            var output = new List<LibraryDTO>();
            try
            {
                var cacheKey = "Library_GetRelateLibrary";
                var redisEncode = await _distributedCache.GetStringAsync(cacheKey);
                if (redisEncode != null)
                {
                    output = JsonConvert.DeserializeObject<List<LibraryDTO>>(redisEncode);
                }
                else
                {
                    var result = await _repoWrapper.Library.GetRelateLibrary(LibraryCategoryId, LibraryId);
                    output = _mapper.Map<List<LibraryDTO>>(result);
                    await _distributedCache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(output), Utils.Util.RedisOptions());
                }
            }
            catch(Exception ex)
            {
                _logger.LogError($"GetRelateLibrary: " + ex.ToString());
            }
            return output;
        }
        [HttpGet]
        public async Task<LibraryDTO> GetLibraryDetail(int LibraryId)
        {
            var output = new LibraryDTO();
            var result = _repoWrapper.Library.GetLibDetail(LibraryId);
            if(result !=null)
            {
                output = _mapper.Map<LibraryDTO>(result);
                output.Content = HttpUtility.HtmlDecode(output.Content);
            }    
            return output;
        }
        /// <summary>
        /// Get Lst Library ( sử dụng cho get theo category và lọc)
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<Library_Search_By_Cate_Result>> GetListLibraryPagging([FromQuery]LibraryFilter filter)
        {
            return await _repoWrapper.Library.GetListLibraryPagging(filter);
        }

        /// <summary>
        /// Post library
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PostLibraryDTO> PostLibrary(PostLibraryDTO model)
        {
            _logger.LogDebug($"PostLibrary: {JsonConvert.SerializeObject(model)}");
            var output = new PostLibraryDTO();
            try
            {
                var LibraryModel = _mapper.Map<Library>(model.Data);
                var LibraryId = await _repoWrapper.Library.PostLibrary(LibraryModel, model.ImgLogo, model.UserId);
                if (LibraryId != 0)
                {
                    // Save MainImage
                    if (!String.IsNullOrEmpty(model.ImgLogo.Base64))
                    {
                        await SaveLogoImage(model.ImgLogo, LibraryId);
                    }
                    
                    output.Data.Library_ID = LibraryId;

                    ////Update Image
                    //_repoWrapper.Brand.UpdateImgProductBrand(ProductBrandModel, model.ImgLogo, model.ImgBanner, ProdBrandId, model.UserId);
                    await _repoWrapper.FCMMessage.PushNotificationToRabitMQ(new NotificationRabitMQModel
                    {
                        Type = "ONDEMAND",
                        NotificationCode = "DKCH",
                        ChannelSend = "ALL",
                        UsingTemplate = true,
                        UserId = model.UserId,
                    });
                    output.ErrorCode = "00";
                    output.Message = "Tạo kiến thức thành công";
                }
                else
                {
                    output.ErrorCode = "001";
                    output.Message = Utils.ConstMessage.GetMsgConst("001");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"PostLibrary: " + ex.ToString());
                output.ErrorCode = "001";
                output.Message = Utils.ConstMessage.GetMsgConst("001");
            }
            return output;
        }

        private async Task SaveLogoImage(ImageUploadDTO MainImage, int libraryId)
        {
            var timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            MainImage.FileName = String.Format("{0}-00-{1}.{2}", libraryId, timestamp, MainImage.ExtensionType.Replace("image/", ""));
            MainImage.PathSave = "Library/mainimages/original";
            await UploadImage(MainImage, MainImage.PathSave, "Library/mainimages/thumb");
            await _repoWrapper.Library.UpdateImgLogoLibrary(MainImage.FileName, libraryId);
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        private async Task<bool> UploadImage(ImageUploadDTO model, string pathMain, string pathThumb)
        {
            try
            {
                var imageDataByteArray = Convert.FromBase64String(model.Base64);

                var imageDataStream = new MemoryStream(imageDataByteArray);
                imageDataStream.Position = 0;

                var file = File(imageDataByteArray, model.ExtensionType);
                var fileName = model.FileName;

                //Image 
                var inputStreamEnd = imageDataStream;

                //Image Thumb
                var inputStreamThumb = Util.ResizeImageStream(inputStreamEnd, 120, 120);

                if (file.FileContents.Length > 0)
                {
                    await Util.UploadS3(model.FileName, pathMain, inputStreamEnd, model.ExtensionType);
                    await Util.UploadS3(model.FileName, pathThumb, inputStreamThumb, file.ContentType);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"UploadImage: " + ex.ToString());
                return false;
            }
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}