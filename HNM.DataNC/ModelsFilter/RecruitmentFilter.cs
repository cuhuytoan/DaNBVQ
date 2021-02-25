namespace HNM.DataNC.ModelsFilter
{
    public class RecruitmentFilter : BaseFilter
    {
        public int? RecruitmentCategoryId { get; set; }
        public int? ProductCategoryId { get; set; }
        public int? LocationId { get; set; }
        public int? FromPrice { get; set; }
        public int? ToPrice { get; set; }
        public int? Experience { get; set; }
    }
}
