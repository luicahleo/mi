using Microsoft.VisualStudio.TestTools.UnitTesting;
using MIDAS.Models;
using MIDAS.Controllers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace MIDASTests.Controllers.Personas.FiltrosDatos
{
    [TestClass]
    public class TestFiltrosDatos
    {
        private PersonasController _controller;

        [TestInitialize]
        public void Initialize()
        {
            _controller = new PersonasController();
        }

        [TestMethod]
        public void TestFiltrosDatos_SinFiltros()
        {
            // Arrange
            string filtrosJson = "{\"actividad\":[],\"centroTrabajo\":[]}";
            var expectedList = Datos.ListaPersonas();

            // Act
            var result = _controller.FiltrosDatos(filtrosJson) as JsonResult;
            var resultJson = result.Data as string;
            var resultList = JsonConvert.DeserializeObject<List<personas>>(resultJson);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Data);
            CollectionAssert.AreEqual(expectedList, resultList);
        }

        [TestMethod]
        public void TestFiltrosDatos_ActividadFiltros()
        {
            // Arrange
            string filtrosJson = "{\"actividad\":[\"Actividad 1\",\"Actividad 2\"],\"centroTrabajo\":[]}";
            var expectedList = Datos.ListaPersonas().FindAll(p => p.Actividad == "Actividad 1" || p.Actividad == "Actividad 2");
            expectedList = expectedList.GroupBy(p => p.Perfil_de_riesgo).Select(g => g.First()).ToList();

            // Act
            var result = _controller.FiltrosDatos(filtrosJson) as JsonResult;
            var resultJson = result.Data as string;
            var resultList = JsonConvert.DeserializeObject<List<personas>>(resultJson);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Data);
            CollectionAssert.AreEqual(expectedList, resultList);
        }

        [TestMethod]
        public void TestFiltrosDatos_CentroTrabajoFiltros()
        {
            // Arrange
            string filtrosJson = "{\"actividad\":[],\"centroTrabajo\":[\"Centro de trabajo 1\",\"Centro de trabajo 2\"]}";
            var expectedList = Datos.ListaPersonas().FindAll(p => p.Centro_de_trabajo == "Centro de trabajo 1" || p.Centro_de_trabajo == "Centro de trabajo 2");
            expectedList = expectedList.GroupBy(p => p.Perfil_de_riesgo).Select(g => g.First()).ToList();

            // Act
            var result = _controller.FiltrosDatos(filtrosJson) as JsonResult;
            var resultJson = result.Data as string;
            var resultList = JsonConvert.DeserializeObject<List<personas>>(resultJson);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Data);
            CollectionAssert.AreEqual(expectedList, resultList);
        }

        [TestMethod]
        public void TestFiltrosDatos_ActividadYCentroTrabajoFiltros()
        {
            // Arrange
            string filtrosJson = "{\"actividad\":[\"Actividad 1\",\"Actividad 2\"],\"centroTrabajo\":[\"Centro de trabajo 1\",\"Centro de trabajo 2\"]}";
            var expectedList = Datos.ListaPersonas().FindAll(p => p.Actividad == "Actividad 1" || p.Actividad == "Actividad 2");
            expectedList = expectedList.FindAll(p => p.Centro_de_trabajo == "Centro de trabajo 1" || p.Centro_de_trabajo == "Centro de trabajo 2");
            expectedList = expectedList.GroupBy(p => p.Perfil_de_riesgo).Select(g => g.First()).ToList();

            // Act
            var result = _controller.FiltrosDatos(filtrosJson) as JsonResult;
            var resultJson = result.Data as string;
            var resultList = JsonConvert.DeserializeObject<List<personas>>(resultJson);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Data);
            CollectionAssert.AreEqual(expectedList, resultList);
        }

        [TestMethod]
        public void TestFiltrosDatos_Excepcion()
        {
            // Arrange
            string filtrosJson = null;

            // Act
            var result = _controller.FiltrosDatos(filtrosJson) as JsonResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNull(result.Data);
        }
    }
}
