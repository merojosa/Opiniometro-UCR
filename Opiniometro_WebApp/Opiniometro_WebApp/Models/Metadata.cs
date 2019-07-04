using System;
using System.ComponentModel.DataAnnotations;

namespace Opiniometro_WebApp.Models
{
    public class DatosEstudianteMetadata
    {
        [Required]
        [StringLength(9, MinimumLength = 9)]
        [DataType(DataType.Text)]
        public string Cedula;

        /*[Required]
        public string Perfil { get; set; }*/

        [Required]
        [StringLength(6, MinimumLength = 6)]
        [DataType(DataType.Text)]
        public string Carne;

        [Required]
        [StringLength(50)]
        [DataType(DataType.Text)]
        public string Nombre1;

        [StringLength(50)]
        [DataType(DataType.Text)]
        public string Nombre2;

        [Required]
        [StringLength(50)]
        [DataType(DataType.Text)]
        public string Apellido1;

        [Required]
        [StringLength(50)]
        [DataType(DataType.Text)]
        public string Apellido2;

        [Required]
        [StringLength(50)]
        [RegularExpression(@"([\w]+\.)([\w])(@ucr.ac.cr)")]
        [DataType(DataType.EmailAddress)]
        public string CorreoInstitucional;

        [Required]
        [StringLength(200)]
        [DataType(DataType.Text)]
        public string DireccionDetallada;

        [Required]
        [StringLength(10)]
        [DataType(DataType.Text)]
        public string SiglaCarrera;

        [Required]
        [StringLength(3, MinimumLength = 1)]
        [RegularExpression(@"[\d]{1,3}")]
        [DataType(DataType.Text)]
        public byte NumeroEnfasis;
    }

    public class PersonaMetadata
    {
        [Required]
        [StringLength(9, MinimumLength = 9)]
        [DataType(DataType.Text)]
        public string Cedula;

        [Required]
        [StringLength(50)]
        [DataType(DataType.Text)]
        public string Nombre1;

        [StringLength(50)]
        [DataType(DataType.Text)]
        public string Nombre2;

        [Required]
        [StringLength(50)]
        [DataType(DataType.Text)]
        public string Apellido1;

        [Required]
        [StringLength(50)]
        [DataType(DataType.Text)]
        public string Apellido2;

        [Required]
        [StringLength(200)]
        [DataType(DataType.Text)]
        public string DireccionDetallada;
    }

    public class UsuarioMetadata
    {
        [Required]
        [StringLength(50)]
        [RegularExpression(@"([\w]+\.)([\w])(@ucr.ac.cr)")]
        [DataType(DataType.EmailAddress)]
        public string CorreoInstitucional;

        [Required]
        public string Contrasena;


        public bool Activo;

        [Required]
        [StringLength(9, MinimumLength = 9)]
        [DataType(DataType.Text)]
        public string Cedula;

        [Required]
        public System.Guid Id;
    }

    public class EstudianteMetadata
    {
        [Required]
        [StringLength(9, MinimumLength = 9)]
        [DataType(DataType.Text)]
        public string CedulaEstudiante;

        [Required]
        [StringLength(6, MinimumLength = 6)]
        [DataType(DataType.Text)]
        public string Carne;
    }

    public class EmpadronadoMetadata
    {
        [Required]
        [StringLength(9, MinimumLength = 9)]
        [DataType(DataType.Text)]
        public string CedulaEstudiante;

        [Required]
        [StringLength(3, MinimumLength = 1)]
        [RegularExpression(@"[\d]{1,3}")]
        [DataType(DataType.Text)]
        public byte NumeroEnfasis;

        [Required]
        [StringLength(10)]
        [DataType(DataType.Text)]
        public string SiglaCarrera;
    }

    public class Tiene_Usuario_Perfil_EnfasisMetadata
    {
        [Required]
        Required]
        [StringLength(50)]
        [RegularExpression(@"([\w]+\.)([\w])(@ucr.ac.cr)")]
        [DataType(DataType.EmailAddress)]
        public string CorreoInstitucional;

        [Required]
        [StringLength(3, MinimumLength = 1)]
        [RegularExpression(@"[\d]{1,3}")]
        [DataType(DataType.Text)]
        public byte NumeroEnfasis;

        [Required]
        [StringLength(10)]
        [DataType(DataType.Text)]
        public string SiglaCarrera;

        [Required]
        [StringLength(30)]
        public string NombrePerfil;

    }
}