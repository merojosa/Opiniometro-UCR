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
    
    public class CrearFormularioController : Controller
    {
        private Opiniometro_DatosEntities db = new Opiniometro_DatosEntities();
        // GET: CrearFormulario
        public ActionResult asignarPreguntas()
        {
            return View();
        }
        public ActionResult preguntasVParcial()
        {
           List<Item> preguntas = db.Item.ToList();

            return PartialView(preguntas);
        }
        public ActionResult seccionesVParcial()
        {
            List<Seccion> secciones = db.Seccion.ToList();
            return PartialView(secciones);
        }
    }
}