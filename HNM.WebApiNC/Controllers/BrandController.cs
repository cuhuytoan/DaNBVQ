using AutoMapper;
using HNM.DataNC.Models;
using HNM.DataNC.ModelsFilter;
using HNM.DataNC.ModelsNC.ModelsDTO;
using HNM.DataNC.ModelsNC.ModelsUtility;
using HNM.RepositoriesNC.PaymentRepositoriesBase;
using HNM.RepositoriesNC.RepositoriesBase;
using HNM.WebApiNC.Logging;
using HNM.WebApiNC.Utils;
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
using System.Web;


namespace HNM.WebApiNC.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class BrandController : ControllerBase, IDisposable
    {
        private readonly ILoggerManager _logger;
        private IRepositoryWrapper _repoWrapper;
        private IMapper _mapper;
        private readonly IDistributedCache _distributedCache;
        private IPaymentRepositoryWrapper _repoPaymentWrapper;
        public BrandController(IPaymentRepositoryWrapper repoPaymentWrapper,IRepositoryWrapper repoWrapper, ILoggerManager logger, IMapper mapper, IDistributedCache distributedCache)
        {
            _repoWrapper = repoWrapper;
            _logger = logger;
            _mapper = mapper;
            _distributedCache = distributedCache;
            _repoPaymentWrapper = repoPaymentWrapper;
        }
        [HttpGet]
        public async Task<TopBrandDTO> GetTopBrand(int topX )
        {
            var output = new TopBrandDTO();
            try
            {
                var cacheKey = $"Brand_GetTopBrand{topX}";
                var redisEncode = await _distributedCache.GetStringAsync(cacheKey);
                if (redisEncode != null)
                {
                    output = JsonConvert.DeserializeObject<TopBrandDTO>(redisEncode);
                }
                else
                {
                    var result = await _repoWrapper.Brand.GetTopBrand(topX);
                    output.Data = _mapper.Map<List<BrandDTO>>(result);
                    
                    output.Data.ForEach(x => x.Description = HttpUtility.HtmlDecode(Common.CommonUtil.StripHTML(HttpUtility.HtmlDecode(x.Description))));
                    
                    await _distributedCache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(output), Utils.Util.RedisOptions());
                }
            }
            catch(Exception ex)
            {
                _logger.LogError($"GetTopBrand: " + ex.ToString());
            }
            return output;
        }
        [HttpGet]
        public async Task<BrandPaggingDTO> GetListBrand(int? page, int? pageSize, int? LocationId, int active = 1, int? productCategoryId = null, string keyword = null)
        {
            var output = new BrandPaggingDTO();
            try
            {
                //var cacheKey = $"Brand_GetListBrand{productCategoryId}{keyword}";
                //var redisEncode = await _distributedCache.GetStringAsync(cacheKey);
                //if (redisEncode != null)
                //{
                //    output = JsonConvert.DeserializeObject<BrandPaggingDTO>(redisEncode);
                //}
                //else
                //{
                    var result = await _repoWrapper.Brand.GetListBrandPagging(page ?? 1, pageSize ?? 20, LocationId, active, productCategoryId: productCategoryId, keyword);
                    int totalRow = result.Item1;
                    output.PageSize = pageSize ?? 20;
                    output.CurrentPage = page ?? 1;
                    output.TotalRecord = totalRow;
                    output.TotalPage = (totalRow - 1) / pageSize ?? 20 + 1;
                    output.Data = _mapper.Map<IEnumerable<ProductBrandSearchDTO>>(result.Item2);
                    output.Data.Select(c => { c.LocationName = Util.LocationDictionary[(int)c.LocationId]; return c; }).ToList();

                 // await _distributedCache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(output), Utils.Util.RedisOptions());
               //}
            }
            catch(Exception ex)
            {
                _logger.LogError($"GetListBrand: " + ex.ToString());
            }
            return output;
        }
        /// <summary>
        /// Get Detail ProductBrand
        /// </summary>
        /// <param name="ProductBrandId"></param>
        /// <returns></returns>

        [HttpGet]
        public async Task<ProductBrandSearchDetailDTO> GetBrandDetails(int ProductBrandId)
        {
            var output = new ProductBrandSearchDetailDTO();
            try
            {
                var result = await _repoWrapper.Brand.FirstOrDefaultAsync(p => p.ProductBrand_ID == ProductBrandId);
                if (result != null)
                {
                    output = _mapper.Map<ProductBrandSearchDetailDTO>(result);
                    var Location = await _repoWrapper.Location.FirstOrDefaultAsync(p => p.Location_ID == output.LocationId);
                    if (Location != null)
                    {
                        output.LocationName = Location.Name;
                    }
                    var CountryName = await _repoWrapper.Country.FirstOrDefaultAsync(p => p.Country_ID == output.Country_ID);
                    if (CountryName != null)
                    {
                        output.CountryName = CountryName.Name;
                    }
                    var district = await _repoWrapper.Location.GetDistrictByName(output.District);
                    if (district != null)
                    {
                        output.DistrictId = district.Id;
                    }
                    if (output.ProductBrandTypeId == null)
                    {
                        output.ProductBrandTypeId = 1; // Set default nhà cc nhỏ
                    }
                    //Get Năm tham gia
                    output.ProductBrandYearJoin = (int)(DateTime.Now.Year - result.CreateDate?.Year) + 1;
                    int totalDay = 100;
                    var prodCount = await _repoWrapper.Product.CountProductByBrand(ProductBrandId);

                    var prodQty = await _repoPaymentWrapper.PaymentServcie.FirstOrDefaultAsync(p => p.ServiceId == output.ProductBrandTypeId);
                    if (prodQty != null)
                    {
                        output.NumberPostRemain = (prodQty.ProductQty - prodCount) + "/" + prodQty.ProductQty;
                        if (output.ProductBrandTypeId != null || output.ProductBrandTypeId != 1)
                        {
                            var paymentOrder = await _repoPaymentWrapper.PaymentUpgrade.GetCurrentOrderDetail(ProductBrandId, output.ProductBrandTypeId ?? 1);
                            if (paymentOrder != null)
                            {
                                var lastBeginDate = DateTime.Parse(paymentOrder.StartDate.ToString());
                                var lastUpgradeDate = DateTime.Parse(paymentOrder.EndDate.ToString());

                                TimeSpan diffTotalDate = lastUpgradeDate - lastBeginDate;
                                TimeSpan diffDate = DateTime.Now - lastUpgradeDate;
                                output.TimeRankBrandRemain = Math.Abs(Math.Round(diffDate.TotalDays)).ToString() + "/" + Math.Abs(Math.Round(diffTotalDate.TotalDays)).ToString();
                            }

                        }

                    }


                }

            }
            catch (Exception ex)
            {
                _logger.LogError("BrandController: GetBrandDetail: " + ex.ToString());
            }
            return output;
        }
        /// <summary>
        /// Get ProductBrand By UserId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ProductBrandSearchDetailDTO> GetBrandDetailByUserId(string userId)
        {
            var output = new ProductBrandSearchDetailDTO();
            try
            {
                var pUserId = Guid.Parse(userId);
                var result = await _repoWrapper.Brand.FirstOrDefaultAsync(p => p.CreateBy == pUserId);
                if (result != null)
                {
                    output = _mapper.Map<ProductBrandSearchDetailDTO>(result);
                    var Location = await _repoWrapper.Location.FirstOrDefaultAsync(p => p.Location_ID == output.LocationId);
                    if (Location != null)
                    {
                        output.LocationName = Location.Name;
                    }
                    var CountryName = await _repoWrapper.Country.FirstOrDefaultAsync(p => p.Country_ID == output.Country_ID);
                    if (CountryName != null)
                    {
                        output.CountryName = CountryName.Name;
                    }
                    var district = await _repoWrapper.Location.GetDistrictByName(output.District);
                    if (district != null)
                    {
                        output.DistrictId = district.Id;
                    }
                    //Get Năm tham gia
                    output.ProductBrandYearJoin = (int)(DateTime.Now.Year - result.CreateDate?.Year);

                }

            }
            catch (Exception ex)
            {
                _logger.LogError("BrandController: GetBrandDetail: " + ex.ToString());
            }
            return output;
        }
        [Authorize]
        [HttpPost]
        public async Task<SentContactBrandDTO> SentContactBrand([FromBody] SentContactToBrand model)
        {
            var MailRec = await _repoWrapper.Brand.SentContactBrand(model.BrandId, model.Name, model.Phone, model.Email, model.Content);
            //Sent Email
            Utils.Util.SendMail(model.Name, MailRec.ContactEmail, MailRec.ContactName,
                        "Thông tin từ Hanoma", MailRec.MailBody, _repoWrapper.AspNetUsers.setting());
            var result = new SentContactBrandDTO();
            result.ErrorCode = "00";
            result.Message = "Gửi email thành công!";
            return result;

        }

        
        [HttpPost]
        public async Task<ModelBaseStatus> PostReferralCode([FromBody] PostReferralCode model)
        {
            var output = new ModelBaseStatus();
            var checkRefCode = await _repoWrapper.AspNetUsers.FirstOrDefaultAsync(p => p.UserName == model.ReferralCode);
            if(checkRefCode == null)
            {
                output.ErrorCode = "01";
                output.Message = $"Mã giới thiệu {model.ReferralCode} không tồn tại trên hệ thống. Mời bạn nhập lại và tiếp tục kiểm tra";
                return output;
            }            

            var checkCodeBrand = await _repoWrapper.Brand.FirstOrDefaultAsync(p => p.ProductBrand_ID == model.ProductBrandId);
            if(checkCodeBrand !=null)
            {
                if(!String.IsNullOrEmpty(checkCodeBrand.ReferralCode))
                {
                    output.ErrorCode = "01";
                    output.Message = $"Tài khoản đã được nhập mã giới thiệu";
                    return output;
                }
            }

            var checkCodeCurrentUser = await _repoWrapper.AspNetUsers.FirstOrDefaultAsync(p => p.Id == model.UserId);
            if(checkCodeCurrentUser != null)
            {
                if(checkCodeCurrentUser.UserName == model.ReferralCode)
                {
                    output.ErrorCode = "01";
                    output.Message = $"Không thể nhập mã giới thiệu của chính tài khoản hiện tại!";
                    return output;
                }                   
            }    
            var result = await _repoWrapper.Brand.UpdateBrandReferralCode(model.ProductBrandId, model.ReferralCode);
            if(result)
            {
                output.ErrorCode = "00";
                output.Message = "Cập nhật thành công";
                return output;
            }
            else
            {
                output.ErrorCode = "01";
                output.Message = "Cập nhật thất bại";
                return output;
            }
            
        }
        [HttpGet]
        public async Task<IEnumerable<ProductCategoryDTO>> GetProductCateByBrand(int ProductBrandID)
        {
            var output = new List<ProductCategoryDTO>();
            try
            {
                var cacheKey = $"Brand_GetProductCateByBrand{ProductBrandID}";
                var redisEncode = await _distributedCache.GetStringAsync(cacheKey);
                if (redisEncode != null)
                {
                    output = JsonConvert.DeserializeObject<List<ProductCategoryDTO>>(redisEncode);
                }
                else
                {
                    var result = await _repoWrapper.Brand.GetProductCateByBrand(ProductBrandID);
                    output = _mapper.Map<List<ProductCategoryDTO>>(result);
                    await _distributedCache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(output), Utils.Util.RedisOptions());
                }
            }
            catch(Exception ex)
            {
                _logger.LogError($"GetProductCateByBrand: " + ex.ToString());
            }
            return output;
        }
        [HttpGet]
        public async Task<IEnumerable<BrandPaggingDTO>> GetlstBrandbyFiltter(ProductBrandFilter filter)
        {
            var output = new List<BrandPaggingDTO>();
            try
            {
                var cacheKey = "Brand_GetlstBrandbyFiltter";
                var redisEncode = await _distributedCache.GetStringAsync(cacheKey);
                if (redisEncode != null)
                {
                    output = JsonConvert.DeserializeObject<List<BrandPaggingDTO>>(redisEncode);
                }
                else
                {
                    var result = await _repoWrapper.Brand.GetListProductBrand(filter);
                    output = _mapper.Map<List<BrandPaggingDTO>>(result);
                    await _distributedCache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(output), Utils.Util.RedisOptions());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetlstBrandbyFiltter: " + ex.ToString());
            }

            return output;
        }
        /// <summary>
        /// List menu ProductBrand
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<MenuBrand>> GetlstMenuBrand()
        {
            List<MenuBrand> output = new List<MenuBrand>();
            try
            {
                var cacheKey = "Brand_GetlstMenuBrand";
                var redisEncode = await _distributedCache.GetStringAsync(cacheKey);
                if (redisEncode != null)
                {
                    output = JsonConvert.DeserializeObject<List<MenuBrand>>(redisEncode);
                }
                else
                {
                    output.Add(new MenuBrand { MenuName = "Thiết bị", ProductTypeId = 1 });
                    output.Add(new MenuBrand { MenuName = "Phụ tùng", ProductTypeId = 5 });
                    output.Add(new MenuBrand { MenuName = "Vật tư", ProductTypeId = 7 });
                    output.Add(new MenuBrand { MenuName = "Dịch vụ", ProductTypeId = 11 });
                    output.Add(new MenuBrand { MenuName = "Thiết bị cho thuê", ProductTypeId = 3 });
                    await _distributedCache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(output), Utils.Util.RedisOptions());
                }
            }
            catch(Exception ex)
            {
                _logger.LogError($"GetlstMenuBrand:" + ex.ToString());
            }
            return output;
        }
        /// <summary>
        /// Tạo gian hàng Model PostProductBrandDTO
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<PostProductBrandDTO> PostProductBrand(PostProductBrandDTO model)
        {
            _logger.LogDebug($"PostProductBrand: {JsonConvert.SerializeObject(model)}");
            var output = new PostProductBrandDTO();
            if (!Util.IsEmail(model.Data.Email))
            {
                output.ErrorCode = "BRAND001";
                output.Message = Utils.ConstMessage.GetMsgConst("BRAND001");
                return output;
            }
            if (!Util.IsPhoneNumber(model.Data.Mobile?.Trim()))
            {
                output.ErrorCode = "BRAND002";
                output.Message = Utils.ConstMessage.GetMsgConst("BRAND002");
                return output;
            }
            if(String.IsNullOrEmpty(model.Data.Address))
            {
                output.ErrorCode = "BRAND005";
                output.Message = Utils.ConstMessage.GetMsgConst("BRAND005");
                return output;
            }
            var UserProfilers = _repoWrapper.AspNetUserProfiles.FirstOrDefault(p => p.UserId == model.UserId);
            if (UserProfilers != null)
            {
                if (UserProfilers.ProductBrand_ID != null & UserProfilers.ProductBrand_ID > 0)
                {
                    output.ErrorCode = "BRAND003";
                    output.Message = ConstMessage.GetMsgConst("BRAND003");
                    return output;
                }
            }
            try
            {
                var ProductBrandModel = _mapper.Map<ProductBrand>(model.Data);
                var ProdBrandId = await _repoWrapper.Brand.PostProductBrand(ProductBrandModel, model.ImgLogo, model.ImgBanner, model.UserId);
                if (ProdBrandId != 0)
                {
                    // Save MainImage
                    if (!String.IsNullOrEmpty(model.ImgLogo.Base64))
                    {
                       await SaveLogoImage(model.ImgLogo, ProdBrandId);
                    }
                    //Save Sub Image
                    if (!String.IsNullOrEmpty(model.ImgBanner.Base64))
                    {
                       await SaveBannerImage(model.ImgBanner, ProdBrandId);
                    }
                    output.Data.ProductBrand_ID = ProdBrandId;
                    ////Update Image
                    //_repoWrapper.Brand.UpdateImgProductBrand(ProductBrandModel, model.ImgLogo, model.ImgBanner, ProdBrandId, model.UserId);
                    _repoWrapper.FCMMessage.PushNotificationToRabitMQ(new NotificationRabitMQModel
                    {
                        Type = "ONDEMAND",
                        NotificationCode = "DKCH",
                        ChannelSend = "ALL",
                        UsingTemplate = true,
                        UserId = model.UserId,         
                    });
                    output.ErrorCode = "00";
                    output.Message = "Tạo gian hàng thành công";
                }
                else
                {
                    output.ErrorCode = "001";
                    output.Message = Utils.ConstMessage.GetMsgConst("001");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"PostProductBrand: " + ex.ToString());
                output.ErrorCode = "001";
                output.Message = Utils.ConstMessage.GetMsgConst("001");
            }
            return output;
        }
        /// <summary>
        /// Cập nhật gian hàng Model PostProductBrandDTO
        /// </summary>
        /// <param name="ProductBrandId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut]
        public async Task<PostProductBrandDTO> PutProductBrand(int ProductBrandId, PostProductBrandDTO model)
        {
            var output = new PostProductBrandDTO();
            output = model;
            var pUserId = Guid.Parse(model.UserId);
            var prodBrand = _repoWrapper.Brand.FirstOrDefault(p => p.ProductBrand_ID == ProductBrandId && p.CreateBy == pUserId);
            if (prodBrand == null)
            {
                output.ErrorCode = "BRAND004";
                output.Message = Utils.ConstMessage.GetMsgConst("BRAND004");
                return output;
            }
            else
            {
                try
                {
                    if (!Util.IsEmail(model.Data.Email))
                    {
                        output.ErrorCode = "BRAND001";
                        output.Message = Utils.ConstMessage.GetMsgConst("BRAND001");
                        return output;
                    }
                    if (!Util.IsPhoneNumber(model.Data.Mobile?.Trim()))
                    {
                        output.ErrorCode = "BRAND002";
                        output.Message = Utils.ConstMessage.GetMsgConst("BRAND002");
                        return output;
                    }
                    if (String.IsNullOrEmpty(model.Data.Address))
                    {
                        output.ErrorCode = "BRAND005";
                        output.Message = Utils.ConstMessage.GetMsgConst("BRAND005");
                        return output;
                    }
                    var ProductBrandModel = _mapper.Map<ProductBrand>(model.Data);
                    ProductBrandModel.ProductBrand_ID = ProductBrandId;
                    var ProdBrandId = await _repoWrapper.Brand.PostProductBrand(ProductBrandModel, model.ImgLogo, model.ImgBanner, model.UserId);
                    if (ProdBrandId != 0)
                    {
                        // Save MainImage
                        if (!String.IsNullOrEmpty(model.ImgLogo.Base64))
                        {
                            SaveLogoImage(model.ImgLogo, ProdBrandId);
                        }
                        //Save Sub Image
                        if (!String.IsNullOrEmpty(model.ImgBanner.Base64))
                        {
                            SaveBannerImage(model.ImgBanner, ProdBrandId);
                        }
                        
                        output.ErrorCode = "00";
                        output.Message = "Cập nhật thành công";
                    }
                    else
                    {
                        output.ErrorCode = "001";
                        output.Message = Utils.ConstMessage.GetMsgConst("001");
                    }
                }
                catch(Exception ex)
                {
                    _logger.LogError($"PutProductBrand: " + ex.ToString());
                }

            }
            return output;
        }
        /// <summary>
        /// Delete ProductBrand for test
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>

        [HttpGet]
        public async Task<object> DeleteProductBrand(string userId)
        {
            var output = new PostProductBrandDTO();
            var pUserId = Guid.Parse(userId);
            try
            {
                var prodBrand = _repoWrapper.Brand.FirstOrDefault(p => p.CreateBy == pUserId);
                if (prodBrand == null)
                {
                    output.ErrorCode = "BRAND004";
                    output.Message = Utils.ConstMessage.GetMsgConst("BRAND004");
                    return output;
                }
                else
                {
                    var aspnetProfilers = _repoWrapper.AspNetUserProfiles.FirstOrDefaultAsync(p => p.UserId == userId);
                    if (aspnetProfilers != null)
                    {
                        try
                        {
                            _repoWrapper.AspNetUserProfiles.UpdateProductBrandToZero(userId);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError($"DeleteProductBrand : {ex.ToString()}");
                            output.Message = "Có lỗi trong quá trình xóa";
                        }
                    }
                }
                output.ErrorCode = "00";
                output.Message = "Xóa thành công";
            }
            catch(Exception ex)
            {
                _logger.LogError($"DeleteProductBrand: " + ex.ToString());
            }
            return output;

        }
        private async Task SaveLogoImage(ImageUploadDTO MainImage, int ProductBrandId)
        {
            var timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            var productBrand = _repoWrapper.Brand.FirstOrDefault(x => x.ProductBrand_ID == ProductBrandId);
            MainImage.FileName = String.Format("{0}-mobile-logo-{1}.{2}", ProductBrandId, timestamp, MainImage.ExtensionType.Replace("image/", ""));
            MainImage.PathSave = "productbrand/logo/original";            
            await UploadImage(MainImage);
            await _repoWrapper.Brand.UpdateImgLogoProductBrand(MainImage.FileName, ProductBrandId);


        }
        private async Task SaveBannerImage(ImageUploadDTO MainImage, int ProductBrandId)
        {
            var timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            var productBrand = _repoWrapper.Brand.FirstOrDefault(x => x.ProductBrand_ID == ProductBrandId);
            MainImage.FileName = String.Format("{0}-mobile-banner-{1}.{2}", ProductBrandId, timestamp, MainImage.ExtensionType.Replace("image/", ""));
            MainImage.PathSave = "productbrand/banner/original";
            await UploadImage(MainImage);
            await _repoWrapper.Brand.UpdateImgBannerProductBrand(MainImage.FileName, ProductBrandId);

        }
        private async Task UploadImage(ImageUploadDTO model)
        {
            try
            {

                var pathAbsolute =@"C:\Domains\DaNBVQ\wwwroot\data\";
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