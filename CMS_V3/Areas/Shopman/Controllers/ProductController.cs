using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using HNM.DataNC.ModelsFilter;
using HNM.DataNC.ModelsNC.ModelsDTO;
using HNM.RepositoriesWeb.RepositoriesBase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using X.PagedList;

namespace CMS_V3.Areas.Shopman.Controllers
{
    [Area("Shopman")]
    [Authorize(policy: "Authenticated")]
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IRepositoryWrapper _repoWrapper;
        private IMapper _mapper;
        private IDistributedCache _cache;
        public ProductController(ILogger<ProductController> logger, IHttpClientFactory clientFactory, IMapper mapper, IRepositoryWrapper repoWrapper, IDistributedCache cache)
        {
            _clientFactory = clientFactory;
            _logger = logger;
            _mapper = mapper;
            _repoWrapper = repoWrapper;
            _cache = cache;
        }
        
        public async Task<IActionResult> Index(int? page = 1, int? pageSize = 20)
        {
            ViewBag.ListProduct = await _repoWrapper.ShopmanProduct.GetLstProductShopMan(null, 1, 20, -1, null, null);
            ViewBag.Page = page ?? 1;
            ViewBag.PageSize = pageSize ?? 20;
            ViewBag.TotalCount = ViewBag.ListProduct.TotalRecord;
            ViewBag.TotalPages = (int)Math.Ceiling(decimal.Divide(ViewBag.ListProduct.TotalRecord, ViewBag.PageSize ?? 15));
            var productbrandInfo = await _repoWrapper.Profile.GetBrandDetailByUserId();
            var productbrand = await _repoWrapper.UpdateStore.GetBrandPackage(productbrandInfo.ProductBrandId);
            ViewBag.ProductBrandTypeId = productbrand.ProductBrandTypeId;
            return View();
        }
        
        public async Task<IActionResult> FilterProductShopmanByParam(string Keyword, int? page, int?pageSize, int?statusTypeId, int? productTypeId, int? productCategoryId)
        {
            if(Keyword == "null")
            {
                Keyword = null;
            }
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.ProductCategory_ID = productCategoryId;
            ViewBag.Keyword = Keyword;
            ViewBag.StatusTypeId = statusTypeId;
            ViewBag.ProductTypeId = productTypeId;
            var productsRes = await _repoWrapper.ShopmanProduct.GetLstProductShopMan(Keyword, page, pageSize, statusTypeId, productTypeId, productCategoryId);
            ViewBag.TotalCount = productsRes.TotalRecord;
            ViewBag.TotalPages = (int)Math.Ceiling(decimal.Divide(productsRes.TotalRecord, (decimal)pageSize));
            ViewBag.ProductPaging =
                new StaticPagedList<ProductSearchResultDTO>
                (
                    productsRes.Data, (int)page, (int)pageSize, productsRes.TotalRecord
                    );
            return PartialView("FilterProductShopman", productsRes);
        }

        public async Task<IActionResult> FilterPartial(int? ProductTypeId, int? ProductCategoryId,string Keyword)
        {
            ViewBag.ProductTypeId = ProductTypeId;
            ViewBag.ProductCategoryID = ProductCategoryId;
            ViewBag.Keyword = Keyword;

            List<KeyValuePair<int, string>> lstProductType = new List<KeyValuePair<int, string>>();
            lstProductType.Add(new KeyValuePair<int, string>(1, "Đăng tin bán thiết bị"));
            lstProductType.Add(new KeyValuePair<int, string>(3, "Đăng tin cho thuê thiết bị"));
            lstProductType.Add(new KeyValuePair<int, string>(5, "Đăng tin bán phụ tùng"));
            lstProductType.Add(new KeyValuePair<int, string>(7, "Đăng tin bán vật tư"));
            lstProductType.Add(new KeyValuePair<int, string>(11, "Đăng tin bán dịch vụ"));

            ViewBag.ListProductType = lstProductType;
            
            int parentId = 654; //Default máy để bán
            if(ProductTypeId != null)
            {
                if(ProductTypeId == 7)
                {
                    parentId = 652;
                }    
                else if(ProductTypeId == 11)
                {
                    parentId = 651;
                }    
            }    

            var resultLstProductCategory = await _repoWrapper.ProductCategory.GetLstMenuByParentId(parentId);
            ViewBag.lstProductCategory = resultLstProductCategory.Data.ToList();
            return PartialView("FilterPartial.cshtml");

        }

        //Đăng tin bán thiết bị
        public IActionResult CreateSellingEquipment()
        {
            return View();
        }

        //Đăng tin cho Thuê
        public IActionResult CreateEmployEquipment()
        {
            return View();
        }

        //Đăng tin phụ tùng
        public IActionResult CreateAccessoriesEquipment()
        {
            return View();
        }

        //Đăng tin dịch vụ
        public IActionResult CreateServiceEquipment()
        {
            return View();
        }

        //Đăng tin vật tư
        public IActionResult CreateMaterialEquipment()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] PostProductShopManDTO model)
        {
            var productbrand = await _repoWrapper.Profile.GetBrandDetailByUserId();
            var checkResult = await _repoWrapper.Product.CheckPostProduct(productbrand.ProductBrandId);
            if(checkResult.ErrorCode == "00" || checkResult.ErrorCode == "0")
            {
                if (model.typeForm == 1)
                {
                    model.Product.ProductTypeId = 1;
                }

                if (model.typeForm == 3)
                {
                    model.Product.ProductTypeId = 3;
                }

                if (model.typeForm == 5)
                {
                    model.Product.ProductTypeId = 5;
                }

                if (model.typeForm == 7)
                {
                    model.Product.ProductTypeId = 7;
                }

                if (model.typeForm == 11)
                {
                    model.Product.ProductTypeId = 11;
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

                return Json(result);
            }
            else
            {
                return Json(checkResult);
            }
            
        }

        public IActionResult ReturnResult(int? ProductId, int Type)
        {
            if(Type == 1)
            {
                ViewBag.LinkEdit = "/Shopman/Product/EditSellingEquipment?ProductId=" + ProductId;
                ViewBag.LinkContinue = "/Shopman/Product/CreateSellingEquipment";
            }

            if (Type == 3)
            {
                ViewBag.LinkEdit = "/Shopman/Product/EditEmployEquipment?ProductId=" + ProductId;
                ViewBag.LinkContinue = "/Shopman/Product/CreateEmployEquipment";
            }

            if (Type == 5)
            {
                ViewBag.LinkEdit = "/Shopman/Product/EditAccessoriesEquipment?ProductId=" + ProductId;
                ViewBag.LinkContinue = "/Shopman/Product/CreateAccessoriesEquipment";
            }

            if (Type == 7)
            {
                ViewBag.LinkEdit = "/Shopman/Product/EditMaterialEquipment?ProductId=" + ProductId;
                ViewBag.LinkContinue = "/Shopman/Product/CreateMaterialEquipment";
            }

            if (Type == 11)
            {
                ViewBag.LinkEdit = "/Shopman/Product/EditServiceEquipment?ProductId=" + ProductId;
                ViewBag.LinkContinue = "/Shopman/Product/CreateServiceEquipment";
            }
            return PartialView("ReturnResult");
        }

        public async Task<IActionResult> CheckPostProduct()
        {
            var productbrand = await _repoWrapper.Profile.GetBrandDetailByUserId();
            var checkResult = await _repoWrapper.Product.CheckPostProduct(productbrand.ProductBrandId);
            return Json(checkResult);
        }

        //Chỉnh sửa tin thiết bị
        public async Task<IActionResult> EditSellingEquipment(int ProductId)
        {
            var data = await _repoWrapper.Product.GetProductDetails(ProductId);
            var productCategory = await _repoWrapper.ProductCategory.GetDetailProductCategory((int)data.ProductDetails.ProductCategory_ID);
            var ProductCategoryParent = await _repoWrapper.ProductCategory.GetDetailProductCategory((int)productCategory.ParentId);
            data.ProductDetails.ParenfProductCategory_ID = ProductCategoryParent.ProductCategoryId;
            data.ProductDetails.ParenfProductCategoryName = ProductCategoryParent.Name;
            return View(data);
        }

        //Chỉnh sửa tin vật tư
        public async Task<IActionResult> EditMaterialEquipment(int ProductId)
        {
            var data = await _repoWrapper.Product.GetProductDetails(ProductId);
            var productCategory = await _repoWrapper.ProductCategory.GetDetailProductCategory((int)data.ProductDetails.ProductCategory_ID);
            var ProductCategoryParent = await _repoWrapper.ProductCategory.GetDetailProductCategory((int)productCategory.ParentId);
            data.ProductDetails.ParenfProductCategory_ID = ProductCategoryParent.ProductCategoryId;
            data.ProductDetails.ParenfProductCategoryName = ProductCategoryParent.Name;
            return View(data);
        }

        //Chỉnh sửa tin cho thuê thiết bị
        public async Task<IActionResult> EditEmployEquipment(int ProductId)
        {
            var data = await _repoWrapper.Product.GetProductDetails(ProductId);
            var productCategory = await _repoWrapper.ProductCategory.GetDetailProductCategory((int)data.ProductDetails.ProductCategory_ID);
            var ProductCategoryParent = await _repoWrapper.ProductCategory.GetDetailProductCategory((int)productCategory.ParentId);
            data.ProductDetails.ParenfProductCategory_ID = ProductCategoryParent.ProductCategoryId;
            data.ProductDetails.ParenfProductCategoryName = ProductCategoryParent.Name;
            return View(data);
        }

        //Chỉnh sửa tin cho thuê dịch vụ
        public async Task<IActionResult> EditServiceEquipment(int ProductId)
        {
            var data = await _repoWrapper.Product.GetProductDetails(ProductId);
            if(data.ProductDetails.RelatedCategoryId != null)
            {
                var productCategory = await _repoWrapper.ProductCategory.GetDetailProductCategory((int)data.ProductDetails.RelatedCategoryId);
                if (productCategory != null)
                {
                    var ProductCategoryParent = await _repoWrapper.ProductCategory.GetDetailProductCategory((int)productCategory.ParentId);
                    data.ProductDetails.ParenfProductCategory_ID = ProductCategoryParent.ProductCategoryId;
                    data.ProductDetails.ParenfProductCategoryName = ProductCategoryParent.Name;
                }
                else
                {
                    data.ProductDetails.ParenfProductCategory_ID = null;
                    data.ProductDetails.ParenfProductCategoryName = null;
                }
            }
            
            return View(data);
        }

        public async Task<IActionResult> EditAccessoriesEquipment(int ProductId)
        {
            var data = await _repoWrapper.Product.GetProductDetails(ProductId);
            var productCategory = await _repoWrapper.ProductCategory.GetDetailProductCategory((int)data.ProductDetails.AccessoriesCategoryId);
            var ProductCategoryParent = await _repoWrapper.ProductCategory.GetDetailProductCategory((int)productCategory.ParentId);
            data.ProductDetails.ParenfProductCategory_ID = ProductCategoryParent.ProductCategoryId;
            data.ProductDetails.ParenfProductCategoryName = ProductCategoryParent.Name;
            return View(data);
        }
        [HttpPost]
        public async Task<IActionResult> EditProduct(ProductShopManDTO ProductShopManDTO, IEnumerable<IFormFile> MainImage, string[] MainImageUpload, string[] MainImageFileName, string[] SubImageUpload, string[] SubImageFileName)
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
                    string step2 = step1.Replace(";base64", "");
                    string imgData = substrings[1];
                    obj.Base64 = imgData;
                    obj.FileName = SubImageFileName[i];
                    obj.ExtensionType = step2;
                    model.SubImage.Add(obj);
                }
            }

            model.Product = ProductShopManDTO;
            

            var result = await _repoWrapper.Product.UpdateProduct(model, (int)ProductShopManDTO.ProductId, 1);

            return Redirect("/Shopman/Product");
        }

        [HttpPost]
        public async Task<IActionResult> EditAccessoriesProduct([FromBody] PostProductShopManDTO model)
        {
            if(model != null)
            {
                if (model.MainImage.Base64 != null)
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
                model.Product.ProductTypeId = 5;
                var result = await _repoWrapper.Product.UpdateProduct(model, (int)model.Product.ProductId, 1);

                return Json(result.Message);
            }
            else
            {
                return Json("Sửa thất bại");
            }
        }

        public IActionResult ReferAccessories()
        {
            return PartialView("ReferAccessories");
        }

        public async Task<JsonResult> GetLstProductCateLv1(string text)
        {
            var result = await _repoWrapper.Product.GetProdCateByParentID(text, 654);
            return Json(result.Data);
        }

        //Filter DS CateID cấp 2
        public async Task<JsonResult> GetLstProductCateLv2(string text, int? ProductCategoryId)
        {
            var result = await _repoWrapper.Product.GetProdCateByParentID(text, ProductCategoryId);
            return Json(result.Data);
        }

        //Chọn hãng sản xuất
        public async Task<JsonResult> GetLstProductCateLv3(string text, int? ProductCategoryId)
        {
            var result = await _repoWrapper.Product.GetProdCateByParentID(text, ProductCategoryId);
            return Json(result.Data);
        }

        public async Task<JsonResult> GetAllUnit()
        {
            var result = await _repoWrapper.Product.GetAllUnit();
            return Json(result);
        }
        
        public async Task<JsonResult> GetAllUnitService()
        {
            var result = await _repoWrapper.Product.GetAllUnitService();
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
        public async Task<JsonResult> GetAllLocation(string text)
        {
            var result = await _repoWrapper.Profile.GetAllLocation(text);
            return Json(result.Data);
        }

        public async Task<JsonResult> GetAllCountry()
        {
            var result = await _repoWrapper.Profile.GetLstCountry(null);
            return Json(result.data);
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

        public async Task<JsonResult> GetAllModelByCate(int ManufactoryId, int ProductCategoryId,string text)
        {
            var lstOutput = new List<ProductModel_SearchByCategory_Result_DTO>();
            var result = await _repoWrapper.Service.GetListModelByManu(ManufactoryId, ProductCategoryId,text);
            if (result != null)
            {
                lstOutput = (List<ProductModel_SearchByCategory_Result_DTO>)result.Data;
                ProductModel_SearchByCategory_Result_DTO item = new ProductModel_SearchByCategory_Result_DTO();
                item.Name = "Khác";
                item.ProductModelId = 0;
                lstOutput.Insert(0, item);
            }
            return Json(lstOutput);
        }

        [HttpPost]
        public async Task<JsonResult> CreateModel(ProductModelDTO productModelDTO)
        {
            var result = await _repoWrapper.Product.CreateModel(productModelDTO);
            return Json(result);
        }
    }

}
