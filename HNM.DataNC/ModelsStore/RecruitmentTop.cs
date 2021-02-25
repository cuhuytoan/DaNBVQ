using HNM.DataNC.ModelsNC.ModelsUtility;
using System;
using System.ComponentModel.DataAnnotations;

namespace HNM.DataNC.ModelsStore
{
    public class RecruitmentTop
    {
        [Key]
        public int Recruitment_ID { get; set; }
        public int? RecruitmentCategory_ID { get; set; }
        public int? ProductBrand_ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        public decimal? PriceFrom { get; set; }
        public decimal? PriceTo { get; set; }
        public bool? IsNew { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? CreateDate { get; set; }
        public Guid? CreateBy { get; set; }
        public DateTime? LastEditDate { get; set; }
        public Guid? LastEditBy { get; set; }
        public bool? Active { get; set; }
        public int? Counter { get; set; }
        public int? ViewCount { get; set; }
        public string URL { get; set; }
        public string Tag { get; set; }
        public string Keyword { get; set; }
        public bool? AllowComment { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
        public string ContactAddress { get; set; }
        public int? Location_ID { get; set; }
        public string ContactEmail { get; set; }
        public int? StatusType_ID { get; set; }
        public string LocationName { get; set; }
        public string LogoBrand { get; set; }
        public string FullUrlLogoBrand => Utils.CloudPath() + $"/productbrand/logo/thumb/{LogoBrand}";
        public string CompanyBusiness { get; set; }
        public int? RecruimentNumber { get; set; }
        public int? RequireExp { get; set; }
        public int? JobType { get; set; }
        public string JobTypeName { get; set; }
        public int? SalaryType { get; set; }
        public string SalaryTypeName { get; set; }
        public string AddressOfCV { get; set; }
        public string ContactPersonOfCV { get; set; }
        public string PhonePersonOfCV { get; set; }
        public string EmailPersonOfCV { get; set; }
        public string CompanyWebsite { get; set; }
        public int? ProductCateRelate { get; set; }
        public int? ProductCateChildRelate { get; set; }
        public DateTime? DeadlineOfCV { get; set; }
        public string LocalWork { get; set; }
        public string RequireOfCV { get; set; }

    }
    public class SentContactRec
    {
        public string MailBody { get; set; }
        public string ContactEmail { get; set; }
        public string ContactName { get; set; }
    }
    public class SentContactBrand
    {
        public string MailBody { get; set; }
        public string ContactEmail { get; set; }
        public string ContactName { get; set; }
    }
}
