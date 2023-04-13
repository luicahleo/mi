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
    public class EmergenciasController : Controller
    {
        //
        // GET: /Emergencias/

        public ActionResult emergencias()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            int idCentral = centroseleccionado.id;

            ViewData["emergencias"] = Datos.ListarEmergencias(idCentral);

            #region generacion fichero
            if (Session["nombreArchivo"] != null && Session["nombreArchivo"].ToString() != "")
            {
                if (Session["nombreArchivo"].ToString().Contains("xlsx"))
                {
                    Response.ContentType = "application/msexcel";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=ExportacionSimulacros.xlsx");
                    Response.TransmitFile(Session["nombreArchivo"].ToString());
                    Response.End();
                }
                Session["nombreArchivo"] = "";
            }
            #endregion

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult emergencias(FormCollection collection)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            VISTA_ObtenerUsuario user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());

            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            int idCentral = centroseleccionado.id;

            List<VISTA_ListarEmergencias> listaEmergencias = Datos.ListarEmergencias(idCentral);

            #region generacion fichero
            string sourceFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaPlantillaWord"] + "ExportacionEmergencias.xlsx");
            string destinationFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaGeneracionWord"] + "ExportacionEmergencias_" + DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + ".xlsx");

            Session["source"] = sourceFile;
            Session["destino"] = destinationFile;
            // Create a copy of the template file and open the copy 
            System.IO.File.Copy(sourceFile, destinationFile, true);

            #region impresion

            using (SpreadsheetDocument document = SpreadsheetDocument.Open(destinationFile, true))
            {
                SheetData sheetData = document.WorkbookPart.WorksheetParts.ElementAt(0).Worksheet.Elements<SheetData>().First();
                foreach (VISTA_ListarEmergencias eme in listaEmergencias)
                {
                    Row row = new Row();

                    string cod_emergencia = string.Empty;
                    string descripcion = string.Empty;
                    string responsable = string.Empty;
                    string fecha_planificada = string.Empty;
                    string fecha_realizacion = string.Empty;
                    string personal_implicado = string.Empty;
                    string medios_empleados = string.Empty;
                    string escenario_planteado = string.Empty;
                    string objetivo = string.Empty;
                    string informe = string.Empty;
                    string conclusiones = string.Empty;

                    if (eme.codigo == null)
                        cod_emergencia = string.Empty;
                    else
                        cod_emergencia = eme.codigo;

                    if (eme.descripcion == null)
                        descripcion = string.Empty;
                    else
                        descripcion = eme.descripcion;

                    if (eme.nombre == null)
                        responsable = string.Empty;
                    else
                        responsable = eme.nombre;

                    if (eme.fechaplanificada == null)
                        fecha_planificada = string.Empty;
                    else
                        fecha_planificada = eme.fechaplanificada.ToString().Replace(" 0:00:00", "");

                    if (eme.fecharealizacion == null)
                        fecha_realizacion = string.Empty;
                    else
                        fecha_realizacion = eme.fecharealizacion.ToString().Replace(" 0:00:00", "");

                    if (eme.personalimplicado == null)
                        personal_implicado = string.Empty;
                    else
                        personal_implicado = eme.personalimplicado;

                    if (eme.mediosempleados == null)
                        medios_empleados = string.Empty;
                    else
                        medios_empleados = eme.mediosempleados;

                    if (eme.escenarioplanteado == null)
                        escenario_planteado = string.Empty;
                    else
                        escenario_planteado = eme.escenarioplanteado;

                    if (eme.objetivos == null)
                        objetivo = string.Empty;
                    else
                        objetivo = eme.objetivos;

                    if (eme.informe == null)
                        informe = string.Empty;
                    else
                        informe = eme.informe.Replace(Server.MapPath("~/Emergencias") + "\\" + eme.id + "\\Informe\\", "");;

                    if (eme.conclusiones == null)
                        conclusiones = string.Empty;
                    else
                        conclusiones = eme.conclusiones;

                    row.Append(
                        Datos.ConstructCell(cod_emergencia.ToString(), CellValues.String),
                        Datos.ConstructCell(descripcion, CellValues.String),
                        Datos.ConstructCell(responsable, CellValues.String),
                        Datos.ConstructCell(fecha_planificada, CellValues.String),
                        Datos.ConstructCell(fecha_realizacion, CellValues.String),
                        Datos.ConstructCell(personal_implicado, CellValues.String),
                        Datos.ConstructCell(medios_empleados, CellValues.String),
                        Datos.ConstructCell(escenario_planteado, CellValues.String),
                        Datos.ConstructCell(objetivo, CellValues.String),
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
            ViewData["emergencias"] = Datos.ListarEmergencias(idCentral);

            return RedirectToAction("emergencias", "Emergencias");
        }

        public ActionResult detalle_emergencia(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            Session["modulo"] = "5";

            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            int idCentral = centroseleccionado.id;

            emergencias obj = Datos.GetDatosEmergencia(id);
            ViewData["emergencia"] = obj;
            List<VISTA_ListarUsuarios> listarResponsables = Datos.ListarUsuariosCentral(idCentral);
            ViewData["responsables"] = listarResponsables;
            ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 5, id);

            #region generacion fichero
            if (Session["nombreArchivo"] != null && Session["nombreArchivo"].ToString() != "")
            {
                if (Session["nombreArchivo"].ToString().Contains("docx"))
                {
                    Response.ContentType = "application/msword";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=FichaSimulacro.docx");
                    Response.TransmitFile(Session["nombreArchivo"].ToString());
                    Response.End();
                }
                Session["nombreArchivo"] = "";
            }
            #endregion

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult detalle_emergencia(int id, FormCollection collection, HttpPostedFileBase file)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }
            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            int idCentral = centroseleccionado.id;
            List<VISTA_ListarUsuarios> listarResponsables = new List<VISTA_ListarUsuarios>();

            emergencias emer = new emergencias();
            string formulario = collection["hdFormularioEjecutado"];

            if (formulario == "GuardarEmergencia")
            {
                #region guardar
                try
                {
                    if (id != 0)
                    {
                        #region actualizar
                        emergencias actualizar = Datos.GetDatosEmergencia(id);
                        actualizar.descripcion = collection["ctl00$MainContent$txtDescripcion"];
                        if (collection["ctl00$MainContent$txtFPlanficada"] != "")
                            actualizar.fechaplanificada = DateTime.Parse(collection["ctl00$MainContent$txtFPlanficada"]);
                        if (collection["ctl00$MainContent$txtFRealizacion"] != "")
                            actualizar.fecharealizacion = DateTime.Parse(collection["ctl00$MainContent$txtFRealizacion"]);
                        if (collection["ctl00$MainContent$ddlResponsable"] != null && collection["ctl00$MainContent$ddlResponsable"] != "0")
                            actualizar.responsable = int.Parse(collection["ctl00$MainContent$ddlResponsable"]);

                        actualizar.personalimplicado = collection["ctl00$MainContent$txtPersonalImplicado"];
                        actualizar.mediosempleados = collection["ctl00$MainContent$txtMediosEmpleados"];
                        actualizar.escenarioplanteado = collection["ctl00$MainContent$txtEscenarioPlanteado"];
                        actualizar.objetivos = collection["ctl00$MainContent$txtObjetivo"];

                        actualizar.conclusiones = collection["ctl00$MainContent$txtConclusiones"];

                        if (actualizar.descripcion != string.Empty && actualizar.fechaplanificada != null && actualizar.fecharealizacion != null && actualizar.personalimplicado != string.Empty && actualizar.mediosempleados != string.Empty && actualizar.escenarioplanteado != string.Empty && actualizar.objetivos != string.Empty)
                        {
                            Datos.ActualizarEmergencia(actualizar);

                            if ((file != null && file.ContentLength > 0))
                            {
                                var fileName = System.IO.Path.GetFileName(file.FileName);

                                var path = System.IO.Path.Combine(Server.MapPath("~/Emergencias/" + id.ToString() + "/Informe"), fileName);
                                actualizar.informe = path;

                                Datos.UpdateInformeEnlace(actualizar);

                                if (Directory.Exists(Server.MapPath("~/Emergencias/" + id.ToString() + "/Informe")))
                                {
                                    file.SaveAs(path);
                                }
                                else
                                {
                                    Directory.CreateDirectory(Server.MapPath("~/Emergencias/" + id.ToString() + "/Informe"));
                                    file.SaveAs(path);
                                }
                            }


                            Session["EdicionEmergenciaMensaje"] = "Información actualizada correctamente";
                            emer = Datos.GetDatosEmergencia(id);
                            ViewData["emergencia"] = emer;
                            listarResponsables = Datos.ListarUsuariosCentral(idCentral);
                            ViewData["responsables"] = listarResponsables;
                            ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 5, id);
                        }
                        else
                        {
                            actualizar.descripcion = collection["ctl00$MainContent$txtDescripcion"];
                            if (collection["ctl00$MainContent$txtFPlanficada"] != "")
                                actualizar.fechaplanificada = DateTime.Parse(collection["ctl00$MainContent$txtFPlanficada"]);
                            if (collection["ctl00$MainContent$txtFRealizacion"] != "")
                                actualizar.fecharealizacion = DateTime.Parse(collection["ctl00$MainContent$txtFRealizacion"]);
                            if (collection["ctl00$MainContent$ddlResponsable"] != null && collection["ctl00$MainContent$ddlResponsable"] != "0")
                                actualizar.responsable = int.Parse(collection["ctl00$MainContent$ddlResponsable"]);

                            actualizar.personalimplicado = collection["ctl00$MainContent$txtPersonalImplicado"];
                            actualizar.mediosempleados = collection["ctl00$MainContent$txtMediosEmpleados"];
                            actualizar.escenarioplanteado = collection["ctl00$MainContent$txtEscenarioPlanteado"];
                            actualizar.objetivos = collection["ctl00$MainContent$txtObjetivo"];

                            actualizar.conclusiones = collection["ctl00$MainContent$txtConclusiones"];
                            Session["EdicionEmergenciaError"] = "Los campos marcados con (*) son obligatorios.";
                            emer = actualizar;
                            ViewData["emergencia"] = emer;
                            listarResponsables = Datos.ListarUsuariosCentral(idCentral);
                            ViewData["responsables"] = listarResponsables;
                            ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 5, id);
                        }
                        return View();
                        #endregion
                    }
                    else
                    {
                        #region insertar
                        emergencias insertar = new emergencias();
                        insertar.descripcion = collection["ctl00$MainContent$txtDescripcion"];
                        insertar.anio = int.Parse(collection["ctl00$MainContent$ddlAnio"]);
                        if (collection["ctl00$MainContent$txtFPlanficada"] != "")
                            insertar.fechaplanificada = DateTime.Parse(collection["ctl00$MainContent$txtFPlanficada"]);
                        if (collection["ctl00$MainContent$txtFRealizacion"] != "")
                            insertar.fecharealizacion = DateTime.Parse(collection["ctl00$MainContent$txtFRealizacion"]);
                        if (collection["ctl00$MainContent$ddlResponsable"] != null && collection["ctl00$MainContent$ddlResponsable"] != "0")
                            insertar.responsable = int.Parse(collection["ctl00$MainContent$ddlResponsable"]);
                        if (Session["CentralElegida"] != null)
                        {
                                insertar.idcentral = centroseleccionado.id;
                        }
                        insertar.personalimplicado = collection["ctl00$MainContent$txtPersonalImplicado"];
                        insertar.mediosempleados = collection["ctl00$MainContent$txtMediosEmpleados"];
                        insertar.escenarioplanteado = collection["ctl00$MainContent$txtEscenarioPlanteado"];
                        insertar.objetivos = collection["ctl00$MainContent$txtObjetivo"];
                        insertar.conclusiones = collection["ctl00$MainContent$txtConclusiones"];

                        if (insertar.descripcion != string.Empty && insertar.fechaplanificada != null && insertar.fecharealizacion != null && insertar.personalimplicado != string.Empty && insertar.mediosempleados != string.Empty && insertar.escenarioplanteado != string.Empty && insertar.objetivos != string.Empty)
                        {
                            int idForm = Datos.ActualizarEmergencia(insertar);

                            if ((file != null && file.ContentLength > 0))
                            {
                                var fileName = System.IO.Path.GetFileName(file.FileName);

                                var path = System.IO.Path.Combine(Server.MapPath("~/Emergencias/" + idForm.ToString() + "/Informe"), fileName);
                                insertar.informe = path;

                                Datos.UpdateInformeEnlace(insertar);

                                if (Directory.Exists(Server.MapPath("~/Emergencias/" + idForm.ToString() + "/Informe")))
                                {
                                    file.SaveAs(path);
                                }
                                else
                                {
                                    Directory.CreateDirectory(Server.MapPath("~/Emergencias/" + idForm.ToString() + "/Informe"));
                                    file.SaveAs(path);
                                }
                            }

                            Session["EdicionEmergenciaMensaje"] = "Información actualizada correctamente";

                            emergencias form = Datos.GetDatosEmergencia(idForm);
                            ViewData["emergencia"] = form;
                            listarResponsables = Datos.ListarUsuariosCentral(idCentral);
                            ViewData["responsables"] = listarResponsables;
                            ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 5, id);
                            return Redirect(Url.RouteUrl(new { controller = "emergencias", action = "detalle_emergencia", id = idForm }));
                        }
                        else
                        {
                            Session["EdicionEmergenciaError"] = "Los campos marcados con (*) son obligatorios.";
                            insertar.descripcion = collection["ctl00$MainContent$txtDescripcion"];
                            if (collection["ctl00$MainContent$txtFPlanficada"] != "")
                                insertar.fechaplanificada = DateTime.Parse(collection["ctl00$MainContent$txtFPlanficada"]);
                            if (collection["ctl00$MainContent$txtFRealizacion"] != "")
                                insertar.fecharealizacion = DateTime.Parse(collection["ctl00$MainContent$txtFRealizacion"]);
                            if (collection["ctl00$MainContent$ddlResponsable"] != null && collection["ctl00$MainContent$ddlResponsable"] != "0")
                                insertar.responsable = int.Parse(collection["ctl00$MainContent$ddlResponsable"]);

                            insertar.personalimplicado = collection["ctl00$MainContent$txtPersonalImplicado"];
                            insertar.mediosempleados = collection["ctl00$MainContent$txtMediosEmpleados"];
                            insertar.escenarioplanteado = collection["ctl00$MainContent$txtEscenarioPlanteado"];
                            insertar.objetivos = collection["ctl00$MainContent$txtObjetivo"];

                            insertar.conclusiones = collection["ctl00$MainContent$txtConclusiones"];
                            emer = insertar;
                            ViewData["emergencia"] = emer;
                            listarResponsables = Datos.ListarUsuariosCentral(idCentral);
                            ViewData["responsables"] = listarResponsables;
                            ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 5, id);
                            return View();
                        }
                        #endregion
                    }

                }
                catch (Exception ex)
                {
                    emer = Datos.GetDatosEmergencia(id);
                    ViewData["emergencia"] = emer;
                    listarResponsables = Datos.ListarUsuariosCentral(idCentral);
                    ViewData["responsables"] = listarResponsables;
                    ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 5, id);
                    return View();
                }
                #endregion
            }

            if (formulario == "btnImprimir")
            {
                #region imprimir
                string sourceFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaPlantillaWord"] + "FichaEmergencia.docx");
                string destinationFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaGeneracionWord"] + "FichaEmergencia_" + DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + ".docx");

                Session["source"] = sourceFile;
                Session["destino"] = destinationFile;
                // Create a copy of the template file and open the copy 
                System.IO.File.Copy(sourceFile, destinationFile, true);

                #region impresion
                VISTA_ListarEmergencias actualizar = Datos.GetDatosEmergenciaFicha(id);


                    // create key value pair, key represents words to be replace and 
                    //values represent values in document in place of keys.
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add("T_Codigo", actualizar.codigo.Replace("\r\n", "<w:br/>"));
                    keyValues.Add("T_Descripcion", actualizar.descripcion.Replace("\r\n", "<w:br/>"));
                    if (actualizar.fechaplanificada != null)
                        keyValues.Add("T_FechaPlanificada", actualizar.fechaplanificada.ToString().Replace(" 0:00:00", "").Replace("\r\n", "<w:br/>"));
                    else
                        keyValues.Add("T_FechaPlanificada", "");
                    if (actualizar.fecharealizacion != null)
                        keyValues.Add("T_FechaReal", actualizar.fecharealizacion.ToString().Replace(" 0:00:00", "").Replace("\r\n", "<w:br/>"));
                    else
                        keyValues.Add("T_FechaReal", "");
                    if (actualizar.nombre != null)
                        keyValues.Add("T_Responsable", actualizar.nombre.Replace("\r\n", "<w:br/>"));
                    else
                        keyValues.Add("T_Responsable", "");

                    if (actualizar.personalimplicado != null)
                        keyValues.Add("T_Personal", actualizar.personalimplicado.Replace("\r\n", "<w:br/>"));
                    else
                        keyValues.Add("T_Personal", "");

                    if (actualizar.mediosempleados != null)
                        keyValues.Add("T_Medios", actualizar.mediosempleados.Replace("\r\n", "<w:br/>"));
                    else
                        keyValues.Add("T_Medios", "");

                    if (actualizar.escenarioplanteado != null)
                        keyValues.Add("T_Escenario", actualizar.escenarioplanteado.Replace("\r\n", "<w:br/>"));
                    else
                        keyValues.Add("T_Escenario", "");

                    if (actualizar.objetivos != null)
                        keyValues.Add("T_Objetivo", actualizar.objetivos.Replace("\r\n", "<w:br/>"));
                    else
                        keyValues.Add("T_Objetivo", "");

                    if (actualizar.informe != null)
                        keyValues.Add("T_Informe", actualizar.informe.Replace(Server.MapPath("~/Emergencias") + "\\" + actualizar.id + "\\Informe\\", "").Replace("\r\n", "<w:br/>"));
                    else
                        keyValues.Add("T_Informe", "");

                    if (actualizar.conclusiones != null)
                        keyValues.Add("T_Conclusiones", actualizar.conclusiones.Replace("\r\n", "<w:br/>"));
                    else
                        keyValues.Add("T_Conclusiones", "");

                    SearchAndReplace(destinationFile, keyValues);

                    Session["nombreArchivo"] = destinationFile;
                #endregion

                emer = Datos.GetDatosEmergencia(id);
                ViewData["emergencia"] = emer;
                listarResponsables = Datos.ListarUsuariosCentral(idCentral);
                ViewData["responsables"] = listarResponsables;
                return Redirect(Url.RouteUrl(new { controller = "emergencias", action = "detalle_emergencia", id = id }));
                #endregion
            }
            else
            {
                emer = Datos.GetDatosEmergencia(id);
                ViewData["emergencia"] = emer;
                listarResponsables = Datos.ListarUsuariosCentral(idCentral);
                ViewData["responsables"] = listarResponsables;
                ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 5, id);
                return View();
            }
        }

        public FileResult ObtenerInforme(int id)
        {
            try
            {
                emergencias IF = Datos.GetDatosEmergencia(id);
                byte[] fileBytes = System.IO.File.ReadAllBytes(IF.informe);
                string fileName = IF.informe.Replace(Server.MapPath("~/Emergencias") + "\\" + IF.id + "\\Informe\\", "");
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ActionResult Eliminar_Emergencia(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            Datos.EliminarEmergencia(id);
            Session["EditarEmergenciasResultado"] = "Eliminado registro";
            return RedirectToAction("emergencias", "emergencias");
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
