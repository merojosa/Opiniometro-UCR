using System;
using System.Web.Mvc;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Opiniometro_WebApp.Controllers;
using Opiniometro_WebApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace Opiniometro_WebAppTest.Controllers
{
    [TestClass]
    public class FormularioControllerTest
    {
        [TestMethod]
        public void TestCreateViewDataMock()
        {
            // Arrange
            var mockDb = new Mock<Opiniometro_DatosEntities>();
            string codigo = "100001";
            Formulario formulario = new Formulario() { CodigoFormulario = "100001", Nombre = "Programación I" };
            mockDb.Setup(m => m.Formulario.Find(codigo)).Returns(formulario);
            FormularioController controller = new FormularioController(mockDb.Object);

            // Act
            controller.Create(formulario);
            ViewResult result = controller.Details(codigo) as ViewResult;

            // Assert
            Assert.AreEqual(result.Model, formulario);
        }

        [TestMethod]
        public void TestEditViewDataMock()
        {
            // Arrange
            var mockDb = new Mock<Opiniometro_DatosEntities>();
            string codigo = "100001";
            Formulario formulario = new Formulario() { CodigoFormulario = "100001", Nombre = "Programación I" };
            mockDb.Setup(m => m.Formulario.Find(codigo)).Returns(formulario);
            FormularioController controller = new FormularioController(mockDb.Object);

            // Act
            ViewResult result = controller.Edit(codigo) as ViewResult;

            // Assert
            Assert.AreEqual(result.Model, formulario);
        }

        [TestMethod]
        public void TestEditViewDataMock2()
        {
            // Arrange
            var mockDb = new Mock<Opiniometro_DatosEntities>();
            string codigo = "100001";
            Formulario formulario = new Formulario() { CodigoFormulario = "100001", Nombre = "Programación I" };
            mockDb.Setup(m => m.Formulario.Find(codigo)).Returns(formulario);
            FormularioController controller = new FormularioController(mockDb.Object);

            // Act
            controller.Edit(formulario);
            ViewResult result = controller.Details(codigo) as ViewResult;

            // Assert
            Assert.AreEqual(result.Model, formulario);
        }

        [TestMethod]
        public void TestDetailsViewDataMock()
        {
            // Arrange
            var mockDb = new Mock<Opiniometro_DatosEntities>();
            string codigo = "100001";
            Formulario formulario = new Formulario() { CodigoFormulario = "100001", Nombre = "Programación I" };
            mockDb.Setup(m => m.Formulario.Find(codigo)).Returns(formulario);
            FormularioController controller = new FormularioController(mockDb.Object);
            
            // Act
            ViewResult result = controller.Details(codigo) as ViewResult;

            // Assert
            Assert.AreEqual(result.Model, formulario);
        }

        [TestMethod]
        public void TestDeleteViewDataMock()
        {
            // Arrange
            var mockDb = new Mock<Opiniometro_DatosEntities>();
            string codigo = "100001";
            Formulario formulario = new Formulario() { CodigoFormulario = "100001", Nombre = "Programación I" };
            mockDb.Setup(m => m.Formulario.Find(codigo)).Returns(formulario);
            FormularioController controller = new FormularioController(mockDb.Object);

            // Act
            ViewResult result = controller.Delete(codigo) as ViewResult;

            // Assert
            Assert.AreEqual(result.Model, formulario);
        }

        [TestMethod]
        public void TestIndexViewDataMock()
        {
            // Arrange
            var formularios = new List<Formulario>
            {
                new Formulario() { CodigoFormulario = "100001", Nombre = "Programación I" },
                new Formulario() { CodigoFormulario = "100002", Nombre = "Programación II" },
                new Formulario() { CodigoFormulario = "100003", Nombre = "Bases de Datos" },
                new Formulario() { CodigoFormulario = "100004", Nombre = "Ingeniería de Software" }
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Formulario>>();

            mockDbSet.As<IQueryable<Formulario>>().Setup(m => m.Provider).Returns(formularios.Provider);
            mockDbSet.As<IQueryable<Formulario>>().Setup(m => m.Expression).Returns(formularios.Expression);
            mockDbSet.As<IQueryable<Formulario>>().Setup(m => m.ElementType).Returns(formularios.ElementType);
            mockDbSet.As<IQueryable<Formulario>>().Setup(m => m.GetEnumerator()).Returns(formularios.GetEnumerator());

            var mockDb = new Mock<Opiniometro_DatosEntities>();
            mockDb.Setup(m => m.Formulario).Returns(mockDbSet.Object);
            FormularioController controller = new FormularioController(mockDb.Object);

            // Act
            ViewResult result = controller.Index() as ViewResult;
            List<Formulario> formulario = (List<Formulario>)result.ViewData.Model;
            // Assert
            Assert.AreEqual(4, formulario.Count);
        }

        [TestMethod]
        public void TestIndexNotNullAndView()
        {
            FormularioController controller = new FormularioController();
            ViewResult result = controller.Index() as ViewResult;
            Assert.IsNotNull(result, "Null");
            Assert.AreEqual("Index", result.ViewName, "ViewName");
        }

        [TestMethod]
        public void TestCreateNotNullAndView()
        {
            FormularioController controller = new FormularioController();
            ViewResult result = controller.Create() as ViewResult;
            Assert.IsNotNull(result, "Null");
            Assert.AreEqual("Create", result.ViewName, "ViewName");
        }
    }
}
