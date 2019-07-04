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
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Opiniometro_WebApp.Controllers
{
   [Authorize]
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
                Conformados = db.Conformado_Item_Sec_Form
            
                    .Where(m => m.CodigoFormulario == codForm)
                    .OrderBy(m => m.TituloSeccion)
                    .ThenBy(m => m.Orden_Item)
                    .ToList(),

                ConformadoS = db.Conformado_For_Sec//Llenamos nuestra lista de secciones del formulario.                   
                    .Where(m => m.CodigoFormulario == codForm)
                    .OrderBy(m => m.TituloSeccion)
                    .ThenBy(m => m.Orden_Seccion)
                    .ToList(),
            FormularioCompleto = new FormularioCompletoModel()//Modelo donde obtenemos las 
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
            }

        };
            //ViewBag.CodigoFormulario = new SelectList(db.Conformado_For_Sec.Distinct(), "CodigoFormulario", "CodigoFormulario");
            ViewBag.CodigoFormulario = new SelectList(db.Formulario.Distinct(), "CodigoFormulario", "Nombre");
            ViewBag.TituloSeccion = new SelectList(db.Conformado_For_Sec.Where(m => m.CodigoFormulario == codForm), "TituloSeccion", "TituloSeccion");

            return View(crearFormulario);
        }

        // GET: CrearFormulario
        public ActionResult AsignarSecciones(string codForm)
        {

            
            CrearFormularioModel conformado_For_Sec = new CrearFormularioModel
            {

                
                Formulario = db.Formulario.Find(codForm),// le pasamos
                Conformado_For_Secs = db.Conformado_For_Sec
                   .ToList()

            };

            //ViewBag.CodigoFormulario = new SelectList(db.Formulario, "Codigo", "Codigo");

            return View(conformado_For_Sec);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public PartialViewResult AgregarConformado(Conformado_Item_Sec_Form conformado)
        {


            if (ModelState.IsValid)
            {
                List<Conformado_Item_Sec_Form> conf = db.Conformado_Item_Sec_Form.Where(m => m.ItemId == conformado.ItemId && m.TituloSeccion == conformado.TituloSeccion && m.CodigoFormulario == conformado.CodigoFormulario).ToList();
                if (conf.Count == 0)
                {
                    db.Conformado_Item_Sec_Form.Add(conformado);
                    db.SaveChanges();
                }
                else
                {
                    return null;
                }                
            }
            //A
            List<Conformado_Item_Sec_Form> conformados =
                    db.Conformado_Item_Sec_Form
                    .Include("Item")
                    .Where(m => m.CodigoFormulario == conformado.CodigoFormulario && m.TituloSeccion == conformado.TituloSeccion)
                    .OrderBy(m => m.Orden_Item)
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
        //----------------------------------------------------------------------------
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

        public ActionResult VistaPreviaPregunta(string id)
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
            return PartialView(item);
        }
        public ActionResult ModalPopUp()
        {
            return View(); 
        }
    }
}