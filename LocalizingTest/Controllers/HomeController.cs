﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;

namespace LocalizingTest.Controllers
{
    public class HomeController : Controller
    {
        [Route("[controller]")]
        [Route("{culture}/[controller]")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
