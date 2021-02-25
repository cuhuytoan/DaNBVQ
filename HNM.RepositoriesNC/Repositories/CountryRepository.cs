using HNM.DataNC.Models;
using HNM.RepositoriesNC.RepositoriesBase;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HNM.RepositoriesNC.Repositories
{
    public interface ICountryRepository : IRepositoryBase<Country>
    {
        Task<List<Country>> GetListCountry(string keyword);
    }
    public class CountryRepository : RepositoryBase<Country>, ICountryRepository
    {
        public CountryRepository(HanomaContext HanomaContext) : base(HanomaContext)
        {
        }
        public async Task<List<Country>> GetListCountry(string keyword)
        {
            return await HanomaContext.Country.Where(p => p.Name.Contains(keyword ?? "")).OrderBy(x => x.Sort).ToListAsync();
        }
    }
}
