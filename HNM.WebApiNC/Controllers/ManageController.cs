using AutoMapper;
using HNM.DataNC.Models;
using HNM.DataNC.ModelsIdentity;
using HNM.DataNC.ModelsNC.ModelsDTO;
using HNM.RepositoriesNC.RepositoriesBase;
using HNM.WebApiNC.Logging;
using HNM.WebApiNC.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading.Tasks;

namespace HNM.WebApiNC.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [Authorize]
    [ApiController]
    public class ManageController : ControllerBase, IDisposable
    {
        private readonly ILoggerManager _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IDistributedCache _distributedCache;
        private IMapper _mapper;
        public ManageController(
            UserManager<ApplicationUser> userManager,
            IRepositoryWrapper repositoryWrapper,
            ILoggerManager logger,
            IMapper mapper,
            IDistributedCache distributedCache
        )
        {
            _userManager = userManager;
            _repositoryWrapper = repositoryWrapper;
            _logger = logger;
            _mapper = mapper;
            _distributedCache = distributedCache;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        [HttpGet]
        public async Task<ActionResult<ProfilersDTO>> Profilers(string UserId)
        {
            var response = new ProfilersDTO();
            try
            {
                var profile = new AspNetUserProfiles();
                profile = _repositoryWrapper.AspNetUserProfiles.FirstOrDefault(p => p.UserId == UserId);
                if (profile == null)
                {
                    _logger.LogError($"[ManageController] {ConstMessage.GetMsgConst("ACC008")}");
                    response.ErrorCode = "ACC008";
                    response.Message = ConstMessage.GetMsgConst("ACC008");
                    return response;
                }
                var result = _mapper.Map<ProfilersDTO>(profile);                
                return result;
            }
            catch (Exception exception)
            {
                _logger.LogError($"[ManageController] {exception.ToString()}");
                response.Message = $"Có lỗi trong quá trình cập nhật";
                return response;
            }
        }
        [HttpPut]
        public async Task<ActionResult<ProfilersDTO>> UpdateProfilers(string userId, ProfilersDTO model)
        {

            try
            {
                if (String.IsNullOrEmpty(userId) || String.IsNullOrEmpty(model.UserId))
                {
                    model.ErrorCode = "ACC008";
                    model.Message = ConstMessage.GetMsgConst("ACC008") + "userId=" + userId + "Model Userid =" + model.UserId;
                    return model;
                }
                var profile = _repositoryWrapper.AspNetUserProfiles.FirstOrDefault(p => p.UserId == userId);
                if (profile == null)
                {
                    _logger.LogError($"[ManageController] {ConstMessage.GetMsgConst("ACC008")}");
                    model.ErrorCode = "ACC008";
                    model.Message = ConstMessage.GetMsgConst("ACC008");
                    return model;
                }
                else
                {
                    model.UserId = userId;
                    _mapper.Map(model, profile);
                    _repositoryWrapper.AspNetUserProfiles.UpdateProfilers(profile);
                    _repositoryWrapper.Save();

                }
                var result = _mapper.Map<ProfilersDTO>(profile);
                result.ErrorCode = "00";
                result.Message = "Cập nhật thành công";
                return result;
            }
            catch (Exception exception)
            {
                _logger.LogError($"[ManageController] {exception.ToString()}");
                model.ErrorCode = "01";
                model.Message = $"Có lỗi trong quá trình cập nhật";
                return model;
            }
        }

    }
}