using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YunStorm.Weather.Teams.Web.Controllers
{
    public class TabsController : Controller
    {
        [Route("config")]
        public IActionResult Config()
        {
            return View();
        }

        [Route("home")]
        public IActionResult Home()
        {
            return View();
        }
    }
}
