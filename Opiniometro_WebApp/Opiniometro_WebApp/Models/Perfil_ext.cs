using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Opiniometro_WebApp.Models
{
    public partial class Perfil
    {
        public static List<String> ObtenerIds()
        {
            using (var context = new Opiniometro_DatosEntities())
            {
                //return context.Perfil.Select(p => p.Id).ToList();
                return (from p in context.Perfil select p.Id).ToList();
            }
        }
    }
}