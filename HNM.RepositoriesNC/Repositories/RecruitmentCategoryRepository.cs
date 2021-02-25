using HNM.DataNC.Models;
using HNM.RepositoriesNC.RepositoriesBase;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HNM.RepositoriesNC.Repositories
{
    public interface IRecruitmentCategoryRepository : IRepositoryBase<RecruitmentCategory>
    {
        Task<IEnumerable<RecruitmentCategory>> GetAllRecruitmentCate();
        Task<RecruitmentCategory> GetRecruitmentById(int Id);
    }

    public class RecruitmentCategoryRepository : RepositoryBase<RecruitmentCategory>, IRecruitmentCategoryRepository
    {
        public RecruitmentCategoryRepository(HanomaContext HanomaContext) : base(HanomaContext)
        {
        }

        public async Task<IEnumerable<RecruitmentCategory>> GetAllRecruitmentCate()
        {
            return await HanomaContext.RecruitmentCategory.OrderBy(x => x.Sort)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<RecruitmentCategory> GetRecruitmentById(int Id)
        {
            return await HanomaContext.RecruitmentCategory.SingleOrDefaultAsync(x=>x.RecruitmentCategory_ID == Id);
        }
    }
}
