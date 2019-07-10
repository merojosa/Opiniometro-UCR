using System;
using System.Web;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Opiniometro_WebApp.Controllers;

namespace Opiniometro_WebAppTest.Controllers
{
    [TestClass]
    public class AuthControllerTest
    {
        [TestMethod]
        public void TestLoginNotNull()
        {
            // Mocks necesarios para tener una sesion "de mentira".
            var mock_controller_contexto = new Mock<ControllerContext>();
            var mock_session = new Mock<HttpSessionStateBase>();
            mock_controller_contexto.Setup(p => p.HttpContext.Session).Returns(mock_session.Object);

            // Arrange
            AuthController controller = new AuthController();
            controller.ControllerContext = mock_controller_contexto.Object;
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
