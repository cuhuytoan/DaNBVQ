namespace HNM.DataNC.ModelsFilter
{
    public class BaseFilter
    {
        public string Keyword { get; set; }        
        public int? Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public int StatusTypeId { get; set; } = 4;
    }
}
