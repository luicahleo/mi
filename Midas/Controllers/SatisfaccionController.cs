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
    public class SatisfaccionController : Controller
    {
        //
        // GET: /Satisfaccion/

        public ActionResult satisfaccion()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            int idCentral = centroseleccionado.id;

            ViewData["satisfaccion"] = Datos.ListarSatisfaccion(idCentral);

            #region generacion fichero
            if (Session["nombreArchivo"] != null && Session["nombreArchivo"].ToString() != "")
            {
                if (Session["nombreArchivo"].ToString().Contains("xlsx"))
                {
                    Response.ContentType = "application/msexcel";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=ExportacionSatisfaccion.xlsx");
                    Response.TransmitFile(Session["nombreArchivo"].ToString());
                    Response.End();
                }
                Session["nombreArchivo"] = "";
            }
            #endregion

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult satisfaccion(FormCollection collection)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            VISTA_ObtenerUsuario user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());

            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            int idCentral = centroseleccionado.id;

            List<VISTA_ListarSatisfaccion> listaSatisfaccion = Datos.ListarSatisfaccion(idCentral);

            #region generacion fichero
            string sourceFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaPlantillaWord"] + "ExportacionSatisfaccion.xlsx");
            string destinationFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaGeneracionWord"] + "ExportacionSatisfaccion_" + DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + ".xlsx");

            Session["source"] = sourceFile;
            Session["destino"] = destinationFile;
            // Create a copy of the template file and open the copy 
            System.IO.File.Copy(sourceFile, destinationFile, true);

            #region impresion

            using (SpreadsheetDocument document = SpreadsheetDocument.Open(destinationFile, true))
            {
                SheetData sheetData = document.WorkbookPart.WorksheetParts.ElementAt(0).Worksheet.Elements<SheetData>().First();
                foreach (VISTA_ListarSatisfaccion sat in listaSatisfaccion)
                {
                    Row row = new Row();

                    string cod_satisfaccion = string.Empty;
                    string stakeholder = string.Empty;
                    string responsable = string.Empty;
                    string fecha_realizacion = string.Empty;
                    string informe = string.Empty;
                    string conclusiones = string.Empty;

                    if (sat.codigo == null)
                        cod_satisfaccion = string.Empty;
                    else
                        cod_satisfaccion = sat.codigo;

                    if (sat.denominacion == null)
                        stakeholder = string.Empty;
                    else
                        stakeholder = sat.denominacion;

                    if (sat.nombre == null)
                        responsable = string.Empty;
                    else
                        responsable = sat.nombre;

                    if (sat.fecharealizacion == null)
                        fecha_realizacion = string.Empty;
                    else
                        fecha_realizacion = sat.fecharealizacion.ToString().Replace(" 0:00:00", "");

                    if (sat.informe == null)
                        informe = string.Empty;
                    else
                        informe = sat.informe.Replace(Server.MapPath("~/Satisfaccion") + "\\" + sat.id + "\\Informe\\", ""); ;

                    if (sat.conclusiones == null)
                        conclusiones = string.Empty;
                    else
                        conclusiones = sat.conclusiones;

                    row.Append(
                        Datos.ConstructCell(cod_satisfaccion.ToString(), CellValues.String),
                        Datos.ConstructCell(stakeholder, CellValues.String),
                        Datos.ConstructCell(responsable, CellValues.String),
                        Datos.ConstructCell(fecha_realizacion, CellValues.String),
                        Datos.ConstructCell(informe, CellValues.String),
                        Datos.ConstructCell(conclusiones, CellValues.String));

                    sheetData.AppendChild(row);
                }

                // save worksheet
                document.WorkbookPart.WorksheetParts.First().Worksheet.Save();

                document.WorkbookPart.Workbook.Save();

                Session["nombreArchivo"] = destinationFile;
            }
            #endregion
            #endregion
            ViewData["satisfaccion"] = Datos.ListarSatisfaccion(idCentral);

            return RedirectToAction("satisfaccion", "Satisfaccion");
        }

        public FileResult ObtenerInforme(int id)
        {
            try
            {
                satisfaccion IF = Datos.GetDatosSatisfaccion(id);
                byte[] fileBytes = System.IO.File.ReadAllBytes(IF.informe);
                string fileName = IF.informe.Replace(Server.MapPath("~/Satisfaccion") + "\\" + IF.id + "\\Informe\\", "");
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ActionResult detalle_satisfaccion(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            Session["modulo"] = "6";

            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            int idCentral = centroseleccionado.id;

            satisfaccion sat = Datos.GetDatosSatisfaccion(id);
            ViewData["detallesatisfaccion"] = sat;
            List<VISTA_ListarUsuarios> listarResponsables = Datos.ListarUsuariosCentral(idCentral);
            ViewData["responsables"] = listarResponsables;
            List<VISTA_StakeholdersN3> listarStakeholders = Datos.ListarStakeholdersN3();
            ViewData["stakeholders"] = listarStakeholders;
            ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 6, id);
            #region generacion fichero
            if (Session["nombreArchivo"] != null && Session["nombreArchivo"].ToString() != "")
            {
                if (Session["nombreArchivo"].ToString().Contains("docx"))
                {
                    Response.ContentType = "application/msword";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=FichaSatisfaccion.docx");
                    Response.TransmitFile(Session["nombreArchivo"].ToString());
                    Response.End();
                }
                Session["nombreArchivo"] = "";
            }
            #endregion

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult detalle_satisfaccion(int id, FormCollection collection, HttpPostedFileBase file)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }
            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            int idCentral = centroseleccionado.id;
            List<VISTA_ListarUsuarios> listarResponsables = new List<VISTA_ListarUsuarios>();
            List<VISTA_StakeholdersN3> listarStakeholders = new List<VISTA_StakeholdersN3>();

            satisfaccion sat = new satisfaccion();
            string formulario = collection["hdFormularioEjecutado"];

            if (formulario == "GuardarSatisfaccion")
            {
                #region guardar
                try
                {
                    if (id != 0)
                    {
                        #region actualizar
                        satisfaccion actualizar = Datos.GetDatosSatisfaccion(id);
                        if (collection["ctl00$MainContent$txtFRealizacion"] != "")
                            actualizar.fecharealizacion = DateTime.Parse(collection["ctl00$MainContent$txtFRealizacion"]);
                        if (collection["ctl00$MainContent$ddlResponsable"] != null && collection["ctl00$MainContent$ddlResponsable"] != "0")
                            actualizar.responsable = int.Parse(collection["ctl00$MainContent$ddlResponsable"]);
                        if (collection["ctl00$MainContent$ddlStakeholder"] != null && collection["ctl00$MainContent$ddlStakeholder"] != "0")
                            actualizar.stakeholder = int.Parse(collection["ctl00$MainContent$ddlStakeholder"]);
                        actualizar.conclusiones = collection["ctl00$MainContent$txtConclusiones"];
                        actualizar.personasinv = collection["ctl00$MainContent$txtPersonasInvolucradas"];

                        if (actualizar.fecharealizacion != null && actualizar.conclusiones != string.Empty)
                        {
                            Datos.ActualizarSatisfaccion(actualizar);

                            if ((file != null && file.ContentLength > 0))
                            {
                                var fileName = System.IO.Path.GetFileName(file.FileName);

                                var path = System.IO.Path.Combine(Server.MapPath("~/Satisfaccion/" + id.ToString() + "/Informe"), fileName);
                                actualizar.informe = path;

                                Datos.UpdateInformeSatisfaccion(actualizar);

                                if (Directory.Exists(Server.MapPath("~/Satisfaccion/" + id.ToString() + "/Informe")))
                                {
                                    file.SaveAs(path);
                                }
                                else
                                {
                                    Directory.CreateDirectory(Server.MapPath("~/Satisfaccion/" + id.ToString() + "/Informe"));
                                    file.SaveAs(path);
                                }
                            }


                            Session["EdicionSatisfaccionMensaje"] = "Información actualizada correctamente";
                            sat = Datos.GetDatosSatisfaccion(id);
                            ViewData["detallesatisfaccion"] = sat;
                            listarResponsables = Datos.ListarUsuariosCentral(idCentral);
                            ViewData["responsables"] = listarResponsables;
                            listarStakeholders = Datos.ListarStakeholdersN3();
                            ViewData["stakeholders"] = listarStakeholders;
                            ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 6, id);
                        }
                        else
                        {
                            Session["EdicionSatisfaccionError"] = "Los campos marcados con (*) son obligatorios.";
                            sat = actualizar;
                            ViewData["detallesatisfaccion"] = sat;
                            listarResponsables = Datos.ListarUsuariosCentral(idCentral);
                            ViewData["responsables"] = listarResponsables;
                            listarStakeholders = Datos.ListarStakeholdersN3();
                            ViewData["stakeholders"] = listarStakeholders;
                            ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 6, id);
                        }
                        return View();
                        #endregion
                    }
                    else
                    {
                        #region insertar
                        satisfaccion insertar = new satisfaccion();
                        insertar.anio = int.Parse(collection["ctl00$MainContent$ddlAnio"]);
                        if (collection["ctl00$MainContent$txtFRealizacion"] != "")
                            insertar.fecharealizacion = DateTime.Parse(collection["ctl00$MainContent$txtFRealizacion"]);
                        if (collection["ctl00$MainContent$ddlResponsable"] != null && collection["ctl00$MainContent$ddlResponsable"] != "0")
                            insertar.responsable = int.Parse(collection["ctl00$MainContent$ddlResponsable"]);
                        if (collection["ctl00$MainContent$ddlStakeholder"] != null && collection["ctl00$MainContent$ddlStakeholder"] != "0")
                            insertar.stakeholder = int.Parse(collection["ctl00$MainContent$ddlStakeholder"]);
                        if (Session["CentralElegida"] != null)
                        {
                            insertar.idcentral = centroseleccionado.id;
                        }
                        insertar.conclusiones = collection["ctl00$MainContent$txtConclusiones"];
                        insertar.personasinv = collection["ctl00$MainContent$txtPersonasInvolucradas"];

                        if (insertar.fecharealizacion != null && insertar.conclusiones != string.Empty)
                        {
                            int idForm = Datos.ActualizarSatisfaccion(insertar);

                            if ((file != null && file.ContentLength > 0))
                            {
                                var fileName = System.IO.Path.GetFileName(file.FileName);

                                var path = System.IO.Path.Combine(Server.MapPath("~/Satisfaccion/" + idForm.ToString() + "/Informe"), fileName);
                                insertar.informe = path;

                                Datos.UpdateInformeSatisfaccion(insertar);

                                if (Directory.Exists(Server.MapPath("~/Satisfaccion/" + idForm.ToString() + "/Informe")))
                                {
                                    file.SaveAs(path);
                                }
                                else
                                {
                                    Directory.CreateDirectory(Server.MapPath("~/Satisfaccion/" + idForm.ToString() + "/Informe"));
                                    file.SaveAs(path);
                                }
                            }

                            Session["EdicionSatisfaccionMensaje"] = "Información actualizada correctamente";

                            satisfaccion form = Datos.GetDatosSatisfaccion(idForm);
                            ViewData["detallesatisfaccion"] = form;
                            listarResponsables = Datos.ListarUsuariosCentral(idCentral);
                            ViewData["responsables"] = listarResponsables;
                            listarStakeholders = Datos.ListarStakeholdersN3();
                            ViewData["stakeholders"] = listarStakeholders;
                            ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 6, id);
                            return Redirect(Url.RouteUrl(new { controller = "satisfaccion", action = "detalle_satisfaccion", id = idForm }));
                        }
                        else
                        {
                            Session["EdicionSatisfaccionError"] = "Los campos marcados con (*) son obligatorios.";

                            satisfaccion form = insertar;
                            ViewData["detallesatisfaccion"] = form;
                            listarResponsables = Datos.ListarUsuariosCentral(idCentral);
                            ViewData["responsables"] = listarResponsables;
                            listarStakeholders = Datos.ListarStakeholdersN3();
                            ViewData["stakeholders"] = listarStakeholders;
                            ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 6, id);

                            return View();
                        }
                        #endregion
                    }

                }
                catch (Exception ex)
                {
                    sat = Datos.GetDatosSatisfaccion(id);
                    ViewData["detalle_satisfaccion"] = sat;
                    listarResponsables = Datos.ListarUsuariosCentral(idCentral);
                    ViewData["responsables"] = listarResponsables;
                    listarStakeholders = Datos.ListarStakeholdersN3();
                    ViewData["stakeholders"] = listarStakeholders;
                    ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 6, id);
                    return View();
                }
                #endregion
            }

            if (formulario == "btnImprimir")
            {
                #region imprimir
                string sourceFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaPlantillaWord"] + "FichaSatisfaccion.docx");
                string destinationFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaGeneracionWord"] + "FichaSatisfaccion_" + DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + ".docx");

                Session["source"] = sourceFile;
                Session["destino"] = destinationFile;
                // Create a copy of the template file and open the copy 
                System.IO.File.Copy(sourceFile, destinationFile, true);

                #region impresion
                VISTA_ListarSatisfaccion actualizar = Datos.GetDatosSatisfaccionFicha(id);


                // create key value pair, key represents words to be replace and 
                //values represent values in document in place of keys.
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add("T_Codigo", actualizar.codigo.Replace("\r\n", "<w:br/>"));
                keyValues.Add("T_Stakeholder", actualizar.denominacion.Replace("\r\n", "<w:br/>"));
                if (actualizar.fecharealizacion != null)
                    keyValues.Add("T_FechaReal", actualizar.fecharealizacion.ToString().Replace(" 0:00:00", "").Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_FechaReal", "");
                if (actualizar.nombre != null)
                    keyValues.Add("T_Responsable", actualizar.nombre.Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_Responsable", "");

                

                if (actualizar.informe != null)
                    keyValues.Add("T_Informe", actualizar.informe.Replace(Server.MapPath("~/Satisfaccion") + "\\" + actualizar.id + "\\Informe\\", "").Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_Informe", "");

                if (actualizar.conclusiones != null)
                    keyValues.Add("T_Conclusiones", actualizar.conclusiones.Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_Conclusiones", "");

                if (actualizar != null)
                    keyValues.Add("T_PersonasInvolucradas", actualizar.personasinv.Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_PersonasInvolucradas", "");

                List<VISTA_ListarAccionesMejora> listaAcciones = Datos.ListarAccionesMejora(idCentral, 6, id);
                string accionesmejora = string.Empty;
                foreach (VISTA_ListarAccionesMejora acc in listaAcciones)
                {
                    accionesmejora = accionesmejora + acc.codigo + "/" + acc.asunto + "\r\n";
                }
                keyValues.Add("T_AccionesMejora", accionesmejora.Replace("\r\n", "<w:br/>"));

                SearchAndReplace(destinationFile, keyValues);

                WordprocessingDocument doc = WordprocessingDocument.Open(destinationFile, true);

                var runPropertiesDespliegue8 = new DocumentFormat.OpenXml.Wordprocessing.RunProperties();
                runPropertiesDespliegue8.AppendChild(new RunFonts() { Ascii = "Calibri" });
                runPropertiesDespliegue8.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "22" });


                DocumentFormat.OpenXml.Wordprocessing.Table tabladespliegue = doc.MainDocumentPart.Document.Body.Descendants<DocumentFormat.OpenXml.Wordprocessing.Table>().ElementAt(1);

                var filadespliegue = tabladespliegue.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>().ElementAt(9);

                DocumentFormat.OpenXml.Wordprocessing.TableCell celdaaccionesmejora = filadespliegue.Elements<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(0);
                var parrafo = celdaaccionesmejora.Elements<DocumentFormat.OpenXml.Wordprocessing.Paragraph>().ElementAt(0);
                var runprocesos = parrafo.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Run());
                runprocesos.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesDespliegue8);
                //ACCIONES DE MEJORA
                List<VISTA_ListarAccionesMejora> listadoAccionesMejora = Datos.ListarAccionesMejora(idCentral, 6, actualizar.id);
                foreach (VISTA_ListarAccionesMejora acc in listadoAccionesMejora)
                {
                    runprocesos.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Text("-" + acc.asunto));
                    runprocesos.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Break());
                }

                doc.MainDocumentPart.Document.Save();

                doc.Close();

                Session["nombreArchivo"] = destinationFile;
                #endregion

                sat = Datos.GetDatosSatisfaccion(id);
                ViewData["detalle_satisfaccion"] = sat;
                listarResponsables = Datos.ListarUsuariosCentral(idCentral);
                ViewData["responsables"] = listarResponsables;
                listarStakeholders = Datos.ListarStakeholdersN3();
                ViewData["stakeholders"] = listarStakeholders;
                ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 6, id);
                return Redirect(Url.RouteUrl(new { controller = "satisfaccion", action = "detalle_satisfaccion", id = id }));
                #endregion
            }
            else
            {
                sat = Datos.GetDatosSatisfaccion(id);
                ViewData["detalle_satisfaccion"] = sat;
                listarResponsables = Datos.ListarUsuariosCentral(idCentral);
                ViewData["responsables"] = listarResponsables;
                listarStakeholders = Datos.ListarStakeholdersN3();
                ViewData["stakeholders"] = listarStakeholders;
                ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 6, id);
                return View();
            }
        }

        public ActionResult Eliminar_Satisfaccion(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            Datos.EliminarSatisfaccion(id);
            Session["EditarSatisfaccionesResultado"] = "Registro eliminado";
            return RedirectToAction("Satisfaccion", "Satisfaccion");
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
        #endregion

    }
}
