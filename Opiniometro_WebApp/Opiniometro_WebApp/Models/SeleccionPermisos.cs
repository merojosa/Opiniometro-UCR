using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Opiniometro_WebApp.Models
{
    public class SeleccionPermisos
    {
        public Permiso PermisoModel { get; set; }
        public Perfil PerfilModel { get; set; }
        public string IdPerfil { get; set; }
        public List<Permiso> ListaPermisos { get; set; }
        public List<Perfil> ListaPerfiles { get; set; }

        public List<String> ListaPerfilesId { get; set; }
    }
}