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
    public class Formulario_RespuestaController : Controller
    {
        private Opiniometro_DatosEntities db;
        public Formulario_RespuestaController()
        {
            db =  new Opiniometro_DatosEntities();
        }

        public Formulario_RespuestaController(Opiniometro_DatosEntities db)
        {
            this.db = db;
        }
        // GET: Formulario_Respuesta
        public ActionResult Index()
        {
            var formulario_Respuesta = db.Formulario_Respuesta.Include(f => f.Formulario).Include(f => f.Grupo).Include(f => f.Persona).Include(f => f.Profesor);
            return View(formulario_Respuesta.ToList());
        }

        // GET: Formulario_Respuesta/Details/5
        public ActionResult Details(DateTime id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Formulario_Respuesta formulario_Respuesta = db.Formulario_Respuesta.Find(id);
            if (formulario_Respuesta == null)
            {
                return HttpNotFound();
            }
            return View(formulario_Respuesta);
        }

        // GET: Formulario_Respuesta/Create
        public ActionResult Create()
        {
            ViewBag.CodigoFormulario = new SelectList(db.Formulario, "CodigoFormulario", "Nombre");
            ViewBag.SiglaGrupo = new SelectList(db.Grupo, "SiglaCurso", "SiglaCurso");
            ViewBag.CedulaPersona = new SelectList(db.Persona, "Cedula", "Nombre");
            ViewBag.CedulaProfesor = new SelectList(db.Profesor, "CedulaProfesor", "CedulaProfesor");
            return View();
        }

        // POST: Formulario_Respuesta/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Fecha,CodigoFormulario,CedulaPersona,CedulaProfesor,AnnoGrupo,SemestreGrupo,NumeroGrupo,SiglaGrupo,Completado")] Formulario_Respuesta formulario_Respuesta)
        {
            if (ModelState.IsValid)
            {
                db.Formulario_Respuesta.Add(formulario_Respuesta);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CodigoFormulario = new SelectList(db.Formulario, "CodigoFormulario", "Nombre", formulario_Respuesta.CodigoFormulario);
            ViewBag.SiglaGrupo = new SelectList(db.Grupo, "SiglaCurso", "SiglaCurso", formulario_Respuesta.SiglaGrupo);
            ViewBag.CedulaPersona = new SelectList(db.Persona, "Cedula", "Nombre", formulario_Respuesta.CedulaPersona);
            ViewBag.CedulaProfesor = new SelectList(db.Profesor, "CedulaProfesor", "CedulaProfesor", formulario_Respuesta.CedulaProfesor);
            return View(formulario_Respuesta);
        }

        // GET: Formulario_Respuesta/Edit/5
        public ActionResult Edit(DateTime id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Formulario_Respuesta formulario_Respuesta = db.Formulario_Respuesta.Find(id);
            if (formulario_Respuesta == null)
            {
                return HttpNotFound();
            }
            ViewBag.CodigoFormulario = new SelectList(db.Formulario, "CodigoFormulario", "Nombre", formulario_Respuesta.CodigoFormulario);
            ViewBag.SiglaGrupo = new SelectList(db.Grupo, "SiglaCurso", "SiglaCurso", formulario_Respuesta.SiglaGrupo);
            ViewBag.CedulaPersona = new SelectList(db.Persona, "Cedula", "Nombre", formulario_Respuesta.CedulaPersona);
            ViewBag.CedulaProfesor = new SelectList(db.Profesor, "CedulaProfesor", "CedulaProfesor", formulario_Respuesta.CedulaProfesor);
            return View(formulario_Respuesta);
        }

        // POST: Formulario_Respuesta/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Fecha,CodigoFormulario,CedulaPersona,CedulaProfesor,AnnoGrupo,SemestreGrupo,NumeroGrupo,SiglaGrupo,Completado")] Formulario_Respuesta formulario_Respuesta)
        {
            if (ModelState.IsValid)
            {
                db.Entry(formulario_Respuesta).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CodigoFormulario = new SelectList(db.Formulario, "CodigoFormulario", "Nombre", formulario_Respuesta.CodigoFormulario);
            ViewBag.SiglaGrupo = new SelectList(db.Grupo, "SiglaCurso", "SiglaCurso", formulario_Respuesta.SiglaGrupo);
            ViewBag.CedulaPersona = new SelectList(db.Persona, "Cedula", "Nombre", formulario_Respuesta.CedulaPersona);
            ViewBag.CedulaProfesor = new SelectList(db.Profesor, "CedulaProfesor", "CedulaProfesor", formulario_Respuesta.CedulaProfesor);
            return View(formulario_Respuesta);
        }

        // GET: Formulario_Respuesta/Delete/5
        public ActionResult Delete(DateTime id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Formulario_Respuesta formulario_Respuesta = db.Formulario_Respuesta.Find(id);
            if (formulario_Respuesta == null)
            {
                return HttpNotFound();
            }
            return View(formulario_Respuesta);
        }

        // POST: Formulario_Respuesta/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(DateTime id)
        {
            Formulario_Respuesta formulario_Respuesta = db.Formulario_Respuesta.Find(id);
            db.Formulario_Respuesta.Remove(formulario_Respuesta);
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
