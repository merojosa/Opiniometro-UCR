using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Opiniometro_WebApp.Models
{
    public class FormularioModel
    {
        public List<SeccionesOrdenadas> Secciones { get; set; }//Lista de secciones con su orden. 


    }
    public class SeccionesOrdenadas
    {
      
        public String Titulo { get; set; }
        public int Orden { get; set; }
        public List<PreguntasOrdenadasEnSeccion> preguntasDeSeccion{ get; set; }
    }
    public class PreguntasOrdenadasEnSeccion
    {
        public String Planteamiento { get; set; }
        public int Orden { get; set; }
    }
}