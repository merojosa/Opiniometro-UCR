﻿using System;
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

        public List<Conformado_For_Sec> Conformado_For_Secs { get; set; }

        public Nullable<int> Orden_Item { get; set; }

        public Nullable<int> Orden_Seccion { get; set; }

        public FormularioCompletoModel FormularioCompleto { get; set; }

        public CopiarSeccionModel CopiarSeccionModel { get; set; }

        public CopiarSeccion CopiarSeccion { get; set; }
    }
    public class FormularioCompletoModel
    {
        public List<Conformado_Item_Sec_Form> Conformados { get; set; }

        public List<Conformado_For_Sec> ConformadoS { get; set; }
    }
    /*
     *Modelo para pasar los parametros de la vista al controller para utilizar el procedimiento almacenado
     */
    public class CopiarSeccionModel
    {
        public string Cod_Form_Dest { get; set; }

        public string Nom_Form_Dest{ get; set; }

        public string Cod_Form_Origen { get; set; }

        public string Titulo_Seccion { get; set; }
    }
    /*
     *Modelo para la vista de copiar secciones, donde pasa los datos necesarios de la vista     * 
    */
    public class CopiarSeccion
    {
        public List<Seccion> Secciones{ get; set; }

        public List<Formulario> Formularios { get; set; }

        public CopiarSeccionModel CopiarSeccionModel { get; set; }

        public Formulario Formulario { get; set; }
    }

}