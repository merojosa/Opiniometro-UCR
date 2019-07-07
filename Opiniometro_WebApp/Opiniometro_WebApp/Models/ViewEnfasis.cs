using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Opiniometro_WebApp.Models
{
    public class ViewEnfasis
    {
        public Enfasis enfasis { get; set; }
        public Carrera carrera { get; set; }
        public Unidad_Academica unidad {get; set;}
        public List<Carrera> ListaCarreras { get; set; }
        public List<Enfasis> ListaEnfasis { get; set; }
        public List<Unidad_Academica> ListaUnidades { get; set; }
    }
}