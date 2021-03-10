using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;
using AutoMapper;
using HNM.CommonNC;
using HNM.DataNC.ModelsFilter;
using HNM.DataNC.ModelsNC.ModelsDTO;
using HNM.RepositoriesWeb.RepositoriesBase;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CMS_V3.Controllers
{
    public class ManagerDemandController : Controller
    {
        private readonly ILogger<ManagerDemandController> _logger;
        private readonly IRepositoryWrapper _repoWrapper;
        private IMapper _mapper;


        public ManagerDemandController(ILogger<ManagerDemandController> logger, IMapper mapper, IRepositoryWrapper repoWrapper)
        {
            _logger = logger;
            _mapper = mapper;
            _repoWrapper = repoWrapper;
        }

        async Task<(ListProductCategoryDTO MenuMachine, ListProductCategoryDTO MenuMaterials, ListProductCategoryDTO MenuServices)> GetCategoryMenu()
        {
            var MenuMachine = await _repoWrapper.ProductCategory.GetLstMenuByParentId(654);
            var MenuMaterials = await _repoWrapper.ProductCategory.GetLstMenuByParentId(652);
            var MenuServices = await _repoWrapper.ProductCategory.GetLstMenuByParentId(651);
            return (MenuMachine, MenuMaterials, MenuServices);
        }
        public IActionResult Index()
        {
            return View();
        }

        //Cần mua vật tư
        public IActionResult NeedBuyMaterial()
        {
            return PartialView("NeedBuyMaterial");
        }

        public IActionResult ReferAccessories()
        {
            return PartialView("ReferAccessories");
        }

        //Cần mua phụ tùng
        public IActionResult NeedBuyAccessories()
        {
            return PartialView("NeedBuyAccessories");
        }
        
        //Cần tuyển dụng
        public IActionResult RecruitmentDemand()
        {
            return PartialView("RecruitmentDemand");
        }

        //Cần tìm việc
        public async Task<IActionResult> CVDemand()
        {
            ViewBag.AllLocation = await _repoWrapper.Profile.GetAllLocation2();
            return PartialView("CVDemand");
        }

        [HttpPost]
        public async Task<IActionResult> NeedBuyAccessories([FromBody] PostProductShopManDTO model)
        {
            model.Product.ProductTypeId = 6;
            //MainImage
            String[] substrings = model.MainImage.Base64.Split(',');
            model.MainImage.Base64 = substrings[1];

            for (int i = 0; i < model.SubImage.Count; i++)
            {
                if (model.SubImage.Count > 0)
                {
                    ImageUploadDTO obj = new ImageUploadDTO();
                    String[] substrings2 = model.SubImage[i].Base64.Split(',');
                    model.SubImage[i].Base64 = substrings2[1];
                }
            }

            var result = await _repoWrapper.Product.CreateProduct(model);

            return Json(result);
        }

        //Cần mua thiết bị
        public IActionResult NeedBuyDemand()
        {
            return PartialView("NeedBuyDemand");
        }

        [HttpPost]
        public async Task<IActionResult> CreateDemand([FromBody] PostProductShopManDTO model)
        {
            if(model.typeForm == 2)
            {
                model.Product.ProductTypeId = 2;
            }

            if (model.typeForm == 4)
            {
                model.Product.ProductTypeId = 4;
            }

            if (model.typeForm == 6)
            {
                model.Product.ProductTypeId = 6;
            }

            if (model.typeForm == 8)
            {
                model.Product.ProductTypeId = 8;
            }

            if (model.typeForm == 12)
            {
                model.Product.ProductTypeId = 12;
            }

            //MainImage
            if (model.MainImage != null)
            {
                String[] substrings = model.MainImage.Base64.Split(',');
                model.MainImage.Base64 = substrings[1];
            }

            //Subimage
            for (int i = 0; i < model.SubImageUpload.Length; i++)
            {
                if (model.SubImageUpload.Length > 0)
                {
                    ImageUploadDTO obj = new ImageUploadDTO();
                    String[] substrings = model.SubImageUpload[i].Split(',');
                    string header = substrings[0];
                    string step1 = header.Replace("data:", "");
                    string step2 = step1.Replace(";base64", "");
                    string imgData = substrings[1];
                    obj.Base64 = imgData;
                    obj.FileName = model.SubImageFileName[i];
                    obj.ExtensionType = step2;
                    model.SubImage.Add(obj);
                }
            }

            var result = await _repoWrapper.Product.CreateProduct(model);

            return Json(result.Message);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRecruitment([FromBody]SumPostRecDTO model)
        {
            //MainImage
            if (model.MainImage != null)
            {
                String[] substrings = model.MainImage.Base64.Split(',');
                model.MainImage.Base64 = substrings[1];
            }

            //Subimage
            for (int i = 0; i < model.SubImageUpload.Length; i++)
            {
                if (model.SubImageUpload.Length > 0)
                {
                    ImageUploadDTO obj = new ImageUploadDTO();
                    String[] substrings = model.SubImageUpload[i].Split(',');
                    string header = substrings[0];
                    string step1 = header.Replace("data:", "");
                    string step2 = step1.Replace(";base64", "");
                    string imgData = substrings[1];
                    obj.Base64 = imgData;
                    obj.FileName = model.SubImageFileName[i];
                    obj.ExtensionType = step2;
                    model.SubImage.Add(obj);
                }
            }

            var result = await _repoWrapper.Product.CreateRecruitment(model);

            return Json(result.Message);
        }

        public async Task<IActionResult> EditRecruitment(int RecruitmentId)
        {
            var data = await _repoWrapper.Product.GetRecruitmentDetails(RecruitmentId);
            data.RecDetail.Description = HttpUtility.HtmlDecode(data.RecDetail.Description);
            data.RecDetail.CompanyBusiness = HttpUtility.HtmlDecode(data.RecDetail.CompanyBusiness);
            return PartialView("EditRecruitment", data);
        }

        [HttpPost]
        public async Task<IActionResult> EditRecruitment([FromBody] SumPostRecDTO model)
        {
            //MainImage
            if (model.MainImage != null)
            {
                String[] substrings = model.MainImage.Base64.Split(',');
                model.MainImage.Base64 = substrings[1];
            }


            for (int i = 0; i < model.SubImageUpload.Length; i++)
            {
                if (model.SubImageUpload.Length > 0)
                {
                    ImageUploadDTO obj = new ImageUploadDTO();
                    String[] substrings = model.SubImageUpload[i].Split(',');
                    string header = substrings[0];
                    string step1 = header.Replace("data:", "");
                    string step2 = step1.Replace(";base64", "");
                    string imgData = substrings[1];
                    obj.Base64 = imgData;
                    obj.FileName = model.SubImageFileName[i];
                    obj.ExtensionType = step2;
                    model.SubImage.Add(obj);
                }
            }

            var result = await _repoWrapper.Product.UpdateRecruitment(model, (int)model.Rec.RecruitmentId, 1);

            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCV([FromBody] SumPostCurriculumnViateDTO model)
        {
            //MainImage
            if (model.MainImage != null)
            {
                String[] substrings = model.MainImage.Base64.Split(',');
                model.MainImage.Base64 = substrings[1];
            }

            //Subimage
            for (int i = 0; i < model.SubImageUpload.Length; i++)
            {
                if (model.SubImageUpload.Length > 0)
                {
                    ImageUploadDTO obj = new ImageUploadDTO();
                    String[] substrings = model.SubImageUpload[i].Split(',');
                    string header = substrings[0];
                    string step1 = header.Replace("data:", "");
                    string step2 = step1.Replace(";base64", "");
                    string imgData = substrings[1];
                    obj.Base64 = imgData;
                    obj.FileName = model.SubImageFileName[i];
                    obj.ExtensionType = step2;
                    model.SubImage.Add(obj);
                }
            }

            var result = await _repoWrapper.Product.CreateCV(model);

            return Json(result.Message);
        }

        public async Task<IActionResult> EditCV(int CVId)
        {
            var data = await _repoWrapper.CurriculumVitae.GetDetailsCV(CVId);
            data.CVDetails.IntroInfomation = HttpUtility.HtmlDecode(data.CVDetails.IntroInfomation);
            return PartialView("EditCV", data);
        }

        [HttpPost]
        public async Task<IActionResult> EditCV([FromBody] SumPostCurriculumnViateDTO model)
        {
            //MainImage
            if (model.MainImage != null)
            {
                String[] substrings = model.MainImage.Base64.Split(',');
                model.MainImage.Base64 = substrings[1];
            }


            for (int i = 0; i < model.SubImageUpload.Length; i++)
            {
                if (model.SubImageUpload.Length > 0)
                {
                    ImageUploadDTO obj = new ImageUploadDTO();
                    String[] substrings = model.SubImageUpload[i].Split(',');
                    string header = substrings[0];
                    string step1 = header.Replace("data:", "");
                    string step2 = step1.Replace(";base64", "");
                    string imgData = substrings[1];
                    obj.Base64 = imgData;
                    obj.FileName = model.SubImageFileName[i];
                    obj.ExtensionType = step2;
                    model.SubImage.Add(obj);
                }
            }

            var result = await _repoWrapper.Product.UpdateCV(model, (int)model.CV.Id, 1);

            return Json(result);
        }



        //Cần thuê thiết bị
        public IActionResult NeedEmployDemand()
        {
            return PartialView("NeedEmployDemand");
        }

        //Cần thuê dịch vụ
        public IActionResult NeedEmployService()
        {
            return PartialView("NeedEmployService");
        }

        public async Task<IActionResult> EditDemand(int ProductId)
        {
            var data = await _repoWrapper.Product.GetProductDetails(ProductId);
            var productCategory = await _repoWrapper.ProductCategory.GetDetailProductCategory((int)data.ProductDetails.ProductCategory_ID);
            var ProductCategoryParent = await _repoWrapper.ProductCategory.GetDetailProductCategory((int)productCategory.ParentId);
            data.ProductDetails.ParenfProductCategory_ID = ProductCategoryParent.ProductCategoryId;
            data.ProductDetails.ParenfProductCategoryName = ProductCategoryParent.Name;
            return PartialView("EditDemand", data);
        }
        
        public async Task<IActionResult> EditMaterial(int ProductId)
        {
            var data = await _repoWrapper.Product.GetProductDetails(ProductId);
            var productCategory = await _repoWrapper.ProductCategory.GetDetailProductCategory((int)data.ProductDetails.ProductCategory_ID);
            var ProductCategoryParent = await _repoWrapper.ProductCategory.GetDetailProductCategory((int)productCategory.ParentId);
            data.ProductDetails.ParenfProductCategory_ID = ProductCategoryParent.ProductCategoryId;
            data.ProductDetails.ParenfProductCategoryName = ProductCategoryParent.Name;
            return PartialView("EditMaterial", data);
        }

        public async Task<IActionResult> EditEmployDemand(int ProductId)
        {
            var data = await _repoWrapper.Product.GetProductDetails(ProductId);
            var productCategory = await _repoWrapper.ProductCategory.GetDetailProductCategory((int)data.ProductDetails.ProductCategory_ID);
            var ProductCategoryParent = await _repoWrapper.ProductCategory.GetDetailProductCategory((int)productCategory.ParentId);
            data.ProductDetails.ParenfProductCategory_ID = ProductCategoryParent.ProductCategoryId;
            data.ProductDetails.ParenfProductCategoryName = ProductCategoryParent.Name;
            return PartialView("EditEmployDemand", data);
        }
        
        public async Task<IActionResult> EditEmployService(int ProductId)
        {
            var data = await _repoWrapper.Product.GetProductDetails(ProductId);
            var productCategory = await _repoWrapper.ProductCategory.GetDetailProductCategory((int)data.ProductDetails.RelatedCategoryId);
            var ProductCategoryParent = await _repoWrapper.ProductCategory.GetDetailProductCategory((int)productCategory.ParentId);
            data.ProductDetails.ParenfProductCategory_ID = ProductCategoryParent.ProductCategoryId;
            data.ProductDetails.ParenfProductCategoryName = ProductCategoryParent.Name;
            return PartialView("EditEmployService", data);
        }

        public async Task<IActionResult> EditAccessories(int ProductId)
        {
            var data = await _repoWrapper.Product.GetProductDetails(ProductId);
            var productCategory = await _repoWrapper.ProductCategory.GetDetailProductCategory((int)data.ProductDetails.AccessoriesCategoryId);
            var ProductCategoryParent = await _repoWrapper.ProductCategory.GetDetailProductCategory((int)productCategory.ParentId);
            data.ProductDetails.ParenfProductCategory_ID = ProductCategoryParent.ProductCategoryId;
            data.ProductDetails.ParenfProductCategoryName = ProductCategoryParent.Name;
            return PartialView("EditAccessories", data);
        }

        [HttpPost]
        public async Task<IActionResult> EditDemand(ProductShopManDTO ProductShopManDTO, IEnumerable<IFormFile> MainImage, string [] MainImageUpload, string[] MainImageFileName,  string [] SubImageUpload, string[] SubImageFileName)
        {
            PostProductShopManDTO model = new PostProductShopManDTO();
            //logo
            foreach (var file in MainImage)
            {
                if (file.Length > 0)
                {
                    using (var ms = new System.IO.MemoryStream())
                    {
                        file.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        string s = Convert.ToBase64String(fileBytes);
                        model.MainImage.Base64 = s;
                        model.MainImage.FileName = file.FileName;
                        model.MainImage.ExtensionType = file.ContentType;
                    }
                }
            }

            //SubImage
            for (int i = 0; i < SubImageUpload.Length; i++)
            {
                if (SubImageUpload.Length > 0)
                {
                    ImageUploadDTO obj = new ImageUploadDTO();
                    String[] substrings = SubImageUpload[i].Split(',');
                    string header = substrings[0];
                    string step1 = header.Replace("data:", "");
                    string step2 = step1.Replace(";base64","");
                    string imgData = substrings[1];
                    obj.Base64 = imgData;
                    obj.FileName = SubImageFileName[i];
                    obj.ExtensionType = step2;
                    model.SubImage.Add(obj);
                }
            }

            model.Product = ProductShopManDTO;

            var result = await _repoWrapper.Product.UpdateProduct(model, (int)ProductShopManDTO.ProductId, 1);

            return Redirect("/tai-khoan?id=2");
        }

        [HttpPost]
        public async Task<IActionResult> EditAccessories([FromBody] PostProductShopManDTO model)
        {
            model.Product.ProductTypeId = 6;
            //MainImage
            if (model.MainImage != null)
            {
                String[] substrings = model.MainImage.Base64.Split(',');
                model.MainImage.Base64 = substrings[1];
            }


            for (int i = 0; i < model.SubImageUpload.Length; i++)
            {
                if (model.SubImageUpload.Length > 0)
                {
                    ImageUploadDTO obj = new ImageUploadDTO();
                    String[] substrings = model.SubImageUpload[i].Split(',');
                    string header = substrings[0];
                    string step1 = header.Replace("data:", "");
                    string step2 = step1.Replace(";base64", "");
                    string imgData = substrings[1];
                    obj.Base64 = imgData;
                    obj.FileName = model.SubImageFileName[i];
                    obj.ExtensionType = step2;
                    model.SubImage.Add(obj);
                }
            }

            var result = await _repoWrapper.Product.UpdateProduct(model, (int)model.Product.ProductId, 1);

            return Json(result);
        }

        public IActionResult ShareLibrary()
        {
            return PartialView("ShareLibrary");
        }

        [HttpPost]
        public async Task<IActionResult> ShareLibrary(PostLibrary postLibrary, IEnumerable<IFormFile> MainImage)
        {
            PostLibraryDTO model = new PostLibraryDTO();

            foreach (var file in MainImage)
            {
                if (file.Length > 0)
                {
                    using (var ms = new System.IO.MemoryStream())
                    {
                        file.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        string s = Convert.ToBase64String(fileBytes);
                        model.ImgLogo.Base64 = s;
                        model.ImgLogo.FileName = file.FileName;
                        model.ImgLogo.ExtensionType = file.ContentType;
                    }
                }
            }

            postLibrary.LastEditDate = DateTime.Now;
            model.Data = postLibrary;

            var result = await _repoWrapper.Library.CreateLibrary(model);
            return Redirect("/tai-khoan?id=2");
        }

        public async Task<IActionResult> DemandDetail()
        {
            ViewBag.CategoryMenuHeader = await GetCategoryMenu();

            var url = RouteData.Values["url"];
            int productId = Utils.RegexRouteIdFromUrl(url.ToString());
            ViewBag.MetaTags = await _repoWrapper.Setting.GetSetting();
            var productTypeId = 2;

            ProductDetailsDTO model = await _repoWrapper.Service.GetProductDetails(productId);

            ViewBag.ListProductAds = await _repoWrapper.Product.GetSponsorBannerProduct(model.ProductDetails.ProductCategory_ID ?? 654, productTypeId, 5);
            ViewBag.ListProductByCate = await _repoWrapper.Product.GetListProductPagging(new ProductFilter { Page = 1, PageSize = 10, StatusTypeId = 4, ProductCategoryId = model.ProductDetails.ProductCategory_ID ?? 654, ProductTypeId = productTypeId });
            ViewBag.ListProductByProductBrand = await _repoWrapper.Product.GetListProductPagging(new ProductFilter { Page = 1, PageSize = 10, StatusTypeId = 4, ProductBrandId = model.ProductDetails.ProductBrand_ID, ProductTypeId = productTypeId });

            ViewBag.ProductCategoryCurrent = await _repoWrapper.ProductCategory.GetDetailProductCategory((int)model.ProductDetails.ProductCategory_ID);
            var profileBuyer = await _repoWrapper.Profile.ProfilersByUserId(model.ProductDetails.CreateBy);
            ViewBag.Buyer = profileBuyer.FullName;
            ViewBag.Phone = profileBuyer.Phone;
            return View(model);
        }

        public async Task<IActionResult> MaterialDetail()
        {
            ViewBag.CategoryMenuHeader = await GetCategoryMenu();

            var url = RouteData.Values["url"];
            int productId = Utils.RegexRouteIdFromUrl(url.ToString());
            ViewBag.MetaTags = await _repoWrapper.Setting.GetSetting();
            var productTypeId = 8;

            ProductDetailsDTO model = await _repoWrapper.Service.GetProductDetails(productId);

            ViewBag.ListProductAds = await _repoWrapper.Product.GetSponsorBannerProduct(model.ProductDetails.ProductCategory_ID ?? 654, productTypeId, 5);
            ViewBag.ListProductByCate = await _repoWrapper.Product.GetListProductPagging(new ProductFilter { Page = 1, PageSize = 10, StatusTypeId = 4, ProductCategoryId = model.ProductDetails.ProductCategory_ID ?? 654, ProductTypeId = productTypeId });
            ViewBag.ListProductByProductBrand = await _repoWrapper.Product.GetListProductPagging(new ProductFilter { Page = 1, PageSize = 10, StatusTypeId = 4, ProductBrandId = model.ProductDetails.ProductBrand_ID, ProductTypeId = productTypeId });

            ViewBag.ProductCategoryCurrent = await _repoWrapper.ProductCategory.GetDetailProductCategory((int)model.ProductDetails.ProductCategory_ID);
            var profileBuyer = await _repoWrapper.Profile.ProfilersByUserId(model.ProductDetails.CreateBy);
            ViewBag.Buyer = profileBuyer.FullName;
            ViewBag.Phone = profileBuyer.Phone;
            return View(model);
        }

        public async Task<IActionResult> AccessoriDetail()
        {
            ViewBag.CategoryMenuHeader = await GetCategoryMenu();

            var url = RouteData.Values["url"];
            int productId = Utils.RegexRouteIdFromUrl(url.ToString());
            ViewBag.MetaTags = await _repoWrapper.Setting.GetSetting();
            var productTypeId = 6;

            ProductDetailsDTO model = await _repoWrapper.Service.GetProductDetails(productId);

            ViewBag.ListProductAds = await _repoWrapper.Product.GetSponsorBannerProduct(model.ProductDetails.ProductCategory_ID ?? 654, productTypeId, 5);
            ViewBag.ListProductByCate = await _repoWrapper.Product.GetListProductPagging(new ProductFilter { Page = 1, PageSize = 10, StatusTypeId = 4, ProductCategoryId = model.ProductDetails.ProductCategory_ID ?? 654, ProductTypeId = productTypeId });
            ViewBag.ListProductByProductBrand = await _repoWrapper.Product.GetListProductPagging(new ProductFilter { Page = 1, PageSize = 10, StatusTypeId = 4, ProductBrandId = model.ProductDetails.ProductBrand_ID, ProductTypeId = productTypeId });

            ViewBag.ProductCategoryCurrent = await _repoWrapper.ProductCategory.GetDetailProductCategory((int)model.ProductDetails.ProductCategory_ID);
            var profileBuyer = await _repoWrapper.Profile.ProfilersByUserId(model.ProductDetails.CreateBy);
            ViewBag.Buyer = profileBuyer.FullName;
            return View(model);
        }

        public async Task<IActionResult> RentDetail()
        {
            ViewBag.CategoryMenuHeader = await GetCategoryMenu();

            var url = RouteData.Values["url"];
            int productId = Utils.RegexRouteIdFromUrl(url.ToString());
            ViewBag.MetaTags = await _repoWrapper.Setting.GetSetting();
            var productTypeId = 4;

            ProductDetailsDTO model = await _repoWrapper.Service.GetProductDetails(productId);

            ViewBag.ListProductAds = await _repoWrapper.Product.GetSponsorBannerProduct(model.ProductDetails.ProductCategory_ID ?? 654, productTypeId, 5);
            ViewBag.ListProductByCate = await _repoWrapper.Product.GetListProductPagging(new ProductFilter { Page = 1, PageSize = 10, StatusTypeId = 4, ProductCategoryId = model.ProductDetails.ProductCategory_ID ?? 654, ProductTypeId = productTypeId });
            ViewBag.ListProductByProductBrand = await _repoWrapper.Product.GetListProductPagging(new ProductFilter { Page = 1, PageSize = 10, StatusTypeId = 4, ProductBrandId = model.ProductDetails.ProductBrand_ID, ProductTypeId = productTypeId });

            ViewBag.ProductCategoryCurrent = await _repoWrapper.ProductCategory.GetDetailProductCategory((int)model.ProductDetails.ProductCategory_ID);
            var profileBuyer = await _repoWrapper.Profile.ProfilersByUserId(model.ProductDetails.CreateBy);
            ViewBag.Buyer = profileBuyer.FullName;
            ViewBag.Phone = profileBuyer.Phone;
            return View(model);
        }

        public async Task<IActionResult> EmployDetail()
        {
            var url = RouteData.Values["url"];
            int Service_ID = Utils.RegexRouteIdFromUrl(url.ToString());
            if (Service_ID == 0) // Error Redirect 301
            {
                //Response.StatusCode = 301;
                //Response.Headers("Location", "https://daninhbinhvinhquang.vn/may-de-ban");
                //Response.End();
            }
            ViewBag.MenuMachine = await _repoWrapper.ProductCategory.GetLstMenuByParentId(654);
            ViewBag.MenuMaterials = await _repoWrapper.ProductCategory.GetLstMenuByParentId(652);
            ViewBag.MenuServices = await _repoWrapper.ProductCategory.GetLstMenuByParentId(651);
            ViewBag.ListCategory = await _repoWrapper.Service.GetProdCateByParentID(651);

            ViewBag.MetaTags = await _repoWrapper.Setting.GetSetting();

            //Lấy ra ds dịch vụ được tài trợ và banner

            ProductDetailsDTO model = await _repoWrapper.Service.GetProductDetails(Service_ID);
            if (model.ErrorCode == "00" || model.ErrorCode == "0")
            {
                ViewBag.ListProductAds = await _repoWrapper.Service.GetSponsorBannerProduct((int)model.ProductDetails.ProductCategory_ID, 12, 5);
                ViewBag.ListProductByCate = await _repoWrapper.Service.GetListServiceByCate(1, 10, 4, 12, (int)model.ProductDetails.ProductCategory_ID, null, null);
                ViewBag.ListProductByProductBrand = await _repoWrapper.Service.GetListServiceByProductBrand(1, 10, 4, 12, (int)model.ProductDetails.ProductBrand_ID);
                var productCategoryDetail = await _repoWrapper.ProductCategory.GetDetailProductCategory((int)model.ProductDetails.ProductCategory_ID);
                ViewBag.ProductCateName = productCategoryDetail.Name;
                ViewBag.ProductCateUrl = productCategoryDetail.URL;
                return View(model);
            }
            else
            {
                return Redirect("/Error/404");
            }
        }

        public async Task<JsonResult> ActionInCV(int?Id, string ActionString)
        {
            var result = await _repoWrapper.Product.ActionInCV(Id, ActionString);
            return Json(result);
        }

        public async Task<JsonResult> ActionInRec(int? Id, string ActionString)
        {
            var result = await _repoWrapper.Product.ActionInRec(Id, ActionString);
            return Json(result);
        }

        public async Task<JsonResult> GetAllUnit()
        {
            var result = await _repoWrapper.Product.GetAllUnit();
            return Json(result);
        }
        
        public async Task<JsonResult> GetAllCareerCategory()
        {
            var result = await _repoWrapper.Product.GetAllCareerCategory();
            return Json(result);
        }

        //Lấy ra ds chuyên mục vật tư c1
        public async Task<JsonResult> GetAllMarterialCate()
        {
            var result = await _repoWrapper.Service.GetProdCateByParentID(652);
            return Json(result.Data);
        }

        //Lấy ra ds chuyên mục vật tư c2
        public async Task<JsonResult> GetAllMarterialCateLv2(int ProductCategoryId)
        {
            var result = await _repoWrapper.Service.GetProdCateByParentID(ProductCategoryId);
            return Json(result.Data);
        }

        //Lấy ra ds chuyên mục vật tư c1
        public async Task<JsonResult> GetAllAccessoriCate()
        {
            var result = await _repoWrapper.Service.GetProdCateByParentID(653);
            return Json(result.Data);
        }

        //Lấy ra ds chuyên mục vật tư c2
        public async Task<JsonResult> GetAllAccessoriLv2(int ProductCategoryId)
        {
            var result = await _repoWrapper.Service.GetProdCateByParentID(ProductCategoryId);
            return Json(result.Data);
        }

        public async Task<JsonResult> GetAllLibCate()
        {
            var result = await _repoWrapper.Video.GetLstVideoCate();
            return Json(result);
        }

        public async Task<JsonResult> GetAllManuByCate(string text)
        {
            var result = await _repoWrapper.Service.GetListManuByCate(text);
            return Json(result);
        }
        
        public async Task<JsonResult> GetAllServiceCate()
        {
            var result = await _repoWrapper.Service.GetProdCateByParentID(651);
            return Json(result.Data);
        }

        public async Task<JsonResult> GetDetailProduct(int ProductId)
        {
            var result = await _repoWrapper.Service.GetProductDetails(ProductId);
            return Json(result.AccessoriesFit);
        }

        public async Task<JsonResult> GetAllModelByCate(int ManufactoryId, int ProductCategoryId, string text = "")
        {
            var lstOutput = new List<ProductModel_SearchByCategory_Result_DTO>();
            var result = await _repoWrapper.Service.GetListModelByManu(ManufactoryId, ProductCategoryId,text);
            if (result != null)
            {
                lstOutput = result.Data.ToList();
                ProductModel_SearchByCategory_Result_DTO item = new ProductModel_SearchByCategory_Result_DTO();
                item.Name = "Khác";
                item.ProductModelId = 0;
                lstOutput.Insert(0,item);
            }
            return Json(lstOutput);
        }
        
        [HttpPost]
        public async Task<JsonResult> CreateModel(ProductModelDTO productModelDTO)
        {
            var result = await _repoWrapper.Product.CreateModel(productModelDTO);
            return Json(result.ProductModelID);
        }
    }
}
