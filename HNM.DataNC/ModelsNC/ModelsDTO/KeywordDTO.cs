using HNM.DataNC.ModelsNC.ModelsUtility;
using System.Collections.Generic;

namespace HNM.DataNC.ModelsNC.ModelsDTO
{
    public class KeywordDTO
    {
        public int MetaKeywordId { get; set; }
        public string Name { get; set; }
    }
    public class ListKeywordDTO : ModelBaseStatus
    {
        public ListKeywordDTO()
        {
            Data = new List<KeywordDTO>();
        }
        public IEnumerable<KeywordDTO> Data { get; set; }
    }
}
