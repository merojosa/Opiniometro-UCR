using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Opiniometro_WebApp.Models
{
    public class RespuestaModel
    {
        public string IdItem { get; set; }
        public string TituloSeccion { get; set; }
        public string[] HilerasDeRespuesta { get; set; }

        public string Observacion { get; set; }
    }

    /*public abstract class RespuestaModel
    {
        public int TipoRespuesta { get; set; }

        public abstract List<string> ObtenerHilerasDeRespuestas();
    }

    public class RespuestaSeleccionUnicaModel : RespuestaModel
    {
        string RespuestaSeleccionada { get; set; }

        public override List<string> ObtenerHilerasDeRespuestas()
        {
            List<string> hilerasRespuestas = new List<string>();

            hilerasRespuestas.Add(RespuestaSeleccionada);

            return hilerasRespuestas;
        }
    }

    public class RespuestaSeleccionMultipleModel : RespuestaModel
    {
        string [] OpcionesMarcadas { get; set; }


        public override List<string> ObtenerHilerasDeRespuestas()
        {
            List<string> hilerasRespuestas = new List<string>();

            foreach (var opcion in OpcionesMarcadas)
            {
                hilerasRespuestas.Add(opcion);
            }

            return hilerasRespuestas;
        }
    }

    public class RespuestaEscalarModel : RespuestaModel
    {
        string OpcionMarcada { get; set; }


        public override List<string> ObtenerHilerasDeRespuestas()
        {
            List<string> hilerasRespuestas = new List<string>();

            hilerasRespuestas.Add(OpcionMarcada);

            return hilerasRespuestas;
        }
    }

    public class RespuestaAbiertaModel : RespuestaModel
    {
        string Contenido { get; set; }


        public override List<string> ObtenerHilerasDeRespuestas()
        {
            List<string> hilerasRespuestas = new List<string>();

            hilerasRespuestas.Add(Contenido);

            return hilerasRespuestas;
        }
    }*/
}