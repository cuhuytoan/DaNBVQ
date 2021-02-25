using HNM.DataNC.Models;
using HNM.DataNC.ModelsStore;
using HNM.RepositoriesNC.RepositoriesBase;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HNM.RepositoriesNC.Repositories
{
    public interface ICareerCategoryRepository : IRepositoryBase<CareerCategory>
    {
        Task<IEnumerable<CareerCategory>> GetAllCareerCategory();
        Task<List<GetCVCategory_Result>> GetCVCategory();

    }
    public class CareerCategoryRepository : RepositoryBase<CareerCategory>, ICareerCategoryRepository
    {
        public CareerCategoryRepository(HanomaContext HanomaContext) : base(HanomaContext)
        {
        }

        public async Task<IEnumerable<CareerCategory>> GetAllCareerCategory()
        {
            return await HanomaContext.CareerCategory.AsNoTracking().ToListAsync();
        }

        public async Task<List<GetCVCategory_Result>> GetCVCategory()
        {
            var pKeyword = new SqlParameter("@Keyword", "");
            return await HanomaContext.Set<GetCVCategory_Result>()
                .FromSqlRaw($"EXECUTE GetCVCategory @Keyword = @Keyword", pKeyword)
                .AsNoTracking()
                .ToListAsync();
        }
    }

}
