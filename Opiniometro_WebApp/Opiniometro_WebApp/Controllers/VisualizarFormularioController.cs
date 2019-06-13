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
    public class VisualizarFormularioController : Controller
    {
        private Opiniometro_DatosEntities db = new Opiniometro_DatosEntities();

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
        public ActionResult Index(String selectcurso, String selectsemestre, String selectanno, String selecgrupo)
        {
            ViewBag.CursoID = new SelectList(db.Curso, "Sigla", "Nombre");
            var semestres = (from sem in db.Ciclo_Lectivo select sem.Semestre).AsEnumerable().Distinct();
            ViewBag.SemestreId = new SelectList(semestres);
            var annos = (from ann in db.Ciclo_Lectivo select ann.Anno).AsEnumerable().Distinct();
            ViewBag.AnnoId = new SelectList(annos);
            var grupos = (from grup in db.Grupo select grup.Numero).AsEnumerable().Distinct();
            ViewBag.GrupoID = new SelectList(grupos);

            IQueryable<Formulario> formulariosO = from form in db.Formulario select form;

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
    }
}