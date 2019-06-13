using Opiniometro_WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Opiniometro_WebApp.Controllers.Servicios;
using System.Security.Claims;
using System.Threading;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace Opiniometro_WebApp.Controllers
{
    public class PerfilController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            Opiniometro_DatosEntities db = new Opiniometro_DatosEntities();
            string correo_autenticado = IdentidadManager.obtener_correo_actual();
            //ICollection<String> perfiles;
            //perfiles = db.ObtenerPerfilUsuario(correo_autenticado).ToList();
            PerfilesUsuario model = new PerfilesUsuario();
            model.ListaPerfiles = db.ObtenerPerfilUsuario(correo_autenticado).ToList();
            return View(model);
        }



        [HttpPost]
        public ActionResult Index(PerfilesUsuario model)
        {
            String perfil_elegido = model.perfilSeleccionado;
            // Si es un distinto perfil, cambie los permisos.
            if (IdentidadManager.obtener_perfil_actual() != perfil_elegido)
            {
                string correo_actual = IdentidadManager.obtener_correo_actual();

                var identidad = new ClaimsIdentity(
                        new[] {
                    new Claim(ClaimTypes.Email, correo_actual),

                    // Nuevo perfil.
                    new Claim(ClaimTypes.Role, perfil_elegido)
                        },
                        DefaultAuthenticationTypes.ApplicationCookie);

                AuthController.eliminar_privilegios(this);

                Thread.CurrentPrincipal = new ClaimsPrincipal(identidad);
                HttpContext.GetOwinContext().Authentication.SignIn(new AuthenticationProperties { IsPersistent = false }, identidad);
                Session[correo_actual] = new IdentidadManager();
            }

            return RedirectToAction("Index", "Home");
        }

        // GET: Perfil
        public ActionResult CambioPerfil(string perfil_elegido)
        {
            // Si es un distinto perfil, cambie los permisos.
            if (IdentidadManager.obtener_perfil_actual() != perfil_elegido)
            {
                string correo_actual = IdentidadManager.obtener_correo_actual();

                var identidad = new ClaimsIdentity(
                        new[] {
                    new Claim(ClaimTypes.Email, correo_actual),

                    // Nuevo perfil.
                    new Claim(ClaimTypes.Role, perfil_elegido)
                        },
                        DefaultAuthenticationTypes.ApplicationCookie);

                AuthController.eliminar_privilegios(this);

                Thread.CurrentPrincipal = new ClaimsPrincipal(identidad);
                HttpContext.GetOwinContext().Authentication.SignIn(new AuthenticationProperties { IsPersistent = false }, identidad);
                Session[correo_actual] = new IdentidadManager();
            }

            return RedirectToAction("Index", "Home");
        }

        // Devuelve los perfiles del que está usando el sistema usuario
        public static ICollection<String> ObtenerPerfiles()
        {
            Opiniometro_DatosEntities db = new Opiniometro_DatosEntities();
            string correo_autenticado = IdentidadManager.obtener_correo_actual();
            ICollection<String> perfiles;
            perfiles = db.ObtenerPerfilUsuario(correo_autenticado).ToList();
            return perfiles;
        }
    }
}