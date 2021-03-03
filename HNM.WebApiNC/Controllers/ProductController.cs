using AutoMapper;
using HNM.DataNC.Models;
using HNM.DataNC.ModelsFilter;
using HNM.DataNC.ModelsNC.ModelsDTO;
using HNM.RepositoriesNC.RepositoriesBase;
using HNM.WebApiNC.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

using System.Web;
using HNM.WebApiNC.Utils;
using HNM.DataNC.ModelsNC.ModelsUtility;
using HNM.RepositoriesNC.PaymentRepositoriesBase;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using HNM.DataNC.ModelsStore;

namespace HNM.WebApiNC.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class ProductController : ControllerBase, IDisposable
    {
        private HanomaContext hanomaContext;
        private readonly ILoggerManager _logger;
        private IRepositoryWrapper _repoWrapper;
        private IPaymentRepositoryWrapper _repoPaymentWrapper;
        private IMapper _mapper;
        private readonly IDistributedCache _distributedCache;
        public IConfiguration _configuration;
        public ProductController(IRepositoryWrapper repoWrapper, ILoggerManager logger, IMapper mapper, HanomaContext _hanomaContext, IDistributedCache distributedCache, IPaymentRepositoryWrapper repoPaymentWrapper, IConfiguration configuration)
        {
            hanomaContext = _hanomaContext;
            _repoWrapper = repoWrapper;
            _logger = logger;
            _mapper = mapper;
            _distributedCache = distributedCache;
            _repoPaymentWrapper = repoPaymentWrapper;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<ProductDetailsDTO> ProcessPhoneProduct()
        {
            var output = new ProductDetailsDTO();

            var s = await _repoWrapper.Product.ProcessPhoneNumber();


            return output;
        }

        [HttpGet]
        public async Task<ProductDetailsDTO> GetProductDetails(int productId)
        {

            //var pr = hanomaContext.Product.FirstOrDefault();
            //var oktest = hanomaContext.Product
            //    .FromSqlRaw($"SELECT TOP 3 B.* " +
            //    $"FROM ProductBlockProduct A " +
            //    $"JOIN Product B ON A.Product_ID = B.Product_ID WHERE A.ProductBlock_ID = 1 ORDER BY B.LastEditDate")
            //    .AsNoTracking()
            //    .ToList();

            var output = new ProductDetailsDTO();
            
            var productDetail = _repoWrapper.Product.FirstOrDefault(x => x.Product_ID == productId);
            if (productDetail == null)
            {
                output.ErrorCode = "P001";
                output.Message = "Sản phẩm không tồn tại";
                return output;
            }
            var productPicture = await _repoWrapper.ProductPicture.GetByProductId(productId);

            string pUser = productDetail.CreateBy.ToString();
            var profiles = await _repoWrapper.AspNetUserProfiles.FirstOrDefaultAsync(p => p.UserId == pUser);
            if(profiles != null)
            {
                output.Profiles = profiles;
            }    



            output.ProductDetails = _mapper.Map<ProductDTO>(productDetail);
            ///output.ProductPictures = _mapper.Map<IEnumerable<ProductPictureDTO>>(productPicture);
            output.ProductPictures = productPicture;
            if(productDetail.ProductBrand_ID != 0)
            {
                var productBrand = _repoWrapper.Brand.FirstOrDefault(x => x.ProductBrand_ID == productDetail.ProductBrand_ID);
                output.ProductBrand = _mapper.Map<BrandDTO>(productBrand);
                //Get Năm tham gia
                output.ProductBrand.ProductBrandYearJoin = (int)(DateTime.Now.Year - productBrand.CreateDate?.Year) + 1;
                output.ProductBrand.LocationName =  _repoWrapper.Location.FirstOrDefault(p => p.Location_ID == productBrand.Location_ID)?.Name;
            }    
            

            output.ProductDetails.ProductModelName = _repoWrapper.ProductModel.FirstOrDefault(x => x.ProductModel_ID == productDetail.ProductModel_ID)?.Name;
            output.ProductDetails.ProductManufactureName = _repoWrapper.ProductManufacture.FirstOrDefault(x => x.ProductManufacture_ID == productDetail.ProductManufacture_ID)?.Name;
            output.ProductDetails.SaleLocationName = _repoWrapper.Location.FirstOrDefault(x => x.Location_ID == productDetail.SaleLocation_ID)?.Name;
            output.ProductDetails.ProductCategoryName = _repoWrapper.ProductCategory.FirstOrDefault(x => x.ProductCategory_ID == productDetail.ProductCategory_ID)?.Name;
            output.ProductDetails.CountryName = _repoWrapper.Country.FirstOrDefault(x => x.Country_ID == productDetail.Country_ID)?.Name;
            output.ProductDetails.Content = HttpUtility.HtmlDecode(output.ProductDetails.Content);
            if(productDetail.ProductType_ID == 11 && productDetail.Unit != null)            
            {
                try
                {
                    var resultUnit = await _repoWrapper.Unit.GetMinorByMinorId(Int32.Parse(productDetail.Unit));
                    if (resultUnit != null)
                    {
                        output.ProductDetails.UnitName = resultUnit.MinorName;
                    }
                }
                catch
                {

                }
            }
            else
            {
                output.ProductDetails.UnitName = _repoWrapper.Unit.FirstOrDefault(x => x.Id.ToString() == productDetail.Unit)?.Name;
            }
            
            output.ProductDetails.RelatedCategoryName = _repoWrapper.ProductCategory.FirstOrDefault(x => x.ProductCategory_ID == productDetail.RelatedCategoryId)?.Name;
            output.ProductDetails.AccessoriesCategoryName = _repoWrapper.ProductCategory.FirstOrDefault(x => x.ProductCategory_ID == productDetail.AccessoriesCategoryId)?.Name;

           
            //Meta keyword            
            output.MetaKeyword = _mapper.Map<IEnumerable<MetaKeywordDTO>>(await _repoWrapper.Product.GetListMetaKeyword(productId));

            return output;
        }
        [HttpGet]
        public async Task<ProductDetailsDTO> GetShopmanProductDetails(int productId)
        {

            //var pr = hanomaContext.Product.FirstOrDefault();
            //var oktest = hanomaContext.Product
            //    .FromSqlRaw($"SELECT TOP 3 B.* " +
            //    $"FROM ProductBlockProduct A " +
            //    $"JOIN Product B ON A.Product_ID = B.Product_ID WHERE A.ProductBlock_ID = 1 ORDER BY B.LastEditDate")
            //    .AsNoTracking()
            //    .ToList();

            var output = new ProductDetailsDTO();
            var productDetail = _repoWrapper.Product.FirstOrDefault(x => x.Product_ID == productId);
            if (productDetail == null)
            {
                output.ErrorCode = "P001";
                output.Message = "Sản phẩm không tồn tại";
                return output;
            }
            var productPicture = await _repoWrapper.ProductPicture.ShopmanGetByProductId(productId);

            var productBrand = _repoWrapper.Brand.FirstOrDefault(x => x.ProductBrand_ID == productDetail.ProductBrand_ID);
            output.ProductDetails = _mapper.Map<ProductDTO>(productDetail);
            ///output.ProductPictures = _mapper.Map<IEnumerable<ProductPictureDTO>>(productPicture);
            output.ProductPictures = productPicture;

            output.ProductBrand = _mapper.Map<BrandDTO>(productBrand);
            output.ProductDetails.ProductModelName = _repoWrapper.ProductModel.FirstOrDefault(x => x.ProductModel_ID == productDetail.ProductModel_ID)?.Name;
            output.ProductDetails.ProductManufactureName = _repoWrapper.ProductManufacture.FirstOrDefault(x => x.ProductManufacture_ID == productDetail.ProductManufacture_ID)?.Name;
            output.ProductDetails.SaleLocationName = _repoWrapper.Location.FirstOrDefault(x => x.Location_ID == productDetail.SaleLocation_ID)?.Name;
            output.ProductDetails.ProductCategoryName = _repoWrapper.ProductCategory.FirstOrDefault(x => x.ProductCategory_ID == productDetail.ProductCategory_ID)?.Name;
            output.ProductDetails.CountryName = _repoWrapper.Country.FirstOrDefault(x => x.Country_ID == productDetail.Country_ID)?.Name;
            output.ProductDetails.Content = HttpUtility.HtmlDecode(output.ProductDetails.Content);
            if(output.ProductDetails.ProductType_ID == 11) //Dịch vụ lấy trong bảng Unit
            {
                try
                {
                    var minor = await _repoWrapper.Unit.GetMinorByMinorId(Convert.ToInt32(productDetail.Unit));
                    output.ProductDetails.UnitName = minor?.MinorName;
                }
                catch(Exception ex)
                {

                }
            }
            else
            {
                output.ProductDetails.UnitName = _repoWrapper.Unit.FirstOrDefault(x => x.Id.ToString() == productDetail.Unit)?.Name;
            }
            
            output.ProductDetails.RelatedCategoryName = _repoWrapper.ProductCategory.FirstOrDefault(x => x.ProductCategory_ID == productDetail.RelatedCategoryId)?.Name;
            output.ProductDetails.AccessoriesCategoryName = _repoWrapper.ProductCategory.FirstOrDefault(x => x.ProductCategory_ID == productDetail.AccessoriesCategoryId)?.Name;

         


            return output;
        }
        // type 2 - cần mua máy, type 4 - cần thuê máy
        [HttpGet]
        public async Task<ListProductMarketDTO> GetTopProductMarket(int productTypeId = 2)
        {
            var output = new ListProductMarketDTO();
            var cacheKey = $"Product_GetTopProductMarket{productTypeId}";
            var redisEncode = await _distributedCache.GetStringAsync(cacheKey);
            if (redisEncode != null)
            {
                output = JsonConvert.DeserializeObject<ListProductMarketDTO>(redisEncode);
            }
            else
            {
                var result = await _repoWrapper.Product.GetTopProductMarket(productTypeId);
                output.Data = _mapper.Map<IEnumerable<ProductSearchResultDTO>>(result);
                await _distributedCache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(output), Utils.Util.RedisOptions());
            }

            return output;
        }
        /// <summary>
        /// các tham số là option, cái gì chưa rõ thì ping em nhé, mô tả vào dài quá
        /// QueryType =1 : Máy để bán máy cho thuê , 2: Phụ tùng 3 : Vật tư
        /// máy để bán : productType = 1 
        ///  bán phụ tùng : producttype_id=5 
        /// cần mua phụ tùng:  producttype_id=6	
        /// cần mua máy : producttype_id = 2 
        /// cho thuê máy : producttype_id = 3 
        /// cần thuê máy : producttype_id = 4 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ProductPaggingDTO> GetListProduct([FromQuery]ProductFilter parameters)
        {                       
            if (parameters.ProductTypeId == 5 || parameters.ProductTypeId == 6)
            {
                var result = await _repoWrapper.Product.GetListAccesories(parameters);
                var firstItem = result.FirstOrDefault();
                int totalRow = (int)(firstItem?.TotalRows ?? 0);
                var output = new ProductPaggingDTO();
                output.PageSize = parameters.PageSize;
                output.CurrentPage = parameters.Page;
                output.TotalRecord = totalRow;
                output.TotalPage = (totalRow - 1) / parameters.PageSize + 1;
                output.Data = _mapper.Map<IEnumerable<ProductSearchResultDTO>>(result);
                return output;
            }
            else if (parameters.ProductTypeId == 7)
            {
                var result = await _repoWrapper.Product.GetListMaterials(parameters);
                var firstItem = result.FirstOrDefault();
                int totalRow = (int)(firstItem?.TotalRows ?? 0);
                var output = new ProductPaggingDTO();
                output.PageSize = parameters.PageSize;
                output.CurrentPage = parameters.Page;
                output.TotalRecord = totalRow;
                output.TotalPage = (totalRow - 1) / parameters.PageSize + 1;
                output.Data = _mapper.Map<IEnumerable<ProductSearchResultDTO>>(result);
                return output;
            }
            else if(parameters.ProductTypeId == 11)
            {
                var result = await _repoWrapper.Product.GetListServices(parameters);
                var firstItem = result.FirstOrDefault();
                int totalRow = (int)(firstItem?.TotalRows ?? 0);
                var output = new ProductPaggingDTO();
                output.PageSize = parameters.PageSize;
                output.CurrentPage = parameters.Page;
                output.TotalRecord = totalRow;
                output.TotalPage = (totalRow - 1) / parameters.PageSize + 1;
                output.Data = _mapper.Map<IEnumerable<ProductSearchResultDTO>>(result);
                return output;
            }    
            else
            {
                var result = await _repoWrapper.Product.GetListProductPagging(parameters);
                var firstItem = result.FirstOrDefault();
                int totalRow = (int)(firstItem?.TotalRows ?? 0);
                var output = new ProductPaggingDTO();
                output.PageSize = parameters.PageSize;
                output.CurrentPage = parameters.Page;
                output.TotalRecord = totalRow;
                output.TotalPage = (totalRow - 1) / parameters.PageSize + 1;
                output.Data = _mapper.Map<IEnumerable<ProductSearchResultDTO>>(result);
                return output;
            }
        }

        [HttpGet]
        public async Task<TopProductByCateIdDTO> GetTopProductByCategoryId(int productCategoryID = 656)
        {
            var output = new TopProductByCateIdDTO();
            try
            {
                var cacheKey = $"Product_GetCarouselList{productCategoryID}";
                var redisEncode = await _distributedCache.GetStringAsync(cacheKey);
                if (redisEncode != null)
                {
                    output = JsonConvert.DeserializeObject<TopProductByCateIdDTO>(redisEncode);
                }
                else
                {
                    var result = await _repoWrapper.Product.GetTopProductByCategoryId(productCategoryID);
                    output.Data = _mapper.Map<IEnumerable<ProductItemByCateIdDTO>>(result);
                await _distributedCache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(output), Utils.Util.RedisOptions());
            }
            }
            catch(Exception ex)
            {
                _logger.LogError($"GetTopProductByCategoryId: " + ex.ToString());
            }
            return output;
        }
        /// <summary>
        /// Get Part Number for Accesories and material
        /// Acessories : ProductType = 5
        /// Material : ProductType = 7
        /// </summary>
        /// <param name="ProductCategoryId"></param>
        /// <param name="ProductTypeId"></param>
        /// <returns></returns>

        [HttpGet]
        public async Task<List<PatchNumberDTO>> GetPartNumber(int? ProductCategoryId, int? ProductTypeId)
        {
            var output = new List<PatchNumberDTO>();
            try
            {
                var result = await _repoWrapper.Product.GetListPartNumber(ProductCategoryId, ProductTypeId);
                output = _mapper.Map<List<PatchNumberDTO>>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetPartNumber : " + ex.ToString());
            }
            return output;
        }

        //}
        /// <summary>
        /// Get List Product Shopman
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<ProductPaggingDTO> GetLstProductShopMan([FromQuery]ProductShopManFilter model)
        {
                        
            var output = new ProductPaggingDTO();
            if(model.ProductTypeId == 5)
            {
                var resultAcc = await _repoWrapper.Product.GetListAccesories(_mapper.Map<ProductFilter>(model));
                var firstItemAcc = resultAcc.FirstOrDefault();
                int totalRowAcc = (int)(firstItemAcc?.TotalRows ?? 0);
                
                output.PageSize = model.PageSize;
                output.CurrentPage = model.Page;
                output.TotalRecord = totalRowAcc;
                output.TotalPage = (totalRowAcc - 1) / model.PageSize + 1;
                output.Data = _mapper.Map<IEnumerable<ProductSearchResultDTO>>(resultAcc);
                return output;
            }    
            var result = await _repoWrapper.Product.GetLstProductShopMan(model);
            var firstItem = result.FirstOrDefault();
            int totalRow = (int)(firstItem?.TotalRows ?? 0);
            
            output.PageSize = model.PageSize;
            output.CurrentPage = model.Page;
            output.TotalRecord = totalRow;
            output.TotalPage = (totalRow - 1) / model.PageSize + 1;
            output.Data = _mapper.Map<IEnumerable<ProductSearchResultDTO>>(result);
            return output;
        }

        /// <summary>
        /// Get List ProductForSell Shopman
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<ProductPaggingDTO> GetLstProductForSellShopMan([FromQuery] ProductShopManFilter model)
        {

            var output = new ProductPaggingDTO();
            if (model.ProductTypeId == 5)
            {
                var resultAcc = await _repoWrapper.Product.GetListAccesories(_mapper.Map<ProductFilter>(model));
                var firstItemAcc = resultAcc.FirstOrDefault();
                int totalRowAcc = (int)(firstItemAcc?.TotalRows ?? 0);

                output.PageSize = model.PageSize;
                output.CurrentPage = model.Page;
                output.TotalRecord = totalRowAcc;
                output.TotalPage = (totalRowAcc - 1) / model.PageSize + 1;
                output.Data = _mapper.Map<IEnumerable<ProductSearchResultDTO>>(resultAcc);
                return output;
            }
            var result = await _repoWrapper.Product.GetLstProductForSellShopMan(model);
            var firstItem = result.FirstOrDefault();
            int totalRow = (int)(firstItem?.TotalRows ?? 0);

            output.PageSize = model.PageSize;
            output.CurrentPage = model.Page;
            output.TotalRecord = totalRow;
            output.TotalPage = (totalRow - 1) / model.PageSize + 1;
            output.Data = _mapper.Map<IEnumerable<ProductSearchResultDTO>>(result);
            return output;
        }

        /// <summary>
        /// Action H : Hủy đăng Action R : Renew
        /// </summary>
        /// <param name="model"></param>
        /// <param name="Action"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<ProductIdDTO> ActionProductShopMan(ProductIdDTO model, string Action)
        {
            _logger.LogDebug($"ActionProductShopMan: model : " + JsonConvert.SerializeObject(model) + " Action: " + Action);
            var output = new ProductIdDTO();
            output = model;
            try
            {
                var UserId = Guid.Parse(model.UserId);
                var checkProd = _repoWrapper.Product.FirstOrDefault(p => p.Product_ID == model.ProductId && p.CreateBy == UserId);
                if (checkProd != null)
                {
                    if(Action == "R")// Renew 
                    {
                        var checkRenew = await _repoWrapper.Product.CheckRenewProductByBrandId(model);
                        if(checkRenew != null)
                        {
                            var brand = await _repoWrapper.Brand.FirstOrDefaultAsync(p => p.CreateBy == UserId);
                            if(brand !=null)
                            {
                                int productBrandType = brand.ProductBrandType_ID ?? 1;
                                if(productBrandType == 1)
                                {
                                    output.ErrorCode = "02"; // mapping client 
                                    output.Message = $"Gian hàng của bạn không có quyền làm mới, Bạn cần nâng cấp gian hàng để có quyền này!";
                                }
                                else
                                {
                                    var QtyRenew = await _repoPaymentWrapper.PaymentServcie.FirstOrDefaultAsync(p => p.ServiceId == productBrandType);
                                    {
                                        if (checkRenew.NumberRenewInOneDay < QtyRenew.ReceiveFromCus)
                                        {
                                            var result = await _repoWrapper.Product.ActionProductShopman(model, Action);
                                            var countRenew = checkRenew.NumberRenewInOneDay == 1 ? 1 : checkRenew.NumberRenewInOneDay + 1;
                                            output.ErrorCode = "00";
                                            output.Message = $"Số lần làm mới trong ngày {countRenew} / {QtyRenew.ReceiveFromCus}";

                                        }
                                        else
                                        {
                                            output.ErrorCode = "01";
                                            output.Message = $"Ban đã đạt giới hạn làm mới tin trong ngày ({QtyRenew.ReceiveFromCus} tin), hãy thử lại vào ngày mai";
                                        }

                                    }
                                }
                               
                            }    
                            
                        }    
                    }
                    else if(Action == "K")
                    {
                        var brand = await _repoWrapper.Brand.FirstOrDefaultAsync(p => p.CreateBy == UserId);
                        var checkCountProd = await _repoWrapper.Product.CountProductByBrand(brand.ProductBrand_ID);
                        var QtyPost = await _repoPaymentWrapper.PaymentServcie.FirstOrDefaultAsync(p => p.ServiceId == brand.ProductBrandType_ID); 
                        if(checkCountProd >= QtyPost.ProductQty)
                        {
                            output.ErrorCode = "02";
                            output.Message = $"Tin đăng đã vượt quá số lượng {QtyPost.ProductQty} bạn cần nâng cấp gian hàng để khôi phục tin đăng";
                        }
                        else
                        {
                            var result = await _repoWrapper.Product.ActionProductShopman(model, Action);
                            output.ErrorCode = result.ErrorCode;
                            output.Message = result.Message;
                        }
                    }    
                    else 
                    {
                        var result = await _repoWrapper.Product.ActionProductShopman(model, Action);
                        output.ErrorCode = result.ErrorCode;
                        output.Message = result.Message;
                    }
                }
                else
                {
                    output.ErrorCode = "001";
                    output.Message = "Không tồn tại: " + model.ProductId;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"ActionProductShopMan: {ex.ToString()}");
                output.ErrorCode = "001";
                output.Message = "Có lỗi trong quá trình cập nhật";
            }
            return output;
        }
        /// <summary>
        /// PostProduct Shopman
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<PostProductShopManResultDTO> PostProductShopMan(PostProductShopManDTO model)
        {
            _logger.LogDebug($"PostProductShopMan: model : " + JsonConvert.SerializeObject(model));
            var output = new PostProductShopManResultDTO();
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
                    Product productModel = _mapper.Map<Product>(model.Product);                    
                    int ProductId = _repoWrapper.Product.PostProductShopman(productModel, model.MainImage, model.SubImage, model.UserId);
                    if (ProductId != 0)
                    {
                        //Save MainImage
                        SaveMainImage(model.MainImage, ProductId);
                        //Save Sub Image
                        SaveIllustrationImages(model.SubImage, ProductId);
                        output.ProductId = ProductId;
                        output.ErrorCode = "00";
                        output.Message = "Thêm mới thành công";

                      
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
                    _logger.LogError($"PostProductShopMan: " + ex.ToString());
                    output.ErrorCode = "001";
                    output.Message = Utils.ConstMessage.GetMsgConst("001");
                    return output;
                }

            }
        }
        /// <summary>
        /// Put ProductShopMan
        /// </summary>
        /// <param name="ProductID"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut]
        public async Task<PostProductShopManResultDTO> PutProductShopMan(int ProductID, PostProductShopManDTO model,int IsRepair)
        {
            _logger.LogDebug($"PutProductShopMan: model : " + JsonConvert.SerializeObject(model) + " ProductID: " + ProductID);
            var output = new PostProductShopManResultDTO();
            var checkCanEdit = _repoWrapper.Product.CanEditProduct(ProductID, model.UserId);
            //Check Can Edit
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
                    
                    Product productModel = _mapper.Map<Product>(model.Product);
                    productModel.Product_ID = ProductID;                    
                    int ProductId = _repoWrapper.Product.PostProductShopman(productModel, model.MainImage, model.SubImage, model.UserId);
                    if (ProductId != 0)
                    {
                        if (IsRepair == 1)// Tiếp tục chỉnh sửa delete subImage
                        {
                            var subImage = await _repoWrapper.ProductPicture.GetListDeleteProdPicture(ProductID);
                          
                            await _repoWrapper.Product.DeleteIllustrationImages(subImage);

                        }
                        //Save MainImage    
                        if (model.MainImage != null)
                        {
                            //Save MainImage
                            SaveMainImage(model.MainImage, ProductId);
                        }
                        
                        //Save Sub Image                        
                        if (model.SubImage != null)
                        {
                            //Delete Image
                            if(model.DeleteProdPicture !=null)
                            {                            
                                await _repoWrapper.Product.DeleteIllustrationImages(model.DeleteProdPicture);
                            }

                            //Save Sub Image
                            SaveIllustrationImages(model.SubImage, ProductId);
                        }
                       
                        output.ProductId = ProductId;
                        output.ErrorCode = "00";
                        output.Message = "Update thành công";

                       

                    }
                    else
                    {
                        output.ErrorCode = "001";
                        output.Message = Utils.ConstMessage.GetMsgConst("001");
                    }
                    return output;

                }
                catch (Exception ex)
                {
                    _logger.LogError($"PutProductShopMan: " + ex.ToString());
                    output.ErrorCode = "001";
                    output.Message = Utils.ConstMessage.GetMsgConst("001") ;
                    return output;
                }

            }

        }
        /// <summary>
        /// Get StatusMachine 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<ProductStatusMachine>> GetProductStatusMachine()
        {
            var output = new List<ProductStatusMachine>();
            output.Add(new ProductStatusMachine() { ID = "Mới", StatusMachine = "Mới" });
            output.Add(new ProductStatusMachine() { ID = "Đã qua sử dụng", StatusMachine = "Đã qua sử dụng" });
            output.Add(new ProductStatusMachine() { ID = "Thanh lý", StatusMachine = "Thanh lý" });
            return output;
        }
        /// <summary>
        /// Check prodbrand maximum producbrand can post 
        /// </summary>
        /// <param name="ProductBrandId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ModelBaseStatus> CheckPostProduct(int ProductBrandId)
        {
            var output = new ModelBaseStatus();
            if(ProductBrandId == 958) // set unlimited for product  brand
            {
                return output;
            }    
            var prodBrand = await _repoWrapper.Brand.FirstOrDefaultAsync(p => p.ProductBrand_ID == ProductBrandId);
            
            if(prodBrand != null)
            {

                if(prodBrand.ProductBrandType_ID == 1 || prodBrand.ProductBrandType_ID == null)
                {
                    var prodQty = await _repoPaymentWrapper.PaymentServcie.FirstOrDefaultAsync(p => p.ServiceId == 1);
                    var prodCount = await _repoWrapper.Product.CountProductByBrand(ProductBrandId);
                    if(prodCount >= prodQty.ProductQty)
                    {
                        output.ErrorCode = "01";
                        output.Message = $"Bạn đã quá số tin đăng cho phép bạn cần hủy hoặc nâng cấp gian hàng lên quy mô cao hơn";
                        return output;
                    }
                }
                if (prodBrand.ProductBrandType_ID == 2)
                {
                    var prodQty = await _repoPaymentWrapper.PaymentServcie.FirstOrDefaultAsync(p => p.ServiceId == 2);
                    var prodCount = await _repoWrapper.Product.CountProductByBrand(ProductBrandId);
                    if (prodCount >= prodQty.ProductQty)
                    {
                        output.ErrorCode = "01";
                        output.Message = $"Bạn đã quá số tin đăng cho phép bạn cần hủy hoặc nâng cấp gian hàng lên quy mô cao hơn";
                        return output;
                    }
                }
                if (prodBrand.ProductBrandType_ID == 3)
                {
                    var prodQty = await _repoPaymentWrapper.PaymentServcie.FirstOrDefaultAsync(p => p.ServiceId == 3);
                    var prodCount = await _repoWrapper.Product.CountProductByBrand(ProductBrandId);
                    if (prodCount >= prodQty.ProductQty)
                    {
                        output.ErrorCode = "01";
                        output.Message = $"Bạn đã quá số tin đăng cho phép bạn cần hủy hoặc nâng cấp gian hàng lên quy mô cao hơn";
                        return output;
                    }
                }
                else
                {
                    output.ErrorCode = "00";
                    output.Message = "Được phép đăng tin";
                    return output;
                }
            }
            else
            {
                output.ErrorCode = "01";
                output.Message = $"Không tồn tại nhà cung cấp";
                return output;
            }
           
            
            return output;
        }
        /// <summary>
        /// Check renew 
        /// </summary>
        /// <param name="MainImage"></param>
        /// <param name="ProductId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ModelBaseStatus> CheckRenew(int ProductBrandId)
        {
            var output = new ModelBaseStatus();

            return output;
        }

        private async Task SaveMainImage(ImageUploadDTO MainImage, int ProductId)
        {
            var pathAbsolute = @"C:\Domains\DaNBVQ\wwwroot";
            var urlProduct = _repoWrapper.Product.CreateImageURL(ProductId);
            MainImage.FileName = String.Format("{0}-00.{1}", urlProduct, MainImage.ExtensionType.Replace("image/", ""));
            MainImage.PathSave = "product/mainimages/original";
            var PatchToSave = Path.Combine(pathAbsolute, MainImage.PathSave);
            var physicalPath = Path.Combine(PatchToSave, MainImage.FileName);
            await UploadImage(MainImage);
            //Utils.Util.WaterMark(physicalPath, MainImage.FileName);
            //Make Thumb & small
            try
            {
                var PatchSmall = "product/mainimages/small";
                var PatchThumb = "product/mainimages/thumb";
                Utils.Util.EditSize(physicalPath, Path.Combine(pathAbsolute, PatchSmall, MainImage.FileName), 500, 500);
                Utils.Util.EditSize(physicalPath, Path.Combine(pathAbsolute, PatchThumb, MainImage.FileName), 200, 200);

            }
            catch (Exception ex)
            {

            }

            ///Update Image Name
            _repoWrapper.Product.UpdateMainImageProduct(MainImage.FileName, ProductId);


        }
        private async Task SaveIllustrationImages(List<ImageUploadDTO> SubImage, int ProductId)
        {
            var count = 0;
            foreach (var p in SubImage)
            {
                var timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
                if (p.Base64 == null) return;
                count += 1;                
                var urlProduct = _repoWrapper.Product.CreateImageURL(ProductId);
                p.FileName = String.Format("{0}-mobile-0{1}-{2}.{3}", urlProduct, count,timestamp, p.ExtensionType.Replace("image/", ""));
                p.PathSave = "productpicture/mainimages/original";       
                var pathAbsolute = @"C:\Domains\DaNBVQ\wwwroot";
                
                var PatchToSave = Path.Combine(pathAbsolute, p.PathSave);
                var physicalPath = Path.Combine(PatchToSave, p.FileName);
                UploadImage(p);
                Utils.Util.WaterMark(physicalPath, p.FileName);
                //Make Thumb & small
                try
                {
                    var PatchSmall = "productpicture/mainimages/small";
                    var PatchThumb = "productpicture/mainimages/thumb";
                    Utils.Util.EditSize(physicalPath, Path.Combine(pathAbsolute, PatchSmall, p.FileName), 500, 500);
                    Utils.Util.EditSize(physicalPath, Path.Combine(pathAbsolute, PatchThumb, p.FileName), 200, 200);

                }
                catch (Exception ex)
                {

                }


                //Update Image Name
                _repoWrapper.Product.UpdateIllustrationImages(p.FileName, ProductId);
            }
        }
        
        [HttpGet]
        public async Task<List<PatchNumber_SearchByCategory_Result>> GetPatchNumberByCategory(int? ProductCategory_Id, int? ProductType_ID, string text)
        {
            return await _repoWrapper.Product.GetPatchNumberByCategory(ProductCategory_Id, ProductType_ID, text);
        }

        [HttpGet]
        public async Task<List<ProductDTO>> GetListProductDemand(int? ProductBrand_ID)
        {
            var output = new List<ProductDTO>();

            var result = await _repoWrapper.Product.GetListDemandByProductBrand((int)ProductBrand_ID);
            output = _mapper.Map<List<ProductDTO>>(result);

            return output;
        }

        /// <summary>
        /// Get TimeLine Post
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="Page"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<TimeLinePostDTO>> GetLstTimeLinePost(string userId,int? Page = 1,int? PageSize = 10)
        {
            var output = new List<TimeLinePostDTO>();
            try
            {
                
                var result = await _repoWrapper.Product.GetLstTimeLinePost(userId, Page, PageSize);
                if (result != null)
                {
                    output = _mapper.Map<List<TimeLinePostDTO>>(result);
                    foreach (var p in output)
                    {                       
                        if (p.ProductId != null)
                        {
                            var lstPicture = await _repoWrapper.ProductPicture.GetByProductId((int)p.ProductId);
                            if (lstPicture != null)
                            {
                                p.ProductPictures = lstPicture.ToList();
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {

            }
            return output;
        }
        private async Task UploadImage(ImageUploadDTO model)
        {
            try
            {

                var pathAbsolute = @"C:\Domains\DaNBVQ\wwwroot";
                var imageDataByteArray = Convert.FromBase64String(model.Base64);

                var imageDataStream = new MemoryStream(imageDataByteArray);
                imageDataStream.Position = 0;

                var file = File(imageDataByteArray, model.ExtensionType);
                var fileName = model.FileName;
                var pathToSave = Path.Combine(pathAbsolute, model.PathSave);
                var outPath = Path.Combine(pathToSave, fileName);


                if (file.FileContents.Length > 0)
                {
                    using (MemoryStream ms = new MemoryStream(imageDataByteArray))
                    {
                        using (Bitmap bm2 = new Bitmap(ms))
                        {
                            bm2.Save(Path.Combine(pathToSave, fileName));
                        }
                    }


                }
                else
                {

                }
            }
            catch (Exception ex)
            {

            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
