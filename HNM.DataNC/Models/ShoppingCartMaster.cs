﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace HNM.DataNC.Models
{
    public partial class ShoppingCartMaster
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public int ProductBrandId { get; set; }
        public int? DeliveryAddress { get; set; }
        public decimal? TotalAmount { get; set; }
        public DateTime? CreateDate { get; set; }
        public Guid? CreateBy { get; set; }
        public DateTime? LastEditDate { get; set; }
        public Guid? LastEditBy { get; set; }
    }
}