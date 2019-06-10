using System;
using System.Data.Entity.Core.Objects;
using System.Text;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Opiniometro_WebApp.Models;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Dynamic;
using System.ComponentModel;

namespace Opiniometro_WebApp.Controllers
{
    public class AdminController : Controller
    {
        private Opiniometro_DatosEntities db = new Opiniometro_DatosEntities();


        public ActionResult VerPersonas(string nom)
        {
            if (!String.IsNullOrEmpty(nom))
            {
                ViewModelAdmin model = new ViewModelAdmin();
                List<Persona> listaPersonas = db.Persona.ToList();
                List<Usuario> listaUsuarios = db.Usuario.ToList();
                var query = from p in listaPersonas
                            join u in listaUsuarios on p.Cedula equals u.Cedula into table1
                            from u in table1
                            where p.Nombre.Contains(nom)
                            select new ViewModelAdmin { persona = p, usuario = u };
                return View(query);
            }
            else
            {
                ViewModelAdmin model = new ViewModelAdmin();
                List<Persona> listaPersonas = db.Persona.ToList();
                List<Usuario> listaUsuarios = db.Usuario.ToList();
                var query = from p in listaPersonas
                            join u in listaUsuarios on p.Cedula equals u.Cedula into table1
                            from u in table1
                            select new ViewModelAdmin { persona = p, usuario = u };
                return View(query);
            }


        }

        
        public ActionResult Editar(string id)
        {
            try
            {
               
                    ViewModelAdmin model = new ViewModelAdmin();
                model.persona = db.Persona.SingleOrDefault(u => u.Cedula == id);
                model.usuario = db.Usuario.SingleOrDefault(u => u.Cedula == id);


                return View(model);
                
            }
            catch (Exception)
            {

                throw;
            }
            
            
        }

        [HttpPost]
        public ActionResult Editar(ViewModelAdmin per)
        {
            try
            {
                using (db)
                {
                    db.SP_ModificarPersona(per.persona.Cedula, per.persona.Cedula, per.persona.Nombre, per.persona.Apellido1, per.persona.Apellido2, per.usuario.CorreoInstitucional,per.persona.Direccion);
                    return RedirectToAction("VerPersonas");
                }
            }
            catch (Exception)
            {

                throw;
            }
           
        }
    

        public ActionResult Borrar(string id)
        {
           // db.SP_EliminarPersona(id);
            return RedirectToAction("VerPersonas");
        }


        public ActionResult CrearUsuario()
        {
            return View();

        }

        [HttpPost]
        public ActionResult CrearUsuario(ViewModelAdmin per)
        {
            try
            {
                using (db)
                {
                    db.SP_AgregarPersonaUsuario(per.persona.Cedula, per.persona.Nombre, per.persona.Apellido1, per.persona.Apellido2, per.usuario.CorreoInstitucional, per.persona.Direccion);
                    return RedirectToAction("VerPersonas");
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public ActionResult VerPorEnfasis()
        {

            return View();
        }


        /* public DataSet GetDataSet(string ConnectionString, string SQL)
         {

                         SqlDataAdapter da = new SqlDataAdapter();

                         cmd.CommandText = SQL;
                         da.SelectCommand = cmd;
                         DataSet ds = new DataSet();

                         conn.Open();
                         da.Fill(ds);
                         conn.Close();

                         return ds;
                     }
             DataTable table = new DataTable();
             using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["DB"].ConnectionString))
             using (var cmd = new SqlCommand("usp_GetABCD", con))
             using (var da = new SqlDataAdapter(cmd))
             {
                 cmd.CommandType = CommandType.StoredProcedure;
                 da.Fill(table);
             }*/

    }
    }