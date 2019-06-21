using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Opiniometro_WebApp.Models
{
    public class VistaPreviaPreguntaModel
    {
        public List<Opciones_De_Respuestas_Seleccion_Unica> Opciones { get; set; }

        public Item Item { get; set; }


    }
}