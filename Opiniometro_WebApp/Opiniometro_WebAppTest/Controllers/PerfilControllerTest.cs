using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Opiniometro_WebApp.Controllers;

namespace Opiniometro_WebAppTest.Controllers
{
    [TestClass]
    public class PerfilControllerTest
    {
        [TestMethod]
        public void TestCrearNotNull()
        {
            // Arrange
            PerfilController controller = new PerfilController();
            // Act
            ViewResult result = controller.Crear() as ViewResult;
            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestCrearView()
        {
            var controller = new PerfilController();
            var result = controller.Crear() as ViewResult;
            Assert.AreEqual("Crear", result.ViewName);
        }

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

        [TestMethod]
        public void TestBorrarView()
        {
            var controller = new PerfilController();
            var result = controller.Borrar() as ViewResult;
            Assert.AreEqual("Borrar", result.ViewName);
        }


        [TestMethod]
        public void TestCambiarNotNull()
        {
            // Arrange
            PerfilController controller = new PerfilController();
            // Act
            ViewResult result = controller.Cambiar() as ViewResult;
            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestCambiarView()
        {
            var controller = new PerfilController();
            var result = controller.Cambiar() as ViewResult;
            Assert.AreEqual("Cambiar", result.ViewName);
        }

        [TestMethod]
        public void TestObtenerPerfilesNotNull()
        {
            // Arrange
            PerfilController controller = new PerfilController();
            // Act
            ICollection<string> result = PerfilController.ObtenerPerfiles();
            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestObtenerPerfilBorrarBadRequest()
        {
            // Instancio el form con la llave NombrePerfil para que tenga un null.
            NameValueCollection form = new NameValueCollection();
            form["NombrePerfil"] = null;
            var controllercontext = new Mock<ControllerContext>();
            controllercontext.Setup(frm => frm.HttpContext.Request.Form).Returns(form);
            PerfilController controller = new PerfilController();
            controller.ControllerContext = controllercontext.Object;

            // Act
            var result = controller.ObtenerPerfilBorrar();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(HttpStatusCodeResult));
            var httpResult = result as HttpStatusCodeResult;

            // 400 = bad request.
            Assert.AreEqual(400, httpResult.StatusCode);
        }

        [TestMethod]
        public void TestObtenerPerfilBorrarRedirect()
        {
            // Instancio el form con la llave NombrePerfil para que tenga un null.
            NameValueCollection form = new NameValueCollection();
            form["NombrePerfil"] = "Prueba no null";
            var controllercontext = new Mock<ControllerContext>();
            controllercontext.Setup(frm => frm.HttpContext.Request.Form).Returns(form);
            PerfilController controller = new PerfilController();
            controller.ControllerContext = controllercontext.Object;

            // Act
            var result = (RedirectToRouteResult)controller.ObtenerPerfilBorrar();

            // Assert
            Assert.IsNotNull(result);

            // Hay que iterar hasta la segunda posicion para obtener el nombre del action.
            var redirectToActionName = result.RouteValues.Values.GetEnumerator();
            redirectToActionName.MoveNext();
            redirectToActionName.MoveNext();

            Assert.AreEqual("ConfirmarBorrado", redirectToActionName.Current);
        }

    }
}
