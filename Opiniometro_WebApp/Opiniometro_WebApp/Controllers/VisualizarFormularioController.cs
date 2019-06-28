using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Opiniometro_WebApp.Models;
using System.Data.Entity.Core.Objects;
using System.Web.Helpers; //Para graficos, borrar despues

namespace Opiniometro_WebApp.Controllers
{
    public class VisualizarFormularioController : Controller
    {


        private Opiniometro_DatosEntities db;

        public VisualizarFormularioController()
        {
            db = new Opiniometro_DatosEntities();
        }

        public VisualizarFormularioController(Opiniometro_DatosEntities db)
        {
            this.db = db;
        }

        // GET: VisualizarFormulario
        public ActionResult Index()
        {
            ViewBag.CursoID = new SelectList(db.Curso, "Sigla", "Nombre");
            var semestres = (from sem in db.Ciclo_Lectivo select sem.Semestre).AsEnumerable().Distinct();
            ViewBag.SemestreId = new SelectList(semestres);
            var annos = (from ann in db.Ciclo_Lectivo select ann.Anno).AsEnumerable().Distinct();
            ViewBag.AnnoId = new SelectList(annos);

            var grupos = (from grup in db.Grupo select grup.Numero).AsEnumerable().Distinct();
            ViewBag.GrupoID = new SelectList(grupos);

            IQueryable<Formulario> formularios = from form in db.Formulario select form;
            return View(formularios);
        }

        [HttpPost]
        public ActionResult Index(int? x)
        {
            ViewBag.CursoID = new SelectList(db.Curso, "Sigla", "Nombre");
            var semestres = (from sem in db.Ciclo_Lectivo select sem.Semestre).AsEnumerable().Distinct();
            ViewBag.SemestreId = new SelectList(semestres);
            var annos = (from ann in db.Ciclo_Lectivo select ann.Anno).AsEnumerable().Distinct();
            ViewBag.AnnoId = new SelectList(annos);
            var grupos = (from grup in db.Grupo select grup.Numero).AsEnumerable().Distinct();
            ViewBag.GrupoID = new SelectList(grupos);

            IQueryable<Formulario> formulariosO = from form in db.Formulario select form;

            String selectcurso = Request.Form["selectcurso"];
            String selectsemestre = Request.Form["selectsemestre"];
            String selectanno = Request.Form["selectanno"];
            String selecgrupo = Request.Form["selecgrupo"];
            if (!String.IsNullOrEmpty(selectsemestre) || !String.IsNullOrEmpty(selectcurso) || !String.IsNullOrEmpty(selectanno) || !String.IsNullOrEmpty(selecgrupo))
            {
                IQueryable<Responde> formularios = from form in db.Responde select form;

                if (!String.IsNullOrEmpty(selectcurso))
                {
                    formularios = formularios.Where(f => f.SiglaGrupoResp.Equals(selectcurso));
                }
                if (!String.IsNullOrEmpty(selectsemestre))
                {
                    formularios = formularios.Where(f => f.SemestreGrupoResp.Equals(Int32.Parse(selectsemestre)));
                }
                if (!String.IsNullOrEmpty(selectanno))
                {
                    formularios = formularios.Where(f => f.AnnoGrupoResp.Equals(Int32.Parse(selectanno)));
                }
                if (!String.IsNullOrEmpty(selecgrupo))
                {
                    formularios = formularios.Where(f => f.NumeroGrupoResp.Equals(Int32.Parse(selecgrupo)));
                }
                List<Formulario> formulariosfiltrados = new List<Formulario>();
                foreach (var f in formularios)
                {
                    Formulario nuevo = db.Formulario.Find(f.CodigoFormularioResp);

                    formulariosfiltrados.Add(nuevo);
                }
                return View(formulariosfiltrados);
            }
            else
            {
                return View(formulariosO);
            }
        }

        // GET: VisualizarFormulario/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Formulario formulario = db.Formulario.Find(id);
            if (formulario == null)
            {
                return HttpNotFound();
            }

            return View(formulario);
        }

        // GET: VisualizarFormulario/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: VisualizarFormulario/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CodigoFormulario,Nombre")] Formulario formulario)
        {
            if (ModelState.IsValid)
            {
                db.Formulario.Add(formulario);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(formulario);
        }

        // GET: VisualizarFormulario/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Formulario formulario = db.Formulario.Find(id);
            if (formulario == null)
            {
                return HttpNotFound();
            }
            return View(formulario);
        }

        // POST: VisualizarFormulario/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CodigoFormulario,Nombre")] Formulario formulario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(formulario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(formulario);
        }

        // GET: VisualizarFormulario/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Formulario formulario = db.Formulario.Find(id);
            if (formulario == null)
            {
                return HttpNotFound();
            }
            return View(formulario);
        }

        // POST: VisualizarFormulario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Formulario formulario = db.Formulario.Find(id);
            db.Formulario.Remove(formulario);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //EFE: Devuelve un Int con la cantidad de respuestas por respuesta.
        //REQ: Que exista la conexion a la base de datos.
        //MOD:--
        [HttpGet]
        private ObjectResult<SP_ContarRespuestasPorGrupo_Result> ObtenerCantidadRespuestasPorPregunta(string codigoFormulario, string cedulaProfesor, short? annoGrupo, byte? semestreGrupo, byte? numeroGrupo, string siglaCurso, string itemId)
        {
            var result = db.SP_ContarRespuestasPorGrupo(codigoFormulario, cedulaProfesor, annoGrupo, semestreGrupo, numeroGrupo, siglaCurso, itemId);
            return result;
        }

        //EFE:Crea un gráfico con la información de los resultados en la base de datos.
        //REQ:Que exista una conexion a la base de datos.
        //MOD:--
        public JsonResult GraficoPie(string itemId)
        {
            var result = ObtenerCantidadRespuestasPorPregunta("131313", "100000002", 2017, 2, 1, "CI1330", itemId).ToList();//ObtenerCantidadRespuestasPorPregunta  "PRE303"
            //int tamanio = result.Count;
            List<object> x = new List<object>();
            List<object> y = new List<object>();
            //string[] leyenda = new string[tamanio];
            //int?[] cntResps = new int?[tamanio];
            //int iter = 0;
            foreach (var itemR in result)
            {
                //leyenda[iter] = itemR.Respuesta;
                //cntResps[iter] = itemR.cntResp;
                //iter++;
                x.Add(itemR.Respuesta);
                y.Add(itemR.cntResp);
            }
            List<object> lista = new List<object> { x, y };
            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        //EFE:Retorna las observaciones del item.
        //REQ:Que exista una conexion a la base de datos.
        //MOD:--
        [HttpGet]
        private ObjectResult<SP_DevolverObservacionesPorGrupo_Result> ObtenerObservacionesPorGrupo(string codigoFormulario, string cedulaProfesor, short? annoGrupo, byte? semestreGrupo, byte? numeroGrupo, string siglaCurso, string itemId)
        {
            var result = db.SP_DevolverObservacionesPorGrupo(codigoFormulario, cedulaProfesor, annoGrupo, semestreGrupo, numeroGrupo, siglaCurso, itemId);
            return result;
        }

        //EFE:Devuelve las observaciones asignadas a una pregunta en especifico.
        //REQ:Que exista una conexion a la base de datos.
        //MOD:--
        public JsonResult ObservacionesPorPregunta(string codigoFormulario, string cedulaProfesor, short annoGrupo, byte semestreGrupo, byte numeroGrupo, string siglaCurso, string itemId)
        {
            var result = ObtenerObservacionesPorGrupo(codigoFormulario, cedulaProfesor, annoGrupo, semestreGrupo, numeroGrupo, siglaCurso, itemId).ToList();

            List<object> obs = new List<object>();
            List<object> nom = new List<object>();
            List<object> ap1 = new List<object>();
            List<object> ap2 = new List<object>();

            foreach (var itemO in result)
            {
                obs.Add(itemO.Observacion);
                nom.Add(itemO.Nombre);
                ap1.Add(itemO.Apellido1);
                ap2.Add(itemO.Apellido2);
            }

            List<object> observaciones = new List<object> { obs, nom, ap1, ap2 };
            return Json(observaciones, JsonRequestBehavior.AllowGet);
        }

        //EFE:Devuelve las preguntas de tipo texto libre.
        //REQ:Que exista una conexion a la base de datos.
        //MOD:--
        public JsonResult ObtenerRespTexto(string codigoFormulario, string cedulaProfesor, short annoGrupo, byte semestreGrupo, byte numeroGrupo, string siglaCurso, string itemId)
        {
            var result = db.ObtenerCantidadRespuestasPorPregunta(codigoFormulario, cedulaProfesor, annoGrupo, semestreGrupo, numeroGrupo, siglaCurso, itemId);

            List<object> respuestasTexto = new List<object>();

            foreach (var item in result)
            {
                respuestasTexto.Add(item.Respuesta);
            }

            List<object> respuestas = new List<object> { respuestasTexto };
            return Json(respuestas, JsonRequestBehavior.AllowGet);
        }
    }
}