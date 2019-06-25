using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Opiniometro_WebApp.Models
{
    public class AsignarPeriodoModel : Controller
    {
        public IEnumerable<Tiene_Grupo_Formulario> Asignaciones { get; set; }

        // Lista de grupos
       // public IEnumerable<Grupo> Grupos { get; set; }

        // Lista de formularios
       // public IEnumerable<Formulario> Formularios { get; set; }
    }
}