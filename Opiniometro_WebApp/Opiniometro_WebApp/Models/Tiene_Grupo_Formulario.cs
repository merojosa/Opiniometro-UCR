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
    
    public partial class Tiene_Grupo_Formulario
    {
        public byte Numero { get; set; }
        public string SiglaCurso { get; set; }
        public string Año { get; set; }
        public byte Ciclo { get; set; }
        public string Codigo { get; set; }
        public System.DateTime FechaInicio { get; set; }
        public System.DateTime FechaFinal { get; set; }
    
        public virtual Fecha_Corte Fecha_Corte { get; set; }
        public virtual Grupo Grupo { get; set; }
    }
}