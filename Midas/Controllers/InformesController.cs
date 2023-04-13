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
    public class InformesController : Controller
    {
        public ActionResult menu()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            #region generacion fichero
            if (Session["nombreArchivo"] != null && Session["nombreArchivo"].ToString() != "")
            {
                if (Session["nombreArchivo"].ToString().Contains("docx"))
                {
                    Response.ContentType = "application/msword";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=Informe.docx");
                    Response.TransmitFile(Session["nombreArchivo"].ToString());
                    Response.End();
                }
                Session["nombreArchivo"] = "";
            }
            #endregion

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult menu(FormCollection collection)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            if (collection["ctl00$MainContent$ddlAnio"] != null)
            {
                string anio = collection["ctl00$MainContent$ddlAnio"].ToString();

                #region Revisión por la dirección
                if (collection["ctl00$MainContent$hdnInformeSeleccionado"] == "RevisionDireccion")
                {
                    InformeDireccion(anio);

                    #region generacion fichero
                    if (Session["nombreArchivo"] != null && Session["nombreArchivo"].ToString() != "")
                    {
                        if (Session["nombreArchivo"].ToString().Contains("docx"))
                        {
                            Response.ContentType = "application/msword";
                            Response.AppendHeader("Content-Disposition", "attachment; filename=Informe.docx");
                            Response.TransmitFile(Session["nombreArchivo"].ToString());
                            Response.End();
                        }
                        Session["nombreArchivo"] = "";
                    }
                    #endregion
                }
                #endregion

                #region Planificación Preventiva
                if (collection["ctl00$MainContent$hdnInformeSeleccionado"] == "PlanificacionPreventiva")
                {
                    PlanificacionPreventiva(anio);

                    #region generacion fichero
                    if (Session["nombreArchivo"] != null && Session["nombreArchivo"].ToString() != "")
                    {
                        if (Session["nombreArchivo"].ToString().Contains("xlsx"))
                        {
                            Response.ContentType = "application/msexcel";
                            Response.AppendHeader("Content-Disposition", "attachment; filename=Informe.xlsx");
                            Response.TransmitFile(Session["nombreArchivo"].ToString());
                            Response.End();
                        }
                        Session["nombreArchivo"] = "";
                    }
                    #endregion
                }
                #endregion


            }
            else
                Session["ImpresionError"] = "Debe seleccionarse un año válido";



            return View();
        }

        public void InformeDireccion(string anio)
        {
            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            int idCentral = centroseleccionado.id;

            #region imprimir

            #region creacion del fichero
            string sourceFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaPlantillaWord"] + "PlantillaRevisionDireccion.docx");
            string destinationFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaGeneracionWord"] + "InformeRevisionDireccion_" + DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + ".docx");

            Session["source"] = sourceFile;
            Session["destino"] = destinationFile;
            // Create a copy of the template file and open the copy 
            System.IO.File.Copy(sourceFile, destinationFile, true);
            #endregion

            #region impresion

            #region replace
            Dictionary<string, string> keyValues = new Dictionary<string, string>();
            keyValues.Add("T_Siglas", centroseleccionado.siglas.Replace("\r\n", "<w:br/>"));
            keyValues.Add("T_Periodo", anio);
            keyValues.Add("T_Anio", DateTime.Now.Year.ToString());
            keyValues.Add("T_Central", centroseleccionado.nombre.Replace("\r\n", "<w:br/>"));
            keyValues.Add("T_FEmision", DateTime.Now.Date.ToShortDateString().Replace(" 0:00:00", "").Replace("\r\n", "<w:br/>"));

            SearchAndReplace(destinationFile, keyValues);
            #endregion

            #region abrir doc
            WordprocessingDocument doc = WordprocessingDocument.Open(destinationFile, true);
            #endregion

            #region cabecera
            foreach (HeaderPart header in doc.MainDocumentPart.HeaderParts)
            {
                string headerText = null;
                using (StreamReader sr = new StreamReader(header.GetStream()))
                {
                    headerText = sr.ReadToEnd();
                }

                headerText = headerText.Replace("T_Siglas", centroseleccionado.siglas);

                using (StreamWriter sw = new StreamWriter(header.GetStream(FileMode.Create)))
                {
                    sw.Write(headerText);
                }

                //Save Header
                header.Header.Save();

            }
            #endregion

            #region tablas

            #region reuniones
            //DocumentFormat.OpenXml.Wordprocessing.Table myTable = doc.MainDocumentPart.Document.Body.Descendants<DocumentFormat.OpenXml.Wordprocessing.Table>().ElementAt(1);
            //DocumentFormat.OpenXml.Wordprocessing.TableRow theRow = myTable.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>().ElementAt(3);

            //List<reuniones> listaReuniones = Datos.ListarReuniones(centroseleccionado.id, fechaInicio, fechaFin);

            //for (int i = 0; i < listaReuniones.Count; i++)
            //{
            //    DocumentFormat.OpenXml.Wordprocessing.TableRow rowCopy = (DocumentFormat.OpenXml.Wordprocessing.TableRow)theRow.CloneNode(true);

            //    var runProperties = GetRunPropertyFromTableCell(rowCopy, 0);
            //    var run = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(listaReuniones[i].fecha_convocatoria.ToString().Replace(" 0:00:00", "")));
            //    run.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runProperties);


            //    rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(0).RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();//removes that text of the copied cell
            //    rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(0).Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(run));
            //    //I only get the the run properties from the first cell in this example, the rest of the cells get the document default style. 

            //    var runPropertiesCenter = new DocumentFormat.OpenXml.Wordprocessing.RunProperties();
            //    runPropertiesCenter.AppendChild(new RunFonts() { Ascii = "Calibri" });
            //    runPropertiesCenter.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "18" });
            //    runPropertiesCenter.AppendChild(new Justification() { Val = JustificationValues.Center });

            //    rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(1).RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();
            //    var runCenter = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(listaReuniones[i].resumen.ToString()));
            //    runCenter.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesCenter);
            //    rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(1).Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(runCenter));

            //    myTable.AppendChild(rowCopy);
            //}

            //myTable.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>().ElementAt(3).Remove();
            #endregion

            #region objetivos
            DocumentFormat.OpenXml.Wordprocessing.Table tablaObjetivos = doc.MainDocumentPart.Document.Body.Descendants<DocumentFormat.OpenXml.Wordprocessing.Table>().ElementAt(3);
            DocumentFormat.OpenXml.Wordprocessing.TableRow filaObjetivos = tablaObjetivos.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>().ElementAt(3);

            List<objetivos> listaObjetivos = Datos.ListarObjetivos(idCentral, anio);

            string seguimientoObjetivos = "";


            for (int i = 0; i < listaObjetivos.Count; i++)
            {
                seguimientoObjetivos = seguimientoObjetivos + listaObjetivos[i].Codigo + " - " + listaObjetivos[i].Seguimiento + "<w:br/>";

                DocumentFormat.OpenXml.Wordprocessing.TableRow rowCopy = (DocumentFormat.OpenXml.Wordprocessing.TableRow)filaObjetivos.CloneNode(true);

                var runProperties = GetRunPropertyFromTableCell(rowCopy, 0);
                var run = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(listaObjetivos[i].Codigo.ToString()));
                run.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runProperties);


                rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(0).RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();//removes that text of the copied cell
                rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(0).Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(run));
                //I only get the the run properties from the first cell in this example, the rest of the cells get the document default style. 

                var runPropertiesTitulo = new DocumentFormat.OpenXml.Wordprocessing.RunProperties();
                runPropertiesTitulo.AppendChild(new RunFonts() { Ascii = "Calibri" });
                runPropertiesTitulo.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "18" });
                runPropertiesTitulo.AppendChild(new Justification() { Val = JustificationValues.Center });

                rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(1).RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();
                var runTitulo = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(listaObjetivos[i].Nombre.ToString()));
                runTitulo.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesTitulo);
                rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(1).Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(runTitulo));

                var runPropertiesEstado = new DocumentFormat.OpenXml.Wordprocessing.RunProperties();
                runPropertiesEstado.AppendChild(new RunFonts() { Ascii = "Calibri" });
                runPropertiesEstado.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "18" });
                runPropertiesEstado.AppendChild(new Justification() { Val = JustificationValues.Center });

                rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(2).RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();
                string estadoobjetivo = "";
                switch (listaObjetivos[i].estado)
                {
                    case 0:
                        estadoobjetivo = "En seguimiento";
                        break;
                    case 1:
                        estadoobjetivo = "Alcanza resultados esperados";
                        break;
                    case 2:
                        estadoobjetivo = "No alcanza resultados esperados";
                        break;
                    case 3:
                        estadoobjetivo = "Desestimado";
                        break;
                    default:
                        estadoobjetivo = "En seguimiento";
                        break;
                }
                var runEstado = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(estadoobjetivo));
                runEstado.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesEstado);
                rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(2).Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(runEstado));

                var runPropertiesConsecucion = new DocumentFormat.OpenXml.Wordprocessing.RunProperties();
                runPropertiesConsecucion.AppendChild(new RunFonts() { Ascii = "Calibri" });
                runPropertiesConsecucion.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "18" });
                runPropertiesConsecucion.AppendChild(new Justification() { Val = JustificationValues.Center });

                rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(3).RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();
                decimal porcConsecucion = 100;

                List<VISTA_Despliegue> accionesdespliegue = Datos.ListarAccionesObjetivo(listaObjetivos[i].id);

                decimal totalAcciones = accionesdespliegue.Count();
                decimal ejecutadoAcciones = accionesdespliegue.Where(x => x.Estado == 2).Count();

                if (totalAcciones > 0)
                {
                    porcConsecucion = Math.Round((100 / totalAcciones) * ejecutadoAcciones, 0);
                }
                else
                {
                    porcConsecucion = 0;
                }

                var runConsecucion = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(porcConsecucion.ToString() + "%"));
                runConsecucion.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesConsecucion);
                rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(3).Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(runConsecucion));

                tablaObjetivos.AppendChild(rowCopy);
            }

            tablaObjetivos.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>().ElementAt(3).Remove();

            #endregion

            #region auditorias

            DocumentFormat.OpenXml.Wordprocessing.Table tablaAuditorias = doc.MainDocumentPart.Document.Body.Descendants<DocumentFormat.OpenXml.Wordprocessing.Table>().ElementAt(5);
            DocumentFormat.OpenXml.Wordprocessing.TableRow filaAuditorias = tablaObjetivos.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>().ElementAt(3);

            List<VISTA_Auditorias> listaAuditorias = Datos.ListarAuditorias(centroseleccionado.id, anio);

            for (int i = 0; i < listaAuditorias.Count; i++)
            {
                DocumentFormat.OpenXml.Wordprocessing.TableRow rowCopy = (DocumentFormat.OpenXml.Wordprocessing.TableRow)filaAuditorias.CloneNode(true);

                var runPropertiesFechaInicioAud = new DocumentFormat.OpenXml.Wordprocessing.RunProperties();
                runPropertiesFechaInicioAud.AppendChild(new RunFonts() { Ascii = "Calibri" });
                runPropertiesFechaInicioAud.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "18" });
                runPropertiesFechaInicioAud.AppendChild(new Justification() { Val = JustificationValues.Center });

                rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(0).RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();
                var runFechaInicioAud = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(listaAuditorias[i].fechafin.ToString().Replace(" 0:00:00", "")));
                runFechaInicioAud.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesFechaInicioAud);
                rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(0).Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(runFechaInicioAud));

                var runPropertiesFechaFinAud = new DocumentFormat.OpenXml.Wordprocessing.RunProperties();
                runPropertiesFechaFinAud.AppendChild(new RunFonts() { Ascii = "Calibri" });
                runPropertiesFechaFinAud.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "18" });
                runPropertiesFechaFinAud.AppendChild(new Justification() { Val = JustificationValues.Center });

                rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(1).RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();
                var runFechaFinAud = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(listaAuditorias[i].fechafin.ToString().Replace(" 0:00:00", "")));
                runFechaFinAud.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesFechaFinAud);
                rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(1).Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(runFechaFinAud));

                var runPropertiesTipo = new DocumentFormat.OpenXml.Wordprocessing.RunProperties();
                runPropertiesTipo.AppendChild(new RunFonts() { Ascii = "Calibri" });
                runPropertiesTipo.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "18" });
                runPropertiesTipo.AppendChild(new Justification() { Val = JustificationValues.Center });

                rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(2).RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();
                string tipoauditoria = "";
                switch (listaObjetivos[i].estado)
                {
                    case 0:
                        tipoauditoria = "Interna";
                        break;
                    case 1:
                        tipoauditoria = "Externa";
                        break;
                }
                var runTipo = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(tipoauditoria));
                runTipo.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesTipo);
                rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(2).Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(runTipo));

                var runPropertiesAlcance = new DocumentFormat.OpenXml.Wordprocessing.RunProperties();
                runPropertiesAlcance.AppendChild(new RunFonts() { Ascii = "Calibri" });
                runPropertiesAlcance.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "18" });
                runPropertiesAlcance.AppendChild(new Justification() { Val = JustificationValues.Center });

                rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(3).RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();

                List<VISTA_AuditoriaReferenciales> referenciales = Datos.ListarReferencialesAsignados(listaAuditorias[i].id);

                string alcance = "";

                foreach (VISTA_AuditoriaReferenciales refer in referenciales)
                {
                    alcance = alcance + " -" + refer.nombre + Environment.NewLine;
                }

                var runAlcance = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(alcance));
                runAlcance.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesAlcance);
                rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(3).Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(runAlcance));


                tablaAuditorias.AppendChild(rowCopy);
            }

            tablaAuditorias.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>().ElementAt(3).Remove();

            #endregion

            #region accionesmejora

            DocumentFormat.OpenXml.Wordprocessing.Table tablaAccmejora = doc.MainDocumentPart.Document.Body.Descendants<DocumentFormat.OpenXml.Wordprocessing.Table>().ElementAt(8);
            DocumentFormat.OpenXml.Wordprocessing.TableRow filaAccmejora = tablaAccmejora.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>().ElementAt(3);

            List<VISTA_ListarAccionesMejora> listaAccMejora = Datos.ListarAccionesMejora(centroseleccionado.id, anio);

            for (int i = 0; i < listaAccMejora.Count; i++)
            {
                DocumentFormat.OpenXml.Wordprocessing.TableRow rowCopy = (DocumentFormat.OpenXml.Wordprocessing.TableRow)filaAccmejora.CloneNode(true);

                var runPropertiesTipo = new DocumentFormat.OpenXml.Wordprocessing.RunProperties();
                runPropertiesTipo.AppendChild(new RunFonts() { Ascii = "Calibri" });
                runPropertiesTipo.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "18" });
                runPropertiesTipo.AppendChild(new Justification() { Val = JustificationValues.Center });

                rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(0).RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();
                var runTipo = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(listaAccMejora[i].tipo));
                runTipo.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesTipo);
                rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(0).Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(runTipo));

                var runPropertiesAsunto = new DocumentFormat.OpenXml.Wordprocessing.RunProperties();
                runPropertiesAsunto.AppendChild(new RunFonts() { Ascii = "Calibri" });
                runPropertiesAsunto.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "18" });
                runPropertiesAsunto.AppendChild(new Justification() { Val = JustificationValues.Center });

                rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(1).RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();
                var runAsunto = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(listaAccMejora[i].asunto));
                runAsunto.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesAsunto);
                rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(1).Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(runAsunto));

                var runPropertiesFechaInicioAcc = new DocumentFormat.OpenXml.Wordprocessing.RunProperties();
                runPropertiesFechaInicioAcc.AppendChild(new RunFonts() { Ascii = "Calibri" });
                runPropertiesFechaInicioAcc.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "18" });
                runPropertiesFechaInicioAcc.AppendChild(new Justification() { Val = JustificationValues.Center });

                rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(2).RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();
                var runFechaInicioAud = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(listaAccMejora[i].fecha_apertura.ToString().Replace(" 0:00:00", "")));
                runFechaInicioAud.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesFechaInicioAcc);
                rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(2).Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(runFechaInicioAud));

                var runPropertiesFechaFinAcc = new DocumentFormat.OpenXml.Wordprocessing.RunProperties();
                runPropertiesFechaFinAcc.AppendChild(new RunFonts() { Ascii = "Calibri" });
                runPropertiesFechaFinAcc.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "18" });
                runPropertiesFechaFinAcc.AppendChild(new Justification() { Val = JustificationValues.Center });

                rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(3).RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();
                var runFechaFinAud = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(listaAccMejora[i].fecha_cierre.ToString().Replace(" 0:00:00", "")));
                runFechaFinAud.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesFechaFinAcc);
                rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(3).Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(runFechaFinAud));


                var runPropertiesEstado = new DocumentFormat.OpenXml.Wordprocessing.RunProperties();
                runPropertiesEstado.AppendChild(new RunFonts() { Ascii = "Calibri" });
                runPropertiesEstado.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "18" });
                runPropertiesEstado.AppendChild(new Justification() { Val = JustificationValues.Center });

                rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(4).RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();
                var runEstado = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(listaAccMejora[i].EstadoEscrito));
                runEstado.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesEstado);
                rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(4).Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(runEstado));

                tablaAccmejora.AppendChild(rowCopy);
            }

            tablaAccmejora.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>().ElementAt(3).Remove();

            #endregion

            #region resumenaccionesmejora
            #region ambitos
            int totalAccMejora = listaAccMejora.Where(x => x.codigo.Contains(anio) || x.estado == 0).Count();

            List<ambitos> listaAmbitos = Datos.ListarAmbitos();

            DocumentFormat.OpenXml.Wordprocessing.Table tablaPorcentajesAcc = doc.MainDocumentPart.Document.Body.Descendants<DocumentFormat.OpenXml.Wordprocessing.Table>().ElementAt(11);
            DocumentFormat.OpenXml.Wordprocessing.TableRow filaPorcentajesAcc = tablaPorcentajesAcc.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>().ElementAt(1);

            foreach (ambitos amb in listaAmbitos)
            {
                if (totalAccMejora > 0)
                {
                    int totalambito = listaAccMejora.Where(x => x.ambito == amb.id).Count();
                    if (totalAccMejora != 0)
                    {
                        int porcentaje = (100 / totalAccMejora) * totalambito;




                        DocumentFormat.OpenXml.Wordprocessing.TableRow rowCopy = (DocumentFormat.OpenXml.Wordprocessing.TableRow)filaPorcentajesAcc.CloneNode(true);

                        var runPropertiesAmbito = new DocumentFormat.OpenXml.Wordprocessing.RunProperties();
                        runPropertiesAmbito.AppendChild(new RunFonts() { Ascii = "Calibri" });
                        runPropertiesAmbito.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "18" });
                        runPropertiesAmbito.AppendChild(new Justification() { Val = JustificationValues.Center });

                        rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(0).RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();
                        var runAmbito = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(amb.nombre_ambito + ": " + porcentaje + "%"));
                        runAmbito.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesAmbito);
                        rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(0).Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(runAmbito));


                        tablaPorcentajesAcc.AppendChild(rowCopy);

                    }
                }

            }
            tablaPorcentajesAcc.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>().ElementAt(1).Remove();
            #endregion

            #region tipos
            List<tipo_accionesmejora> listaTipos = Datos.ListarTiposAccionMejora();

            DocumentFormat.OpenXml.Wordprocessing.Table tablaPorcentajesTipoAcc = doc.MainDocumentPart.Document.Body.Descendants<DocumentFormat.OpenXml.Wordprocessing.Table>().ElementAt(12);
            DocumentFormat.OpenXml.Wordprocessing.TableRow filaPorcentajesTipoAcc = tablaPorcentajesTipoAcc.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>().ElementAt(1);

            foreach (tipo_accionesmejora amb in listaTipos)
            {

                if (listaAccMejora.Count > 0)
                {
                    int totaltipo = listaAccMejora.Where(x => x.tipoid == amb.id).Count();
                    int porcentaje = (100 / totalAccMejora) * totaltipo;




                    DocumentFormat.OpenXml.Wordprocessing.TableRow rowCopy = (DocumentFormat.OpenXml.Wordprocessing.TableRow)filaPorcentajesTipoAcc.CloneNode(true);

                    var runPropertiesTipo = new DocumentFormat.OpenXml.Wordprocessing.RunProperties();
                    runPropertiesTipo.AppendChild(new RunFonts() { Ascii = "Calibri" });
                    runPropertiesTipo.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "18" });
                    runPropertiesTipo.AppendChild(new Justification() { Val = JustificationValues.Center });

                    rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(0).RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();
                    var runTipo = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(amb.nombre + ": " + porcentaje + "%"));
                    runTipo.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesTipo);
                    rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(0).Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(runTipo));


                    tablaPorcentajesTipoAcc.AppendChild(rowCopy);
                }


            }
            tablaPorcentajesTipoAcc.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>().ElementAt(1).Remove();
            #endregion

            #region antecedentes

            List<modulos> listaModulos = Datos.ListarModulos();

            DocumentFormat.OpenXml.Wordprocessing.Table tablaPorcentajesAntAcc = doc.MainDocumentPart.Document.Body.Descendants<DocumentFormat.OpenXml.Wordprocessing.Table>().ElementAt(13);
            DocumentFormat.OpenXml.Wordprocessing.TableRow filaPorcentajesAntAcc = tablaPorcentajesAntAcc.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>().ElementAt(1);

            foreach (modulos amb in listaModulos)
            {

                if (listaAccMejora.Count > 0)
                {
                    int totaltipo = listaAccMejora.Where(x => x.antecedente != 0).Where(x => x.antecedente == amb.id).Count();
                    int porcentaje = (100 / totalAccMejora) * totaltipo;




                    DocumentFormat.OpenXml.Wordprocessing.TableRow rowCopy = (DocumentFormat.OpenXml.Wordprocessing.TableRow)filaPorcentajesAntAcc.CloneNode(true);

                    var runPropertiesTipo = new DocumentFormat.OpenXml.Wordprocessing.RunProperties();
                    runPropertiesTipo.AppendChild(new RunFonts() { Ascii = "Calibri" });
                    runPropertiesTipo.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "18" });
                    runPropertiesTipo.AppendChild(new Justification() { Val = JustificationValues.Center });

                    rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(0).RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();
                    var runTipo = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(amb.nombre + ": " + porcentaje + "%"));
                    runTipo.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesTipo);
                    rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(0).Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(runTipo));


                    tablaPorcentajesAntAcc.AppendChild(rowCopy);
                }


            }
            tablaPorcentajesAntAcc.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>().ElementAt(1).Remove();

            #endregion

            #region contratistas

            DocumentFormat.OpenXml.Wordprocessing.Table tablaPorcentajesCont = doc.MainDocumentPart.Document.Body.Descendants<DocumentFormat.OpenXml.Wordprocessing.Table>().ElementAt(14);
            DocumentFormat.OpenXml.Wordprocessing.TableRow filaPorcentajesSiAcc = tablaPorcentajesCont.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>().ElementAt(1);
            DocumentFormat.OpenXml.Wordprocessing.TableRow filaPorcentajesNoAcc = tablaPorcentajesCont.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>().ElementAt(2);

            if (totalAccMejora > 0)
            {



                int totalSi = listaAccMejora.Where(x => x.contratista == 1).Count();
                int totalNo = listaAccMejora.Where(x => x.contratista == 0).Count();
                int porcentajeSi = (100 / totalAccMejora) * totalSi;
                int porcentajeNo = (100 / totalAccMejora) * totalNo;



                DocumentFormat.OpenXml.Wordprocessing.TableRow rowCopySi = (DocumentFormat.OpenXml.Wordprocessing.TableRow)filaPorcentajesSiAcc.CloneNode(true);

                var runPropertiesSi = new DocumentFormat.OpenXml.Wordprocessing.RunProperties();
                runPropertiesSi.AppendChild(new RunFonts() { Ascii = "Calibri" });
                runPropertiesSi.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "18" });
                runPropertiesSi.AppendChild(new Justification() { Val = JustificationValues.Center });

                rowCopySi.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(0).RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();
                var runSi = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text("Sí" + ": " + porcentajeSi + "%"));
                runSi.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesSi);
                rowCopySi.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(0).Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(runSi));


                tablaPorcentajesCont.AppendChild(rowCopySi);


                DocumentFormat.OpenXml.Wordprocessing.TableRow rowCopyNo = (DocumentFormat.OpenXml.Wordprocessing.TableRow)filaPorcentajesSiAcc.CloneNode(true);

                var runPropertiesNo = new DocumentFormat.OpenXml.Wordprocessing.RunProperties();
                runPropertiesNo.AppendChild(new RunFonts() { Ascii = "Calibri" });
                runPropertiesNo.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "18" });
                runPropertiesNo.AppendChild(new Justification() { Val = JustificationValues.Center });

                rowCopyNo.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(0).RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();
                var runNo = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text("No" + ": " + porcentajeNo + "%"));
                runNo.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesNo);
                rowCopyNo.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(0).Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(runNo));


                tablaPorcentajesCont.AppendChild(rowCopyNo);



            }

            tablaPorcentajesCont.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>().ElementAt(1).Remove();
            tablaPorcentajesCont.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>().ElementAt(1).Remove();

            #endregion

            #region acciones planificadas

            DocumentFormat.OpenXml.Wordprocessing.Table tablaAccionesPlan = doc.MainDocumentPart.Document.Body.Descendants<DocumentFormat.OpenXml.Wordprocessing.Table>().ElementAt(16);
            DocumentFormat.OpenXml.Wordprocessing.TableRow filaAccionesPlan = tablaAccionesPlan.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>().ElementAt(1);

            List<VISTA_AccionMejora_Accion> listaAcciones = Datos.ListarAccionesAccionesMejora(anio);

            int totalaccionesaccmejora = listaAcciones.Count();

            //Valor absoluto planificadas
            DocumentFormat.OpenXml.Wordprocessing.TableRow rowTotalAcciones = (DocumentFormat.OpenXml.Wordprocessing.TableRow)filaAccionesPlan.CloneNode(true);

            var runPropertiesTotalAcciones = new DocumentFormat.OpenXml.Wordprocessing.RunProperties();
            runPropertiesTotalAcciones.AppendChild(new RunFonts() { Ascii = "Calibri" });
            runPropertiesTotalAcciones.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "18" });
            runPropertiesTotalAcciones.AppendChild(new Justification() { Val = JustificationValues.Center });

            rowTotalAcciones.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(0).RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();
            var runTotalAcciones = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text("Valor absoluto acciones planificadas en el período (incluye inmediatas)" + ": " + totalaccionesaccmejora));
            runTotalAcciones.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesTotalAcciones);
            rowTotalAcciones.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(0).Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(runTotalAcciones));

            tablaAccionesPlan.AppendChild(rowTotalAcciones);

            //TotalEjecutadas
            DocumentFormat.OpenXml.Wordprocessing.TableRow rowTotalEjecutadas = (DocumentFormat.OpenXml.Wordprocessing.TableRow)filaAccionesPlan.CloneNode(true);
            int totalEjecutadas = listaAcciones.Where(x => x.estado == 0).Count();

            var runPropertiesTotalEjecutadas = new DocumentFormat.OpenXml.Wordprocessing.RunProperties();
            runPropertiesTotalEjecutadas.AppendChild(new RunFonts() { Ascii = "Calibri" });
            runPropertiesTotalEjecutadas.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "18" });
            runPropertiesTotalEjecutadas.AppendChild(new Justification() { Val = JustificationValues.Center });

            rowTotalEjecutadas.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(0).RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();
            var runTotalEjecutadas = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text("Total acciones ejecutadas" + ": " + totalEjecutadas));
            runTotalEjecutadas.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesTotalEjecutadas);
            rowTotalEjecutadas.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(0).Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(runTotalEjecutadas));

            tablaAccionesPlan.AppendChild(rowTotalEjecutadas);

            //% Ejecutadas
            DocumentFormat.OpenXml.Wordprocessing.TableRow rowPorcEjecutadas = (DocumentFormat.OpenXml.Wordprocessing.TableRow)filaAccionesPlan.CloneNode(true);
            int PorcEjecutadas = 0;
            if (totalaccionesaccmejora > 0)
                PorcEjecutadas = (100 / totalaccionesaccmejora) * totalEjecutadas;

            var runPropertiesPorcEjecutadas = new DocumentFormat.OpenXml.Wordprocessing.RunProperties();
            runPropertiesPorcEjecutadas.AppendChild(new RunFonts() { Ascii = "Calibri" });
            runPropertiesPorcEjecutadas.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "18" });
            runPropertiesPorcEjecutadas.AppendChild(new Justification() { Val = JustificationValues.Center });

            rowPorcEjecutadas.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(0).RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();
            var runPorcEjecutadas = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text("% acciones ejecutadas" + ": " + PorcEjecutadas + "%"));
            runPorcEjecutadas.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesPorcEjecutadas);
            rowPorcEjecutadas.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(0).Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(runPorcEjecutadas));

            tablaAccionesPlan.AppendChild(rowPorcEjecutadas);

            //TotalPasadasPlazo
            DocumentFormat.OpenXml.Wordprocessing.TableRow rowPlazo = (DocumentFormat.OpenXml.Wordprocessing.TableRow)filaAccionesPlan.CloneNode(true);
            int PasadasPlazo = listaAcciones.Where(x => x.fecha_cierre == null && x.fecha_fin < DateTime.Now.Date).Count();

            var runPropertiesPasadasPlazo = new DocumentFormat.OpenXml.Wordprocessing.RunProperties();
            runPropertiesPasadasPlazo.AppendChild(new RunFonts() { Ascii = "Calibri" });
            runPropertiesPasadasPlazo.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "18" });
            runPropertiesPasadasPlazo.AppendChild(new Justification() { Val = JustificationValues.Center });

            rowPlazo.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(0).RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();
            var runPasadasPlazo = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text("Total acciones pasadas de plazo" + ": " + PasadasPlazo));
            runPasadasPlazo.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesPasadasPlazo);
            rowPlazo.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(0).Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(runPasadasPlazo));

            tablaAccionesPlan.AppendChild(rowPlazo);


            //% PasadasPlazo
            DocumentFormat.OpenXml.Wordprocessing.TableRow rowPorcPlazo = (DocumentFormat.OpenXml.Wordprocessing.TableRow)filaAccionesPlan.CloneNode(true);
            int PorcPasadasPlazo = 0;
            if (totalaccionesaccmejora > 0)
                PorcPasadasPlazo = (100 / totalaccionesaccmejora) * PasadasPlazo;

            var runPropertiesPorcPlazo = new DocumentFormat.OpenXml.Wordprocessing.RunProperties();
            runPropertiesPorcPlazo.AppendChild(new RunFonts() { Ascii = "Calibri" });
            runPropertiesPorcPlazo.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "18" });
            runPropertiesPorcPlazo.AppendChild(new Justification() { Val = JustificationValues.Center });

            rowPorcPlazo.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(0).RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();
            var runPorcPlazo = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text("% acciones pasadas de plazo" + ": " + PorcPasadasPlazo + "%"));
            runPorcPlazo.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesPorcPlazo);
            rowPorcPlazo.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(0).Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(runPorcPlazo));

            tablaAccionesPlan.AppendChild(rowPorcPlazo);

            tablaAccionesPlan.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>().ElementAt(1).Remove();
            #endregion

            #endregion

            #region cumplimientolegal

            List<VISTA_RequisitosLegales> listaReq = Datos.ListarRequisitos(idCentral, anio);

            #region evaluacion1
            if (listaReq != null && listaReq.Count > 0)
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

                        if (listaReq[0].cumple != null)
                        {
                            Cell theCell = InsertCellInWorksheet("B", 2, ws);
                            theCell.CellValue = new CellValue(listaReq[0].cumple.ToString());
                            theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                        }

                        if (listaReq[0].tramite != null)
                        {
                            Cell theCell = InsertCellInWorksheet("B", 3, ws);
                            theCell.CellValue = new CellValue(listaReq[0].tramite.ToString());
                            theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                        }

                        if (listaReq[0].nocumple != null)
                        {
                            Cell theCell = InsertCellInWorksheet("B", 4, ws);
                            theCell.CellValue = new CellValue(listaReq[0].nocumple.ToString());
                            theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                        }

                        if (listaReq[0].observacion != null)
                        {
                            Cell theCell = InsertCellInWorksheet("B", 5, ws);
                            theCell.CellValue = new CellValue(listaReq[0].observacion.ToString());
                            theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                        }

                        if (listaReq[0].noprocede != null)
                        {
                            Cell theCell = InsertCellInWorksheet("B", 6, ws);
                            theCell.CellValue = new CellValue(listaReq[0].noprocede.ToString());
                            theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                        }

                        if (listaReq[0].noverificado != null)
                        {
                            Cell theCell = InsertCellInWorksheet("B", 7, ws);
                            theCell.CellValue = new CellValue(listaReq[0].noverificado.ToString());
                            theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                        }

                        ws.Save();

                        var cachedValues = doc.MainDocumentPart.ChartParts.ElementAt(0).ChartSpace.Descendants<DocumentFormat.OpenXml.Drawing.Charts.Values>();
                        int row = 0;
                        int col = 0;
                        foreach (var cachedValue in cachedValues)
                        { // By column B ; C ; D
                            row = 0;

                            if (listaReq[0].cumple != null)
                                cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(0).Text = listaReq[0].cumple.ToString();
                            if (listaReq[0].tramite != null)
                                cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(1).Text = listaReq[0].tramite.ToString();
                            if (listaReq[0].nocumple != null)
                                cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(2).Text = listaReq[0].nocumple.ToString();
                            if (listaReq[0].observacion != null)
                                cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(3).Text = listaReq[0].observacion.ToString();
                            if (listaReq[0].noprocede != null)
                                cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(4).Text = listaReq[0].noprocede.ToString();
                            if (listaReq[0].noverificado != null)
                                cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(5).Text = listaReq[0].noverificado.ToString();

                            col++;
                        }
                    }
                }

                doc.MainDocumentPart.ChartParts.ElementAt(0).ChartSpace.Save();
            }

            #endregion

            #region evaluacion2

            if (listaReq != null && listaReq.Count > 1)
            {
                Stream stream = doc.MainDocumentPart.ChartParts.ElementAt(1).EmbeddedPackagePart.GetStream();
                using (SpreadsheetDocument ssDoc = SpreadsheetDocument.Open(stream, true))
                {
                    WorkbookPart wbPart = ssDoc.WorkbookPart;
                    Sheet theSheet = wbPart.Workbook.Descendants<Sheet>().
                      Where(s => s.Name == "Hoja1").FirstOrDefault();
                    if (theSheet != null)
                    {
                        Worksheet ws = ((WorksheetPart)(wbPart.GetPartById(theSheet.Id))).Worksheet;

                        if (listaReq[1].cumple != null)
                        {
                            Cell theCell = InsertCellInWorksheet("B", 2, ws);
                            theCell.CellValue = new CellValue(listaReq[1].cumple.ToString());
                            theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                        }

                        if (listaReq[1].tramite != null)
                        {
                            Cell theCell = InsertCellInWorksheet("B", 3, ws);
                            theCell.CellValue = new CellValue(listaReq[1].tramite.ToString());
                            theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                        }

                        if (listaReq[1].nocumple != null)
                        {
                            Cell theCell = InsertCellInWorksheet("B", 4, ws);
                            theCell.CellValue = new CellValue(listaReq[1].nocumple.ToString());
                            theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                        }

                        if (listaReq[1].observacion != null)
                        {
                            Cell theCell = InsertCellInWorksheet("B", 5, ws);
                            theCell.CellValue = new CellValue(listaReq[1].observacion.ToString());
                            theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                        }

                        if (listaReq[1].noprocede != null)
                        {
                            Cell theCell = InsertCellInWorksheet("B", 6, ws);
                            theCell.CellValue = new CellValue(listaReq[1].noprocede.ToString());
                            theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                        }

                        if (listaReq[1].noverificado != null)
                        {
                            Cell theCell = InsertCellInWorksheet("B", 7, ws);
                            theCell.CellValue = new CellValue(listaReq[1].noverificado.ToString());
                            theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                        }

                        ws.Save();

                        var cachedValues = doc.MainDocumentPart.ChartParts.ElementAt(1).ChartSpace.Descendants<DocumentFormat.OpenXml.Drawing.Charts.Values>();
                        int row = 0;
                        int col = 0;
                        foreach (var cachedValue in cachedValues)
                        { // By column B ; C ; D
                            row = 0;

                            if (listaReq[1].cumple != null)
                                cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(0).Text = listaReq[1].cumple.ToString();
                            if (listaReq[1].tramite != null)
                                cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(1).Text = listaReq[1].tramite.ToString();
                            if (listaReq[1].nocumple != null)
                                cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(2).Text = listaReq[1].nocumple.ToString();
                            if (listaReq[1].observacion != null)
                                cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(3).Text = listaReq[1].observacion.ToString();
                            if (listaReq[1].noprocede != null)
                                cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(4).Text = listaReq[1].noprocede.ToString();
                            if (listaReq[1].noverificado != null)
                                cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(5).Text = listaReq[1].noverificado.ToString();

                            col++;
                        }
                    }
                }

                doc.MainDocumentPart.ChartParts.ElementAt(1).ChartSpace.Save();
            }

            #endregion

            #endregion

            #region aspectos

            DocumentFormat.OpenXml.Wordprocessing.Table tablaAspectos = doc.MainDocumentPart.Document.Body.Descendants<DocumentFormat.OpenXml.Wordprocessing.Table>().ElementAt(24);
            DocumentFormat.OpenXml.Wordprocessing.TableRow filaAspectos = tablaAspectos.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>().ElementAt(2);

            List<VISTA_AspectosValoracion> listaAspectos = Datos.ListarAspectosValoracion(centroseleccionado.id, anio);

            listaAspectos = listaAspectos.Where(x => x.significancia6 == 1).ToList();

            for (int i = 0; i < listaAspectos.Count; i++)
            {
                DocumentFormat.OpenXml.Wordprocessing.TableRow rowCopy = (DocumentFormat.OpenXml.Wordprocessing.TableRow)filaAspectos.CloneNode(true);

                var runPropertiesCodigo = GetRunPropertyFromTableCell(rowCopy, 0);
                var runCodigo = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(listaAspectos[i].Codigo.ToString()));
                runCodigo.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesCodigo);

                rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(0).RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();//removes that text of the copied cell
                rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(0).Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(runCodigo));
                //I only get the the run properties from the first cell in this example, the rest of the cells get the document default style. 

                var runProperties = GetRunPropertyFromTableCell(rowCopy, 0);
                var run = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(listaAspectos[i].grupo.ToString()));
                run.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runProperties);

                rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(1).RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();//removes that text of the copied cell
                rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(1).Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(run));
                //I only get the the run properties from the first cell in this example, the rest of the cells get the document default style. 

                var runPropertiesTitulo = new DocumentFormat.OpenXml.Wordprocessing.RunProperties();
                runPropertiesTitulo.AppendChild(new RunFonts() { Ascii = "Calibri" });
                runPropertiesTitulo.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "18" });
                runPropertiesTitulo.AppendChild(new Justification() { Val = JustificationValues.Center });

                rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(2).RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();
                DocumentFormat.OpenXml.Wordprocessing.Run runTitulo = new DocumentFormat.OpenXml.Wordprocessing.Run();
                if (listaAspectos[i].Nombre != null)
                    runTitulo = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(listaAspectos[i].Nombre.ToString()));
                else
                    runTitulo = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(""));
                runTitulo.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesTitulo);
                rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(2).Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(runTitulo));

                var runPropertiesEstado = new DocumentFormat.OpenXml.Wordprocessing.RunProperties();
                runPropertiesEstado.AppendChild(new RunFonts() { Ascii = "Calibri" });
                runPropertiesEstado.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "18" });
                runPropertiesEstado.AppendChild(new Justification() { Val = JustificationValues.Center });

                rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(3).RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();
                var runEstado = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(listaAspectos[i].descripcion));
                runEstado.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesEstado);
                rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(3).Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(runEstado));

                var runPropertiesConsecucion = new DocumentFormat.OpenXml.Wordprocessing.RunProperties();
                runPropertiesConsecucion.AppendChild(new RunFonts() { Ascii = "Calibri" });
                runPropertiesConsecucion.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "18" });
                runPropertiesConsecucion.AppendChild(new Justification() { Val = JustificationValues.Center });

                rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(4).RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();
                var runConsecucion = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text("Significativo"));
                runConsecucion.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesConsecucion);
                rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(4).Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(runConsecucion));

                tablaAspectos.AppendChild(rowCopy);
            }

            tablaAspectos.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>().ElementAt(2).Remove();

            #endregion

            #region stakeholders

            List<VISTA_Comunicaciones> listaCom = Datos.ListarComunicaciones(centroseleccionado.id, anio);

            #region partesinteresadas

            List<VISTA_StakeholdersN3> listaStakeholders = Datos.ListarStakeholdersN3();

            DocumentFormat.OpenXml.Wordprocessing.Table tablaStakeholders = doc.MainDocumentPart.Document.Body.Descendants<DocumentFormat.OpenXml.Wordprocessing.Table>().ElementAt(31);
            DocumentFormat.OpenXml.Wordprocessing.TableRow filaStakeholders = tablaStakeholders.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>().ElementAt(1);

            foreach (VISTA_StakeholdersN3 pi in listaStakeholders)
            {


                List<VISTA_Comunicaciones> listaComFiltrado = listaCom.Where(x => x.stakeholder == pi.id).ToList();

                if (listaComFiltrado.Count > 0)
                {

                    DocumentFormat.OpenXml.Wordprocessing.TableRow rowCopy = (DocumentFormat.OpenXml.Wordprocessing.TableRow)filaStakeholders.CloneNode(true);

                    var runProperties = GetRunPropertyFromTableCell(rowCopy, 0);
                    var run = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(pi.denominacionn3.ToString()));
                    run.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runProperties);

                    rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(0).RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();//removes that text of the copied cell
                    rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(0).Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(run));
                    //I only get the the run properties from the first cell in this example, the rest of the cells get the document default style.                         

                    tablaStakeholders.AppendChild(rowCopy);

                }

            }
            tablaStakeholders.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>().ElementAt(1).Remove();

            #endregion

            #region canales
            List<comunicacion_canales> listaCanales = Datos.ListarCanalesComunicacion();

            DocumentFormat.OpenXml.Wordprocessing.Table tablaCanales = doc.MainDocumentPart.Document.Body.Descendants<DocumentFormat.OpenXml.Wordprocessing.Table>().ElementAt(32);
            DocumentFormat.OpenXml.Wordprocessing.TableRow filaCanales = tablaCanales.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>().ElementAt(1);

            foreach (comunicacion_canales canal in listaCanales)
            {
                List<VISTA_Comunicaciones> listaComFiltrado = listaCom.Where(x => x.idcanal == canal.id).ToList();

                if (listaComFiltrado.Count > 0)
                {

                    DocumentFormat.OpenXml.Wordprocessing.TableRow rowCopy = (DocumentFormat.OpenXml.Wordprocessing.TableRow)filaCanales.CloneNode(true);

                    var runProperties = GetRunPropertyFromTableCell(rowCopy, 0);
                    var run = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(canal.canal.ToString()));
                    run.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runProperties);

                    rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(0).RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();//removes that text of the copied cell
                    rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(0).Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(run));
                    //I only get the the run properties from the first cell in this example, the rest of the cells get the document default style.                         

                    tablaCanales.AppendChild(rowCopy);

                }

            }
            tablaCanales.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>().ElementAt(1).Remove();

            #endregion

            #endregion

            #region formacion

            DocumentFormat.OpenXml.Wordprocessing.Table tablaFormacion = doc.MainDocumentPart.Document.Body.Descendants<DocumentFormat.OpenXml.Wordprocessing.Table>().ElementAt(34);
            DocumentFormat.OpenXml.Wordprocessing.TableRow filaFormacion = tablaFormacion.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>().ElementAt(3);

            List<formacion> listaFormacion = new List<formacion>();

            if (centroseleccionado.tipo != 4)
                listaFormacion = Datos.ListarFormacion(centroseleccionado.id, anio);
            else
                listaFormacion = Datos.ListarFormacion(0, anio);


            for (int i = 0; i < listaFormacion.Count; i++)
            {
                DocumentFormat.OpenXml.Wordprocessing.TableRow rowCopy = (DocumentFormat.OpenXml.Wordprocessing.TableRow)filaFormacion.CloneNode(true);

                var runProperties = GetRunPropertyFromTableCell(rowCopy, 0);
                var run = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(listaFormacion[i].codigo + "-" + listaFormacion[i].denominacion));
                run.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runProperties);
                rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(0).RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();//removes that text of the copied cell
                rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(0).Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(run));

                var runPropertiesEficaz = GetRunPropertyFromTableCell(rowCopy, 1);
                string eficaz = "";
                if (listaFormacion[i].valoracion_eficacia == 1)
                    eficaz = "Eficaz";
                else
                    eficaz = "No eficaz";
                var runEficaz = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(eficaz));
                runEficaz.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesEficaz);
                rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(1).RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();//removes that text of the copied cell
                rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(1).Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(runEficaz));

                var runPropertiesNumAccionesTexto = GetRunPropertyFromTableCell(rowCopy, 2);
                runPropertiesNumAccionesTexto.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Color() { Val = "FFFFFF" });
                var runNumAccionesTexto = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text("Número de acciones de formación realizadas"));
                runNumAccionesTexto.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesNumAccionesTexto);
                rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(2).RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();//removes that text of the copied cell
                rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(2).Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(runNumAccionesTexto));

                var runPropertiesNumAccionesNum = GetRunPropertyFromTableCell(rowCopy, 3);
                var runNumAccionesNum = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(""));
                runNumAccionesNum.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesNumAccionesNum);
                rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(3).RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();//removes that text of the copied cell
                rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(3).Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(runNumAccionesNum));

                var runPropertiesNumAccionesPlanTexto = GetRunPropertyFromTableCell(rowCopy, 4);
                runPropertiesNumAccionesPlanTexto.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Color() { Val = "FFFFFF" });
                var runNumAccionesPlanTexto = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text("Número de acciones planificadas"));
                runNumAccionesPlanTexto.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesNumAccionesPlanTexto);
                rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(4).RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();//removes that text of the copied cell
                rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(4).Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(runNumAccionesPlanTexto));

                var runPropertiesNumAccionesPlanNum = GetRunPropertyFromTableCell(rowCopy, 5);
                var runNumAccionesPlanNum = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(""));
                runNumAccionesPlanNum.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesNumAccionesPlanNum);
                rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(5).RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();//removes that text of the copied cell
                rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(5).Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(runNumAccionesPlanNum));

                tablaFormacion.AppendChild(rowCopy);
            }

            tablaFormacion.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>().ElementAt(3).Remove();

            #endregion

            #region satisfaccion

            DocumentFormat.OpenXml.Wordprocessing.Table tablaSatisfaccion = doc.MainDocumentPart.Document.Body.Descendants<DocumentFormat.OpenXml.Wordprocessing.Table>().ElementAt(38);
            DocumentFormat.OpenXml.Wordprocessing.TableRow filaSatisfaccion = tablaSatisfaccion.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>().ElementAt(4);

            List<VISTA_ListarSatisfaccion> listaSatisfaccion = Datos.ListarSatisfaccion(centroseleccionado.id, anio);

            for (int i = 0; i < listaSatisfaccion.Count; i++)
            {
                DocumentFormat.OpenXml.Wordprocessing.TableRow rowCopy = (DocumentFormat.OpenXml.Wordprocessing.TableRow)filaSatisfaccion.CloneNode(true);

                var runProperties = GetRunPropertyFromTableCell(rowCopy, 0);
                var run = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(listaSatisfaccion[i].denominacion));
                run.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runProperties);
                rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(0).RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();//removes that text of the copied cell
                rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(0).Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(run));

                var runPropertiesFecha = GetRunPropertyFromTableCell(rowCopy, 1);
                var runFecha = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(listaSatisfaccion[i].fecharealizacion.ToString().Replace(" 0:00:00", "")));
                runFecha.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesFecha);
                rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(1).RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();//removes that text of the copied cell
                rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(1).Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(runFecha));

                var runPropertiesConclusiones = GetRunPropertyFromTableCell(rowCopy, 2);
                var runConclusiones = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(listaSatisfaccion[i].conclusiones));
                runConclusiones.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesConclusiones);
                rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(2).RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();//removes that text of the copied cell
                rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(2).Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(runConclusiones));


                tablaSatisfaccion.AppendChild(rowCopy);
            }

            tablaSatisfaccion.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>().ElementAt(4).Remove();

            #endregion

            #region emergencia

            DocumentFormat.OpenXml.Wordprocessing.Table tablaEmergencia = doc.MainDocumentPart.Document.Body.Descendants<DocumentFormat.OpenXml.Wordprocessing.Table>().ElementAt(44);
            DocumentFormat.OpenXml.Wordprocessing.TableRow filaEmergencia = tablaEmergencia.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>().ElementAt(2);

            List<VISTA_ListarEmergencias> listaEmergencia = Datos.ListarEmergencias(centroseleccionado.id, anio);

            for (int i = 0; i < listaEmergencia.Count; i++)
            {
                DocumentFormat.OpenXml.Wordprocessing.TableRow rowCopy = (DocumentFormat.OpenXml.Wordprocessing.TableRow)filaEmergencia.CloneNode(true);

                var runProperties = GetRunPropertyFromTableCell(rowCopy, 0);
                var run = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(listaEmergencia[i].descripcion));
                run.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runProperties);
                rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(0).RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();//removes that text of the copied cell
                rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(0).Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(run));

                var runPropertiesFecha = GetRunPropertyFromTableCell(rowCopy, 1);
                var runFecha = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(listaEmergencia[i].fecharealizacion.ToString().Replace(" 0:00:00", "")));
                runFecha.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesFecha);
                rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(1).RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();//removes that text of the copied cell
                rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(1).Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(runFecha));

                var runPropertiesPersonal = GetRunPropertyFromTableCell(rowCopy, 2);
                var runPersonañ = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(listaEmergencia[i].personalimplicado));
                runPersonañ.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesPersonal);
                rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(2).RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();//removes that text of the copied cell
                rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(2).Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(runPersonañ));

                var runPropertiesEscenario = GetRunPropertyFromTableCell(rowCopy, 3);
                var runEscenario = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(listaEmergencia[i].escenarioplanteado));
                runEscenario.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesEscenario);
                rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(3).RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();//removes that text of the copied cell
                rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(3).Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(runEscenario));

                var runPropertiesConclusiones = GetRunPropertyFromTableCell(rowCopy, 4);
                var runConclusiones = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(listaEmergencia[i].conclusiones));
                runConclusiones.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesConclusiones);
                rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(4).RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();//removes that text of the copied cell
                rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(4).Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(runConclusiones));

                tablaEmergencia.AppendChild(rowCopy);
            }

            tablaEmergencia.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>().ElementAt(2).Remove();

            #endregion

            #region indicadores


            List<VISTA_IndicadoresFichaDireccion> listaIndicadores = Datos.ListarIndicadoresPlanificados(centroseleccionado.id, int.Parse(anio));


            #region clonartablas
            DocumentFormat.OpenXml.Wordprocessing.Table tablaIndicadores = doc.MainDocumentPart.Document.Body.Descendants<DocumentFormat.OpenXml.Wordprocessing.Table>().ElementAt(40);
            DocumentFormat.OpenXml.Wordprocessing.TableRow filaIndicadores1 = tablaIndicadores.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>().ElementAt(1);
            DocumentFormat.OpenXml.Wordprocessing.TableRow filaIndicadores2 = tablaIndicadores.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>().ElementAt(3);
            DocumentFormat.OpenXml.Wordprocessing.TableRow filaIndicadores3 = tablaIndicadores.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>().ElementAt(6);
            DocumentFormat.OpenXml.Wordprocessing.TableRow filaIndicadores4 = tablaIndicadores.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>().ElementAt(9);
            DocumentFormat.OpenXml.Wordprocessing.Table tablaGraficos = doc.MainDocumentPart.Document.Body.Descendants<DocumentFormat.OpenXml.Wordprocessing.Table>().ElementAt(41);

            if (listaIndicadores.Count > 1)
            {
                for (int i = 1; i < listaIndicadores.Count; i++)
                {
                    DocumentFormat.OpenXml.Wordprocessing.Table clon1 = (DocumentFormat.OpenXml.Wordprocessing.Table)doc.MainDocumentPart.Document.Body.Descendants<DocumentFormat.OpenXml.Wordprocessing.Table>().ElementAt(40).Clone();
                    DocumentFormat.OpenXml.Wordprocessing.Table clon2 = (DocumentFormat.OpenXml.Wordprocessing.Table)doc.MainDocumentPart.Document.Body.Descendants<DocumentFormat.OpenXml.Wordprocessing.Table>().ElementAt(41).Clone();

                    tablaGraficos.InsertAfterSelf(clon2);
                    tablaGraficos.InsertAfterSelf(clon1);
                    tablaGraficos.InsertAfterSelf(new DocumentFormat.OpenXml.Wordprocessing.Paragraph());

                    //doc.MainDocumentPart.ChangeIdOfPart(doc.MainDocumentPart.ChartParts.ElementAt(i), "rId" + (i + 15).ToString());

                }

            }
            #endregion

            int idTablaIndicadores = 40;
            int idTablaGraficos = 41;

            for (int i = 0; i < listaIndicadores.Count; i++)
            {
                #region actualizar tabla usada
                tablaIndicadores = doc.MainDocumentPart.Document.Body.Descendants<DocumentFormat.OpenXml.Wordprocessing.Table>().ElementAt(idTablaIndicadores);
                filaIndicadores1 = tablaIndicadores.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>().ElementAt(1);
                filaIndicadores2 = tablaIndicadores.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>().ElementAt(3);
                filaIndicadores3 = tablaIndicadores.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>().ElementAt(6);
                filaIndicadores4 = tablaIndicadores.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>().ElementAt(9);
                tablaGraficos = doc.MainDocumentPart.Document.Body.Descendants<DocumentFormat.OpenXml.Wordprocessing.Table>().ElementAt(idTablaGraficos);
                #endregion

                #region fila1
                var runPropertiesIndNombre = GetRunPropertyFromTableCell(filaIndicadores1, 0);
                var runIndNombre = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(listaIndicadores[i].Nombre));
                runIndNombre.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesIndNombre);
                filaIndicadores1.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(0).RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();//removes that text of the copied cell
                filaIndicadores1.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(0).Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(runIndNombre));
                #endregion

                #region fila2
                var runPropertiesIndUnidad = GetRunPropertyFromTableCell(filaIndicadores2, 0);
                var runIndUnidad = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(listaIndicadores[i].Unidad));
                runIndUnidad.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesIndUnidad);
                filaIndicadores2.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(0).RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();//removes that text of the copied cell
                filaIndicadores2.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(0).Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(runIndUnidad));

                var runPropertiesIndProceso = GetRunPropertyFromTableCell(filaIndicadores2, 1);
                var runIndProceso = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(listaIndicadores[i].proceso));
                runIndProceso.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesIndProceso);
                filaIndicadores2.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(1).RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();//removes that text of the copied cell
                filaIndicadores2.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(1).Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(runIndProceso));

                var runPropertiesIndMetodo = GetRunPropertyFromTableCell(filaIndicadores2, 2);
                var runIndMetodo = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(listaIndicadores[i].MetodoMedicion));
                runIndMetodo.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesIndMetodo);
                filaIndicadores2.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(2).RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();//removes that text of the copied cell
                filaIndicadores2.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(2).Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(runIndMetodo));
                #endregion

                #region fila3
                var runPropertiesRef1 = GetRunPropertyFromTableCell(filaIndicadores3, 0);
                var runRef1 = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(listaIndicadores[i].ValorReferencia1.ToString()));
                runRef1.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesRef1);
                filaIndicadores3.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(0).RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();//removes that text of the copied cell
                filaIndicadores3.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(0).Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(runRef1));

                var runPropertiesRef2 = GetRunPropertyFromTableCell(filaIndicadores3, 1);
                var runRef2 = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(listaIndicadores[i].ValorReferencia2.ToString()));
                runRef2.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesRef2);
                filaIndicadores3.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(1).RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();//removes that text of the copied cell
                filaIndicadores3.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(1).Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(runRef2));

                var runPropertiesRef3 = GetRunPropertyFromTableCell(filaIndicadores3, 2);
                var runRef3 = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(listaIndicadores[i].ValorReferencia3.ToString()));
                runRef3.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesRef3);
                filaIndicadores3.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(2).RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();//removes that text of the copied cell
                filaIndicadores3.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(2).Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(runRef3));

                var runPropertiesRef4 = GetRunPropertyFromTableCell(filaIndicadores3, 3);
                var runRef4 = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(listaIndicadores[i].ValorReferencia4.ToString()));
                runRef4.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesRef4);
                filaIndicadores3.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(3).RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();//removes that text of the copied cell
                filaIndicadores3.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(3).Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(runRef4));

                var runPropertiesRef5 = GetRunPropertyFromTableCell(filaIndicadores3, 4);
                var runRef5 = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(listaIndicadores[i].ValorReferencia5.ToString()));
                runRef5.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesRef5);
                filaIndicadores3.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(4).RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();//removes that text of the copied cell
                filaIndicadores3.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(4).Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(runRef5));

                var runPropertiesRef6 = GetRunPropertyFromTableCell(filaIndicadores3, 5);
                var runRef6 = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(listaIndicadores[i].ValorReferencia6.ToString()));
                runRef6.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesRef6);
                filaIndicadores3.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(5).RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();//removes that text of the copied cell
                filaIndicadores3.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(5).Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(runRef6));

                var runPropertiesRef7 = GetRunPropertyFromTableCell(filaIndicadores3, 6);
                var runRef7 = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(listaIndicadores[i].ValorReferencia7.ToString()));
                runRef7.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesRef7);
                filaIndicadores3.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(6).RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();//removes that text of the copied cell
                filaIndicadores3.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(6).Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(runRef7));

                var runPropertiesRef8 = GetRunPropertyFromTableCell(filaIndicadores3, 7);
                var runRef8 = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(listaIndicadores[i].ValorReferencia8.ToString()));
                runRef8.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesRef8);
                filaIndicadores3.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(7).RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();//removes that text of the copied cell
                filaIndicadores3.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(7).Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(runRef8));

                var runPropertiesRef9 = GetRunPropertyFromTableCell(filaIndicadores3, 8);
                var runRef9 = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(listaIndicadores[i].ValorReferencia9.ToString()));
                runRef9.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesRef9);
                filaIndicadores3.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(8).RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();//removes that text of the copied cell
                filaIndicadores3.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(8).Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(runRef9));

                var runPropertiesRef10 = GetRunPropertyFromTableCell(filaIndicadores3, 9);
                var runRef10 = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(listaIndicadores[i].ValorReferencia10.ToString()));
                runRef10.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesRef10);
                filaIndicadores3.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(9).RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();//removes that text of the copied cell
                filaIndicadores3.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(9).Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(runRef10));

                var runPropertiesRef11 = GetRunPropertyFromTableCell(filaIndicadores3, 10);
                var runRef11 = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(listaIndicadores[i].ValorReferencia11.ToString()));
                runRef11.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesRef11);
                filaIndicadores3.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(10).RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();//removes that text of the copied cell
                filaIndicadores3.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(10).Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(runRef11));

                var runPropertiesRef12 = GetRunPropertyFromTableCell(filaIndicadores3, 11);
                var runRef12 = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(listaIndicadores[i].ValorReferencia12.ToString()));
                runRef12.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesRef12);
                filaIndicadores3.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(11).RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();//removes that text of the copied cell
                filaIndicadores3.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(11).Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(runRef12));
                #endregion

                #region fila4
                var runPropertiesVal1 = GetRunPropertyFromTableCell(filaIndicadores4, 0);
                var runVal1 = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(listaIndicadores[i].ValorCalculado1.ToString().Replace("0,00", "")));
                runVal1.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesVal1);
                filaIndicadores4.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(0).RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();//removes that text of the copied cell
                filaIndicadores4.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(0).Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(runVal1));

                var runPropertiesVal2 = GetRunPropertyFromTableCell(filaIndicadores4, 1);
                var runVal2 = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(listaIndicadores[i].ValorCalculado2.ToString().Replace("0,00", "")));
                runVal2.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesVal2);
                filaIndicadores4.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(1).RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();//removes that text of the copied cell
                filaIndicadores4.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(1).Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(runVal2));

                var runPropertiesVal3 = GetRunPropertyFromTableCell(filaIndicadores4, 2);
                var runVal3 = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(listaIndicadores[i].ValorCalculado3.ToString().Replace("0,00", "")));
                runVal3.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesVal3);
                filaIndicadores4.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(2).RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();//removes that text of the copied cell
                filaIndicadores4.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(2).Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(runVal3));

                var runPropertiesVal4 = GetRunPropertyFromTableCell(filaIndicadores4, 3);
                var runVal4 = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(listaIndicadores[i].ValorCalculado4.ToString().Replace("0,00", "")));
                runVal4.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesVal4);
                filaIndicadores4.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(3).RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();//removes that text of the copied cell
                filaIndicadores4.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(3).Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(runVal4));

                var runPropertiesVal5 = GetRunPropertyFromTableCell(filaIndicadores4, 4);
                var runVal5 = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(listaIndicadores[i].ValorCalculado5.ToString().Replace("0,00", "")));
                runVal5.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesVal5);
                filaIndicadores4.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(4).RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();//removes that text of the copied cell
                filaIndicadores4.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(4).Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(runVal5));

                var runPropertiesVal6 = GetRunPropertyFromTableCell(filaIndicadores4, 5);
                var runVal6 = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(listaIndicadores[i].ValorCalculado6.ToString().Replace("0,00", "")));
                runVal6.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesVal6);
                filaIndicadores4.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(5).RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();//removes that text of the copied cell
                filaIndicadores4.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(5).Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(runVal6));

                var runPropertiesVal7 = GetRunPropertyFromTableCell(filaIndicadores4, 6);
                var runVal7 = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(listaIndicadores[i].ValorCalculado7.ToString().Replace("0,00", "")));
                runVal7.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesVal7);
                filaIndicadores4.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(6).RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();//removes that text of the copied cell
                filaIndicadores4.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(6).Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(runVal7));

                var runPropertiesVal8 = GetRunPropertyFromTableCell(filaIndicadores4, 7);
                var runVal8 = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(listaIndicadores[i].ValorCalculado8.ToString().Replace("0,00", "")));
                runVal8.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesVal8);
                filaIndicadores4.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(7).RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();//removes that text of the copied cell
                filaIndicadores4.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(7).Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(runVal8));

                var runPropertiesVal9 = GetRunPropertyFromTableCell(filaIndicadores4, 8);
                var runVal9 = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(listaIndicadores[i].ValorCalculado9.ToString().Replace("0,00", "")));
                runVal9.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesVal9);
                filaIndicadores4.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(8).RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();//removes that text of the copied cell
                filaIndicadores4.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(8).Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(runVal9));

                var runPropertiesVal10 = GetRunPropertyFromTableCell(filaIndicadores4, 9);
                var runVal10 = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(listaIndicadores[i].ValorCalculado10.ToString().Replace("0,00", "")));
                runVal10.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesVal10);
                filaIndicadores4.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(9).RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();//removes that text of the copied cell
                filaIndicadores4.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(9).Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(runVal10));

                var runPropertiesVal11 = GetRunPropertyFromTableCell(filaIndicadores4, 10);
                var runVal11 = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(listaIndicadores[i].ValorCalculado11.ToString().Replace("0,00", "")));
                runVal11.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesVal11);
                filaIndicadores4.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(10).RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();//removes that text of the copied cell
                filaIndicadores4.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(10).Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(runVal11));

                var runPropertiesVal12 = GetRunPropertyFromTableCell(filaIndicadores4, 11);
                var runVal12 = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(listaIndicadores[i].ValorCalculado12.ToString().Replace("0,00", "")));
                runVal12.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesVal12);
                filaIndicadores4.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(11).RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();//removes that text of the copied cell
                filaIndicadores4.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(11).Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(runVal12));
                #endregion

                #region grafico

                //Stream stream2 = doc.MainDocumentPart.ChartParts.ElementAt(1).EmbeddedPackagePart.GetStream();
                //using (SpreadsheetDocument ssDoc = SpreadsheetDocument.Open(stream2, true))
                //{
                //    WorkbookPart wbPart = ssDoc.WorkbookPart;
                //    Sheet theSheet = wbPart.Workbook.Descendants<Sheet>().
                //      Where(s => s.Name == "Hoja1").FirstOrDefault();
                //    if (theSheet != null)
                //    {
                //        Worksheet ws = ((WorksheetPart)(wbPart.GetPartById(theSheet.Id))).Worksheet;


                #region celdasreferencia
                //if (listaIndicadores[i].ValorReferencia1 != null)
                //{
                //    Cell theCell = InsertCellInWorksheet("B", 2, ws);
                //    theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                //    if (listaIndicadores[i].ValorReferencia1 != null && listaIndicadores[i].ValorReferencia1 != 0)
                //        theCell.CellValue = new CellValue(((decimal)listaIndicadores[i].ValorReferencia1).ToString("F").Replace(",", "."));
                //    else
                //        theCell.CellValue = new CellValue("0,00");
                //}

                //if (listaIndicadores[i].ValorReferencia2 != null)
                //{
                //    Cell theCell = InsertCellInWorksheet("B", 3, ws);
                //    theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                //    if (listaIndicadores[i].ValorReferencia2 != null && listaIndicadores[i].ValorReferencia2 != 0)
                //        theCell.CellValue = new CellValue(((decimal)listaIndicadores[i].ValorReferencia2).ToString("F").Replace(",", "."));
                //    else
                //        theCell.CellValue = new CellValue("0,00");
                //}

                //if (listaIndicadores[i].ValorReferencia3 != null)
                //{
                //    Cell theCell = InsertCellInWorksheet("B", 4, ws);
                //    theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                //    if (listaIndicadores[i].ValorReferencia3 != null && listaIndicadores[i].ValorReferencia3 != 0)
                //        theCell.CellValue = new CellValue(((decimal)listaIndicadores[i].ValorReferencia3).ToString("F").Replace(",", "."));
                //    else
                //        theCell.CellValue = new CellValue("0,00");
                //}

                //if (listaIndicadores[i].ValorReferencia4 != null)
                //{
                //    Cell theCell = InsertCellInWorksheet("B", 5, ws);
                //    theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                //    if (listaIndicadores[i].ValorReferencia4 != null && listaIndicadores[i].ValorReferencia4 != 0)
                //        theCell.CellValue = new CellValue(((decimal)listaIndicadores[i].ValorReferencia4).ToString("F").Replace(",", "."));
                //    else
                //        theCell.CellValue = new CellValue("0,00");
                //}

                //if (listaIndicadores[i].ValorReferencia5 != null)
                //{
                //    Cell theCell = InsertCellInWorksheet("B", 6, ws);
                //    theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                //    if (listaIndicadores[i].ValorReferencia5 != null && listaIndicadores[i].ValorReferencia5 != 0)
                //        theCell.CellValue = new CellValue(((decimal)listaIndicadores[i].ValorReferencia5).ToString("F").Replace(",", "."));
                //    else
                //        theCell.CellValue = new CellValue("0,00");
                //}

                //if (listaIndicadores[i].ValorReferencia6 != null)
                //{
                //    Cell theCell = InsertCellInWorksheet("B", 7, ws);
                //    theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                //    if (listaIndicadores[i].ValorReferencia6 != null && listaIndicadores[i].ValorReferencia6 != 0)
                //        theCell.CellValue = new CellValue(((decimal)listaIndicadores[i].ValorReferencia6).ToString("F").Replace(",", "."));
                //    else
                //        theCell.CellValue = new CellValue("0,00");

                //}

                //if (listaIndicadores[i].ValorReferencia7 != null)
                //{
                //    Cell theCell = InsertCellInWorksheet("B", 8, ws);
                //    theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                //    if (listaIndicadores[i].ValorReferencia7 != null && listaIndicadores[i].ValorReferencia7 != 0)
                //        theCell.CellValue = new CellValue(((decimal)listaIndicadores[i].ValorReferencia7).ToString("F").Replace(",", "."));
                //    else
                //        theCell.CellValue = new CellValue("0,00");

                //}
                //if (listaIndicadores[i].ValorReferencia8 != null)
                //{
                //    Cell theCell = InsertCellInWorksheet("B", 9, ws);
                //    theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                //    if (listaIndicadores[i].ValorReferencia8 != null && listaIndicadores[i].ValorReferencia8 != 0)
                //        theCell.CellValue = new CellValue(((decimal)listaIndicadores[i].ValorReferencia8).ToString("F").Replace(",", "."));
                //    else
                //        theCell.CellValue = new CellValue("0,00");

                //}
                //if (listaIndicadores[i].ValorReferencia9 != null)
                //{
                //    Cell theCell = InsertCellInWorksheet("B", 10, ws);
                //    theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                //    if (listaIndicadores[i].ValorReferencia9 != null && listaIndicadores[i].ValorReferencia9 != 0)
                //        theCell.CellValue = new CellValue(((decimal)listaIndicadores[i].ValorReferencia9).ToString("F").Replace(",", "."));
                //    else
                //        theCell.CellValue = new CellValue("0,00");

                //}
                //if (listaIndicadores[i].ValorReferencia10 != null)
                //{
                //    Cell theCell = InsertCellInWorksheet("B", 11, ws);
                //    theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                //    if (listaIndicadores[i].ValorReferencia10 != null && listaIndicadores[i].ValorReferencia10 != 0)
                //        theCell.CellValue = new CellValue(((decimal)listaIndicadores[i].ValorReferencia10).ToString("F").Replace(",", "."));
                //    else
                //        theCell.CellValue = new CellValue("0,00");

                //}
                //if (listaIndicadores[i].ValorReferencia11 != null)
                //{
                //    Cell theCell = InsertCellInWorksheet("B", 12, ws);
                //    theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                //    if (listaIndicadores[i].ValorReferencia11 != null && listaIndicadores[i].ValorReferencia11 != 0)
                //        theCell.CellValue = new CellValue(((decimal)listaIndicadores[i].ValorReferencia11).ToString("F").Replace(",", "."));
                //    else
                //        theCell.CellValue = new CellValue("0,00");

                //}
                //if (listaIndicadores[i].ValorReferencia12 != null)
                //{
                //    Cell theCell = InsertCellInWorksheet("B", 13, ws);
                //    theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                //    if (listaIndicadores[i].ValorReferencia12 != null && listaIndicadores[i].ValorReferencia12 != 0)
                //        theCell.CellValue = new CellValue(((decimal)listaIndicadores[i].ValorReferencia12).ToString("F").Replace(",", "."));
                //    else
                //        theCell.CellValue = new CellValue("0,00");

                //}
                #endregion

                #region celdasoperacion
                //if (listaIndicadores[i].ValorCalculado1 != null)
                //{
                //    Cell theCell = InsertCellInWorksheet("C", 2, ws);
                //    theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                //    if (listaIndicadores[i].ValorCalculado1 != null && listaIndicadores[i].ValorCalculado1 != 0)
                //        theCell.CellValue = new CellValue(((decimal)listaIndicadores[i].ValorCalculado1).ToString("F").Replace(",", "."));
                //    else
                //        theCell.CellValue = new CellValue("0,00");
                //}

                //if (listaIndicadores[i].ValorCalculado2 != null)
                //{
                //    Cell theCell = InsertCellInWorksheet("D", 3, ws);
                //    theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                //    if (listaIndicadores[i].ValorCalculado2 != null && listaIndicadores[i].ValorCalculado2 != 0)
                //        theCell.CellValue = new CellValue(((decimal)listaIndicadores[i].ValorCalculado2).ToString("F").Replace(",", "."));
                //    else
                //        theCell.CellValue = new CellValue("0,00");
                //}

                //if (listaIndicadores[i].ValorCalculado3 != null)
                //{
                //    Cell theCell = InsertCellInWorksheet("C", 4, ws);
                //    theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                //    if (listaIndicadores[i].ValorCalculado3 != null && listaIndicadores[i].ValorCalculado3 != 0)
                //        theCell.CellValue = new CellValue(((decimal)listaIndicadores[i].ValorCalculado3).ToString("F").Replace(",", "."));
                //    else
                //        theCell.CellValue = new CellValue("0,00");
                //}

                //if (listaIndicadores[i].ValorCalculado4 != null)
                //{
                //    Cell theCell = InsertCellInWorksheet("C", 5, ws);
                //    theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                //    if (listaIndicadores[i].ValorCalculado4 != null && listaIndicadores[i].ValorCalculado4 != 0)
                //        theCell.CellValue = new CellValue(((decimal)listaIndicadores[i].ValorCalculado4).ToString("F").Replace(",", "."));
                //    else
                //        theCell.CellValue = new CellValue("0,00");
                //}

                //if (listaIndicadores[i].ValorCalculado5 != null)
                //{
                //    Cell theCell = InsertCellInWorksheet("C", 6, ws);
                //    theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                //    if (listaIndicadores[i].ValorCalculado5 != null && listaIndicadores[i].ValorCalculado5 != 0)
                //        theCell.CellValue = new CellValue(((decimal)listaIndicadores[i].ValorCalculado5).ToString("F").Replace(",", "."));
                //    else
                //        theCell.CellValue = new CellValue("0,00");
                //}

                //if (listaIndicadores[i].ValorCalculado6 != null)
                //{
                //    Cell theCell = InsertCellInWorksheet("C", 7, ws);
                //    theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                //    if (listaIndicadores[i].ValorCalculado6 != null && listaIndicadores[i].ValorCalculado6 != 0)
                //        theCell.CellValue = new CellValue(((decimal)listaIndicadores[i].ValorCalculado6).ToString("F").Replace(",", "."));
                //    else
                //        theCell.CellValue = new CellValue("0,00");

                //}

                //if (listaIndicadores[i].ValorCalculado7 != null)
                //{
                //    Cell theCell = InsertCellInWorksheet("C", 8, ws);
                //    theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                //    if (listaIndicadores[i].ValorCalculado7 != null && listaIndicadores[i].ValorCalculado7 != 0)
                //        theCell.CellValue = new CellValue(((decimal)listaIndicadores[i].ValorCalculado7).ToString("F").Replace(",", "."));
                //    else
                //        theCell.CellValue = new CellValue("0,00");

                //}
                //if (listaIndicadores[i].ValorCalculado8 != null)
                //{
                //    Cell theCell = InsertCellInWorksheet("C", 9, ws);
                //    theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                //    if (listaIndicadores[i].ValorCalculado8 != null && listaIndicadores[i].ValorCalculado8 != 0)
                //        theCell.CellValue = new CellValue(((decimal)listaIndicadores[i].ValorCalculado8).ToString("F").Replace(",", "."));
                //    else
                //        theCell.CellValue = new CellValue("0,00");

                //}
                //if (listaIndicadores[i].ValorCalculado9 != null)
                //{
                //    Cell theCell = InsertCellInWorksheet("C", 10, ws);
                //    theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                //    if (listaIndicadores[i].ValorCalculado9 != null && listaIndicadores[i].ValorCalculado9 != 0)
                //        theCell.CellValue = new CellValue(((decimal)listaIndicadores[i].ValorCalculado9).ToString("F").Replace(",", "."));
                //    else
                //        theCell.CellValue = new CellValue("0,00");

                //}
                //if (listaIndicadores[i].ValorCalculado10 != null)
                //{
                //    Cell theCell = InsertCellInWorksheet("C", 11, ws);
                //    theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                //    if (listaIndicadores[i].ValorCalculado10 != null && listaIndicadores[i].ValorCalculado10 != 0)
                //        theCell.CellValue = new CellValue(((decimal)listaIndicadores[i].ValorCalculado10).ToString("F").Replace(",", "."));
                //    else
                //        theCell.CellValue = new CellValue("0,00");

                //}
                //if (listaIndicadores[i].ValorCalculado11 != null)
                //{
                //    Cell theCell = InsertCellInWorksheet("C", 12, ws);
                //    theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                //    if (listaIndicadores[i].ValorCalculado11 != null && listaIndicadores[i].ValorCalculado11 != 0)
                //        theCell.CellValue = new CellValue(((decimal)listaIndicadores[i].ValorCalculado11).ToString("F").Replace(",", "."));
                //    else
                //        theCell.CellValue = new CellValue("0,00");

                //}
                //if (listaIndicadores[i].ValorCalculado12 != null)
                //{
                //    Cell theCell = InsertCellInWorksheet("C", 13, ws);
                //    theCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                //    if (listaIndicadores[i].ValorCalculado12 != null && listaIndicadores[i].ValorCalculado12 != 0)
                //        theCell.CellValue = new CellValue(((decimal)listaIndicadores[i].ValorCalculado12).ToString("F").Replace(",", "."));
                //    else
                //        theCell.CellValue = new CellValue("0,00");

                //}
                #endregion

                //ws.Save();

                #region valores cache
                //var cachedValues = doc.MainDocumentPart.ChartParts.ElementAt(1).ChartSpace.Descendants<DocumentFormat.OpenXml.Drawing.Charts.Values>();
                //int row = 0;
                //int col = 0;
                //foreach (var cachedValue in cachedValues)
                //{ // By column B ; C ; D
                //    row = 0;
                //    if (col == 1)
                //    {
                //        if (listaIndicadores[i].ValorReferencia1 != null && listaIndicadores[i].ValorReferencia1 != 0)
                //            cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(0).Text = ((decimal)listaIndicadores[i].ValorReferencia1).ToString("F").Replace(",", ".");
                //        else
                //            cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(0).Text = "";
                //        if (listaIndicadores[i].ValorReferencia2 != null && listaIndicadores[i].ValorReferencia2 != 0)
                //            cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(1).Text = ((decimal)listaIndicadores[i].ValorReferencia2).ToString("F").Replace(",", ".");
                //        else
                //            cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(1).Text = "";
                //        if (listaIndicadores[i].ValorReferencia3 != null && listaIndicadores[i].ValorReferencia3 != 0)
                //            cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(2).Text = ((decimal)listaIndicadores[i].ValorReferencia3).ToString("F").Replace(",", ".");
                //        else
                //            cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(2).Text = "";
                //        if (listaIndicadores[i].ValorReferencia4 != null && listaIndicadores[i].ValorReferencia4 != 0)
                //            cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(3).Text = ((decimal)listaIndicadores[i].ValorReferencia4).ToString("F").Replace(",", ".");
                //        else
                //            cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(3).Text = "";
                //        if (listaIndicadores[i].ValorReferencia5 != null && listaIndicadores[i].ValorReferencia5 != 0)
                //            cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(4).Text = ((decimal)listaIndicadores[i].ValorReferencia5).ToString("F").Replace(",", ".");
                //        else
                //            cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(4).Text = "";
                //        if (listaIndicadores[i].ValorReferencia6 != null && listaIndicadores[i].ValorReferencia6 != 0)
                //            cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(5).Text = ((decimal)listaIndicadores[i].ValorReferencia6).ToString("F").Replace(",", ".");
                //        else
                //            cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(5).Text = "";
                //        if (listaIndicadores[i].ValorReferencia7 != null && listaIndicadores[i].ValorReferencia7 != 0)
                //            cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(6).Text = ((decimal)listaIndicadores[i].ValorReferencia7).ToString("F").Replace(",", ".");
                //        else
                //            cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(6).Text = "";
                //        if (listaIndicadores[i].ValorReferencia8 != null && listaIndicadores[i].ValorReferencia8 != 0)
                //            cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(7).Text = ((decimal)listaIndicadores[i].ValorReferencia8).ToString("F").Replace(",", ".");
                //        else
                //            cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(7).Text = "";
                //        if (listaIndicadores[i].ValorReferencia9 != null && listaIndicadores[i].ValorReferencia9 != 0)
                //            cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(8).Text = ((decimal)listaIndicadores[i].ValorReferencia9).ToString("F").Replace(",", ".");
                //        else
                //            cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(8).Text = "";
                //        if (listaIndicadores[i].ValorReferencia10 != null && listaIndicadores[i].ValorReferencia10 != 0)
                //            cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(9).Text = ((decimal)listaIndicadores[i].ValorReferencia10).ToString("F").Replace(",", ".");
                //        else
                //            cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(9).Text = "";
                //        if (listaIndicadores[i].ValorReferencia11 != null && listaIndicadores[i].ValorReferencia11 != 0)
                //            cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(10).Text = ((decimal)listaIndicadores[i].ValorReferencia11).ToString("F").Replace(",", ".");
                //        else
                //            cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(10).Text = "";
                //        if (listaIndicadores[i].ValorReferencia12 != null && listaIndicadores[i].ValorReferencia12 != 0)
                //            cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(11).Text = ((decimal)listaIndicadores[i].ValorReferencia12).ToString("F").Replace(",", ".");
                //        else
                //            cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(11).Text = "";
                //    }

                //    if (col == 0)
                //    {
                //        if (listaIndicadores[i].ValorCalculado1 != null && listaIndicadores[i].ValorCalculado1 != 0)
                //            cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(0).Text = ((decimal)listaIndicadores[i].ValorCalculado1).ToString("F").Replace(",", ".");
                //        else
                //            cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(0).Text = "";
                //        if (listaIndicadores[i].ValorCalculado2 != null && listaIndicadores[i].ValorCalculado2 != 0)
                //            cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(1).Text = ((decimal)listaIndicadores[i].ValorCalculado2).ToString("F").Replace(",", ".");
                //        else
                //            cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(1).Text = "";
                //        if (listaIndicadores[i].ValorCalculado3 != null && listaIndicadores[i].ValorCalculado3 != 0)
                //            cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(2).Text = ((decimal)listaIndicadores[i].ValorCalculado3).ToString("F").Replace(",", ".");
                //        else
                //            cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(2).Text = "";
                //        if (listaIndicadores[i].ValorCalculado4 != null && listaIndicadores[i].ValorCalculado4 != 0)
                //            cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(3).Text = ((decimal)listaIndicadores[i].ValorCalculado4).ToString("F").Replace(",", ".");
                //        else
                //            cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(3).Text = "";
                //        if (listaIndicadores[i].ValorCalculado5 != null && listaIndicadores[i].ValorCalculado5 != 0)
                //            cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(4).Text = ((decimal)listaIndicadores[i].ValorCalculado5).ToString("F").Replace(",", ".");
                //        else
                //            cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(4).Text = "";
                //        if (listaIndicadores[i].ValorCalculado6 != null && listaIndicadores[i].ValorCalculado6 != 0)
                //            cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(5).Text = ((decimal)listaIndicadores[i].ValorCalculado6).ToString("F").Replace(",", ".");
                //        else
                //            cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(5).Text = "";
                //        if (listaIndicadores[i].ValorCalculado7 != null && listaIndicadores[i].ValorCalculado7 != 0)
                //            cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(6).Text = ((decimal)listaIndicadores[i].ValorCalculado7).ToString("F").Replace(",", ".");
                //        else
                //            cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(6).Text = "";
                //        if (listaIndicadores[i].ValorCalculado8 != null && listaIndicadores[i].ValorCalculado8 != 0)
                //            cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(7).Text = ((decimal)listaIndicadores[i].ValorCalculado8).ToString("F").Replace(",", ".");
                //        else
                //            cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(7).Text = "";
                //        if (listaIndicadores[i].ValorCalculado9 != null && listaIndicadores[i].ValorCalculado9 != 0)
                //            cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(8).Text = ((decimal)listaIndicadores[i].ValorCalculado9).ToString("F").Replace(",", ".");
                //        else
                //            cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(8).Text = "";
                //        if (listaIndicadores[i].ValorCalculado10 != null && listaIndicadores[i].ValorCalculado10 != 0)
                //            cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(9).Text = ((decimal)listaIndicadores[i].ValorCalculado10).ToString("F").Replace(",", ".");
                //        else
                //            cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(9).Text = "";
                //        if (listaIndicadores[i].ValorCalculado11 != null && listaIndicadores[i].ValorCalculado11 != 0)
                //            cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(10).Text = ((decimal)listaIndicadores[i].ValorCalculado11).ToString("F").Replace(",", ".");
                //        else
                //            cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(10).Text = "";
                //        if (listaIndicadores[i].ValorCalculado12 != null && listaIndicadores[i].ValorCalculado12 != 0)
                //            cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(11).Text = ((decimal)listaIndicadores[i].ValorCalculado12).ToString("F").Replace(",", ".");
                //        else
                //            cachedValue.Descendants<DocumentFormat.OpenXml.Drawing.Charts.NumericValue>().ElementAt(11).Text = "";
                //    }
                //    col++;
                //}
                #endregion
                //    }
                //}
                //doc.MainDocumentPart.ChartParts.ElementAt(1).ChartSpace.Save();

                #endregion


                idTablaIndicadores = idTablaIndicadores + 2;
                idTablaGraficos = idTablaGraficos + 2;
            }

            #endregion

            if (listaIndicadores.Count == 0)
            {
                doc.MainDocumentPart.Document.Body.Descendants<DocumentFormat.OpenXml.Wordprocessing.Table>().ElementAt(41).Remove();
                doc.MainDocumentPart.Document.Body.Descendants<DocumentFormat.OpenXml.Wordprocessing.Table>().ElementAt(40).Remove();

            }


            if (listaReq.Count == 1)
                doc.MainDocumentPart.Document.Body.Descendants<DocumentFormat.OpenXml.Wordprocessing.Table>().ElementAt(20).Remove();


            #endregion

            #region cerrardoc
            doc.MainDocumentPart.Document.Save();

            doc.Close();

            keyValues.Add("T_Seguimiento", seguimientoObjetivos);


            int totalAccMejoraCerradas = listaAccMejora.Where(x => x.codigo.Contains(anio)).Where(x => x.estado == 1).Count();
            int totalAccMejoraCerradasNoEficaz = listaAccMejora.Where(x => x.codigo.Contains(anio)).Where(x => x.estado == 2).Count();

            keyValues.Add("T_VAbsAcciones", totalAccMejora.ToString());
            keyValues.Add("T_NCCerradas", totalAccMejoraCerradas.ToString());
            keyValues.Add("T_NCPasadas", totalAccMejoraCerradasNoEficaz.ToString());

            #region cumplimientolegal


            #region evaluacion1
            if (listaReq != null && listaReq.Count > 0)
            {
                keyValues.Add("CL_Anio1", listaReq[0].codigo.ToString());
                keyValues.Add("CL_Ambito1", listaReq[0].nombre_ambito.ToString());
                keyValues.Add("CL_Denominacion1", listaReq[0].denominacion.ToString());
                keyValues.Add("CL_Informe1", listaReq[0].informeevaluacion.Replace(Server.MapPath("~/Requisitos") + "\\" + listaReq[0].id + "\\", ""));
                keyValues.Add("CL_Fecha1", listaReq[0].fecharegistro.ToString().Replace(" 0:00:00", ""));
                keyValues.Add("CL_NumRequisitos1", listaReq[0].numrequisitos.ToString());
                keyValues.Add("CL_Cumple1", listaReq[0].cumple.ToString());
                keyValues.Add("CL_Tramite1", listaReq[0].tramite.ToString());
                keyValues.Add("CL_NC1", listaReq[0].nocumple.ToString());
                keyValues.Add("CL_Obs1", listaReq[0].observacion.ToString());
                keyValues.Add("CL_NP1", listaReq[0].noprocede.ToString());
                keyValues.Add("CL_NV1", listaReq[0].noverificado.ToString());
            }

            #endregion

            #region evaluacion2

            if (listaReq != null && listaReq.Count > 1)
            {
                keyValues.Add("CL_Anio2", listaReq[1].codigo.ToString());
                keyValues.Add("CL_Ambito2", listaReq[1].nombre_ambito.ToString());
                keyValues.Add("CL_Denominacion2", listaReq[1].denominacion.ToString());
                keyValues.Add("CL_Informe2", listaReq[1].informeevaluacion.Replace(Server.MapPath("~/Requisitos") + "\\" + listaReq[1].id + "\\", ""));
                keyValues.Add("CL_Fecha2", listaReq[1].fecharegistro.ToString().Replace(" 0:00:00", ""));
                keyValues.Add("CL_NumRequisitos2", listaReq[1].numrequisitos.ToString());
                keyValues.Add("CL_Cumple2", listaReq[1].cumple.ToString());
                keyValues.Add("CL_Tramite2", listaReq[1].tramite.ToString());
                keyValues.Add("CL_NC2", listaReq[1].nocumple.ToString());
                keyValues.Add("CL_Obs2", listaReq[1].observacion.ToString());
                keyValues.Add("CL_NP2", listaReq[1].noprocede.ToString());
                keyValues.Add("CL_NV2", listaReq[1].noverificado.ToString());
            }

            #endregion

            #endregion

            #region partesinteresadas

            int totalComunicacion = listaCom.Count();
            int totalComunicacionInt = listaCom.Where(x => x.Clasificacion == "Comunicación interna").Count();
            int totalComunicacionExt = listaCom.Where(x => x.Clasificacion == "Comunicación externa").Count();

            keyValues.Add("PI_TotalAccionesComunicacion", totalComunicacion.ToString());
            keyValues.Add("PI_NumAccionesComInternas", totalComunicacionInt.ToString());
            keyValues.Add("PI_NumAccionesComExternas", totalComunicacionExt.ToString());

            #endregion

            SearchAndReplace(destinationFile, keyValues);
            #endregion

            Session["nombreArchivo"] = destinationFile;

            #endregion

            #endregion
        }

        public void PlanificacionPreventiva(string anio)
        {
            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            int idCentral = centroseleccionado.id;

            #region impresion

            string sourceFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaPlantillaWord"] + "PlanificacionPreventiva.xlsx");
            string destinationFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaGeneracionWord"] + "ExportacionPlanificacionPreventiva_" + DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + ".xlsx");

            Session["source"] = sourceFile;
            Session["destino"] = destinationFile;
            // Create a copy of the template file and open the copy 
            System.IO.File.Copy(sourceFile, destinationFile, true);

            using (SpreadsheetDocument document = SpreadsheetDocument.Open(destinationFile, true))
            {
                WorksheetPart worksheetPart = GetWorksheetPartByName(document, "FCIP");

                if (worksheetPart != null)
                {
                    Cell siglascentral = GetCell(worksheetPart.Worksheet, "K", 2);
                    siglascentral.CellValue = new CellValue(centroseleccionado.siglas);
                    siglascentral.DataType = new EnumValue<CellValues>(CellValues.String);

                    Cell fechainforme = GetCell(worksheetPart.Worksheet, "L", 3);
                    fechainforme.CellValue = new CellValue(DateTime.Now.ToShortDateString());
                    fechainforme.DataType = new EnumValue<CellValues>(CellValues.String);

                    #region generación filas de objetivos
                    List<VISTA_Objetivos> listObjetivos = new List<VISTA_Objetivos>();
                    listObjetivos = Datos.ListarObjetivosFechas(centroseleccionado.id, anio);
                    List<VISTA_Despliegue> listAcciones = Datos.ListarAccionesObjetivo(listObjetivos);
                    Row filaObjetivos = GetRow(worksheetPart.Worksheet, 7);
                    uint totalObjetivos = (uint)listObjetivos.Count + (uint)listAcciones.Count;

                    SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

                    if (totalObjetivos > 0)
                    {
                        for (int j = 0; j < totalObjetivos - 1; j++)
                        {
                            CopyToLine(filaObjetivos, 7 + (uint)j, sheetData);
                        }
                    }

                    MergeCells mergecells = worksheetPart.Worksheet.Elements<MergeCells>().First();

                    int i = 0;
                    foreach (VISTA_Objetivos obj in listObjetivos)
                    {
                        Cell referencia = GetCell(worksheetPart.Worksheet, "A", 7 + (uint)i);
                        referencia.CellValue = new CellValue(obj.Codigo);
                        referencia.DataType = new EnumValue<CellValues>(CellValues.String);

                        Cell actuaciones = GetCell(worksheetPart.Worksheet, "B", 7 + (uint)i);
                        actuaciones.CellValue = new CellValue(obj.Nombre);
                        actuaciones.DataType = new EnumValue<CellValues>(CellValues.String);

                        Cell responsable = GetCell(worksheetPart.Worksheet, "D", 7 + (uint)i);
                        responsable.CellValue = new CellValue(obj.Responsable);
                        responsable.DataType = new EnumValue<CellValues>(CellValues.String);

                        Cell coste = GetCell(worksheetPart.Worksheet, "E", 7 + (uint)i);
                        coste.CellValue = new CellValue(obj.Coste);
                        coste.DataType = new EnumValue<CellValues>(CellValues.String);

                        Cell fechaestimada = GetCell(worksheetPart.Worksheet, "F", 7 + (uint)i);
                        fechaestimada.CellValue = new CellValue(obj.FechaEstimada.ToString().Replace(" 0:00:00",""));
                        fechaestimada.DataType = new EnumValue<CellValues>(CellValues.String);

                        Cell fechareal = GetCell(worksheetPart.Worksheet, "G", 7 + (uint)i);
                        fechareal.CellValue = new CellValue(obj.FechaReal.ToString().Replace(" 0:00:00", ""));
                        fechareal.DataType = new EnumValue<CellValues>(CellValues.String);

                        Cell estado = GetCell(worksheetPart.Worksheet, "H", 7 + (uint)i);
                        string estadoStr = obj.estado.ToString();
                        switch(estadoStr)
                        {
                            case "1":
                                estado.CellValue = new CellValue("Alcanza resultados esperados");
                                break;
                            case "2":
                                estado.CellValue = new CellValue("No alcanza resultados esperados");
                                break;
                            case "3":
                                estado.CellValue = new CellValue("Desestimado");
                                break;
                            default:
                                estado.CellValue = new CellValue("En seguimiento");
                                break;
                        }                        
                        estado.DataType = new EnumValue<CellValues>(CellValues.String);

                        //Cell implantacion = GetCell(worksheetPart.Worksheet, "I", 7 + (uint)i);
                        //List<VISTA_Despliegue> listadoAccionesObj = Datos.ListarAccionesObjetivo(listObjetivos[i].id);
                        //decimal gradoCons = 0;
                        //if (listadoAccionesObj != null && listadoAccionesObj.Count>0)
                        //{
                        //    decimal accionesEjecutadas = listadoAccionesObj.Where(x=>x.Estado == 2).Count();
                        //    decimal accionesPlanificadas = listadoAccionesObj.Count();

                        //    gradoCons = Math.Round((100 / accionesPlanificadas) * accionesEjecutadas, 0);
                        //}
                        //else
                        //    gradoCons = 100;
                        //implantacion.CellValue = new CellValue(gradoCons.ToString()+"%");
                        //implantacion.DataType = new EnumValue<CellValues>(CellValues.String);

                        Cell observaciones = GetCell(worksheetPart.Worksheet, "J", 7 + (uint)i);
                        observaciones.CellValue = new CellValue(obj.Comentarios);
                        observaciones.DataType = new EnumValue<CellValues>(CellValues.String);

                        mergecells.Append(new MergeCell() { Reference = new StringValue("B"+ (7 + i) + ":C" + (7 + i)) });
                        mergecells.Append(new MergeCell() { Reference = new StringValue("J" + (7 + i) + ":M" + (7 + i)) });

                        List<VISTA_Despliegue> listaAccionesObjetivo = listAcciones.Where(x => x.idObjetivo == obj.id).ToList();

                        foreach (VISTA_Despliegue acc in listaAccionesObjetivo)
                        {
                            i++;

                            Cell referenciaAcc = GetCell(worksheetPart.Worksheet, "A", 7 + (uint)i);
                            referenciaAcc.CellValue = new CellValue(acc.NumeroAccionDespliegue);
                            referenciaAcc.DataType = new EnumValue<CellValues>(CellValues.String);

                            Cell actuacionesAcc = GetCell(worksheetPart.Worksheet, "B", 7 + (uint)i);
                            actuacionesAcc.CellValue = new CellValue(acc.Nombre);
                            actuacionesAcc.DataType = new EnumValue<CellValues>(CellValues.String);

                            Cell responsableAcc = GetCell(worksheetPart.Worksheet, "D", 7 + (uint)i);
                            responsableAcc.CellValue = new CellValue(acc.NombreResponsable);
                            responsableAcc.DataType = new EnumValue<CellValues>(CellValues.String);

                            Cell costeAcc = GetCell(worksheetPart.Worksheet, "E", 7 + (uint)i);
                            costeAcc.CellValue = new CellValue(acc.Recursos);
                            costeAcc.DataType = new EnumValue<CellValues>(CellValues.String);

                            Cell fechaestimadaAcc = GetCell(worksheetPart.Worksheet, "F", 7 + (uint)i);
                            fechaestimadaAcc.CellValue = new CellValue(acc.FechaEstimada.ToString().Replace(" 0:00:00", ""));
                            fechaestimadaAcc.DataType = new EnumValue<CellValues>(CellValues.String);

                            Cell fecharealAcc = GetCell(worksheetPart.Worksheet, "G", 7 + (uint)i);
                            fecharealAcc.CellValue = new CellValue(acc.FechaReal.ToString().Replace(" 0:00:00", ""));
                            fecharealAcc.DataType = new EnumValue<CellValues>(CellValues.String);

                            Cell estadoAcc = GetCell(worksheetPart.Worksheet, "H", 7 + (uint)i);
                            string estadoStrAcc = acc.Estado.ToString();
                            switch (estadoStrAcc)
                            {
                                case "0":
                                    estadoAcc.CellValue = new CellValue("No ejecutado");
                                    break;
                                case "1":
                                    estadoAcc.CellValue = new CellValue("Pendiente de ejecutar");
                                    break;
                                case "2":
                                    estadoAcc.CellValue = new CellValue("Ejecutado");
                                    break;
                                default:
                                    estadoAcc.CellValue = new CellValue("Pendiente de ejecutar");
                                    break;
                            }
                            estadoAcc.DataType = new EnumValue<CellValues>(CellValues.String);

                            Cell implantacionAcc = GetCell(worksheetPart.Worksheet, "I", 7 + (uint)i);                            
                            implantacionAcc.CellValue = new CellValue(acc.Porcentaje.ToString() + "%");
                            implantacionAcc.DataType = new EnumValue<CellValues>(CellValues.String);

                            Cell observacionesAcc = GetCell(worksheetPart.Worksheet, "J", 7 + (uint)i);
                            observacionesAcc.CellValue = new CellValue(acc.Comentarios);
                            observacionesAcc.DataType = new EnumValue<CellValues>(CellValues.String);

                            mergecells.Append(new MergeCell() { Reference = new StringValue("B" + (7 + i) + ":C" + (7 + i)) });
                            mergecells.Append(new MergeCell() { Reference = new StringValue("J" + (7 + i) + ":M" + (7 + i)) });
                        }
                        i++;
                    }

                    #endregion

                    if (totalObjetivos == 0)
                        totalObjetivos = 1;

                    mergecells.Append(new MergeCell() { Reference = new StringValue("A" + (7 + totalObjetivos) + ":M" + (7 + totalObjetivos)) });
                    mergecells.Append(new MergeCell() { Reference = new StringValue("B" + (8 + totalObjetivos) + ":C" + (8 + totalObjetivos)) });
                    mergecells.Append(new MergeCell() { Reference = new StringValue("I" + (8 + totalObjetivos) + ":M" + (8 + totalObjetivos)) });

                    #region generación filas de acciones de mejora
                    List<VISTA_ListarAccionesMejora> listAccionesMejora = new List<VISTA_ListarAccionesMejora>();
                    listAccionesMejora = Datos.ListarAccionesMejoraFechas(centroseleccionado.id, anio);
                    Row filaAccionesMejora = GetRow(worksheetPart.Worksheet, 9 + (totalObjetivos));
                    uint totalAccionesMejora = (uint)listAccionesMejora.Count;

                    if (listAccionesMejora.Count > 0)
                    {
                        for (int j = 0; j < listAccionesMejora.Count - 1; j++)
                        {
                            CopyToLine(filaAccionesMejora, 9 + totalObjetivos + (uint)j, sheetData);
                        }
                    }

                    for (int j = 0; j < listAccionesMejora.Count; j++)
                    {
                        Cell referenciaacc = GetCell(worksheetPart.Worksheet, "A", 9 + totalObjetivos + (uint)j);
                        referenciaacc.CellValue = new CellValue(listAccionesMejora[j].codigo);
                        referenciaacc.DataType = new EnumValue<CellValues>(CellValues.String);

                        Cell asuntoacc = GetCell(worksheetPart.Worksheet, "B", 9 + totalObjetivos + (uint)j);
                        asuntoacc.CellValue = new CellValue(listAccionesMejora[j].asunto);
                        asuntoacc.DataType = new EnumValue<CellValues>(CellValues.String);

                        Cell responsable = GetCell(worksheetPart.Worksheet, "D", 9 + totalObjetivos + (uint)j);
                        responsable.CellValue = new CellValue(listAccionesMejora[j].Responsable);
                        responsable.DataType = new EnumValue<CellValues>(CellValues.String);

                        Cell fechaaperturaacc = GetCell(worksheetPart.Worksheet, "E", 9 + totalObjetivos + (uint)j);
                        fechaaperturaacc.CellValue = new CellValue(listAccionesMejora[j].fecha_apertura.ToString().Replace(" 0:00:00", ""));
                        fechaaperturaacc.DataType = new EnumValue<CellValues>(CellValues.String);

                        Cell fechacierreacc = GetCell(worksheetPart.Worksheet, "F", 9 + totalObjetivos + (uint)j);
                        fechacierreacc.CellValue = new CellValue(listAccionesMejora[j].fecha_cierre.ToString().Replace(" 0:00:00", ""));
                        fechacierreacc.DataType = new EnumValue<CellValues>(CellValues.String);

                        Cell estadoacc = GetCell(worksheetPart.Worksheet, "H", 9 + totalObjetivos + (uint)j);
                        estadoacc.CellValue = new CellValue(listAccionesMejora[j].EstadoEscrito);
                        estadoacc.DataType = new EnumValue<CellValues>(CellValues.String);

                        Cell observacionesacc = GetCell(worksheetPart.Worksheet, "I", 9 + totalObjetivos + (uint)j);
                        observacionesacc.CellValue = new CellValue(listAccionesMejora[j].descripcion);
                        observacionesacc.DataType = new EnumValue<CellValues>(CellValues.String);

                        mergecells.Append(new MergeCell() { Reference = new StringValue("B" + (9 + totalObjetivos + j) + ":C" + (9 + totalObjetivos + j)) });
                        mergecells.Append(new MergeCell() { Reference = new StringValue("I" + (9 + totalObjetivos + j) + ":M" + (9 + totalObjetivos + j)) });
                    }

                    if (totalAccionesMejora == 0)
                        totalAccionesMejora = 1;

                    mergecells.Append(new MergeCell() { Reference = new StringValue("A" + (9 + totalObjetivos + totalAccionesMejora) + ":E" + (9 + totalObjetivos + totalAccionesMejora)) });
                    mergecells.Append(new MergeCell() { Reference = new StringValue("F" + (9 + totalObjetivos + totalAccionesMejora) + ":M" + (9 + totalObjetivos + totalAccionesMejora)) });
                    #endregion

                    // save worksheet
                    document.WorkbookPart.WorksheetParts.First().Worksheet.Save();

                    document.WorkbookPart.Workbook.Save();

                    Session["nombreArchivo"] = destinationFile;
                }
            }
            #endregion
        }

        #region utiles
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

        private static DocumentFormat.OpenXml.Wordprocessing.RunProperties GetRunPropertyFromTableCell(DocumentFormat.OpenXml.Wordprocessing.TableRow rowCopy, int cellIndex)
        {
            var runProperties = new DocumentFormat.OpenXml.Wordprocessing.RunProperties();
            var fontname = "Calibri";
            var fontSize = "22";
            try
            {
                fontname =
                    rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>()
                       .ElementAt(cellIndex)
                       .GetFirstChild<DocumentFormat.OpenXml.Wordprocessing.Paragraph>()
                       .GetFirstChild<DocumentFormat.OpenXml.Wordprocessing.ParagraphProperties>()
                       .GetFirstChild<ParagraphMarkRunProperties>()
                       .GetFirstChild<RunFonts>()
                       .Ascii;
            }
            catch
            {
                //swallow
            }
            try
            {
                fontSize =
                       rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>()
                          .ElementAt(cellIndex)
                          .GetFirstChild<DocumentFormat.OpenXml.Wordprocessing.Paragraph>()
                          .GetFirstChild<DocumentFormat.OpenXml.Wordprocessing.ParagraphProperties>()
                          .GetFirstChild<ParagraphMarkRunProperties>()
                          .GetFirstChild<DocumentFormat.OpenXml.Wordprocessing.FontSize>()
                          .Val;
            }
            catch
            {
                //swallow
            }
            runProperties.AppendChild(new RunFonts() { Ascii = fontname });
            runProperties.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = fontSize });
            return runProperties;
        }

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
        #endregion

        #region métodos para insertar en excel
        private Row CopyToLine(Row refRow, uint rowIndex, SheetData sheetData)
        {
            uint newRowIndex;
            var newRow = (Row)refRow.CloneNode(true);
            // Loop through all the rows in the worksheet with higher row 
            // index values than the one you just added. For each one,
            // increment the existing row index.
            IEnumerable<Row> rows = sheetData.Descendants<Row>().Where(r => r.RowIndex.Value >= rowIndex);
            foreach (Row row in rows)
            {
                newRowIndex = System.Convert.ToUInt32(row.RowIndex.Value + 1);

                foreach (Cell cell in row.Elements<Cell>())
                {
                    // Update the references for reserved cells.
                    string cellReference = cell.CellReference.Value;
                    cell.CellReference = new StringValue(cellReference.Replace(row.RowIndex.Value.ToString(), newRowIndex.ToString()));
                }
                // Update the row index.
                row.RowIndex = new UInt32Value(newRowIndex);
            }

            sheetData.InsertBefore(newRow, refRow);
            return newRow;
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

        // Given a worksheet, a column name, and a row index, 
        // gets the cell at the specified column and 
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


        // Given a worksheet and a row index, return the row.
        private static Row GetRow(Worksheet worksheet, uint rowIndex)
        {
            return worksheet.GetFirstChild<SheetData>().
              Elements<Row>().Where(r => r.RowIndex == rowIndex).First();
        }
        #endregion

    }
}
