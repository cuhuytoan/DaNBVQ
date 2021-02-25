using HNM.DataNC.Models;
using HNM.DataNC.ModelsStore;
using HNM.RepositoriesNC.RepositoriesBase;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HNM.RepositoriesNC.Repositories
{
    public interface IResumeRepository : IRepositoryBase<Resume>
    {
        Task<IEnumerable<Resume_Search_Result>> GetTopResume(int topX);
        Task<IEnumerable<Resume_Search_Result>> GetListResumePagging(int page, int pageSize, int statusTypeId);
    }
    public class ResumeRepository : RepositoryBase<Resume>, IResumeRepository
    {
        public ResumeRepository(HanomaContext HanomaContext) : base(HanomaContext)
        {
        }

        public async Task<IEnumerable<Resume_Search_Result>> GetTopResume(int topX)
        {
            var OrderBy = new SqlParameter("@OrderBy", "LastEditDate");
            var PageSize = new SqlParameter("@PageSize", topX);
            var CurrentPage = new SqlParameter("@CurrentPage", 1);

            var output = await HanomaContext.Set<Resume_Search_Result>()
                .FromSqlRaw($"EXECUTE dbo.Resume_Search @OrderBy = @OrderBy, @PageSize = @PageSize, @CurrentPage = @CurrentPage", OrderBy, PageSize, CurrentPage)
                .AsNoTracking()
                .ToListAsync();
            return output;
        }
        public async Task<IEnumerable<Resume_Search_Result>> GetListResumePagging(int page, int pageSize, int statusTypeId)
        {
            var StatusType_ID = new SqlParameter("@StatusType_ID", statusTypeId);
            var PageSize = new SqlParameter("@PageSize", pageSize);
            var CurrentPage = new SqlParameter("@CurrentPage", page);

            var output = await HanomaContext.Set<Resume_Search_Result>()
                .FromSqlRaw($"EXECUTE dbo.Resume_Search  @StatusType_ID = @StatusType_ID, @PageSize = @PageSize, @CurrentPage = @CurrentPage", StatusType_ID, PageSize, CurrentPage)
                .AsNoTracking()
                .ToListAsync();
            return output;
        }
    }
}
