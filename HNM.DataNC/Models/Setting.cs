﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace HNM.DataNC.Models
{
    public partial class Setting
    {
        public int Setting_ID { get; set; }
        public string AdminName { get; set; }
        public string EmailSupport { get; set; }
        public string EmailOrder { get; set; }
        public string EmailSenderSMTP { get; set; }
        public string EmailSenderPort { get; set; }
        public bool? EmailSenderSSL { get; set; }
        public string EmailSender { get; set; }
        public string EmailSenderPassword { get; set; }
        public string Telephone { get; set; }
        public bool? AppStatus { get; set; }
        public int? Counter { get; set; }
        public int DefaultLanguage_ID { get; set; }
        public int DefaultSkin_ID { get; set; }
        public string WebsiteName { get; set; }
        public string MetaDescriptionDefault { get; set; }
        public string MetaKeywordsDefault { get; set; }
        public string MetaTitleDefault { get; set; }
        public string GoogleAnalytics { get; set; }
        public string OtherCode { get; set; }
        public string FacebookPageID { get; set; }
        public string FacebookAppID { get; set; }
        public string FacebookAdmin { get; set; }
        public string Domain { get; set; }
        public string TwitterID { get; set; }
        public string VBeeApp_ID { get; set; }
        public string VBeeUser_ID { get; set; }

        public virtual Language DefaultLanguage_ { get; set; }
        public virtual Language DefaultSkin_ { get; set; }
    }
}