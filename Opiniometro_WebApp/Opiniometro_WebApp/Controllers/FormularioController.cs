using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using Opiniometro_WebApp.Models;

namespace Opiniometro_WebApp.Controllers
{
    [Authorize]
    public class FormularioController : Controller
    {
        private Opiniometro_DatosEntities db;

        public FormularioController()
        {
            db = new Opiniometro_DatosEntities();
        }

        public FormularioController(Opiniometro_DatosEntities db)
        {
            this.db = db;
        }

        //Method used to know if a CodigoFormulario is already in use
        public JsonResult IsCodigoFormularioAvailable(string CodigoFormulario)
        {
            return Json(!db.Formulario.Any(formulario => formulario.CodigoFormulario == CodigoFormulario), JsonRequestBehavior.AllowGet);
        }

        // GET: Formulario
        public ActionResult Index(int? page)
        {
            var formulario = db.Formulario;
            return View("Index", formulario.ToList().ToPagedList(page ?? 1, 5));
        }

        // GET: Formulario/Details/5
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
            return View("Details", formulario);
        }

        // GET: Formulario/Create
        public ActionResult Create()
        {
            ViewBag.CodigoUnidadAca = new SelectList(db.Unidad_Academica, "Codigo", "Codigo");

            return View("Create");
        }

        // POST: Formulario/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CodigoFormulario,Nombre, CodigoUnidadAca")] Formulario formulario)
        {
            if (ModelState.IsValid)
            {
                db.Formulario.Add(formulario);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View("Create", formulario);
        }

        // GET: Formulario/Edit/5
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
            return View("Edit", formulario);
        }

        // POST: Formulario/Edit/5
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
            return View("Edit", formulario);
        }

        // GET: Formulario/Delete/5
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
            return View("Delete", formulario);
        }

        // POST: Formulario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Formulario formulario = db.Formulario.Find(id);
            db.Formulario.Remove(formulario);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult VistaFormularioVParcial(String codForm)
        {
            FormularioCompletoModel formularioVistaPrevia = new FormularioCompletoModel()//Modelo donde obtenemos las 
            {
                Conformados = db.Conformado_Item_Sec_Form

                    .Where(m => m.CodigoFormulario == codForm)
                    .OrderBy(m => m.TituloSeccion)
                    .ThenBy(m => m.Orden_Item)
                    .ToList(),

                ConformadoS = db.Conformado_For_Sec//Llenamos nuestra lista de secciones del formulario.                   
                    .Where(m => m.CodigoFormulario == codForm)
                    .OrderBy(m => m.TituloSeccion)
                    .ThenBy(m => m.Orden_Seccion)
                    .ToList()
            };

            return PartialView(formularioVistaPrevia);
        }
        public ActionResult SeleccionUnicaVParcial(String id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VistaPreviaPreguntaModel vistaPrevia = new VistaPreviaPreguntaModel
            {
                Item = db.Item.Find(id),
                Opciones = db.Opciones_De_Respuestas_Seleccion_Unica.Where(m => m.ItemId == id).ToList()
            };
            return PartialView(vistaPrevia);
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