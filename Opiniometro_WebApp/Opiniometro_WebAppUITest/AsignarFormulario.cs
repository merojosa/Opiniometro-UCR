using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UITest.Extension;
using Keyboard = Microsoft.VisualStudio.TestTools.UITesting.Keyboard;

namespace Opiniometro_WebAppUITest
{

    [CodedUITest]
    public class AsignarFormulario
    {
        [TestMethod]
        public void ValidarTituloAsignacion()
        {
            this.UIMap.loginUsuario();
            this.UIMap.IngresoAsignacionFormularios();
            this.UIMap.validacionTituloAsignacionFormularios();
        }

        [TestMethod]
        public void ValidarFiltros()  //valida los títulos de los filtros
        {
            this.UIMap.loginUsuario();
            this.UIMap.IngresoAsignacionFormularios();

            this.UIMap.validacionTextoFiltroSemestre();
            this.UIMap.validacionTextoFiltroAnno();
            this.UIMap.validacionTextoFiltroUnidadAcademica();
            this.UIMap.validacionTextoFiltroCarrera();
            this.UIMap.validacionTextoFiltroCurso();
        }


        [TestInitialize()]
        public void MyTestInitialize()
        {
            BrowserWindow.CurrentBrowser = "ie";
            this.UIMap.InicializarExplorador();
            // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.         
        }

        ////Use TestCleanup to run code after each test has run         
        [TestCleanup()]
        public void MyTestCleanup()
        {   
            // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.         
        }


        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }
        private TestContext testContextInstance;

        public UIMap UIMap
        {
            get
            {
                if (this.map == null)
                {
                    this.map = new UIMap();
                }

                return this.map;
            }
        }
        private UIMap map;
    }
}



