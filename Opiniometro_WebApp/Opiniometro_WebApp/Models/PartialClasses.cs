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

    [MetadataType(typeof(EmpadronadoMetadata))]
    public partial class Empadronado
    { }

    [MetadataType(typeof(ProfesorMetadata))]
    public partial class Profesor
    { }

    [MetadataType(typeof(Tiene_Usuario_Perfil_EnfasisMetadata))]
    public partial class Tiene_Usuario_Perfil_Enfasis
    { }
}