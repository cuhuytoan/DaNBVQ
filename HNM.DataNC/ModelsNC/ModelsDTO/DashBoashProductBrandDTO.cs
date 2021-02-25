using HNM.DataNC.ModelsNC.ModelsUtility;
using System;

namespace HNM.DataNC.ModelsNC.ModelsDTO
{
    public class DashBoashProductBrandDTO : ModelBase
    {
        public DashBoashProductBrandDTO()
        {
            Brand = new DashBoardBrand();
            Customer = new DashBoardCustomer();
            Post = new DashBoardPost();
            Order = new DashBoardOrder();
            Finance = new DashBoardFinance();
        }
        public DashBoardBrand Brand { get; set; }
        public DashBoardCustomer Customer { get; set; }
        public DashBoardPost Post { get; set; }
        public DashBoardOrder Order { get; set; }
        public DashBoardFinance Finance { get; set; }
    }
    public class DashBoardBrand
    {
        public string BrandSize { get; set; }
        public Nullable<int> BrandTotalPoint { get; set; } = 1;
        public Nullable<int> BrandTotalCare { get; set; } = 2;
        public Nullable<int> BrandTotalFollow { get; set; } = 3;
    }
    public class DashBoardCustomer
    {
        public Nullable<int> CustomerViewTotal { get; set; } = 4;
        public Nullable<int> CustomerViewToday { get; set; } = 5;
        public Nullable<int> CustomerViewYesterday { get; set; } = 6;
        public Nullable<int> CustomerViewOption { get; set; } = 7;
    }
    public class DashBoardPost
    {
        public Nullable<int> PostOption { get; set; } = 8;
        public Nullable<int> PostProduct { get; set; } = 9;
        public Nullable<int> PostPromo { get; set; } = 10;
        public Nullable<int> PostMemory { get; set; } = 11;
        public Nullable<int> PostLibrary { get; set; } = 12;
    }
    public class DashBoardOrder
    {
        public Nullable<int> OrderOption { get; set; } = 13;
        public Nullable<int> OrderDeliverySS { get; set; } = 14;
        public Nullable<int> OrderDelivery { get; set; } = 15;
        public Nullable<int> OrderReturn { get; set; } = 16;
    }
    public class DashBoardFinance
    {
        public Nullable<int> FinanceOption { get; set; } = 17;
        public Nullable<int> FinanceCollection { get; set; } = 18;
        public Nullable<int> FinanceCollectCus { get; set; } = 19;
        public Nullable<int> FinanceReturnCus { get; set; } = 20;
    }
}
