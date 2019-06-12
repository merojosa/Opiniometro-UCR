using System.Collections.Generic;
using System.Linq;
using Opiniometro_WebApp.Models;

namespace Opiniometro_WebApp.Models
{
    public class ElegirGrupoEditorViewModel
    {
        // Atributo que salva si un grupo se encuentra seleccionado o no:
        public bool Seleccionado { get; set; }

        // Llave primaria compuesta de la entidad grupo:
        public string SiglaCurso { get; set; }
        public byte Numero { get; set; }
        public short Anno { get; set; }
        public byte Semestre { get; set; }

        // Otros atributos que se deben mostrar en la vista:
        public List<Profesor> Profesores { get; set; }
        public string NombreCurso { get; set; }
       
        public string Enfasis { get; set; }

        // Atributos que no se muestran pero se utilizan para consultar y filtrar
        public string NombreUnidadAcademica { get; set; }
        public string NombresCarreras { get; set; }

        public ElegirGrupoEditorViewModel()
        {
            Profesores = new List<Profesor>();
        }

        public bool PerteneceAUA (string NombreUAPorBuscar)
        {
            return NombreUnidadAcademica.Contains(NombreUAPorBuscar);
        }

        public bool EstaEnCarrera (string NombreCarreraPorBuscar)
        {
            return NombresCarreras.Contains(NombreCarreraPorBuscar);
        }
    }
}