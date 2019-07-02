using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Opiniometro_WebApp.Models
{
    public class DatosFormulario
    {
        public string item { set; get; }
        public bool? tieneObservacion { get; set; }
        public int tipoPregunta { set; get;}

        public List<string> opcionesPregunta { set; get; }

    }

    public class FormularioPorCurso {

        public IQueryable<DatosFormulario> preguntasFormulario;

    }

}