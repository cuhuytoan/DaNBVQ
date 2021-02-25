namespace HNM.DataNC.ModelsFilter
{
    public class LibraryFilter : BaseFilter
    {
        public int? LibraryCategoryID { get; set; }
        /// <summary>
        /// Parent Product Category
        /// </summary>
        public int? ProductCategoryID { get; set; }
        public int? ManufactoriesID { get; set; }
        public int? ModelID { get; set; }
        /// <summary>
        /// Child Product Category
        /// </summary>
        public int? ProductCategoryID2 { get; set; }
        public string CreateBy { get; set; }
    }
}
