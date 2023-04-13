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
    public class RequisitosController : Controller
    {
        //
        // GET: /Requisitos/

        public ActionResult requisitos_legales()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            int idCentral = centroseleccionado.id;

            ViewData["requisitos"] = Datos.ListarRequisitos(idCentral);

            #region generacion fichero
            if (Session["nombreArchivo"] != null && Session["nombreArchivo"].ToString() != "")
            {
                if (Session["nombreArchivo"].ToString().Contains("xlsx"))
                {
                    Response.ContentType = "application/msexcel";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=ExportacionRequisitos.xlsx");
                    Response.TransmitFile(Session["nombreArchivo"].ToString());
                    Response.End();
                }
                Session["nombreArchivo"] = "";
            }
            #endregion

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult requisitos_legales(FormCollection collection)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            int idCentral = centroseleccionado.id;

            VISTA_ObtenerUsuario user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());

            #region generacionfichero
            string sourceFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaPlantillaWord"] + "ExportacionRequisitos.xlsx");
            string destinationFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaGeneracionWord"] + "ExportacionRequisitos_" + DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + ".xlsx");

            Session["source"] = sourceFile;
            Session["destino"] = destinationFile;
            // Create a copy of the template file and open the copy 
            System.IO.File.Copy(sourceFile, destinationFile, true);

            #region impresion
            using (SpreadsheetDocument document = SpreadsheetDocument.Open(destinationFile, true))
            {

                List<VISTA_RequisitosLegales> form = Datos.ListarRequisitos(idCentral);
                SheetData sheetData = document.WorkbookPart.WorksheetParts.ElementAt(0).Worksheet.Elements<SheetData>().First();
                foreach (VISTA_RequisitosLegales fo in form)
                {
                    Row row = new Row();

                    string anio = string.Empty;
                    string ambito = string.Empty;
                    string denominacion = string.Empty;
                    string informescump = string.Empty;
                    string fechareg = string.Empty;
                    string resultado = string.Empty;
                    string accionesmejora = string.Empty;

                    if (fo.anio == null)
                        anio = string.Empty;
                    else
                        anio = fo.anio.ToString();

                    if (fo.nombre_ambito == null)
                        ambito = string.Empty;
                    else
                        ambito = fo.nombre_ambito;

                    if (fo.denominacion == null)
                        denominacion = string.Empty;
                    else
                        denominacion = fo.denominacion;

                    if (fo.informeevaluacion == null)
                        informescump = string.Empty;
                    else
                        informescump = fo.informeevaluacion.Replace(Server.MapPath("~/Requisitos") + "\\" + fo.id + "\\", "");

                    if (fo.fecharegistro == null)
                        fechareg = string.Empty;
                    else
                        fechareg = fo.fecharegistro.ToString().Replace(" 0:00:00", "");

                    string cadenares = " Nº requisitos: " + fo.numrequisitos + "\r\n";
                    cadenares = cadenares + " Cumple: " + fo.cumple + "\r\n";
                    cadenares = cadenares + " En trámite: " + fo.tramite + "\r\n";
                    cadenares = cadenares + " No cumple: " + fo.nocumple + "\r\n";
                    cadenares = cadenares + " Observación: " + fo.observacion + "\r\n";
                    cadenares = cadenares + " No procede: " + fo.noprocede + "\r\n";
                    cadenares = cadenares + " No verificado: " + fo.noverificado + "\r\n";

                    resultado = cadenares;

                    List<VISTA_ListarAccionesMejora> listaacciones = Datos.ListarAccionesMejora(idCentral, 8, fo.id);

                    foreach (VISTA_ListarAccionesMejora acc in listaacciones)
                    {
                        accionesmejora = accionesmejora + acc.codigo + "/" + acc.asunto + "\r\n";
                    }

                    row.Append(
                        Datos.ConstructCell(anio, CellValues.String),
                        Datos.ConstructCell(ambito, CellValues.String),
                        Datos.ConstructCell(denominacion, CellValues.String),
                        Datos.ConstructCell(informescump, CellValues.String),
                        Datos.ConstructCell(fechareg, CellValues.String),
                        Datos.ConstructCell(resultado, CellValues.String),
                        Datos.ConstructCell(accionesmejora, CellValues.String));

                    sheetData.AppendChild(row);
                }

                // save worksheet
                document.WorkbookPart.WorksheetParts.First().Worksheet.Save();

                document.WorkbookPart.Workbook.Save();

            }   

            #endregion

            #endregion

            Session["nombreArchivo"] = destinationFile;

            ViewData["requisitos"] = Datos.ListarRequisitos(idCentral);

            return RedirectToAction("requisitos_legales", "Requisitos");

        }

        public FileResult ObtenerInformeEvaluacion(int id)
        {
            try
            {
                requisitoslegales IF = Datos.GetDatosRequisito(id);
                byte[] fileBytes = System.IO.File.ReadAllBytes(IF.informeevaluacion);
                string fileName = IF.informeevaluacion.Replace(Server.MapPath("~/Requisitos") + "\\" + IF.id + "\\", "");
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ActionResult detalle_requisito(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            Session["modulo"] = "8";

            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            int idCentral = centroseleccionado.id;

            requisitoslegales req = Datos.GetDatosRequisito(id);
            ViewData["requisito"] = req;
            List<ambitos> amb = Datos.ListarAmbitos();
            ViewData["ambitos"] = amb;
            ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 8, id);
            #region generacion fichero
            if (Session["nombreArchivo"] != null && Session["nombreArchivo"].ToString() != "")
            {
                if (Session["nombreArchivo"].ToString().Contains("docx"))
                {
                    Response.ContentType = "application/msword";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=ExportacionRequisitos.docx");
                    Response.TransmitFile(Session["nombreArchivo"].ToString());
                    Response.End();
                }
                Session["nombreArchivo"] = "";
            }
            #endregion

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult detalle_requisito(int id, FormCollection collection, HttpPostedFileBase file)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }
            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            int idCentral = centroseleccionado.id;

            requisitoslegales proce = new requisitoslegales();
            string formulario = collection["hdFormularioEjecutado"];

            if (formulario == "GuardarRequisito")
            {
                #region guardar requisito
                try
                {
                    if (id != 0)
                    {
                        #region actualizar
                        requisitoslegales actualizar = Datos.GetDatosRequisito(id);
                        actualizar.denominacion = collection["ctl00$MainContent$txtDenominacion"];
                        if (collection["ctl00$MainContent$txtFRegistro"] != "")
                            actualizar.fecharegistro = DateTime.Parse(collection["ctl00$MainContent$txtFRegistro"]);

                        if (collection["ctl00$MainContent$txtNumRequisitos"] != "")
                            actualizar.numrequisitos = int.Parse(collection["ctl00$MainContent$txtNumRequisitos"]);
                        if (collection["ctl00$MainContent$txtCumple"] != "")
                            actualizar.cumple = int.Parse(collection["ctl00$MainContent$txtCumple"]);
                        if (collection["ctl00$MainContent$txtTramite"] != "")
                            actualizar.tramite = int.Parse(collection["ctl00$MainContent$txtTramite"]);
                        if (collection["ctl00$MainContent$txtNoCumple"] != "")
                            actualizar.nocumple = int.Parse(collection["ctl00$MainContent$txtNoCumple"]);
                        if (collection["ctl00$MainContent$txtObservacion"] != "")
                            actualizar.observacion = int.Parse(collection["ctl00$MainContent$txtObservacion"]);
                        if (collection["ctl00$MainContent$txtNoProcede"] != "")
                            actualizar.noprocede = int.Parse(collection["ctl00$MainContent$txtNoProcede"]);
                        if (collection["ctl00$MainContent$txtNoVerificado"] != "")
                            actualizar.noverificado = int.Parse(collection["ctl00$MainContent$txtNoVerificado"]);
                        if (collection["ctl00$MainContent$ddlAmbito"] != "")
                            actualizar.ambito = int.Parse(collection["ctl00$MainContent$ddlAmbito"]);

                        if (actualizar.denominacion != string.Empty && actualizar.fecharegistro != null)
                        {

                            Datos.ActualizarRequisito(actualizar);

                            if ((file != null && file.ContentLength > 0))
                            {
                                var fileName = System.IO.Path.GetFileName(file.FileName);

                                var path = System.IO.Path.Combine(Server.MapPath("~/Requisitos/" + id.ToString()), fileName);
                                actualizar.informeevaluacion = path;

                                Datos.UpdateInformeEvaluacionEnlace(actualizar);

                                if (Directory.Exists(Server.MapPath("~/Requisitos/" + id.ToString())))
                                {
                                    file.SaveAs(path);
                                }
                                else
                                {
                                    Directory.CreateDirectory(Server.MapPath("~/Requisitos/" + id.ToString()));
                                    file.SaveAs(path);
                                }
                            }

                            Session["EdicionRequisitoMensaje"] = "Información actualizada correctamente";
                            proce = Datos.GetDatosRequisito(id);
                        }
                        else
                        {
                            Session["EdicionRequisitoError"] = "Los campos marcados con (*) son obligatorios.";
                            proce = actualizar;
                        }
                        ViewData["requisito"] = proce;
                        List<ambitos> amb = Datos.ListarAmbitos();
                        ViewData["ambitos"] = amb;
                        ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 8, id);
                        return View();
                        #endregion
                    }
                    else
                    {
                        #region insertar


                        requisitoslegales insertar = new requisitoslegales();
                        insertar.idcentral = idCentral;
                        insertar.denominacion = collection["ctl00$MainContent$txtDenominacion"];
                        if (collection["ctl00$MainContent$txtFRegistro"] != "")
                            insertar.fecharegistro = DateTime.Parse(collection["ctl00$MainContent$txtFRegistro"]);

                        if (collection["ctl00$MainContent$txtNumRequisitos"] != "")
                            insertar.numrequisitos = int.Parse(collection["ctl00$MainContent$txtNumRequisitos"]);
                        if (collection["ctl00$MainContent$txtCumple"] != "")
                            insertar.cumple = int.Parse(collection["ctl00$MainContent$txtCumple"]);
                        if (collection["ctl00$MainContent$txtTramite"] != "")
                            insertar.tramite = int.Parse(collection["ctl00$MainContent$txtTramite"]);
                        if (collection["ctl00$MainContent$txtNoCumple"] != "")
                            insertar.nocumple = int.Parse(collection["ctl00$MainContent$txtNoCumple"]);
                        if (collection["ctl00$MainContent$txtObservacion"] != "")
                            insertar.observacion = int.Parse(collection["ctl00$MainContent$txtObservacion"]);
                        if (collection["ctl00$MainContent$txtNoProcede"] != "")
                            insertar.noprocede = int.Parse(collection["ctl00$MainContent$txtNoProcede"]);
                        if (collection["ctl00$MainContent$txtNoVerificado"] != "")
                            insertar.noverificado = int.Parse(collection["ctl00$MainContent$txtNoVerificado"]);
                        if (collection["ctl00$MainContent$ddlAmbito"] != "")
                            insertar.ambito = int.Parse(collection["ctl00$MainContent$ddlAmbito"]);
                        if (collection["ctl00$MainContent$ddlAnio"] != "")
                            insertar.anio = int.Parse(collection["ctl00$MainContent$ddlAnio"]);

                        if (insertar.denominacion != string.Empty && insertar.fecharegistro != null)
                        {
                            int idForm = Datos.ActualizarRequisito(insertar);

                            if ((file != null && file.ContentLength > 0))
                            {
                                var fileName = System.IO.Path.GetFileName(file.FileName);

                                var path = System.IO.Path.Combine(Server.MapPath("~/Requisitos/" + idForm.ToString()), fileName);
                                insertar.informeevaluacion = path;

                                Datos.UpdateInformeEvaluacionEnlace(insertar);

                                if (Directory.Exists(Server.MapPath("~/Requisitos/" + idForm.ToString())))
                                {
                                    file.SaveAs(path);
                                }
                                else
                                {
                                    Directory.CreateDirectory(Server.MapPath("~/Requisitos/" + idForm.ToString()));
                                    file.SaveAs(path);
                                }
                            }

                            Session["EdicionRequisitoMensaje"] = "Información actualizada correctamente";

                            proce = Datos.GetDatosRequisito(id);
                            ViewData["requisito"] = proce;
                            List<ambitos> amb = Datos.ListarAmbitos();
                            ViewData["ambitos"] = amb;
                            ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 8, id);
                            return Redirect(Url.RouteUrl(new { controller = "Requisitos", action = "detalle_requisito", id = idForm }));
                        }
                        else
                        {
                            Session["EdicionRequisitoError"] = "Los campos marcados con (*) son obligatorios.";
                            proce = insertar;
                            ViewData["requisito"] = proce;
                            List<ambitos> amb = Datos.ListarAmbitos();
                            ViewData["ambitos"] = amb;
                            ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 8, id);
                            return View();
                        }
                        
                        #endregion
                    }

                }
                catch (Exception ex)
                {
                    proce = Datos.GetDatosRequisito(id);
                    ViewData["requisito"] = proce;
                    List<ambitos> amb = Datos.ListarAmbitos();
                    ViewData["ambitos"] = amb;
                    ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 8, id);
                    return View();
                }
                #endregion
            }

            if (formulario == "btnImprimir")
            {
                #region imprimir
                string sourceFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaPlantillaWord"] + "FichaRequisitoGrafico.docx");
                string destinationFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaGeneracionWord"] + "FichaRequisito_" + DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + ".docx");

                Session["source"] = sourceFile;
                Session["destino"] = destinationFile;
                // Create a copy of the template file and open the copy 
                System.IO.File.Copy(sourceFile, destinationFile, true);

                #region impresion


                //LISTADO REQUISITOS
                requisitoslegales form = Datos.GetDatosRequisito(id);
                if (form != null)
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add("T_Codigo", form.codigo.Replace("\r\n", "<w:br/>"));
                    keyValues.Add("T_Nombre", form.denominacion.Replace("\r\n", "<w:br/>"));
                    ambitos ambito = Datos.ObtenerAmbito(int.Parse(form.ambito.ToString()));
                    keyValues.Add("T_Ambito", ambito.nombre_ambito.Replace("\r\n", "<w:br/>"));
                    if (form.informeevaluacion != null)
                        keyValues.Add("T_Informe", form.informeevaluacion.Replace(Server.MapPath("~/Requisitos") + "\\" + form.id + "\\", "").Replace("\r\n", "<w:br/>"));
                    else
                        keyValues.Add("T_Informe", "");

                    if (form.fecharegistro != null)
                        keyValues.Add("T_FRegistro", form.fecharegistro.ToString().Replace(" 0:00:00", "").Replace("\r\n", "<w:br/>"));
                    else
                        keyValues.Add("T_FRegistro", "");

                    if (form.numrequisitos != null)
                        keyValues.Add("T_NumReq", form.numrequisitos.ToString().Replace("\r\n", "<w:br/>"));
                    else
                        keyValues.Add("T_NumReq", "0");

                    if (form.cumple != null)
                        keyValues.Add("T_Cumple", form.cumple.ToString().Replace("\r\n", "<w:br/>"));
                    else
                        keyValues.Add("T_Cumple", "0");

                    if (form.tramite != null)
                        keyValues.Add("T_Tramite", form.tramite.ToString().Replace("\r\n", "<w:br/>"));
                    else
                        keyValues.Add("T_Tramite", "0");

                    if (form.nocumple != null)
                        keyValues.Add("T_NoCumple", form.nocumple.ToString().Replace("\r\n", "<w:br/>"));
                    else
                        keyValues.Add("T_NoCumple", "0");

                    if (form.observacion != null)
                        keyValues.Add("T_Observacion", form.observacion.ToString().Replace("\r\n", "<w:br/>"));
                    else
                        keyValues.Add("T_Observacion", "0");

                    if (form.noprocede != null)
                        keyValues.Add("T_NoProcede", form.noprocede.ToString().Replace("\r\n", "<w:br/>"));
                    else
                        keyValues.Add("T_NoProcede", "0");

                    if (form.noverificado != null)
                        keyValues.Add("T_NoVerificado", form.noverificado.ToString().Replace("\r\n", "<w:br/>"));
                    else
                        keyValues.Add("T_NoVerificado", "0");

                    SearchAndReplace(destinationFile, keyValues);

                }

                WordprocessingDocument doc = WordprocessingDocument.Open(destinationFile, true);
                DocumentFormat.OpenXml.Wordprocessing.Table tabladespliegue = doc.MainDocumentPart.Document.Body.Descendants<DocumentFormat.OpenXml.Wordprocessing.Table>().ElementAt(1);

                Stream stream = doc.MainDocumentPart.ChartParts.First().EmbeddedPackagePart.GetStream();
                using (SpreadsheetDocument ssDoc = SpreadsheetDocument.Open(stream, true))
                {
                    WorkbookPart wbPart = ssDoc.WorkbookPart;
                    Sheet theSheet = wbPart.Workbook.Descendants<Sheet>().
                      Where(s => s.Name == "Hoja1").FirstOrDefault();
                    if (theSheet != null)
                    {
                        Worksheet ws = ((WorksheetPart)(wbPart.GetPartById(theSheet.Id))).Worksheet;

                        if (form.cumple != null)
                        {
                            Cell theCell = InsertCellInWorksheet("B", 2, ws);
                            theCell.CellValue = new CellValue(form.cumple.ToString());
                            theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                        }

                        if (form.tramite != null)
                        {
                            Cell theCell = InsertCellInWorksheet("B", 3, ws);
                            theCell.CellValue = new CellValue(form.tramite.ToString());
                            theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                        }

                        if (form.nocumple != null)
                        {
                            Cell theCell = InsertCellInWorksheet("B", 4, ws);
                            theCell.CellValue = new CellValue(form.nocumple.ToString());
                            theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                        }

                        if (form.observacion != null)
                        {
                            Cell theCell = InsertCellInWorksheet("B", 5, ws);
                            theCell.CellValue = new CellValue(form.observacion.ToString());
                            theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                        }

                        if (form.noprocede != null)
                        {
                            Cell theCell = InsertCellInWorksheet("B", 6, ws);
                            theCell.CellValue = new CellValue(form.noprocede.ToString());
                            theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                        }

                        if (form.noverificado != null)
                        {
                            Cell theCell = InsertCellInWorksheet("B", 7, ws);
                            theCell.CellValue = new CellValue(form.noverificado.ToString());
                            theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                        }

                        ws.Save();

                        var cachedValues = doc.MainDocumentPart.ChartParts.ElementAt(0).ChartSpace.Descendants<DocumentFormat.OpenXml.Drawing.Charts.Values>();
                        int row = 0;
                        int col = 0;
                        foreach (var cachedValue in cachedValues)
                        { // By column B ; C ; D
                            row = 0;

                            if (form.cumple != null)
                                cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(0).Text = form.cumple.ToString();
                            if (form.tramite != null)
                                cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(1).Text = form.tramite.ToString();
                            if (form.nocumple != null)
                                cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(2).Text = form.nocumple.ToString();
                            if (form.observacion != null)
                                cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(3).Text = form.observacion.ToString();
                            if (form.noprocede != null)
                                cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(4).Text = form.noprocede.ToString();
                            if (form.noverificado != null)
                                cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(5).Text = form.noverificado.ToString();

                            col++;
                        }
                    }
                }
                doc.MainDocumentPart.ChartParts.ElementAt(0).ChartSpace.Save();

                doc.MainDocumentPart.Document.Save();

                doc.Close();

                #endregion

                Session["nombreArchivo"] = destinationFile;

                proce = Datos.GetDatosRequisito(id);
                ViewData["requisito"] = proce;
                List<ambitos> amb = Datos.ListarAmbitos();
                ViewData["ambitos"] = amb;
                ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 8, id);
                return Redirect(Url.RouteUrl(new { controller = "Requisitos", action = "detalle_requisito", id = id }));
                #endregion
            }
            else
            {
                #region recarga
                proce = Datos.GetDatosRequisito(id);
                ViewData["requisito"] = proce;
                List<ambitos> amb = Datos.ListarAmbitos();
                ViewData["ambitos"] = amb;
                ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 8, id);
                return View();
                #endregion
            }
        }
        
        public ActionResult Eliminar_Requisito(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            int idProceso = Datos.EliminarRequisito(id);
            Session["EditarRequisitoResultado"] = "ELIMINADOREQUISITO";
            return RedirectToAction("requisitos_legales", "Requisitos");
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
