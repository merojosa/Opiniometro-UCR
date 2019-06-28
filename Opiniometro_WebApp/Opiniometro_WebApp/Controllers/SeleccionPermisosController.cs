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
        public void agregarTupla(string perf, short perm, List<Posee_Enfasis_Perfil_Permiso> listaP, List<Enfasis> listaE)
        {
            using (var context = new Opiniometro_DatosEntities())
            {
                //Hay que hacer borrado para todos los enfasis
                foreach (var item in listaE)
                {
                    if (listaP.Any(tupla => tupla.NombrePerfil == perf && tupla.IdPermiso == perm && tupla.SiglaCarrera == item.SiglaCarrera && tupla.NumeroEnfasis == item.Numero))
                    {
                        System.Diagnostics.Debug.Print("Existe para agregar");
                    }
                }
            }
        }

        // GET: SeleccionPermisos
        public ActionResult SeleccionarPermisos()
        {
            //Verificar si sesion esta activa
            if (IdentidadManager.verificar_sesion(this) == true)
            {
                //Verificar si es administrador
                if (IdentidadManager.obtener_perfil_actual() == "Administrador")
                {
                    bool asignado;
                    using (var context = new Opiniometro_DatosEntities())
                    {
                        SeleccionPermisos model = new SeleccionPermisos();
                        model.ListaPerfiles = context.Perfil.ToList();
                        model.ListaPermisos = context.Permiso.ToList();
                        model.ListaPosee = context.Posee_Enfasis_Perfil_Permiso.ToList();
                        model.ListaEnfasis = context.Enfasis.ToList();
                        model.ListaAsoc = new List<SeleccionPermisos.Asociaciones>();//Contiene la listaPosee pero sin tuplas que repitan enfasis
                        model.ListaGuardar = new List<SeleccionPermisos.GuardarPerm>();

                        foreach (var posee in model.ListaPosee)
                        {
                            SeleccionPermisos.Asociaciones asoc = new SeleccionPermisos.Asociaciones(posee.NombrePerfil, posee.IdPermiso);
                            //Para verificar si ya existe la combinacion de permiso en una tupla solamente de permiso (id) y perfil (id)
                            //para no insertarla en la lista ListaAsoc. Todo esto pues hay tuplas muy similares debido a que lo que difiere es el enfasis
                            if (!model.ListaAsoc.Any(item => item.Perfil == asoc.Perfil && item.Permiso == asoc.Permiso))
                            {
                                model.ListaAsoc.Add(asoc);
                            }
                        }

                        //Se llena la listaGuardar tanto con las relaciones existentes como con las posibles que se pueden crear a partir de un seleccion de checkbox
                        foreach (var perfil in model.ListaPerfiles)
                        {
                            foreach (var permiso in model.ListaPermisos)
                            {
                                asignado = false;//Para cada combinacion
                                for (int cont = 0; cont < model.ListaAsoc.Count; cont++)
                                {
                                    //Si existe la combinacion la agrega indicando que es existente
                                    if (perfil.Nombre == model.ListaAsoc[cont].Perfil && permiso.Id == model.ListaAsoc[cont].Permiso)
                                    {
                                        model.ListaGuardar.Add(new SeleccionPermisos.GuardarPerm(perfil.Nombre, permiso.Id, true));
                                        asignado = true;
                                    }
                                    //Si no existe
                                    if (cont == model.ListaAsoc.Count - 1 && asignado == false)
                                    {
                                        model.ListaGuardar.Add(new SeleccionPermisos.GuardarPerm(perfil.Nombre, permiso.Id, false));
                                        asignado = false;
                                    }
                                }
                            }
                        }
                        return View(model);
                    }
                }
                //No tiene permiso entonces se redirige a Home
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            //Si no entonces a re-autenticarse
            else
            {
                return RedirectToAction("Login", "Auth");
            }
        }

        [HttpPost]
        public ActionResult Guardar(SeleccionPermisos mod)
        {
            using (var context = new Opiniometro_DatosEntities())
            {
                SeleccionPermisos model = new SeleccionPermisos();
                model.ListaPerfiles = context.Perfil.ToList();
                model.ListaPermisos = context.Permiso.ToList();
                model.ListaEnfasis = context.Enfasis.ToList();
                model.ListaPosee = context.Posee_Enfasis_Perfil_Permiso.ToList();

                foreach (var item in mod.ListaGuardar)
                {
                    if (item.Existe)//Si quedo seleccionado la intenta agregar si ya existe o no
                    {
                        //Si esta checked hay que ver si esta ya en la base o no
                        //Para todos los enfasis se intenta insertar
                        foreach (var enf in model.ListaEnfasis)
                        {
                            //Verificar si NO existe en la tabla para insertarlo
                            if (!model.ListaPosee.Any(tupla => tupla.NombrePerfil == item.Perfil && tupla.IdPermiso == item.Permiso && tupla.SiglaCarrera == enf.SiglaCarrera && tupla.NumeroEnfasis == enf.Numero))
                            {
                                var nuevaTupla = new Posee_Enfasis_Perfil_Permiso()
                                {
                                    IdPermiso = item.Permiso,
                                    NombrePerfil = item.Perfil,
                                    SiglaCarrera = enf.SiglaCarrera,
                                    NumeroEnfasis = enf.Numero,
                                };

                                context.Posee_Enfasis_Perfil_Permiso.Add(nuevaTupla);
                            }
                        }
                    }
                    else//Si no fue seleccionado o fue deseleccionado intenta borrar la tupla si existe o no en la tabla
                    {
                        //Si esta unchecked se fija para ver si esta en la base para quitarla sino no hace nada pues no estaba previamente
                        //Hay que tratar de hacer borrado para todos los enfasis
                        foreach (var enf in model.ListaEnfasis)
                        {
                            var consulta = (from p in context.Posee_Enfasis_Perfil_Permiso
                                            where p.IdPermiso == item.Permiso
                                            && p.NombrePerfil == item.Perfil
                                            && p.NumeroEnfasis == enf.Numero
                                            && p.SiglaCarrera == enf.SiglaCarrera
                                            select p).FirstOrDefault();
                            //Si no esta vacio significa que existe y por ende hay que quitarla de la tabla
                            if (consulta != null)
                            {
                                context.Posee_Enfasis_Perfil_Permiso.Remove(consulta);
                            }
                        }
                    }
                }

                if (ModelState.IsValid)
                {
                    //Notificacion de que todo sale bien
                    context.SaveChanges();
                    TempData["msg"] = "<script>alert('Se han guardado los cambios a los permisos exitosamente.');</script>";
                    return RedirectToAction("SeleccionarPermisos");
                }

                return View(model);
            }
        }
    }
}