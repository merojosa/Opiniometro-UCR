using System.Collections.Generic;
using System.Linq;

namespace Opiniometro_WebApp.Models
{
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

        // Lista de grupos
        public IQueryable<ElegirGrupoEditorViewModel> Grupos { get; set; }

        // Lista de formularios
        public IEnumerable<Formulario> Formularios { get; set; }

        // Lista asignaciones hechas
        //public IEnumerable<SelectListItem> Asignaciones { get; set; }

        public AsignarFormulariosViewModel()
        {
            Grupos = new List<ElegirGrupoEditorViewModel>().AsQueryable();
            Formularios = new List<Formulario>();
        }

        public IEnumerable<Grupo> gruposSeleccionados()
        {
            return (from gr in this.Grupos
                    where gr.Seleccionado
                    select new Grupo
                    {
                        SiglaCurso = gr.SiglaCurso,
                        Numero = gr.Numero,
                        AnnoGrupo = gr.Anno,
                        SemestreGrupo = gr.Semestre
                    }).ToList();
        }
    }
}