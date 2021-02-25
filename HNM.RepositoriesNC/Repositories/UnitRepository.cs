using HNM.DataNC.Models;
using HNM.RepositoriesNC.RepositoriesBase;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HNM.RepositoriesNC.Repositories
{
    public interface IUnitRepository : IRepositoryBase<Unit>
    {
        Task<List<Unit>> GetListUnits(string keyword);
        Task<List<Minor>> GetListMinor(int MajorId);
        Task<Minor> GetMinorByMinorId(int MinorId);
    }
    public class UnitRepository : RepositoryBase<Unit>, IUnitRepository
    {
        public UnitRepository(HanomaContext HanomaContext) : base(HanomaContext)
        {
        }

        public async Task<List<Minor>> GetListMinor(int MajorId)
        {
            return await HanomaContext.Minor.Where(p => p.MajorID == MajorId).ToListAsync();
        }

        public async Task<List<Unit>> GetListUnits(string keyword)
        {
            return await HanomaContext.Unit.Where(p => p.Name.Contains(keyword ?? "")).ToListAsync();
        }

        public async Task<Minor> GetMinorByMinorId(int MinorId)
        {
            return await HanomaContext.Minor.FirstOrDefaultAsync(p => p.MinorID == MinorId);
        }

    }
}
