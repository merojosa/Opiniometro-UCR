using Opiniometro_WebApp.Controllers.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Opiniometro_WebApp.Controllers
{
    // Esto hace que se necesite un usuario autenticado para poder hacer uso de los action methods.
    [Authorize]
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

        // Necesito de este action method para inicializar atributos (cuando no pasa por el login).
        public ActionResult Inicio()
        {
            PermisosUsuario permisos_usuario = new PermisosUsuario();
            Session[PermisosUsuario.obtener_correo_actual()] = permisos_usuario;

            return RedirectToAction("Index");
        }
    }
}