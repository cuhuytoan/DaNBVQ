namespace HNM.DataNC.ModelsFilter
{
    public class CVFilter : BaseFilter
    {
        public int? CareerCategoryId { get; set; }
        public int? ProductCategoryId { get; set; }
        public string ProvinceName { get; set; }
        public int? YearOfExprience { get; set; }
        public int? MinSalary { get; set; }
        public int? MaxSalary { get; set; }

    }
}
