﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace HNM.DataNC.Models
{
    public partial class ProductBrandBlock
    {
        public int ProductBrandBlock_id { get; set; }
        public int? ProductBrandCategory_ID { get; set; }
        public int? Language_ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public bool? Active { get; set; }
        public int? Sort { get; set; }
        public int? Style_ID { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string LastEditedBy { get; set; }
        public DateTime? LastEditedDate { get; set; }
        public bool? Lock { get; set; }
    }
}