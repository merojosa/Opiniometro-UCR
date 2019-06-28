using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Opiniometro_WebApp.Models
{
    public class SeleccionPermisos
    {
        public class Asociaciones
        {
            public Asociaciones(string idPerfil, short idPerm)
            {
                this.Perfil = idPerfil;
                this.Permiso = idPerm;
            }

            public string Perfil { get; set; }
            public short Permiso { get; set; }
        }

        public class GuardarPerm
        {
            public GuardarPerm()
            {
            }

            public GuardarPerm(string idPerfil, short idPermiso, bool existe)
            {
                this.Perfil = idPerfil;
                this.Permiso = idPermiso;
                this.Existe = existe;
            }

            public string Perfil { get; set; }
            public short Permiso { get; set; }
            public bool Existe { get; set; }
        }

        public List<Enfasis> ListaEnfasis { get; set; }
        public List<Permiso> ListaPermisos { get; set; }
        public List<Perfil> ListaPerfiles { get; set; }
        public List<GuardarPerm> ListaGuardar { get; set; }
        public List<Asociaciones> ListaAsoc { get; set; }
        //Lista con info de tabla Posee_enfasis_permisos
        public List<Posee_Enfasis_Perfil_Permiso> ListaPosee { get; set; }
    }
}