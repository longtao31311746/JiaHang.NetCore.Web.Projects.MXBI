using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JiaHang.NetCore.Web.Projects.MXBI.Web.Controllers
{
    public class SysRouteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
