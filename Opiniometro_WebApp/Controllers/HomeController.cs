using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Opiniometro_WebApp.Controllers
{
//    [Authorize] se comenta para que no haya conflicto, con Authorize se necesita tener un usuario autenticado para acceder a los action methods del controlador Home.
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}