using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Opiniometro_WebApp.Models;
using System.Diagnostics;
using Newtonsoft.Json;
using Opiniometro_WebApp.Controllers.Servicios;

namespace Opiniometro_WebApp.Controllers
{
    [Authorize]
    public class FormularioCursoController: Controller
    {
        private Opiniometro_DatosEntities db = new Opiniometro_DatosEntities();

        [HttpGet]
        public ActionResult Index(string cedulaEstudiante, string codigoForm)
        {
            var modelo = new FormularioPorCurso
            {
                preguntasFormulario = obtenerPreguntasFormulario(cedulaEstudiante,codigoForm)
            };
            return View(modelo);
        }

        public IQueryable<Pregunta> obtenerPreguntasFormulario(string cedulaEstudiante, string codigoForm)
        {

            IQueryable<Pregunta> formulario =
                from it in db.Item
                join confSecItem in db.Conformado_Item_Sec_Form on it.ItemId equals confSecItem.ItemId
                join sec in db.Seccion on confSecItem.TituloSeccion equals sec.Titulo
                where (confSecItem.CodigoFormulario == codigoForm)

                select new Pregunta
                {
                    itemId = it.ItemId,
                    item = it.TextoPregunta,
                    tieneObservacion = it.TieneObservacion,
                    tipoPregunta = it.TipoPregunta
                };

            List<Pregunta> preguntas = new List<Pregunta>();
            foreach (Pregunta p in formulario)
            {
                string id = p.itemId;
                string texto = p.item;
                bool? observacion = p.tieneObservacion;
                int tipo = p.tipoPregunta;
                if (p.tipoPregunta == 1)
                    preguntas.Add(new TextoLibre { itemId = id, item = texto, tieneObservacion = observacion, tipoPregunta = tipo });
                else if (p.tipoPregunta == 2)
                {
                    IQueryable<String> opciones = from ops in db.Opciones_De_Respuestas_Seleccion_Unica
                                                  where ops.ItemId == id
                                                  select ops.OpcionRespuesta;
                    preguntas.Add(new SeleccionUnica{ itemId = id, item = texto, tieneObservacion = observacion, tipoPregunta = tipo,
                        Opciones = opciones.ToList() });
                }
            }

            return preguntas.AsQueryable();
        }

        /* Llamar con:
         
           $.post("GuardarRespuestas",
                {
                    PeriodosIndicados: JSON.stringify(cedEst),
                    CedulaProfesor: JSON.stringify(cedProf),
                    Grupo: ...
                },
                function (data, status) {
                    // qué hacer cuando termina 
                }
            );
         */
        public void GuardarRespuestas (string CedulaEstudiante, string CedulaProfesor, string Grupo, 
            string CodigoFormulario, string FechaRespuestas, string Respuestas)
        {
            var cedulaEst = JsonConvert.DeserializeObject<string>(CedulaEstudiante);
            var cedulaProf = JsonConvert.DeserializeObject<string>(CedulaProfesor);
            var grupoEval = JsonConvert.DeserializeObject<Grupo>(Grupo);
            var codigoF = JsonConvert.DeserializeObject<string>(CodigoFormulario);
            var fecha = JsonConvert.DeserializeObject<DateTime>(FechaRespuestas);
            var listaRespuestas = JsonConvert.DeserializeObject<RespuestaModel[]>(Respuestas);

            var tuplas = new List<Responde>();

            foreach (RespuestaModel respuesta in listaRespuestas)
            {
                foreach (var hileraRespuesta in respuesta.HilerasDeRespuesta)
                {
                    tuplas.Add( new Responde {
                            ItemId = respuesta.IdItem,
                            TituloSeccion = respuesta.TituloSeccion,
                            FechaRespuesta = fecha,
                            CodigoFormularioResp = codigoF,
                            CedulaPersona = cedulaEst,
                            CedulaProfesor = cedulaProf,
                            AnnoGrupoResp = grupoEval.AnnoGrupo,
                            SemestreGrupoResp = grupoEval.SemestreGrupo,
                            NumeroGrupoResp = grupoEval.Numero,
                            SiglaGrupoResp = grupoEval.SiglaCurso,
                            Observacion = respuesta.Observacion,
                            Respuesta = hileraRespuesta,
                            RespuestaProfesor = ""
                        });
                }
            }

            // tuplas contiene todas las tuplas por insertar a la base.
        }

        public ActionResult ObtenerOpcionesSelUnica(string id)
        {
            //Console.WriteLine(id);
            //List<SeleccionUnica> preguntas = new List<SeleccionUnica>();
        /*
            string id = p.itemId;
            string texto = p.item;
            bool? observacion = p.tieneObservacion;
            int tipo = p.tipoPregunta;*/
            IEnumerable<String> opciones = from ops in db.Opciones_De_Respuestas_Seleccion_Unica
                                            where ops.ItemId == id
                                            select ops.OpcionRespuesta;
            /*preguntas.Add(new SeleccionUnica
            {
                itemId = id,
                item = texto,
                tieneObservacion = obs,
                tipoPregunta = tipo,
                Opciones = opciones.ToList()
            });*/
            return PartialView("SeleccionUnica", opciones);
        }
    }
}