using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Opiniometro_WebApp.Models;
using System.Diagnostics;
using Opiniometro_WebApp.Controllers.Servicios;

namespace Opiniometro_WebApp.Controllers
{
    [Authorize]
    public class CursosMatriculadosController : Controller
    {
        private Opiniometro_DatosEntities db = new Opiniometro_DatosEntities();

        [HttpGet]
        public ActionResult Index()
        {
            var modelo = new EstudianteGruposMatriculado
            {
                gruposMatriculado = ObtenerGrupoMatriculado(obtenerCedulaEstLoggeado(), 1, 2019)               
            };
            return View(modelo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// efecto:retorna los cursos matriculados del estudiante loggeado
        /// recibe:
        /// <param name="cedulaDelEstudiante"> cedula del estudiante que está loggeado</param>
        /// <param name="semestre"> recibe el semestre actual </param>
        /// <param name="ano">recibe el año actual </param>
        /// modifica--
        /// <returns></returns>
        public IQueryable<EstudianteGruposMatriculado> ObtenerGrupoMatriculado(string cedulaDelEstudiante, int semestre, int ano) {
            IQueryable<EstudianteGruposMatriculado> grupos =
            from mat in db.Matricula
            join est in db.Estudiante on mat.CedulaEstudiante equals est.CedulaEstudiante
            join gru in db.Grupo on new { a = mat.Semestre, b = mat.Numero,/*c=mat.Anno,*/d = mat.Sigla }
            equals new { a = gru.SemestreGrupo, b = gru.Numero,/*c=gru.AnnoGrupo,*/d = gru.SiglaCurso }
            join cur in db.Curso on gru.SiglaCurso equals cur.Sigla
            join imp in db.Imparte on new { par1 = gru.SemestreGrupo, par2 = gru.Numero,/*par3=gru.AnnoGrupo,*/par4 = gru.SiglaCurso }
            equals new { par1 = imp.Semestre, par2 = imp.Numero,/*par3=imp.Anno,*/par4 = imp.Sigla }
            join prof in db.Profesor on imp.CedulaProfesor equals prof.CedulaProfesor
            join tiene in db.Tiene_Grupo_Formulario on new { p1 = gru.AnnoGrupo, p2 = gru.SemestreGrupo, p3 = gru.Numero, p4 = gru.SiglaCurso }
            equals new { p1 = tiene.Anno, p2 = tiene.Ciclo, p3 = tiene.Numero, p4 = tiene.SiglaCurso }
            join form in db.Formulario on tiene.Codigo equals form.CodigoFormulario
            where (est.CedulaEstudiante == cedulaDelEstudiante && gru.SemestreGrupo == semestre && ano == gru.AnnoGrupo)


            select new EstudianteGruposMatriculado
            {
                siglaCursoMatriculado = mat.Sigla,
                nombreCursoMatriculado = cur.Nombre,
                semestreGrupo = gru.SemestreGrupo,
                anoGrupo = gru.AnnoGrupo,
                nombreProfeCurso = prof.Persona.Nombre,
                apellido1Profe = prof.Persona.Apellido1,
                apellido2Profe = prof.Persona.Apellido2,
                formulario = form.Nombre,
                cedEst=cedulaDelEstudiante, // se guarda muchas veces, no es necesario
                codFormulario = form.CodigoFormulario
            };            
            
            return grupos;
        }
        /// <summary>
        /// efecto:recupera la cedula del estudiante loggeado 
        /// requiere: --
        /// modifica:--
        /// </summary>
        /// <returns></returns>
        public string obtenerCedulaEstLoggeado()
        {            
            string correoUsLog = IdentidadManager.obtener_correo_actual();
            string cedula = (from us in db.Usuario
                             where us.CorreoInstitucional == correoUsLog
                             select us).First().Cedula.ToString();
            return cedula;
        }

    }
}