using HNM.DataNC.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HNM.WebApiNC.Logging
{
    public class ApiLogService
    {
        private readonly HanomaContext _db;

        public ApiLogService(HanomaContext db)
        {
            _db = db;
        }

        public async Task Log(LogAPI apiLogItem)
        {
            _db.LogAPI.Add(apiLogItem);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<LogAPI>> Get()
        {
            var items = from i in _db.LogAPI
                        orderby i.Id descending
                        select i;

            return await items.ToListAsync();
        }
    }
}
