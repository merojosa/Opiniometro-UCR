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
using System.Security.Cryptography;
using System.Net.Mail;
using System.Net;

namespace Opiniometro_WebApp.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private Opiniometro_DatosEntities db = new Opiniometro_DatosEntities();
        private const string caracteres_aleatorios = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        

        public Persona Persona { get; private set; }

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
                Opiniometro_WebApp.Models.PersonaPerfilEnfasisModel modelPersona = new Opiniometro_WebApp.Models.PersonaPerfilEnfasisModel();
                modelPersona.Persona = db.Persona.Find(id);

                try
                {
                    String correoInstitucional = db.Usuario.Where(m => m.Cedula == id).First().CorreoInstitucional;
                    //modelPersona.Perfil = db.ObtenerPerfilUsuario(correoInstitucional).ToList();
                    modelPersona.Persona = db.Persona.SingleOrDefault(u => u.Cedula == id);
                    modelPersona.usuario = db.Usuario.SingleOrDefault(u => u.Cedula == id);
                    modelPersona.Perfil = db.Perfil.Select(n => n.Id).ToList();
                    return View(modelPersona);
                }
                catch (Exception)
                {
                    return View(modelPersona);
                }


            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public ActionResult Editar(PersonaPerfilEnfasisModel per)
        {
            try
            {
                using (db)
                {
                    db.SP_ModificarPersona(per.Persona.Cedula, per.Persona.Cedula, per.Persona.Nombre, per.Persona.Apellido1, per.Persona.Apellido2, per.usuario.CorreoInstitucional, per.Persona.Direccion);
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
                    string contrasenna_generada = GenerarContrasenna(10);
                    db.SP_AgregarPersonaUsuario(per.usuario.CorreoInstitucional, contrasenna_generada, per.persona.Cedula, per.persona.Nombre, per.persona.Apellido1, per.persona.Apellido2, per.persona.Direccion);             

                    string contenido =
                     "<p>Se le ha creado un usuario en Opiniometro@UCR.</p>" +
                     "<p>A continuación, su contraseña temporal, ingrésela junto con su correo institucional:</p> <b>"
                     + contrasenna_generada + "</b>";

                    // Envio correo con la contrasenna autogenerada
                    EnviarCorreo(per.usuario.CorreoInstitucional, "Usuario creado - Opiniómetro@UCR", contenido);
                    return RedirectToAction("VerPersonas");
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        private void EnviarCorreo(string receptor, string asunto, string contenido)
        {
            string autor = System.Configuration.ConfigurationManager.AppSettings["CorreoEmisor"].ToString();
            string contrasenna = System.Configuration.ConfigurationManager.AppSettings["ContrasennaEmisor"].ToString();

            SmtpClient cliente = new SmtpClient("smtp.gmail.com", 587);
            cliente.EnableSsl = true;
            cliente.Timeout = 100000;
            cliente.DeliveryMethod = SmtpDeliveryMethod.Network;
            cliente.UseDefaultCredentials = false;
            cliente.Credentials = new NetworkCredential(autor, contrasenna);

            MailMessage correo = new MailMessage(autor, receptor, asunto, contenido);
            correo.IsBodyHtml = true;
            correo.BodyEncoding = UTF8Encoding.UTF8;
            cliente.Send(correo);

        }

        /*
         * EFECTO:
         * REQUIERE:
         * MODIFICA:
         * 
         * Basado en: https://stackoverflow.com/questions/1344221/how-can-i-generate-random-alphanumeric-strings
         */
        private string GenerarContrasenna(int tamanno)
        {
            byte[] datos = new byte[tamanno];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetBytes(datos);
            }

            StringBuilder contrasenna = new StringBuilder(tamanno);

            foreach (byte byte_aleatorio in datos)
            {
                contrasenna.Append(caracteres_aleatorios[byte_aleatorio % (caracteres_aleatorios.Length)]);
            }
            return contrasenna.ToString();
        }


    }
    }