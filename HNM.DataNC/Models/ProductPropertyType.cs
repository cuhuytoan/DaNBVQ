﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace HNM.DataNC.Models
{
    public partial class ProductPropertyType
    {
        public ProductPropertyType()
        {
            ProductProperty = new HashSet<ProductProperty>();
        }

        public int ProductPropertyType_ID { get; set; }
        public string Name { get; set; }
        public string TemplateDisplay { get; set; }
        public string TemplateEdit { get; set; }

        public virtual ICollection<ProductProperty> ProductProperty { get; set; }
    }
}