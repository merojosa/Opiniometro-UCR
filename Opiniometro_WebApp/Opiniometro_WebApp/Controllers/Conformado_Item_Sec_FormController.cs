/*using System;
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
    public class Conformado_Item_Sec_FormController : Controller
    {
        private Opiniometro_DatosEntities db = new Opiniometro_DatosEntities();

        // GET: Conformado_Item_Sec_Form
        public ActionResult Index()
        {
            var conformado_Item_Sec_Form = db.Conformado_Item_Sec_Form.Include(c => c.Formulario).Include(c => c.Item).Include(c => c.Seccion);
            return View(conformado_Item_Sec_Form.ToList());
        }

        // GET: Conformado_Item_Sec_Form/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Conformado_Item_Sec_Form conformado_Item_Sec_Form = db.Conformado_Item_Sec_Form.Find(id);
            if (conformado_Item_Sec_Form == null)
            {
                return HttpNotFound();
            }
            return View(conformado_Item_Sec_Form);
        }

        // GET: Conformado_Item_Sec_Form/Create
        public ActionResult Create()
        {
            ViewBag.CodigoFormulario = new SelectList(db.Formulario, "CodigoFormulario", "Nombre");
            ViewBag.ItemId = new SelectList(db.Item, "ItemID", "TextoPregunta");
            ViewBag.TituloSeccion = new SelectList(db.Seccion, "Titulo", "Descripcion");
            return View();
        }

        // POST: Conformado_Item_Sec_Form/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ItemId,CodigoFormulario,TituloSeccion,NombreFormulario")] Conformado_Item_Sec_Form conformado_Item_Sec_Form)
        {
            if (ModelState.IsValid)
            {
                db.Conformado_Item_Sec_Form.Add(conformado_Item_Sec_Form);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CodigoFormulario = new SelectList(db.Formulario, "CodigoFormulario", "Nombre", conformado_Item_Sec_Form.CodigoFormulario);
            ViewBag.ItemId = new SelectList(db.Item, "ItemID", "TextoPregunta", conformado_Item_Sec_Form.ItemId);
            ViewBag.TituloSeccion = new SelectList(db.Seccion, "Titulo", "Descripcion", conformado_Item_Sec_Form.TituloSeccion);
            return View(conformado_Item_Sec_Form);
        }

        // GET: Conformado_Item_Sec_Form/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Conformado_Item_Sec_Form conformado_Item_Sec_Form = db.Conformado_Item_Sec_Form.Find(id);
            if (conformado_Item_Sec_Form == null)
            {
                return HttpNotFound();
            }
            ViewBag.CodigoFormulario = new SelectList(db.Formulario, "CodigoFormulario", "Nombre", conformado_Item_Sec_Form.CodigoFormulario);
            ViewBag.ItemId = new SelectList(db.Item, "ItemID", "TextoPregunta", conformado_Item_Sec_Form.ItemId);
            ViewBag.TituloSeccion = new SelectList(db.Seccion, "Titulo", "Descripcion", conformado_Item_Sec_Form.TituloSeccion);
            return View(conformado_Item_Sec_Form);
        }

        // POST: Conformado_Item_Sec_Form/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ItemId,CodigoFormulario,TituloSeccion,NombreFormulario")] Conformado_Item_Sec_Form conformado_Item_Sec_Form)
        {
            if (ModelState.IsValid)
            {
                db.Entry(conformado_Item_Sec_Form).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CodigoFormulario = new SelectList(db.Formulario, "CodigoFormulario", "Nombre", conformado_Item_Sec_Form.CodigoFormulario);
            ViewBag.ItemId = new SelectList(db.Item, "ItemID", "TextoPregunta", conformado_Item_Sec_Form.ItemId);
            ViewBag.TituloSeccion = new SelectList(db.Seccion, "Titulo", "Descripcion", conformado_Item_Sec_Form.TituloSeccion);
            return View(conformado_Item_Sec_Form);
        }

        // GET: Conformado_Item_Sec_Form/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Conformado_Item_Sec_Form conformado_Item_Sec_Form = db.Conformado_Item_Sec_Form.Find(id);
            if (conformado_Item_Sec_Form == null)
            {
                return HttpNotFound();
            }
            return View(conformado_Item_Sec_Form);
        }

        // POST: Conformado_Item_Sec_Form/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Conformado_Item_Sec_Form conformado_Item_Sec_Form = db.Conformado_Item_Sec_Form.Find(id);
            db.Conformado_Item_Sec_Form.Remove(conformado_Item_Sec_Form);
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
*/