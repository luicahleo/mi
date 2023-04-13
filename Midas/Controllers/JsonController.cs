using MIDAS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MIDAS.Controllers
{
    public class JsonController : Controller
    {
        // GET: Json
        public ActionResult Index()
        {
            

            return View();
        }


        public JsonResult ObtenerListaImagenes()
        {
            List<medidas_generales_imagenes> listaI;

            listaI = Datos.GetListMedidasGeneralesImagenes();

            return Json(listaI, JsonRequestBehavior.AllowGet);
        }

    }
}