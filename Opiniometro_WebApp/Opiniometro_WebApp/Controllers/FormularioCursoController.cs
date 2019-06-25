using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Opiniometro_WebApp.Models;
using System.Diagnostics;
using Opiniometro_WebApp.Controllers.Servicios;

namespace Opiniometro_WebApp.Controllers
{
    public class FormularioCursoController: Controller
    {
        private Opiniometro_DatosEntities db = new Opiniometro_DatosEntities();

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

  
        public string obtenerCedulaEstLoggeado() {
            string correoUsLog = IdentidadManager.obtener_correo_actual();
            string cedula = (from us in db.Usuario
                             where us.CorreoInstitucional == correoUsLog
                             select us).First().Cedula.ToString();
            return cedula;
        }


    }
}