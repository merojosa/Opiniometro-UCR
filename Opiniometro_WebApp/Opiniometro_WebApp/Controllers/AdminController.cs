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


namespace Opiniometro_WebApp.Controllers
{
    public class AdminController : Controller
    {
        private Opiniometro_DatosEntities db = new Opiniometro_DatosEntities();

        public ActionResult VerPersonas()
        {
            return View();
        }

       /* public ActionResult VerPersonas(Persona persona)
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
            return PartialView(persona);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                //Populating a DataTable from database.
                DataTable dt = this.GetData();

                //Building an HTML string.
                StringBuilder html = new StringBuilder();

                //Table start.
                html.Append("<table border = '1'>");

                //Building the Header row.
                html.Append("<tr>");
                foreach (DataColumn column in dt.Columns)
                {
                    html.Append("<th>");
                    html.Append(column.ColumnName);
                    html.Append("</th>");
                }
                html.Append("</tr>");

                //Building the Data rows.
                foreach (DataRow row in dt.Rows)
                {
                    html.Append("<tr>");
                    foreach (DataColumn column in dt.Columns)
                    {
                        html.Append("<td>");
                        html.Append(row[column.ColumnName]);
                        html.Append("</td>");
                    }
                    html.Append("</tr>");
                }

                //Table end.
                html.Append("</table>");

                //Append the HTML string to Placeholder.
                CrearUsuario.Controls.Add(new Literal { Text = html.ToString() });
            }
        }*/

    

        public ActionResult CrearUsuario()
        {

            return PartialView();
        }

       /* public DataSet GetDataSet(string ConnectionString, string SQL)
        {
            
                        SqlDataAdapter da = new SqlDataAdapter();

                        cmd.CommandText = SQL;
                        da.SelectCommand = cmd;
                        DataSet ds = new DataSet();

                        conn.Open();
                        da.Fill(ds);
                        conn.Close();

                        return ds;
                    }
            DataTable table = new DataTable();
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["DB"].ConnectionString))
            using (var cmd = new SqlCommand("usp_GetABCD", con))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(table);
            }*/

        }
    }