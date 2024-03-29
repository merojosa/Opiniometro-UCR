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
    
    public partial class Grupo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Grupo()
        {
            this.Formulario_Respuesta = new HashSet<Formulario_Respuesta>();
            this.Tiene_Grupo_Formulario = new HashSet<Tiene_Grupo_Formulario>();
            this.Profesor = new HashSet<Profesor>();
        }
    
        public string SiglaCurso { get; set; }
        public byte Numero { get; set; }
        public short AnnoGrupo { get; set; }
        public byte SemestreGrupo { get; set; }
    
        public virtual Ciclo_Lectivo Ciclo_Lectivo { get; set; }
        public virtual Curso Curso { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Formulario_Respuesta> Formulario_Respuesta { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tiene_Grupo_Formulario> Tiene_Grupo_Formulario { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Profesor> Profesor { get; set; }
    }
}
