﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Opiniometro_WebApp.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class Opiniometro_DatosEntities : DbContext
    {
        public Opiniometro_DatosEntities()
            : base("name=Opiniometro_DatosEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<C__RefactorLog> C__RefactorLog { get; set; }
        public virtual DbSet<Administrativo> Administrativo { get; set; }
        public virtual DbSet<Carrera> Carrera { get; set; }
        public virtual DbSet<Categoria> Categoria { get; set; }
        public virtual DbSet<Ciclo_Lectivo> Ciclo_Lectivo { get; set; }
        public virtual DbSet<Conformado_Item_Sec_Form> Conformado_Item_Sec_Form { get; set; }
        public virtual DbSet<Curso> Curso { get; set; }
        public virtual DbSet<Empadronado> Empadronado { get; set; }
        public virtual DbSet<Enfasis> Enfasis { get; set; }
        public virtual DbSet<Estudiante> Estudiante { get; set; }
        public virtual DbSet<Facultad> Facultad { get; set; }
        public virtual DbSet<Fecha_Corte> Fecha_Corte { get; set; }
        public virtual DbSet<Formulario> Formulario { get; set; }
        public virtual DbSet<Formulario_Respuesta> Formulario_Respuesta { get; set; }
        public virtual DbSet<Grupo> Grupo { get; set; }
        public virtual DbSet<Imparte> Imparte { get; set; }
        public virtual DbSet<Item> Item { get; set; }
        public virtual DbSet<Matricula> Matricula { get; set; }
        public virtual DbSet<Opciones_De_Respuestas_Seleccion_Unica> Opciones_De_Respuestas_Seleccion_Unica { get; set; }
        public virtual DbSet<Perfil> Perfil { get; set; }
        public virtual DbSet<Permiso> Permiso { get; set; }
        public virtual DbSet<Persona> Persona { get; set; }
        public virtual DbSet<Posee_Enfasis_Perfil_Permiso> Posee_Enfasis_Perfil_Permiso { get; set; }
        public virtual DbSet<Profesor> Profesor { get; set; }
        public virtual DbSet<Responde> Responde { get; set; }
        public virtual DbSet<Seccion> Seccion { get; set; }
        public virtual DbSet<Seleccion_Unica> Seleccion_Unica { get; set; }
        public virtual DbSet<TelefonoPersona> TelefonoPersona { get; set; }
        public virtual DbSet<Texto_Libre> Texto_Libre { get; set; }
        public virtual DbSet<Tiene_Grupo_Formulario> Tiene_Grupo_Formulario { get; set; }
        public virtual DbSet<Tiene_Usuario_Perfil_Enfasis> Tiene_Usuario_Perfil_Enfasis { get; set; }
        public virtual DbSet<Unidad_Academica> Unidad_Academica { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }
    
        public virtual ObjectResult<DatosEstudiante_Result> DatosEstudiante(string cedula)
        {
            var cedulaParameter = cedula != null ?
                new ObjectParameter("Cedula", cedula) :
                new ObjectParameter("Cedula", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<DatosEstudiante_Result>("DatosEstudiante", cedulaParameter);
        }
    
        public virtual ObjectResult<MostrarEstudiantes_Result> MostrarEstudiantes()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<MostrarEstudiantes_Result>("MostrarEstudiantes");
        }
    
        public virtual ObjectResult<string> NombrePersona(string cedula)
        {
            var cedulaParameter = cedula != null ?
                new ObjectParameter("Cedula", cedula) :
                new ObjectParameter("Cedula", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("NombrePersona", cedulaParameter);
        }
    
        public virtual ObjectResult<string> ObtenerPerfilPorDefecto(string correo)
        {
            var correoParameter = correo != null ?
                new ObjectParameter("correo", correo) :
                new ObjectParameter("correo", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("ObtenerPerfilPorDefecto", correoParameter);
        }
    
        public virtual ObjectResult<string> ObtenerPerfilUsuario(string correo)
        {
            var correoParameter = correo != null ?
                new ObjectParameter("correo", correo) :
                new ObjectParameter("correo", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("ObtenerPerfilUsuario", correoParameter);
        }
    
        public virtual int SP_AgregarPersonaUsuario(string correo, string contrasenna, string cedula, string nombre, string apellido1, string apellido2, string direccion)
        {
            var correoParameter = correo != null ?
                new ObjectParameter("Correo", correo) :
                new ObjectParameter("Correo", typeof(string));
    
            var contrasennaParameter = contrasenna != null ?
                new ObjectParameter("Contrasenna", contrasenna) :
                new ObjectParameter("Contrasenna", typeof(string));
    
            var cedulaParameter = cedula != null ?
                new ObjectParameter("Cedula", cedula) :
                new ObjectParameter("Cedula", typeof(string));
    
            var nombreParameter = nombre != null ?
                new ObjectParameter("Nombre", nombre) :
                new ObjectParameter("Nombre", typeof(string));
    
            var apellido1Parameter = apellido1 != null ?
                new ObjectParameter("Apellido1", apellido1) :
                new ObjectParameter("Apellido1", typeof(string));
    
            var apellido2Parameter = apellido2 != null ?
                new ObjectParameter("Apellido2", apellido2) :
                new ObjectParameter("Apellido2", typeof(string));
    
            var direccionParameter = direccion != null ?
                new ObjectParameter("Direccion", direccion) :
                new ObjectParameter("Direccion", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_AgregarPersonaUsuario", correoParameter, contrasennaParameter, cedulaParameter, nombreParameter, apellido1Parameter, apellido2Parameter, direccionParameter);
        }
    
        public virtual int SP_AgregarUsuario(string correo, string contrasenna, string cedula)
        {
            var correoParameter = correo != null ?
                new ObjectParameter("Correo", correo) :
                new ObjectParameter("Correo", typeof(string));
    
            var contrasennaParameter = contrasenna != null ?
                new ObjectParameter("Contrasenna", contrasenna) :
                new ObjectParameter("Contrasenna", typeof(string));
    
            var cedulaParameter = cedula != null ?
                new ObjectParameter("Cedula", cedula) :
                new ObjectParameter("Cedula", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_AgregarUsuario", correoParameter, contrasennaParameter, cedulaParameter);
        }
    
        public virtual int SP_CambiarContrasenna(string correo, string contrasenna_Nueva)
        {
            var correoParameter = correo != null ?
                new ObjectParameter("Correo", correo) :
                new ObjectParameter("Correo", typeof(string));
    
            var contrasenna_NuevaParameter = contrasenna_Nueva != null ?
                new ObjectParameter("Contrasenna_Nueva", contrasenna_Nueva) :
                new ObjectParameter("Contrasenna_Nueva", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_CambiarContrasenna", correoParameter, contrasenna_NuevaParameter);
        }
    
        public virtual ObjectResult<SP_ContarRespuestasPorGrupo_Result> SP_ContarRespuestasPorGrupo(string codigoFormulario, string cedulaProfesor, Nullable<short> annoGrupo, Nullable<byte> semestreGrupo, Nullable<byte> numeroGrupo, string siglaCurso, string itemId)
        {
            var codigoFormularioParameter = codigoFormulario != null ?
                new ObjectParameter("codigoFormulario", codigoFormulario) :
                new ObjectParameter("codigoFormulario", typeof(string));
    
            var cedulaProfesorParameter = cedulaProfesor != null ?
                new ObjectParameter("cedulaProfesor", cedulaProfesor) :
                new ObjectParameter("cedulaProfesor", typeof(string));
    
            var annoGrupoParameter = annoGrupo.HasValue ?
                new ObjectParameter("annoGrupo", annoGrupo) :
                new ObjectParameter("annoGrupo", typeof(short));
    
            var semestreGrupoParameter = semestreGrupo.HasValue ?
                new ObjectParameter("semestreGrupo", semestreGrupo) :
                new ObjectParameter("semestreGrupo", typeof(byte));
    
            var numeroGrupoParameter = numeroGrupo.HasValue ?
                new ObjectParameter("numeroGrupo", numeroGrupo) :
                new ObjectParameter("numeroGrupo", typeof(byte));
    
            var siglaCursoParameter = siglaCurso != null ?
                new ObjectParameter("siglaCurso", siglaCurso) :
                new ObjectParameter("siglaCurso", typeof(string));
    
            var itemIdParameter = itemId != null ?
                new ObjectParameter("itemId", itemId) :
                new ObjectParameter("itemId", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_ContarRespuestasPorGrupo_Result>("SP_ContarRespuestasPorGrupo", codigoFormularioParameter, cedulaProfesorParameter, annoGrupoParameter, semestreGrupoParameter, numeroGrupoParameter, siglaCursoParameter, itemIdParameter);
        }
    
        public virtual ObjectResult<string> SP_DevolverObservacionesPorGrupo(string codigoFormulario, string cedulaProfesor, Nullable<short> annoGrupo, Nullable<byte> semestreGrupo, Nullable<byte> numeroGrupo, string siglaCurso, string itemId)
        {
            var codigoFormularioParameter = codigoFormulario != null ?
                new ObjectParameter("codigoFormulario", codigoFormulario) :
                new ObjectParameter("codigoFormulario", typeof(string));
    
            var cedulaProfesorParameter = cedulaProfesor != null ?
                new ObjectParameter("cedulaProfesor", cedulaProfesor) :
                new ObjectParameter("cedulaProfesor", typeof(string));
    
            var annoGrupoParameter = annoGrupo.HasValue ?
                new ObjectParameter("annoGrupo", annoGrupo) :
                new ObjectParameter("annoGrupo", typeof(short));
    
            var semestreGrupoParameter = semestreGrupo.HasValue ?
                new ObjectParameter("semestreGrupo", semestreGrupo) :
                new ObjectParameter("semestreGrupo", typeof(byte));
    
            var numeroGrupoParameter = numeroGrupo.HasValue ?
                new ObjectParameter("numeroGrupo", numeroGrupo) :
                new ObjectParameter("numeroGrupo", typeof(byte));
    
            var siglaCursoParameter = siglaCurso != null ?
                new ObjectParameter("siglaCurso", siglaCurso) :
                new ObjectParameter("siglaCurso", typeof(string));
    
            var itemIdParameter = itemId != null ?
                new ObjectParameter("itemId", itemId) :
                new ObjectParameter("itemId", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("SP_DevolverObservacionesPorGrupo", codigoFormularioParameter, cedulaProfesorParameter, annoGrupoParameter, semestreGrupoParameter, numeroGrupoParameter, siglaCursoParameter, itemIdParameter);
        }
    
        public virtual int SP_ExistenciaCorreo(string correo, ObjectParameter resultado)
        {
            var correoParameter = correo != null ?
                new ObjectParameter("Correo", correo) :
                new ObjectParameter("Correo", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_ExistenciaCorreo", correoParameter, resultado);
        }
    
        public virtual int SP_LoginUsuario(string correo, string contrasenna, ObjectParameter resultado)
        {
            var correoParameter = correo != null ?
                new ObjectParameter("Correo", correo) :
                new ObjectParameter("Correo", typeof(string));
    
            var contrasennaParameter = contrasenna != null ?
                new ObjectParameter("Contrasenna", contrasenna) :
                new ObjectParameter("Contrasenna", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_LoginUsuario", correoParameter, contrasennaParameter, resultado);
        }
    
        public virtual int SP_ModificarPersona(string cedulaBusqueda, string cedula, string nombre, string apellido1, string apellido2, string correo, string direccion)
        {
            var cedulaBusquedaParameter = cedulaBusqueda != null ?
                new ObjectParameter("CedulaBusqueda", cedulaBusqueda) :
                new ObjectParameter("CedulaBusqueda", typeof(string));
    
            var cedulaParameter = cedula != null ?
                new ObjectParameter("Cedula", cedula) :
                new ObjectParameter("Cedula", typeof(string));
    
            var nombreParameter = nombre != null ?
                new ObjectParameter("Nombre", nombre) :
                new ObjectParameter("Nombre", typeof(string));
    
            var apellido1Parameter = apellido1 != null ?
                new ObjectParameter("Apellido1", apellido1) :
                new ObjectParameter("Apellido1", typeof(string));
    
            var apellido2Parameter = apellido2 != null ?
                new ObjectParameter("Apellido2", apellido2) :
                new ObjectParameter("Apellido2", typeof(string));
    
            var correoParameter = correo != null ?
                new ObjectParameter("Correo", correo) :
                new ObjectParameter("Correo", typeof(string));
    
            var direccionParameter = direccion != null ?
                new ObjectParameter("Direccion", direccion) :
                new ObjectParameter("Direccion", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_ModificarPersona", cedulaBusquedaParameter, cedulaParameter, nombreParameter, apellido1Parameter, apellido2Parameter, correoParameter, direccionParameter);
        }
    
        public virtual ObjectResult<SP_ObtenerPermisosUsuario_Result> SP_ObtenerPermisosUsuario(string correo, string perfil)
        {
            var correoParameter = correo != null ?
                new ObjectParameter("Correo", correo) :
                new ObjectParameter("Correo", typeof(string));
    
            var perfilParameter = perfil != null ?
                new ObjectParameter("Perfil", perfil) :
                new ObjectParameter("Perfil", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_ObtenerPermisosUsuario_Result>("SP_ObtenerPermisosUsuario", correoParameter, perfilParameter);
        }
    
        public virtual ObjectResult<CursosSegunCarrera_Result> CursosSegunCarrera(string siglaCarrera)
        {
            var siglaCarreraParameter = siglaCarrera != null ?
                new ObjectParameter("siglaCarrera", siglaCarrera) :
                new ObjectParameter("siglaCarrera", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<CursosSegunCarrera_Result>("CursosSegunCarrera", siglaCarreraParameter);
        }
    }
}
