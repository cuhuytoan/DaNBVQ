using System;
using System.ComponentModel.DataAnnotations;

namespace HNM.DataNC.ModelsStore
{
    public class ProductModel_SearchByCategory_Result
    {
        [Key]
        public int NoItem { get; set; }
        public int ProductModel_ID { get; set; }
        public string Name { get; set; }
        public string URL { get; set; }
        public Nullable<int> ProductCount { get; set; }
    }
}
