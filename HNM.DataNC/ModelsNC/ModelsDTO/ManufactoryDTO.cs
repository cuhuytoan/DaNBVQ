using HNM.DataNC.ModelsNC.ModelsUtility;
using System;
using System.Collections.Generic;

namespace HNM.DataNC.ModelsNC.ModelsDTO
{
    public class ManufactoryDTO
    {
    }
    public class ProductManufacture_SearchByCategory_Result_DTO
    {
        public int ProductManufactureId { get; set; }
        public string Name { get; set; }
        public Nullable<int> ProductCount { get; set; }
    }
    public class ListManufactureSearchDTO : ModelBaseStatus
    {
        public ListManufactureSearchDTO()
        {
            Data = new List<ProductManufacture_SearchByCategory_Result_DTO>();
        }
        public IEnumerable<ProductManufacture_SearchByCategory_Result_DTO> Data { get; set; }
    }
}
