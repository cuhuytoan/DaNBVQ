﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace HNM.DataNC.Models
{
    public partial class UserNotify
    {
        public Guid UserNotify_ID { get; set; }
        public int? UserNotifyType_ID { get; set; }
        public Guid? ToUserID { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public string URL { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool? Readed { get; set; }
        public int? ReadCount { get; set; }
    }
}