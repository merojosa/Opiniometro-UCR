using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Opiniometro_WebApp.Models
{
    public class Pregunta
    {
        public string itemId { set; get; }
        public string item { set; get; }
        public bool? tieneObservacion { get; set; }
        public int tipoPregunta { set; get;}

        public string[] Opciones;

    }

    public class SeccionFormulario
    {
        public string Titulo { set; get; }

        public Pregunta[] PreguntasFormulario;

    }

    public class FormularioPorCurso {

        public SeccionFormulario[] Secciones;

    }
}