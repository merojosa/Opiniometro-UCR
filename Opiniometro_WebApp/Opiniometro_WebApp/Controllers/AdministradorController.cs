using System;
using System.CodeDom;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Drawing.Drawing2D;
using System.Dynamic;
using System.EnterpriseServices;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
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
                        //Tupla cumple preeliminarmente con requisitos de formato
                        filasValidas.Rows.Add(tupla);

                    }
                    catch (Exception e)
                    {
                        if (e is ArgumentException || e is InvalidCastException || e is ConstraintException ||
                            e is NoNullAllowedException)
                        {
                            AgregarTuplaInvalida(filaLeida, e, filasInvalidas, numeroFilasLeidas);
                        }

                    
                    }
                }
                //fin de chequeos de formato
            }
            //chequeos de contenido
            verificacionContenidoTuplasValidas(filasValidas, filasInvalidas, numeroFilasLeidas);
            filasValidas.AcceptChanges();
            filasInvalidas.AcceptChanges();
            return filasInvalidas;
        }


        private void verificacionContenidoTuplasValidas(DataTable filasValidas, DataTable filasInvalidas,long numeroFilasLeidas)
        {
            //Chequeos relacionados con el contenido proveido en el archivo csv

            //Fin de chequeos de contenido
            //Insercion en bloque
            DataTable personaBD = crearTablaPersonaBD();
            DataTable usuarioBD = crearTablaUsuarioBD();
            multicastDatosProvisionados(filasValidas, personaBD, usuarioBD);
            insercionEnBloque(personaBD);
        }

        

        private int multicastDatosProvisionados(DataTable filasValidas, DataTable personaBD, DataTable usuarioBD)
        {
            string hileraConexion = ConfigurationManager.ConnectionStrings["Opiniometro_DatosEntities"].ConnectionString;
            
            DataRow rd = filasValidas.NewRow();
            
            for(int indexFilasValidas = 0; indexFilasValidas < filasValidas.Rows.Count; ++indexFilasValidas)
            {
                Persona existePersona = null;
                Usuario existeUsuario = null;
                if ((existePersona = db.Persona.Find(filasValidas.Rows[indexFilasValidas]["cedula"])) == null)
                {
                    DataRow nuevaPersona = personaBD.NewRow();
                    nuevaPersona["cedula"] = filasValidas.Rows[indexFilasValidas]["cedula"];
                    nuevaPersona["nombre1"] = filasValidas.Rows[indexFilasValidas]["nombre1"];
                    nuevaPersona["apellido1"] = filasValidas.Rows[indexFilasValidas]["apellido1"];
                    nuevaPersona["apellido2"] = filasValidas.Rows[indexFilasValidas]["apellido2"];
                    nuevaPersona["direccion_exacta"] = filasValidas.Rows[indexFilasValidas]["direccion_exacta"];
                    personaBD.Rows.Add(nuevaPersona);
                    personaBD.AcceptChanges();
                }

                if ((existeUsuario = db.Usuario.Find(filasValidas.Rows[indexFilasValidas]["cedula"])) == null)
                {
                    DataRow nuevoUsuario = usuarioBD.NewRow();
                    
                    nuevoUsuario["correo"] = filasValidas.Rows[indexFilasValidas]["correo"];

                    //Insercion de un usuario requiere que tenga un contrasena cifrada con un guid
                    ObjectParameter contrasenaGenerada = new ObjectParameter("contrasenaGenerada", typeof(System.Data.SqlTypes.SqlChars));
                    db.SP_GenerarContrasena(contrasenaGenerada);
                    ObjectParameter guidGenerado = new ObjectParameter("guidGenerado", typeof(System.Data.SqlTypes.SqlGuid));
                    db.SP_GenerarIdUnico(guidGenerado);
                    System.Guid guid = new Guid(guidGenerado.Value.ToString()); //!HAY QUE REVISAR ESTA LINEA.
                    ObjectParameter contrasenaHash = new ObjectParameter("contrasenaHash", typeof(System.Data.SqlTypes.SqlChars));
                    db.SP_GenerarContrasenaHash(guid, contrasenaGenerada.ToString(), contrasenaHash);

                    nuevoUsuario["contrasena"] = contrasenaHash.ToString();
                    nuevoUsuario["activo"] = true;
                    nuevoUsuario["cedula"] = filasValidas.Rows[indexFilasValidas]["cedula"];
                    nuevoUsuario["id"] = guidGenerado.ToString();

                }

                
            }
            
            using (SqlConnection conexionBD = new SqlConnection(hileraConexion))
            {
                /*
                 * Orden de insercion
                 *
                 * Verificar en tabla persona si persona con cedula provisionada en archivo csv ya existe.
                 *      -> Persona no existe, hay que insertarlo en tabla Persona.
                 *      -> Persona sí existe con cedula provisionada
                 *
                 * Persona existe como usuario con correo electrónico provisionado?
                 *      -> Persona no existe como usuario con el correo provisionado, hay que insertarlo en Usuario con una contraseña generada.
                 *      -> Persona sí existe como usuario con el correo provisionado.
                 *
                 * Persona existe con perfil provisionado?
                 *      -> Si existe con perfil provisionado reportar error.
                 *      -> No existe con perfil provisionado, insertar con ese perfil.
                 *
                 */

            }

            return 1;

        }
        private void insercionEnBloque(DataTable tablaAInsertar)
        {
            string hileraConexion = ConfigurationManager.ConnectionStrings["Opiniometro_DatosEntities"].ConnectionString;
            using (SqlConnection conexionBD = new SqlConnection(hileraConexion))
            {
                using (SqlBulkCopy insercionEnBloque = new SqlBulkCopy(conexionBD))
                {
                    insercionEnBloque.DestinationTableName = tablaAInsertar.TableName;
                    mapearColumnasATablaDeDestino(tablaAInsertar.TableName, insercionEnBloque);
                }
                /*
                 * Orden de insercion
                 *
                 * Verificar en tabla persona si persona con cedula provisionada en archivo csv ya existe.
                 *      -> Persona no existe, hay que insertarlo en tabla Persona.
                 *      -> Persona sí existe con cedula provisionada
                 *
                 * Persona existe como usuario con correo electrónico provisionado?
                 *      -> Persona no existe como usuario con el correo provisionado, hay que insertarlo en Usuario con una contraseña generada.
                 *      -> Persona sí existe como usuario con el correo provisionado.
                 *
                 * Persona existe con perfil provisionado?
                 *      -> Si existe con perfil provisionado reportar error.
                 *      -> No existe con perfil provisionado, insertar con ese perfil.
                 *
                 */

            }
        }

        private void mapearColumnasATablaDeDestino(string tableName, SqlBulkCopy insercionEnBloque)
        {
            switch (tableName)
            {
                case "Persona":
                {
                    insercionEnBloque.ColumnMappings.Add("cedula", "Cedula");
                    insercionEnBloque.ColumnMappings.Add("nombre1", "Nombre");
                    insercionEnBloque.ColumnMappings.Add("apellido1", "Apellido1");
                    insercionEnBloque.ColumnMappings.Add("apellido2", "Apellido2");
                    insercionEnBloque.ColumnMappings.Add("direccion_exacta", "Direccion");
                }
                break;
            }
        }


        private void AgregarTuplaInvalida(string filaLeida, Exception exception, DataTable filasInvalidas, long numeroFilasLeidas)
        {
            throw new NotImplementedException();
        }

        private DataTable crearTablaUsuarios()
        {
            DataTable dt = new DataTable();
            dt.TableName = "usuarios_validos";
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
        
            // Estableciendo tamano maximo de las columnas de texto
            dt.Columns[0].MaxLength = 9;
            dt.Columns[0].AllowDBNull = false;

            dt.Columns[1].MaxLength = 10;
            dt.Columns[1].AllowDBNull = false;

            dt.Columns[2].MaxLength = 6;

            dt.Columns[3].MaxLength = 50;
            dt.Columns[3].AllowDBNull = false;

            dt.Columns[4].MaxLength = 50;

            dt.Columns[5].MaxLength = 50;
            dt.Columns[5].AllowDBNull = false;

            dt.Columns[6].MaxLength = 50;
            dt.Columns[6].AllowDBNull = false;

            dt.Columns[7].MaxLength = 50;
            dt.Columns[7].AllowDBNull = false;

            dt.Columns[8].MaxLength = 50;
            dt.Columns[8].AllowDBNull = false;

            dt.Columns[9].MaxLength = 50;
            dt.Columns[9].AllowDBNull = false;

            dt.Columns[10].MaxLength = 50;
            dt.Columns[10].AllowDBNull = false;

            dt.Columns[11].MaxLength = 200;
            dt.Columns[11].AllowDBNull = false;

            dt.Columns[12].MaxLength = 10;
            dt.Columns[13].MaxLength = 3; //Need to check if this applies correctly to tiny ints.
            return dt;
        }




        private DataTable crearTablaUsuariosInvalidos()
        {
            DataTable dt = new DataTable();
            dt.TableName = "usuarios_invalidos"
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

        private DataTable crearTablaPersonaBD()
        {
            DataTable dt = new DataTable();
            dt.TableName = "Persona";
            dt.Columns.Add("cedula", typeof(System.Data.SqlTypes.SqlChars));
            dt.Columns.Add("nombre1", typeof(System.Data.SqlTypes.SqlChars));           
            dt.Columns.Add("nombre2", typeof(System.Data.SqlTypes.SqlChars));           
            dt.Columns.Add("apellido1", typeof(System.Data.SqlTypes.SqlChars));         
            dt.Columns.Add("apellido2", typeof(System.Data.SqlTypes.SqlChars));
            dt.Columns.Add("direccion_exacta", typeof(System.Data.SqlTypes.SqlChars));

            dt.Columns[0].AllowDBNull = false;
            dt.Columns[1].AllowDBNull = false;
            dt.Columns[2].AllowDBNull = false;
            dt.Columns[3].AllowDBNull = false;
            dt.Columns[4].AllowDBNull = false;
            dt.Columns[5].AllowDBNull = false;
            return dt;
        }

        private DataTable crearTablaUsuarioBD()
        {
            DataTable dt = new DataTable();
            dt.TableName = "Usuario";
            dt.Columns.Add("correo", typeof(System.Data.SqlTypes.SqlChars));
            dt.Columns.Add("contrasena", typeof(System.Data.SqlTypes.SqlChars));
            dt.Columns.Add("activo", typeof(System.Data.SqlTypes.SqlBoolean));
            dt.Columns.Add("cedula", typeof(System.Data.SqlTypes.SqlChars));
            dt.Columns.Add("id", typeof(System.Data.SqlTypes.SqlGuid));
            return dt;
        }
    }


}