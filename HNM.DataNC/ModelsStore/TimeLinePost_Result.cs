using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HNM.DataNC.ModelsStore
{
    public class TimeLinePost_Result
    {
        [Key]
        public int Id { get; set; }
        public Nullable<int> TotalRows { get; set; }
        public Nullable<int> ProductTypeId { get; set; }
        public string TypeName { get; set; }
        public Nullable<int> ProductId { get; set; }
        public Nullable<int> ProductCategoryId { get; set; }
        public string ProductFullUrl { get; set; }
        public Nullable<int> LibraryId { get; set; }
        public Nullable<int> LibraryCategoryId { get; set; }
        public string LibraryFullUrl { get; set; }
        public Nullable<int> CVId { get; set; }
        public Nullable<int> CVCategoryId { get; set; }
        public string CVFullUrl { get; set; }
        public Nullable<int> RecId { get; set; }
        public Nullable<int> RecCategoryId { get; set; }
        public string RecFullUrl { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string MainImageUrl { get; set; }
        public Nullable<int> StatusTypeId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastEditDate { get; set; }
    }
}
