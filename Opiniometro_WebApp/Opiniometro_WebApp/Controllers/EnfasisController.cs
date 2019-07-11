using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Opiniometro_WebApp.Models;
using Opiniometro_WebApp.Controllers.Servicios;

namespace Opiniometro_WebApp.Controllers
{
    [Authorize]
    public class EnfasisController : Controller
    {
        // GET: Enfasis
        public ActionResult VerEnfasis()
        {
            using (var context = new Opiniometro_DatosEntities())
            {
                ViewEnfasis model = new ViewEnfasis();
                model.ListaEnfasis = context.Enfasis.ToList();
                model.ListaCarreras = context.Carrera.ToList();
                model.ListaUnidades = context.Unidad_Academica.ToList();
                var query = from u in model.ListaUnidades
                            join c in model.ListaCarreras on u.Codigo equals c.CodigoUnidadAcademica
                            join e in model.ListaEnfasis on c.Sigla equals e.SiglaCarrera
                            select new ViewEnfasis
                            {
                                unidad = u,
                                carrera = c,
                                enfasis = e
                            };

                return View("Ver Enfasis", query);
            }
        }
    }
}