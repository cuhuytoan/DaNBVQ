﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace HNM.DataNC.Models
{
    public partial class FCMClient
    {
        public Guid FCMClient_ID { get; set; }
        public string Device_ID { get; set; }
        public string OS { get; set; }
        public string Token { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? LastSeen { get; set; }
        public string UserName { get; set; }
        public Guid? UserID { get; set; }
        public string Topic { get; set; }
    }
}