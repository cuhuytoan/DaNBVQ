using System;
using System.ComponentModel.DataAnnotations;

namespace HNM.DataNC.ModelsStore
{
    public class ProductManufacture_SearchByCategory_Result
    {
        [Key]
        public int NoItem { get; set; }
        public int ProductManufacture_ID { get; set; }
        public string Name { get; set; }
        public Nullable<int> ProductCount { get; set; }
    }
}
