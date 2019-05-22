using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Opiniometro_WebApp.Models
{
    public class AsignarFormulariosModel
    {
        // Listas con los parámetros de los filtros
        public IEnumerable<Ciclo_Lectivo> Ciclos { get; set; }
        public IEnumerable<Carrera> Carreras { get; set; }
        public IEnumerable<Enfasis> Enfasis { get; set; }
        public IEnumerable<Curso> Cursos { get; set; }

        // Lista de grupos
        public IEnumerable<Grupo> Grupos { get; set; }

        // Lista de formularios
        //public IEnumerable<SelectListItem> Formularios { get; set; }

        // Lista signaciones hechas
        //public IEnumerable<SelectListItem> Asignaciones { get; set; }
    }
}