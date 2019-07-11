using System;
using System.Web;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Opiniometro_WebApp.Controllers;
using Moq;

namespace Opiniometro_WebAppTest.Controllers
{
    [TestClass]
    public class EnfasisControllerTest
    {
        [TestMethod]
        public void TestVerEnfasisNotNull()
        {
            //Arrange
            EnfasisController controller = new EnfasisController();

            //Act
            ViewResult result = controller.VerEnfasis() as ViewResult;

            //Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestVerEnfasisView()
        {
            //Arrange
            var controller = new EnfasisController();

            //Act
            var result = controller.VerEnfasis() as ViewResult;

            //Assert
            Assert.AreEqual("Ver Enfasis", result.ViewName);
        }
    }
}
