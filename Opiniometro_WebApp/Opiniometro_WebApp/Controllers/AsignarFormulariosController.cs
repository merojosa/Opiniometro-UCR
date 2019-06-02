using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Opiniometro_WebApp.Models;

namespace Opiniometro_WebApp.Controllers
{

    public class AsignarFormulariosController : Controller
    {
        private Opiniometro_DatosEntities db = new Opiniometro_DatosEntities();

        // Para la vista completa
        public ActionResult Index(/*short anno, byte semestre, String codigoUnidadAcadem, 
            String siglaCarrera, byte? numEnfasis, String siglaCurso,*/ string searchString)
        {
            var modelo = new AsignarFormulariosModel
            {
                Ciclos = ObtenerCiclos(""),
                Unidad = ObtenerUnidadAcademica(0, 0, ""),
                Carreras = ObtenerCarreras(0, 0, ""),
                //Enfasis = ObtenerEnfasis(0, 0, "", ""),
                Cursos = ObtenerCursos(0, 0, "", "", null),
                Grupos = ObtenerGrupos(0, 0, "", "", 255, "", searchString),
                Formularios = ObtenerFormularios()
                //Asignaciones = 
            };

            return View(modelo);
        }

        // Para el filtro por ciclos
        public IQueryable<Ciclo_Lectivo> ObtenerCiclos(String codigoUnidadAcadem)
        {
            return new List<Ciclo_Lectivo>().AsQueryable();
        }

        //GET
        //Para el filtro por Unidad Academica
        public IQueryable<Unidad_Academica> ObtenerUnidadAcademica(short anno, byte semestre, String codigoUnidadAcadem)
        {
            IQueryable<Unidad_Academica> unidadAcademica = from u in db.Unidad_Academica
                                                           select u;
            ViewBag.UnidadAcademica = new SelectList(unidadAcademica, "Codigo", "Nombre");
            return unidadAcademica;
        }

        [HttpPost]
        public IQueryable<Unidad_Academica> ObtenerUnidadAcademica(String changeUnidad)
        {
            IQueryable<Unidad_Academica> unidadAcademica = from u in db.Unidad_Academica
                                                           select u;
            ViewBag.UnidadAcademica = new SelectList(unidadAcademica, "Codigo", "Nombre");
            return unidadAcademica;
        }

        //GET
        // Para el filtro por carreras
        public IQueryable<Carrera> ObtenerCarreras(short anno, byte semestre, String codigoUnidadAcadem)
        {
            IQueryable<Carrera> carreras = from car in db.Carrera
                                           select car;

            if (!String.IsNullOrEmpty(codigoUnidadAcadem))
            {
                carreras = carreras.Where(c => c.CodigoUnidadAcademica.Equals(codigoUnidadAcadem));
            }

            ViewBag.Carreras = new SelectList(carreras, "Sigla", "Nombre");
            return carreras;
        }

        [HttpPost]
        public IQueryable<Carrera> ObtenerCarreras(String changeUnidad)
        {
            IQueryable<Carrera> carreras = from car in db.Carrera
                                           select car;

            if (!String.IsNullOrEmpty(changeUnidad))
            {
                carreras = carreras.Where(c => c.CodigoUnidadAcademica.Equals(changeUnidad));
            }

            ViewBag.Carreras = new SelectList(carreras, "Sigla", "Nombre");
            return carreras;
        }

        //// Para el filtro por énfasis
        //public IQueryable<Enfasis> ObtenerEnfasis(short anno, byte semestre, String codigoUnidadAcadem, String siglaCarrera)
        //{
        //    return new List<Enfasis>().AsQueryable();
        //}

        // GET: 
        public IEnumerable<Curso> ObtenerCursos(short anno, byte semestre,
            String codigoUnidadAcadem, String siglaCarrera, byte? numEnfasis)
        {
            IQueryable<Curso> cursos = from cur in db.Curso 
                                       select cur; 

            if (!String.IsNullOrEmpty(siglaCarrera))
            {
                //Carrera carrera = db.Carrera.Find(siglaCarrera);
                cursos = cursos.Where(c => c.Enfasis.Equals(siglaCarrera));
            }
            ViewBag.Cursos = new SelectList(cursos, "Sigla", "Nombre");
            return cursos;
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

        /// <summary>
        /// Retorna la lista de grupos de cursos que se mostrarán.
        /// </summary>
        /// <param name="ciclo">Ciclo en los que se imparten los grupos.</param>
        /// <param name="codigoUnidadAcadem">Código de la unidad academica a la que pertenecen los cursos de los grupos.</param>
        /// <param name="siglaCarrera">Sigla de la carrera en la que se encuentran los cursos de los grupos.</param>
        /// <param name="numEnfasis">Número del énfasis de la carrera en el que se encuentran los cursos de los grupos.</param>
        /// <param name="siglaCurso">Sigla del curso al que pertenecen los grupos</param>
        /// <returns>Lista de los grupos que satisfacen los filtros utilizados como parámetros.</returns>
        public IEnumerable<GrupoConInfoExtra> ObtenerGrupos(short anno, byte semestre, String codigoUnidadAcadem,
            String siglaCarrera, byte? numEnfasis, String siglaCurso, String searchString)
        {
            IQueryable<GrupoConInfoExtra> grupos =
                from cur in db.Curso
                join gru in db.Grupo on cur.Sigla equals gru.SiglaCurso
                select new GrupoConInfoExtra
                {
                    siglaCurso = cur.Sigla,
                    numero = gru.Numero,
                    anno = gru.AnnoGrupo,
                    semestre = gru.SemestreGrupo,
                    nombreCurso = cur.Nombre,
                    codigoUnidad = cur.CodigoUnidad
                };

            if (!String.IsNullOrEmpty(searchString))
            {
                grupos = grupos.Where(c => c.nombreCurso.Contains(searchString));
            }
            return grupos;
        }

        //para la vista de los formularios
        public IEnumerable<Formulario> ObtenerFormularios()
        {
            return db.Formulario.ToList();
        }



    }
}