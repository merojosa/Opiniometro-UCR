﻿﻿using System;
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

        public List<Conformado_Item_Sec_Form> Conformados { get; set; }

        public List<Conformado_For_Sec> ConformadoS { get; set; }

        public Nullable<int> Orden_Item { get; set; }

        public Nullable<int> Orden_Seccion { get; set; }

        public FormularioCompletoModel FormularioCompleto { get; set; }
    }
    public class FormularioCompletoModel
    {
        public List<Conformado_Item_Sec_Form> Conformados { get; set; }

        public List<Conformado_For_Sec> ConformadoS { get; set; }
    }


}