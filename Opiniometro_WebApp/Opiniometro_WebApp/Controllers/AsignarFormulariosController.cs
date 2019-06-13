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
                Carreras = ObtenerCarreras(0, 0, ""),
                //Enfasis = ObtenerEnfasis(0, 0, "", ""),
                Cursos = ObtenerCursos(0, 0, "", "", null),
                Grupos = ObtenerGrupos(0, 0, "", "", "", "", 255, "", "", ""),
                Formularios = ObtenerFormularios("")
            };

            return View("Index", modelo);
        }

        [HttpPost]
        public ActionResult Index(string unidadAcademica, string nombreCarrera, string nombreCurso, string searchString)
        {
            var modelo = new AsignarFormulariosViewModel
            {
                UnidadesAcademicas = ObtenerUnidadAcademica(0, 0, ""),
                Carreras = ObtenerCarreras(0, 0, ""),
                Grupos = ObtenerGrupos(0, 0, "", unidadAcademica, "", nombreCarrera, 255, "", nombreCurso, searchString),
                Cursos = ObtenerCursos(0, 0, "", "", null),
                Formularios = ObtenerFormularios("")

            };
            return View(modelo);
        }
        
        public void Asignar(GruposYFormsSeleccionados gruFormsSeleccionados)
        {
            /*System.Diagnostics.Debug.WriteLine("\n\nAsignar()\n");

            var GruposSeleccionados = modelo.ActualizarGruposSeleccionados();
            var FormulariosSeleccionados = modelo.ActualizarFormulariosSeleccionados();

            System.Diagnostics.Debug.WriteLine("{0} grupos seleccionados:", GruposSeleccionados.Count());
            foreach (var grupo in GruposSeleccionados)
            {
                System.Diagnostics.Debug.WriteLine("{0}-{1}: {2}, #{3}", grupo.Semestre, grupo.Anno, grupo.SiglaCurso, grupo.Numero);
            }

            System.Diagnostics.Debug.WriteLine("\n{0} formularios seleccionados:", FormulariosSeleccionados.Count());
            foreach (var formulario in FormulariosSeleccionados)
            {
                System.Diagnostics.Debug.WriteLine("{0} - {1}", formulario.CodigoFormulario, formulario.NombreFormulario);
            }

            System.Diagnostics.Debug.WriteLine("\nFin del reporte.");*/

            //System.Diagnostics.Debug.WriteLine();
        }

        public ActionResult AsignacionFormularioGrupo (List<ElegirGrupoEditorViewModel> grupos, List<ElegirFormularioEditorViewModel> formularios)
        {
            GruposYFormsSeleccionados gruFormsSeleccionados
                = new GruposYFormsSeleccionados(grupos, formularios);
            Asignar(gruFormsSeleccionados);
            return PartialView("AsignacionFormularioGrupo", gruFormsSeleccionados);
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
            ViewBag.unidadAcademica = new SelectList(unidadAcademica, "Nombre", "Nombre");
            return unidadAcademica;
        }

        // Para el filtro por carreras
        public IQueryable<Carrera> ObtenerCarreras(short anno, byte semestre, String codigoUnidadAcadem)
        {

            IQueryable<Carrera> nombreCarrera = from car in db.Carrera select car;

            if (!String.IsNullOrEmpty(codigoUnidadAcadem))
            {
                nombreCarrera = nombreCarrera.Where(c => c.CodigoUnidadAcademica.Equals(codigoUnidadAcadem));
            }

            ViewBag.nombreCarrera = new SelectList(nombreCarrera, "Nombre", "Nombre");
            return nombreCarrera;
        }


        // Para el filtro por énfasis
        public IQueryable<Enfasis> ObtenerEnfasis(short anno, byte semestre, String codigoUnidadAcadem, String siglaCarrera)
        {
            return new List<Enfasis>().AsQueryable();
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
        public IQueryable<Curso> ObtenerCursos(short anno, byte semestre,
            String codigoUnidadAcadem, String siglaCarrera, byte? numEnfasis)
        {
            IQueryable<Curso> nombreCurso = from cur in db.Curso select cur;
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
        public List<ElegirGrupoEditorViewModel> ObtenerGrupos(short anno, byte semestre, String codigoUnidadAcadem,
             string nomUnidadAcad, String siglaCarrera, String nombCarrera, byte? numEnfasis, String siglaCurso, string nombreCurso, String searchString)
        {
            // En la base, cuando este query se transforme en un proc. almacenado, se deberá usar join con la tabla curso
            List<ElegirGrupoEditorViewModel> grupos =
                (from cur in db.Curso
                join gru in db.Grupo on cur.Sigla equals gru.SiglaCurso
                join uni in db.Unidad_Academica on cur.CodigoUnidad equals uni.Codigo
                /*join imp in db.Imparte on 
                    new { anno = gru.AnnoGrupo, sem = gru.SemestreGrupo, sigla = gru.SiglaCurso, num = gru.Numero }
                    equals new { anno = imp.Anno, sem = imp.Semestre, sigla = imp.Sigla, num = imp.Numero }*/
                 select new ElegirGrupoEditorViewModel
            {
                Seleccionado = false,
                SiglaCurso = cur.Sigla,
                Numero = gru.Numero,
                Anno = gru.AnnoGrupo,
                Semestre = gru.SemestreGrupo,
                Profesores = gru.Profesor.ToList(),
                NombreCurso = gru.Curso.Nombre,
                NombreUnidadAcademica = gru.Curso.Unidad_Academica.Nombre,
                //NombresCarreras =  cur.Enfasis.
            }).ToList();

            grupos = FiltreGrupos(searchString, semestre, nomUnidadAcad, nombCarrera, nombreCurso, grupos.AsQueryable()).ToList();

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
        public IQueryable<ElegirGrupoEditorViewModel> FiltreGrupos(string searchString, byte semestre, string nomUnidadAcad, string nombCarrera, string nombCurso ,IQueryable<ElegirGrupoEditorViewModel> grupos)
        {
            if (!String.IsNullOrEmpty(searchString))
            {
                grupos = grupos.Where(c => c.NombreCurso.Contains(searchString));
            }

            if (!String.IsNullOrEmpty(nomUnidadAcad))
            {
                grupos = grupos.Where(c => c.NombreUnidadAcademica.Contains(nomUnidadAcad));
            }

            /*if (!String.IsNullOrEmpty(nombCarrera))
            {
                grupos = grupos.Where(c => c.NombreCarrera.Contains(nombCarrera));
            }*/

            if (!String.IsNullOrEmpty(nombCurso))
            {
                grupos = grupos.Where(c => c.NombreCurso.Contains(nombCurso));
            }

            //if (semestre != null){
            //    grupos = grupos.Where(c => c.semestre == semestre);
            //}

            return grupos;
        }

        // Para la vista de los formularios
        public List<ElegirFormularioEditorViewModel> ObtenerFormularios(string searchString)
        {
            IQueryable<ElegirFormularioEditorViewModel> formularios =
                from formul in db.Formulario
                select new ElegirFormularioEditorViewModel
                {
                    Seleccionado = false,
                    CodigoFormulario = formul.CodigoFormulario,
                    NombreFormulario = formul.Nombre
                };

            return formularios.ToList();
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