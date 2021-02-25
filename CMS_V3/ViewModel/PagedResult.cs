using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS_V3.ViewModel
{
    public class PagedResult<T> : PagedResultBase
    {
        public List<T> Items { set; get; }
    }
}
