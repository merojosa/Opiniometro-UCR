//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Opiniometro_WebApp.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Tiene_Usuario_Perfil_Enfasis
    {
        public string CorreoInstitucional { get; set; }
        public byte NumeroEnfasis { get; set; }
        public string SiglaCarrera { get; set; }
        public string IdPerfil { get; set; }
    
        public virtual Enfasis Enfasis { get; set; }
        public virtual Perfil Perfil { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}