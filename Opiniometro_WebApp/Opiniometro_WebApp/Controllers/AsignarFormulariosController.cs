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

        // GET: AsignarFormularios
        public ActionResult Index(string searchString)
        {
            var cursos = from m in db.Curso
                         select m;
            //searchString = "Programacion";
            if (!String.IsNullOrEmpty(searchString))
            {
                cursos = cursos.Where(s => s.Nombre.Contains(searchString));
            }

            /*var modelo = new AsignarFormulariosModel
            {
                Ciclos = ObtenerCiclos(""),
                Carreras = ObtenerCarreras(0, 0, ""),
                Enfasis = ObtenerEnfasis(0, 0, "", ""),
                Cursos = ObtenerCursos(0, 0, "", "", null, searchString),
                Grupos = ObtenerGrupos(0, 0, "", "", 255, "", searchString) //,
                //Formularios = 
                //Asignaciones = 
            };*/

            var modelo = new AsignarFormulariosModel { Cursos = ObtenerCursos(0, 0, "", "", null, searchString) };
            return View(modelo);
        }

        public IQueryable<Ciclo_Lectivo> ObtenerCiclos(String codigoUnidadAcadem)
        {
            return new List<Ciclo_Lectivo>().AsQueryable();
        }

        public IQueryable<Carrera> ObtenerCarreras(short anno, byte semestre, String codigoUnidadAcadem)
        {
            return new List<Carrera>().AsQueryable();
        }

        public IQueryable<Enfasis> ObtenerEnfasis(short anno, byte semestre, String codigoUnidadAcadem, String siglaCarrera)
        {
            return new List<Enfasis>().AsQueryable();
        }

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
        public IQueryable<Curso> ObtenerCursos(short anno, byte semestre, String codigoUnidadAcadem, String siglaCarrera, byte? numEnfasis,
                                                          String searchString)
        {
            IQueryable<Curso> cursos = from m in db.Curso
                                       select m;
            //searchString = "Programacion";
            if (!String.IsNullOrEmpty(searchString))
            {
                cursos = cursos.Where(s => s.Nombre.Contains(searchString));
            }

            return cursos;
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
        public IEnumerable<SelectListItem> ObtenerGrupos(String ciclo, String codigoUnidadAcadem, String siglaCarrera, byte? numEnfasis,
                                                         String siglaCurso)
        {
            return new List<SelectListItem>() { };
        }
        
    }
}