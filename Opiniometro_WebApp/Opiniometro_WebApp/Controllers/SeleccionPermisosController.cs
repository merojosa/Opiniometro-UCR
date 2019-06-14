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
            model.ListaPosee = db.Posee_Enfasis_Perfil_Permiso.ToList();
            int tam = model.ListaPerfiles.Count;

            List<String> listaIds = Perfil.ObtenerIds();

            return View(model);
        }

        [HttpPost]
        public ActionResult Guardar()
        {
            return View();
        }

        private void InsertarPersona(Persona p)
        {
            using (var context = new Opiniometro_DatosEntities())
            {
                context.Persona.Add(p);
            }
            var persona = new Perfil()
            {
                Id = "1234567",
                Tipo = "Felipe"
            };
        }
    }
}