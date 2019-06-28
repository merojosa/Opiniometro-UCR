using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Opiniometro_WebApp.Models;
using System.Diagnostics;

namespace Opiniometro_WebApp.Controllers
{
    public class LlenarFormularioController : Controller
    {
        private Opiniometro_DatosEntities db = new Opiniometro_DatosEntities();

        [HttpGet]
        public ActionResult Index()
        {
            var modelo = new EstudianteGruposMatriculado
            {
                gruposMatriculado = ObtenerGrupoMatriculado("116720500", 1, 2019)               
            };
            return View(modelo);
        }


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
            where (est.CedulaEstudiante==cedulaDelEstudiante && gru.SemestreGrupo == semestre && ano == gru.AnnoGrupo)

            select new EstudianteGruposMatriculado
            {
                siglaCursoMatriculado = mat.Sigla,
                nombreCursoMatriculado = cur.Nombre,
                semestreGrupo = gru.SemestreGrupo,
                anoGrupo= gru.AnnoGrupo,
                nombreProfeCurso = prof.Persona.Nombre,
                apellidoProfe = prof.Persona.Apellido1
                
            };

            return grupos;
        }
                                           

    }
}