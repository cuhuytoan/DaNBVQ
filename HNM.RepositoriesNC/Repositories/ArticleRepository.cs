using HNM.DataNC.Models;
using HNM.DataNC.ModelsNC.ModelsDTO;
using HNM.DataNC.ModelsStore;
using HNM.RepositoriesNC.RepositoriesBase;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace HNM.RepositoriesNC.Repositories
{
    public interface IArticleRepository : IRepositoryBase<Article>
    {
        Task<IEnumerable<Article>> GetRelateArticle(int? ArticleCategoryId, int ArticleId, int ArticleTypeId);
        Task<IEnumerable<Article_Search_Result>> GetArtByCate(int? ArticleCategory_ID, int ArticleTypeId, int? page, int PageSize);
        Article GetArticleDetail(int ArticleId, int ArticleTypeId);
        ArticleVoice GetArticleVoice(int ArticleId, int VoiceId);
        Task<ArticleSearchDTO> SearchArticle(string keyword, int? ArticleCategory_ID, int? ArticleTypeId, int? page, int? PageSize);
        Task<IEnumerable<Article>> GetHomeVerticleBlockOneArticlePartialView(int top);
        Task<IEnumerable<Article>> GetHomeVerticleBlockTwoArticlePartialView(int top);
        Task<Article> GetHomeHighLightsArticlePartialView();
        Task<IEnumerable<Article>> GetHomeArticleCategoryPartialView(int articleCategoryId, int top);
        Task<List<GetArticleHome_Result>> GetArtByCateHome(string keyword);
        Task<IEnumerable<Article>> GetRelateVideo(int? VideoCategoryId, int ArticleId, int ArticleTypeId, int page, int pageSize);
        Task<IEnumerable<Video_Search_Mobile_Result>> GetVideoByCate(int? VideoCategory_ID, int ArticleTypeId, int? page, int PageSize);
    }
    public class ArticleRepository : RepositoryBase<Article>, IArticleRepository
    {
        public ArticleRepository(HanomaContext HanomaContext) : base(HanomaContext)
        {
        }

        public async Task<IEnumerable<Article>> GetRelateArticle(int? ArticleCategoryId, int ArticleId, int ArticleTypeId)
        {
            if (ArticleCategoryId != null)
            {

                //return await HanomaContext.Article.Where(p => p.ArticleCategory_ID == ArticleCategoryId && p.Article_ID != ArticleId && p.ArticleType_ID == ArticleTypeId && p.StatusType_ID == 4 && p.Article_ID <28600).OrderByDescending(x => x.LastEditDate).Take(5).ToListAsync();
                return await HanomaContext.Article.Where(p => p.ArticleCategory_ID == ArticleCategoryId && p.Article_ID != ArticleId && p.ArticleType_ID == ArticleTypeId && p.StatusType_ID == 4
                && !HanomaContext.ArticleCovid.Any(d => d.Article_ID == p.Article_ID)
                ).OrderByDescending(x => x.LastEditDate).Take(8).ToListAsync();

            }
            else
            {
                //return await HanomaContext.Article.Where(p =>   p.Article_ID != ArticleId && p.ArticleType_ID == ArticleTypeId && p.StatusType_ID == 4 && p.Article_ID < 28600).OrderByDescending(x => x.LastEditDate).Take(5).ToListAsync();
                return await HanomaContext.Article.Where(p => p.Article_ID != ArticleId && p.ArticleType_ID == ArticleTypeId && p.StatusType_ID == 4
                && !HanomaContext.ArticleCovid.Any(d => d.Article_ID == p.Article_ID)
                ).OrderByDescending(x => x.LastEditDate).Take(8).ToListAsync();
            }

        }

        public async Task<IEnumerable<Article>> GetRelateVideo(int? VideoCategoryId, int ArticleId, int ArticleTypeId, int page, int pageSize)
        {
            if (VideoCategoryId != null)
            {
                var data = await HanomaContext.Article.Where(p => p.VideoCategory_ID == VideoCategoryId && p.Article_ID != ArticleId && p.ArticleType_ID == ArticleTypeId && p.StatusType_ID == 4).OrderByDescending(x => x.LastEditDate).Take(pageSize).ToListAsync();
                if(data.Count <= 10)
                {
                    return await HanomaContext.Article.Where(p => p.VideoCategory_ID == VideoCategoryId && p.Article_ID != ArticleId && p.ArticleType_ID == ArticleTypeId && p.StatusType_ID == 4).OrderByDescending(x => x.LastEditDate).Take(pageSize).ToListAsync();
                }
                else
                {
                    return await HanomaContext.Article.Where(p => p.VideoCategory_ID == VideoCategoryId && p.Article_ID != ArticleId && p.ArticleType_ID == ArticleTypeId && p.StatusType_ID == 4).OrderByDescending(x => x.LastEditDate).Skip(page * pageSize).Take(pageSize).ToListAsync();
                }
            }
            else
            {
                //return await HanomaContext.Article.Where(p =>   p.Article_ID != ArticleId && p.ArticleType_ID == ArticleTypeId && p.StatusType_ID == 4 && p.Article_ID < 28600).OrderByDescending(x => x.LastEditDate).Take(5).ToListAsync();
                return await HanomaContext.Article.Where(p => p.Article_ID != ArticleId && p.ArticleType_ID == ArticleTypeId && p.StatusType_ID == 4
                && !HanomaContext.ArticleCovid.Any(d => d.Article_ID == p.Article_ID)
                ).OrderByDescending(x => x.LastEditDate).Skip(page * pageSize).Take(pageSize).ToListAsync(); 
            }

        }

        public async Task<IEnumerable<Article_Search_Result>> GetArtByCate(int? ArticleCategory_ID, int ArticleTypeId, int? page, int PageSize)
        {
            var pArticleCategory_ID = new SqlParameter("@ArticleCategory_ID", ArticleCategory_ID ?? (object)DBNull.Value);
            var pArticleTypeId = new SqlParameter("@ArticleType_ID", ArticleTypeId);
            var pPage = new SqlParameter("@CurrentPage", page ?? 1);
            var pPageSize = new SqlParameter("@PageSize", PageSize);
            var pExceptionArticleID = new SqlParameter("@ExceptionArticle_ID", ArticleCategory_ID ?? (object)DBNull.Value);
            var pStatusTypeID = new SqlParameter("@StatusType_ID", 4);
            var pItemCount = new SqlParameter("@ItemCount", SqlDbType.Int) { Direction = ParameterDirection.Output };
            var result = await HanomaContext.Set<Article_Search_Result>()
                .FromSqlRaw(
                    $"EXECUTE dbo.Article_Search_Mobile @ArticleCategory_ID=@ArticleCategory_ID , @ExceptionArticle_ID=@ExceptionArticle_ID,@StatusType_ID = @StatusType_ID, @ArticleType_ID = @ArticleType_ID,@CurrentPage = @CurrentPage,@PageSize = @PageSize,@ItemCount = @ItemCount out ",
                    pArticleCategory_ID, pExceptionArticleID,pStatusTypeID, pArticleTypeId, pPage, pPageSize, pItemCount)
                .AsNoTracking()
                .ToListAsync();


            return result;

        }

        public async Task<IEnumerable<Video_Search_Mobile_Result>> GetVideoByCate(int? VideoCategory_ID, int ArticleTypeId, int? page, int PageSize)
        {
            var pVideoCategory_ID = new SqlParameter("@VideoCategory_ID", VideoCategory_ID ?? (object)DBNull.Value);
            var pArticleTypeId = new SqlParameter("@ArticleType_ID", ArticleTypeId);
            var pPage = new SqlParameter("@CurrentPage", page ?? 1);
            var pPageSize = new SqlParameter("@PageSize", PageSize);
            var pStatusTypeID = new SqlParameter("@StatusType_ID", 4);

            var result = await HanomaContext.Set<Video_Search_Mobile_Result>()
                .FromSqlRaw(
                    $"EXECUTE dbo.Video_Search_Mobile @VideoCategory_ID=@VideoCategory_ID, @StatusType_ID = @StatusType_ID, @ArticleType_ID = @ArticleType_ID,@CurrentPage = @CurrentPage,@PageSize = @PageSize ",
                    pVideoCategory_ID, pStatusTypeID, pArticleTypeId, pPage, pPageSize)
                .AsNoTracking()
                .ToListAsync();

            return result;
        }


        public Article GetArticleDetail(int ArticleId, int ArticleTypeId)
        {
            return HanomaContext.Article.FirstOrDefault(p =>
                p.Article_ID == ArticleId && p.ArticleType_ID == ArticleTypeId);
        }

        public ArticleVoice GetArticleVoice(int ArticleId, int VoiceId)
        {
            return HanomaContext.ArticleVoice.OrderByDescending(p => p.LastUpdateDate).FirstOrDefault(x => x.Article_ID == ArticleId);
        }

        public async Task<ArticleSearchDTO> SearchArticle(string keyword, int? ArticleCategory_ID, int? ArticleTypeId, int? page, int? PageSize)
        {
            var output = new ArticleSearchDTO();
            var pKeyword = new SqlParameter("@Keyword", keyword);
            var pArticleCategory_ID = new SqlParameter("@ArticleCategory_ID", ArticleCategory_ID ?? (object)DBNull.Value);
            var pArticleTypeId = new SqlParameter("@ArticleType_ID", ArticleTypeId ?? (object)DBNull.Value);
            var pPage = new SqlParameter("@CurrentPage", page ?? 1);
            var pPageSize = new SqlParameter("@PageSize", PageSize ?? 10);
            var pActiveCount = new SqlParameter("@ActiveCount", SqlDbType.Int) { Direction = ParameterDirection.Output };
            var pDeActiveCount = new SqlParameter("@DeActiveCount", SqlDbType.Int) { Direction = ParameterDirection.Output };
            var pItemCount = new SqlParameter("@ItemCount", SqlDbType.Int) { Direction = ParameterDirection.Output };
            //var pExceptionArticleID = new SqlParameter("@ExceptionArticle_ID", ArticleCategory_ID ?? (object)DBNull.Value);
            output.Data =  await HanomaContext.Set<Article_Search_Result>()
                .FromSqlRaw(
                    $"EXECUTE dbo.Article_Search_Mobile @Keyword = @Keyword, @ArticleCategory_ID = @ArticleCategory_ID,  @ArticleType_ID = @ArticleType_ID,@CurrentPage = @CurrentPage,@PageSize = @PageSize,@ActiveCount = @ActiveCount out, @DeActiveCount = @DeActiveCount out, @ItemCount = @ItemCount out",
                    pKeyword, pArticleCategory_ID, pArticleTypeId, pPage, pPageSize, pActiveCount, pDeActiveCount, pItemCount)
                .AsNoTracking()
                .ToListAsync();
            output.CurrentPage = page ?? 1;
            output.PageSize = PageSize ?? 10;
            output.TotalRecord = (int)pItemCount.Value;
            output.TotalPage = (int)pActiveCount.Value;
            return output;
        }

        public async Task<IEnumerable<Article>> GetHomeVerticleBlockOneArticlePartialView(int top)
        {
            return await (from articleblockarticle in HanomaContext.ArticleBlockArticle
                          join article in HanomaContext.Article on articleblockarticle.Article_id equals article.Article_ID
                          //join d in HanomaContext.ArticleCovid on article.Article_ID != d.Article_ID
                          where articleblockarticle.ArticleBlock_id == 2
                          //&& !HanomaContext.ArticleCovid.Any(d => d.Article_ID == article.Article_ID)
                          orderby article.LastEditDate descending
                          select article).Take(top).ToListAsync();

        }

        public async Task<IEnumerable<Article>> GetHomeVerticleBlockTwoArticlePartialView(int top)
        {
            return await (from articleblockarticle in HanomaContext.ArticleBlockArticle
                          join article in HanomaContext.Article on articleblockarticle.Article_id equals article.Article_ID
                          where articleblockarticle.ArticleBlock_id == 3
                          //&& !HanomaContext.ArticleCovid.Any(d => d.Article_ID == article.Article_ID)
                          orderby article.LastEditDate descending
                          select article).Take(top).ToListAsync();

        }

        public async Task<Article> GetHomeHighLightsArticlePartialView()
        {
            return await (from articleblockarticle in HanomaContext.ArticleBlockArticle
                          join article in HanomaContext.Article on articleblockarticle.Article_id equals article.Article_ID
                          orderby article.LastEditDate descending
                          where articleblockarticle.ArticleBlock_id == 1
                          //&& !HanomaContext.ArticleCovid.Any(d => d.Article_ID == article.Article_ID)
                          select article).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Article>> GetHomeArticleCategoryPartialView(int articleCategoryId, int top)
        {
            return await (from ac in HanomaContext.ArticleCategory
                          join aca in HanomaContext.ArticleCategoryArticle on ac.ArticleCategory_ID equals aca.ArticleCategory_ID
                          join a in HanomaContext.Article on aca.Article_ID equals a.Article_ID
                          where ac.ArticleCategory_ID == articleCategoryId
                          //&& !HanomaContext.ArticleCovid.Any(d => d.Article_ID == a.Article_ID)
                          orderby a.LastEditDate descending
                          select a).Take(top).ToListAsync();
        }

        public async Task<List<GetArticleHome_Result>> GetArtByCateHome(string keyword)
        {
            var pKeyword = new SqlParameter("@Keyword", keyword);
            return await HanomaContext.Set<GetArticleHome_Result>()
                .FromSqlRaw(
                    $"EXECUTE dbo.GetArticleHome @Keyword = @Keyword",
                    pKeyword)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
