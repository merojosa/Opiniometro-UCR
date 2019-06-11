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
    public class CategoriaControllerTest
    {
        [TestMethod]
        public void TestCreateViewDataMock()
        {
            // Arrange
            var mockDb = new Mock<Opiniometro_DatosEntities>();
            string nombreCategoria = "Profesor";
            Categoria categoria = new Categoria() { NombreCategoria = "Profesor" };
            mockDb.Setup(m => m.Categoria.Find(nombreCategoria)).Returns(categoria);
            CategoriaController controller = new CategoriaController(mockDb.Object);

            // Act
            controller.Create(categoria);
            ViewResult result = controller.Details(nombreCategoria) as ViewResult;

            // Assert
            Assert.AreEqual(result.Model, categoria);
        }

        [TestMethod]
        public void TestEditViewDataMock()
        {
            // Arrange
            var mockDb = new Mock<Opiniometro_DatosEntities>();
            string nombreCategoria = "Profesor";
            Categoria categoria = new Categoria() { NombreCategoria = "Profesor" };
            mockDb.Setup(m => m.Categoria.Find(nombreCategoria)).Returns(categoria);
            CategoriaController controller = new CategoriaController(mockDb.Object);

            // Act
            ViewResult result = controller.Edit(nombreCategoria) as ViewResult;

            // Assert
            Assert.AreEqual(result.Model, categoria);
        }

        [TestMethod]
        public void TestEditViewDataMock2()
        {
            // Arrange
            var mockDb = new Mock<Opiniometro_DatosEntities>();
            string nombreCategoria = "Profesor";
            Categoria categoria = new Categoria() { NombreCategoria = "Profesor" };
            mockDb.Setup(m => m.Categoria.Find(nombreCategoria)).Returns(categoria);
            CategoriaController controller = new CategoriaController(mockDb.Object);

            // Act
            controller.Edit(categoria);
            ViewResult result = controller.Details(nombreCategoria) as ViewResult;

            // Assert
            Assert.AreEqual(result.Model, categoria);
        }

        [TestMethod]
        public void TestDetailsViewDataMock()
        {
            // Arrange
            var mockDb = new Mock<Opiniometro_DatosEntities>();
            string nombreCategoria = "Profesor";
            Categoria categoria = new Categoria() { NombreCategoria = "Profesor" };
            mockDb.Setup(m => m.Categoria.Find(nombreCategoria)).Returns(categoria);
            CategoriaController controller = new CategoriaController(mockDb.Object);

            // Act
            ViewResult result = controller.Details(nombreCategoria) as ViewResult;

            // Assert
            Assert.AreEqual(result.Model, categoria);
        }

        [TestMethod]
        public void TestDeleteViewDataMock()
        {
            // Arrange
            var mockDb = new Mock<Opiniometro_DatosEntities>();
            string nombreCategoria = "Profesor";
            Categoria categoria = new Categoria() { NombreCategoria = "Profesor" };
            mockDb.Setup(m => m.Categoria.Find(nombreCategoria)).Returns(categoria);
            CategoriaController controller = new CategoriaController(mockDb.Object);

            // Act
            ViewResult result = controller.Delete(nombreCategoria) as ViewResult;

            // Assert
            Assert.AreEqual(result.Model, categoria);
        }

        [TestMethod]
        public void TestDeleteViewDataMock2()
        {
            // Arrange
            var mockDb = new Mock<Opiniometro_DatosEntities>();
            string nombreCategoria = "Profesor";
            Categoria categoria = new Categoria() { NombreCategoria = "Profesor" };
            mockDb.Setup(m => m.Categoria.Find(nombreCategoria)).Returns(categoria);
            CategoriaController controller = new CategoriaController(mockDb.Object);

            // Act
            controller.DeleteConfirmed(categoria.NombreCategoria);
            ViewResult result = controller.Details(nombreCategoria) as ViewResult;

            // Assert
            Assert.AreEqual(result.Model, categoria);
        }

        [TestMethod]
        public void TestIndexViewDataMock()
        {
            // Arrange
            var categorias = new List<Categoria>
            {
                new Categoria() { NombreCategoria = "Profesor" },
                new Categoria() { NombreCategoria = "Estudiante" },
                new Categoria() { NombreCategoria = "Asistente" },
                new Categoria() { NombreCategoria = "Curso" }
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Categoria>>();

            mockDbSet.As<IQueryable<Categoria>>().Setup(m => m.Provider).Returns(categorias.Provider);
            mockDbSet.As<IQueryable<Categoria>>().Setup(m => m.Expression).Returns(categorias.Expression);
            mockDbSet.As<IQueryable<Categoria>>().Setup(m => m.ElementType).Returns(categorias.ElementType);
            mockDbSet.As<IQueryable<Categoria>>().Setup(m => m.GetEnumerator()).Returns(categorias.GetEnumerator());

            var mockDb = new Mock<Opiniometro_DatosEntities>();
            mockDb.Setup(m => m.Categoria).Returns(mockDbSet.Object);
            CategoriaController controller = new CategoriaController(mockDb.Object);

            // Act
            ViewResult result = controller.Index() as ViewResult;
            List<Categoria> categoria = (List<Categoria>)result.ViewData.Model;
            // Assert
            Assert.AreEqual(4, categoria.Count);
        }

        [TestMethod]
        public void TestIndexNotNullAndView()
        {
            CategoriaController controller = new CategoriaController();
            ViewResult result = controller.Index() as ViewResult;
            Assert.IsNotNull(result, "Null");
            Assert.AreEqual("Index", result.ViewName, "ViewName");
        }

        [TestMethod]
        public void TestCreateNotNullAndView()
        {
            CategoriaController controller = new CategoriaController();
            ViewResult result = controller.Create() as ViewResult;
            Assert.IsNotNull(result, "Null");
            Assert.AreEqual("Create", result.ViewName, "ViewName");
        }
    }
}
