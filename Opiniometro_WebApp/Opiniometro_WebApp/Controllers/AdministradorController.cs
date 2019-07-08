using System;
using System.CodeDom;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing.Drawing2D;
using System.EnterpriseServices;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using Opiniometro_WebApp.Models;
using System.Security.Cryptography;
using System.Net.Mail;
using System.Text;

//using Microsoft.SqlServer.Dts;
//using Microsoft.SqlServer.Dts.Runtime;


namespace Opiniometro_WebApp.Controllers
{
    public class AdministradorController : Controller
    {
        private Opiniometro_DatosEntities db = new Opiniometro_DatosEntities();
        private const string caracteres_aleatorios = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        // GET: Administrador
        public ActionResult Index()
        {

            return View();
        }




        [HttpGet]
        public ActionResult CargarArchivo()
        {
            return View();
        }



        [HttpPost]
        public ActionResult CargarArchivo(HttpPostedFileBase postedFile)
        {
            if (postedFile != null)
            {
                string path = Server.MapPath("~/App_Data/ArchivosCargados/");
                if (!Directory.Exists(path))
                {
                    try
                    {
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        postedFile.SaveAs(path + Path.GetFileName(postedFile.FileName));
                        ViewBag.Message = "Archivo cargado con exito.";
                    }
                    catch (Exception e)
                    {

                        ViewBag.Message = "Error al cargar el archivo. Intente de nuevo mas tarde.";
                        Console.WriteLine(e);
                        throw;
                    }

                }

                postedFile.SaveAs(path + Path.GetFileName(postedFile.FileName));
                ViewBag.Message = "File uploaded successfully.";
            }

            return View();
        }



        private DataTable ProcesarArchivo(string path)
        {
            DataTable filasValidas = crearTablaUsuarios();
            DataTable filasInvalidas = crearTablaUsuariosInvalidos();
            string filaLeida = String.Empty;
            DataRow tupla;
            long numeroFilasLeidas = 0;
            using (StreamReader streamCsv = new StreamReader(path))
            {
                while ((filaLeida = streamCsv.ReadLine()) != null)
                {
                    ++numeroFilasLeidas;
                    tupla = filasValidas.NewRow();

                    try
                    {
                        tupla.ItemArray = filaLeida.Split(',');
                    }
                    catch (Exception e)
                    {
                        if (e is ArgumentException || e is InvalidCastException || e is ConstraintException ||
                            e is NoNullAllowedException)
                        {
                            AgregarTuplaInvalida(filaLeida, e, filasInvalidas, numeroFilasLeidas);
                        }
                        else
                        {
                            AgregarTuplaValida(filaLeida, filasValidas, numeroFilasLeidas);
                        }


                    }
                }
            }

            return filasInvalidas;
        }


        private void AgregarTuplaValida(string filaLeida, DataTable filasValidas, long numeroFilasLeidas)
        {
            DataRow tupla;
            tupla = filasValidas.NewRow();
            tupla.ItemArray = filaLeida.Split(',');
            int Tamano = tupla.ItemArray.Count();
            filasValidas.Rows.Add(tupla);
        }


        private void AgregarTuplaInvalida(string filaLeida, Exception exception, DataTable filasInvalidas, long numeroFilasLeidas)
        {
            throw new NotImplementedException();
        }




        private DataTable crearTablaUsuarios()
        {
            DataTable dt = new DataTable();

            // Ver documentacion sobre los tipos en https://docs.microsoft.com/en-us/dotnet/api/system.data.sqltypes?view=netframework-4.6.1
            //dt.Columns.Add("cedula", System.Type.GetType("System.Data.SqlTypes.SqlChars"));
            dt.Columns.Add("cedula", typeof(System.Data.SqlTypes.SqlChars));            //0
            dt.Columns.Add("perfil", typeof(System.Data.SqlTypes.SqlChars));            //1
            dt.Columns.Add("carne", typeof(System.Data.SqlTypes.SqlChars));             //2
            dt.Columns.Add("nombre1", typeof(System.Data.SqlTypes.SqlChars));           //3
            dt.Columns.Add("nombre2", typeof(System.Data.SqlTypes.SqlChars));           //4
            dt.Columns.Add("apellido1", typeof(System.Data.SqlTypes.SqlChars));         //5
            dt.Columns.Add("apellido2", typeof(System.Data.SqlTypes.SqlChars));         //6
            dt.Columns.Add("correo", typeof(System.Data.SqlTypes.SqlChars));            //7
            dt.Columns.Add("provincia", typeof(System.Data.SqlTypes.SqlChars));         //8
            dt.Columns.Add("canton", typeof(System.Data.SqlTypes.SqlChars));            //9
            dt.Columns.Add("distrito", typeof(System.Data.SqlTypes.SqlChars));          //10
            dt.Columns.Add("direccion_exacta", typeof(System.Data.SqlTypes.SqlChars));  //11
            dt.Columns.Add("sigla_carrera", typeof(System.Data.SqlTypes.SqlChars));     //12
            dt.Columns.Add("enfasis", typeof(System.Data.SqlTypes.SqlByte));            //13


            // Estableciendo constraint NOT NULL para cada columna de la tabla de datos.
            foreach (DataColumn column in dt.Columns)
            {
                column.AllowDBNull = false;
            }

            // Estableciendo tamano maximo de las columnas de texto
            dt.Columns[0].MaxLength = 9;
            dt.Columns[1].MaxLength = 10;
            dt.Columns[2].MaxLength = 6;
            dt.Columns[3].MaxLength = 50;
            dt.Columns[4].MaxLength = 50;
            dt.Columns[5].MaxLength = 50;
            dt.Columns[6].MaxLength = 50;
            dt.Columns[7].MaxLength = 50;
            dt.Columns[8].MaxLength = 50;
            dt.Columns[9].MaxLength = 50;
            dt.Columns[10].MaxLength = 50;
            dt.Columns[11].MaxLength = 200;
            dt.Columns[12].MaxLength = 10;
            dt.Columns[13].MaxLength = 3; //Need to check if this applies correctly to tiny ints.
            return dt;
        }




        private DataTable crearTablaUsuariosInvalidos()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Error", typeof(string));
            dt.Columns.Add("cedula", typeof(string));
            dt.Columns.Add("perfil", typeof(string));
            dt.Columns.Add("carne", typeof(string));
            dt.Columns.Add("nombre1", typeof(string));
            dt.Columns.Add("nombre2", typeof(string));
            dt.Columns.Add("apellido1", typeof(string));
            dt.Columns.Add("apellido2", typeof(string));
            dt.Columns.Add("correo", typeof(string));
            dt.Columns.Add("provincia", typeof(string));
            dt.Columns.Add("canton", typeof(string));
            dt.Columns.Add("distrito", typeof(string));
            dt.Columns.Add("direccion_exacta", typeof(string));
            dt.Columns.Add("sigla_carrera", typeof(string));
            dt.Columns.Add("enfasis", typeof(string));

            return dt;
        }


        public Persona Persona { get; private set; }

        public ActionResult VerPersonas(string nom, string ced)
        {
            if (!String.IsNullOrEmpty(nom) && !String.IsNullOrEmpty(ced))
            {
                ViewModelAdmin model = new ViewModelAdmin();
                List<Persona> listaPersonas = db.Persona.ToList();
                List<Usuario> listaUsuarios = db.Usuario.ToList();
                var query = from p in listaPersonas
                            join u in listaUsuarios on p.Cedula equals u.Cedula into table1
                            from u in table1
                            where p.Nombre1.Contains(nom)
                            where p.Cedula.Contains(ced)
                            select new ViewModelAdmin { Persona = p, Usuario = u };
                return View(query);
            }
            else if (!String.IsNullOrEmpty(ced))
            {
                ViewModelAdmin model = new ViewModelAdmin();
                List<Persona> listaPersonas = db.Persona.ToList();
                List<Usuario> listaUsuarios = db.Usuario.ToList();
                var query = from p in listaPersonas
                            join u in listaUsuarios on p.Cedula equals u.Cedula into table1
                            from u in table1
                            where p.Cedula.Contains(ced)
                            select new ViewModelAdmin { Persona = p, Usuario = u };
                return View(query);
            }
            else if (!String.IsNullOrEmpty(nom))
            {
                ViewModelAdmin model = new ViewModelAdmin();
                List<Persona> listaPersonas = db.Persona.ToList();
                List<Usuario> listaUsuarios = db.Usuario.ToList();
                var query = from p in listaPersonas
                            join u in listaUsuarios on p.Cedula equals u.Cedula into table1
                            from u in table1
                            where p.Nombre1.Contains(nom)
                            select new ViewModelAdmin { Persona = p, Usuario = u };
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
                            select new ViewModelAdmin { Persona = p, Usuario = u };
                return View(query);
            }


        }


        public ActionResult Editar(string id)
        {

            Opiniometro_WebApp.Models.PersonaPerfilEnfasisModel modelPersona = new Opiniometro_WebApp.Models.PersonaPerfilEnfasisModel();
            modelPersona.Persona = db.Persona.Find(id);

            String correoInstitucional = db.Usuario.Where(m => m.Cedula == id).First().CorreoInstitucional;
            modelPersona.Persona = db.Persona.SingleOrDefault(u => u.Cedula == id);
            modelPersona.Usuario = db.Usuario.SingleOrDefault(u => u.Cedula == id);
            modelPersona.viejaCedula = id;
            modelPersona.PerfilDeUsuario = db.ObtenerPerfilUsuario(correoInstitucional).ToList();
            modelPersona.Perfil = db.Perfil.Select(n => n.Nombre).ToList();
            modelPersona.perfilesAsignados = modelPersona.getAsignarPerfil(modelPersona.PerfilDeUsuario, modelPersona.Perfil);
            modelPersona.tienePerfil = new List<Boolean>();
            for (int contador = 0; contador < modelPersona.perfilesAsignados.Count; contador++)
            {
                modelPersona.tienePerfil.Add(modelPersona.perfilesAsignados.ElementAt(contador).asignar);
            }

            return View(modelPersona);



        }

        [HttpPost]
        public ActionResult Editar(PersonaPerfilEnfasisModel per)
        {
            try
            {
                if ((per.Persona.Cedula != null) && (per.Persona.Cedula != null) && (per.Persona.Nombre1 != null) && (per.Persona.Apellido1 != null) && (per.Persona.Apellido2 != null)
                    && (per.Usuario.CorreoInstitucional != null)
                    && (per.Persona.Cedula.Length == 9) && (per.Persona.Nombre1.Length <= 50) && (per.Persona.Apellido1.Length <= 50) && (per.Persona.Apellido2.Length <= 50)
                    && (per.Usuario.CorreoInstitucional.Length <= 100))
                {
                    using (db)
                    {
                        db.SP_ModificarPersona(per.Persona.Cedula, per.Persona.Cedula, per.Persona.Nombre1, "", per.Persona.Apellido1, per.Persona.Apellido2, per.Usuario.CorreoInstitucional);
                    }
                }
                else
                {
                    //Mensaje de error
                }
                return RedirectToAction("VerPersonas");
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
                if ((per.Persona.Cedula != null) && (per.Persona.Cedula != null) && (per.Persona.Nombre1 != null) && (per.Persona.Apellido1 != null) && (per.Persona.Apellido2 != null)
                    && (per.Usuario.CorreoInstitucional != null)
                    && (per.Persona.Cedula.Length == 9) && (per.Persona.Nombre1.Length <= 50) && (per.Persona.Apellido1.Length <= 50) && (per.Persona.Apellido2.Length <= 50)
                    && (per.Usuario.CorreoInstitucional.Length <= 100))
                {
                    using (db)
                    {
                        string contrasenna_generada = GenerarContrasenna(10);
                        db.SP_AgregarPersonaUsuario(per.Usuario.CorreoInstitucional, contrasenna_generada, per.Persona.Cedula, per.Persona.Nombre1, "", per.Persona.Apellido1, per.Persona.Apellido2);

                        string contenido =
                         "<p>Se le ha creado un usuario en Opiniometro@UCR.</p>" +
                         "<p>A continuación, su contraseña temporal, ingrésela junto con su correo institucional:</p> <b>"
                         + contrasenna_generada + "</b>";

                        // Envio correo con la contrasenna autogenerada
                        EnviarCorreo(per.Usuario.CorreoInstitucional, "Usuario creado - Opiniómetro@UCR", contenido);
                    }
                    return RedirectToAction("VerPersonas");
                }
                else
                {
                    //Mensaje de error
                }
            }
            catch (Exception)
            {

                throw;
            }

            return null;
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