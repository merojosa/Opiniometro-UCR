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
    [Authorize]
    public class Asignacion_FormularioController : Controller
    {
        private Opiniometro_DatosEntities db = new Opiniometro_DatosEntities();

        // GET: Asignacion_Formulario
        public ActionResult Index()
        {
            var tiene_Grupo_Formulario = db.Tiene_Grupo_Formulario.Include(t => t.Fecha_Corte).Include(t => t.Formulario).Include(t => t.Grupo);
            return View(tiene_Grupo_Formulario.ToList());
        }

        /*/ GET: Asignacion_Formulario/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tiene_Grupo_Formulario tiene_Grupo_Formulario = db.Tiene_Grupo_Formulario.Find(id);
            if (tiene_Grupo_Formulario == null)
            {
                return HttpNotFound();
            }
            ViewBag.FechaInicio = new SelectList(db.Fecha_Corte, "FechaInicio", "FechaInicio", tiene_Grupo_Formulario.FechaInicio);
            ViewBag.Codigo = new SelectList(db.Formulario, "CodigoFormulario", "Nombre", tiene_Grupo_Formulario.Codigo);
            ViewBag.SiglaCurso = new SelectList(db.Grupo, "SiglaCurso", "SiglaCurso", tiene_Grupo_Formulario.SiglaCurso);
            return View(tiene_Grupo_Formulario);
        }

        // POST: Asignacion_Formulario/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SiglaCurso,Numero,Anno,Ciclo,Codigo,FechaInicio,FechaFinal")] Tiene_Grupo_Formulario tiene_Grupo_Formulario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tiene_Grupo_Formulario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FechaInicio = new SelectList(db.Fecha_Corte, "FechaInicio", "FechaInicio", tiene_Grupo_Formulario.FechaInicio);
            ViewBag.Codigo = new SelectList(db.Formulario, "CodigoFormulario", "Nombre", tiene_Grupo_Formulario.Codigo);
            ViewBag.SiglaCurso = new SelectList(db.Grupo, "SiglaCurso", "SiglaCurso", tiene_Grupo_Formulario.SiglaCurso);
            return View(tiene_Grupo_Formulario);
        }*/

        // GET: Asignacion_Formulario/Delete/5
        public ActionResult Delete(Grupo GrupoElim, Formulario FormularioElim, Fecha_Corte PeriodoElim)
        {
            if (GrupoElim == null || FormularioElim == null || PeriodoElim == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tiene_Grupo_Formulario tiene_Grupo_Formulario = db.Tiene_Grupo_Formulario.Find(GrupoElim, FormularioElim, PeriodoElim);

            if (tiene_Grupo_Formulario == null)
            {
                return HttpNotFound();
            }

            return View(tiene_Grupo_Formulario);
        }

        // POST: Asignacion_Formulario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Grupo GrupoElim, Formulario FormularioElim, Fecha_Corte PeriodoElim)
        {
            Tiene_Grupo_Formulario tiene_Grupo_Formulario = db.Tiene_Grupo_Formulario.Find(GrupoElim, FormularioElim, PeriodoElim);
            db.Tiene_Grupo_Formulario.Remove(tiene_Grupo_Formulario);
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
