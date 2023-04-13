using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MIDAS.Models;
using System.Configuration;
using DocumentFormat.OpenXml.Packaging;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Spreadsheet;

namespace MIDAS.Controllers
{
    public class AuditoriasController : Controller
    {
        //
        // GET: /Auditorias/

        public ActionResult auditorias()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            int idCentral = centroseleccionado.id;

            ViewData["auditorias"] = Datos.ListarAuditorias(idCentral);

            #region generacion fichero
            if (Session["nombreArchivo"] != null && Session["nombreArchivo"].ToString() != "")
            {
                if (Session["nombreArchivo"].ToString().Contains("xlsx"))
                {
                    Response.ContentType = "application/msexcel";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=ExportacionAuditorias.xlsx");
                    Response.TransmitFile(Session["nombreArchivo"].ToString());
                    Response.End();
                }
                Session["nombreArchivo"] = "";
            }
            #endregion

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult auditorias(FormCollection collection)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            VISTA_ObtenerUsuario user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());

            string sourceFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaPlantillaWord"] + "ExportacionAuditorias.xlsx");
            string destinationFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaGeneracionWord"] + "ExportacionAuditorias_" + DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + ".xlsx");

            Session["source"] = sourceFile;
            Session["destino"] = destinationFile;
            // Create a copy of the template file and open the copy 
            System.IO.File.Copy(sourceFile, destinationFile, true);

            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            int idCentral = centroseleccionado.id;
            centros actualizar = Datos.ObtenerCentroPorID(idCentral);

            #region impresionlistado

            using (SpreadsheetDocument document = SpreadsheetDocument.Open(destinationFile, true))
            {

                List<VISTA_Auditorias> form = Datos.ListarAuditorias(idCentral);
                SheetData sheetData = document.WorkbookPart.WorksheetParts.ElementAt(0).Worksheet.Elements<SheetData>().First();
                foreach (VISTA_Auditorias fo in form)
                {
                    Row row = new Row();

                    string central = string.Empty;
                    string fechainicio = string.Empty;
                    string fechafin = string.Empty;
                    string tipo = string.Empty;
                    string referenciales = string.Empty;
                    string planaudit = string.Empty;
                    string equipoaudit = string.Empty;
                    string observadores = string.Empty;
                    string infoaudit = string.Empty;
                    string comentario = string.Empty;
                    string accmejora = string.Empty;

                    if (fo.nombre == null)
                        central = string.Empty;
                    else
                        central = fo.nombre;

                    if (fo.fechainicio == null)
                        fechainicio = string.Empty;
                    else
                        fechainicio = fo.fechainicio.ToString().Replace(" 0:00:00", "");

                    if (fo.fechafin == null)
                        fechafin = string.Empty;
                    else
                        fechafin = fo.fechafin.ToString().Replace(" 0:00:00", "");

                    if (fo.tipo == null)
                        tipo = string.Empty;
                    else
                        tipo = fo.tipo;

                    List<VISTA_AuditoriaReferenciales> listaReferenciales = Datos.ListarReferencialesAsignados(fo.id);
                    foreach (VISTA_AuditoriaReferenciales refe in listaReferenciales)
                    {
                        referenciales = referenciales + "-" + refe.nombre + "\r\n";
                    }

                    if (fo.programa == null)
                        planaudit = string.Empty;
                    else
                        planaudit = fo.programa.Replace(Server.MapPath("~/Auditorias") + "\\" + fo.id + "\\Programa\\", "");

                    List<VISTA_Auditores> listaAuditores = Datos.ListarAuditores(fo.id);
                    foreach (VISTA_Auditores au in listaAuditores)
                    {
                        equipoaudit = equipoaudit + "-" + au.nombre + "\r\n";
                    }

                    List<VISTA_AuditoriaObservadores> listaObservadores = Datos.ListarObservadores(fo.id);
                    foreach (VISTA_AuditoriaObservadores obs in listaObservadores)
                    {
                        observadores = observadores + "-" + obs.nombre + "\r\n";
                    }

                    if (fo.informe == null)
                        infoaudit = string.Empty;
                    else
                        infoaudit = fo.informe.Replace(Server.MapPath("~/Auditorias") + "\\" + fo.id + "\\Informe\\", "");

                    if (fo.comentario == null)
                        comentario = string.Empty;
                    else
                        comentario = fo.comentario;

                    List<VISTA_ListarAccionesMejora> listaacciones = Datos.ListarAccionesMejora(idCentral, 2, fo.id);

                    foreach (VISTA_ListarAccionesMejora acc in listaacciones)
                    {
                        accmejora = accmejora + acc.codigo + "/" + acc.asunto + "\r\n";
                    }

                    row.Append(
                        Datos.ConstructCell(central, CellValues.String),
                        Datos.ConstructCell(fechainicio, CellValues.String),
                        Datos.ConstructCell(fechafin, CellValues.String),
                        Datos.ConstructCell(tipo, CellValues.String),
                        Datos.ConstructCell(referenciales, CellValues.String),
                        Datos.ConstructCell(planaudit, CellValues.String),
                        Datos.ConstructCell(equipoaudit, CellValues.String),
                        Datos.ConstructCell(observadores, CellValues.String),
                        Datos.ConstructCell(infoaudit, CellValues.String),
                        Datos.ConstructCell(comentario, CellValues.String),
                        Datos.ConstructCell(accmejora, CellValues.String));

                    sheetData.AppendChild(row);
                }

                // save worksheet
                document.WorkbookPart.WorksheetParts.First().Worksheet.Save();

                document.WorkbookPart.Workbook.Save();
            }  

            #endregion

            Session["nombreArchivo"] = destinationFile;

            ViewData["auditorias"] = Datos.ListarAuditorias();

            return RedirectToAction("auditorias", "Auditorias");

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

        public ActionResult detalle_auditoria(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            Session["modulo"] = "2";

            auditorias aud = Datos.GetDatosAuditoria(id);
            ViewData["auditoria"] = aud;
            if (Session["CentralElegida"] != null)
            {
                int centralElegida = int.Parse(Session["CentralElegida"].ToString());

                centros central = Datos.ObtenerCentroPorID(centralElegida);

                List<centros> listaCentrales = new List<centros>();

                listaCentrales.Add(central);

                ViewData["centros"] = listaCentrales;
                ViewData["accionesmejora"] = Datos.ListarAccionesMejora(centralElegida, 2, id);
            }
            List<referenciales> refe = Datos.ListarReferencialesAsignar(id);
            ViewData["referenciales"] = refe;
            List<VISTA_AuditoriaReferenciales> refeasig = Datos.ListarReferencialesAsignados(id);
            ViewData["referencialesasignados"] = refeasig;
            

            List<usuarios> usus = Datos.ListarUsuariosAsignar(id);
            ViewData["usuariosasignar"] = usus;
            List<VISTA_AuditoriaObservadores> ususasig = Datos.ListarUsuariosAsignados(id);
            ViewData["usuariosasignados"] = ususasig;

            List<auditores> auds = Datos.ListarAuditoresAsignar(id);
            ViewData["auditoresasignar"] = auds;
            List<VISTA_Auditores> ListarAuditores = Datos.ListarAuditores(id);
            ViewData["auditores"] = ListarAuditores;

            #region generacion fichero
            if (Session["nombreArchivo"] != null && Session["nombreArchivo"].ToString() != "")
            {
                if (Session["nombreArchivo"].ToString().Contains("docx"))
                {
                    Response.ContentType = "application/msword";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=FichaAuditoria.docx");
                    Response.TransmitFile(Session["nombreArchivo"].ToString());
                    Response.End();
                }
                Session["nombreArchivo"] = "";
            }
            #endregion

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult detalle_auditoria(int id, FormCollection collection, HttpPostedFileBase[] file)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }
            auditorias audi = new auditorias();
            string formulario = collection["hdFormularioEjecutado"];

            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            int idCentral = centroseleccionado.id;  

            if (formulario == "GuardarAuditoria")
            {
                #region guardar auditoria
                try
                {
                    if (id != 0)
                    {
                        #region actualizar
                        auditorias actualizar = Datos.GetDatosAuditoria(id);
                        //actualizar.programa = collection["ctl00$MainContent$txtPrograma"];
                        if (collection["ctl00$MainContent$txtFechaInicio"] != "")
                            actualizar.fechainicio = DateTime.Parse(collection["ctl00$MainContent$txtFechaInicio"]);
                        if (collection["ctl00$MainContent$txtFechaFin"] != "")
                            actualizar.fechafin = DateTime.Parse(collection["ctl00$MainContent$txtFechaFin"]);
                        if (collection["ctl00$MainContent$ddlTipo"] != null)
                            actualizar.tipo = int.Parse(collection["ctl00$MainContent$ddlTipo"]);
                        //actualizar.informe = collection["ctl00$MainContent$txtInforme"];
                        actualizar.comentario = collection["ctl00$MainContent$txtComentario"];

                        Datos.ActualizarAuditoria(actualizar);

                        if ((file[0] != null && file[0].ContentLength > 0))
                        {
                            var fileName = System.IO.Path.GetFileName(file[0].FileName);

                            var path = System.IO.Path.Combine(Server.MapPath("~/Auditorias/" + id.ToString() + "/Programa"), fileName);
                            actualizar.programa = path;

                            Datos.UpdateProgramaAuditoriaEnlace(actualizar);

                            if (Directory.Exists(Server.MapPath("~/Auditorias/" + id.ToString() + "/Programa")))
                            {
                                file[0].SaveAs(path);
                            }
                            else
                            {
                                Directory.CreateDirectory(Server.MapPath("~/Auditorias/" + id.ToString() + "/Programa"));
                                file[0].SaveAs(path);
                            }
                        }

                        if ((file[1] != null && file[1].ContentLength > 0))
                        {
                            var fileName = System.IO.Path.GetFileName(file[1].FileName);

                            var path = System.IO.Path.Combine(Server.MapPath("~/Auditorias/" + id.ToString() + "/Informe"), fileName);
                            actualizar.informe = path;

                            Datos.UpdateInformeAuditoriaEnlace(actualizar);

                            if (Directory.Exists(Server.MapPath("~/Auditorias/" + id.ToString() + "/Informe")))
                            {
                                file[1].SaveAs(path);
                            }
                            else
                            {
                                Directory.CreateDirectory(Server.MapPath("~/Auditorias/" + id.ToString() + "/Informe"));
                                file[1].SaveAs(path);
                            }
                        }

                        Session["EdicionAuditoriaMensaje"] = "Información actualizada correctamente";
                        audi = Datos.GetDatosAuditoria(id);
                        ViewData["auditoria"] = audi;
                        if (Session["CentralElegida"] != null)
                        {
                            int centralElegida = int.Parse(Session["CentralElegida"].ToString());

                            centros central = Datos.ObtenerCentroPorID(centralElegida);

                            List<centros> listaCentrales = new List<centros>();

                            listaCentrales.Add(central);

                            ViewData["centros"] = listaCentrales;
                        }
                        List<referenciales> refe = Datos.ListarReferencialesAsignar(id);
                        ViewData["referenciales"] = refe;
                        List<VISTA_AuditoriaReferenciales> refeasig = Datos.ListarReferencialesAsignados(id);
                        ViewData["referencialesasignados"] = refeasig;

                        ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 2, id);

                        List<usuarios> usus = Datos.ListarUsuariosAsignar(id);
                        ViewData["usuariosasignar"] = usus;
                        List<VISTA_AuditoriaObservadores> ususasig = Datos.ListarUsuariosAsignados(id);
                        ViewData["usuariosasignados"] = ususasig;

                        List<auditores> auds = Datos.ListarAuditoresAsignar(id);
                        ViewData["auditoresasignar"] = auds;
                        List<VISTA_Auditores> ListarAuditores = Datos.ListarAuditores(id);
                        ViewData["auditores"] = ListarAuditores;

                        return View();
                        #endregion
                    }
                    else
                    {
                        #region insertar
                        auditorias insertar = new auditorias();
                        //insertar.programa = collection["ctl00$MainContent$txtPrograma"];
                        if (collection["ctl00$MainContent$txtFechaInicio"] != "")
                            insertar.fechainicio = DateTime.Parse(collection["ctl00$MainContent$txtFechaInicio"]);
                        if (collection["ctl00$MainContent$txtFechaFin"] != "")
                            insertar.fechafin = DateTime.Parse(collection["ctl00$MainContent$txtFechaFin"]);
                        if (collection["ctl00$MainContent$ddlCentral"] != null && collection["ctl00$MainContent$ddlCentral"] != "0")
                            insertar.idCentral = int.Parse(collection["ctl00$MainContent$ddlCentral"]);
                        if (collection["ctl00$MainContent$ddlTipo"] != null)
                            insertar.tipo = int.Parse(collection["ctl00$MainContent$ddlTipo"]);
                        //insertar.informe = collection["ctl00$MainContent$txtInforme"];
                        insertar.comentario = collection["ctl00$MainContent$txtComentario"];


                        int idForm = Datos.ActualizarAuditoria(insertar);


                        if ((file[0] != null && file[0].ContentLength > 0))
                        {
                            var fileName = System.IO.Path.GetFileName(file[0].FileName);

                            var path = System.IO.Path.Combine(Server.MapPath("~/Auditorias/" + idForm.ToString() + "/Programa"), fileName);
                            insertar.programa = path;

                            Datos.UpdateProgramaAuditoriaEnlace(insertar);

                            if (Directory.Exists(Server.MapPath("~/Auditorias/" + idForm.ToString() + "/Programa")))
                            {
                                file[0].SaveAs(path);
                            }
                            else
                            {
                                Directory.CreateDirectory(Server.MapPath("~/Auditorias/" + idForm.ToString() + "/Programa"));
                                file[0].SaveAs(path);
                            }
                        }

                        if ((file[1] != null && file[1].ContentLength > 0))
                        {
                            var fileName = System.IO.Path.GetFileName(file[1].FileName);

                            var path = System.IO.Path.Combine(Server.MapPath("~/Auditorias/" + idForm.ToString() + "/Informe"), fileName);
                            insertar.informe = path;

                            Datos.UpdateInformeAuditoriaEnlace(insertar);

                            if (Directory.Exists(Server.MapPath("~/Auditorias/" + idForm.ToString() + "/Informe")))
                            {
                                file[1].SaveAs(path);
                            }
                            else
                            {
                                Directory.CreateDirectory(Server.MapPath("~/Auditorias/" + idForm.ToString() + "/Informe"));
                                file[1].SaveAs(path);
                            }
                        }

                        Session["EdicionAuditoriaMensaje"] = "Información actualizada correctamente";

                        audi = Datos.GetDatosAuditoria(idForm);
                        ViewData["auditoria"] = audi;
                        if (Session["CentralElegida"] != null)
                        {
                            int centralElegida = int.Parse(Session["CentralElegida"].ToString());

                            centros central = Datos.ObtenerCentroPorID(centralElegida);

                            List<centros> listaCentrales = new List<centros>();

                            listaCentrales.Add(central);

                            ViewData["centros"] = listaCentrales;
                        }
                        List<referenciales> refe = Datos.ListarReferencialesAsignar(id);
                        ViewData["referenciales"] = refe;
                        List<VISTA_AuditoriaReferenciales> refeasig = Datos.ListarReferencialesAsignados(id);
                        ViewData["referencialesasignados"] = refeasig;
                        ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 2, id);
                        List<usuarios> usus = Datos.ListarUsuariosAsignar(id);
                        ViewData["usuariosasignar"] = usus;
                        List<VISTA_AuditoriaObservadores> ususasig = Datos.ListarUsuariosAsignados(id);
                        ViewData["usuariosasignados"] = ususasig;

                        List<auditores> auds = Datos.ListarAuditoresAsignar(id);
                        ViewData["auditoresasignar"] = auds;
                        List<VISTA_Auditores> ListarAuditores = Datos.ListarAuditores(id);
                        ViewData["auditores"] = ListarAuditores;

                        return Redirect(Url.RouteUrl(new { controller = "Auditorias", action = "detalle_auditoria", id = idForm }));
                        #endregion
                    }
                }
                catch (Exception ex)
                {
                    audi = Datos.GetDatosAuditoria(id);
                    ViewData["auditoria"] = audi;
                    if (Session["CentralElegida"] != null)
                    {
                        int centralElegida = int.Parse(Session["CentralElegida"].ToString());

                        centros central = Datos.ObtenerCentroPorID(centralElegida);

                        List<centros> listaCentrales = new List<centros>();

                        listaCentrales.Add(central);

                        ViewData["centros"] = listaCentrales;
                    }
                    List<referenciales> refe = Datos.ListarReferencialesAsignar(id);
                    ViewData["referenciales"] = refe;
                    List<VISTA_AuditoriaReferenciales> refeasig = Datos.ListarReferencialesAsignados(id);
                    ViewData["referencialesasignados"] = refeasig;
                    ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 2, id);
                    List<usuarios> usus = Datos.ListarUsuariosAsignar(id);
                    ViewData["usuariosasignar"] = usus;
                    List<VISTA_AuditoriaObservadores> ususasig = Datos.ListarUsuariosAsignados(id);
                    ViewData["usuariosasignados"] = ususasig;

                    List<auditores> auds = Datos.ListarAuditoresAsignar(id);
                    ViewData["auditoresasignar"] = auds;
                    List<VISTA_Auditores> ListarAuditores = Datos.ListarAuditores(id);
                    ViewData["auditores"] = ListarAuditores;

                    return View();
                }
                #endregion
            }

            if (formulario == "btnAddReferencial")
            {
                #region añadir referencial
                if (collection["ctl00$MainContent$ddlReferenciales"] != null)
                {
                    int idReferencial = int.Parse(collection["ctl00$MainContent$ddlReferenciales"]);
                    Datos.AsociarAuditoriaReferencial(id, idReferencial);
                }


                audi = Datos.GetDatosAuditoria(id);
                ViewData["auditoria"] = audi;
                if (Session["CentralElegida"] != null)
                {
                    int centralElegida = int.Parse(Session["CentralElegida"].ToString());

                    centros central = Datos.ObtenerCentroPorID(centralElegida);

                    List<centros> listaCentrales = new List<centros>();

                    listaCentrales.Add(central);

                    ViewData["centros"] = listaCentrales;
                }
                List<referenciales> refe = Datos.ListarReferencialesAsignar(id);
                ViewData["referenciales"] = refe;
                List<VISTA_AuditoriaReferenciales> refeasig = Datos.ListarReferencialesAsignados(id);
                ViewData["referencialesasignados"] = refeasig;
                ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 2, id);
                List<usuarios> usus = Datos.ListarUsuariosAsignar(id);
                ViewData["usuariosasignar"] = usus;
                List<VISTA_AuditoriaObservadores> ususasig = Datos.ListarUsuariosAsignados(id);
                ViewData["usuariosasignados"] = ususasig;

                List<auditores> auds = Datos.ListarAuditoresAsignar(id);
                ViewData["auditoresasignar"] = auds;
                List<VISTA_Auditores> ListarAuditores = Datos.ListarAuditores(id);
                ViewData["auditores"] = ListarAuditores;

                Session["EdicionAuditoriaMensaje"] = "Referencial asignado correctamente";
                return View();
                #endregion
            }

            if (formulario == "btnAddObservador")
            {
                #region añadir observador
                if (collection["ctl00$MainContent$ddlObservadores"] != null)
                {
                    int idObservador = int.Parse(collection["ctl00$MainContent$ddlObservadores"]);
                    Datos.AsociarAuditoriaObservador(id, idObservador);
                }


                audi = Datos.GetDatosAuditoria(id);
                ViewData["auditoria"] = audi;
                if (Session["CentralElegida"] != null)
                {
                    int centralElegida = int.Parse(Session["CentralElegida"].ToString());

                    centros central = Datos.ObtenerCentroPorID(centralElegida);

                    List<centros> listaCentrales = new List<centros>();

                    listaCentrales.Add(central);

                    ViewData["centros"] = listaCentrales;
                }
                List<referenciales> refe = Datos.ListarReferencialesAsignar(id);
                ViewData["referenciales"] = refe;
                List<VISTA_AuditoriaReferenciales> refeasig = Datos.ListarReferencialesAsignados(id);
                ViewData["referencialesasignados"] = refeasig;
                ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 2, id);
                List<usuarios> usus = Datos.ListarUsuariosAsignar(id);
                ViewData["usuariosasignar"] = usus;
                List<VISTA_AuditoriaObservadores> ususasig = Datos.ListarUsuariosAsignados(id);
                ViewData["usuariosasignados"] = ususasig;

                List<auditores> auds = Datos.ListarAuditoresAsignar(id);
                ViewData["auditoresasignar"] = auds;
                List<VISTA_Auditores> ListarAuditores = Datos.ListarAuditores(id);
                ViewData["auditores"] = ListarAuditores;

                Session["EdicionAuditoriaMensaje"] = "Observador asignado correctamente";
                return View();
                #endregion
            }

            if (formulario == "btnAddAuditor")
            {
                #region añadir auditor
                if (collection["ctl00$MainContent$ddlAuditor"] != null)
                {
                    int auditor = int.Parse(collection["ctl00$MainContent$ddlAuditor"].ToString());
                    Datos.AgregarAuditoriaAuditor(id, auditor);
                }

                audi = Datos.GetDatosAuditoria(id);
                ViewData["auditoria"] = audi;
                if (Session["CentralElegida"] != null)
                {
                    int centralElegida = int.Parse(Session["CentralElegida"].ToString());

                    centros central = Datos.ObtenerCentroPorID(centralElegida);

                    List<centros> listaCentrales = new List<centros>();

                    listaCentrales.Add(central);

                    ViewData["centros"] = listaCentrales;
                }
                List<referenciales> refe = Datos.ListarReferencialesAsignar(id);
                ViewData["referenciales"] = refe;
                List<VISTA_AuditoriaReferenciales> refeasig = Datos.ListarReferencialesAsignados(id);
                ViewData["referencialesasignados"] = refeasig;
                ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 2, id);
                List<usuarios> usus = Datos.ListarUsuariosAsignar(id);
                ViewData["usuariosasignar"] = usus;
                List<VISTA_AuditoriaObservadores> ususasig = Datos.ListarUsuariosAsignados(id);
                ViewData["usuariosasignados"] = ususasig;

                List<auditores> auds = Datos.ListarAuditoresAsignar(id);
                ViewData["auditoresasignar"] = auds;
                List<VISTA_Auditores> ListarAuditores = Datos.ListarAuditores(id);
                ViewData["auditores"] = ListarAuditores;

                Session["EdicionAuditoriaMensaje"] = "Auditor asignado correctamente";
                return View();
                #endregion
            }

            if (formulario == "btnImprimir")
            {
                #region imprimir
                string sourceFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaPlantillaWord"] + "FichaAuditoria.docx");
                string destinationFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaGeneracionWord"] + "FichaAuditoria_" + DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + ".docx");

                Session["source"] = sourceFile;
                Session["destino"] = destinationFile;
                // Create a copy of the template file and open the copy 
                System.IO.File.Copy(sourceFile, destinationFile, true);


                centros actualizar = Datos.ObtenerCentroPorID(idCentral);
                VISTA_Auditorias form = new VISTA_Auditorias();
                form = Datos.ListarAuditoria(id);

                #region impresionlistado
                if (actualizar != null)
                {
                    // create key value pair, key represents words to be replace and 
                    //values represent values in document in place of keys.
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add("T_Codigo", actualizar.siglas.Replace("\r\n", "<w:br/>"));
                    keyValues.Add("T_Central", form.nombre.Replace("\r\n", "<w:br/>"));
                    keyValues.Add("T_TipoAuditoria", form.tipo.Replace("\r\n", "<w:br/>"));
                    if (form.informe != null)
                        keyValues.Add("T_Informe", form.informe.Replace(Server.MapPath("~/Auditorias") + "\\" + form.id + "\\Informe\\", "").Replace("\r\n", "<w:br/>"));
                    else
                        keyValues.Add("T_Informe", "");
                    if (form.programa != null)
                        keyValues.Add("T_Plan", form.programa.Replace(Server.MapPath("~/Auditorias") + "\\" + form.id + "\\Programa\\", "").Replace("\r\n", "<w:br/>"));
                    else
                        keyValues.Add("T_Plan", "");
                    if (form.fechainicio != null)
                        keyValues.Add("T_FechaInicio", form.fechainicio.ToString().Replace(" 0:00:00", "").Replace("\r\n", "<w:br/>"));
                    else
                        keyValues.Add("T_FechaInicio", "");
                    if (form.fechafin != null)
                        keyValues.Add("T_FechaFin", form.fechafin.ToString().Replace(" 0:00:00", "").Replace("\r\n", "<w:br/>"));
                    else
                        keyValues.Add("T_FechaFin", "");

                    List<VISTA_Auditores> listaAuditores = Datos.ListarAuditores(form.id);
                    string auditores = string.Empty;
                    foreach (VISTA_Auditores refe in listaAuditores)
                    {
                        auditores = auditores + ("-" + refe.nombre + "\r\n");
                    }
                    keyValues.Add("T_EquipoAuditor", auditores.Replace("\r\n", "<w:br/>"));

                    List<VISTA_AuditoriaObservadores> listaObservadores = Datos.ListarObservadores(form.id);
                    string observadores = string.Empty;
                    foreach (VISTA_AuditoriaObservadores obs in listaObservadores)
                    {
                        observadores = observadores + ("-" + obs.nombre + "\r\n");
                    }
                    keyValues.Add("T_Observadores", observadores.Replace("\r\n", "<w:br/>"));

                    List<VISTA_AuditoriaReferenciales> listaReferenciales = Datos.ListarReferencialesAsignados(form.id);
                    string referenciales = string.Empty;
                    foreach (VISTA_AuditoriaReferenciales refe in listaReferenciales)
                    {
                        referenciales = referenciales + ("-" + refe.nombre + "\r\n");
                    }
                    keyValues.Add("T_Referenciales", referenciales.Replace("\r\n", "<w:br/>"));

                    keyValues.Add("T_Comentario", form.comentario.Replace("\r\n", "<w:br/>"));

                    List<VISTA_ListarAccionesMejora> listadoAccionesMejora = Datos.ListarAccionesMejora(idCentral, 2, id);
                    string accionesmejora = string.Empty;
                    foreach (VISTA_ListarAccionesMejora acc in listadoAccionesMejora)
                    {
                        accionesmejora = accionesmejora + ("-" + acc.codigo + "/" + acc.asunto + "\r\n");
                    }
                    keyValues.Add("T_AccionesMejora", accionesmejora.Replace("\r\n", "<w:br/>"));

                    SearchAndReplace(destinationFile, keyValues);
                }

                #endregion

                #region session y viewstate

                Session["nombreArchivo"] = destinationFile;

                audi = Datos.GetDatosAuditoria(id);
                ViewData["auditoria"] = audi;
                if (Session["CentralElegida"] != null)
                {
                    int centralElegida = int.Parse(Session["CentralElegida"].ToString());

                    centros central = Datos.ObtenerCentroPorID(centralElegida);

                    List<centros> listaCentrales = new List<centros>();

                    listaCentrales.Add(central);

                    ViewData["centros"] = listaCentrales;
                }
                List<referenciales> refer = Datos.ListarReferencialesAsignar(id);
                ViewData["referenciales"] = refer;
                List<VISTA_AuditoriaReferenciales> refeasig = Datos.ListarReferencialesAsignados(id);
                ViewData["referencialesasignados"] = refeasig;
                ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 2, id);
                List<usuarios> usus = Datos.ListarUsuariosAsignar(id);
                ViewData["usuariosasignar"] = usus;
                List<VISTA_AuditoriaObservadores> ususasig = Datos.ListarUsuariosAsignados(id);
                ViewData["usuariosasignados"] = ususasig;

                List<auditores> auds = Datos.ListarAuditoresAsignar(id);
                ViewData["auditoresasignar"] = auds;
                List<VISTA_Auditores> ListarAuditores = Datos.ListarAuditores(id);
                ViewData["auditores"] = ListarAuditores;
                #endregion

                return Redirect(Url.RouteUrl(new { controller = "Auditorias", action = "detalle_auditoria", id = id }));
                #endregion
            }
            else
            {
                #region recarga
                audi = Datos.GetDatosAuditoria(id);
                ViewData["auditoria"] = audi;
                if (Session["CentralElegida"] != null)
                {
                    int centralElegida = int.Parse(Session["CentralElegida"].ToString());

                    centros central = Datos.ObtenerCentroPorID(centralElegida);

                    List<centros> listaCentrales = new List<centros>();

                    listaCentrales.Add(central);

                    ViewData["centros"] = listaCentrales;
                }
                List<referenciales> refe = Datos.ListarReferencialesAsignar(id);
                ViewData["referenciales"] = refe;
                List<VISTA_AuditoriaReferenciales> refeasig = Datos.ListarReferencialesAsignados(id);
                ViewData["referencialesasignados"] = refeasig;
                ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 2, id);
                List<usuarios> usus = Datos.ListarUsuariosAsignar(id);
                ViewData["usuariosasignar"] = usus;
                List<VISTA_AuditoriaObservadores> ususasig = Datos.ListarUsuariosAsignados(id);
                ViewData["usuariosasignados"] = ususasig;

                List<auditores> auds = Datos.ListarAuditoresAsignar(id);
                ViewData["auditoresasignar"] = auds;
                List<VISTA_Auditores> ListarAuditores = Datos.ListarAuditores(id);
                ViewData["auditores"] = ListarAuditores;

                return View();
                #endregion
            }
        }

        public ActionResult eliminar_auditoria(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            Datos.EliminarAuditoria(id);
            Session["EditarAuditoriaResultado"] = "Auditoría eliminada";
            return RedirectToAction("auditorias", "Auditorias");
        }

        public FileResult ObtenerProgramaAuditoria(int id)
        {
            try
            {
                auditorias IF = Datos.GetDatosAuditoria(id);
                byte[] fileBytes = System.IO.File.ReadAllBytes(IF.programa);
                string fileName = IF.programa.Replace(Server.MapPath("~/Auditorias") + "\\" + IF.id + "\\Programa\\", "");
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public FileResult ObtenerInformeAuditoria(int id)
        {
            try
            {
                auditorias IF = Datos.GetDatosAuditoria(id);
                byte[] fileBytes = System.IO.File.ReadAllBytes(IF.informe);
                string fileName = IF.informe.Replace(Server.MapPath("~/Auditorias") + "\\" + IF.id + "\\Informe\\", "");
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ActionResult eliminar_referencialasociado(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            int auditoria = Datos.EliminarReferencialAsociado(id);
            Session["EdicionAuditoriaMensaje"] = "Referencial eliminado";
            return RedirectToAction("detalle_auditoria/" + auditoria, "Auditorias");
        }

        public ActionResult eliminar_observadorasociado(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            int auditoria = Datos.EliminarObservadorAsociado(id);
            Session["EdicionAuditoriaMensaje"] = "Observador eliminado";
            return RedirectToAction("detalle_auditoria/" + auditoria, "Auditorias");
        }

        public ActionResult eliminar_auditor(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            int auditoria = Datos.EliminarAuditorAsociado(id);
            Session["EdicionAuditoriaMensaje"] = "Auditor eliminado";
            return RedirectToAction("detalle_auditoria/" + auditoria, "Auditorias");
        }

        public ActionResult cualificacion(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            ViewData["auditor"] = Datos.ObtenerAuditorPorID(id);

            //Contenido del grid
            ViewData["cualificacionauditor"] = Datos.ObtenerCualificacionAuditor(id);

            return View();
        }

        [HttpPost]
        public ActionResult cualificacion(int id, FormCollection collection, HttpPostedFileBase file)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            if (file != null && file.ContentLength > 0 && collection["txtNombre"] != null)
            {
                #region guardar fichero cualificacion
                var fileName = System.IO.Path.GetFileName(file.FileName);

                var path = System.IO.Path.Combine(Server.MapPath("~/Cualificaciones/" + id), fileName);

                if (Directory.Exists(Server.MapPath("~/Cualificaciones/" + id)))
                {
                    file.SaveAs(path);
                }
                else
                {
                    Directory.CreateDirectory(Server.MapPath("~/Cualificaciones/" + id));
                    file.SaveAs(path);
                }

                cualificaciones IF = new cualificaciones();
                IF.nombre = collection["txtNombre"];
                IF.nombrefichero = file.FileName;
                IF.enlace = path;
                IF.idAuditor = id;
                IF.fecha = DateTime.Now.Date;
                Datos.InsertCualificacion(IF);
                #endregion
            }
            else
            {
                Session["error"] = 1;
            }
            return RedirectToAction("cualificacion/" + id, "Auditorias");
        }

        public FileResult ObtenerCualificacion(int id)
        {
            try
            {
                cualificaciones IF = Datos.GetDatosCualificacion(id);
                byte[] fileBytes = System.IO.File.ReadAllBytes(IF.enlace);
                string fileName = IF.enlace.Replace(Server.MapPath("~/Cualificaciones") + "\\" + IF.id + "\\", "");
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ActionResult eliminar_cualificacion(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            int auditoria = Datos.EliminarCualificacion(id);
            Session["EdicionAuditoriaMensaje"] = "Cualificacion eliminada";
            return RedirectToAction("cualificacion/" + auditoria, "Auditorias");
        }
    }
}
