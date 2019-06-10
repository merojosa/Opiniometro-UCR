using System.Collections.Generic;
using System.Linq;
using Opiniometro_WebApp.Models;

namespace Opiniometro_WebApp.Models
{
    public class ElegirFormularioEditorViewModel
    {
        // Atributo que salva si un formulario se encuentra seleccionado o no:
        public bool Seleccionado { get; set; }

        public string CodigoFormulario { get; set; }
        public string NombreFormulario { get; set; }

        public ElegirFormularioEditorViewModel() { }
    }
}