using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Opiniometro_WebApp.Models;

namespace Opiniometro_WebApp.Controllers
{
    public class LlenarFormularioController : Controller
    {
        private Opiniometro_DatosEntities db = new Opiniometro_DatosEntities();
        // GET: LlenarFormulario
        public ActionResult Index()
        {
            return View(db.Conformado_Item_Sec_Form.Where(f => f.CodigoFormulario.Contains("121212")));
        }
    }
}