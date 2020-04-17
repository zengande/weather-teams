using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YunStorm.Weather.Teams.Web.Controllers
{
    [Authorize]
    public class TabsController : Controller
    {
        [Route("home")]
        public IActionResult Home()
        {
            return View();
        }
    }
}
