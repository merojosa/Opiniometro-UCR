using System;
using System.Web;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Opiniometro_WebApp.Controllers;
using Opiniometro_WebApp.Controllers.Servicios;
using Moq;

using System.Security.Claims;
using System.Threading;
using System.Linq;

namespace Opiniometro_WebAppTest.Controllers
{
    [TestClass]
    public class SeleccionPermisosTest
    {
        [TestMethod]
        //Si el perfil no es administrador no carga la vista
        public void SeleccionarPermisosNull()
        {
            //Mock de sesion
            var mock_controller_contexto = new Mock<ControllerContext>();
            var mock_session = new Mock<HttpSessionStateBase>();
            mock_controller_contexto.Setup(p => p.HttpContext.Session).Returns(mock_session.Object);

            //ClaimsPrincipal cp = new ClaimsPrincipal();

            //var identidad = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Role, "Administrador"), new Claim(ClaimTypes.Email, "admin@ucr.ac.cr") });
            //Thread.CurrentPrincipal = new ClaimsPrincipal(identidad);

            //Arrange
            SeleccionPermisosController controller = new SeleccionPermisosController();
            controller.ControllerContext = mock_controller_contexto.Object;
            
            //Act
            ViewResult result = controller.SeleccionarPermisos() as ViewResult;

            //Assert
            Assert.IsNull(result);
        }
    }
}
