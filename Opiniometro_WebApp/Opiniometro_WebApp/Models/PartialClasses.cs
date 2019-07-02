using System;
using System.ComponentModel.DataAnnotations;

namespace Opiniometro_WebApp.Models
{
    /*[MetadataType(typeof(DatosEstudianteMetadata))]
    public partial class DatosEstudiante
    { }*/

    [MetadataType(typeof(PersonaMetadata))]
    public partial class Persona
    { }

    [MetadataType(typeof(UsuarioMetadata))]
    public partial class Usuario
    { }

    [MetadataType(typeof(EstudianteMetadata))]
    public partial class Estudiante
    { }

    [MetadataType(typeof(EnfasisMetadata))]
    public partial class Enfasis
    { }

    [MetadataType(typeof(EmpadronadoMetadata))]
    public partial class Empadronado
    { }
}