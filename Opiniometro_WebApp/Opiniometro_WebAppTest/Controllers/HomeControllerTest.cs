using System;
using System.Web;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Opiniometro_WebApp.Controllers;
using Opiniometro_WebApp.Controllers.Servicios;

namespace Opiniometro_WebAppTest.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void TestHomeNotNull()
        {
            var mockControllerContext = new Mock<ControllerContext>();
            var mockSession = new Mock<HttpSessionStateBase>();
            mockSession.SetupGet(s => s["login_fallido"]).Returns(null); //somevalue
            mockControllerContext.Setup(p => p.HttpContext.Session).Returns(mockSession.Object);

            var controller = new HomeController();
            controller.ControllerContext = mockControllerContext.Object;
            var result = controller.Index() as ActionResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
