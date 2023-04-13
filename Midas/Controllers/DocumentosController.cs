using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using MIDAS.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MIDAS.Controllers
{
    public class DocumentosController : Controller
    {
        //
        // GET: /Documentos/

        public ActionResult admrepositorio()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            ViewData["tecnologias"] = Datos.ListarTecnologias();
            ViewData["ficherosN1"] = Datos.ListarDocumentacion(1, centroseleccionado);
            ViewData["ficherosN2"] = Datos.ListarDocumentacion(2, centroseleccionado);
            ViewData["ficherosN3"] = Datos.ListarDocumentacion(3, centroseleccionado);
            ViewData["ficherosN4"] = Datos.ListarDocumentacion(4, centroseleccionado);

            #region generacion fichero
            if (Session["nombreArchivo"] != null && Session["nombreArchivo"].ToString() != "")
            {
                if (Session["nombreArchivo"].ToString().Contains("xlsx"))
                {
                    Response.ContentType = "application/msexcel";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=ExportacionDocumentos.xlsx");
                    Response.TransmitFile(Session["nombreArchivo"].ToString());
                    Response.End();
                }
                Session["nombreArchivo"] = "";
            }
            #endregion

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult admrepositorio(FormCollection collection)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            VISTA_ObtenerUsuario user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());

            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));

            string formulario = collection["hdFormularioEjecutado"];


            if (formulario == "btnImprimir")
            {
                #region generacion fichero
                string sourceFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaPlantillaWord"] + "ExportacionDocumentos.xlsx");
                string destinationFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaGeneracionWord"] + "ExportacionDocumentos_" + DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + ".xlsx");

                Session["source"] = sourceFile;
                Session["destino"] = destinationFile;
                // Create a copy of the template file and open the copy 
                System.IO.File.Copy(sourceFile, destinationFile, true);

                using (SpreadsheetDocument document = SpreadsheetDocument.Open(destinationFile, true))
                {
                    #region nivel1
                    List<VISTA_ObtenerDocumentacion> nivel1 = Datos.ListarDocumentacion(1, centroseleccionado);
                    SheetData sheetData = document.WorkbookPart.WorksheetParts.ElementAt(2).Worksheet.Elements<SheetData>().First();
                    foreach (VISTA_ObtenerDocumentacion doc in nivel1)
                    {
                        Row row = new Row();

                        string cod_fichero = string.Empty;
                        string titulo = string.Empty;
                        string tipodoc = string.Empty;
                        string version = string.Empty;
                        string fecha_aprobacion = string.Empty;
                        string fecha_publicacion = string.Empty;

                        if (doc.cod_fichero == null)
                            cod_fichero = string.Empty;
                        else
                            cod_fichero = doc.cod_fichero;
                        if (doc.titulo == null)
                            titulo = string.Empty;
                        else
                            titulo = doc.titulo;
                        if (doc.tipodoc == null)
                            tipodoc = string.Empty;
                        else
                            tipodoc = doc.tipodoc;
                        if (doc.version == null)
                            version = string.Empty;
                        else
                            version = doc.version;
                        if (doc.fecha_aprobacion == null)
                            fecha_aprobacion = string.Empty;
                        else
                            fecha_aprobacion = doc.fecha_aprobacion.ToString().Replace(" 0:00:00", "");
                        if (doc.fecha_publicacion == null)
                            fecha_publicacion = string.Empty;
                        else
                            fecha_publicacion = doc.fecha_publicacion.ToString().Replace(" 0:00:00", "");

                        row.Append(
                            Datos.ConstructCell(cod_fichero.ToString(), CellValues.String),
                            Datos.ConstructCell(titulo, CellValues.String),
                            Datos.ConstructCell(tipodoc, CellValues.String),
                            Datos.ConstructCell(version, CellValues.String),
                            Datos.ConstructCell(fecha_aprobacion, CellValues.String),
                            Datos.ConstructCell(fecha_publicacion, CellValues.String));

                        sheetData.AppendChild(row);
                    }
                    #endregion

                    #region nivel2
                    List<VISTA_ObtenerDocumentacion> nivel2 = Datos.ListarDocumentacion(2, centroseleccionado);
                    SheetData sheetData2 = document.WorkbookPart.WorksheetParts.ElementAt(1).Worksheet.Elements<SheetData>().First();
                    foreach (VISTA_ObtenerDocumentacion doc in nivel2)
                    {
                        Row row = new Row();

                        string cod_fichero = string.Empty;
                        string titulo = string.Empty;
                        string tipodoc = string.Empty;
                        string version = string.Empty;
                        string fecha_aprobacion = string.Empty;
                        string fecha_publicacion = string.Empty;

                        if (doc.cod_fichero == null)
                            cod_fichero = string.Empty;
                        else
                            cod_fichero = doc.cod_fichero;
                        if (doc.titulo == null)
                            titulo = string.Empty;
                        else
                            titulo = doc.titulo;
                        if (doc.tipodoc == null)
                            tipodoc = string.Empty;
                        else
                            tipodoc = doc.tipodoc;
                        if (doc.version == null)
                            version = string.Empty;
                        else
                            version = doc.version;
                        if (doc.fecha_aprobacion == null)
                            fecha_aprobacion = string.Empty;
                        else
                            fecha_aprobacion = doc.fecha_aprobacion.ToString().Replace(" 0:00:00", "");
                        if (doc.fecha_publicacion == null)
                            fecha_publicacion = string.Empty;
                        else
                            fecha_publicacion = doc.fecha_publicacion.ToString().Replace(" 0:00:00", "");

                        row.Append(
                            Datos.ConstructCell(cod_fichero.ToString(), CellValues.String),
                            Datos.ConstructCell(titulo, CellValues.String),
                            Datos.ConstructCell(tipodoc, CellValues.String),
                            Datos.ConstructCell(version, CellValues.String),
                            Datos.ConstructCell(fecha_aprobacion, CellValues.String),
                            Datos.ConstructCell(fecha_publicacion, CellValues.String));

                        sheetData2.AppendChild(row);
                    }
                    #endregion

                    #region nivel3

                    List<VISTA_ObtenerDocumentacion> nivel3 = Datos.ListarDocumentacion(3, centroseleccionado);
                    SheetData sheetData3 = document.WorkbookPart.WorksheetParts.ElementAt(0).Worksheet.Elements<SheetData>().First();
                    foreach (VISTA_ObtenerDocumentacion doc in nivel3)
                    {
                        Row row = new Row();

                        string cod_fichero = string.Empty;
                        string titulo = string.Empty;
                        string tipodoc = string.Empty;
                        string version = string.Empty;
                        string fecha_aprobacion = string.Empty;
                        string fecha_publicacion = string.Empty;

                        if (doc.cod_fichero == null)
                            cod_fichero = string.Empty;
                        else
                            cod_fichero = doc.cod_fichero;
                        if (doc.titulo == null)
                            titulo = string.Empty;
                        else
                            titulo = doc.titulo;
                        if (doc.tipodoc == null)
                            tipodoc = string.Empty;
                        else
                            tipodoc = doc.tipodoc;
                        if (doc.version == null)
                            version = string.Empty;
                        else
                            version = doc.version;
                        if (doc.fecha_aprobacion == null)
                            fecha_aprobacion = string.Empty;
                        else
                            fecha_aprobacion = doc.fecha_aprobacion.ToString().Replace(" 0:00:00", "");
                        if (doc.fecha_publicacion == null)
                            fecha_publicacion = string.Empty;
                        else
                            fecha_publicacion = doc.fecha_publicacion.ToString().Replace(" 0:00:00", "");

                        row.Append(
                            Datos.ConstructCell(cod_fichero.ToString(), CellValues.String),
                            Datos.ConstructCell(titulo, CellValues.String),
                            Datos.ConstructCell(tipodoc, CellValues.String),
                            Datos.ConstructCell(version, CellValues.String),
                            Datos.ConstructCell(fecha_aprobacion, CellValues.String),
                            Datos.ConstructCell(fecha_publicacion, CellValues.String));

                        sheetData3.AppendChild(row);
                    }

                    if (centroseleccionado.ubicacion == 6)
                    {

                        List<VISTA_ObtenerDocumentacion> nivel4 = Datos.ListarDocumentacion(4, centroseleccionado);

                        foreach (VISTA_ObtenerDocumentacion doc in nivel4)
                        {
                            Row row = new Row();

                            string cod_fichero = string.Empty;
                            string titulo = string.Empty;
                            string tipodoc = string.Empty;
                            string version = string.Empty;
                            string fecha_aprobacion = string.Empty;
                            string fecha_publicacion = string.Empty;

                            if (doc.cod_fichero == null)
                                cod_fichero = string.Empty;
                            else
                                cod_fichero = doc.cod_fichero;
                            if (doc.titulo == null)
                                titulo = string.Empty;
                            else
                                titulo = doc.titulo;
                            if (doc.tipodoc == null)
                                tipodoc = string.Empty;
                            else
                                tipodoc = doc.tipodoc;
                            if (doc.version == null)
                                version = string.Empty;
                            else
                                version = doc.version;
                            if (doc.fecha_aprobacion == null)
                                fecha_aprobacion = string.Empty;
                            else
                                fecha_aprobacion = doc.fecha_aprobacion.ToString().Replace(" 0:00:00", "");
                            if (doc.fecha_publicacion == null)
                                fecha_publicacion = string.Empty;
                            else
                                fecha_publicacion = doc.fecha_publicacion.ToString().Replace(" 0:00:00", "");

                            row.Append(
                                Datos.ConstructCell(cod_fichero.ToString(), CellValues.String),
                                Datos.ConstructCell(titulo, CellValues.String),
                                Datos.ConstructCell(tipodoc, CellValues.String),
                                Datos.ConstructCell(version, CellValues.String),
                                Datos.ConstructCell(fecha_aprobacion, CellValues.String),
                                Datos.ConstructCell(fecha_publicacion, CellValues.String));

                            sheetData3.AppendChild(row);
                        }
                    }

                    #endregion

                    // save worksheet
                    document.WorkbookPart.WorksheetParts.First().Worksheet.Save();

                    document.WorkbookPart.Workbook.Save();

                    Session["nombreArchivo"] = destinationFile;
                }
                #endregion
            }

            if (formulario == "btnImprimirSIG")
            {
                #region generacion fichero
                string sourceFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaPlantillaWord"] + "ExportacionDocumentosSIG.xlsx");
                string destinationFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaGeneracionWord"] + "ExportacionDocumentosSIG_" + DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + ".xlsx");

                Session["source"] = sourceFile;
                Session["destino"] = destinationFile;
                // Create a copy of the template file and open the copy 
                System.IO.File.Copy(sourceFile, destinationFile, true);

                using (SpreadsheetDocument document = SpreadsheetDocument.Open(destinationFile, true))
                {
                    #region nivel1
                    List<VISTA_ObtenerDocumentacion> nivel1 = Datos.ListarDocumentacion(1, centroseleccionado);
                    SheetData sheetData = document.WorkbookPart.WorksheetParts.ElementAt(2).Worksheet.Elements<SheetData>().First();
                    foreach (VISTA_ObtenerDocumentacion doc in nivel1)
                    {
                        if (doc.tipo == 1 || doc.tipo == 2 || doc.tipo == 6 || doc.tipo == 8 || doc.tipo == 9 | doc.tipo == 11)
                        {
                            Row row = new Row();

                            string cod_fichero = string.Empty;
                            string titulo = string.Empty;
                            string tipodoc = string.Empty;
                            string version = string.Empty;
                            string fecha_aprobacion = string.Empty;
                            string fecha_publicacion = string.Empty;

                            if (doc.cod_fichero == null)
                                cod_fichero = string.Empty;
                            else
                                cod_fichero = doc.cod_fichero;
                            if (doc.titulo == null)
                                titulo = string.Empty;
                            else
                                titulo = doc.titulo;
                            if (doc.tipodoc == null)
                                tipodoc = string.Empty;
                            else
                                tipodoc = doc.tipodoc;
                            if (doc.version == null)
                                version = string.Empty;
                            else
                                version = doc.version;
                            if (doc.fecha_aprobacion == null)
                                fecha_aprobacion = string.Empty;
                            else
                                fecha_aprobacion = doc.fecha_aprobacion.ToString().Replace(" 0:00:00", "");
                            if (doc.fecha_publicacion == null)
                                fecha_publicacion = string.Empty;
                            else
                                fecha_publicacion = doc.fecha_publicacion.ToString().Replace(" 0:00:00", "");

                            row.Append(
                                Datos.ConstructCell(cod_fichero.ToString(), CellValues.String),
                                Datos.ConstructCell(titulo, CellValues.String),
                                Datos.ConstructCell(tipodoc, CellValues.String),
                                Datos.ConstructCell(version, CellValues.String),
                                Datos.ConstructCell(fecha_aprobacion, CellValues.String),
                                Datos.ConstructCell(fecha_publicacion, CellValues.String));

                            sheetData.AppendChild(row);
                        }
                    }
                    #endregion

                    #region nivel2
                    List<VISTA_ObtenerDocumentacion> nivel2 = Datos.ListarDocumentacion(2, centroseleccionado);
                    SheetData sheetData2 = document.WorkbookPart.WorksheetParts.ElementAt(1).Worksheet.Elements<SheetData>().First();
                    foreach (VISTA_ObtenerDocumentacion doc in nivel2)
                    {
                        if (doc.tipo == 1 || doc.tipo == 2 || doc.tipo == 6 || doc.tipo == 8 || doc.tipo == 9 | doc.tipo == 11)
                        {
                            Row row = new Row();

                            string cod_fichero = string.Empty;
                            string titulo = string.Empty;
                            string tipodoc = string.Empty;
                            string version = string.Empty;
                            string fecha_aprobacion = string.Empty;
                            string fecha_publicacion = string.Empty;

                            if (doc.cod_fichero == null)
                                cod_fichero = string.Empty;
                            else
                                cod_fichero = doc.cod_fichero;
                            if (doc.titulo == null)
                                titulo = string.Empty;
                            else
                                titulo = doc.titulo;
                            if (doc.tipodoc == null)
                                tipodoc = string.Empty;
                            else
                                tipodoc = doc.tipodoc;
                            if (doc.version == null)
                                version = string.Empty;
                            else
                                version = doc.version;
                            if (doc.fecha_aprobacion == null)
                                fecha_aprobacion = string.Empty;
                            else
                                fecha_aprobacion = doc.fecha_aprobacion.ToString().Replace(" 0:00:00", "");
                            if (doc.fecha_publicacion == null)
                                fecha_publicacion = string.Empty;
                            else
                                fecha_publicacion = doc.fecha_publicacion.ToString().Replace(" 0:00:00", "");

                            row.Append(
                                Datos.ConstructCell(cod_fichero.ToString(), CellValues.String),
                                Datos.ConstructCell(titulo, CellValues.String),
                                Datos.ConstructCell(tipodoc, CellValues.String),
                                Datos.ConstructCell(version, CellValues.String),
                                Datos.ConstructCell(fecha_aprobacion, CellValues.String),
                                Datos.ConstructCell(fecha_publicacion, CellValues.String));

                            sheetData2.AppendChild(row);
                        }
                    }
                    #endregion

                    #region nivel3

                    List<VISTA_ObtenerDocumentacion> nivel3 = Datos.ListarDocumentacion(3, centroseleccionado);
                    SheetData sheetData3 = document.WorkbookPart.WorksheetParts.ElementAt(0).Worksheet.Elements<SheetData>().First();
                    foreach (VISTA_ObtenerDocumentacion doc in nivel3)
                    {
                        if (doc.tipo == 1 || doc.tipo == 2 || doc.tipo == 6 || doc.tipo == 8 || doc.tipo == 9 | doc.tipo == 11)
                        {
                            Row row = new Row();

                            string cod_fichero = string.Empty;
                            string titulo = string.Empty;
                            string tipodoc = string.Empty;
                            string version = string.Empty;
                            string fecha_aprobacion = string.Empty;
                            string fecha_publicacion = string.Empty;

                            if (doc.cod_fichero == null)
                                cod_fichero = string.Empty;
                            else
                                cod_fichero = doc.cod_fichero;
                            if (doc.titulo == null)
                                titulo = string.Empty;
                            else
                                titulo = doc.titulo;
                            if (doc.tipodoc == null)
                                tipodoc = string.Empty;
                            else
                                tipodoc = doc.tipodoc;
                            if (doc.version == null)
                                version = string.Empty;
                            else
                                version = doc.version;
                            if (doc.fecha_aprobacion == null)
                                fecha_aprobacion = string.Empty;
                            else
                                fecha_aprobacion = doc.fecha_aprobacion.ToString().Replace(" 0:00:00", "");
                            if (doc.fecha_publicacion == null)
                                fecha_publicacion = string.Empty;
                            else
                                fecha_publicacion = doc.fecha_publicacion.ToString().Replace(" 0:00:00", "");

                            row.Append(
                                Datos.ConstructCell(cod_fichero.ToString(), CellValues.String),
                                Datos.ConstructCell(titulo, CellValues.String),
                                Datos.ConstructCell(tipodoc, CellValues.String),
                                Datos.ConstructCell(version, CellValues.String),
                                Datos.ConstructCell(fecha_aprobacion, CellValues.String),
                                Datos.ConstructCell(fecha_publicacion, CellValues.String));

                            sheetData3.AppendChild(row);
                        }
                    }

                    List<VISTA_ObtenerDocumentacion> nivel4 = Datos.ListarDocumentacion(4, centroseleccionado);

                    foreach (VISTA_ObtenerDocumentacion doc in nivel4)
                    {
                        if (doc.tipo == 1 || doc.tipo == 2 || doc.tipo == 6 || doc.tipo == 8 || doc.tipo == 9 | doc.tipo == 11)
                        {

                            Row row = new Row();

                            string cod_fichero = string.Empty;
                            string titulo = string.Empty;
                            string tipodoc = string.Empty;
                            string version = string.Empty;
                            string fecha_aprobacion = string.Empty;
                            string fecha_publicacion = string.Empty;

                            if (doc.cod_fichero == null)
                                cod_fichero = string.Empty;
                            else
                                cod_fichero = doc.cod_fichero;
                            if (doc.titulo == null)
                                titulo = string.Empty;
                            else
                                titulo = doc.titulo;
                            if (doc.tipodoc == null)
                                tipodoc = string.Empty;
                            else
                                tipodoc = doc.tipodoc;
                            if (doc.version == null)
                                version = string.Empty;
                            else
                                version = doc.version;
                            if (doc.fecha_aprobacion == null)
                                fecha_aprobacion = string.Empty;
                            else
                                fecha_aprobacion = doc.fecha_aprobacion.ToString().Replace(" 0:00:00", "");
                            if (doc.fecha_publicacion == null)
                                fecha_publicacion = string.Empty;
                            else
                                fecha_publicacion = doc.fecha_publicacion.ToString().Replace(" 0:00:00", "");

                            row.Append(
                                Datos.ConstructCell(cod_fichero.ToString(), CellValues.String),
                                Datos.ConstructCell(titulo, CellValues.String),
                                Datos.ConstructCell(tipodoc, CellValues.String),
                                Datos.ConstructCell(version, CellValues.String),
                                Datos.ConstructCell(fecha_aprobacion, CellValues.String),
                                Datos.ConstructCell(fecha_publicacion, CellValues.String));

                            sheetData3.AppendChild(row);
                        }
                    }

                    #endregion

                    // save worksheet
                    document.WorkbookPart.WorksheetParts.First().Worksheet.Save();

                    document.WorkbookPart.Workbook.Save();

                    Session["nombreArchivo"] = destinationFile;
                }
                #endregion
            }

            ViewData["tecnologias"] = Datos.ListarTecnologias();
            ViewData["ficherosN1"] = Datos.ListarDocumentacion(1, centroseleccionado);
            ViewData["ficherosN2"] = Datos.ListarDocumentacion(2, centroseleccionado);
            ViewData["ficherosN3"] = Datos.ListarDocumentacion(3, centroseleccionado);
            ViewData["ficherosN4"] = Datos.ListarDocumentacion(4, centroseleccionado);

            return RedirectToAction("admrepositorio", "Documentos");
        }

        public ActionResult editar_documento(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            #region recarga
            documentacion IF = Datos.ObtenerFicheroDocPorID(id);
            ViewData["fichero"] = IF;

            if (IF != null)
                ViewData["tiposdocumento"] = Datos.ListarTipoDocumento(IF.nivel, IF.tipocentral);
            else
            {
                centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
                if (centroseleccionado.tipo == 4)
                {
                    ViewData["tiposdocumento"] = Datos.ListarTipoDocumento(1, null);
                }
                else
                {
                    ViewData["tiposdocumento"] = Datos.ListarTipoDocumento(3, centroseleccionado.tipo);
                }
            }
            ViewData["tecnologias"] = Datos.ListarTecnologias();
            ViewData["procesos"] = Datos.ListarProcesos();
            ViewData["historial_doc"] = Datos.ListarLogDocumentacion(id);
            Session["ficheroOriginal"] = id;
            #endregion
            return View();
        }

        [HttpPost]
        public ActionResult editar_documento(int id, FormCollection collection, HttpPostedFileBase file)
        {
            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            try
            {
                string formulario = collection["hdFormularioEjecutado"];
                string cod_fichero = collection["ctl00$MainContent$txtCodigo"];

                if (formulario == "GuardarDocumento")
                {
                    if (id == 0)
                    {
                        if ((file != null && file.ContentLength > 0) && (collection["ctl00$MainContent$txtTitulo"] != null))
                        {
                            #region ficheronuevo
                            //FICHERO NUEVO
                            documentacion IF = new documentacion();

                            IF.titulo = collection["ctl00$MainContent$txtTitulo"];
                            IF.cod_fichero = collection["ctl00$MainContent$txtCodigo"];
                            IF.version = collection["ctl00$MainContent$txtVersion"];
                            IF.tipo = int.Parse(collection["ctl00$MainContent$ddlTipo"]);
                            IF.nivel = 1;
                            //IF.nivel = int.Parse(collection["ctl00$MainContent$ddlNivel"]);
                            IF.estado = 0;
                            if (collection["ctl00$MainContent$ddlTecnologia"] != null && IF.nivel == 2)
                                IF.tipocentral = int.Parse(collection["ctl00$MainContent$ddlTecnologia"]);
                            if (collection["ctl00$MainContent$txtFAprobacion"] != null && collection["ctl00$MainContent$txtFAprobacion"] != "")
                                IF.fecha_aprobacion = DateTime.Parse(collection["ctl00$MainContent$txtFAprobacion"]);
                            if (collection["ctl00$MainContent$txtFPublicacion"] != null && collection["ctl00$MainContent$txtFPublicacion"] != "")
                                IF.fecha_publicacion = DateTime.Parse(collection["ctl00$MainContent$txtFPublicacion"]);
                            if (collection["ctl00$MainContent$ddlProcesos"] != null && collection["ctl00$MainContent$ddlProcesos"] != "0")
                                IF.idproceso = int.Parse(collection["ctl00$MainContent$ddlProcesos"]);

                            if (IF.nivel == 3)
                            {
                                IF.idcentro = centroseleccionado.id;
                                IF.titulo = centroseleccionado.siglas + "_" + IF.titulo;
                            }
                            var fileName = System.IO.Path.GetFileName(file.FileName);

                            IF.nombre_fichero = fileName;
                            if (IF.titulo != string.Empty && IF.fecha_aprobacion != null && IF.fecha_publicacion != null && IF.version != null)
                            {
                            int idFichero = Datos.InsertDocumentacion(IF);

                            var path = System.IO.Path.Combine(Server.MapPath("~/Documentacion/" + idFichero.ToString()), fileName);
                            IF.enlace = path;

                            
                                Datos.UpdateDocumentacionEnlace(IF);
                            

                            if (Directory.Exists(Server.MapPath("~/Documentacion/" + idFichero.ToString())))
                            {
                                file.SaveAs(path);
                            }
                            else
                            {
                                Directory.CreateDirectory(Server.MapPath("~/Documentacion/" + idFichero.ToString()));
                                file.SaveAs(path);
                            }

                            documentacion_hist historico = new documentacion_hist();
                            historico.fechasubida = DateTime.Now.Date;
                            historico.fecha_aprobacion = IF.fecha_aprobacion;
                            historico.fecha_publicacion = IF.fecha_publicacion;
                            historico.idVigente = idFichero;
							historico.version = IF.version;							   

                            int idHistorico = Datos.InsertHistorico(historico);

                            var pathhistorico = System.IO.Path.Combine(Server.MapPath("~/Documentacion/" + idFichero.ToString() + "/" + idHistorico.ToString()), fileName);
                            historico.enlace = pathhistorico;

                            if (Directory.Exists(Server.MapPath("~/Documentacion/" + idFichero.ToString() + "/" + idHistorico.ToString())))
                            {
                                file.SaveAs(pathhistorico);
                            }
                            else
                            {
                                Directory.CreateDirectory(Server.MapPath("~/Documentacion/" + idFichero.ToString() + "/" + idHistorico.ToString()));
                                file.SaveAs(pathhistorico);
                            }

                            Datos.UpdateHistorico(historico);
                                                           

                            Session["EditarDocumentacionResultado"] = "Documentación agregada correctamente.";

                            ViewData["fichero"] = Datos.ObtenerFicheroDocPorID(IF.idFichero);

                            if (IF.nivel == 2)
                            {
                                if (centroseleccionado.tipo == 4)
                                {
                                    if (IF.tipocentral == null)
                                    {
                                        ViewData["tecnologias"] = Datos.ListarTecnologias();
                                        ViewData["tiposdocumento"] = Datos.ListarTipoDocumento(IF.nivel, 1);
                                    }
                                    else
                                    {
                                        ViewData["tiposdocumento"] = Datos.ListarTipoDocumento(IF.nivel, IF.tipocentral);
                                        ViewData["tecnologias"] = Datos.ListarTecnologias();
                                    }
                                }
                                else
                                {
                                    ViewData["tiposdocumento"] = Datos.ListarTipoDocumento(IF.nivel, centroseleccionado.tipo);
                                    ViewData["tecnologias"] = Datos.ListarTecnologias();
                                }
                            }
                            else
                            {
                                ViewData["tiposdocumento"] = Datos.ListarTipoDocumento(IF.nivel, IF.tipocentral);
                                ViewData["tecnologias"] = Datos.ListarTecnologias();
                            }
                            ViewData["procesos"] = Datos.ListarProcesos();
                            ViewData["historial_doc"] = Datos.ListarLogDocumentacion(id);
                            Session["ficheroOriginal"] = id;

                            return RedirectToAction("editar_documento/" + idFichero, "Documentos");
                            }
                            else
                            {
                                Session["EditarDocumentacionError"] = "Los campos marcados con (*) son obligatorios.";
                                return RedirectToAction("editar_documento/0", "Documentos");
                            }
                            #endregion
                        }
                        else
                        {
                            #region fichero no adjunto
                            documentacion IF = new documentacion();
                            IF.titulo = collection["ctl00$MainContent$txtTitulo"];
                            IF.cod_fichero = collection["ctl00$MainContent$txtCodigo"];
                            IF.version = collection["ctl00$MainContent$txtVersion"];
                            IF.tipo = int.Parse(collection["ctl00$MainContent$ddlTipo"]);
                            IF.nivel = 1;
                            //IF.nivel = int.Parse(collection["ctl00$MainContent$ddlNivel"]);                            
                            IF.estado = 0;
                            if (collection["ctl00$MainContent$ddlTecnologia"] != null && IF.nivel == 2)
                                IF.tipocentral = int.Parse(collection["ctl00$MainContent$ddlTecnologia"]);
                            if (collection["ctl00$MainContent$txtFAprobacion"] != null && collection["ctl00$MainContent$txtFAprobacion"] != "")
                                IF.fecha_aprobacion = DateTime.Parse(collection["ctl00$MainContent$txtFAprobacion"]);
                            if (collection["ctl00$MainContent$txtFPublicacion"] != null && collection["ctl00$MainContent$txtFPublicacion"] != "")
                                IF.fecha_publicacion = DateTime.Parse(collection["ctl00$MainContent$txtFPublicacion"]);
                            if (collection["ctl00$MainContent$ddlProcesos"] != null && collection["ctl00$MainContent$ddlProcesos"] != "0")
                                IF.idproceso = int.Parse(collection["ctl00$MainContent$ddlProcesos"]);

                            Session["EditarDocumentacionError"] = "Debe adjuntar un fichero e indicar un título para el mismo";

                            ViewData["fichero"] = IF;

                            if (IF.nivel == 2)
                            {
                                if (centroseleccionado.tipo == 4)
                                {
                                    if (IF.tipocentral == null)
                                    {
                                        ViewData["tecnologias"] = Datos.ListarTecnologias();
                                        ViewData["tiposdocumento"] = Datos.ListarTipoDocumento(IF.nivel, 1);
                                    }
                                    else
                                    {
                                        ViewData["tiposdocumento"] = Datos.ListarTipoDocumento(IF.nivel, IF.tipocentral);
                                        ViewData["tecnologias"] = Datos.ListarTecnologias();
                                    }
                                }
                                else
                                {
                                    ViewData["tiposdocumento"] = Datos.ListarTipoDocumento(IF.nivel, centroseleccionado.tipo);
                                    ViewData["tecnologias"] = Datos.ListarTecnologias();
                                }
                            }
                            else
                            {
                                ViewData["tiposdocumento"] = Datos.ListarTipoDocumento(IF.nivel, IF.tipocentral);
                                ViewData["tecnologias"] = Datos.ListarTecnologias();
                            }
                            ViewData["procesos"] = Datos.ListarProcesos();
                            ViewData["historial_doc"] = Datos.ListarLogDocumentacion(id);
                            Session["ficheroOriginal"] = id;

                            return View();
                            #endregion
                        }
                    }
                    else
                    {
                        #region actualizacion
                        //ACTUALIZACION
                        if (collection["ctl00$MainContent$txtTitulo"] != null)
                        {
                            documentacion IF = new documentacion();

                            IF.idFichero = id;
                            IF.titulo = collection["ctl00$MainContent$txtTitulo"];
                            IF.cod_fichero = collection["ctl00$MainContent$txtCodigo"];
                            IF.version = collection["ctl00$MainContent$txtVersion"];
                            IF.tipo = int.Parse(collection["ctl00$MainContent$ddlTipo"]);
                            if (collection["ctl00$MainContent$txtFAprobacion"] != null && collection["ctl00$MainContent$txtFAprobacion"] != "")
                                IF.fecha_aprobacion = DateTime.Parse(collection["ctl00$MainContent$txtFAprobacion"]);
                            if (collection["ctl00$MainContent$txtFPublicacion"] != null && collection["ctl00$MainContent$txtFPublicacion"] != "")
                                IF.fecha_publicacion = DateTime.Parse(collection["ctl00$MainContent$txtFPublicacion"]);
                            if (collection["ctl00$MainContent$ddlProcesos"] != null && collection["ctl00$MainContent$ddlProcesos"] != "0")
                                IF.idproceso = int.Parse(collection["ctl00$MainContent$ddlProcesos"]);

                            if (IF.titulo != string.Empty && IF.fecha_aprobacion != null && IF.fecha_publicacion != null && IF.version != null)
                            {
                                Datos.UpdateDocumentacion(IF);
                                Session["EditarDocumentacionResultado"] = "Datos actualizados correctamente.";


                                #region actualizacion ruta
                                if (file != null && file.ContentLength > 0)
                                {
                                    var fileName = System.IO.Path.GetFileName(file.FileName);

                                    IF.nombre_fichero = fileName;

                                    var path = System.IO.Path.Combine(Server.MapPath("~/Documentacion/" + id.ToString()), fileName);
                                    IF.enlace = path;

                                    Datos.UpdateDocumentacionEnlace(IF);

                                    if (Directory.Exists(Server.MapPath("~/Documentacion/" + id)))
                                    {
                                        file.SaveAs(path);
                                    }
                                    else
                                    {
                                        Directory.CreateDirectory(Server.MapPath("~/Documentacion/" + id));
                                        file.SaveAs(path);
                                    }

                                    documentacion_hist historico = new documentacion_hist();
                                    historico.fechasubida = DateTime.Now.Date;
                                    historico.fecha_aprobacion = IF.fecha_aprobacion;
                                    historico.fecha_publicacion = IF.fecha_publicacion;
                                    historico.idVigente = id;
									historico.version = IF.version;							   

                                    int idHistorico = Datos.InsertHistorico(historico);

                                    var pathhistorico = System.IO.Path.Combine(Server.MapPath("~/Documentacion/" + id + "/" + idHistorico.ToString()), fileName);
                                    historico.enlace = pathhistorico;

                                    if (Directory.Exists(Server.MapPath("~/Documentacion/" + id + "/" + idHistorico.ToString())))
                                    {
                                        file.SaveAs(pathhistorico);
                                    }
                                    else
                                    {
                                        Directory.CreateDirectory(Server.MapPath("~/Documentacion/" + id + "/" + idHistorico.ToString()));
                                        file.SaveAs(pathhistorico);
                                    }

                                    Datos.UpdateHistorico(historico);
                                    Session["EditarDocumentacionResultado"] = "Documentación agregada correctamente.";
                                }
                                #endregion
                            }
                            else
                            {
                                Session["EditarDocumentacionError"] = "Los campos marcados con (*) son obligatorios.";
                            }

                            IF = Datos.ObtenerFicheroDocPorID(id);
                            ViewData["fichero"] = IF;
                            if (IF.nivel == 2)
                            {
                                if (centroseleccionado.tipo == 4)
                                {
                                    if (IF.tipocentral == null)
                                    {
                                        ViewData["tecnologias"] = Datos.ListarTecnologias();
                                        ViewData["tiposdocumento"] = Datos.ListarTipoDocumento(IF.nivel, 1);
                                    }
                                    else
                                    {
                                        ViewData["tiposdocumento"] = Datos.ListarTipoDocumento(IF.nivel, IF.tipocentral);
                                        ViewData["tecnologias"] = Datos.ListarTecnologias();
                                    }
                                }
                                else
                                {
                                    ViewData["tiposdocumento"] = Datos.ListarTipoDocumento(IF.nivel, centroseleccionado.tipo);
                                    ViewData["tecnologias"] = Datos.ListarTecnologias();
                                }
                            }
                            else
                            {
                                ViewData["tiposdocumento"] = Datos.ListarTipoDocumento(IF.nivel, IF.tipocentral);
                                ViewData["tecnologias"] = Datos.ListarTecnologias();
                            }
                            ViewData["procesos"] = Datos.ListarProcesos();
                            ViewData["historial_doc"] = Datos.ListarLogDocumentacion(id);
                            Session["ficheroOriginal"] = id;
                            return View();
                        }
                        else
                        {
                            Session["EditarDocumentacionError"] = "Debe indicar un título para el documento";

                            documentacion IF = Datos.ObtenerFicheroDocPorID(id);
                            ViewData["fichero"] = IF;

                            if (IF.nivel == 2)
                            {
                                if (centroseleccionado.tipo == 4)
                                {
                                    if (IF.tipocentral == null)
                                    {
                                        ViewData["tecnologias"] = Datos.ListarTecnologias();
                                        ViewData["tiposdocumento"] = Datos.ListarTipoDocumento(IF.nivel, 1);
                                    }
                                    else
                                    {
                                        ViewData["tiposdocumento"] = Datos.ListarTipoDocumento(IF.nivel, IF.tipocentral);
                                        ViewData["tecnologias"] = Datos.ListarTecnologias();
                                    }
                                }
                                else
                                {
                                    ViewData["tiposdocumento"] = Datos.ListarTipoDocumento(IF.nivel, centroseleccionado.tipo);
                                    ViewData["tecnologias"] = Datos.ListarTecnologias();
                                }
                            }
                            else
                            {
                                ViewData["tiposdocumento"] = Datos.ListarTipoDocumento(IF.nivel, IF.tipocentral);
                                ViewData["tecnologias"] = Datos.ListarTecnologias();
                            }
                            ViewData["procesos"] = Datos.ListarProcesos();
                            ViewData["historial_doc"] = Datos.ListarLogDocumentacion(id);
                            Session["ficheroOriginal"] = id;
                            return View();
                        }
                        #endregion
                    }
                }
                else
                {
                    #region validacion cod documento unico
                    if (Datos.ComprobarCodigoDocumento(cod_fichero, id) == true)
                    {
                        Session["EditarDocumentacionError"] = "El código de documento ya está asignado";
                    }

                    documentacion IF = new documentacion();
                    IF.titulo = collection["ctl00$MainContent$txtTitulo"];
                    IF.cod_fichero = collection["ctl00$MainContent$txtCodigo"];
                    IF.version = collection["ctl00$MainContent$txtVersion"];
                    if (collection["ctl00$MainContent$ddlTipo"] != null)
                        IF.tipo = int.Parse(collection["ctl00$MainContent$ddlTipo"]);
                    if (collection["ctl00$MainContent$ddlNivel"] != null)
                        IF.nivel = int.Parse(collection["ctl00$MainContent$ddlNivel"]);
                    IF.estado = 0;
                    if (collection["ctl00$MainContent$ddlTecnologia"] != null && IF.nivel == 2)
                        IF.tipocentral = int.Parse(collection["ctl00$MainContent$ddlTecnologia"]);
                    if (collection["ctl00$MainContent$txtFAprobacion"] != null && collection["ctl00$MainContent$txtFAprobacion"] != "")
                        IF.fecha_aprobacion = DateTime.Parse(collection["ctl00$MainContent$txtFAprobacion"]);
                    if (collection["ctl00$MainContent$txtFPublicacion"] != null && collection["ctl00$MainContent$txtFPublicacion"] != "")
                        IF.fecha_publicacion = DateTime.Parse(collection["ctl00$MainContent$txtFPublicacion"]);
                    if (collection["ctl00$MainContent$ddlProcesos"] != null && collection["ctl00$MainContent$ddlProcesos"] != "0")
                        IF.idproceso = int.Parse(collection["ctl00$MainContent$ddlProcesos"]);

                    if (id != 0)
                    {
                        IF.idFichero = id;
                    }
                    ViewData["fichero"] = IF;

                    if (IF.nivel == 2)
                    {
                        if (centroseleccionado.tipo == 4)
                        {
                            if (IF.tipocentral == null)
                            {
                                ViewData["tecnologias"] = Datos.ListarTecnologias();
                                ViewData["tiposdocumento"] = Datos.ListarTipoDocumento(IF.nivel, 1);
                            }
                            else
                            {
                                ViewData["tiposdocumento"] = Datos.ListarTipoDocumento(IF.nivel, IF.tipocentral);
                                ViewData["tecnologias"] = Datos.ListarTecnologias();
                            }
                        }
                        else
                        {
                            ViewData["tiposdocumento"] = Datos.ListarTipoDocumento(IF.nivel, centroseleccionado.tipo);
                            ViewData["tecnologias"] = Datos.ListarTecnologias();
                        }
                    }
                    else
                    {
                        ViewData["tiposdocumento"] = Datos.ListarTipoDocumento(IF.nivel, IF.tipocentral);
                        ViewData["tecnologias"] = Datos.ListarTecnologias();
                    }
                    ViewData["procesos"] = Datos.ListarProcesos();
                    ViewData["historial_doc"] = Datos.ListarLogDocumentacion(id);

                    return View();
                    #endregion
                }
            }
            catch (Exception ex)
            {
                #region exception
                documentacion IF = new documentacion();
                IF.titulo = collection["ctl00$MainContent$txtTitulo"];
                IF.cod_fichero = collection["ctl00$MainContent$txtCodigo"];
                IF.version = collection["ctl00$MainContent$txtVersion"];
                IF.tipo = int.Parse(collection["ctl00$MainContent$ddlTipo"]);

                // Por ahora solo nivel 1
                IF.nivel = 1;
                //IF.nivel = int.Parse(collection["ctl00$MainContent$ddlNivel"]);
                IF.estado = 0;
                if (collection["ctl00$MainContent$ddlTecnologia"] != null && IF.nivel == 2)
                    IF.tipocentral = int.Parse(collection["ctl00$MainContent$ddlTecnologia"]);
                if (collection["ctl00$MainContent$txtFAprobacion"] != null && collection["ctl00$MainContent$txtFAprobacion"] != "")
                    IF.fecha_aprobacion = DateTime.Parse(collection["ctl00$MainContent$txtFAprobacion"]);
                if (collection["ctl00$MainContent$txtFPublicacion"] != null && collection["ctl00$MainContent$txtFPublicacion"] != "")
                    IF.fecha_publicacion = DateTime.Parse(collection["ctl00$MainContent$txtFPublicacion"]);
                if (id != 0)
                    IF.idFichero = id;

                Session["EditarDocumentacionError"] = "Se ha producido un error: " + ex.Message;

                ViewData["fichero"] = IF;

                if (IF.nivel == 2)
                {
                    if (centroseleccionado.tipo == 4)
                    {
                        if (IF.tipocentral == null)
                        {
                            ViewData["tecnologias"] = Datos.ListarTecnologias();
                            ViewData["tiposdocumento"] = Datos.ListarTipoDocumento(IF.nivel, 1);
                        }
                        else
                        {
                            ViewData["tiposdocumento"] = Datos.ListarTipoDocumento(IF.nivel, IF.tipocentral);
                            ViewData["tecnologias"] = Datos.ListarTecnologias();
                        }
                    }
                    else
                    {
                        ViewData["tiposdocumento"] = Datos.ListarTipoDocumento(IF.nivel, centroseleccionado.tipo);
                        ViewData["tecnologias"] = Datos.ListarTecnologias();
                    }
                }
                else
                {
                    ViewData["tiposdocumento"] = Datos.ListarTipoDocumento(IF.nivel, IF.tipocentral);
                    ViewData["tecnologias"] = Datos.ListarTecnologias();
                }
                ViewData["procesos"] = Datos.ListarProcesos();
                ViewData["historial_doc"] = Datos.ListarLogDocumentacion(id);
                Session["ficheroOriginal"] = id;
                return View();
                #endregion
            }
        }

        public ActionResult eliminar_documentacion(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            Datos.EliminarDocumentacion(id);
            Session["EditarDocumentacionResultado"] = "Fichero eliminado correctamente";
            return RedirectToAction("admrepositorio", "Documentos");
        }

        public ActionResult eliminar_documentacionhist(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }
            int idFichero = int.Parse(Session["ficheroOriginal"].ToString());
            Datos.EliminarDocumentacionHist(id);
            Session["EditarDocumentacionResultado"] = "Fichero eliminado correctamente";
            return RedirectToAction("editar_documento/" + idFichero, "Documentos");
        }

        public FileResult ObtenerDocumento(int id)
        {
            try
            {
                ficheros IF = Datos.ObtenerDocumentoPorID(id);
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

        public FileResult ObtenerDocumentoHist(int id)
        {
            try
            {
                ficheros IF = Datos.ObtenerDocumentoHistPorID(id);
                byte[] fileBytes = System.IO.File.ReadAllBytes(IF.enlace);
                string fileName = IF.enlace.Replace(Server.MapPath("~/Documentacion") + "\\", "");
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
