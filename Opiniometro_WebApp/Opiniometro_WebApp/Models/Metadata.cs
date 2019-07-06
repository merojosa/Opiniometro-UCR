using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Opiniometro_WebApp.Models
{
    public class DatosProvisionadosMetadata
    {
        [Required]
        [StringLength(9, MinimumLength = 9, ErrorMessage = "El numero de {0} es de nueve dígitos.")]
        [DataType(DataType.Text)]
        [RegularExpression(@"[0-9]+", ErrorMessage = "Solo digite numeros para la {0}.")]
        public string Cedula;  //0

        [Required]
        [StringLength(30, ErrorMessage = "Un nombre de perfil no debe exceder de {1} caracteres.")]
        [DataType(DataType.Text)]
        [RegularExpression(@"Estudiante|Profesor", ErrorMessage = "Ha digitado un nombre de perfil inválido.")]
        public string Perfil { get; set; } //1

        [Required]
        [Display(Name = "Primer nombre")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "El {o} debe contener entre {2} y {1} caracteres.")]
        [DataType(DataType.Text)]
        public string Nombre1; //2

        [Display(Name = "Segundo nombre")]
        [StringLength(50, ErrorMessage = "El {o} debe contener entre 0 y {1} caracteres.")]
        [DataType(DataType.Text)]
        public string Nombre2; //3

        [Required]
        [Display(Name = "Primer apellido")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "El {o} debe contener entre {2} y {1} caracteres.")]
        [DataType(DataType.Text)]
        public string Apellido1; //4

        [Required]
        [Display(Name = "Segundo apellido")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "El {o} debe contener entre {2} y {1} caracteres.")]
        [DataType(DataType.Text)]
        public string Apellido2; //5

        [Required]
        [StringLength(50, ErrorMessage = "El {o} debe contener hasta un máximo de {1} caracteres.")]
        [RegularExpression(@"([\w]+\.)([\w])(@ucr.ac.cr)", ErrorMessage = "Formato de correo institucional invalido.")]
        [DataType(DataType.EmailAddress)]
        public string CorreoInstitucional; //6

        
        [StringLength(6, MinimumLength = 6)]
        [DataType(DataType.Text)]
        public string Carne; //7

        [Required]
        [StringLength(10)]
        [DataType(DataType.Text)]
        public string SiglaCarrera; //8

        [Required]
        [Display(Name = "Énfasis")]
        [StringLength(3, MinimumLength = 1, ErrorMessage = "El {o} debe contener hasta un máximo de {1} caracteres")]
        [RegularExpression(@"[\d]{1,3}", ErrorMessage = "Solo digite numeros para el número del enfasis")]
        [DataType(DataType.Text)]
        public byte NumeroEnfasis; //9
    }

    public class PersonaMetadata
    {
        [Required]
        [StringLength(9, MinimumLength = 9)]
        [RegularExpression(@"[0-9]+")]
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

        [Required]
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
        [RegularExpression(@"[0-9]+")]
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
        [RegularExpression(@"[0-9]+")]
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

    public class ProfesorMetadata
    {
        [Required]
        [StringLength(9, MinimumLength = 9)]
        [DataType(DataType.Text)]
        [RegularExpression(@"[0-9]+")]
        public string CedulaProfesor;
    }

    
}