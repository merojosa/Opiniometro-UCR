using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Opiniometro_WebApp.Models;

namespace Opiniometro_WebApp.Controllers
{
    public class AsignarFormulariosController : Controller
    {
        private Opiniometro_DatosEntities db = new Opiniometro_DatosEntities();

        // GET: AsignarFormularios
        public ActionResult Index(string searchString)
        {
            var cursos = from m in db.Curso
                         select m;
            //searchString = "Programacion";
            if (!String.IsNullOrEmpty(searchString))
            {
                cursos = cursos.Where(s => s.Nombre.Contains(searchString));
            }

            var modelo = new AsignarFormulariosModel { Cursos = cursos };
            return View(modelo);
        }
    }
}