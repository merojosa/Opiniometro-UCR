using System;
using System.Web.Mvc;
using System.Collections.Generic;
using Opiniometro_WebApp.Controllers;
using Opiniometro_WebApp.Models;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;
using System.Linq;
using System.Data.Entity.Core.Objects;


namespace Opiniometro_WebAppTest.Controllers
{
    [TestClass]
    public class CursosMatriculadosTest
    {
        [TestMethod]
        public void TestIndexNoNulo()
        {
            // Arrange
            CursosMatriculadosController controller = new CursosMatriculadosController();
            // Act
            ViewResult result = controller.Index() as ViewResult;
            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestIndexVista()
        {
            var controller = new CursosMatriculadosController();
            var result = controller.Index() as ViewResult;

            Assert.AreEqual("Index", result.ViewName);
        }

        [TestMethod]
        public void cicloActual()
        {
            CursosMatriculadosController controller = new CursosMatriculadosController();

            int ciclo = controller.ciclo(4);

            Assert.AreEqual(ciclo, 1);

        }

        [TestMethod]
        public void cicloActual2()
        {
            CursosMatriculadosController controller = new CursosMatriculadosController();

            int ciclo = controller.ciclo(10);

            Assert.AreEqual(ciclo, 2);

        }

        //[TestMethod]
        //public void cedulaLoggeado() {
        //    CursosMatriculadosController controller = new CursosMatriculadosController();

        //    string cedula = controller.obtenerCedulaEstLoggeado("jose.mejiasrojas@ucr.ac.cr");

        //    Assert.AreEqual(cedula, "116720500");

        //}

        [TestMethod]
        public void cedulaLoggeado2()
        {
            CursosMatriculadosController metodo = new CursosMatriculadosController();

            string cedula = metodo.obtenerCedulaEstLoggeado("admin@ucr.ac.cr");

            Assert.AreEqual(cedula, "123456789");

        }

        [TestMethod]
        public void cedulaLog() {
            var mockDB = new Mock<Opiniometro_DatosEntities>();
            ObjectParameter cedula = new ObjectParameter("resultado", "");
            Usuario usuario = new Usuario() { CorreoInstitucional = "jose.mejiasrojas@ucr.ac.cr", Contrasena = "123456", Activo = true, Cedula = "116720500", RecuperarContrasenna = false };
            mockDB.Setup(v => v.SP_ObtenerCedula(usuario.CorreoInstitucional, cedula)).Returns( Convert.ToInt32(usuario.Cedula) );
            CursosMatriculadosController controller = new CursosMatriculadosController(mockDB.Object);
            Assert.AreEqual(mockDB.Object.SP_ObtenerCedula("jose.mejiasrojas@ucr.ac.cr", cedula), 116720500);
        }

    }
}
