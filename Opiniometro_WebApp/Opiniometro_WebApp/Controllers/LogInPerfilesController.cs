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
        /*
         * Agregar o remover roles: https://stackoverflow.com/questions/22570743/how-do-i-remove-an-existing-claim-from-a-claimsprinciple
         */

        // GET: LogInPerfiles
        public ActionResult Index()
        {
            return View();
        }
    }
}