using HNM.DataNC.ModelsNC.ModelsUtility;
using System.Collections.Generic;

namespace HNM.DataNC.ModelsNC.ModelsDTO
{
    public class HomeMenuDTO
    {
        public int MenuId { get; set; }
        public string Name { get; set; }
        public string MenuIcon { get; set; }
        public string URL { get; set; }
        public string Type { get; set; }
        public string MetaData { get; set; }
        public int? Sort { get; set; }
        public int? ParentCategoryId { get; set; }
        public int? ProductTypeId { get; set; }
        public int? StatusTypeId { get; set; }
        public int? ProductCount { get; set; }


    }
    public class ListHomeMenuDTO : ModelBaseStatus
    {
        public ListHomeMenuDTO()
        {
            Data = new List<HomeMenuDTO>();
        }
        public IEnumerable<HomeMenuDTO> Data { get; set; }
    }
}
