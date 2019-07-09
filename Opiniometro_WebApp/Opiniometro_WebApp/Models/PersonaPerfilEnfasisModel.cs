namespace Opiniometro_WebApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public partial class PersonaPerfilEnfasisModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PersonaPerfilEnfasisModel()
        {

        }

        public class AsignarPerfil
        {
            public AsignarPerfil(String perfil, Boolean asignado)
            {
                perfilActual = perfil;
                asignar = asignado;

            }
            public String perfilActual;
            public Boolean asignar;
        }

        public List<AsignarPerfil> getAsignarPerfil(ICollection<String> perfilDeUsuarioP, ICollection<String> perfilP)
        {
            List<AsignarPerfil> listaAsignarPerfil = new List<AsignarPerfil>();
            for (int contador = 0; contador < perfilP.Count; contador++)
            {
                if (perfilDeUsuarioP.Contains(perfilP.ElementAt(contador)))
                {
                    listaAsignarPerfil.Add(new AsignarPerfil(perfilP.ElementAt(contador), true));
                }
                else
                {
                    listaAsignarPerfil.Add(new AsignarPerfil(perfilP.ElementAt(contador), false));
                }
            }


            return listaAsignarPerfil;
        }


        public virtual List<AsignarPerfil> perfilesAsignados { get; set; }

        public virtual List <Boolean> tienePerfil { get; set; }


        public virtual ICollection<Enfasis> Enfasis { get; set; }

        public virtual ICollection<String> PerfilDeUsuario { get; set; }

        public virtual List<String> Perfil { get; set; }

        public List<Persona> listaPersonas { get; set; }
        public List<Usuario> listaUsuarios { get; set; }
        
        public Persona Persona { get; set; }
        public string viejaCedula { get; set; }
        public Usuario Usuario { get; set; }
    }
}