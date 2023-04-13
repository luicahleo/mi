using Microsoft.VisualStudio.TestTools.UnitTesting;

using MIDAS.Controllers;
using Moq;
using OfficeOpenXml;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web;

namespace MIDASTests.Controllers.Personas
{
    [TestClass()]
    public class PersonasTests
    {
        [TestMethod]
        public void TestPersonas_ValidFileExcel()
        {
            // Arrange
            var controller = new ConfiguracionController();
            var fileMock = new Mock<HttpPostedFileBase>();
            var fileStream = new MemoryStream();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var package = new ExcelPackage();
            var sheet = package.Workbook.Worksheets.Add("Sheet1");
            sheet.Cells[1, 1].Value = "Nº_Empleado";
            sheet.Cells[1, 2].Value = "Perfil_de_riesgo";
            sheet.Cells[2, 1].Value = "1";
            sheet.Cells[2, 2].Value = "Alto";
            package.SaveAs(fileStream);
            fileStream.Position = 0;
            fileMock.Setup(_ => _.InputStream).Returns(fileStream);

            // Act
            var result = controller.personas(fileMock.Object);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ActionResult));
        }

        [TestMethod]
        public void TestPersonas_InvalidFileExcel()
        {
            // Arrange
            var controller = new ConfiguracionController();
            var fileMock = new Mock<HttpPostedFileBase>();
            var fileStream = new MemoryStream();
            var text = Encoding.UTF8.GetBytes("This is not an excel file");
            fileStream.Write(text, 0, text.Length);
            fileStream.Position = 0;
            fileMock.Setup(_ => _.InputStream).Returns(fileStream);

            // Act
            var result = controller.personas(fileMock.Object);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ActionResult));
        }



    }
}
