using System;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Opiniometro_WebApp.Controllers;
using Opiniometro_WebApp.Controllers.Servicios;

namespace Opiniometro_WebAppTest.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void TestIndexView()
        {
            
            var controller = new HomeController();
            controller.Session["test"] = new IdentidadManager();
            var result = controller.Index() as ViewResult;
            Assert.AreEqual("Index", result.ViewName);
        }
    }
}
