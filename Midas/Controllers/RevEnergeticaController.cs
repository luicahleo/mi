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
    public class RevEnergeticaController : Controller
    {
        //
        // GET: /RevEnergetica/

        public ActionResult revisiones()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            int idCentral = centroseleccionado.id;

            ViewData["revisiones"] = Datos.ListarRevisiones(idCentral);

            #region generacion fichero
            if (Session["nombreArchivo"] != null && Session["nombreArchivo"].ToString() != "")
            {
                if (Session["nombreArchivo"].ToString().Contains("xlsx"))
                {
                    Response.ContentType = "application/msexcel";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=ExportacionRevisiones.xlsx");
                    Response.TransmitFile(Session["nombreArchivo"].ToString());
                    Response.End();
                }
                Session["nombreArchivo"] = "";
            }
            #endregion

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult revisiones(FormCollection collection)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            VISTA_ObtenerUsuario user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());

            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            int idCentral = centroseleccionado.id;

            List<VISTA_ListarRevisionesEnergeticas> listaRevisiones = Datos.ListarRevisiones(idCentral);

            #region generacion fichero
            string sourceFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaPlantillaWord"] + "ExportacionRevisiones.xlsx");
            string destinationFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaGeneracionWord"] + "ExportacionRevisiones_" + DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + ".xlsx");

            Session["source"] = sourceFile;
            Session["destino"] = destinationFile;
            // Create a copy of the template file and open the copy 
            System.IO.File.Copy(sourceFile, destinationFile, true);

            #region impresion

            using (SpreadsheetDocument document = SpreadsheetDocument.Open(destinationFile, true))
            {
                SheetData sheetData = document.WorkbookPart.WorksheetParts.ElementAt(0).Worksheet.Elements<SheetData>().First();
                foreach (VISTA_ListarRevisionesEnergeticas rev in listaRevisiones)
                {
                    Row row = new Row();

                    string cod_revision = string.Empty;
                    string responsable = string.Empty;
                    string fecha_planificacion = string.Empty;
                    string planificacion = string.Empty;
                    string fecha_revision = string.Empty;
                    string revision = string.Empty;
                    string conclusiones = string.Empty;
                    string accionesmejora = string.Empty;

                    if (rev.codigo == null)
                        cod_revision = string.Empty;
                    else
                        cod_revision = rev.codigo;

                    if (rev.nombre == null)
                        responsable = string.Empty;
                    else
                        responsable = rev.nombre;

                    if (rev.fechaplanificacion == null)
                        fecha_planificacion = string.Empty;
                    else
                        fecha_planificacion = rev.fechaplanificacion.ToString().Replace(" 0:00:00", "");

                    if (rev.fecharevision == null)
                        fecha_revision = string.Empty;
                    else
                        fecha_revision = rev.fecharevision.ToString().Replace(" 0:00:00", "");

                    if (rev.planificacionenergetica == null)
                        planificacion = string.Empty;
                    else
                        planificacion = rev.planificacionenergetica.Replace(Server.MapPath("~/RevEnergetica") + "\\" + rev.id + "\\Planificacion\\", "");

                    if (rev.revisionenergetica == null)
                        revision = string.Empty;
                    else
                        revision = rev.revisionenergetica.Replace(Server.MapPath("~/RevEnergetica") + "\\" + rev.id + "\\Revision\\", ""); 

                    if (rev.conclusiones == null)
                        conclusiones = string.Empty;
                    else
                        conclusiones = rev.conclusiones;

                    List<VISTA_ListarAccionesMejora> listaacciones = Datos.ListarAccionesMejora(idCentral, 7, rev.id);

                    foreach (VISTA_ListarAccionesMejora acc in listaacciones)
                    {
                        accionesmejora = accionesmejora + acc.codigo + "/" + acc.asunto + "\r\n";
                    }

                    row.Append(
                        Datos.ConstructCell(cod_revision.ToString(), CellValues.String),
                        Datos.ConstructCell(fecha_planificacion, CellValues.String),
                        Datos.ConstructCell(planificacion, CellValues.String),
                        Datos.ConstructCell(fecha_revision, CellValues.String),
                        Datos.ConstructCell(revision, CellValues.String),
                        Datos.ConstructCell(responsable, CellValues.String),
                        Datos.ConstructCell(conclusiones, CellValues.String),
                        Datos.ConstructCell(accionesmejora, CellValues.String));

                    sheetData.AppendChild(row);
                }

                // save worksheet
                document.WorkbookPart.WorksheetParts.First().Worksheet.Save();

                document.WorkbookPart.Workbook.Save();

                Session["nombreArchivo"] = destinationFile;
            }
            #endregion
            #endregion
            ViewData["revisiones"] = Datos.ListarRevisiones(idCentral);

            return RedirectToAction("revisiones", "RevEnergetica");
        }

        public ActionResult detalle_revision(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            Session["modulo"] = "7";

            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            int idCentral = centroseleccionado.id;

            revision_energetica rev = Datos.GetDatosRevision(id);
            ViewData["revision"] = rev;
            List<VISTA_ListarUsuarios> listarResponsables = Datos.ListarUsuariosCentral(idCentral);
            ViewData["responsables"] = listarResponsables;
            ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 7, id);
            #region generacion fichero
            if (Session["nombreArchivo"] != null && Session["nombreArchivo"].ToString() != "")
            {
                if (Session["nombreArchivo"].ToString().Contains("docx"))
                {
                    Response.ContentType = "application/msword";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=FichaRevision.docx");
                    Response.TransmitFile(Session["nombreArchivo"].ToString());
                    Response.End();
                }
                Session["nombreArchivo"] = "";
            }
            #endregion

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult detalle_revision(int id, FormCollection collection, HttpPostedFileBase[] file)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }
            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            int idCentral = centroseleccionado.id;
            List<VISTA_ListarUsuarios> listarResponsables = new List<VISTA_ListarUsuarios>();

            revision_energetica rev = new revision_energetica();
            string formulario = collection["hdFormularioEjecutado"];

            if (formulario == "GuardarRevision")
            {
                #region guardar
                try
                {
                    if (id != 0)
                    {
                        #region actualizar
                        revision_energetica actualizar = Datos.GetDatosRevision(id);
                        if (collection["ctl00$MainContent$txtFPlanificacion"] != "")
                            actualizar.fechaplanificacion = DateTime.Parse(collection["ctl00$MainContent$txtFPlanificacion"]);
                        if (collection["ctl00$MainContent$txtFRevision"] != "")
                            actualizar.fecharevision = DateTime.Parse(collection["ctl00$MainContent$txtFRevision"]);
                        
                        if (collection["ctl00$MainContent$ddlResponsable"] != null && collection["ctl00$MainContent$ddlResponsable"] != "0")
                            actualizar.responsable = int.Parse(collection["ctl00$MainContent$ddlResponsable"]);
                        actualizar.conclusiones = collection["ctl00$MainContent$txtConclusiones"];
                        actualizar.personasinv = collection["ctl00$MainContent$txtPersonasInvolucradas"];

                        if (actualizar.fechaplanificacion != null && actualizar.fecharevision != null && actualizar.conclusiones != string.Empty)
                        {
                            Datos.ActualizarRevision(actualizar);

                            if ((file[0] != null && file[0].ContentLength > 0))
                            {
                                var fileName = System.IO.Path.GetFileName(file[0].FileName);

                                var path = System.IO.Path.Combine(Server.MapPath("~/RevEnergetica/" + id.ToString() + "/Planificacion"), fileName);
                                actualizar.planificacionenergetica = path;

                                Datos.UpdatePlanificacionRev(actualizar);

                                if (Directory.Exists(Server.MapPath("~/RevEnergetica/" + id.ToString() + "/Planificacion")))
                                {
                                    file[0].SaveAs(path);
                                }
                                else
                                {
                                    Directory.CreateDirectory(Server.MapPath("~/RevEnergetica/" + id.ToString() + "/Planificacion"));
                                    file[0].SaveAs(path);
                                }
                            }

                            if ((file[1] != null && file[1].ContentLength > 0))
                            {
                                var fileName = System.IO.Path.GetFileName(file[1].FileName);

                                var path = System.IO.Path.Combine(Server.MapPath("~/RevEnergetica/" + id.ToString() + "/Revision"), fileName);
                                actualizar.revisionenergetica = path;

                                Datos.UpdateRevisionRev(actualizar);

                                if (Directory.Exists(Server.MapPath("~/RevEnergetica/" + id.ToString() + "/Revision")))
                                {
                                    file[1].SaveAs(path);
                                }
                                else
                                {
                                    Directory.CreateDirectory(Server.MapPath("~/RevEnergetica/" + id.ToString() + "/Revision"));
                                    file[1].SaveAs(path);
                                }
                            }


                            Session["EdicionRevisionMensaje"] = "Información actualizada correctamente";
                            rev = Datos.GetDatosRevision(id);
                            ViewData["revision"] = rev;
                            listarResponsables = Datos.ListarUsuariosCentral(idCentral);
                            ViewData["responsables"] = listarResponsables;
                            ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 7, id);
                        }
                        else
                        {
                            Session["EdicionRevisionError"] = "Los campos marcados con (*) son obligatorios.";
                            rev = actualizar;
                            ViewData["revision"] = rev;
                            listarResponsables = Datos.ListarUsuariosCentral(idCentral);
                            ViewData["responsables"] = listarResponsables;
                            ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 7, id);
                        }
                        return View();
                        #endregion
                    }
                    else
                    {
                        #region insertar
                        revision_energetica insertar = new revision_energetica();
                        insertar.anio = int.Parse(collection["ctl00$MainContent$ddlAnio"]);
                        if (collection["ctl00$MainContent$txtFPlanificacion"] != "")
                            insertar.fechaplanificacion = DateTime.Parse(collection["ctl00$MainContent$txtFPlanificacion"]);
                        if (collection["ctl00$MainContent$txtFRevision"] != "")
                            insertar.fecharevision = DateTime.Parse(collection["ctl00$MainContent$txtFRevision"]);
                        if (collection["ctl00$MainContent$ddlResponsable"] != null && collection["ctl00$MainContent$ddlResponsable"] != "0")
                            insertar.responsable = int.Parse(collection["ctl00$MainContent$ddlResponsable"]);

                        if (Session["CentralElegida"] != null)
                        {
                            insertar.idcentral = centroseleccionado.id;
                        }
                        insertar.conclusiones = collection["ctl00$MainContent$txtConclusiones"];
                        insertar.personasinv = collection["ctl00$MainContent$txtPersonasInvolucradas"];

                        if (insertar.fechaplanificacion != null && insertar.fecharevision != null)
                        {
                            int idForm = Datos.ActualizarRevision(insertar);

                            if ((file[0] != null && file[0].ContentLength > 0))
                            {
                                var fileName = System.IO.Path.GetFileName(file[0].FileName);

                                var path = System.IO.Path.Combine(Server.MapPath("~/RevEnergetica/" + idForm.ToString() + "/Planificacion"), fileName);
                                insertar.planificacionenergetica = path;

                                Datos.UpdatePlanificacionRev(insertar);

                                if (Directory.Exists(Server.MapPath("~/RevEnergetica/" + idForm.ToString() + "/Planificacion")))
                                {
                                    file[0].SaveAs(path);
                                }
                                else
                                {
                                    Directory.CreateDirectory(Server.MapPath("~/RevEnergetica/" + idForm.ToString() + "/Planificacion"));
                                    file[0].SaveAs(path);
                                }
                            }

                            if ((file[1] != null && file[1].ContentLength > 0))
                            {
                                var fileName = System.IO.Path.GetFileName(file[1].FileName);

                                var path = System.IO.Path.Combine(Server.MapPath("~/RevEnergetica/" + id.ToString() + "/Revision"), fileName);
                                insertar.revisionenergetica = path;

                                Datos.UpdateRevisionRev(insertar);

                                if (Directory.Exists(Server.MapPath("~/RevEnergetica/" + id.ToString() + "/Revision")))
                                {
                                    file[1].SaveAs(path);
                                }
                                else
                                {
                                    Directory.CreateDirectory(Server.MapPath("~/RevEnergetica/" + id.ToString() + "/Revision"));
                                    file[1].SaveAs(path);
                                }
                            }

                            Session["EdicionRevisionMensaje"] = "Información actualizada correctamente";

                            revision_energetica form = Datos.GetDatosRevision(idForm);
                            ViewData["revision"] = form;
                            listarResponsables = Datos.ListarUsuariosCentral(idCentral);
                            ViewData["responsables"] = listarResponsables;
                            ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 7, id);
                            return Redirect(Url.RouteUrl(new { controller = "RevEnergetica", action = "detalle_revision", id = idForm }));
                        }
                        else
                        {
                            revision_energetica form = insertar;
                            ViewData["revision"] = form;
                            listarResponsables = Datos.ListarUsuariosCentral(idCentral);
                            ViewData["responsables"] = listarResponsables;
                            ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 7, id);
                            return View();
                        }
                        #endregion
                    }

                }
                catch (Exception ex)
                {
                    rev = Datos.GetDatosRevision(id);
                    ViewData["revision"] = rev;
                    listarResponsables = Datos.ListarUsuariosCentral(idCentral);
                    ViewData["responsables"] = listarResponsables;
                    ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 7, id);
                    return View();
                }
                #endregion
            }

            if (formulario == "btnImprimir")
            {
                #region imprimir
                string sourceFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaPlantillaWord"] + "FichaRevision.docx");
                string destinationFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaGeneracionWord"] + "FichaRevision_" + DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + ".docx");

                Session["source"] = sourceFile;
                Session["destino"] = destinationFile;
                // Create a copy of the template file and open the copy 
                System.IO.File.Copy(sourceFile, destinationFile, true);

                #region impresion
                VISTA_ListarRevisionesEnergeticas actualizar = Datos.GetDatosRevisionFicha(id);


                // create key value pair, key represents words to be replace and 
                //values represent values in document in place of keys.
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add("T_Codigo", actualizar.codigo.Replace("\r\n", "<w:br/>"));
                if (actualizar.fechaplanificacion != null)
                    keyValues.Add("T_FechaPlanificacion", actualizar.fechaplanificacion.ToString().Replace(" 0:00:00", "").Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_FechaPlanificacion", "");

                if (actualizar.fecharevision != null)
                    keyValues.Add("T_FechaRevision", actualizar.fecharevision.ToString().Replace(" 0:00:00", "").Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_FechaRevision", "");

                if (actualizar.nombre != null)
                    keyValues.Add("T_Responsable", actualizar.nombre.Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_Responsable", "");

                if (actualizar.planificacionenergetica != null)
                    keyValues.Add("T_Planificacion", actualizar.planificacionenergetica.Replace(Server.MapPath("~/RevEnergetica") + "\\" + actualizar.id + "\\Planificacion\\", "").Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_Planificacion", "");

                if (actualizar.revisionenergetica != null)
                    keyValues.Add("T_Revision", actualizar.revisionenergetica.Replace(Server.MapPath("~/RevEnergetica") + "\\" + actualizar.id + "\\Revision\\", "").Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_Revision", "");

                if (actualizar.conclusiones != null)
                    keyValues.Add("T_Conclusiones", actualizar.conclusiones.Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_Conclusiones", "");

                if (actualizar.personasinv != null)
                    keyValues.Add("T_PersonasInv", actualizar.personasinv.Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_PersonasInv", "");

                SearchAndReplace(destinationFile, keyValues);

                Session["nombreArchivo"] = destinationFile;
                #endregion

                rev = Datos.GetDatosRevision(id);
                ViewData["revision"] = rev;
                listarResponsables = Datos.ListarUsuariosCentral(idCentral);
                ViewData["responsables"] = listarResponsables;
                ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 7, id);
                return Redirect(Url.RouteUrl(new { controller = "RevEnergetica", action = "detalle_revision", id = id }));
                #endregion
            }
            else
            {
                rev = Datos.GetDatosRevision(id);
                ViewData["detalle_satisfaccion"] = rev;
                listarResponsables = Datos.ListarUsuariosCentral(idCentral);
                ViewData["responsables"] = listarResponsables;
                ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 7, id);
                return View();
            }
        }

        public FileResult ObtenerPlanificacion(int id)
        {
            try
            {
                revision_energetica IF = Datos.GetDatosRevision(id);
                byte[] fileBytes = System.IO.File.ReadAllBytes(IF.planificacionenergetica);
                string fileName = IF.planificacionenergetica.Replace(Server.MapPath("~/RevEnergetica") + "\\" + IF.id + "\\Planificacion\\", "");
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

        public FileResult ObtenerRevision(int id)
        {
            try
            {
                revision_energetica IF = Datos.GetDatosRevision(id);
                byte[] fileBytes = System.IO.File.ReadAllBytes(IF.revisionenergetica);
                string fileName = IF.revisionenergetica.Replace(Server.MapPath("~/RevEnergetica") + "\\" + IF.id + "\\Informe\\", "");
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

        public ActionResult Eliminar_Revision(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            Datos.EliminarRevision(id);
            Session["EditarRevisionesResultado"] = "Eliminado registro";
            return RedirectToAction("revisiones", "RevEnergetica");
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
