using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using MIDAS.Models;
using System.Configuration;
using DocumentFormat.OpenXml.Packaging;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;

namespace MIDAS.Controllers
{
    public class ComunicacionController : Controller
    {
        //
        // GET: /Comunicacion/

        public ActionResult gestion_comunicacion()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            int idCentral = centroseleccionado.id;
            ViewData["partes"] = Datos.ListarPartes(idCentral);
            #region generacion fichero
            if (Session["nombreArchivo"] != null && Session["nombreArchivo"].ToString() != "")
            {
                if (Session["nombreArchivo"].ToString().Contains("xlsx"))
                {
                    Response.ContentType = "application/msword";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=ExportacionComunicaciones.xlsx");
                    Response.TransmitFile(Session["nombreArchivo"].ToString());
                    Response.End();
                }
                Session["nombreArchivo"] = "";
            }
            #endregion

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult gestion_comunicacion(FormCollection collection)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            VISTA_ObtenerUsuario user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());
            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            int idCentral = centroseleccionado.id;

            List<VISTA_Comunicaciones> comunicaciones = Datos.ListarComunicaciones(idCentral);
            List<VISTA_ListarEventosAmbientales> eventosambientales = Datos.ListarEventosAmbientales(idCentral);
            List<VISTA_ListarEventosCalidad> eventoscalidad = Datos.ListarEventosCalidad(idCentral);
            List<VISTA_Eventos_Seguridad> eventosseguridad = Datos.ListarEventosSeguridad(idCentral);
            List<partes> partesriesgos = Datos.ListarPartes(idCentral);
            

            #region generacion fichero
            string sourceFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaPlantillaWord"] + "ExportacionComunicacion.xlsx");
            string destinationFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaGeneracionWord"] + "ExportacionComunicaciones_" + DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + ".xlsx");

            Session["source"] = sourceFile;
            Session["destino"] = destinationFile;
            // Create a copy of the template file and open the copy 
            System.IO.File.Copy(sourceFile, destinationFile, true);

            #region impresion


            using (SpreadsheetDocument document = SpreadsheetDocument.Open(destinationFile, true))
            {

                #region hojaparteriesgos
                SheetData sheetData = document.WorkbookPart.WorksheetParts.ElementAt(0).Worksheet.Elements<SheetData>().First();
                foreach (partes obj in partesriesgos)
                {
                    Row row = new Row();

                    string rie_numero = string.Empty;
                    string rie_empresa = string.Empty;
                    string rie_instalacion = string.Empty;
                    string rie_trabajo = string.Empty;
                    string rie_detalleriesgo = string.Empty;
                    string rie_accionescorrectoras = string.Empty;
                    string rie_accionesprevistas = string.Empty;
                    string rie_cumplimentado = string.Empty;
                    string rie_fechacump = string.Empty;
                    string rie_entregado = string.Empty;
                    string rie_fechaentr = string.Empty;
                    string rie_recibido = string.Empty;
                    string rie_fechareci = string.Empty;
                    string rie_resuelto = string.Empty;
                    string rie_fecharesu = string.Empty;
                    string rie_observaciones = string.Empty;
                    string rie_desest = string.Empty;
                    string rie_fechadesest = string.Empty;

                    if (obj.idcomunicacion == null)
                        rie_numero = string.Empty;
                    else
                        rie_numero = obj.idcomunicacion;

                    if (obj.empresa == null)
                        rie_empresa = string.Empty;
                    else
                        rie_empresa = obj.empresa;

                    if (obj.instalacion == null)
                        rie_instalacion = string.Empty;
                    else
                        rie_instalacion = obj.instalacion;

                    if (obj.trabajo == null)
                        rie_trabajo = string.Empty;
                    else
                        rie_trabajo = obj.trabajo;

                    if (obj.detalle == null)
                        rie_detalleriesgo = string.Empty;
                    else
                        rie_detalleriesgo = obj.detalle;

                    if (obj.accionescorrectoras == null)
                        rie_accionescorrectoras = string.Empty;
                    else
                        rie_accionescorrectoras = obj.accionescorrectoras;

                    if (obj.accionescorrectoras == null)
                        rie_accionesprevistas = string.Empty;
                    else
                        rie_accionesprevistas = obj.accionescorrectoras;

                    if (obj.cumplimentadopor == null)
                        rie_cumplimentado = string.Empty;
                    else
                        rie_cumplimentado = obj.cumplimentadopor;

                    if (obj.cumplimentadofecha == null)
                        rie_fechacump = string.Empty;
                    else
                        rie_fechacump = obj.cumplimentadofecha.ToString().Replace(" 0:00:00", "");

                    if (obj.entregadopor == null)
                        rie_entregado = string.Empty;
                    else
                        rie_entregado = obj.entregadopor;

                    if (obj.entregadofecha == null)
                        rie_fechaentr = string.Empty;
                    else
                        rie_fechaentr = obj.entregadofecha.ToString().Replace(" 0:00:00", "");

                    if (obj.recibidounidadorg == null)
                        rie_fechareci = string.Empty;
                    else
                        rie_fechareci = obj.recibidounidadorg;

                    if (obj.recibidofecha == null)
                        rie_fechaentr = string.Empty;
                    else
                        rie_fechaentr = obj.entregadofecha.ToString().Replace(" 0:00:00", "");

                    if (obj.resueltopor == null)
                        rie_resuelto = string.Empty;
                    else
                        rie_resuelto = obj.resueltopor;

                    if (obj.resueltofecha == null)
                        rie_fecharesu = string.Empty;
                    else
                        rie_fecharesu = obj.resueltofecha.ToString().Replace(" 0:00:00", "");

                    if (obj.observaciones == null)
                        rie_observaciones = string.Empty;
                    else
                        rie_observaciones = obj.observaciones;

                    if (obj.desest == null)
                        rie_desest = string.Empty;
                    else
                        rie_desest = obj.desest;

                    if (obj.desestfecha == null)
                        rie_fechadesest = string.Empty;
                    else
                        rie_fechadesest = obj.desestfecha.ToString().Replace(" 0:00:00", "");

                    row.Append(
                        Datos.ConstructCell(rie_numero, CellValues.String),
                        Datos.ConstructCell(rie_empresa, CellValues.String),
                        Datos.ConstructCell(rie_instalacion, CellValues.String),
                        Datos.ConstructCell(rie_trabajo, CellValues.String),
                        Datos.ConstructCell(rie_detalleriesgo, CellValues.String),
                        Datos.ConstructCell(rie_accionescorrectoras, CellValues.String),
                        Datos.ConstructCell(rie_accionesprevistas, CellValues.String),
                        Datos.ConstructCell(rie_cumplimentado, CellValues.String),
                        Datos.ConstructCell(rie_fechacump, CellValues.String),
                        Datos.ConstructCell(rie_entregado, CellValues.String),
                        Datos.ConstructCell(rie_fechaentr, CellValues.String),
                        Datos.ConstructCell(rie_recibido, CellValues.String),
                        Datos.ConstructCell(rie_fechareci, CellValues.String),
                        Datos.ConstructCell(rie_resuelto, CellValues.String),
                        Datos.ConstructCell(rie_fecharesu, CellValues.String),
                        Datos.ConstructCell(rie_observaciones, CellValues.String),
                        Datos.ConstructCell(rie_desest, CellValues.String),
                        Datos.ConstructCell(rie_fechadesest, CellValues.String)
                        );

                    sheetData.AppendChild(row);
                }
                #endregion

                // save worksheet
                document.WorkbookPart.WorksheetParts.First().Worksheet.Save();

                document.WorkbookPart.Workbook.Save();

                Session["nombreArchivo"] = destinationFile;
            }
            #endregion
            #endregion

            ViewData["partes"] = Datos.ListarPartes(idCentral);
            return RedirectToAction("gestion_comunicacion", "Comunicacion");

        }

        public ActionResult Eliminar_Comunicacion(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            int idProceso = Datos.EliminarComunicacion(id);
            Session["EditarComunicacionResultado"] = "Comunicación eliminada";
            return RedirectToAction("gestion_comunicacion", "Comunicacion");
        }

        public ActionResult eliminar_parte(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            int idProceso = Datos.EliminarParte(id);
            Session["EditarComunicacionResultado"] = "Parte eliminado";
            return RedirectToAction("gestion_comunicacion", "Comunicacion");
        }

        public ActionResult Eliminar_Evento_Amb(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            int idProceso = Datos.EliminarEventoAmb(id);
            Session["EditarComunicacionResultado"] = "Evento eliminado";
            return RedirectToAction("gestion_comunicacion", "Comunicacion");
        }

        public ActionResult eliminar_evento_seg(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            int idProceso = Datos.EliminarEventoSeg(id);
            Session["EditarComunicacionResultado"] = "Evento eliminado";
            return RedirectToAction("gestion_comunicacion", "Comunicacion");
        }

        public ActionResult Eliminar_Evento_Cal(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            int idProceso = Datos.EliminarEventoCal(id);
            Session["EditarComunicacionResultado"] = "Evento eliminado";
            return RedirectToAction("gestion_comunicacion", "Comunicacion");
        }

        public ActionResult detalle_comunicacion(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            Session["modulo"] = "3";

            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            int idCentral = centroseleccionado.id;  

            comunicacion com = Datos.GetDatosComunicacion(id);
            ViewData["comunicacion"] = com;
            ViewData["clasificaciones"] = Datos.ListarClasifComunicacion();
            ViewData["tipos"] = Datos.ListarTiposComunicacion();
            ViewData["stakeholders"] = Datos.ListarStakeholdersN3();
            ViewData["canales"] = Datos.ListarCanalesComunicacion();
            ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 3, id);
            ViewData["procesosAsignables"] = Datos.ListarProcesosAsignablesComunicacion(id);
            ViewData["procesosAsignados"] = Datos.ListarProcesosAsignadosComunicacion(id);

            ViewData["documentoscomunicacion"] = Datos.ListarDocumentosComunicacion(id);

            #region generacion fichero

            if (Session["nombreArchivo"] != null && Session["nombreArchivo"].ToString() != "")
            {
                if (Session["nombreArchivo"].ToString().Contains("docx"))
                {
                    Response.ContentType = "application/msword";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=FichaComunicacion.docx");
                    Response.TransmitFile(Session["nombreArchivo"].ToString());
                    Response.End();
                }
                Session["nombreArchivo"] = "";
            }

            #endregion

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult detalle_comunicacion(int id, FormCollection collection, HttpPostedFileBase file)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }
            comunicacion comu = new comunicacion();
            string formulario = collection["hdFormularioEjecutado"];

            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            int idCentral = centroseleccionado.id; 

            if (formulario == "GuardarComunicacion")
            {
                try
                {

                    if (id != 0)
                    {
                        #region actualizar
                        comunicacion actualizar = Datos.GetDatosComunicacion(id);
                        actualizar.tipo = int.Parse(collection["ctl00$MainContent$ddlTipo"]);
                        actualizar.clasificacion = int.Parse(collection["ctl00$MainContent$ddlClasificacion"]);
                        actualizar.stakeholder = int.Parse(collection["ctl00$MainContent$ddlStakeholder"]);
                        actualizar.canal = int.Parse(collection["ctl00$MainContent$ddlCanal"]);
                        actualizar.eficacia = int.Parse(collection["ctl00$MainContent$ddlEficacia"]);
                        actualizar.asunto = collection["ctl00$MainContent$txtAsunto"];
                        actualizar.remitente = collection["ctl00$MainContent$txtRemitente"];
                        if (collection["ctl00$MainContent$txtFInicio"] != "")
                            actualizar.fechainicio = DateTime.Parse(collection["ctl00$MainContent$txtFInicio"]);
                        if (collection["ctl00$MainContent$txtFFin"] != "")
                            actualizar.fechafin = DateTime.Parse(collection["ctl00$MainContent$txtFFin"]);
                        actualizar.descripcion = collection["ctl00$MainContent$txtDescripcionCom"];
                        actualizar.descripcionres = collection["ctl00$MainContent$txtDescripcionRes"];
                        Datos.ActualizarComunicacion(actualizar);

                        string hdnProcesos = collection["ctl00$MainContent$hdnProcesosSeleccionados"].ToString();

                        string[] arrayprocesos = hdnProcesos.Split(new char[] { ';' });

                        Datos.EliminarAsociacionComunicacionProcesos(id);

                        if (arrayprocesos.Count() > 0)
                        {
                            for (int i = 0; i < arrayprocesos.Count() - 1; i++)
                            {
                                Datos.AsociarComunicacionProceso(id, int.Parse(arrayprocesos[i]));
                            }
                        }



                        Session["EdicionComunicacionMensaje"] = "Información actualizada correctamente";

                        comunicacion com = Datos.GetDatosComunicacion(id);
                        ViewData["comunicacion"] = com;
                        ViewData["clasificaciones"] = Datos.ListarClasifComunicacion();
                        ViewData["tipos"] = Datos.ListarTiposComunicacion();
                        ViewData["stakeholders"] = Datos.ListarStakeholdersN3();
                        ViewData["canales"] = Datos.ListarCanalesComunicacion();

                        ViewData["procesosAsignables"] = Datos.ListarProcesosAsignablesComunicacion(id);
                        ViewData["procesosAsignados"] = Datos.ListarProcesosAsignadosComunicacion(id);

                        ViewData["documentoscomunicacion"] = Datos.ListarDocumentosComunicacion(id);
                        ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 3, id);
                        return View();
                        #endregion
                    }
                    else
                    {
                        #region insertar
                        comunicacion insertar = new comunicacion();
                        insertar.tipo = int.Parse(collection["ctl00$MainContent$ddlTipo"]);
                        insertar.clasificacion = int.Parse(collection["ctl00$MainContent$ddlClasificacion"]);
                        insertar.stakeholder = int.Parse(collection["ctl00$MainContent$ddlStakeholder"]);
                        insertar.canal = int.Parse(collection["ctl00$MainContent$ddlCanal"]);
                        insertar.eficacia = int.Parse(collection["ctl00$MainContent$ddlEficacia"]);
                        insertar.asunto = collection["ctl00$MainContent$txtAsunto"];
                        insertar.remitente = collection["ctl00$MainContent$txtRemitente"];
                        if (collection["ctl00$MainContent$txtFInicio"] != "")
                            insertar.fechainicio = DateTime.Parse(collection["ctl00$MainContent$txtFInicio"]);
                        if (collection["ctl00$MainContent$txtFFin"] != "")
                            insertar.fechafin = DateTime.Parse(collection["ctl00$MainContent$txtFFin"]);
                        insertar.descripcion = collection["ctl00$MainContent$txtDescripcionCom"];
                        insertar.descripcionres = collection["ctl00$MainContent$txtDescripcionRes"];
                        insertar.idcentral = idCentral;
                        int idCom = Datos.ActualizarComunicacion(insertar);

                        Session["EdicionComunicacionMensaje"] = "Información actualizada correctamente";

                        comunicacion com = Datos.GetDatosComunicacion(id);
                        ViewData["comunicacion"] = com;
                        ViewData["clasificaciones"] = Datos.ListarClasifComunicacion();
                        ViewData["tipos"] = Datos.ListarTiposComunicacion();
                        ViewData["stakeholders"] = Datos.ListarStakeholdersN3();
                        ViewData["canales"] = Datos.ListarCanalesComunicacion();

                        ViewData["procesosAsignables"] = Datos.ListarProcesosAsignablesComunicacion(id);
                        ViewData["procesosAsignados"] = Datos.ListarProcesosAsignadosComunicacion(id);

                        ViewData["documentoscomunicacion"] = Datos.ListarDocumentosComunicacion(id);
                        ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 3, id);
                        return Redirect(Url.RouteUrl(new { controller = "Comunicacion", action = "detalle_comunicacion", id = idCom }));
                        #endregion
                    }
                }
                catch (Exception ex)
                {
                    #region exception
                    comunicacion com = new comunicacion();
                    if (id != 0)
                    {
                        com = Datos.GetDatosComunicacion(id);
                        com.tipo = int.Parse(collection["ctl00$MainContent$ddlTipo"]);
                        com.clasificacion = int.Parse(collection["ctl00$MainContent$ddlClasificacion"]);
                        com.stakeholder = int.Parse(collection["ctl00$MainContent$ddlStakeholder"]);
                        com.canal = int.Parse(collection["ctl00$MainContent$ddlCanal"]);
                        com.eficacia = int.Parse(collection["ctl00$MainContent$ddlEficacia"]);
                        com.asunto = collection["ctl00$MainContent$txtAsunto"];
                        if (collection["ctl00$MainContent$txtFInicio"] != "")
                            com.fechainicio = DateTime.Parse(collection["ctl00$MainContent$txtFInicio"]);
                        if (collection["ctl00$MainContent$txtFFin"] != "")
                            com.fechafin = DateTime.Parse(collection["ctl00$MainContent$txtFFin"]);
                    }
                    else
                    {
                        com.tipo = int.Parse(collection["ctl00$MainContent$ddlTipo"]);
                        com.clasificacion = int.Parse(collection["ctl00$MainContent$ddlClasificacion"]);
                        com.stakeholder = int.Parse(collection["ctl00$MainContent$ddlStakeholder"]);
                        com.canal = int.Parse(collection["ctl00$MainContent$ddlCanal"]);
                        com.eficacia = int.Parse(collection["ctl00$MainContent$ddlEficacia"]);
                        com.asunto = collection["ctl00$MainContent$txtAsunto"];
                        if (collection["ctl00$MainContent$txtFInicio"] != "")
                            com.fechainicio = DateTime.Parse(collection["ctl00$MainContent$txtFInicio"]);
                        if (collection["ctl00$MainContent$txtFFin"] != "")
                            com.fechafin = DateTime.Parse(collection["ctl00$MainContent$txtFFin"]);
                    }


                    ViewData["comunicacion"] = com;
                    ViewData["clasificaciones"] = Datos.ListarClasifComunicacion();
                    ViewData["tipos"] = Datos.ListarTiposComunicacion();
                    ViewData["stakeholders"] = Datos.ListarStakeholdersN3();
                    ViewData["canales"] = Datos.ListarCanalesComunicacion();

                    ViewData["procesosAsignables"] = Datos.ListarProcesosAsignablesComunicacion(id);
                    ViewData["procesosAsignados"] = Datos.ListarProcesosAsignadosComunicacion(id);

                    ViewData["documentoscomunicacion"] = Datos.ListarDocumentosComunicacion(id);
                    ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 3, id);
                    return View();
                    #endregion
                }
            }

            if (formulario == "btnAddDocumento")
            {
                #region add documento
                if (file != null && file.ContentLength > 0 && collection["ctl00$MainContent$txtNombreDoc"] != null)
                {
                    #region guardar fichero cualificacion
                    var fileName = System.IO.Path.GetFileName(file.FileName);

                    var path = System.IO.Path.Combine(Server.MapPath("~/Comunicacion/" + id), fileName);

                    if (Directory.Exists(Server.MapPath("~/Comunicacion/" + id)))
                    {
                        file.SaveAs(path);
                    }
                    else
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Comunicacion/" + id));
                        file.SaveAs(path);
                    }

                    comunicacion_documentos IF = new comunicacion_documentos();
                    IF.nombre = collection["ctl00$MainContent$txtNombreDoc"];
                    IF.nombrefichero = file.FileName;
                    IF.enlace = path;
                    IF.idcomunicacion = id;
                    IF.fecha = DateTime.Now.Date;
                    Datos.InsertDocComunicacion(IF);
                    #endregion
                }

                comunicacion com = Datos.GetDatosComunicacion(id);
                ViewData["comunicacion"] = com;
                ViewData["clasificaciones"] = Datos.ListarClasifComunicacion();
                ViewData["tipos"] = Datos.ListarTiposComunicacion();
                ViewData["stakeholders"] = Datos.ListarStakeholdersN3();
                ViewData["canales"] = Datos.ListarCanalesComunicacion();

                ViewData["procesosAsignables"] = Datos.ListarProcesosAsignablesComunicacion(id);
                ViewData["procesosAsignados"] = Datos.ListarProcesosAsignadosComunicacion(id);

                ViewData["documentoscomunicacion"] = Datos.ListarDocumentosComunicacion(id);
                ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 3, id);
                return View();
                #endregion
            }

            if (formulario == "btnImprimir")
            {
                #region imprimir
                string sourceFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaPlantillaWord"] + "FichaComunicacion.docx");
                string destinationFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaGeneracionWord"] + "FichaComunicacion_" + DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + ".docx");

                Session["source"] = sourceFile;
                Session["destino"] = destinationFile;
                // Create a copy of the template file and open the copy 
                System.IO.File.Copy(sourceFile, destinationFile, true);

                centros actualizar = Datos.ObtenerCentroPorID(idCentral);

                #region impresionlistado
                //LISTADO COMUNICACIONES
                VISTA_Comunicaciones comunicaciones = new VISTA_Comunicaciones();
                comunicaciones = Datos.ListarComunicacion(id);


                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add("T_Codigo", actualizar.siglas.Replace("\r\n", "<w:br/>"));
                keyValues.Add("T_IdComunicacion", comunicaciones.idcomunicacion.Replace("\r\n", "<w:br/>"));
                keyValues.Add("T_Remitente", comunicaciones.remitente.Replace("\r\n", "<w:br/>"));
                keyValues.Add("T_Tipo", comunicaciones.tipo.Replace("\r\n", "<w:br/>"));
                keyValues.Add("T_Clasificacion", comunicaciones.Clasificacion.Replace("\r\n", "<w:br/>"));
                keyValues.Add("T_Stakeholder", comunicaciones.denominacion.Replace("\r\n", "<w:br/>"));

                if (comunicaciones.fechainicio != null)
                    keyValues.Add("T_FechaRegistro", comunicaciones.fechainicio.ToString().Replace(" 0:00:00", "").Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_FechaRegistro", "");

                if (comunicaciones.fechafin != null)
                    keyValues.Add("T_FechaCierre", comunicaciones.fechafin.ToString().Replace(" 0:00:00", "").Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_FechaCierre", "");

                keyValues.Add("T_Canal", comunicaciones.canal.Replace("\r\n", "<w:br/>"));
                keyValues.Add("T_AsuntoComunicacion", comunicaciones.asunto.Replace("\r\n", "<w:br/>"));
                keyValues.Add("T_Eficacia", comunicaciones.eficacia.Replace("\r\n", "<w:br/>"));

                List<procesos> listaProcesos = Datos.ListarProcesosAsignadosComunicacion(comunicaciones.id);
                string procesos = string.Empty;
                foreach (procesos obs in listaProcesos)
                {
                    procesos = procesos + obs.nombre + "\r\n";
                }
                keyValues.Add("T_Procesos", procesos.Replace("\r\n", "<w:br/>"));

                List<comunicacion_documentos> listaDocumentos = Datos.ListarDocumentosComunicacion(comunicaciones.id);
                string documentos = string.Empty;
                foreach (comunicacion_documentos obs in listaDocumentos)
                {
                    documentos = documentos + obs.nombre + "\r\n";
                }
                keyValues.Add("T_Documentos", documentos.Replace("\r\n", "<w:br/>"));

                List<VISTA_ListarAccionesMejora> listaAcciones = Datos.ListarAccionesMejora(idCentral, 3, id);
                string accionesmejora = string.Empty;
                foreach (VISTA_ListarAccionesMejora acc in listaAcciones)
                {
                    accionesmejora = accionesmejora + acc.codigo + "/" + acc.asunto + "\r\n";
                }
                keyValues.Add("T_AccionesMejora", accionesmejora.Replace("\r\n", "<w:br/>"));

                SearchAndReplace(destinationFile, keyValues);               

                #endregion

                Session["nombreArchivo"] = destinationFile;

                comunicacion com = Datos.GetDatosComunicacion(id);
                ViewData["comunicacion"] = com;
                ViewData["clasificaciones"] = Datos.ListarClasifComunicacion();
                ViewData["tipos"] = Datos.ListarTiposComunicacion();
                ViewData["stakeholders"] = Datos.ListarStakeholdersN3();
                ViewData["canales"] = Datos.ListarCanalesComunicacion();

                ViewData["procesosAsignables"] = Datos.ListarProcesosAsignablesComunicacion(id);
                ViewData["procesosAsignados"] = Datos.ListarProcesosAsignadosComunicacion(id);

                ViewData["documentoscomunicacion"] = Datos.ListarDocumentosComunicacion(id);
                ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 3, id);
                return Redirect(Url.RouteUrl(new { controller = "Comunicacion", action = "detalle_comunicacion", id = id }));
                #endregion
            }
            else
            {
                #region recarga
                comunicacion com = new comunicacion();
                if (id != 0)
                {
                    com = Datos.GetDatosComunicacion(id);
                    com.tipo = int.Parse(collection["ctl00$MainContent$ddlTipo"]);
                    com.clasificacion = int.Parse(collection["ctl00$MainContent$ddlClasificacion"]);
                    com.stakeholder = int.Parse(collection["ctl00$MainContent$ddlStakeholder"]);
                    com.canal = int.Parse(collection["ctl00$MainContent$ddlCanal"]);
                    com.eficacia = int.Parse(collection["ctl00$MainContent$ddlEficacia"]);
                    com.asunto = collection["ctl00$MainContent$txtAsunto"];
                    if (collection["ctl00$MainContent$txtFInicio"] != "")
                        com.fechainicio = DateTime.Parse(collection["ctl00$MainContent$txtFInicio"]);
                    if (collection["ctl00$MainContent$txtFFin"] != "")
                        com.fechafin = DateTime.Parse(collection["ctl00$MainContent$txtFFin"]);
                }
                else
                {
                    com.tipo = int.Parse(collection["ctl00$MainContent$ddlTipo"]);
                    com.clasificacion = int.Parse(collection["ctl00$MainContent$ddlClasificacion"]);
                    com.stakeholder = int.Parse(collection["ctl00$MainContent$ddlStakeholder"]);
                    com.canal = int.Parse(collection["ctl00$MainContent$ddlCanal"]);
                    com.eficacia = int.Parse(collection["ctl00$MainContent$ddlEficacia"]);
                    com.asunto = collection["ctl00$MainContent$txtAsunto"];
                    if (collection["ctl00$MainContent$txtFInicio"] != "")
                        com.fechainicio = DateTime.Parse(collection["ctl00$MainContent$txtFInicio"]);
                    if (collection["ctl00$MainContent$txtFFin"] != "")
                        com.fechafin = DateTime.Parse(collection["ctl00$MainContent$txtFFin"]);
                }


                ViewData["comunicacion"] = com;
                ViewData["clasificaciones"] = Datos.ListarClasifComunicacion();
                ViewData["tipos"] = Datos.ListarTiposComunicacion();
                ViewData["stakeholders"] = Datos.ListarStakeholdersN3();
                ViewData["canales"] = Datos.ListarCanalesComunicacion();

                ViewData["procesosAsignables"] = Datos.ListarProcesosAsignablesComunicacion(id);
                ViewData["procesosAsignados"] = Datos.ListarProcesosAsignadosComunicacion(id);

                ViewData["documentoscomunicacion"] = Datos.ListarDocumentosComunicacion(id);
                ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 3, id);
                return View();
                #endregion
            }
        }

        public FileResult ObtenerDocComunicacion(int id)
        {
            try
            {
                comunicacion_documentos IF = Datos.GetDatosDocComunicacion(id);
                byte[] fileBytes = System.IO.File.ReadAllBytes(IF.enlace);
                string fileName = IF.enlace.Replace(Server.MapPath("~/Comunicacion") + "\\" + IF.id + "\\", "");
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ActionResult eliminar_doccomunicacion(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            int idcomunicacion = Datos.EliminarDocComunicacion(id);
            Session["EdicionComunicacionMensaje"] = "Documentación eliminada";
            return RedirectToAction("detalle_comunicacion/" + idcomunicacion, "Comunicacion");
        }

        public ActionResult eliminar_doceventoamb(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            int idEventoAmb = Datos.EliminarDocEventoAmb(id);
            Session["EdicionEventoAmbMensaje"] = "Documentación eliminada";
            return RedirectToAction("detalle_evento_amb/" + idEventoAmb, "Comunicacion");
        }

        public ActionResult eliminar_docseg(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            int idSeg = Datos.EliminarDocSeg(id);
            Session["EdicionSegMensaje"] = "Documentación eliminada";
            return RedirectToAction("detalle_evento_seg/" + idSeg, "Comunicacion");
        }

        public ActionResult eliminar_fotoeventoamb(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            int idEventoAmb = Datos.EliminarFotoEventoAmb(id);
            Session["EdicionEventoAmbMensaje"] = "Foto eliminada";
            return RedirectToAction("detalle_evento_amb/" + idEventoAmb, "Comunicacion");
        }

        public ActionResult eliminar_fotoeventoseg(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            int idEventoSeg = Datos.EliminarFotoEventoSeg(id);
            Session["EdicionEventoSegMensaje"] = "Foto eliminada";
            return RedirectToAction("detalle_evento_seg/" + idEventoSeg, "Comunicacion");
        }

        public FileResult ObtenerDocEventoAmb(int id)
        {
            try
            {
                evento_ambiental_documentos IF = Datos.GetDatosDocEventoAmb(id);
                byte[] fileBytes = System.IO.File.ReadAllBytes(IF.enlace);
                //string fileName = IF.enlace.Replace(Server.MapPath("~/EventosAmbientales") + "\\" + IF.id + "\\", "");
                string fileName = IF.nombrefichero;
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public FileResult ObtenerDocSeg(int id)
        {
            try
            {
                evento_seguridad_documentos IF = Datos.GetDatosDocEventoSeg(id);
                byte[] fileBytes = System.IO.File.ReadAllBytes(IF.enlace);
                //string fileName = IF.enlace.Replace(Server.MapPath("~/EventosAmbientales") + "\\" + IF.id + "\\", "");
                string fileName = IF.nombrefichero;
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ActionResult detalle_evento_cal(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            Session["modulo"] = "14";

            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            int idCentral = centroseleccionado.id;
            Session["ReferenciaAccionMejora"] = id;

            evento_calidad even = Datos.GetDatosEventoCal(id);
            ViewData["eventocalidad"] = even;
            ViewData["responsables"] = Datos.ListarUsuariosCentral(idCentral);
            ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 14, id);

            #region generacion fichero

            if (Session["nombreArchivo"] != null && Session["nombreArchivo"].ToString() != "")
            {
                if (Session["nombreArchivo"].ToString().Contains("docx"))
                {
                    Response.ContentType = "application/msword";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=FichaEventoCal.docx");
                    Response.TransmitFile(Session["nombreArchivo"].ToString());
                    Response.End();
                }
                Session["nombreArchivo"] = "";
            }

            #endregion

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult detalle_evento_cal(int id, FormCollection collection)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }
            string formulario = collection["hdFormularioEjecutado"];

            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            int idCentral = centroseleccionado.id;

            if (formulario == "GuardarEventoCal")
            {
                try
                {

                    if (id != 0)
                    {
                        #region actualizar
                        evento_calidad actualizar = Datos.GetDatosEventoCal(id);

                        actualizar.asunto = collection["ctl00$MainContent$txtAsunto"];
                        if (collection["ctl00$MainContent$txtFechaComienzo"] != "")
                            actualizar.fechacomienzo = DateTime.Parse(collection["ctl00$MainContent$txtFechaComienzo"]);
                        if (collection["ctl00$MainContent$txtFechaFin"] != "")
                            actualizar.fechafin = DateTime.Parse(collection["ctl00$MainContent$txtFechaFin"]);
                        actualizar.tecnologia = int.Parse(collection["ctl00$MainContent$ddlTecnologia"]);
                        actualizar.idcentral = int.Parse(collection["ctl00$MainContent$ddlCentral"]);
                        actualizar.unidad = collection["ctl00$MainContent$txtUnidad"];
                        actualizar.evento = collection["ctl00$MainContent$txtEvento"];
                        actualizar.descripcion = collection["ctl00$MainContent$txtDescripcion"];
                        actualizar.impacto = collection["ctl00$MainContent$txtImpacto"];
                        actualizar.cargo = collection["ctl00$MainContent$txtCargo"];
                        actualizar.persona = int.Parse(collection["ctl00$MainContent$ddlResponsable"]);
                        actualizar.compania = "Applus";
                        actualizar.pais = "España";
                        actualizar.codevento = "Significativo/Significant";

                        Datos.ActualizarEventoCal(actualizar);

                        Session["EdicionEventoCalMensaje"] = "Información actualizada correctamente";

                        evento_calidad even = Datos.GetDatosEventoCal(id);
                        ViewData["eventocalidad"] = even;
                        ViewData["responsables"] = Datos.ListarUsuariosCentral(idCentral);
                        ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 14, id);
                        return View();
                        #endregion
                    }
                    else
                    {
                        #region insertar
                        evento_calidad insertar = new evento_calidad();

                        insertar.asunto = collection["ctl00$MainContent$txtAsunto"];
                        if (collection["ctl00$MainContent$txtFechaComienzo"] != "")
                            insertar.fechacomienzo = DateTime.Parse(collection["ctl00$MainContent$txtFechaComienzo"]);
                        if (collection["ctl00$MainContent$txtFechaFin"] != "")
                            insertar.fechafin = DateTime.Parse(collection["ctl00$MainContent$txtFechaFin"]);
                        insertar.tecnologia = int.Parse(collection["ctl00$MainContent$ddlTecnologia"]);
                        insertar.idcentral = int.Parse(collection["ctl00$MainContent$ddlCentral"]);
                        insertar.unidad = collection["ctl00$MainContent$txtUnidad"];
                        insertar.evento = collection["ctl00$MainContent$txtEvento"];
                        insertar.descripcion = collection["ctl00$MainContent$txtDescripcion"];
                        insertar.impacto = collection["ctl00$MainContent$txtImpacto"];
                        insertar.cargo = collection["ctl00$MainContent$txtCargo"];
                        insertar.persona = int.Parse(collection["ctl00$MainContent$ddlResponsable"]);
                        insertar.compania = "Applus";
                        insertar.pais = "España";
                        insertar.codevento = "Significativo/Significant";

                        int idCom = Datos.ActualizarEventoCal(insertar);

                        Session["EdicionEventoCalMensaje"] = "Información actualizada correctamente";

                        evento_calidad even = Datos.GetDatosEventoCal(id);
                        ViewData["eventocalidad"] = even;
                        ViewData["responsables"] = Datos.ListarUsuariosCentral(idCentral);
                        ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 14, id);

                        return Redirect(Url.RouteUrl(new { controller = "Comunicacion", action = "detalle_evento_cal", id = idCom }));
                        #endregion
                    }
                }
                catch (Exception ex)
                {
                    #region exception
                    evento_calidad even = Datos.GetDatosEventoCal(id);
                    ViewData["eventocalidad"] = even;
                    ViewData["responsables"] = Datos.ListarUsuariosCentral(idCentral);
                    ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 14, id);
                    return View();
                    #endregion
                }
            }

            if (formulario == "btnImprimir")
            {
                #region imprimir
                string sourceFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaPlantillaWord"] + "FichaEventoCalidad.docx");
                string destinationFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaGeneracionWord"] + "FichaEventoCalidad_" + DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + ".docx");

                Session["source"] = sourceFile;
                Session["destino"] = destinationFile;
                // Create a copy of the template file and open the copy 
                System.IO.File.Copy(sourceFile, destinationFile, true);

                evento_calidad actualizar = Datos.GetDatosEventoCal(id);

                // create key value pair, key represents words to be replace and 
                //values represent values in document in place of keys.
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                if (actualizar.fechacomienzo != null)
                    keyValues.Add("T_FechaComienzo", actualizar.fechacomienzo.ToString().Replace(" 0:00:00", "").Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_FechaComienzo", "");

                if (actualizar.fechafin != null)
                    keyValues.Add("T_FechaFin", actualizar.fechafin.ToString().Replace(" 0:00:00", "").Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_FechaFin", "");                

                if (actualizar.asunto != null)
                    keyValues.Add("T_Asunto", actualizar.asunto.Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_Asunto", "");

                if (actualizar.unidad != null)
                    keyValues.Add("T_Unidad", actualizar.unidad.Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_Unidad", "");

                centros central = Datos.ObtenerCentroPorID(actualizar.idcentral);
                keyValues.Add("T_Business", central.nombre.Replace("\r\n", "<w:br/>"));

                tipocentral tcentral = Datos.ListarTecnologia(central.tipo);
                keyValues.Add("T_Tecnologia", tcentral.nombre.Replace("\r\n", "<w:br/>"));

                if (actualizar.evento != null)
                    keyValues.Add("T_Evento", actualizar.evento.Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_Evento", "");

                if (actualizar.descripcion != null)
                    keyValues.Add("T_Descripcion", actualizar.descripcion.Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_Descripcion", "");

                if (actualizar.impacto != null)
                    keyValues.Add("T_Impacto", actualizar.impacto.Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_Impacto", "");

                if (actualizar.cargo != null)
                    keyValues.Add("T_Cargo", actualizar.cargo.Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_Cargo", "");

                usuarios usu = Datos.ObtenerUsuario(actualizar.persona);
                keyValues.Add("T_Responsable", usu.nombre.Replace("\r\n", "<w:br/>"));

                SearchAndReplace(destinationFile, keyValues);

                Session["nombreArchivo"] = destinationFile;

                evento_calidad even = Datos.GetDatosEventoCal(id);
                ViewData["eventocalidad"] = even;
                ViewData["responsables"] = Datos.ListarUsuariosCentral(idCentral);
                ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 14, id);
                return Redirect(Url.RouteUrl(new { controller = "Comunicacion", action = "detalle_evento_cal", id = id }));
                #endregion imprimir

            }
            else
            {
                #region recarga
                evento_calidad even = Datos.GetDatosEventoCal(id);
                ViewData["eventocalidad"] = even;
                ViewData["responsables"] = Datos.ListarUsuariosCentral(idCentral);
                ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 14, id);
                return View();
                #endregion
            }
        }

        public ActionResult detalle_evento_amb(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            Session["modulo"] = "13";

            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            int idCentral = centroseleccionado.id;
            Session["ReferenciaAccionMejora"] = id;

            List<evento_ambiental_tipo> eventipos = Datos.GetDatosEventoAmbTipos();
            ViewData["tiposeventoamb"] = eventipos;
            List<evento_ambiental_matriz> evenmatrices= Datos.GetDatosEventoAmbMatrices();
            ViewData["matriceseventoamb"] = evenmatrices;
            evento_ambiental even = Datos.GetDatosEventoAmb(id);
            ViewData["eventoambiental"] = even;
            ViewData["documentoseventoamb"] = Datos.ListarDocumentosEventoAmb(id);
            ViewData["fotoseventoamb"] = Datos.ListarFotosEventoAmb(id);
            ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 13, id);

            #region generacion fichero

            if (Session["nombreArchivo"] != null && Session["nombreArchivo"].ToString() != "")
            {
                if (Session["nombreArchivo"].ToString().Contains("xlsx"))
                {
                    Response.ContentType = "application/msexcel";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=FichaEventoAmb.xlsx");
                    Response.TransmitFile(Session["nombreArchivo"].ToString());
                    Response.End();
                }
                Session["nombreArchivo"] = "";
            }

            #endregion

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult detalle_evento_amb(int id, FormCollection collection, HttpPostedFileBase[] file)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }
            string formulario = collection["hdFormularioEjecutado"];
            evento_ambiental even = new evento_ambiental();

            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            int idCentral = centroseleccionado.id;

            if (formulario == "GuardarEventoAmb")
            {
                #region guardar
                try
                {

                    if (id != 0)
                    {
                        #region actualizar
                        evento_ambiental actualizar = Datos.GetDatosEventoAmb(id);

                        if (collection["ctl00$MainContent$txtFechaEvento"] != "")
                            actualizar.fechaevento = DateTime.Parse(collection["ctl00$MainContent$txtFechaEvento"]);
                        actualizar.tipo = int.Parse(collection["ctl00$MainContent$ddlTipo"]);
                        actualizar.matrizprincipal = int.Parse(collection["ctl00$MainContent$ddlMatrizPrincipal"]);
                        actualizar.matrizsecundaria = int.Parse(collection["ctl00$MainContent$ddlMatrizSecundaria"]);
                        actualizar.unidadnegocio = collection["ctl00$MainContent$txtUnidadNegocio"];
                        actualizar.idcentral = centroseleccionado.id;
                        actualizar.idtecnologia = int.Parse(collection["ctl00$MainContent$ddlTecnologia"]);
                        actualizar.companiainvolucrada = int.Parse(collection["ctl00$MainContent$ddlCompInvolucrada"]);
                        actualizar.empresacontratista = collection["ctl00$MainContent$txtEmpContratista"];

                        if (actualizar.tipo == 1 || actualizar.tipo == 2 || actualizar.tipo == 3)
                        {
                            actualizar.claseevento_sec2 = int.Parse(collection["ctl00$MainContent$ddlClaseEvento"]);
                            actualizar.extension_sec2 = collection["ctl00$MainContent$txtExtension"];
                            actualizar.impacto_sec2 = int.Parse(collection["ctl00$MainContent$ddlImpacto"]);
                            actualizar.localizacion_sec2 = collection["ctl00$MainContent$txtLocalizacion"];
                            actualizar.descripcion_sec2 = collection["ctl00$MainContent$txtDescripcionSec2"];
                            actualizar.causa_sec2 = collection["ctl00$MainContent$txtCausa"];
                            actualizar.accionesinmediatas_sec2 = collection["ctl00$MainContent$txtAccionesInmediatas"];
                            actualizar.infoadicional = collection["ctl00$MainContent$txtInfoAdicionalSec2"];
                        }

                        if (actualizar.tipo == 4 || actualizar.tipo == 5)
                        {
                            actualizar.descripcion_sec3 = collection["ctl00$MainContent$txtDescripcionSec3"];
                            actualizar.demandante_sec3 = collection["ctl00$MainContent$txtDemandante"];
                            actualizar.tipodemantante_sec3 = int.Parse(collection["ctl00$MainContent$ddlTipoDemandante"]);
                            actualizar.tipocriticidad_sec3 = int.Parse(collection["ctl00$MainContent$ddlCriticidad"]);
                            actualizar.infoadicional = collection["ctl00$MainContent$txtInfoAdicionalSec3"];
                        }

                        Datos.ActualizarEventoAmb(actualizar);

                        Session["EdicionEventoAmbMensaje"] = "Información actualizada correctamente";

                        List<evento_ambiental_tipo> eventipos = Datos.GetDatosEventoAmbTipos();
                        ViewData["tiposeventoamb"] = eventipos;
                        List<evento_ambiental_matriz> evenmatrices = Datos.GetDatosEventoAmbMatrices();
                        ViewData["matriceseventoamb"] = evenmatrices;
                        even = Datos.GetDatosEventoAmb(id);
                        ViewData["eventoambiental"] = even;
                        ViewData["documentoseventoamb"] = Datos.ListarDocumentosEventoAmb(id);
                        ViewData["fotoseventoamb"] = Datos.ListarFotosEventoAmb(id);
                        ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 13, id);
                        return View();
                        #endregion
                    }
                    else
                    {
                        #region insertar
                        evento_ambiental insertar = new evento_ambiental();
                        if (collection["ctl00$MainContent$txtFechaEvento"] != "")
                            insertar.fechaevento = DateTime.Parse(collection["ctl00$MainContent$txtFechaEvento"]);
                        insertar.tipo = int.Parse(collection["ctl00$MainContent$ddlTipo"]);
                        insertar.matrizprincipal = int.Parse(collection["ctl00$MainContent$ddlMatrizPrincipal"]);
                        insertar.matrizsecundaria = int.Parse(collection["ctl00$MainContent$ddlMatrizSecundaria"]);
                        insertar.organizacion = collection["ctl00$MainContent$txtOrganizacion"];
                        insertar.pais = collection["ctl00$MainContent$txtPais"];
                        insertar.unidadnegocio = collection["ctl00$MainContent$txtUnidadNegocio"];
                        insertar.idcentral = centroseleccionado.id;
                        insertar.idtecnologia = int.Parse(collection["ctl00$MainContent$ddlTecnologia"]);
                        insertar.companiainvolucrada = int.Parse(collection["ctl00$MainContent$ddlCompInvolucrada"]);
                        insertar.companiaenel = collection["ctl00$MainContent$txtCompENEL"];
                        insertar.empresacontratista = collection["ctl00$MainContent$txtEmpContratista"];

                        if (insertar.tipo == 1 || insertar.tipo == 2 || insertar.tipo == 3)
                        {
                            insertar.claseevento_sec2 = int.Parse(collection["ctl00$MainContent$ddlClaseEvento"]);
                            insertar.extension_sec2 = collection["ctl00$MainContent$txtExtension"];
                            insertar.impacto_sec2 = int.Parse(collection["ctl00$MainContent$ddlImpacto"]);
                            insertar.localizacion_sec2 = collection["ctl00$MainContent$txtLocalizacion"];
                            insertar.descripcion_sec2 = collection["ctl00$MainContent$txtDescripcionSec2"];
                            insertar.causa_sec2 = collection["ctl00$MainContent$txtCausa"];
                            insertar.accionesinmediatas_sec2 = collection["ctl00$MainContent$txtAccionesInmediatas"];
                        }

                        if (insertar.tipo == 4 || insertar.tipo == 5)
                        {
                            insertar.descripcion_sec3 = collection["ctl00$MainContent$txtDescripcionSec3"];
                            insertar.demandante_sec3 = collection["ctl00$MainContent$txtDemandante"];
                            insertar.tipodemantante_sec3 = int.Parse(collection["ctl00$MainContent$ddlTipoDemandante"]);
                            insertar.tipocriticidad_sec3 = int.Parse(collection["ctl00$MainContent$ddlCriticidad"]);
                        }

                        int idCom = Datos.ActualizarEventoAmb(insertar);

                        Session["EdicionEventoAmbMensaje"] = "Información actualizada correctamente";

                        List<evento_ambiental_tipo> eventipos = Datos.GetDatosEventoAmbTipos();
                        ViewData["tiposeventoamb"] = eventipos;
                        List<evento_ambiental_matriz> evenmatrices = Datos.GetDatosEventoAmbMatrices();
                        ViewData["matriceseventoamb"] = evenmatrices;
                        even = Datos.GetDatosEventoAmb(id);
                        ViewData["eventoambiental"] = even;
                        ViewData["documentoseventoamb"] = Datos.ListarDocumentosEventoAmb(id);
                        ViewData["fotoseventoamb"] = Datos.ListarFotosEventoAmb(id);
                        ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 13, id);
                        return Redirect(Url.RouteUrl(new { controller = "Comunicacion", action = "detalle_evento_amb", id = idCom }));
                        #endregion
                    }
                }
                catch (Exception ex)
                {
                    #region exception
                    List<evento_ambiental_tipo> eventipos = Datos.GetDatosEventoAmbTipos();
                    ViewData["tiposeventoamb"] = eventipos;
                    List<evento_ambiental_matriz> evenmatrices = Datos.GetDatosEventoAmbMatrices();
                    ViewData["matriceseventoamb"] = evenmatrices;
                    even = Datos.GetDatosEventoAmb(id);
                    ViewData["eventoambiental"] = even;
                    ViewData["documentoseventoamb"] = Datos.ListarDocumentosEventoAmb(id);
                    ViewData["fotoseventoamb"] = Datos.ListarFotosEventoAmb(id);
                    ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 13, id);
                    return View();
                    #endregion
                }
                #endregion
            }

            if (formulario == "btnAddDocumento")
            {
                #region add documento
                if (file != null && file[0].ContentLength > 0 && collection["ctl00$MainContent$txtNombreDoc"] != null)
                {
                    #region guardar fichero cualificacion
                    var fileName = System.IO.Path.GetFileName(file[0].FileName);

                    var path = System.IO.Path.Combine(Server.MapPath("~/EventosAmbientales/" + id), fileName);

                    if (Directory.Exists(Server.MapPath("~/EventosAmbientales/" + id)))
                    {
                        file[0].SaveAs(path);
                    }
                    else
                    {
                        Directory.CreateDirectory(Server.MapPath("~/EventosAmbientales/" + id));
                        file[0].SaveAs(path);
                    }

                    evento_ambiental_documentos IF = new evento_ambiental_documentos();
                    IF.nombre = collection["ctl00$MainContent$txtNombreDoc"];
                    IF.nombrefichero = file[0].FileName;
                    IF.enlace = path;
                    IF.ideventoamb = id;
                    IF.fecha = DateTime.Now.Date;
                    Datos.InsertDocEventoAmb(IF);
                    #endregion
                }

                List<evento_ambiental_tipo> eventipos = Datos.GetDatosEventoAmbTipos();
                ViewData["tiposeventoamb"] = eventipos;
                List<evento_ambiental_matriz> evenmatrices = Datos.GetDatosEventoAmbMatrices();
                ViewData["matriceseventoamb"] = evenmatrices;
                even = Datos.GetDatosEventoAmb(id);
                ViewData["eventoambiental"] = even;
                ViewData["documentoseventoamb"] = Datos.ListarDocumentosEventoAmb(id);
                ViewData["fotoseventoamb"] = Datos.ListarFotosEventoAmb(id);
                ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 13, id);
                return View();
                #endregion
            }

            if (formulario == "SubirDocumento")
            {
                #region subirDocumento
                if (collection["ctl00$MainContent$txtNombreFichero"] != null && collection["ctl00$MainContent$txtNombreFichero"] != "")
                {

                    if (file[1] != null && file[1].ContentLength > 0)
                    {
                        evento_ambiental_foto datostec = new evento_ambiental_foto();

                        var fileName = System.IO.Path.GetFileName(file[1].FileName);

                        datostec.idEventoAmbiental = id;
                        datostec.nombre = collection["ctl00$MainContent$txtNombreFichero"];
                        datostec.nombre_fichero = fileName;

                        var path = System.IO.Path.Combine(Server.MapPath("~/EventosAmbientales/" + id.ToString()), fileName);
                        datostec.enlace = path;

                        int idFichero = Datos.InsertFotoEventoAmbiental(datostec);

                        if (Directory.Exists(Server.MapPath("~/EventosAmbientales/" + id)))
                        {
                            file[1].SaveAs(path);
                        }
                        else
                        {
                            Directory.CreateDirectory(Server.MapPath("~/EventosAmbientales/" + id));
                            file[1].SaveAs(path);
                        }

                        Session["EdicionEventoAmbMensaje"] = "Foto agregada correctamente.";
                    }
                }
                else
                {
                    Session["EdicionEventoAmbError"] = "Los campos marcados con (*) son obligatorios.";
                }

                List<evento_ambiental_tipo> eventipos = Datos.GetDatosEventoAmbTipos();
                ViewData["tiposeventoamb"] = eventipos;
                List<evento_ambiental_matriz> evenmatrices = Datos.GetDatosEventoAmbMatrices();
                ViewData["matriceseventoamb"] = evenmatrices;
                even = Datos.GetDatosEventoAmb(id);
                ViewData["eventoambiental"] = even;
                ViewData["documentoseventoamb"] = Datos.ListarDocumentosEventoAmb(id);
                ViewData["fotoseventoamb"] = Datos.ListarFotosEventoAmb(id);
                ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 13, id);
                return View();
                #endregion
            }
              
            if (formulario == "btnImprimir")
            {
                #region imprimir
                string sourceFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaPlantillaWord"] + "ExportacionEventosAmbientales.xlsx");
                string destinationFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaGeneracionWord"] + "ExportacionEventosAmbientales_" + DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + ".xlsx");

                Session["source"] = sourceFile;
                Session["destino"] = destinationFile;
                // Create a copy of the template file and open the copy 
                System.IO.File.Copy(sourceFile, destinationFile, true);

                evento_ambiental actualizar = Datos.GetDatosEventoAmb(id);

                using (SpreadsheetDocument document = SpreadsheetDocument.Open(destinationFile, true))
                {
                    WorksheetPart worksheetPart1 = GetWorksheetPartByName(document, "Registro Comunicación eventos");

                    if (worksheetPart1 != null)
                    {
                        Cell fechev = GetCell(worksheetPart1.Worksheet, "C", 21);
                        if (actualizar.fechaevento != null)
                            fechev.CellValue = new CellValue(actualizar.fechaevento.ToString().Replace(" 0:00:00", ""));
                        fechev.DataType = new EnumValue<CellValues>(CellValues.String);

                        Cell tipev = GetCell(worksheetPart1.Worksheet, "C", 22);
                        string tipoEvento;
                        switch (actualizar.tipo)
                        {
                            case 1:
                                tipoEvento = "Incidente ambiental (Environmental Near Miss)";
                                break;
                            case 2:
                                tipoEvento = "Incidente ambiental significativo (Environmental Significant Near Miss)";
                                break;
                            case 3:
                                tipoEvento = "Daño ambiental (Environmental Damage)";
                                break;
                            case 4:
                                tipoEvento = "Criticidad ambiental (Environmental Criticality)";
                                break;
                            case 5:
                                tipoEvento = "Litigio ambiental (Environmental Litigation)";
                                break;
                            default:
                                tipoEvento = "";
                                break;
                        }
                        tipev.CellValue = new CellValue(tipoEvento.ToString());
                        tipev.DataType = new EnumValue<CellValues>(CellValues.String);

                        Cell matrizprinciapl = GetCell(worksheetPart1.Worksheet, "C", 23);
                        string matrizPrincipal;
                        switch (actualizar.matrizprincipal)
                        {
                            case 1:
                                matrizPrincipal = "Aire";
                                break;
                            case 2:
                                matrizPrincipal = "Suelo";
                                break;
                            case 3:
                                matrizPrincipal = "Aguas subterráneas";
                                break;
                            case 4:
                                matrizPrincipal = "Aguas superficiales";
                                break;
                            case 5:
                                matrizPrincipal = "Biodiversidad";
                                break;
                            case 6:
                                matrizPrincipal = "Otros aspectos ambientales: ruido/vibraciones y radiación";
                                break;
                            default:
                                matrizPrincipal = "";
                                break;
                        }
                        matrizprinciapl.CellValue = new CellValue(matrizPrincipal.ToString());
                        matrizprinciapl.DataType = new EnumValue<CellValues>(CellValues.String);

                        Cell matrizsecundaria = GetCell(worksheetPart1.Worksheet, "C", 24);
                        string matrizSecundaria;
                        switch (actualizar.matrizsecundaria)
                        {
                            case 1:
                                matrizSecundaria = "Aire";
                                break;
                            case 2:
                                matrizSecundaria = "Suelo";
                                break;
                            case 3:
                                matrizSecundaria = "Aguas subterráneas";
                                break;
                            case 4:
                                matrizSecundaria = "Aguas superficiales";
                                break;
                            case 5:
                                matrizSecundaria = "Biodiversidad";
                                break;
                            case 6:
                                matrizSecundaria = "Otros aspectos ambientales: ruido/vibraciones y radiación";
                                break;
                            default:
                                matrizSecundaria = "";
                                break;
                        }
                        matrizsecundaria.CellValue = new CellValue(matrizSecundaria.ToString());
                        matrizsecundaria.DataType = new EnumValue<CellValues>(CellValues.String);

                        Cell unidadnegocio = GetCell(worksheetPart1.Worksheet, "C", 27);
                        if (actualizar.unidadnegocio != null)
                            unidadnegocio.CellValue = new CellValue(actualizar.unidadnegocio.ToString());
                        unidadnegocio.DataType = new EnumValue<CellValues>(CellValues.String);

                        Cell sede = GetCell(worksheetPart1.Worksheet, "C", 28);
                        centros central = Datos.ObtenerCentroPorID(actualizar.idcentral);
                        if (central != null)
                            sede.CellValue = new CellValue(central.nombre.ToString());
                        sede.DataType = new EnumValue<CellValues>(CellValues.String);

                        Cell tsede = GetCell(worksheetPart1.Worksheet, "C", 29);
                        tipocentral tcentral = Datos.ListarTecnologia(central.tipo);
                        if (tcentral != null)
                            tsede.CellValue = new CellValue(tcentral.nombre.ToString());
                        tsede.DataType = new EnumValue<CellValues>(CellValues.String);

                        Cell compania = GetCell(worksheetPart1.Worksheet, "C", 30);
                        string companiainv;
                        switch (actualizar.companiainvolucrada)
                        {
                            case 1:
                                companiainv = "Applus";
                                break;
                            case 2:
                                companiainv = actualizar.empresacontratista;
                                break;
                            default:
                                companiainv = "";
                                break;
                        }
                        compania.CellValue = new CellValue(companiainv.ToString());
                        compania.DataType = new EnumValue<CellValues>(CellValues.String);

                        Cell clasesec2 = GetCell(worksheetPart1.Worksheet, "C", 33);
                        string claseevento_sec2 = "";
                        switch (actualizar.claseevento_sec2)
                        {
                            case 1:
                                claseevento_sec2 = "Derrame de aceite/Oil spill";
                                break;
                            case 2:
                                claseevento_sec2 = "Derrame de fuel/Fuel spill";
                                break;
                            case 3:
                                claseevento_sec2 = "Derrame sustancia química/Chemicals spill";
                                break;
                            case 4:
                                claseevento_sec2 = "Emisión de ruido/Noise emissions";
                                break;
                            case 5:
                                claseevento_sec2 = "Vertido de agua/Water emissions";
                                break;
                            case 6:
                                claseevento_sec2 = "Emisiones atmosféricas/Air emissions";
                                break;
                            case 7:
                                claseevento_sec2 = "Fuego/Fire";
                                break;
                            case 8:
                                claseevento_sec2 = "Radiación/Radiation";
                                break;
                            case 9:
                                claseevento_sec2 = "Otro/Other";
                                break;
                        }
                        clasesec2.CellValue = new CellValue(claseevento_sec2.ToString());
                        clasesec2.DataType = new EnumValue<CellValues>(CellValues.String);

                        Cell extension = GetCell(worksheetPart1.Worksheet, "C", 34);
                        if (actualizar.extension_sec2 != null)
                            extension.CellValue = new CellValue(actualizar.extension_sec2.ToString());
                        extension.DataType = new EnumValue<CellValues>(CellValues.String);

                        Cell impacto = GetCell(worksheetPart1.Worksheet, "C", 35);
                        string impactosec2 = "";
                        switch (actualizar.impacto_sec2)
                        {
                            case 1:
                                impactosec2 = "No afectado por el evento/Not affected by the event";
                                break;
                            case 2:
                                impactosec2 = "Posible interés por las partes interesadas/Possible interest of the stakeholders";
                                break;
                            case 3:
                                impactosec2 = "Interés por las partes interesadas/Stakeholder interest";
                                break;
                        }
                        impacto.CellValue = new CellValue(impactosec2.ToString());
                        impacto.DataType = new EnumValue<CellValues>(CellValues.String);

                        Cell localizacion = GetCell(worksheetPart1.Worksheet, "C", 36);
                        if (actualizar.localizacion_sec2 != null)
                            localizacion.CellValue = new CellValue(actualizar.localizacion_sec2.ToString());
                        localizacion.DataType = new EnumValue<CellValues>(CellValues.String);

                        Cell descripcion2 = GetCell(worksheetPart1.Worksheet, "C", 37);
                        if (actualizar.descripcion_sec2 != null)
                            descripcion2.CellValue = new CellValue(actualizar.descripcion_sec2.ToString().Replace("&", "&amp;"));
                        descripcion2.DataType = new EnumValue<CellValues>(CellValues.String);

                        Cell causa2 = GetCell(worksheetPart1.Worksheet, "C", 38);
                        if (actualizar.causa_sec2 != null)
                            causa2.CellValue = new CellValue(actualizar.causa_sec2.ToString().Replace("&", "&amp;"));
                        causa2.DataType = new EnumValue<CellValues>(CellValues.String);

                        Cell accionesinm = GetCell(worksheetPart1.Worksheet, "C", 39);
                        if (actualizar.accionesinmediatas_sec2 != null)
                            accionesinm.CellValue = new CellValue(actualizar.accionesinmediatas_sec2.ToString().Replace("&", "&amp;"));
                        accionesinm.DataType = new EnumValue<CellValues>(CellValues.String);

                        Cell descripcion3 = GetCell(worksheetPart1.Worksheet, "C", 41);
                        if (actualizar.descripcion_sec3 != null)
                            descripcion3.CellValue = new CellValue(actualizar.descripcion_sec3.ToString().Replace("&", "&amp;"));
                        descripcion3.DataType = new EnumValue<CellValues>(CellValues.String);

                        Cell demandante = GetCell(worksheetPart1.Worksheet, "C", 42);
                        if (actualizar.demandante_sec3 != null)
                            demandante.CellValue = new CellValue(actualizar.demandante_sec3.ToString().Replace("&", "&amp;"));
                        demandante.DataType = new EnumValue<CellValues>(CellValues.String);

                        Cell tipodemandantesec3 = GetCell(worksheetPart1.Worksheet, "C", 43);
                        string tipodemandante = "";
                        switch (actualizar.tipodemantante_sec3)
                        {
                            case 1:
                                tipodemandante = "Público/Public";
                                break;
                            case 2:
                                tipodemandante = "Privado/Private";
                                break;
                        }
                        tipodemandantesec3.CellValue = new CellValue(tipodemandante.ToString().Replace("&", "&amp;"));
                        tipodemandantesec3.DataType = new EnumValue<CellValues>(CellValues.String);

                        Cell tipocriticidad3 = GetCell(worksheetPart1.Worksheet, "C", 44);
                        string tipocriticidad = "";
                        switch (actualizar.tipocriticidad_sec3)
                        {
                            case 1:
                                tipocriticidad = "Iniciativa pública/Public";
                                break;
                            case 2:
                                tipocriticidad = "Iniciativa privada/Private iniciative";
                                break;
                            case 3:
                                tipocriticidad = "Medida administrativa/Administrative Measure";
                                break;
                            case 4:
                                tipocriticidad = "Carta de apercibimiento/Warning letter";
                                break;
                            case 5:
                                tipocriticidad = "Otro/Other";
                                break;
                        }
                        tipocriticidad3.CellValue = new CellValue(tipocriticidad.ToString().Replace("&", "&amp;"));
                        tipocriticidad3.DataType = new EnumValue<CellValues>(CellValues.String);
                    }

                    WorksheetPart worksheetPart2 = GetWorksheetPartByName(document, "Fotos adjuntas");

                    if (worksheetPart2 != null)
                    {
                        List<evento_ambiental_foto> listaFotosEvAmb = Datos.ListarFotosEventoAmb(id);
                        int idFoto = 0;
                        foreach (evento_ambiental_foto foto in listaFotosEvAmb)
                        {
                            string directoriofotos = foto.enlace;
                            ImagePart imagePart = worksheetPart2.DrawingsPart.ImageParts.ElementAt(idFoto);

                            foreach (ImagePart imgPart in worksheetPart2.DrawingsPart.ImageParts)
                            {
                                string PartID = worksheetPart2.DrawingsPart.GetIdOfPart(imgPart);

                                if (PartID.Replace("rId", "") == (idFoto + 1).ToString())
                                {
                                    imagePart = imgPart;
                                }
                            }

                            if (System.IO.File.Exists(directoriofotos))
                            {
                                try
                                {
                                    var newImageBytes = System.IO.File.ReadAllBytes(directoriofotos); // however the image is generated or obtained

                                    using (var writer = new BinaryWriter(imagePart.GetStream()))
                                    {
                                        writer.Write(newImageBytes);
                                    }
                                }
                                catch
                                {
                                }
                            }

                            idFoto++;
                        }


                    }

                    document.WorkbookPart.WorksheetParts.ElementAt(0).Worksheet.Save();
                    document.WorkbookPart.WorksheetParts.ElementAt(1).Worksheet.Save();
                    document.WorkbookPart.Workbook.Save();
                }

                
                    Session["nombreArchivo"] = destinationFile;

                    List<evento_ambiental_tipo> eventipos = Datos.GetDatosEventoAmbTipos();
                    ViewData["tiposeventoamb"] = eventipos;
                    List<evento_ambiental_matriz> evenmatrices = Datos.GetDatosEventoAmbMatrices();
                    ViewData["matriceseventoamb"] = evenmatrices;
                    even = Datos.GetDatosEventoAmb(id);
                    ViewData["eventoambiental"] = even;
                    ViewData["documentoseventoamb"] = Datos.ListarDocumentosEventoAmb(id);
                    ViewData["fotoseventoamb"] = Datos.ListarFotosEventoAmb(id);
                    ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 13, id);
                    return Redirect(Url.RouteUrl(new { controller = "Comunicacion", action = "detalle_evento_amb", id = id }));
                #endregion imprimir
            }
            else
            {
                #region recarga
                List<evento_ambiental_tipo> eventipos = Datos.GetDatosEventoAmbTipos();
                ViewData["tiposeventoamb"] = eventipos;
                List<evento_ambiental_matriz> evenmatrices = Datos.GetDatosEventoAmbMatrices();
                ViewData["matriceseventoamb"] = evenmatrices;
                even = Datos.GetDatosEventoAmb(id);
                ViewData["eventoambiental"] = even;
                ViewData["documentoseventoamb"] = Datos.ListarDocumentosEventoAmb(id);
                ViewData["fotoseventoamb"] = Datos.ListarFotosEventoAmb(id);
                ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 13, id);
                return View();
                #endregion
            }
        }

        public ActionResult detalle_evento_seg(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            Session["modulo"] = "15";

            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            int idCentral = centroseleccionado.id;
            Session["ReferenciaAccionMejora"] = id;

            List<evento_seguridad_tipo> eventipos = Datos.GetDatosEventoSegTipos();
            ViewData["tiposeventoseg"] = eventipos;
            List<evento_seguridad_severidad> evenseveridades = Datos.GetDatosEventoSegSeveridades();
            ViewData["severidadeseventoseg"] = evenseveridades;

            List<evento_seguridad_tipoevento> eventipoeven = Datos.GetDatosEventoSegTiposEven();
            ViewData["tiposeveneventoeventoseg"] = eventipoeven;
            List<evento_seguridad_subtipoevento> evensubtipoeven = Datos.GetDatosEventoSegSubtiposEven();
            ViewData["subtiposeveneventoeventoseg"] = evensubtipoeven;

            evento_seguridad even = Datos.GetDatosEventoSeg(id);
            ViewData["eventoseguridad"] = even;

            ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 15, id);
            ViewData["fotoseventoseg"] = Datos.ListarFotosEventoSeg(id);

            #region generacion fichero

            if (Session["nombreArchivo"] != null && Session["nombreArchivo"].ToString() != "")
            {
                if (Session["nombreArchivo"].ToString().Contains("docx"))
                {
                    Response.ContentType = "application/msword";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=FichaEventoSeg.docx");
                    Response.TransmitFile(Session["nombreArchivo"].ToString());
                    Response.End();
                }

                if (Session["nombreArchivo"].ToString().Contains("xlsx"))
                {
                    Response.ContentType = "application/msexcel";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=RegistroEvento.xlsx");
                    Response.TransmitFile(Session["nombreArchivo"].ToString());
                    Response.End();
                }
                Session["nombreArchivo"] = "";
            }

            #endregion

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult detalle_evento_seg(int id, FormCollection collection, HttpPostedFileBase [] file)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }
            string formulario = collection["hdFormularioEjecutado"];

            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            int idCentral = centroseleccionado.id;

            if (formulario == "GuardarEventoSeg")
            {
                try
                {

                    if (id != 0)
                    {
                        #region actualizar
                        evento_seguridad actualizar = Datos.GetDatosEventoSeg(id);

                        if (collection["ctl00$MainContent$txtFechaEvento"] != "")
                            actualizar.fecha = DateTime.Parse(collection["ctl00$MainContent$txtFechaEvento"]);
                        actualizar.tipo = int.Parse(collection["ctl00$MainContent$ddlTipo"]);
                        actualizar.severidad = int.Parse(collection["ctl00$MainContent$ddlSeveridad"]);
                        actualizar.personalafectado = int.Parse(collection["ctl00$MainContent$ddlPersonalAfectado"]);
                        actualizar.organizacion = int.Parse(collection["ctl00$MainContent$ddlOrganizacion"]);
                        if (collection["ctl00$MainContent$txtHora"] != "")
                            actualizar.hora = DateTime.Parse(collection["ctl00$MainContent$txtHora"]).TimeOfDay;
                        actualizar.unidadnegocio = collection["ctl00$MainContent$txtUnidadNegocio"];
                        actualizar.idcentral = int.Parse(collection["ctl00$MainContent$ddlCentral"]);
                        actualizar.compania = collection["ctl00$MainContent$txtCompENEL"];
                        actualizar.baja = int.Parse(collection["ctl00$MainContent$ddlBaja"]);
                        if (collection["ctl00$MainContent$txtFechaBaja"] != "")
                            actualizar.fechabaja = DateTime.Parse(collection["ctl00$MainContent$txtFechaBaja"]);
                        if (collection["ctl00$MainContent$txtHoraJornada"] != "")
                            actualizar.horaaccidente = DateTime.Parse(collection["ctl00$MainContent$txtHoraJornada"]).TimeOfDay;

                        if (actualizar != null && actualizar.id != 0)
                        {
                            actualizar.ie_region = collection["ctl00$MainContent$txtRegion_ie"];
                            actualizar.ie_localizacion = collection["ctl00$MainContent$txtLocalizacion_ie"];
                            actualizar.ie_descripcion = collection["ctl00$MainContent$txtDescripcion_ie"];

                            //actualizar.pa_nombre = collection["ctl00$MainContent$txtNombre_pa"];
                            //actualizar.pa_apellido = collection["ctl00$MainContent$txtApellido_pa"];
                            actualizar.pa_genero = int.Parse(collection["ctl00$MainContent$ddlGenero_pa"]);
                            if (collection["ctl00$MainContent$txtEdad_pa"] != null && collection["ctl00$MainContent$txtEdad_pa"] != string.Empty)
                                actualizar.pa_edad = int.Parse(collection["ctl00$MainContent$txtEdad_pa"].ToString());
                            actualizar.pa_puesto = collection["ctl00$MainContent$txtPuesto_pa"];
                            actualizar.pa_nacionalidad = collection["ctl00$MainContent$txtNacionalidad_pa"];
                            actualizar.pa_antiguedadempresa = collection["ctl00$MainContent$txtAntiguedadEmpresa"];
                            actualizar.pa_antiguedadcategoria = collection["ctl00$MainContent$txtAntiguedadCategoria"];
                            actualizar.pa_tipocontrato = int.Parse(collection["ctl00$MainContent$ddlContrato"]);
                            actualizar.pa_accidentesant = int.Parse(collection["ctl00$MainContent$ddlAccidentesAnt"]);
                            if (collection["ctl00$MainContent$txtNumAccidentes"] != null && collection["ctl00$MainContent$txtNumAccidentes"] != string.Empty)
                                actualizar.pa_numacc = int.Parse(collection["ctl00$MainContent$txtNumAccidentes"].ToString());
                            if (collection["ctl00$MainContent$txtDiasBaja"] != null && collection["ctl00$MainContent$txtDiasBaja"] != string.Empty)
                                actualizar.pa_diasbaja = int.Parse(collection["ctl00$MainContent$txtDiasBaja"].ToString());
                            if (collection["ctl00$MainContent$txtFechaUltimoModulo"] != "")
                                actualizar.pa_fechamodformacion = DateTime.Parse(collection["ctl00$MainContent$txtFechaUltimoModulo"]);
                            actualizar.pa_nombreultformacion = collection["ctl00$MainContent$txtNombreUltimaForm"];

                            actualizar.id_informemedico = collection["ctl00$MainContent$txtInformeMedico_id"];
                            actualizar.id_diasconv_primerpron = collection["ctl00$MainContent$txtDiasConvalescencia_id"];
                            actualizar.id_reqasistencia = int.Parse(collection["ctl00$MainContent$ddlAsistenciaEndesa"]);
                            actualizar.id_asistenciaen = collection["ctl00$MainContent$txtAsistenciaEn"];
                            actualizar.id_personalsanitario = collection["ctl00$MainContent$txtPersonalSanitario"];
                            actualizar.id_naturalezalesion = collection["ctl00$MainContent$txtNaturalezaLesion"];
                            actualizar.id_localizacionanatomica = collection["ctl00$MainContent$txtLocalizacionAnatomica"];
                            actualizar.id_agentelesion = collection["ctl00$MainContent$txtAgenteLesion"];
                            actualizar.id_envmutua = int.Parse(collection["ctl00$MainContent$ddlEnvioMutua"]);
                            actualizar.id_mutua = collection["ctl00$MainContent$txtNombreMutua"];
                            actualizar.id_localidadmutua = collection["ctl00$MainContent$txtLocalidadMutua"];
                            actualizar.id_envcs = int.Parse(collection["ctl00$MainContent$ddlEnvioCS"]);
                            actualizar.id_centrosanitario = collection["ctl00$MainContent$txtNombreCS"];
                            actualizar.id_localidadcs = collection["ctl00$MainContent$txtLocalidadCS"];
                            actualizar.id_mandodirecto = collection["ctl00$MainContent$txtMandoDirecto"];
                            actualizar.id_testigo1 = collection["ctl00$MainContent$txtTestigo1"];
                            actualizar.id_testigo2 = collection["ctl00$MainContent$txtTestigo2"];
                            actualizar.id_testigo3 = collection["ctl00$MainContent$txtTestigo3"];

                            actualizar.te_tipo = int.Parse(collection["ctl00$MainContent$ddlTipoEvento_te"]);
                            actualizar.te_subtipo = int.Parse(collection["ctl00$MainContent$ddlSubtipoEvento_te"]);
                            actualizar.te_categorizacion = int.Parse(collection["ctl00$MainContent$ddlCategorizacion_te"]);
                            actualizar.te_causa = collection["ctl00$MainContent$txtCausa_te"];
                            actualizar.te_accionesinm = collection["ctl00$MainContent$txtAccionesInm_te"];

                            actualizar.ic_nombreempresa = collection["ctl00$MainContent$txtNombreEmpresa_ic"];
                            actualizar.ic_actividad = collection["ctl00$MainContent$txtActividad_ic"];
                            actualizar.ic_personaref = collection["ctl00$MainContent$txtPersonaRef_ic"];
                            actualizar.ic_telefono = collection["ctl00$MainContent$txtTelefono_ic"];
                            actualizar.ic_email = collection["ctl00$MainContent$txtEmail_ic"];
                            if (collection["ctl00$MainContent$ddlContratista_ic"] != null)
                                actualizar.ic_empcontratista = int.Parse(collection["ctl00$MainContent$ddlContratista_ic"].ToString());
                            actualizar.ic_subcontrata = collection["ctl00$MainContent$txtContratistaPrincipal_ic"];
                            actualizar.ic_personalsanit = collection["ctl00$MainContent$txtPersonalSanitario_ic"];
                            actualizar.ic_domicilio = collection["ctl00$MainContent$txtDomicilio_ic"];
                            actualizar.ic_cif = collection["ctl00$MainContent$txtCIF_ic"];
                            actualizar.ic_localidad = collection["ctl00$MainContent$txtLocalidad_ic"];

                            actualizar.ia_horario = collection["ctl00$MainContent$txtHorario_ia"];
                            actualizar.ia_desde = collection["ctl00$MainContent$txtDesde_ia"];
                            actualizar.ia_hacia = collection["ctl00$MainContent$txtHacia_ia"];
                            actualizar.ia_lugar = collection["ctl00$MainContent$txtLugar_ia"];
                            actualizar.ia_medio = collection["ctl00$MainContent$txtMedio_ia"];
                            actualizar.ia_propiedadmediotransporte = collection["ctl00$MainContent$txtPropiedad_ia"];
                            actualizar.ia_causa = collection["ctl00$MainContent$txtCausa_ia"];
                        }

                        Datos.ActualizarEventoSeg(actualizar);

                        Session["EdicionEventoSegMensaje"] = "Información actualizada correctamente";

                        List<evento_seguridad_tipo> eventipos = Datos.GetDatosEventoSegTipos();
                        ViewData["tiposeventoseg"] = eventipos;
                        List<evento_seguridad_severidad> evenseveridades = Datos.GetDatosEventoSegSeveridades();
                        ViewData["severidadeseventoseg"] = evenseveridades;

                        List<evento_seguridad_tipoevento> eventipoeven = Datos.GetDatosEventoSegTiposEven();
                        ViewData["tiposeveneventoeventoseg"] = eventipoeven;
                        List<evento_seguridad_subtipoevento> evensubtipoeven = Datos.GetDatosEventoSegSubtiposEven();
                        ViewData["subtiposeveneventoeventoseg"] = evensubtipoeven;

                        evento_seguridad even = Datos.GetDatosEventoSeg(id);
                        ViewData["eventoseguridad"] = even;
                        ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 15, id);
                        ViewData["fotoseventoseg"] = Datos.ListarFotosEventoSeg(id);
                        return View();
                        #endregion
                    }
                    else
                    {
                        #region insertar
                        evento_seguridad insertar = new evento_seguridad();
                        if (collection["ctl00$MainContent$txtFechaEvento"] != "")
                            insertar.fecha = DateTime.Parse(collection["ctl00$MainContent$txtFechaEvento"]);
                        insertar.tipo = int.Parse(collection["ctl00$MainContent$ddlTipo"]);
                        insertar.severidad = int.Parse(collection["ctl00$MainContent$ddlSeveridad"]);
                        insertar.personalafectado = int.Parse(collection["ctl00$MainContent$ddlPersonalAfectado"]);
                        insertar.organizacion = int.Parse(collection["ctl00$MainContent$ddlOrganizacion"]);
                        if (collection["ctl00$MainContent$txtHora"] != "")
                            insertar.hora = DateTime.Parse(collection["ctl00$MainContent$txtHora"]).TimeOfDay;
                        insertar.unidadnegocio = collection["ctl00$MainContent$txtUnidadNegocio"];
                        insertar.idcentral = int.Parse(collection["ctl00$MainContent$ddlCentral"]);
                        insertar.compania = collection["ctl00$MainContent$txtCompENEL"];
                                                
                        int idCom = Datos.ActualizarEventoSeg(insertar);

                        Session["EdicionEventoAmbMensaje"] = "Información actualizada correctamente";

                        List<evento_seguridad_tipo> eventipos = Datos.GetDatosEventoSegTipos();
                        ViewData["tiposeventoseg"] = eventipos;
                        List<evento_seguridad_severidad> evenseveridades = Datos.GetDatosEventoSegSeveridades();
                        ViewData["severidadeseventoseg"] = evenseveridades;

                        List<evento_seguridad_tipoevento> eventipoeven = Datos.GetDatosEventoSegTiposEven();
                        ViewData["tiposeveneventoeventoseg"] = eventipoeven;
                        List<evento_seguridad_subtipoevento> evensubtipoeven = Datos.GetDatosEventoSegSubtiposEven();
                        ViewData["subtiposeveneventoeventoseg"] = evensubtipoeven;

                        evento_seguridad even = Datos.GetDatosEventoSeg(id);
                        ViewData["eventoseguridad"] = even;
                        ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 15, id);
                        ViewData["fotoseventoseg"] = Datos.ListarFotosEventoSeg(id);
                        return Redirect(Url.RouteUrl(new { controller = "Comunicacion", action = "detalle_evento_seg", id = idCom }));
                        #endregion
                    }
                }
                catch (Exception ex)
                {
                    #region exception
                    Session["EdicionEventoSegError"] = "Se ha producido un error, compruebe los datos introducidos.";

                    List<evento_seguridad_tipo> eventipos = Datos.GetDatosEventoSegTipos();
                    ViewData["tiposeventoseg"] = eventipos;
                    List<evento_seguridad_severidad> evenseveridades = Datos.GetDatosEventoSegSeveridades();
                    ViewData["severidadeseventoseg"] = evenseveridades;

                    List<evento_seguridad_tipoevento> eventipoeven = Datos.GetDatosEventoSegTiposEven();
                    ViewData["tiposeveneventoeventoseg"] = eventipoeven;
                    List<evento_seguridad_subtipoevento> evensubtipoeven = Datos.GetDatosEventoSegSubtiposEven();
                    ViewData["subtiposeveneventoeventoseg"] = evensubtipoeven;

                    evento_seguridad even = Datos.GetDatosEventoSeg(id);
                    ViewData["eventoseguridad"] = even;
                    ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 15, id);
                    return View();
                    #endregion
                }
            }

            if (formulario == "SubirDocumento")
            {
                #region subirDocumento
                if (collection["ctl00$MainContent$txtNombreFichero"] != null && collection["ctl00$MainContent$txtNombreFichero"] != "")
                {

                    if (file[0] != null && file[0].ContentLength > 0)
                    {
                        evento_seguridad_foto datostec = new evento_seguridad_foto();

                        var fileName = System.IO.Path.GetFileName(file[0].FileName);

                        datostec.idEventoSeg = id;
                        datostec.nombre = collection["ctl00$MainContent$txtNombreFichero"];
                        datostec.nombre_fichero = fileName;

                        var path = System.IO.Path.Combine(Server.MapPath("~/EventosSeguridad/" + id.ToString()), fileName);
                        datostec.enlace = path;

                        int idFichero = Datos.InsertFotoEventoSeguridad(datostec);

                        if (Directory.Exists(Server.MapPath("~/EventosSeguridad/" + id)))
                        {
                            file[0].SaveAs(path);
                        }
                        else
                        {
                            Directory.CreateDirectory(Server.MapPath("~/EventosSeguridad/" + id));
                            file[0].SaveAs(path);
                        }

                        Session["EdicionEventoSegMensaje"] = "Foto agregada correctamente.";
                    }
                }
                else
                {
                    Session["EdicionEventoSegError"] = "Los campos marcados con (*) son obligatorios.";
                }

                List<evento_seguridad_tipo> eventipos = Datos.GetDatosEventoSegTipos();
                ViewData["tiposeventoseg"] = eventipos;
                List<evento_seguridad_severidad> evenseveridades = Datos.GetDatosEventoSegSeveridades();
                ViewData["severidadeseventoseg"] = evenseveridades;

                List<evento_seguridad_tipoevento> eventipoeven = Datos.GetDatosEventoSegTiposEven();
                ViewData["tiposeveneventoeventoseg"] = eventipoeven;
                List<evento_seguridad_subtipoevento> evensubtipoeven = Datos.GetDatosEventoSegSubtiposEven();
                ViewData["subtiposeveneventoeventoseg"] = evensubtipoeven;

                evento_seguridad even = Datos.GetDatosEventoSeg(id);
                ViewData["eventoseguridad"] = even;
                ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 15, id);
                ViewData["fotoseventoseg"] = Datos.ListarFotosEventoSeg(id);
                return View();
            }

            #endregion

            if (formulario == "SubirArchivo")
            {
                #region subirArchivo
                if (collection["ctl00$MainContent$DocumentoName"] != null && collection["ctl00$MainContent$DocumentoName"] != "")
                {

                    if (file[1] != null && file[1].ContentLength > 0)
                    {
                        evento_seguridad_documentos datosArchivo = new evento_seguridad_documentos();

                        var fileName = System.IO.Path.GetFileName(file[1].FileName);

                        datosArchivo.idSeg = id;
                        datosArchivo.nombre = collection["ctl00$MainContent$DocumentoName"];
                        datosArchivo.nombrefichero = fileName;

                        var path = System.IO.Path.Combine(Server.MapPath("~/EventosSeguridad/" + id.ToString()), fileName);
                        datosArchivo.enlace = path;
                        datosArchivo.fecha = DateTime.Now.Date;

                        int idFichero = Datos.InserDocEventoSeguridad(datosArchivo);

                        if (Directory.Exists(Server.MapPath("~/EventosSeguridad/" + id)))
                        {
                            file[1].SaveAs(path);
                        }
                        else
                        {
                            Directory.CreateDirectory(Server.MapPath("~/EventosSeguridad/" + id));
                            file[1].SaveAs(path);
                        }

                        Session["EdicionEventoSegMensaje"] = "Documentación agregada correctamente.";
                    }
                }
                else
                {
                    Session["EdicionEventoSegError"] = "Los campos marcados con (*) son obligatorios.";
                }

                List<evento_seguridad_tipo> eventipos = Datos.GetDatosEventoSegTipos();
                ViewData["tiposeventoseg"] = eventipos;
                List<evento_seguridad_severidad> evenseveridades = Datos.GetDatosEventoSegSeveridades();
                ViewData["severidadeseventoseg"] = evenseveridades;

                List<evento_seguridad_tipoevento> eventipoeven = Datos.GetDatosEventoSegTiposEven();
                ViewData["tiposeveneventoeventoseg"] = eventipoeven;
                List<evento_seguridad_subtipoevento> evensubtipoeven = Datos.GetDatosEventoSegSubtiposEven();
                ViewData["subtiposeveneventoeventoseg"] = evensubtipoeven;

                evento_seguridad even = Datos.GetDatosEventoSeg(id);
                ViewData["eventoseguridad"] = even;
                ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 15, id);
                ViewData["fotoseventoseg"] = Datos.ListarFotosEventoSeg(id);
                ViewData["eventosegdocs"] = Datos.ListarArchivosEventoSeg(id);

                return View();
                #endregion
            }

            if (formulario == "btnImprimirReg")
            {
                VISTA_ObtenerUsuario user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());

                evento_seguridad actualizar = Datos.GetDatosEventoSeg(id);

                #region generacion fichero
                string sourceFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaPlantillaWord"] + "RegistroEventos.xlsx");
                string destinationFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaGeneracionWord"] + "RegistroEventos_" + DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + ".xlsx");

                Session["source"] = sourceFile;
                Session["destino"] = destinationFile;
                // Create a copy of the template file and open the copy 
                System.IO.File.Copy(sourceFile, destinationFile, true);

                #region impresion

                using (SpreadsheetDocument document = SpreadsheetDocument.Open(destinationFile, true))
                {
                    WorksheetPart worksheetPart = GetWorksheetPartByName(document, "Event Communication Form");
                    if (worksheetPart != null)
                    {

                            Row row = new Row();
                            #region inserción campos en excel
                            if (actualizar.tipo != null)
                            {
                                evento_seguridad_tipo tipoevento = Datos.GetDatosEventoSegTipos().Where(x=>x.id==actualizar.tipo).First();
                                Cell tipoeven = GetCell(worksheetPart.Worksheet, "D", 6);
                                tipoeven.CellValue = new CellValue(tipoevento.tipo);
                                tipoeven.DataType = new EnumValue<CellValues>(CellValues.String);
                            }

                            if (actualizar.severidad != null)
                            {
                                evento_seguridad_severidad severidad = Datos.GetDatosEventoSegSeveridades().Where(x => x.id == actualizar.severidad).First(); ;
                                Cell sever = GetCell(worksheetPart.Worksheet, "D", 7);
                                sever.CellValue = new CellValue(severidad.severidad);
                                sever.DataType = new EnumValue<CellValues>(CellValues.String);
                            }

                            if (actualizar.personalafectado != null)
                            {
                                Cell persafectado = GetCell(worksheetPart.Worksheet, "D", 8);
                                switch (actualizar.personalafectado)
                                {
                                    case 1:
                                        persafectado.CellValue = new CellValue("Empleado Enel");
                                        break;
                                    case 2:
                                        persafectado.CellValue = new CellValue("Empleado Contratista");
                                        break;
                                    case 3:
                                        persafectado.CellValue = new CellValue("Tercera Parte");
                                        break;
                                }                                
                                persafectado.DataType = new EnumValue<CellValues>(CellValues.String);
                            }

                            if (actualizar.organizacion != null)
                            {
                                Cell org = GetCell(worksheetPart.Worksheet, "D", 9);
                                switch (actualizar.organizacion)
                                {
                                    case 1:
                                        org.CellValue = new CellValue("O&M: Operación y Mantenimiento");
                                        break;
                                    case 2:
                                        org.CellValue = new CellValue("E&C: Ingeniería y Construcción");
                                        break;
                                }
                                org.DataType = new EnumValue<CellValues>(CellValues.String);
                            }                            

                            if (actualizar.fecha != null)
                            {
                                Cell fecha = GetCell(worksheetPart.Worksheet, "D", 11);
                                fecha.CellValue = new CellValue(actualizar.fecha.ToString().Replace(" 0:00:00", ""));
                                fecha.DataType = new EnumValue<CellValues>(CellValues.String);
                            }

                            if (actualizar.hora != null)
                            {
                                Cell hora = GetCell(worksheetPart.Worksheet, "D", 12);
                                hora.CellValue = new CellValue(actualizar.hora.ToString());
                                hora.DataType = new EnumValue<CellValues>(CellValues.String);
                            }                                                   

                            if (actualizar.idcentral != null)
                            {                                
                                Cell centro = GetCell(worksheetPart.Worksheet, "D", 14);
                                centro.CellValue = new CellValue(centroseleccionado.nombre);
                                centro.DataType = new EnumValue<CellValues>(CellValues.String);
                            }

                            if (actualizar.idcentral != null)
                            {
                                tipocentral tipocent = Datos.ListarTecnologia(centroseleccionado.tipo);
                                Cell centro = GetCell(worksheetPart.Worksheet, "D", 15);
                                centro.CellValue = new CellValue(tipocent.nombre);
                                centro.DataType = new EnumValue<CellValues>(CellValues.String);
                            }

                            if (actualizar.ie_region != null)
                            {
                                Cell celda = GetCell(worksheetPart.Worksheet, "D", 18);
                                celda.CellValue = new CellValue(actualizar.ie_region);
                                celda.DataType = new EnumValue<CellValues>(CellValues.String);
                            }

                            if (actualizar.ie_localizacion != null)
                            {
                                Cell celda = GetCell(worksheetPart.Worksheet, "D", 19);
                                celda.CellValue = new CellValue(actualizar.ie_localizacion);
                                celda.DataType = new EnumValue<CellValues>(CellValues.String);
                            }

                            if (actualizar.ie_descripcion != null)
                            {
                                Cell celda = GetCell(worksheetPart.Worksheet, "D", 20);
                                celda.CellValue = new CellValue(actualizar.ie_descripcion);
                                celda.DataType = new EnumValue<CellValues>(CellValues.String);
                            }

                            if (actualizar.pa_nombre != null)
                            {
                                Cell celda = GetCell(worksheetPart.Worksheet, "D", 22);
                                celda.CellValue = new CellValue(actualizar.pa_nombre);
                                celda.DataType = new EnumValue<CellValues>(CellValues.String);
                            }

                            if (actualizar.pa_genero != null)
                            {
                                Cell celda = GetCell(worksheetPart.Worksheet, "D", 24);
                                switch (actualizar.pa_genero)
                                {
                                    case 1:
                                        celda.CellValue = new CellValue("Masculino/Male");
                                        break;
                                    case 2:
                                        celda.CellValue = new CellValue("Femenino/Female");
                                        break;
                                }
                                celda.DataType = new EnumValue<CellValues>(CellValues.String);
                            }

                            if (actualizar.pa_edad != null)
                            {
                                Cell celda = GetCell(worksheetPart.Worksheet, "D", 25);
                                celda.CellValue = new CellValue(actualizar.pa_edad.ToString());
                                celda.DataType = new EnumValue<CellValues>(CellValues.String);
                            }

                            if (actualizar.pa_puesto != null)
                            {
                                Cell celda = GetCell(worksheetPart.Worksheet, "D", 26);
                                celda.CellValue = new CellValue(actualizar.pa_puesto);
                                celda.DataType = new EnumValue<CellValues>(CellValues.String);
                            }

                            if (actualizar.pa_nacionalidad != null)
                            {
                                Cell celda = GetCell(worksheetPart.Worksheet, "D", 27);
                                celda.CellValue = new CellValue(actualizar.pa_nacionalidad);
                                celda.DataType = new EnumValue<CellValues>(CellValues.String);
                            }

                            if (actualizar.id_informemedico != null)
                            {
                                Cell celda = GetCell(worksheetPart.Worksheet, "D", 29);
                                celda.CellValue = new CellValue(actualizar.id_informemedico);
                                celda.DataType = new EnumValue<CellValues>(CellValues.String);
                            }

                            if (actualizar.id_diasconv_primerpron != null)
                            {
                                Cell celda = GetCell(worksheetPart.Worksheet, "D", 30);
                                celda.CellValue = new CellValue(actualizar.id_diasconv_primerpron);
                                celda.DataType = new EnumValue<CellValues>(CellValues.String);
                            }

                            if (actualizar.te_tipo != null)
                            {
                                Cell celda = GetCell(worksheetPart.Worksheet, "D", 32);
                                switch (actualizar.te_tipo)
                                {
                                    case 1:
                                        celda.CellValue = new CellValue("Mecánico");
                                        break;
                                    case 2:
                                        celda.CellValue = new CellValue("Físico");
                                        break;
                                    case 3:
                                        celda.CellValue = new CellValue("Eléctrico");
                                        break;
                                    case 4:
                                        celda.CellValue = new CellValue("Químico");
                                        break;
                                    case 5:
                                        celda.CellValue = new CellValue("Ergonómico");
                                        break;
                                    case 6:
                                        celda.CellValue = new CellValue("Viaje");
                                        break;
                                    case 7:
                                        celda.CellValue = new CellValue("Otros");
                                        break;
                                }
                                celda.DataType = new EnumValue<CellValues>(CellValues.String);
                            }

                            if (actualizar.te_subtipo != null)
                            {
                                Cell celda = GetCell(worksheetPart.Worksheet, "D", 33);
                                switch (actualizar.te_subtipo)
                                {
                                    case 1:
                                        celda.CellValue = new CellValue("Impacto con objetos");
                                        break;
                                    case 2:
                                        celda.CellValue = new CellValue("Atrapamiento");
                                        break;
                                    case 3:
                                        celda.CellValue = new CellValue("Cortes");
                                        break;
                                    case 4:
                                        celda.CellValue = new CellValue("Proyecciones");
                                        break;
                                    case 5:
                                        celda.CellValue = new CellValue("Caída de objetos");
                                        break;
                                    case 6:
                                        celda.CellValue = new CellValue("Caída a distinto nivel");
                                        break;
                                    case 7:
                                        celda.CellValue = new CellValue("Caída al mismo nivel");
                                        break;
                                    case 8:
                                        celda.CellValue = new CellValue("Viaje");
                                        break;
                                    case 9:
                                        celda.CellValue = new CellValue("Eléctrico");
                                        break;
                                    case 10:
                                        celda.CellValue = new CellValue("Agentes químicos");
                                        break;
                                    case 11:
                                        celda.CellValue = new CellValue("Contacto térmico");
                                        break;
                                    case 12:
                                        celda.CellValue = new CellValue("Estrés térmico");
                                        break;
                                    case 13:
                                        celda.CellValue = new CellValue("Fuego");
                                        break;
                                    case 14:
                                        celda.CellValue = new CellValue("Explosión");
                                        break;
                                }
                                celda.DataType = new EnumValue<CellValues>(CellValues.String);
                            }

                            if (actualizar.te_categorizacion != null)
                            {
                                Cell celda = GetCell(worksheetPart.Worksheet, "D", 34);
                                switch (actualizar.te_categorizacion)
                                {
                                    case 1:
                                        celda.CellValue = new CellValue("Estructuras");
                                        break;
                                    case 2:
                                        celda.CellValue = new CellValue("Comportamientos");
                                        break;
                                    case 3:
                                        celda.CellValue = new CellValue("Organización");
                                        break;
                                    case 4:
                                        celda.CellValue = new CellValue("Otros factores");
                                        break;
                                }
                                celda.DataType = new EnumValue<CellValues>(CellValues.String);
                            }

                            if (actualizar.te_causa != null)
                            {
                                Cell celda = GetCell(worksheetPart.Worksheet, "D", 35);
                                celda.CellValue = new CellValue(actualizar.te_causa);
                                celda.DataType = new EnumValue<CellValues>(CellValues.String);
                            }

                            if (actualizar.te_accionesinm != null)
                            {
                                Cell celda = GetCell(worksheetPart.Worksheet, "D", 36);
                                celda.CellValue = new CellValue(actualizar.te_accionesinm);
                                celda.DataType = new EnumValue<CellValues>(CellValues.String);
                            }

                            if (actualizar.ic_nombreempresa != null)
                            {
                                Cell celda = GetCell(worksheetPart.Worksheet, "D", 38);
                                celda.CellValue = new CellValue(actualizar.ic_nombreempresa);
                                celda.DataType = new EnumValue<CellValues>(CellValues.String);
                            }

                            if (actualizar.ic_actividad != null)
                            {
                                Cell celda = GetCell(worksheetPart.Worksheet, "D", 39);
                                celda.CellValue = new CellValue(actualizar.ic_actividad);
                                celda.DataType = new EnumValue<CellValues>(CellValues.String);
                            }

                            if (actualizar.ic_personaref != null)
                            {
                                Cell celda = GetCell(worksheetPart.Worksheet, "D", 40);
                                celda.CellValue = new CellValue(actualizar.ic_personaref);
                                celda.DataType = new EnumValue<CellValues>(CellValues.String);
                            }

                            if (actualizar.ic_telefono != null)
                            {
                                Cell celda = GetCell(worksheetPart.Worksheet, "D", 41);
                                celda.CellValue = new CellValue(actualizar.ic_telefono);
                                celda.DataType = new EnumValue<CellValues>(CellValues.String);
                            }

                            if (actualizar.ic_email != null)
                            {
                                Cell celda = GetCell(worksheetPart.Worksheet, "D", 42);
                                celda.CellValue = new CellValue(actualizar.ic_email);
                                celda.DataType = new EnumValue<CellValues>(CellValues.String);
                            }

                            if (actualizar.horaaccidente != null)
                            {
                                Cell celda = GetCell(worksheetPart.Worksheet, "D", 44);
                                celda.CellValue = new CellValue(actualizar.horaaccidente.ToString());
                                celda.DataType = new EnumValue<CellValues>(CellValues.String);
                            }

                            if (actualizar.ia_desde != null)
                            {
                                Cell celda = GetCell(worksheetPart.Worksheet, "D", 45);
                                celda.CellValue = new CellValue(actualizar.ia_desde);
                                celda.DataType = new EnumValue<CellValues>(CellValues.String);
                            }

                            if (actualizar.ia_hacia != null)
                            {
                                Cell celda = GetCell(worksheetPart.Worksheet, "D", 46);
                                celda.CellValue = new CellValue(actualizar.ia_hacia);
                                celda.DataType = new EnumValue<CellValues>(CellValues.String);
                            }

                            if (actualizar.ia_lugar != null)
                            {
                                Cell celda = GetCell(worksheetPart.Worksheet, "D", 47);
                                celda.CellValue = new CellValue(actualizar.ia_lugar);
                                celda.DataType = new EnumValue<CellValues>(CellValues.String);
                            }

                            if (actualizar.ia_medio != null)
                            {
                                Cell celda = GetCell(worksheetPart.Worksheet, "D", 48);
                                celda.CellValue = new CellValue(actualizar.ia_medio);
                                celda.DataType = new EnumValue<CellValues>(CellValues.String);
                            }

                            if (actualizar.ia_propiedadmediotransporte != null)
                            {
                                Cell celda = GetCell(worksheetPart.Worksheet, "D", 49);
                                celda.CellValue = new CellValue(actualizar.ia_propiedadmediotransporte);
                                celda.DataType = new EnumValue<CellValues>(CellValues.String);
                            }

                            if (actualizar.ia_causa != null)
                            {
                                Cell celda = GetCell(worksheetPart.Worksheet, "D", 50);
                                celda.CellValue = new CellValue(actualizar.ia_causa);
                                celda.DataType = new EnumValue<CellValues>(CellValues.String);
                            }
                            #endregion

                    }

                    WorksheetPart worksheetPart2 = GetWorksheetPartByName(document, "Pictures attachment");

                    if (worksheetPart2 != null)
                    {
                        List<evento_seguridad_foto> listaFotosEv = Datos.ListarFotosEventoSeg(id);
                        int idFoto = 0;
                        foreach (evento_seguridad_foto foto in listaFotosEv)
                        {
                            string directoriofotos = foto.enlace;
                            ImagePart imagePart = worksheetPart2.DrawingsPart.ImageParts.ElementAt(idFoto);

                            foreach (ImagePart imgPart in worksheetPart2.DrawingsPart.ImageParts)
                            {
                                string PartID = worksheetPart2.DrawingsPart.GetIdOfPart(imgPart);

                                if (PartID.Replace("rId", "") == (idFoto + 1).ToString())
                                {
                                    imagePart = imgPart;
                                }
                            }

                            if (System.IO.File.Exists(directoriofotos))
                            {
                                try
                                {
                                    var newImageBytes = System.IO.File.ReadAllBytes(directoriofotos); // however the image is generated or obtained

                                    using (var writer = new BinaryWriter(imagePart.GetStream()))
                                    {
                                        writer.Write(newImageBytes);
                                    }
                                }
                                catch
                                {
                                }
                            }

                            idFoto++;
                        }


                    }

                    // save worksheet
                    document.WorkbookPart.WorksheetParts.First().Worksheet.Save();

                    document.WorkbookPart.Workbook.Save();

                    Session["nombreArchivo"] = destinationFile;
                }
                #endregion
                #endregion

                List<evento_seguridad_tipo> eventipos = Datos.GetDatosEventoSegTipos();
                ViewData["tiposeventoseg"] = eventipos;
                List<evento_seguridad_severidad> evenseveridades = Datos.GetDatosEventoSegSeveridades();
                ViewData["severidadeseventoseg"] = evenseveridades;

                List<evento_seguridad_tipoevento> eventipoeven = Datos.GetDatosEventoSegTiposEven();
                ViewData["tiposeveneventoeventoseg"] = eventipoeven;
                List<evento_seguridad_subtipoevento> evensubtipoeven = Datos.GetDatosEventoSegSubtiposEven();
                ViewData["subtiposeveneventoeventoseg"] = evensubtipoeven;

                evento_seguridad even = Datos.GetDatosEventoSeg(id);
                ViewData["eventoseguridad"] = even;
                ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 15, id);
                ViewData["fotoseventoseg"] = Datos.ListarFotosEventoSeg(id);
                return Redirect(Url.RouteUrl(new { controller = "Comunicacion", action = "detalle_evento_seg", id = id }));
            }

            if (formulario == "btnImprimir")
            {
                #region imprimir
                string sourceFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaPlantillaWord"] + "ComIncidente.docx");
                string destinationFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaGeneracionWord"] + "ComIncidente_" + DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + ".docx");

                Session["source"] = sourceFile;
                Session["destino"] = destinationFile;
                // Create a copy of the template file and open the copy 
                System.IO.File.Copy(sourceFile, destinationFile, true);

                #region impresion
                evento_seguridad actualizar = Datos.GetDatosEventoSeg(id);


                // create key value pair, key represents words to be replace and 
                //values represent values in document in place of keys.
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                if (actualizar.ic_nombreempresa != null)
                    keyValues.Add("T_NombreCont", actualizar.ic_nombreempresa.Replace("\r\n", "<w:br/>").Replace("&","&amp;"));
                else
                    keyValues.Add("T_NombreCont", string.Empty);
                if (actualizar.pa_edad != null)
                    keyValues.Add("T_Edad", actualizar.pa_edad.ToString().Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_Edad", string.Empty);
                if (actualizar.pa_puesto != null)
                    keyValues.Add("T_Puesto", actualizar.pa_puesto.Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_Puesto", string.Empty);
                if (actualizar.pa_antiguedadempresa != null)
                    keyValues.Add("T_AntigEmpresa", actualizar.pa_antiguedadempresa.Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_AntigEmpresa", string.Empty);
                if (actualizar.pa_antiguedadcategoria != null)
                    keyValues.Add("T_AntigCategoria", actualizar.pa_antiguedadcategoria.Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_AntigCategoria", string.Empty);
                if (actualizar.pa_tipocontrato == 0)
                    keyValues.Add("T_TipoContrato", "Fijo");
                else
                    keyValues.Add("T_TipoContrato", "Temporal");
                if (actualizar.pa_accidentesant == 0)
                    keyValues.Add("T_AccidenteAnt", "No");
                else
                    keyValues.Add("T_AccidenteAnt", "Sí");
                if (actualizar.pa_numacc != null)
                    keyValues.Add("T_Cuantos", actualizar.pa_numacc.ToString().Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_Cuantos", string.Empty);
                if (actualizar.pa_diasbaja != null)
                    keyValues.Add("T_DiasBaja", actualizar.pa_diasbaja.ToString().Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_DiasBaja", string.Empty);
                if (actualizar.pa_fechamodformacion != null)
                    keyValues.Add("T_UltimoMod", actualizar.pa_fechamodformacion.ToString().Replace(" 0:00:00", "").Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_UltimoMod", "");
                if (actualizar.pa_nombreultformacion != null)
                    keyValues.Add("T_NombreUltMod", actualizar.pa_nombreultformacion.Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_NombreUltMod", string.Empty);

                if (actualizar.ie_descripcion != null)
                    keyValues.Add("T_DescripcionEvento", actualizar.ie_descripcion.Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_DescripcionEvento", string.Empty);
                if (actualizar.ia_lugar != null)
                    keyValues.Add("T_LugarAccidente", actualizar.ia_lugar.Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_LugarAccidente", string.Empty);
                if (actualizar.ie_localizacion != null)
                    keyValues.Add("T_Localizacion", actualizar.ie_localizacion.Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_Localizacion", string.Empty);
                if (actualizar.fecha != null)
                    keyValues.Add("T_FechaEvento", actualizar.fecha.ToString().Replace(" 0:00:00", "").Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_FechaEvento", "");
                if (actualizar.hora != null)
                    keyValues.Add("T_Hora", actualizar.hora.ToString());
                else
                    keyValues.Add("T_Hora", string.Empty);
                if (actualizar.horaaccidente != null)
                    keyValues.Add("T_JornadaHora", actualizar.horaaccidente.ToString());
                else
                    keyValues.Add("T_JornadaHora", string.Empty);
                if (actualizar.fechabaja != null)
                    keyValues.Add("T_FechaBaja", actualizar.fechabaja.ToString().Replace(" 0:00:00", "").Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_FechaBaja", "");

                if (actualizar.id_informemedico != null)
                    keyValues.Add("T_InformeMedico", actualizar.id_informemedico.Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_InformeMedico", string.Empty);
                if (actualizar.id_diasconv_primerpron != null)
                    keyValues.Add("T_Pronostico", actualizar.id_diasconv_primerpron.Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_Pronostico", string.Empty);
                if (actualizar.id_reqasistencia == 0)
                    keyValues.Add("T_AsistEndesa", "No");
                else
                    keyValues.Add("T_AsistEndesa", "Sí");
                if (actualizar.id_asistenciaen != null)
                    keyValues.Add("T_AsistenciaEn", actualizar.id_asistenciaen.Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_AsistenciaEn", string.Empty);
                if (actualizar.id_personalsanitario != null)
                    keyValues.Add("T_PersonalSanitario", actualizar.id_personalsanitario.Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_PersonalSanitario", string.Empty);

                if (actualizar.id_naturalezalesion != null)
                    keyValues.Add("T_NaturalezaLesion", actualizar.id_naturalezalesion.Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_NaturalezaLesion", string.Empty);
                if (actualizar.id_localizacionanatomica != null)
                    keyValues.Add("T_LocAnatomica", actualizar.id_localizacionanatomica.Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_LocAnatomica", string.Empty);
                if (actualizar.id_agentelesion != null)
                    keyValues.Add("T_AgenteLesion", actualizar.id_agentelesion.Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_AgenteLesion", string.Empty);
                if (actualizar.id_envmutua == 0)
                    keyValues.Add("T_EnvMutua", "No");
                else
                    keyValues.Add("T_EnvMutua", "Sí");
                if (actualizar.id_localidadmutua != null)
                    keyValues.Add("T_LocalidadMutua", actualizar.id_localidadmutua.Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_LocalidadMutua", string.Empty);
                if (actualizar.id_envcs == 0)
                    keyValues.Add("T_EnviadoCent", "No");
                else
                    keyValues.Add("T_EnviadoCent", "Sí");
                if (actualizar.id_localidadcs != null)
                    keyValues.Add("T_LocalidadCent", actualizar.id_localidadcs.Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_LocalidadCent", string.Empty);
                if (actualizar.id_mandodirecto != null)
                    keyValues.Add("T_MandoDirecto", actualizar.id_mandodirecto.Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_MandoDirecto", string.Empty);
                if (actualizar.id_testigo1 != null)
                    keyValues.Add("T_Testigo1", actualizar.id_testigo1.Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_Testigo1", string.Empty);
                if (actualizar.id_testigo2 != null)
                    keyValues.Add("T_Testigo2", actualizar.id_testigo2.Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_Testigo2", string.Empty);
                if (actualizar.id_testigo3 != null)
                    keyValues.Add("T_Testigo3", actualizar.id_testigo3.Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_Testigo3", string.Empty);

                SearchAndReplace(destinationFile, keyValues);

                accionesmejora acci = Datos.GetDatosAccionMejora(id);

                Session["nombreArchivo"] = destinationFile;
                #endregion


                List<evento_seguridad_tipo> eventipos = Datos.GetDatosEventoSegTipos();
                ViewData["tiposeventoseg"] = eventipos;
                List<evento_seguridad_severidad> evenseveridades = Datos.GetDatosEventoSegSeveridades();
                ViewData["severidadeseventoseg"] = evenseveridades;

                List<evento_seguridad_tipoevento> eventipoeven = Datos.GetDatosEventoSegTiposEven();
                ViewData["tiposeveneventoeventoseg"] = eventipoeven;
                List<evento_seguridad_subtipoevento> evensubtipoeven = Datos.GetDatosEventoSegSubtiposEven();
                ViewData["subtiposeveneventoeventoseg"] = evensubtipoeven;

                evento_seguridad even = Datos.GetDatosEventoSeg(id);
                ViewData["eventoseguridad"] = even;
                ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 15, id);
                ViewData["fotoseventoseg"] = Datos.ListarFotosEventoSeg(id);
                return Redirect(Url.RouteUrl(new { controller = "Comunicacion", action = "detalle_evento_seg", id = id }));
                #endregion
            }
            else
            {
                #region recarga
                List<evento_seguridad_tipo> eventipos = Datos.GetDatosEventoSegTipos();
                ViewData["tiposeventoseg"] = eventipos;
                List<evento_seguridad_severidad> evenseveridades = Datos.GetDatosEventoSegSeveridades();
                ViewData["severidadeseventoseg"] = evenseveridades;

                List<evento_seguridad_tipoevento> eventipoeven = Datos.GetDatosEventoSegTiposEven();
                ViewData["tiposeveneventoeventoseg"] = eventipoeven;
                List<evento_seguridad_subtipoevento> evensubtipoeven = Datos.GetDatosEventoSegSubtiposEven();
                ViewData["subtiposeveneventoeventoseg"] = evensubtipoeven;

                evento_seguridad even = Datos.GetDatosEventoSeg(id);
                ViewData["eventoseguridad"] = even;
                ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 15, id);
                ViewData["fotoseventoseg"] = Datos.ListarFotosEventoSeg(id);
                return View();
                #endregion
            }


        }

        public ActionResult detalle_parte(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            Session["modulo"] = "16";

            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            int idCentral = centroseleccionado.id;

            Session["ReferenciaAccionMejora"] = id;
            ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 16, id);

            partes com = Datos.GetDatosParte(id);
            ViewData["parte"] = com;

            #region generacion fichero
            if (Session["nombreArchivo"] != null && Session["nombreArchivo"].ToString() != "")
            {
                if (Session["nombreArchivo"].ToString().Contains("docx"))
                {
                    Response.ContentType = "application/msword";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=FormatoParte.docx");
                    Response.TransmitFile(Session["nombreArchivo"].ToString());
                    Response.End();
                }
                Session["nombreArchivo"] = "";
            }
            #endregion

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult detalle_parte(int id, FormCollection collection)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }
            partes comu = new partes();
            string formulario = collection["hdFormularioEjecutado"];

            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            int idCentral = centroseleccionado.id;

            if(formulario == "btnImprimir")
            {
                #region Imprimir

                string sourceFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaPlantillaWord"] + "FormatoParte.docx");
                string destinationFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaGeneracionWord"] + "ParteComuniacion" + DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + ".docx");

                Session["source"] = sourceFile;
                Session["destino"] = destinationFile;

                System.IO.File.Copy(sourceFile, destinationFile, true);

                partes parte = Datos.GetDatosParte(id);

                Dictionary<string, string> keyValues = new Dictionary<string, string>();

                if (parte.idcomunicacion != null)
                    keyValues.Add("T_NumComunicacion", parte.idcomunicacion);
                else
                    keyValues.Add("T_NumComunicacion", string.Empty);

                if (parte.instalacion != null)
                    keyValues.Add("T_Instalacion", parte.instalacion);
                else
                    keyValues.Add("T_NumComunicacion", string.Empty);

                if (parte.trabajo != null)
                    keyValues.Add("T_Trabajo", parte.trabajo);
                else
                    keyValues.Add("T_Trabajo", parte.trabajo);

                if (parte.detalle != null)
                    keyValues.Add("T_Detalle", parte.detalle.Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_Detalle", string.Empty);

                if (parte.accionescorrectoras != null)
                    keyValues.Add("T_AccionesCorrectoras", parte.accionescorrectoras.Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_AccionesCorrectoras", string.Empty);

                if (parte.accionesprevistas != null)
                    keyValues.Add("T_AccionesPrevistas", parte.accionesprevistas.Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_AccionesPrevistas", string.Empty);

                if (parte.cumplimentadopor != null)
                    keyValues.Add("T_CumplimentadoName", parte.cumplimentadopor);
                else
                    keyValues.Add("T_CumplimentadoName", string.Empty);

                if (parte.cumplimentadofecha != null)
                    keyValues.Add("T_CumplimentadoFecha", parte.cumplimentadofecha.ToString().Replace(" 0:00:00", ""));
                else
                    keyValues.Add("T_CumplimentadoFecha", string.Empty);

                if (parte.entregadopor != null)
                    keyValues.Add("T_Entregadoa", parte.entregadopor);
                else
                    keyValues.Add("T_Entregadoa", string.Empty);

                if (parte.entregadofecha != null)
                    keyValues.Add("T_EntregadoFecha", parte.entregadofecha.ToString().Replace(" 0:00:00", ""));
                else
                    keyValues.Add("T_EntregadoFecha", string.Empty);

                if (parte.recibidounidadorg != null)
                    keyValues.Add("T_RecibidoUnidad", parte.recibidounidadorg);
                else
                    keyValues.Add("T_RecibidoUnidad", string.Empty);

                if (parte.recibidofecha != null)
                    keyValues.Add("T_RecibidoFecha", parte.recibidofecha.ToString().Replace(" 0:00:00", ""));
                else
                    keyValues.Add("T_RecibidoFecha", string.Empty);

                if (parte.resueltopor != null)
                    keyValues.Add("T_ResueltoPor", parte.resueltopor);
                else
                    keyValues.Add("T_ResueltoPor", string.Empty);

                if (parte.resueltofecha != null)
                    keyValues.Add("T_ResueltoFecha", parte.resueltofecha.ToString().Replace(" 0:00:00", ""));
                else
                    keyValues.Add("T_ResueltoFecha", string.Empty);

                if (parte.observaciones != null)
                    keyValues.Add("T_Observaciones", parte.observaciones.Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_Observaciones", string.Empty);

                SearchAndReplace(destinationFile, keyValues);

                Session["nombreArchivo"] = destinationFile;

                partes com = Datos.GetDatosParte(id);
                ViewData["parte"] = com;
                return Redirect(Url.RouteUrl(new { controller = "Comunicacion", action = "detalle_parte", id = id }));

                #endregion

            }

            if (formulario == "GuardarParte")
            {
                try
                {

                    if (id != 0)
                    {
                        #region actualizar
                        partes actualizar = Datos.GetDatosParte(id);
                        //actualizar.empresa = collection["ctl00$MainContent$txtEmpresa"];
                        //actualizar.instalacion = collection["ctl00$MainContent$txtInstalacion"];
                        //actualizar.trabajo = collection["ctl00$MainContent$txtTrabajo"];
                        //actualizar.detalle = collection["ctl00$MainContent$txtDetalle"];
                        //actualizar.accionescorrectoras = collection["ctl00$MainContent$txtAccionesCorrectoras"];
                        actualizar.accionesprevistas = collection["ctl00$MainContent$txtAccionesPrevistas"];

                        //actualizar.cumplimentadopor = collection["ctl00$MainContent$txtCumplimentadoPor"];
                        //if (collection["ctl00$MainContent$txtCumplimentadoFecha"] != "")
                        //    actualizar.cumplimentadofecha = DateTime.Parse(collection["ctl00$MainContent$txtCumplimentadoFecha"]);

                        //actualizar.entregadopor = collection["ctl00$MainContent$txtEntregadoPor"];
                        //if (collection["ctl00$MainContent$txtEntregadoFecha"] != "")
                        //    actualizar.entregadofecha = DateTime.Parse(collection["ctl00$MainContent$txtEntregadoFecha"]);

                        actualizar.recibidounidadorg = collection["ctl00$MainContent$txtRecibidoPor"];
                        if (collection["ctl00$MainContent$txtRecibidoFecha"] != "")
                            actualizar.recibidofecha = DateTime.Parse(collection["ctl00$MainContent$txtRecibidoFecha"]);

                        actualizar.resueltopor = collection["ctl00$MainContent$txtResueltoPor"];
                        if (collection["ctl00$MainContent$txtResueltoFecha"] != "")
                            actualizar.resueltofecha = DateTime.Parse(collection["ctl00$MainContent$txtResueltoFecha"]);

                        actualizar.observaciones = collection["ctl00$MainContent$txtObservaciones"];

                        actualizar.desest = collection["ctl00$MainContent$txtMotivoDesestimacion"];
                        if (collection["ctl00$MainContent$txtFechaDesestimacion"] != "")
                            actualizar.desestfecha = DateTime.Parse(collection["ctl00$MainContent$txtFechaDesestimacion"]);

                        actualizar.asunto = collection["ctl00$MainContent$txtAsunto"];

                        Datos.ActualizarParte(actualizar);
                        
                        Session["EdicionParteMensaje"] = "Información actualizada correctamente";

                        partes com = Datos.GetDatosParte(id);
                        ViewData["parte"] = com;
                        return View();
                        #endregion
                    }
                    else
                    {
                        #region insertar
                        partes insertar = new partes();
                        insertar.empresa = collection["ctl00$MainContent$txtEmpresa"];
                        insertar.instalacion = collection["ctl00$MainContent$txtInstalacion"];
                        insertar.trabajo = collection["ctl00$MainContent$txtTrabajo"];
                        insertar.detalle = collection["ctl00$MainContent$txtDetalle"];
                        insertar.accionescorrectoras = collection["ctl00$MainContent$txtAccionesCorrectoras"];
                        //insertar.accionesprevistas = collection["ctl00$MainContent$txtAccionesPrevistas"];

                        insertar.cumplimentadopor = collection["ctl00$MainContent$txtCumplimentadoPor"];
                        if (collection["ctl00$MainContent$txtCumplimentadoFecha"] != "")
                            insertar.cumplimentadofecha = DateTime.Parse(collection["ctl00$MainContent$txtCumplimentadoFecha"]);

                        insertar.entregadopor = collection["ctl00$MainContent$txtEntregadoPor"];
                        if (collection["ctl00$MainContent$txtEntregadoFecha"] != "")
                            insertar.entregadofecha = DateTime.Parse(collection["ctl00$MainContent$txtEntregadoFecha"]);
                        
                        insertar.asunto = collection["ctl00$MainContent$txtAsunto"];

                        //insertar.recibidounidadorg = collection["ctl00$MainContent$txtRecibidoPor"];
                        //if (collection["ctl00$MainContent$txtRecibidoFecha"] != "")
                        //    insertar.recibidofecha = DateTime.Parse(collection["ctl00$MainContent$txtRecibidoFecha"]);

                        //insertar.resueltopor = collection["ctl00$MainContent$txtResueltoPor"];
                        //if (collection["ctl00$MainContent$txtResueltoFecha"] != "")
                        //    insertar.resueltofecha = DateTime.Parse(collection["ctl00$MainContent$txtResueltoFecha"]);

                        //insertar.observaciones = collection["ctl00$MainContent$txtObservaciones"];
                        insertar.idcentral = idCentral;
                        int idCom = Datos.ActualizarParte(insertar);

                        Session["EdicionParteMensaje"] = "Información actualizada correctamente";

                        partes com = Datos.GetDatosParte(id);
                        ViewData["parte"] = com;
                        return Redirect(Url.RouteUrl(new { controller = "Comunicacion", action = "detalle_parte", id = idCom }));
                        #endregion
                    }
                }
                catch (Exception ex)
                {
                    #region exception
                    partes com = new partes();
                    if (id != 0)
                    {
                        com = Datos.GetDatosParte(id);
                        com.empresa = collection["ctl00$MainContent$txtEmpresa"];
                        com.instalacion = collection["ctl00$MainContent$txtInstalacion"];
                        com.trabajo = collection["ctl00$MainContent$txtTrabajo"];
                        com.detalle = collection["ctl00$MainContent$txtDetalle"];
                        com.accionescorrectoras = collection["ctl00$MainContent$txtAccionesCorrectoras"];
                        com.accionesprevistas = collection["ctl00$MainContent$txtAccionesPrevistas"];

                        com.cumplimentadopor = collection["ctl00$MainContent$txtCumplimentadoPor"];
                        if (collection["ctl00$MainContent$txtCumplimentadoFecha"] != "")
                            com.cumplimentadofecha = DateTime.Parse(collection["ctl00$MainContent$txtCumplimentadoFecha"]);

                        com.entregadopor = collection["ctl00$MainContent$txtEntregadoPor"];
                        if (collection["ctl00$MainContent$txtEntregadoFecha"] != "")
                            com.entregadofecha = DateTime.Parse(collection["ctl00$MainContent$txtEntregadoFecha"]);

                        com.recibidounidadorg = collection["ctl00$MainContent$txtRecibidoPor"];
                        if (collection["ctl00$MainContent$txtRecibidoFecha"] != "")
                            com.recibidofecha = DateTime.Parse(collection["ctl00$MainContent$txtRecibidoFecha"]);

                        com.resueltopor = collection["ctl00$MainContent$txtResueltoPor"];
                        if (collection["ctl00$MainContent$txtResueltoFecha"] != "")
                            com.resueltofecha = DateTime.Parse(collection["ctl00$MainContent$txtResueltoFecha"]);

                        com.observaciones = collection["ctl00$MainContent$txtObservaciones"];

                        com.asunto = collection["ctl00$MainContent$txtAsunto"];

                        com.desest = collection["ctl00$MainContent$txtMotivoDesestimacion"];
                        if (collection["ctl00$MainContent$txtFechaDesestimacion"] != "")
                            com.desestfecha = DateTime.Parse(collection["ctl00$MainContent$txtFechaDesestimacion"]);
                    }
                    else
                    {
                        com.empresa = collection["ctl00$MainContent$txtEmpresa"];
                        com.instalacion = collection["ctl00$MainContent$txtInstalacion"];
                        com.trabajo = collection["ctl00$MainContent$txtTrabajo"];
                        com.detalle = collection["ctl00$MainContent$txtDetalle"];
                        com.accionescorrectoras = collection["ctl00$MainContent$txtAccionesCorrectoras"];
                        com.accionesprevistas = collection["ctl00$MainContent$txtAccionesPrevistas"];

                        com.cumplimentadopor = collection["ctl00$MainContent$txtCumplimentadoPor"];
                        if (collection["ctl00$MainContent$txtCumplimentadoFecha"] != "")
                            com.cumplimentadofecha = DateTime.Parse(collection["ctl00$MainContent$txtCumplimentadoFecha"]);

                        com.entregadopor = collection["ctl00$MainContent$txtEntregadoPor"];
                        if (collection["ctl00$MainContent$txtEntregadoFecha"] != "")
                            com.entregadofecha = DateTime.Parse(collection["ctl00$MainContent$txtEntregadoFecha"]);

                        com.recibidounidadorg = collection["ctl00$MainContent$txtRecibidoPor"];
                        if (collection["ctl00$MainContent$txtRecibidoFecha"] != "")
                            com.recibidofecha = DateTime.Parse(collection["ctl00$MainContent$txtRecibidoFecha"]);

                        com.resueltopor = collection["ctl00$MainContent$txtResueltoPor"];
                        if (collection["ctl00$MainContent$txtResueltoFecha"] != "")
                            com.resueltofecha = DateTime.Parse(collection["ctl00$MainContent$txtResueltoFecha"]);

                        com.observaciones = collection["ctl00$MainContent$txtObservaciones"];

                        com.desest = collection["ctl00$MainContent$txtMotivoDesestimacion"];
                        if (collection["ctl00$MainContent$txtFechaDesestimacion"] != "")
                            com.desestfecha = DateTime.Parse(collection["ctl00$MainContent$txtFechaDesestimacion"]);
                    }

                    ViewData["parte"] = com;
                    return View();
                    #endregion
                }
            }
            else
            {
                #region recarga
                partes com = Datos.GetDatosParte(id);
                ViewData["parte"] = com;
                return View();
                #endregion
            }
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

        #region métodos para insertar en excel
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

