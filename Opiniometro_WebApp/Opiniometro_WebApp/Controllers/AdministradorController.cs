using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Opiniometro_WebApp.Models;
using System.IO;
using System.Threading.Tasks;
//Para ejecutar paquete de integración:
//using Microsoft.SqlServer.X-->Dts<--X.Runtime;
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
        public ActionResult CargarArchivo(HttpPostedFileBase archivoCargado)
        {
            if (archivoCargado != null)
            {
                string path = Server.MapPath("~/App_Data/ArchivosCargados/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                archivoCargado.SaveAs(path + Path.GetFileName(archivoCargado.FileName));
                ViewBag.Message = "File uploaded successfully.";
            }

            //Código para procesamiento de archivo por ssis


            //Invocar paquete de integración, pasar como parametro el nombre del archivo cargado
            Application app = new Application();
            Package package = null;
            try
            {
                string fileName = Server.MapPath(System.IO.Path.GetFileName(archivoCargado.FileName.ToString()));
                archivoCargado.SaveAs(fileName);

                //Load DTSX
                        //Ruta al paquete local
//                package = app.LoadPackage(@&quot; D:\SSIS_ASP_NET\SSIS_ASP_NET_DEMO\SSIS_ASP_NET_DEMO\Package1.dtsx & quot;, null);

                //Global Package Variable
                Variables vars = package.Variables;
                        //
 //               vars[&quot; Business_ID & quot;].Value = txtBusinessID.Text;
 //               vars[&quot; Business_Name & quot;].Value = txtBusinessName.Text;

                //Specify Excel Connection From DTSX Connection Manager
                //package.Connections[&quot; SourceConnectionExcel & quot;].ConnectionString =
       // &quot; provider = Microsoft.Jet.OLEDB.4.0; data source = &quot; +fileName + &quot; ; Extended Properties = Excel 8.0; &quot; ;

                //Execute DTSX.
                Microsoft.SqlServer.Dts.Runtime.DTSExecResult results = package.Execute();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                package.Dispose();
                package = null;
            }

            return View();
        }

    }
}
