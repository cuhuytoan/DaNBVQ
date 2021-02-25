using AutoMapper;
using HNM.DataNC.ModelsNC.ModelsDTO;
using HNM.RepositoriesNC.RepositoriesBase;
using HNM.WebApiNC.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HNM.WebApiNC.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class MenuController : ControllerBase, IDisposable
    {
        private readonly ILoggerManager _logger;
        private IRepositoryWrapper _repoWrapper;
        private IMapper _mapper;
        private readonly IDistributedCache _distributedCache;
        public MenuController(IRepositoryWrapper repoWrapper, ILoggerManager logger, IMapper mapper, IDistributedCache distributedCache)
        {
            _repoWrapper = repoWrapper;
            _logger = logger;
            _mapper = mapper;
            _distributedCache = distributedCache;
        }
        /// <summary>
        /// danh sách loại hình dịch vụ, nếu type = product thì get category theo parentCategoryId ở api(/api/v1/ProductCategory/GetProdCateByParentID?parentId=parentCategoryId)
        /// nếu type = recruitment thì get category theo api (/api/v1/RecruitmentCategory/GetAll)
        /// nếu type = manufacture thì chuyển sang screen danh sách nhà cung cấp
        /// </summary>
        /// <param name="ArticleCategoryId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ListHomeMenuDTO> GetMenus()
        {
            var output = new ListHomeMenuDTO();
            var cacheKey = "Menu_GetMenus";
            var redisEncode = await _distributedCache.GetStringAsync(cacheKey);
            if (redisEncode != null)
            {
                output = JsonConvert.DeserializeObject<ListHomeMenuDTO>(redisEncode);
            }
            else
            {
                var result = await _repoWrapper.Menu.GetMenus();
                output.Data = _mapper.Map<IEnumerable<HomeMenuDTO>>(result);
                await _distributedCache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(output), Utils.Util.RedisOptions());
            }

            return output;
        }
        /// <summary>
        /// get Menu khi đã login và chưa login
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>

        [HttpGet]
        public async Task<ListHomeMenuDTO> GetMenusAccount(string UserId)
        {
            var output = new ListHomeMenuDTO();
            //Check Login            
            if (!String.IsNullOrEmpty(UserId))
            {
                var checkExistsBrand = await _repoWrapper.AspNetUserProfiles.FirstOrDefaultAsync(p => p.UserId == UserId);
                //Check exists ProductBrand
                if (checkExistsBrand != null && checkExistsBrand.ProductBrand_ID > 0)
                {
                    var result = await _repoWrapper.Menu.GetMenusAccount(1);
                    output.Data = _mapper.Map<IEnumerable<HomeMenuDTO>>(result);
                    output.Data = output.Data.Where(p => p.Name != "Tạo gian hàng");
                }
                else
                {
                    var result = await _repoWrapper.Menu.GetMenusAccount(1);
                    output.Data = _mapper.Map<IEnumerable<HomeMenuDTO>>(result);
                    output.Data = output.Data.Where(p => p.Name != "Quản lý gian hàng");
                }
            }
            else
            {
                var result = await _repoWrapper.Menu.GetMenusAccount(0);
                output.Data = _mapper.Map<IEnumerable<HomeMenuDTO>>(result);
            }
            return output;
        }
        /// <summary>
        /// Get Menu quản lý gian hàng
        /// </summary>
        /// <param name="ParentId">menuId của quản lý gian hàng (values 15) Menu post (values 9999)</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<ListHomeMenuDTO> GetMenuBrandManage(int ParentId)
        {
            var output = new ListHomeMenuDTO();
            var result = await _repoWrapper.Menu.GetMenusByParentId(ParentId);
            output.Data = _mapper.Map<IEnumerable<HomeMenuDTO>>(result);
            return output;
        }
        [Authorize]
        [HttpGet]
        public async Task<ListHomeMenuDTO> GetMenuBrandManageByUser(string UserId,int ProductTypeId)
        {
            var output = new ListHomeMenuDTO();
            try
            {
                var result = await _repoWrapper.Menu.GetMenusStatusByUser(UserId, ProductTypeId);
                output.Data = _mapper.Map<IEnumerable<HomeMenuDTO>>(result);
            }
            catch(Exception ex)
            {

            }
            return output;
        }


        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}