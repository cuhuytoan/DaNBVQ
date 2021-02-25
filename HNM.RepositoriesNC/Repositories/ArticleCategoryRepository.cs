using HNM.DataNC.Models;
using HNM.RepositoriesNC.RepositoriesBase;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HNM.RepositoriesNC.Repositories
{
    public interface IArticleCategoryRepository : IRepositoryBase<ArticleCategory>
    {
        Task<List<ArticleCategory>> GetAllArticle();
        Task<List<VideoCategory>> GetVideoCategory();
        Task<ArticleCategory> GetArtCategoryByUrl(string url);
    }
    public class ArticleCategoryRepository : RepositoryBase<ArticleCategory>, IArticleCategoryRepository
    {
        public ArticleCategoryRepository(HanomaContext HanomaContext) : base(HanomaContext)
        {
        }

        public async Task<List<ArticleCategory>> GetAllArticle()
        {
            var output = new List<ArticleCategory>();
            try
            {
                
                output = await HanomaContext.ArticleCategory.Where(p => p.ArticleCategory_ID >= 87 && p.DisplayMenu == true)                    
                    .ToListAsync();
                output.Add(new ArticleCategory { ArticleCategory_ID = 9999, Name = "Trang chủ", Sort = 0, MenuIconUrl = "https://cdn.hanoma.vn/DataMobile/articleCategory/Icons/trang-chu.svg" });
                output = output.OrderBy(p => p.Sort).ToList();


            }
            catch(Exception ex)
            {
                return output;
            }
            return output;
        }

        public async Task<List<VideoCategory>> GetVideoCategory()
        {
            var output = new List<VideoCategory>();
            try
            {
                output = await HanomaContext.VideoCategory.OrderBy(p => p.Sort).AsNoTracking().ToListAsync();
            }
            catch(Exception ex)
            {
                return output;
            }
            return output;
        }

        public async Task<ArticleCategory> GetArtCategoryByUrl(string url)
        {
            return await HanomaContext.ArticleCategory.SingleOrDefaultAsync(x => x.URL == url);
        }
    }
}
