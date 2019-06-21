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
//using Microsoft.SqlServer.Dts;
//using Microsoft.SqlServer.Dts.Runtime;


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
    }


}