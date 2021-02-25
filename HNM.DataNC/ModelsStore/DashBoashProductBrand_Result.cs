using System;
using System.ComponentModel.DataAnnotations;

namespace HNM.DataNC.ModelsStore
{
    public class DashBoashProductBrand_Result
    {
        [Key]
        public Nullable<int> ProductBrandID { get; set; }
        public Nullable<int> TotalViewCount { get; set; }
        public Nullable<int> TotalProduct { get; set; }
        public Nullable<int> TotalRecruitment { get; set; }
        public Nullable<int> TotalArticle { get; set; }
        public Nullable<int> ProductPost { get; set; }
        public Nullable<int> ProductPending { get; set; }
        public Nullable<int> ProductSaveDraft { get; set; }
        public Nullable<int> ProductCancel { get; set; }
        public Nullable<int> RecruitmentPost { get; set; }
        public Nullable<int> RecruitmentPending { get; set; }
        public Nullable<int> RecruitmentSaveDraft { get; set; }
        public Nullable<int> RecruitmentCancel { get; set; }
        public Nullable<int> ArticlePost { get; set; }
        public Nullable<int> ArticlePending { get; set; }
        public Nullable<int> ArticleSaveDraft { get; set; }
        public Nullable<int> ArticleCancel { get; set; }
        public Nullable<int> ProductBrandRank { get; set; }
        public Nullable<int> ProductBrandPoint { get; set; }
        public Nullable<int> ProductBrandLike { get; set; }
        public Nullable<int> ProductBrandFollow { get; set; }

    }
}
