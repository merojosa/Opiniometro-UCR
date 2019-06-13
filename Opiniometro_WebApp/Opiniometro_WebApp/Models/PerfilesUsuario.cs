namespace Opiniometro_WebApp.Models
{
    using System;
    using System.Collections.Generic;

    public partial class PerfilesUsuario
    {
        public PerfilesUsuario()
        {

        }
        public virtual ICollection<Perfil> ListaPerfiles { get; set; }
    }
}
