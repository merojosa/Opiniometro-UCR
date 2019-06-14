namespace Opiniometro_WebApp.Models
{
    using System;
    using System.Collections.Generic;

    public partial class PerfilesUsuario
    {
        public PerfilesUsuario()
        {

        }
        public virtual ICollection<String> ListaPerfiles { get; set; }
        public virtual String perfilSeleccionado{ get; set; }
        public List<Persona> listaPersonas { get; set; }
        public List<Usuario> listaUsuarios { get; set; }
        public List<Enfasis> listaEnfasis { get; set; }

        public Persona persona { get; set; }
        public Usuario usuario { get; set; }
        public Enfasis enfasis { get; set; }
    }
}
