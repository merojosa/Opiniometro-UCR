using System;
using System.Web;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Opiniometro_WebApp.Controllers;
using Opiniometro_WebApp.Controllers.Servicios;
using System.Net;
using Opiniometro_WebApp.Models;


namespace Opiniometro_WebAppTest.Controllers
{
    [TestClass]
    public class AdministradorControllerTest
    {
        [TestMethod]
        public void EditIsNotNull()
        {
            //Arrange
            AdministradorController controller = new AdministradorController();
            //Act
            ViewResult result = controller.Editar("116720500") as ViewResult;
            //Assert
            Assert.IsNotNull(result, "Null");
        }
    }
}
