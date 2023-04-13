using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using MIDAS.Models;
using System.Configuration;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using MIDAS.Classes;
using DocumentFormat.OpenXml;

namespace MIDAS.Controllers
{
    public class AspectosAmbientalesController : Controller
    {
        //
        // GET: /AspectosAmbientales/

        public ActionResult ayuda()
        {
            

            return View();
        }

        public ActionResult gestion_aspectos()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }
            int centralElegida = int.Parse(Session["CentralElegida"].ToString());

            

            centros central = Datos.ObtenerCentroPorID(centralElegida);

            ViewData["aspectosAplicablesP"] = Datos.ListarGruposAplicables(0);
            ViewData["aspectosAplicablesF"] = Datos.ListarGruposAplicables(1);

            ViewData["aspectosvaloradosP"] = Datos.ListarAspectosValoracion(central.id, 0);
            ViewData["aspectosvaloradosF"] = Datos.ListarAspectosValoracion(central.id, 1);

            #region generacion fichero
            if (Session["nombreArchivo"] != null && Session["nombreArchivo"].ToString() != "")
            {
                if (Session["nombreArchivo"].ToString().Contains("xlsx"))
                {
                    Response.ContentType = "application/msexcel";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=ExportacionAspectos.xlsx");
                    Response.TransmitFile(Session["nombreArchivo"].ToString());
                    Response.End();
                }
                Session["nombreArchivo"] = "";
            }
            #endregion

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult gestion_aspectos(FormCollection collection)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }
            int centralElegida = int.Parse(Session["CentralElegida"].ToString());
            centros central = Datos.ObtenerCentroPorID(centralElegida);
            auditorias audi = new auditorias();
            string formulario = collection["hdFormularioEjecutado"];

            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            int idCentral = centroseleccionado.id;

            if (formulario == "btnAddAspectoParametro")
            {
                int Foco = 0;

                #region añadir indicador
                if (collection["ctl00$MainContent$ddlAspectosParametro"] != null)
                {
                    int idAspecto = int.Parse(collection["ctl00$MainContent$ddlAspectosParametro"]);

                    Datos.CrearValoracionAspecto(idAspecto, idCentral, Foco);
                }
                

                ViewData["aspectosAplicablesP"] = Datos.ListarGruposAplicables(0);
                ViewData["aspectosAplicablesF"] = Datos.ListarGruposAplicables(1);

                ViewData["aspectosvaloradosP"] = Datos.ListarAspectosValoracion(central.id, 0);
                ViewData["aspectosvaloradosF"] = Datos.ListarAspectosValoracion(central.id, 1);

                Session["EdicionAspectosMensaje"] = "Aspecto asignado correctamente";
                return View();
                #endregion
            }
            if (formulario == "btnAddAspectoFoco")
            {
                int Foco = 1;

                #region añadir indicador
                if (collection["ctl00$MainContent$ddlAspectosFoco"] != null)
                {
                    int idAspecto = int.Parse(collection["ctl00$MainContent$ddlAspectosFoco"]);
                    string nombrefoco = collection["ctl00$MainContent$txtNombreFoco"];
                    int continuo = 0;
                    if (collection["ctl00$MainContent$ddlContinuo"] != null)
                        continuo = int.Parse(collection["ctl00$MainContent$ddlContinuo"]);

                    Datos.CrearValoracionAspecto(idAspecto, idCentral, Foco, nombrefoco, continuo);
                }


                ViewData["aspectosAplicablesP"] = Datos.ListarGruposAplicables(0);
                ViewData["aspectosAplicablesF"] = Datos.ListarGruposAplicables(1);

                ViewData["aspectosvaloradosP"] = Datos.ListarAspectosValoracion(central.id, 0);
                ViewData["aspectosvaloradosF"] = Datos.ListarAspectosValoracion(central.id, 1);

                Session["EdicionAspectosMensaje"] = "Aspecto asignado correctamente";
                return View();
                #endregion
            }
            if (formulario == "btnImprimir")
            {

                if (collection["ctl00$MainContent$ddlAnio"] != null && collection["ctl00$MainContent$ddlAnio"].ToString() !="0")
                {
                    int anio = int.Parse(collection["ctl00$MainContent$ddlAnio"]);
                    log_parametros logparam = ObtenerHistoricoParametros(centroseleccionado.id, anio);
                    List<VISTA_ListarLogEvaluacionesParametro> listaEvalParametros;
                    List<VISTA_ListarLogEvaluacionesFoco> listaEvalFoco;
                    List<VISTA_ListarLogParametrosFocos> listaParametrosEvaluados;

                    #region generacion fichero
                    string sourceFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaPlantillaWord"] + "ExportacionHistAspectos.xlsx");
                    string destinationFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaGeneracionWord"] + "ExportacionHistAspectos_" + DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + ".xlsx");

                    Session["source"] = sourceFile;
                    Session["destino"] = destinationFile;
                    // Create a copy of the template file and open the copy 
                    System.IO.File.Copy(sourceFile, destinationFile, true);

                    #region impresion

                    using (SpreadsheetDocument document = SpreadsheetDocument.Open(destinationFile, true))
                    {
                        #region hoja1-parametros
                        WorksheetPart worksheetPart = GetWorksheetPartByName(document, "Parámetros Evaluación");
                        
                        #region crear campos y celdas
                        Row row = new Row();

                        #region celdas mwhb

                        Cell mwhb1 = GetCell(worksheetPart.Worksheet, "A", 2);
                        mwhb1.CellValue = new CellValue(logparam.mwhb1.ToString());
                        mwhb1.DataType = new EnumValue<CellValues>(CellValues.String);

                        Cell mwhb2 = GetCell(worksheetPart.Worksheet, "B", 2);
                        mwhb2.CellValue = new CellValue(logparam.mwhb2.ToString());
                        mwhb2.DataType = new EnumValue<CellValues>(CellValues.String);

                        Cell mwhb3 = GetCell(worksheetPart.Worksheet, "C", 2);
                        mwhb3.CellValue = new CellValue(logparam.mwhb3.ToString());
                        mwhb3.DataType = new EnumValue<CellValues>(CellValues.String);

                        Cell mwhb4 = GetCell(worksheetPart.Worksheet, "D", 2);
                        mwhb4.CellValue = new CellValue(logparam.mwhb4.ToString());
                        mwhb4.DataType = new EnumValue<CellValues>(CellValues.String);

                        Cell mwhb5 = GetCell(worksheetPart.Worksheet, "E", 2);
                        mwhb5.CellValue = new CellValue(logparam.mwhb5.ToString());
                        mwhb5.DataType = new EnumValue<CellValues>(CellValues.String);

                        Cell mwhb6 = GetCell(worksheetPart.Worksheet, "F", 2);
                        mwhb6.CellValue = new CellValue(logparam.mwhb6.ToString());
                        mwhb6.DataType = new EnumValue<CellValues>(CellValues.String);
                        #endregion

                        #region celdas hanioge

                        Cell hanioge1 = GetCell(worksheetPart.Worksheet, "A", 4);
                        hanioge1.CellValue = new CellValue(logparam.hanioge1.ToString());
                        hanioge1.DataType = new EnumValue<CellValues>(CellValues.String);

                        Cell hanioge2 = GetCell(worksheetPart.Worksheet, "B", 4);
                        hanioge2.CellValue = new CellValue(logparam.hanioge2.ToString());
                        hanioge2.DataType = new EnumValue<CellValues>(CellValues.String);

                        Cell hanioge3 = GetCell(worksheetPart.Worksheet, "C", 4);
                        hanioge3.CellValue = new CellValue(logparam.hanioge3.ToString());
                        hanioge3.DataType = new EnumValue<CellValues>(CellValues.String);

                        Cell hanioge4 = GetCell(worksheetPart.Worksheet, "D", 4);
                        hanioge4.CellValue = new CellValue(logparam.hanioge4.ToString());
                        hanioge4.DataType = new EnumValue<CellValues>(CellValues.String);

                        Cell hanioge5 = GetCell(worksheetPart.Worksheet, "E", 4);
                        hanioge5.CellValue = new CellValue(logparam.hanioge5.ToString());
                        hanioge5.DataType = new EnumValue<CellValues>(CellValues.String);

                        Cell hanioge6 = GetCell(worksheetPart.Worksheet, "F", 4);
                        hanioge6.CellValue = new CellValue(logparam.hanioge6.ToString());
                        hanioge6.DataType = new EnumValue<CellValues>(CellValues.String);
                        #endregion

                        #region celdas kmanio

                        Cell kmanio1 = GetCell(worksheetPart.Worksheet, "A", 6);
                        kmanio1.CellValue = new CellValue(logparam.kmanio1.ToString());
                        kmanio1.DataType = new EnumValue<CellValues>(CellValues.String);

                        Cell kmanio2 = GetCell(worksheetPart.Worksheet, "B", 6);
                        kmanio2.CellValue = new CellValue(logparam.kmanio2.ToString());
                        kmanio2.DataType = new EnumValue<CellValues>(CellValues.String);

                        Cell kmanio3 = GetCell(worksheetPart.Worksheet, "C", 6);
                        kmanio3.CellValue = new CellValue(logparam.kmanio3.ToString());
                        kmanio3.DataType = new EnumValue<CellValues>(CellValues.String);

                        Cell kmanio4 = GetCell(worksheetPart.Worksheet, "D", 6);
                        kmanio4.CellValue = new CellValue(logparam.kmanio4.ToString());
                        kmanio4.DataType = new EnumValue<CellValues>(CellValues.String);

                        Cell kmanio5 = GetCell(worksheetPart.Worksheet, "E", 6);
                        kmanio5.CellValue = new CellValue(logparam.kmanio5.ToString());
                        kmanio5.DataType = new EnumValue<CellValues>(CellValues.String);

                        Cell kmanio6 = GetCell(worksheetPart.Worksheet, "F", 6);
                        kmanio6.CellValue = new CellValue(logparam.kmanio6.ToString());
                        kmanio6.DataType = new EnumValue<CellValues>(CellValues.String);
                        #endregion

                        #region celdas m3hora

                        Cell m3hora1 = GetCell(worksheetPart.Worksheet, "A", 8);
                        m3hora1.CellValue = new CellValue(logparam.m3hora1.ToString());
                        m3hora1.DataType = new EnumValue<CellValues>(CellValues.String);

                        Cell m3hora2 = GetCell(worksheetPart.Worksheet, "B", 8);
                        m3hora2.CellValue = new CellValue(logparam.m3hora2.ToString());
                        m3hora2.DataType = new EnumValue<CellValues>(CellValues.String);

                        Cell m3hora3 = GetCell(worksheetPart.Worksheet, "C", 8);
                        m3hora3.CellValue = new CellValue(logparam.m3hora3.ToString());
                        m3hora3.DataType = new EnumValue<CellValues>(CellValues.String);

                        Cell m3hora4 = GetCell(worksheetPart.Worksheet, "D", 8);
                        m3hora4.CellValue = new CellValue(logparam.m3hora4.ToString());
                        m3hora4.DataType = new EnumValue<CellValues>(CellValues.String);

                        Cell m3hora5 = GetCell(worksheetPart.Worksheet, "E", 8);
                        m3hora5.CellValue = new CellValue(logparam.m3hora5.ToString());
                        m3hora5.DataType = new EnumValue<CellValues>(CellValues.String);

                        Cell m3hora6 = GetCell(worksheetPart.Worksheet, "F", 8);
                        m3hora6.CellValue = new CellValue(logparam.m3hora6.ToString());
                        m3hora6.DataType = new EnumValue<CellValues>(CellValues.String);
                        #endregion

                        #region celdas hfunc

                        Cell hfunc1 = GetCell(worksheetPart.Worksheet, "A", 10);
                        hfunc1.CellValue = new CellValue(logparam.hfunc1.ToString());
                        hfunc1.DataType = new EnumValue<CellValues>(CellValues.String);

                        Cell hfunc2 = GetCell(worksheetPart.Worksheet, "B", 10);
                        hfunc2.CellValue = new CellValue(logparam.hfunc2.ToString());
                        hfunc2.DataType = new EnumValue<CellValues>(CellValues.String);

                        Cell hfunc3 = GetCell(worksheetPart.Worksheet, "C", 10);
                        hfunc3.CellValue = new CellValue(logparam.hfunc3.ToString());
                        hfunc3.DataType = new EnumValue<CellValues>(CellValues.String);

                        Cell hfunc4 = GetCell(worksheetPart.Worksheet, "D", 10);
                        hfunc4.CellValue = new CellValue(logparam.hfunc4.ToString());
                        hfunc4.DataType = new EnumValue<CellValues>(CellValues.String);

                        Cell hfunc5 = GetCell(worksheetPart.Worksheet, "E", 10);
                        hfunc5.CellValue = new CellValue(logparam.hfunc5.ToString());
                        hfunc5.DataType = new EnumValue<CellValues>(CellValues.String);

                        Cell hfunc6 = GetCell(worksheetPart.Worksheet, "F", 10);
                        hfunc6.CellValue = new CellValue(logparam.hfunc6.ToString());
                        hfunc6.DataType = new EnumValue<CellValues>(CellValues.String);
                        #endregion

                        #region celdas numtrab

                        Cell numtrab1 = GetCell(worksheetPart.Worksheet, "A", 12);
                        numtrab1.CellValue = new CellValue(logparam.numtrab1.ToString());
                        numtrab1.DataType = new EnumValue<CellValues>(CellValues.String);

                        Cell numtrab2 = GetCell(worksheetPart.Worksheet, "B", 12);
                        numtrab2.CellValue = new CellValue(logparam.numtrab2.ToString());
                        numtrab2.DataType = new EnumValue<CellValues>(CellValues.String);

                        Cell numtrab3 = GetCell(worksheetPart.Worksheet, "C", 12);
                        numtrab3.CellValue = new CellValue(logparam.numtrab3.ToString());
                        numtrab3.DataType = new EnumValue<CellValues>(CellValues.String);

                        Cell numtrab4 = GetCell(worksheetPart.Worksheet, "D", 12);
                        numtrab4.CellValue = new CellValue(logparam.numtrab4.ToString());
                        numtrab4.DataType = new EnumValue<CellValues>(CellValues.String);

                        Cell numtrab5 = GetCell(worksheetPart.Worksheet, "E", 12);
                        numtrab5.CellValue = new CellValue(logparam.numtrab5.ToString());
                        numtrab5.DataType = new EnumValue<CellValues>(CellValues.String);

                        Cell numtrab6 = GetCell(worksheetPart.Worksheet, "F", 12);
                        numtrab6.CellValue = new CellValue(logparam.numtrab6.ToString());
                        numtrab6.DataType = new EnumValue<CellValues>(CellValues.String);
                        #endregion

                        #region celdas aguadesalada

                        Cell aguadesalada1 = GetCell(worksheetPart.Worksheet, "A", 14);
                        aguadesalada1.CellValue = new CellValue(logparam.m3aguadesalada1.ToString());
                        aguadesalada1.DataType = new EnumValue<CellValues>(CellValues.String);

                        Cell aguadesalada2 = GetCell(worksheetPart.Worksheet, "B", 14);
                        aguadesalada2.CellValue = new CellValue(logparam.m3aguadesalada2.ToString());
                        aguadesalada2.DataType = new EnumValue<CellValues>(CellValues.String);

                        Cell aguadesalada3 = GetCell(worksheetPart.Worksheet, "C", 14);
                        aguadesalada3.CellValue = new CellValue(logparam.m3aguadesalada3.ToString());
                        aguadesalada3.DataType = new EnumValue<CellValues>(CellValues.String);

                        Cell aguadesalada4 = GetCell(worksheetPart.Worksheet, "D", 14);
                        aguadesalada4.CellValue = new CellValue(logparam.m3aguadesalada4.ToString());
                        aguadesalada4.DataType = new EnumValue<CellValues>(CellValues.String);

                        Cell aguadesalada5 = GetCell(worksheetPart.Worksheet, "E", 14);
                        aguadesalada5.CellValue = new CellValue(logparam.m3aguadesalada5.ToString());
                        aguadesalada5.DataType = new EnumValue<CellValues>(CellValues.String);

                        Cell aguadesalada6 = GetCell(worksheetPart.Worksheet, "F", 14);
                        aguadesalada6.CellValue = new CellValue(logparam.m3aguadesalada6.ToString());
                        aguadesalada6.DataType = new EnumValue<CellValues>(CellValues.String);
                        #endregion

                        #region celdas numtrab

                        Cell trabcantera1 = GetCell(worksheetPart.Worksheet, "A", 16);
                        trabcantera1.CellValue = new CellValue(logparam.trabcantera1.ToString());
                        trabcantera1.DataType = new EnumValue<CellValues>(CellValues.String);

                        Cell trabcantera2 = GetCell(worksheetPart.Worksheet, "B", 16);
                        trabcantera2.CellValue = new CellValue(logparam.trabcantera2.ToString());
                        trabcantera2.DataType = new EnumValue<CellValues>(CellValues.String);

                        Cell trabcantera3 = GetCell(worksheetPart.Worksheet, "C", 16);
                        trabcantera3.CellValue = new CellValue(logparam.trabcantera3.ToString());
                        trabcantera3.DataType = new EnumValue<CellValues>(CellValues.String);

                        Cell trabcantera4 = GetCell(worksheetPart.Worksheet, "D", 16);
                        trabcantera4.CellValue = new CellValue(logparam.trabcantera4.ToString());
                        trabcantera4.DataType = new EnumValue<CellValues>(CellValues.String);

                        Cell trabcantera5 = GetCell(worksheetPart.Worksheet, "E", 16);
                        trabcantera5.CellValue = new CellValue(logparam.trabcantera5.ToString());
                        trabcantera5.DataType = new EnumValue<CellValues>(CellValues.String);

                        Cell trabcantera6 = GetCell(worksheetPart.Worksheet, "F", 16);
                        trabcantera6.CellValue = new CellValue(logparam.trabcantera6.ToString());
                        trabcantera6.DataType = new EnumValue<CellValues>(CellValues.String);
                        #endregion

                        #endregion


                        // save worksheet
                        document.WorkbookPart.WorksheetParts.ElementAt(0).Worksheet.Save();

                        #endregion

                        #region hoja2-Evaluaciones por parámetro

                        worksheetPart = GetWorksheetPartByName(document, "Evaluaciones por parámetro");

                        listaEvalParametros = Datos.ListarLogEvaluacionesParametro(centroseleccionado.id, anio);

                        SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();
                        foreach (VISTA_ListarLogEvaluacionesParametro asp in listaEvalParametros)
                        {
                            #region crear campos y celdas
                            Row fila = new Row();

                            #region declaracion campos
                            string codigocentral = string.Empty;
                            string codigo = string.Empty;
                            string nombre = string.Empty;
                            string identificacion = string.Empty;
                            string valor = string.Empty;
                            string variacion = string.Empty;
                            string acercamiento = string.Empty;
                            string referencia = string.Empty;
                            string datoabsoluto = string.Empty;
                            string queja = string.Empty;
                            string quejaobs = string.Empty;
                            string resmagnitud = string.Empty;
                            string resnaturaleza = string.Empty;
                            string resorigen = string.Empty;
                            string significancia = string.Empty;
                            string idregistro = string.Empty;

                            #endregion
                            #region asignacion campos
                            if (asp.codaspcentral == null)
                                codigocentral = string.Empty;
                            else
                                codigocentral = asp.codaspcentral;

                            if (asp.Codigo == null)
                                codigo = string.Empty;
                            else
                                codigo = asp.Codigo;

                            if (asp.grupo == null)
                                valor = string.Empty;
                            else
                                nombre = asp.grupo;

                            if (asp.Nombre == null)
                                identificacion = asp.grupo;
                            else
                                identificacion = asp.Nombre;

                            if (asp.valoranio == null)
                                valor = string.Empty;
                            else
                                valor = asp.valoranio.ToString();

                            if (asp.variacion == null)
                                variacion = string.Empty;
                            else
                                variacion = asp.variacion.ToString();

                            if (asp.acercamiento == null)
                                acercamiento = string.Empty;
                            else
                                acercamiento = asp.acercamiento.ToString();

                            if (asp.referencia == null)
                                referencia = string.Empty;
                            else
                                referencia = asp.referencia.ToString();

                            if (asp.datoabsoluto == null)
                                datoabsoluto = string.Empty;
                            else
                                datoabsoluto = asp.datoabsoluto.ToString();

                            if (asp.queja == null)
                                queja = string.Empty;
                            else
                                queja = asp.queja.ToString();

                            if (asp.quejaobs == null)
                                quejaobs = string.Empty;
                            else
                                quejaobs = asp.quejaobs.ToString();

                            if (asp.resmagnitud == null)
                                resmagnitud = string.Empty;
                            else
                                resmagnitud = asp.resmagnitud.ToString();

                            if (asp.resnaturaleza == null)
                                resnaturaleza = string.Empty;
                            else
                                resnaturaleza = asp.resnaturaleza.ToString();

                            if (asp.resorigen == null)
                                resorigen = string.Empty;
                            else
                                resorigen = asp.resorigen.ToString();

                            if (asp.significancia == null)
                                significancia = string.Empty;
                            else
                            {

                                if (asp.significancia.ToString() == "1")
                                    significancia = "Significativo";
                                else
                                    significancia = "No significativo";
                            }

                            if (asp.id_registro == null)
                                idregistro = string.Empty;
                            else
                                idregistro = asp.id_registro.ToString();
                            #endregion

                            #region construccion fila
                            fila.Append(
                            Datos.ConstructCell(anio.ToString(), CellValues.String),
                            Datos.ConstructCell(codigocentral, CellValues.String),
                            Datos.ConstructCell(codigo, CellValues.String),
                            Datos.ConstructCell(nombre, CellValues.String),
                            Datos.ConstructCell(identificacion, CellValues.String),
                            Datos.ConstructCell(valor, CellValues.String),
                            Datos.ConstructCell(variacion, CellValues.String),
                            Datos.ConstructCell(acercamiento, CellValues.String),
                            Datos.ConstructCell(referencia, CellValues.String),
                            Datos.ConstructCell(datoabsoluto, CellValues.String),
                            Datos.ConstructCell(queja, CellValues.String),
                            Datos.ConstructCell(quejaobs, CellValues.String),
                            Datos.ConstructCell(resmagnitud, CellValues.String),
                            Datos.ConstructCell(resnaturaleza, CellValues.String),
                            Datos.ConstructCell(resorigen, CellValues.String),
                            Datos.ConstructCell(significancia, CellValues.String),
                            Datos.ConstructCell(idregistro, CellValues.String));
                            #endregion
                            sheetData.AppendChild(fila);
                            #endregion
                        }

                        // save worksheet
                        document.WorkbookPart.WorksheetParts.ElementAt(1).Worksheet.Save();

                        #endregion

                        #region hoja3-Evaluaciones por foco

                        worksheetPart = GetWorksheetPartByName(document, "Evaluaciones por foco");

                        listaEvalFoco = Datos.ListarLogEvaluacionesFoco(centroseleccionado.id, anio);

                        sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();
                        foreach (VISTA_ListarLogEvaluacionesFoco asp in listaEvalFoco)
                        {
                            #region crear campos y celdas
                            Row fila = new Row();

                            #region declaracion campos
                            string codigocentral = string.Empty;
                            string codigo = string.Empty;
                            string nombre = string.Empty;
                            string identificacion = string.Empty;
                            string valor = string.Empty;
                            string variacion = string.Empty;
                            string acercamiento = string.Empty;
                            string referencia = string.Empty;
                            string datoabsoluto = string.Empty;
                            string queja = string.Empty;
                            string quejaobs = string.Empty;
                            string resmagnitud = string.Empty;
                            string resnaturaleza = string.Empty;
                            string resorigen = string.Empty;
                            string significancia = string.Empty;
                            string idregistro = string.Empty;

                            #endregion
                            #region asignacion campos
                            if (asp.codaspcentral == null)
                                codigocentral = string.Empty;
                            else
                                codigocentral = asp.codaspcentral;

                            if (asp.Codigo == null)
                                codigo = string.Empty;
                            else
                                codigo = asp.Codigo;

                            if (asp.Nombre == null)
                                identificacion = string.Empty;
                            else
                                identificacion = asp.Nombre;

                            if (asp.valoranio == null)
                                valor = string.Empty;
                            else
                                valor = asp.valoranio.ToString();

                            if (asp.variacion == null)
                                variacion = string.Empty;
                            else
                                variacion = asp.variacion.ToString();

                            if (asp.acercamiento == null)
                                acercamiento = string.Empty;
                            else
                                acercamiento = asp.acercamiento.ToString();

                            if (asp.referencia == null)
                                referencia = string.Empty;
                            else
                                referencia = asp.referencia.ToString();

                            if (asp.datoabsoluto == null)
                                datoabsoluto = string.Empty;
                            else
                                datoabsoluto = asp.datoabsoluto.ToString();

                            if (asp.queja == null)
                                queja = string.Empty;
                            else
                                queja = asp.queja.ToString();

                            if (asp.quejaobs == null)
                                quejaobs = string.Empty;
                            else
                                quejaobs = asp.quejaobs.ToString();

                            if (asp.resmagnitud == null)
                                resmagnitud = string.Empty;
                            else
                                resmagnitud = asp.resmagnitud.ToString();

                            if (asp.resnaturaleza == null)
                                resnaturaleza = string.Empty;
                            else
                                resnaturaleza = asp.resnaturaleza.ToString();

                            if (asp.resorigen == null)
                                resorigen = string.Empty;
                            else
                                resorigen = asp.resorigen.ToString();

                            if (asp.significancia == null)
                                significancia = string.Empty;
                            else
                            {

                                if (asp.significancia.ToString() == "1")
                                    significancia = "Significativo";
                                else
                                    significancia = "No significativo";
                            }

                            if (asp.id_registro == null)
                                idregistro = string.Empty;
                            else
                                idregistro = asp.id_registro.ToString();
                            #endregion

                            #region construccion fila
                            fila.Append(
                            Datos.ConstructCell(anio.ToString(), CellValues.String),
                            Datos.ConstructCell(codigocentral, CellValues.String),
                            Datos.ConstructCell(codigo, CellValues.String),                            
                            Datos.ConstructCell(nombre, CellValues.String),
                            Datos.ConstructCell(identificacion, CellValues.String),
                            Datos.ConstructCell(valor, CellValues.String),
                            Datos.ConstructCell(variacion, CellValues.String),
                            Datos.ConstructCell(acercamiento, CellValues.String),
                            Datos.ConstructCell(referencia, CellValues.String),
                            Datos.ConstructCell(datoabsoluto, CellValues.String),
                            Datos.ConstructCell(queja, CellValues.String),
                            Datos.ConstructCell(quejaobs, CellValues.String),
                            Datos.ConstructCell(resmagnitud, CellValues.String),
                            Datos.ConstructCell(resnaturaleza, CellValues.String),
                            Datos.ConstructCell(resorigen, CellValues.String),
                            Datos.ConstructCell(significancia, CellValues.String),
                            Datos.ConstructCell(idregistro, CellValues.String));
                            #endregion
                            sheetData.AppendChild(fila);
                            #endregion
                        }

                        // save worksheet
                        document.WorkbookPart.WorksheetParts.ElementAt(2).Worksheet.Save();

                        #endregion

                        #region hoja4-Parámetros focos

                        worksheetPart = GetWorksheetPartByName(document, "Parámetros focos");

                        listaParametrosEvaluados = Datos.ListarLogParametrosFocos(centroseleccionado.id, anio);

                        sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();
                        foreach (VISTA_ListarLogParametrosFocos asp in listaParametrosEvaluados)
                        {
                            #region crear campos y celdas
                            Row fila = new Row();

                            #region declaracion campos
                            string idvaloracion = string.Empty;
                            string codigo = string.Empty;
                            string grupo = string.Empty; 
                            string nombre = string.Empty; 
                            string nombrefoco = string.Empty; 
                            string nombreparametro = string.Empty;
                            string mes1 = string.Empty;
                            string mes2 = string.Empty;
                            string mes3 = string.Empty;
                            string mes4 = string.Empty;
                            string mes5 = string.Empty;
                            string mes6 = string.Empty;
                            string mes7 = string.Empty;
                            string mes8 = string.Empty;
                            string mes9 = string.Empty;
                            string mes10 = string.Empty;
                            string mes11 = string.Empty;
                            string mes12 = string.Empty;
                            string valor = string.Empty;
                            string dia_ref = string.Empty;
                            string tarde_ref = string.Empty;
                            string noche_ref = string.Empty;
                            string dia = string.Empty;
                            string tarde = string.Empty;
                            string noche = string.Empty;
                            string variacion = string.Empty;
                            string acercamiento = string.Empty;
                            string referencia = string.Empty;
                            string referenciasup = string.Empty;
                            string resmagnitud = string.Empty;
                            string resnaturaleza = string.Empty;
                            string resorigen = string.Empty;
                            string significancia = string.Empty;

                            #endregion
                            #region asignacion campos
                            if (asp.id_registro == null)
                                idvaloracion = string.Empty;
                            else
                                idvaloracion = asp.id_registro.ToString();

                            if (asp.Codigo == null)
                                codigo = string.Empty;
                            else
                                codigo = asp.Codigo;

                            if (asp.grupo == null)
                                grupo = string.Empty;
                            else
                                grupo = asp.grupo;

                            if (asp.nombrefoco == null)
                                nombrefoco = string.Empty;
                            else
                                nombrefoco = asp.nombrefoco.ToString();

                            if (asp.nombre == null)
                                nombreparametro = string.Empty;
                            else
                                nombreparametro = asp.nombre.ToString();

                            if (asp.mes1 == null)
                                mes1 = string.Empty;
                            else
                                mes1 = asp.mes1.ToString();

                            if (asp.mes2 == null)
                                mes2 = string.Empty;
                            else
                                mes2 = asp.mes2.ToString();

                            if (asp.mes3 == null)
                                mes3 = string.Empty;
                            else
                                mes3 = asp.mes3.ToString();

                            if (asp.mes4 == null)
                                mes4 = string.Empty;
                            else
                                mes4 = asp.mes4.ToString();

                            if (asp.mes5 == null)
                                mes5 = string.Empty;
                            else
                                mes5 = asp.mes5.ToString();

                            if (asp.mes6 == null)
                                mes6 = string.Empty;
                            else
                                mes6 = asp.mes6.ToString();

                            if (asp.mes7 == null)
                                mes7 = string.Empty;
                            else
                                mes7 = asp.mes7.ToString();

                            if (asp.mes8 == null)
                                mes8 = string.Empty;
                            else
                                mes8 = asp.mes8.ToString();

                            if (asp.mes9 == null)
                                mes9 = string.Empty;
                            else
                                mes9 = asp.mes9.ToString();

                            if (asp.mes10 == null)
                                mes10 = string.Empty;
                            else
                                mes10 = asp.mes10.ToString();

                            if (asp.mes11 == null)
                                mes11 = string.Empty;
                            else
                                mes11 = asp.mes11.ToString();

                            if (asp.mes12 == null)
                                mes12 = string.Empty;
                            else
                                mes12 = asp.mes12.ToString();

                            if (asp.valor == null)
                                valor = string.Empty;
                            else
                                valor = asp.valor.ToString();

                            if (asp.RU_DiaRef == null)
                                dia_ref = string.Empty;
                            else
                                dia_ref = asp.RU_DiaRef.ToString();

                            if (asp.RU_TardeRef == null)
                                tarde_ref = string.Empty;
                            else
                                tarde_ref = asp.RU_TardeRef.ToString();

                            if (asp.RU_NocheRef == null)
                                noche_ref = string.Empty;
                            else
                                noche_ref = asp.RU_NocheRef.ToString();

                            if (asp.RU_Dia == null)
                                dia = string.Empty;
                            else
                                dia = asp.RU_Dia.ToString();

                            if (asp.RU_Tarde == null)
                                tarde = string.Empty;
                            else
                                tarde = asp.RU_Tarde.ToString();

                            if (asp.RU_Noche == null)
                                noche = string.Empty;
                            else
                                noche = asp.RU_Noche.ToString();

                            if (asp.variacion == null)
                                variacion = string.Empty;
                            else
                                variacion = asp.variacion.ToString();

                            if (asp.acercamiento == null)
                                acercamiento = string.Empty;
                            else
                                acercamiento = asp.acercamiento.ToString();

                            if (asp.referencia == null)
                                referencia = string.Empty;
                            else
                                referencia = asp.referencia.ToString();

                            if (asp.referenciasup == null)
                                referenciasup = string.Empty;
                            else
                                referenciasup = asp.referenciasup.ToString();

                            if (asp.resmagnitud == null)
                                resmagnitud = string.Empty;
                            else
                                resmagnitud = asp.resmagnitud.ToString();

                            if (asp.resnaturaleza == null)
                                resnaturaleza = string.Empty;
                            else
                                resnaturaleza = asp.resnaturaleza.ToString();

                            if (asp.resorigen == null)
                                resorigen = string.Empty;
                            else
                                resorigen = asp.resorigen.ToString();

                            if (asp.significancia == null)
                                significancia = string.Empty;
                            else
                            {

                                if (asp.significancia.ToString() == "1")
                                    significancia = "Significativo";
                                else
                                    significancia = "No significativo";
                            }

                            #endregion

                            #region construccion fila
                            fila.Append(
                            Datos.ConstructCell(anio.ToString(), CellValues.String),
                            Datos.ConstructCell(idvaloracion, CellValues.String),
                            Datos.ConstructCell(codigo, CellValues.String),
                            Datos.ConstructCell(grupo, CellValues.String),
                            Datos.ConstructCell(nombrefoco, CellValues.String),
                            Datos.ConstructCell(nombreparametro, CellValues.String),
                            Datos.ConstructCell(mes1, CellValues.String),
                            Datos.ConstructCell(mes2, CellValues.String),
                            Datos.ConstructCell(mes3, CellValues.String),
                            Datos.ConstructCell(mes4, CellValues.String),
                            Datos.ConstructCell(mes5, CellValues.String),
                            Datos.ConstructCell(mes6, CellValues.String),
                            Datos.ConstructCell(mes7, CellValues.String),
                            Datos.ConstructCell(mes8, CellValues.String),
                            Datos.ConstructCell(mes9, CellValues.String),
                            Datos.ConstructCell(mes10, CellValues.String),
                            Datos.ConstructCell(mes11, CellValues.String),
                            Datos.ConstructCell(mes12, CellValues.String),
                            Datos.ConstructCell(valor, CellValues.String),
                            Datos.ConstructCell(dia_ref, CellValues.String),
                            Datos.ConstructCell(tarde_ref, CellValues.String),
                            Datos.ConstructCell(noche_ref, CellValues.String),
                            Datos.ConstructCell(dia, CellValues.String),
                            Datos.ConstructCell(tarde, CellValues.String),
                            Datos.ConstructCell(noche, CellValues.String),
                            Datos.ConstructCell(variacion, CellValues.String),
                            Datos.ConstructCell(acercamiento, CellValues.String),
                            Datos.ConstructCell(referencia, CellValues.String),
                            Datos.ConstructCell(referenciasup, CellValues.String),
                            Datos.ConstructCell(resmagnitud, CellValues.String),
                            Datos.ConstructCell(resnaturaleza, CellValues.String),
                            Datos.ConstructCell(resorigen, CellValues.String),
                            Datos.ConstructCell(significancia, CellValues.String));
                            #endregion
                            sheetData.AppendChild(fila);
                            #endregion
                        }

                        // save worksheet
                        document.WorkbookPart.WorksheetParts.ElementAt(3).Worksheet.Save();

                        #endregion

                        document.WorkbookPart.Workbook.Save();

                        Session["nombreArchivo"] = destinationFile;
                    }
                    #endregion
                    #endregion

                }
                else
                {
                    List<VISTA_AspectosValoracion> aspectos = Datos.ListarAspectosValoracion(centroseleccionado.id);

                    #region generacion fichero
                    string sourceFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaPlantillaWord"] + "ExportacionAspectos.xlsx");
                    string destinationFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaGeneracionWord"] + "ExportacionAspectos_" + DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + ".xlsx");

                    Session["source"] = sourceFile;
                    Session["destino"] = destinationFile;
                    // Create a copy of the template file and open the copy 
                    System.IO.File.Copy(sourceFile, destinationFile, true);

                    #region impresion

                    using (SpreadsheetDocument document = SpreadsheetDocument.Open(destinationFile, true))
                    {
                        SheetData sheetData = document.WorkbookPart.WorksheetParts.ElementAt(0).Worksheet.Elements<SheetData>().First();
                        foreach (VISTA_AspectosValoracion asp in aspectos)
                        {
                            #region crear campos y celdas
                            Row row = new Row();

                            #region declaracion campos
                            string codigo = string.Empty;
                            string grupo = string.Empty;
                            string identificacion = string.Empty;
                            string unidad = string.Empty;
                            string descripcion = string.Empty;
                            string impacto = string.Empty;
                            string magnitud = string.Empty;
                            string naturaleza = string.Empty;
                            string origen = string.Empty;
                            string queja = string.Empty;
                            string significancia = string.Empty;
                            #endregion
                            #region asignacion campos
                            if (asp.Codigo == null)
                                codigo = string.Empty;
                            else
                                codigo = asp.Expr1;
                            if (asp.grupo == null)
                                grupo = string.Empty;
                            else
                                grupo = asp.grupo;

                            if (asp.foco == 0)
                            {
                                if (asp.Nombre == null)
                                    identificacion = string.Empty;
                                else
                                    identificacion = asp.Nombre;
                            }
                            else
                            {
                                if (asp.nombrefoco == null)
                                    identificacion = string.Empty;
                                else
                                    identificacion = asp.nombrefoco;
                            }

                            if (asp.Unidad == null)
                                unidad = string.Empty;
                            else
                                unidad = asp.Unidad;

                            if (asp.descripcion == null)
                                descripcion = string.Empty;
                            else
                                descripcion = asp.descripcion;

                            if (asp.Impacto == null)
                                impacto = string.Empty;
                            else
                                impacto = asp.Impacto;

                            if (asp.resmagnitud == null)
                                magnitud = string.Empty;
                            else
                                magnitud = asp.resmagnitud.ToString();

                            if (asp.resnaturaleza == null)
                                naturaleza = string.Empty;
                            else
                                naturaleza = asp.resnaturaleza.ToString();

                            if (asp.resorigen == null)
                                origen = string.Empty;
                            else
                                origen = asp.resorigen.ToString();

                            if (asp.queja == null || asp.queja == 0)
                                queja = "No";
                            else
                                queja = "Sí";

                            if (asp.significancia6 == null)
                                significancia = string.Empty;
                            if (asp.significancia6 == 0)
                                significancia = "No Significativo";
                            if (asp.significancia6 == 1)
                                significancia = "Significativo";
                            #endregion

                            #region construccion fila
                            row.Append(
                            Datos.ConstructCell(codigo, CellValues.String),
                            Datos.ConstructCell(grupo, CellValues.String),
                            Datos.ConstructCell(identificacion, CellValues.String),
                            Datos.ConstructCell(unidad, CellValues.String),
                            Datos.ConstructCell(descripcion, CellValues.String),
                            Datos.ConstructCell(impacto, CellValues.String),
                            Datos.ConstructCell(magnitud, CellValues.String),
                            Datos.ConstructCell(naturaleza, CellValues.String),
                            Datos.ConstructCell(origen, CellValues.String),
                            Datos.ConstructCell(queja, CellValues.String),
                            Datos.ConstructCell(significancia, CellValues.String));
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
                }


                ViewData["aspectosAplicablesP"] = Datos.ListarGruposAplicables(0);
                ViewData["aspectosAplicablesF"] = Datos.ListarGruposAplicables(1);

                ViewData["aspectosvaloradosP"] = Datos.ListarAspectosValoracion(central.id, 0);
                ViewData["aspectosvaloradosF"] = Datos.ListarAspectosValoracion(central.id, 1);
                return RedirectToAction("gestion_aspectos", "AspectosAmbientales");
            }

            if (formulario == "btnImprimirCatalogo")
            {
                #region impresión catálogo

                List<VISTA_MaestroAspectos> aspectos = Datos.ListarAspectos();

                #region generacion fichero
                string sourceFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaPlantillaWord"] + "ExportacionAspectosMaestro.xlsx");
                string destinationFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaGeneracionWord"] + "ExportacionAspectosMaestro_" + DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + ".xlsx");

                Session["source"] = sourceFile;
                Session["destino"] = destinationFile;
                // Create a copy of the template file and open the copy 
                System.IO.File.Copy(sourceFile, destinationFile, true);

                #region impresion

                using (SpreadsheetDocument document = SpreadsheetDocument.Open(destinationFile, true))
                {
                    SheetData sheetData = document.WorkbookPart.WorksheetParts.ElementAt(0).Worksheet.Elements<SheetData>().First();
                    foreach (VISTA_MaestroAspectos asp in aspectos)
                    {
                        #region crear campos y celdas
                        Row row = new Row();

                        #region declaracion campos
                        string grupo = string.Empty;
                        string identificacion = string.Empty;
                        string elemento = string.Empty;
                        string unidad = string.Empty;

                        #endregion
                        #region asignacion campos
                        if (asp.grupo == null)
                            grupo = string.Empty;
                        else
                            grupo = asp.grupo;
                        if (asp.Codigo == null)
                            identificacion = string.Empty;
                        else
                            identificacion = asp.Codigo;
                        if (asp.Nombre == null)
                            elemento = string.Empty;
                        else
                            elemento = asp.Nombre;
                        if (asp.Unidad == null)
                            unidad = string.Empty;
                        else
                            unidad = asp.Unidad;

                        #endregion

                        #region construccion fila
                        row.Append(
                         Datos.ConstructCell(identificacion, CellValues.String),
                        Datos.ConstructCell(grupo, CellValues.String),
                        Datos.ConstructCell(elemento, CellValues.String),
                        Datos.ConstructCell(unidad, CellValues.String));
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


                ViewData["aspectosAplicablesP"] = Datos.ListarGruposAplicables(0);
                ViewData["aspectosAplicablesF"] = Datos.ListarGruposAplicables(1);

                ViewData["aspectosvaloradosP"] = Datos.ListarAspectosValoracion(central.id, 0);
                ViewData["aspectosvaloradosF"] = Datos.ListarAspectosValoracion(central.id, 1);
                return RedirectToAction("gestion_aspectos", "AspectosAmbientales");
                #endregion
            }
            else
            {
                int Foco = 1;
                if (collection["ctl00$MainContent$ddlFoco"] != null)
                {
                    Foco = int.Parse(collection["ctl00$MainContent$ddlFoco"].ToString());
                }

                ViewData["aspectosAplicablesP"] = Datos.ListarGruposAplicables(0);
                ViewData["aspectosAplicablesF"] = Datos.ListarGruposAplicables(1);

                ViewData["aspectosvaloradosP"] = Datos.ListarAspectosValoracion(central.id, 0);
                ViewData["aspectosvaloradosF"] = Datos.ListarAspectosValoracion(central.id, 1);
                return View();
            }
        }

        public log_parametros ObtenerHistoricoParametros (int idCentral, int Anio)
        {
            log_parametros parametros = new log_parametros();
            int anioparametros = Anio - 5;
            int primeraniovigenteactual = DateTime.Now.Year - 6;
            aspecto_parametros asp_param = Datos.ObtenerParametrosCentral(idCentral);
            

            int diferenciaanios = primeraniovigenteactual - anioparametros;

            if (diferenciaanios == 1)
            {
                aspecto_parametros_log asp_param_log1 = Datos.ObtenerParametrosLogCentral(idCentral, Anio - 5);

                parametros.mwhb6 = asp_param.Mwhb5;
                parametros.mwhb5 = asp_param.Mwhb4;
                parametros.mwhb4 = asp_param.Mwhb3;
                parametros.mwhb3 = asp_param.Mwhb2;
                parametros.mwhb2 = asp_param.Mwhb1;
                parametros.mwhb1 = asp_param_log1.Mwhb;

                parametros.kmanio6 = asp_param.kmAnio5;
                parametros.kmanio5 = asp_param.kmAnio4;
                parametros.kmanio4 = asp_param.kmAnio3;
                parametros.kmanio3 = asp_param.kmAnio2;
                parametros.kmanio2 = asp_param.kmAnio1;
                parametros.kmanio1 = asp_param_log1.kmAnio;

                parametros.m3hora6 = asp_param.m3Hora5;
                parametros.m3hora5 = asp_param.m3Hora4;
                parametros.m3hora4 = asp_param.m3Hora3;
                parametros.m3hora3 = asp_param.m3Hora2;
                parametros.m3hora2 = asp_param.m3Hora1;
                parametros.m3hora1 = asp_param_log1.m3Hora;

                parametros.numtrab6 = asp_param.numtrabAnio5;
                parametros.numtrab5 = asp_param.numtrabAnio4;
                parametros.numtrab4 = asp_param.numtrabAnio3;
                parametros.numtrab3 = asp_param.numtrabAnio2;
                parametros.numtrab2 = asp_param.numtrabAnio1;
                parametros.numtrab1 = asp_param_log1.numtrab;

                parametros.m3aguadesalada6 = asp_param.m3aguadesaladaAnio5;
                parametros.m3aguadesalada5 = asp_param.m3aguadesaladaAnio4;
                parametros.m3aguadesalada4 = asp_param.m3aguadesaladaAnio3;
                parametros.m3aguadesalada3 = asp_param.m3aguadesaladaAnio2;
                parametros.m3aguadesalada2 = asp_param.m3aguadesaladaAnio1;
                parametros.m3aguadesalada1 = asp_param_log1.m3aguadesalada;

                parametros.trabcantera6 = asp_param.trabcanteraAnio5;
                parametros.trabcantera5 = asp_param.trabcanteraAnio4;
                parametros.trabcantera4 = asp_param.trabcanteraAnio3;
                parametros.trabcantera3 = asp_param.trabcanteraAnio2;
                parametros.trabcantera2 = asp_param.trabcanteraAnio1;
                parametros.trabcantera1 = asp_param_log1.trabcantera;
            }

            if (diferenciaanios == 2)
            {
                aspecto_parametros_log asp_param_log2 = Datos.ObtenerParametrosLogCentral(idCentral, Anio - 4);
                aspecto_parametros_log asp_param_log1 = Datos.ObtenerParametrosLogCentral(idCentral, Anio - 5);

                parametros.mwhb6 = asp_param.Mwhb4;
                parametros.mwhb5 = asp_param.Mwhb3;
                parametros.mwhb4 = asp_param.Mwhb2;
                parametros.mwhb3 = asp_param.Mwhb1;
                parametros.mwhb2 = asp_param_log2.Mwhb;
                parametros.mwhb1 = asp_param_log1.Mwhb;

                parametros.kmanio6 = asp_param.kmAnio4;
                parametros.kmanio5 = asp_param.kmAnio3;
                parametros.kmanio4 = asp_param.kmAnio2;
                parametros.kmanio3 = asp_param.kmAnio1;
                parametros.kmanio2 = asp_param_log2.kmAnio;
                parametros.kmanio1 = asp_param_log1.kmAnio;

                parametros.m3hora6 = asp_param.m3Hora4;
                parametros.m3hora5 = asp_param.m3Hora3;
                parametros.m3hora4 = asp_param.m3Hora2;
                parametros.m3hora3 = asp_param.m3Hora1;
                parametros.m3hora2 = asp_param_log2.m3Hora;
                parametros.m3hora1 = asp_param_log1.m3Hora;

                parametros.numtrab6 = asp_param.numtrabAnio4;
                parametros.numtrab5 = asp_param.numtrabAnio3;
                parametros.numtrab4 = asp_param.numtrabAnio2;
                parametros.numtrab3 = asp_param.numtrabAnio1;
                parametros.numtrab2 = asp_param_log2.numtrab;
                parametros.numtrab1 = asp_param_log1.numtrab;

                parametros.m3aguadesalada6 = asp_param.m3aguadesaladaAnio4;
                parametros.m3aguadesalada5 = asp_param.m3aguadesaladaAnio3;
                parametros.m3aguadesalada4 = asp_param.m3aguadesaladaAnio2;
                parametros.m3aguadesalada3 = asp_param.m3aguadesaladaAnio1;
                parametros.m3aguadesalada2 = asp_param_log2.m3aguadesalada;
                parametros.m3aguadesalada1 = asp_param_log1.m3aguadesalada;

                parametros.trabcantera6 = asp_param.trabcanteraAnio4;
                parametros.trabcantera5 = asp_param.trabcanteraAnio3;
                parametros.trabcantera4 = asp_param.trabcanteraAnio2;
                parametros.trabcantera3 = asp_param.trabcanteraAnio1;
                parametros.trabcantera2 = asp_param_log2.trabcantera;
                parametros.trabcantera1 = asp_param_log1.trabcantera;
            }

            if (diferenciaanios == 3)
            {
                aspecto_parametros_log asp_param_log3 = Datos.ObtenerParametrosLogCentral(idCentral, Anio - 3);
                aspecto_parametros_log asp_param_log2 = Datos.ObtenerParametrosLogCentral(idCentral, Anio - 4);
                aspecto_parametros_log asp_param_log1 = Datos.ObtenerParametrosLogCentral(idCentral, Anio - 5);

                parametros.mwhb6 = asp_param.Mwhb3;
                parametros.mwhb5 = asp_param.Mwhb2;
                parametros.mwhb4 = asp_param.Mwhb1;
                parametros.mwhb3 = asp_param_log3.Mwhb;
                parametros.mwhb2 = asp_param_log2.Mwhb;
                parametros.mwhb1 = asp_param_log1.Mwhb;

                parametros.kmanio6 = asp_param.kmAnio3;
                parametros.kmanio5 = asp_param.kmAnio2;
                parametros.kmanio4 = asp_param.kmAnio1;
                parametros.kmanio3 = asp_param_log3.kmAnio;
                parametros.kmanio2 = asp_param_log2.kmAnio;
                parametros.kmanio1 = asp_param_log1.kmAnio;

                parametros.m3hora6 = asp_param.m3Hora3;
                parametros.m3hora5 = asp_param.m3Hora2;
                parametros.m3hora4 = asp_param.m3Hora1;
                parametros.m3hora3 = asp_param_log3.m3Hora;
                parametros.m3hora2 = asp_param_log2.m3Hora;
                parametros.m3hora1 = asp_param_log1.m3Hora;

                parametros.numtrab6 = asp_param.numtrabAnio3;
                parametros.numtrab5 = asp_param.numtrabAnio2;
                parametros.numtrab4 = asp_param.numtrabAnio1;
                parametros.numtrab3 = asp_param_log3.numtrab;
                parametros.numtrab2 = asp_param_log2.numtrab;
                parametros.numtrab1 = asp_param_log1.numtrab;

                parametros.m3aguadesalada6 = asp_param.m3aguadesaladaAnio3;
                parametros.m3aguadesalada5 = asp_param.m3aguadesaladaAnio2;
                parametros.m3aguadesalada4 = asp_param.m3aguadesaladaAnio1;
                parametros.m3aguadesalada3 = asp_param_log3.m3aguadesalada;
                parametros.m3aguadesalada2 = asp_param_log2.m3aguadesalada;
                parametros.m3aguadesalada1 = asp_param_log1.m3aguadesalada;

                parametros.trabcantera6 = asp_param.trabcanteraAnio3;
                parametros.trabcantera5 = asp_param.trabcanteraAnio2;
                parametros.trabcantera4 = asp_param.trabcanteraAnio1;
                parametros.trabcantera3 = asp_param_log3.trabcantera;
                parametros.trabcantera2 = asp_param_log2.trabcantera;
                parametros.trabcantera1 = asp_param_log1.trabcantera;
            }

            if (diferenciaanios == 4)
            {
                aspecto_parametros_log asp_param_log4 = Datos.ObtenerParametrosLogCentral(idCentral, Anio - 2);
                aspecto_parametros_log asp_param_log3 = Datos.ObtenerParametrosLogCentral(idCentral, Anio - 3);
                aspecto_parametros_log asp_param_log2 = Datos.ObtenerParametrosLogCentral(idCentral, Anio - 4);
                aspecto_parametros_log asp_param_log1 = Datos.ObtenerParametrosLogCentral(idCentral, Anio - 5);

                parametros.mwhb6 = asp_param.Mwhb2;
                parametros.mwhb5 = asp_param.Mwhb1;
                parametros.mwhb4 = asp_param_log4.Mwhb;
                parametros.mwhb3 = asp_param_log3.Mwhb;
                parametros.mwhb2 = asp_param_log2.Mwhb;
                parametros.mwhb1 = asp_param_log1.Mwhb;

                parametros.kmanio6 = asp_param.kmAnio2;
                parametros.kmanio5 = asp_param.kmAnio1;
                parametros.kmanio4 = asp_param_log4.kmAnio;
                parametros.kmanio3 = asp_param_log3.kmAnio;
                parametros.kmanio2 = asp_param_log2.kmAnio;
                parametros.kmanio1 = asp_param_log1.kmAnio;

                parametros.m3hora6 = asp_param.m3Hora2;
                parametros.m3hora5 = asp_param.m3Hora1;
                parametros.m3hora4 = asp_param_log4.m3Hora;
                parametros.m3hora3 = asp_param_log3.m3Hora;
                parametros.m3hora2 = asp_param_log2.m3Hora;
                parametros.m3hora1 = asp_param_log1.m3Hora;

                parametros.numtrab6 = asp_param.numtrabAnio2;
                parametros.numtrab5 = asp_param.numtrabAnio1;
                parametros.numtrab4 = asp_param_log4.numtrab;
                parametros.numtrab3 = asp_param_log3.numtrab;
                parametros.numtrab2 = asp_param_log2.numtrab;
                parametros.numtrab1 = asp_param_log1.numtrab;

                parametros.m3aguadesalada6 = asp_param.m3aguadesaladaAnio2;
                parametros.m3aguadesalada5 = asp_param.m3aguadesaladaAnio1;
                parametros.m3aguadesalada4 = asp_param_log4.m3aguadesalada;
                parametros.m3aguadesalada3 = asp_param_log3.m3aguadesalada;
                parametros.m3aguadesalada2 = asp_param_log2.m3aguadesalada;
                parametros.m3aguadesalada1 = asp_param_log1.m3aguadesalada;

                parametros.trabcantera6 = asp_param.trabcanteraAnio2;
                parametros.trabcantera5 = asp_param.trabcanteraAnio1;
                parametros.trabcantera4 = asp_param_log4.trabcantera;
                parametros.trabcantera3 = asp_param_log3.trabcantera;
                parametros.trabcantera2 = asp_param_log2.trabcantera;
                parametros.trabcantera1 = asp_param_log1.trabcantera;
            }

            if (diferenciaanios == 5)
            {
                aspecto_parametros_log asp_param_log5 = Datos.ObtenerParametrosLogCentral(idCentral, Anio - 1);
                aspecto_parametros_log asp_param_log4 = Datos.ObtenerParametrosLogCentral(idCentral, Anio - 2);
                aspecto_parametros_log asp_param_log3 = Datos.ObtenerParametrosLogCentral(idCentral, Anio - 3);
                aspecto_parametros_log asp_param_log2 = Datos.ObtenerParametrosLogCentral(idCentral, Anio - 4);
                aspecto_parametros_log asp_param_log1 = Datos.ObtenerParametrosLogCentral(idCentral, Anio - 5);

                parametros.mwhb6 = asp_param.Mwhb1;
                parametros.mwhb5 = asp_param_log5.Mwhb;
                parametros.mwhb4 = asp_param_log4.Mwhb;
                parametros.mwhb3 = asp_param_log3.Mwhb;
                parametros.mwhb2 = asp_param_log2.Mwhb;
                parametros.mwhb1 = asp_param_log1.Mwhb;

                parametros.kmanio6 = asp_param.kmAnio1;
                parametros.kmanio5 = asp_param_log5.kmAnio;
                parametros.kmanio4 = asp_param_log4.kmAnio;
                parametros.kmanio3 = asp_param_log3.kmAnio;
                parametros.kmanio2 = asp_param_log2.kmAnio;
                parametros.kmanio1 = asp_param_log1.kmAnio;

                parametros.m3hora6 = asp_param.m3Hora1;
                parametros.m3hora5 = asp_param_log5.m3Hora;
                parametros.m3hora4 = asp_param_log4.m3Hora;
                parametros.m3hora3 = asp_param_log3.m3Hora;
                parametros.m3hora2 = asp_param_log2.m3Hora;
                parametros.m3hora1 = asp_param_log1.m3Hora;

                parametros.numtrab6 = asp_param.numtrabAnio1;
                parametros.numtrab5 = asp_param_log5.numtrab;
                parametros.numtrab4 = asp_param_log4.numtrab;
                parametros.numtrab3 = asp_param_log3.numtrab;
                parametros.numtrab2 = asp_param_log2.numtrab;
                parametros.numtrab1 = asp_param_log1.numtrab;

                parametros.m3aguadesalada6 = asp_param.m3aguadesaladaAnio1;
                parametros.m3aguadesalada5 = asp_param_log5.m3aguadesalada;
                parametros.m3aguadesalada4 = asp_param_log4.m3aguadesalada;
                parametros.m3aguadesalada3 = asp_param_log3.m3aguadesalada;
                parametros.m3aguadesalada2 = asp_param_log2.m3aguadesalada;
                parametros.m3aguadesalada1 = asp_param_log1.m3aguadesalada;

                parametros.trabcantera6 = asp_param.trabcanteraAnio1;
                parametros.trabcantera5 = asp_param_log5.trabcantera;
                parametros.trabcantera4 = asp_param_log4.trabcantera;
                parametros.trabcantera3 = asp_param_log3.trabcantera;
                parametros.trabcantera2 = asp_param_log2.trabcantera;
                parametros.trabcantera1 = asp_param_log1.trabcantera;
            }

            if (diferenciaanios == 6)
            {
                aspecto_parametros_log asp_param_log6 = Datos.ObtenerParametrosLogCentral(idCentral, Anio);
                aspecto_parametros_log asp_param_log5 = Datos.ObtenerParametrosLogCentral(idCentral, Anio - 1);
                aspecto_parametros_log asp_param_log4 = Datos.ObtenerParametrosLogCentral(idCentral, Anio - 2);
                aspecto_parametros_log asp_param_log3 = Datos.ObtenerParametrosLogCentral(idCentral, Anio - 3);
                aspecto_parametros_log asp_param_log2 = Datos.ObtenerParametrosLogCentral(idCentral, Anio - 4);
                aspecto_parametros_log asp_param_log1 = Datos.ObtenerParametrosLogCentral(idCentral, Anio - 5);

                parametros.mwhb6 = asp_param_log6.Mwhb;
                parametros.mwhb5 = asp_param_log5.Mwhb;
                parametros.mwhb4 = asp_param_log4.Mwhb;
                parametros.mwhb3 = asp_param_log3.Mwhb;
                parametros.mwhb2 = asp_param_log2.Mwhb;
                parametros.mwhb1 = asp_param_log1.Mwhb;

                parametros.kmanio6 = asp_param_log6.kmAnio;
                parametros.kmanio5 = asp_param_log5.kmAnio;
                parametros.kmanio4 = asp_param_log4.kmAnio;
                parametros.kmanio3 = asp_param_log3.kmAnio;
                parametros.kmanio2 = asp_param_log2.kmAnio;
                parametros.kmanio1 = asp_param_log1.kmAnio;

                parametros.m3hora6 = asp_param_log6.m3Hora;
                parametros.m3hora5 = asp_param_log5.m3Hora;
                parametros.m3hora4 = asp_param_log4.m3Hora;
                parametros.m3hora3 = asp_param_log3.m3Hora;
                parametros.m3hora2 = asp_param_log2.m3Hora;
                parametros.m3hora1 = asp_param_log1.m3Hora;

                parametros.numtrab6 = asp_param_log6.numtrab;
                parametros.numtrab5 = asp_param_log5.numtrab;
                parametros.numtrab4 = asp_param_log4.numtrab;
                parametros.numtrab3 = asp_param_log3.numtrab;
                parametros.numtrab2 = asp_param_log2.numtrab;
                parametros.numtrab1 = asp_param_log1.numtrab;

                parametros.m3aguadesalada6 = asp_param_log6.m3aguadesalada;
                parametros.m3aguadesalada5 = asp_param_log5.m3aguadesalada;
                parametros.m3aguadesalada4 = asp_param_log4.m3aguadesalada;
                parametros.m3aguadesalada3 = asp_param_log3.m3aguadesalada;
                parametros.m3aguadesalada2 = asp_param_log2.m3aguadesalada;
                parametros.m3aguadesalada1 = asp_param_log1.m3aguadesalada;

                parametros.trabcantera6 = asp_param_log6.trabcantera;
                parametros.trabcantera5 = asp_param_log5.trabcantera;
                parametros.trabcantera4 = asp_param_log4.trabcantera;
                parametros.trabcantera3 = asp_param_log3.trabcantera;
                parametros.trabcantera2 = asp_param_log2.trabcantera;
                parametros.trabcantera1 = asp_param_log1.trabcantera;
            }

            return parametros;
        }

        private static WorksheetPart
             GetWorksheetPartByName(SpreadsheetDocument document,
             string sheetName)
        {
            IEnumerable<Sheet> sheets =
               document.WorkbookPart.Workbook.GetFirstChild<Sheets>().
               Elements<Sheet>().Where(s => s.Name == sheetName);

            if (sheets.Count() == 0)
            {
                // The specified worksheet does not exist.

                return null;
            }

            string relationshipId = sheets.First().Id.Value;
            WorksheetPart worksheetPart = (WorksheetPart)
                 document.WorkbookPart.GetPartById(relationshipId);
            return worksheetPart;

        }

        private static Cell GetCell(Worksheet worksheet,
                  string columnName, uint rowIndex)
        {
            Row row = GetRow(worksheet, rowIndex);

            if (row == null)
                return null;

            return row.Elements<Cell>().Where(c => string.Compare
                   (c.CellReference.Value, columnName +
                   rowIndex, true) == 0).First();
        }

        private static Row GetRow(Worksheet worksheet, uint rowIndex)
        {
            return worksheet.GetFirstChild<SheetData>().
              Elements<Row>().Where(r => r.RowIndex == rowIndex).First();
        }

        public ActionResult aspecto_parametros(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            int idCentral = centroseleccionado.id;

            aspecto_parametro_valoracion buscarParametro = Datos.ObtenerParametroAspecto(id);
            ViewData["valoracionparametro"] = buscarParametro;
            aspecto_valoracion buscarValoracion = Datos.ObtenerValoracionAspecto(buscarParametro.id_aspecto);
            ViewData["valoracionaspecto"] = buscarValoracion;
            aspecto_tipo buscarTipoAspecto = Datos.ObtenerTipoAspecto(int.Parse(buscarValoracion.idAspecto.ToString()));
            ViewData["tipoaspecto"] = buscarTipoAspecto;
            aspecto_parametros buscarParametros = Datos.ObtenerParametrosCentral(idCentral);
            ViewData["EditarParametros"] = buscarParametros;

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult aspecto_parametros(int id, FormCollection collection)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            string formulario = collection["hdFormularioEjecutado"];

            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            int idCentral = centroseleccionado.id;

            if (formulario == "GuardarAspectoValoracion")
            {
                #region Guardar Aspecto
                try
                {      
                    aspecto_parametro_valoracion valoracion = Datos.ObtenerParametroAspecto(id);

                    if (collection["ctl00$MainContent$txtObservaciones"] != null)
                        valoracion.observaciones = collection["ctl00$MainContent$txtObservaciones"].ToString();

                    #region mediciones
                    if (collection["ctl00$MainContent$txtReferencia"] != null && collection["ctl00$MainContent$txtReferencia"] != string.Empty)
                        valoracion.referencia = decimal.Parse(collection["ctl00$MainContent$txtReferencia"].ToString().Replace(".", ","));
                    else
                        valoracion.referencia = 0;

                    if (collection["ctl00$MainContent$txtRefSuperior"] != null && collection["ctl00$MainContent$txtRefSuperior"] != string.Empty)
                        valoracion.referenciasup = decimal.Parse(collection["ctl00$MainContent$txtRefSuperior"].ToString().Replace(".", ","));
                    else
                        valoracion.referenciasup = 0;

                    #region valores mensuales

                    decimal mayorvalor = -1;

                    if (collection["ctl00$MainContent$txtEne"] != null && collection["ctl00$MainContent$txtEne"] != string.Empty)
                    {
                        valoracion.mes1 = decimal.Parse(collection["ctl00$MainContent$txtEne"].ToString().Replace(".", ","));
                        if (valoracion.mes1 > mayorvalor)
                            mayorvalor = decimal.Parse(valoracion.mes1.ToString());
                    }
                    else
                        valoracion.mes1 = null;

                    if (collection["ctl00$MainContent$txtFeb"] != null && collection["ctl00$MainContent$txtFeb"] != string.Empty)
                    {
                        valoracion.mes2 = decimal.Parse(collection["ctl00$MainContent$txtFeb"].ToString().Replace(".", ","));
                        if (valoracion.mes2 > mayorvalor)
                            mayorvalor = decimal.Parse(valoracion.mes2.ToString());
                    }
                    else
                        valoracion.mes2 = null;

                    if (collection["ctl00$MainContent$txtMar"] != null && collection["ctl00$MainContent$txtMar"] != string.Empty)
                    {
                        valoracion.mes3 = decimal.Parse(collection["ctl00$MainContent$txtMar"].ToString().Replace(".", ","));
                        if (valoracion.mes3 > mayorvalor)
                            mayorvalor = decimal.Parse(valoracion.mes3.ToString());
                    }
                    else
                        valoracion.mes3 = null;

                    if (collection["ctl00$MainContent$txtAbr"] != null && collection["ctl00$MainContent$txtAbr"] != string.Empty)
                    {
                        valoracion.mes4 = decimal.Parse(collection["ctl00$MainContent$txtAbr"].ToString().Replace(".", ","));
                        if (valoracion.mes4 > mayorvalor)
                            mayorvalor = decimal.Parse(valoracion.mes4.ToString());
                    }
                    else
                        valoracion.mes4 = null;

                    if (collection["ctl00$MainContent$txtMay"] != null && collection["ctl00$MainContent$txtMay"] != string.Empty)
                    {
                        valoracion.mes5 = decimal.Parse(collection["ctl00$MainContent$txtMay"].ToString().Replace(".", ","));
                        if (valoracion.mes5 > mayorvalor)
                            mayorvalor = decimal.Parse(valoracion.mes5.ToString());
                    }
                    else
                        valoracion.mes5 = null;

                    if (collection["ctl00$MainContent$txtJun"] != null && collection["ctl00$MainContent$txtJun"] != string.Empty)
                    {
                        valoracion.mes6 = decimal.Parse(collection["ctl00$MainContent$txtJun"].ToString().Replace(".", ","));
                        if (valoracion.mes6 > mayorvalor)
                            mayorvalor = decimal.Parse(valoracion.mes6.ToString());
                    }
                    else
                        valoracion.mes6 = null;

                    if (collection["ctl00$MainContent$txtJul"] != null && collection["ctl00$MainContent$txtJul"] != string.Empty)
                    {
                        valoracion.mes7 = decimal.Parse(collection["ctl00$MainContent$txtJul"].ToString().Replace(".", ","));
                        if (valoracion.mes7 > mayorvalor)
                            mayorvalor = decimal.Parse(valoracion.mes7.ToString());
                    }
                    else
                        valoracion.mes7 = null;

                    if (collection["ctl00$MainContent$txtAgo"] != null && collection["ctl00$MainContent$txtAgo"] != string.Empty)
                    {
                        valoracion.mes8 = decimal.Parse(collection["ctl00$MainContent$txtAgo"].ToString().Replace(".", ","));
                        if (valoracion.mes8 > mayorvalor)
                            mayorvalor = decimal.Parse(valoracion.mes8.ToString());
                    }
                    else
                        valoracion.mes8 = null;

                    if (collection["ctl00$MainContent$txtSep"] != null && collection["ctl00$MainContent$txtSep"] != string.Empty)
                    {
                        valoracion.mes9 = decimal.Parse(collection["ctl00$MainContent$txtSep"].ToString().Replace(".", ","));
                        if (valoracion.mes9 > mayorvalor)
                            mayorvalor = decimal.Parse(valoracion.mes9.ToString());
                    }
                    else
                        valoracion.mes9 = null;

                    if (collection["ctl00$MainContent$txtOct"] != null && collection["ctl00$MainContent$txtOct"] != string.Empty)
                    {
                        valoracion.mes10 = decimal.Parse(collection["ctl00$MainContent$txtOct"].ToString().Replace(".", ","));
                        if (valoracion.mes10 > mayorvalor)
                            mayorvalor = decimal.Parse(valoracion.mes10.ToString());
                    }
                    else
                        valoracion.mes10 = null;

                    if (collection["ctl00$MainContent$txtNov"] != null && collection["ctl00$MainContent$txtNov"] != string.Empty)
                    {
                        valoracion.mes11 = decimal.Parse(collection["ctl00$MainContent$txtNov"].ToString().Replace(".", ","));
                        if (valoracion.mes11 > mayorvalor)
                            mayorvalor = decimal.Parse(valoracion.mes11.ToString());
                    }
                    else
                        valoracion.mes11 = null;

                    if (collection["ctl00$MainContent$txtDic"] != null && collection["ctl00$MainContent$txtDic"] != string.Empty)
                    {
                        valoracion.mes12 = decimal.Parse(collection["ctl00$MainContent$txtDic"].ToString().Replace(".", ","));
                        if (valoracion.mes12 > mayorvalor)
                            mayorvalor = decimal.Parse(valoracion.mes12.ToString());
                    }
                    else
                        valoracion.mes12 = null;
                    #endregion
                    
                    if (collection["ctl00$MainContent$txtMed1"] != null && collection["ctl00$MainContent$txtMed1"] != string.Empty)
                        valoracion.anio1 = decimal.Parse(collection["ctl00$MainContent$txtMed1"].ToString().Replace(".", ","));
                    else
                        valoracion.anio1 = null;
                    if (collection["ctl00$MainContent$txtMed2"] != null && collection["ctl00$MainContent$txtMed2"] != string.Empty)
                        valoracion.anio2 = decimal.Parse(collection["ctl00$MainContent$txtMed2"].ToString().Replace(".", ","));
                    else
                        valoracion.anio2 = null;
                    if (collection["ctl00$MainContent$txtMed3"] != null && collection["ctl00$MainContent$txtMed3"] != string.Empty)
                        valoracion.anio3 = decimal.Parse(collection["ctl00$MainContent$txtMed3"].ToString().Replace(".", ","));
                    else
                        valoracion.anio3 = null;
                    if (collection["ctl00$MainContent$txtMed4"] != null && collection["ctl00$MainContent$txtMed4"] != string.Empty)
                        valoracion.anio4 = decimal.Parse(collection["ctl00$MainContent$txtMed4"].ToString().Replace(".", ","));
                    else
                        valoracion.anio4 = null;
                    if (collection["ctl00$MainContent$txtMed5"] != null && collection["ctl00$MainContent$txtMed5"] != string.Empty)
                        valoracion.anio5 = decimal.Parse(collection["ctl00$MainContent$txtMed5"].ToString().Replace(".", ","));
                    else
                        valoracion.anio5 = null;

                    if (collection["ctl00$MainContent$txtMed6"] != null && collection["ctl00$MainContent$txtMed6"] != string.Empty)
                        valoracion.anio6 = decimal.Parse(collection["ctl00$MainContent$txtMed6"].ToString().Replace(".", ","));
                    else
                        valoracion.anio6 = null;


                    #endregion

                    aspecto_parametros Parametros = Datos.ObtenerParametrosCentral(idCentral);
                    aspecto_valoracion buscarValoracion = Datos.ObtenerValoracionAspecto(valoracion.id_aspecto);
                    aspecto_tipo TipoAspecto = Datos.ObtenerTipoAspecto(int.Parse(buscarValoracion.idAspecto.ToString()));

                    #region valoresrelativizar
                    if (TipoAspecto.Grupo == 13 || TipoAspecto.Grupo == 17 || TipoAspecto.Grupo == 18 || TipoAspecto.Grupo == 19 || TipoAspecto.Grupo == 22 || TipoAspecto.Grupo == 23 || TipoAspecto.Grupo == 24)
                    {
                        valoracion.magnitud = int.Parse(collection["ctl00$MainContent$ddlMagnitud"].ToString());
                    }
                    if (TipoAspecto.Grupo == 2 || TipoAspecto.Grupo == 3 || TipoAspecto.Grupo == 4 || TipoAspecto.Grupo == 5 || TipoAspecto.Grupo == 7 || TipoAspecto.Grupo == 8 || TipoAspecto.Grupo == 9 || TipoAspecto.Grupo == 10 || TipoAspecto.Grupo == 11 || TipoAspecto.Grupo == 12 || TipoAspecto.Grupo == 15 || TipoAspecto.Grupo == 16 || TipoAspecto.Grupo == 17 || TipoAspecto.Grupo == 18 || TipoAspecto.Grupo == 19 || TipoAspecto.Grupo == 22 || TipoAspecto.Grupo == 23 || TipoAspecto.Grupo == 24)
                    {
                        valoracion.naturaleza = int.Parse(collection["ctl00$MainContent$ddlNaturaleza"].ToString());
                    }

                    if (TipoAspecto.Grupo == 1 || TipoAspecto.Grupo == 2 || TipoAspecto.Grupo == 3 || TipoAspecto.Grupo == 4 || TipoAspecto.Grupo == 5 || TipoAspecto.Grupo == 6 || TipoAspecto.Grupo == 7 || TipoAspecto.Grupo == 8 || TipoAspecto.Grupo == 13 || TipoAspecto.Grupo == 14 || TipoAspecto.Grupo == 16 || TipoAspecto.Grupo == 17 || TipoAspecto.Grupo == 22 || TipoAspecto.Grupo == 23 || TipoAspecto.Grupo == 24)
                    {
                        valoracion.origen = int.Parse(collection["ctl00$MainContent$ddlOrigen"].ToString());
                    }

                    #endregion

                    #region calculos
                    decimal valorrelativoAnio1 = -1;
                    decimal valorrelativoAnio2 = -1;
                    decimal valorrelativoAnio3 = -1;
                    decimal valorrelativoAnio4 = -1;
                    decimal valorrelativoAnio5 = -1;
                    decimal valorrelativoAnio6 = -1;
                    #region relativoMwhb
                    if (TipoAspecto.relativoMwhb == true)
                    {
                        if (valoracion.anio1 != null && Parametros.Mwhb1 != null)
                            valorrelativoAnio1 = decimal.Parse(valoracion.anio1.ToString()) / decimal.Parse(Parametros.Mwhb1.ToString());

                        if (valoracion.anio2 != null && Parametros.Mwhb2 != null)
                            valorrelativoAnio2 = decimal.Parse(valoracion.anio2.ToString()) / decimal.Parse(Parametros.Mwhb2.ToString());

                        if (valoracion.anio3 != null && Parametros.Mwhb3 != null)
                            valorrelativoAnio3 = decimal.Parse(valoracion.anio3.ToString()) / decimal.Parse(Parametros.Mwhb3.ToString());

                        if (valoracion.anio4 != null && Parametros.Mwhb4 != null)
                            valorrelativoAnio4 = decimal.Parse(valoracion.anio4.ToString()) / decimal.Parse(Parametros.Mwhb4.ToString());

                        if (valoracion.anio5 != null && Parametros.Mwhb5 != null)
                            valorrelativoAnio5 = decimal.Parse(valoracion.anio5.ToString()) / decimal.Parse(Parametros.Mwhb5.ToString());

                        if (valoracion.anio6 != null && Parametros.Mwhb6 != null)
                            valorrelativoAnio6 = decimal.Parse(valoracion.anio6.ToString()) / decimal.Parse(Parametros.Mwhb6.ToString());
                    }
                    #endregion
                    #region relativom3Hora
                    if (TipoAspecto.relativom3Hora == true)
                    {
                        if (valoracion.anio1 != null && Parametros.m3Hora1 != null)
                            valorrelativoAnio1 = decimal.Parse(valoracion.anio1.ToString()) / decimal.Parse(Parametros.m3Hora1.ToString());

                        if (valoracion.anio2 != null && Parametros.m3Hora2 != null)
                            valorrelativoAnio2 = decimal.Parse(valoracion.anio2.ToString()) / decimal.Parse(Parametros.m3Hora2.ToString());

                        if (valoracion.anio3 != null && Parametros.m3Hora3 != null)
                            valorrelativoAnio3 = decimal.Parse(valoracion.anio3.ToString()) / decimal.Parse(Parametros.m3Hora3.ToString());

                        if (valoracion.anio4 != null && Parametros.m3Hora4 != null)
                            valorrelativoAnio4 = decimal.Parse(valoracion.anio4.ToString()) / decimal.Parse(Parametros.m3Hora4.ToString());

                        if (valoracion.anio5 != null && Parametros.m3Hora5 != null)
                            valorrelativoAnio5 = decimal.Parse(valoracion.anio5.ToString()) / decimal.Parse(Parametros.m3Hora5.ToString());

                        if (valoracion.anio6 != null && Parametros.m3Hora6 != null)
                            valorrelativoAnio6 = decimal.Parse(valoracion.anio6.ToString()) / decimal.Parse(Parametros.m3Hora6.ToString());
                    }
                    #endregion
                    #region relativokmAnio
                    if (TipoAspecto.relativokmAnio == true)
                    {
                        if (valoracion.anio1 != null && Parametros.kmAnio1 != null)
                            valorrelativoAnio1 = decimal.Parse(valoracion.anio1.ToString()) / decimal.Parse(Parametros.kmAnio1.ToString());

                        if (valoracion.anio2 != null && Parametros.kmAnio2 != null)
                            valorrelativoAnio2 = decimal.Parse(valoracion.anio2.ToString()) / decimal.Parse(Parametros.kmAnio2.ToString());

                        if (valoracion.anio3 != null && Parametros.kmAnio3 != null)
                            valorrelativoAnio3 = decimal.Parse(valoracion.anio3.ToString()) / decimal.Parse(Parametros.kmAnio3.ToString());

                        if (valoracion.anio4 != null && Parametros.kmAnio4 != null)
                            valorrelativoAnio4 = decimal.Parse(valoracion.anio4.ToString()) / decimal.Parse(Parametros.kmAnio4.ToString());

                        if (valoracion.anio5 != null && Parametros.kmAnio5 != null)
                            valorrelativoAnio5 = decimal.Parse(valoracion.anio5.ToString()) / decimal.Parse(Parametros.kmAnio5.ToString());

                        if (valoracion.anio6 != null && Parametros.kmAnio6 != null)
                            valorrelativoAnio6 = decimal.Parse(valoracion.anio6.ToString()) / decimal.Parse(Parametros.kmAnio6.ToString());
                    }
                    #endregion
                    #region relativohfuncAnio
                    if (TipoAspecto.relativohfuncAnio == true)
                    {
                        if (valoracion.anio1 != null && Parametros.hfuncAnio1 != null)
                            valorrelativoAnio1 = decimal.Parse(valoracion.anio1.ToString()) / decimal.Parse(Parametros.hfuncAnio1.ToString());

                        if (valoracion.anio2 != null && Parametros.hfuncAnio2 != null)
                            valorrelativoAnio2 = decimal.Parse(valoracion.anio2.ToString()) / decimal.Parse(Parametros.hfuncAnio2.ToString());

                        if (valoracion.anio3 != null && Parametros.hfuncAnio3 != null)
                            valorrelativoAnio3 = decimal.Parse(valoracion.anio3.ToString()) / decimal.Parse(Parametros.hfuncAnio3.ToString());

                        if (valoracion.anio4 != null && Parametros.hfuncAnio4 != null)
                            valorrelativoAnio4 = decimal.Parse(valoracion.anio4.ToString()) / decimal.Parse(Parametros.hfuncAnio4.ToString());

                        if (valoracion.anio5 != null && Parametros.hfuncAnio5 != null)
                            valorrelativoAnio5 = decimal.Parse(valoracion.anio5.ToString()) / decimal.Parse(Parametros.hfuncAnio5.ToString());

                        if (valoracion.anio6 != null && Parametros.hfuncAnio6 != null)
                            valorrelativoAnio6 = decimal.Parse(valoracion.anio6.ToString()) / decimal.Parse(Parametros.hfuncAnio6.ToString());
                    }
                    #endregion
                    #region relativohAnioGE
                    if (TipoAspecto.relativohAnioGE == true)
                    {
                        if (valoracion.anio1 != null && Parametros.hAnioGE1 != null)
                            valorrelativoAnio1 = decimal.Parse(valoracion.anio1.ToString()) / decimal.Parse(Parametros.hAnioGE1.ToString());

                        if (valoracion.anio2 != null && Parametros.hAnioGE2 != null)
                            valorrelativoAnio2 = decimal.Parse(valoracion.anio2.ToString()) / decimal.Parse(Parametros.hAnioGE2.ToString());

                        if (valoracion.anio3 != null && Parametros.hAnioGE3 != null)
                            valorrelativoAnio3 = decimal.Parse(valoracion.anio3.ToString()) / decimal.Parse(Parametros.hAnioGE3.ToString());

                        if (valoracion.anio4 != null && Parametros.hAnioGE4 != null)
                            valorrelativoAnio4 = decimal.Parse(valoracion.anio4.ToString()) / decimal.Parse(Parametros.hAnioGE4.ToString());

                        if (valoracion.anio5 != null && Parametros.hAnioGE5 != null)
                            valorrelativoAnio5 = decimal.Parse(valoracion.anio5.ToString()) / decimal.Parse(Parametros.hAnioGE5.ToString());

                        if (valoracion.anio6 != null && Parametros.hAnioGE6 != null)
                            valorrelativoAnio6 = decimal.Parse(valoracion.anio6.ToString()) / decimal.Parse(Parametros.hAnioGE6.ToString());
                    }
                    #endregion

                    #region relativoNumTrabajadores
                    if (TipoAspecto.relativonumtrab == true)
                    {
                        if (valoracion.anio1 != null && Parametros.numtrabAnio1 != null)
                            valorrelativoAnio1 = decimal.Parse(valoracion.anio1.ToString()) / decimal.Parse(Parametros.numtrabAnio1.ToString());

                        if (valoracion.anio2 != null && Parametros.numtrabAnio2 != null)
                            valorrelativoAnio2 = decimal.Parse(valoracion.anio2.ToString()) / decimal.Parse(Parametros.numtrabAnio2.ToString());

                        if (valoracion.anio3 != null && Parametros.numtrabAnio3 != null)
                            valorrelativoAnio3 = decimal.Parse(valoracion.anio3.ToString()) / decimal.Parse(Parametros.numtrabAnio3.ToString());

                        if (valoracion.anio4 != null && Parametros.numtrabAnio4 != null)
                            valorrelativoAnio4 = decimal.Parse(valoracion.anio4.ToString()) / decimal.Parse(Parametros.numtrabAnio4.ToString());

                        if (valoracion.anio5 != null && Parametros.numtrabAnio5 != null)
                            valorrelativoAnio5 = decimal.Parse(valoracion.anio5.ToString()) / decimal.Parse(Parametros.numtrabAnio5.ToString());

                        if (valoracion.anio6 != null && Parametros.numtrabAnio6 != null)
                            valorrelativoAnio6 = decimal.Parse(valoracion.anio6.ToString()) / decimal.Parse(Parametros.numtrabAnio6.ToString());
                    }
                    #endregion

                    #region relativoAguaDesalada
                    if (TipoAspecto.relativom3aguadeitsalada == true)
                    {
                        if (valoracion.anio1 != null && Parametros.m3aguadesaladaAnio1 != null)
                            valorrelativoAnio1 = decimal.Parse(valoracion.anio1.ToString()) / decimal.Parse(Parametros.m3aguadesaladaAnio1.ToString());

                        if (valoracion.anio2 != null && Parametros.m3aguadesaladaAnio2 != null)
                            valorrelativoAnio2 = decimal.Parse(valoracion.anio2.ToString()) / decimal.Parse(Parametros.m3aguadesaladaAnio2.ToString());

                        if (valoracion.anio3 != null && Parametros.m3aguadesaladaAnio3 != null)
                            valorrelativoAnio3 = decimal.Parse(valoracion.anio3.ToString()) / decimal.Parse(Parametros.m3aguadesaladaAnio3.ToString());

                        if (valoracion.anio4 != null && Parametros.m3aguadesaladaAnio4 != null)
                            valorrelativoAnio4 = decimal.Parse(valoracion.anio4.ToString()) / decimal.Parse(Parametros.m3aguadesaladaAnio4.ToString());

                        if (valoracion.anio5 != null && Parametros.m3aguadesaladaAnio5 != null)
                            valorrelativoAnio5 = decimal.Parse(valoracion.anio5.ToString()) / decimal.Parse(Parametros.m3aguadesaladaAnio5.ToString());

                        if (valoracion.anio6 != null && Parametros.m3aguadesaladaAnio6 != null)
                            valorrelativoAnio6 = decimal.Parse(valoracion.anio6.ToString()) / decimal.Parse(Parametros.m3aguadesaladaAnio6.ToString());
                    }
                    #endregion

                    #region relativoTrabCantera
                    if (TipoAspecto.relativotrabcantera == true)
                    {
                        if (valoracion.anio1 != null && Parametros.trabcanteraAnio1 != null)
                            valorrelativoAnio1 = decimal.Parse(valoracion.anio1.ToString()) / decimal.Parse(Parametros.trabcanteraAnio1.ToString());

                        if (valoracion.anio2 != null && Parametros.trabcanteraAnio2 != null)
                            valorrelativoAnio2 = decimal.Parse(valoracion.anio2.ToString()) / decimal.Parse(Parametros.trabcanteraAnio2.ToString());

                        if (valoracion.anio3 != null && Parametros.trabcanteraAnio3 != null)
                            valorrelativoAnio3 = decimal.Parse(valoracion.anio3.ToString()) / decimal.Parse(Parametros.trabcanteraAnio3.ToString());

                        if (valoracion.anio4 != null && Parametros.trabcanteraAnio4 != null)
                            valorrelativoAnio4 = decimal.Parse(valoracion.anio4.ToString()) / decimal.Parse(Parametros.trabcanteraAnio4.ToString());

                        if (valoracion.anio5 != null && Parametros.trabcanteraAnio5 != null)
                            valorrelativoAnio5 = decimal.Parse(valoracion.anio5.ToString()) / decimal.Parse(Parametros.trabcanteraAnio5.ToString());

                        if (valoracion.anio6 != null && Parametros.trabcanteraAnio6 != null)
                            valorrelativoAnio6 = decimal.Parse(valoracion.anio6.ToString()) / decimal.Parse(Parametros.trabcanteraAnio6.ToString());
                    }
                    #endregion

                    #region norelativo

                    if (TipoAspecto.relativono == true)
                    {
                        if (valoracion.anio1 != null)
                            valorrelativoAnio1 = decimal.Parse(valoracion.anio1.ToString());

                        if (valoracion.anio2 != null)
                            valorrelativoAnio2 = decimal.Parse(valoracion.anio2.ToString());

                        if (valoracion.anio3 != null)
                            valorrelativoAnio3 = decimal.Parse(valoracion.anio3.ToString());

                        if (valoracion.anio4 != null)
                            valorrelativoAnio4 = decimal.Parse(valoracion.anio4.ToString());

                        if (valoracion.anio5 != null)
                            valorrelativoAnio5 = decimal.Parse(valoracion.anio5.ToString());

                        if (valoracion.anio6 != null)
                            valorrelativoAnio6 = decimal.Parse(valoracion.anio6.ToString());

                    }
                    #endregion

                    decimal sumavaloresrelativos = 0;
                    int cantidadvalores = 0;

                    if (valorrelativoAnio1 != -1)
                    {
                        sumavaloresrelativos = sumavaloresrelativos + valorrelativoAnio1;
                        cantidadvalores++;
                    }

                    if (valorrelativoAnio2 != -1)
                    {
                        sumavaloresrelativos = sumavaloresrelativos + valorrelativoAnio2;
                        cantidadvalores++;
                    }

                    if (valorrelativoAnio3 != -1)
                    {
                        sumavaloresrelativos = sumavaloresrelativos + valorrelativoAnio3;
                        cantidadvalores++;
                    }

                    if (valorrelativoAnio4 != -1)
                    {
                        sumavaloresrelativos = sumavaloresrelativos + valorrelativoAnio4;
                        cantidadvalores++;
                    }

                    if (valorrelativoAnio5 != -1)
                    {
                        sumavaloresrelativos = sumavaloresrelativos + valorrelativoAnio5;
                        cantidadvalores++;
                    }

                    decimal valorpromediorelativoaniosanteriores = sumavaloresrelativos / cantidadvalores;

                    //Variación
                    if (TipoAspecto.Grupo == 1 || TipoAspecto.Grupo == 2 || TipoAspecto.Grupo == 3 || TipoAspecto.Grupo == 4 || TipoAspecto.Grupo == 5 || TipoAspecto.Grupo == 6 || TipoAspecto.Grupo == 7 || TipoAspecto.Grupo == 8 || TipoAspecto.Grupo == 9 || TipoAspecto.Grupo == 10 || TipoAspecto.Grupo == 11 || TipoAspecto.Grupo == 12 || TipoAspecto.Grupo == 14 || TipoAspecto.Grupo == 15 || TipoAspecto.Grupo == 16 || TipoAspecto.Grupo == 20 || TipoAspecto.Grupo == 21)
                    {
                        if (valorrelativoAnio6 == 0)
                            valoracion.variacion = 0;
                        else
                        {
                            if (valorpromediorelativoaniosanteriores == 0)
                                valoracion.variacion = 100;
                            else
                                valoracion.variacion = ((valorrelativoAnio6 * 100) / valorpromediorelativoaniosanteriores) - 100;
                        }
                        
                    }
                    //Acercamiento

                    if (TipoAspecto.Grupo == 13 || TipoAspecto.Grupo == 14)
                    {
                        #region acercamientoruidoexterior
                        decimal diferenciaDia = 30000;
                        decimal diferenciaTarde = 30000;
                        decimal diferenciaNoche = 30000;


                        if ((collection["ctl00$MainContent$txtDia"] != null && collection["ctl00$MainContent$txtDia"] != string.Empty) && (collection["ctl00$MainContent$txtRefDia"] != null && collection["ctl00$MainContent$txtRefDia"] != string.Empty))
                        {
                            valoracion.RU_Dia = decimal.Parse(collection["ctl00$MainContent$txtDia"].Replace(".", ","));
                            valoracion.RU_DiaRef = decimal.Parse(collection["ctl00$MainContent$txtRefDia"].Replace(".", ","));
                            diferenciaDia = decimal.Parse(valoracion.RU_DiaRef.ToString()) - decimal.Parse(valoracion.RU_Dia.ToString());
                        }
                        else
                        {
                            valoracion.RU_Dia = null;
                            valoracion.RU_DiaRef = null;
                        }
                        if ((collection["ctl00$MainContent$txtTarde"] != null && collection["ctl00$MainContent$txtTarde"] != string.Empty) && (collection["ctl00$MainContent$txtRefTarde"] != null && collection["ctl00$MainContent$txtRefTarde"] != string.Empty))
                        {
                            valoracion.RU_Tarde = decimal.Parse(collection["ctl00$MainContent$txtTarde"].Replace(".", ","));
                            valoracion.RU_TardeRef = decimal.Parse(collection["ctl00$MainContent$txtRefTarde"].Replace(".", ","));
                            diferenciaTarde = decimal.Parse(valoracion.RU_TardeRef.ToString()) - decimal.Parse(valoracion.RU_Tarde.ToString());
                        }
                        else
                        {
                            valoracion.RU_Tarde = null;
                            valoracion.RU_TardeRef = null;
                        }
                        if ((collection["ctl00$MainContent$txtNoche"] != null && collection["ctl00$MainContent$txtNoche"] != string.Empty) && (collection["ctl00$MainContent$txtRefNoche"] != null && collection["ctl00$MainContent$txtRefNoche"] != string.Empty))
                        {
                            valoracion.RU_Noche = decimal.Parse(collection["ctl00$MainContent$txtNoche"].Replace(".", ","));
                            valoracion.RU_NocheRef = decimal.Parse(collection["ctl00$MainContent$txtRefNoche"].Replace(".", ","));
                            diferenciaNoche = decimal.Parse(valoracion.RU_NocheRef.ToString()) - decimal.Parse(valoracion.RU_Noche.ToString());
                        }
                        else
                        {
                            valoracion.RU_Noche = null;
                            valoracion.RU_NocheRef = null;
                        }
                        if ((diferenciaDia <= diferenciaTarde) && (diferenciaDia <= diferenciaNoche) && diferenciaDia != 30000)
                        {
                            valoracion.acercamiento = diferenciaDia;
                        }
                        if ((diferenciaTarde <= diferenciaDia) && (diferenciaTarde <= diferenciaNoche) && diferenciaTarde != 30000)
                        {
                            valoracion.acercamiento = diferenciaTarde;
                        }
                        if ((diferenciaNoche <= diferenciaDia) && (diferenciaNoche <= diferenciaTarde) && diferenciaNoche != 30000)
                        {
                            valoracion.acercamiento = diferenciaNoche;
                        }
                        #endregion
                    }
                    if (TipoAspecto.Grupo == 1 || TipoAspecto.Grupo == 2 || TipoAspecto.Grupo == 3 || TipoAspecto.Grupo == 4 || TipoAspecto.Grupo == 5 || TipoAspecto.Grupo == 6 || TipoAspecto.Grupo == 7 || TipoAspecto.Grupo == 8 || TipoAspecto.Grupo == 9 || TipoAspecto.Grupo == 10 || TipoAspecto.Grupo == 11 || TipoAspecto.Grupo == 12 || TipoAspecto.Grupo == 14 || TipoAspecto.Grupo == 15 || TipoAspecto.Grupo == 16 || TipoAspecto.Grupo == 20 || TipoAspecto.Grupo == 21)
                    {


                        if (TipoAspecto.Grupo == 6 && valoracion.nombre == "PH")
                        {
                            if (mayorvalor > 0)
                            {
                                if (mayorvalor < valoracion.referencia)
                                {
                                    valoracion.acercamiento = (mayorvalor * 100 / valoracion.referencia);
                                }
                                if (mayorvalor > valoracion.referenciasup)
                                {
                                    valoracion.acercamiento = (mayorvalor * 100 / valoracion.referenciasup);
                                }
                                if ((valoracion.referencia != null && valoracion.referenciasup != null) && (mayorvalor < valoracion.referenciasup && mayorvalor > valoracion.referencia))
                                {
                                    decimal datocantidad = decimal.Parse(mayorvalor.ToString());
                                    decimal valorinf = decimal.Parse(valoracion.referencia.ToString());
                                    decimal valorsup = decimal.Parse(valoracion.referenciasup.ToString());
                                    valoracion.acercamiento = Math.Abs((((valorinf + valorsup) / 2) - datocantidad) * 100) / (((valorinf + valorsup) / 2) - valorinf);
                                }
                            }
                            else
                            {
                                if (mayorvalor < valoracion.referencia)
                                {
                                    valoracion.acercamiento = (mayorvalor * 100 / valoracion.referencia);
                                }
                                if (mayorvalor > valoracion.referenciasup)
                                {
                                    valoracion.acercamiento = (mayorvalor * 100 / valoracion.referencia);
                                }
                                if (mayorvalor < valoracion.referenciasup && mayorvalor > valoracion.referencia)
                                {
                                    valoracion.acercamiento = 0;
                                }
                            }

                            
                        }
                        else
                        {
                            if (valoracion.referencia != null && valoracion.referencia != 0 && mayorvalor >= 0)
                            {
                                valoracion.acercamiento = (mayorvalor * 100 / valoracion.referencia);
                            }
                            else
                            {
                                if (valoracion.referencia != null && valoracion.anio6 != null && valoracion.referencia != 0)
                                {
                                    valoracion.acercamiento = (valoracion.anio6 * 100 / valoracion.referencia);
                                }
                            }
                        }
                    }

                    //MAGNITUD
                    int MAGNITUD = 3;
                    //decimal valorpromedioaniosanteriores = (decimal.Parse(valoracion.anio1.ToString()) + decimal.Parse(valoracion.anio2.ToString()) + decimal.Parse(valoracion.anio3.ToString()) + decimal.Parse(valoracion.anio4.ToString()) + decimal.Parse(valoracion.anio5.ToString())) / 5;
                    //decimal valorpromedioaniosanteriores5porciento = valorpromedioaniosanteriores + ((valorpromedioaniosanteriores / 100) * 5);
                    //decimal valorpromedioaniosanteriores2porciento = valorpromedioaniosanteriores + ((valorpromedioaniosanteriores / 100) * 2);
                    if (TipoAspecto.Grupo == 1 || TipoAspecto.Grupo == 3 || TipoAspecto.Grupo == 4 || TipoAspecto.Grupo == 5 || TipoAspecto.Grupo == 6 || TipoAspecto.Grupo == 7 || TipoAspecto.Grupo == 9 || TipoAspecto.Grupo == 10 || TipoAspecto.Grupo == 11 || TipoAspecto.Grupo == 12 || TipoAspecto.Grupo == 14 || TipoAspecto.Grupo == 15 || TipoAspecto.Grupo == 16 || TipoAspecto.Grupo == 20 || TipoAspecto.Grupo == 21)
                    {
                        if (valoracion.variacion < 0)
                            MAGNITUD = 1;
                        if (valoracion.variacion >= 0 && valoracion.variacion <= 5)
                            MAGNITUD = 2;
                        if (valoracion.variacion > 5)
                            MAGNITUD = 3;
                    }
                    if (TipoAspecto.Grupo == 9)
                    {
                        if (valoracion.variacion < 0)
                            MAGNITUD = 1;
                        if (valoracion.variacion >= 0 && valoracion.variacion <= 2)
                            MAGNITUD = 2;
                        if (valoracion.variacion > 2 || (valoracion.anio1 == null && valoracion.anio2 == null && valoracion.anio3 == null && valoracion.anio4 == null && valoracion.anio5 == null && valoracion.anio6 != null ))
                            MAGNITUD = 3;
                    }
                    if (TipoAspecto.Grupo == 2)
                    {
                        if (valoracion.variacion < 0)
                            MAGNITUD = 1;
                        if (valoracion.variacion >= 0 && valoracion.variacion <= 2)
                            MAGNITUD = 2;
                        if (valoracion.variacion > 2)
                            MAGNITUD = 3;
                    }
                    if (TipoAspecto.Grupo == 8)
                    {
                        decimal totalresiduos = Datos.CalcularTotalResiduosCentral(idCentral);
                        if (((Datos.GetDatosValoracion(id)).anio6 == null || Datos.GetDatosValoracion(id).anio6 == 0) && valoracion.anio6 != null)
                            totalresiduos = totalresiduos + decimal.Parse(valoracion.anio6.ToString());

                        decimal valortotalresiduos05 = ((totalresiduos / 100) * (5 / 10));
                        decimal valortotalresiduos5 = ((totalresiduos / 100) * 5);

                        if (valoracion.anio6 <= valortotalresiduos05)
                        {
                            MAGNITUD = 1;
                        }

                        if (valoracion.anio6 > valortotalresiduos05 && valoracion.anio6 < valortotalresiduos5)
                        {
                            MAGNITUD = 2;
                        }

                        if (valoracion.anio6 >= valortotalresiduos5)
                        {
                            MAGNITUD = 3;
                        }

                        valoracion.acercamiento = totalresiduos;
                    }
                    if (TipoAspecto.Grupo == 13 || TipoAspecto.Grupo == 17 || TipoAspecto.Grupo == 18 || TipoAspecto.Grupo == 19 || TipoAspecto.Grupo == 22 || TipoAspecto.Grupo == 23 || TipoAspecto.Grupo == 24)
                    {
                        MAGNITUD = int.Parse(valoracion.magnitud.ToString());
                    }

                    //NATURALEZA
                    int NATURALEZA = 3;

                    if (TipoAspecto.Grupo == 1 || TipoAspecto.Grupo == 6 || TipoAspecto.Grupo == 20)
                    {
                        if (valoracion.acercamiento <= 40)
                            NATURALEZA = 1;
                        if (valoracion.acercamiento > 40 && valoracion.acercamiento <= 80)
                            NATURALEZA = 2;
                        if (valoracion.acercamiento > 80)
                            NATURALEZA = 3;
                    }
                    if (TipoAspecto.Grupo == 2 || TipoAspecto.Grupo == 3 || TipoAspecto.Grupo == 4 || TipoAspecto.Grupo == 5 || TipoAspecto.Grupo == 7 || TipoAspecto.Grupo == 8 || TipoAspecto.Grupo == 9 || TipoAspecto.Grupo == 10 || TipoAspecto.Grupo == 11 || TipoAspecto.Grupo == 12 || TipoAspecto.Grupo == 15 || TipoAspecto.Grupo == 16 || TipoAspecto.Grupo == 17 || TipoAspecto.Grupo == 18 || TipoAspecto.Grupo == 19 || TipoAspecto.Grupo == 22 || TipoAspecto.Grupo == 23 || TipoAspecto.Grupo == 24)
                    {
                        NATURALEZA = int.Parse(valoracion.naturaleza.ToString());
                    }
                    if (TipoAspecto.Grupo == 21)
                    {
                        if (valoracion.acercamiento >= 2)
                            NATURALEZA = 1;
                        if (valoracion.acercamiento < 2 && valoracion.acercamiento > 1)
                            NATURALEZA = 2;
                        if (valoracion.acercamiento <= 1)
                            NATURALEZA = 3;
                    }
                    if (TipoAspecto.Grupo == 13 || TipoAspecto.Grupo == 14)
                    {
                        if (decimal.Parse(valoracion.acercamiento.ToString()) < 5)
                        {
                            NATURALEZA = 3;
                        }
                        if (decimal.Parse(valoracion.acercamiento.ToString()) >= 5 && decimal.Parse(valoracion.acercamiento.ToString()) <= 10)
                        {
                            NATURALEZA = 2;
                        }
                        if (decimal.Parse(valoracion.acercamiento.ToString()) > 10)
                        {
                            NATURALEZA = 1;
                        }
                    }

                    //ORIGEN
                    int ORIGEN = 3;
                    if (TipoAspecto.Grupo == 10 || TipoAspecto.Grupo == 11 || TipoAspecto.Grupo == 15 || TipoAspecto.Grupo == 18 || TipoAspecto.Grupo == 19 || TipoAspecto.Grupo == 20 || TipoAspecto.Grupo == 21)
                    {
                        ORIGEN = (MAGNITUD + NATURALEZA) / 2;
                    }

                    if (TipoAspecto.Grupo == 1 || TipoAspecto.Grupo == 2 || TipoAspecto.Grupo == 3 || TipoAspecto.Grupo == 4 || TipoAspecto.Grupo == 5 || TipoAspecto.Grupo == 6 || TipoAspecto.Grupo == 7 || TipoAspecto.Grupo == 8 || TipoAspecto.Grupo == 13 || TipoAspecto.Grupo == 14 || TipoAspecto.Grupo == 16 || TipoAspecto.Grupo == 17 || TipoAspecto.Grupo == 22 || TipoAspecto.Grupo == 23 || TipoAspecto.Grupo == 24)
                    {
                        ORIGEN = int.Parse(valoracion.origen.ToString());
                    }
                    if (TipoAspecto.Grupo == 9)
                    {
                        decimal totalconsumocombustible = Datos.CalcularTotalConsumoCombustibleCentral(idCentral);
                        //decimal valortotalconsumocombustible05 = ((totalconsumocombustible / 100) * (5 / 10));
                        //decimal valortotalconsumocombustible5 = ((totalconsumocombustible / 100) * 5);
                        if (((Datos.GetDatosParametro(id)).anio6 == null || Datos.GetDatosParametro(id).anio6 == 0) && valoracion.anio6 != null)
                            totalconsumocombustible = totalconsumocombustible + decimal.Parse(valoracion.anio6.ToString());
                        /*Definimos variables*/
                        decimal total3 = totalconsumocombustible;

                        if (valoracion.anio6 != null)
                        {
                            decimal resultado = decimal.Parse(valoracion.anio6.ToString()) / total3 * 100;
                            valoracion.acercamiento = resultado;
                        }

                        if (valoracion.acercamiento > 5)
                        {
                            ORIGEN = 3;
                        }
                        else
                        {
                            if (valoracion.acercamiento > decimal.Parse("0,5"))
                            {
                                ORIGEN = 2;
                            }
                            else
                            {
                                ORIGEN = 1;
                            }
                        }

                        List<aspecto_parametro_valoracion> listadoCombustiblesSustancias = Datos.ListarParametrosCombSust(centroseleccionado.id);

                        foreach (aspecto_parametro_valoracion asp_res in listadoCombustiblesSustancias)
                        {
                            if (asp_res.id != valoracion.id && asp_res.anio6 != null)
                            {
                                decimal totalconsumosustanciasrec = Datos.CalcularTotalConsumoCombustibleCentral(idCentral);
                                //decimal valortotalconsumosustancias05 = ((totalconsumosustancias / 100) * (5 / 10));
                                //decimal valortotalconsumosustancias5 = ((totalconsumosustancias / 100) * 5);

                                if (((Datos.GetDatosParametro(id)).anio6 == null || Datos.GetDatosParametro(id).anio6 == 0) && valoracion.anio6 != null)
                                    totalconsumosustanciasrec = totalconsumosustanciasrec + decimal.Parse(valoracion.anio6.ToString());
                                else
                                {
                                    if (valoracion.anio6 != null && valoracion.anio6 > 0)
                                        totalconsumosustanciasrec = (totalconsumosustanciasrec - decimal.Parse(((Datos.GetDatosParametro(id)).anio6).ToString())) + decimal.Parse(valoracion.anio6.ToString());
                                }

                                decimal total3rec = totalconsumosustanciasrec;
                                if (asp_res.anio6 != null)
                                {
                                    decimal resultado = (decimal.Parse(asp_res.anio6.ToString()) * 100) / total3rec;
                                    asp_res.acercamiento = resultado;
                                }

                                if (asp_res.acercamiento > 5)
                                {
                                    asp_res.origen = 3;
                                    asp_res.resorigen = 3;
                                }
                                else
                                {
                                    if (asp_res.acercamiento > decimal.Parse("0,5"))
                                    {
                                        asp_res.origen = 2;
                                        asp_res.resorigen = 2;
                                    }
                                    else
                                    {
                                        asp_res.origen = 1;
                                        asp_res.resorigen = 1;
                                    }
                                }

                                if (asp_res.magnitud + asp_res.naturaleza + asp_res.origen >= 7 && asp_res.anio6 > 0)
                                    asp_res.significancia6 = 1;
                                else
                                    asp_res.significancia6 = 0;

                                if (asp_res.acercamiento != null && asp_res.acercamiento > 100)
                                {
                                    asp_res.significancia6 = 1;
                                }

                                Datos.ActualizarValoracion(asp_res);

                            }
                        }
                    }
                    if (TipoAspecto.Grupo == 12)
                    {
                        decimal totalconsumosustancias = Datos.CalcularTotalConsumoSustanciasCentral(idCentral);
                        //decimal valortotalconsumosustancias05 = ((totalconsumosustancias / 100) * (5 / 10));
                        //decimal valortotalconsumosustancias5 = ((totalconsumosustancias / 100) * 5);

                        if (((Datos.GetDatosParametro(id)).anio6 == null || Datos.GetDatosParametro(id).anio6 == 0) && valoracion.anio6 != null)
                            totalconsumosustancias = totalconsumosustancias + decimal.Parse(valoracion.anio6.ToString());
                        else
                        {
                            if (valoracion.anio6 != null && valoracion.anio6 > 0)
                                totalconsumosustancias = (totalconsumosustancias - decimal.Parse(((Datos.GetDatosParametro(id)).anio6).ToString())) + decimal.Parse(valoracion.anio6.ToString());
                        }

                        decimal total3 = totalconsumosustancias;
                        if (valoracion.anio6 != null)
                        {
                            decimal resultado = (decimal.Parse(valoracion.anio6.ToString()) * 100) / total3;
                            valoracion.acercamiento = resultado;
                        }

                        if (valoracion.acercamiento > 5)
                        {
                            ORIGEN = 3;
                        }
                        else
                        {
                            if (valoracion.acercamiento > decimal.Parse("0,5"))
                            {
                                ORIGEN = 2;
                            }
                            else
                            {
                                ORIGEN = 1;
                            }
                        }

                        List<aspecto_parametro_valoracion> listadoCombustiblesSustancias = Datos.ListarParametrosCombSust(centroseleccionado.id);

                        foreach (aspecto_parametro_valoracion asp_res in listadoCombustiblesSustancias)
                        {
                            if (asp_res.id != valoracion.id && asp_res.anio6 != null)
                            {
                                decimal totalconsumosustanciasrec = Datos.CalcularTotalConsumoSustanciasCentral(idCentral);
                                //decimal valortotalconsumosustancias05 = ((totalconsumosustancias / 100) * (5 / 10));
                                //decimal valortotalconsumosustancias5 = ((totalconsumosustancias / 100) * 5);

                                if (((Datos.GetDatosParametro(id)).anio6 == null || Datos.GetDatosParametro(id).anio6 == 0) && valoracion.anio6 != null)
                                    totalconsumosustanciasrec = totalconsumosustanciasrec + decimal.Parse(valoracion.anio6.ToString());
                                else
                                {
                                    if (valoracion.anio6 != null && valoracion.anio6 > 0)
                                        totalconsumosustanciasrec = (totalconsumosustanciasrec - decimal.Parse(((Datos.GetDatosParametro(id)).anio6).ToString())) + decimal.Parse(valoracion.anio6.ToString());
                                }

                                decimal total3rec = totalconsumosustanciasrec;
                                if (asp_res.anio6 != null)
                                {
                                    decimal resultado = (decimal.Parse(asp_res.anio6.ToString()) * 100) / total3rec;
                                    asp_res.acercamiento = resultado;
                                }

                                if (asp_res.acercamiento > 5)
                                {
                                    asp_res.origen = 3;
                                    asp_res.resorigen = 3;
                                }
                                else
                                {
                                    if (asp_res.acercamiento > decimal.Parse("0,5"))
                                    {
                                        asp_res.origen = 2;
                                        asp_res.resorigen = 2;
                                    }
                                    else
                                    {
                                        asp_res.origen = 1;
                                        asp_res.resorigen = 1;
                                    }
                                }

                                if (asp_res.magnitud + asp_res.naturaleza + asp_res.origen >= 7 && asp_res.anio6 > 0)
                                    asp_res.significancia6 = 1;
                                else
                                    asp_res.significancia6 = 0;

                                if (asp_res.acercamiento != null && asp_res.acercamiento > 100)
                                {
                                    asp_res.significancia6 = 1;
                                }

                                Datos.ActualizarValoracion(asp_res);

                            }
                        }


                        //if (valoracion.anio6 <= valortotalconsumosustancias05)
                        //{
                        //    ORIGEN = 1;
                        //}

                        //if (valoracion.anio6 > valortotalconsumosustancias05 && valoracion.anio6 < valortotalconsumosustancias5)
                        //{
                        //    ORIGEN = 2;
                        //}

                        //if (valoracion.anio6 >= valortotalconsumosustancias5)
                        //{
                        //    ORIGEN = 3;
                        //}
                    }

                    //SIGNIFICANCIA
                    int SIGNIFICANCIA = 0;
                    if (TipoAspecto.Grupo == 8 && valoracion.anio6 == 0)
                        SIGNIFICANCIA = 0;
                    else
                    {
                        if (MAGNITUD + NATURALEZA + ORIGEN >= 7)
                            SIGNIFICANCIA = 1;
                    }

                    if (TipoAspecto.Grupo != 8 && TipoAspecto.Grupo != 9 && TipoAspecto.Grupo != 12 && valoracion.acercamiento != null && valoracion.acercamiento > 100)
                    {
                        SIGNIFICANCIA = 1;
                    }

                    if ((TipoAspecto.Grupo == 13 || TipoAspecto.Grupo == 14) && valoracion.acercamiento < 0)
                    {
                        SIGNIFICANCIA = 1;
                    }

                    #endregion
                    valoracion.magnitud = MAGNITUD;
                    valoracion.resmagnitud = MAGNITUD;
                    valoracion.naturaleza = NATURALEZA;
                    valoracion.resnaturaleza = NATURALEZA;
                    valoracion.origen = ORIGEN;
                    valoracion.resorigen = ORIGEN;
                    valoracion.significancia6 = SIGNIFICANCIA;

                    Datos.ActualizarValoracion(valoracion);

                    if (buscarValoracion.resmagnitud == null || buscarValoracion.resmagnitud < valoracion.resmagnitud)
                        buscarValoracion.resmagnitud = valoracion.resmagnitud;
                    if (buscarValoracion.resnaturaleza == null || buscarValoracion.resnaturaleza < valoracion.resnaturaleza)
                        buscarValoracion.resnaturaleza = valoracion.resnaturaleza;
                    if (buscarValoracion.resorigen == null || buscarValoracion.resorigen < valoracion.resorigen)
                        buscarValoracion.resorigen = valoracion.resorigen;

                    decimal significanciavaloracionaspecto = decimal.Parse(buscarValoracion.resmagnitud.ToString()) + decimal.Parse(buscarValoracion.resnaturaleza.ToString()) + decimal.Parse(buscarValoracion.resorigen.ToString());

                    if (significanciavaloracionaspecto >= 7)
                        buscarValoracion.significancia6 = 1;
                    else
                        buscarValoracion.significancia6 = 0;

                    if (TipoAspecto.Grupo != 8 && TipoAspecto.Grupo != 9 && TipoAspecto.Grupo != 12 && valoracion.acercamiento != null && valoracion.acercamiento > 100)
                    {
                        buscarValoracion.significancia6 = 1;
                    }

                    if ((TipoAspecto.Grupo == 13 || TipoAspecto.Grupo == 14) && valoracion.acercamiento < 0)
                    {
                        buscarValoracion.significancia6 = 1;
                    }

                    Datos.ActualizarSignificanciaAspecto(buscarValoracion);

                    aspecto_parametro_valoracion buscarParametro = Datos.ObtenerParametroAspecto(id);
                    ViewData["valoracionparametro"] = buscarParametro;
                    ViewData["valoracionaspecto"] = buscarValoracion;
                    aspecto_tipo buscarTipoAspecto = Datos.ObtenerTipoAspecto(int.Parse(buscarValoracion.idAspecto.ToString()));
                    ViewData["tipoaspecto"] = buscarTipoAspecto;
                    aspecto_parametros buscarParametros = Datos.ObtenerParametrosCentral(idCentral);
                    ViewData["EditarParametros"] = buscarParametros;
                    return View();
                }
                catch (Exception ex)
                {
                    aspecto_parametro_valoracion buscarParametro = Datos.ObtenerParametroAspecto(id);
                    ViewData["valoracionparametro"] = buscarParametro;
                    aspecto_valoracion buscarValoracion = Datos.ObtenerValoracionAspecto(buscarParametro.id_aspecto);
                    ViewData["valoracionaspecto"] = buscarValoracion;
                    aspecto_tipo buscarTipoAspecto = Datos.ObtenerTipoAspecto(int.Parse(buscarValoracion.idAspecto.ToString()));
                    ViewData["tipoaspecto"] = buscarTipoAspecto;
                    aspecto_parametros buscarParametros = Datos.ObtenerParametrosCentral(idCentral);
                    ViewData["EditarParametros"] = buscarParametros;
                    Session["EdicionIndicadorError"] = "No se han podido guardar los datos, compruebe que los datos introducidos son correctos.";
                    return View();
                }
                #endregion
            }
            else
            {
                #region recarga
                aspecto_parametro_valoracion buscarParametro = Datos.ObtenerParametroAspecto(id);
                ViewData["valoracionparametro"] = buscarParametro;
                aspecto_valoracion buscarValoracion = Datos.ObtenerValoracionAspecto(buscarParametro.id_aspecto);
                ViewData["valoracionaspecto"] = buscarValoracion;
                aspecto_tipo buscarTipoAspecto = Datos.ObtenerTipoAspecto(int.Parse(buscarValoracion.idAspecto.ToString()));
                ViewData["tipoaspecto"] = buscarTipoAspecto;
                aspecto_parametros buscarParametros = Datos.ObtenerParametrosCentral(idCentral);
                ViewData["EditarParametros"] = buscarParametros;
                return View();
                #endregion
            }
        }

        public ActionResult parametros()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            int idCentral = centroseleccionado.id;

            aspecto_parametros buscarParametros = Datos.ObtenerParametrosCentral(idCentral);
            ViewData["EditarParametros"] = buscarParametros;

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult parametros(FormCollection collection)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            string formulario = collection["hdFormularioEjecutado"];

            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            int idCentral = centroseleccionado.id;
            
            if (formulario == "GuardarParametros")
            {
                #region Guardar Parametros
                try
                {
                    aspecto_parametros actualizar = Datos.ObtenerParametrosCentral(idCentral);

                    if (actualizar == null)
                        actualizar = new aspecto_parametros();

                    actualizar.idCentral = idCentral;

                    if (collection["ctl00$MainContent$txtMwhb1"] != null && collection["ctl00$MainContent$txtMwhb1"] != string.Empty)
                        actualizar.Mwhb1 = decimal.Parse(collection["ctl00$MainContent$txtMwhb1"].ToString().Replace(".", ","));
                    else
                        actualizar.Mwhb1 = null;
                    if (collection["ctl00$MainContent$txtMwhb2"] != null && collection["ctl00$MainContent$txtMwhb2"] != string.Empty)
                        actualizar.Mwhb2 = decimal.Parse(collection["ctl00$MainContent$txtMwhb2"].ToString().Replace(".", ","));
                    else
                        actualizar.Mwhb2 = null;
                    if (collection["ctl00$MainContent$txtMwhb3"] != null && collection["ctl00$MainContent$txtMwhb3"] != string.Empty)
                        actualizar.Mwhb3 = decimal.Parse(collection["ctl00$MainContent$txtMwhb3"].ToString().Replace(".", ","));
                    else
                        actualizar.Mwhb3 = null;
                    if (collection["ctl00$MainContent$txtMwhb4"] != null && collection["ctl00$MainContent$txtMwhb4"] != string.Empty)
                        actualizar.Mwhb4 = decimal.Parse(collection["ctl00$MainContent$txtMwhb4"].ToString().Replace(".", ","));
                    else
                        actualizar.Mwhb4 = null;
                    if (collection["ctl00$MainContent$txtMwhb5"] != null && collection["ctl00$MainContent$txtMwhb5"] != string.Empty)
                        actualizar.Mwhb5 = decimal.Parse(collection["ctl00$MainContent$txtMwhb5"].ToString().Replace(".", ","));
                    else
                        actualizar.Mwhb5 = null;
                    if (collection["ctl00$MainContent$txtMwhb6"] != null && collection["ctl00$MainContent$txtMwhb6"] != string.Empty)
                        actualizar.Mwhb6 = decimal.Parse(collection["ctl00$MainContent$txtMwhb6"].ToString().Replace(".", ","));
                    else
                        actualizar.Mwhb6 = null;

                    if (collection["ctl00$MainContent$txtkmAnio1"] != null && collection["ctl00$MainContent$txtkmAnio1"] != string.Empty)
                        actualizar.kmAnio1 = decimal.Parse(collection["ctl00$MainContent$txtkmAnio1"].ToString().Replace(".", ","));
                    else
                        actualizar.kmAnio1 = null;
                    if (collection["ctl00$MainContent$txtkmAnio2"] != null && collection["ctl00$MainContent$txtkmAnio2"] != string.Empty)
                        actualizar.kmAnio2 = decimal.Parse(collection["ctl00$MainContent$txtkmAnio2"].ToString().Replace(".", ","));
                    else
                        actualizar.kmAnio2 = null;
                    if (collection["ctl00$MainContent$txtkmAnio3"] != null && collection["ctl00$MainContent$txtkmAnio3"] != string.Empty)
                        actualizar.kmAnio3 = decimal.Parse(collection["ctl00$MainContent$txtkmAnio3"].ToString().Replace(".", ","));
                    else
                        actualizar.kmAnio3 = null;
                    if (collection["ctl00$MainContent$txtkmAnio4"] != null && collection["ctl00$MainContent$txtkmAnio4"] != string.Empty)
                        actualizar.kmAnio4 = decimal.Parse(collection["ctl00$MainContent$txtkmAnio4"].ToString().Replace(".", ","));
                    else
                        actualizar.kmAnio4 = null;
                    if (collection["ctl00$MainContent$txtkmAnio5"] != null && collection["ctl00$MainContent$txtkmAnio5"] != string.Empty)
                        actualizar.kmAnio5 = decimal.Parse(collection["ctl00$MainContent$txtkmAnio5"].ToString().Replace(".", ","));
                    else
                        actualizar.kmAnio5 = null;
                    if (collection["ctl00$MainContent$txtkmAnio6"] != null && collection["ctl00$MainContent$txtkmAnio6"] != string.Empty)
                        actualizar.kmAnio6 = decimal.Parse(collection["ctl00$MainContent$txtkmAnio6"].ToString().Replace(".", ","));
                    else
                        actualizar.kmAnio6 = null;

                    if (collection["ctl00$MainContent$txtm3Hora1"] != null && collection["ctl00$MainContent$txtm3Hora1"] != string.Empty)
                        actualizar.m3Hora1 = decimal.Parse(collection["ctl00$MainContent$txtm3Hora1"].ToString().Replace(".", ","));
                    else
                        actualizar.m3Hora1 = null;
                    if (collection["ctl00$MainContent$txtm3Hora2"] != null && collection["ctl00$MainContent$txtm3Hora2"] != string.Empty)
                        actualizar.m3Hora2 = decimal.Parse(collection["ctl00$MainContent$txtm3Hora2"].ToString().Replace(".", ","));
                    else
                        actualizar.m3Hora2 = null;
                    if (collection["ctl00$MainContent$txtm3Hora3"] != null && collection["ctl00$MainContent$txtm3Hora3"] != string.Empty)
                        actualizar.m3Hora3 = decimal.Parse(collection["ctl00$MainContent$txtm3Hora3"].ToString().Replace(".", ","));
                    else
                        actualizar.m3Hora3 = null;
                    if (collection["ctl00$MainContent$txtm3Hora4"] != null && collection["ctl00$MainContent$txtm3Hora4"] != string.Empty)
                        actualizar.m3Hora4 = decimal.Parse(collection["ctl00$MainContent$txtm3Hora4"].ToString().Replace(".", ","));
                    else
                        actualizar.m3Hora4 = null;
                    if (collection["ctl00$MainContent$txtm3Hora5"] != null && collection["ctl00$MainContent$txtm3Hora5"] != string.Empty)
                        actualizar.m3Hora5 = decimal.Parse(collection["ctl00$MainContent$txtm3Hora5"].ToString().Replace(".", ","));
                    else
                        actualizar.m3Hora5 = null;
                    if (collection["ctl00$MainContent$txtm3Hora6"] != null && collection["ctl00$MainContent$txtm3Hora6"] != string.Empty)
                        actualizar.m3Hora6 = decimal.Parse(collection["ctl00$MainContent$txtm3Hora6"].ToString().Replace(".", ","));
                    else
                        actualizar.m3Hora6 = null;

                    if (collection["ctl00$MainContent$txtnumtrabAnio1"] != null && collection["ctl00$MainContent$txtnumtrabAnio1"] != string.Empty)
                        actualizar.numtrabAnio1 = decimal.Parse(collection["ctl00$MainContent$txtnumtrabAnio1"].ToString().Replace(".", ","));
                    else
                        actualizar.numtrabAnio1 = null;
                    if (collection["ctl00$MainContent$txtnumtrabAnio2"] != null && collection["ctl00$MainContent$txtnumtrabAnio2"] != string.Empty)
                        actualizar.numtrabAnio2 = decimal.Parse(collection["ctl00$MainContent$txtnumtrabAnio2"].ToString().Replace(".", ","));
                    else
                        actualizar.numtrabAnio2 = null;
                    if (collection["ctl00$MainContent$txtnumtrabAnio3"] != null && collection["ctl00$MainContent$txtnumtrabAnio3"] != string.Empty)
                        actualizar.numtrabAnio3 = decimal.Parse(collection["ctl00$MainContent$txtnumtrabAnio3"].ToString().Replace(".", ","));
                    else
                        actualizar.numtrabAnio3 = null;
                    if (collection["ctl00$MainContent$txtnumtrabAnio4"] != null && collection["ctl00$MainContent$txtnumtrabAnio4"] != string.Empty)
                        actualizar.numtrabAnio4 = decimal.Parse(collection["ctl00$MainContent$txtnumtrabAnio4"].ToString().Replace(".", ","));
                    else
                        actualizar.numtrabAnio4 = null;
                    if (collection["ctl00$MainContent$txtnumtrabAnio5"] != null && collection["ctl00$MainContent$txtnumtrabAnio5"] != string.Empty)
                        actualizar.numtrabAnio5 = decimal.Parse(collection["ctl00$MainContent$txtnumtrabAnio5"].ToString().Replace(".", ","));
                    else
                        actualizar.numtrabAnio5 = null;
                    if (collection["ctl00$MainContent$txtnumtrabAnio6"] != null && collection["ctl00$MainContent$txtnumtrabAnio6"] != string.Empty)
                        actualizar.numtrabAnio6 = decimal.Parse(collection["ctl00$MainContent$txtnumtrabAnio6"].ToString().Replace(".", ","));
                    else
                        actualizar.numtrabAnio6 = null;

                    if (collection["ctl00$MainContent$txtm3aguadesaladaAnio1"] != null && collection["ctl00$MainContent$txtm3aguadesaladaAnio1"] != string.Empty)
                        actualizar.m3aguadesaladaAnio1 = decimal.Parse(collection["ctl00$MainContent$txtm3aguadesaladaAnio1"].ToString().Replace(".", ","));
                    else
                        actualizar.m3aguadesaladaAnio1 = null;
                    if (collection["ctl00$MainContent$txtm3aguadesaladaAnio2"] != null && collection["ctl00$MainContent$txtm3aguadesaladaAnio2"] != string.Empty)
                        actualizar.m3aguadesaladaAnio2 = decimal.Parse(collection["ctl00$MainContent$txtm3aguadesaladaAnio2"].ToString().Replace(".", ","));
                    else
                        actualizar.m3aguadesaladaAnio2 = null;
                    if (collection["ctl00$MainContent$txtm3aguadesaladaAnio3"] != null && collection["ctl00$MainContent$txtm3aguadesaladaAnio3"] != string.Empty)
                        actualizar.m3aguadesaladaAnio3 = decimal.Parse(collection["ctl00$MainContent$txtm3aguadesaladaAnio3"].ToString().Replace(".", ","));
                    else
                        actualizar.m3aguadesaladaAnio3 = null;
                    if (collection["ctl00$MainContent$txtm3aguadesaladaAnio4"] != null && collection["ctl00$MainContent$txtm3aguadesaladaAnio4"] != string.Empty)
                        actualizar.m3aguadesaladaAnio4 = decimal.Parse(collection["ctl00$MainContent$txtm3aguadesaladaAnio4"].ToString().Replace(".", ","));
                    else
                        actualizar.m3aguadesaladaAnio4 = null;
                    if (collection["ctl00$MainContent$txtm3aguadesaladaAnio5"] != null && collection["ctl00$MainContent$txtm3aguadesaladaAnio5"] != string.Empty)
                        actualizar.m3aguadesaladaAnio5 = decimal.Parse(collection["ctl00$MainContent$txtm3aguadesaladaAnio5"].ToString().Replace(".", ","));
                    else
                        actualizar.m3aguadesaladaAnio5 = null;
                    if (collection["ctl00$MainContent$txtm3aguadesaladaAnio6"] != null && collection["ctl00$MainContent$txtm3aguadesaladaAnio6"] != string.Empty)
                        actualizar.m3aguadesaladaAnio6 = decimal.Parse(collection["ctl00$MainContent$txtm3aguadesaladaAnio6"].ToString().Replace(".", ","));
                    else
                        actualizar.m3aguadesaladaAnio6 = null;

                    if (collection["ctl00$MainContent$txttrabcanteraAnio1"] != null && collection["ctl00$MainContent$txttrabcanteraAnio1"] != string.Empty)
                        actualizar.trabcanteraAnio1 = decimal.Parse(collection["ctl00$MainContent$txttrabcanteraAnio1"].ToString().Replace(".", ","));
                    else
                        actualizar.trabcanteraAnio1 = null;
                    if (collection["ctl00$MainContent$txttrabcanteraAnio2"] != null && collection["ctl00$MainContent$txttrabcanteraAnio2"] != string.Empty)
                        actualizar.trabcanteraAnio2 = decimal.Parse(collection["ctl00$MainContent$txttrabcanteraAnio2"].ToString().Replace(".", ","));
                    else
                        actualizar.trabcanteraAnio2 = null;
                    if (collection["ctl00$MainContent$txttrabcanteraAnio3"] != null && collection["ctl00$MainContent$txttrabcanteraAnio3"] != string.Empty)
                        actualizar.trabcanteraAnio3 = decimal.Parse(collection["ctl00$MainContent$txttrabcanteraAnio3"].ToString().Replace(".", ","));
                    else
                        actualizar.trabcanteraAnio3 = null;
                    if (collection["ctl00$MainContent$txttrabcanteraAnio4"] != null && collection["ctl00$MainContent$txttrabcanteraAnio4"] != string.Empty)
                        actualizar.trabcanteraAnio4 = decimal.Parse(collection["ctl00$MainContent$txttrabcanteraAnio4"].ToString().Replace(".", ","));
                    else
                        actualizar.trabcanteraAnio4 = null;
                    if (collection["ctl00$MainContent$txttrabcanteraAnio5"] != null && collection["ctl00$MainContent$txttrabcanteraAnio5"] != string.Empty)
                        actualizar.trabcanteraAnio5 = decimal.Parse(collection["ctl00$MainContent$txttrabcanteraAnio5"].ToString().Replace(".", ","));
                    else
                        actualizar.trabcanteraAnio5 = null;
                    if (collection["ctl00$MainContent$txttrabcanteraAnio6"] != null && collection["ctl00$MainContent$txttrabcanteraAnio6"] != string.Empty)
                        actualizar.trabcanteraAnio6 = decimal.Parse(collection["ctl00$MainContent$txttrabcanteraAnio6"].ToString().Replace(".", ","));
                    else
                        actualizar.trabcanteraAnio6 = null;

                    Datos.ActualizarParametros(actualizar);

                    aspecto_parametros buscarParametros = Datos.ObtenerParametrosCentral(idCentral);
                    ViewData["EditarParametros"] = buscarParametros;
                    return View();
                }
                catch (Exception ex)
                {
                    aspecto_parametros buscarParametros = Datos.ObtenerParametrosCentral(idCentral);
                    ViewData["EditarParametros"] = buscarParametros;
                    Session["EdicionParametrosError"] = "No se han podido guardar los datos, compruebe que los datos introducidos son correctos.";
                    return View();
                }
                #endregion
            }
            else
            {
                #region recarga
                aspecto_parametros buscarParametros = Datos.ObtenerParametrosCentral(idCentral);
                ViewData["EditarParametros"] = buscarParametros;
                return View();
                #endregion
            }
        }

        public ActionResult detalle_aspecto(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            Session["modulo"] = "10";

            ViewData["idValoracion"] = id;

            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            int idCentral = centroseleccionado.id;

            aspecto_parametros buscarParametros = Datos.ObtenerParametrosCentral(idCentral);
            ViewData["EditarParametros"] = buscarParametros;

            #region lista de quejas
            List<System.Web.UI.WebControls.ListItem> listado = new List<System.Web.UI.WebControls.ListItem>();
                List<MIDAS.Models.VISTA_Comunicaciones> listaCom = MIDAS.Models.Datos.ListarQuejas(centroseleccionado.id);
                foreach (MIDAS.Models.VISTA_Comunicaciones com in listaCom)
                {
                    System.Web.UI.WebControls.ListItem nuevoItem = new System.Web.UI.WebControls.ListItem();
                    nuevoItem.Value = com.id.ToString();
                    if (com.fechainicio != null)
                    {
                        nuevoItem.Text = com.idcomunicacion + " - " + com.Clasificacion + " - " + DateTime.Parse(com.fechainicio.ToString()).Date.ToShortDateString();
                    }
                    else
                    {
                        nuevoItem.Text = com.idcomunicacion + " - " + com.Clasificacion;
                    }

                    listado.Add(nuevoItem);
                }
                ViewData["quejas"] = listado;
            #endregion

            aspecto_valoracion buscarValoracion = Datos.ObtenerValoracionAspecto(id);
            ViewData["EditarAspectoValoracion"] = buscarValoracion;
            aspecto_tipo buscarTipoAspecto = Datos.ObtenerTipoAspecto(int.Parse(buscarValoracion.idAspecto.ToString()));
            ViewData["EditarTipoAspecto"] = buscarTipoAspecto;
            ViewData["parametros"] = Datos.ListarParametros(buscarTipoAspecto.Grupo);
            ViewData["parametrosAsignados"] = Datos.ListarParametrosAsignados(buscarValoracion.id);
            #region generacion fichero
            if (Session["nombreArchivo"] != null && Session["nombreArchivo"].ToString() != "")
            {
                if (Session["nombreArchivo"].ToString().Contains("docx"))
                {
                    Response.ContentType = "application/msword";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=ExportacionAspecto.docx");
                    Response.TransmitFile(Session["nombreArchivo"].ToString());
                    Response.End();
                }
                Session["nombreArchivo"] = "";
            }
            #endregion
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult detalle_aspecto(int id, FormCollection collection, HttpPostedFileBase[] file)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            string formulario = collection["hdFormularioEjecutado"];

            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            int idCentral = centroseleccionado.id;

            if (formulario == "GuardarAspectoValoracion")
            {
                #region Guardar Aspecto
                try
                {
                    aspecto_valoracion valoracion = Datos.GetDatosValoracion(id);

                    if (collection["ctl00$MainContent$txtIdentificacion"] != null && collection["ctl00$MainContent$txtIdentificacion"] != string.Empty)
                    {
                        if (valoracion.foco == 1)
                            valoracion.nombrefoco = collection["ctl00$MainContent$txtIdentificacion"].ToString();
                        else
                            valoracion.identificacion = collection["ctl00$MainContent$txtIdentificacion"].ToString();
                    }

                    aspecto_tipo TipoAspecto = Datos.ObtenerTipoAspecto(int.Parse(valoracion.idAspecto.ToString()));

                    if (valoracion.foco == 0 || (valoracion.foco == 1 && TipoAspecto.Grupo == 12))
                    {
                        #region mediciones
                        if (collection["ctl00$MainContent$txtReferencia"] != null && collection["ctl00$MainContent$txtReferencia"] != string.Empty)
                            valoracion.referencia = decimal.Parse(collection["ctl00$MainContent$txtReferencia"].ToString().Replace(".", ","));
                        else
                            valoracion.referencia = 0;

                        if (collection["ctl00$MainContent$txtAbsoluto"] != null && collection["ctl00$MainContent$txtAbsoluto"] != string.Empty)
                            valoracion.datoabsoluto = decimal.Parse(collection["ctl00$MainContent$txtAbsoluto"].ToString().Replace(".", ","));
                        else
                            valoracion.datoabsoluto = 0;

                        if (collection["ctl00$MainContent$txtServicio"] != null && collection["ctl00$MainContent$txtServicio"] != string.Empty)
                            valoracion.IN_ServicioPrestado = collection["ctl00$MainContent$txtServicio"].ToString();
                        if (collection["ctl00$MainContent$ddlTipoIndirecto"] != null && collection["ctl00$MainContent$ddlTipoIndirecto"] != string.Empty)
                            valoracion.IN_TipoActividad = int.Parse(collection["ctl00$MainContent$ddlTipoIndirecto"].ToString());
                        if (collection["ctl00$MainContent$ddlAspecto"] != null && collection["ctl00$MainContent$ddlAspecto"] != string.Empty)
                            valoracion.IN_Aspecto = int.Parse(collection["ctl00$MainContent$ddlAspecto"].ToString());

                        if (collection["ctl00$MainContent$txtMed1"] != null && collection["ctl00$MainContent$txtMed1"] != string.Empty)
                            valoracion.anio1 = decimal.Parse(collection["ctl00$MainContent$txtMed1"].ToString().Replace(".", ","));
                        else
                            valoracion.anio1 = null;
                        if (collection["ctl00$MainContent$txtMed2"] != null && collection["ctl00$MainContent$txtMed2"] != string.Empty)
                            valoracion.anio2 = decimal.Parse(collection["ctl00$MainContent$txtMed2"].ToString().Replace(".", ","));
                        else
                            valoracion.anio2 = null;
                        if (collection["ctl00$MainContent$txtMed3"] != null && collection["ctl00$MainContent$txtMed3"] != string.Empty)
                            valoracion.anio3 = decimal.Parse(collection["ctl00$MainContent$txtMed3"].ToString().Replace(".", ","));
                        else
                            valoracion.anio3 = null;
                        if (collection["ctl00$MainContent$txtMed4"] != null && collection["ctl00$MainContent$txtMed4"] != string.Empty)
                            valoracion.anio4 = decimal.Parse(collection["ctl00$MainContent$txtMed4"].ToString().Replace(".", ","));
                        else
                            valoracion.anio4 = null;
                        if (collection["ctl00$MainContent$txtMed5"] != null && collection["ctl00$MainContent$txtMed5"] != string.Empty)
                            valoracion.anio5 = decimal.Parse(collection["ctl00$MainContent$txtMed5"].ToString().Replace(".", ","));
                        else
                            valoracion.anio5 = null;

                        if (collection["ctl00$MainContent$txtMed6"] != null && collection["ctl00$MainContent$txtMed6"] != string.Empty)
                            valoracion.anio6 = decimal.Parse(collection["ctl00$MainContent$txtMed6"].ToString().Replace(".", ","));
                        else
                            valoracion.anio6 = null;

                        #endregion

                        aspecto_parametros Parametros = Datos.ObtenerParametrosCentral(idCentral);
                        
                        #region modificarTipoAspecto (ya no se hace en maestro)
                        //if (collection["ctl00$MainContent$ddlPeligroso"] != null)
                        //    TipoAspecto.RE_Peligroso = int.Parse(collection["ctl00$MainContent$ddlPeligroso"]);
                        //TipoAspecto.Nombre = collection["ctl00$MainContent$txtIdentificacion"];
                        //TipoAspecto.Impacto = collection["ctl00$MainContent$txtImpacto"];
                        //TipoAspecto.Descripcion = collection["ctl00$MainContent$txtDescripcion"];
                        //TipoAspecto.Unidad = collection["ctl00$MainContent$txtUnidad"];

                        //Datos.ActualizarTipoAspecto(TipoAspecto);
                        #endregion

                        #region valoresrelativizar
                        if (TipoAspecto.Grupo == 13 || TipoAspecto.Grupo == 17 || TipoAspecto.Grupo == 18 || TipoAspecto.Grupo == 19 || TipoAspecto.Grupo == 22 || TipoAspecto.Grupo == 23 || TipoAspecto.Grupo == 24)
                        {
                            valoracion.magnitud = int.Parse(collection["ctl00$MainContent$ddlMagnitud"].ToString());
                            if (valoracion.magnitud == 4)
                                valoracion.magnitud = 3;
                        }
                        if (TipoAspecto.Grupo == 2 || TipoAspecto.Grupo == 3 || TipoAspecto.Grupo == 4 || TipoAspecto.Grupo == 5 || TipoAspecto.Grupo == 7 || TipoAspecto.Grupo == 8 || TipoAspecto.Grupo == 9 || TipoAspecto.Grupo == 10 || TipoAspecto.Grupo == 11 || (TipoAspecto.Grupo == 12 && valoracion.foco != 1 ) || TipoAspecto.Grupo == 15 || TipoAspecto.Grupo == 16 || TipoAspecto.Grupo == 17 || TipoAspecto.Grupo == 18 || TipoAspecto.Grupo == 19 || TipoAspecto.Grupo == 22 || TipoAspecto.Grupo == 23 || TipoAspecto.Grupo == 24)
                        {
                            valoracion.naturaleza = int.Parse(collection["ctl00$MainContent$ddlNaturaleza"].ToString());
                        }

                        if (TipoAspecto.Grupo == 1 || TipoAspecto.Grupo == 2 || TipoAspecto.Grupo == 3 || TipoAspecto.Grupo == 4 || TipoAspecto.Grupo == 5 || TipoAspecto.Grupo == 6 || TipoAspecto.Grupo == 7 || TipoAspecto.Grupo == 8 || TipoAspecto.Grupo == 13 || TipoAspecto.Grupo == 14 || TipoAspecto.Grupo == 16 || TipoAspecto.Grupo == 17 || TipoAspecto.Grupo == 22 || TipoAspecto.Grupo == 23 || TipoAspecto.Grupo == 24)
                        {
                            valoracion.origen = int.Parse(collection["ctl00$MainContent$ddlOrigen"].ToString());
                        }

                        #endregion

                        #region calculos
                        decimal valorrelativoAnio1 = -1;
                        decimal valorrelativoAnio2 = -1;
                        decimal valorrelativoAnio3 = -1;
                        decimal valorrelativoAnio4 = -1;
                        decimal valorrelativoAnio5 = -1;
                        decimal valorrelativoAnio6 = -1;
                        #region relativoMwhb
                        if (TipoAspecto.relativoMwhb == true)
                        {
                            if (valoracion.anio1 != null && Parametros.Mwhb1 != null)
                                valorrelativoAnio1 = decimal.Parse(valoracion.anio1.ToString()) / decimal.Parse(Parametros.Mwhb1.ToString());

                            if (valoracion.anio2 != null && Parametros.Mwhb2 != null)
                                valorrelativoAnio2 = decimal.Parse(valoracion.anio2.ToString()) / decimal.Parse(Parametros.Mwhb2.ToString());

                            if (valoracion.anio3 != null && Parametros.Mwhb3 != null)
                                valorrelativoAnio3 = decimal.Parse(valoracion.anio3.ToString()) / decimal.Parse(Parametros.Mwhb3.ToString());

                            if (valoracion.anio4 != null && Parametros.Mwhb4 != null)
                                valorrelativoAnio4 = decimal.Parse(valoracion.anio4.ToString()) / decimal.Parse(Parametros.Mwhb4.ToString());

                            if (valoracion.anio5 != null && Parametros.Mwhb5 != null)
                                valorrelativoAnio5 = decimal.Parse(valoracion.anio5.ToString()) / decimal.Parse(Parametros.Mwhb5.ToString());

                            if (valoracion.anio6 != null && Parametros.Mwhb6 != null)
                                valorrelativoAnio6 = decimal.Parse(valoracion.anio6.ToString()) / decimal.Parse(Parametros.Mwhb6.ToString());
                        }
                        #endregion
                        #region relativom3Hora
                        if (TipoAspecto.relativom3Hora == true)
                        {
                            if (valoracion.anio1 != null && Parametros.m3Hora1 != null)
                                valorrelativoAnio1 = decimal.Parse(valoracion.anio1.ToString()) / decimal.Parse(Parametros.m3Hora1.ToString());

                            if (valoracion.anio2 != null && Parametros.m3Hora2 != null)
                                valorrelativoAnio2 = decimal.Parse(valoracion.anio2.ToString()) / decimal.Parse(Parametros.m3Hora2.ToString());

                            if (valoracion.anio3 != null && Parametros.m3Hora3 != null)
                                valorrelativoAnio3 = decimal.Parse(valoracion.anio3.ToString()) / decimal.Parse(Parametros.m3Hora3.ToString());

                            if (valoracion.anio4 != null && Parametros.m3Hora4 != null)
                                valorrelativoAnio4 = decimal.Parse(valoracion.anio4.ToString()) / decimal.Parse(Parametros.m3Hora4.ToString());

                            if (valoracion.anio5 != null && Parametros.m3Hora5 != null)
                                valorrelativoAnio5 = decimal.Parse(valoracion.anio5.ToString()) / decimal.Parse(Parametros.m3Hora5.ToString());

                            if (valoracion.anio6 != null && Parametros.m3Hora6 != null)
                                valorrelativoAnio6 = decimal.Parse(valoracion.anio6.ToString()) / decimal.Parse(Parametros.m3Hora6.ToString());
                        }
                        #endregion
                        #region relativokmAnio
                        if (TipoAspecto.relativokmAnio == true)
                        {
                            if (valoracion.anio1 != null && Parametros.kmAnio1 != null)
                                valorrelativoAnio1 = decimal.Parse(valoracion.anio1.ToString()) / decimal.Parse(Parametros.kmAnio1.ToString());

                            if (valoracion.anio2 != null && Parametros.kmAnio2 != null)
                                valorrelativoAnio2 = decimal.Parse(valoracion.anio2.ToString()) / decimal.Parse(Parametros.kmAnio2.ToString());

                            if (valoracion.anio3 != null && Parametros.kmAnio3 != null)
                                valorrelativoAnio3 = decimal.Parse(valoracion.anio3.ToString()) / decimal.Parse(Parametros.kmAnio3.ToString());

                            if (valoracion.anio4 != null && Parametros.kmAnio4 != null)
                                valorrelativoAnio4 = decimal.Parse(valoracion.anio4.ToString()) / decimal.Parse(Parametros.kmAnio4.ToString());

                            if (valoracion.anio5 != null && Parametros.kmAnio5 != null)
                                valorrelativoAnio5 = decimal.Parse(valoracion.anio5.ToString()) / decimal.Parse(Parametros.kmAnio5.ToString());

                            if (valoracion.anio6 != null && Parametros.kmAnio6 != null)
                                valorrelativoAnio6 = decimal.Parse(valoracion.anio6.ToString()) / decimal.Parse(Parametros.kmAnio6.ToString());
                        }
                        #endregion
                        #region relativohfuncAnio
                        if (TipoAspecto.relativohfuncAnio == true)
                        {
                            if (valoracion.anio1 != null && Parametros.hfuncAnio1 != null)
                                valorrelativoAnio1 = decimal.Parse(valoracion.anio1.ToString()) / decimal.Parse(Parametros.hfuncAnio1.ToString());

                            if (valoracion.anio2 != null && Parametros.hfuncAnio2 != null)
                                valorrelativoAnio2 = decimal.Parse(valoracion.anio2.ToString()) / decimal.Parse(Parametros.hfuncAnio2.ToString());

                            if (valoracion.anio3 != null && Parametros.hfuncAnio3 != null)
                                valorrelativoAnio3 = decimal.Parse(valoracion.anio3.ToString()) / decimal.Parse(Parametros.hfuncAnio3.ToString());

                            if (valoracion.anio4 != null && Parametros.hfuncAnio4 != null)
                                valorrelativoAnio4 = decimal.Parse(valoracion.anio4.ToString()) / decimal.Parse(Parametros.hfuncAnio4.ToString());

                            if (valoracion.anio5 != null && Parametros.hfuncAnio5 != null)
                                valorrelativoAnio5 = decimal.Parse(valoracion.anio5.ToString()) / decimal.Parse(Parametros.hfuncAnio5.ToString());

                            if (valoracion.anio6 != null && Parametros.hfuncAnio6 != null)
                                valorrelativoAnio6 = decimal.Parse(valoracion.anio6.ToString()) / decimal.Parse(Parametros.hfuncAnio6.ToString());
                        }
                        #endregion
                        #region relativohAnioGE
                        if (TipoAspecto.relativohAnioGE == true)
                        {
                            if (valoracion.anio1 != null && Parametros.hAnioGE1 != null)
                                valorrelativoAnio1 = decimal.Parse(valoracion.anio1.ToString()) / decimal.Parse(Parametros.hAnioGE1.ToString());

                            if (valoracion.anio2 != null && Parametros.hAnioGE2 != null)
                                valorrelativoAnio2 = decimal.Parse(valoracion.anio2.ToString()) / decimal.Parse(Parametros.hAnioGE2.ToString());

                            if (valoracion.anio3 != null && Parametros.hAnioGE3 != null)
                                valorrelativoAnio3 = decimal.Parse(valoracion.anio3.ToString()) / decimal.Parse(Parametros.hAnioGE3.ToString());

                            if (valoracion.anio4 != null && Parametros.hAnioGE4 != null)
                                valorrelativoAnio4 = decimal.Parse(valoracion.anio4.ToString()) / decimal.Parse(Parametros.hAnioGE4.ToString());

                            if (valoracion.anio5 != null && Parametros.hAnioGE5 != null)
                                valorrelativoAnio5 = decimal.Parse(valoracion.anio5.ToString()) / decimal.Parse(Parametros.hAnioGE5.ToString());

                            if (valoracion.anio6 != null && Parametros.hAnioGE6 != null)
                                valorrelativoAnio6 = decimal.Parse(valoracion.anio6.ToString()) / decimal.Parse(Parametros.hAnioGE6.ToString());
                        }
                        #endregion

                        #region relativoNumTrabajadores
                        if (TipoAspecto.relativonumtrab == true)
                        {
                            if (valoracion.anio1 != null && Parametros.numtrabAnio1 != null)
                                valorrelativoAnio1 = decimal.Parse(valoracion.anio1.ToString()) / decimal.Parse(Parametros.numtrabAnio1.ToString());

                            if (valoracion.anio2 != null && Parametros.numtrabAnio2 != null)
                                valorrelativoAnio2 = decimal.Parse(valoracion.anio2.ToString()) / decimal.Parse(Parametros.numtrabAnio2.ToString());

                            if (valoracion.anio3 != null && Parametros.numtrabAnio3 != null)
                                valorrelativoAnio3 = decimal.Parse(valoracion.anio3.ToString()) / decimal.Parse(Parametros.numtrabAnio3.ToString());

                            if (valoracion.anio4 != null && Parametros.numtrabAnio4 != null)
                                valorrelativoAnio4 = decimal.Parse(valoracion.anio4.ToString()) / decimal.Parse(Parametros.numtrabAnio4.ToString());

                            if (valoracion.anio5 != null && Parametros.numtrabAnio5 != null)
                                valorrelativoAnio5 = decimal.Parse(valoracion.anio5.ToString()) / decimal.Parse(Parametros.numtrabAnio5.ToString());

                            if (valoracion.anio6 != null && Parametros.numtrabAnio6 != null)
                                valorrelativoAnio6 = decimal.Parse(valoracion.anio6.ToString()) / decimal.Parse(Parametros.numtrabAnio6.ToString());
                        }
                        #endregion

                        #region relativoAguaDesalada
                        if (TipoAspecto.relativom3aguadeitsalada == true)
                        {
                            if (valoracion.anio1 != null && Parametros.m3aguadesaladaAnio1 != null)
                                valorrelativoAnio1 = decimal.Parse(valoracion.anio1.ToString()) / decimal.Parse(Parametros.m3aguadesaladaAnio1.ToString());

                            if (valoracion.anio2 != null && Parametros.m3aguadesaladaAnio2 != null)
                                valorrelativoAnio2 = decimal.Parse(valoracion.anio2.ToString()) / decimal.Parse(Parametros.m3aguadesaladaAnio2.ToString());

                            if (valoracion.anio3 != null && Parametros.m3aguadesaladaAnio3 != null)
                                valorrelativoAnio3 = decimal.Parse(valoracion.anio3.ToString()) / decimal.Parse(Parametros.m3aguadesaladaAnio3.ToString());

                            if (valoracion.anio4 != null && Parametros.m3aguadesaladaAnio4 != null)
                                valorrelativoAnio4 = decimal.Parse(valoracion.anio4.ToString()) / decimal.Parse(Parametros.m3aguadesaladaAnio4.ToString());

                            if (valoracion.anio5 != null && Parametros.m3aguadesaladaAnio5 != null)
                                valorrelativoAnio5 = decimal.Parse(valoracion.anio5.ToString()) / decimal.Parse(Parametros.m3aguadesaladaAnio5.ToString());

                            if (valoracion.anio6 != null && Parametros.m3aguadesaladaAnio6 != null)
                                valorrelativoAnio6 = decimal.Parse(valoracion.anio6.ToString()) / decimal.Parse(Parametros.m3aguadesaladaAnio6.ToString());
                        }
                        #endregion

                        #region relativoTrabCantera
                        if (TipoAspecto.relativotrabcantera == true)
                        {
                            if (valoracion.anio1 != null && Parametros.trabcanteraAnio1 != null)
                                valorrelativoAnio1 = decimal.Parse(valoracion.anio1.ToString()) / decimal.Parse(Parametros.trabcanteraAnio1.ToString());

                            if (valoracion.anio2 != null && Parametros.trabcanteraAnio2 != null)
                                valorrelativoAnio2 = decimal.Parse(valoracion.anio2.ToString()) / decimal.Parse(Parametros.trabcanteraAnio2.ToString());

                            if (valoracion.anio3 != null && Parametros.trabcanteraAnio3 != null)
                                valorrelativoAnio3 = decimal.Parse(valoracion.anio3.ToString()) / decimal.Parse(Parametros.trabcanteraAnio3.ToString());

                            if (valoracion.anio4 != null && Parametros.trabcanteraAnio4 != null)
                                valorrelativoAnio4 = decimal.Parse(valoracion.anio4.ToString()) / decimal.Parse(Parametros.trabcanteraAnio4.ToString());

                            if (valoracion.anio5 != null && Parametros.trabcanteraAnio5 != null)
                                valorrelativoAnio5 = decimal.Parse(valoracion.anio5.ToString()) / decimal.Parse(Parametros.trabcanteraAnio5.ToString());

                            if (valoracion.anio6 != null && Parametros.trabcanteraAnio6 != null)
                                valorrelativoAnio6 = decimal.Parse(valoracion.anio6.ToString()) / decimal.Parse(Parametros.trabcanteraAnio6.ToString());
                        }
                        #endregion

                        #region norelativo

                        if (TipoAspecto.relativono == true)
                        {
                            if (valoracion.anio1 != null)
                                valorrelativoAnio1 = decimal.Parse(valoracion.anio1.ToString());

                            if (valoracion.anio2 != null)
                                valorrelativoAnio2 = decimal.Parse(valoracion.anio2.ToString());

                            if (valoracion.anio3 != null)
                                valorrelativoAnio3 = decimal.Parse(valoracion.anio3.ToString());

                            if (valoracion.anio4 != null)
                                valorrelativoAnio4 = decimal.Parse(valoracion.anio4.ToString());

                            if (valoracion.anio5 != null)
                                valorrelativoAnio5 = decimal.Parse(valoracion.anio5.ToString());

                            if (valoracion.anio6 != null)
                                valorrelativoAnio6 = decimal.Parse(valoracion.anio6.ToString());

                        }
                        #endregion

                        decimal sumavaloresrelativos = 0;
                        int cantidadvalores = 0;

                        if (valorrelativoAnio1 != -1)
                        {
                            sumavaloresrelativos = sumavaloresrelativos + valorrelativoAnio1;
                            cantidadvalores++;
                        }

                        if (valorrelativoAnio2 != -1)
                        {
                            sumavaloresrelativos = sumavaloresrelativos + valorrelativoAnio2;
                            cantidadvalores++;
                        }

                        if (valorrelativoAnio3 != -1)
                        {
                            sumavaloresrelativos = sumavaloresrelativos + valorrelativoAnio3;
                            cantidadvalores++;
                        }

                        if (valorrelativoAnio4 != -1)
                        {
                            sumavaloresrelativos = sumavaloresrelativos + valorrelativoAnio4;
                            cantidadvalores++;
                        }

                        if (valorrelativoAnio5 != -1)
                        {
                            sumavaloresrelativos = sumavaloresrelativos + valorrelativoAnio5;
                            cantidadvalores++;
                        }

                        decimal valorpromediorelativoaniosanteriores = 0;

                        if (cantidadvalores != 0)
                            valorpromediorelativoaniosanteriores = sumavaloresrelativos / cantidadvalores;

                        //Variación
                        if (TipoAspecto.Grupo == 1 || TipoAspecto.Grupo == 2 || TipoAspecto.Grupo == 3 || TipoAspecto.Grupo == 4 || TipoAspecto.Grupo == 5 || TipoAspecto.Grupo == 6 || TipoAspecto.Grupo == 7 || TipoAspecto.Grupo == 9 || TipoAspecto.Grupo == 10 || TipoAspecto.Grupo == 11 || TipoAspecto.Grupo == 12 || TipoAspecto.Grupo == 14 || TipoAspecto.Grupo == 15 || TipoAspecto.Grupo == 16 || TipoAspecto.Grupo == 20 || TipoAspecto.Grupo == 21)
                        {
                            if (valorpromediorelativoaniosanteriores > 0)
                                valoracion.variacion = ((valorrelativoAnio6 * 100) / valorpromediorelativoaniosanteriores) - 100;
                            else
                                valoracion.variacion = 100;
                        }
                        //Acercamiento

                        if (TipoAspecto.Grupo == 13 || TipoAspecto.Grupo == 14)
                        {
                            #region acercamientoruidoexterior
                            int diferenciaDia = 30000;
                            int diferenciaTarde = 30000;
                            int diferenciaNoche = 30000;

                            if (collection["ctl00$MainContent$txtDia"] != null && collection["ctl00$MainContent$txtRefDia"] != null)
                            {
                                valoracion.RU_Dia = int.Parse(collection["ctl00$MainContent$txtDia"].Replace(".", ","));
                                valoracion.RU_DiaRef = int.Parse(collection["ctl00$MainContent$txtRefDia"].Replace(".", ","));
                                diferenciaDia = int.Parse(valoracion.RU_DiaRef.ToString()) - int.Parse(valoracion.RU_Dia.ToString());
                            }
                            if (collection["ctl00$MainContent$txtTarde"] != null && collection["ctl00$MainContent$txtRefTarde"] != null)
                            {
                                valoracion.RU_Tarde = int.Parse(collection["ctl00$MainContent$txtTarde"].Replace(".", ","));
                                valoracion.RU_TardeRef = int.Parse(collection["ctl00$MainContent$txtRefTarde"].Replace(".", ","));
                                diferenciaTarde = int.Parse(valoracion.RU_TardeRef.ToString()) - int.Parse(valoracion.RU_Tarde.ToString());
                            }
                            if (collection["ctl00$MainContent$txtNoche"] != null && collection["ctl00$MainContent$txtRefNoche"] != null)
                            {
                                valoracion.RU_Noche = int.Parse(collection["ctl00$MainContent$txtNoche"].Replace(".", ","));
                                valoracion.RU_NocheRef = int.Parse(collection["ctl00$MainContent$txtRefNoche"].Replace(".", ","));
                                diferenciaNoche = int.Parse(valoracion.RU_NocheRef.ToString()) - int.Parse(valoracion.RU_Noche.ToString());
                            }
                            if ((diferenciaDia <= diferenciaTarde) && (diferenciaDia <= diferenciaNoche) && diferenciaDia != 30000)
                            {
                                valoracion.acercamiento = diferenciaDia;
                            }
                            if ((diferenciaTarde <= diferenciaDia) && (diferenciaTarde <= diferenciaNoche) && diferenciaTarde != 30000)
                            {
                                valoracion.acercamiento = diferenciaTarde;
                            }
                            if ((diferenciaNoche <= diferenciaDia) && (diferenciaNoche <= diferenciaTarde) && diferenciaNoche != 30000)
                            {
                                valoracion.acercamiento = diferenciaNoche;
                            }
                            #endregion
                        }
                        if (TipoAspecto.Grupo == 1 || TipoAspecto.Grupo == 2 || TipoAspecto.Grupo == 3 || TipoAspecto.Grupo == 4 || TipoAspecto.Grupo == 5 || TipoAspecto.Grupo == 6 || TipoAspecto.Grupo == 7 || TipoAspecto.Grupo == 8 || TipoAspecto.Grupo == 9 || TipoAspecto.Grupo == 10 || TipoAspecto.Grupo == 11 || TipoAspecto.Grupo == 12 || TipoAspecto.Grupo == 14 || TipoAspecto.Grupo == 15 || TipoAspecto.Grupo == 16 || TipoAspecto.Grupo == 20 || TipoAspecto.Grupo == 21)
                        {
                            if (valoracion.referencia != null && valoracion.referencia != 0)
                            {
                                valoracion.acercamiento = (valoracion.datoabsoluto * 100 / valoracion.referencia);
                            }

                            if (TipoAspecto.Grupo == 8)
                            {
                                if (valorpromediorelativoaniosanteriores == 0)
                                {
                                    valoracion.acercamiento = 100;
                                }
                                else
                                {
                                    valoracion.acercamiento = ((valorrelativoAnio6 * 100 / valorpromediorelativoaniosanteriores) - 100);
                                }
                            }
                        }

                        //MAGNITUD
                        decimal MAGNITUD = 3;
                        //decimal valorpromedioaniosanteriores = (decimal.Parse(valoracion.anio1.ToString()) + decimal.Parse(valoracion.anio2.ToString()) + decimal.Parse(valoracion.anio3.ToString()) + decimal.Parse(valoracion.anio4.ToString()) + decimal.Parse(valoracion.anio5.ToString())) / 5;
                        //decimal valorpromedioaniosanteriores5porciento = valorpromedioaniosanteriores + ((valorpromedioaniosanteriores / 100) * 5);
                        //decimal valorpromedioaniosanteriores2porciento = valorpromedioaniosanteriores + ((valorpromedioaniosanteriores / 100) * 2);
                        if (TipoAspecto.Grupo == 1 || TipoAspecto.Grupo == 3 || TipoAspecto.Grupo == 4 || TipoAspecto.Grupo == 5 || TipoAspecto.Grupo == 6 || TipoAspecto.Grupo == 7 || TipoAspecto.Grupo == 9 || TipoAspecto.Grupo == 10 || TipoAspecto.Grupo == 11 || TipoAspecto.Grupo == 12 || TipoAspecto.Grupo == 14 || TipoAspecto.Grupo == 15 || TipoAspecto.Grupo == 16 || TipoAspecto.Grupo == 20 || TipoAspecto.Grupo == 21)
                        {
                            if (valoracion.variacion < 0)
                                MAGNITUD = 1;
                            if (valoracion.variacion >= 0 && valoracion.variacion <= 5)
                                MAGNITUD = 2;
                            if (valoracion.variacion > 5)
                                MAGNITUD = 3;
                        }
                        if (TipoAspecto.Grupo == 2)
                        {
                            if (valoracion.variacion < 0)
                                MAGNITUD = 1;
                            if (valoracion.variacion >= 0 && valoracion.variacion <= 2)
                                MAGNITUD = 2;
                            if (valoracion.variacion > 2)
                                MAGNITUD = 3;
                        }
                        if (TipoAspecto.Grupo == 8)
                        {
                            decimal totalresiduos = Datos.CalcularTotalResiduosCentral(idCentral);
                            if (((Datos.GetDatosValoracion(id)).anio6 == null || Datos.GetDatosValoracion(id).anio6 == 0) && valoracion.anio6 != null)
                                totalresiduos = totalresiduos + decimal.Parse(valoracion.anio6.ToString());
                            
                            valoracion.acercamiento = totalresiduos;

                            decimal resultado = 100;
                            if (valoracion.anio1 == null && valoracion.anio2 == null && valoracion.anio3 == null && valoracion.anio4 == null && valoracion.anio5 == null &&
                                valoracion.anio6 != null)
                            {
                                valoracion.variacion = resultado;
                            }
                            else
                            {
                                if (totalresiduos > 0)
                                {
                                    resultado = (100 / totalresiduos) * decimal.Parse(valoracion.anio6.ToString());
                                    valoracion.variacion = resultado;
                                }
                            }

                            if (valoracion.variacion > 5)
                            {
                                MAGNITUD = 3;
                            }
                            else
                                if (valoracion.variacion > decimal.Parse("0,5"))
                                    MAGNITUD = 2;
                                else
                                    MAGNITUD = 1;
                                    

                            List<aspecto_valoracion> listadoResiduos = Datos.ListarAspectosResiduos(centroseleccionado.id);

                            foreach (aspecto_valoracion asp_res in listadoResiduos)
                            {
                                if (asp_res.id != valoracion.id && asp_res.anio6 != null)
                                {
                                    decimal totalresiduosrec = Datos.CalcularTotalResiduosCentral(idCentral);
                                    if (((Datos.GetDatosValoracion(id)).anio6 == null || Datos.GetDatosValoracion(id).anio6 == 0) && valoracion.anio6 != null)
                                        totalresiduosrec = totalresiduosrec + decimal.Parse(valoracion.anio6.ToString());

                                    asp_res.acercamiento = totalresiduosrec;

                                    //decimal valortotalresiduos05rec = ((totalresiduosrec / 100) * (5 / 10));
                                    //decimal valortotalresiduos5rec = ((totalresiduosrec / 100) * 5);
                                    decimal resultadorec = 100;

                                    if (asp_res.anio1 == null && asp_res.anio2 == null && asp_res.anio3 == null && asp_res.anio4 == null && asp_res.anio5 == null &&
                                    asp_res.anio6 != null)
                                    {
                                        asp_res.variacion = resultadorec;
                                    }
                                    else
                                    {
                                        if (totalresiduosrec > 0)
                                        {
                                            resultadorec = (100 / totalresiduosrec) * decimal.Parse(asp_res.anio6.ToString());
                                            asp_res.variacion = resultadorec;
                                        }
                                    }

                                    if (asp_res.variacion > 5)
                                    {
                                        asp_res.magnitud = 3;
                                        asp_res.resmagnitud = 3;
                                    }
                                    else
                                    {
                                        if (asp_res.variacion > decimal.Parse("0,5"))
                                        {
                                            asp_res.magnitud = 2;
                                            asp_res.resmagnitud = 2;
                                        }
                                        else
                                        {
                                            asp_res.magnitud = 1;
                                            asp_res.resmagnitud = 1;
                                        }
                                    }


                                    if (asp_res.anio6 == 0)
                                    {
                                        asp_res.significancia6 = 0;
                                    }
                                    else
                                    {
                                        if (asp_res.magnitud + asp_res.naturaleza + asp_res.origen >= 7 && asp_res.anio6 > 0)
                                            asp_res.significancia6 = 1;
                                        else
                                            asp_res.significancia6 = 0;
                                    }

                                    Datos.ActualizarValoracion(asp_res);
                                }
                            }


                        }
                        if (TipoAspecto.Grupo == 13 || TipoAspecto.Grupo == 17 || TipoAspecto.Grupo == 18 || TipoAspecto.Grupo == 19 || TipoAspecto.Grupo == 22 || TipoAspecto.Grupo == 23 || TipoAspecto.Grupo == 24)
                        {
                            MAGNITUD = int.Parse(valoracion.magnitud.ToString());
                        }

                        //NATURALEZA
                        decimal NATURALEZA = 3;

                        if (TipoAspecto.Grupo == 1 || TipoAspecto.Grupo == 6 || TipoAspecto.Grupo == 20)
                        {
                            if (valoracion.acercamiento <= 40)
                                NATURALEZA = 1;
                            if (valoracion.acercamiento > 40 && valoracion.acercamiento <= 80)
                                NATURALEZA = 2;
                            if (valoracion.acercamiento > 80)
                                NATURALEZA = 3;
                        }
                        if (TipoAspecto.Grupo == 2 || TipoAspecto.Grupo == 3 || TipoAspecto.Grupo == 4 || TipoAspecto.Grupo == 5 || TipoAspecto.Grupo == 7 || TipoAspecto.Grupo == 8 || TipoAspecto.Grupo == 9 || TipoAspecto.Grupo == 10 || TipoAspecto.Grupo == 11 || (TipoAspecto.Grupo == 12 && valoracion.foco != 1) || TipoAspecto.Grupo == 15 || TipoAspecto.Grupo == 16 || TipoAspecto.Grupo == 17 || TipoAspecto.Grupo == 18 || TipoAspecto.Grupo == 19 || TipoAspecto.Grupo == 22 || TipoAspecto.Grupo == 23 || TipoAspecto.Grupo == 24)
                        {
                            NATURALEZA = int.Parse(valoracion.naturaleza.ToString());
                        }
                        if (TipoAspecto.Grupo == 12 && valoracion.foco == 1)
                        {
                           List<aspecto_parametro_valoracion> listaParametros = Datos.ListarParametrosAsignados(valoracion.id);

                           if (listaParametros.Count > 0 && listaParametros.Max(x => x.resnaturaleza) != null)
                            NATURALEZA = decimal.Parse(listaParametros.Max(x => x.resnaturaleza).ToString());
                        }
                        if (TipoAspecto.Grupo == 21)
                        {
                            if (valoracion.acercamiento >= 2)
                                NATURALEZA = 1;
                            if (valoracion.acercamiento < 2 && valoracion.acercamiento > 1)
                                NATURALEZA = 2;
                            if (valoracion.acercamiento <= 1)
                                NATURALEZA = 3;
                        }
                        if (TipoAspecto.Grupo == 13 || TipoAspecto.Grupo == 14)
                        {
                            if (int.Parse(valoracion.acercamiento.ToString()) <= 5)
                            {
                                NATURALEZA = 3;
                            }
                            if (int.Parse(valoracion.acercamiento.ToString()) > 5 && int.Parse(valoracion.acercamiento.ToString()) <= 10)
                            {
                                NATURALEZA = 2;
                            }
                            if (int.Parse(valoracion.acercamiento.ToString()) > 10)
                            {
                                NATURALEZA = 1;
                            }
                        }

                        //ORIGEN
                        decimal ORIGEN = 3;
                        if (TipoAspecto.Grupo == 10 || TipoAspecto.Grupo == 11 || TipoAspecto.Grupo == 15 || TipoAspecto.Grupo == 18 || TipoAspecto.Grupo == 19 || TipoAspecto.Grupo == 20 || TipoAspecto.Grupo == 21)
                        {
                            ORIGEN = (MAGNITUD + NATURALEZA) / 2;
                        }

                        if (TipoAspecto.Grupo == 1 || TipoAspecto.Grupo == 2 || TipoAspecto.Grupo == 3 || TipoAspecto.Grupo == 4 || TipoAspecto.Grupo == 5 || TipoAspecto.Grupo == 6 || TipoAspecto.Grupo == 7 || TipoAspecto.Grupo == 8 || TipoAspecto.Grupo == 13 || TipoAspecto.Grupo == 14 || TipoAspecto.Grupo == 16 || TipoAspecto.Grupo == 17 || TipoAspecto.Grupo == 22 || TipoAspecto.Grupo == 23 || TipoAspecto.Grupo == 24)
                        {
                            ORIGEN = int.Parse(valoracion.origen.ToString());
                        }
                        if (TipoAspecto.Grupo == 9)
                        {
                            decimal totalconsumocombustible = Datos.CalcularTotalConsumoCombustibleCentral(idCentral);
                            //decimal valortotalconsumocombustible05 = ((totalconsumocombustible / 100) * (5 / 10));
                            //decimal valortotalconsumocombustible5 = ((totalconsumocombustible / 100) * 5);
                            if (((Datos.GetDatosValoracion(id)).anio6 == null || Datos.GetDatosValoracion(id).anio6 == 0) && valoracion.anio6 != null)
                                totalconsumocombustible = totalconsumocombustible + decimal.Parse(valoracion.anio6.ToString());
                            /*Definimos variables*/
                            decimal total3 = totalconsumocombustible;

                            if (valoracion.anio6 != null)
                            {
                                decimal resultado = (decimal.Parse(valoracion.anio6.ToString()) * 100) / total3;
                                valoracion.acercamiento = resultado;
                            }

                            if (valoracion.acercamiento > 5)
                            {
                                ORIGEN = 3;
                            }
                            else
                            {
                                if (valoracion.acercamiento > decimal.Parse("0,5"))
                                {
                                    ORIGEN = 2;
                                }
                                else
                                {
                                    ORIGEN = 1;
                                }
                            }
                        }
                        if (TipoAspecto.Grupo == 12)
                        {
                            decimal totalconsumosustancias = Datos.CalcularTotalConsumoSustanciasCentral(idCentral);
                            ///decimal valortotalconsumosustancias05 = ((totalconsumosustancias / 100) * (5 / 10));
                            //decimal valortotalconsumosustancias5 = ((totalconsumosustancias / 100) * 5);

                            if (((Datos.GetDatosValoracion(id)).anio6 == null || Datos.GetDatosValoracion(id).anio6 == 0) && valoracion.anio6 != null)
                                totalconsumosustancias = totalconsumosustancias + decimal.Parse(valoracion.anio6.ToString());

                            decimal total3 = totalconsumosustancias;
                            if (valoracion.anio6 != null)
                            {
                                decimal resultado = (decimal.Parse(valoracion.anio6.ToString()) * 100) / total3;
                                valoracion.acercamiento = resultado;
                            }

                            if (valoracion.acercamiento > 5)
                            {
                                ORIGEN = 3;
                            }
                            else
                            {
                                if (valoracion.acercamiento > decimal.Parse("0,5"))
                                {
                                    ORIGEN = 2;
                                }
                                else
                                {
                                    ORIGEN = 1;
                                }
                            }

                            //if (valoracion.anio6 <= valortotalconsumosustancias05)
                            //{
                            //    ORIGEN = 1;
                            //}

                            //if (valoracion.anio6 > valortotalconsumosustancias05 && valoracion.anio6 < valortotalconsumosustancias5)
                            //{
                            //    ORIGEN = 2;
                            //}

                            //if (valoracion.anio6 >= valortotalconsumosustancias5)
                            //{
                            //    ORIGEN = 3;
                            //}
                        }

                        //SIGNIFICANCIA
                        int SIGNIFICANCIA = 0;
                        if (TipoAspecto.Grupo == 8 && valoracion.anio6 == 0)
                        {
                            SIGNIFICANCIA = 0;
                        }
                        else
                        {
                            if (MAGNITUD + NATURALEZA + ORIGEN >= 7)
                                SIGNIFICANCIA = 1;
                        }

                        if (TipoAspecto.Grupo == 23 && TipoAspecto.Codigo == "AP-9" && collection["ctl00$MainContent$ddlMagnitud"].ToString() == "4")
                            SIGNIFICANCIA = 1;

                        if (TipoAspecto.Grupo != 8 && TipoAspecto.Grupo != 9 && TipoAspecto.Grupo != 12 && valoracion.acercamiento != null && valoracion.acercamiento > 100)
                        {
                            SIGNIFICANCIA = 1;
                        }

                        #endregion
                        valoracion.resmagnitud = MAGNITUD;
                        valoracion.resnaturaleza = NATURALEZA;
                        valoracion.resorigen = ORIGEN;
                        valoracion.significancia6 = SIGNIFICANCIA;
                        if (collection["ctl00$MainContent$txtDescripcion"] != null)
                            valoracion.descripcion = collection["ctl00$MainContent$txtDescripcion"];

                        if (collection["ctl00$MainContent$ddlQueja"] != null)
                            valoracion.queja = int.Parse(collection["ctl00$MainContent$ddlQueja"]);
                        if (collection["ctl00$MainContent$txtObsQueja"] != null)
                            valoracion.quejaobs = collection["ctl00$MainContent$txtObsQueja"];
                        if (collection["ctl00$MainContent$ddlQuejaId"] != null)
                            valoracion.idqueja = int.Parse(collection["ctl00$MainContent$ddlQuejaId"]);

                        if (valoracion.queja == 1)
                            valoracion.significancia6 = 1;

                        Datos.ActualizarValoracion(valoracion);
                    }
                    else
                    {
                        if (collection["ctl00$MainContent$txtDescripcion"] != null)
                            valoracion.descripcion = collection["ctl00$MainContent$txtDescripcion"];

                        if (collection["ctl00$MainContent$ddlQueja"] != null)
                            valoracion.queja = int.Parse(collection["ctl00$MainContent$ddlQueja"]);
                        if (collection["ctl00$MainContent$txtObsQueja"] != null)
                            valoracion.quejaobs = collection["ctl00$MainContent$txtObsQueja"];
                        if (collection["ctl00$MainContent$ddlQuejaId"] != null && valoracion.queja == 1)
                            valoracion.idqueja = int.Parse(collection["ctl00$MainContent$ddlQuejaId"]);

                        List<aspecto_parametro_valoracion> listaparametros = Datos.ListarParametrosAsignados(valoracion.id);

                        if (listaparametros.Count > 0)
                        {
                            valoracion.resmagnitud = listaparametros.Select(x => x.resmagnitud).Max();
                            valoracion.resnaturaleza = listaparametros.Select(x => x.resnaturaleza).Max();
                            valoracion.resorigen = listaparametros.Select(x => x.resorigen).Max();
                        }

                        bool incumplimientolegal = false;
                        foreach (aspecto_parametro_valoracion val in listaparametros)
                        {
                            if (val.resmagnitud + val.resnaturaleza + val.resorigen < 7 && val.significancia6 == 1)
                                incumplimientolegal = true;
                        }

                        if (valoracion.queja == 1 || (valoracion.resmagnitud + valoracion.resnaturaleza + valoracion.resorigen) >= 7)
                            valoracion.significancia6 = 1;
                        else
                            valoracion.significancia6 = 0;

                        if (incumplimientolegal == true)
                            valoracion.significancia6 = 1;

                        Datos.ActualizarValoracion(valoracion);
                    }

                    #region lista de quejas
                    List<System.Web.UI.WebControls.ListItem> listado = new List<System.Web.UI.WebControls.ListItem>();
                    List<MIDAS.Models.VISTA_Comunicaciones> listaCom = MIDAS.Models.Datos.ListarQuejas(centroseleccionado.id);
                    foreach (MIDAS.Models.VISTA_Comunicaciones com in listaCom)
                    {
                        System.Web.UI.WebControls.ListItem nuevoItem = new System.Web.UI.WebControls.ListItem();
                        nuevoItem.Value = com.id.ToString();
                        if (com.fechainicio != null)
                        {
                            nuevoItem.Text = com.idcomunicacion + " - " + com.Clasificacion + " - " + DateTime.Parse(com.fechainicio.ToString()).Date.ToShortDateString();
                        }
                        else
                        {
                            nuevoItem.Text = com.idcomunicacion + " - " + com.Clasificacion;
                        }

                        listado.Add(nuevoItem);
                    }
                    ViewData["quejas"] = listado;
                    #endregion

                    aspecto_parametros buscarParametros = Datos.ObtenerParametrosCentral(idCentral);
                    ViewData["EditarParametros"] = buscarParametros;

                    aspecto_valoracion buscarValoracion = Datos.ObtenerValoracionAspecto(id);
                    ViewData["EditarAspectoValoracion"] = buscarValoracion;
                    aspecto_tipo buscarTipoAspecto = Datos.ObtenerTipoAspecto(int.Parse(buscarValoracion.idAspecto.ToString()));
                    ViewData["EditarTipoAspecto"] = buscarTipoAspecto;
                    ViewData["parametros"] = Datos.ListarParametros(buscarTipoAspecto.Grupo);
                    ViewData["parametrosAsignados"] = Datos.ListarParametrosAsignados(buscarValoracion.id);
                    return View();
                }
                catch (Exception ex)
                {
                    #region lista de quejas
                    List<System.Web.UI.WebControls.ListItem> listado = new List<System.Web.UI.WebControls.ListItem>();
                    List<MIDAS.Models.VISTA_Comunicaciones> listaCom = MIDAS.Models.Datos.ListarQuejas(centroseleccionado.id);
                    foreach (MIDAS.Models.VISTA_Comunicaciones com in listaCom)
                    {
                        System.Web.UI.WebControls.ListItem nuevoItem = new System.Web.UI.WebControls.ListItem();
                        nuevoItem.Value = com.id.ToString();
                        if (com.fechainicio != null)
                        {
                            nuevoItem.Text = com.idcomunicacion + " - " + com.Clasificacion + " - " + DateTime.Parse(com.fechainicio.ToString()).Date.ToShortDateString();
                        }
                        else
                        {
                            nuevoItem.Text = com.idcomunicacion + " - " + com.Clasificacion;
                        }

                        listado.Add(nuevoItem);
                    }
                    ViewData["quejas"] = listado;
                    #endregion

                    aspecto_parametros buscarParametros = Datos.ObtenerParametrosCentral(idCentral);
                    ViewData["EditarParametros"] = buscarParametros;

                    aspecto_valoracion buscarValoracion = Datos.ObtenerValoracionAspecto(id);
                    ViewData["EditarAspectoValoracion"] = buscarValoracion;
                    aspecto_tipo buscarTipoAspecto = Datos.ObtenerTipoAspecto(int.Parse(buscarValoracion.idAspecto.ToString()));
                    ViewData["EditarTipoAspecto"] = buscarTipoAspecto;
                    ViewData["parametros"] = Datos.ListarParametros(buscarTipoAspecto.Grupo);
                    ViewData["parametrosAsignados"] = Datos.ListarParametrosAsignados(buscarValoracion.id);
                    Session["EdicionIndicadorError"] = "No se han podido guardar los datos, compruebe que los datos introducidos son correctos.";
                    return View();
                }
                #endregion
            }
            if (formulario == "btnAddParametro")
            {
                #region añadir indicador

                    int idParametro = 0;
                    string nombre = string.Empty;
                    if (collection["ctl00$MainContent$ddlParametros"] != null)
                    {
                        idParametro = int.Parse(collection["ctl00$MainContent$ddlParametros"]);
                        aspecto_parametro param = Datos.ObtenerParametro(idParametro);
                        nombre = param.nombre;
                    }
                    else
                    {
                        nombre = collection["ctl00$MainContent$txtParametro"];
                    }
                    int idValoracion = id;
                    
                    Datos.CrearValoracionParametro(idValoracion, idParametro, nombre);

                    #region lista de quejas
                    List<System.Web.UI.WebControls.ListItem> listado = new List<System.Web.UI.WebControls.ListItem>();
                    List<MIDAS.Models.VISTA_Comunicaciones> listaCom = MIDAS.Models.Datos.ListarQuejas(centroseleccionado.id);
                    foreach (MIDAS.Models.VISTA_Comunicaciones com in listaCom)
                    {
                        System.Web.UI.WebControls.ListItem nuevoItem = new System.Web.UI.WebControls.ListItem();
                        nuevoItem.Value = com.id.ToString();
                        if (com.fechainicio != null)
                        {
                            nuevoItem.Text = com.idcomunicacion + " - " + com.Clasificacion + " - " + DateTime.Parse(com.fechainicio.ToString()).Date.ToShortDateString();
                        }
                        else
                        {
                            nuevoItem.Text = com.idcomunicacion + " - " + com.Clasificacion;
                        }

                        listado.Add(nuevoItem);
                    }
                    ViewData["quejas"] = listado;
                    #endregion

                aspecto_parametros buscarParametros = Datos.ObtenerParametrosCentral(idCentral);
                ViewData["EditarParametros"] = buscarParametros;

                aspecto_valoracion buscarValoracion = Datos.ObtenerValoracionAspecto(id);
                ViewData["EditarAspectoValoracion"] = buscarValoracion;
                aspecto_tipo buscarTipoAspecto = Datos.ObtenerTipoAspecto(int.Parse(buscarValoracion.idAspecto.ToString()));
                ViewData["EditarTipoAspecto"] = buscarTipoAspecto;
                ViewData["parametros"] = Datos.ListarParametros(buscarTipoAspecto.Grupo);
                ViewData["parametrosAsignados"] = Datos.ListarParametrosAsignados(buscarValoracion.id);

                Session["EdicionAspectoMensaje"] = "Parámetro asignado correctamente";
                return View();
                #endregion
            }
            else
            {
                #region recarga
                #region lista de quejas
                List<System.Web.UI.WebControls.ListItem> listado = new List<System.Web.UI.WebControls.ListItem>();
                List<MIDAS.Models.VISTA_Comunicaciones> listaCom = MIDAS.Models.Datos.ListarQuejas(centroseleccionado.id);
                foreach (MIDAS.Models.VISTA_Comunicaciones com in listaCom)
                {
                    System.Web.UI.WebControls.ListItem nuevoItem = new System.Web.UI.WebControls.ListItem();
                    nuevoItem.Value = com.id.ToString();
                    if (com.fechainicio != null)
                    {
                        nuevoItem.Text = com.idcomunicacion + " - " + com.Clasificacion + " - " + DateTime.Parse(com.fechainicio.ToString()).Date.ToShortDateString();
                    }
                    else
                    {
                        nuevoItem.Text = com.idcomunicacion + " - " + com.Clasificacion;
                    }

                    listado.Add(nuevoItem);
                }
                ViewData["quejas"] = listado;
                #endregion

                aspecto_parametros buscarParametros = Datos.ObtenerParametrosCentral(idCentral);
                ViewData["EditarParametros"] = buscarParametros;
                
                aspecto_valoracion buscarValoracion = Datos.ObtenerValoracionAspecto(id);
                #region mediciones
                if (collection["ctl00$MainContent$txtReferencia"] != null && collection["ctl00$MainContent$txtReferencia"] != string.Empty)
                    buscarValoracion.referencia = decimal.Parse(collection["ctl00$MainContent$txtReferencia"].ToString().Replace(".", ","));

                if (collection["ctl00$MainContent$txtAbsoluto"] != null && collection["ctl00$MainContent$txtAbsoluto"] != string.Empty)
                    buscarValoracion.datoabsoluto = decimal.Parse(collection["ctl00$MainContent$txtAbsoluto"].ToString().Replace(".", ","));

                if (collection["ctl00$MainContent$txtProveedor"] != null && collection["ctl00$MainContent$txtProveedor"] != string.Empty)
                    buscarValoracion.IN_Proveedor = collection["ctl00$MainContent$txtProveedor"].ToString();
                if (collection["ctl00$MainContent$txtServicio"] != null && collection["ctl00$MainContent$txtServicio"] != string.Empty)
                    buscarValoracion.IN_ServicioPrestado = collection["ctl00$MainContent$txtServicio"].ToString();
                if (collection["ctl00$MainContent$ddlTipoIndirecto"] != null && collection["ctl00$MainContent$ddlTipoIndirecto"] != string.Empty)
                    buscarValoracion.IN_TipoActividad = int.Parse(collection["ctl00$MainContent$ddlTipoIndirecto"].ToString());
                if (collection["ctl00$MainContent$ddlAspecto"] != null && collection["ctl00$MainContent$ddlAspecto"] != string.Empty)
                    buscarValoracion.IN_Aspecto = int.Parse(collection["ctl00$MainContent$ddlAspecto"].ToString());

                if (collection["ctl00$MainContent$txtMed1"] != null && collection["ctl00$MainContent$txtMed1"] != string.Empty)
                    buscarValoracion.anio1 = decimal.Parse(collection["ctl00$MainContent$txtMed1"].ToString().Replace(".", ","));

                if (collection["ctl00$MainContent$txtMed2"] != null && collection["ctl00$MainContent$txtMed2"] != string.Empty)
                    buscarValoracion.anio2 = decimal.Parse(collection["ctl00$MainContent$txtMed2"].ToString().Replace(".", ","));

                if (collection["ctl00$MainContent$txtMed3"] != null && collection["ctl00$MainContent$txtMed3"] != string.Empty)
                    buscarValoracion.anio3 = decimal.Parse(collection["ctl00$MainContent$txtMed3"].ToString().Replace(".", ","));

                if (collection["ctl00$MainContent$txtMed4"] != null && collection["ctl00$MainContent$txtMed4"] != string.Empty)
                    buscarValoracion.anio4 = decimal.Parse(collection["ctl00$MainContent$txtMed4"].ToString().Replace(".", ","));

                if (collection["ctl00$MainContent$txtMed5"] != null && collection["ctl00$MainContent$txtMed5"] != string.Empty)
                    buscarValoracion.anio5 = decimal.Parse(collection["ctl00$MainContent$txtMed5"].ToString().Replace(".", ","));

                #endregion
                ViewData["EditarAspectoValoracion"] = buscarValoracion;
                aspecto_tipo buscarTipoAspecto = Datos.ObtenerTipoAspecto(int.Parse(buscarValoracion.idAspecto.ToString()));
                ViewData["EditarTipoAspecto"] = buscarTipoAspecto;
                ViewData["parametros"] = Datos.ListarParametros(buscarTipoAspecto.Grupo);
                ViewData["parametrosAsignados"] = Datos.ListarParametrosAsignados(buscarValoracion.id);
                return View();
                #endregion
            }
        }

        public ActionResult eliminar_aspecto(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            Datos.EliminarAspectoValoracion(id);
            Session["EditarAspectosResultado"] = "Eliminado registro";
            return RedirectToAction("gestion_aspectos", "AspectosAmbientales");
        }

        public ActionResult eliminar_parametro(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            int idAspecto = Datos.EliminarParametroValoracion(id);
            Session["EditarAspectoResultado"] = "Eliminado registro";
            return RedirectToAction("detalle_aspecto/" + idAspecto.ToString(), "aspectosambientales");
        }

        public FileResult ObtenerCriterios()
        {
            try
            {
                ficheros IF = Datos.ObtenerCriteriosEvaluacion("EG-SIG-PGA-001.03");
                byte[] fileBytes = System.IO.File.ReadAllBytes(IF.enlace);
                string fileName = IF.enlace.Replace(Server.MapPath("~/Documentacion") + "\\" + IF.nombre_fichero, "");
                fileName = fileName.Replace(IF.nombre_fichero, "");
                fileName = fileName.Replace("á", "a");
                fileName = fileName.Replace("é", "e");
                fileName = fileName.Replace("í", "i");
                fileName = fileName.Replace("ó", "o");
                fileName = fileName.Replace("ú", "u");
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
