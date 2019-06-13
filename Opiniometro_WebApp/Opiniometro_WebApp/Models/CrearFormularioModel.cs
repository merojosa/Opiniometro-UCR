using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Opiniometro_WebApp.Models
{
    public class CrearFormularioModel
    {

        public List<Seccion> Secciones { get; set; }

        public List<Item> Items { get; set; }

        public Formulario Formulario { get; set; }

    }


}