using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;


namespace HNM.DataNC.ModelsStore
{
   
    public partial class Video_Search_Mobile_Result
    {
        public int NoItem { get; set; }
        [Key]
        public int Article_ID { get; set; }
        public Nullable<int> Store_ID { get; set; }
        public Nullable<int> VideoCategory_ID { get; set; }
        public Nullable<int> ArticleType_ID { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Image { get; set; }
        public string ImageDescription { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<int> Counter { get; set; }
        public Nullable<System.Guid> CreateBy { get; set; }
        public string UserName { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.Guid> LastEditBy { get; set; }
        public Nullable<System.DateTime> LastEditDate { get; set; }
        public string URL { get; set; }
        public string Tag { get; set; }
        public string Keyword { get; set; }
        public Nullable<bool> AllowComment { get; set; }
        public Nullable<int> StatusType_ID { get; set; }
        public Nullable<int> TotalCount { get; set; }
    }
}
