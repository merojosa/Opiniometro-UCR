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
        [HttpGet]
        public ActionResult Index()
        {
            var modelo = new AsignarFormulariosModel
            {
                Ciclos = ObtenerCiclos(""),
                Unidad = ObtenerUnidadAcademica(0, 0, ""),
                Carreras = ObtenerCarreras(0, 0, "", ""),
                //Enfasis = ObtenerEnfasis(0, 0, "", ""),
                Cursos = ObtenerCursos(0, 0, "", "", null, "", ""),
                Grupos = ObtenerGrupos(0, 0, "","", "", "", 255, "", "" ,""),
                Formularios = ObtenerFormularios()
            };

            return View(modelo);
        }

        [HttpPost]
        public ActionResult Index(string unidadAcademica, string nombreCarrera, string nombreCurso, string searchString)
        {
            var modelo = new AsignarFormulariosModel
            {
                Unidad = ObtenerUnidadAcademica(0, 0, ""),
                Carreras = ObtenerCarreras(0, 0, "", Request.Form["changeUnidad"]),
                Grupos = ObtenerGrupos(0, 0, "", unidadAcademica, "", nombreCarrera, 255, "",nombreCurso, searchString),
                Cursos = ObtenerCursos(0, 0, "", "", null, Request.Form["changeUnidad"], Request.Form["changeCarrera"]),
                Formularios = ObtenerFormularios()

            };
            return View(modelo);
        }

        // Para el filtro por ciclos
        public IQueryable<Ciclo_Lectivo> ObtenerCiclos(String codigoUnidadAcadem)
        {
            IQueryable<Ciclo_Lectivo> ciclo = (from c in db.Ciclo_Lectivo select c);
            //ViewBag.semestre = new SelectList(ciclo, "Semestre", "Semestre");
           // ViewBag.ano = new SelectList(ciclo, "Anno", "Anno");
            return ciclo;
        }

        // Para el filtro por Unidad Academica
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


        // Para el filtro por énfasis
        //public IQueryable<Enfasis> ObtenerEnfasis(short anno, byte semestre, String codigoUnidadAcadem, String siglaCarrera)
        //{
        //    return new List<Enfasis>().AsQueryable();
        //}


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
        public IQueryable<Curso> ObtenerCursos(short anno, byte semestre,
            String codigoUnidadAcadem, String siglaCarrera, byte? numEnfasis, string changeUnidad, string changeCarrera)
        {
            IQueryable<Curso> nombreCurso = from cur in db.Curso select cur;

            if (!String.IsNullOrEmpty(changeUnidad))
            {
                nombreCurso = nombreCurso.Where(c => c.CodigoUnidad.Equals(changeUnidad));
            }

            if (!String.IsNullOrEmpty(changeCarrera))
            {
               // nombreCurso = nombreCurso.Where(c => c.Enfasis.Codigo.Equals(changeCarrera));
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
        public IEnumerable<GrupoConInfoExtra> ObtenerGrupos(short anno, byte semestre, String codigoUnidadAcadem,
             string nomUnidadAcad, String siglaCarrera, String nombCarrera, byte? numEnfasis, String siglaCurso, string nombreCurso, String searchString)
        {
            IQueryable<GrupoConInfoExtra> grupos =
                 from cur in db.Curso
                 join gru in db.Grupo on cur.Sigla equals gru.SiglaCurso
                 join uni in db.Unidad_Academica on cur.CodigoUnidad equals uni.Codigo
                 join car in db.Carrera on uni.Codigo equals car.CodigoUnidadAcademica
                 select new GrupoConInfoExtra
                {
                    siglaCurso = cur.Sigla,
                    numero = gru.Numero,
                    anno = gru.AnnoGrupo,
                    semestre = gru.SemestreGrupo,
                    nombreCurso = cur.Nombre,
                    codigoUnidad = cur.CodigoUnidad,
                    nombreUnidadAcademica = uni.Nombre,
                    carrera = car.Nombre
                };

            grupos = filtreGrupos(searchString, semestre, nomUnidadAcad, nombCarrera, nombreCurso, grupos);

            return grupos;

        }

        /// <summary>
        /// filtra la lista de grupos
        /// </summary>
        /// <param name="searchString"> string que podria contener el nombre del curso que se ingreso para buscar</param>
        /// <param name="semestre"> semestre podria contener el semestre que se indico en el filtro </param>
        /// <param name="nomUnidadAcad"> podria contener el nombre de la unidad academica</param>
        /// <param name="nombCarrera"> podria contener el nombre de la carrera</param>
        /// <param name="grupos"> lista de grupos que se envia desde el metodo ObtenerGrupos</param>
        /// <returns> los grupos filtrados</returns>
        public IQueryable<GrupoConInfoExtra> filtreGrupos(string searchString, byte semestre, string nomUnidadAcad, string nombCarrera, string nombCurso ,IQueryable<GrupoConInfoExtra> grupos)
        {
            if (!String.IsNullOrEmpty(searchString))
            {
                grupos = grupos.Where(c => c.nombreCurso.Contains(searchString));
            }

            if (!String.IsNullOrEmpty(nomUnidadAcad))
            {
                grupos = grupos.Where(c => c.nombreUnidadAcademica.Contains(nomUnidadAcad));
            }

            if (!String.IsNullOrEmpty(nombCarrera))
            {
                grupos = grupos.Where(c => c.carrera.Contains(nombCarrera));
            }

            if (!String.IsNullOrEmpty(nombCurso))
            {
                grupos = grupos.Where(c => c.nombreCurso.Contains(nombCurso));
            }

            //if (semestre != null){
            //    grupos = grupos.Where(c => c.semestre == semestre);
            //}

            return grupos;
        }


        //para la vista de los formularios
        public IEnumerable<Formulario> ObtenerFormularios()
        {
            return db.Formulario.ToList();
        }



    }
}