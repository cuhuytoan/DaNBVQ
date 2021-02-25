using HNM.DataNC.Models;
using HNM.RepositoriesNC.RepositoriesBase;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace HNM.RepositoriesNC.Repositories
{
    public interface ILocationRepository : IRepositoryBase<Location>
    {
        Task<IEnumerable<Location>> GetAllLocation(string keyword);
        Task<IEnumerable<District>> GetDistrictByLocation(string keyword, int? LocationId);
        Task<IEnumerable<Location>> GetLocationByCountry(int CountryId, string keyword);
        Task<District> GetDistrictByName(string keyword);
    }
    public class LocationRepository : RepositoryBase<Location>, ILocationRepository
    {
        public LocationRepository(HanomaContext HanomaContext) : base(HanomaContext)
        {
        }

        public async Task<IEnumerable<Location>> GetAllLocation(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                var output = await HanomaContext.Location.Where(p => p.Name.Contains(keyword)).OrderBy(x => x.Sort).ToListAsync();
                return output;
            }
            else
            {
                var output = await HanomaContext.Location.OrderBy(x => x.Sort).ToListAsync();
                return output;
            }

        }

        public async Task<IEnumerable<District>> GetDistrictByLocation(string keyword, int? LocationId)
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                var output = await HanomaContext.District.Where(p => p.Name.Contains(keyword) && p.LocationId == LocationId).OrderBy(x => x.Name).ToListAsync();
                return output;
            }
            else
            {
                var output = await HanomaContext.District.Where(p => p.LocationId == LocationId).OrderBy(x => x.Name).ToListAsync();
                return output;
            }
        }

        public async Task<District> GetDistrictByName(string keyword)
        {
            var output = await HanomaContext.District.FirstOrDefaultAsync(p => p.Name == keyword);
            return output;
        }

        public async Task<IEnumerable<Location>> GetLocationByCountry(int CountryId, string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                var output = await HanomaContext.Location.Where(p => p.Name.Contains(keyword)).OrderBy(x => x.Sort).ToListAsync();
                return output;
            }
            else
            {
                var output = await HanomaContext.Location.OrderBy(x => x.Sort).ToListAsync();
                return output;
            }
        }
    }
}
