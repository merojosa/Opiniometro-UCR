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

            public GuardarPerm(string idPerfil, short idPermiso)
            {
                this.Perfil = idPerfil;
                this.Permiso = idPermiso;
            }

            public string Perfil { get; set; }
            public short Permiso { get; set; }
            public bool Existe { get; set; }
        }

        //IdPerfil es para la seleccion actual de perfil
        public string IdPerfil { get; set; }
        public List<Enfasis> ListaEnfasis { get; set; }
        public List<Permiso> ListaPermisos { get; set; }
        public List<Perfil> ListaPerfiles { get; set; }
        public List<GuardarPerm> ListaGuardar { get; set; }
        public List<Asociaciones> ListaAsoc { get; set; }
        public List<Posee_Enfasis_Perfil_Permiso> ListaPosee { get; set; }
        public List<String> ListaPerfilesId { get; set; }
    }
}