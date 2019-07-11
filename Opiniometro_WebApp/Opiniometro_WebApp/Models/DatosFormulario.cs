using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Opiniometro_WebApp.Models
{
    public class Pregunta
    {
        public int numPregunta { set; get; }
        public string itemId { set; get; }
        public string item { set; get; }
        public bool? tieneObservacion { get; set; }
        public int tipoPregunta { set; get;}

        public string[] Opciones;

    }
    //      |   |   |
    //      V   V   V
    public class SeccionFormulario
    {
        public string Titulo { set; get; }

        public Pregunta[] PreguntasFormulario;

    }
    //      |   |   |
    //      V   V   V
    public class FormularioPorCurso {

        public string cedProf { set; get; }
        public string siglaCurso { set; get; }
        public int anoGrupo { get; set; }
        public int semestreGrupo { set; get; }
        public int numGrupo { get; set; }
        public string cedEst { get; set; }
        public string codFormulario { get; set; }

        public SeccionFormulario[] Secciones;

    }
}