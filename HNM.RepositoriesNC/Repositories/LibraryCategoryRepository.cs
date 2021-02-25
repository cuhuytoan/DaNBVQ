using HNM.DataNC.Models;
using HNM.RepositoriesNC.RepositoriesBase;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HNM.RepositoriesNC.Repositories
{
    public interface ILibraryCategoryRepository : IRepositoryBase<LibraryCategory>
    {
        Task<IEnumerable<LibraryCategory>> GetLibCategory();
        Task<LibraryCategory> GetLibCategoryById(int? LibraryCategoryId);
        Task<LibraryCategory> GetLibCategoryByUrl(string url);
    }
    public class LibraryCategoryRepository : RepositoryBase<LibraryCategory>, ILibraryCategoryRepository
    {
        public LibraryCategoryRepository(HanomaContext HanomaContext) : base(HanomaContext)
        {
        }
        public async Task<IEnumerable<LibraryCategory>> GetLibCategory()
        {
            return await HanomaContext.LibraryCategory.Where(x => x.Parent_ID != null).OrderBy(p => p.Sort)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<LibraryCategory> GetLibCategoryById(int ?LibraryCategoryId)
        {
            return await HanomaContext.LibraryCategory.SingleOrDefaultAsync(x => x.LibraryCategory_ID == LibraryCategoryId);
        }

        public async Task<LibraryCategory> GetLibCategoryByUrl(string url)
        {
            return await HanomaContext.LibraryCategory.SingleOrDefaultAsync(x => x.URL == url);
        }
    }
}
