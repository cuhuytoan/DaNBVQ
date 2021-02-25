using HNM.DataNC.Models;
using HNM.DataNC.ModelsNC.ModelsUtility;
using HNM.DataNC.ModelsStore;
using System;
using System.Collections.Generic;

namespace HNM.DataNC.ModelsNC.ModelsDTO
{
    public class ProductDTO
    {
        public int Product_ID { get; set; }
        public int? ProductCategory_ID { get; set; }
        public int? ParenfProductCategory_ID { get; set; }
        public string ParenfProductCategoryName { get; set; }
        public int? ProductCategoryMachine_ID { get; set; }
        public int? ProductType_ID { get; set; }
        public int? ProductBrand_ID { get; set; }
        public int? ProductModel_ID { get; set; }
        public int? ProductManufacture_ID { get; set; }
        public int? Country_ID { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        public string UrlImage => Utils.CloudPath() + $"/product/mainimages/small/{Image}";
        public string Price { get; set; }
        public DateTime? LastEditDate { get; set; }
        public int? ViewCount { get; set; }
        public int? SaleLocation_ID { get; set; }
        public string SaleName { get; set; }
        public string SaleContactName { get; set; }
        public string SaleEmail { get; set; }
        public string SalePhone { get; set; }
        public int? YearManufacture { get; set; }
        public string StatusMachine { get; set; }
        public int? HoursOfWork { get; set; }
        public bool? AirConditioning { get; set; }
        public string WorkingAddress { get; set; }
        public int? WorkingLocation_ID { get; set; }
        public string ProductModelName { get; set; }
        public string ProductManufactureName { get; set; }
        public string SaleLocationName { get; set; }
        public string SerialNumber { get; set; }
        public string Unit { get; set; }
        public int? HourOfWorkType { get; set; }        
        public string ProductCategoryName { get; set; }
        public string CountryName { get; set; }
        public int? MadeCountryId { get; set; }
        public string UnitName { get; set; }
        public string UnitPrice => "₫";
        public string Label { get; set; }
        public string PartNumber { get; set; }
        public int? RelatedCategoryId { get; set; }
        public string RelatedCategoryName { get; set; }
        public string AccessoriesModelName { get; set; }
        public int? AccessoriesCategoryId { get; set; }
        public string AccessoriesCategoryName { get; set; }
        public string AccessoriesManufactureName { get; set; }
        public string CreateBy { get; set; }
        public string Description { get; set; }
        public string FullUrl { get; set; }
        public int SellCount { get; set; }
    }
    public class MetaKeywordDTO
    {
        public int MetaKeyword_ID { get; set; }
        public Nullable<int> ProductCategory_ID { get; set; }
        public string Name { get; set; }
        public string URL { get; set; }
        public string Type { get; set; }
    }
    public class ProductDetailsDTO : ModelBaseStatus
    {
        public ProductDetailsDTO()
        {
            ProductDetails = new ProductDTO();
            ProductPictures = new List<ProductPictureDTO>();
            ProductBrand = new BrandDTO();
            AccessoriesFit = new List<AccessoriesFitDTO>();
            Profiles = new AspNetUserProfiles();
        }
        public BrandDTO ProductBrand { get; set; }
        public ProductDTO ProductDetails { get; set; }
        public AspNetUserProfiles Profiles { get; set; }
        public IEnumerable<ProductPictureDTO> ProductPictures { get; set; }
        public IEnumerable<AccessoriesFitDTO> AccessoriesFit { get; set; }
        public IEnumerable<MetaKeywordDTO> MetaKeyword { get; set; }
    }

    public class TimeLinePostDTO :ModelBaseStatus
    {
        public TimeLinePostDTO()
        {
            ProductPictures = new List<ProductPictureDTO>();
            CVPictures = new List<CVPictureDTO>();
            RecruitmentPictures = new List<RecPictureDTO>();
        }
        public int Id { get; set; }
        public Nullable<int> TotalRows { get; set; }
        public Nullable<int> ProductTypeId { get; set; }
        public string TypeName { get; set; }
        public Nullable<int> ProductId { get; set; }
        public Nullable<int> ProductCategoryId { get; set; }
        public string ProductFullUrl { get; set; }
        public Nullable<int> LibraryId { get; set; }
        public Nullable<int> LibraryCategoryId { get; set; }
        public string LibraryFullUrl { get; set; }
        public Nullable<int> CVId { get; set; }
        public Nullable<int> CVCategoryId { get; set; }
        public string CVFullUrl { get; set; }
        public Nullable<int> RecId { get; set; }
        public Nullable<int> RecCategoryId { get; set; }
        public string RecFullUrl { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string MainImageUrl { get; set; }
        public Nullable<int> StatusTypeId { get; set; }
        public Nullable<DateTime> CreateDate { get; set; }
        public Nullable<DateTime> LastEditDate { get; set; }
        public List<ProductPictureDTO> ProductPictures { get; set; }
        public List<CVPictureDTO> CVPictures { get; set; }
        public List<RecPictureDTO> RecruitmentPictures { get; set; }
    }

    public class ProductMostViewDTO : ModelBase
    {
        public string Image { get; set; }
        public string URL { get; set; }
        public string Title { get; set; }
    }
    public class HomeProductSlide_ResultDTO : ModelBase
    {
        public string TableName { get; set; }
        public string Image { get; set; }
        public string URL { get; set; }
    }
    public class ProductSearchResultDTO
    {
        public int ProductId { get; set; }
        public Nullable<int> ProductCategoryId { get; set; }
        public Nullable<int> ProductBrandId { get; set; }
        public Nullable<int> ProductType_ID { get; set; }
        public Nullable<int> ProductModelId { get; set; }
        public Nullable<int> ProductManufactureId { get; set; }
        public Nullable<int> CountryId { get; set; }
        public string Name { get; set; }
        public Nullable<decimal> Price { get; set; }
        public string Image { get; set; }
        public string UrlImage => Utils.CloudPath() + $"/product/mainimages/small/{Image}";
        public Nullable<int> SaleLocationId { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> LastEditDate { get; set; }
        public string LocationName { get; set; }
        public Nullable<int> ViewCount { get; set; } = 0;
        public Nullable<int> StatusType_ID { get; set; }
        public string FullURL { get; set; }
        public string SalePhone { get; set; }
        public string SaleName { get; set; }
    }
    public class AccessoriesFitDTO
    {
        public string CategoryName { get; set; }
        public string ManufactureName { get; set; }
        public string ModelName { get; set; }
        public int? CategoryId { get; set; }
        public int? ManufactureId { get; set; }
        public int? ModelId { get; set; }
    }
    public class ListProductMarketDTO : ModelBaseStatus
    {
        public ListProductMarketDTO()
        {
            Data = new List<ProductSearchResultDTO>();
        }
        public IEnumerable<ProductSearchResultDTO> Data { get; set; }
    }
    public class ProductItemByCateIdDTO
    {
        public int ProductId { get; set; }
        public Nullable<int> ProductCategoryId { get; set; }
        public Nullable<int> ProductTypeId { get; set; }
        public Nullable<int> ProductBrandId { get; set; }
        public Nullable<int> ProductModelId { get; set; }
        public Nullable<int> ProductManufactureId { get; set; }
        public Nullable<int> CountryId { get; set; }
        public string Name { get; set; }
        public string FullURL { get; set; }
        public string Image { get; set; }
        public string UrlImage => Utils.CloudPath() + $"/product/mainimages/small/{Image}";
        public Nullable<decimal> Price { get; set; }
        public Nullable<int> SaleLocationId { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> LastEditDate { get; set; }
        public string LocationName { get; set; }
    }
    public class TopProductByCateIdDTO : ModelBaseStatus
    {
        public TopProductByCateIdDTO()
        {
            Data = new List<ProductItemByCateIdDTO>();
        }
        public IEnumerable<ProductItemByCateIdDTO> Data { get; set; }
    }
    public class SponsorBanner :ModelBase
    {
        public SponsorBanner()
        {
            DataSponsor = new List<ProductSearchResultDTO>();
            DataBanner = new List<AdvertisingCarouselDTO>();
        }
        public List<ProductSearchResultDTO> DataSponsor { get; set; }
        public List<AdvertisingCarouselDTO> DataBanner { get; set; }
    }
    public class ProductPaggingDTO : ModelBaseStatus
    {
        public int TotalRecord { get; set; }
        public int TotalPage { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public IEnumerable<ProductSearchResultDTO> Data { get; set; }
        public ProductPaggingDTO()
        {
            Data = new List<ProductSearchResultDTO>();
        }

    }
    public class ProductIdDTO : ModelBase
    {
        public int? ProductId { get; set; }
        public int? ProductBrandId { get; set; }
        public int? NumberRenewInOneDay { get; set; }
    }
    public class ProductShopManDTO
    {
        public int? ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string MetaDescription { get; set; }
        public string Tag { get; set; }
        public int? HoursOfWork { get; set; }
        public string SaleName { get; set; }
        public string SaleContactName { get; set; }
        public string SalePhone { get; set; }
        public string SaleAddress { get; set; }
        public string SaleEmail { get; set; }
        public int? YearManufacture { get; set; }
        public string StatusMachine { get; set; }
        public int ProductManufactureId { get; set; }
        public int ProductCategoryId { get; set; }
        public int ProductModelId { get; set; }
        public int ProductTypeId { get; set; }
        public string SerialNumber { get; set; }
        public int? SaleLocationId { get; set; }
        public bool? HasNewModel { get; set; }
        public string ReferralCode { get; set; }
        public long Price { get; set; }
        public int? MadeCountryId { get; set; }
        public int? HourOfWorkType { get; set; }
        public string Unit { get; set; }
        public string Label { get; set; }
        public string PartNumber { get; set; }
        public int? RelatedCategoryId { get; set; }
        public int? AccessoriesCategoryId { get; set; }
        public string AccessoriesManufactureName { get; set; }
        public string AccessoriesModelName { get; set; }
        public int SellCount { get; set; }
    }
    public class PostProductShopManDTO : ModelBase
    {
        public PostProductShopManDTO()
        {
            Product = new ProductShopManDTO();
            MainImage = new ImageUploadDTO();
            SubImage = new List<ImageUploadDTO>();
            DeleteProdPicture = new List<DeleteImageProductPicture>();
            AccessoriesFit = new List<AccessoriesFitDTO>();
        }
        public int? typeForm { get; set; }
        public string [] SubImageUpload { get; set; }
        public string [] SubImageFileName { get; set; }
        public ProductShopManDTO Product { get; set; }
        public ImageUploadDTO MainImage { get; set; }
        public List<ImageUploadDTO> SubImage { get; set; }
        public List<DeleteImageProductPicture> DeleteProdPicture { get; set; }
        public List<AccessoriesFitDTO> AccessoriesFit { get;set;}
        
    }

    public class PostProductTest
    {
        public int? ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string MetaDescription { get; set; }
        public string Tag { get; set; }
        public int? HoursOfWork { get; set; }
        public string SaleName { get; set; }
        public string SaleContactName { get; set; }
        public string SalePhone { get; set; }
        public string SaleAddress { get; set; }
        public string SaleEmail { get; set; }
        public int? YearManufacture { get; set; }
        public string StatusMachine { get; set; }
        public int ProductManufactureId { get; set; }
        public int ProductCategoryId { get; set; }
        public int ProductModelId { get; set; }
        public int ProductTypeId { get; set; }
        public string SerialNumber { get; set; }
        public int? SaleLocationId { get; set; }
        public bool? HasNewModel { get; set; }
        public string ReferralCode { get; set; }
        public long Price { get; set; }
        public int? MadeCountryId { get; set; }
        public int? HourOfWorkType { get; set; }
        public string Unit { get; set; }
        public string Label { get; set; }
        public string PartNumber { get; set; }
        public int? RelatedCategoryId { get; set; }
        public int? AccessoriesCategoryId { get; set; }
        public string AccessoriesManufactureName { get; set; }
        public string AccessoriesModelName { get; set; }
        public int SellCount { get; set; }
        public ImageUploadDTO MainImage { get; set; }
        public List<ImageUploadDTO> SubImage { get; set; }
        public List<DeleteImageProductPicture> DeleteProdPicture { get; set; }
        public List<AccessoriesFitDTO> AccessoriesFit { get; set; }
        public PostProductTest()
        {
            MainImage = new ImageUploadDTO();
            SubImage = new List<ImageUploadDTO>();
            AccessoriesFit = new List<AccessoriesFitDTO>();
        }

    }

    public class PostProductShopManResultDTO : ModelBaseStatus
    {
        public int ProductId { get; set; }
    }
    public class ProductStatusMachine
    {
        public string ID { get; set; }
        public string StatusMachine { get; set; }

    }
}
