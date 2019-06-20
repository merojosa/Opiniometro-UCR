using System;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Opiniometro_WebApp.Controllers;
using Opiniometro_WebApp.Controllers.Servicios;

namespace Opiniometro_WebAppTest.Controllers
{
    [TestClass]
    public class IdentidadManagerTest
    {
        [TestMethod]
        public void obtener_correo_actual_test()
        {
            // Mocks necesarios para tener una sesion "de mentira".
            var mock_controller_contexto = new Mock<ControllerContext>();
            var mock_session = new Mock<HttpSessionStateBase>();

            var identidad = new ClaimsIdentity(
                        new[] {
                    new Claim(ClaimTypes.Email, "prueba@ucr.ac.cr")
                        },
                        DefaultAuthenticationTypes.ApplicationCookie);

            Thread.CurrentPrincipal = new ClaimsPrincipal(identidad);

            var resultado_correo = IdentidadManager.obtener_correo_actual();
            Assert.AreEqual("prueba@ucr.ac.cr", resultado_correo);        }

        public void obtener_perfil_actual_test()
        {
            // Mocks necesarios para tener una sesion "de mentira".
            var mock_controller_contexto = new Mock<ControllerContext>();
            var mock_session = new Mock<HttpSessionStateBase>();

            var identidad = new ClaimsIdentity(
                        new[] {

                    // Nuevo perfil.
                    new Claim(ClaimTypes.Role, "Perfil Prueba")
                        },
                        DefaultAuthenticationTypes.ApplicationCookie);

            Thread.CurrentPrincipal = new ClaimsPrincipal(identidad);

            var resultado_perfil = IdentidadManager.obtener_perfil_actual();

            Assert.AreEqual("Perfil Prueba", resultado_perfil);
        }
    }
}
