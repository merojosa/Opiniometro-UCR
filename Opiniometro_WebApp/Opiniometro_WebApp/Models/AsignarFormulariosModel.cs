using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Opiniometro_WebApp.Models
{
    public class GrupoConInfoExtra
    {
        // Atributos originales de la clase Grupo
        public string siglaCurso;
        public byte numero;
        public short anno;
        public byte semestre;

        // Atributos extra obtenidos de otras clases
        public string nombreCurso;
        public string codigoUnidad;

    }

    public class AsignarFormulariosModel
    {
        // Listas con los parámetros de los filtros
        public IEnumerable<Ciclo_Lectivo> Ciclos { get; set; }
        public IEnumerable<Carrera> Carreras { get; set; }
        public IEnumerable<Enfasis> Enfasis { get; set; }
        public IEnumerable<Curso> Cursos { get; set; }

        // Lista de grupos
        public IEnumerable<GrupoConInfoExtra> Grupos { get; set; }

        // Lista de formularios
        //public IEnumerable<SelectListItem> Formularios { get; set; }

        // Lista signaciones hechas
        //public IEnumerable<SelectListItem> Asignaciones { get; set; }
    }
}