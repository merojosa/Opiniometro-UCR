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

        // GET: Auth/Login
        public ActionResult Login()
        {
            return View();
        }

        /*  
         *  Action method que recibe un correo y contraseña por medio de un FormCollection y verifica en la base de datos si se puede
         *  autenticar.
         *  Fuente: https://stackoverflow.com/questions/31584506/how-to-implement-custom-authentication-in-asp-net-mvc-5
        */
        [HttpPost]
        public ActionResult Login(FormCollection form_collection)
        {
            ObjectParameter exito = new ObjectParameter("Resultado", 0);
            db.SP_LoginUsuario(form_collection["Correo"], form_collection["Contrasenna"], exito);
            
            // Si se pudo autenticar.
            if ((bool)exito.Value == true)
            {
                var ident = new ClaimsIdentity(
                    new[] { 
                    new Claim(ClaimTypes.NameIdentifier, form_collection["Correo"]),
                    new Claim(ClaimTypes.Name, form_collection["Correo"]),

                    /*
                    // optionally you could add roles if any
                    new Claim(ClaimTypes.Role, "RoleName"),
                    new Claim(ClaimTypes.Role, "AnotherRole"),
                    */
                    },
                    DefaultAuthenticationTypes.ApplicationCookie);

                HttpContext.GetOwinContext().Authentication.SignIn( new AuthenticationProperties { IsPersistent = false }, ident);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Devolverse a la misma pagina de Loing informando de que hay un error de autenticacion.
                return RedirectToAction("Login", "Auth");
            }
        }

        public ActionResult Error()
        {
            return View();
        }
    }
}