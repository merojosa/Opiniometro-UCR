using System;
using System.Web.Mvc;
using System.Collections.Generic;
using Opiniometro_WebApp.Controllers;
using Opiniometro_WebApp.Models;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;
using System.Linq;

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
        public void cedulaLoggeado() {
            CursosMatriculadosController controller = new CursosMatriculadosController();

            string cedula = controller.obtenerCedulaEstLoggeado("jose.mejiasrojas@ucr.ac.cr");

            Assert.AreEqual(cedula, "116720500");

        }

        [TestMethod]
        public void cedulaLoggeado2()
        {
            CursosMatriculadosController metodo = new CursosMatriculadosController();

            string cedula = metodo.obtenerCedulaEstLoggeado("admin@ucr.ac.cr");

            Assert.AreEqual(cedula, "123456789");

        }

        [TestMethod]
        public void cicloActual()
        {
            CursosMatriculadosController controller = new CursosMatriculadosController();

            int ciclo = controller.ciclo(4);

            Assert.AreEqual(ciclo,1);

        }

        [TestMethod]
        public void cicloActual2()
        {
            CursosMatriculadosController controller = new CursosMatriculadosController();

            int ciclo = controller.ciclo(10);

            Assert.AreEqual(ciclo, 2);

        }



    }
}
