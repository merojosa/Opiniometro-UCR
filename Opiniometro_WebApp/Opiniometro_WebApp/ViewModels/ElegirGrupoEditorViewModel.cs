using System.Collections.Generic;
using System.Linq;
using Opiniometro_WebApp.Models;
using System;

namespace Opiniometro_WebApp.Models
{
    public class ElegirGrupoEditorViewModel : IEquatable<ElegirGrupoEditorViewModel>
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
        public IQueryable<string> NombresCarreras { get; set; }
        public string CodigoUnidadAcademica { get; set; }
        public string SiglaCarrera { get; set; }

        public ElegirGrupoEditorViewModel()
        {
            Profesores = new List<Profesor>();
        }

        public bool Equals(ElegirGrupoEditorViewModel otro)
        {
            if (this == null && otro == null)
                return true;
            else if (this == null || otro == null)
                return false;
            else if (this.Anno == otro.Anno
                && this.Semestre == otro.Semestre
                && this.SiglaCurso == otro.SiglaCurso
                && this.Numero == otro.Numero)
                return true;
            else
                return false;
        }

        public int GetHashCode(ElegirGrupoEditorViewModel gr)
        {
            int hCode = ((gr.Anno.GetHashCode() * 17 + gr.Semestre.GetHashCode()) * 17 + gr.Numero.GetHashCode()) * 17 + gr.SiglaCurso.GetHashCode();
            return GetHashCode();
        }
    }
}