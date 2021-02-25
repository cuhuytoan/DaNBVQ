using HNM.DataNC.ModelsNC.ModelsUtility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HNM.DataNC.ModelsStore
{
    public class GetArticleHome_Result
    {
		[Key]
		public int Id { get; set; }
		public int ArticleCategoryId { get; set; }
		public string ArticleCategoryName { get; set; }
		public int ArticleId { get; set; }
		public string Title { get; set; }
		public string Image { get; set; }
		public string Author { get; set; }
		public DateTime LastEditDate { get; set; }
		public string FullUrlImage => Utils.CloudPath() + $"/article/mainimages/original/{Image}";
	}
}
