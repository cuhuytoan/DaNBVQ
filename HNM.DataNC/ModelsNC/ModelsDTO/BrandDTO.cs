using HNM.DataNC.ModelsNC.ModelsUtility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace HNM.DataNC.ModelsNC.ModelsDTO
{
    public class BrandDTO
    {
        public int ProductBrandId { get; set; }
        public Nullable<int> LocationId { get; set; }
        public string Name { get; set; }
        public string BrandName { get; set; }
        public string Address { get; set; }
        public string Image { get; set; }
        public string Banner { get; set; }
        public string UrlBannerImage => Utils.CloudPath() + $"/productbrand/banner/original/{Banner}";
        public string Logo { get; set; }
        public string UrlLogoImage => Utils.CloudPath() + $"/productbrand/logo/thumb/{Logo}";
        public Nullable<bool> Verified { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> LastEditDate { get; set; }
        public string Description { get; set; }
        public int ProductBrandTypeID { get; set; }        
        public int ProductBrandYearJoin { get; set; }
        public string CreateBy { get; set; }
        public string URL { get; set; }
        public int? ViewCount { get; set; }
        public string LocationName { get; set; }
    }
    public class TopBrandDTO : ModelBaseStatus
    {
        public TopBrandDTO()
        {
            Data = new List<BrandDTO>();
        }
        public List<BrandDTO> Data { get; set; }
    }
    public class SentContactBrandDTO : ModelBase
    {

    }
    public class ProductBrandSearchDTO
    {
        public int ProductBrandId { get; set; }
        public int ProductBrandTypeId { get; set; }
        public Nullable<int> LocationId { get; set; }
        public string LocationName { get; set; }
        public string Name { get; set; }
        public string BrandName { get; set; }
        public string Address { get; set; }
        public string Image { get; set; }
        public string Banner { get; set; }
        public string UrlBannerImage => Utils.CloudPath() + $"/productbrand/banner/original/{Banner}";
        public string Logo { get; set; }
        public string UrlLogoImage => Utils.CloudPath() + $"/productbrand/logo/thumb/{Logo}";
        public string Description { get; set; }
        public Nullable<bool> Verified { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> LastEditDate { get; set; }
        public string URL { get; set; }
    }
    public class ProductBrandSearchDetailDTO
    {
        public int ProductBrandId { get; set; }
        public Nullable<int> LocationId { get; set; }
        public string LocationName { get; set; }
        public string Name { get; set; }
        public string BrandName { get; set; }
        public string Address { get; set; }
        public string Image { get; set; }
        public string Banner { get; set; }
        public string UrlBannerImage => Utils.CloudPath() + $"/productbrand/banner/original/{Banner}";
        public string Logo { get; set; }
        public string Description { get; set; }
        public string UrlLogoImage => Utils.CloudPath() + $"/productbrand/logo/thumb/{Logo}";
        public string District { get; set; }
        public Nullable<int> DistrictId { get; set; }
        public string PostalCode { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public bool EmailDisplay { get; set; }
        public string Website { get; set; }
        public string ReferralCode { get; set; }
        public Nullable<int> Country_ID { get; set; }
        public string CountryName { get; set; }
        public Nullable<bool> Verified { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> LastEditDate { get; set; }
        public Nullable<int> ProductBrandTypeId { get; set; }         
        public string NumberPostRemain { get; set; }
        public string TimeRankBrandRemain { get; set; }
        public int ProductBrandYearJoin { get; set; }
        public string CreateBy { get; set; }
    }
    public class BrandPaggingDTO : ModelBaseStatus
    {
        public int TotalRecord { get; set; }
        public int TotalPage { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public IEnumerable<ProductBrandSearchDTO> Data { get; set; }
        public BrandPaggingDTO()
        {
            Data = new List<ProductBrandSearchDTO>();
        }

    }
    public class MenuBrand
    {
        public string MenuName { get; set; }
        public int? ProductTypeId { get; set; }
    }

    public class PostProductBrandDTO : ModelBase
    {
        public PostProductBrandDTO()
        {
            Data = new PostProductBrand();
            ImgLogo = new ImageUploadDTO();
            ImgBanner = new ImageUploadDTO();
        }
        public PostProductBrand Data { get; set; }
        public ImageUploadDTO ImgLogo { get; set; }
        public ImageUploadDTO ImgBanner { get; set; }
    }
    public class PostProductBrand
    {
        public int ProductBrand_ID { get; set; }
        [Required(ErrorMessage = "Lựa chọn quốc gia")]
        public Nullable<int> Country_ID { get; set; }
        [Required(ErrorMessage = "Lựa chọn tỉnh thành")]
        public Nullable<int> Location_ID { get; set; }
        [Required(ErrorMessage = "Nhập tên cửa hàng")]
        public string Name { get; set; }      
        public string Address { get; set; }
        public string District { get; set; }
        public string PostalCode { get; set; }
        [Required(ErrorMessage = "Nhập số điên thoại")]
        public string Mobile { get; set; }
        [Required(ErrorMessage = "Nhập email")]
        public string Email { get; set; }
        public bool EmailDisplay { get; set; }
        public string Website { get; set; }
        [Required(ErrorMessage = "Nhập mô tả cửa hàng")]
        public string Description { get; set; }
        public string ReferralCode { get; set; }
    }
    public class SentContactToBrand
    {
        public int BrandId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Content { get; set; }        
    }
    public class PostReferralCode:ModelBase
    {
        public int ProductBrandId { get; set; }
        public string ReferralCode { get; set; }
    }
}
