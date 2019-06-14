namespace Opiniometro_WebApp.Models
{
    using System;
    using System.Collections.Generic;

    public partial class PersonaPerfilEnfasisModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PersonaPerfilEnfasisModel()
        {

        }

        
        public virtual ICollection<Enfasis> Enfasis { get; set; }

        public virtual ICollection<String> Perfil { get; set; }

        public List<Persona> listaPersonas { get; set; }
        public List<Usuario> listaUsuarios { get; set; }
        
        public Persona Persona { get; set; }
        public Usuario usuario { get; set; }
    }
}