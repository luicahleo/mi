using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using MIDAS.Models;
using MIDAS.Helpers;
using System.Security.Principal;
using System.Web.Security;
using System.Configuration;
using System.Web.Routing;
using DocumentFormat.OpenXml.Packaging;
using System.Text.RegularExpressions;
using System.Diagnostics;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using A = DocumentFormat.OpenXml.Drawing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;
using System.Drawing;
using System.Drawing.Imaging;
using System.ComponentModel;
using System.Xml.Linq;
using DocumentFormat.OpenXml.Drawing.Wordprocessing;
using DocumentFormat.OpenXml.Spreadsheet;
using d = DocumentFormat.OpenXml.Drawing;
using dc = DocumentFormat.OpenXml.Drawing.Charts;
using dw = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using DocumentFormat.OpenXml.Drawing;

namespace MIDAS.Controllers
{
    [HandleError]
    public class ImagenController : Controller
    {
        public ActionResult imagenes()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }
            ViewData["imagenesGenerales"] = Datos.ListarMedidasGeneralesImagenes();
            ViewData["imagenesRiesgos"] = Datos.ListarRiesgosMedidasImagenes();
            ViewData["imagenesPreventivas"] = Datos.ListarMedidasPreventivasImagenes();


            return View();
        }

        public ActionResult editar_imagen(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            ViewData["idImagen"] = id;

            return View();
        }

    }
}
