using HNM.DataNC.ModelsNC.ModelsUtility;
using System;
using System.Collections.Generic;

namespace HNM.DataNC.ModelsNC.ModelsDTO
{
    public class AdvertisingDTO
    {
        public int Advertising_ID { get; set; }
        public int? AdvertisingBlock_ID { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string Ext { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public string URL { get; set; }
        public int? Sort { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsCode { get; set; }
        public bool? Active { get; set; }
        public int? Counter { get; set; }
        public Guid? CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public Guid? LastEditBy { get; set; }
        public DateTime? LastEditDate { get; set; }
    }
    public class AdvertisingCarouselDTO
    {
        public string Image { get; set; }
        public string UrlImage => Utils.CloudPath() + $"/advertising/mainimages/original/{Image}";
        public string URL { get; set; }
        public string Title { get; set; }
        public string FormId { get; set; }
        public int? ParamId { get; set; }
        public int? CategoryId { get; set; }
        public int? ProductTypeId { get; set; }

    }
    public class ListAdvertisingCarouselDTO : ModelBaseStatus
    {
        public ListAdvertisingCarouselDTO()
        {
            Data = new List<AdvertisingCarouselDTO>();
        }
        public IEnumerable<AdvertisingCarouselDTO> Data { get; set; }
    }
    public class ListAdvertisingCarouselDTOTest : ModelBaseStatus
    {
        public ListAdvertisingCarouselDTOTest()
        {
            data = new List<AdvertisingCarouselDTO>();
        }
        public IEnumerable<AdvertisingCarouselDTO> data { get; set; }
    }
    public class AdvertisingMobileDTO
    {
        public int AdvertisingBlock_ID { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string UrlImage => Utils.CloudPath() + $"/advertising/mainimages/original/{Image}";
        public string URL { get; set; }
        public string Title { get; set; }
        public int? AdMobileType { get; set; }
        public int? FormId { get; set; }
        public int? ParameterId { get; set; }
    }
    public class ListAdvertisingMobileDTO : ModelBaseStatus
    {
        public ListAdvertisingMobileDTO()
        {
            Data = new List<AdvertisingMobileDTO>();
        }
        public IEnumerable<AdvertisingMobileDTO> Data { get; set; }
    }
    public class ContactAdsDTO :ModelBase
    {        
        public string Name { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        public string Content { get; set; }
        public string Email { get; set; }        
        public int? Status { get; set; }
    }
}
