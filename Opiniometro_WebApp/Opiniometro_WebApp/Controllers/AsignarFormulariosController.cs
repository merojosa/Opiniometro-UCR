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
                //Ciclos = obtenerCiclos(),
                //Carreras = ...
                //Enfasis =
                //Cursos =

                //Grupos = ObtenerGrupos("", "", "", 255, "") //,

                //Formularios =

                //Asignaciones =
            };*/
            var modelo = new AsignarFormulariosModel { Cursos = cursos };
            return View(modelo);
        }

        // ### Métodos utilizados en el constructor ###
        // Ejemplo: 
        /* public IEnumerable<SelectListItem> ObtenerCiclos()
        {
            // Calcular u obtener los valores por mostrar
            return new List<SelectListItem>() { };
        } */
        // Los parámetros pueden ser usados para especificar qué elementos se deben enlistar.

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

        //Se obtienen las carreras de la tabla Carrera
        //public IEnumerable<SelectListItem> ObtenerCarreras(string codigoUnidad)
        //{
        //    return db.Carrera.Select(carrera => new SelectListItem { Value = carrera.Sigla.ToString(), Text = carrera.Nombre }).ToList();
        //}

        //Se obtienen los cursos de la tabla Curso
        //public IEnumerable<SelectListItem> ObtenerCursos(string codigoUnidad)
        //{
        //    return db.Curso.Select(curso => new SelectListItem { Value = curso.Sigla.ToString(), Text = curso.Nombre }).ToList();
        //}

        //Se despliegan los filtros (Unidad Academica, Carrera, Cursos)
        [HttpGet]
        public ActionResult Index()
        {
            List<SelectListItem> list = new List<SelectListItem>();

            list.Add(new SelectListItem() { Text = "I Ciclo 2019", Value = "1" });
            list.Add(new SelectListItem() { Text = "III Ciclo 2018", Value = "2" });
            list.Add(new SelectListItem() { Text = "II Ciclo 2018", Value = "3" });
            list.Add(new SelectListItem() { Text = "I Ciclo 2018", Value = "4" });

            ViewBag.Ciclo = list;

            Unidad_Academica unidad = db.Unidad_Academica.Find("UC-023874"); //"UC-023874"

            var carr = from s in db.Carrera
                       select s;
            ViewBag.Unidad = new SelectList(db.Unidad_Academica, "Codigo", "Nombre", unidad.Codigo);

            if (!String.IsNullOrEmpty(unidad.Codigo))
            {
                carr = carr.Where(s => s.CodigoUnidadAcademica.Equals(unidad.Codigo));
            }

            ViewBag.Carreras = new SelectList(carr, "Sigla", "Nombre", unidad.Codigo);

            var curso = from s in db.Curso
                        select s;

            if (!String.IsNullOrEmpty(unidad.Codigo))
            {
                curso = curso.Where(s => s.CodigoUnidad.Equals(unidad.Codigo));
            }
            ViewBag.Curso = new SelectList(curso, "Sigla", "Nombre");

            var modelo = new AsignarFormulariosModel { Curso = curso };
            return View(modelo);
        }

    }
}