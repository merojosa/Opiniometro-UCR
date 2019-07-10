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
            AdministradorController controller = new AdministradorController();
            ViewResult result = controller.Editar("116720500") as ViewResult;
            Assert.IsNotNull(result, "Null");
        }

        /*[TestMethod]
        public void TestEdit()
        {
            AdministradorController controller = new AdministradorController();
            PersonaPerfilEnfasisModel mockModel = new PersonaPerfilEnfasisModel();
            mockModel.Persona = new Persona() {Cedula = "000999111", Nombre1 = "Juan", Nombre2 = "Miguel", Apellido1 = "Hernandez", Apellido2 = "Herrera"};
            mockModel.Usuario = new Usuario() { CorreoInstitucional = "juan.miguelh@ucr.ac.cr" };
            mockModel.viejaCedula = "000999111";

            ViewResult result = controller.Editar(mockModel) as ViewResult;

            Assert.AreEqual(result.Model, mockModel);
        }*/

        [TestMethod]
        public void TestVerPersonasNotNull()
        {
            AdministradorController controller = new AdministradorController();
            ViewResult result = controller.VerPersonas("","") as ViewResult;
            Assert.IsNotNull(result, "Null");
        }

        [TestMethod]
        public void TestCrearUsuarioNotNull()
        {
            AdministradorController controller = new AdministradorController();
            ViewResult result = controller.CrearUsuario() as ViewResult;
            Assert.IsNotNull(result, "Null");
        }

        /*[TestMethod]
        public void TestCrearUsuario()
        {
            AdministradorController controller = new AdministradorController();
            ViewModelAdmin mockModel = new ViewModelAdmin();
            mockModel.Persona = new Persona() { Cedula = "000999111", Nombre1 = "Juan", Nombre2 = "Miguel", Apellido1 = "Hernandez", Apellido2 = "Herrera" };
            mockModel.Usuario = new Usuario() { CorreoInstitucional = "juan.miguelh@ucr.ac.cr" };

            ViewResult result = controller.CrearUsuario(mockModel) as ViewResult;

            Assert.AreEqual(result.Model, mockModel);
        }*/
    }
}
