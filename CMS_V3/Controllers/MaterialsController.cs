using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CMS_V3.Controllers
{
    public class MaterialsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
