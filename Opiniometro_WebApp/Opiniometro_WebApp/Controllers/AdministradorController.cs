 using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using Opiniometro_WebApp.Models;
using Microsoft.SqlServer.Dts;
using Microsoft.SqlServer.Dts.Runtime;


namespace Opiniometro_WebApp.Controllers
{
    public class AdministradorController : Controller
    {
        private Opiniometro_DatosEntities db = new Opiniometro_DatosEntities();

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
            if (postedFile != null && postedFile.ContentLength > 0)
            {
                if (postedFile.FileName.EndsWith(".csv"))
                {
                    try
                    {
                        string path = Server.MapPath("~/App_Data/ArchivosCargados/");
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
                else
                {
                    ViewBag.Message = "Error, este formato de archivo no es compatible";
                }
                
            }


            //Codigo para procesamiento de archivo por ssis

            //Invocar paquete de intregracion, pasar como parametro el nombre del archivo cargado.

            //Application app = new Application()
            

            return View();
        }

        private DataTable ProcesarArchivo(string path)
        {
            DataTable filasValidas = crearTablaUsuarios();
            DataTable filasInvalidas = crearTablaUsuarios();
            string fila = String.Empty;



            using (StreamReader streamCsv = new StreamReader(path))
            {
                
            }

            return filasInvalidas;
        }

        private DataTable crearTablaUsuarios()
        {
            DataTable dt = new DataTable();

            //dt.Columns.Add("cedula", System.Type.GetType("System.Data.SqlTypes.SqlChars"));
            dt.Columns.Add("cedula", typeof(System.Data.SqlTypes.SqlChars));
            dt.Columns.Add("perfil", typeof(System.Data.SqlTypes.SqlChars));
            dt.Columns.Add("carne", typeof(SystemException));
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

    }

    
}
