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

    }

    public class FormularioPorCurso {

        public IQueryable<Pregunta> preguntasFormulario;

    }

    public class TextoLibre : Pregunta
    {

    }

    public class SeleccionUnica : Pregunta
    {
        public IEnumerable<string> Opciones;
    }

    public class SiNo : Pregunta
    {

    }
}