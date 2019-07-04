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
        // GET: SeleccionPermisos
        public ActionResult SeleccionarPermisos()
        {
            using (var context = new Opiniometro_DatosEntities())
            {
                SeleccionPermisos seleccionPermisos = new SeleccionPermisos();
                SeleccionPermisos model = seleccionPermisos;
                model.ListaPerfiles = context.Perfil.ToList();
                model.ListaPermisos = context.Permiso.ToList();
                model.ListaPosee = context.Posee_Enfasis_Perfil_Permiso.ToList();
                model.ListaPerfilesId = Perfil.ObtenerIds();
                model.ListaEnfasis = context.Enfasis.ToList();
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult Guardar()
        {
            return View();
        }
    }
}