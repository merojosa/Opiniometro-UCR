//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Opiniometro_WebApp.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Responde
    {
        public string ItemId { get; set; }
        public string TituloSeccion { get; set; }
        public System.DateTime FechaRespuesta { get; set; }
        public string CodigoFormularioResp { get; set; }
        public string CedulaPersona { get; set; }
        public string CedulaProfesor { get; set; }
        public short AnnoGrupoResp { get; set; }
        public byte SemestreGrupoResp { get; set; }
        public byte NumeroGrupoResp { get; set; }
        public string SiglaGrupoResp { get; set; }
        public string Observacion { get; set; }
        public string Respuesta { get; set; }
        public string RespuestaProfesor { get; set; }
    
        public virtual Formulario_Respuesta Formulario_Respuesta { get; set; }
        public virtual Item Item { get; set; }
        public virtual Seccion Seccion { get; set; }
    }
}
