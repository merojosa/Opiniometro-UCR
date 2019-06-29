using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Opiniometro_WebApp.Models
{
    [MetadataType(typeof(FormularioMetadata))]
    public partial class Formulario
    {
    }

    [MetadataType(typeof(SeccionMetadata))]
    public partial class Seccion
    {
    }

    [MetadataType(typeof(CategoriaMetadata))]
    public partial class Categoria
    {
    }

    [MetadataType(typeof(ItemMetadata))]
    public partial class Item
    {
    }

    [MetadataType(typeof(CrearFormularioMetadata))]
    public partial class CrearFormulario
    {

    }

    // The Strategists.
    [MetadataType(typeof(PerfilMetadata))]
    public partial class Perfil
    {
    }
}