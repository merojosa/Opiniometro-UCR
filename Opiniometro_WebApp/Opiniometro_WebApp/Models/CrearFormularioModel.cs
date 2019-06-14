using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Opiniometro_WebApp.Models
{

    public class ItemYSeccion
    {
        public int ordenPregunta { get; set;  }
        public int ordenSeccion { get; set; }
        public String titulo { get; set; }
        public int ultimaSeccionAñadida { get; set; }
        public ItemYSeccion()
        {
            ordenPregunta = 0;
            ordenSeccion = 0;
            ultimaSeccionAñadida = 0; 
        }
        public ItemYSeccion(int ordenPregunta, int ordenSeccion, String titulo)
        {
            this.ordenPregunta = ordenPregunta;
            this.ordenSeccion = ordenSeccion;
            this.titulo = titulo; 
        }
        public int aumentarOrdenPregunta()
        {
            ordenPregunta++;
            return ordenPregunta;
        }
        public int aumentarUltimaSeccion()
        {
            ultimaSeccionAñadida++;
            return ultimaSeccionAñadida; 
        }
    }
    public class CrearFormularioModel
    {

        public List<Seccion> Secciones { get; set; }

        public List<Item> Items { get; set; }

        public Formulario Formulario { get; set; }

        public List<Conformado_Item_Sec_Form> Conformados { get; set; }

        public Nullable<int> Orden_Item { get; set; }

        public Nullable<int> Orden_Seccion { get; set; }

        public List<ItemYSeccion> listaItemYSeccion { get; set; }

         
    }


}