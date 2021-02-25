using HNM.DataNC.ModelsNC.ModelsUtility;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace HNM.DataNC.ModelsNC.ModelsDTO
{
    public class ArticleDTOExt
    {
        public int ArticleID { get; set; }
        public int? ArticleCategory_ID { get; set; }
        /// <summary>
        /// ArticleTypeID =1 tin tức =2 video
        /// </summary>
        public int? ArticleTypeID { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Image { get; set; }
        /// <summary>
        /// Full link Image
        /// </summary>
        public string FullUrlImage => Utils.CloudPath() + $"/article/mainimages/original/{Image}";
        public string ImageDescription { get; set; }
        public string BannerImages { get; set; }
        /// <summary>
        /// Mô tả title
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Nội dung
        /// </summary>
        public string Content { get; set; }
        public string UrlVideo => HttpUtility.HtmlDecode(Regex.Match(Content, "&lt;iframe.*?&lt;/iframe&gt;").ToString());
        /// <summary>
        /// Tác giả
        /// </summary>
        public string Author { get; set; }
        public string URL { get; set; }
        public bool? AllowComment { get; set; }
        /// <summary>
        /// Ngày cập nhật cuối cùng
        /// </summary>
        public DateTime LastEditDate { get; set; }
    }
}
