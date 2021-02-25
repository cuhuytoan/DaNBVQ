using HNM.DataNC.ModelsNC.ModelsUtility;
using System.Collections.Generic;

namespace HNM.DataNC.ModelsNC.ModelsDTO
{
    public class RecruitmentCategoryDTO
    {
        public int RecruitmentCategoryId { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public string URL { get; set; }
        public string Icon { get; set; }
        public string IconUrl => this.Icon != null ? Utils.CloudPath() + $"/DataMobile/Categories/Icons/{Icon}" : null;
        public int? Sort { get; set; }
        public bool? Active { get; set; }
    }
    public class ListRecruitmentCategoryDTO : ModelBaseStatus
    {
        public IEnumerable<RecruitmentCategoryDTO> Data { get; set; }
        public ListRecruitmentCategoryDTO()
        {
            Data = new List<RecruitmentCategoryDTO>();
        }

    }
}
