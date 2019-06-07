using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
        public List<ElegirFormularioEditorViewModel> Formularios { get; set; }

        // Lista asignaciones hechas
        //public IEnumerable<SelectListItem> Asignaciones { get; set; }

        public AsignarFormulariosViewModel()
        {
            Grupos = new List<ElegirGrupoEditorViewModel>().AsQueryable();
            Formularios = new List<ElegirFormularioEditorViewModel>();
        }

        public IEnumerable<Grupo> gruposSeleccionados()
        {
            System.Diagnostics.Debug.WriteLine(this.Grupos.Count());
            foreach (ElegirGrupoEditorViewModel gr in this.Grupos)
            {
                if (gr.Seleccionado)
                    System.Diagnostics.Debug.WriteLine("Grupo seleccionado.");
                else
                    System.Diagnostics.Debug.WriteLine("Grupo no seleccionado.");

            }
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

        public IEnumerable<string> formulariosSeleccionados()
        {
            return (from formul in this.Formularios
                    where formul.Seleccionado
                    select formul.CodigoFormulario).ToList();
        }
    }
}