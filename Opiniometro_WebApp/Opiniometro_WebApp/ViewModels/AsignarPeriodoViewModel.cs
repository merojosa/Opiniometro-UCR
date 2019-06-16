using System.Collections.Generic;
using System.Linq;

namespace Opiniometro_WebApp.Models
{

    public class AsignarPeriodoViewModel
    {
        public IEnumerable<Ciclo_Lectivo> Ciclos { get; set; }
        public IEnumerable<Carrera> Carreras { get; set; }
        public IEnumerable<Enfasis> Enfasis { get; set; }
        public IEnumerable<Curso> Cursos { get; set; }
        public IEnumerable<Unidad_Academica> UnidadesAcademicas { get; set; }

        // Lista de grupos
        public IQueryable<ElegirGrupoEditorViewModel> Grupos { get; set; }


    }
}