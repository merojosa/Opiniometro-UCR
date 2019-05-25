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
    public class Texto_LibreController : Controller
    {
        private Opiniometro_DatosEntities db = new Opiniometro_DatosEntities();

        // GET: Texto_Libre
        public ActionResult Index()
        {
            var texto_Libre = db.Texto_Libre.Include(t => t.Item);
            return View(texto_Libre.ToList());
        }

        // GET: Texto_Libre/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Texto_Libre texto_Libre = db.Texto_Libre.Find(id);
            if (texto_Libre == null)
            {
                return HttpNotFound();
            }
            return View(texto_Libre);
        }

        // GET: Texto_Libre/Create
        public ActionResult Create()
        {
            ViewBag.ItemId = new SelectList(db.Item, "ItemID", "TextoPregunta");
            return View();
        }

        // POST: Texto_Libre/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ItemId")] Texto_Libre texto_Libre)
        {
            if (ModelState.IsValid)
            {
                db.Texto_Libre.Add(texto_Libre);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ItemId = new SelectList(db.Item, "ItemID", "TextoPregunta", texto_Libre.ItemId);
            return View(texto_Libre);
        }

        // GET: Texto_Libre/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Texto_Libre texto_Libre = db.Texto_Libre.Find(id);
            if (texto_Libre == null)
            {
                return HttpNotFound();
            }
            ViewBag.ItemId = new SelectList(db.Item, "ItemID", "TextoPregunta", texto_Libre.ItemId);
            return View(texto_Libre);
        }

        // POST: Texto_Libre/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ItemId")] Texto_Libre texto_Libre)
        {
            if (ModelState.IsValid)
            {
                db.Entry(texto_Libre).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ItemId = new SelectList(db.Item, "ItemID", "TextoPregunta", texto_Libre.ItemId);
            return View(texto_Libre);
        }

        // GET: Texto_Libre/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Texto_Libre texto_Libre = db.Texto_Libre.Find(id);
            if (texto_Libre == null)
            {
                return HttpNotFound();
            }
            return View(texto_Libre);
        }

        // POST: Texto_Libre/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Texto_Libre texto_Libre = db.Texto_Libre.Find(id);
            db.Texto_Libre.Remove(texto_Libre);
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
