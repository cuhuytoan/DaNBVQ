namespace HNM.DataNC.ModelsFilter
{
    public class ProductFilter
    {      
        public string Keyword { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public int StatusTypeId { get; set; } = 4;
        public int? ProductTypeId { get; set; } = null;
        public int? ProductCategoryId { get; set; } = null;
        public int? ProductBrandId { get; set; } = null;
        public int? LocationId { get; set; }
        public int? ProductModelId { get; set; }
        public int? MainSystemId { get; set; }
        public int? AccDetailId { get; set; }
        public string PatchNum { get; set; }
        /// <summary>
        /// Label for qry materials
        /// </summary>
        public string ProductLabel { get; set; }
        public string MaterialManufactory { get; set; }
        /// <summary>
        /// Mới, Đã qua sử dụng, Thanh lý
        /// </summary>
        public string StatusMachine { get; set; }
        public int? ProductManufactureId { get; set; }
        //Related category for services
        public int? RelatedCategoryId { get; set; }
        public string UserId { get; set; }
    }
    public class ProductShopManFilter
    {
        public string Keyword { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public int? StatusTypeId { get; set; }
        public int? ProductTypeId { get; set; }
        public int? ProductCategoryId { get; set; }
        public string UserId { get; set; }
    }
}
