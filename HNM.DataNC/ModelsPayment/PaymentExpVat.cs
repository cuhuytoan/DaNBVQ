﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace HNM.DataNC.ModelsPayment
{
    public partial class PaymentExpVat
    {
        public int PaymentExpVatId { get; set; }
        public string OrderCode { get; set; }
        public string CompanyName { get; set; }
        public string TaxCode { get; set; }
        public string BuyerName { get; set; }
        public string CompanyAddress { get; set; }
        public string ReceiveBillAddress { get; set; }
        public Guid? CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public Guid? LastEditBy { get; set; }
        public DateTime? LastEditDate { get; set; }
        public string Email { get; set; }
    }
}