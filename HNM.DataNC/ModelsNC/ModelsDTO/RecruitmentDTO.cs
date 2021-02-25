using HNM.DataNC.ModelsNC.ModelsUtility;
using System;
using System.Collections.Generic;

namespace HNM.DataNC.ModelsNC.ModelsDTO
{
    public class RecruitmentDTO
    {
        public int RecruitmentId { get; set; }
        public int? RecruitmentCategoryId { get; set; }
        public string RecruitmentCategoryName { get; set; }
        public int? ProductBrandId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        public string FullUrlImage => Utils.CloudPath() + $"/recruitment/mainimages/original/{Image}";
        public string LogoBrand { get; set; }
        public string FullUrlLogoBrand => Utils.CloudPath() + $"/productbrand/logo/thumb/{LogoBrand}";
        public decimal? PriceFrom { get; set; }
        public decimal? PriceTo { get; set; }
        public string Salary => Utils.RenderPriceRecruitment(PriceFrom, PriceTo);
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
        public string LocationName { get; set; }
        public string ContactEmail { get; set; }
        public int? StatusType_ID { get; set; }
        public string ReferralCode { get; set; }
        public string SKU { get; set; }
        public string CompanyBusiness { get; set; }
        public int? RecruimentNumber { get; set; }
        public int? RequireExp { get; set; }
        public int? JobType { get; set; }
        public int? SalaryType { get; set; }
        public string AddressOfCV { get; set; }
        public string ContactPersonOfCV { get; set; }
        public string PhonePersonOfCV { get; set; }
        public string EmailPersonOfCV { get; set; }
        public string CompanyWebsite { get; set; }
        public int? ProductCateRelate { get; set; }
        public int? ProductCateChildRelate { get; set; }
        public string ProductCategoryChildRelateName { get; set; }
        public DateTime? DeadlineOfCV { get; set; }
        public string RequireOfCV { get; set; }
        public string LocalWork { get; set; }

    }
    public class RecPictureDTO
    {
        public Guid RecruitmentPicture_ID { get; set; }
        public string Image { get; set; }
        public string UrlImage => Utils.CloudPath() + $"/recruitmentpicture/mainimages/original/{Image}";
    }
    public class RecDetailDTO :ModelBase
    {
        public RecDetailDTO()
        {
            RecDetail = new RecruitmentDTO();
            RecPictures = new List<RecPictureDTO>();
        }
        public RecruitmentDTO RecDetail { get; set; }
        public List<RecPictureDTO> RecPictures { get; set; }
    }
    public class TopRecruitmentDTO : ModelBaseStatus
    {
        public TopRecruitmentDTO()
        {
            Data = new List<RecruitmentDTO>();
        }
        public IEnumerable<RecruitmentDTO> Data { get; set; }
    }
    public class RecruitmentSearchResultDTO
    {
        public int RecruitmentId { get; set; }
        public int? RecruitmentCategoryId { get; set; }
        public int? ProductBrandId { get; set; }
        public string Name { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? LastEditDate { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public int? LocationId { get; set; }
        public string LocationName { get; set; }
        public string PriceFrom { get; set; }
        public string PriceTo { get; set; }
        public string Salary => Utils.RenderPriceRecruitment(PriceFrom, PriceTo);
        public string LogoBrand { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string URL { get; set; }
        public string Tag { get; set; }
        public string Keyword { get; set; }
        public Nullable<bool> AllowComment { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
        public string FullUrlImage => Utils.CloudPath() + $"/recruitment/mainimages/small/{Image}";
        public string FullUrlLogoBrand => Utils.CloudPath() + $"/recruitment/mainimages/small/{Image}";
        public string FullUrlLogoBrandWeb => Utils.CloudPath() + $"/productbrand/logo/thumb/{LogoBrand}";
        public string ContactPhone { get; set; }
        public string ContactAddress { get; set; }
        public string ContactEmail { get; set; }
    }

    public class RecruitmentPaggingDTO : ModelBaseStatus
    {
        public int TotalRecord { get; set; }
        public int TotalPage { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public IEnumerable<RecruitmentSearchResultDTO> Data { get; set; }
        public RecruitmentPaggingDTO()
        {
            Data = new List<RecruitmentSearchResultDTO>();
        }

    }

    public class SentContactRecruitment : ModelBase
    {

    }

    public class ContactRecruitment
    {
        public int RecruitmentId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Content { get; set; }
    }
    public class SumPostRecDTO :ModelBase
    {
        public SumPostRecDTO()
        {
            Rec = new RecruitmentDTO();
            MainImage = new ImageUploadDTO();
            SubImage = new List<ImageUploadDTO>();
            DeleteRecPicture = new List<DeleteImageRecPicture>();
        }
        public RecruitmentDTO Rec { get; set; }
        public string[] SubImageUpload { get; set; }
        public string[] SubImageFileName { get; set; }
        public ImageUploadDTO MainImage { get; set; }
        public List<ImageUploadDTO> SubImage { get; set; }
        public List<DeleteImageRecPicture> DeleteRecPicture { get; set; }
    }
    public class DeleteImageRecPicture
    {

        /// <summary>
        /// ProductPicture ID for delete
        /// </summary>
        public string RecruitmentPicture_ID { get; set; }
    }
    public class ReturnPostRecDTO : ModelBaseStatus
    {
        public int Id { get; set; }
    }
}

