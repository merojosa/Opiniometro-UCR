namespace Opiniometro_WebApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public partial class EditarPerfil
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EditarPerfil()
        {

        }

        [StringLength(30, ErrorMessage = "El límite de este campo son de 30 caracteres.")]
        [Required(ErrorMessage = "Este campo es requerido.")]
        public string Nombre { get; set; }
        [StringLength(80, ErrorMessage = "El límite de este campo son de 80 caracteres.")]
        [Required(ErrorMessage = "Este campo es requerido.")]
        public string Descripcion { get; set; }
        public string NombreViejo { get; set; }
    }
}
