﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace HNM.DataNC.Models
{
    public partial class ExchangeRate
    {
        public int ExchangeRate_ID { get; set; }
        public string CurrencyCode { get; set; }
        public decimal? Buy { get; set; }
        public decimal? Sell { get; set; }
    }
}