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
    [Authorize]
    public class PerfilController : Controller
    {
        public ActionResult Index()
        {
            PerfilesUsuario model = new PerfilesUsuario();
            model.ListaPerfiles = ObtenerPerfiles();

            // Si el usuario recien se loggea
            if(IdentidadManager.obtener_perfil_actual() == null)
            {
                // Se escoge un perfil por defecto en caso de que le de cancelar o pase de pagina (no elige perfil).
                cambiar_perfil(model.ListaPerfiles.ElementAt(0));
            }
            // Si no es la primera vez, no se cambia el perfil porque ya hay uno elegido.

            return View(model);
        }



        [HttpPost]
        public ActionResult Index(PerfilesUsuario model)
        {
            cambiar_perfil(model.perfilSeleccionado);
            return RedirectToAction("Index", "Home");
        }

        // Por cuestiones de seguirdad, TIENE que ser privado.
        private void cambiar_perfil(string perfil_elegido)
        {
            // Si es un distinto perfil, cambie los permisos.
            if (IdentidadManager.obtener_perfil_actual() != perfil_elegido)
            {
                string correo_actual = IdentidadManager.obtener_correo_actual();

                var identidad = new ClaimsIdentity
                    (
                        new[] 
                        {
                            new Claim(ClaimTypes.Email, correo_actual),
                            new Claim(ClaimTypes.Role, perfil_elegido)
                        },
                        DefaultAuthenticationTypes.ApplicationCookie
                    );

                AuthController.eliminar_privilegios(this);

                Thread.CurrentPrincipal = new ClaimsPrincipal(identidad);
                HttpContext.GetOwinContext().Authentication.SignIn(new AuthenticationProperties { IsPersistent = false }, identidad);
                Session[correo_actual] = new IdentidadManager();
            }
        }

        // Devuelve los perfiles del usuario loggeado.
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