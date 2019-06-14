using System;
using System.CodeDom;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Drawing.Drawing2D;
using System.Dynamic;
using System.EnterpriseServices;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Security.Permissions;
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


        /*
         * EFECTO:
         * REQUIERE:
         * MODIFICA:
         */
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
                DataTable filasInvalidas = ProcesarArchivo(path + Path.GetFileName(postedFile.FileName));

                
            }

            return View();
        }


        /*
         * EFECTO: Lee y procesa archivo csv subido por el usuario.
         * REQUIERE: Un archivo subido por el usuario.
         * MODIFICA: 
         */
        private DataTable ProcesarArchivo(string path)
        {
            DataTable filasValidas = CrearTablaUsuarios();
            DataTable filasInvalidas = CrearTablaUsuariosInvalidos();
            string filaLeida = String.Empty;
            DataRow tupla;
            int numeroFilasLeidas = 0;
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

            //verificacionContenidoTuplasValidas(filasValidas, filasInvalidas, numeroFilasLeidas);
            filasValidas.AcceptChanges();

            //Tablas en memoria con que poseen el mismo esquema que las tablas en la base de datos.
            DataTable personaBD = CrearTablaPersonaBD();
            DataTable usuarioBD = CrearTablaUsuarioBD();
            
            //Multicast
            MulticastDatosProvisionados(filasValidas, personaBD, usuarioBD);
            filasValidas.Dispose();

            if (personaBD.Rows.Count > 0)
            {
                InsercionEnBloque(personaBD);
            }
            else
            {
                ViewBag.InsercionPersona = "No hubo inserciones en la tabla Persona";
            }

            if (usuarioBD.Rows.Count > 0)
            {
                InsercionEnBloque(usuarioBD);
            }
            else
            {
                ViewBag.InsercionPersona = "No hubo inserciones en la tabla Usuario";
            }

            personaBD.Dispose();
            usuarioBD.Dispose();
            filasInvalidas.AcceptChanges();
            return filasInvalidas;
        }

        /*
         * EFECTO: Revisa el contenido de los datos provisionados en el archivo csv.
         * REQUIERE:
         * MODIFICA:
         */
        private void VerificacionContenidoTuplasValidas(DataTable filasValidas, DataTable filasInvalidas,long numeroFilasLeidas)
        {
            throw new NotImplementedException();
            //Chequeos relacionados con el contenido proveido en el archivo csv

            //Fin de chequeos de contenido

        }

        /*
         * EFECTO: Clasifica y dispersa los datos de la tabla en memoria filasValidas. 
         * REQUIERE: Tabla en memoria que contiene los datos provisionados en el archivo csv,
         * MODIFICA: n/a
         */
        private int MulticastDatosProvisionados(DataTable filasValidas, DataTable personaBD, DataTable usuarioBD)
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
            for (int indexFilasValidas = 0; indexFilasValidas < filasValidas.Rows.Count; ++indexFilasValidas)
            {
                Persona existePersona = null;
                Usuario existeUsuario = null;
                if ((existePersona = db.Persona.Find(filasValidas.Rows[indexFilasValidas]["cedula"])) == null)
                {
                    
                    DataRow nuevaPersona = personaBD.NewRow();
                    nuevaPersona["cedula"] = new SqlChars(filasValidas.Rows[indexFilasValidas]["cedula"].ToString().ToCharArray());
                    nuevaPersona["nombre1"] = new SqlChars(filasValidas.Rows[indexFilasValidas]["nombre1"].ToString().ToCharArray());
                    nuevaPersona["apellido1"] = new SqlChars(filasValidas.Rows[indexFilasValidas]["apellido1"].ToString().ToCharArray());
                    nuevaPersona["apellido2"] = new SqlChars(filasValidas.Rows[indexFilasValidas]["apellido2"].ToString().ToCharArray());
                    nuevaPersona["direccion_exacta"] = new SqlChars(filasValidas.Rows[indexFilasValidas]["direccion_exacta"].ToString().ToCharArray());
                    personaBD.Rows.Add(nuevaPersona);
                    personaBD.AcceptChanges();
                }

                if ((existeUsuario = db.Usuario.Find(filasValidas.Rows[indexFilasValidas]["cedula"])) == null)
                {
                    DataRow nuevoUsuario = usuarioBD.NewRow();
                    
                    nuevoUsuario["correo"] = new SqlChars(filasValidas.Rows[indexFilasValidas]["correo"].ToString().ToCharArray());

                    //Insercion de un usuario requiere que tenga un contrasena cifrada con un guid
                    ObjectParameter contrasenaGenerada = new ObjectParameter("resultado", typeof(String));
                    db.SP_GenerarContrasena(contrasenaGenerada);
                    ObjectParameter guidGenerado = new ObjectParameter("id", typeof(SqlGuid));
                    db.SP_GenerarIdUnico(guidGenerado);
                   
                    Guid guid = new Guid(guidGenerado.Value.ToString()); //!HAY QUE REVISAR ESTA LINEA.
                    ObjectParameter contrasenaHash = new ObjectParameter("contrasenaHash", typeof(SqlBinary));
                    db.SP_GenerarContrasenaHash(guid.ToString(), (string)contrasenaGenerada.Value, contrasenaHash);

                    nuevoUsuario["contrasena"] = new SqlChars(contrasenaHash.ToString().ToCharArray());
                    nuevoUsuario["activo"] = new SqlBoolean(true);
                    nuevoUsuario["cedula"] = new SqlChars(filasValidas.Rows[indexFilasValidas]["cedula"].ToString().ToCharArray());
                    nuevoUsuario["id"] = new SqlGuid(guidGenerado.Value.ToString());
                    usuarioBD.Rows.Add(nuevoUsuario);
                    usuarioBD.AcceptChanges();

                    
                }

                
            }
            
            personaBD.AcceptChanges();
            usuarioBD.AcceptChanges();
            return 1;

        }

        /*
         * EFECTO: Realiza una insercion en bloque en una tabla de base de datos.
         * REQUIERE: Conexion instanciada y abierta hacia la base de datos. Una tabla en memoria que previamente haya sido llenada con los nuevos valores a guardar en la base de datos.
         * MODIFICA: Agrega tuplas en una tabla en la base de datos. 
         */
        private void InsercionEnBloque(DataTable tablaAInsertar)
        {
            //string hileraConexion = ConfigurationManager.ConnectionStrings["Opiniometro_DatosEntities"].ConnectionString;
            string hileraConexion =
                "data source=(localdb)\\ProjectsV13;initial catalog=Opiniometro_Datos;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework&quot;";
            using (SqlConnection conexionBD = new SqlConnection(hileraConexion))
            {
                
                conexionBD.Open();
                using (SqlBulkCopy insercionEnBloque = new SqlBulkCopy(conexionBD))
                {
                    insercionEnBloque.DestinationTableName = tablaAInsertar.TableName;
                    MapearColumnasATablaDeDestino(tablaAInsertar.TableName, insercionEnBloque);
                    insercionEnBloque.BatchSize = tablaAInsertar.Rows.Count;
                    try
                    {
                        insercionEnBloque.WriteToServer(tablaAInsertar);
                        PonerMensajeExito(tablaAInsertar.TableName);
                        
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                }
                

            }
        }

        private void PonerMensajeExito(string nombreTabla)
        {
            string hileraBase = "Insercion exitosa en tabla " + nombreTabla;

            switch (nombreTabla)
            {
                case "Persona":
                {
                    ViewBag.InsercionPersona = hileraBase;
                }
                break;
                case "Usuario":
                {
                    ViewBag.InsercionUsuario = hileraBase;
                }
                break;
            }
        }


        /*
         * EFECTO: Realiza un mapeo de las columnas de las tablas en memoria con las columnas de las tablas en la base de datos. 
         * REQUIERE: Conexion instanciada y abierta hacia la base de datos.
         * MODIFICA: n/a
         */
        private void MapearColumnasATablaDeDestino(string tableName, SqlBulkCopy insercionEnBloque)
        {
            switch (tableName)
            {
                case "Persona":
                {
                    insercionEnBloque.ColumnMappings.Add("cedula", "Cedula");
                    insercionEnBloque.ColumnMappings.Add("nombre1", "Nombre1");
                    insercionEnBloque.ColumnMappings.Add("nombre2", "Nombre2");
                    insercionEnBloque.ColumnMappings.Add("apellido1", "Apellido1");
                    insercionEnBloque.ColumnMappings.Add("apellido2", "Apellido2");
                    insercionEnBloque.ColumnMappings.Add("direccion_exacta", "DireccionDetallada");
                }
                break;
                case "Usuario":
                {
                        insercionEnBloque.ColumnMappings.Add("correo", "CorreoInstitucional");
                        insercionEnBloque.ColumnMappings.Add("contrasena", "Contrasena");
                        insercionEnBloque.ColumnMappings.Add("activo", "Activo");
                        insercionEnBloque.ColumnMappings.Add("cedula", "Cedula");
                        insercionEnBloque.ColumnMappings.Add("id", "Id");
                }
                break;
            }
        }


        /*
         * EFECTO: Agrega una fila incorrecta en la tabla de filas invalidas.
         * REQUIERE: n/a
         * MODIFICA: n/a
         */
        private void AgregarTuplaInvalida(string filaLeida, Exception exception, DataTable filasInvalidas, int numeroFilasLeidas)
        {
            DataRow filaInvalida = filasInvalidas.NewRow();
            filaInvalida["fila"] = numeroFilasLeidas;
            filaInvalida["error"] = exception.Message;
            filaInvalida["cedula"] = filasInvalidas.Rows[numeroFilasLeidas - 1]["cedula"];
            filaInvalida["perfil"] = filasInvalidas.Rows[numeroFilasLeidas - 1]["perfil"];
            filaInvalida["carne"] = filasInvalidas.Rows[numeroFilasLeidas - 1]["carne"];
            filaInvalida["nombre1"] = filasInvalidas.Rows[numeroFilasLeidas - 1]["nombre1"];
            filaInvalida["nombre2"] = filasInvalidas.Rows[numeroFilasLeidas - 1]["nombre2"];
            filaInvalida["apellido1"] = filasInvalidas.Rows[numeroFilasLeidas - 1]["apellido1"];
            filaInvalida["apellido2"] = filasInvalidas.Rows[numeroFilasLeidas - 1]["apellido2"];
            filaInvalida["correo"] = filasInvalidas.Rows[numeroFilasLeidas - 1]["correo"];
            filaInvalida["provincia"] = filasInvalidas.Rows[numeroFilasLeidas - 1]["provincia"];
            filaInvalida["canton"] = filasInvalidas.Rows[numeroFilasLeidas - 1]["canton"];
            filaInvalida["distrito"] = filasInvalidas.Rows[numeroFilasLeidas - 1]["distrito"];
            filaInvalida["direccion_exacta"] = filasInvalidas.Rows[numeroFilasLeidas - 1]["direccion_exacta"];
            filaInvalida["sigla_carrera"] = filasInvalidas.Rows[numeroFilasLeidas - 1]["sigla_carrera"];
            filaInvalida["enfasis"] = filasInvalidas.Rows[numeroFilasLeidas - 1]["enfasis"];
        }

        /*
         * EFECTO: Retorna tabla en memoria con un esquema basico. Utilizado para almacenar filas del archivo que preeliminarmente estan correctas.
         * REQUIERE: n/a
         * MODIFICA: n/a
         */
        private DataTable CrearTablaUsuarios()
        {
            DataTable dt = new DataTable();
            dt.TableName = "usuarios_validos";
            // Ver documentacion sobre los tipos en https://docs.microsoft.com/en-us/dotnet/api/system.data.sqltypes?view=netframework-4.6.1
            //dt.Columns.Add("cedula", System.Type.GetType("System.Data.SqlTypes.SqlChars"));
            dt.Columns.Add("cedula", typeof(string));            //0
            dt.Columns.Add("perfil", typeof(string));            //1
            dt.Columns.Add("carne", typeof(string));             //2
            dt.Columns.Add("nombre1", typeof(string));           //3
            dt.Columns.Add("nombre2", typeof(string));           //4
            dt.Columns.Add("apellido1", typeof(string));         //5
            dt.Columns.Add("apellido2", typeof(string));         //6
            dt.Columns.Add("correo", typeof(string));            //7
            dt.Columns.Add("provincia", typeof(string));         //8
            dt.Columns.Add("canton", typeof(string));            //9
            dt.Columns.Add("distrito", typeof(string));          //10
            dt.Columns.Add("direccion_exacta", typeof(string));  //11
            dt.Columns.Add("sigla_carrera", typeof(string));     //12
            dt.Columns.Add("enfasis", typeof(byte));            //13
        
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
            //dt.Columns[13].MaxLength = 3; //Need to check if this applies correctly to tiny ints.
            return dt;
        }



        /*
         * EFECTO: Retorna una tabla en memoria sin esquema para guardar las filas incorrectas provisionadas en el archivo csv.
         * REQUIERE: n/a
         * MODIFICA: n/a
         */
        private DataTable CrearTablaUsuariosInvalidos()
        {
            DataTable dt = new DataTable();
            dt.TableName = "usuarios_invalidos";

            dt.Columns.Add("fila", typeof(int));
            dt.Columns.Add("error", typeof(string));
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


        /*
         * EFECTO: Retorna una tabla en memoria con el esquema de la tabla Persona de la base de datos.
         * REQUIERE: n/a
         * MODIFICA: n/a
         */
        private DataTable CrearTablaPersonaBD()
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

            dt.Columns[3].AllowDBNull = false;
            dt.Columns[4].AllowDBNull = false;
            dt.Columns[5].AllowDBNull = false;
            return dt;
        }



        /*
         * EFECTO: retorna una tabla en memoria con el esquema de la tabla Usuario de la base de datos.
         * REQUIERE: n/a
         * MODIFICA: n/a
         */
        private DataTable CrearTablaUsuarioBD()
        {
            DataTable dt = new DataTable();
            dt.TableName = "Usuario";
            dt.Columns.Add("correo", typeof(System.Data.SqlTypes.SqlChars));
            dt.Columns.Add("contrasena", typeof(System.Data.SqlTypes.SqlChars));
            dt.Columns.Add("activo", typeof(System.Data.SqlTypes.SqlBoolean));
            dt.Columns.Add("cedula", typeof(System.Data.SqlTypes.SqlChars));
            dt.Columns.Add("id", typeof(System.Data.SqlTypes.SqlGuid));

            dt.Columns[0].AllowDBNull = false;
            dt.Columns[1].AllowDBNull = false;
            dt.Columns[2].AllowDBNull = false;
            dt.Columns[3].AllowDBNull = false;
            dt.Columns[4].AllowDBNull = false;

            return dt;
        }
    }
 
}