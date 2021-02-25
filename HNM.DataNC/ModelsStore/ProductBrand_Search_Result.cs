using System;
using System.ComponentModel.DataAnnotations;

namespace HNM.DataNC.ModelsStore
{
    public class ProductBrand_Search_Result
    {
        [Key]
        public int NoItem { get; set; }
        public int ProductBrand_ID { get; set; }
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
        public string Telephone { get; set; }
        public string Fax { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string Facebook { get; set; }
        public string Zalo { get; set; }
        public string Hotline { get; set; }
        public string Skype { get; set; }
        public string PRInfo { get; set; }
        public string Agency { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Banner { get; set; }
        public string Logo { get; set; }
        public string PersonName { get; set; }
        public string PersonAddress { get; set; }
        public string PersonMobile { get; set; }
        public string PersonEmail { get; set; }
        public string URL { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<bool> Verified { get; set; }
        public Nullable<int> Rank { get; set; }
        public Nullable<int> ViewCount { get; set; }
        public string DirectorName { get; set; }
        public Nullable<System.DateTime> DirectorBirthday { get; set; }
        public string DirectorAddress { get; set; }
        public string DirectorMobile { get; set; }
        public string DirectorEmail { get; set; }
        public string DirectorPosition { get; set; }
        public Nullable<System.Guid> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.Guid> LastEditBy { get; set; }
        public Nullable<System.DateTime> LastEditDate { get; set; }
    }
}
