﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace HNM.DataNC.Models
{
    public partial class Library
    {
        public int Library_ID { get; set; }
        public int? Store_ID { get; set; }
        public int? LibraryCategory_ID { get; set; }
        public int? LibraryType_ID { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Image { get; set; }
        public string ImageDescription { get; set; }
        public string BannerImages { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? Active { get; set; }
        public int? Counter { get; set; }
        public Guid? CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public Guid? LastEditBy { get; set; }
        public DateTime? LastEditDate { get; set; }
        public string URL { get; set; }
        public string Tag { get; set; }
        public string Keyword { get; set; }
        public bool? AllowComment { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
        public int? StatusType_ID { get; set; }
        public string ReferralCode { get; set; }
        public string SKU { get; set; }
        public string AuthorPhone { get; set; }
        public string AuthorEmail { get; set; }
        public string AuthorJob { get; set; }
        public string AuthorCompany { get; set; }
        public int? ProductCategoryID { get; set; }
        public int? ManufactoryID { get; set; }
        public int? ModelID { get; set; }
        public int? ProductCategoryID2 { get; set; }
    }
}