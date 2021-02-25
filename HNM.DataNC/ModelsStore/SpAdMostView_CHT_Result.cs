using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HNM.DataNC.ModelsStore
{
    public class SpAdMostView_CHT_Result
    {
        [Key]
        public int Advertising_ID { get; set; }
        public Nullable<int> AdvertisingBlock_ID { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string Ext { get; set; }
        public Nullable<int> Width { get; set; }
        public Nullable<int> Height { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public string URL { get; set; }
        public Nullable<int> Sort { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<bool> IsCode { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<int> Counter { get; set; }
        public Nullable<System.Guid> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.Guid> LastEditBy { get; set; }
        public Nullable<System.DateTime> LastEditDate { get; set; }
        public Nullable<int> AdMostViewType { get; set; }
        public Nullable<int> AdMostViewCate { get; set; }
        public Nullable<int> AdMostViewChildCate { get; set; }
        public string FormId { get; set; }
        public Nullable<int> ParamId { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public Nullable<int> ProductTypeId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryName2 { get; set; }
        public string AdMostViewTypeName { get; set; }
        

    }
}
