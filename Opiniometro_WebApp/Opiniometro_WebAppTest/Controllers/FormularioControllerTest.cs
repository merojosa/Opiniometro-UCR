using System;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Opiniometro_WebApp.Controllers;

namespace Opiniometro_WebAppTest.Controllers
{
    [TestClass]
    public class FormularioControllerTest
    {
        [TestMethod]
        public void TestIndexNotNull()
        {
            FormularioController controller = new FormularioController();
            ViewResult result = controller.Index() as ViewResult;
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void TestIndexView()
        {
            FormularioController controller = new FormularioController();
            ViewResult result = controller.Index() as ViewResult;
            Assert.AreEqual("Index", result.ViewName);
        }
    }
}
