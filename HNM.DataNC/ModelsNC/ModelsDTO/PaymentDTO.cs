using HNM.DataNC.ModelsNC.ModelsUtility;
using System;
using System.Collections.Generic;
using System.Text;

namespace HNM.DataNC.ModelsNC.ModelsDTO
{
    public class PaymentDTO
    {

    }
    public class ServicesDTO
    {
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public int? ProductQty { get; set; }
        public int? ReceiveFromCus { get; set; }
        public int? ReviewPerDay { get; set; }
        public bool? RelateProduct { get; set; }
        public bool? DisplayPriorityTopPage { get; set; }
        public bool? PromoInDay { get; set; }
        public bool? PriorityArticle { get; set; }
        public bool? SupportSale { get; set; }
        public int? MorePostPrice { get; set; }
        public int? PricePerMonth { get; set; }
        public DateTime? CreateDate { get; set; }
        public Guid? CreateBy { get; set; }
        public DateTime? LastEditDate { get; set; }
        public Guid? LastEditBy { get; set; }
        public int? Status { get; set; }
    }
    public class DiscountConfigDTO
    {
        public int Discount_ID { get; set; }
        public int? Month { get; set; }
        public decimal? Percent_Discount { get; set; }
        public bool? Status { get; set; }
    }
    public class PaymentPackageDTO
    {
        public List<ServicesDTO> Package { get; set; }
        public List<DiscountConfigDTO> MonthDisCount { get; set; }
        public int ProductBrandTypeId { get; set; }
        public int MonthRemain { get; set; }
        public int MonthPrice { get; set; }
        public decimal? TotalDeduct { get; set; }
    }
    public class PaymentBankingDTO
    {
        public int Id { get; set; }
        public int PaymentTypeId { get; set; }
        public string BankCode { get; set; }
        public string BankName { get; set; }
    }
    public class OrderDetailDTO :ModelBase
    {
        public string OrderDetail_ID { get; set; }
        public string Order_ID { get; set; }
        public string ProductName { get; set; }
        public int? ProductBrand_ID { get; set; }
        public string ProductBrandName { get; set; }
        public string Unit { get; set; }
        public int? Quatity { get; set; }
        public decimal? Price { get; set; }
        public decimal? VAT { get; set; }
        public decimal? Sum { get; set; }
        public decimal? DisCount { get; set; }
        public decimal? Total { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string OrderCode { get; set; }
        public int MonthRemain { get; set; }
        public int MonthPrice { get; set; }
        public decimal? TotalDeduct { get; set; }
        public int PaymentStatus_ID { get; set; }
    }
    public class OrderDTO :ModelBase
    {
        public string Order_ID { get; set; }
        public int? Service_ID { get; set; }
        public decimal? Sum { get; set; }
        public decimal? DisCount { get; set; }
        public decimal? Total { get; set; }
        public int? Discount_ID { get; set; }
        public int? PaymentType_ID { get; set; }
        public int? PaymentStatus_ID { get; set; }
        public int? ProductBrand_ID { get; set; }
        public string ProductBrandName { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerEmail { get; set; }
        public string SalesUserName { get; set; }
        public string SalesName { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string LastEditBy { get; set; }
        public DateTime? LastEditDate { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string JsonMetaData { get; set; }
        public decimal? VAT { get; set; }
        public string OrderCode { get; set; }
    }
    public class UpgrageBrandPackageDTO :ModelBase
    {
        public int ProductBrandId { get; set; }
        public int ServiceId { get; set; }
        public int Discount_ID { get; set; }        
    }
    public class PostUrlVNPay
    {
        public string OrderCode { get; set; }
        public string BankCode { get; set; }
        public string IPAddress { get; set; }
        public int PaymentTypeId { get; set; }
        public int PostType { get; set; } // 0 -app 1 --web
    }
    public class PostPaymentVAT:ModelBase
    {
        public string OrderCode { get; set; }
        public string CompanyName { get; set; }
        public string TaxCode { get; set; }
        public string BuyerName { get; set; }
        public string CompanyAddress { get; set; }
        public string ReceiveBillAddress { get; set; }
        public string Email { get; set; }
    }
    public class OrderRemain
    {
        public int MonthRemain { get; set; }
        public int MonthPrice { get; set; }
        public decimal? TotalDeduct { get; set; }
    }
    public class OrderHistory
    {
        public string OrderDetail_ID { get; set; }
        public string Order_ID { get; set; }
        public string ProductName { get; set; }
        public int? ProductBrand_ID { get; set; }
        public string ProductBrandName { get; set; }
        public string Unit { get; set; }
        public int? Quatity { get; set; }
        public decimal? Price { get; set; }
        public decimal? VAT { get; set; }
        public decimal? Sum { get; set; }
        public decimal? DisCount { get; set; }
        public decimal? Total { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string OrderCode { get; set; }
        public string OrderImage { get; set; }
        public int PaymentStatus_ID { get; set; }
        public int PaymentType_ID { get; set; }
        public DateTime? LastEditDate { get; set; }
        public int? GroupByMonth => LastEditDate?.Month;
        public string GroupByMonthYear => LastEditDate?.Month + "/" + LastEditDate?.Year;
    }
    public class OrderHistoryGroup
    {
        public OrderHistoryGroup()
        {
            data = new List<OrderHistory>();
        }
        public string Monthly { get; set; }
        public List<OrderHistory> data { get; set; }
    }
}
