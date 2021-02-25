using HNM.DataNC.ModelsNC.ModelsUtility;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Web;

namespace HNM.DataNC.ModelsStore
{
    public class Article_Search_Result
    {
        [Key]
        public int NoItem { get; set; }
        public int Article_ID { get; set; }
        public Nullable<int> Store_ID { get; set; }
        public Nullable<int> ArticleCategory_ID { get; set; }
        public Nullable<int> ArticleType_ID { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Image { get; set; }
        /// <summary>
        /// Full Url Link image
        /// </summary>
        public string FullUrlImage => Utils.CloudPath() + $"/article/mainimages/original/{Image}";
        public string ImageDescription { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Author = "";
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
        public string UrlVideo => HttpUtility.HtmlDecode(Regex.Match(Content, "&lt;iframe.*?&lt;/iframe&gt;").ToString());

        /// <summary>
        /// Full Url Link
        /// </summary>
        public string FullUrl => $"https://hanoma.vn/bai-viet/{URL}";
        public string Tag { get; set; }
        public string Keyword { get; set; }
        public Nullable<bool> AllowComment { get; set; }
        public Nullable<int> StatusType_ID { get; set; }
        public Nullable<int> TotalCount { get; set; }

    }  
}
