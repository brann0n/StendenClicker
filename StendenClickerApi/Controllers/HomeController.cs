﻿using StendenClickerApi.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StendenClickerApi.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View(SessionExtensions.Get());
        }
    }
}