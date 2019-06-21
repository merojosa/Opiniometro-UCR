using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Opiniometro_WebApp.Models
{
    public class EstudianteGruposMatriculado
    {

        public string nombreCursoMatriculado { set; get; }
        public string siglaCursoMatriculado { set; get; }
        public string nombreProfeCurso { set; get; }
        public string apellidoProfe { get; set; }
        public short anoGrupo { get; set; }
        public int semestreGrupo { set; get; }
        //public string formulario { set; get; }

        public IQueryable<EstudianteGruposMatriculado> gruposMatriculado { set; get; }

    }

}