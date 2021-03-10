using HNM.DataNC.ModelsNC.ModelsUtility;
using System;

namespace HNM.DataNC.ModelsNC.ModelsDTO
{
    public class LibraryDTO : ModelBase
    {
        public int Library_ID { get; set; }
        public int? LibraryCategory_ID { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Image { get; set; }
        public string FullUrlImage => Utils.CloudPath() + $"/Library/mainimages/original/{Image}";
        public string ImageDescription { get; set; }
        public string BannerImages { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public string URL { get; set; }
        public bool? AllowComment { get; set; }
        public string LastEditDate { get; set; }
    }
    public class LibraryCategoryDTO : ModelBase
    {
        /// <summary>
        /// Library Category
        /// </summary>
        public int LibraryCategory_ID { get; set; }
        public string Name { get; set; }
        public string URL { get; set; }
        public string Description { get; set; }
        public int? Sort { get; set; }
        /// <summary>
        /// Full Url
        /// </summary>
        public string FullUrl => $"https://daninhbinhvinhquang.vn/thu-vien-kien-thuc/{URL}";
        public string LastEditDate { get; set; }
    }

    public class PostLibrary
    {
        public int? LibraryCategory_ID { get; set; }
        public int? LibraryType_ID { get; set; }
        public int? Library_ID { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public int? Counter { get; set; }
        public Guid? CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public Guid? LastEditBy { get; set; }
        public DateTime? LastEditDate { get; set; }
        public string URL { get; set; }
        public string Tag { get; set; }
        public string Keyword { get; set; }
        public bool? AllowComment { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
        public int? StatusType_ID { get; set; }
        public string AuthorPhone { get; set; }
        public string AuthorEmail { get; set; }
        public string AuthorJob { get; set; }
        public string AuthorCompany { get; set; }
        public int? ProductCategoryID { get; set; }
        public int? ManufactoryID { get; set; }
        public int? ModelID { get; set; }
        public int? ProductCategoryID2 { get; set; }
    }

    public class PostLibraryDTO : ModelBase
    {
        public PostLibraryDTO()
        {
            Data = new PostLibrary();
            ImgLogo = new ImageUploadDTO();
        }
        public PostLibrary Data { get; set; }
        public ImageUploadDTO ImgLogo { get; set; }
    }
}
