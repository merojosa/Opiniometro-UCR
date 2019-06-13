﻿using System;
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
    
    public class CrearFormularioController : Controller
    {
        Opiniometro_DatosEntities db = new Opiniometro_DatosEntities();
        // GET: CrearFormulario
        public ActionResult AsignarPreguntas(string codForm)
        {
            CrearFormularioModel crearFormulario = new CrearFormularioModel
            {
                Secciones = db.Seccion.ToList(),
                Items = db.Item.ToList(),
                Formulario = db.Formulario.Find(codForm),// le pasamos
                Conformados = new List<Conformado_Item_Sec_Form>()

        };
            
            return View(crearFormulario);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public PartialViewResult AgregarConformado(Conformado_Item_Sec_Form conformado)
        {
            if (ModelState.IsValid)
            {
                db.Conformado_Item_Sec_Form.Add(conformado);
                db.SaveChanges();             
            }
            List<Conformado_Item_Sec_Form> conformados =
                    db.Conformado_Item_Sec_Form
                    .Where(m => m.CodigoFormulario == conformado.CodigoFormulario && m.Seccion == conformado.Seccion)
                    .ToList();
            return PartialView("ConformadoVParcial", conformados);

        }

        [HttpGet]
        public JsonResult GetConformados()
        {
            return Json(db.Conformado_Item_Sec_Form.First().Seccion, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SeccionesAsignadas(string CodForm)
        {
            List<Conformado_Item_Sec_Form> SeccionesAsig = db.Conformado_Item_Sec_Form.Where(m => m.CodigoFormulario == CodForm).Distinct().ToList();
            return PartialView(SeccionesAsig);
        }

        public ActionResult PreguntasAsignadas(string CodForm, string TituloSecc)
        {
            List<Conformado_Item_Sec_Form> PreguntasAsig = db.Conformado_Item_Sec_Form.Where(m => m.CodigoFormulario == CodForm).Where(m => m.TituloSeccion == TituloSecc).ToList();
            return PartialView(PreguntasAsig);
        }
        public ActionResult PreguntasVParcial()
        {
           List<Item> preguntas = db.Item.ToList();

            return PartialView(preguntas);
        }
        public ActionResult SeccionesVParcial()
        {
            List<Seccion> secciones = db.Seccion.ToList();
            return PartialView(secciones);
        }
        // Metodo para poder validar si un numero esta disponible. 
        public JsonResult IsOrden_ItemAvailable(int Orden_Item)
        {
            return Json(!db.Conformado_Item_Sec_Form.Any(pregunta => pregunta.Orden_Item == Orden_Item), JsonRequestBehavior.AllowGet);

        }
        public JsonResult IsOrden_SeccionAvailable(int Orden_Seccion)
        {
            return Json(!db.Conformado_Item_Sec_Form.Any(pregunta => pregunta.Orden_Seccion == Orden_Seccion), JsonRequestBehavior.AllowGet);

        }
    }
}