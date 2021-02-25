using AutoMapper;
using HNM.DataNC.Models;
using HNM.DataNC.ModelsNC.ModelsDTO;
using HNM.RepositoriesNC.RepositoriesBase;
using HNM.WebApiNC.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HNM.WebApiNC.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class ProductModelController : ControllerBase, IDisposable
    {
        private readonly ILoggerManager _logger;
        private IRepositoryWrapper _repoWrapper;
        private IMapper _mapper;
        public ProductModelController(IRepositoryWrapper repoWrapper, ILoggerManager logger, IMapper mapper)
        {
            _repoWrapper = repoWrapper;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ListProductModelSearchDTO> Search(int? productCategoryId, int? productTypeId, int? manufactoryId, string search)
        {
            var result = await _repoWrapper.ProductModel.SearchProductModel(productCategoryId, productTypeId, manufactoryId, search);
            var output = new ListProductModelSearchDTO();
            output.Data = _mapper.Map<IEnumerable<ProductModel_SearchByCategory_Result_DTO>>(result);
            return output;
        }
        [HttpGet]
        public async Task<ListProductModelSearchDTO> ShopmanSearchModel(int? productCategoryId, int? productTypeId, int? manufactoryId, string search ="")
        {
            var result = await _repoWrapper.ProductModel.ShopmanSearchProductModel(productCategoryId, productTypeId, manufactoryId, search);
            var output = new ListProductModelSearchDTO();
            output.Data = _mapper.Map<IEnumerable<ProductModel_SearchByCategory_Result_DTO>>(result);
            return output;
        }
        /// <summary>
        /// Post Product Model (bắt buộc chọn Cateog
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<ProductModelDTO> AddNewModel(ProductModelDTO model)
        {
            _logger.LogDebug($"AddNewModel: {JsonConvert.SerializeObject(model)}");
            var output = new ProductModelDTO();
            if (!ModelState.IsValid)
            {
                return output;
            }
            var checkUser = _repoWrapper.AspNetUsers.FirstOrDefault(p => p.Id == model.UserId);
            if (checkUser == null)
            {
                output.ErrorCode = "001";
                output.Message = "Bạn không có quyền thêm mới model";
            }
            else
            {
                try
                {
                    var checkModel = _repoWrapper.ProductModel.FirstOrDefault(p => p.ProductCategory_ID == model.ProductCategoryID && p.ProductManufacture_ID == model.ProductManufactureID && p.Name.ToUpper().Equals(model.Name.ToUpper()));
                    if (checkModel != null)
                    {
                        output.ErrorCode = "001";
                        output.Message = $"Đã tồn tại model {model.Name}";
                        output.ProductModelID = checkModel.ProductModel_ID;
                        output.ProductManufactureID = checkModel.ProductManufacture_ID;
                        output.ProductCategoryID = checkModel.ProductCategory_ID;
                        output.Name = checkModel.Name;

                    }
                    else
                    {

                        var postProductModel = await _repoWrapper.ProductModel.PostProductModel(_mapper.Map<ProductModel>(model), model.UserId);
                        if (postProductModel.ProductModel_ID > 0)
                        {
                            output.ErrorCode = "00";
                            output.Message = "Thêm mới thành công";
                            output = _mapper.Map<ProductModelDTO>(postProductModel);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"AddNewModel: {ex.ToString()} ");
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