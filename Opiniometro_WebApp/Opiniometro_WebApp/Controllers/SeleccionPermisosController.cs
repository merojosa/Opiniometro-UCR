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
            if (IdentidadManager.verificar_sesion(this) == true)
            {
                if (IdentidadManager.obtener_perfil_actual() == "Administrador")
                {
                    bool asignado;
                    using (var context = new Opiniometro_DatosEntities())
                    {
                        SeleccionPermisos model = new SeleccionPermisos();
                        model.ListaPerfiles = context.Perfil.ToList();
                        model.ListaPerfilesId = Perfil.ObtenerIds();
                        model.ListaPermisos = context.Permiso.ToList();
                        model.ListaPosee = context.Posee_Enfasis_Perfil_Permiso.ToList();
                        model.ListaEnfasis = context.Enfasis.ToList();
                        model.ListaAsoc = new List<SeleccionPermisos.Asociaciones>();
                        //model.ListaGuardar = new List<SeleccionPermisos.GuardarPerm>();

                        foreach (var posee in model.ListaPosee)
                        {
                            SeleccionPermisos.Asociaciones asoc = new SeleccionPermisos.Asociaciones(posee.IdPerfil, posee.IdPermiso);
                            //Para verificar si ya existe la combinacion de permiso en una tupla solamente de permiso (id) y perfil (id)
                            //para no insertarla en la lista ListaAsoc. Todo esto pues hay tuplas muy similares debido a que lo que difiere es el enfasis
                            if (!model.ListaAsoc.Any(item => item.Perfil == asoc.Perfil && item.Permiso == asoc.Permiso))
                            {
                                model.ListaAsoc.Add(asoc);
                            }
                        }

                        foreach (var a in model.ListaAsoc)
                        {
                            System.Diagnostics.Debug.Print(a.Perfil + " " + a.Permiso);
                        }

                        foreach (var perfil in model.ListaPerfiles)
                        {
                            foreach (var permiso in model.ListaPermisos)
                            {
                                asignado = false;
                                for (int cont = 0; cont < model.ListaAsoc.Count; cont++)
                                {
                                    if (perfil.Id == model.ListaAsoc[cont].Perfil)
                                    {
                                        if (permiso.Id == model.ListaAsoc[cont].Permiso)
                                        {
                                            model.ListaGuardar.Add(new SeleccionPermisos.GuardarPerm(perfil.Id, permiso.Id));
                                        }
                                    }
                                }
                            }
                        }

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