using System.Collections.Generic;
using System.Linq;
using Opiniometro_WebApp.Models;

namespace Opiniometro_WebApp.Models
{
    public class MostrarAsignacionesEditorViewModel
    {
        // Atributo que salva si un formulario se encuentra seleccionado o no
        public string CodigoFormulario { get; set; }
        public string SiglaCurso { get; set; }

        public MostrarAsignacionesEditorViewModel() { }
    }
}