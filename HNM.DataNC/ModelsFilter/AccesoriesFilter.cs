namespace HNM.DataNC.ModelsFilter
{
    public class AccesoriesFilter : BaseFilter
    {
        public string Keyword { get; set; }
        public int? ProductCategory_ID { get; set; }
        public int? ProductManufactory_ID { get; set; }
        public int? Model_ID { get; set; }
        public int? Location_ID { get; set; }
        public int? ProductType_ID { get; set; }
        public int? MainSystem_ID { get; set; }
        public int? AccDetail_ID { get; set; }
        public string PatchNum { get; set; }
        public string Status { get; set; }
        public int? LocationID { get; set; }

    }
}
