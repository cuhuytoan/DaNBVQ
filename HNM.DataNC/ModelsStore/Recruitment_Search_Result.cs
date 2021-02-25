using HNM.DataNC.ModelsNC.ModelsUtility;
using System;
using System.ComponentModel.DataAnnotations;

namespace HNM.DataNC.ModelsStore
{
    public class Recruitment_Search_Result
    {
        [Key]
        public Nullable<long> NoItem { get; set; }
        public Nullable<int> TotalRows { get; set; }
        public Nullable<int> TotalRowsActive { get; set; }
        public Nullable<int> TotalRowsDeActive { get; set; }
        public Nullable<int> Recruitment_ID { get; set; }
        public Nullable<int> RecruitmentCategory_ID { get; set; }
        public Nullable<int> ProductBrand_ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        public Nullable<decimal> PriceFrom { get; set; }
        public Nullable<decimal> PriceTo { get; set; }
        public Nullable<bool> IsNew { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string UserName { get; set; }
        public Nullable<System.Guid> CreateBy { get; set; }
        public Nullable<System.DateTime> LastEditDate { get; set; }
        public Nullable<System.Guid> LastEditBy { get; set; }
        public Nullable<bool> Active { get; set; }
        public int Counter { get; set; }
        public Nullable<int> ViewCount { get; set; }
        public string URL { get; set; }
        public string Tag { get; set; }
        public string Keyword { get; set; }
        public Nullable<bool> AllowComment { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
        public string ContactAddress { get; set; }
        public Nullable<int> Location_ID { get; set; }
        public string ContactEmail { get; set; }
        public Nullable<int> StatusType_ID { get; set; }
        public string LocationName { get; set; }
        public string LogoBrand { get; set; }
        public string FullUrlLogoBrand => Utils.CloudPath() + $"/productbrand/logo/thumb/{LogoBrand}";
    }
}
