using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Opiniometro_WebApp.Models
{
    public class FormularioCursoModel
    {
        public string item { set; get; }
        public bool? tieneObservacion { get; set; }
        public int tipoPregunta { get; set; }

        public IQueryable<FormularioCursoModel> preguntasFormulario;
    }
}