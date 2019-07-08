using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Opiniometro_WebApp.Models
{
    public class ViewModelAdmin
    {
        public List<Persona> listaPersonas { get; set; }
        public List<Usuario> listaUsuarios { get; set; }
        public List<Enfasis> listaEnfasis { get; set; }

        public Persona Persona { get; set; }
        public Usuario Usuario { get; set; }
        public Enfasis Enfasis { get; set; }
    }
}