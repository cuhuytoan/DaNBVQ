using AutoMapper;
using HNM.DataNC.Models;
using HNM.DataNC.ModelsNC.ModelsDTO;
using HNM.DataNC.ModelsNC.ModelsUtility;
using HNM.RepositoriesNC.RepositoriesBase;
using HNM.WebApiNC.Logging;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace HNM.WebApiNC.Controllers
{

    [Route("api/v1/[controller]/[action]")]

    [ApiController]
    public class FCMMessageController : ControllerBase, IDisposable
    {
        private readonly ILoggerManager _logger;
        private IRepositoryWrapper _repoWrapper;
        private IMapper _mapper;
        public FCMMessageController(IRepositoryWrapper repoWrapper, ILoggerManager logger, IMapper mapper)
        {
            _repoWrapper = repoWrapper;
            _logger = logger;
            _mapper = mapper;
        }
        /// <summary>
        /// Get List mesage by User
        /// </summary>
        /// <param name="UserID">Parameter User ID</param>
        /// <param name="Top"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<LstFCMMessageUser> GetListMsgByUser(string UserID, int Top, int? NotiSpecType = 0)
        {
            var output = new LstFCMMessageUser();
            try
            {
                var lstFCMMessage = await _repoWrapper.FCMMessage.GetListMsgByUser(UserID, Top, NotiSpecType);
                var numberUnread = await _repoWrapper.FCMMessage.GetNumberFCMUnread(UserID);
                List<LstFCMMessage> lstItem = new List<LstFCMMessage>();
                if(lstFCMMessage.Count() > 0)
                {
                    foreach (var p in lstFCMMessage)
                    {
                        var item = new LstFCMMessage();
                        item.FCMMessageId = p.FCMMessage_ID;
                        item.Title = p.Title ?? "";
                        item.Body = HttpUtility.HtmlDecode(p.Body) ?? "";
                        item.notifyType = p.NotificationType ?? 0;
                        item.formId = p.Form_ID ?? "";
                        item.id = p.ParameterId ?? 0;
                        item.categoryId = p.CategoryId ?? 0;
                        item.fullUrl = p.FullUrl ?? "";
                        item.fullUrlImage = p.FullUrlImage ??"";
                        item.typeId = p.ProductTypeId ?? 0;
                        item.notiSpecType = p.NotiSpecType ?? 0;
                        item.isPinTop = p.IsPinTop ?? 0;
                        item.formAppName = p.FormNameApp ?? "";
                        item.CreateDate = p.CreateDate;
                        item.HasRead = p.HasRead ?? 0;
                        lstItem.Add(item);
                    }
                    
                }
                output.LstFCMMessage = lstItem;
                output.NumberUnread = numberUnread;
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetListMsgByUser: " + ex.ToString());
            }
            return output;
        }
        /// <summary>
        /// Update HasRead FCM if FCMMessageID = 0 SET all hasread for tab notispectype ..else set hasread for FCMMessageID
        /// 
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="FCMMessageID"></param>
        /// <param name="NotiSpecType"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<FCMHasReadDTO> FCMHasRead(string UserId, int HasRead, int? FCMMessageID = 0, int? NotiSpecType = 0)
        {
            var output = new FCMHasReadDTO();
            try
            {
                var result = await _repoWrapper.FCMMessage.UpdateHasRead(UserId, HasRead, FCMMessageID, NotiSpecType);
                var numberUnread = await _repoWrapper.FCMMessage.GetNumberFCMUnread(UserId);
                output.NumberUnread = numberUnread;
                if (result == 0)
                {
                    output.ErrorCode = "01";
                    output.Message = "Có lỗi trong quá trình update";
                }
                else
                {
                    output.ErrorCode = "00";
                    output.Message = "Cập nhật thành công";
                }
            }
            catch(Exception ex)
            {
                _logger.LogError($"FCMHasRead: " + ex.ToString());
                output.ErrorCode = "01";
                output.Message = "Có lỗi trong quá trình update";
            }
            return output;
        }
        /// <summary>
        /// Get Message Detail
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="FCMMessageID"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<LstFCMMessage> GetDetailMsg(string UserID, int FCMMessageID)
        {
            var output = new LstFCMMessage();
            try
            {
                var result = await _repoWrapper.FCMMessage.GetDetailMessage(UserID, FCMMessageID);
                if(result !=null)
                {
                    output.FCMMessageId = result.FCMMessage_ID;
                    output.Title = result.Title;
                    output.Body = result.Body;
                    output.formId = result.Form_ID;
                    output.id = result.ParameterId;
                    output.categoryId = result.CategoryId;
                    output.typeId = result.ProductTypeId;
                    output.formAppName = result.FormNameApp;
                    output.notiSpecType = result.NotiSpecType;
                    output.fullUrl = result.FullUrl;
                    output.fullUrlImage = result.FullUrlImage;
                    output.notifyType = result.NotificationType;
                    output.CreateDate = result.CreateDate;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"GetDetailMsg: " + ex.ToString());
            }
            return output;
        }
        /// <summary>
        /// Pusht notification
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<FCMMessageDTO> PushNoti([FromBody]FCMMessageDTO model)
        {
            try
            {
                //Sent Noti to FireBase
                var output = Utils.Util.SendMessageFirebase(model.FCMMsg);
                //Save Message
                //int FCMMessageID = _repoWrapper.FCMMessage.AddNewData(UserID);
                //Update Data
                FCMMessage fCMMessage = new FCMMessage();
                fCMMessage.Title = model.FCMMsg.Notification.Title;
                fCMMessage.Body = model.FCMMsg.Notification.Body;
                fCMMessage.CreateBy = model.UserId;
                fCMMessage.CreateDate = DateTime.Now;
                fCMMessage.LastEditBy = model.UserId;
                fCMMessage.LastEditDate = DateTime.Now;
                fCMMessage.UserID = model.UserId;
                fCMMessage.Topic = model.FCMMsg.Topic;
                fCMMessage.NotificationType = model.FCMMsg.Data.notifyType;
                fCMMessage.Form_ID = model.FCMMsg.Data.formId;
                fCMMessage.ParameterId = model.FCMMsg.Data.id;
                fCMMessage.FullUrl = model.FCMMsg.Data.fullUrl;
                fCMMessage.FullUrlImage = model.FCMMsg.Data.fullUrlImage;
                fCMMessage.NotiSpecType = model.FCMMsg.Data.notiSpecType;
                fCMMessage.IsPinTop = model.FCMMsg.Data.isPinTop;
                fCMMessage.FormNameApp = model.FCMMsg.Data.formAppName;
                fCMMessage.ProductTypeId = model.FCMMsg.Data.typeId;
                fCMMessage.CategoryId = model.FCMMsg.Data.categoryId;
                
                fCMMessage.HasRead = 0;
                await _repoWrapper.FCMMessage.AddNewFCMMessage(fCMMessage);
                

                if (model.FCMMsg.Topic == "Global")
                {
                    var lstMsg = _repoWrapper.FCMClient.FindAll().ToList();
                    foreach (var p in lstMsg)
                    {
                        if (!String.IsNullOrEmpty(p.UserID.ToString()))
                        {
                            FCMMessage fCMMessage1 = new FCMMessage();

                            fCMMessage1.Title = model.FCMMsg.Notification.Title;
                            fCMMessage1.Body = model.FCMMsg.Notification.Body;
                            fCMMessage1.CreateBy = model.UserId;
                            fCMMessage1.CreateDate = DateTime.Now;
                            fCMMessage1.LastEditBy = model.UserId;
                            fCMMessage1.LastEditDate = DateTime.Now;
                            fCMMessage1.UserID = model.UserId;
                            fCMMessage1.Topic = p.UserID.ToString();
                            fCMMessage1.NotificationType = model.FCMMsg.Data.notifyType;
                            fCMMessage1.Form_ID = model.FCMMsg.Data.formId;
                            fCMMessage1.CategoryId = model.FCMMsg.Data.categoryId;
                            fCMMessage1.ParameterId = model.FCMMsg.Data.id;
                            fCMMessage1.FullUrl = model.FCMMsg.Data.fullUrl;
                            fCMMessage1.FullUrlImage = model.FCMMsg.Data.fullUrlImage;
                            fCMMessage1.NotiSpecType = model.FCMMsg.Data.notiSpecType;
                            fCMMessage1.IsPinTop = model.FCMMsg.Data.isPinTop;
                            fCMMessage1.FormNameApp = model.FCMMsg.Data.formAppName;
                            fCMMessage1.ProductTypeId = model.FCMMsg.Data.typeId;
                            fCMMessage1.HasRead = 0;
                            await _repoWrapper.FCMMessage.AddNewFCMMessage(fCMMessage1);
                            
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"FCMMessageDTO: {ex.ToString()}");
                model.ErrorCode = "005";
                model.Message = Utils.ConstMessage.GetMsgConst("005");
            }
            return model;
        }

        [HttpPost]
        public async Task<FCMMessageChatDTO> PushChatNoti([FromBody] FCMMessageChatDTO model)
        {
            try
            {
                //Sent Noti to FireBase
                var output = Utils.Util.SendMessageFirebaseChatViaTokken(model);                
            }
            catch (Exception ex)
            {
                _logger.LogError($"FCMMessageDTO: {ex.ToString()}");
                model.ErrorCode = "005";
                model.Message = Utils.ConstMessage.GetMsgConst("005");
            }
            return model;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}