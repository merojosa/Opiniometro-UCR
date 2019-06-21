using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Opiniometro_WebApp.Models;

namespace Opiniometro_WebApp.Controllers
{
    public class SeleccionFormulariosController : Controller
    {
        private Opiniometro_DatosEntities db = new Opiniometro_DatosEntities();
        //prueba para mocking tests
        public SeleccionFormulariosController()
        {
            db = new Opiniometro_DatosEntities();
        }

        public SeleccionFormulariosController(Opiniometro_DatosEntities db)
        {
            this.db = db;
        }
        /*
        // GET: SeleccionFormularios
        public ActionResult SeleccionFormularios()
        {
            return PartialView(db.Formulario.ToList());
        }*/

        [HttpPost]
        public ActionResult SeleccionFormularios(Formulario forms)
        {
            //IQueryable<Formulario> formulario = from f in db.Formulario select f;

            //if (!String.IsNullOrEmpty(Search))
            //{
            //    formulario = formulario.Where(f => f.Nombre.Equals(Search));
            //}
            return PartialView(forms);
        }

        //método para imprimir lista de formularios de un estudiante
        //public List<Formulario> ObtenerFormularios(String IdEstudiante)
        //{
           

        //    return 0;
        //}

    }
}