using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Opiniometro_WebApp.Models;
using Moq;

namespace Opiniometro_WebAppTest.Controllers
{
    [TestClass]
    public class Formulario_RespuestaControllerTest
    {
        [TestMethod]
        public void TestIndexViewDataMock()
        {
            //ARRANGE
            var formularios = new List<Formulario_Respuesta>
            {
                new Formulario_Respuesta()
                    { Fecha = DateTime.Now, CodigoFormulario = , CedulaPersona = , CedulaProfesor =,AnnoGrupo =,SemestreGrupo =, NumeroGrupo=, SiglaGrupo =, Completado= },
                new Formulario_Respuesta()
                    { Fecha = DateTime.Now, CodigoFormulario = , CedulaPersona = , CedulaProfesor =,AnnoGrupo =,SemestreGrupo =, NumeroGrupo=, SiglaGrupo =, Completado= },
                new Formulario_Respuesta()
                    { Fecha = DateTime.Now, CodigoFormulario = , CedulaPersona = , CedulaProfesor =,AnnoGrupo =,SemestreGrupo =, NumeroGrupo=, SiglaGrupo =, Completado= },
                new Formulario_Respuesta()
                    { Fecha = DateTime.Now, CodigoFormulario = , CedulaPersona = , CedulaProfesor =,AnnoGrupo =,SemestreGrupo =, NumeroGrupo=, SiglaGrupo =, Completado= },
                new Formulario_Respuesta()
                    { Fecha = DateTime.Now, CodigoFormulario = , CedulaPersona = , CedulaProfesor =,AnnoGrupo =,SemestreGrupo =, NumeroGrupo=, SiglaGrupo =, Completado= }
            }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Formulario_Respuesta>>();

            var mockDb = new Mock<Opiniometro_DatosEntities>();

            //mockDb.Setup(m => m.Formulario_Respuesta.Find()).Returns(Formulario_Respuesta);
            //Act

            //ASSERT
        }
    }
}
