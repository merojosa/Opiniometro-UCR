using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Core.Objects;
using Opiniometro_WebApp.Models;
using System.Data.SqlClient;
using System.Web.Helpers;

namespace Opiniometro_WebApp.Controllers
{
    public class RespondeController : Controller
    {
        private Opiniometro_DatosEntities db = new Opiniometro_DatosEntities();

        // GET: Responde
        public ActionResult Index()
        {
            var responde = db.Responde.Include(r => r.Formulario_Respuesta).Include(r => r.Item).Include(r => r.Seccion);
            return View(responde.ToList());
        }

        //public void ObtenerSeccionesFormulario(string CodigoFormulario)
        //{
        //    System.Data.Entity.Core.Objects.ObjectResult<string> Secciones = db.Obtener_Secciones_Por_Formulario(CodigoFormulario);
        //}

        // GET: Responde/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Responde responde = db.Responde.Find(id);
            if (responde == null)
            {
                return HttpNotFound();
            }
            return View(responde);
        }

        [HttpGet]
        private int ObtenerCantidadRespuestasPorPregunta(string codigoFormulario, string cedulaProfesor, short annoGrupo, byte semestreGrupo, byte numeroGrupo, string siglaCurso, int itemId, string respuesta)
        {
            ObjectParameter cntRespuestas = new ObjectParameter("cntResp", typeof(int));
            db.SP_ContarRespuestasPorGrupo(codigoFormulario, cedulaProfesor, annoGrupo, semestreGrupo, numeroGrupo, siglaCurso, itemId, respuesta, cntRespuestas);
            if (DBNull.Value.Equals(cntRespuestas.Value))
                return 0;
            return Convert.ToInt32(cntRespuestas.Value);
        }

        public ActionResult GraficoPie()
        {
            decimal x = ObtenerCantidadRespuestasPorPregunta("131313", "100000002", 2017, 2, 1, "CI1330", 1, "1");
            decimal y = ObtenerCantidadRespuestasPorPregunta("131313", "100000002", 2017, 2, 1, "CI1330", 1, "2");
            decimal z = ObtenerCantidadRespuestasPorPregunta("131313", "100000002", 2017, 2, 1, "CI1330", 1, "3");

            string myGraf = 
                @"<Chart BackColor=""Transparent"" >
                                <ChartAreas>
                                    <ChartArea Name=""Default"" BackColor=""Transparent""></ChartArea>
                                </ChartAreas>
                            </Chart>";
            new Chart(width: 350, height: 350, theme: myGraf)
                .AddSeries(
                    chartType: "pie",
                    xValue: new[] { "Sí", "No", "No Se/No Aplica" },
                    yValues: new[] { x, y, z })
                .Write("png");
            return null;
        }

        //[HttpGet]
        //private int ObtenerObservaciones(int cursoId)
        //{
        //    //Parametro para que se guarde el resultado             
        //    ObjectParameter resultado = new ObjectParameter("resultado", typeof(Double));

        //    //Invoca el metodo            
        //    db.ProcObtenerPromedioCurso(cursoId, resultado);

        //    //Devuelve el resultado            
        //    return Convert.ToDouble(resultado.Value);

        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
