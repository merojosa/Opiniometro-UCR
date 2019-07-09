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
                Secciones = obtenerPreguntasFormulario(cedulaEstudiante,codigoForm)
            };
            return View(modelo);
        }

        public SeccionFormulario[] obtenerPreguntasFormulario(string cedulaEstudiante, string codigoForm)
        {
            // Se recuperan las secciones del formulario con el código respectivo
            IQueryable < SeccionFormulario > seccionesQuery =
                from conf in db.Conformado_Item_Sec_Form
                where conf.CodigoFormulario == codigoForm
                select new SeccionFormulario
                {
                    Titulo = conf.TituloSeccion
                };

            // Se crea el arreglo de secciones que se asignarán al modelo
            SeccionFormulario[] secciones = new SeccionFormulario[0];
            if(seccionesQuery != null)
                secciones = seccionesQuery.Distinct().ToArray(); // Distinct: impedancia en la tabla Conf_Item_Sec_Form

            // Para cada sección se recuperan sus preguntas
            for (int seccion = 0; seccion < secciones.Count(); ++ seccion)
            {
                string titulo = secciones[seccion].Titulo;
                IQueryable<Pregunta> preguntasQuery =
                from it in db.Item
                join confSecItem in db.Conformado_Item_Sec_Form on it.ItemId equals confSecItem.ItemId
                join sec in db.Seccion on confSecItem.TituloSeccion equals titulo
                where (confSecItem.CodigoFormulario == codigoForm && confSecItem.TituloSeccion == titulo)
                select new Pregunta
                {
                    itemId = it.ItemId,
                    item = it.TextoPregunta,
                    tieneObservacion = it.TieneObservacion,
                    tipoPregunta = it.TipoPregunta
                };

                // Se crea el arreglo de preguntas que se asignarán a la sección y se asigna
                Pregunta[] preguntas = new Pregunta[0];
                if (preguntasQuery != null)
                    preguntas = preguntasQuery.Distinct().ToArray();
                secciones[seccion].PreguntasFormulario = preguntas;

                // Si le sección no tiene preguntas asignadas
                if (secciones[seccion].PreguntasFormulario != null)
                {
                    // Para cada pregunta se recuperan sus opciones, según su tipo
                    for (int pregunta = 0; pregunta < secciones[seccion].PreguntasFormulario.Count(); ++pregunta)
                    {
                        if (secciones[seccion].PreguntasFormulario[pregunta].tipoPregunta == 1)
                        {
                            // La pregunta de respuesta libre no tiene opciones, solo campo de texto
                            ;
                        }
                        else if (secciones[seccion].PreguntasFormulario[pregunta].tipoPregunta == 2)
                        {
                            string id = secciones[seccion].PreguntasFormulario[pregunta].itemId;
                            IQueryable<String> opciones = from ops in db.Opciones_De_Respuestas_Seleccion_Unica
                                                          where ops.ItemId == id
                                                          select ops.OpcionRespuesta;

                            // Se asigna el arreglo de opciones
                            secciones[seccion].PreguntasFormulario[pregunta].Opciones = opciones.ToArray();
                        }
                        else if (secciones[seccion].PreguntasFormulario[pregunta].tipoPregunta == 5)
                        {
                            string id = secciones[seccion].PreguntasFormulario[pregunta].itemId;
                            var ini = from range in db.Escalar
                                         where range.ItemId == id
                                         select range.Inicio;
                            var fin = from range in db.Escalar
                                         where range.ItemId == id
                                         select range.Fin;

                            int inicio = int.Parse(ini.ToString());
                            int final = int.Parse(fin.ToString());
                            int posicion = 0;
                            string[] rango = new string[final-inicio];
                            for (int valor = inicio; inicio >= final; valor++)
                            {
                                rango[posicion] = valor.ToString();
                                posicion++;
                            }
                            secciones[seccion].PreguntasFormulario[pregunta].Opciones = rango;
                        }


                            //#############################################################
                            // AQUI SEGUIR RECUPERANDO OPCIONES Y ASIGNARLAS A COVENIENCIA
                            //#############################################################
                        }
                    
                }
                
            }

            // Para debuggear
            /* 
             foreach(SeccionFormulario s in secciones)
             {
                 Debug.WriteLine(s.Titulo);
                 if(s.PreguntasFormulario != null)
                 {
                     foreach (Pregunta p in s.PreguntasFormulario)
                     {
                         Debug.WriteLine("\t" + p.item);

                         if (p.Opciones != null)
                         {
                             foreach (String o in p.Opciones)
                             {
                                 Debug.WriteLine("\t\t" + o);
                             }
                         }
                     }
                 }
             }*/

            return secciones;
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

        // Esto podria servir para empezar a romper el codigo (separar en metodos)
        public string ObtenerOpcionesSelUnica(string id)
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
            //SeleccionUnica selec = new SeleccionUnica { itemId = id, item = texto, tieneObservacion = observacion, tipoPregunta = tipo, Opciones = opciones };
            string[] op = opciones.ToArray();

            //return PartialView("SeleccionUnica", selec);
            var json = JsonConvert.SerializeObject(op);
            return json;
        }


        //public ActionResult ObtenerRangoEscalar(string id)
        //{  
        //    IEnumerable<String> rango = from range in db.Escalar
        //                                   where range.ItemId == id
        //                                   select range;

        //    return PartialView("Escalar", rango);
        //}
    }
}