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
    public class Seleccion_UnicaController : Controller
    {
        private Opiniometro_DatosEntities db = new Opiniometro_DatosEntities();

        // GET: Seleccion_Unica
        public ActionResult Index()
        {
            var seleccion_Unica = db.Seleccion_Unica.Include(s => s.Item);
            return View(seleccion_Unica.ToList());
        }

        // GET: Seleccion_Unica/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Seleccion_Unica seleccion_Unica = db.Seleccion_Unica.Find(id);
            if (seleccion_Unica == null)
            {
                return HttpNotFound();
            }
            return View(seleccion_Unica);
        }

        // GET: Seleccion_Unica/Create
        public ActionResult Create()
        {
            ViewBag.ItemId = new SelectList(db.Item, "ItemId", "TextoPregunta");
            return View();
        }

        // POST: Seleccion_Unica/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ItemId,IsaLikeDislike")] Seleccion_Unica seleccion_Unica)
        {
            if (ModelState.IsValid)
            {
                db.Seleccion_Unica.Add(seleccion_Unica);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ItemId = new SelectList(db.Item, "ItemId", "TextoPregunta", seleccion_Unica.ItemId);
            return View(seleccion_Unica);
        }

        // GET: Seleccion_Unica/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Seleccion_Unica seleccion_Unica = db.Seleccion_Unica.Find(id);
            if (seleccion_Unica == null)
            {
                return HttpNotFound();
            }
            ViewBag.ItemId = new SelectList(db.Item, "ItemId", "TextoPregunta", seleccion_Unica.ItemId);
            return View(seleccion_Unica);
        }

        // POST: Seleccion_Unica/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ItemId,IsaLikeDislike")] Seleccion_Unica seleccion_Unica)
        {
            if (ModelState.IsValid)
            {
                db.Entry(seleccion_Unica).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ItemId = new SelectList(db.Item, "ItemId", "TextoPregunta", seleccion_Unica.ItemId);
            return View(seleccion_Unica);
        }

        // GET: Seleccion_Unica/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Seleccion_Unica seleccion_Unica = db.Seleccion_Unica.Find(id);
            if (seleccion_Unica == null)
            {
                return HttpNotFound();
            }
            return View(seleccion_Unica);
        }

        // POST: Seleccion_Unica/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Seleccion_Unica seleccion_Unica = db.Seleccion_Unica.Find(id);
            db.Seleccion_Unica.Remove(seleccion_Unica);
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
