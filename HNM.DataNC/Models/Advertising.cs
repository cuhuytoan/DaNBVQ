﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace HNM.DataNC.Models
{
    public partial class Advertising
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
        public int? AdMostViewType { get; set; }
        public int? AdMostViewCate { get; set; }
        public int? AdMostViewChildCate { get; set; }
        public int? AdMobileType { get; set; }
        public string FormId { get; set; }
        public int? ParameterId { get; set; }
        public int? CategoryId { get; set; }
        public int? ProductTypeId { get; set; }

        public virtual AdvertisingBlock AdvertisingBlock_ { get; set; }
    }
}