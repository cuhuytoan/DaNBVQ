using AutoMapper;
using HNM.Common;
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
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace HNM.WebApiNC.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class RecruitmentController : ControllerBase, IDisposable
    {
        private readonly ILoggerManager _logger;
        private IRepositoryWrapper _repoWrapper;
        private IMapper _mapper;
        private readonly IDistributedCache _distributedCache;
        public IConfiguration _configuration;
        public RecruitmentController(IRepositoryWrapper repoWrapper, ILoggerManager logger, IMapper mapper, IDistributedCache distributedCache, IConfiguration configuration)
        {
            _repoWrapper = repoWrapper;
            _logger = logger;
            _mapper = mapper;
            _distributedCache = distributedCache;
            _configuration = configuration;
        }
        // type 1 - cần mua máy, type 2 - cần thuê máy
        [HttpGet]
        public async Task<IEnumerable<RecruitmentDTO>> GetTopRecruitment(int topX = 5)
        {
            var output = new List<RecruitmentDTO>();
            try
            {
                var cacheKey = $"Recruitment_GetTopRecruitment{topX}";
                var redisEncode = await _distributedCache.GetStringAsync(cacheKey);
                if (redisEncode != null)
                {
                    output = JsonConvert.DeserializeObject<List<RecruitmentDTO>>(redisEncode);
                }
                else
                {
                    var result = await _repoWrapper.Recruitment.GetTopRecruitment(topX);
                    output = _mapper.Map<List<RecruitmentDTO>>(result);
                    output.ForEach(x => x.Content = HttpUtility.HtmlDecode(CommonUtil.StripHTML(HttpUtility.HtmlDecode(x.Content))));
                    await _distributedCache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(output), Utils.Util.RedisOptions());
                }
            }
            catch (Exception ex)
            {

            }
            return output;
        }
        [HttpGet]
        public async Task<RecruitmentPaggingDTO> GetListRecruitment([FromQuery] RecruitmentFilter fitler)
        {

            var result = await _repoWrapper.Recruitment.GetListRecruitmentPagging(fitler);
            var firstItem = result.FirstOrDefault();
            int totalRow = (int)(firstItem?.TotalRows ?? 0);
            var output = new RecruitmentPaggingDTO();
            output.PageSize = fitler.PageSize;
            output.CurrentPage = fitler.Page ?? 1;
            output.TotalRecord = totalRow;
            output.TotalPage = (totalRow - 1) / fitler.PageSize + 1;
            output.Data = _mapper.Map<IEnumerable<RecruitmentSearchResultDTO>>(result);
            return output;
        }
        [HttpPost]
        public async Task<SentContactRecruitment> SentContactRecruitment([FromBody] ContactRecruitment model)
        {
            var MailRec = await _repoWrapper.Recruitment.SentContactRecruitment(model.RecruitmentId, model.Name, model.Phone, model.Email, model.Content);
            //Sent Email
            Utils.Util.SendMail(model.Name, MailRec.ContactEmail, MailRec.ContactName,
                        "Thông tin từ Hanoma", MailRec.MailBody, _repoWrapper.AspNetUsers.setting());
            var result = new SentContactRecruitment();
            result.ErrorCode = "00";
            result.Message = "Gửi email thành công!";
            return result;

        }
        [HttpGet]
        public async Task<RecDetailDTO> GetRecruitmentDetail(int Recruitment_ID)
        {
            var output = new RecDetailDTO();
            try
            {
                var result = await _repoWrapper.Recruitment.GetRecruitmentDetails(Recruitment_ID);
                output.RecDetail = _mapper.Map<RecruitmentDTO>(result);
                var recName = await _repoWrapper.CareerCategory.FirstOrDefaultAsync(p => p.Id == output.RecDetail.RecruitmentCategoryId);
                if(recName != null)
                {
                    output.RecDetail.RecruitmentCategoryName = recName.Name;
                }
                var childCateName = await _repoWrapper.ProductCategory.FirstOrDefaultAsync(p => p.ProductCategory_ID == output.RecDetail.ProductCateChildRelate);
                if(childCateName != null)
                {
                    output.RecDetail.ProductCategoryChildRelateName = childCateName.Name;
                }    
                var lstPictures = await _repoWrapper.Recruitment.GetLstPicture(Recruitment_ID);
                output.RecPictures = _mapper.Map<List<RecPictureDTO>>(lstPictures);

            }
            catch(Exception ex)
            {
                _logger.LogError($"GetDetailsCV:" + ex.ToString());
            }
            
            //Format Price

            return output;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<ReturnPostRecDTO> PostRec([FromBody] SumPostRecDTO model)
        {
            _logger.LogDebug($"PostRec: model : " + JsonConvert.SerializeObject(model));
            var output = new ReturnPostRecDTO();
            var pUserId = Guid.Parse(model.UserId);
            var prodBrand = _repoWrapper.Brand.FirstOrDefault(p => p.CreateBy == pUserId);
            if(prodBrand == null)
            {
                output.ErrorCode = "01";
                output.Message = "Không tồn tại nhà cung cấp";
            }    
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
                    var recId = await _repoWrapper.Recruitment.PostRecruitment(_mapper.Map<Recruitment>(model.Rec), model.UserId);
                    if(recId != 0)
                    {
                        var modelImgRabbit = new ImageUploadAWSDTO();
                        modelImgRabbit.MainImage = model.MainImage;
                        modelImgRabbit.SubImage = model.SubImage;
                        modelImgRabbit.ParameterId = recId;
                        modelImgRabbit.Type = "Recruitment";
                        //Push To rabbit
                        Task.Run(() => _repoWrapper.Product.PushAWSImageToRabbit(modelImgRabbit, _configuration["AWS_ImageRabbit"]));

                        output.Id = recId;
                        output.ErrorCode = "00";
                        output.Message = "Thêm mới thành công";
                    }
                    else
                    {
                        output.ErrorCode = "01";
                        output.Message = " Có lỗi trong quá trình thêm mới";
                    }
                }
                catch(Exception ex)
                {

                }
            }
            return output;
        }

        [Authorize]
        [HttpPut]
        public async Task<ReturnPostRecDTO> PutRec(int Id, SumPostRecDTO model, int IsRepair)
        {
            _logger.LogDebug($"PutRec: model : " + JsonConvert.SerializeObject(model) + " ID: " + Id);
            var output = new ReturnPostRecDTO();
            var checkCanEdit = _repoWrapper.Recruitment.CanEditRec(Id, model.UserId);
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
                    var recId = await _repoWrapper.Recruitment.PostRecruitment(_mapper.Map<Recruitment>(model.Rec), model.UserId);
                    if(recId != 0)
                    {
                        if(IsRepair == 1)
                        {
                            var subImage = await _repoWrapper.Recruitment.GetListDeleteRecPicture(Id);
                            await _repoWrapper.Recruitment.DeleteRecIllustrationImages(subImage);
                        }
                        //Save MainImage
                        if(model.MainImage !=null)
                        {
                            var modelImgRabbit = new ImageUploadAWSDTO();
                            modelImgRabbit.MainImage = model.MainImage;
                            modelImgRabbit.ParameterId = recId;
                            modelImgRabbit.Type = "Recruitment";
                            //Push To rabbit
                            Task.Run(() => _repoWrapper.Product.PushAWSImageToRabbit(modelImgRabbit, _configuration["AWS_ImageRabbit"]));
                        }    
                        //Save subImage
                        if(model.SubImage !=null)
                        {
                            if(model.DeleteRecPicture !=null)
                            {
                                await _repoWrapper.Recruitment.DeleteRecIllustrationImages(model.DeleteRecPicture);
                            }

                            var modelImgRabbit = new ImageUploadAWSDTO();
                            //modelImgRabbit.MainImage = model.MainImage;
                            modelImgRabbit.SubImage = model.SubImage;
                            modelImgRabbit.ParameterId = recId;
                            modelImgRabbit.Type = "Recruitment";
                            //Push To rabbit
                            Task.Run(() => _repoWrapper.Product.PushAWSImageToRabbit(modelImgRabbit, _configuration["AWS_ImageRabbit"]));
                        }    

                        output.Id = recId;
                        output.ErrorCode = "00";
                        output.Message = "Cập nhật thành công";
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
                    _logger.LogError($"PutRec:" + ex.ToString());
                    output.Message = "001";
                    output.Message = Utils.ConstMessage.GetMsgConst("001");
                    return output;
                }
            }

        }

        [Authorize]
        [HttpGet]
        public async Task<ModelBaseStatus> ActionInRec(int Id, string Action)
        {
            _logger.LogDebug($"ActionInRec: id : {Id}" + " Action: " + Action);
            var output = new ModelBaseStatus();
            var checkCV = await _repoWrapper.Recruitment.FirstOrDefaultAsync(p => p.Recruitment_ID == Id);
            if (checkCV != null)
            {
                if (Action == "D") // Xóa
                {
                    await _repoWrapper.Recruitment.ActionInRec(Id, Action);
                    output.ErrorCode = "00";
                    output.Message = "Cập nhật thành công.";

                }
            }
            return output;
        }
        /// <summary>
        /// Home search Recruitment
        /// </summary>
        /// <param name="fitler"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<RecruitmentPaggingDTO> SearchRecruitment([FromQuery] BaseFilter fitler)
        {
            var result = await _repoWrapper.Recruitment.SearchRecruitment(fitler);
            var firstItem = result.FirstOrDefault();
            int totalRow = (int)(firstItem?.TotalRows ?? 0);
            var output = new RecruitmentPaggingDTO();
            output.PageSize = fitler.PageSize;
            output.CurrentPage = fitler.Page ?? 1;
            output.TotalRecord = totalRow;
            output.TotalPage = (totalRow - 1) / fitler.PageSize + 1;
            output.Data = _mapper.Map<IEnumerable<RecruitmentSearchResultDTO>>(result);
            return output;
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}