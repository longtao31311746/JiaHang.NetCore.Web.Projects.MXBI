﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace JiaHang.NetCore.Web.Projects.MXBI.Web.Controllers.DimStore
{
    public class DimStoreController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}