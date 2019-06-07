using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web;

namespace Opiniometro_WebApp.Models
{
    public class VModel
    {
        public IEnumerable<SelectListItem> Profesores { get; set; }
    }
}