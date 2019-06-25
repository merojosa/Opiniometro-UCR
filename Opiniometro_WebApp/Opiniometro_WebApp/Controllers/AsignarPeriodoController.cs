using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Opiniometro_WebApp.Models;
using System.Diagnostics;

namespace Opiniometro_WebApp.Controllers
{
    public class AsignarPeriodoController : Controller
    {
        private Opiniometro_DatosEntities db = new Opiniometro_DatosEntities();

        //prueba para mocking tests
        public AsignarPeriodoController()
        {
            db = new Opiniometro_DatosEntities();
        }

        public AsignarPeriodoController(Opiniometro_DatosEntities db)
        {
            this.db = db;
        }


        // GET: AsignarPeriodo
        public ActionResult Index()
        {
            var modelo = new AsignarPeriodoViewModel
            {
                Asignaciones = ObtenerGruposconFormulario()
                //Grupos = ObtenerGrupos(),
                //Formularios = ObtenerFormularios(),
            };
            return View();
        }


        //para mostrar los grupos 
        public IQueryable<Grupo> ObtenerGrupos()
        {
            IQueryable<Grupo> grupo = (from g in db.Grupo select g).Distinct();
            ViewBag.sigla = new SelectList(grupo, "Sigla", "Sigla");
            ViewBag.nombre = new SelectList(grupo, "Nombre", "Nombre");
            return grupo;
        }

        //para moistrar los formularios
        //public IQueryable<Formulario> ObtenerFormularios()
        //{
        //    IQueryable<Formulario> formulario = (from f in db.Formulario select f).Distinct();
        //    ViewBag.codigoForm = new SelectList(formulario, "CodigoFormulario", "CodigoFormulario");
        //    ViewBag.nombre = new SelectList(formulario, "Nombre", "Nombre");
        //    return formulario;
        //}

        public List<Formulario> ObtenerFormularios()
        {
            var formularios =
                from formul in db.Formulario
                select new Formulario
                {
                    CodigoFormulario = formul.CodigoFormulario,
                    Nombre = formul.Nombre
                };

            return formularios.ToList();
        }

        public List<MostrarAsignacionesEditorViewModel> ObtenerGruposconFormulario()
        {
            IQueryable<MostrarAsignacionesEditorViewModel> Asignaciones =
                            from asig in db.Tiene_Grupo_Formulario
                            select new MostrarAsignacionesEditorViewModel
                            {
                                CodigoFormulario = asig.Codigo,
                                SiglaCurso = asig.SiglaCurso
                            };

            return Asignaciones.ToList();
        }
    }
}