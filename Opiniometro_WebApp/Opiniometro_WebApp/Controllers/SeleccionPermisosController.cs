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
        public ActionResult SeleccionarPermisos()
        {
            SeleccionPermisos seleccionPermisos = new SeleccionPermisos();
            SeleccionPermisos model = seleccionPermisos;
            model.ListaPerfiles = db.Perfil.ToList();
            model.ListaPermisos = db.Permiso.ToList();
            int tam = model.ListaPerfiles.Count;

            model.ListaPerfilesId = new List<string>(new string[tam]);

            for(int i = 0; i < model.ListaPerfiles.Count; i++)
            {
                model.ListaPerfilesId[i] = model.ListaPerfiles[i].Id;
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Guardar()
        {
            return View();
        }
    }
}