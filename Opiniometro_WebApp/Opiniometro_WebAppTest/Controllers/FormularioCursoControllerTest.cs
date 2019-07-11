using System;
using System.Web.Mvc;
using System.Collections.Generic;
using Opiniometro_WebApp.Controllers;
using Opiniometro_WebApp.Models;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;
using System.Linq;

namespace Opiniometro_WebAppTest.Controllers
{
    [TestClass]
    public class FormularioCursoControllerTest
    {
        [TestMethod]
        public void TestIndexNotNull()
        {
            FormularioCursoController controller = new FormularioCursoController();
            ViewResult result = controller.Index("", "", "", 0 , 0, "", 0) as ViewResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestIndexView()
        {
            FormularioCursoController controller = new FormularioCursoController();
            ViewResult result = controller.Index("", "", "", 0, 0, "", 0) as ViewResult;
            Assert.AreEqual("Index", result.ViewName);
        }
    }
}
