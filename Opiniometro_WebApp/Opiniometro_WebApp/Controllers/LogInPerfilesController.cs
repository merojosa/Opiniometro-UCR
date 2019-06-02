using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Opiniometro_WebApp.Controllers
{
    [Authorize]
    public class LogInPerfilesController : Controller
    {
        // GET: LogInPerfiles
        public ActionResult Index()
        {
            return View();
        }
    }
}