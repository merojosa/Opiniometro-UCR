using Opiniometro_WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Opiniometro_WebApp.Controllers.Servicios;
using System.Security.Claims;
using System.Threading;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Net;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Data.Entity;
using System.Data;

namespace Opiniometro_WebApp.Controllers
{
    [Authorize]
    public class PerfilController : Controller
    {
        private Opiniometro_DatosEntities db = new Opiniometro_DatosEntities();

        public ActionResult Detalles(String nombre)
        {
            return View(db.Perfil.Find(nombre));
        }

        public ActionResult Editar(String nombre)
        {
            try
            {
                EditarPerfil perfil = new EditarPerfil();
                Perfil infoPerfil = db.Perfil.Find(nombre);
                perfil.Nombre = infoPerfil.Nombre;
                perfil.NombreViejo = infoPerfil.Nombre;
                perfil.Descripcion = infoPerfil.Descripcion;
                return View(perfil);
            }
            catch (Exception)
            {
                TempData["msg"] = "<script>alert('No se pudo acceder al perfil');</script>";
                return RedirectToAction("VerPerfiles", "Perfil");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar([Bind(Include = "Nombre,Descripcion,NombreViejo")]EditarPerfil perfil)
        {
            try
            {
                if (perfil.Nombre == perfil.NombreViejo || !db.Perfil.Any(model => model.Nombre == perfil.Nombre))
                {
                    if (perfil.Nombre != "Administrador")
                    {

                        if (ModelState.IsValid)
                        {

                            // Parametros
                            var Nombre = new SqlParameter("@nombre", perfil.Nombre);
                            var NombreViejo = new SqlParameter("@nombreViejo", perfil.NombreViejo);
                            var Descripcion = new SqlParameter("@descripcion", perfil.Descripcion);
                            var Numero_Error = new SqlParameter("@Numero_Error", 0);
                            Numero_Error.Direction = ParameterDirection.Output;
                            Numero_Error.SqlDbType = SqlDbType.Int;

                            db.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction,
                                "EXEC EditarPerfil @nombre, @nombreViejo, @descripcion, @Numero_Error OUT", Nombre, NombreViejo, Descripcion, Numero_Error);

                            if ((int)Numero_Error.Value == 0)
                            {
                                TempData["msg"] = "<script> $(document).ready(function(){ alert('El perfil se ha editado exitosamente.');}); </script>";
                                return RedirectToAction("VerPerfiles");
                            }
                            else
                            {
                                ModelState.AddModelError("ErrorEditarPerfil", "Error al editar perfil");
                            }

                        }

                        TempData["msg"] = "<script>alert('Se editó el perfil');</script>";
                        return RedirectToAction("VerPerfiles", "Perfil");
                    }
                    else
                    {
                        return RedirectToAction("VerPerfiles", "Perfil");
                    }
                }
                else
                {
                    TempData["msg"] = "<script>alert('No se pudo editar el perfil, no se puede usar ese nombre de perfil  ');</script>";
                    return RedirectToAction("Editar", "Perfil", new { nombre = perfil.NombreViejo });
                }
            }
            catch (Exception)
            {
                TempData["msg"] = "<script>alert('Ocurrió un error y no se pudo editar el perfil');</script>";
                return RedirectToAction("Editar", "Perfil", new { nombre = perfil.NombreViejo });
            }
        }



        public ActionResult VerPerfiles(String nom)
        {
            if (!String.IsNullOrEmpty(nom))
            {
                return View(db.Perfil.Where(m=>m.Nombre.Contains(nom)).ToList());
            }
            else {
                return View(db.Perfil.ToList());
            }   
        }

        public ActionResult Index()
        {
            PerfilesUsuario model = new PerfilesUsuario();
            model.ListaPerfiles = ObtenerPerfiles();

            // Si el usuario recien se loggea
            if(IdentidadManager.obtener_perfil_actual() == null)
            {
                // Se escoge un perfil por defecto en caso de que le de cancelar o pase de pagina (no elige perfil).
                cambiar_perfil(model.ListaPerfiles.ElementAt(0));
            }
            // Si no es la primera vez, no se cambia el perfil porque ya hay uno elegido.

            return View(model);
        }

        public ActionResult Cambiar()
        {
            PerfilesUsuario model = new PerfilesUsuario();
            model.ListaPerfiles = ObtenerPerfiles();

            // Si el usuario recien se loggea
            if (IdentidadManager.obtener_perfil_actual() == null)
            {
                // Se escoge un perfil por defecto en caso de que le de cancelar o pase de pagina (no elige perfil).
                cambiar_perfil(model.ListaPerfiles.ElementAt(0));
            }
            // Si no es la primera vez, no se cambia el perfil porque ya hay uno elegido.

            return View("Cambiar", model);
        }

        [HttpPost]
        public ActionResult Cambiar(PerfilesUsuario model)
        {
            cambiar_perfil(model.perfilSeleccionado);
            return RedirectToAction("Index", "Home");
        }

        // Por cuestiones de seguirdad, TIENE que ser privado.
        private void cambiar_perfil(string perfil_elegido)
        {
            // Si es un distinto perfil, cambie los permisos.
            if (IdentidadManager.obtener_perfil_actual() != perfil_elegido)
            {
                string correo_actual = IdentidadManager.obtener_correo_actual();

                var identidad = new ClaimsIdentity
                    (
                        new[] 
                        {
                            new Claim(ClaimTypes.Email, correo_actual),
                            new Claim(ClaimTypes.Role, perfil_elegido)
                        },
                        DefaultAuthenticationTypes.ApplicationCookie
                    );

                AuthController.eliminar_privilegios(this);

                Thread.CurrentPrincipal = new ClaimsPrincipal(identidad);
                HttpContext.GetOwinContext().Authentication.SignIn(new AuthenticationProperties { IsPersistent = false }, identidad);
                Session[correo_actual] = new IdentidadManager();
            }
        }

        // Devuelve los perfiles del usuario loggeado.
        public static ICollection<string> ObtenerPerfiles()
        {
            Opiniometro_DatosEntities db = new Opiniometro_DatosEntities();
            string correo_autenticado = IdentidadManager.obtener_correo_actual();
            ICollection<String> perfiles;
            perfiles = db.ObtenerPerfilUsuario(correo_autenticado).ToList();
            return perfiles;
        }

        [HttpPost]
        public ActionResult ObtenerPerfilBorrar()
        {
            if (Request.Form["NombrePerfil"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string nombre_perfil = Request.Form["NombrePerfil"].ToString();


            return RedirectToAction("ConfirmarBorrado", new { id = nombre_perfil });
        }

        public ActionResult ConfirmarBorrado(string id)
        {
            Perfil perfil = db.Perfil.Find(id);
            if (perfil == null)
            {
                return HttpNotFound();
            }
            return View(perfil);
        }

        // POST: CRUDPERFILES/Delete/5
        [HttpPost, ActionName("ConfirmarBorrado")]
        [ValidateAntiForgeryToken]
        public ActionResult BorrarConfirmado(string id)
        {
            // No se puede eliminar el perfil administrador.
            if(id != "Administrador")
            {
                Perfil perfil = db.Perfil.Find(id);
                db.Perfil.Remove(perfil);
                db.SaveChanges();
                
                // Desplegar alert hasta que se cargue la pagina por completo.
                TempData["msg"] = "<script> $(document).ready(function(){ alert('El perfil se ha borrado exitosamente.');}); </script>";
                return RedirectToAction("Borrar");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotAcceptable);
            }
        }

        public ActionResult Borrar()
        {
            return View("Borrar", db.Perfil.ToList());
        }

        // GET: CRUDPERFILES/Create
        public ActionResult Crear()
        {
            return View("Crear");
        }

        // POST: CRUDPERFILES/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear([Bind(Include = "Nombre,Descripcion")] Perfil perfil)
        {
            if (ModelState.IsValid)
            {
                string nombre = System.Text.RegularExpressions.Regex.Replace(perfil.Nombre.Trim(), @"\s+", " ");
                string descripcion = System.Text.RegularExpressions.Regex.Replace(perfil.Descripcion.Trim(), @"\s+", " ");

                // Parametros
                var Nombre = new SqlParameter("@Nombre", nombre);
                var Descripcion = new SqlParameter("@Descripcion", descripcion);
                var Numero_Error = new SqlParameter("@Numero_Error", 0);
                Numero_Error.Direction = ParameterDirection.Output;
                Numero_Error.SqlDbType = SqlDbType.Int;

                db.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction,
                    "EXEC SP_CrearPerfil @Nombre, @Descripcion, @Numero_Error OUT", Nombre, Descripcion, Numero_Error);

                if ((int)Numero_Error.Value == 0)
                {
                    TempData["msg"] = "<script> $(document).ready(function(){ alert('El perfil se ha creado exitosamente.');}); </script>";
                    return RedirectToAction("VerPerfiles");
                }
                else
                {
                    ModelState.AddModelError("ErrorCrearPerfil", "Error al crear perfil");
                }
            }

            return View(perfil);
        }

        /*
         *  REQUIERE: el nombre del perfil.
         *  EFECTUA: busca si el nombre brindado existe como llave primaria en la base de datos.
         *  MODIFICA: n/a
         */
        public JsonResult IsNombrePerfilAvailable(string Nombre)
        {
            return Json(!db.Perfil.Any(perfil => perfil.Nombre == Nombre), JsonRequestBehavior.AllowGet);
        }
    }
}