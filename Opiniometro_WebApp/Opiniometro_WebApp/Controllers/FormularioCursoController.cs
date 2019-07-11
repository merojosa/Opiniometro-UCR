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
        private Opiniometro_DatosEntities db;

        public FormularioCursoController()
        {
            this.db = new Opiniometro_DatosEntities();
        }
        public FormularioCursoController(Opiniometro_DatosEntities db)
        {
            this.db = db;
        }

        [HttpGet]
        public ActionResult Index( string cedulaProfesor, string cedulaEstudiante, string codigoForm, short anno, int semestre, string siglaCurso, byte numGrupo)
        {
            var modelo = new FormularioPorCurso
            {
                cedProf = cedulaProfesor,
                cedEst = cedulaEstudiante,
                codFormulario = codigoForm,
                anoGrupo = anno,
                semestreGrupo = semestre,
                siglaCurso = siglaCurso,
                numGrupo = numGrupo,
                Secciones = obtenerPreguntasFormulario(codigoForm)
            };
            return View("Index",modelo);
        }

        public SeccionFormulario[] obtenerPreguntasFormulario(string codigoForm)
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

            int preguntaActual = 1;
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

                // Si le sección tiene preguntas asignadas
                if (secciones[seccion].PreguntasFormulario != null)
                {
                    // Para cada pregunta se recuperan sus opciones, según su tipo
                    for (int pregunta = 0; pregunta < secciones[seccion].PreguntasFormulario.Count(); ++pregunta)
                    {
                        secciones[seccion].PreguntasFormulario[pregunta].numPregunta = preguntaActual;
                        ++preguntaActual;
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

                        else if (secciones[seccion].PreguntasFormulario[pregunta].tipoPregunta == 3)
                        {
                            string id = secciones[seccion].PreguntasFormulario[pregunta].itemId;
                            IQueryable<String> opciones = from ops in db.Opciones_De_Respuestas_Seleccion_Unica
                                                          where ops.ItemId == id
                                                          select ops.OpcionRespuesta;

                            // Se asigna el arreglo de opciones
                            secciones[seccion].PreguntasFormulario[pregunta].Opciones = opciones.ToArray();
                        }

                        else if (secciones[seccion].PreguntasFormulario[pregunta].tipoPregunta == 4)
                        {
                            string id = secciones[seccion].PreguntasFormulario[pregunta].itemId;
                            IQueryable<String> opciones = from ops in db.Opciones_De_Respuestas_Seleccion_Multiple
                                                          where ops.ItemId == id
                                                          select ops.OpcionRespuesta;

                            // Se asigna el arreglo de opciones
                            secciones[seccion].PreguntasFormulario[pregunta].Opciones = opciones.ToArray();
                        }

                        else if (secciones[seccion].PreguntasFormulario[pregunta].tipoPregunta == 5 ||
                           secciones[seccion].PreguntasFormulario[pregunta].tipoPregunta == 6)
                        {
                            string id = secciones[seccion].PreguntasFormulario[pregunta].itemId;
                            var ini = (from range in db.Escalar
                                       where range.ItemId == id
                                       select range.Inicio).First();
                            var fin = (from range in db.Escalar
                                       where range.ItemId == id
                                       select range.Fin).First();

                            int inicio = Convert.ToInt32(ini);
                            int final = Convert.ToInt32(fin);
                            int valor = inicio;
                            int posicion = 0;

                            string[] rango = new string[(final - inicio) + 1];

                            if(inicio == 0)
                            {
                                rango = new string[(final - inicio)];
                            }
                            foreach (string r in rango)
                            {
                                rango[posicion] = valor.ToString();
                                valor++;
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

        public void GuardarRespuestas (string CedulaEstudiante, string CedulaProfesor, string Grupo, string CodigoFormulario, string Respuestas)
        {
            //Debug.WriteLine("\n\nGrupo: \"" + Grupo + "\"\n\n");
            var cedulaEst = JsonConvert.DeserializeObject<string>(CedulaEstudiante);
            var cedulaProf = JsonConvert.DeserializeObject<string>(CedulaProfesor);
            var grupoParcial = JsonConvert.DeserializeAnonymousType(Grupo, new { Anno = "", Semestre = "", SiglaCurso = "", NumeroGrupo = ""});
            var grupoEval = new Grupo
            {
                AnnoGrupo = Convert.ToInt16(grupoParcial.Anno),
                SemestreGrupo = Convert.ToByte(grupoParcial.Semestre),
                SiglaCurso = grupoParcial.SiglaCurso,
                Numero = Convert.ToByte(grupoParcial.NumeroGrupo)
            };
            var codigoF = JsonConvert.DeserializeObject<string>(CodigoFormulario);
            var fecha = DateTime.Now;
            var listaRespuestas = JsonConvert.DeserializeObject<RespuestaModel[]>(Respuestas);

            // tuplas contiene todas las tuplas por insertar a la base.
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
                            RespuestaProfesor = null
                        });
                }
            }

            var formResp = new Formulario_Respuesta
            {
                Fecha = fecha,
                CodigoFormulario = codigoF,
                CedulaPersona = cedulaEst,
                CedulaProfesor = cedulaProf,
                AnnoGrupo = grupoEval.AnnoGrupo,
                SemestreGrupo = grupoEval.SemestreGrupo,
                NumeroGrupo = grupoEval.Numero,
                SiglaGrupo = grupoEval.SiglaCurso,
                Completado = true
            };

            if (ModelState.IsValid)
            {
                db.Formulario_Respuesta.Add(formResp);
                db.Responde.AddRange(tuplas.AsEnumerable());
                db.SaveChanges();
            }
        }

        // Esto podria servir para empezar a romper el codigo (separar en metodos)
        public string ObtenerOpcionesSelUnica(string id)
        {
            IEnumerable<String> opciones = from ops in db.Opciones_De_Respuestas_Seleccion_Unica
                                            where ops.ItemId == id
                                            select ops.OpcionRespuesta;
            string[] op = opciones.ToArray();

            var json = JsonConvert.SerializeObject(op);
            return json;
        }
    }
}