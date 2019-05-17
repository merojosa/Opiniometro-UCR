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

        // POST: Responde/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ItemId,TituloSeccion,FechaRespuesta,CodigoFormularioResp,CedulaPersona,CedulaProfesor,AñoGrupoResp,SemestreGrupoResp,NumeroGrupoResp,SiglaGrupoResp,Observacion,Respuesta,RespuestaProfesor")] Responde responde)
        {
            if (ModelState.IsValid)
            {
                db.Responde.Add(responde);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FechaRespuesta = new SelectList(db.Formulario_Respuesta, "Fecha", "CodigoFormulario", responde.FechaRespuesta);
            return View(responde);
        }

        // GET: Responde/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.FechaRespuesta = new SelectList(db.Formulario_Respuesta, "Fecha", "CodigoFormulario", responde.FechaRespuesta);
            return View(responde);
        }

        // POST: Responde/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ItemId,TituloSeccion,FechaRespuesta,CodigoFormularioResp,CedulaPersona,CedulaProfesor,AñoGrupoResp,SemestreGrupoResp,NumeroGrupoResp,SiglaGrupoResp,Observacion,Respuesta,RespuestaProfesor")] Responde responde)
        {
            if (ModelState.IsValid)
            {
                db.Entry(responde).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FechaRespuesta = new SelectList(db.Formulario_Respuesta, "Fecha", "CodigoFormulario", responde.FechaRespuesta);
            return View(responde);
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
