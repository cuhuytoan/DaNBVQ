using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS_V3.Helper
{
    public class CommonHelper
    {
        /// <summary>
        /// Setting exipre
        /// </summary>
        /// <returns></returns>
        public static DistributedCacheEntryOptions RedisOptions()
        {
            return new DistributedCacheEntryOptions()
             .SetSlidingExpiration(TimeSpan.FromMinutes(5))
             .SetAbsoluteExpiration(TimeSpan.FromHours(3));
        }
    }
}
