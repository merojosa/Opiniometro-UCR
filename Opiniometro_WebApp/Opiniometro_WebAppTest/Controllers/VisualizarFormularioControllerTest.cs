using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Opiniometro_WebApp.Controllers;
using Opiniometro_WebApp.Models;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Opiniometro_WebAppTest.Controllers
{
    [TestClass]
    public class VisualizarFormularioControllerTest
    {
        [TestMethod]
        public void IndexNotNull()
        {
            //Arrange
            VisualizarFormularioController controller = new VisualizarFormularioController();
            //Act
            ViewResult result = controller.Index() as ViewResult;
            //Assert
            Assert.IsNotNull(result, "Null");
        }
        [TestMethod]
        public void DetailsIsNotNull()
        {
            //Arrange
            VisualizarFormularioController controller = new VisualizarFormularioController();
            //Act
            ViewResult result = controller.Details("131313") as ViewResult;
            //Assert
            Assert.IsNotNull(result, "Null");
        }
        [TestMethod]
        public void DetailsIsNotNullFormularioNull()
        {
            //Arrange
            VisualizarFormularioController controller = new VisualizarFormularioController();
            //Act
            ViewResult result = controller.Details("131315") as ViewResult;
            //Assert
            Assert.IsNull(result, "Null");
        }
        [TestMethod]
        public void DetailsIsNull()
        {
            //Arrange
            VisualizarFormularioController controller = new VisualizarFormularioController();
            //Act
            ViewResult result = controller.Details(null) as ViewResult;
            //Assert
            Assert.IsNull(result, "Null");
        }
        [TestMethod]
        public void EditIsNotNull()
        {
            //Arrange
            VisualizarFormularioController controller = new VisualizarFormularioController();
            //Act
            ViewResult result = controller.Edit("131313") as ViewResult;
            //Assert
            Assert.IsNotNull(result, "Null");
        }

        [TestMethod]
        public void CreateIsNotNull()
        {
            //Arrange
            VisualizarFormularioController controller = new VisualizarFormularioController();
            //Act
            ViewResult result = controller.Create() as ViewResult;
            //Assert
            Assert.IsNotNull(result, "Null");
        }
        [TestMethod]
        public void EtiquetasNotNull()
        {

        }
    }
}
