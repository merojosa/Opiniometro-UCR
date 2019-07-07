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
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.WebPages;

//using Microsoft.SqlServer.Dts;
//using Microsoft.SqlServer.Dts.Runtime;


namespace Opiniometro_WebApp.Controllers
{
    enum Tablas { DatosProvisionados, Persona, Usuario, Estudiante, Empadronado, Profesor, TieneUsuarioPerfilEnfasis};
    enum EntradasProvisionadas {Cedula, Perfil, Nombre1, Nombre2, Apellido1, Apellido2, Correo, Carne, SiglaCarrera, NumeroEnfasis}
    public class AdministradorController : Controller
    {
        private Opiniometro_DatosEntities db = new Opiniometro_DatosEntities();

        // GET: Administrador
        public ActionResult Index()
        {

            return View();
        }

        [HttpGet]
        public ActionResult CargarUsuarios()
        {
            return View();
        }


        /*
         * EFECTO:
         * REQUIERE:
         * MODIFICA:
         */
        [HttpPost]
        public ActionResult CargarUsuarios(HttpPostedFileBase postedFile)
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
                        System.IO.File.Delete(path + Path.GetFileName(postedFile.FileName));
                    }
                    catch (Exception e)
                    {

                        ViewBag.Message = "Error al cargar el archivo. Intente de nuevo mas tarde.";
                        System.IO.File.Delete(path + Path.GetFileName(postedFile.FileName));
                        Console.WriteLine(e);
                        throw;
                    }

                    
                }
                else
                {
                    ViewBag.Message = "Ha intentado cargar un archivo incompatible";
                }
                
            }

            return View(filasInvalidas);
        }


        /*
         * EFECTO: Lee y procesa archivo csv subido por el usuario.
         * REQUIERE: Un archivo subido por el usuario.
         * MODIFICA: 
         */
        private DataTable ProcesarArchivo(string path)
        {
            DataTable filasValidas = ObtenerTabla(Tablas.DatosProvisionados);//CrearTablaUsuarios();
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
                        String[] entradasFilaLeida = filaLeida.Split(',');
                        bool valido = ValidacionEntrada(entradasFilaLeida, filasInvalidas, numeroFilasLeidas);
                        if (valido)
                        {
                            tupla.ItemArray = entradasFilaLeida;
                            //Tupla cumple preeliminarmente con requisitos de formato
                            if (tupla.HasErrors)
                            {
                                AgregarTuplaInvalida(filaLeida, filasInvalidas, tupla, numeroFilasLeidas);
                            }
                            else
                            {
                                filasValidas.Rows.Add(tupla);
                            }
                        }
                        
                        

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
            DataTable personaBD = ObtenerTabla(Tablas.Persona);
            DataTable usuarioBD = ObtenerTabla(Tablas.Usuario);
            DataTable estudianteBD = ObtenerTabla(Tablas.Estudiante);
            DataTable empadronadoBD = ObtenerTabla(Tablas.Empadronado);
            DataTable profesorBD = ObtenerTabla(Tablas.Profesor);
            DataTable tieneUsuarioPerfilEnfasisBD = ObtenerTabla(Tablas.TieneUsuarioPerfilEnfasis);

            //Multicast
            MulticastDatosProvisionados(filasValidas, personaBD, usuarioBD, estudianteBD, empadronadoBD, profesorBD, tieneUsuarioPerfilEnfasisBD);
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
                ViewBag.InsercionUsuario = "No hubo inserciones en la tabla Usuario";
            }

            if(estudianteBD.Rows.Count > 0)
            {
                InsercionEnBloque(estudianteBD);
            }
            else
            {
                ViewBag.InsercionEstudiante = "No hubo inserciones en la tabla Estudiante";
            }

            if(empadronadoBD.Rows.Count > 0)
            {
                InsercionEnBloque(empadronadoBD);
            }
            else
            {
                ViewBag.InsercionEmpadronado = "No hubo inserciones en la tabla Empadronado";
            }

            if (profesorBD.Rows.Count > 0)
            {
                InsercionEnBloque(profesorBD);
            }
            else
            {
                ViewBag.InsercionProfesor = "No hubo inserciones en la tabla Profesor";
            }

            if(tieneUsuarioPerfilEnfasisBD.Rows.Count > 0)
            {
                InsercionEnBloque(tieneUsuarioPerfilEnfasisBD);
            }
            else
            {
                ViewBag.InsercionTUPE = "No hubo inserciones en la tabla Tiene_Usuario_Perfil_Enfasis";
            }

            personaBD.Dispose();
            usuarioBD.Dispose();
            estudianteBD.Dispose();
            empadronadoBD.Dispose();
            profesorBD.Dispose();
            tieneUsuarioPerfilEnfasisBD.Dispose();
            filasInvalidas.AcceptChanges();
            return filasInvalidas;
        }

        private bool ValidacionEntrada(string[] entradasFilaLeida, DataTable filasInvalidas, int numeroFilasLeidas)
        {
            DataRow filaInvalida = filasInvalidas.NewRow();
            if (ValidacionLongitud(filaInvalida, entradasFilaLeida) &&
                ValidacionContenido(filaInvalida, entradasFilaLeida))
            {
                return true;
            }
            
            

            if (filaInvalida.RowState == DataRowState.Modified)
            {
                filaInvalida[1] = numeroFilasLeidas;
                for (int i = 0; i < entradasFilaLeida.Length; i++)
                {
                    filaInvalida[i + 2] = entradasFilaLeida[i];
                }
                

                filasInvalidas.Rows.Add(filaInvalida);
                filasInvalidas.AcceptChanges();
            }

            return false;
        }

        private bool ValidacionContenido(DataRow filaInvalida, string[] entradasFilaLeida)
        {
            bool valido = true;

            if (!Regex.IsMatch(entradasFilaLeida[0], @"[0-9]+"))
            {
                filaInvalida["error"] += "El numero de cédula solo debe contener dígitos entre 0-9\n";
                valido = false;
            }

            if (!Regex.IsMatch(entradasFilaLeida[1], @"Estudiante|Profesor", RegexOptions.IgnoreCase))
            {
                filaInvalida["error"] += "El perfil debe ser Estudiante o Profesor\n";
                valido = false;
            }

            for (int i = 2; i < 6; ++i)
            {
                if (!Regex.IsMatch(entradasFilaLeida[i], @"[a-zA-Z]"))
                {
                    string nombreCampo = String.Empty;
                    
                    if(i == 2) { nombreCampo = "primer nombre";}
                        else if(i == 3) { nombreCampo = "segundo nombre";}
                            else if(i == 4) { nombreCampo = "primer apellido";}
                                else if(i == 5) { nombreCampo = "segundo apellido";}

                    filaInvalida["error"] += "El " + nombreCampo + " solo debe contener caracteres que sean letras\n";
                }
            }

            if (!Regex.IsMatch(entradasFilaLeida[6], @"([\w]+\.)([\w]+)(@ucr.ac.cr)"))
            {
                filaInvalida["error"] +=
                    "Direccion de correo debe tener el formato de las direcciones emitidas por la UCR\n"; 
                valido = false;
            }

            if (!entradasFilaLeida[7].IsEmpty() && !Regex.IsMatch(entradasFilaLeida[7], @"[A-Z]{1}[\d]{5}"))
            {
                filaInvalida["error"] +=
                    "El carne debe tener una letra de la A-Z en mayúscula seguido de cinco dígitos entre 0-9\n";
                valido = false;
            }

            if (!Regex.IsMatch(entradasFilaLeida[9], @"[\d]{1,3}"))
            {
                filaInvalida["error"] += "El numero del énfasis solo debe contener dígitos entre 0-9\n";
                valido = false;
            }

            return valido;
        }

        private bool ValidacionLongitud(DataRow filaInvalida, string[] entradasFilaLeida)
        {
            bool valido = false;
            if (entradasFilaLeida.Length != 10)
            {
                if (entradasFilaLeida.Length < 10)
                {
                    filaInvalida["error"] += "La fila de entrada contiene menos de diez items.\n";
                }
                else
                {
                    filaInvalida["error"] += "La fila de entrada contiene más de diez items.\n";
                }
            }
            else
            {
                valido = true;
                for (int i = 0; i < entradasFilaLeida.Length; i++)
                {

                    switch (i)
                    {
                        case 0: //cedula
                        {
                            if (entradasFilaLeida[i].IsEmpty() || entradasFilaLeida[i].Length != 9)
                            {
                                filaInvalida["error"] += "El numero de cédula es un campo obligatorio y debe contener nueve caracteres\n";
                                valido = false;
                            }
                        }
                            break;
                        case 1: //perfil
                        {
                            if (entradasFilaLeida[i].IsEmpty() || entradasFilaLeida[i].Length > 30)
                            {
                                filaInvalida["error"] +=
                                    "El nombre de un perfil es un campo obligatorio y debe contener al menos 1 caracter y no debe exceder de 30 caracteres\n";
                                valido = false;
                            }
                        }
                            break;
                        case 2: //nombre
                        case 4: //apellido1
                        case 5: //apellido2
                        case 6: //correo institucional
                        {
                            if (entradasFilaLeida[i].IsEmpty() || entradasFilaLeida[i].Length > 50)
                            {
                                string nombreCampo = String.Empty;
                                if (i == 2)
                                {
                                    nombreCampo = "primer nombre";
                                }
                                else if (i == 4)
                                {
                                    nombreCampo = "primer apellido";
                                }
                                else if (i == 5)
                                {
                                    nombreCampo = "segundo apellido";
                                }
                                else if (i == 6)
                                {
                                    nombreCampo = "correo institucional";
                                }

                                filaInvalida["error"] += "El " + nombreCampo +
                                                         " es un campo obligatorio y debe contener al menos un caracter y no debe exceder de 50 caracteres\n";
                                valido = false;
                            }
                        }
                            break;
                        case 3: //nombre2
                        case 7: //carne
                        {
                            if (!entradasFilaLeida[i].IsEmpty())
                            {
                                if (i == 3 && entradasFilaLeida[i].Length > 50)
                                {
                                    filaInvalida["error"] += "El segundo nombre no debe exceder de 50 caracteres\n";
                                    valido = false;
                                }
                                else if (i == 7 && entradasFilaLeida[i].Length != 6)
                                {
                                    filaInvalida["error"] += "El carne debe contener seis caracteres\n";
                                    valido = false;
                                }
                            }
                        }
                            break;
                        case 8://sigla carrera
                        {
                            if (entradasFilaLeida[i].Length > 10)
                            {
                                filaInvalida["error"] += "La sigla de carrera no debe exceder de diez caracteres\n";
                                valido = false;
                            }
                        }
                            break;
                        case 9://numero enfasis
                        {
                            if (entradasFilaLeida[i].IsEmpty() || entradasFilaLeida[i].Length > 3)
                            {
                                filaInvalida["error"] +=
                                    "El numero de enfasis de contener al menos un caracter y no debe exceder de tres caracteres\n";
                                valido = false;
                            }
                        }
                            break;
                    }
                }

                
            }

            return valido;
        }

        private void AgregarTuplaInvalida(string filaLeida, DataTable filasInvalidas, DataRow tupla, int numeroFilasLeidas)
        {
            DataColumn[] columnasConError = tupla.GetColumnsInError();
            DataRow filaInvalida = filasInvalidas.NewRow();
            foreach (DataColumn columna in columnasConError)
            {
                filaInvalida["error"] += tupla.GetColumnError(columna.ColumnName);
            }

            filaInvalida["fila"] = numeroFilasLeidas;
            filaInvalida["cedula"] = tupla["cedula"];
            filaInvalida["perfil"] = tupla["perfil"];
            filaInvalida["carne"] = tupla["carne"];
            filaInvalida["nombre1"] = tupla["nombre1"];
            filaInvalida["nombre2"] = tupla["nombre2"];
            filaInvalida["apellido1"] = tupla["apellido1"];
            filaInvalida["apellido2"] = tupla["apellido2"];
            filaInvalida["correo"] = tupla["correo"];
            filaInvalida["sigla_carrera"] = tupla["sigla_carrera"];
            filaInvalida["enfasis"] = tupla["enfasis"];

            filasInvalidas.Rows.Add(filaInvalida);
            filasInvalidas.AcceptChanges();
        }

        /*
         * EFECTO: Clasifica y dispersa los datos de la tabla en memoria filasValidas. 
         * REQUIERE: Tabla en memoria que contiene los datos provisionados en el archivo csv,
         * MODIFICA: n/a
         */
        private int MulticastDatosProvisionados(DataTable filasValidas, DataTable personaBD, DataTable usuarioBD, DataTable estudianteBD, DataTable empadronadoBD, DataTable profesorBD, DataTable tieneUsuarioPerfilEnfasisBD)
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
                
                DataRow filaNueva = filasValidas.Rows[indexFilasValidas];


                if (db.Persona.Find(filaNueva["Cedula"]) == null)
                {
                    InsertarFilaEnTablaEnMemoria(filaNueva, personaBD);
                    personaBD.AcceptChanges();
                }
                if (db.Usuario.Find(filaNueva["Cedula"]) == null)
                {
                    InsertarFilaEnTablaEnMemoria(filaNueva, usuarioBD);
                    usuarioBD.AcceptChanges();
                }
                if (filaNueva["Perfil"].ToString().CompareTo("Estudiante") == 0)
                {
                    if (db.Estudiante.Find(filaNueva["Cedula"]) == null)
                    {
                        InsertarFilaEnTablaEnMemoria(filaNueva, estudianteBD);
                        estudianteBD.AcceptChanges();
                    }
                    if (db.Empadronado.Find(filaNueva["Cedula"], filaNueva["NumeroEnfasis"], filaNueva["SiglaCarrera"]) == null)
                    {
                        InsertarFilaEnTablaEnMemoria(filaNueva, empadronadoBD);
                        empadronadoBD.AcceptChanges();
                    }
                }
                else if (filaNueva["Perfil"].ToString().CompareTo("Profesor") == 0)
                {
                    if (db.Profesor.Find(filaNueva["Cedula"]) == null)
                    {
                        InsertarFilaEnTablaEnMemoria(filaNueva, profesorBD);
                    }
                }
                
                
                if(db.Tiene_Usuario_Perfil_Enfasis.Find(filaNueva["CorreoInstitucional"], filaNueva["NumeroEnfasis"], filaNueva["SiglaCarrera"], filaNueva["Perfil"]) == null)
                {
                    InsertarFilaEnTablaEnMemoria(filaNueva, tieneUsuarioPerfilEnfasisBD);
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
                case "Estudiante":
                {
                    InsertarEnEstudianteBD(filaAInsertar, tablaDestino);
                }
                break;
                case "Empadronado":
                {
                    InsertarEnEmpadronadoBD(filaAInsertar, tablaDestino);
                }
                break;
                case "Profesor":
                {
                    InsertarEnProfesorBD(filaAInsertar, tablaDestino);
                }
                break;
                case "Tiene_Usuario_Perfil_Enfasis":
                {
                    InsertarEnTieneUsuarioPerfilEnfasisBD(filaAInsertar, tablaDestino);
                }
                break;
            }
            
        }

        private void InsertarEnPersonaBD(DataRow filaAInsertar, DataTable personaBD)
        {
            DataRow filaNueva = personaBD.NewRow();
            
            filaNueva["Cedula"] = filaAInsertar["Cedula"];
            filaNueva["Nombre1"] = filaAInsertar["Nombre1"];
            filaNueva["Nombre2"] = filaAInsertar["Nombre2"];
            filaNueva["Apellido1"] = filaAInsertar["Apellido1"];
            filaNueva["Apellido2"] = filaAInsertar["Apellido2"];
            
            personaBD.Rows.Add(filaNueva);
            personaBD.AcceptChanges();
        }

        private void InsertarEnUsuarioBD(DataRow filaAInsertar, DataTable usuarioBD)
        {
            string hileraConexion =
                "data source=(localdb)\\ProjectsV13;initial catalog=Opiniometro_Datos;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework&quot;";
            DataRow filaNueva = usuarioBD.NewRow();
            filaNueva["CorreoInstitucional"] = filaAInsertar["CorreoInstitucional"];




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
            filaNueva["Contrasena"] = contrasenaCifrada; //contrasenaHash;//contrasenaHash.Substring(0,50);//
            filaNueva["Activo"] = true;
            filaNueva["Cedula"] = filaAInsertar["Cedula"];
            //filaNueva["id"] = new SqlGuid(guid.ToString());
            filaNueva["Id"] = guid;
            usuarioBD.Rows.Add(filaNueva);
            usuarioBD.AcceptChanges();
            
        }
        
        private void InsertarEnEstudianteBD(DataRow filaAInsertar, DataTable estudianteBD)
        {
            DataRow filaNueva = estudianteBD.NewRow();
            filaNueva["CedulaEstudiante"] = filaAInsertar["Cedula"];
            filaNueva["Carne"] = filaAInsertar["Carne"];
            estudianteBD.Rows.Add(filaNueva);
            estudianteBD.AcceptChanges();
        }

        private void InsertarEnEmpadronadoBD(DataRow filaAInsertar, DataTable empadronadoBD)
        {
            DataRow filaNueva = empadronadoBD.NewRow();
            filaNueva["CedulaEstudiante"] = filaAInsertar["Cedula"];
            filaNueva["NumeroEnfasis"] = filaAInsertar["NumeroEnfasis"];
            filaNueva["SiglaCarrera"] = filaAInsertar["SiglaCarrera"];
            empadronadoBD.Rows.Add(filaNueva);
            empadronadoBD.AcceptChanges();
        }

        private void InsertarEnProfesorBD(DataRow filaAInsertar, DataTable profesorBD)
        {
            DataRow filaNueva = profesorBD.NewRow();
            filaNueva["CedulaProfesor"] = filaAInsertar["Cedula"];
            profesorBD.Rows.Add(filaNueva);
            profesorBD.AcceptChanges();
        }

        private void InsertarEnTieneUsuarioPerfilEnfasisBD(DataRow filaAInsertar, DataTable tieneUsuarioPerfilEnfasisBD)
        {
            DataRow filaNueva = tieneUsuarioPerfilEnfasisBD.NewRow();

            filaNueva["CorreoInstitucional"] = filaAInsertar["CorreoInstitucional"];
            filaNueva["NumeroEnfasis"] = filaAInsertar["NumeroEnfasis"];
            filaNueva["SiglaCarrera"] = filaAInsertar["SiglaCarrera"];
            if (Regex.IsMatch(filaAInsertar["Perfil"].ToString(), @"Estudiante", RegexOptions.IgnoreCase))
            {
                filaNueva["NombrePerfil"] = "Estudiante";
            }
            else if (Regex.IsMatch(filaAInsertar["Perfil"].ToString(), @"Profesor", RegexOptions.IgnoreCase))
            {
                filaNueva["NombrePerfil"] = "Profesor";
            }
             
            tieneUsuarioPerfilEnfasisBD.Rows.Add(filaNueva);
            tieneUsuarioPerfilEnfasisBD.AcceptChanges();
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
                                                          //Fuente  Destino
                    insercionEnBloque.ColumnMappings.Add("Cedula", "Cedula");
                    insercionEnBloque.ColumnMappings.Add("Nombre1", "Nombre1");
                    insercionEnBloque.ColumnMappings.Add("Nombre2", "Nombre2");
                    insercionEnBloque.ColumnMappings.Add("Apellido1", "Apellido1");
                    insercionEnBloque.ColumnMappings.Add("Apellido2", "Apellido2");
                    
                }
                break;
                case "Usuario":
                {
                    insercionEnBloque.ColumnMappings.Add("CorreoInstitucional", "CorreoInstitucional");
                    insercionEnBloque.ColumnMappings.Add("Contrasena", "Contrasena");
                    insercionEnBloque.ColumnMappings.Add("Activo", "Activo");
                    insercionEnBloque.ColumnMappings.Add("Cedula", "Cedula");
                    insercionEnBloque.ColumnMappings.Add("Id", "Id");
                }
                break;
                case "Estudiante":
                {
                    insercionEnBloque.ColumnMappings.Add("CedulaEstudiante", "CedulaEstudiante");
                    insercionEnBloque.ColumnMappings.Add("Carne", "Carne");
                }
                break;
                case "Empadronado":
                {
                    insercionEnBloque.ColumnMappings.Add("CedulaEstudiante", "CedulaEstudiante");
                    insercionEnBloque.ColumnMappings.Add("NumeroEnfasis", "NumeroEnfasis");
                    insercionEnBloque.ColumnMappings.Add("SiglaCarrera", "SiglaCarrera");
                }
                break;
                case "Profesor":
                {
                    insercionEnBloque.ColumnMappings.Add("CedulaProfesor", "CedulaProfesor");
                }
                break;
                case "Tiene_Usuario_Perfil_Enfasis":
                {
                    insercionEnBloque.ColumnMappings.Add("CorreoInstitucional", "CorreoInstitucional");
                    insercionEnBloque.ColumnMappings.Add("NumeroEnfasis", "NumeroEnfasis");
                    insercionEnBloque.ColumnMappings.Add("SiglaCarrera", "SiglaCarrera");
                    insercionEnBloque.ColumnMappings.Add("NombrePerfil", "NombrePerfil");
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
            filaInvalida["sigla_carrera"] = filasInvalidas.Rows[numeroFilasLeidas - 1]["sigla_carrera"];
            filaInvalida["enfasis"] = filasInvalidas.Rows[numeroFilasLeidas - 1]["enfasis"];

            filasInvalidas.Rows.Add(filaInvalida);
            filasInvalidas.AcceptChanges();
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
            
            dt.Columns.Add("nombre1", typeof(string));
            dt.Columns.Add("nombre2", typeof(string));
            dt.Columns.Add("apellido1", typeof(string));
            dt.Columns.Add("apellido2", typeof(string));
            dt.Columns.Add("correo", typeof(string));
            dt.Columns.Add("carne", typeof(string));
            dt.Columns.Add("sigla_carrera", typeof(string));
            dt.Columns.Add("enfasis", typeof(string));
            
            return dt;
        }

        private DataTable ObtenerTabla(Tablas EnumeracionTipoTabla)
        {
            Type Tipo = ObtenerTipoDeTabla(EnumeracionTipoTabla);
            DataTable dt = null;
            if(Tipo != null)
            {
                PropertyInfo[] propiedadesDeTipo = Tipo.GetProperties();
                dt = new DataTable(Tipo.Name);

                foreach(PropertyInfo propiedad in propiedadesDeTipo)
                {
                    dt.Columns.Add(propiedad.Name, Nullable.GetUnderlyingType(propiedad.PropertyType) ?? propiedad.PropertyType);
                }
            }
            return dt;
        }

        private Type ObtenerTipoDeTabla(Tablas EnumeracionTipoTabla)
        {
            Type Tipo = null;

            switch (EnumeracionTipoTabla)
            {
                case Tablas.DatosProvisionados:
                    {
                        Tipo = typeof(DatosProvisionados);
                    }
                    break;
                case Tablas.Persona:
                    {
                        Tipo = typeof(Persona);
                    }
                    break;
                case Tablas.Usuario:
                    {
                        Tipo = typeof(Usuario);
                    }
                    break;
                case Tablas.Estudiante:
                    {
                        Tipo = typeof(Estudiante);
                    }
                    break;
                case Tablas.Empadronado:
                    {
                        Tipo = typeof(Empadronado) ;
                    }
                    break;
                case Tablas.Profesor:
                    {
                        Tipo =  typeof(Profesor);
                    }
                    break;
                case Tablas.TieneUsuarioPerfilEnfasis:
                    {
                        Tipo = typeof(Tiene_Usuario_Perfil_Enfasis);
                    }
                    break;
            }
            return Tipo;
        }
    }
 
}