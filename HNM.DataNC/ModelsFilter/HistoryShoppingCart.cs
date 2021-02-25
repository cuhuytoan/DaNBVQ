using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace HNM.DataNC.ModelsFilter
{
    public class HistoryShoppingCart
    {
        public string UserId { get; set; }
        public int? ProductBrandId { get; set; }
        public int? StatusCart { get; set; }
        public int? ShopingCartMasterId { get; set; }
        public int? ShopingCartDetailId { get; set; }
        public string ShopingCartCode { get; set; }
        public int? PageSize { get; set; }
        public int? CurrentPage { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
    public class CountHistoryShoppingCart
    {
        public string UserId { get; set; }
        public int? ProductBrandId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string ShopingCartCode { get; set; }
    }
}
