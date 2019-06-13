using System;
using System.Web.Mvc;
using System.Collections.Generic;
using Opiniometro_WebApp.Controllers;
using Opiniometro_WebApp.Models;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Opiniometro_WebAppTest.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void TestIndexView()
        {
            var controller = new HomeController();
            var result = controller.Index() as ViewResult;
            Assert.AreEqual("Index", result.ViewName);
        }

    }
}
