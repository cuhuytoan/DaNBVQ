﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace HNM.DataNC.Models
{
    public partial class ProductPicture
    {
        public Guid ProductPicture_ID { get; set; }
        public Guid? Job_ID { get; set; }
        public int? Product_ID { get; set; }
        public string Image { get; set; }
        public bool? Active { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? Job1Flag { get; set; }
    }
}