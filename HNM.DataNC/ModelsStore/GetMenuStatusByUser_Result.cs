using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HNM.DataNC.ModelsStore
{
    public class GetMenuStatusByUser_Result
    {
        [Key]
        public int Menu_ID { get; set; }
        public int? Parent_ID { get; set; }
        public int? Language_ID { get; set; }
        public string Name { get; set; }
        public string URL { get; set; }
        public int? Sort { get; set; }
        public bool? Active { get; set; }
        public Guid? CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public Guid? LastEditedBy { get; set; }
        public DateTime? LastEditedDate { get; set; }
        public string MenuIcon { get; set; }
        public string Type { get; set; }
        public string MetaData { get; set; }
        public int? ParentCategory_ID { get; set; }
        public int? ProductTypeId { get; set; }
        public int? StatusTypeId { get; set; }
        public int? ProductCount { get; set; }
    }
}
