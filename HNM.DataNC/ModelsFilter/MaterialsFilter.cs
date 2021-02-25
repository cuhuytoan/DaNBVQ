namespace HNM.DataNC.ModelsFilter
{
    public class MaterialsFilter : BaseFilter
    {
        public int? ProductCategoryID { get; set; }
        public int? ProductManufactoryID { get; set; }
        public int? ModelID { get; set; }
        public int? LocationID { get; set; }
        public int? ProductTypeID { get; set; }
        public string PatchNum { get; set; }
        public string Status { get; set; }
    }
}
