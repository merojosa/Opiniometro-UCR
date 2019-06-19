using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Opiniometro_WebApp.Models;
using Opiniometro_WebApp.Controllers.Servicios;

namespace Opiniometro_WebApp.Controllers
{
    [Authorize]
    public class SeleccionPermisosController : Controller
    {
        // GET: SeleccionPermisos
        public ActionResult SeleccionarPermisos()
        {
            if(IdentidadManager.verificar_sesion(this) == true)
            {
                if(IdentidadManager.obtener_perfil_actual() == "Admin")
                {
                    using (var context = new Opiniometro_DatosEntities())
                    {
                        SeleccionPermisos seleccionPermisos = new SeleccionPermisos();
                        SeleccionPermisos model = seleccionPermisos;
                        model.IdPerfil = "";
                        model.ListaPerfiles = context.Perfil.ToList();
                        model.ListaPerfilesId = Perfil.ObtenerIds();
                        model.ListaPermisos = context.Permiso.ToList();
                        model.ListaPosee = context.Posee_Enfasis_Perfil_Permiso.ToList();
                        model.ListaEnfasis = context.Enfasis.ToList();
                        return View(model);
                    }
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Login", "Auth");
            }
        }

        [HttpPost]
        public ActionResult Guardar()
        {
            return View();
        }
    }
}