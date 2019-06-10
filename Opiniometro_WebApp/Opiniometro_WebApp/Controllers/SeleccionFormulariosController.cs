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

        // GET: SeleccionFormularios
        public ActionResult SeleccionFormularios()
        {
            return PartialView(db.Formulario.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult POSTSeleccionFormularios(string form)
        {
            IQueryable<Formulario> formulario = from f in db.Formulario select f;

            if (!String.IsNullOrEmpty(form))
            {
                formulario = formulario.Where(f => f.Nombre.Equals(form));
            }

            return PartialView(formulario);
        }
    }
}