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
    public class SeleccionUnicaController : Controller
    {
        private Opiniometro_DatosEntities db = new Opiniometro_DatosEntities();

        // GET: SeleccionUnica
        public ActionResult Index()
        {
            var seleccionUnica = db.SeleccionUnica.Include(s => s.Item);
            return View(seleccionUnica.ToList());
        }

        // GET: SeleccionUnica/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SeleccionUnica seleccionUnica = db.SeleccionUnica.Find(id);
            if (seleccionUnica == null)
            {
                return HttpNotFound();
            }
            return View(seleccionUnica);
        }

        // GET: SeleccionUnica/Create
        public ActionResult Create()
        {
            ViewBag.ItemID = new SelectList(db.Item, "ItemID", "TextoPregunta");
            return View();
        }

        // POST: SeleccionUnica/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ItemID,IsaLikeDislike")] SeleccionUnica seleccionUnica)
        {
            if (ModelState.IsValid)
            {
                db.SeleccionUnica.Add(seleccionUnica);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ItemID = new SelectList(db.Item, "ItemID", "TextoPregunta", seleccionUnica.ItemID);
            return View(seleccionUnica);
        }

        // GET: SeleccionUnica/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SeleccionUnica seleccionUnica = db.SeleccionUnica.Find(id);
            if (seleccionUnica == null)
            {
                return HttpNotFound();
            }
            ViewBag.ItemID = new SelectList(db.Item, "ItemID", "TextoPregunta", seleccionUnica.ItemID);
            return View(seleccionUnica);
        }

        // POST: SeleccionUnica/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ItemID,IsaLikeDislike")] SeleccionUnica seleccionUnica)
        {
            if (ModelState.IsValid)
            {
                db.Entry(seleccionUnica).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ItemID = new SelectList(db.Item, "ItemID", "TextoPregunta", seleccionUnica.ItemID);
            return View(seleccionUnica);
        }

        // GET: SeleccionUnica/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SeleccionUnica seleccionUnica = db.SeleccionUnica.Find(id);
            if (seleccionUnica == null)
            {
                return HttpNotFound();
            }
            return View(seleccionUnica);
        }

        // POST: SeleccionUnica/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SeleccionUnica seleccionUnica = db.SeleccionUnica.Find(id);
            db.SeleccionUnica.Remove(seleccionUnica);
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
