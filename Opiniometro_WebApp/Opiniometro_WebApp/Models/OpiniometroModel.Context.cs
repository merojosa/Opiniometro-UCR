﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
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
        public virtual DbSet<Perfil> Perfil { get; set; }
        public virtual DbSet<Permiso> Permiso { get; set; }
        public virtual DbSet<Persona> Persona { get; set; }
        public virtual DbSet<Posee_Enfasis_Perfil_Permiso> Posee_Enfasis_Perfil_Permiso { get; set; }
        public virtual DbSet<Preguntas> Preguntas { get; set; }
        public virtual DbSet<Profesor> Profesor { get; set; }
        public virtual DbSet<Responde> Responde { get; set; }
        public virtual DbSet<Seccion> Seccion { get; set; }
        public virtual DbSet<Seleccion_Unica> Seleccion_Unica { get; set; }
        public virtual DbSet<TelefonoPersona> TelefonoPersona { get; set; }
        public virtual DbSet<Texto_Libre> Texto_Libre { get; set; }
        public virtual DbSet<Tiene_Grupo_Formulario> Tiene_Grupo_Formulario { get; set; }
        public virtual DbSet<Unidad_Academica> Unidad_Academica { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }
    
        public virtual ObjectResult<BuscarCursoPorNombre_Result> BuscarCursoPorNombre(string nombreCurso)
        {
            var nombreCursoParameter = nombreCurso != null ?
                new ObjectParameter("NombreCurso", nombreCurso) :
                new ObjectParameter("NombreCurso", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<BuscarCursoPorNombre_Result>("BuscarCursoPorNombre", nombreCursoParameter);
        }
    
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
    }
}
