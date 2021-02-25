using System;
using System.ComponentModel.DataAnnotations;

namespace HNM.DataNC.ModelsStore
{
    public class Materials_Search_Result
    {
        [Key]
        public Nullable<long> NoItem { get; set; }
        public Nullable<int> TotalRows { get; set; }
        public Nullable<int> TotalRowsSaveDraft { get; set; }
        public Nullable<int> TotalRowsWaitApproval { get; set; }
        public Nullable<int> TotalRowsInfoError { get; set; }
        public Nullable<int> TotalRowsPublised { get; set; }
        public Nullable<int> TotalRowsStopPublised { get; set; }
        public Nullable<int> Product_ID { get; set; }
        public Nullable<int> Store_ID { get; set; }
        public Nullable<int> ProductCategory_ID { get; set; }
        public Nullable<int> ProductType_ID { get; set; }
        public Nullable<int> ProductBrand_ID { get; set; }
        public Nullable<int> ProductModel_ID { get; set; }
        public Nullable<int> ProductManufacture_ID { get; set; }
        public Nullable<int> Country_ID { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string BannerImages { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<int> Counter { get; set; }
        public Nullable<int> ViewCount { get; set; }
        public Nullable<int> SellCount { get; set; }
        public string URL { get; set; }
        public string FullURL { get; set; }
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
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> LastEditDate { get; set; }
        public Nullable<bool> HasNewModel { get; set; }
        public string LocationName { get; set; }
        public Nullable<int> StatusType_ID { get; set; }
        public Nullable<int> ProductBrandRank { get; set; }
        public string ProductBrandUrl { get; set; }
        public string ProductBrandLink { get; set; }
        public string ProductBrandName { get; set; }
        public Nullable<System.DateTime> ProductBrandIssuedDate { get; set; }
        public string PartNumber { get; set; }
    }
}
