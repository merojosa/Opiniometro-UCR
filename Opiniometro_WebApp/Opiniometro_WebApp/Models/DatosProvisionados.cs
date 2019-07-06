﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Opiniometro_WebApp.Models
{
    [MetadataType(typeof(DatosProvisionadosMetadata))]
    public partial class DatosProvisionados { 

        [Required]
        [StringLength(9, MinimumLength = 9)]
        [DataType(DataType.Text)]
        public string Cedula { get; set; }

        [Required]
        [StringLength(30)]
        [DataType(DataType.Text)]
        [RegularExpression(@"Estudiante|Profesor|Administrador")]
        public string Perfil { get; set; }

        [Required]
        [StringLength(50)]
        [DataType(DataType.Text)]
        public string Nombre1 { get; set; }

        [StringLength(50)]
        [DataType(DataType.Text)]
        public string Nombre2 { get; set; }

        [Required]
        [StringLength(50)]
        [DataType(DataType.Text)]
        public string Apellido1 { get; set; }

        [Required]
        [StringLength(50)]
        [DataType(DataType.Text)]
        public string Apellido2 { get; set; }

        [Required]
        [StringLength(50)]
        [RegularExpression(@"([\w]+\.)([\w])(@ucr.ac.cr)")]
        [DataType(DataType.EmailAddress)]
        public string CorreoInstitucional { get; set; }

        [StringLength(6, MinimumLength = 6)]
        [DataType(DataType.Text)]
        public string Carne { get; set; }

        [Required]
        [StringLength(10)]
        [DataType(DataType.Text)]
        public string SiglaCarrera { get; set; }

        [Required]
        [StringLength(3, MinimumLength = 1)]
        [RegularExpression(@"[\d]{1,3}")]
        [DataType(DataType.Text)]
        public byte NumeroEnfasis { get; set; }

       
    }
}