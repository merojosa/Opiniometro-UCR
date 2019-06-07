using Opiniometro_WebApp.Controllers.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Opiniometro_WebApp.Models;
using System.Security.Claims;
using System.Threading;

namespace Opiniometro_WebApp.Controllers
{
    

    // Esto hace que se necesite un usuario autenticado para poder hacer uso de los action methods.
    [Authorize]
    public class HomeController : Controller
    {
        
        public ActionResult Index(String perfil)
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

        //devuelve los perfiles del que está usando el sistema usuario
        public static ICollection<String> ObtenerPerfiles()
        {
            Opiniometro_DatosEntities db = new Opiniometro_DatosEntities();

            var identidad_autenticada = (ClaimsPrincipal)Thread.CurrentPrincipal;

            string correo_autenticado = identidad_autenticada.Claims.Where(c => c.Type == ClaimTypes.Email)
                                                .Select(c => c.Value).SingleOrDefault();
            ICollection<String> perfiles;
            perfiles = db.ObtenerPerfilUsuario(correo_autenticado).ToList();
            return perfiles;
        }

        // Necesito de este action method para inicializar atributos (cuando no pasa por el login).
        public ActionResult Inicio()
        {
            IdentidadManager permisos_usuario = new IdentidadManager();
            Session[IdentidadManager.obtener_correo_actual()] = permisos_usuario;

            return RedirectToAction("Index");
        }
    }
}