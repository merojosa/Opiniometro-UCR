using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using Opiniometro_WebApp.Models;

namespace Opiniometro_WebApp.Controllers.Servicios
{
    public class IdentidadManager
    {
        // La llave Carrera-Enfasis es unica.
        // Estructura de datos: hash: (Carrera-Enfasis, permiso).

        private HashSet<string> permisos_usuario;
        private Opiniometro_DatosEntities db = new Opiniometro_DatosEntities();

        public IdentidadManager()
        {
            permisos_usuario = new HashSet<string>();
            cargar_permisos();
        }

        public bool verificar_permiso(string sigla_carrera, int enfasis, int id_permiso)
        {
            // Verifico si existe la llave de la combinacion (sigla de la carrera, su enfasis, un permiso en especifico).
            return permisos_usuario.Contains(sigla_carrera + ',' + enfasis + ',' + id_permiso);
        }

        /*
         * Basado en:
         * Obtener multiples tuplas llamando a un procedimiento almacenado: https://docs.microsoft.com/es-es/ef/ef6/modeling/designer/advanced/multiple-result-sets#multiple-result-sets-with-configured-in-edmx
         */
        private void cargar_permisos()
        {
            var identidad_autenticada = (ClaimsPrincipal)Thread.CurrentPrincipal;

            string correo_autenticado = identidad_autenticada.Claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).SingleOrDefault();

            if(correo_autenticado != null)
            {
                limpiar_permisos();

                List<string> siglas_carrera = new List<string>();
                List<int> numeros_enfasis = new List<int>();
                List<int> permisos = new List<int>();

                // Guardo las tuplas resultantes del llamado al procedimiento almacenado, orden: Sigla de carrera, numero de enfasis, permiso
                // ToDo: hacerlo con el enfasis actual.
                var tuplas_resultantes = db.SP_ObtenerPermisosUsuario(correo_autenticado);

                string llave_hash = null;

                foreach (var tupla in tuplas_resultantes)
                {
                    // Creo la llave unica y la agrego a los permisos de usuario.
                    llave_hash = tupla.SiglaCarrera + ',' + tupla.NumeroEnfasis + ',' + tupla.IdPermiso;
                    permisos_usuario.Add(llave_hash);
                }
            }
        }

        public void limpiar_permisos()
        {
            permisos_usuario.Clear();
        }

        public static string obtener_correo_actual()
        {
            // Obtener la identidad de la sesion actual.
            var identidad = (ClaimsPrincipal)Thread.CurrentPrincipal;

            // Obtener el correo de la sesion.
            var correo = identidad.Claims.Where(c => c.Type == ClaimTypes.Email)
                               .Select(c => c.Value).SingleOrDefault();

            return correo;
        }

        public static string obtener_perfil_actual()
        {
            var identidad_autenticada = (ClaimsPrincipal)Thread.CurrentPrincipal;

            // Solo puede haber uno.
            string perfil_actual = identidad_autenticada.Claims.Where(c => c.Type == ClaimTypes.Role)
                                                .Select(c => c.Value).SingleOrDefault();

            return perfil_actual;

        }

        public static bool usuario_loggeado()
        {
            if (obtener_correo_actual() == null)
                return false;
            else
                return true;
        }
    }
}