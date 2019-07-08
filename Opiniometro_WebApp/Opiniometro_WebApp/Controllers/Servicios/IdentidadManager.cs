using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web.Mvc;
using Opiniometro_WebApp.Models;

namespace Opiniometro_WebApp.Controllers.Servicios
{
    public class IdentidadManager
    {
        // La llave Carrera-Enfasis es unica.
        // Estructura de datos: hash: (Carrera-Enfasis, permiso).

        private HashSet<string> permisos_usuario;
        private List<Enfasis> lista_enfasis;
        private Opiniometro_DatosEntities db = new Opiniometro_DatosEntities();

        public IdentidadManager()
        {
            permisos_usuario = new HashSet<string>();
            cargar_permisos();
            lista_enfasis = obtener_enfasis_usuario();
        }

        /*
         *  REQUIERE: la sigla de la carrera y el numero del enfasis, junto con el permiso.
         *  EFECTUA: busca en permisos_usuarios si esa llave pasada por parametro existe.
         *  MODIFICA: n/a
         */
        public bool verificar_permiso(string sigla_carrera, int enfasis, int id_permiso)
        {
            // Verifico si existe la llave de la combinacion (sigla de la carrera, su enfasis, un permiso en especifico).
            return permisos_usuario.Contains(sigla_carrera + ',' + enfasis + ',' + id_permiso);
        }


        /*
         *  REQUIERE: el permiso con el que se quiere pedir permiso.
         *  EFECTUA: busca en permisos_usuarios si esta el permiso iterando en todos los enfasis que tiene actualmente.
         *  MODIFICA: n/a
         */
        public bool verificar_permiso(int id_permiso)
        {
            bool autorizado = false;

            // Recorrer enfasis.
            foreach(Enfasis un_enfasis in lista_enfasis)
            {
                // Si ya esta autorizado, no tiene que hacer nada.
                if(autorizado == false)
                {
                    autorizado = verificar_permiso(un_enfasis.SiglaCarrera, un_enfasis.Numero, id_permiso);
                }
            }

            return autorizado;
        }

        /*
         *  REQUIERE: el correo del usuario autenticado.
         *  EFECTUA: obtiene los enfasis de un usuario en particular.
         *  MODIFICA: n/a
         */
        private List<Enfasis> obtener_enfasis_usuario()
        {
            var identidad_autenticada = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string correo_autenticado = identidad_autenticada.Claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).SingleOrDefault();

            if(correo_autenticado != null)
            {
                List<Enfasis> lista = new List<Enfasis>();

                // Procedimiento almacenado.
                // Guardo las tuplas resultantes del llamado al procedimiento almacenado, orden: Sigla de carrera, numero de enfasis, permiso
                var tuplas_resultantes = db.ObtenerPerfilesUsuario(correo_autenticado);
                Enfasis enfasis = null;

                // Iterar por cada tupla
                foreach (var tupla in tuplas_resultantes)
                {
                    enfasis = new Enfasis();
                    enfasis.SiglaCarrera = tupla.SiglaCarrera;
                    enfasis.Numero = tupla.NumeroEnfasis;
                    lista.Add(enfasis);
                }
                return lista;
            }
            else
            {
                return null;
            }
        }

        public List<Enfasis> obtener_lista_enfasis()
        {
            return lista_enfasis;
        }

        /*
         * Basado en:
         * Obtener multiples tuplas llamando a un procedimiento almacenado: https://docs.microsoft.com/es-es/ef/ef6/modeling/designer/advanced/multiple-result-sets#multiple-result-sets-with-configured-in-edmx
         */
        private void cargar_permisos()
        {
            var identidad_autenticada = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string correo_autenticado = identidad_autenticada.Claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).SingleOrDefault();

            if (correo_autenticado != null)
            {
                limpiar_permisos();

                // Guardo las tuplas resultantes del llamado al procedimiento almacenado, orden: Sigla de carrera, numero de enfasis, permiso
                var tuplas_resultantes = db.SP_ObtenerPermisosUsuario(correo_autenticado, obtener_perfil_actual());

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

        public static bool verificar_sesion(Controller controller)
        {
            // Si no tiene permisos, el usuario tiene que loggearse nuevamente para obtenerlos.
            if(controller.Session[obtener_correo_actual()] != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string obtener_nombre_actual()
        {
            Opiniometro_DatosEntities db = new Opiniometro_DatosEntities();

            ObjectParameter nombre = new ObjectParameter("Nombre", "");
            ObjectParameter apellido = new ObjectParameter("Apellido", "");
            db.SP_ObtenerNombre(obtener_correo_actual(), nombre, apellido);


            return (string)nombre.Value + " " + (string)apellido.Value;
        }
    }
}