using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Opiniometro_WebApp.Models
{
    public class FormularioMetadata
    {
        [Remote("IsCodigoFormularioAvailable", "Formulario", ErrorMessage = "Este Código de Formulario ya está en uso.")]
        [StringLength(6, ErrorMessage = "El límite de este campo son 6 caracteres.")]
        [Required]
        public string CodigoFormulario { get; set; }
        [StringLength(25, ErrorMessage = "El límite de este campo son 25 caracteres.")]
        [Required]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Campo obligatorio: Seleccione la unidad academica.")]
        public string CodigoUnidadAca { get; set; }
    }

    public class SeccionMetadata
    {
        [Remote("IsTituloAvailable", "Seccion", ErrorMessage = "Este Título ya está en uso.")]
        [StringLength(120, ErrorMessage = "El límite de este campo son 120 caracteres.")]
        [Required]
        public string Titulo { get; set; }
        [StringLength(300, ErrorMessage = "El límite de este campo son 300 caracteres.")]
        [Required]
        public string Descripcion { get; set; }
    }

    public class CategoriaMetadata
    {
        [StringLength(20, ErrorMessage = "El límite de este campo son 20 caracteres.")]
        [Required]
        public string NombreCategoria { get; set; }
    }

    public class ItemMetadata
    {
        [Remote("IsItemIdAvailable", "Item", ErrorMessage = "Este Código ya está en uso.")]
        [StringLength(10, ErrorMessage = "El límite de este campo son 10 caracteres.")]
        [Required(ErrorMessage = "Campo obligatorio: Ingrese un Código.")]
        public string ItemId { get; set; }
        [StringLength(120, ErrorMessage = "El límite de este campo son 120 caracteres.")]
        [Required(ErrorMessage = "Campo obligatorio: Ingrese el Texto de la Pregunta.")]
        public string TextoPregunta { get; set; }
        [Required(ErrorMessage = "Campo obligatorio: Seleccione si Tiene Observación o no.")]
        public Nullable<bool> TieneObservacion { get; set; }
        [Required(ErrorMessage = "Campo obligatorio: Seleccione el Tipo de Pregunta.")]
        public byte TipoPregunta { get; set; }
        [Required(ErrorMessage = "Campo obligatorio: Seleccione el Nombre de la Categoría.")]
        public string NombreCategoria { get; set; }
    }

    public class CrearFormularioMetadata
    {
        [Remote("IsOrden_ItemAvailable", "CrearFormulario", ErrorMessage = "Este numero de pregunta ya esta ocupado.")]
        [Range(1, 1000)]
        public Nullable<int> Orden_Item { get; set; }

        [Remote("IsOrden_SeccionAvailable", "CrearFormulario", ErrorMessage = "Este numero de Seccion ya esta ocupado.")]
        [Range(1, 1000)]
        public Nullable<int> Orden_Seccion { get; set; }
    }

    
}