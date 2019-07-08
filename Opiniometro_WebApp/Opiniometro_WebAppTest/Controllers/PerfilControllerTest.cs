using System;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Opiniometro_WebApp.Controllers;

namespace Opiniometro_WebAppTest.Controllers
{
    [TestClass]
    public class PerfilControllerTest
    {
        [TestMethod]
        public void TestBorrarNotNull()
        {
            // Arrange
            PerfilController controller = new PerfilController();
            // Act
            ViewResult result = controller.Borrar() as ViewResult;
            // Assert
            Assert.IsNotNull(result);
        }
    }
}
