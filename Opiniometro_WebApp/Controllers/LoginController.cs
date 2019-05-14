using System;
using System.Data.Entity.Core.Objects;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Opiniometro_WebApp.Models;

namespace Opiniometro_WebApp.Controllers
{
    public class LoginController : Controller
    {
        private Opiniometro_DatosEntities db = new Opiniometro_DatosEntities();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        // Action method que recibe un correo y contraseña por medio de un FormCollection.
        [HttpPost]
        public string Login(FormCollection form_collection)
        {
            ObjectParameter exito = new ObjectParameter("Resultado", 0);
            db.SP_LoginUsuario(form_collection["Correo"], form_collection["Contrasenna"], exito);

            if ((bool)exito.Value == true)
                return "Login exitoso";
            else
                return "Login fallido";
        }
    }
}