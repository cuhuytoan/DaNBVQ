using HNM.DataNC.Models;
using HNM.RepositoriesNC.RepositoriesBase;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HNM.RepositoriesNC.Repositories
{
    public interface IIntroRepository : IRepositoryBase<AppIntroScreen>
    {
        Task<IEnumerable<AppIntroScreen>> GetIntroScreens();
    }
    public class IntroRepository : RepositoryBase<AppIntroScreen>, IIntroRepository
    {
        public IntroRepository(HanomaContext HanomaContext) : base(HanomaContext)
        {
        }
        public async Task<IEnumerable<AppIntroScreen>> GetIntroScreens()
        {
            var output = await HanomaContext.AppIntroScreen
            .FromSqlRaw($"SELECT TOP (100) * FROM dbo.AppIntroScreen WITH (NOLOCK) ORDER BY Sort ASC")
            .AsNoTracking()
            .ToListAsync();
            return output;

        }
    }
}
