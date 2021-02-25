using HNM.DataNC.ModelsNC.ModelsUtility;
using HNM.DataNC.ModelsStore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HNM.DataNC.ModelsNC.ModelsDTO
{
    class ShoppingCartDTO
    {
    }
    public class PostShoppingCart : ModelBase
    {
        public PostShoppingCart()
        {
            LstShopCartDetail = new List<ShoppingCartDetailDTO>();
        }
        public int Id { get; set; }
        public int ProductBrandID { get; set; }
        public int DeliveryAddress { get; set; }
        public decimal TotalAmmout { get; set; }
        public List<ShoppingCartDetailDTO> LstShopCartDetail { get; set; }
    }
    public class ShopingCartHistory :ModelBase
    {

    }

    public class ShoppingCartDetailDTO :ModelBase
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public string Remark { get; set; }
        public int StatusCart { get; set; }
        public string ReasonCancel { get; set; }
        public int? IsCancelByBuyer { get; set; }
    }
    public class DeliveryAddressDTO :ModelBase
    {
        public int Id { get; set; }        
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public int? LocationId { get; set; }
        public int? DistrictId { get; set; }
        public string Address { get; set; }
        public bool? IsDefault { get; set; }
        public string LocationName { get; set; }
        public string DistrictName { get; set; }
    }
    public class HistoryShopCart :ModelBase
    {
        public HistoryShopCart()
        {
            Data = new List<HistoryShopingCart_Result>();
            HistoryCount = new CountHistoryShopingCart_Result();
        }
        public List<HistoryShopingCart_Result> Data { get; set; }
        public CountHistoryShopingCart_Result HistoryCount { get; set; }
    }
}
