﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace HNM.DataNC.Models
{
    public partial class SMSMessage
    {
        public int SMSMessage_ID { get; set; }
        public int? ProductBrand_ID { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public Guid? CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public Guid? LastEditBy { get; set; }
        public DateTime? LastEditDate { get; set; }
        public bool? CanDelete { get; set; }
    }
}