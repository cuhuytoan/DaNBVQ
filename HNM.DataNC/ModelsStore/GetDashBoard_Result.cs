using System;
using System.ComponentModel.DataAnnotations;

namespace HNM.DataNC.ModelsStore
{
    public class GetDashBoard_Result
    {
        [Key]
        public Nullable<int> ProductBrandID { get; set; }
        public string BrandSize { get; set; }
        public Nullable<int> BrandTotalPoint { get; set; }
        public Nullable<int> BrandTotalCare { get; set; }
        public Nullable<int> BrandTotalFollow { get; set; }
        public Nullable<int> CustomerViewTotal { get; set; }
        public Nullable<int> CustomerViewToday { get; set; }
        public Nullable<int> CustomerViewYesterday { get; set; }
        public Nullable<int> CustomerViewOption { get; set; }
        public Nullable<int> PostOption { get; set; }
        public Nullable<int> PostProduct { get; set; }
        public Nullable<int> PostPromo { get; set; }
        public Nullable<int> PostMemory { get; set; }
        public Nullable<int> PostLibrary { get; set; }
        public Nullable<int> OrderOption { get; set; }
        public Nullable<int> OrderDeliverySS { get; set; }
        public Nullable<int> OrderDelivery { get; set; }
        public Nullable<int> OrderReturn { get; set; }
        public Nullable<int> FinanceOption { get; set; }
        public Nullable<int> FinanceCollection { get; set; }
        public Nullable<int> FinanceCollectCus { get; set; }
        public Nullable<int> FinanceReturnCus { get; set; }


    }
}
