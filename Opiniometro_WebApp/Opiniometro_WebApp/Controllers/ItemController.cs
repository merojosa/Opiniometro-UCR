using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Opiniometro_WebApp.Models;

namespace Opiniometro_WebApp.Controllers
{
    public class ItemController : Controller
    {
        private Opiniometro_DatosEntities db = new Opiniometro_DatosEntities();

         //GET: Item
        public ActionResult Index()
        {
           
            var item = db.Item.Include(i => i.Seleccion_Unica).Include(i => i.Texto_Libre);
            return View(item.ToList());
        }

         //GET: Item/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Item.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

         //GET: Item/Create
        public ActionResult Create()
        {
            ViewBag.TipoPreguntaItems = new List<ListItem>
            {
                  new ListItem { Text = "Sí-No", Value="3" },
                  new ListItem { Text = "Texto Libre", Value="1" }
            };
            ViewBag.BooleanItems = new List<ListItem>
            {
                  new ListItem { Text = "Sí", Value="true" },
                  new ListItem { Text = "No", Value="false" }
            };

            ViewBag.NombreCategoria = new SelectList(db.Categoria, "NombreCategoria", "NombreCategoria");
            ViewBag.ItemID = new SelectList(db.Seleccion_Unica, "ItemID", "ItemID");
            ViewBag.ItemID = new SelectList(db.Texto_Libre, "ItemId", "ItemId");
            return View();
        }

         //POST: Item/Create
         //To protect from overposting attacks, please enable the specific properties you want to bind to, for 
         //more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ItemID,TextoPregunta,TieneObservacion,TipoPregunta,NombreCategoria")] Item item)
        {



            if (ModelState.IsValid)
            {
                db.Item.Add(item);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ItemID = new SelectList(db.Seleccion_Unica, "ItemID", "ItemID", item.ItemId);
            ViewBag.ItemID = new SelectList(db.Texto_Libre, "ItemId", "ItemId", item.ItemId);
            return View(item);
        }

         //GET: Item/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Item.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            ViewBag.ItemID = new SelectList(db.Seleccion_Unica, "ItemID", "ItemID", item.ItemId);
            ViewBag.ItemID = new SelectList(db.Texto_Libre, "ItemId", "ItemId", item.ItemId);
            return View(item);
        }

         //POST: Item/Edit/5
         //To protect from overposting attacks, please enable the specific properties you want to bind to, for 
         //more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ItemID,TextoPregunta,Categoria,TieneObservacion")] Item item)
        {
            if (ModelState.IsValid)
            {
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ItemID = new SelectList(db.Seleccion_Unica, "ItemID", "ItemID", item.ItemId);
            ViewBag.ItemID = new SelectList(db.Texto_Libre, "ItemId", "ItemId", item.ItemId);
            return View(item);
        }

         //GET: Item/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Item.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }
        
         //POST: Item/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Item item = db.Item.Find(id);
            db.Item.Remove(item);
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
