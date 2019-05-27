using System;
using System.Data.Entity.Core.Objects;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Opiniometro_WebApp.Models;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Security.Cryptography;
using System.Threading;

namespace Opiniometro_WebApp.Controllers
{
    public class AuthController : Controller
    {
        private Opiniometro_DatosEntities db = new Opiniometro_DatosEntities();
        private const string caracteres_aleatorios = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        /* GET: Auth/Login
         * EFECTO: retornar vista parcial, la cual implica que no se despliega _Layout de la carpeta Shared.
         * REQUIERE: cshtml con el nombre Login.
         * MODIFICA: n/a
         */
        public ActionResult Login()
        {
            return PartialView();
        }

        /*  
         *  EFECTO: verificar los datos brindados en la base de datos.
         *  REQUIERE: correo y contrasenna en "empaquetado" en la clase Usuario.
         *  MODIFICA: la identidad para que el usario este loggeado en el sistema.
         *  
         *  Basado en:
         *  SigIn: https://stackoverflow.com/questions/31584506/how-to-implement-custom-authentication-in-asp-net-mvc-5,
         *  Obtener la identidad desde otro sitio: https://stackoverflow.com/questions/22246538/access-claim-values-in-controller-in-mvc-5
         */
        [HttpPost]
        public ActionResult Login(Usuario usuario)
        {
            ObjectParameter exito = new ObjectParameter("Resultado", 0);
            db.SP_LoginUsuario(usuario.CorreoInstitucional, usuario.Contrasena, exito);

            // Si se pudo autenticar.
            if ((bool)exito.Value == true)
            {
                var identidad = new ClaimsIdentity(
                    new[] {
                    new Claim(ClaimTypes.Email, usuario.CorreoInstitucional)

                    /*
                    // optionally you could add roles if any
                    new Claim(ClaimTypes.Role, "RoleName"),
                    new Claim(ClaimTypes.Role, "AnotherRole"),
                    */
                    },
                    DefaultAuthenticationTypes.ApplicationCookie);
                Thread.CurrentPrincipal = new ClaimsPrincipal(identidad);

                HttpContext.GetOwinContext().Authentication.SignIn(new AuthenticationProperties { IsPersistent = false }, identidad);
                return RedirectToAction("Index", "LogInPerfiles");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "");
                // Devolverse a la misma pagina de Login informando de que hay un error de autenticacion.
                return PartialView(usuario);
            }

        }
        /*
         * GET: Auth/Recuperar
         * EFECTO: retornar vista parcial, la cual implica que no se despliega _Layout de la carpeta Shared.
         * REQUIERE: cshtml con el nombre Recuperar.
         * MODIFICA: n/a
         */
        public ActionResult Recuperar()
        {
            return PartialView();
        }

        /*
         * EFECTO: verifica si el correo existe en la base de datos, y si esta, envia un correo con una contrasena nueva.
         * REQUIERE: un correo.
         * MODIFICA: la contrasena del usuario, si es que calza con lo que hay en la base de datos.
         */
        [HttpPost]
        public ActionResult Recuperar(Usuario usuario)
        {
            ObjectParameter exito = new ObjectParameter("Resultado", 0);
            db.SP_ExistenciaCorreo(usuario.CorreoInstitucional, exito);

            if ((bool)exito.Value == true)
            {
                // Autogenero una contrasenna generica.
                string contrasenna_generada = GenerarContrasenna(10);

                // La guardo en la base de datos llamando al procedimiento almacenado.
                db.SP_CambiarContrasenna(usuario.CorreoInstitucional, contrasenna_generada);
                

                string contenido =
                    "<p>A continuación, su contraseña temporal, ingrésela junto con su correo institucional:</p> <b>"
                    + contrasenna_generada + "</b>";

                // Envio correo con la contrasenna autogenerada
                EnviarCorreo(usuario.CorreoInstitucional, "Cambio de contraseña - Opiniómetro@UCR", contenido);
            }
            ModelState.AddModelError(string.Empty, "");
            return PartialView(usuario);
        }

        /*
         * EFECTO: envia un correo con una contrasena aleatoria.
         * REQUIERE: el correo de la persona que va a recibir la contrasena, el asunto de la persona, y el contenido del mismo.
         * MODIFICA: n/a
         */
        private void EnviarCorreo(string correo_receptor, string asunto, string contenido)
        {
            string correo_emisor = System.Configuration.ConfigurationManager.AppSettings["CorreoEmisor"].ToString();
            string contrasenna = System.Configuration.ConfigurationManager.AppSettings["ContrasennaEmisor"].ToString();

            SmtpClient cliente = new SmtpClient("smtp.gmail.com", 587);
            cliente.EnableSsl = true;
            cliente.Timeout = 100000;
            cliente.DeliveryMethod = SmtpDeliveryMethod.Network;
            cliente.UseDefaultCredentials = false;
            cliente.Credentials = new NetworkCredential(correo_emisor, contrasenna);

            MailMessage correo = new MailMessage(correo_emisor, correo_receptor, asunto, contenido);
            correo.IsBodyHtml = true;
            correo.BodyEncoding = UTF8Encoding.UTF8;
            cliente.Send(correo);

        }

        /*
         * EFECTO: genera y retorna una contrasena aleatoria.
         * REQUIERE: el tamano de la contrasena.
         * MODIFICA: n/a
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