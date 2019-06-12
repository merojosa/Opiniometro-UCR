using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Opiniometro_WebApp.Models
{
    public class GruposYFormsSeleccionados
    {

        public List<ElegirGrupoEditorViewModel> GruposSeleccionados { get; set; }
        public List<ElegirFormularioEditorViewModel> FormulariosSeleccionados { get; set; }

        public GruposYFormsSeleccionados(List<ElegirGrupoEditorViewModel> todosGrupos,
            List<ElegirFormularioEditorViewModel> todosFormularios)
        {
            GruposSeleccionados = (from gr in todosGrupos where gr.Seleccionado select gr).ToList();
            FormulariosSeleccionados = (from form in todosFormularios where form.Seleccionado select form).ToList();
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
            
#if false    
            // Datos de prueba

            GruposSeleccionados.Add(new ElegirGrupoEditorViewModel { NombreCurso = "Prueba1", SiglaCurso = "PBA0001", Numero = 1, Profesores = new List<Profesor>() });
            GruposSeleccionados.Add(new ElegirGrupoEditorViewModel { NombreCurso = "Prueba2", SiglaCurso = "PBA0002", Numero = 1, Profesores = new List<Profesor>() });
            GruposSeleccionados.Add(new ElegirGrupoEditorViewModel { NombreCurso = "Prueba3", SiglaCurso = "PBA0003", Numero = 1, Profesores = new List<Profesor>() });
            GruposSeleccionados.Add(new ElegirGrupoEditorViewModel { NombreCurso = "Prueba4", SiglaCurso = "PBA0004", Numero = 1, Profesores = new List<Profesor>() });

            Persona personaPrueba1 = new Persona { Nombre = "Zacarías", Apellido1 = "Piedra", Apellido2 = "del Río", Cedula = "101230123" };
            Profesor profPrueba1 = new Profesor { CedulaProfesor = "101230123", Persona = personaPrueba1 };

            Persona personaPrueba2 = new Persona { Nombre = "Elba", Apellido1 = "Calao", Apellido2 = "del Río", Cedula = "201230123" };
            Profesor profPrueba2 = new Profesor { CedulaProfesor = "201230123", Persona = personaPrueba2 };

            GruposSeleccionados.ElementAt(1).Profesores.Add(profPrueba1);
            GruposSeleccionados.ElementAt(2).Profesores.Add(profPrueba2);
            GruposSeleccionados.ElementAt(3).Profesores.Add(profPrueba1); GruposSeleccionados.ElementAt(3).Profesores.Add(profPrueba2);
            FormulariosSeleccionados.Add(new ElegirFormularioEditorViewModel { CodigoFormulario = "FM92.3", NombreFormulario = "Prueba F" });
            FormulariosSeleccionados.Add(new ElegirFormularioEditorViewModel { CodigoFormulario = "AM100", NombreFormulario = "Prueba A" });
#endif
        }
    }
}