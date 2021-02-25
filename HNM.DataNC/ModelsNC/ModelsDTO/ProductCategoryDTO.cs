using HNM.DataNC.Models;
using HNM.DataNC.ModelsNC.ModelsUtility;
using System.Collections.Generic;

namespace HNM.DataNC.ModelsNC.ModelsDTO
{
    public class ProductCategoryDTO
    {
        public int ProductCategoryId { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public string URL { get; set; }
        public string Icon { get; set; }
        public string IconUrl => this.Icon != null ? Utils.CloudPath() + $"/DataMobile/Categories/Icons/{Icon}" : null;
        public int? Sort { get; set; }
        public bool? Active { get; set; }
        public int? ProductCount { get; set; } = 0;
        public List<ProductCategory> SubData { get; set; }
    }
    public class ListProductCategoryDTO : ModelBaseStatus
    {
        public IEnumerable<ProductCategoryDTO> Data { get; set; }
        public ListProductCategoryDTO()
        {
            Data = new List<ProductCategoryDTO>();
        }

    }
    public class ListAllProductCategoryDTO : ModelBaseStatus
    {
        public ListAllProductCategoryDTO()
        {
            Parent = new ProductCategoryDTO();
            Child = new List<ProductCategoryDTO>();
        }
        public ProductCategoryDTO Parent { get; set; }
        public IEnumerable<ProductCategoryDTO> Child { get; set; }
    }
    public class ProductCategoryHighLightDTO
    {
        public int Id { get; set; }
        public int? Position { get; set; }
        public bool? Active { get; set; }
        public string AreaTitle { get; set; }
        public string AreaName { get; set; }
        public int? ProductCategoryId { get; set; }
    }

    public class ListHighLightCategoryDTO : ModelBaseStatus
    {
        public IEnumerable<ProductCategoryHighLightDTO> Data { get; set; }
        public ListHighLightCategoryDTO()
        {
            Data = new List<ProductCategoryHighLightDTO>();
        }
    }
    public class ProductCategoryTwoLevelDTO : ModelBaseStatus
    {
        public string ParentCategoryName { get; set; }
        public string ParentCategoryUrl { get; set; }
        public List<ProductCategory> data { get; set; }
    }
}
