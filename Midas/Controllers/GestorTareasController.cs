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
    public class GestorTareasController : Controller
    {
        //
        // GET: /GestorTareas/

        public ActionResult gestion_tareas()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            int idCentral = centroseleccionado.id;

            //ViewData["tareas"] = Datos.ListarTareas(idCentral, idResponsable);

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult gestion_tareas(FormCollection collection)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            int idCentral = centroseleccionado.id;

            //ViewData["tareas"] = Datos.ListarTareas(idCentral, idResponsable);

            return View();
        }

        #region métodos para traer eventos y formatearlos
        /// <summary>
        /// Metodo que devuelve un Array de eventos en formato Json
        /// summary>
        /// <param name="start">Star Dateparam>
        /// <param name="end">End Dateparam>
        /// <returns>returns>
        public JsonResult GetEvents(double start, double end)
        {
            VISTA_ObtenerUsuario user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());

            var startDateTime = FromUnixTimestamp(start);
            var endDateTime = FromUnixTimestamp(end);
            DIMASSTEntities bd = new DIMASSTEntities();
            var events = from reps in bd.VISTA_CalendarioEventos
                         where (reps.inicio > startDateTime || reps.fin < endDateTime)
                         && reps.Responsable == user.idUsuario
                         select reps;
            var clientList = new List<object>();
            foreach (var e in events)
            {
                if (e.inicio != null && e.fin != null)
                {
                    #region casos por tipo de evento
                    switch (e.TipoEvento)
                    {
                        case 1:
                            clientList.Add(new
                            {
                                id = e.id,
                                title = e.titulo,
                                description = e.descripcion,
                                start = ConvertToTimestamp(DateTime.Parse(e.inicio.ToString())),
                                end = ConvertToTimestamp(DateTime.Parse(e.fin.ToString())),
                                allDay = 1,
                                color = "#0555FA",
                                url = "/objetivos/detalle_objetivo/" + e.id
                            });
                            break;
                        case 2:
                            clientList.Add(new
                            {
                                id = e.id,
                                title = e.titulo,
                                description = e.descripcion,
                                start = ConvertToTimestamp(DateTime.Parse(e.inicio.ToString())),
                                end = ConvertToTimestamp(DateTime.Parse(e.fin.ToString())),
                                allDay = 1,
                                color = "#41b9e6",
                                url = "/objetivos/detalle_objetivo/" + e.id
                            });
                            break;
                        case 3:
                            clientList.Add(new
                            {
                                id = e.id,
                                title = e.titulo,
                                description = e.descripcion,
                                start = ConvertToTimestamp(DateTime.Parse(e.inicio.ToString())),
                                end = ConvertToTimestamp(DateTime.Parse(e.fin.ToString())),
                                allDay = 1,
                                color = "#008c5a",
                                url = "/AccionMejora/detalle_accion/" + e.id
                            });
                            break;
                        case 4:
                            clientList.Add(new
                            {
                                id = e.id,
                                title = e.titulo,
                                description = e.descripcion,
                                start = ConvertToTimestamp(DateTime.Parse(e.inicio.ToString())),
                                end = ConvertToTimestamp(DateTime.Parse(e.fin.ToString())),
                                allDay = 1,
                                color = "#55be5a",
                                url = "/AccionMejora/detalle_accion/" + e.id
                            });
                            break;
                        case 5:
                            clientList.Add(new
                            {
                                id = e.id,
                                title = e.titulo,
                                description = e.descripcion,
                                start = ConvertToTimestamp(DateTime.Parse(e.inicio.ToString())),
                                end = ConvertToTimestamp(DateTime.Parse(e.fin.ToString())),
                                allDay = 1,
                                color = "#ff0f64",
                                url = "/emergencias/detalle_emergencia/" + e.id
                            });
                            break;
                        case 6:
                            clientList.Add(new
                            {
                                id = e.id,
                                title = e.titulo,
                                description = e.descripcion,
                                start = ConvertToTimestamp(DateTime.Parse(e.inicio.ToString())),
                                end = ConvertToTimestamp(DateTime.Parse(e.fin.ToString())),
                                allDay = 1,
                                color = "#ff5a0f",
                                url = "/actasreunion/detalle_reunion/" + e.id
                            });
                            break;
                        case 7:
                            clientList.Add(new
                            {
                                id = e.id,
                                title = e.titulo,
                                description = e.descripcion,
                                start = ConvertToTimestamp(DateTime.Parse(e.inicio.ToString())),
                                end = ConvertToTimestamp(DateTime.Parse(e.fin.ToString())),
                                allDay = 1,
                                color = "#38d130",
                                url = "/AccionMejora/detalle_accion/" + e.id
                            });
                            break;
                    }
                    #endregion

                }
            } return Json(clientList.ToArray(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Convierte de Unix Timestamp a Datetime
        /// summary>
        /// <param name="timestamp">Date to convertparam>
        /// <returns>returns>
        private static DateTime FromUnixTimestamp(double timestamp)
        {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
        }
        /// <summary>
        /// convierte de DateTime a UNIX Timestamp
        /// summary>
        /// <param name="value">Date to convertparam>
        /// <returns>returns>
        private static double ConvertToTimestamp(DateTime value)
        {
            //create Timespan by subtracting the value provided from
            //the Unix Epoch
            TimeSpan span = (value - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime());
            //return the total seconds (which is a UNIX timestamp)
            return (double)span.TotalSeconds;
        }
        #endregion

    }
}
