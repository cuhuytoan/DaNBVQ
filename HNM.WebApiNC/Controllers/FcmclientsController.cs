    using AutoMapper;
using HNM.DataNC.Models;
using HNM.DataNC.ModelsNC.ModelsDTO;
using HNM.RepositoriesNC.RepositoriesBase;
using HNM.WebApiNC.Logging;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;


namespace HNM.WebApiNC.Controllers
{

    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class FCMClientsController : ControllerBase, IDisposable
    {
        private readonly ILoggerManager _logger;
        private IRepositoryWrapper _repoWrapper;
        private IMapper _mapper;

        public FCMClientsController(IRepositoryWrapper repoWrapper, ILoggerManager logger, IMapper mapper)
        {
            _repoWrapper = repoWrapper;
            _logger = logger;
            _mapper = mapper;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        ////// GET: api/FCMClients/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FCMClientDTO>> GetFCMClient(Guid id)
        {
            var FCMClient = await _repoWrapper.FCMClient.FindAsync(id);

            if (FCMClient == null)
            {
                return NotFound();
            }
            var result = _mapper.Map<FCMClientDTO>(FCMClient);
            return result;
        }
        /// <summary>
        /// Get FCMCLient by Token or Device ID
        /// </summary>
        /// <param name="DeviceId"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<FCMClientDTO>> GetFCMClientByTokenDevice(string DeviceId, string Token)
        {
            var output = new FCMClientDTO();
            var fcmClient = await _repoWrapper.FCMClient.FirstOrDefaultAsync(p => p.Device_ID == DeviceId || p.Token == Token);
            if (fcmClient == null)
            {
                output.ErrorCode = "01";
                output.Message = "Không tồn tại dữ liệu";
            }
            output = _mapper.Map<FCMClientDTO>(fcmClient);
            return output;

        }      

        /// <summary>
        /// Add new Tokken
        /// </summary>
        /// <param name="FCMClient"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<FCMClientDTO>> PostFCMClient(FCMClientDTO model)
        {
            var output = new FCMClientDTO();
            try
            {
                if (model == null)
                {
                    output.ErrorCode = "001";
                    output.Message = Utils.ConstMessage.GetMsgConst("001");
                    return output;
                }
                var checkExists = _repoWrapper.FCMClient.FirstOrDefault(p => p.Token == model.Token);
                if (checkExists != null)
                {
                    output.ErrorCode = "009";
                    output.Message = Utils.ConstMessage.GetMsgConst("009");
                    return output;
                }
                model.CreateDate = DateTime.Now;
                model.LastSeen = DateTime.Now;
                var models = _mapper.Map<FCMClient>(model);
                _repoWrapper.FCMClient.Create(models);
                await _repoWrapper.SaveChangesAsync();
                output.FCMClientID = models.FCMClient_ID;
                output.Message = Utils.ConstMessage.GetMsgConst("006");
                return output;
            }
            catch (Exception ex)
            {
                _logger.LogError($"PostFCMClient: " + ex.ToString());
                output.ErrorCode = "01";
                output.Message = $"Có lỗi trong quá trình cập nhật";
                return output;
            }
        }
        [HttpPut]
        public async Task<ActionResult<FCMClientDTO>> UpdateFCMClient(Guid FCMClientID, FCMClientDTO model)
        {
           
            if (model == null || FCMClientID == null)
            {
                model.ErrorCode = "001";
                model.Message = Utils.ConstMessage.GetMsgConst("001");
                return model;
            }
            var checkExists = _repoWrapper.FCMClient.FirstOrDefault(p => p.FCMClient_ID == FCMClientID);
            if (checkExists == null)
            {
                model.ErrorCode = "001";
                model.Message = Utils.ConstMessage.GetMsgConst("001");
                return model;
            }
            else
            {
                try
                {

                    var models = _mapper.Map<FCMClient>(model);                    
                    _repoWrapper.FCMClient.UpdateFCMClient(FCMClientID, models);                    
                    model.Message = Utils.ConstMessage.GetMsgConst("007");
                    return model;
                }
                catch (Exception ex)
                {
                    _logger.LogError($"UpdateFCMClient: " + ex.ToString());
                    model.Message = $"Có lỗi trong quá trình cập nhật";
                    return model;
                }
            }


        }
        /// <summary>
        /// Delete FCM CLient when
        /// </summary>
        /// <param name="FCMClientId"></param>
        /// <returns></returns>
        [HttpDelete("{FCMClientId}")]
        private async Task<ActionResult<FCMClientDTO>> DeleteFCMClient(Guid FCMClientId)
        {
            var output = new FCMClientDTO();
            try
            {
                var FCMClient = await _repoWrapper.FCMClient.FindAsync(FCMClientId);
                if (FCMClient == null)
                {
                    output.ErrorCode = "004";
                    output.Message = Utils.ConstMessage.GetMsgConst("004");
                    return output;
                }

                var result = _mapper.Map<FCMClientDTO>(FCMClient);
                _repoWrapper.FCMClient.Delete(FCMClient);
                await _repoWrapper.SaveChangesAsync();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"DeleteFCMClient: " + ex.ToString());
                output.ErrorCode = $"01";
                output.Message = $"Lỗi trong quá trình xóa";
                return output;
            }
        }


    }
}
