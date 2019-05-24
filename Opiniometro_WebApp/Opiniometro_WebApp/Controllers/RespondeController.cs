using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Opiniometro_WebApp.Models;
using System.Data.SqlClient;

namespace Opiniometro_WebApp.Controllers
{
    public class RespondeController : Controller
    {
        private Opiniometro_DatosEntities db = new Opiniometro_DatosEntities();

        // GET: Responde
        public ActionResult Index()
        {
            var responde = db.Responde.Include(r => r.Formulario_Respuesta);
            return View(responde.ToList());
        }

        // GET: Responde/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Responde responde = db.Responde.Find(id);
            if (responde == null)
            {
                return HttpNotFound();
            }
            return View(responde);
        }

        [HttpGet]
        private int ObtenerCantidadRespuestasPorPregunta(string codigoFormulario, string cedulaProfesor, short annoGrupo, byte semestreGrupo, byte numeroGrupo, string siglaCurso, int itemId, string respuesta)
        {
            ObjectParameter resultado = new ObjectParameter("resultado", typeof(int));

            db.SP_ContarRespuestasPorGrupo(codigoFormulario, cedulaProfesor, annoGrupo, semestreGrupo, numeroGrupo, siglaCurso, itemId, respuesta, resultado);

            return Convert.ToInt32(resultado.Value);

        }

        //[HttpGet]
        //private int ObtenerObservaciones(int cursoId)
        //{
        //    //Parametro para que se guarde el resultado             
        //    ObjectParameter resultado = new ObjectParameter("resultado", typeof(Double));

        //    //Invoca el metodo            
        //    db.ProcObtenerPromedioCurso(cursoId, resultado);

        //    //Devuelve el resultado            
        //    return Convert.ToDouble(resultado.Value);

        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
