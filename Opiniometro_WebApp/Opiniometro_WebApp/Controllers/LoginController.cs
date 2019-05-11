using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Opiniometro_WebApp.Models;

namespace Opiniometro_WebApp.Controllers
{
    public class LoginController : Controller
    {
        private Opiniometro_DatosEntities3 db = new Opiniometro_DatosEntities3();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        // Recibe el correo y contraseña en form_collection.
        [HttpPost]
        public string Login(FormCollection form_collection)
        {
            // Fuente: https://stackoverflow.com/questions/17047057/calling-sql-defined-function-in-c-sharp
            bool exito = false;

            // Obtener el string de la conexion por medio de db.
            SqlConnection conexion = new SqlConnection();
            conexion.ConnectionString = db.Database.Connection.ConnectionString;

            // Comando para ejecutar la funcion, note que se usan los parametros brindados entre ' como siempre en SQL.
            SqlCommand instruccion = new SqlCommand("SELECT dbo.SF_LoginUsuario(@Correo, @Contrasenna)", conexion);

            instruccion.Parameters.Add("@Correo", SqlDbType.NVarChar);
            instruccion.Parameters.Add("@Contrasenna", SqlDbType.NVarChar);

            instruccion.Parameters["@Correo"].Value = form_collection["Correo"];
            instruccion.Parameters["@Contrasenna"].Value = form_collection["Contrasenna"];

            
            conexion.Open();                            // Abro conexion.       
            exito = (bool) instruccion.ExecuteScalar(); // Llamo a la funcion almacenada y obtengo el valor que devuelve.
            conexion.Close();                           // Cierro conexion.

            if(exito == true)
            {
                return "Login exitoso";
            }
            else
            {
                return "Login fallido";
            }
        }
    }
}