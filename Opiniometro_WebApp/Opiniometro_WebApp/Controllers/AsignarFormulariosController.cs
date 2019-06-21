using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Opiniometro_WebApp.Models;
using System.Diagnostics;

namespace Opiniometro_WebApp.Controllers
{

    public class AsignarFormulariosController : Controller
    {
        private Opiniometro_DatosEntities db = new Opiniometro_DatosEntities();

        // Para la vista completa
        [HttpGet]
        public ActionResult Index()
        {
            var modelo = new AsignarFormulariosViewModel
            {
                Ciclos = ObtenerCiclos(""),
                UnidadesAcademicas = ObtenerUnidadAcademica(0, 0, ""),
                Enfasis = ObtenerEnfasis(0, 0, "", ""),
                Carreras = ObtenerCarreras(0, 0, "", ""),
                //Enfasis = ObtenerEnfasis(0, 0, "", ""),
                Cursos = ObtenerCursos(0, 0, "", "", null, "", ""),
                //Grupos = ObtenerGrupos(0, 0, "", "", "", "", 255, "", "" ,""),
                Formularios = ObtenerFormularios()
            };

            return View("Index", modelo);
        }

        [HttpPost]
        public ActionResult Index(string unidadAcademica, string nombreCarrera, string nombreCurso, string searchString, byte? semestre, short? ano)
        {
            var modelo = new AsignarFormulariosViewModel
            {
                Ciclos = ObtenerCiclos(""),
                UnidadesAcademicas = ObtenerUnidadAcademica(0, 0, ""),
                Carreras = ObtenerCarreras(0, 0, "", Request.Form["changeUnidad"]),
                Grupos = ObtenerGrupos(ano, semestre, unidadAcademica, "", "", nombreCarrera, 255, "", Request.Form["changeCurso"], searchString),
                Cursos = ObtenerCursos(ano, semestre, "", "", null, Request.Form["changeUnidad"], Request.Form["changeCarrera"]),
                Formularios = ObtenerFormularios(),
                Enfasis = ObtenerEnfasis(0,0, "", "")

            };

            return View(modelo);
        }

        [HttpPost]
        public ActionResult Asignar(GruposYFormsSeleccionados GruFormsSeleccionados)
        {
            if (GruFormsSeleccionados != null)
            {
                if (GruFormsSeleccionados.TieneQueAsignar())
                {
                    // Realizar asignaciones

                    if (ModelState.IsValid)
                    {
                        Debug.WriteLine("\n--- Asignaciones casi logradas ---\n");
                    }
                    else { Debug.WriteLine("\n--- Modelo invalido ---\n"); }
                }
                else { Debug.WriteLine("\n--- No hay asignaciones por hacer: {0} grupos y {1} formularios ---\n", 
                    GruFormsSeleccionados.GruposSeleccionados.Count(), GruFormsSeleccionados.FormulariosSeleccionados.Count()); }
            }
            else { Debug.WriteLine("\n--- No se retorna el modelo ---\n"); }
            return RedirectToAction("Index", "Home"); // Cambiar por ("Index", "AsignarPeriodosViewModel");
        }

        public ActionResult AsignacionFormularioGrupo (List<ElegirGrupoEditorViewModel> grupos, List<ElegirFormularioEditorViewModel> formularios)
        {
            GruposYFormsSeleccionados gruFormsSeleccionados;
            if (grupos != null && formularios != null)
            {
                /// Fecha por defecto usada para aplicar un formulario. (Desde hoy en 1 semana hasta dentro de 2 semanas)
                Fecha_Corte fechaPorDefecto = new Fecha_Corte { FechaInicio = DateTime.Now.AddDays(7), FechaFinal = DateTime.Now.AddDays(14) };

                foreach (var form in formularios)
                {
                    form.FechaDeCorte = fechaPorDefecto;
                }

                gruFormsSeleccionados
                    = new GruposYFormsSeleccionados(grupos, formularios);
            }
            else
            {
                gruFormsSeleccionados = new GruposYFormsSeleccionados();
            }
            return PartialView("AsignacionFormularioGrupo", gruFormsSeleccionados);
        }

        // Para el filtro por ciclos
        public IQueryable<Ciclo_Lectivo> ObtenerCiclos(String codigoUnidadAcadem)
        {
            IQueryable<Ciclo_Lectivo> ciclo = (from c in db.Ciclo_Lectivo select c).Distinct();
            ViewBag.semestre = new SelectList(ciclo, "Semestre", "Semestre");
            ViewBag.ano = new SelectList(ciclo, "Anno", "Anno");
            return ciclo;
        }

        public IQueryable<Unidad_Academica> ObtenerUnidadAcademica(short anno, byte semestre, String codigoUnidadAcadem)
        {
            IQueryable<Unidad_Academica> unidadAcademica = from u in db.Unidad_Academica select u;
            ViewBag.unidadAcademica = new SelectList(unidadAcademica, "Codigo", "Nombre");
            return unidadAcademica;
        }



        // Para el filtro por carreras
        public IQueryable<Carrera> ObtenerCarreras(short anno, byte semestre, String codigoUnidadAcadem, String changeUnidad)
        {

            IQueryable<Carrera> nombreCarrera = from car in db.Carrera select car;

            if (!String.IsNullOrEmpty(changeUnidad))
            {
                nombreCarrera = nombreCarrera.Where(c => c.CodigoUnidadAcademica.Equals(changeUnidad));
            }

            ViewBag.nombreCarrera = new SelectList(nombreCarrera, "Sigla", "Nombre");
            return nombreCarrera;
        }

        //public IQueryable<Profesor> ObtenerProfesores() {

        //    IQueryable<Profesor> profesores = from prof in db.Profesor select prof;
        //        return profesores;
        //}


        // Para el filtro por énfasis
        public IQueryable<Enfasis> ObtenerEnfasis(short anno, byte semestre, String codigoUnidadAcadem, String siglaCarrera)
        {
            IQueryable<Enfasis> enfasis = from u in db.Enfasis select u;
            ViewBag.enfasis = new SelectList(enfasis, "Numero", "SiglaCarrera");
            return enfasis;
        }

        // Para el filtro por cursos
        /// <summary>
        /// Retorna la lista de cursos que pueden ser elegido en el filtro de curso.
        /// </summary>
        /// <param name="anno">Año del oiclo en los que se imparten los cursos.</param>
        /// <param name="semestre">Semestre del ciclo en el que se imparten los cursos.</param>
        /// <param name="codigoUnidadAcadem">Código de la unidad academica a la que pertenecen los cursos.</param>
        /// <param name="siglaCarrera">Sigla de la carrera en la que se encuentran los cursos.</param>
        /// <param name="numEnfasis">Número del énfasis de la carrera en el que se encuentran los cursos.</param>
        /// <param name="searchString">Frase usada para buscar el nombre o código de un grupo.</param>
        /// <returns>Lista de los cursos que satisfacen los filtros utilizados como parámetros.</returns>
        public IQueryable<Curso> ObtenerCursos(short? anno, byte? semestre,
            String codigoUnidadAcadem, String siglaCarrera, byte? numEnfasis, string changeUnidad, string changeCarrera)
        {
            IQueryable<Curso> nombreCurso = from c in db.Curso
                                            select c;

            if (!String.IsNullOrEmpty(changeUnidad))
            {
                nombreCurso = nombreCurso.Where(c => c.CodigoUnidad.Equals(changeUnidad));
            }

            if (!String.IsNullOrEmpty(changeCarrera))
            {
                List<Curso> cursos = new List<Curso>();
                Opiniometro_DatosEntities opi = new Opiniometro_DatosEntities();
                var cur = opi.CursosSegunCarrera(changeCarrera);
                foreach (var c in cur)
                {
                    Curso nuevo = new Curso
                    {
                        CodigoUnidad = c.CodigoUnidad,
                        Tipo = c.Tipo,
                        Sigla = c.Sigla,
                        Nombre = c.Nombre
                    };
                    cursos.Add(nuevo);
                }

                nombreCurso = cursos.AsQueryable();
            }

            ViewBag.nombreCurso = new SelectList(nombreCurso, "Nombre", "Nombre");
            return nombreCurso;
        }

        /// <summary>
        /// Retorna la lista de grupos de cursos que se mostrarán.
        /// </summary>
        /// <param name="ciclo">Ciclo en los que se imparten los grupos.</param>
        /// <param name="codigoUnidadAcadem">Código de la unidad academica a la que pertenecen los cursos de los grupos.</param>
        /// <param name="siglaCarrera">Sigla de la carrera en la que se encuentran los cursos de los grupos.</param>
        /// <param name="numEnfasis">Número del énfasis de la carrera en el que se encuentran los cursos de los grupos.</param>
        /// <param name="siglaCurso">Sigla del curso al que pertenecen los grupos</param>
        /// <returns>Lista de los grupos que satisfacen los filtros utilizados como parámetros.</returns>
        public List<ElegirGrupoEditorViewModel> ObtenerGrupos(short? anno, byte? semestre, string codigoUnidadAcadem,
             string nomUnidadAcad, string siglaCarrera, string nombCarrera, byte? numEnfasis, string siglaCurso, string nombreCurso, string searchString)
        {
            // En la base, cuando este query se transforme en un proc. almacenado, se deberá usar join con la tabla curso
            IQueryable<ElegirGrupoEditorViewModel> grupos =
                (from cur in db.Curso
                join gru in db.Grupo on cur.Sigla equals gru.SiglaCurso
                join uni in db.Unidad_Academica on cur.CodigoUnidad equals uni.Codigo
                join car in db.Carrera on uni.Codigo equals car.CodigoUnidadAcademica

            select new ElegirGrupoEditorViewModel
            {
                Seleccionado = false,
                SiglaCurso = cur.Sigla,
                Numero = gru.Numero,
                Anno = gru.AnnoGrupo,
                Semestre = gru.SemestreGrupo,
                //Profesores = gru.Profesor.ToList(),
                NombreCurso = gru.Curso.Nombre,
                NombreUnidadAcademica = gru.Curso.Unidad_Academica.Nombre,
                CodigoUnidadAcademica = cur.CodigoUnidad,
                SiglaCarrera = car.Sigla
                //NombresCarreras =  cur.Enfasis.
            });

            grupos = FiltreGrupos(searchString, semestre, anno, codigoUnidadAcadem, siglaCarrera, nombreCurso, grupos);

            return grupos.ToList();

        }

        /// <summary>
        /// filtra la lista de grupos
        /// </summary>
        /// <param name="searchString"> string que podria contener el nombre del curso que se ingreso para buscar</param>
        /// <param name="semestre"> semestre podria contener el semestre que se indico en el filtro </param>
        /// <param name="codigoUnidadAcad"> podria contener el nombre de la unidad academica</param>
        /// <param name="codigoCarrera"> podria contener el nombre de la carrera</param>
        /// <param name="grupos"> lista de grupos que se envia desde el metodo ObtenerGrupos</param>
        /// <returns> los grupos filtrados</returns>
        public IQueryable<ElegirGrupoEditorViewModel> FiltreGrupos(string searchString, byte? semestre, short? anno, string CodigoUnidadAcad, string siglaCarrera, string nombCurso ,IQueryable<ElegirGrupoEditorViewModel> grupos)
        {
            if (!String.IsNullOrEmpty(searchString))
            {
                grupos = grupos.Where(c => c.NombreCurso.Contains(searchString));
            }

            if (!String.IsNullOrEmpty(CodigoUnidadAcad))
            {
                grupos = grupos.Where(c => c.CodigoUnidadAcademica.Equals(CodigoUnidadAcad));
            }

            if (!String.IsNullOrEmpty(siglaCarrera))
            {
                grupos = grupos.Where(c => c.SiglaCarrera.Equals(siglaCarrera));
            }

            //if (!String.IsNullOrEmpty(nombCarrera))
            //{
            //    var carrera = (from c in db.Carrera
            //                   select new { Sigla = c.Sigla, Nombre = c.Nombre }
            //                    ).Where(c => c.Nombre == nombCarrera);
            //    string SiglaCarr = carrera.First().Sigla;
            //}

            if (!String.IsNullOrEmpty(nombCurso))
            {
                grupos = grupos.Where(c => c.NombreCurso.Contains(nombCurso));
            }

            if (semestre != null)
            {
                grupos = grupos.Where(c => c.Semestre == semestre);
            }

            if (anno != null)
            {
                grupos = grupos.Where(c => c.Anno == anno);
            }

            return grupos;
        }

        // Para la vista de los formularios
        public List<ElegirFormularioEditorViewModel> ObtenerFormularios()
        {
            List<ElegirFormularioEditorViewModel> formularios =
                (from formul in db.Formulario
                select new ElegirFormularioEditorViewModel
                {
                    Seleccionado = false,
                    CodigoFormulario = formul.CodigoFormulario,
                    NombreFormulario = formul.Nombre,
                    FechaDeCorte = null
                }).ToList();

            return formularios;
        }

        public ActionResult SeleccionFormularios(string formulario)
        {
            IQueryable<Formulario> form = from f in db.Formulario select f;

            if (!String.IsNullOrEmpty(formulario))
            {
                form = form.Where(f => f.Nombre.Contains(formulario));
            }

            return PartialView("SeleccionFormularios", form);
        }
    }
}