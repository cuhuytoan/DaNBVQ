using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace HNM.DataNC.ElasticModels
{
    #region product


    public class ProductElastic
    {
        public string Id { get; set; }
        public Nullable<int> Product_ID { get; set; }
        public Nullable<int> Store_ID { get; set; }
        public Nullable<int> ProductCategory_ID { get; set; }
        public Nullable<int> ProductCategoryMachine_ID { get; set; }
        public Nullable<int> ProductType_ID { get; set; }
        public Nullable<int> ProductBrand_ID { get; set; }
        public Nullable<int> ProductModel_ID { get; set; }
        public Nullable<int> ProductManufacture_ID { get; set; }
        public Nullable<int> Country_ID { get; set; }
        public string BarCode { get; set; }
        public string QRCodePublic { get; set; }
        public string Name { get; set; }
        public string ProductNotation { get; set; }
        public string PartNumber { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string PRInfo { get; set; }
        public string LegalInfo { get; set; }
        public string Image { get; set; }
        public string BannerImages { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<decimal> PriceOld { get; set; }
        public Nullable<decimal> Discount { get; set; }
        public Nullable<int> DiscountRate { get; set; }
        public Nullable<bool> IsSecondHand { get; set; }
        public Nullable<bool> IsAuthor { get; set; }
        public Nullable<bool> IsBestSale { get; set; }
        public Nullable<bool> IsSaleOff { get; set; }
        public Nullable<bool> IsNew { get; set; }
        public Nullable<bool> IsComming { get; set; }
        public Nullable<bool> IsOutStock { get; set; }
        public Nullable<bool> IsDiscontinue { get; set; }
        public string Unit { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.Guid> CreateBy { get; set; }
        public Nullable<System.DateTime> LastEditDate { get; set; }
        public Nullable<System.Guid> LastEditBy { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<int> Counter { get; set; }
        public Nullable<int> ViewCount { get; set; }
        public Nullable<int> SellCount { get; set; }
        public string URL { get; set; }
        public string FullURL { get; set; }
        public string Alias { get; set; }
        public string Tag { get; set; }
        public string Keyword { get; set; }
        public Nullable<bool> AllowComment { get; set; }
        public string Expiry { get; set; }
        public Nullable<int> ExpiryByDate { get; set; }
        public Nullable<int> WarrantyMonth { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
        public string SaleName { get; set; }
        public string SaleContactName { get; set; }
        public string SalePhone { get; set; }
        public string SaleAddress { get; set; }
        public Nullable<int> SaleLocation_ID { get; set; }
        public string SaleEmail { get; set; }
        public string SerialNumber { get; set; }
        public Nullable<int> YearManufacture { get; set; }
        public string StatusMachine { get; set; }
        public Nullable<int> HoursOfWork { get; set; }
        public Nullable<bool> AirConditioning { get; set; }
        public string EngineModel { get; set; }
        public string EngineManufacture { get; set; }
        public string RentalPeriod { get; set; }
        public string RentalType { get; set; }
        public string WorkingAddress { get; set; }
        public Nullable<int> WorkingLocation_ID { get; set; }
        public string PaymentlType { get; set; }
        public Nullable<int> Job1Flag { get; set; }
        public Nullable<int> StatusType_ID { get; set; }
        public string MetaKeywordIds { get; set; }
        public string LocationName { get; set; }
        public string ProductBrandName { get; set; }
        public string ProductBrandAddress { get; set; }
        public string ProductBrandMobile { get; set; }
        public string ProductBrandEmail { get; set; }
        public string ProductBrandWebsite { get; set; }
        public string ProductBrandImage { get; set; }
        public string ProductBrandUrl { get; set; }
        public string ProductBrandLogo { get; set; }
        public string ProductBrandBanner { get; set; }


    }
    public class ProductsList
    {
        [JsonProperty("total_record")]
        public long TotalRecord { get; set; }
        [JsonProperty("total_page")]
        public long TotalPage { get; set; }
        [JsonProperty("current_page")]
        public long CurrentPage { get; set; }
        [JsonProperty("products")]
        public List<ProductElastic> Products { get; set; }
        public ProductsList()
        {
            Products = new List<ProductElastic>();
        }
    }

    #endregion


    #region brand

    public class BrandElastic
    {
        public string Id { get; set; }
        public int ProductBrand_ID { get; set; }
        public Nullable<int> Country_ID { get; set; }
        public Nullable<int> Location_ID { get; set; }
        public Nullable<int> ProductBrandType_ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string TradingName { get; set; }
        public string BrandName { get; set; }
        public string TaxCode { get; set; }
        public string RegistrationNumber { get; set; }
        public Nullable<System.DateTime> IssuedDate { get; set; }
        public string BusinessArea { get; set; }
        public string Address { get; set; }
        public string District { get; set; }
        public string PostalCode { get; set; }
        public string MapCode { get; set; }
        public string Telephone { get; set; }
        public string Fax { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public bool EmailDisplay { get; set; }
        public string Website { get; set; }
        public string Facebook { get; set; }
        public string Zalo { get; set; }
        public string Hotline { get; set; }
        public string Skype { get; set; }
        public string PRInfo { get; set; }
        public string Agency { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string PersonName { get; set; }
        public string PersonAddress { get; set; }
        public string PersonMobile { get; set; }
        public string PersonEmail { get; set; }
        public string DirectorName { get; set; }
        public Nullable<System.DateTime> DirectorBirthday { get; set; }
        public string DirectorAddress { get; set; }
        public string DirectorMobile { get; set; }
        public string DirectorEmail { get; set; }
        public string DirectorPosition { get; set; }
        public string URL { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<bool> Verified { get; set; }
        public Nullable<int> Rank { get; set; }
        public Nullable<int> ViewCount { get; set; }
        public Nullable<System.Guid> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.Guid> LastEditBy { get; set; }
        public Nullable<System.DateTime> LastEditDate { get; set; }
        public string Logo { get; set; }
        public string Banner { get; set; }
    }
    public class BrandListElastic
    {
        [JsonProperty("total_record")]
        public long TotalRecord { get; set; }
        [JsonProperty("total_page")]
        public long TotalPage { get; set; }
        [JsonProperty("current_page")]
        public long CurrentPage { get; set; }
        [JsonProperty("brands")]
        public List<BrandElastic> Brands { get; set; }
        public BrandListElastic()
        {
            Brands = new List<BrandElastic>();
        }
    }

    #endregion
}
