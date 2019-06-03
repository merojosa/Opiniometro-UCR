using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using Opiniometro_WebApp.Models;

namespace Opiniometro_WebApp.Controllers.Servicios
{
    public class PermisosUsuario
    {
        // Esto sera otra estructura de datos (probablemente un hash).
        private List<int> permisos_usuario = new List<int>();
        private Opiniometro_DatosEntities db = new Opiniometro_DatosEntities();

        public PermisosUsuario()
        {
            cargar_permisos();
        }

        public bool verificar_permiso(int id_permiso)
        {
            return permisos_usuario.Contains(id_permiso);
        }

        /*
         * Basado en:
         * Obtener multiples tuplas llamando a un procedimiento almacenado: https://docs.microsoft.com/es-es/ef/ef6/modeling/designer/advanced/multiple-result-sets#multiple-result-sets-with-configured-in-edmx
         */
        public void cargar_permisos()
        {
            var identidad_autenticada = (ClaimsPrincipal)Thread.CurrentPrincipal;

            string correo_autenticado = identidad_autenticada.Claims.Where(c => c.Type == ClaimTypes.Email)
                                                .Select(c => c.Value).SingleOrDefault();

            if(correo_autenticado != null)
            {
                // Guardo las tuplas resultantes del llamado al procedimiento almacenado.
                var resultados_permisos = db.SP_ObtenerPermisosUsuario(correo_autenticado);
                limpiar_permisos();

                // Itero por las tuplas resultantes, en caso de que no hayan, no entra en el foreach.
                foreach (var permiso in resultados_permisos)
                {
                    permisos_usuario.Add(permiso.Value);
                }
            }
        }

        public void limpiar_permisos()
        {
            permisos_usuario.Clear();
        }
    }
}