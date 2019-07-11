using System.Collections.Generic;
using System.Linq;

namespace Opiniometro_WebApp.Models
{
    public class GruposYFormsSeleccionados
    {
        public Dictionary< Formulario, Fecha_Corte> PeriodosDeAplicación;

        public List<ElegirGrupoEditorViewModel> GruposSeleccionados { get; set; }
        public List<ElegirFormularioEditorViewModel> FormulariosSeleccionados { get; set; }

        public GruposYFormsSeleccionados(List<ElegirGrupoEditorViewModel> todosGrupos,
            List<ElegirFormularioEditorViewModel> todosFormularios)
        {
            // Obtiene los grupos seleccionados
            GruposSeleccionados = (from gr in todosGrupos where gr.Seleccionado select gr).ToList();
            // Obtiene los formularios seleccionados
            FormulariosSeleccionados = (from form in todosFormularios where form.Seleccionado select form).ToList();
        }

        public GruposYFormsSeleccionados()
        {
            GruposSeleccionados = new List<ElegirGrupoEditorViewModel>();
            FormulariosSeleccionados = new List<ElegirFormularioEditorViewModel>();
        }

        public bool TieneQueAsignar()
        {
            return GruposSeleccionados.Count() > 0 && FormulariosSeleccionados.Count() > 0;
        }
    }

    /// <summary>
    /// Modelo para Asignación de Formularios
    /// </summary>
    public class AsignarFormulariosViewModel
    {
        // Listas con los parámetros de los filtros
        public IEnumerable<Ciclo_Lectivo> Ciclos { get; set; }
        public IEnumerable<Carrera> Carreras { get; set; }
        public IEnumerable<Enfasis> Enfasis { get; set; }
        public IEnumerable<Curso> Cursos { get; set; }
        public IEnumerable<Unidad_Academica> UnidadesAcademicas { get; set; }
        public List<Profesor> Profs { get; set; }


        // Lista de grupos
        public List<ElegirGrupoEditorViewModel> Grupos { get; set; }

        // Lista de formularios
        public List<ElegirFormularioEditorViewModel> Formularios { get; set; }

        // Listas de grupos y formularios seleccionados para hacer las asignaciones
        public GruposYFormsSeleccionados GruFormsSeleccionados { get; set; }

        // Lista asignaciones hechas
        //public IEnumerable<SelectListItem> Asignaciones { get; set; }

        public AsignarFormulariosViewModel()
        {
            // Vacía por default
            Grupos = new List<ElegirGrupoEditorViewModel>();
            // Vacía por default
            Formularios = new List<ElegirFormularioEditorViewModel>();
            // (Por ende,) Vacía por default
            GruFormsSeleccionados = new GruposYFormsSeleccionados (Grupos, Formularios);
        }
    }
}