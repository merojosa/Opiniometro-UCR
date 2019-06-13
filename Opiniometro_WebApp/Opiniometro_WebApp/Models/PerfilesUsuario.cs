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
    }
}
