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

        public SeccionFormulario[] Secciones;

    }
}