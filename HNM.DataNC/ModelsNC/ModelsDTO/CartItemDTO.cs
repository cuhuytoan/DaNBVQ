using System;
using System.Collections.Generic;
using System.Text;

namespace HNM.DataNC.ModelsNC.ModelsDTO
{
    public class CartItemDTO
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public int BrandId { get; set; }
        public int ProductCategoryId { get; set; }
        public int ProductTypeId { get; set; }
        public int Amount { get; set; }
    }
}
