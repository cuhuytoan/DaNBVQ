﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace HNM.DataNC.Models
{
    public partial class LogEvent
    {
        public Guid LogEvent_ID { get; set; }
        public string EventObject { get; set; }
        public string Object_ID { get; set; }
        public int EventCode { get; set; }
        public string Message { get; set; }
        public string OldContent { get; set; }
        public string Content { get; set; }
        public Guid? UserStart { get; set; }
        public Guid? UserApproved { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}