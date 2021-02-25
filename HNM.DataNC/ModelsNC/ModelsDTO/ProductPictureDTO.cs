using HNM.DataNC.ModelsNC.ModelsUtility;
using System;

namespace HNM.DataNC.ModelsNC.ModelsDTO
{
    public class ProductPictureDTO
    {
        public Guid ProductPicture_ID { get; set; }
        public string Image { get; set; }
        //public string UrlImage => Utils.CloudPath() + $"/productpicture/mainimages/small/{Image}";
        public string UrlImage { get; set; }
    }
    public class RecruitmentPictureDTO
    {
        public Guid RecruimentPicture_ID { get; set; }
        public string Image { get; set; }
        //public string UrlImage => Utils.CloudPath() + $"/productpicture/mainimages/small/{Image}";
        public string UrlImage => Utils.CloudPath() + $"/recruitmentpicture/mainimages/original/{Image}";
    }



}
