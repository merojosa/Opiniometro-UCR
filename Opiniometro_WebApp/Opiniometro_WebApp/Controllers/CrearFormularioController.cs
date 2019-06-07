using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Opiniometro_WebApp.Models;


namespace Opiniometro_WebApp.Controllers
{
    public class CrearFormularioController : Controller
    {
        Opiniometro_DatosEntities db = new Opiniometro_DatosEntities();
        // GET: CrearFormulario
        public ActionResult AsignarPreguntas()
        {
            List<Conformado_Item_Sec_Form> todo = db.Conformado_Item_Sec_Form.ToList();
            return View(todo);
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
    }
}