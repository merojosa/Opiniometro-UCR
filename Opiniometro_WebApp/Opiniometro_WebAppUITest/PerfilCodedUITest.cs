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
    public class PerfilCodedUITest
    {
        public PerfilCodedUITest()
        {
        }
        [TestMethod]
        public void CodedUITestLogin()
        {
            this.UIMap.PruebaIngresoAplicacion();
            this.UIMap.ValidarBotonLogin();
            this.UIMap.ValidarRecuperarContrasennaLink();
        }

        [TestMethod]
        public void CodedUITestExistenciaAdministrador()
        {
            this.UIMap.PruebaIngresoAplicacion();
            this.UIMap.ValidarLoginAdmin();
            this.UIMap.ValidarVerPerfiles();
            this.UIMap.ValidarPerfilAdministrador();
        }

        [TestMethod]
        public void CodedUITestRecuperarContrasenna()
        {
            this.UIMap.PruebaIngresoAplicacion();
            this.UIMap.ValidarRecuperarContrasenna();
            this.UIMap.ValidarBotonEnviarCorreo();
        }

        #region Additional test attributes
        // You can use the following additional attributes as you write your tests:
        //Use TestInitialize to run code before running each test
        [TestInitialize()]
        public void MyTestInitialize()
        {
            BrowserWindow.CurrentBrowser = "ie"; // "ie" “Chrome” “firefox”
            this.UIMap.InicializarExplorador();
        }
        //Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {
        }
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
