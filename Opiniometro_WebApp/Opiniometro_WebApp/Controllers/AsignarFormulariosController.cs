using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Opiniometro_WebApp.Models;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using System.Globalization;

namespace Opiniometro_WebApp.Controllers
{
    [Authorize]
    public class AsignarFormulariosController : Controller
    {
        private Opiniometro_DatosEntities db = new Opiniometro_DatosEntities();

        //prueba para mocking tests
        public AsignarFormulariosController()
        {
            db = new Opiniometro_DatosEntities();
        }

        public AsignarFormulariosController(Opiniometro_DatosEntities db)
        {
            this.db = db;
        }

        public class TipoPeriodosIndicados { public string CodigoForm; public string FechaInicio; public string FechaFinal; };

        // Para la vista completa
        [HttpGet]
        public ActionResult Index()
        {
            var modelo = new AsignarFormulariosViewModel
            {
                Ciclos = ObtenerCiclos(""),
                UnidadesAcademicas = ObtenerUnidadAcademica(0, 0, ""),
                Enfasis = ObtenerEnfasis(null, null, "", ""),
                Carreras = ObtenerCarreras(null, null, ""),
                //Enfasis = ObtenerEnfasis(0, 0, "", ""),
                Cursos = ObtenerCursos(null, null, "", "", null),
                //Grupos = ObtenerGrupos(0, 0, "", "", "", "", 255, "", "" ,""),
                Formularios = ObtenerFormularios()
            };

            return View("Index", modelo);
        }

        [HttpPost]
        public ActionResult Index(string unidadAcademica, string siglaCarrera, string nombreCurso, string searchString, byte? semestre, short? ano)
        {
            if (!String.IsNullOrEmpty(Request.Form["changeAnno"]))
            {
                ano = Convert.ToInt16(Request.Form["changeAnno"]);
            }

            if (!String.IsNullOrEmpty(Request.Form["changeSemestre"]))
            {
                semestre = Convert.ToByte(Request.Form["changeSemestre"]);
            }

            if (!String.IsNullOrEmpty(Request.Form["changeUnidad"]))
            {
                unidadAcademica = Request.Form["changeUnidad"];
            }

            if (!String.IsNullOrEmpty(Request.Form["changeCarrera"]))
            {
                siglaCarrera = Request.Form["changeCarrera"];
            }

            var modelo = new AsignarFormulariosViewModel
            {
                Ciclos = ObtenerCiclos(unidadAcademica),
                UnidadesAcademicas = ObtenerUnidadAcademica(ano, semestre, unidadAcademica),
                Carreras = ObtenerCarreras(ano, semestre, unidadAcademica),
                Grupos = ObtenerGrupos(ano, semestre, unidadAcademica, siglaCarrera, 255, Request.Form["changeCurso"], searchString),
                Cursos = ObtenerCursos(ano, semestre, unidadAcademica, siglaCarrera, null),
                Formularios = ObtenerFormularios(),
                Enfasis = ObtenerEnfasis(ano,semestre, unidadAcademica, siglaCarrera)
            };

            return View(modelo);
        }

        public ActionResult AsignacionFormularioGrupo (List<ElegirGrupoEditorViewModel> grupos, List<ElegirFormularioEditorViewModel> formularios)
        {
            GruposYFormsSeleccionados gruFormsSeleccionados;
            if (grupos != null && formularios != null)
            {
                /// Fecha por defecto usada para aplicar un formulario. (Desde hoy en 1 semana hasta dentro de 2 semanas)
                Fecha_Corte fechaPorDefecto = new Fecha_Corte { FechaInicio = DateTime.Now.AddDays(7), FechaFinal = DateTime.Now.AddDays(14) };

                foreach (var form in formularios)
                {
                    form.FechaDeCorte = fechaPorDefecto;
                }

                gruFormsSeleccionados
                    = new GruposYFormsSeleccionados(grupos, formularios);
            }
            else
            {
                gruFormsSeleccionados = new GruposYFormsSeleccionados();
            }
            return PartialView("AsignacionFormularioGrupo", gruFormsSeleccionados);
        }

        // Para el filtro por ciclos
        public IQueryable<Ciclo_Lectivo> ObtenerCiclos(String codigoUnidadAcadem)
        {
            IQueryable<Ciclo_Lectivo> ciclo = (from c in db.Ciclo_Lectivo orderby c.Semestre select c);
            
            var listaEditable = new SelectList(ciclo, "Semestre", "Semestre").ToList();
            int index = 0;
            while (index < listaEditable.Count()-1)
            {
                if (listaEditable.ElementAt(index).Value == listaEditable.ElementAt(index+1).Value)
                {
                    listaEditable.RemoveAt(index + 1);
                }
                else
                {
                    ++index;
                }
            }
            //listaSemestres.ElementAt(1).Selected = true;
            ViewBag.semestre = listaEditable.AsEnumerable();

            ciclo = from c in ciclo orderby c.Anno select c;
            
            listaEditable = new SelectList(ciclo, "Anno", "Anno").ToList();
            index = 0;
            while (index < listaEditable.Count() - 1)
            {
                if (listaEditable.ElementAt(index).Value == listaEditable.ElementAt(index + 1).Value)
                {
                    listaEditable.RemoveAt(index + 1);
                }
                else
                {
                    ++index;
                }
            }

            //listaAnnos.ElementAt(1).Selected = true;
            ViewBag.ano = listaEditable.AsEnumerable();

            return ciclo;
        }

        // Para el filtro por Unidad Academica
        public IQueryable<Unidad_Academica> ObtenerUnidadAcademica(short? anno, byte? semestre, String codigoUnidadAcadem)
        {
            IQueryable<Unidad_Academica> unidadAcademica = from u in db.Unidad_Academica select u;
            ViewBag.unidadAcademica = new SelectList(unidadAcademica, "Codigo", "Nombre");
            return unidadAcademica;
        }

        // Para el filtro por carreras
        public IQueryable<Carrera> ObtenerCarreras(short? anno, byte? semestre, String codigoUnidadAcadem)
        {

            IQueryable<Carrera> siglaCarrera = from car in db.Carrera select car;

            if (!String.IsNullOrEmpty(codigoUnidadAcadem))
            {
                siglaCarrera = siglaCarrera.Where(c => c.CodigoUnidadAcademica.Equals(codigoUnidadAcadem));
            }

            ViewBag.siglaCarrera = new SelectList(siglaCarrera, "Sigla", "Nombre");
            return siglaCarrera;
        }

        //public IQueryable<Profesor> ObtenerProfesores() {

        //    IQueryable<Profesor> profesores = from prof in db.Profesor select prof;
        //        return profesores;
        //}


        // Para el filtro por énfasis
        public IQueryable<Enfasis> ObtenerEnfasis(short? anno, byte? semestre, String codigoUnidadAcadem, String siglaCarrera)
        {
            IQueryable<Enfasis> enfasis = from u in db.Enfasis select u;
            ViewBag.enfasis = new SelectList(enfasis, "Numero", "SiglaCarrera");
            return enfasis;
        }

        // Para el filtro por cursos
        /// <summary>
        /// Retorna la lista de cursos que pueden ser elegidos en el filtro de cursos.
        /// </summary>
        /// <param name="anno">Año del ciclo en los que se imparten los cursos.</param>
        /// <param name="semestre">Semestre del ciclo en el que se imparten los cursos.</param>
        /// <param name="codigoUnidadAcadem">Código de la unidad academica a la que pertenecen los cursos.</param>
        /// <param name="siglaCarrera">Sigla de la carrera en la que se encuentran los cursos.</param>
        /// <param name="numEnfasis">Número del énfasis de la carrera en el que se encuentran los cursos.</param>
        /// <returns>Lista de los cursos que satisfacen los filtros utilizados como parámetros.</returns>
        public IQueryable<Curso> ObtenerCursos(short? anno, byte? semestre, String codigoUnidadAcadem, String siglaCarrera, byte? numEnfasis)
        {
            IQueryable<Curso> nombreCurso = from c in db.Curso
                                            select c;
            List<Curso> cursos = new List<Curso>();

            if (semestre != null)
            {
                List<Curso> cursosSemestre = new List<Curso>();
                Opiniometro_DatosEntities opi = new Opiniometro_DatosEntities();
                var cur = opi.CursosSegunSemestre(semestre);
                
                    foreach (var c in cur)
                    {
                        Curso nuevo = new Curso();
                        nuevo.CodigoUnidad = c.CodigoUnidad;
                        nuevo.Tipo = c.Tipo;
                        nuevo.Sigla = c.Sigla;
                        nuevo.Nombre = c.Nombre;
                        cursosSemestre.Add(nuevo);
                    }

                cursos = cursosSemestre;
                nombreCurso = cursos.AsQueryable();
            }

            if (anno != null)
            {
                List<Curso> cursosAnno = new List<Curso>();
                Opiniometro_DatosEntities opi = new Opiniometro_DatosEntities();
                var cur = opi.CursosSegunAnno(anno);
                if (semestre == null)
                {               
                    foreach (var c in cur)
                    {
                        Curso nuevo = new Curso();
                        nuevo.CodigoUnidad = c.CodigoUnidad;
                        nuevo.Tipo = c.Tipo;
                        nuevo.Sigla = c.Sigla;
                        nuevo.Nombre = c.Nombre;
                        cursosAnno.Add(nuevo);
                    }
                }
                //Si ya se seleccionó algún otro filtro, se restringe la lista de cursos de acuerdo al mismo
                else
                {
                    foreach (var c in cur)
                    {
                        Curso nuevo = new Curso();
                        nuevo = cursos.Find(cu => cu.Sigla.Equals(c.Sigla));
                        if(nuevo != null)
                        {
                            cursosAnno.Add(nuevo);
                        }
                    }  
                }
                cursos = cursosAnno;
                nombreCurso = cursos.AsQueryable();
            }                          

            if (!String.IsNullOrEmpty(codigoUnidadAcadem))
            {
                if(semestre == null && anno == null)
                {
                    var uni = from c in db.Curso
                              where c.CodigoUnidad.Equals(codigoUnidadAcadem)
                              select c;

                    cursos = uni.ToList();
                }
                //Si ya se seleccionó algún otro filtro, se restringe la lista de cursos de acuerdo al mismo
                else
                {
                    cursos = cursos.Where(c => c.CodigoUnidad.Equals(codigoUnidadAcadem)).ToList();
                }
                nombreCurso = cursos.AsQueryable();
            }

            if (!String.IsNullOrEmpty(siglaCarrera))
            {
                List<Curso> cursosCarrera = new List<Curso>();
                Opiniometro_DatosEntities opi = new Opiniometro_DatosEntities();
                var cur = opi.CursosSegunCarrera(siglaCarrera);

                if(semestre == null && anno == null && String.IsNullOrEmpty(codigoUnidadAcadem))
                {
                    foreach (var c in cur)
                    {
                        Curso nuevo = new Curso();
                        nuevo.CodigoUnidad = c.CodigoUnidad;
                        nuevo.Tipo = c.Tipo;
                        nuevo.Sigla = c.Sigla;
                        nuevo.Nombre = c.Nombre;
                        cursosCarrera.Add(nuevo);
                    }
                }
                //Si ya se seleccionó algún otro filtro, se restringe la lista de cursos de acuerdo al mismo
                else
                {
                    foreach (var c in cur)
                    {
                        Curso nuevo = new Curso();
                        nuevo = cursos.Find(cu => cu.Sigla.Equals(c.Sigla));
                        if (nuevo != null)
                        {
                            cursosCarrera.Add(nuevo);
                        }
                    }
                }

                cursos = cursosCarrera;
                nombreCurso = cursos.AsQueryable();
            }

            ViewBag.nombreCurso = new SelectList(nombreCurso, "Nombre", "Nombre");
            return nombreCurso;
        }

        /// <summary>
        /// Retorna la lista de grupos de cursos que se mostrarán.
        /// </summary>
        /// <param name="ciclo">Ciclo en los que se imparten los grupos.</param>
        /// <param name="codigoUnidadAcadem">Código de la unidad academica a la que pertenecen los cursos de los grupos.</param>
        /// <param name="siglaCarrera">Sigla de la carrera en la que se encuentran los cursos de los grupos.</param>
        /// <param name="numEnfasis">Número del énfasis de la carrera en el que se encuentran los cursos de los grupos.</param>
        /// <param name="siglaCurso">Sigla del curso al que pertenecen los grupos</param>
        /// <returns>Lista de los grupos que satisfacen los filtros utilizados como parámetros.</returns>
        public List<ElegirGrupoEditorViewModel> ObtenerGrupos(short? anno, byte? semestre, String codigoUnidadAcadem,
             String siglaCarrera, byte? numEnfasis, string nombreCurso, String searchString)
        {
            // En la base, cuando este query se transforme en un proc. almacenado, se deberá usar join con la tabla curso
            IQueryable<ElegirGrupoEditorViewModel> grupos =
                (from cur in db.Curso
                join gru in db.Grupo on cur.Sigla equals gru.SiglaCurso
                join uni in db.Unidad_Academica on cur.CodigoUnidad equals uni.Codigo
                //join car in db.Carrera on uni.Codigo equals car.CodigoUnidadAcademica

            select new ElegirGrupoEditorViewModel
            {
                Seleccionado = false,
                SiglaCurso = cur.Sigla,
                Numero = gru.Numero,
                Anno = gru.AnnoGrupo,
                Semestre = gru.SemestreGrupo,
                Profesores = gru.Profesor.ToList(),
                NombreCurso = gru.Curso.Nombre,
                NombreUnidadAcademica = gru.Curso.Unidad_Academica.Nombre,
                CodigoUnidadAcademica = cur.CodigoUnidad,
                Enfasis = cur.Enfasis.ToList()
            });
            grupos = FiltreGrupos(searchString, semestre, anno, codigoUnidadAcadem, siglaCarrera, nombreCurso, grupos);

            return grupos.ToList();
        }

        /// <summary>
        /// filtra la lista de grupos
        /// </summary>
        /// <param name="searchString"> string que podria contener el nombre del curso que se ingreso para buscar</param>
        /// <param name="semestre"> semestre podria contener el semestre que se indico en el filtro </param>
        /// <param name="codigoUnidadAcad"> podria contener el nombre de la unidad academica</param>
        /// <param name="codigoCarrera"> podria contener el nombre de la carrera</param>
        /// <param name="grupos"> lista de grupos que se envia desde el metodo ObtenerGrupos</param>
        /// <returns> los grupos filtrados</returns>
        public IQueryable<ElegirGrupoEditorViewModel> FiltreGrupos(string searchString, byte? semestre, short? anno, string CodigoUnidadAcad, string siglaCarrera, string nombCurso ,IQueryable<ElegirGrupoEditorViewModel> grupos)
        {
            if (!String.IsNullOrEmpty(searchString))
            {
                grupos = grupos.Where(c => c.NombreCurso.Contains(searchString));
            }

            if (!String.IsNullOrEmpty(CodigoUnidadAcad))
            {
                grupos = grupos.Where(c => c.CodigoUnidadAcademica.Equals(CodigoUnidadAcad));
            }

            /*if (!String.IsNullOrEmpty(siglaCarrera))
            {
                //grupos = grupos.Where(c => c.SiglaCarrera.Equals(siglaCarrera));
                var cs = from carr in db.Carrera where carr.Sigla == siglaCarrera select new Carrera { CodigoUnidadAcademica=carr.CodigoUnidadAcademica,  };
                Carrera carrera = 
                grupos = grupos.Where(c => c.Enfasis.Contains())
            }*/
            if (!String.IsNullOrEmpty(siglaCarrera))
            {
                foreach (var gru in grupos)
                {
                    bool grupoEstaEnLaCarrera = false;
                    foreach (var enf in gru.Enfasis)
                    {
                        if (enf.SiglaCarrera == siglaCarrera)
                        {
                            grupoEstaEnLaCarrera = true;
                            break;
                        }
                    }
                    // Remueve el grupo de un curso que no está en la carrera seleccionada
                    if (!grupoEstaEnLaCarrera)
                    {
                        grupos = grupos.Where(g => g != gru);
                    }
                }
            }

            //if (!String.IsNullOrEmpty(nombCarrera))
            //{
            //    var carrera = (from c in db.Carrera
            //                   select new { Sigla = c.Sigla, Nombre = c.Nombre }
            //                    ).Where(c => c.Nombre == nombCarrera);
            //    string SiglaCarr = carrera.First().Sigla;
            //}

            if (!String.IsNullOrEmpty(nombCurso))
            {
                grupos = grupos.Where(c => c.NombreCurso.Contains(nombCurso));
            }

            if (semestre != null)
            {
                grupos = grupos.Where(c => c.Semestre == semestre);
            }

            if (anno != null)
            {
                grupos = grupos.Where(c => c.Anno == anno);
            }

            return grupos;
        }

        // Para la vista de los formularios
        public List<ElegirFormularioEditorViewModel> ObtenerFormularios()
        {
            List<ElegirFormularioEditorViewModel> formularios =
                (from formul in db.Formulario
                select new ElegirFormularioEditorViewModel
                {
                    Seleccionado = false,
                    CodigoFormulario = formul.CodigoFormulario,
                    NombreFormulario = formul.Nombre,
                    FechaDeCorte = null
                }).ToList();

            return formularios;
        }

        public ActionResult SeleccionFormularios(string formulario)
        {
            IQueryable<Formulario> form = from f in db.Formulario select f;

            if (!String.IsNullOrEmpty(formulario))
            {
                form = form.Where(f => f.Nombre.Contains(formulario));
            }

            return PartialView("SeleccionFormularios", form);
        }

        
        //Efecto: Envía un correo al estudiante notificandole que existe un formulario que debe llenar.
        //Requiere: el correo de la persona que va a recibir el recordatorio, el asunto del mensaje y el contenido del correo.
        //Modifica: N/A.
        private void EnviarCorreo(string correo_receptor, string asunto, string contenido)
        {
            string correo_emisor = System.Configuration.ConfigurationManager.AppSettings["CorreoEmisor"].ToString();
            string contrasenna = System.Configuration.ConfigurationManager.AppSettings["ContrasennaEmisor"].ToString();

            SmtpClient cliente = new SmtpClient("smtp.gmail.com", 587);
            cliente.EnableSsl = true;
            cliente.Timeout = 100000;
            cliente.DeliveryMethod = SmtpDeliveryMethod.Network;
            cliente.UseDefaultCredentials = false;
            cliente.Credentials = new NetworkCredential(correo_emisor, contrasenna);

            MailMessage correo = new MailMessage(correo_emisor, correo_receptor, asunto, contenido);
            correo.IsBodyHtml = true;
            correo.BodyEncoding = UTF8Encoding.UTF8;
            cliente.Send(correo);

        }

        //public ActionResult EnviarCorreoFormulario()
        //{
        //    return View("Index");
        //}

        //Efecto: Envía un correo a los estudiantes matriculados en cierto curso.
        //Requiere: la sigla del curso.
        //Modifica: N/A.
        public void EnviarCorreoFormulario(Curso curso)
        {
            List<Usuario> usuarios = (from u in db.Usuario
                                      select u).ToList();

            List<Persona> personas = (from p in db.Persona
                                      select p).ToList();

            var matriculados = from m in db.Matricula
                               where m.Sigla.Equals(curso.Sigla)
                               select m;
            foreach (var ma in matriculados)
            {
                Usuario usuario = new Usuario();
                usuario = usuarios.Find(us => us.Cedula.Equals(ma.CedulaEstudiante));

                Persona persona = new Persona();
                persona = personas.Find(p => p.Cedula.Equals(ma.CedulaEstudiante));

                if(usuario != null && persona != null)
                {
                    string contenido = "<p>Estimado/a " + persona.Nombre1 + " " + persona.Apellido1 + ", se le solicita dedicar unos minutos de su tiempo para evaluar los cursos" +
                                   " en los cuales se encuentra matriculado/a. Favor ingresar a  Opiniómetro@UCR.</p> <b>";

                    // Se le envía al usuario el correo con la notificación del formulario 
                    EnviarCorreo(usuario.CorreoInstitucional, "Evaluación pendiente - Opiniómetro@UCR", contenido);
                }              
            }           
        }
     

        [HttpPost]
        public string EfectuarAsignaciones(string Grupos, string PeriodosIndicados)
        {
            string mensajes = "";
            int numErrores = 0;
            var FormulariosConPeriodos = JsonConvert.DeserializeObject<TipoPeriodosIndicados[]>(PeriodosIndicados);
            var GruposEnLista = JsonConvert.DeserializeObject<Grupo[]>(Grupos);
            DateTime ahora = DateTime.Now;

            //Almacena los cursos a los que se le deben enviar los correos.
            List<Curso> CursosCorreos = new List<Curso>();    

            List<Tiene_Grupo_Formulario> asignaciones = new List<Tiene_Grupo_Formulario>();
            foreach (var fcp in FormulariosConPeriodos)
            {
                DateTime inicioPeriodo = new DateTime(), finPeriodo = new DateTime();
                bool fechaIEsCorrecta = DateTime.TryParseExact(fcp.FechaInicio, "yyyy-MM-ddThh:mm", CultureInfo.CurrentCulture, DateTimeStyles.None, out inicioPeriodo);
                bool fechaFEsCorrecta = DateTime.TryParseExact(fcp.FechaFinal, "yyyy-MM-ddThh:mm", CultureInfo.CurrentCulture, DateTimeStyles.None, out finPeriodo);

                //bool fechaIEsCorrecta = DateTime.TryParse(fcp.FechaInicio, CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal, out inicioPeriodo);
                //bool fechaFEsCorrecta = DateTime.TryParse(fcp.FechaFinal, CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal, out finPeriodo);

                if (fechaIEsCorrecta && fechaFEsCorrecta)
                {
                    //Debug.WriteLine("Fecha inicio: {0}\nFecha fin: {1}\n\n", inicioPeriodo.ToString(), finPeriodo.ToString());

                    if (ahora < inicioPeriodo)
                    {
                        if (inicioPeriodo < finPeriodo)
                        {

                            if ((from fc in db.Fecha_Corte
                                 where (fc.FechaInicio == inicioPeriodo && fc.FechaFinal == finPeriodo)
                                 select fc).Count() == 0)
                            {
                                if (ModelState.IsValid)
                                {
                                    db.Fecha_Corte.Add(new Fecha_Corte { FechaInicio = inicioPeriodo, FechaFinal = finPeriodo });
                                    db.SaveChanges();
                                }
                            }


                            foreach (var g in GruposEnLista)
                            {
                                asignaciones.Add(new Tiene_Grupo_Formulario
                                {
                                    SiglaCurso = g.SiglaCurso,
                                    Numero = g.Numero,
                                    Anno = g.AnnoGrupo,
                                    Ciclo = g.SemestreGrupo,
                                    Codigo = fcp.CodigoForm,
                                    FechaInicio = inicioPeriodo,
                                    FechaFinal = finPeriodo
                                });
                                //Para enviar los correos a los cursos respectivos
                                Curso curso = db.Curso.Find(g.SiglaCurso);
                                CursosCorreos.Add(curso);
                            }
                        }
                        else // El periodo inicia después de que termina
                        {
                            ++numErrores;
                            mensajes += "- El inicio del periodo para el formulario " + fcp.CodigoForm + " debe corresponder a una fecha anterior al final del mismo periodo.\n";
                        }
                    }
                    else // Inicio del periodo NO es posterior a la fecha actual
                    {
                        ++numErrores;
                        mensajes += "- El periodo para el formulario " + fcp.CodigoForm + " debe comenzar en una fecha posterior a la actual\n";
                    }
                }
                else
                {
                    //Debug.Write("\n\nFecha incorrecta /\n\n");
                    ++numErrores;
                    mensajes += "- Ingrese correctamente el periodo de aplicación para el formulario " + fcp.CodigoForm + "\n";
                }
            }

            if (numErrores > 0)
            {
                mensajes += "\nPor favor corrija lo indicado antes de realizar las asignaciones.\n";
            }
            else
            {
                if (ModelState.IsValid)
                {
                    db.Tiene_Grupo_Formulario.AddRange(asignaciones);
                    db.SaveChanges();

                    //Se envían los correos
                    foreach (var e in CursosCorreos)
                    {
                        EnviarCorreoFormulario(e);
                    }
                }  
                else
                {
                    mensajes += "Hubo un error al guardar las asignaciones. Por favor contacte a soporte técnico.\n";
                }
            }

            return JsonConvert.SerializeObject(mensajes == ""? null : mensajes);
        }
    }
}