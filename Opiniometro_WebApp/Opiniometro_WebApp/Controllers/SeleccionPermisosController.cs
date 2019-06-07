using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Opiniometro_WebApp.Models;

namespace Opiniometro_WebApp.Controllers
{
    public class SeleccionPermisosController : Controller
    {
        private Opiniometro_DatosEntities db = new Opiniometro_DatosEntities();
        // GET: SeleccionPermisos
        public ActionResult SeleccionPermisosView()
        {
            return View();
        }
    }
}