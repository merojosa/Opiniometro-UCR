using System;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Opiniometro_WebApp.Controllers;

namespace Opiniometro_WebAppTest.Controllers
{
    [TestClass]
    public class AuthControllerTest
    {
        [TestMethod]
        public void TestLoginNotNull()
        {
            // Arrange
            AuthController controller = new AuthController();
            // Act
            ViewResult result = controller.Login() as ViewResult;
            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestRecuperarNotNull()
        {
            // Arrange
            AuthController controller = new AuthController();
            // Act
            ViewResult result = controller.Recuperar() as ViewResult;
            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestRecuperarView()
        {
            var controller = new AuthController();
            var result = controller.Recuperar() as ViewResult;
            Assert.AreEqual("Recuperar", result.ViewName);
        }
    }
}
