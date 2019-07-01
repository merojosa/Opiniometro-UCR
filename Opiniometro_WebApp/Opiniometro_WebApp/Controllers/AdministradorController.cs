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
using System.Security.Cryptography;
using Opiniometro_WebApp.Models;
using System.Text;
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
            DataTable filasInvalidas = null;

            if (postedFile != null && postedFile.ContentLength > 0)
            {
                if (postedFile.FileName.EndsWith(".csv"))
                {
                    string path = Server.MapPath("~/App_Data/ArchivosCargados/");
                    
                    try
                    {
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        postedFile.SaveAs(path + Path.GetFileName(postedFile.FileName));
                        filasInvalidas = ProcesarArchivo(path + Path.GetFileName(postedFile.FileName));
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
                    ViewBag.Message = "Ha intentado cargar un archivo incompatible";
                }
                
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
                DataRow filaNueva = filasValidas.Rows[indexFilasValidas];
                if (db.Persona.Find(filasValidas.Rows[indexFilasValidas]["cedula"]) == null)
                {
                    InsertarFilaEnTablaEnMemoria(filaNueva, personaBD);
                    personaBD.AcceptChanges();
                }
                if (db.Usuario.Find(filasValidas.Rows[indexFilasValidas]["cedula"]) == null)
                {
                    InsertarFilaEnTablaEnMemoria(filaNueva, usuarioBD);
                    usuarioBD.AcceptChanges();
                }

                
            }
            
            
            
            return 1;

        }

        private void InsertarFilaEnTablaEnMemoria(DataRow filaAInsertar, DataTable tablaDestino)
        {
            
            switch (tablaDestino.TableName)
            {
                
                case "Persona":
                {
                    InsertarEnPersonaBD(filaAInsertar, tablaDestino);
                }
                break;
                case "Usuario":
                {
                    InsertarEnUsuarioBD(filaAInsertar, tablaDestino);

                }
                break;
            }
            
        }

        private void InsertarEnPersonaBD(DataRow filaAInsertar, DataTable personaBD)
        {
            DataRow filaNueva = personaBD.NewRow();
            filaNueva["cedula"] = new SqlChars(filaAInsertar["cedula"].ToString().ToCharArray());
            filaNueva["nombre1"] = new SqlChars(filaAInsertar["nombre1"].ToString().ToCharArray());
            filaNueva["apellido1"] = new SqlChars(filaAInsertar["apellido1"].ToString().ToCharArray());
            filaNueva["apellido2"] = new SqlChars(filaAInsertar["apellido2"].ToString().ToCharArray());
            filaNueva["direccion_exacta"] = new SqlChars(filaAInsertar["direccion_exacta"].ToString().ToCharArray());
            personaBD.Rows.Add(filaNueva);
            personaBD.AcceptChanges();
        }

        private void InsertarEnUsuarioBD(DataRow filaAInsertar, DataTable usuarioBD)
        {
            string hileraConexion =
                "data source=(localdb)\\ProjectsV13;initial catalog=Opiniometro_Datos;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework&quot;";
            DataRow filaNueva = usuarioBD.NewRow();
            filaNueva["correo"] = new SqlChars(filaAInsertar["correo"].ToString().ToCharArray());




            //Insercion de un usuario requiere que tenga un contrasena cifrada con un guid
            string contrasenaRandom = GenerarContrasenaRandom(10, null);
            Guid guid = ObtenerIdUnico();
            byte[] resultadoHash;
            /*using (SqlConnection conexionBD = new SqlConnection(hileraConexion))
            {
                conexionBD.Open();
                SqlCommand cmd = new SqlCommand("dbo.SP_GenerarContrasenaHash", conexionBD);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", guid.ToString());
                cmd.Parameters.AddWithValue("contrasena", contrasenaRandom);
                cmd.Parameters.Add("@contrasenaHash", System.Data.SqlDbType.VarBinary, 50).Direction = System.Data.ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                resultadoHash = (byte[]) cmd.Parameters["@contrasenaHash"].Value;
            }
            */


            ObjectParameter contrasenaHash = new ObjectParameter("contrasenaHash", typeof(SqlBinary));
            db.SP_GenerarContrasenaHash(guid.ToString().Substring(0,36), contrasenaRandom, contrasenaHash);
            //string contrasenaHash = GenerarContrasenaCifrada(contrasenaRandom += guid.ToString().Substring(0, 36));
            //SqlChars chars = new SqlChars((char[])contrasenaHash.Value);
            //SqlChars chars = new SqlChars(contrasenaHash.ToCharArray());
            //string contrasenaString = Encoding.Unicode.GetString(contrasenaCifrada, 0, contrasenaCifrada.Length);
            //SqlBinary datosCifrados = (SqlBinary)contrasenaHash.Value;
            //string contrasenaCifrada = Convert.ToString(contrasenaHash.Value);
            resultadoHash = (byte[])contrasenaHash.Value;
            string contrasenaCifrada = resultadoHash.ToString();
            filaNueva["contrasena"] = new SqlChars(contrasenaCifrada.ToCharArray()); //contrasenaHash;//contrasenaHash.Substring(0,50);//
            filaNueva["activo"] = new SqlBoolean(true);
            filaNueva["cedula"] = new SqlChars(filaAInsertar["cedula"].ToString().ToCharArray());
            //filaNueva["id"] = new SqlGuid(guid.ToString());
            filaNueva["id"] = guid;
            usuarioBD.Rows.Add(filaNueva);
            usuarioBD.AcceptChanges();
            
        }

        private string GenerarContrasenaCifrada(string stringUnico)
        {
            byte[] datosCrudos = Encoding.Unicode.GetBytes(stringUnico);
            
            byte[] datosCifrados;
            using(SHA512 sha = SHA512Managed.Create())
            {
                
                datosCifrados = sha.ComputeHash(datosCrudos);
                
            }
            string base64 = System.Convert.ToBase64String(datosCifrados);
            byte[] base64bytes = System.Convert.FromBase64String(base64);
            return Encoding.Unicode.GetString(base64bytes, 0, base64bytes.Length);
            //return System.Convert.ToBase64String(datosCifrados);
            
            //Encoding.Unicode.GetString()
            //return Encoding.Unicode.GetString(datosCifrados, 0, datosCifrados.Length);
            

        }

        private string GenerarContrasenaRandom(int tamano, string caracteresPermitidos=null)
        {
            /*
             * if(string.IsNullOrEmpty(caracteresPermitidos))
            {
                caracteresPermitidos = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            }
            byte[] valoresRandom = new byte[tamano];

            using(RNGCryptoServiceProvider proveedor = new RNGCryptoServiceProvider())
            {
                proveedor.GetBytes(valoresRandom);
            }
            char[] caracteres = caracteresPermitidos.ToCharArray();
            char[] contrasenaGenerada = new char[tamano];
            for(int i = 0; i<tamano;++i)
            {
                contrasenaGenerada[i] = caracteres[valoresRandom[i] % tamano];
            }
            return new string(contrasenaGenerada);
             */
            return "hjjaadbegd";
        }

        private Guid ObtenerIdUnico()
        {
            //return Guid.NewGuid();
            return Guid.Parse("db8f24c9-76a5-4db7-86a8-e40b82fef13a");
        }
        /*
         * EFECTO: Realiza una insercion en bloque en una tabla de base de datos.
         * REQUIERE: Conexion instanciada y abierta hacia la base de datos. Una tabla en memoria que previamente haya sido llenada con los nuevos valores a guardar en la base de datos.
         * MODIFICA: Agrega tuplas en una tabla en la base de datos. 
         */
        private void InsercionEnBloque(DataTable tablaAInsertar)
        {
            //string hileraConexion = ConfigurationManager.ConnectionStrings["Opiniometro_DatosEntities"].ConnectionString;
            //string hileraConexion = "data source=10.1.4.55;initial catalog=Opiniometro;persist security info=True;user id=DeveloperOp;password=devop2019;multipleactiveresultsets=True;application name=EntityFramework&quot;";
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
            //dt.Columns.Add("contrasena", typeof(string));
            dt.Columns.Add("activo", typeof(System.Data.SqlTypes.SqlBoolean));
            dt.Columns.Add("cedula", typeof(System.Data.SqlTypes.SqlChars));
            dt.Columns.Add("id", typeof(System.Data.SqlTypes.SqlGuid));

            dt.Columns[0].AllowDBNull = false;
            
            dt.Columns[1].AllowDBNull = false;
            //dt.Columns[1].MaxLength = 50;
            dt.Columns[2].AllowDBNull = false;
            dt.Columns[3].AllowDBNull = false;
            dt.Columns[4].AllowDBNull = false;

            

            return dt;
        }
    }
 
}