using System;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using Opiniometro_WebApp.Controllers;
using Opiniometro_WebApp.Models;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Opiniometro_WebApp.Controllers.Servicios;

namespace Opiniometro_WebAppTest.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void TestHomeNotNullLoginFallido()
        {
            // Mocks necesarios para tener una sesion "de mentira".
            var mock_controller_contexto = new Mock<ControllerContext>();
            var mock_session = new Mock<HttpSessionStateBase>();

            // Tiene que dar null ya que es un login fallido.
            mock_session.SetupGet(s => s["login_fallido"]).Returns(null);
            mock_controller_contexto.Setup(p => p.HttpContext.Session).Returns(mock_session.Object);

            var controller = new HomeController();

            // Agrego los mocks al contexto del controlador.
            controller.ControllerContext = mock_controller_contexto.Object;
            var result = controller.Index() as ActionResult;

            Assert.IsNotNull(result);
        }

    }
}
