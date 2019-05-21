using System;
using System.Data.Entity.Core.Objects;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Opiniometro_WebApp.Models;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace Opiniometro_WebApp.Controllers
{
    public class AuthController : Controller
    {
        private Opiniometro_DatosEntities db = new Opiniometro_DatosEntities();

        /* GET: Auth/Login
         * EFECTO: retornar vista parcial, la cual implica que no se despliega _Layout de la carpeta Shared.
         * REQUIERE: cshtml con el nombre Login.
         * MODIFICA: n/a
         */
        public ActionResult Login()
        {
            return PartialView();
        }

        /*  
         *  
         *  EFECTO: verificar los datos brindados en la base de datos.
         *  REQUIERE: correo y contrasenna en "empaquetado" en la clase Usuario.
         *  MODIFICA: variable Resultado para saber si se acepta la autenticacion (con un true).
         *  
         *  Basado en: https://stackoverflow.com/questions/31584506/how-to-implement-custom-authentication-in-asp-net-mvc-5
         */
        [HttpPost]
        public ActionResult Login(Usuario usuario)
        {
            ObjectParameter exito = new ObjectParameter("Resultado", 0);
            db.SP_LoginUsuario(usuario.CorreoInstitucional, usuario.Contrasena, exito);

            // Si se pudo autenticar.
            if ((bool)exito.Value == true)
            {
                var ident = new ClaimsIdentity(
                    new[] {
                    new Claim(ClaimTypes.NameIdentifier, usuario.CorreoInstitucional),
                    new Claim(ClaimTypes.Name, usuario.CorreoInstitucional),

                    /*
                    // optionally you could add roles if any
                    new Claim(ClaimTypes.Role, "RoleName"),
                    new Claim(ClaimTypes.Role, "AnotherRole"),
                    */
                    },
                    DefaultAuthenticationTypes.ApplicationCookie);

                HttpContext.GetOwinContext().Authentication.SignIn(new AuthenticationProperties { IsPersistent = false }, ident);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "");
                // Devolverse a la misma pagina de Login informando de que hay un error de autenticacion.
                return PartialView(usuario);
            }

        }
        /*
         * GET: Auth/Recuperar
         * EFECTO: retornar vista parcial, la cual implica que no se despliega _Layout de la carpeta Shared.
         * REQUIERE: cshtml con el nombre Recuperar.
         * MODIFICA: n/a
         */
        public ActionResult Recuperar()
        {
            return PartialView();
        }

        /*
         * EFECTO: verifica si el correo existe en la base de datos, y si esta, envia un correo con una contrasena nueva.
         * REQUIERE: un correo.
         * MODIFICA: la variable Resultado la cual si es true, enviaria el correo.
         */
        [HttpPost]
        public ActionResult Recuperar(Usuario usuario)
        {
            ObjectParameter exito = new ObjectParameter("Resultado", 0);
            db.SP_ExistenciaCorreo(usuario.CorreoInstitucional, exito);

            if ((bool)exito.Value == true)
            {
                // Enviar correo
            }
            ModelState.AddModelError(string.Empty, "");
            return PartialView(usuario);
        }
    }
}