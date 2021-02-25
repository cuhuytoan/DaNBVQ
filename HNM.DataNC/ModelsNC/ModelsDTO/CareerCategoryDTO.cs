using HNM.DataNC.ModelsNC.ModelsUtility;
using System;

namespace HNM.DataNC.ModelsNC.ModelsDTO
{
    public class CareerCategoryDTO : ModelBase
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public int? LanguageId { get; set; }
        public string Name { get; set; }
        public string URL { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public int? Sort { get; set; }
        public int? Counter { get; set; }
        public bool? DisplayMenu { get; set; }
        public bool? Active { get; set; }
        public bool? CanDelete { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
        public Guid? CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public Guid? LastEditedBy { get; set; }
        public DateTime? LastEditedDate { get; set; }
        public bool? HasChildren { get; set; }
    }
}
