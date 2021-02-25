using HNM.DataNC.ModelsNC.ModelsUtility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HNM.DataNC.ModelsNC.ModelsDTO
{
    public class CurriculumVitaeDTO : ModelBase
    {
        [Key]
        public int Id { get; set; }
        public string FullName { get; set; }
        public int? YearOfBirth { get; set; }
        public DateTime? DOB { get; set; }
        public int? ProductCateRelate { get; set; }
        public string ProductCateRelateName { get; set; }
        public bool? Gender { get; set; }
        public string PlaceOfBirth { get; set; }
        public string HomeTown { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Certificate { get; set; }
        public string Avatar { get; set; }
        public string FullUrlAvatar => Utils.CloudPath() + $"/curriculumvitae/mainimages/original/{Avatar}";
        public string MainOccupation { get; set; }
        public int? CareerCategoryId { get; set; }
        public string CareerCategoryName { get; set; }
        public string Degree { get; set; }
        public int? YearsOfExperience { get; set; }
        public string TitleName { get; set; }
        public decimal? Salary { get; set; }
        public string TypeOfWork { get; set; }
        public string IntroInfomation { get; set; }
        public string LocalWork { get; set; }
        public Guid? CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public Guid? LastEditBy { get; set; }
        public DateTime? LastEditDate { get; set; }
        public string URL { get; set; }
        public int? StatusTypeId { get; set; }
        public string Title => $"{FullName} - {MainOccupation}";
    }
    public class CVPictureDTO
    {
        public Guid CVPicture_ID { get; set; }        
        public string Image { get; set; }
        public string UrlImage => Utils.CloudPath() + $"/cvpicture/mainimages/original/{Image}";
    }
    public class CVDetailDTO : ModelBase
    {
        public CVDetailDTO()
        {
            CVDetails = new CurriculumVitaeDTO();
            CVPictures = new List<CVPictureDTO>();
        }
        public CurriculumVitaeDTO CVDetails { get; set; }
        public List<CVPictureDTO> CVPictures { get; set; }
    }
    public class PostCurriculumnViateDTO :ModelBase
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int? YearOfBirth { get; set; }
        public DateTime? DOB { get; set; }
        public int? ProductCateRelate { get; set; }
        public bool? Gender { get; set; }
        public string ExprienceDescription { get; set; }
        public string PlaceOfBirth { get; set; }
        public string HomeTown { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Certificate { get; set; }
        public string Avatar { get; set; }
        public string MainOccupation { get; set; }
        public int? CareerCategoryId { get; set; }
        public string Degree { get; set; }
        public int? YearsOfExperience { get; set; }
        public string TitleName { get; set; }
        public decimal? Salary { get; set; }
        public string TypeOfWork { get; set; }
        public string IntroInfomation { get; set; }
        public string LocalWork { get; set; }     
        public string URL { get; set; }        
        public int? LocationWorkId { get; set; }
    }
    public class SumPostCurriculumnViateDTO :ModelBase
    {
        public SumPostCurriculumnViateDTO()
        {
            CV = new PostCurriculumnViateDTO();
            MainImage = new ImageUploadDTO();
            SubImage = new List<ImageUploadDTO>();
            DeleteCVPicture = new List<DeleteImageCVPicture>();
        }
        public string[] SubImageUpload { get; set; }
        public string[] SubImageFileName { get; set; }
        public PostCurriculumnViateDTO CV { get; set; }
        public ImageUploadDTO MainImage { get; set; }
        public List<ImageUploadDTO> SubImage { get; set; }
        public List<DeleteImageCVPicture> DeleteCVPicture { get; set; }
    }
    public class DeleteImageCVPicture
    {

        /// <summary>
        /// ProductPicture ID for delete
        /// </summary>
        public string CVPicture_ID { get; set; }
    }
    public class ReturnPostCVDTO :ModelBaseStatus
    {
        public int Id { get; set; }
    }
    public class CVSearchDTO : ModelBase
    {
        [Key]
        public Nullable<long> NoItem { get; set; }
        public Nullable<int> TotalRows { get; set; }
        public Nullable<int> Id { get; set; }
        public string FullName { get; set; }
        public Nullable<int> YearOfBirth { get; set; }
        public Nullable<bool> Gender { get; set; }
        public string PlaceOfBirth { get; set; }
        public string HomeTown { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Certificate { get; set; }
        public string Avatar { get; set; }
        public string FullUrlAvatar => Utils.CloudPath() + $"/curriculumvitae/mainimages/original/{Avatar}";
        public string MainOccupation { get; set; }
        public Nullable<int> CareerCategoryId { get; set; }        
        public string Degree { get; set; }
        public Nullable<int> YearsOfExperience { get; set; }
        public string TitleName { get; set; }
        public Nullable<decimal> Salary { get; set; }
        public string TypeOfWork { get; set; }
        public string IntroInfomation { get; set; }
        public string LocalWork { get; set; }
        public Nullable<System.Guid> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.Guid> LastEditBy { get; set; }
        public Nullable<System.DateTime> LastEditDate { get; set; }
        public string URL { get; set; }
        public Nullable<int> StatusTypeId { get; set; }
        public string Title => $"{FullName} - {MainOccupation}";
    }
}
