using HNM.DataNC.ModelsNC.ModelsUtility;
using HNM.DataNC.ModelsStore;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;

namespace HNM.DataNC.ModelsNC.ModelsDTO
{
    public class ArticleDTO : ModelBase
    {
        /// <summary>
        /// Article ID
        /// </summary>
        public int Article_ID { get; set; }
        public int? ArticleCategory_ID { get; set; }
        /// <summary>
        /// ArticleTypeID =1 tin tức =2 video
        /// </summary>
        public int? ArticleType_ID { get; set; }
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
        public int VideoCategory_ID { get; set; }

    }

    public class ArticleCategoryDTO : ModelBase
    {
        /// <summary>
        /// ArticleCategory ID
        /// </summary>
        public int ArticleCategory_ID { get; set; }
        /// <summary>
        /// Name of Category
        /// </summary>
        public string Name { get; set; }
        public string URL { get; set; }
        public string Description { get; set; }
        public int? Sort { get; set; }
        public string MenuIconUrl { get; set; }
        /// <summary>
        /// Full Url
        /// </summary>
        public string FullUrl => $"https://daninhbinhvinhquang.vn/chuyen-muc/{URL}";
        public string LastEditDate { get; set; }
    }
    public class VideoCategoryDTO : ModelBase
    {
        /// <summary>
        /// ArticleCategory ID
        /// </summary>
        public int VideoCategory_ID { get; set; }
        /// <summary>
        /// Name of Category
        /// </summary>
        public string Name { get; set; }
        public string URL { get; set; }
        public string Description { get; set; }
        public int? Sort { get; set; }
        public string MenuIconUrl { get; set; }
        /// <summary>
        /// Full Url
        /// </summary>
        public string FullUrl => $"https://daninhbinhvinhquang.vn/chuyen-muc/{URL}";
        public string LastEditDate { get; set; }
    }
    public class ArticleVoiceDTO : ModelBase
    {
        /// <summary>
        /// Article Voice ID
        /// </summary>
        public int ArticleVoice_ID { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public int? Article_ID { get; set; }
        public int? Sort { get; set; }
        public string LastEditDate { get; set; }
        /// <summary>
        /// Link Url video can load
        /// </summary>
        public string FullUrl => Utils.CloudPath() + $"/article/audio/{FileName}";
    }
    public class HomeArticleDTO : ModelBase
    {        
        public ArticleShortInfo ArticleHightLight { get; set; }
        public List<ArticleCategoryDTO> ArticleCategory { get; set; }
        public List<HomeArticleByCateDTO> ListArticleByCate { get; set; }
    }
    public class HomeArticleByCateDTO
    {
        public HomeArticleByCateDTO()
        {
            Data = new List<ArticleShortInfo>();
        }
        public int ArticleCategoryId { get; set; }
        public string ArticleCategoryName { get; set; }
        public IEnumerable<ArticleShortInfo> Data { get; set; }
    }
    public class ArticleShortInfo
    {
        public int ArticleId { get; set; }    
        public int ArticleCategoryId { get; set; }
        public string Title { get; set; }        
        public string Image { get; set; }
        public string FullUrlImage => Utils.CloudPath() + $"/article/mainimages/original/{Image}";
        public string Author { get; set; }
        public DateTime LastEditDate { get; set; }
        public string Description { get; set; }
        public string URL { get; set; }
        
    }
    public class ArticleSearchDTO
    {
        public int TotalRecord { get; set; }
        public int TotalPage { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public IEnumerable<Article_Search_Result> Data { get; set; }
        public ArticleSearchDTO()
        {
            Data = new List<Article_Search_Result>();
        }
    }
    public class VideoShortInfo
    {
        public int ArticleId { get; set; }
        public int ArticleCategoryId { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string FullUrlImage => Utils.CloudPath() + $"/article/mainimages/original/{Image}";
        public string Content { get; set; }
        //public string UrlVideo => HttpUtility.HtmlDecode(Regex.Match(Content, "&lt;iframe.*?&lt;/iframe&gt;").ToString());
        //public string UrlVideo => HttpUtility.HtmlDecode(Regex.Match(Content, "www.youtube.com/embed/.*?\"").ToString());
        public string UrlVideo => HttpUtility.HtmlDecode(Regex.Match(Content, "embed/(.*?)\"").Groups[1].ToString());
        public string Author { get; set; }
        public DateTime LastEditDate { get; set; }
        public string Description { get; set; }
        public string URL { get; set; }
        public int TotalCount { get; set; }
    }
}
