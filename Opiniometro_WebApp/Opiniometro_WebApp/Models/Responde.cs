//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Opiniometro_WebApp.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Responde
    {
        public int ItemId { get; set; }
        public string TituloSeccion { get; set; }
        public System.DateTime FechaRespuesta { get; set; }
        public string CodigoFormularioResp { get; set; }
        public string CedulaPersona { get; set; }
        public string CedulaProfesor { get; set; }
        public short AñoGrupoResp { get; set; }
        public byte SemestreGrupoResp { get; set; }
        public byte NumeroGrupoResp { get; set; }
        public string SiglaGrupoResp { get; set; }
        public string Observacion { get; set; }
        public string Respuesta { get; set; }
        public string RespuestaProfesor { get; set; }
    
        public virtual Formulario_Respuesta Formulario_Respuesta { get; set; }
    }
}
