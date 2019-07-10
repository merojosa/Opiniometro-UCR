using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Opiniometro_WebApp.Models
{
    public class EstudianteGruposMatriculado
    {
        public string cedulaProf { set; get; }
        public string nombreCursoMatriculado { set; get; }
        public string siglaCursoMatriculado { set; get; }
        public string nombreProfeCurso { set; get; }
        public string apellido1Profe { get; set; }
        public string apellido2Profe { get; set; }
        public short anoGrupo { get; set; }
        public int semestreGrupo { set; get; }
        public byte numGrupo { get; set; }
        public string formulario { set; get; }
        public string cedEst { get; set; }
        public string codFormulario { get; set; }
        public IQueryable<EstudianteGruposMatriculado> gruposMatriculado { set; get; }

    }

}