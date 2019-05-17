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
    
    public partial class Formulario_Respuesta
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Formulario_Respuesta()
        {
            this.Responde = new HashSet<Responde>();
        }
    
        public System.DateTime Fecha { get; set; }
        public string CodigoFormulario { get; set; }
        public string CedulaPersona { get; set; }
        public string CedulaProfesor { get; set; }
        public short AñoGrupo { get; set; }
        public byte SemestreGrupo { get; set; }
        public byte NumeroGrupo { get; set; }
        public string SiglaGrupo { get; set; }
        public Nullable<bool> Completado { get; set; }
    
        public virtual Persona Persona { get; set; }
        public virtual Profesor Profesor { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Responde> Responde { get; set; }
    }
}
