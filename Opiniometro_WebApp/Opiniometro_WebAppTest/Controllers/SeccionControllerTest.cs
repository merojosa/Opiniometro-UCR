using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Opiniometro_WebApp.Models;
using Opiniometro_WebApp.Controllers;
using Opiniometro_WebApp;

namespace Opiniometro_WebAppTest.Controllers
{
    /// <summary>
    /// Summary description for SeccionControllerTest
    /// </summary>
    [TestClass]
    public class SeccionControllerTest
    {
              
        [TestMethod]
        public void TestIndexNotNullAndView()
        {
            SeccionController controller = new SeccionController();
            ViewResult result = controller.Index(1) as ViewResult;
            Assert.IsNotNull(result, "Null");
            Assert.AreEqual("Index", result.ViewName, "ViewName");

        }

        [TestMethod]
        public void TestEditViewDataMock()
        {
            // Arrange
            var mockDb = new Mock<Opiniometro_DatosEntities>();
            string titulo = "Prueba1";
            Seccion seccion = new Seccion() { Titulo = "Prueba1", Descripcion = "Programación I" };
            mockDb.Setup(m => m.Seccion.Find(titulo)).Returns(seccion);
            SeccionController controller = new SeccionController(mockDb.Object);

            // Act
            ViewResult result = controller.Edit(titulo) as ViewResult;

            // Assert
            Assert.AreEqual(result.Model, seccion);
        }

        [TestMethod]
        public void TestEditDataMock()
        {
            // Arrange
            var mockDb = new Mock<Opiniometro_DatosEntities>();
            string titulo = "Prueba1";
            Seccion seccion = new Seccion() { Titulo = "Prueba1", Descripcion = "Programación I" };
            mockDb.Setup(m => m.Seccion.Find(titulo)).Returns(seccion);
            SeccionController controller = new SeccionController(mockDb.Object);

            // Act
            controller.Edit(seccion);
            ViewResult result = controller.Edit(titulo) as ViewResult;

            //Assert.IsNull();
            // Assert
            Assert.AreEqual(result.Model, seccion);
        }

        [TestMethod]
        public void TestCreateView()
        {
            // Arrange
            SeccionController controller = new SeccionController();

            // Act
            ViewResult result = controller.Create() as ViewResult;

            // Assert
            Assert.AreEqual(result.ViewName, "Create");
        }

        [TestMethod]
        public void TestCreateDataMock()
        {
            // Arrange
            var mockDb = new Mock<Opiniometro_DatosEntities>();
            string titulo = "Prueba1";
            Seccion seccion = new Seccion() { Titulo = "Prueba1", Descripcion = "Programación I" };
            mockDb.Setup(m => m.Seccion.Find(titulo)).Returns(seccion);
            SeccionController controller = new SeccionController(mockDb.Object);

            // Act
             controller.Create(seccion) ;
            ViewResult result = controller.Details(titulo) as ViewResult;

            //Assert.IsNull();
            // Assert
            Assert.AreEqual(result.Model, seccion);
        }

    }
}
