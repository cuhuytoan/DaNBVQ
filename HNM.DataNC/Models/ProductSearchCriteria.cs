﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace HNM.DataNC.Models
{
    public partial class ProductSearchCriteria
    {
        public int ProductSearchCriteria_ID { get; set; }
        public int? ProductProperty_ID { get; set; }
        public string Display { get; set; }
        public string Formula { get; set; }
        public int? Sort { get; set; }

        public virtual ProductProperty ProductProperty_ { get; set; }
    }
}