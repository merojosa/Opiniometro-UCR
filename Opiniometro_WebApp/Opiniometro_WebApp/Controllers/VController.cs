using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Opiniometro_WebApp.Models;

namespace Opiniometro_WebApp.Controllers
{
    public class VController : Controller
    {
        Opiniometro_DatosEntities db = new Opiniometro_DatosEntities();
        // GET: V
        public ActionResult Index()
        {
            var modelo = new VModel
            {
                Profesores = ObtenerProf(),
            }; return View(modelo);
        }

        public IEnumerable<SelectListItem> ObtenerProf() {
            return db.Profesor



                .Select(prof => new SelectListItem {
                    Value = prof.CedulaProfesor,
                    Text = prof.Persona.Nombre
                }).ToList();
        }
    }
}