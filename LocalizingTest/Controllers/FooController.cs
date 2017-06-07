using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;

namespace LocalizingTest.Controllers
{
    [Route("[controller]")]
    [Route("{culture}/[controller]")]
    public class FooController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
