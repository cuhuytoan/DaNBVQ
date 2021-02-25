using HNM.DataNC.Models;
using HNM.RepositoriesNC.RepositoriesBase;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HNM.RepositoriesNC.Repositories
{
    public interface IPageLayoutRepository : IRepositoryBase<PageLayout>
    {
        Task<IEnumerable<PageLayout>> GetPageLayout();
    }
    public class PageLayoutRepository : RepositoryBase<PageLayout>, IPageLayoutRepository
    {
        public PageLayoutRepository(HanomaContext HanomaContext) : base(HanomaContext)
        {
        }

        public async Task<IEnumerable<PageLayout>> GetPageLayout()
        {
            return await HanomaContext.PageLayout
                .FromSqlRaw($"select * from dbo.PageLayout with (nolock) where Active = 1 order by Position asc")
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
