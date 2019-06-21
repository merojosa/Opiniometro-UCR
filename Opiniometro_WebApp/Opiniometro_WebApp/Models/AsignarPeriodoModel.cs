using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Opiniometro_WebApp.Models
{
    public class AsignarPeriodoModel : Controller
    {
        // Lista de grupos
        public IEnumerable<Grupo> Grupos { get; set; }

        // Lista de formularios
        public IEnumerable<Formulario> Formularios { get; set; }
    }
}