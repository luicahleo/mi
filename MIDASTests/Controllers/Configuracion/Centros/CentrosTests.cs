using Microsoft.VisualStudio.TestTools.UnitTesting;

using MIDAS.Controllers;
using MIDAS.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MIDASTests.Controllers.Configuracion.Centros
{
    [TestClass()]
    public class CentrosTests
    {

        [TestMethod]
        public void EditarCentro_SessionIsNull_RedirectsToLogOn()
        {
            // Arrange
            var controller = new ConfiguracionController();
            controller.Session["usuario"] = null;

            // Act
            var result = controller.editar_centro(1) as RedirectToRouteResult;

            // Assert
            Assert.AreEqual("LogOn", result.RouteValues["action"]);
            Assert.AreEqual("Account", result.RouteValues["controller"]);
        }

        [TestMethod]
        public void EditarCentro_SessionIsNotNull_ReturnsView()
        {
            // Arrange
            var controller = new ConfiguracionController();
            controller.Session["usuario"] = "some value";

            // Act
            var result = controller.editar_centro(1) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void EditarCentro_CorrectId_SetsIdCentralInViewData()
        {
            // Arrange
            var controller = new ConfiguracionController();
            controller.Session["usuario"] = "some value";
            int expectedId = 1;

            // Act
            controller.editar_centro(expectedId);

            // Assert
            Assert.AreEqual(expectedId, controller.ViewData["idCentral"]);
        }

        //[TestMethod]
        //public void EditarCentro_CallsListarAreas_SetsAreasInViewData()
        //{
        //    // Arrange
        //    var controller = new ConfiguracionController();
        //    controller.Session["usuario"] = "some value";
        //    var expectedAreas = new List<string> { "area1", "area2" };
        //    var datos = new DatosHelperMock { ListarAreasResult = expectedAreas };
        //    controller.Datos = datos;

        //    // Act
        //    controller.editar_centro(1);

        //    // Assert
        //    Assert.AreEqual(expectedAreas, controller.ViewData["areas"]);
        //}






    }

}
