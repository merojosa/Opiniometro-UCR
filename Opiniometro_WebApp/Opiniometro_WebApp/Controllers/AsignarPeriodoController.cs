using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Opiniometro_WebApp.Models;
using System.Diagnostics;

namespace Opiniometro_WebApp.Controllers
{
    public class AsignarPeriodoController : Controller
    {
        private Opiniometro_DatosEntities db = new Opiniometro_DatosEntities();

        //prueba para mocking tests
        public AsignarPeriodoController()
        {
            db = new Opiniometro_DatosEntities();
        }

        public AsignarPeriodoController(Opiniometro_DatosEntities db)
        {
            this.db = db;
        }
        // GET: AsignarPeriodo
        public ActionResult Index()
        {
            return View();
        }
    }
}