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
        public List<Enfasi> listaEnfasis { get; set; }

        public Persona persona { get; set; }
        public Usuario usuario { get; set; }
        public Enfasi enfasis { get; set; }
    }
}