﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace HNM.DataNC.ModelsPayment
{
    public partial class Services
    {
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public int? ProductQty { get; set; }
        public int? ReceiveFromCus { get; set; }
        public int? ReviewPerDay { get; set; }
        public bool? RelateProduct { get; set; }
        public bool? DisplayPriorityTopPage { get; set; }
        public bool? PromoInDay { get; set; }
        public bool? PriorityArticle { get; set; }
        public bool? SupportSale { get; set; }
        public int? MorePostPrice { get; set; }
        public int? PricePerMonth { get; set; }
        public DateTime? CreateDate { get; set; }
        public Guid? CreateBy { get; set; }
        public DateTime? LastEditDate { get; set; }
        public Guid? LastEditBy { get; set; }
        public int? Status { get; set; }
    }
}