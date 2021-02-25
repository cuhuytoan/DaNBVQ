using HNM.DataNC.ModelsNC.ModelsUtility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HNM.DataNC.ModelsStore
{
    public class HistoryShopingCart_Result
    {
        [Key]
        public Nullable<long> NoItem { get; set; }
        public Nullable<int> TotalRows { get; set; }
        public Nullable<int> TotalWaitConfirm { get; set; }
        public Nullable<int> TotalProcessing { get; set; }
        public Nullable<int> TotalSuccess { get; set; }
        public Nullable<int> TotalCancel { get; set; }
        public Nullable<int> ShoppingCartMasterId { get; set; }
        public Nullable<int> ShoppingCartDetailId { get; set; }
        public string ShopingCartCode { get; set; }
        public Nullable<int> ProductId { get; set; }
        public Nullable<int> ProductCategoryId { get; set; }
        public Nullable<int> ProductTypeId { get; set; }
        public Nullable<int> Qty { get; set; }
        public Nullable<Decimal> Price { get; set; }
        public Nullable<Decimal> DisCount { get; set; }
        public string Remark { get; set; }
        public Nullable<int> StatusCart { get; set; }
        public Nullable<Guid> UserId { get; set; }
        public Nullable<int> ProductBrandId { get; set; }
        public Nullable<int> DeliveryAddress { get; set; }
        public Nullable<Decimal> TotalAmount { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public Nullable<int> LocationId { get; set; }
        public string LocationName { get; set; }
        public Nullable<int>  DistrictId { get; set; }
        public string DistrictName { get; set; }
        public string Address { get; set; }
        public Nullable<bool> IsDefault { get; set; }
        public string ProductBrandName { get; set; }
        public Nullable<DateTime> CreateDate { get; set; }
        public Nullable<DateTime> LastEditDate { get; set; }
        public string ProductName { get; set; }
        public string UrlProduct { get; set; }
        public string UrlImageProduct { get; set; }
        public string FullUrlImageProduct => Utils.CdnPath() + $"/product/mainimages/small/{UrlImageProduct}";
        public string ReasonCancel { get; set; }
        public int? IsCancelByBuyer { get; set; }
    }
    public class CountHistoryShopingCart_Result
    {
        [Key]
        public int Id { get; set; }
        public Nullable<int> TotalWaitConfirm { get; set; }
        public Nullable<int> TotalProcessing { get; set; }
        public Nullable<int> TotalSuccess { get; set; }
        public Nullable<int> TotalCancel { get; set; }
    }
}
