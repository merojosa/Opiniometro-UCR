using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Opiniometro_WebApp.Models
{
    public class InfoPersonaUsuario
    {
        public string CorreoInstitucional { get; set; }
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public string Direccion { get; set; }
    }
}