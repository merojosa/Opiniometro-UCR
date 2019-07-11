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
    /// <summary>
    /// Summary description for CodedUITest1
    /// </summary>
    [CodedUITest]
    public class FormularioCurso
    {
        public FormularioCurso()
        {
        }

        [TestMethod]        public void login()
        {

            this.UIMap.loginUsuario();

        }

        [TestMethod]        public void verCursosMatriculados()
        {
            this.UIMap.loginUsuario();
            this.UIMap.IngresarCursosMatriculados();

        }

        [TestMethod]
        public void validarFormulario()
        {
            this.UIMap.loginUsuario();
            this.UIMap.IngresarCursosMatriculados();
            this.UIMap.validacionEnlace();
        }

        [TestInitialize()]
        public void MyTestInitialize()
        {
            BrowserWindow.CurrentBrowser = "ie";
            this.UIMap.InicializarExplorador();
            // To generate code for this test, select "Generate Code for Coded UI Test" from the
            //shortcut menu and select one of the menu items.
        }
        ////Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {
            // To generate code for this test, select "Generate Code for Coded UI Test" from the
            //shortcut menu and select one of the menu items.
        }

        //[TestMethod]
        //public void CodedUITestMethod1()
        //{
        //    // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
        //}

        #region Additional test attributes

        // You can use the following additional attributes as you write your tests:

        ////Use TestInitialize to run code before running each test 
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{        
        //    // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
        //}

        ////Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{        
        //    // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
        //}

        #endregion

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
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
