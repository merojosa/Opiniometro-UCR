using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Opiniometro_WebApp.Models;
using Opiniometro_WebApp.Controllers;
using Opiniometro_WebApp;

namespace Opiniometro_WebAppTest.Controllers
{
    [TestClass]
    public class ItemControllerTest
    {
        [TestMethod]
        public void TestCreateView()
        {
            // Arrange
            ItemController controller = new ItemController();

            // Act
            ViewResult result = controller.Create() as ViewResult;

            // Assert
            Assert.AreEqual(result.ViewName, "Create");
        }

        [TestMethod]
        public void TestAgregarOpcionesView()
        {
            // Arrange
            ItemController controller = new ItemController();

            // Act
            ViewResult result = controller.AgregarOpciones() as ViewResult;

            // Assert
            Assert.AreEqual(result.ViewName, "AgregarOpciones");
        }
    }
}
