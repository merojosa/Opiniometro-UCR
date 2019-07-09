using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Opiniometro_WebApp.Models
{
    [MetadataType(typeof(DatosProvisionadosMetadata))]
    public class DatosProvisionados
    {
        public string Cedula { get; set; }
        public string Perfil { get; set; }
        public string Nombre1 { get; set; }
        public string Nombre2 { get; set; }
        public string Apellido1 { get; set; }
        public string Apellido2 { get; set; }
        public string CorreoInstitucional { get; set; }
        public string Carne { get; set; }
        public string SiglaCarrera { get; set; }
        public byte NumeroEnfasis { get; set; }


    }
}