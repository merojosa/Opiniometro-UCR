﻿using System;
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
    public class AsignarFormulariosControllerTest
    {
        [TestMethod]
        public void TestIndexNotNull()
        {
            AsignarFormulariosController controller = new AsignarFormulariosController();
            ViewResult result = controller.Index() as ViewResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestIndexView()
        {
            AsignarFormulariosController controller = new AsignarFormulariosController();
            ViewResult result = controller.Index() as ViewResult;
            Assert.AreEqual("Index", result.ViewName);

        }

        [TestMethod]
        public void TestControllerObtenerCarreras()
        {
            AsignarFormulariosController controller = new AsignarFormulariosController();
            ViewResult result = controller.ObtenerCarreras(0, 0, "") as ViewResult;  //anno, semestre, nombre de unidad academica
            Assert.IsNull(result);
        }

        [TestMethod]
        public void TestObtenerCursos()
        {
            //Arrange
            AsignarFormulariosController asignar = new AsignarFormulariosController();
            //Act
            IQueryable<Curso> lista = asignar.ObtenerCursos(null, null, "UC-023874", "SC-01234", null);
            //Assert
            Assert.IsNotNull(lista);
        }

        //[TestMethod]
        //public void TestMockControllerObtenerCarreras()
        //{
        //    var mockDb = new Mock<Opiniometro_DatosEntities>();

        //    Carrera C = new Carrera() { Sigla = "Sigla", Nombre = "Nombre", CodigoUnidadAcademica = "Codigo" };

        //    mockDb.Object.Carrera.Add(C);
        //    IQueryable<Carrera> ListaC = from car in mockDb.Object.Carrera select car;

        //    var mockContr = new Mock<AsignarFormulariosController>(mockDb);

        //    mockContr.Setup(m => m.ObtenerCarreras(0, 0, "")).Returns(ListaC);  //anno, semestre, nombre de unidad academica

        //    Assert.AreEqual(ListaC, mockContr.Object.ObtenerCarreras(0, 0, ""));
        //}

        /*
        [TestMethod]
        public void TestSeleccionFormulariosPartialView()
        {
            // Arrange
            var forms = new List<Formulario>
            {
                new Formulario() { CodigoFormulario = "pr1", Nombre = "form progra 1" },
                new Formulario() { CodigoFormulario = "bd2", Nombre = "form bases 2" },
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Formulario>>();

            mockDbSet.As<IQueryable<Formulario>>().Setup(m => m.Provider).Returns(forms.Provider);
            mockDbSet.As<IQueryable<Formulario>>().Setup(m => m.Expression).Returns(forms.Expression);
            mockDbSet.As<IQueryable<Formulario>>().Setup(m => m.ElementType).Returns(forms.ElementType);
            mockDbSet.As<IQueryable<Formulario>>().Setup(m => m.GetEnumerator()).Returns(forms.GetEnumerator());

            var mockDb = new Mock<Opiniometro_DatosEntities>();

            mockDb.Setup(m => m.Formulario).Returns(mockDbSet.Object);

            AsignarFormulariosController controller = new AsignarFormulariosController(mockDb.Object);

            // Act
            ViewResult result = controller.Index() ViewResult;
            List<Formulario> pais = (List<Formulario>)result.ViewData.Model;
            // Assert
            Assert.AreEqual(2, pais.Count);

//        }*/
    }
}
