using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HNM.DataNC.ModelsNC.ModelsDTO;

namespace CMS_V3.ViewModel
{
    public class ArticleViewModel
    {
        public ArticleViewModel()
        {
            highlightArticle = new ArticleDTO();

            lstBlock1 = new List<ArticleDTO>();

            lstBlock2 = new List<ArticleDTO>();

            lstHotArticle = new List<ArticleShortInfo>();

            lstNews = new List<ArticleShortInfo>();

            lstBidding = new List<ArticleShortInfo>();

            ArticleCategoryHotArticle = new List<ArticleCategoryDTO>();

            ArticleCategoryNew = new List<ArticleCategoryDTO>();

            ArticleCategoryBidding = new List<ArticleCategoryDTO>();

         }
        public ArticleDTO highlightArticle { get; set; }

        public List<ArticleDTO> lstBlock1 { get; set; }

        public List<ArticleDTO> lstBlock2 { get; set; }

        public List<ArticleShortInfo> lstHotArticle { get; set; }

        public List<ArticleShortInfo> lstNews { get; set; }

        public List<ArticleShortInfo> lstBidding { get; set; }

        public List<ArticleCategoryDTO> ArticleCategoryHotArticle { get; set; }

        public List<ArticleCategoryDTO> ArticleCategoryNew { get; set; }

        public List<ArticleCategoryDTO> ArticleCategoryBidding { get; set; }

    }
}
