using HNM.DataNC.ModelsNC.ModelsUtility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HNM.DataNC.ModelsNC.ModelsDTO
{
    public class ProductModelDTO : ModelBase
    {
        public int ProductModelID { get; set; }
        [Required(ErrorMessage = "Lựa chọn chủng loại")]
        public int? ProductManufactureID { get; set; }
        [Required(ErrorMessage = "Lựa chọn hãng sản xuất")]
        public int? ProductCategoryID { get; set; }
        public string Name { get; set; }
    }
    public class ProductModel_SearchByCategory_Result_DTO
    {
        public int ProductModelId { get; set; }
        public string Name { get; set; }
        public Nullable<int> ProductCount { get; set; }
    }
    public class ListProductModelSearchDTO : ModelBaseStatus
    {
        public ListProductModelSearchDTO()
        {
            Data = new List<ProductModel_SearchByCategory_Result_DTO>();
        }
        public IEnumerable<ProductModel_SearchByCategory_Result_DTO> Data { get; set; }
    }

}
