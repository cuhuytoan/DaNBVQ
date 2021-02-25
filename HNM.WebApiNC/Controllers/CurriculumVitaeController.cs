using AutoMapper;
using HNM.DataNC.Models;
using HNM.DataNC.ModelsFilter;
using HNM.DataNC.ModelsNC.ModelsDTO;
using HNM.DataNC.ModelsNC.ModelsUtility;
using HNM.RepositoriesNC.RepositoriesBase;
using HNM.WebApiNC.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HNM.WebApiNC.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class CurriculumVitaeController : ControllerBase, IDisposable
    {
        private readonly ILoggerManager _logger;
        private IRepositoryWrapper _repoWrapper;
        private IMapper _mapper;
        private readonly IDistributedCache _distributedCache;
        public IConfiguration _configuration;
        public CurriculumVitaeController(IRepositoryWrapper repoWrapper, ILoggerManager logger, IMapper mapper, IDistributedCache distributedCache, IConfiguration configuration)
        {
            _repoWrapper = repoWrapper;
            _logger = logger;
            _mapper = mapper;
            _distributedCache = distributedCache;
            _configuration = configuration;
        }
        /// <summary>
        /// Get Details CV 
        /// </summary>
        /// <param name="CVId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<CVDetailDTO> GetDetailsCV(int CVId)
        {
            var output = new CVDetailDTO();
            try
            {                
                var result = await _repoWrapper.CurriculumVitae.FirstOrDefaultAsync(p => p.Id == CVId);
                if (result == null)
                {
                    output.ErrorCode = "01";
                    output.Message = "Hồ sơ không tồn tại";
                    return output;
                }
                output.CVDetails = _mapper.Map<CurriculumVitaeDTO>(result);
                output.CVDetails.ProductCateRelateName = _repoWrapper.ProductCategory.FirstOrDefault(p => p.ProductCategory_ID == result.ProductCateRelate)?.Name;
                output.CVDetails.CareerCategoryName = _repoWrapper.CareerCategory.FirstOrDefault(p => p.Id == result.CareerCategoryId)?.Name;                
                var lstPictures = await _repoWrapper.CurriculumVitae.GetLstPicture(CVId);
                output.CVPictures = _mapper.Map<List<CVPictureDTO>>(lstPictures);                    
            }
            catch(Exception ex)
            {
                _logger.LogError($"GetDetailsCV:" + ex.ToString());
            }
            return output;
        }
        /// <summary>
        /// Get Top CV
        /// </summary>
        /// <param name="TopX"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<CurriculumVitaeDTO>> GetTopCV(int? TopX = 20)
        {
            var output = new List<CurriculumVitaeDTO>();
            try
            {
                var cacheKey = $"CurriculumVitae_GetTopCV{TopX}";
                var redisEncode = await _distributedCache.GetStringAsync(cacheKey);
                if (redisEncode != null)
                {
                    output = JsonConvert.DeserializeObject<List<CurriculumVitaeDTO>>(redisEncode);
                }
                else
                {
                    var result = await _repoWrapper.CurriculumVitae.GetTopCV(TopX);
                    output = _mapper.Map<List<CurriculumVitaeDTO>>(result);
                    await _distributedCache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(output), Utils.Util.RedisOptions());
                }
            }
            catch(Exception ex)
            {
                _logger.LogError($"GetTopCV: " + ex.ToString());
            }
            return output;
        }
        /// <summary>
        /// Get List CV Paggin
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<CVSearchDTO>> GetListCVPagging([FromQuery] CVFilter filter)
        {
            var output = new List<CVSearchDTO>();
            try
            {
                var result = await _repoWrapper.CurriculumVitae.GetListCV(filter);
                if(result !=null)
                {
                    output = _mapper.Map<List<CVSearchDTO>>(result);
                }    
                
            }
            catch(Exception ex)
            {
                _logger.LogError($"GetListCVPagging:" + ex.ToString());
            }
            return output;
        }
        
        /// <summary>
        /// Getrelate CurricolumVitae
        /// </summary>
        /// <param name="CareerCategoryId"></param>
        /// <param name="CurriculumVitaeId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<CurriculumVitaeDTO>> GetRelateCurriculumVitae(int? CareerCategoryId, int CurriculumVitaeId)
        {
            var output = new List<CurriculumVitaeDTO>();
            try
            {
                var result = await _repoWrapper.CurriculumVitae.GetRelateCurriculumVitae(CareerCategoryId,CurriculumVitaeId);
                output = _mapper.Map<List<CurriculumVitaeDTO>>(result);                
            }
            catch(Exception ex)
            {
                _logger.LogError($"GetRelateCurriculumVitae:" + ex.ToString());
            }
            return output;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<ReturnPostCVDTO> PostCV([FromBody] SumPostCurriculumnViateDTO model)
        {
            _logger.LogDebug($"PostCV: model : " + JsonConvert.SerializeObject(model));
            var output = new ReturnPostCVDTO();
            if (model == null)
            {
                output.ErrorCode = "004";
                output.Message = Utils.ConstMessage.GetMsgConst("004");
                return output;
            }
            else
            {
                try
                {
                    var CvId = await _repoWrapper.CurriculumVitae.PostCV(_mapper.Map<CurriculumVitae>(model.CV), model.UserId);
                    if (CvId != 0)
                    {
                        var modelImgRabbit = new ImageUploadAWSDTO();
                        modelImgRabbit.MainImage = model.MainImage;
                        modelImgRabbit.SubImage = model.SubImage;
                        modelImgRabbit.ParameterId = CvId;
                        modelImgRabbit.Type = "CV";
                        //Push To rabbit
                        Task.Run(() => _repoWrapper.Product.PushAWSImageToRabbit(modelImgRabbit, _configuration["AWS_ImageRabbit"]));

                        output.Id = CvId;
                        output.ErrorCode = "00";
                        output.Message = "Thêm mới thành công";

                        //Task.Run(() => _repoWrapper.FCMMessage.PushNotificationToRabitMQ(new NotificationRabitMQModel
                        //{
                        //    Type = "ONDEMAND",
                        //    NotificationCode = "DTCD",
                        //    ChannelSend = "ALL",
                        //    UsingTemplate = true,
                        //    UserId = model.UserId,
                        //    ObjectId = CvId,
                        //    ObjectType = "PRODUCT_DEMAND"
                        //}));
                    }
                    else
                    {
                        output.Message = "001";
                        output.Message = Utils.ConstMessage.GetMsgConst("001");
                    }
                    return output;
                }
                catch (Exception ex)
                {
                    _logger.LogError($"PostCV:" + ex.ToString());
                    output.Message = "001";
                    output.Message = Utils.ConstMessage.GetMsgConst("001");
                    return output;
                }
            }
            
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="model"></param>
        /// <param name="IsRepair"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut]
        public async Task<ReturnPostCVDTO> PutCV(int Id, SumPostCurriculumnViateDTO model, int IsRepair)
        {
            _logger.LogDebug($"PutCV: model : " + JsonConvert.SerializeObject(model) + " ID: " + Id);
            var output = new ReturnPostCVDTO();
            var checkCanEdit = _repoWrapper.CurriculumVitae.CanEditCV(Id, model.UserId);
            if (!checkCanEdit)
            {
                output.ErrorCode = "PROD001";
                output.Message = Utils.ConstMessage.GetMsgConst("PROD001");
                return output;
            }
            else
            {
                try
                {
                    model.CV.Id = Id;
                    var CvId = await _repoWrapper.CurriculumVitae.PostCV(_mapper.Map<CurriculumVitae>(model.CV), model.UserId);
                    if (CvId != 0)
                    {
                        if(IsRepair == 1) // Tiếp tục chỉnh sửa delete subImage
                        {
                            var subImage = await _repoWrapper.CurriculumVitae.GetListDeleteCVPicture(Id);
                            await _repoWrapper.CurriculumVitae.DeleteCVIllustrationImages(subImage);
                        }
                        //Save MainImage
                        if (model.MainImage != null)
                        {
                            var modelImgRabbit = new ImageUploadAWSDTO();
                            modelImgRabbit.MainImage = model.MainImage;
                            modelImgRabbit.ParameterId = CvId;
                            modelImgRabbit.Type = "CV";
                            //Push To rabbit
                            Task.Run(() => _repoWrapper.Product.PushAWSImageToRabbit(modelImgRabbit, _configuration["AWS_ImageRabbit"]));
                        }    
                        //Save SubImage
                        if(model.SubImage !=null)
                        {
                            //Delete Image
                            if (model.DeleteCVPicture != null)
                            {
                                await _repoWrapper.CurriculumVitae.DeleteCVIllustrationImages(model.DeleteCVPicture);
                            }


                            var modelImgRabbit = new ImageUploadAWSDTO();
                            //modelImgRabbit.MainImage = model.MainImage;
                            modelImgRabbit.SubImage = model.SubImage;
                            modelImgRabbit.ParameterId = CvId;
                            modelImgRabbit.Type = "CV";
                            //Push To rabbit
                            Task.Run(() => _repoWrapper.Product.PushAWSImageToRabbit(modelImgRabbit, _configuration["AWS_ImageRabbit"]));
                        }    

                        output.Id = CvId;
                        output.ErrorCode = "00";
                        output.Message = "Cập nhật thành công";

                        //Task.Run(() => _repoWrapper.FCMMessage.PushNotificationToRabitMQ(new NotificationRabitMQModel
                        //{
                        //    Type = "ONDEMAND",
                        //    NotificationCode = "DTCD",
                        //    ChannelSend = "ALL",
                        //    UsingTemplate = true,
                        //    UserId = model.UserId,
                        //    ObjectId = CvId,
                        //    ObjectType = "PRODUCT_DEMAND"
                        //}));
                    }
                    else
                    {
                        output.Message = "001";
                        output.Message = Utils.ConstMessage.GetMsgConst("001");
                    }
                    return output;
                }
                catch(Exception ex)
                {
                    _logger.LogError($"PutCV:" + ex.ToString());
                    output.Message = "001";
                    output.Message = Utils.ConstMessage.GetMsgConst("001");
                    return output;
                }

            }
        }

        /// <summary>
        /// Cập nhật trạng thái hủy đăng H : Hủy đăng K:Khôi phục
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Action"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<ModelBaseStatus> ActionInCV(int Id, string Action)
        {
            _logger.LogDebug($"ActionInCV: id : {Id}" +" Action: " + Action);
            var output = new ModelBaseStatus();
            var checkCV = await _repoWrapper.CurriculumVitae.FirstOrDefaultAsync(p => p.Id == Id);
            if(checkCV !=null)
            {
                if(Action == "D") // Hủy đăng 
                {
                    await _repoWrapper.CurriculumVitae.ActionInCV(Id, Action);
                    output.ErrorCode = "00";
                    output.Message = "Cập nhật thành công.";
                    
                }    
            }
            return output;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}