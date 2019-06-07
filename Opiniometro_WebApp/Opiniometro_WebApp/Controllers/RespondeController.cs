
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

        //GET: Responde
        public ActionResult Index()
        {
            var responde = db.Responde.Include(r => r.Formulario_Respuesta).Include(r => r.Item).Include(r => r.Seccion);
            return View(responde.ToList());
        }

        //public void ObtenerSeccionesFormulario(string CodigoFormulario)
        //{
        //    System.Data.Entity.Core.Objects.ObjectResult<string> Secciones = db.Obtener_Secciones_Por_Formulario(CodigoFormulario);
        //}

        //GET: Responde/Details/5
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

        //EFE: Devuelve un Int con la cantidad de respuestas por respuesta.
        //REQ: Que exista la conexion a la base de datos.
        //MOD:--
        [HttpGet]
        private ObjectResult<SP_ContarRespuestasPorGrupo_Result> ObtenerCantidadRespuestasPorPregunta(string codigoFormulario, string cedulaProfesor, short annoGrupo, byte semestreGrupo, byte numeroGrupo, string siglaCurso, string itemId)
        {
            var result = db.SP_ContarRespuestasPorGrupo(codigoFormulario, cedulaProfesor, annoGrupo, semestreGrupo, numeroGrupo, siglaCurso, itemId);
            return result;
        }

        //EFE:Crea un gráfico con la información de los resultados en la base de datos.
        //REQ:Que exista una conexion a la base de datos.
        //MOD:--
        public ActionResult GraficoPie(string itemId)
        {

            var result = ObtenerCantidadRespuestasPorPregunta("131313", "100000002", 2017, 2, 1, "CI1330", itemId);
            int tamanio = result.Count();
            string[] leyenda = new string[tamanio];
            int?[] cntResps = new int?[tamanio];
            int iter = 0;
            foreach(var item in result.ToList())
            {
                leyenda[iter] = item.Respuesta;
                cntResps[iter] = item.cntResp;
                iter++;
            }
            string myGraf =
                @"<Chart BackColor=""Transparent"" >
                                <ChartAreas>
                                    <ChartArea Name=""Default"" BackColor=""Transparent""></ChartArea>
                                </ChartAreas>
                            </Chart>";
            new Chart(width: 350, height: 350, theme: myGraf)
                .AddSeries(
                    chartType: "pie",
                    xValue: leyenda,
                    yValues: cntResps)
                .Write("png");
            result.Dispose();
            return null;
        }

        [HttpGet]
        private ObjectResult ObtenerObservaciones(int cursoId)
        {
            //Parametro para que se guarde el resultado
            //ObjectParameter resultado = new ObjectParameter("resultado", typeof(Double));

            //Invoca el metodo
            //db.ProcObtenerPromedioCurso(cursoId, resultado);
            var result = db.SP_DevolverObservacionesPorGrupo("131313", "100000002", 2017, 2, 1, "CI1330", "1");
            foreach (var valor in result)
            {
                Console.WriteLine("1. " + valor.ToString());
            }
            //Devuelve el resultado
            return (result);

        }

        private ActionResult RecComentarios()
        {
            return View(ObtenerObservaciones(1));
        }

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
