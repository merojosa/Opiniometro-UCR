using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Opiniometro_WebApp.Models;
using System.Diagnostics;
using Opiniometro_WebApp.Controllers.Servicios;

namespace Opiniometro_WebApp.Controllers
{
    [Authorize]
    public class FormularioCursoController: Controller
    {
        private Opiniometro_DatosEntities db = new Opiniometro_DatosEntities();

        [HttpGet]
        public ActionResult Index(string cedulaEstudiante, string codigoForm)
        {
            var modelo = new FormularioCursoModel
            {
                preguntasFormulario = obtenerPreguntasFormulario(cedulaEstudiante,codigoForm)
            };
            return View(modelo);
        }

        public IQueryable<FormularioCursoModel> obtenerPreguntasFormulario(string cedulaEstudiante, string codigoForm)
        {

            IQueryable<FormularioCursoModel> formulario =
                from it in db.Item
                join confSecItem in db.Conformado_Item_Sec_Form on it.ItemId equals confSecItem.ItemId
                join sec in db.Seccion on confSecItem.TituloSeccion equals sec.Titulo
                where (confSecItem.CodigoFormulario == codigoForm)

                select new FormularioCursoModel
                {
                    item = it.TextoPregunta,
                    tieneObservacion = it.TieneObservacion
                };

            return formulario;
        }
   
    }
}