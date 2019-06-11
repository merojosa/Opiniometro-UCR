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
        public List<ElegirGrupoEditorViewModel> Grupos { get; set; }

        // Lista de formularios
        public List<ElegirFormularioEditorViewModel> Formularios { get; set; }

        // Listas de grupos y formularios seleccionados para hacer las asignaciones
        public List<ElegirGrupoEditorViewModel> GruposSeleccionados { get; set; }
        public List<ElegirFormularioEditorViewModel> FormulariosSeleccionados { get; set; }

        // Lista asignaciones hechas
        //public IEnumerable<SelectListItem> Asignaciones { get; set; }

        public AsignarFormulariosViewModel()
        {
            Grupos = new List<ElegirGrupoEditorViewModel>();
            Formularios = new List<ElegirFormularioEditorViewModel>();
            GruposSeleccionados = new List<ElegirGrupoEditorViewModel>();
            FormulariosSeleccionados = new List<ElegirFormularioEditorViewModel>();


#if true    // Datos de prueba

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

        // Actualiza la lista de grupos que han sido seleccionados por el usuario y los retorna.
        public IEnumerable<ElegirGrupoEditorViewModel> ActualizarGruposSeleccionados()
        {
            this.GruposSeleccionados.Clear();
            this.GruposSeleccionados = (from gr in this.Grupos where gr.Seleccionado select gr).ToList();
               /*(from gr in this.Grupos
                where gr.Seleccionado
                select new Grupo
                {
                    SiglaCurso = gr.SiglaCurso,
                    Numero = gr.Numero,
                    AnnoGrupo = gr.Anno,
                    SemestreGrupo = gr.Semestre
                }).ToList();*/

            return this.GruposSeleccionados;
        }

        // Actualiza la lista de formularios que han sido seleccionados por el usuario y los retorna.
        public IEnumerable<ElegirFormularioEditorViewModel> ActualizarFormulariosSeleccionados()
        {
            this.FormulariosSeleccionados.Clear();
            this.FormulariosSeleccionados = (from form in this.Formularios where form.Seleccionado select form).ToList();
               /*(from formul in this.Formularios
                where formul.Seleccionado
                select new Formulario
                {
                    CodigoFormulario = formul.CodigoFormulario,
                    Nombre = formul.NombreFormulario
                }).ToList();*/

            return this.FormulariosSeleccionados;
        }
    }
}