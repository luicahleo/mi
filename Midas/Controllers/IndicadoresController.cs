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
    public class IndicadoresController : Controller
    {
        //
        // GET: /Indicadores/

        public ActionResult gestion_indicadores()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }
            int centralElegida = int.Parse(Session["CentralElegida"].ToString());

            centros central = Datos.ObtenerCentroPorID(centralElegida);

            List<indicadores> refe = Datos.ListarIndicadoresAplicables(central);
            ViewData["indicadores"] = refe;

            #region generacion fichero
            if (Session["nombreArchivo"] != null && Session["nombreArchivo"].ToString() != "")
            {
                if (Session["nombreArchivo"].ToString().Contains("xlsx"))
                {
                    Response.ContentType = "application/msexcel";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=ExportacionIndicadores.xlsx");
                    Response.TransmitFile(Session["nombreArchivo"].ToString());
                    Response.End();
                }
                Session["nombreArchivo"] = "";
            }
            #endregion

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult gestion_indicadores(FormCollection collection)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }
            auditorias audi = new auditorias();
            string formulario = collection["hdFormularioEjecutado"];

            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            int idCentral = centroseleccionado.id;

            if (formulario == "btnAddIndicador")
            {
                #region añadir indicador
                if (collection["ctl00$MainContent$ddlindicadores"] != null)
                {
                    int idIndicador = int.Parse(collection["ctl00$MainContent$ddlindicadores"]);
                    Datos.CrearPlanificacionIndicador(idIndicador, centroseleccionado);
                }


                List<indicadores> refe = Datos.ListarIndicadoresAplicables(centroseleccionado);
                ViewData["indicadores"] = refe;

                Session["EdicionIndicadoresMensaje"] = "Indicador asignado correctamente";
                return View();
                #endregion
            }

            if (formulario == "btnImprimirCatalogo")
            {
                #region impresión catálogo

                List<VISTA_Indicadores> indicadores = Datos.ListarIndicadores();

                #region generacion fichero
                string sourceFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaPlantillaWord"] + "ExportacionIndicadoresMaestro.xlsx");
                string destinationFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaGeneracionWord"] + "ExportacionIndicadoresMaestro_" + DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + ".xlsx");

                Session["source"] = sourceFile;
                Session["destino"] = destinationFile;
                // Create a copy of the template file and open the copy 
                System.IO.File.Copy(sourceFile, destinationFile, true);

                #region impresion

                using (SpreadsheetDocument document = SpreadsheetDocument.Open(destinationFile, true))
                {
                    SheetData sheetData = document.WorkbookPart.WorksheetParts.ElementAt(0).Worksheet.Elements<SheetData>().First();
                    foreach (VISTA_Indicadores ind in indicadores)
                    {
                        #region crear campos y celdas
                        Row row = new Row();

                        #region declaracion campos
                        string nombre = string.Empty;
                        string tecnologia = string.Empty;
                        string unidad = string.Empty;
                        string proceso = string.Empty;

                        #endregion
                        #region asignacion campos
                        if (ind.Nombre == null)
                            nombre = string.Empty;
                        else
                            nombre = ind.Nombre;
                        if (ind.NombreTecnologia == null)
                            tecnologia = string.Empty;
                        else
                            tecnologia = ind.NombreTecnologia;
                        if (ind.Unidad == null)
                            unidad = string.Empty;
                        else
                            unidad = ind.Unidad;
                        if (ind.NombreProceso == null)
                            proceso = string.Empty;
                        else
                            proceso = ind.NombreProceso;

                        #endregion

                        #region construccion fila
                        row.Append(
                         Datos.ConstructCell(nombre, CellValues.String),
                        Datos.ConstructCell(tecnologia, CellValues.String),
                        Datos.ConstructCell(unidad, CellValues.String),
                        Datos.ConstructCell(proceso, CellValues.String));
                        #endregion
                        sheetData.AppendChild(row);
                        #endregion
                    }

                    // save worksheet
                    document.WorkbookPart.WorksheetParts.First().Worksheet.Save();

                    document.WorkbookPart.Workbook.Save();

                    Session["nombreArchivo"] = destinationFile;
                }
                #endregion
                #endregion


                List<indicadores> refe = Datos.ListarIndicadoresAplicables(centroseleccionado);
                ViewData["indicadores"] = refe;
                return RedirectToAction("gestion_indicadores", "Indicadores");
                #endregion
            }
            else
            {
                int anio = int.Parse(collection["ctl00$MainContent$ddlAnio"]);

                List<VISTA_ListadoIndicadores> indicadores = Datos.GetInformeIndicador(idCentral, anio);

                #region generacion fichero
                string sourceFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaPlantillaWord"] + "ExportacionIndicadores.xlsx");
                string destinationFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaGeneracionWord"] + "ExportacionIndicadores_" + DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + ".xlsx");

                Session["source"] = sourceFile;
                Session["destino"] = destinationFile;
                // Create a copy of the template file and open the copy 
                System.IO.File.Copy(sourceFile, destinationFile, true);

                #region impresion

                using (SpreadsheetDocument document = SpreadsheetDocument.Open(destinationFile, true))
                {
                    SheetData sheetData = document.WorkbookPart.WorksheetParts.ElementAt(0).Worksheet.Elements<SheetData>().First();
                    foreach (VISTA_ListadoIndicadores ind in indicadores)
                    {
                        #region crear campos y celdas
                        Row row = new Row();

                        #region declaracion campos
                        string nombre = string.Empty;
                        string metodo = string.Empty;
                        string proceso = string.Empty;
                        string unidad = string.Empty;
                        string tecnologia = string.Empty;
                        string descripcion = string.Empty;
                        string period = string.Empty;
                        string operacion = string.Empty;
                        string aniomed = string.Empty;
                        string ref1 = string.Empty;
                        string ref2 = string.Empty;
                        string ref3 = string.Empty;
                        string ref4 = string.Empty;
                        string ref5 = string.Empty;
                        string ref6 = string.Empty;
                        string ref7 = string.Empty;
                        string ref8 = string.Empty;
                        string ref9 = string.Empty;
                        string ref10 = string.Empty;
                        string ref11 = string.Empty;
                        string ref12 = string.Empty;

                        string med1 = string.Empty;
                        string med2 = string.Empty;
                        string med3 = string.Empty;
                        string med4 = string.Empty;
                        string med5 = string.Empty;
                        string med6 = string.Empty;
                        string med7 = string.Empty;
                        string med8 = string.Empty;
                        string med9 = string.Empty;
                        string med10 = string.Empty;
                        string med11 = string.Empty;
                        string med12 = string.Empty;

                        string calc1 = string.Empty;
                        string calc2 = string.Empty;
                        string calc3 = string.Empty;
                        string calc4 = string.Empty;
                        string calc5 = string.Empty;
                        string calc6 = string.Empty;
                        string calc7 = string.Empty;
                        string calc8 = string.Empty;
                        string calc9 = string.Empty;
                        string calc10 = string.Empty;
                        string calc11 = string.Empty;
                        string calc12 = string.Empty;
                        #endregion
                        #region asignacion campos
                        if (ind.Nombre == null)
                            nombre = string.Empty;
                        else
                            nombre = ind.Nombre;
                        if (ind.MetodoMedicion == null)
                            metodo = string.Empty;
                        else
                            metodo = ind.MetodoMedicion;
                        if (ind.nombreProceso == null)
                            proceso = string.Empty;
                        else
                            proceso = ind.nombreProceso;
                        if (ind.Unidad == null)
                            unidad = string.Empty;
                        else
                            unidad = ind.Unidad;
                        if (ind.nombretecnologia == null)
                            tecnologia = string.Empty;
                        else
                            tecnologia = ind.nombretecnologia;
                        if (ind.Descripcion == null)
                            descripcion = string.Empty;
                        else
                            descripcion = ind.Descripcion;
                        if (ind.Periodicidad == null)
                            period = string.Empty;
                        else
                            period = ind.Periodicidad;
                        if (ind.Operacion == null)
                            operacion = string.Empty;
                        else
                            operacion = ind.Operacion;
                        if (ind.anio == null)
                            aniomed = string.Empty;
                        else
                            aniomed = ind.anio.ToString().Replace("Computo", "Cómputo").Replace("AcumuladoPeriodo", "Acumulado Período").Replace("Promedio Periodo", "Promedio Período");
                        #endregion

                        #region valoresreferencia
                        if (ind.ValorReferencia1 == null)
                            ref1 = string.Empty;
                        else
                            ref1 = ind.ValorReferencia1.ToString();
                        if (ind.ValorReferencia2 == null)
                            ref2 = string.Empty;
                        else
                            ref2 = ind.ValorReferencia2.ToString();
                        if (ind.ValorReferencia3 == null)
                            ref3 = string.Empty;
                        else
                            ref3 = ind.ValorReferencia3.ToString();
                        if (ind.ValorReferencia4 == null)
                            ref4 = string.Empty;
                        else
                            ref4 = ind.ValorReferencia4.ToString();
                        if (ind.ValorReferencia5 == null)
                            ref5 = string.Empty;
                        else
                            ref5 = ind.ValorReferencia5.ToString();
                        if (ind.ValorReferencia6 == null)
                            ref6 = string.Empty;
                        else
                            ref6 = ind.ValorReferencia6.ToString();
                        if (ind.ValorReferencia7 == null)
                            ref7 = string.Empty;
                        else
                            ref7 = ind.ValorReferencia7.ToString();
                        if (ind.ValorReferencia8 == null)
                            ref8 = string.Empty;
                        else
                            ref8 = ind.ValorReferencia8.ToString();
                        if (ind.ValorReferencia9 == null)
                            ref9 = string.Empty;
                        else
                            ref9 = ind.ValorReferencia9.ToString();
                        if (ind.ValorReferencia10 == null)
                            ref10 = string.Empty;
                        else
                            ref10 = ind.ValorReferencia10.ToString();
                        if (ind.ValorReferencia11 == null)
                            ref11 = string.Empty;
                        else
                            ref11 = ind.ValorReferencia11.ToString();
                        if (ind.ValorReferencia12 == null)
                            ref12 = string.Empty;
                        else
                            ref12 = ind.ValorReferencia12.ToString();
                        #endregion
                        #region valoresmedicion
                        if (ind.Valoracion1 == null)
                            med1 = string.Empty;
                        else
                            med1 = ind.Valoracion1.ToString();
                        if (ind.Valoracion2 == null)
                            med2 = string.Empty;
                        else
                            med2 = ind.Valoracion2.ToString();
                        if (ind.Valoracion3 == null)
                            med3 = string.Empty;
                        else
                            med3 = ind.Valoracion3.ToString();
                        if (ind.Valoracion4 == null)
                            med4 = string.Empty;
                        else
                            med4 = ind.Valoracion4.ToString();
                        if (ind.Valoracion5 == null)
                            med5 = string.Empty;
                        else
                            med5 = ind.Valoracion5.ToString();
                        if (ind.Valoracion6 == null)
                            med6 = string.Empty;
                        else
                            med6 = ind.Valoracion6.ToString();
                        if (ind.Valoracion7 == null)
                            med7 = string.Empty;
                        else
                            med7 = ind.Valoracion7.ToString();
                        if (ind.Valoracion8 == null)
                            med8 = string.Empty;
                        else
                            med8 = ind.Valoracion8.ToString();
                        if (ind.Valoracion9 == null)
                            med9 = string.Empty;
                        else
                            med9 = ind.Valoracion9.ToString();
                        if (ind.Valoracion10 == null)
                            med10 = string.Empty;
                        else
                            med10 = ind.Valoracion10.ToString();
                        if (ind.Valoracion11 == null)
                            med11 = string.Empty;
                        else
                            med11 = ind.Valoracion11.ToString();
                        if (ind.Valoracion12 == null)
                            med12 = string.Empty;
                        else
                            med12 = ind.Valoracion12.ToString();
                        #endregion
                        #region valorescalculados
                        if (ind.ValorCalculado1 == null)
                            calc1 = string.Empty;
                        else
                            calc1 = ind.ValorCalculado1.ToString();
                        if (ind.ValorCalculado2 == null)
                            calc2 = string.Empty;
                        else
                            calc2 = ind.ValorCalculado2.ToString();
                        if (ind.ValorCalculado3 == null)
                            calc3 = string.Empty;
                        else
                            calc3 = ind.ValorCalculado3.ToString();
                        if (ind.ValorCalculado4 == null)
                            calc4 = string.Empty;
                        else
                            calc4 = ind.ValorCalculado4.ToString();
                        if (ind.ValorCalculado5 == null)
                            calc5 = string.Empty;
                        else
                            calc5 = ind.ValorCalculado5.ToString();
                        if (ind.ValorCalculado6 == null)
                            calc6 = string.Empty;
                        else
                            calc6 = ind.ValorCalculado6.ToString();
                        if (ind.ValorCalculado7 == null)
                            calc7 = string.Empty;
                        else
                            calc7 = ind.ValorCalculado7.ToString();
                        if (ind.ValorCalculado8 == null)
                            calc8 = string.Empty;
                        else
                            calc8 = ind.ValorCalculado8.ToString();
                        if (ind.ValorCalculado9 == null)
                            calc9 = string.Empty;
                        else
                            calc9 = ind.ValorCalculado9.ToString();
                        if (ind.ValorCalculado10 == null)
                            calc10 = string.Empty;
                        else
                            calc10 = ind.ValorCalculado10.ToString();
                        if (ind.ValorCalculado11 == null)
                            calc11 = string.Empty;
                        else
                            calc11 = ind.ValorCalculado11.ToString();
                        if (ind.ValorCalculado12 == null)
                            calc12 = string.Empty;
                        else
                            calc12 = ind.ValorCalculado12.ToString();
                        #endregion

                        #region construccion fila
                        row.Append(
                        Datos.ConstructCell(nombre, CellValues.String),
                        Datos.ConstructCell(metodo, CellValues.String),
                        Datos.ConstructCell(proceso, CellValues.String),
                        Datos.ConstructCell(unidad, CellValues.String),
                        Datos.ConstructCell(tecnologia, CellValues.String),
                        Datos.ConstructCell(descripcion, CellValues.String),
                        Datos.ConstructCell(period, CellValues.String),
                        Datos.ConstructCell(operacion, CellValues.String),
                        Datos.ConstructCell(aniomed, CellValues.String),
                        Datos.ConstructCell(ref1, CellValues.String),
                        Datos.ConstructCell(ref2, CellValues.String),
                        Datos.ConstructCell(ref3, CellValues.String),
                        Datos.ConstructCell(ref4, CellValues.String),
                        Datos.ConstructCell(ref5, CellValues.String),
                        Datos.ConstructCell(ref6, CellValues.String),
                        Datos.ConstructCell(ref7, CellValues.String),
                        Datos.ConstructCell(ref8, CellValues.String),
                        Datos.ConstructCell(ref9, CellValues.String),
                        Datos.ConstructCell(ref10, CellValues.String),
                        Datos.ConstructCell(ref11, CellValues.String),
                        Datos.ConstructCell(ref12, CellValues.String),
                        Datos.ConstructCell(med1, CellValues.String),
                        Datos.ConstructCell(med2, CellValues.String),
                        Datos.ConstructCell(med3, CellValues.String),
                        Datos.ConstructCell(med4, CellValues.String),
                        Datos.ConstructCell(med5, CellValues.String),
                        Datos.ConstructCell(med6, CellValues.String),
                        Datos.ConstructCell(med7, CellValues.String),
                        Datos.ConstructCell(med8, CellValues.String),
                        Datos.ConstructCell(med9, CellValues.String),
                        Datos.ConstructCell(med10, CellValues.String),
                        Datos.ConstructCell(med11, CellValues.String),
                        Datos.ConstructCell(med12, CellValues.String),
                        Datos.ConstructCell(calc1, CellValues.String),
                        Datos.ConstructCell(calc2, CellValues.String),
                        Datos.ConstructCell(calc3, CellValues.String),
                        Datos.ConstructCell(calc4, CellValues.String),
                        Datos.ConstructCell(calc5, CellValues.String),
                        Datos.ConstructCell(calc6, CellValues.String),
                        Datos.ConstructCell(calc7, CellValues.String),
                        Datos.ConstructCell(calc8, CellValues.String),
                        Datos.ConstructCell(calc9, CellValues.String),
                        Datos.ConstructCell(calc10, CellValues.String),
                        Datos.ConstructCell(calc11, CellValues.String),
                        Datos.ConstructCell(calc12, CellValues.String));
                        #endregion
                        sheetData.AppendChild(row);
                        #endregion
                    }

                    // save worksheet
                    document.WorkbookPart.WorksheetParts.First().Worksheet.Save();

                    document.WorkbookPart.Workbook.Save();

                    Session["nombreArchivo"] = destinationFile;
                }
                #endregion
                #endregion

                List<indicadores> refe = Datos.ListarIndicadoresAplicables(centroseleccionado);
                ViewData["indicadores"] = refe;
                return RedirectToAction("gestion_indicadores", "Indicadores");
            }
        }

        public ActionResult detalle_indicador(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            Session["modulo"] = "9";

            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            int idCentral = centroseleccionado.id;

            ViewData["idIndicador"] = id;

            ViewData["procesos"] = Datos.ListarProcesos();
            ViewData["tecnologias"] = Datos.ListarTecnologias();

            indicadores buscarIndicador = Datos.ObtenerIndicador(id);
            ViewData["EditarIndicador"] = buscarIndicador;

            indicadores_imputacion buscarImputacion = Datos.ObtenerValoracionInd(id, DateTime.Now.Year, idCentral);

            if (buscarImputacion == null)
            {
                Datos.CrearValoracionIndicador(id, DateTime.Now.Year, idCentral);

                buscarImputacion = Datos.ObtenerValoracionInd(id, DateTime.Now.Year, idCentral);
            }

            ViewData["EditarImputacion"] = buscarImputacion;

            #region generacion fichero
            if (Session["nombreArchivo"] != null && Session["nombreArchivo"].ToString() != "")
            {
                if (Session["nombreArchivo"].ToString().Contains("docx"))
                {
                    Response.ContentType = "application/msword";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=ExportacionIndicador.docx");
                    Response.TransmitFile(Session["nombreArchivo"].ToString());
                    Response.End();
                }
                Session["nombreArchivo"] = "";
            }
            #endregion
            return View();
        }

        public bool comprobarfrecuenciasoperacion(string frecuenciaimp, string frecuenciaexp, string calculo)
        {
            #region compatibilidad de frecuencias
            bool valido = false;
            if ((frecuenciaimp == "Mensual" && frecuenciaexp == "Mensual") ||
                (frecuenciaimp == "Mensual" && frecuenciaexp == "Bimestral") ||
                (frecuenciaimp == "Mensual" && frecuenciaexp == "Trimestral") ||
                (frecuenciaimp == "Mensual" && frecuenciaexp == "Cuatrimestral") ||
                (frecuenciaimp == "Mensual" && frecuenciaexp == "Semestral") ||
                (frecuenciaimp == "Mensual" && frecuenciaexp == "Anual") ||
                (frecuenciaimp == "Bimestral" && frecuenciaexp == "Bimestral") ||
                (frecuenciaimp == "Bimestral" && frecuenciaexp == "Cuatrimestral") ||
                (frecuenciaimp == "Bimestral" && frecuenciaexp == "Semestral") ||
                (frecuenciaimp == "Bimestral" && frecuenciaexp == "Anual") ||
                (frecuenciaimp == "Trimestral" && frecuenciaexp == "Trimestral") ||
                (frecuenciaimp == "Trimestral" && frecuenciaexp == "Semestral") ||
                (frecuenciaimp == "Trimestral" && frecuenciaexp == "Anual") ||
                (frecuenciaimp == "Cuatrimestral" && frecuenciaexp == "Cuatrimestral") ||
                (frecuenciaimp == "Cuatrimestral" && frecuenciaexp == "Anual") ||
                (frecuenciaimp == "Semestral" && frecuenciaexp == "Semestral") ||
                (frecuenciaimp == "Semestral" && frecuenciaexp == "Anual") ||
                (frecuenciaimp == "Anual" && frecuenciaexp == "Anual")
                )
            {
                valido = true;
            }
            else
            {
                valido = false;
                Session["EdicionIndicadorError"] = "Las frecuencias de medición y explotación no son compatibles.";
            }
            #endregion

            #region comprobar requisitos de cómputo
            if (calculo == "Computo" && (frecuenciaexp != frecuenciaimp))
            {
                valido = false;
                Session["EdicionIndicadorError"] = "Al seleccionar la operación Cómputo, las frecuencias de medición y explotación deben ser iguales.";
            }
            #endregion

            return valido;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult detalle_indicador(int id, FormCollection collection, HttpPostedFileBase[] file)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            int anio = int.Parse(collection["ctl00$MainContent$ddlAnio"].ToString());

            string formulario = collection["hdFormularioEjecutado"];

            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            int idCentral = centroseleccionado.id;

            if (formulario == "GuardarIndicador")
            {
                #region Guardar Indicador
                try
                {
                    indicadores_imputacion actualizar = Datos.ObtenerValoracionInd(id, anio, idCentral);
                    indicadores indic = Datos.ObtenerIndicador(actualizar.IdIndicador);

                    actualizar.Operacion = collection["ctl00$MainContent$ddlOperacion"].ToString();

                    #region mediciones
                    if (collection["ctl00$MainContent$txtMed1"] != null && collection["ctl00$MainContent$txtMed1"] != string.Empty)
                        actualizar.Valoracion1 = decimal.Parse(collection["ctl00$MainContent$txtMed1"].ToString().Replace(".", ","));
                    else
                        actualizar.Valoracion1 = null;
                    if (collection["ctl00$MainContent$txtMed2"] != null && collection["ctl00$MainContent$txtMed2"] != string.Empty)
                        actualizar.Valoracion2 = decimal.Parse(collection["ctl00$MainContent$txtMed2"].ToString().Replace(".", ","));
                    else
                        actualizar.Valoracion2 = null;
                    if (collection["ctl00$MainContent$txtMed3"] != null && collection["ctl00$MainContent$txtMed3"] != string.Empty)
                        actualizar.Valoracion3 = decimal.Parse(collection["ctl00$MainContent$txtMed3"].ToString().Replace(".", ","));
                    else
                        actualizar.Valoracion3 = null;
                    if (collection["ctl00$MainContent$txtMed4"] != null && collection["ctl00$MainContent$txtMed4"] != string.Empty)
                        actualizar.Valoracion4 = decimal.Parse(collection["ctl00$MainContent$txtMed4"].ToString().Replace(".", ","));
                    else
                        actualizar.Valoracion4 = null;
                    if (collection["ctl00$MainContent$txtMed5"] != null && collection["ctl00$MainContent$txtMed5"] != string.Empty)
                        actualizar.Valoracion5 = decimal.Parse(collection["ctl00$MainContent$txtMed5"].ToString().Replace(".", ","));
                    else
                        actualizar.Valoracion5 = null;
                    if (collection["ctl00$MainContent$txtMed6"] != null && collection["ctl00$MainContent$txtMed6"] != string.Empty)
                        actualizar.Valoracion6 = decimal.Parse(collection["ctl00$MainContent$txtMed6"].ToString().Replace(".", ","));
                    else
                        actualizar.Valoracion6 = null;
                    if (collection["ctl00$MainContent$txtMed7"] != null && collection["ctl00$MainContent$txtMed7"] != string.Empty)
                        actualizar.Valoracion7 = decimal.Parse(collection["ctl00$MainContent$txtMed7"].ToString().Replace(".", ","));
                    else
                        actualizar.Valoracion7 = null;
                    if (collection["ctl00$MainContent$txtMed8"] != null && collection["ctl00$MainContent$txtMed8"] != string.Empty)
                        actualizar.Valoracion8 = decimal.Parse(collection["ctl00$MainContent$txtMed8"].ToString().Replace(".", ","));
                    else
                        actualizar.Valoracion8 = null;
                    if (collection["ctl00$MainContent$txtMed9"] != null && collection["ctl00$MainContent$txtMed9"] != string.Empty)
                        actualizar.Valoracion9 = decimal.Parse(collection["ctl00$MainContent$txtMed9"].ToString().Replace(".", ","));
                    else
                        actualizar.Valoracion9 = null;
                    if (collection["ctl00$MainContent$txtMed10"] != null && collection["ctl00$MainContent$txtMed10"] != string.Empty)
                        actualizar.Valoracion10 = decimal.Parse(collection["ctl00$MainContent$txtMed10"].ToString().Replace(".", ","));
                    else
                        actualizar.Valoracion10 = null;
                    if (collection["ctl00$MainContent$txtMed11"] != null && collection["ctl00$MainContent$txtMed11"] != string.Empty)
                        actualizar.Valoracion11 = decimal.Parse(collection["ctl00$MainContent$txtMed11"].ToString().Replace(".", ","));
                    else
                        actualizar.Valoracion11 = null;
                    if (collection["ctl00$MainContent$txtMed12"] != null && collection["ctl00$MainContent$txtMed12"] != string.Empty)
                        actualizar.Valoracion12 = decimal.Parse(collection["ctl00$MainContent$txtMed12"].ToString().Replace(".", ","));
                    else
                        actualizar.Valoracion12 = null;
                    #endregion
                    #region valoresreferencia
                    if (collection["ctl00$MainContent$txtRef1"] != null && collection["ctl00$MainContent$txtRef1"] != string.Empty)
                        actualizar.ValorReferencia1 = decimal.Parse(collection["ctl00$MainContent$txtRef1"].ToString().Replace(".", ","));
                    else
                        actualizar.ValorReferencia1 = null;
                    if (collection["ctl00$MainContent$txtRef2"] != null && collection["ctl00$MainContent$txtRef2"] != string.Empty)
                        actualizar.ValorReferencia2 = decimal.Parse(collection["ctl00$MainContent$txtRef2"].ToString().Replace(".", ","));
                    else
                        actualizar.ValorReferencia2 = null;
                    if (collection["ctl00$MainContent$txtRef3"] != null && collection["ctl00$MainContent$txtRef3"] != string.Empty)
                        actualizar.ValorReferencia3 = decimal.Parse(collection["ctl00$MainContent$txtRef3"].ToString().Replace(".", ","));
                    else
                        actualizar.ValorReferencia3 = null;
                    if (collection["ctl00$MainContent$txtRef4"] != null && collection["ctl00$MainContent$txtRef4"] != string.Empty)
                        actualizar.ValorReferencia4 = decimal.Parse(collection["ctl00$MainContent$txtRef4"].ToString().Replace(".", ","));
                    else
                        actualizar.ValorReferencia4 = null;
                    if (collection["ctl00$MainContent$txtRef5"] != null && collection["ctl00$MainContent$txtRef5"] != string.Empty)
                        actualizar.ValorReferencia5 = decimal.Parse(collection["ctl00$MainContent$txtRef5"].ToString().Replace(".", ","));
                    else
                        actualizar.ValorReferencia5 = null;
                    if (collection["ctl00$MainContent$txtRef6"] != null && collection["ctl00$MainContent$txtRef6"] != string.Empty)
                        actualizar.ValorReferencia6 = decimal.Parse(collection["ctl00$MainContent$txtRef6"].ToString().Replace(".", ","));
                    else
                        actualizar.ValorReferencia6 = null;
                    if (collection["ctl00$MainContent$txtRef7"] != null && collection["ctl00$MainContent$txtRef7"] != string.Empty)
                        actualizar.ValorReferencia7 = decimal.Parse(collection["ctl00$MainContent$txtRef7"].ToString().Replace(".", ","));
                    else
                        actualizar.ValorReferencia7 = null;
                    if (collection["ctl00$MainContent$txtRef8"] != null && collection["ctl00$MainContent$txtRef8"] != string.Empty)
                        actualizar.ValorReferencia8 = decimal.Parse(collection["ctl00$MainContent$txtRef8"].ToString().Replace(".", ","));
                    else
                        actualizar.ValorReferencia8 = null;
                    if (collection["ctl00$MainContent$txtRef9"] != null && collection["ctl00$MainContent$txtRef9"] != string.Empty)
                        actualizar.ValorReferencia9 = decimal.Parse(collection["ctl00$MainContent$txtRef9"].ToString().Replace(".", ","));
                    else
                        actualizar.ValorReferencia9 = null;
                    if (collection["ctl00$MainContent$txtRef10"] != null && collection["ctl00$MainContent$txtRef10"] != string.Empty)
                        actualizar.ValorReferencia10 = decimal.Parse(collection["ctl00$MainContent$txtRef10"].ToString().Replace(".", ","));
                    else
                        actualizar.ValorReferencia10 = null;
                    if (collection["ctl00$MainContent$txtRef11"] != null && collection["ctl00$MainContent$txtRef11"] != string.Empty)
                        actualizar.ValorReferencia11 = decimal.Parse(collection["ctl00$MainContent$txtRef11"].ToString().Replace(".", ","));
                    else
                        actualizar.ValorReferencia11 = null;
                    if (collection["ctl00$MainContent$txtRef12"] != null && collection["ctl00$MainContent$txtRef12"] != string.Empty)
                        actualizar.ValorReferencia12 = decimal.Parse(collection["ctl00$MainContent$txtRef12"].ToString().Replace(".", ","));
                    else
                        actualizar.ValorReferencia12 = null;
                    #endregion
                    #region valorescalculados
                    if (collection["ctl00$MainContent$txtCalc1"] != null && collection["ctl00$MainContent$txtCalc1"] != string.Empty)
                        actualizar.ValorCalculado1 = decimal.Parse(collection["ctl00$MainContent$txtCalc1"].ToString().Replace(".", ","));
                    else
                        actualizar.ValorCalculado1 = null;
                    if (collection["ctl00$MainContent$txtCalc2"] != null && collection["ctl00$MainContent$txtCalc2"] != string.Empty)
                        actualizar.ValorCalculado2 = decimal.Parse(collection["ctl00$MainContent$txtCalc2"].ToString().Replace(".", ","));
                    else
                        actualizar.ValorCalculado2 = null;
                    if (collection["ctl00$MainContent$txtCalc3"] != null && collection["ctl00$MainContent$txtCalc3"] != string.Empty)
                        actualizar.ValorCalculado3 = decimal.Parse(collection["ctl00$MainContent$txtCalc3"].ToString().Replace(".", ","));
                    else
                        actualizar.ValorCalculado3 = null;
                    if (collection["ctl00$MainContent$txtCalc4"] != null && collection["ctl00$MainContent$txtCalc4"] != string.Empty)
                        actualizar.ValorCalculado4 = decimal.Parse(collection["ctl00$MainContent$txtCalc4"].ToString().Replace(".", ","));
                    else
                        actualizar.ValorCalculado4 = null;
                    if (collection["ctl00$MainContent$txtCalc5"] != null && collection["ctl00$MainContent$txtCalc5"] != string.Empty)
                        actualizar.ValorCalculado5 = decimal.Parse(collection["ctl00$MainContent$txtCalc5"].ToString().Replace(".", ","));
                    else
                        actualizar.ValorCalculado5 = null;
                    if (collection["ctl00$MainContent$txtCalc6"] != null && collection["ctl00$MainContent$txtCalc6"] != string.Empty)
                        actualizar.ValorCalculado6 = decimal.Parse(collection["ctl00$MainContent$txtCalc6"].ToString().Replace(".", ","));
                    else
                        actualizar.ValorCalculado6 = null;
                    if (collection["ctl00$MainContent$txtCalc7"] != null && collection["ctl00$MainContent$txtCalc7"] != string.Empty)
                        actualizar.ValorCalculado7 = decimal.Parse(collection["ctl00$MainContent$txtCalc7"].ToString().Replace(".", ","));
                    else
                        actualizar.ValorCalculado7 = null;
                    if (collection["ctl00$MainContent$txtCalc8"] != null && collection["ctl00$MainContent$txtCalc8"] != string.Empty)
                        actualizar.ValorCalculado8 = decimal.Parse(collection["ctl00$MainContent$txtCalc8"].ToString().Replace(".", ","));
                    else
                        actualizar.ValorCalculado8 = null;
                    if (collection["ctl00$MainContent$txtCalc9"] != null && collection["ctl00$MainContent$txtCalc9"] != string.Empty)
                        actualizar.ValorCalculado9 = decimal.Parse(collection["ctl00$MainContent$txtCalc9"].ToString().Replace(".", ","));
                    else
                        actualizar.ValorCalculado9 = null;
                    if (collection["ctl00$MainContent$txtCalc10"] != null && collection["ctl00$MainContent$txtCalc10"] != string.Empty)
                        actualizar.ValorCalculado10 = decimal.Parse(collection["ctl00$MainContent$txtCalc10"].ToString().Replace(".", ","));
                    else
                        actualizar.ValorCalculado10 = null;
                    if (collection["ctl00$MainContent$txtCalc11"] != null && collection["ctl00$MainContent$txtCalc11"] != string.Empty)
                        actualizar.ValorCalculado11 = decimal.Parse(collection["ctl00$MainContent$txtCalc11"].ToString().Replace(".", ","));
                    else
                        actualizar.ValorCalculado11 = null;
                    if (collection["ctl00$MainContent$txtCalc12"] != null && collection["ctl00$MainContent$txtCalc12"] != string.Empty)
                        actualizar.ValorCalculado12 = decimal.Parse(collection["ctl00$MainContent$txtCalc12"].ToString().Replace(".", ","));
                    else
                        actualizar.ValorCalculado12 = null;
                    #endregion
                    #region calculos
                    switch (actualizar.Operacion)
                    {
                        case "Computo":
                            actualizar.ValorCalculado1 = actualizar.Valoracion1;
                            actualizar.ValorCalculado2 = actualizar.Valoracion2;
                            actualizar.ValorCalculado3 = actualizar.Valoracion3;
                            actualizar.ValorCalculado4 = actualizar.Valoracion4;
                            actualizar.ValorCalculado5 = actualizar.Valoracion5;
                            actualizar.ValorCalculado6 = actualizar.Valoracion6;
                            actualizar.ValorCalculado7 = actualizar.Valoracion7;
                            actualizar.ValorCalculado8 = actualizar.Valoracion8;
                            actualizar.ValorCalculado9 = actualizar.Valoracion9;
                            actualizar.ValorCalculado10 = actualizar.Valoracion10;
                            actualizar.ValorCalculado11 = actualizar.Valoracion11;
                            actualizar.ValorCalculado12 = actualizar.Valoracion12;
                            break;

                        case "Acumulado":
                            actualizar.ValorCalculado1 = actualizar.Valoracion1;
                            actualizar.ValorCalculado2 = actualizar.ValorCalculado1 + actualizar.Valoracion2;
                            actualizar.ValorCalculado3 = actualizar.ValorCalculado2 + actualizar.Valoracion3;
                            actualizar.ValorCalculado4 = actualizar.ValorCalculado3 + actualizar.Valoracion4;
                            actualizar.ValorCalculado5 = actualizar.ValorCalculado4 + actualizar.Valoracion5;
                            actualizar.ValorCalculado6 = actualizar.ValorCalculado5 + actualizar.Valoracion6;
                            actualizar.ValorCalculado7 = actualizar.ValorCalculado6 + actualizar.Valoracion7;
                            actualizar.ValorCalculado8 = actualizar.ValorCalculado7 + actualizar.Valoracion8;
                            actualizar.ValorCalculado9 = actualizar.ValorCalculado8 + actualizar.Valoracion9;
                            actualizar.ValorCalculado10 = actualizar.ValorCalculado9 + actualizar.Valoracion10;
                            actualizar.ValorCalculado11 = actualizar.ValorCalculado10 + actualizar.Valoracion11;
                            actualizar.ValorCalculado12 = actualizar.ValorCalculado11 + actualizar.Valoracion12;
                            break;

                        case "Promedio":
                            actualizar.ValorCalculado1 = actualizar.Valoracion1;
                            actualizar.ValorCalculado2 = (actualizar.ValorCalculado1 + actualizar.Valoracion2) / 2;
                            actualizar.ValorCalculado3 = (actualizar.Valoracion1 + actualizar.Valoracion2 + actualizar.Valoracion3) / 3;
                            actualizar.ValorCalculado4 = (actualizar.Valoracion1 + actualizar.Valoracion2 + actualizar.Valoracion3 + actualizar.Valoracion4) / 4;
                            actualizar.ValorCalculado5 = (actualizar.Valoracion1 + actualizar.Valoracion2 + actualizar.Valoracion3 + actualizar.Valoracion4 + actualizar.Valoracion5) / 5;
                            actualizar.ValorCalculado6 = (actualizar.Valoracion1 + actualizar.Valoracion2 + actualizar.Valoracion3 + actualizar.Valoracion4 + actualizar.Valoracion5 + actualizar.Valoracion6) / 6;
                            actualizar.ValorCalculado7 = (actualizar.Valoracion1 + actualizar.Valoracion2 + actualizar.Valoracion3 + actualizar.Valoracion4 + actualizar.Valoracion5 + actualizar.Valoracion6 + actualizar.Valoracion7) / 7;
                            actualizar.ValorCalculado8 = (actualizar.Valoracion1 + actualizar.Valoracion2 + actualizar.Valoracion3 + actualizar.Valoracion4 + actualizar.Valoracion5 + actualizar.Valoracion6 + actualizar.Valoracion7 + actualizar.Valoracion8) / 8;
                            actualizar.ValorCalculado9 = (actualizar.Valoracion1 + actualizar.Valoracion2 + actualizar.Valoracion3 + actualizar.Valoracion4 + actualizar.Valoracion5 + actualizar.Valoracion6 + actualizar.Valoracion7 + actualizar.Valoracion8 + actualizar.Valoracion9) / 9;
                            actualizar.ValorCalculado10 = (actualizar.Valoracion1 + actualizar.Valoracion2 + actualizar.Valoracion3 + actualizar.Valoracion4 + actualizar.Valoracion5 + actualizar.Valoracion6 + actualizar.Valoracion7 + actualizar.Valoracion8 + actualizar.Valoracion9 + actualizar.Valoracion10) / 10;
                            actualizar.ValorCalculado11 = (actualizar.Valoracion1 + actualizar.Valoracion2 + actualizar.Valoracion3 + actualizar.Valoracion4 + actualizar.Valoracion5 + actualizar.Valoracion6 + actualizar.Valoracion7 + actualizar.Valoracion8 + actualizar.Valoracion9 + actualizar.Valoracion10 + actualizar.Valoracion11) / 11;
                            actualizar.ValorCalculado12 = (actualizar.Valoracion1 + actualizar.Valoracion2 + actualizar.Valoracion3 + actualizar.Valoracion4 + actualizar.Valoracion5 + actualizar.Valoracion6 + actualizar.Valoracion7 + actualizar.Valoracion8 + actualizar.Valoracion9 + actualizar.Valoracion10 + actualizar.Valoracion11 + actualizar.Valoracion12) / 12;
                            break;

                        case "AcumuladoPeriodo":
                            if (indic.Periodicidad == "Mensual")
                            {
                                actualizar.ValorCalculado1 = actualizar.Valoracion1;
                                actualizar.ValorCalculado2 = actualizar.Valoracion2;
                                actualizar.ValorCalculado3 = actualizar.Valoracion3;
                                actualizar.ValorCalculado4 = actualizar.Valoracion4;
                                actualizar.ValorCalculado5 = actualizar.Valoracion5;
                                actualizar.ValorCalculado6 = actualizar.Valoracion6;
                                actualizar.ValorCalculado7 = actualizar.Valoracion7;
                                actualizar.ValorCalculado8 = actualizar.Valoracion8;
                                actualizar.ValorCalculado9 = actualizar.Valoracion9;
                                actualizar.ValorCalculado10 = actualizar.Valoracion10;
                                actualizar.ValorCalculado11 = actualizar.Valoracion11;
                                actualizar.ValorCalculado12 = actualizar.Valoracion12;
                            }
                            if (indic.Periodicidad == "Bimestral")
                            {
                                actualizar.ValorCalculado2 = actualizar.Valoracion1 + actualizar.Valoracion2;
                                actualizar.ValorCalculado4 = actualizar.Valoracion3 + actualizar.Valoracion4;
                                actualizar.ValorCalculado6 = actualizar.Valoracion5 + actualizar.Valoracion6;
                                actualizar.ValorCalculado8 = actualizar.Valoracion7 + actualizar.Valoracion8;
                                actualizar.ValorCalculado10 = actualizar.Valoracion9 + actualizar.Valoracion10;
                                actualizar.ValorCalculado12 = actualizar.Valoracion11 + actualizar.Valoracion12;
                            }

                            if (indic.Periodicidad == "Trimestral")
                            {
                                actualizar.ValorCalculado3 = actualizar.Valoracion1 + actualizar.Valoracion2 + actualizar.Valoracion3;
                                actualizar.ValorCalculado6 = actualizar.Valoracion4 + actualizar.Valoracion5 + actualizar.Valoracion6;
                                actualizar.ValorCalculado9 = actualizar.Valoracion7 + actualizar.Valoracion8 + actualizar.Valoracion9;
                                actualizar.ValorCalculado12 = actualizar.Valoracion10 + actualizar.Valoracion11 + actualizar.Valoracion12;
                            }

                            if (indic.Periodicidad == "Cuatrimestral")
                            {
                                actualizar.ValorCalculado4 = actualizar.Valoracion1 + actualizar.Valoracion2 + actualizar.Valoracion3 + actualizar.Valoracion4;
                                actualizar.ValorCalculado8 = actualizar.Valoracion5 + actualizar.Valoracion6 + actualizar.Valoracion7 + actualizar.Valoracion8;
                                actualizar.ValorCalculado12 = actualizar.Valoracion9 + actualizar.Valoracion10 + actualizar.Valoracion11 + actualizar.Valoracion12;
                            }
                            if (indic.Periodicidad == "Semestral")
                            {
                                actualizar.ValorCalculado6 = actualizar.Valoracion1 + actualizar.Valoracion2 + actualizar.Valoracion3 + actualizar.Valoracion4 + actualizar.Valoracion5 + actualizar.Valoracion6;
                                actualizar.ValorCalculado12 = actualizar.Valoracion7 + actualizar.Valoracion8 + actualizar.Valoracion9 + actualizar.Valoracion10 + actualizar.Valoracion11 + actualizar.Valoracion12;
                            }
                            if (indic.Periodicidad == "Anual")
                            {
                                actualizar.ValorCalculado12 = actualizar.Valoracion1 + actualizar.Valoracion2 + actualizar.Valoracion3 + actualizar.Valoracion4 + actualizar.Valoracion5 + actualizar.Valoracion6 + actualizar.Valoracion7 + actualizar.Valoracion8 + actualizar.Valoracion9 + actualizar.Valoracion10 + actualizar.Valoracion11 + actualizar.Valoracion12;
                            }
                            break;

                        case "PromedioPeriodo":
                            if (indic.Periodicidad == "Mensual")
                            {
                                actualizar.ValorCalculado1 = actualizar.Valoracion1;
                                actualizar.ValorCalculado2 = actualizar.Valoracion2;
                                actualizar.ValorCalculado3 = actualizar.Valoracion3;
                                actualizar.ValorCalculado4 = actualizar.Valoracion4;
                                actualizar.ValorCalculado5 = actualizar.Valoracion5;
                                actualizar.ValorCalculado6 = actualizar.Valoracion6;
                                actualizar.ValorCalculado7 = actualizar.Valoracion7;
                                actualizar.ValorCalculado8 = actualizar.Valoracion8;
                                actualizar.ValorCalculado9 = actualizar.Valoracion9;
                                actualizar.ValorCalculado10 = actualizar.Valoracion10;
                                actualizar.ValorCalculado11 = actualizar.Valoracion11;
                                actualizar.ValorCalculado12 = actualizar.Valoracion12;
                            }
                            if (indic.Periodicidad == "Bimestral")
                            {
                                actualizar.ValorCalculado2 = (actualizar.Valoracion1 + actualizar.Valoracion2) / 2;
                                actualizar.ValorCalculado4 = (actualizar.Valoracion3 + actualizar.Valoracion4) / 2;
                                actualizar.ValorCalculado6 = (actualizar.Valoracion5 + actualizar.Valoracion6) / 2;
                                actualizar.ValorCalculado8 = (actualizar.Valoracion7 + actualizar.Valoracion8) / 2;
                                actualizar.ValorCalculado10 = (actualizar.Valoracion9 + actualizar.Valoracion10) / 2;
                                actualizar.ValorCalculado12 = (actualizar.Valoracion11 + actualizar.Valoracion12) / 2;
                            }

                            if (indic.Periodicidad == "Trimestral")
                            {
                                actualizar.ValorCalculado3 = (actualizar.Valoracion1 + actualizar.Valoracion2 + actualizar.Valoracion3) / 3;
                                actualizar.ValorCalculado6 = (actualizar.Valoracion4 + actualizar.Valoracion5 + actualizar.Valoracion6) / 3;
                                actualizar.ValorCalculado9 = (actualizar.Valoracion7 + actualizar.Valoracion8 + actualizar.Valoracion9) / 3;
                                actualizar.ValorCalculado12 = (actualizar.Valoracion10 + actualizar.Valoracion11 + actualizar.Valoracion12) / 3;
                            }

                            if (indic.Periodicidad == "Cuatrimestral")
                            {
                                actualizar.ValorCalculado4 = (actualizar.Valoracion1 + actualizar.Valoracion2 + actualizar.Valoracion3 + actualizar.Valoracion4) / 4;
                                actualizar.ValorCalculado8 = (actualizar.Valoracion5 + actualizar.Valoracion6 + actualizar.Valoracion7 + actualizar.Valoracion8) / 4;
                                actualizar.ValorCalculado12 = (actualizar.Valoracion9 + actualizar.Valoracion10 + actualizar.Valoracion11 + actualizar.Valoracion12) / 4;
                            }
                            if (indic.Periodicidad == "Semestral")
                            {
                                actualizar.ValorCalculado6 = (actualizar.Valoracion1 + actualizar.Valoracion2 + actualizar.Valoracion3 + actualizar.Valoracion4 + actualizar.Valoracion5 + actualizar.Valoracion6) / 6;
                                actualizar.ValorCalculado12 = (actualizar.Valoracion7 + actualizar.Valoracion8 + actualizar.Valoracion9 + actualizar.Valoracion10 + actualizar.Valoracion11 + actualizar.Valoracion12) / 6;
                            }
                            if (indic.Periodicidad == "Anual")
                            {
                                actualizar.ValorCalculado12 = (actualizar.Valoracion1 + actualizar.Valoracion2 + actualizar.Valoracion3 + actualizar.Valoracion4 + actualizar.Valoracion5 + actualizar.Valoracion6 + actualizar.Valoracion7 + actualizar.Valoracion8 + actualizar.Valoracion9 + actualizar.Valoracion10 + actualizar.Valoracion11 + actualizar.Valoracion12) / 12;
                            }
                            break;
                    }
                    #endregion

                    Datos.ActualizarImputacion(actualizar);

                    ViewData["procesos"] = Datos.ListarProcesos();
                    ViewData["tecnologias"] = Datos.ListarTecnologias();
                    indicadores buscarIndicador = Datos.ObtenerIndicador(id);
                    ViewData["EditarIndicador"] = buscarIndicador;
                    indicadores_imputacion buscarImputacion = Datos.ObtenerValoracionInd(id, anio, idCentral);

                    if (buscarImputacion == null)
                    {
                        Datos.CrearValoracionIndicador(id, anio, idCentral);

                        buscarImputacion = Datos.ObtenerValoracionInd(id, anio, idCentral);
                    }

                    ViewData["EditarImputacion"] = buscarImputacion;
                    return View();
                }
                catch (Exception ex)
                {
                    #region exception
                    ViewData["procesos"] = Datos.ListarProcesos();
                    ViewData["tecnologias"] = Datos.ListarTecnologias();
                    indicadores buscarIndicador = Datos.ObtenerIndicador(id);
                    ViewData["EditarIndicador"] = buscarIndicador;
                    indicadores_imputacion buscarImputacion = Datos.ObtenerValoracionInd(id, DateTime.Now.Year, idCentral);

                    if (buscarImputacion == null)
                    {
                        Datos.CrearValoracionIndicador(id, anio, idCentral);

                        buscarImputacion = Datos.ObtenerValoracionInd(id, anio, idCentral);
                    }

                    ViewData["EditarImputacion"] = buscarImputacion;
                    Session["EdicionIndicadorError"] = "No se han podido guardar los datos, compruebe que los datos introducidos son correctos.";
                    return View();
                    #endregion
                }
                #endregion
            }
            if (formulario == "btnImprimir")
            {
                #region imprimir

                indicadores oIndicador = Datos.ObtenerIndicador(id);

                string sourceFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaPlantillaWord"] + "FichaIndicadorMensual.docx");
                if (oIndicador.Periodicidad == "Anual")
                {
                    sourceFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaPlantillaWord"] + "FichaIndicadorAnual.docx");
                }
                string destinationFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaGeneracionWord"] + "FichaIndicador_" + DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + ".docx");

                Session["source"] = sourceFile;
                Session["destino"] = destinationFile;
                // Create a copy of the template file and open the copy 
                System.IO.File.Copy(sourceFile, destinationFile, true);

                #region impresion


                //LISTADO REQUISITOS


                indicadores_imputacion oImputacion = Datos.ObtenerValoracionInd(id, anio, idCentral);
                if (oImputacion != null)
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add("T_Nombre", oIndicador.Nombre.Replace("\r\n", "<w:br/>"));
                    if (oIndicador.ProcesoAsociado != null && oIndicador.ProcesoAsociado != 0)
                    {
                        procesos proc = Datos.GetDatosProceso(int.Parse(oIndicador.ProcesoAsociado.ToString()));
                        keyValues.Add("T_Proceso", (proc.cod_proceso + " - " + proc.nombre).Replace("\r\n", "<w:br/>"));
                    }
                    keyValues.Add("T_Unidad", oIndicador.Unidad.Replace("\r\n", "<w:br/>"));
                    keyValues.Add("T_Metodo", oIndicador.MetodoMedicion.Replace("\r\n", "<w:br/>"));
                    #region valoresreferencia
                    if (oImputacion.ValorReferencia1 != null)
                        keyValues.Add("T_Ref1_", oImputacion.ValorReferencia1.ToString());
                    else
                        keyValues.Add("T_Ref1_", "");
                    if (oImputacion.ValorReferencia2 != null)
                        keyValues.Add("T_Ref2", oImputacion.ValorReferencia2.ToString());
                    else
                        keyValues.Add("T_Ref2", "");
                    if (oImputacion.ValorReferencia3 != null)
                        keyValues.Add("T_Ref3", oImputacion.ValorReferencia3.ToString());
                    else
                        keyValues.Add("T_Ref3", "");
                    if (oImputacion.ValorReferencia4 != null)
                        keyValues.Add("T_Ref4", oImputacion.ValorReferencia4.ToString());
                    else
                        keyValues.Add("T_Ref4", "");
                    if (oImputacion.ValorReferencia5 != null)
                        keyValues.Add("T_Ref5", oImputacion.ValorReferencia5.ToString());
                    else
                        keyValues.Add("T_Ref5", "");
                    if (oImputacion.ValorReferencia6 != null)
                        keyValues.Add("T_Ref6", oImputacion.ValorReferencia6.ToString());
                    else
                        keyValues.Add("T_Ref6", "");
                    if (oImputacion.ValorReferencia7 != null)
                        keyValues.Add("T_Ref7", oImputacion.ValorReferencia7.ToString());
                    else
                        keyValues.Add("T_Ref7", "");
                    if (oImputacion.ValorReferencia8 != null)
                        keyValues.Add("T_Ref8", oImputacion.ValorReferencia8.ToString());
                    else
                        keyValues.Add("T_Ref8", "");
                    if (oImputacion.ValorReferencia9 != null)
                        keyValues.Add("T_Ref9", oImputacion.ValorReferencia9.ToString());
                    else
                        keyValues.Add("T_Ref9", "");
                    if (oImputacion.ValorReferencia10 != null)
                        keyValues.Add("T_Ref10", oImputacion.ValorReferencia10.ToString());
                    else
                        keyValues.Add("T_Ref10", "");
                    if (oImputacion.ValorReferencia11 != null)
                        keyValues.Add("T_Ref11", oImputacion.ValorReferencia11.ToString());
                    else
                        keyValues.Add("T_Ref11", "");
                    if (oImputacion.ValorReferencia12 != null)
                        keyValues.Add("T_Ref12", oImputacion.ValorReferencia12.ToString());
                    else
                        keyValues.Add("T_Ref12", "");
                    #endregion
                    #region valoresMedicion
                    if (oImputacion.Valoracion1 != null)
                        keyValues.Add("T_Med1_", oImputacion.Valoracion1.ToString());
                    else
                        keyValues.Add("T_Med1_", "");
                    if (oImputacion.Valoracion2 != null)
                        keyValues.Add("T_Med2", oImputacion.Valoracion2.ToString());
                    else
                        keyValues.Add("T_Med2", "");
                    if (oImputacion.Valoracion3 != null)
                        keyValues.Add("T_Med3", oImputacion.Valoracion3.ToString());
                    else
                        keyValues.Add("T_Med3", "");
                    if (oImputacion.Valoracion4 != null)
                        keyValues.Add("T_Med4", oImputacion.Valoracion4.ToString());
                    else
                        keyValues.Add("T_Med4", "");
                    if (oImputacion.Valoracion5 != null)
                        keyValues.Add("T_Med5", oImputacion.Valoracion5.ToString());
                    else
                        keyValues.Add("T_Med5", "");
                    if (oImputacion.Valoracion6 != null)
                        keyValues.Add("T_Med6", oImputacion.Valoracion6.ToString());
                    else
                        keyValues.Add("T_Med6", "");
                    if (oImputacion.Valoracion7 != null)
                        keyValues.Add("T_Med7", oImputacion.Valoracion7.ToString());
                    else
                        keyValues.Add("T_Med7", "");
                    if (oImputacion.Valoracion8 != null)
                        keyValues.Add("T_Med8", oImputacion.Valoracion8.ToString());
                    else
                        keyValues.Add("T_Med8", "");
                    if (oImputacion.Valoracion9 != null)
                        keyValues.Add("T_Med9", oImputacion.Valoracion9.ToString());
                    else
                        keyValues.Add("T_Med9", "");
                    if (oImputacion.Valoracion10 != null)
                        keyValues.Add("T_Med10", oImputacion.Valoracion10.ToString());
                    else
                        keyValues.Add("T_Med10", "");
                    if (oImputacion.Valoracion11 != null)
                        keyValues.Add("T_Med11", oImputacion.Valoracion11.ToString());
                    else
                        keyValues.Add("T_Med11", "");
                    if (oImputacion.Valoracion12 != null)
                        keyValues.Add("T_Med12", oImputacion.Valoracion12.ToString());
                    else
                        keyValues.Add("T_Med12", "");
                    #endregion
                    #region valoresCalculados
                    if (oImputacion.ValorCalculado1 != null)
                        keyValues.Add("T_Calc1_", oImputacion.ValorCalculado1.ToString());
                    else
                        keyValues.Add("T_Calc1_", "");
                    if (oImputacion.ValorCalculado2 != null)
                        keyValues.Add("T_Calc2", oImputacion.ValorCalculado2.ToString());
                    else
                        keyValues.Add("T_Calc2", "");
                    if (oImputacion.ValorCalculado3 != null)
                        keyValues.Add("T_Calc3", oImputacion.ValorCalculado3.ToString());
                    else
                        keyValues.Add("T_Calc3", "");
                    if (oImputacion.ValorCalculado4 != null)
                        keyValues.Add("T_Calc4", oImputacion.ValorCalculado4.ToString());
                    else
                        keyValues.Add("T_Calc4", "");
                    if (oImputacion.ValorCalculado5 != null)
                        keyValues.Add("T_Calc5", oImputacion.ValorCalculado5.ToString());
                    else
                        keyValues.Add("T_Calc5", "");
                    if (oImputacion.ValorCalculado6 != null)
                        keyValues.Add("T_Calc6", oImputacion.ValorCalculado6.ToString());
                    else
                        keyValues.Add("T_Calc6", "");
                    if (oImputacion.ValorCalculado7 != null)
                        keyValues.Add("T_Calc7", oImputacion.ValorCalculado7.ToString());
                    else
                        keyValues.Add("T_Calc7", "");
                    if (oImputacion.ValorCalculado8 != null)
                        keyValues.Add("T_Calc8", oImputacion.ValorCalculado8.ToString());
                    else
                        keyValues.Add("T_Calc8", "");
                    if (oImputacion.ValorCalculado9 != null)
                        keyValues.Add("T_Calc9", oImputacion.ValorCalculado9.ToString());
                    else
                        keyValues.Add("T_Calc9", "");
                    if (oImputacion.ValorCalculado10 != null)
                        keyValues.Add("T_Calc10", oImputacion.ValorCalculado10.ToString());
                    else
                        keyValues.Add("T_Calc10", "");
                    if (oImputacion.ValorCalculado11 != null)
                        keyValues.Add("T_Calc11", oImputacion.ValorCalculado11.ToString());
                    else
                        keyValues.Add("T_Calc11", "");
                    if (oImputacion.ValorCalculado12 != null)
                        keyValues.Add("T_Calc12", oImputacion.ValorCalculado12.ToString());
                    else
                        keyValues.Add("T_Calc12", "");
                    #endregion
                    SearchAndReplace(destinationFile, keyValues);

                }
                WordprocessingDocument doc = WordprocessingDocument.Open(destinationFile, true);
                DocumentFormat.OpenXml.Wordprocessing.Table tabladespliegue = doc.MainDocumentPart.Document.Body.Descendants<DocumentFormat.OpenXml.Wordprocessing.Table>().ElementAt(1);

                if (oIndicador.Periodicidad == "Mensual" || oIndicador.Periodicidad == "Bimestral" || oIndicador.Periodicidad == "Trimestral" || oIndicador.Periodicidad == "Cuatrimestral" || oIndicador.Periodicidad == "Semestral")
                {

                    Stream stream = doc.MainDocumentPart.ChartParts.First().EmbeddedPackagePart.GetStream();
                    using (SpreadsheetDocument ssDoc = SpreadsheetDocument.Open(stream, true))
                    {
                        WorkbookPart wbPart = ssDoc.WorkbookPart;
                        Sheet theSheet = wbPart.Workbook.Descendants<Sheet>().
                          Where(s => s.Name == "Hoja1").FirstOrDefault();
                        if (theSheet != null)
                        {
                            Worksheet ws = ((WorksheetPart)(wbPart.GetPartById(theSheet.Id))).Worksheet;

                            if (oImputacion.Valoracion1 != null)
                            {
                                Cell theCell = InsertCellInWorksheet("B", 2, ws);
                                theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                                theCell.CellValue = new CellValue(((decimal)oImputacion.Valoracion1).ToString("F").Replace(",", "."));
                            }

                            if (oImputacion.Valoracion2 != null)
                            {
                                Cell theCell = InsertCellInWorksheet("B", 3, ws);
                                theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                                theCell.CellValue = new CellValue(((decimal)oImputacion.Valoracion2).ToString("F").Replace(",", "."));
                            }

                            if (oImputacion.Valoracion3 != null)
                            {
                                Cell theCell = InsertCellInWorksheet("B", 4, ws);
                                theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                                theCell.CellValue = new CellValue(((decimal)oImputacion.Valoracion3).ToString("F").Replace(",", "."));
                            }

                            if (oImputacion.Valoracion4 != null)
                            {
                                Cell theCell = InsertCellInWorksheet("B", 5, ws);
                                theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                                theCell.CellValue = new CellValue(((decimal)oImputacion.Valoracion4).ToString("F").Replace(",", "."));
                            }

                            if (oImputacion.Valoracion5 != null)
                            {
                                Cell theCell = InsertCellInWorksheet("B", 6, ws);
                                theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                                theCell.CellValue = new CellValue(((decimal)oImputacion.Valoracion5).ToString("F").Replace(",", "."));
                            }

                            if (oImputacion.Valoracion6 != null)
                            {
                                Cell theCell = InsertCellInWorksheet("B", 7, ws);
                                theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                                theCell.CellValue = new CellValue(((decimal)oImputacion.Valoracion6).ToString("F").Replace(",", "."));

                            }

                            if (oImputacion.Valoracion7 != null)
                            {
                                Cell theCell = InsertCellInWorksheet("B", 8, ws);
                                theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                                theCell.CellValue = new CellValue(((decimal)oImputacion.Valoracion7).ToString("F").Replace(",", "."));

                            }
                            if (oImputacion.Valoracion8 != null)
                            {
                                Cell theCell = InsertCellInWorksheet("B", 9, ws);
                                theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                                theCell.CellValue = new CellValue(((decimal)oImputacion.Valoracion8).ToString("F").Replace(",", "."));

                            }
                            if (oImputacion.Valoracion9 != null)
                            {
                                Cell theCell = InsertCellInWorksheet("B", 10, ws);
                                theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                                theCell.CellValue = new CellValue(((decimal)oImputacion.Valoracion9).ToString("F").Replace(",", "."));

                            }
                            if (oImputacion.Valoracion10 != null)
                            {
                                Cell theCell = InsertCellInWorksheet("B", 11, ws);
                                theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                                theCell.CellValue = new CellValue(((decimal)oImputacion.Valoracion10).ToString("F").Replace(",", "."));

                            }
                            if (oImputacion.Valoracion11 != null)
                            {
                                Cell theCell = InsertCellInWorksheet("B", 12, ws);
                                theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                                theCell.CellValue = new CellValue(((decimal)oImputacion.Valoracion11).ToString("F").Replace(",", "."));

                            }
                            if (oImputacion.Valoracion12 != null)
                            {
                                Cell theCell = InsertCellInWorksheet("B", 13, ws);
                                theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                                theCell.CellValue = new CellValue(((decimal)oImputacion.Valoracion12).ToString("F").Replace(",", "."));

                            }

                            #region celdasreferencia
                            if (oImputacion.ValorReferencia1 != null)
                            {
                                Cell theCell = InsertCellInWorksheet("C", 2, ws);
                                theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                                theCell.CellValue = new CellValue(((decimal)oImputacion.ValorReferencia1).ToString("F").Replace(",", "."));
                            }

                            if (oImputacion.ValorReferencia2 != null)
                            {
                                Cell theCell = InsertCellInWorksheet("C", 3, ws);
                                theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                                theCell.CellValue = new CellValue(((decimal)oImputacion.ValorReferencia2).ToString("F").Replace(",", "."));
                            }

                            if (oImputacion.ValorReferencia3 != null)
                            {
                                Cell theCell = InsertCellInWorksheet("C", 4, ws);
                                theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                                theCell.CellValue = new CellValue(((decimal)oImputacion.ValorReferencia3).ToString("F").Replace(",", "."));
                            }

                            if (oImputacion.ValorReferencia4 != null)
                            {
                                Cell theCell = InsertCellInWorksheet("C", 5, ws);
                                theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                                theCell.CellValue = new CellValue(((decimal)oImputacion.ValorReferencia4).ToString("F").Replace(",", "."));
                            }

                            if (oImputacion.ValorReferencia5 != null)
                            {
                                Cell theCell = InsertCellInWorksheet("C", 6, ws);
                                theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                                theCell.CellValue = new CellValue(((decimal)oImputacion.ValorReferencia5).ToString("F").Replace(",", "."));
                            }

                            if (oImputacion.ValorReferencia6 != null)
                            {
                                Cell theCell = InsertCellInWorksheet("C", 7, ws);
                                theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                                theCell.CellValue = new CellValue(((decimal)oImputacion.ValorReferencia6).ToString("F").Replace(",", "."));

                            }

                            if (oImputacion.ValorReferencia7 != null)
                            {
                                Cell theCell = InsertCellInWorksheet("C", 8, ws);
                                theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                                theCell.CellValue = new CellValue(((decimal)oImputacion.ValorReferencia7).ToString("F").Replace(",", "."));

                            }
                            if (oImputacion.ValorReferencia8 != null)
                            {
                                Cell theCell = InsertCellInWorksheet("C", 9, ws);
                                theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                                theCell.CellValue = new CellValue(((decimal)oImputacion.ValorReferencia8).ToString("F").Replace(",", "."));

                            }
                            if (oImputacion.ValorReferencia9 != null)
                            {
                                Cell theCell = InsertCellInWorksheet("C", 10, ws);
                                theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                                theCell.CellValue = new CellValue(((decimal)oImputacion.ValorReferencia9).ToString("F").Replace(",", "."));

                            }
                            if (oImputacion.ValorReferencia10 != null)
                            {
                                Cell theCell = InsertCellInWorksheet("C", 11, ws);
                                theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                                theCell.CellValue = new CellValue(((decimal)oImputacion.ValorReferencia10).ToString("F").Replace(",", "."));

                            }
                            if (oImputacion.ValorReferencia11 != null)
                            {
                                Cell theCell = InsertCellInWorksheet("C", 12, ws);
                                theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                                theCell.CellValue = new CellValue(((decimal)oImputacion.ValorReferencia11).ToString("F").Replace(",", "."));

                            }
                            if (oImputacion.ValorReferencia12 != null)
                            {
                                Cell theCell = InsertCellInWorksheet("C", 13, ws);
                                theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                                theCell.CellValue = new CellValue(((decimal)oImputacion.ValorReferencia12).ToString("F").Replace(",", "."));

                            }
                            #endregion

                            #region celdasoperacion
                            if (oImputacion.ValorCalculado1 != null)
                            {
                                Cell theCell = InsertCellInWorksheet("D", 2, ws);
                                theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                                theCell.CellValue = new CellValue(((decimal)oImputacion.ValorCalculado1).ToString("F").Replace(",", "."));
                            }

                            if (oImputacion.ValorCalculado2 != null)
                            {
                                Cell theCell = InsertCellInWorksheet("D", 3, ws);
                                theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                                theCell.CellValue = new CellValue(((decimal)oImputacion.ValorCalculado2).ToString("F").Replace(",", "."));
                            }

                            if (oImputacion.ValorCalculado3 != null)
                            {
                                Cell theCell = InsertCellInWorksheet("D", 4, ws);
                                theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                                theCell.CellValue = new CellValue(((decimal)oImputacion.ValorCalculado3).ToString("F").Replace(",", "."));
                            }

                            if (oImputacion.ValorCalculado4 != null)
                            {
                                Cell theCell = InsertCellInWorksheet("D", 5, ws);
                                theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                                theCell.CellValue = new CellValue(((decimal)oImputacion.ValorCalculado4).ToString("F").Replace(",", "."));
                            }

                            if (oImputacion.ValorCalculado5 != null)
                            {
                                Cell theCell = InsertCellInWorksheet("D", 6, ws);
                                theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                                theCell.CellValue = new CellValue(((decimal)oImputacion.ValorCalculado5).ToString("F").Replace(",", "."));
                            }

                            if (oImputacion.ValorCalculado6 != null)
                            {
                                Cell theCell = InsertCellInWorksheet("D", 7, ws);
                                theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                                theCell.CellValue = new CellValue(((decimal)oImputacion.ValorCalculado6).ToString("F").Replace(",", "."));

                            }

                            if (oImputacion.ValorCalculado7 != null)
                            {
                                Cell theCell = InsertCellInWorksheet("D", 8, ws);
                                theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                                theCell.CellValue = new CellValue(((decimal)oImputacion.ValorCalculado7).ToString("F").Replace(",", "."));

                            }
                            if (oImputacion.ValorCalculado8 != null)
                            {
                                Cell theCell = InsertCellInWorksheet("D", 9, ws);
                                theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                                theCell.CellValue = new CellValue(((decimal)oImputacion.ValorCalculado8).ToString("F").Replace(",", "."));

                            }
                            if (oImputacion.ValorCalculado9 != null)
                            {
                                Cell theCell = InsertCellInWorksheet("D", 10, ws);
                                theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                                theCell.CellValue = new CellValue(((decimal)oImputacion.ValorCalculado9).ToString("F").Replace(",", "."));

                            }
                            if (oImputacion.ValorCalculado10 != null)
                            {
                                Cell theCell = InsertCellInWorksheet("D", 11, ws);
                                theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                                theCell.CellValue = new CellValue(((decimal)oImputacion.ValorCalculado10).ToString("F").Replace(",", "."));

                            }
                            if (oImputacion.ValorCalculado11 != null)
                            {
                                Cell theCell = InsertCellInWorksheet("D", 12, ws);
                                theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                                theCell.CellValue = new CellValue(((decimal)oImputacion.ValorCalculado11).ToString("F").Replace(",", "."));

                            }
                            if (oImputacion.ValorCalculado12 != null)
                            {
                                Cell theCell = InsertCellInWorksheet("D", 13, ws);
                                theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                                theCell.CellValue = new CellValue(((decimal)oImputacion.ValorCalculado12).ToString("F").Replace(",", "."));

                            }
                            #endregion

                            ws.Save();

                            var cachedValues = doc.MainDocumentPart.ChartParts.ElementAt(0).ChartSpace.Descendants<DocumentFormat.OpenXml.Drawing.Charts.Values>();
                            int row = 0;
                            int col = 0;
                            foreach (var cachedValue in cachedValues)
                            { // By column B ; C ; D
                                row = 0;
                                if (col == 0)
                                {
                                    if (oImputacion.Valoracion1 != null)
                                        cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(0).Text = ((decimal)oImputacion.Valoracion1).ToString("F").Replace(",", ".");
                                    if (oImputacion.Valoracion2 != null)
                                        cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(1).Text = ((decimal)oImputacion.Valoracion2).ToString("F").Replace(",", ".");
                                    if (oImputacion.Valoracion3 != null)
                                        cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(2).Text = ((decimal)oImputacion.Valoracion3).ToString("F").Replace(",", ".");
                                    if (oImputacion.Valoracion4 != null)
                                        cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(3).Text = ((decimal)oImputacion.Valoracion4).ToString("F").Replace(",", ".");
                                    if (oImputacion.Valoracion5 != null)
                                        cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(4).Text = ((decimal)oImputacion.Valoracion5).ToString("F").Replace(",", ".");
                                    if (oImputacion.Valoracion6 != null)
                                        cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(5).Text = ((decimal)oImputacion.Valoracion6).ToString("F").Replace(",", ".");
                                    if (oImputacion.Valoracion7 != null)
                                        cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(6).Text = ((decimal)oImputacion.Valoracion7).ToString("F").Replace(",", ".");
                                    if (oImputacion.Valoracion8 != null)
                                        cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(7).Text = ((decimal)oImputacion.Valoracion8).ToString("F").Replace(",", ".");
                                    if (oImputacion.Valoracion9 != null)
                                        cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(8).Text = ((decimal)oImputacion.Valoracion9).ToString("F").Replace(",", ".");
                                    if (oImputacion.Valoracion10 != null)
                                        cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(9).Text = ((decimal)oImputacion.Valoracion10).ToString("F").Replace(",", ".");
                                    if (oImputacion.Valoracion11 != null)
                                        cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(10).Text = ((decimal)oImputacion.Valoracion11).ToString("F").Replace(",", ".");
                                    if (oImputacion.Valoracion12 != null)
                                        cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(11).Text = ((decimal)oImputacion.Valoracion12).ToString("F").Replace(",", ".");
                                }
                                if (col == 2)
                                {
                                    if (oImputacion.ValorReferencia1 != null)
                                        cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(0).Text = ((decimal)oImputacion.ValorReferencia1).ToString("F").Replace(",", ".");
                                    if (oImputacion.ValorReferencia2 != null)
                                        cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(1).Text = ((decimal)oImputacion.ValorReferencia2).ToString("F").Replace(",", ".");
                                    if (oImputacion.ValorReferencia3 != null)
                                        cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(2).Text = ((decimal)oImputacion.ValorReferencia3).ToString("F").Replace(",", ".");
                                    if (oImputacion.ValorReferencia4 != null)
                                        cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(3).Text = ((decimal)oImputacion.ValorReferencia4).ToString("F").Replace(",", ".");
                                    if (oImputacion.ValorReferencia5 != null)
                                        cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(4).Text = ((decimal)oImputacion.ValorReferencia5).ToString("F").Replace(",", ".");
                                    if (oImputacion.ValorReferencia6 != null)
                                        cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(5).Text = ((decimal)oImputacion.ValorReferencia6).ToString("F").Replace(",", ".");
                                    if (oImputacion.ValorReferencia7 != null)
                                        cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(6).Text = ((decimal)oImputacion.ValorReferencia7).ToString("F").Replace(",", ".");
                                    if (oImputacion.ValorReferencia8 != null)
                                        cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(7).Text = ((decimal)oImputacion.ValorReferencia8).ToString("F").Replace(",", ".");
                                    if (oImputacion.ValorReferencia9 != null)
                                        cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(8).Text = ((decimal)oImputacion.ValorReferencia9).ToString("F").Replace(",", ".");
                                    if (oImputacion.ValorReferencia10 != null)
                                        cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(9).Text = ((decimal)oImputacion.ValorReferencia10).ToString("F").Replace(",", ".");
                                    if (oImputacion.ValorReferencia11 != null)
                                        cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(10).Text = ((decimal)oImputacion.ValorReferencia11).ToString("F").Replace(",", ".");
                                    if (oImputacion.ValorReferencia12 != null)
                                        cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(11).Text = ((decimal)oImputacion.ValorReferencia12).ToString("F").Replace(",", ".");
                                }

                                if (col == 1)
                                {
                                    if (oImputacion.ValorCalculado1 != null)
                                        cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(0).Text = ((decimal)oImputacion.ValorCalculado1).ToString("F").Replace(",", ".");
                                    if (oImputacion.ValorCalculado2 != null)
                                        cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(1).Text = ((decimal)oImputacion.ValorCalculado2).ToString("F").Replace(",", ".");
                                    if (oImputacion.ValorCalculado3 != null)
                                        cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(2).Text = ((decimal)oImputacion.ValorCalculado3).ToString("F").Replace(",", ".");
                                    if (oImputacion.ValorCalculado4 != null)
                                        cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(3).Text = ((decimal)oImputacion.ValorCalculado4).ToString("F").Replace(",", ".");
                                    if (oImputacion.ValorCalculado5 != null)
                                        cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(4).Text = ((decimal)oImputacion.ValorCalculado5).ToString("F").Replace(",", ".");
                                    if (oImputacion.ValorCalculado6 != null)
                                        cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(5).Text = ((decimal)oImputacion.ValorCalculado6).ToString("F").Replace(",", ".");
                                    if (oImputacion.ValorCalculado7 != null)
                                        cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(6).Text = ((decimal)oImputacion.ValorCalculado7).ToString("F").Replace(",", ".");
                                    if (oImputacion.ValorCalculado8 != null)
                                        cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(7).Text = ((decimal)oImputacion.ValorCalculado8).ToString("F").Replace(",", ".");
                                    if (oImputacion.ValorCalculado9 != null)
                                        cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(8).Text = ((decimal)oImputacion.ValorCalculado9).ToString("F").Replace(",", ".");
                                    if (oImputacion.ValorCalculado10 != null)
                                        cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(9).Text = ((decimal)oImputacion.ValorCalculado10).ToString("F").Replace(",", ".");
                                    if (oImputacion.ValorCalculado11 != null)
                                        cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(10).Text = ((decimal)oImputacion.ValorCalculado11).ToString("F").Replace(",", ".");
                                    if (oImputacion.ValorCalculado12 != null)
                                        cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(11).Text = ((decimal)oImputacion.ValorCalculado12).ToString("F").Replace(",", ".");
                                }
                                col++;
                            }
                        }
                    }
                }
                else
                {
                    indicadores_imputacion impanio5 = Datos.ObtenerValoracionInd(id, anio - 4, idCentral);
                    indicadores_imputacion impanio4 = Datos.ObtenerValoracionInd(id, anio - 3, idCentral);
                    indicadores_imputacion impanio3 = Datos.ObtenerValoracionInd(id, anio - 2, idCentral);
                    indicadores_imputacion impanio2 = Datos.ObtenerValoracionInd(id, anio - 1, idCentral);

                    Stream stream = doc.MainDocumentPart.ChartParts.First().EmbeddedPackagePart.GetStream();
                    using (SpreadsheetDocument ssDoc = SpreadsheetDocument.Open(stream, true))
                    {
                        WorkbookPart wbPart = ssDoc.WorkbookPart;
                        Sheet theSheet = wbPart.Workbook.Descendants<Sheet>().
                          Where(s => s.Name == "Hoja1").FirstOrDefault();
                        if (theSheet != null)
                        {
                            Worksheet ws = ((WorksheetPart)(wbPart.GetPartById(theSheet.Id))).Worksheet;

                            Cell theCellAnio5 = InsertCellInWorksheet("A", 2, ws);
                            theCellAnio5.DataType = new EnumValue<CellValues>(CellValues.String);
                            theCellAnio5.CellValue = new CellValue((anio - 4).ToString());
                            Cell theCellAnio4 = InsertCellInWorksheet("A", 3, ws);
                            theCellAnio4.DataType = new EnumValue<CellValues>(CellValues.String);
                            theCellAnio4.CellValue = new CellValue((anio - 3).ToString());
                            Cell theCellAnio3 = InsertCellInWorksheet("A", 4, ws);
                            theCellAnio3.DataType = new EnumValue<CellValues>(CellValues.String);
                            theCellAnio3.CellValue = new CellValue((anio - 2).ToString());
                            Cell theCellAnio2 = InsertCellInWorksheet("A", 5, ws);
                            theCellAnio2.DataType = new EnumValue<CellValues>(CellValues.String);
                            theCellAnio2.CellValue = new CellValue((anio - 1).ToString());
                            Cell theCellAnio1 = InsertCellInWorksheet("A", 6, ws);
                            theCellAnio1.DataType = new EnumValue<CellValues>(CellValues.String);
                            theCellAnio1.CellValue = new CellValue((anio).ToString());

                            if (impanio5 != null)
                            {
                                if (impanio5.Valoracion12 != null)
                                {
                                    Cell theCell = InsertCellInWorksheet("B", 2, ws);
                                    theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                                    theCell.CellValue = new CellValue(((decimal)impanio5.Valoracion12).ToString("F").Replace(",", "."));
                                }
                                if (impanio5.ValorReferencia12 != null)
                                {
                                    Cell theCell = InsertCellInWorksheet("C", 2, ws);
                                    theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                                    theCell.CellValue = new CellValue(((decimal)impanio5.ValorReferencia12).ToString("F").Replace(",", "."));
                                }
                                if (impanio5.ValorCalculado12 != null)
                                {
                                    Cell theCell = InsertCellInWorksheet("D", 2, ws);
                                    theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                                    theCell.CellValue = new CellValue(((decimal)impanio5.ValorCalculado12).ToString("F").Replace(",", "."));
                                }
                            }

                            if (impanio4 != null)
                            {
                                if (impanio4.Valoracion12 != null)
                                {
                                    Cell theCell = InsertCellInWorksheet("B", 3, ws);
                                    theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                                    theCell.CellValue = new CellValue(((decimal)impanio4.Valoracion12).ToString("F").Replace(",", "."));
                                }
                                if (impanio4.ValorReferencia12 != null)
                                {
                                    Cell theCell = InsertCellInWorksheet("C", 3, ws);
                                    theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                                    theCell.CellValue = new CellValue(((decimal)impanio4.ValorReferencia12).ToString("F").Replace(",", "."));
                                }
                                if (impanio4.ValorCalculado12 != null)
                                {
                                    Cell theCell = InsertCellInWorksheet("D", 3, ws);
                                    theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                                    theCell.CellValue = new CellValue(((decimal)impanio4.ValorCalculado12).ToString("F").Replace(",", "."));
                                }
                            }

                            if (impanio3 != null)
                            {
                                if (impanio3.Valoracion12 != null)
                                {
                                    Cell theCell = InsertCellInWorksheet("B", 4, ws);
                                    theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                                    theCell.CellValue = new CellValue(((decimal)impanio3.Valoracion12).ToString("F").Replace(",", "."));
                                }
                                if (impanio3.ValorReferencia12 != null)
                                {
                                    Cell theCell = InsertCellInWorksheet("C", 4, ws);
                                    theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                                    theCell.CellValue = new CellValue(((decimal)impanio3.ValorReferencia12).ToString("F").Replace(",", "."));
                                }
                                if (impanio3.ValorCalculado12 != null)
                                {
                                    Cell theCell = InsertCellInWorksheet("D", 4, ws);
                                    theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                                    theCell.CellValue = new CellValue(((decimal)impanio3.ValorCalculado12).ToString("F").Replace(",", "."));
                                }
                            }

                            if (impanio2 != null)
                            {
                                if (impanio2.Valoracion12 != null)
                                {
                                    Cell theCell = InsertCellInWorksheet("B", 5, ws);
                                    theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                                    theCell.CellValue = new CellValue(((decimal)impanio2.Valoracion12).ToString("F").Replace(",", "."));
                                }
                                if (impanio2.ValorReferencia12 != null)
                                {
                                    Cell theCell = InsertCellInWorksheet("C", 5, ws);
                                    theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                                    theCell.CellValue = new CellValue(((decimal)impanio2.ValorReferencia12).ToString("F").Replace(",", "."));
                                }
                                if (impanio2.ValorCalculado12 != null)
                                {
                                    Cell theCell = InsertCellInWorksheet("D", 5, ws);
                                    theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                                    theCell.CellValue = new CellValue(((decimal)impanio2.ValorCalculado12).ToString("F").Replace(",", "."));
                                }
                            }

                            if (oImputacion != null)
                            {
                                if (oImputacion.Valoracion12 != null)
                                {
                                    Cell theCell = InsertCellInWorksheet("B", 6, ws);
                                    theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                                    theCell.CellValue = new CellValue(((decimal)oImputacion.Valoracion12).ToString("F").Replace(",", "."));
                                }
                                if (oImputacion.ValorReferencia12 != null)
                                {
                                    Cell theCell = InsertCellInWorksheet("C", 6, ws);
                                    theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                                    theCell.CellValue = new CellValue(((decimal)oImputacion.ValorReferencia12).ToString("F").Replace(",", "."));
                                }
                                if (oImputacion.ValorCalculado12 != null)
                                {
                                    Cell theCell = InsertCellInWorksheet("D", 6, ws);
                                    theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                                    theCell.CellValue = new CellValue(((decimal)oImputacion.ValorCalculado12).ToString("F").Replace(",", "."));
                                }
                            }

                            ws.Save();

                            var cachedValues = doc.MainDocumentPart.ChartParts.ElementAt(0).ChartSpace.Descendants<DocumentFormat.OpenXml.Drawing.Charts.Values>();
                            int row = 0;
                            int col = 0;
                            foreach (var cachedValue in cachedValues)
                            { // By column B ; C ; D
                                row = 0;
                                if (col == 0)
                                {
                                    if (impanio5 != null && impanio5.Valoracion12 != null)
                                        cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(0).Text = ((decimal)impanio5.Valoracion12).ToString("F").Replace(",", ".");
                                    if (impanio4 != null && impanio4.Valoracion12 != null)
                                        cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(1).Text = ((decimal)impanio4.Valoracion12).ToString("F").Replace(",", ".");
                                    if (impanio3 != null && impanio3.Valoracion12 != null)
                                        cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(2).Text = ((decimal)impanio3.Valoracion12).ToString("F").Replace(",", ".");
                                    if (impanio2 != null && impanio2.Valoracion12 != null)
                                        cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(3).Text = ((decimal)impanio2.Valoracion12).ToString("F").Replace(",", ".");
                                    if (oImputacion != null && oImputacion.Valoracion12 != null)
                                        cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(4).Text = ((decimal)oImputacion.Valoracion12).ToString("F").Replace(",", ".");
                                }
                                if (col == 2)
                                {
                                    if (impanio5 != null && impanio5.ValorReferencia12 != null)
                                        cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(0).Text = ((decimal)impanio5.ValorReferencia12).ToString("F").Replace(",", ".");
                                    if (impanio4 != null && impanio4.ValorReferencia12 != null)
                                        cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(1).Text = ((decimal)impanio4.ValorReferencia12).ToString("F").Replace(",", ".");
                                    if (impanio3 != null && impanio3.ValorReferencia12 != null)
                                        cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(2).Text = ((decimal)impanio3.ValorReferencia12).ToString("F").Replace(",", ".");
                                    if (impanio2 != null && impanio2.ValorReferencia12 != null)
                                        cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(3).Text = ((decimal)impanio2.ValorReferencia12).ToString("F").Replace(",", ".");
                                    if (oImputacion != null && oImputacion.ValorReferencia12 != null)
                                        cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(4).Text = ((decimal)oImputacion.ValorReferencia12).ToString("F").Replace(",", ".");
                                }

                                if (col == 1)
                                {
                                    if (impanio5 != null && impanio5.ValorCalculado12 != null)
                                        cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(0).Text = ((decimal)impanio5.ValorCalculado12).ToString("F").Replace(",", ".");
                                    if (impanio4 != null && impanio4.ValorCalculado12 != null)
                                        cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(1).Text = ((decimal)impanio4.ValorCalculado12).ToString("F").Replace(",", ".");
                                    if (impanio3 != null && impanio3.ValorCalculado12 != null)
                                        cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(2).Text = ((decimal)impanio3.ValorCalculado12).ToString("F").Replace(",", ".");
                                    if (impanio2 != null && impanio2.ValorCalculado12 != null)
                                        cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(3).Text = ((decimal)impanio2.ValorCalculado12).ToString("F").Replace(",", ".");
                                    if (oImputacion != null && oImputacion.ValorCalculado12 != null)
                                        cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(4).Text = ((decimal)oImputacion.ValorCalculado12).ToString("F").Replace(",", ".");
                                }
                                col++;
                            }
                        }
                    }
                }


                doc.MainDocumentPart.ChartParts.ElementAt(0).ChartSpace.Save();

                doc.MainDocumentPart.Document.Save();

                doc.Close();

                #endregion

                Session["nombreArchivo"] = destinationFile;

                Session["anioImputacion"] = anio.ToString();
                ViewData["procesos"] = Datos.ListarProcesos();
                ViewData["tecnologias"] = Datos.ListarTecnologias();
                VISTA_IndicadoresPlanificados buscarIndicador = Datos.ObtenerIndicadorPlanificado(id);
                ViewData["EditarIndicador"] = buscarIndicador;
                indicadores_imputacion buscarImputacion = Datos.ObtenerImputacionIndicador(id, anio);
                ViewData["EditarImputacion"] = buscarImputacion;
                return Redirect(Url.RouteUrl(new { controller = "Indicadores", action = "detalle_indicador", id = id }));
                #endregion
            }
            else
            {
                #region recarga

                indicadores_imputacion existeImputacion = Datos.ObtenerImputacionIndicador(id, anio);
                if (existeImputacion == null)
                    Datos.CrearPlanificacionIndicador(id, id, anio);

                Session["anioImputacion"] = anio.ToString();
                ViewData["procesos"] = Datos.ListarProcesos();
                ViewData["tecnologias"] = Datos.ListarTecnologias();
                indicadores buscarIndicador = Datos.ObtenerIndicador(id);
                ViewData["EditarIndicador"] = buscarIndicador;
                indicadores_imputacion buscarImputacion = Datos.ObtenerValoracionInd(id, anio, idCentral);

                if (buscarImputacion == null)
                {
                    Datos.CrearValoracionIndicador(id, anio, idCentral);

                    buscarImputacion = Datos.ObtenerValoracionInd(id, anio, idCentral);
                }

                ViewData["EditarImputacion"] = buscarImputacion;
                return View();
                #endregion
            }
        }

        public ActionResult eliminar_planindicador(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            Datos.EliminarIndicadorPlanificado(id);
            Session["EditarIndicadoresResultado"] = "Eliminado registro";
            return RedirectToAction("gestion_indicadores", "indicadores");
        }

        public ActionResult parametros()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }
            int centralElegida = int.Parse(Session["CentralElegida"].ToString());

            centros central = Datos.ObtenerCentroPorID(centralElegida);

            List<VISTA_ParametrosInd> parametros = Datos.ListarParametrosInd(central.id);
            ViewData["parametros"] = parametros;

            return View();
        }

        public ActionResult editar_parametro(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            int idCentral = int.Parse(Session["CentralElegida"].ToString());

            indicadores_hojadedatos_valores indHojaDatos = Datos.ObtenerParametroInd(id, DateTime.Now.Year, idCentral);

            if (indHojaDatos != null)
            {
                ViewData["idParametro"] = indHojaDatos.Id;

            }
            else
            {
                Datos.CrearParametroIndicador(id, DateTime.Now.Year, idCentral);

                indHojaDatos = Datos.ObtenerParametroInd(id, DateTime.Now.Year, idCentral);
            }

            ViewData["editarparametro"] = indHojaDatos;
            if (indHojaDatos != null)
                ViewData["consultaparametro"] = Datos.ObtenerParametroInd(indHojaDatos.CodIndiHojaDatos);


            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult editar_parametro(int id, FormCollection collection)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            string formulario = collection["hdFormularioEjecutado"];

            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            int idCentral = centroseleccionado.id;

            if (formulario == "GuardarParametro")
            {
                #region Guardar Indicador
                try
                {
                    int anio = int.Parse(collection["ctl00$MainContent$ddlAnio"].ToString());
                    indicadores_hojadedatos_valores actualizar = Datos.ObtenerParametroInd(id, anio, idCentral);

                    #region mediciones
                    if (collection["ctl00$MainContent$txtMed1"] != null && collection["ctl00$MainContent$txtMed1"] != string.Empty)
                        actualizar.valor1 = decimal.Parse(collection["ctl00$MainContent$txtMed1"].ToString().Replace(".",","));
                    else
                        actualizar.valor1 = null;
                    if (collection["ctl00$MainContent$txtMed2"] != null && collection["ctl00$MainContent$txtMed2"] != string.Empty)
                        actualizar.valor2 = decimal.Parse(collection["ctl00$MainContent$txtMed2"].ToString().Replace(".", ","));
                    else
                        actualizar.valor2 = null;
                    if (collection["ctl00$MainContent$txtMed3"] != null && collection["ctl00$MainContent$txtMed3"] != string.Empty)
                        actualizar.valor3 = decimal.Parse(collection["ctl00$MainContent$txtMed3"].ToString().Replace(".", ","));
                    else
                        actualizar.valor3 = null;
                    if (collection["ctl00$MainContent$txtMed4"] != null && collection["ctl00$MainContent$txtMed4"] != string.Empty)
                        actualizar.valor4 = decimal.Parse(collection["ctl00$MainContent$txtMed4"].ToString());
                    else
                        actualizar.valor4 = null;
                    if (collection["ctl00$MainContent$txtMed5"] != null && collection["ctl00$MainContent$txtMed5"] != string.Empty)
                        actualizar.valor5 = decimal.Parse(collection["ctl00$MainContent$txtMed5"].ToString().Replace(".", ","));
                    else
                        actualizar.valor5 = null;
                    if (collection["ctl00$MainContent$txtMed6"] != null && collection["ctl00$MainContent$txtMed6"] != string.Empty)
                        actualizar.valor6 = decimal.Parse(collection["ctl00$MainContent$txtMed6"].ToString().Replace(".", ","));
                    else
                        actualizar.valor6 = null;
                    if (collection["ctl00$MainContent$txtMed7"] != null && collection["ctl00$MainContent$txtMed7"] != string.Empty)
                        actualizar.valor7 = decimal.Parse(collection["ctl00$MainContent$txtMed7"].ToString().Replace(".", ","));
                    else
                        actualizar.valor7 = null;
                    if (collection["ctl00$MainContent$txtMed8"] != null && collection["ctl00$MainContent$txtMed8"] != string.Empty)
                        actualizar.valor8 = decimal.Parse(collection["ctl00$MainContent$txtMed8"].ToString().Replace(".", ","));
                    else
                        actualizar.valor8 = null;
                    if (collection["ctl00$MainContent$txtMed9"] != null && collection["ctl00$MainContent$txtMed9"] != string.Empty)
                        actualizar.valor9 = decimal.Parse(collection["ctl00$MainContent$txtMed9"].ToString().Replace(".", ","));
                    else
                        actualizar.valor9 = null;
                    if (collection["ctl00$MainContent$txtMed10"] != null && collection["ctl00$MainContent$txtMed10"] != string.Empty)
                        actualizar.valor10 = decimal.Parse(collection["ctl00$MainContent$txtMed10"].ToString().Replace(".", ","));
                    else
                        actualizar.valor10 = null;
                    if (collection["ctl00$MainContent$txtMed11"] != null && collection["ctl00$MainContent$txtMed11"] != string.Empty)
                        actualizar.valor11 = decimal.Parse(collection["ctl00$MainContent$txtMed11"].ToString().Replace(".", ","));
                    else
                        actualizar.valor11 = null;
                    if (collection["ctl00$MainContent$txtMed12"] != null && collection["ctl00$MainContent$txtMed12"] != string.Empty)
                        actualizar.valor12 = decimal.Parse(collection["ctl00$MainContent$txtMed12"].ToString().Replace(".", ","));
                    else
                        actualizar.valor12 = null;
                    #endregion

                    Datos.ActualizarParametroInd(actualizar);

                    #region recalcular parametro en indicadores

                    List<VISTA_IndicadoresAfectadosParametro> listaIndicadores = Datos.ObtenerIndicadoresAfectados(id, anio, idCentral);

                    foreach (VISTA_IndicadoresAfectadosParametro oIndicador in listaIndicadores)
                    {
                        #region calculos

                        MIDAS.Models.indicadores_hojadedatos_valores operador1 = new MIDAS.Models.indicadores_hojadedatos_valores();
                        MIDAS.Models.indicadores_hojadedatos_valores operador2 = new MIDAS.Models.indicadores_hojadedatos_valores();
                        MIDAS.Models.indicadores_hojadedatos_valores operador3 = new MIDAS.Models.indicadores_hojadedatos_valores();
                        if (oIndicador.Operador1 != null)
                        {
                            operador1 = MIDAS.Models.Datos.ObtenerParametroInd(int.Parse(oIndicador.Operador1.ToString()), anio, centroseleccionado.id);
                        }
                        if (oIndicador.Operador2 != null)
                        {
                            operador2 = MIDAS.Models.Datos.ObtenerParametroInd(int.Parse(oIndicador.Operador2.ToString()), anio, centroseleccionado.id);
                        }
                        if (oIndicador.Operador3 != null)
                        {
                            operador3 = MIDAS.Models.Datos.ObtenerParametroInd(int.Parse(oIndicador.Operador3.ToString()), anio, centroseleccionado.id);
                        }

                        if (oIndicador.Operacion1 != null && operador1 != null && operador2 != null)
                        {
                            decimal resultado = 0;
                            switch (oIndicador.Operacion1.Trim())
                            {
                                case ("+"):
                                    if (operador1.valor1 != null && operador2.valor1 != null)
                                        resultado = decimal.Parse(operador1.valor1.ToString()) + decimal.Parse(operador2.valor1.ToString());
                                    break;
                                case ("-"):
                                    if (operador1.valor1 != null && operador2.valor1 != null)
                                        resultado = decimal.Parse(operador1.valor1.ToString()) - decimal.Parse(operador2.valor1.ToString());
                                    break;
                                case ("x"):
                                    if (operador1.valor1 != null && operador2.valor1 != null)
                                        resultado = decimal.Parse(operador1.valor1.ToString()) * decimal.Parse(operador2.valor1.ToString());
                                    break;
                                case ("/"):
                                    if (operador1.valor1 != null && operador2.valor1 != null && operador2.valor1 != 0)
                                        resultado = decimal.Parse(operador1.valor1.ToString()) / decimal.Parse(operador2.valor1.ToString());
                                    break;
                            }
                            if (oIndicador.Periodicidad == "Mensual")
                                oIndicador.Valoracion1 = resultado;

                            if (oIndicador.Operacion2 != null && operador3 != null)
                            {
                                switch (oIndicador.Operacion2.Trim())
                                {
                                    case ("+"):
                                        if (operador3.valor1 != null)
                                            resultado = resultado + decimal.Parse(operador3.valor1.ToString());
                                        break;
                                    case ("-"):
                                        if (operador3.valor1 != null)
                                            resultado = resultado - decimal.Parse(operador3.valor1.ToString());
                                        break;
                                    case ("x"):
                                        if (operador3.valor1 != null)
                                            resultado = resultado * decimal.Parse(operador3.valor1.ToString());
                                        break;
                                    case ("/"):
                                        if (operador3.valor1 != null && operador3.valor1 != 0)
                                            resultado = resultado / decimal.Parse(operador3.valor1.ToString());
                                        break;
                                }
                                if (oIndicador.Periodicidad == "Mensual")
                                    oIndicador.Valoracion1 = resultado;

                            }


                            resultado = 0;
                            switch (oIndicador.Operacion1.Trim())
                            {
                                case ("+"):
                                    if (operador1.valor2 != null && operador2.valor2 != null)
                                        resultado = decimal.Parse(operador1.valor2.ToString()) + decimal.Parse(operador2.valor2.ToString());
                                    break;
                                case ("-"):
                                    if (operador1.valor2 != null && operador2.valor2 != null)
                                        resultado = decimal.Parse(operador1.valor2.ToString()) - decimal.Parse(operador2.valor2.ToString());
                                    break;
                                case ("x"):
                                    if (operador1.valor2 != null && operador2.valor2 != null)
                                        resultado = decimal.Parse(operador1.valor2.ToString()) * decimal.Parse(operador2.valor2.ToString());
                                    break;
                                case ("/"):
                                    if (operador1.valor2 != null && operador2.valor2 != null && operador2.valor2 != 0)
                                        resultado = decimal.Parse(operador1.valor2.ToString()) / decimal.Parse(operador2.valor2.ToString());
                                    break;
                            }
                            if (oIndicador.Periodicidad == "Mensual" || oIndicador.Periodicidad == "Bimestral")
                                oIndicador.Valoracion2 = resultado;

                            if (oIndicador.Operacion2 != null && operador3 != null)
                            {
                                switch (oIndicador.Operacion2.Trim())
                                {
                                    case ("+"):
                                        if (operador3.valor2 != null)
                                            resultado = resultado + decimal.Parse(operador3.valor2.ToString());
                                        break;
                                    case ("-"):
                                        if (operador3.valor2 != null)
                                            resultado = resultado - decimal.Parse(operador3.valor2.ToString());
                                        break;
                                    case ("x"):
                                        if (operador3.valor2 != null)
                                            resultado = resultado * decimal.Parse(operador3.valor2.ToString());
                                        break;
                                    case ("/"):
                                        if (operador3.valor2 != null && operador3.valor2 != 0)
                                            resultado = resultado / decimal.Parse(operador3.valor2.ToString());
                                        break;
                                }
                                if (oIndicador.Periodicidad == "Mensual" || oIndicador.Periodicidad == "Bimestral")
                                    oIndicador.Valoracion2 = resultado;

                            }


                            resultado = 0;
                            switch (oIndicador.Operacion1.Trim())
                            {
                                case ("+"):
                                    if (operador1.valor3 != null && operador2.valor3 != null)
                                        resultado = decimal.Parse(operador1.valor3.ToString()) + decimal.Parse(operador2.valor3.ToString());
                                    break;
                                case ("-"):
                                    if (operador1.valor3 != null && operador2.valor3 != null)
                                        resultado = decimal.Parse(operador1.valor3.ToString()) - decimal.Parse(operador2.valor3.ToString());
                                    break;
                                case ("x"):
                                    if (operador1.valor3 != null && operador2.valor3 != null)
                                        resultado = decimal.Parse(operador1.valor3.ToString()) * decimal.Parse(operador2.valor3.ToString());
                                    break;
                                case ("/"):
                                    if (operador1.valor3 != null && operador2.valor3 != null && operador2.valor3 != 0)
                                        resultado = decimal.Parse(operador1.valor3.ToString()) / decimal.Parse(operador2.valor3.ToString());
                                    break;
                            }
                            if (oIndicador.Periodicidad == "Mensual" || oIndicador.Periodicidad == "Trimestral")
                                oIndicador.Valoracion3 = resultado;
                            if (oIndicador.Operacion2 != null && operador3 != null)
                            {
                                switch (oIndicador.Operacion2.Trim())
                                {
                                    case ("+"):
                                        if (operador3.valor3 != null)
                                            resultado = resultado + decimal.Parse(operador3.valor3.ToString());
                                        break;
                                    case ("-"):
                                        if (operador3.valor3 != null)
                                            resultado = resultado - decimal.Parse(operador3.valor3.ToString());
                                        break;
                                    case ("x"):
                                        if (operador3.valor3 != null)
                                            resultado = resultado * decimal.Parse(operador3.valor3.ToString());
                                        break;
                                    case ("/"):
                                        if (operador3.valor3 != null && operador3.valor3 != 0)
                                            resultado = resultado / decimal.Parse(operador3.valor3.ToString());
                                        break;
                                }
                                if (oIndicador.Periodicidad == "Mensual" || oIndicador.Periodicidad == "Trimestral")
                                    oIndicador.Valoracion3 = resultado;

                            }


                            resultado = 0;
                            switch (oIndicador.Operacion1.Trim())
                            {
                                case ("+"):
                                    if (operador1.valor4 != null && operador2.valor4 != null)
                                        resultado = decimal.Parse(operador1.valor4.ToString()) + decimal.Parse(operador2.valor4.ToString());
                                    break;
                                case ("-"):
                                    if (operador1.valor4 != null && operador2.valor4 != null)
                                        resultado = decimal.Parse(operador1.valor4.ToString()) - decimal.Parse(operador2.valor4.ToString());
                                    break;
                                case ("x"):
                                    if (operador1.valor4 != null && operador2.valor4 != null)
                                        resultado = decimal.Parse(operador1.valor4.ToString()) * decimal.Parse(operador2.valor4.ToString());
                                    break;
                                case ("/"):
                                    if (operador1.valor4 != null && operador2.valor4 != null && operador2.valor4 != 0)
                                        resultado = decimal.Parse(operador1.valor4.ToString()) / decimal.Parse(operador2.valor4.ToString());
                                    break;
                            }
                            if (oIndicador.Periodicidad == "Mensual" || oIndicador.Periodicidad == "Bimestral")
                                oIndicador.Valoracion4 = resultado;

                            if (oIndicador.Operacion2 != null && operador3 != null)
                            {
                                switch (oIndicador.Operacion2.Trim())
                                {
                                    case ("+"):
                                        if (operador3.valor4 != null)
                                            resultado = resultado + decimal.Parse(operador3.valor4.ToString());
                                        break;
                                    case ("-"):
                                        if (operador3.valor4 != null)
                                            resultado = resultado - decimal.Parse(operador3.valor4.ToString());
                                        break;
                                    case ("x"):
                                        if (operador3.valor4 != null)
                                            resultado = resultado * decimal.Parse(operador3.valor4.ToString());
                                        break;
                                    case ("/"):
                                        if (operador3.valor4 != null && operador3.valor4 != 0)
                                            resultado = resultado / decimal.Parse(operador3.valor4.ToString());
                                        break;
                                }
                                if (oIndicador.Periodicidad == "Mensual" || oIndicador.Periodicidad == "Bimestral")
                                    oIndicador.Valoracion4 = resultado;

                            }


                            resultado = 0;
                            switch (oIndicador.Operacion1.Trim())
                            {
                                case ("+"):
                                    if (operador1.valor5 != null && operador2.valor5 != null)
                                        resultado = decimal.Parse(operador1.valor5.ToString()) + decimal.Parse(operador2.valor5.ToString());
                                    break;
                                case ("-"):
                                    if (operador1.valor5 != null && operador2.valor5 != null)
                                        resultado = decimal.Parse(operador1.valor5.ToString()) - decimal.Parse(operador2.valor5.ToString());
                                    break;
                                case ("x"):
                                    if (operador1.valor5 != null && operador2.valor5 != null)
                                        resultado = decimal.Parse(operador1.valor5.ToString()) * decimal.Parse(operador2.valor5.ToString());
                                    break;
                                case ("/"):
                                    if (operador1.valor5 != null && operador2.valor5 != null && operador2.valor5 != 0)
                                        resultado = decimal.Parse(operador1.valor5.ToString()) / decimal.Parse(operador2.valor5.ToString());
                                    break;
                            }
                            if (oIndicador.Periodicidad == "Mensual")
                                oIndicador.Valoracion5 = resultado;

                            if (oIndicador.Operacion2 != null && operador3 != null)
                            {
                                switch (oIndicador.Operacion2.Trim())
                                {
                                    case ("+"):
                                        if (operador3.valor5 != null)
                                            resultado = resultado + decimal.Parse(operador3.valor5.ToString());
                                        break;
                                    case ("-"):
                                        if (operador3.valor5 != null)
                                            resultado = resultado - decimal.Parse(operador3.valor5.ToString());
                                        break;
                                    case ("x"):
                                        if (operador3.valor5 != null)
                                            resultado = resultado * decimal.Parse(operador3.valor5.ToString());
                                        break;
                                    case ("/"):
                                        if (operador3.valor5 != null && operador3.valor5 != 0)
                                            resultado = resultado / decimal.Parse(operador3.valor5.ToString());
                                        break;
                                }
                                if (oIndicador.Periodicidad == "Mensual")
                                    oIndicador.Valoracion5 = resultado;

                            }



                            resultado = 0;
                            switch (oIndicador.Operacion1.Trim())
                            {
                                case ("+"):
                                    if (operador1.valor6 != null && operador2.valor6 != null)
                                        resultado = decimal.Parse(operador1.valor6.ToString()) + decimal.Parse(operador2.valor6.ToString());
                                    break;
                                case ("-"):
                                    if (operador1.valor6 != null && operador2.valor6 != null)
                                        resultado = decimal.Parse(operador1.valor6.ToString()) - decimal.Parse(operador2.valor6.ToString());
                                    break;
                                case ("x"):
                                    if (operador1.valor6 != null && operador2.valor6 != null)
                                        resultado = decimal.Parse(operador1.valor6.ToString()) * decimal.Parse(operador2.valor6.ToString());
                                    break;
                                case ("/"):
                                    if (operador1.valor6 != null && operador2.valor6 != null && operador2.valor6 != 0)
                                        resultado = decimal.Parse(operador1.valor6.ToString()) / decimal.Parse(operador2.valor6.ToString());
                                    break;
                            }
                            if (oIndicador.Periodicidad == "Mensual" || oIndicador.Periodicidad == "Bimestral" || oIndicador.Periodicidad == "Trimestral" || oIndicador.Periodicidad == "Semestral")
                                oIndicador.Valoracion6 = resultado;
                            if (oIndicador.Operacion2 != null && operador3 != null)
                            {
                                switch (oIndicador.Operacion2.Trim())
                                {
                                    case ("+"):
                                        if (operador3.valor6 != null)
                                            resultado = resultado + decimal.Parse(operador3.valor6.ToString());
                                        break;
                                    case ("-"):
                                        if (operador3.valor6 != null)
                                            resultado = resultado - decimal.Parse(operador3.valor6.ToString());
                                        break;
                                    case ("x"):
                                        if (operador3.valor6 != null)
                                            resultado = resultado * decimal.Parse(operador3.valor6.ToString());
                                        break;
                                    case ("/"):
                                        if (operador3.valor6 != null && operador3.valor6 != 0)
                                            resultado = resultado / decimal.Parse(operador3.valor6.ToString());
                                        break;
                                }
                                if (oIndicador.Periodicidad == "Mensual" || oIndicador.Periodicidad == "Bimestral" || oIndicador.Periodicidad == "Trimestral" || oIndicador.Periodicidad == "Semestral")
                                    oIndicador.Valoracion6 = resultado;

                            }


                            resultado = 0;
                            switch (oIndicador.Operacion1.Trim())
                            {
                                case ("+"):
                                    if (operador1.valor7 != null && operador2.valor7 != null)
                                        resultado = decimal.Parse(operador1.valor7.ToString()) + decimal.Parse(operador2.valor7.ToString());
                                    break;
                                case ("-"):
                                    if (operador1.valor7 != null && operador2.valor7 != null)
                                        resultado = decimal.Parse(operador1.valor7.ToString()) - decimal.Parse(operador2.valor7.ToString());
                                    break;
                                case ("x"):
                                    if (operador1.valor7 != null && operador2.valor7 != null)
                                        resultado = decimal.Parse(operador1.valor7.ToString()) * decimal.Parse(operador2.valor7.ToString());
                                    break;
                                case ("/"):
                                    if (operador1.valor7 != null && operador2.valor7 != null && operador2.valor7 != 0)
                                        resultado = decimal.Parse(operador1.valor7.ToString()) / decimal.Parse(operador2.valor7.ToString());
                                    break;
                            }
                            if (oIndicador.Periodicidad == "Mensual")
                                oIndicador.Valoracion7 = resultado;

                            if (oIndicador.Operacion2 != null && operador3 != null)
                            {
                                switch (oIndicador.Operacion2.Trim())
                                {
                                    case ("+"):
                                        if (operador3.valor7 != null)
                                            resultado = resultado + decimal.Parse(operador3.valor7.ToString());
                                        break;
                                    case ("-"):
                                        if (operador3.valor7 != null)
                                            resultado = resultado - decimal.Parse(operador3.valor7.ToString());
                                        break;
                                    case ("x"):
                                        if (operador3.valor7 != null)
                                            resultado = resultado * decimal.Parse(operador3.valor7.ToString());
                                        break;
                                    case ("/"):
                                        if (operador3.valor7 != null && operador3.valor7 != 0)
                                            resultado = resultado / decimal.Parse(operador3.valor7.ToString());
                                        break;
                                }
                                if (oIndicador.Periodicidad == "Mensual")
                                    oIndicador.Valoracion7 = resultado;
                            }


                            resultado = 0;
                            switch (oIndicador.Operacion1.Trim())
                            {
                                case ("+"):
                                    if (operador1.valor8 != null && operador2.valor8 != null)
                                        resultado = decimal.Parse(operador1.valor8.ToString()) + decimal.Parse(operador2.valor8.ToString());
                                    break;
                                case ("-"):
                                    if (operador1.valor8 != null && operador2.valor8 != null)
                                        resultado = decimal.Parse(operador1.valor8.ToString()) - decimal.Parse(operador2.valor8.ToString());
                                    break;
                                case ("x"):
                                    if (operador1.valor8 != null && operador2.valor8 != null)
                                        resultado = decimal.Parse(operador1.valor8.ToString()) * decimal.Parse(operador2.valor8.ToString());
                                    break;
                                case ("/"):
                                    if (operador1.valor8 != null && operador2.valor8 != null && operador2.valor8 != 0)
                                        resultado = decimal.Parse(operador1.valor8.ToString()) / decimal.Parse(operador2.valor8.ToString());
                                    break;
                            }
                            if (oIndicador.Periodicidad == "Mensual" || oIndicador.Periodicidad == "Bimestral")
                                oIndicador.Valoracion8 = resultado;

                            if (oIndicador.Operacion2 != null && operador3 != null)
                            {
                                switch (oIndicador.Operacion2.Trim())
                                {
                                    case ("+"):
                                        if (operador3.valor8 != null)
                                            resultado = resultado + decimal.Parse(operador3.valor8.ToString());
                                        break;
                                    case ("-"):
                                        if (operador3.valor8 != null)
                                            resultado = resultado - decimal.Parse(operador3.valor8.ToString());
                                        break;
                                    case ("x"):
                                        if (operador3.valor8 != null)
                                            resultado = resultado * decimal.Parse(operador3.valor8.ToString());
                                        break;
                                    case ("/"):
                                        if (operador3.valor8 != null && operador3.valor8 != 0)
                                            resultado = resultado / decimal.Parse(operador3.valor8.ToString());
                                        break;
                                }
                                if (oIndicador.Periodicidad == "Mensual" || oIndicador.Periodicidad == "Bimestral")
                                    oIndicador.Valoracion8 = resultado;
                            }


                            resultado = 0;
                            switch (oIndicador.Operacion1.Trim())
                            {
                                case ("+"):
                                    if (operador1.valor9 != null && operador2.valor9 != null)
                                        resultado = decimal.Parse(operador1.valor9.ToString()) + decimal.Parse(operador2.valor9.ToString());
                                    break;
                                case ("-"):
                                    if (operador1.valor9 != null && operador2.valor9 != null)
                                        resultado = decimal.Parse(operador1.valor9.ToString()) - decimal.Parse(operador2.valor9.ToString());
                                    break;
                                case ("x"):
                                    if (operador1.valor9 != null && operador2.valor9 != null)
                                        resultado = decimal.Parse(operador1.valor9.ToString()) * decimal.Parse(operador2.valor9.ToString());
                                    break;
                                case ("/"):
                                    if (operador1.valor9 != null && operador2.valor9 != null && operador2.valor9 != 0)
                                        resultado = decimal.Parse(operador1.valor9.ToString()) / decimal.Parse(operador2.valor9.ToString());
                                    break;
                            }
                            if (oIndicador.Periodicidad == "Mensual" || oIndicador.Periodicidad == "Trimestral")
                                oIndicador.Valoracion9 = resultado;

                            if (oIndicador.Operacion2 != null && operador3 != null)
                            {
                                switch (oIndicador.Operacion2.Trim())
                                {
                                    case ("+"):
                                        if (operador3.valor9 != null)
                                            resultado = resultado + decimal.Parse(operador3.valor9.ToString());
                                        break;
                                    case ("-"):
                                        if (operador3.valor9 != null)
                                            resultado = resultado - decimal.Parse(operador3.valor9.ToString());
                                        break;
                                    case ("x"):
                                        if (operador3.valor9 != null)
                                            resultado = resultado * decimal.Parse(operador3.valor9.ToString());
                                        break;
                                    case ("/"):
                                        if (operador3.valor9 != null && operador3.valor9 != 0)
                                            resultado = resultado / decimal.Parse(operador3.valor9.ToString());
                                        break;
                                }
                                if (oIndicador.Periodicidad == "Mensual" || oIndicador.Periodicidad == "Trimestral")
                                    oIndicador.Valoracion9 = resultado;
                            }


                            resultado = 0;
                            switch (oIndicador.Operacion1.Trim())
                            {
                                case ("+"):
                                    if (operador1.valor10 != null && operador2.valor10 != null)
                                        resultado = decimal.Parse(operador1.valor10.ToString()) + decimal.Parse(operador2.valor10.ToString());
                                    break;
                                case ("-"):
                                    if (operador1.valor10 != null && operador2.valor10 != null)
                                        resultado = decimal.Parse(operador1.valor10.ToString()) - decimal.Parse(operador2.valor10.ToString());
                                    break;
                                case ("x"):
                                    if (operador1.valor10 != null && operador2.valor10 != null)
                                        resultado = decimal.Parse(operador1.valor10.ToString()) * decimal.Parse(operador2.valor10.ToString());
                                    break;
                                case ("/"):
                                    if (operador1.valor10 != null && operador2.valor10 != null && operador2.valor10 != 0)
                                        resultado = decimal.Parse(operador1.valor10.ToString()) / decimal.Parse(operador2.valor10.ToString());
                                    break;
                            }
                            if (oIndicador.Periodicidad == "Mensual" || oIndicador.Periodicidad == "Bimestral")
                                oIndicador.Valoracion10 = resultado;

                            if (oIndicador.Operacion2 != null && operador3 != null)
                            {
                                switch (oIndicador.Operacion2.Trim())
                                {
                                    case ("+"):
                                        if (operador3.valor10 != null)
                                            resultado = resultado + decimal.Parse(operador3.valor10.ToString());
                                        break;
                                    case ("-"):
                                        if (operador3.valor10 != null)
                                            resultado = resultado - decimal.Parse(operador3.valor10.ToString());
                                        break;
                                    case ("x"):
                                        if (operador3.valor10 != null)
                                            resultado = resultado * decimal.Parse(operador3.valor10.ToString());
                                        break;
                                    case ("/"):
                                        if (operador3.valor10 != null && operador3.valor10 != 0)
                                            resultado = resultado / decimal.Parse(operador3.valor10.ToString());
                                        break;
                                }
                                if (oIndicador.Periodicidad == "Mensual" || oIndicador.Periodicidad == "Bimestral")
                                    oIndicador.Valoracion10 = resultado;
                            }


                            resultado = 0;
                            switch (oIndicador.Operacion1.Trim())
                            {
                                case ("+"):
                                    if (operador1.valor11 != null && operador2.valor11 != null)
                                        resultado = decimal.Parse(operador1.valor11.ToString()) + decimal.Parse(operador2.valor11.ToString());
                                    break;
                                case ("-"):
                                    if (operador1.valor11 != null && operador2.valor11 != null)
                                        resultado = decimal.Parse(operador1.valor11.ToString()) - decimal.Parse(operador2.valor11.ToString());
                                    break;
                                case ("x"):
                                    if (operador1.valor11 != null && operador2.valor11 != null)
                                        resultado = decimal.Parse(operador1.valor11.ToString()) * decimal.Parse(operador2.valor11.ToString());
                                    break;
                                case ("/"):
                                    if (operador1.valor11 != null && operador2.valor11 != null && operador2.valor11 != 0)
                                        resultado = decimal.Parse(operador1.valor11.ToString()) / decimal.Parse(operador2.valor11.ToString());
                                    break;
                            }
                            if (oIndicador.Periodicidad == "Mensual")
                                oIndicador.Valoracion11 = resultado;

                            if (oIndicador.Operacion2 != null && operador3 != null)
                            {
                                switch (oIndicador.Operacion2.Trim())
                                {
                                    case ("+"):
                                        if (operador3.valor11 != null)
                                            resultado = resultado + decimal.Parse(operador3.valor11.ToString());
                                        break;
                                    case ("-"):
                                        if (operador3.valor11 != null)
                                            resultado = resultado - decimal.Parse(operador3.valor11.ToString());
                                        break;
                                    case ("x"):
                                        if (operador3.valor11 != null)
                                            resultado = resultado * decimal.Parse(operador3.valor11.ToString());
                                        break;
                                    case ("/"):
                                        if (operador3.valor11 != null && operador3.valor11 != 0)
                                            resultado = resultado / decimal.Parse(operador3.valor11.ToString());
                                        break;
                                }
                                if (oIndicador.Periodicidad == "Mensual")
                                    oIndicador.Valoracion11 = resultado;

                            }


                            resultado = 0;
                            switch (oIndicador.Operacion1.Trim())
                            {
                                case ("+"):
                                    if (operador1.valor12 != null && operador2.valor12 != null)
                                        resultado = decimal.Parse(operador1.valor12.ToString()) + decimal.Parse(operador2.valor12.ToString());
                                    break;
                                case ("-"):
                                    if (operador1.valor12 != null && operador2.valor12 != null)
                                        resultado = decimal.Parse(operador1.valor12.ToString()) - decimal.Parse(operador2.valor12.ToString());
                                    break;
                                case ("x"):
                                    if (operador1.valor12 != null && operador2.valor12 != null)
                                        resultado = decimal.Parse(operador1.valor12.ToString()) * decimal.Parse(operador2.valor12.ToString());
                                    break;
                                case ("/"):
                                    if (operador1.valor12 != null && operador2.valor12 != null && operador2.valor12 != 0)
                                        resultado = decimal.Parse(operador1.valor12.ToString()) / decimal.Parse(operador2.valor12.ToString());
                                    break;
                            }
                            if (oIndicador.Periodicidad == "Mensual" || oIndicador.Periodicidad == "Bimestral" || oIndicador.Periodicidad == "Trimestral" || oIndicador.Periodicidad == "Semestral" || oIndicador.Periodicidad == "Anual")
                                oIndicador.Valoracion12 = resultado;

                            if (oIndicador.Operacion2 != null && operador3 != null)
                            {
                                switch (oIndicador.Operacion2.Trim())
                                {
                                    case ("+"):
                                        if (operador3.valor12 != null)
                                            resultado = resultado + decimal.Parse(operador3.valor12.ToString());
                                        break;
                                    case ("-"):
                                        if (operador3.valor12 != null)
                                            resultado = resultado - decimal.Parse(operador3.valor12.ToString());
                                        break;
                                    case ("x"):
                                        if (operador3.valor12 != null)
                                            resultado = resultado * decimal.Parse(operador3.valor12.ToString());
                                        break;
                                    case ("/"):
                                        if (operador3.valor12 != null && operador3.valor12 != 0)
                                            resultado = resultado / decimal.Parse(operador3.valor12.ToString());
                                        break;
                                }
                                if (oIndicador.Periodicidad == "Mensual" || oIndicador.Periodicidad == "Bimestral" || oIndicador.Periodicidad == "Trimestral" || oIndicador.Periodicidad == "Semestral" || oIndicador.Periodicidad == "Anual")
                                    oIndicador.Valoracion12 = resultado;
                            }


                        }
                        #endregion

                        #region valor calculado

                        switch (oIndicador.Operacion)
                        {
                            case "Computo":
                                oIndicador.ValorCalculado1 = oIndicador.Valoracion1;
                                oIndicador.ValorCalculado2 = oIndicador.Valoracion2;
                                oIndicador.ValorCalculado3 = oIndicador.Valoracion3;
                                oIndicador.ValorCalculado4 = oIndicador.Valoracion4;
                                oIndicador.ValorCalculado5 = oIndicador.Valoracion5;
                                oIndicador.ValorCalculado6 = oIndicador.Valoracion6;
                                oIndicador.ValorCalculado7 = oIndicador.Valoracion7;
                                oIndicador.ValorCalculado8 = oIndicador.Valoracion8;
                                oIndicador.ValorCalculado9 = oIndicador.Valoracion9;
                                oIndicador.ValorCalculado10 = oIndicador.Valoracion10;
                                oIndicador.ValorCalculado11 = oIndicador.Valoracion11;
                                oIndicador.ValorCalculado12 = oIndicador.Valoracion12;
                                break;

                            case "Acumulado":
                                oIndicador.ValorCalculado1 = oIndicador.Valoracion1;
                                oIndicador.ValorCalculado2 = oIndicador.ValorCalculado1 + oIndicador.Valoracion2;
                                oIndicador.ValorCalculado3 = oIndicador.ValorCalculado2 + oIndicador.Valoracion3;
                                oIndicador.ValorCalculado4 = oIndicador.ValorCalculado3 + oIndicador.Valoracion4;
                                oIndicador.ValorCalculado5 = oIndicador.ValorCalculado4 + oIndicador.Valoracion5;
                                oIndicador.ValorCalculado6 = oIndicador.ValorCalculado5 + oIndicador.Valoracion6;
                                oIndicador.ValorCalculado7 = oIndicador.ValorCalculado6 + oIndicador.Valoracion7;
                                oIndicador.ValorCalculado8 = oIndicador.ValorCalculado7 + oIndicador.Valoracion8;
                                oIndicador.ValorCalculado9 = oIndicador.ValorCalculado8 + oIndicador.Valoracion9;
                                oIndicador.ValorCalculado10 = oIndicador.ValorCalculado9 + oIndicador.Valoracion10;
                                oIndicador.ValorCalculado11 = oIndicador.ValorCalculado10 + oIndicador.Valoracion11;
                                oIndicador.ValorCalculado12 = oIndicador.ValorCalculado11 + oIndicador.Valoracion12;
                                break;

                            case "Promedio":
                                oIndicador.ValorCalculado1 = oIndicador.Valoracion1;
                                oIndicador.ValorCalculado2 = (oIndicador.ValorCalculado1 + oIndicador.Valoracion2) / 2;
                                oIndicador.ValorCalculado3 = (oIndicador.Valoracion1 + oIndicador.Valoracion2 + oIndicador.Valoracion3) / 3;
                                oIndicador.ValorCalculado4 = (oIndicador.Valoracion1 + oIndicador.Valoracion2 + oIndicador.Valoracion3 + oIndicador.Valoracion4) / 4;
                                oIndicador.ValorCalculado5 = (oIndicador.Valoracion1 + oIndicador.Valoracion2 + oIndicador.Valoracion3 + oIndicador.Valoracion4 + oIndicador.Valoracion5) / 5;
                                oIndicador.ValorCalculado6 = (oIndicador.Valoracion1 + oIndicador.Valoracion2 + oIndicador.Valoracion3 + oIndicador.Valoracion4 + oIndicador.Valoracion5 + oIndicador.Valoracion6) / 6;
                                oIndicador.ValorCalculado7 = (oIndicador.Valoracion1 + oIndicador.Valoracion2 + oIndicador.Valoracion3 + oIndicador.Valoracion4 + oIndicador.Valoracion5 + oIndicador.Valoracion6 + oIndicador.Valoracion7) / 7;
                                oIndicador.ValorCalculado8 = (oIndicador.Valoracion1 + oIndicador.Valoracion2 + oIndicador.Valoracion3 + oIndicador.Valoracion4 + oIndicador.Valoracion5 + oIndicador.Valoracion6 + oIndicador.Valoracion7 + oIndicador.Valoracion8) / 8;
                                oIndicador.ValorCalculado9 = (oIndicador.Valoracion1 + oIndicador.Valoracion2 + oIndicador.Valoracion3 + oIndicador.Valoracion4 + oIndicador.Valoracion5 + oIndicador.Valoracion6 + oIndicador.Valoracion7 + oIndicador.Valoracion8 + oIndicador.Valoracion9) / 9;
                                oIndicador.ValorCalculado10 = (oIndicador.Valoracion1 + oIndicador.Valoracion2 + oIndicador.Valoracion3 + oIndicador.Valoracion4 + oIndicador.Valoracion5 + oIndicador.Valoracion6 + oIndicador.Valoracion7 + oIndicador.Valoracion8 + oIndicador.Valoracion9 + oIndicador.Valoracion10) / 10;
                                oIndicador.ValorCalculado11 = (oIndicador.Valoracion1 + oIndicador.Valoracion2 + oIndicador.Valoracion3 + oIndicador.Valoracion4 + oIndicador.Valoracion5 + oIndicador.Valoracion6 + oIndicador.Valoracion7 + oIndicador.Valoracion8 + oIndicador.Valoracion9 + oIndicador.Valoracion10 + oIndicador.Valoracion11) / 11;
                                oIndicador.ValorCalculado12 = (oIndicador.Valoracion1 + oIndicador.Valoracion2 + oIndicador.Valoracion3 + oIndicador.Valoracion4 + oIndicador.Valoracion5 + oIndicador.Valoracion6 + oIndicador.Valoracion7 + oIndicador.Valoracion8 + oIndicador.Valoracion9 + oIndicador.Valoracion10 + oIndicador.Valoracion11 + oIndicador.Valoracion12) / 12;
                                break;

                            case "AcumuladoPeriodo":
                                if (oIndicador.Periodicidad == "Mensual")
                                {
                                    oIndicador.ValorCalculado1 = oIndicador.Valoracion1;
                                    oIndicador.ValorCalculado2 = oIndicador.Valoracion2;
                                    oIndicador.ValorCalculado3 = oIndicador.Valoracion3;
                                    oIndicador.ValorCalculado4 = oIndicador.Valoracion4;
                                    oIndicador.ValorCalculado5 = oIndicador.Valoracion5;
                                    oIndicador.ValorCalculado6 = oIndicador.Valoracion6;
                                    oIndicador.ValorCalculado7 = oIndicador.Valoracion7;
                                    oIndicador.ValorCalculado8 = oIndicador.Valoracion8;
                                    oIndicador.ValorCalculado9 = oIndicador.Valoracion9;
                                    oIndicador.ValorCalculado10 = oIndicador.Valoracion10;
                                    oIndicador.ValorCalculado11 = oIndicador.Valoracion11;
                                    oIndicador.ValorCalculado12 = oIndicador.Valoracion12;
                                }
                                if (oIndicador.Periodicidad == "Bimestral")
                                {
                                    oIndicador.ValorCalculado2 = oIndicador.Valoracion1 + oIndicador.Valoracion2;
                                    oIndicador.ValorCalculado4 = oIndicador.Valoracion3 + oIndicador.Valoracion4;
                                    oIndicador.ValorCalculado6 = oIndicador.Valoracion5 + oIndicador.Valoracion6;
                                    oIndicador.ValorCalculado8 = oIndicador.Valoracion7 + oIndicador.Valoracion8;
                                    oIndicador.ValorCalculado10 = oIndicador.Valoracion9 + oIndicador.Valoracion10;
                                    oIndicador.ValorCalculado12 = oIndicador.Valoracion11 + oIndicador.Valoracion12;
                                }

                                if (oIndicador.Periodicidad == "Trimestral")
                                {
                                    oIndicador.ValorCalculado3 = oIndicador.Valoracion1 + oIndicador.Valoracion2 + oIndicador.Valoracion3;
                                    oIndicador.ValorCalculado6 = oIndicador.Valoracion4 + oIndicador.Valoracion5 + oIndicador.Valoracion6;
                                    oIndicador.ValorCalculado9 = oIndicador.Valoracion7 + oIndicador.Valoracion8 + oIndicador.Valoracion9;
                                    oIndicador.ValorCalculado12 = oIndicador.Valoracion10 + oIndicador.Valoracion11 + oIndicador.Valoracion12;
                                }

                                if (oIndicador.Periodicidad == "Cuatrimestral")
                                {
                                    oIndicador.ValorCalculado4 = oIndicador.Valoracion1 + oIndicador.Valoracion2 + oIndicador.Valoracion3 + oIndicador.Valoracion4;
                                    oIndicador.ValorCalculado8 = oIndicador.Valoracion5 + oIndicador.Valoracion6 + oIndicador.Valoracion7 + oIndicador.Valoracion8;
                                    oIndicador.ValorCalculado12 = oIndicador.Valoracion9 + oIndicador.Valoracion10 + oIndicador.Valoracion11 + oIndicador.Valoracion12;
                                }
                                if (oIndicador.Periodicidad == "Semestral")
                                {
                                    oIndicador.ValorCalculado6 = oIndicador.Valoracion1 + oIndicador.Valoracion2 + oIndicador.Valoracion3 + oIndicador.Valoracion4 + oIndicador.Valoracion5 + oIndicador.Valoracion6;
                                    oIndicador.ValorCalculado12 = oIndicador.Valoracion7 + oIndicador.Valoracion8 + oIndicador.Valoracion9 + oIndicador.Valoracion10 + oIndicador.Valoracion11 + oIndicador.Valoracion12;
                                }
                                if (oIndicador.Periodicidad == "Anual")
                                {
                                    oIndicador.ValorCalculado12 = oIndicador.Valoracion1 + oIndicador.Valoracion2 + oIndicador.Valoracion3 + oIndicador.Valoracion4 + oIndicador.Valoracion5 + oIndicador.Valoracion6 + oIndicador.Valoracion7 + oIndicador.Valoracion8 + oIndicador.Valoracion9 + oIndicador.Valoracion10 + oIndicador.Valoracion11 + oIndicador.Valoracion12;
                                }
                                break;

                            case "PromedioPeriodo":
                                if (oIndicador.Periodicidad == "Mensual")
                                {
                                    oIndicador.ValorCalculado1 = oIndicador.Valoracion1;
                                    oIndicador.ValorCalculado2 = oIndicador.Valoracion2;
                                    oIndicador.ValorCalculado3 = oIndicador.Valoracion3;
                                    oIndicador.ValorCalculado4 = oIndicador.Valoracion4;
                                    oIndicador.ValorCalculado5 = oIndicador.Valoracion5;
                                    oIndicador.ValorCalculado6 = oIndicador.Valoracion6;
                                    oIndicador.ValorCalculado7 = oIndicador.Valoracion7;
                                    oIndicador.ValorCalculado8 = oIndicador.Valoracion8;
                                    oIndicador.ValorCalculado9 = oIndicador.Valoracion9;
                                    oIndicador.ValorCalculado10 = oIndicador.Valoracion10;
                                    oIndicador.ValorCalculado11 = oIndicador.Valoracion11;
                                    oIndicador.ValorCalculado12 = oIndicador.Valoracion12;
                                }
                                if (oIndicador.Periodicidad == "Bimestral")
                                {
                                    oIndicador.ValorCalculado2 = (oIndicador.Valoracion1 + oIndicador.Valoracion2) / 2;
                                    oIndicador.ValorCalculado4 = (oIndicador.Valoracion3 + oIndicador.Valoracion4) / 2;
                                    oIndicador.ValorCalculado6 = (oIndicador.Valoracion5 + oIndicador.Valoracion6) / 2;
                                    oIndicador.ValorCalculado8 = (oIndicador.Valoracion7 + oIndicador.Valoracion8) / 2;
                                    oIndicador.ValorCalculado10 = (oIndicador.Valoracion9 + oIndicador.Valoracion10) / 2;
                                    oIndicador.ValorCalculado12 = (oIndicador.Valoracion11 + oIndicador.Valoracion12) / 2;
                                }

                                if (oIndicador.Periodicidad == "Trimestral")
                                {
                                    oIndicador.ValorCalculado3 = (oIndicador.Valoracion1 + oIndicador.Valoracion2 + oIndicador.Valoracion3) / 3;
                                    oIndicador.ValorCalculado6 = (oIndicador.Valoracion4 + oIndicador.Valoracion5 + oIndicador.Valoracion6) / 3;
                                    oIndicador.ValorCalculado9 = (oIndicador.Valoracion7 + oIndicador.Valoracion8 + oIndicador.Valoracion9) / 3;
                                    oIndicador.ValorCalculado12 = (oIndicador.Valoracion10 + oIndicador.Valoracion11 + oIndicador.Valoracion12) / 3;
                                }

                                if (oIndicador.Periodicidad == "Cuatrimestral")
                                {
                                    oIndicador.ValorCalculado4 = (oIndicador.Valoracion1 + oIndicador.Valoracion2 + oIndicador.Valoracion3 + oIndicador.Valoracion4) / 4;
                                    oIndicador.ValorCalculado8 = (oIndicador.Valoracion5 + oIndicador.Valoracion6 + oIndicador.Valoracion7 + oIndicador.Valoracion8) / 4;
                                    oIndicador.ValorCalculado12 = (oIndicador.Valoracion9 + oIndicador.Valoracion10 + oIndicador.Valoracion11 + oIndicador.Valoracion12) / 4;
                                }
                                if (oIndicador.Periodicidad == "Semestral")
                                {
                                    oIndicador.ValorCalculado6 = (oIndicador.Valoracion1 + oIndicador.Valoracion2 + oIndicador.Valoracion3 + oIndicador.Valoracion4 + oIndicador.Valoracion5 + oIndicador.Valoracion6) / 6;
                                    oIndicador.ValorCalculado12 = (oIndicador.Valoracion7 + oIndicador.Valoracion8 + oIndicador.Valoracion9 + oIndicador.Valoracion10 + oIndicador.Valoracion11 + oIndicador.Valoracion12) / 6;
                                }
                                if (oIndicador.Periodicidad == "Anual")
                                {
                                    oIndicador.ValorCalculado12 = (oIndicador.Valoracion1 + oIndicador.Valoracion2 + oIndicador.Valoracion3 + oIndicador.Valoracion4 + oIndicador.Valoracion5 + oIndicador.Valoracion6 + oIndicador.Valoracion7 + oIndicador.Valoracion8 + oIndicador.Valoracion9 + oIndicador.Valoracion10 + oIndicador.Valoracion11 + oIndicador.Valoracion12) / 12;
                                }
                                break;
                        }

                        #endregion

                        Datos.ActualizarImputacion(oIndicador);
                    }
                    #endregion

                    ViewData["editarparametro"] = actualizar;
                    if (actualizar != null)
                        ViewData["consultaparametro"] = Datos.ObtenerParametroInd(actualizar.CodIndiHojaDatos);
                    return View();
                }
                catch (Exception ex)
                {
                    int anio = int.Parse(collection["ctl00$MainContent$ddlAnio"].ToString());
                    indicadores_hojadedatos_valores indHojaDatos = Datos.ObtenerParametroInd(id, anio, idCentral);
                    ViewData["editarparametro"] = indHojaDatos;
                    if (indHojaDatos != null)
                        ViewData["consultaparametro"] = Datos.ObtenerParametroInd(indHojaDatos.CodIndiHojaDatos);

                    Session["EdicionParametroError"] = "No se han podido guardar los datos, compruebe que los datos introducidos son correctos.";
                    return View();
                }
                #endregion
            }
            else
            {
                #region recarga
                int anio = int.Parse(collection["ctl00$MainContent$ddlAnio"].ToString());
                indicadores_hojadedatos_valores indHojaDatos = Datos.ObtenerParametroInd(id, anio, idCentral);

                if (indHojaDatos != null)
                {
                    ViewData["idParametro"] = indHojaDatos.Id;
                }
                else
                {
                    Datos.CrearParametroIndicador(id, anio, idCentral);
                    indHojaDatos = Datos.ObtenerParametroInd(id, anio, idCentral);
                }
                Session["anioImputacion"] = anio;
                ViewData["editarparametro"] = indHojaDatos;
                if (indHojaDatos != null)
                    ViewData["consultaparametro"] = Datos.ObtenerParametroInd(indHojaDatos.CodIndiHojaDatos);
                return View();
                #endregion
            }
        }


        #region utiles
        private static Cell InsertCellInWorksheet(string columnName, uint rowIndex, Worksheet worksheet)
        {
            SheetData sheetData = worksheet.GetFirstChild<SheetData>();
            string cellReference = columnName + rowIndex;
            Row row;
            if (sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).Count() != 0)
            {
                row = sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).First();
            }
            else
            {
                row = new Row() { RowIndex = rowIndex };
                sheetData.Append(row);
            }
            if (row.Elements<Cell>().Where(c => c.CellReference.Value == columnName + rowIndex).Count() > 0)
            {
                return row.Elements<Cell>().Where(c => c.CellReference.Value == cellReference).First();
            }
            else
            {
                Cell refCell = null;
                foreach (Cell cell in row.Elements<Cell>())
                {
                    if (string.Compare(cell.CellReference.Value, cellReference, true) > 0)
                    {
                        refCell = cell;
                        break;
                    }
                }

                Cell newCell = new Cell() { CellReference = cellReference };
                row.InsertBefore(newCell, refCell);
                worksheet.Save();
                return newCell;
            }
        }

        public static void SearchAndReplace(string document, Dictionary<string, string> dict)
        {
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(document, true))
            {
                string docText = null;
                using (StreamReader sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
                {
                    docText = sr.ReadToEnd();
                }

                foreach (KeyValuePair<string, string> item in dict)
                {
                    Regex regexText = new Regex(item.Key);
                    docText = regexText.Replace(docText, item.Value);
                }

                using (StreamWriter sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
                {
                    sw.Write(docText);
                }
            }
        }
        #endregion

    }
}
