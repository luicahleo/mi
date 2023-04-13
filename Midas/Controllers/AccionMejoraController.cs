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
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Spreadsheet;
using MIDAS.Classes;


namespace MIDAS.Controllers
{
    public class AccionMejoraController : Controller
    {
        //
        // GET: /AccionMejora/

        public ActionResult accionesmejora()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            Session["modulo"] = "0";

            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            int idCentral = centroseleccionado.id;

            ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral);

            #region generacion fichero
            if (Session["nombreArchivo"] != null && Session["nombreArchivo"].ToString() != "")
            {
                if (Session["nombreArchivo"].ToString().Contains("xlsx"))
                {
                    Response.ContentType = "application/msexcel";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=ExportacionAccionesMejora.xlsx");
                    Response.TransmitFile(Session["nombreArchivo"].ToString());
                    Response.End();
                }
                Session["nombreArchivo"] = "";
            }
            #endregion

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult accionesmejora(FormCollection collection)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            VISTA_ObtenerUsuario user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());

            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));

            int idCentral = centroseleccionado.id;

            int anioFiltro = 0;
            if (collection["ctl00$MainContent$ddlAnio"] != "0")
                anioFiltro = int.Parse(collection["ctl00$MainContent$ddlAnio"]);

            List<VISTA_ListarAccionesMejora> listadoAcciones = Datos.ListarAccionesMejora(centroseleccionado.id);

            if (anioFiltro != 0) 
                listadoAcciones = listadoAcciones.Where(x=> x.codigo.Contains(anioFiltro.ToString())).ToList();

            #region generacion fichero
            string sourceFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaPlantillaWord"] + "ExportacionAccionesMejora.xlsx");
            string destinationFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaGeneracionWord"] + "ExportacionAccionesMejora_" + DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + ".xlsx");

            Session["source"] = sourceFile;
            Session["destino"] = destinationFile;
            // Create a copy of the template file and open the copy 
            System.IO.File.Copy(sourceFile, destinationFile, true);

            #region impresion

            using (SpreadsheetDocument document = SpreadsheetDocument.Open(destinationFile, true))
            {
                SheetData sheetData = document.WorkbookPart.WorksheetParts.ElementAt(0).Worksheet.Elements<SheetData>().First();
                foreach (VISTA_ListarAccionesMejora acc in listadoAcciones)
                {
                    Row row = new Row();

                    string anio = string.Empty;
                    string asunto = string.Empty;
                    string descripcion = string.Empty;
                    string tipo = string.Empty;
                    string responsable = string.Empty;
                    string fecha_apertura = string.Empty;
                    string referenciales = string.Empty;
                    string fecha_cierre = string.Empty;
                    string estado = string.Empty;

                    if (acc.codigo == null)
                        anio = string.Empty;
                    else
                        anio = acc.codigo;

                    if (acc.asunto == null)
                        asunto = string.Empty;
                    else
                        asunto = acc.asunto;

                    if (acc.descripcion == null)
                        descripcion = string.Empty;
                    else
                        descripcion = acc.descripcion;

                    if (acc.tipo == null)
                        tipo = string.Empty;
                    else
                        tipo = acc.tipo;

                    if (acc.Responsable == null)
                        responsable = string.Empty;
                    else
                        responsable = acc.Responsable;

                    if (acc.fecha_apertura == null)
                        fecha_apertura = string.Empty;
                    else
                        fecha_apertura = acc.fecha_apertura.ToString().Replace(" 0:00:00", "");
                    if (acc.fecha_cierre == null)
                        fecha_cierre = string.Empty;
                    else
                        fecha_cierre = acc.fecha_cierre.ToString().Replace(" 0:00:00", "");

                    switch (acc.estado)
                    {
                        case 0:
                            estado = "Abierto";
                            break;
                        case 1:
                            estado = "Cerrado eficaz";
                            break;
                        case 2:
                            estado = "Cerrado no eficaz";
                            break;
                        default:
                            estado = "Abierto";
                            break;
                    }
                    string cadenareferenciales = string.Empty;
                    if (acc.especifico != null && acc.especifico == 0)
                        cadenareferenciales = "General"; 
                    else
                    {
                       int idAccion = acc.id;
                       List<referenciales> listaReferenciales = MIDAS.Models.Datos.ListarReferencialesAsignadosAccmejora(idAccion);
                       foreach (referenciales refer in listaReferenciales)
                       {
                           if (cadenareferenciales == string.Empty)
                           {
                               cadenareferenciales = refer.nombre;
                           }
                           else
                           {
                               cadenareferenciales = cadenareferenciales + ", " + refer.nombre;
                           }
                       }
                    }
                    referenciales = cadenareferenciales;

                    row.Append(
                        Datos.ConstructCell(anio.ToString(), CellValues.String),
                        Datos.ConstructCell(asunto, CellValues.String),
                        Datos.ConstructCell(descripcion, CellValues.String),
                        Datos.ConstructCell(tipo, CellValues.String),
                        Datos.ConstructCell(responsable, CellValues.String),
                        Datos.ConstructCell(fecha_apertura, CellValues.String),
                        Datos.ConstructCell(fecha_cierre, CellValues.String),
                        Datos.ConstructCell(referenciales, CellValues.String),
                        Datos.ConstructCell(estado, CellValues.String));

                    sheetData.AppendChild(row);

                    //  Se instancian las acciones de cada objeto accion mejora, dandole valores.
                    //   @count utilizada para listar acciones

                    List<VISTA_AccionMejora_Accion> acciones = Datos.ListarAccionesAccionesMejora(acc.id);
                    short count = 0;


                    // Se iteran los objetos de acciones de la accion mejora que se esté iterando en el bucle foreach.

                    foreach (VISTA_AccionMejora_Accion accion in acciones)
                    {

                        //Aumentamos count y damos valor a las variables de la accion.
                        count++;
                        Row subRow = new Row();
                        string nombre = "Acción " + count;
                        string desc = accion.descripcion;
                        string estadoAccionAccion;

                        switch (accion.estado)
                        {
                            case 0:
                                estadoAccionAccion = "Ejecutada";
                                break;
                            default:
                                estadoAccionAccion = "Cerrado eficaz";
                                break;
                        }

                        string responsableName = accion.nombre;
                        string fechaCierre = accion.fecha_cierre.ToString();
                        //  Pasamos los datos a las celdas que corresponden.
                        subRow.Append(
                        Datos.ConstructCell("", CellValues.String),
                        Datos.ConstructCell(nombre, CellValues.String),
                        Datos.ConstructCell(desc, CellValues.String),
                        Datos.ConstructCell("", CellValues.String),
                        Datos.ConstructCell(responsableName, CellValues.String),
                        Datos.ConstructCell("", CellValues.String),
                        Datos.ConstructCell(fechaCierre, CellValues.String),
                        Datos.ConstructCell("", CellValues.String),
                        Datos.ConstructCell(estadoAccionAccion, CellValues.String));
                        //  Pasamos la fila al éxcel y pasamos a la siguiente acción.
                        sheetData.Append(subRow);
                    }
                }                

                // save worksheet
                document.WorkbookPart.WorksheetParts.First().Worksheet.Save();

                document.WorkbookPart.Workbook.Save();

                Session["nombreArchivo"] = destinationFile;
            }
            #endregion
            #endregion
            ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral);

            return RedirectToAction("accionesmejora", "AccionMejora");
        }

        public ActionResult detalle_accion(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }
            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            int idCentral = centroseleccionado.id;

            accionesmejora acc = Datos.GetDatosAccionMejora(id);
            ViewData["accionmejora"] = acc;
            
            //ACCIONMEJORA DESDE OTROS MODULOS
            if (Session["ModuloAccionMejora"] != null && Session["ReferenciaAccionMejora"] != null)
            {
                int ModuloAccionMejora = int.Parse(Session["ModuloAccionMejora"].ToString());
                int ReferenciaAccionMejora = int.Parse(Session["ReferenciaAccionMejora"].ToString());
                ViewData["modulos"] = Datos.ListarModulos().Where(x=>x.id == ModuloAccionMejora).ToList();
                ViewData["referencias"] = ListadoReferencias(ModuloAccionMejora, centroseleccionado.id).Where(x=>x.id == ReferenciaAccionMejora).ToList();
            }
            else
            {
                if (acc != null && acc.antecedente != 0)
                {
                    ViewData["referencias"] = ListadoReferencias(acc.antecedente, centroseleccionado.id);
                }
                ViewData["modulos"] = Datos.ListarModulos();
            }
            ViewData["tiposaccionmejora"] = Datos.ListarTiposAccionMejora();
            ViewData["responsables"] = Datos.ListarUsuariosCentral(idCentral);
            ViewData["ambitos"] = Datos.ListarAmbitos();
            ViewData["referencialesAsignables"] = Datos.ListarReferencialesAsignablesAccMejora(id);
            ViewData["referencialesAsignadas"] = Datos.ListarReferencialesAsignadosAccmejora(id);                
            ViewData["procesos"] = Datos.ListarProcesos();
            ViewData["acciones"] = Datos.ListarAccionesAccionesMejora(id);

            ViewData["documentosaccionesmejora"] = Datos.ListarDocumentosAccionMejora(id);

            #region generacion fichero
            if (Session["nombreArchivo"] != null && Session["nombreArchivo"].ToString() != "")
            {
                if (Session["nombreArchivo"].ToString().Contains("docx"))
                {
                    Response.ContentType = "application/msword";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=FichaAccionMejora.docx");
                    Response.TransmitFile(Session["nombreArchivo"].ToString());
                    Response.End();
                }
                Session["nombreArchivo"] = "";
            }
            #endregion

            return View();
        }

        public ActionResult eliminar_docaccmejora(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            int idAccMejora = Datos.EliminarDocAccMejora(id);
            Session["EdicionAccionMejoraMensaje"] = "Documentación eliminada";
            return RedirectToAction("detalle_accion/" + idAccMejora, "AccionMejora");
        }

        public FileResult ObtenerDocAccMejora(int id)
        {
            try
            {
                accionmejora_documento IF = Datos.GetDatosDocAccMejora(id);
                byte[] fileBytes = System.IO.File.ReadAllBytes(IF.enlace);
                string fileName = IF.enlace.Replace(Server.MapPath("~/AccionMejora") + "\\" + IF.id + "\\", "");
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult detalle_accion(int id, FormCollection collection, HttpPostedFileBase file)
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

            if (formulario == "GuardarAccionMejora")
            {
                #region guardar
                try
                {
                    if (id != 0)
                    {
                        #region actualizar
                        accionesmejora actualizar = Datos.GetDatosAccionMejora(id);
                        actualizar.asunto = collection["ctl00$MainContent$txtAsunto"];
                        if (collection["ctl00$MainContent$txtFApertura"] != "")
                            actualizar.fecha_apertura = DateTime.Parse(collection["ctl00$MainContent$txtFApertura"]);
                        else
                            actualizar.fecha_apertura = null;
                        if (collection["ctl00$MainContent$txtFCierre"] != "")
                            actualizar.fecha_cierre = DateTime.Parse(collection["ctl00$MainContent$txtFCierre"]);
                        else
                            actualizar.fecha_cierre = null;
                        actualizar.ambito = int.Parse(collection["ctl00$MainContent$ddlAmbito"]);
                        if (collection["ctl00$MainContent$ddlResponsable"] != null && collection["ctl00$MainContent$ddlResponsable"] != "0")
                            actualizar.responsable = int.Parse(collection["ctl00$MainContent$ddlResponsable"]);
                        if (collection["ctl00$MainContent$ddlEstado"] != null)
                            actualizar.estado = int.Parse(collection["ctl00$MainContent$ddlEstado"]);
                        if (collection["ctl00$MainContent$ddlTipo"] != null)
                            actualizar.tipo = int.Parse(collection["ctl00$MainContent$ddlTipo"]);
                        if (collection["ctl00$MainContent$ddlProceso"] != null)
                            actualizar.proceso = int.Parse(collection["ctl00$MainContent$ddlProceso"]);
                        if (collection["ctl00$MainContent$ddlContratista"] != null)
                            actualizar.contratista = int.Parse(collection["ctl00$MainContent$ddlContratista"]);
                        actualizar.detectadopor = collection["ctl00$MainContent$txtDetectado"];
                        actualizar.causas = collection["ctl00$MainContent$txtCausa"];
                        actualizar.descripcion = collection["ctl00$MainContent$txtDescripcion"];
                        actualizar.personasinv = collection["ctl00$MainContent$txtPersonasInvolucradas"];
                        actualizar.antecedente = int.Parse(collection["ctl00$MainContent$ddlAntecedente"]);
                        if (actualizar.antecedente != 0 && actualizar.antecedente != 9)
                            actualizar.referencia = int.Parse(collection["ctl00$MainContent$ddlReferencia"]);
                        else
                            actualizar.referencianoconforme = collection["ctl00$MainContent$txtReferencia"];

                        actualizar.ai_descripcion = collection["ctl00$MainContent$txtDescripcionAI"];
                        if (collection["ctl00$MainContent$ddlResponsableAI"] != null && collection["ctl00$MainContent$ddlResponsableAI"] != "0")
                            actualizar.ai_responsable = int.Parse(collection["ctl00$MainContent$ddlResponsableAI"]);
                        if (collection["ctl00$MainContent$txtFFinAI"] != "")
                            actualizar.ai_ffin_prevista = DateTime.Parse(collection["ctl00$MainContent$txtFFinAI"]);
                        else
                            actualizar.ai_ffin_prevista = null;
                        if (collection["ctl00$MainContent$txtFCierreAI"] != "")
                            actualizar.ai_fcierre = DateTime.Parse(collection["ctl00$MainContent$txtFCierreAI"]);
                        else
                            actualizar.ai_fcierre = null;

                        if (collection["ctl00$MainContent$ddlEstadoAI"] != null)
                            actualizar.ai_estado = int.Parse(collection["ctl00$MainContent$ddlEstadoAI"]);
                        actualizar.ai_comentario = collection["ctl00$MainContent$txtComentarioAI"];
                        if (collection["ctl00$MainContent$ddlEspecifico"] != null && collection["ctl00$MainContent$ddlEspecifico"] != "0")
                            actualizar.especifico = int.Parse(collection["ctl00$MainContent$ddlEspecifico"]);



                        Datos.ActualizarAccionMejora(actualizar);

                        if (actualizar.especifico == 1)
                        {
                            string hdnReferenciales = collection["ctl00$MainContent$hdnReferencialesSeleccionados"].ToString();

                            string[] arrayReferenciales = hdnReferenciales.Split(new char[] { ';' });

                            Datos.EliminarAsociacionAccMejoraReferencial(id);

                            if (arrayReferenciales.Count() > 0)
                            {
                                for (int i = 0; i < arrayReferenciales.Count() - 1; i++)
                                {
                                    Datos.AsociarAccMejoraReferencial(id, int.Parse(arrayReferenciales[i]));
                                }
                            }
                        }

                        Session["EdicionAccionMejoraMensaje"] = "Información actualizada correctamente";
                        accionesmejora acc = Datos.GetDatosAccionMejora(id);
                        ViewData["accionmejora"] = acc;
                        if (acc != null && acc.antecedente != 0)
                        {
                            ViewData["referencias"] = ListadoReferencias(acc.antecedente, centroseleccionado.id);
                        }
                        ViewData["tiposaccionmejora"] = Datos.ListarTiposAccionMejora();
                        ViewData["responsables"] = Datos.ListarUsuariosCentral(idCentral);
                        ViewData["ambitos"] = Datos.ListarAmbitos();
                        ViewData["referencialesAsignables"] = Datos.ListarReferencialesAsignablesAccMejora(id);
                        ViewData["referencialesAsignadas"] = Datos.ListarReferencialesAsignadosAccmejora(id);      
                        ViewData["modulos"] = Datos.ListarModulos();
                        ViewData["procesos"] = Datos.ListarProcesos();
                        ViewData["acciones"] = Datos.ListarAccionesAccionesMejora(id);
                        ViewData["documentosaccionesmejora"] = Datos.ListarDocumentosAccionMejora(id);
                        return View();
                        #endregion
                    }
                    else
                    {
                        #region insertar
                        accionesmejora insertar = new accionesmejora();
                        insertar.idcentral = centroseleccionado.id;
                        insertar.anio = int.Parse(collection["ctl00$MainContent$ddlAnio"]);
                        insertar.asunto = collection["ctl00$MainContent$txtAsunto"];
                        if (collection["ctl00$MainContent$txtFApertura"] != "")
                            insertar.fecha_apertura = DateTime.Parse(collection["ctl00$MainContent$txtFApertura"]);
                        if (collection["ctl00$MainContent$txtFCierre"] != "")
                            insertar.fecha_cierre = DateTime.Parse(collection["ctl00$MainContent$txtFCierre"]);
                        insertar.ambito = int.Parse(collection["ctl00$MainContent$ddlAmbito"]);
                        if (collection["ctl00$MainContent$ddlResponsable"] != null && collection["ctl00$MainContent$ddlResponsable"] != "0")
                            insertar.responsable = int.Parse(collection["ctl00$MainContent$ddlResponsable"]);
                        if (collection["ctl00$MainContent$ddlEstado"] != null)
                            insertar.estado = int.Parse(collection["ctl00$MainContent$ddlEstado"]);
                        if (collection["ctl00$MainContent$ddlTipo"] != null)
                            insertar.tipo = int.Parse(collection["ctl00$MainContent$ddlTipo"]);
                        if (collection["ctl00$MainContent$ddlProceso"] != null)
                            insertar.proceso = int.Parse(collection["ctl00$MainContent$ddlProceso"]);
                        if (collection["ctl00$MainContent$ddlContratista"] != null )
                            insertar.contratista = int.Parse(collection["ctl00$MainContent$ddlContratista"]);
                        insertar.antecedente = int.Parse(collection["ctl00$MainContent$ddlAntecedente"]);
                        if (insertar.antecedente != 0 && insertar.antecedente != 9)
                            insertar.referencia = int.Parse(collection["ctl00$MainContent$ddlReferencia"]);
                        else
                            insertar.referencianoconforme = collection["ctl00$MainContent$txtReferencia"];
                        insertar.causas = collection["ctl00$MainContent$txtCausa"];
                        insertar.descripcion = collection["ctl00$MainContent$txtDescripcion"];
                        insertar.personasinv = collection["ctl00$MainContent$txtPersonasInvolucradas"];

                        insertar.ai_descripcion = collection["ctl00$MainContent$txtDescripcionAI"];
                        if (collection["ctl00$MainContent$ddlResponsableAI"] != null && collection["ctl00$MainContent$ddlResponsableAI"] != "0")
                            insertar.ai_responsable = int.Parse(collection["ctl00$MainContent$ddlResponsableAI"]);
                        if (collection["ctl00$MainContent$txtFFinAI"] != "")
                            insertar.ai_ffin_prevista = DateTime.Parse(collection["ctl00$MainContent$txtFFinAI"]);
                        if (collection["ctl00$MainContent$txtFCierreAI"] != "")
                            insertar.ai_fcierre = DateTime.Parse(collection["ctl00$MainContent$txtFCierreAI"]);

                        if (collection["ctl00$MainContent$ddlEstadoAI"] != null)
                            insertar.ai_estado = int.Parse(collection["ctl00$MainContent$ddlEstadoAI"]);
                        insertar.ai_comentario = collection["ctl00$MainContent$txtComentarioAI"];
                        if (collection["ctl00$MainContent$ddlEspecifico"] != null && collection["ctl00$MainContent$ddlEspecifico"] != "0")
                            insertar.especifico = int.Parse(collection["ctl00$MainContent$ddlEspecifico"]);
                        insertar.detectadopor = collection["ctl00$MainContent$txtDetectado"];
                        int idForm = Datos.ActualizarAccionMejora(insertar);

                        if (insertar.especifico == 1)
                        {
                            string hdnReferenciales = collection["ctl00$MainContent$hdnReferencialesSeleccionados"].ToString();

                            string[] arrayReferenciales = hdnReferenciales.Split(new char[] { ';' });

                            Datos.EliminarAsociacionAccMejoraReferencial(idForm);

                            if (arrayReferenciales.Count() > 0)
                            {
                                for (int i = 0; i < arrayReferenciales.Count() - 1; i++)
                                {
                                    Datos.AsociarAccMejoraReferencial(idForm, int.Parse(arrayReferenciales[i]));
                                }
                            }
                        }

                        Session["EdicionAccionMejoraMensaje"] = "Información actualizada correctamente";

                        accionesmejora acc = Datos.GetDatosAccionMejora(id);
                        ViewData["accionmejora"] = acc;
                        if (acc != null && acc.antecedente != 0)
                        {
                            ViewData["referencias"] = ListadoReferencias(acc.antecedente, centroseleccionado.id);
                        }
                        ViewData["tiposaccionmejora"] = Datos.ListarTiposAccionMejora();
                        ViewData["responsables"] = Datos.ListarUsuariosCentral(idCentral);
                        ViewData["ambitos"] = Datos.ListarAmbitos();
                        ViewData["referencialesAsignables"] = Datos.ListarReferencialesAsignablesAccMejora(id);
                        ViewData["referencialesAsignadas"] = Datos.ListarReferencialesAsignadosAccmejora(id);      
                        ViewData["modulos"] = Datos.ListarModulos();
                        ViewData["procesos"] = Datos.ListarProcesos();
                        ViewData["acciones"] = Datos.ListarAccionesAccionesMejora(id);
                        ViewData["documentosaccionesmejora"] = Datos.ListarDocumentosAccionMejora(id);
                        return Redirect(Url.RouteUrl(new { controller = "AccionMejora", action = "detalle_accion", id = idForm }));
                        #endregion
                    }

                }
                catch (Exception ex)
                {
                    #region exception
                    accionesmejora acc = Datos.GetDatosAccionMejora(id);
                    ViewData["accionmejora"] = acc;
                    if (acc != null && acc.antecedente != 0)
                    {
                        ViewData["referencias"] = ListadoReferencias(acc.antecedente, centroseleccionado.id);
                    }
                    ViewData["tiposaccionmejora"] = Datos.ListarTiposAccionMejora();
                    ViewData["responsables"] = Datos.ListarUsuariosCentral(idCentral);
                    ViewData["ambitos"] = Datos.ListarAmbitos();
                    ViewData["referencialesAsignables"] = Datos.ListarReferencialesAsignablesAccMejora(id);
                    ViewData["referencialesAsignadas"] = Datos.ListarReferencialesAsignadosAccmejora(id);      
                    ViewData["modulos"] = Datos.ListarModulos();
                    ViewData["procesos"] = Datos.ListarProcesos();
                    ViewData["acciones"] = Datos.ListarAccionesAccionesMejora(id);
                    ViewData["documentosaccionesmejora"] = Datos.ListarDocumentosAccionMejora(id);
                    return View();
                    #endregion
                }
                #endregion
            }

            if (formulario == "btnAddDocumento")
            {
                #region add documento
                if (file != null && file.ContentLength > 0 && collection["ctl00$MainContent$txtNombreDoc"] != null)
                {
                    #region guardar fichero cualificacion
                    var fileName = System.IO.Path.GetFileName(file.FileName);

                    var path = System.IO.Path.Combine(Server.MapPath("~/AccionMejora/" + id), fileName);

                    if (Directory.Exists(Server.MapPath("~/AccionMejora/" + id)))
                    {
                        file.SaveAs(path);
                    }
                    else
                    {
                        Directory.CreateDirectory(Server.MapPath("~/AccionMejora/" + id));
                        file.SaveAs(path);
                    }

                    accionmejora_documento IF = new accionmejora_documento();
                    IF.nombre = collection["ctl00$MainContent$txtNombreDoc"];
                    IF.nombrefichero = file.FileName;
                    IF.enlace = path;
                    IF.idaccionmejora = id;
                    IF.fecha = DateTime.Now.Date;
                    Datos.InsertDocAccMejora(IF);
                    #endregion
                }

                Session["EdicionAccionMejoraMensaje"] = "Información actualizada correctamente";
                accionesmejora acc = Datos.GetDatosAccionMejora(id);
                ViewData["accionmejora"] = acc;
                if (acc != null && acc.antecedente != 0)
                {
                    ViewData["referencias"] = ListadoReferencias(acc.antecedente, centroseleccionado.id);
                }
                ViewData["tiposaccionmejora"] = Datos.ListarTiposAccionMejora();
                ViewData["responsables"] = Datos.ListarUsuariosCentral(idCentral);
                ViewData["ambitos"] = Datos.ListarAmbitos();
                ViewData["referencialesAsignables"] = Datos.ListarReferencialesAsignablesAccMejora(id);
                ViewData["referencialesAsignadas"] = Datos.ListarReferencialesAsignadosAccmejora(id);
                ViewData["modulos"] = Datos.ListarModulos();
                ViewData["procesos"] = Datos.ListarProcesos();
                ViewData["acciones"] = Datos.ListarAccionesAccionesMejora(id);
                ViewData["documentosaccionesmejora"] = Datos.ListarDocumentosAccionMejora(id);
                return View();
                #endregion
            }

            if (formulario == "btnNuevaAccion")
            {
                #region nueva accion
                try
                {
                    try
                    {
                        if (collection["ctl00$MainContent$txtDescripcionAccion"] != null)
                        {
                            #region alta y modificacion
                            accionmejora_accion nuevaAccion = new accionmejora_accion();

                            nuevaAccion.descripcion = collection["ctl00$MainContent$txtDescripcionAccion"];
                            if (collection["ctl00$MainContent$ddlResponsableAccion"] != null)
                                nuevaAccion.responsable = int.Parse(collection["ctl00$MainContent$ddlResponsableAccion"].ToString());
                            nuevaAccion.comentario = collection["ctl00$MainContent$txtComentariosAccion"];
                            nuevaAccion.estado = int.Parse(collection["ctl00$MainContent$ddlEstadoAccion"]);
                            if (collection["ctl00$MainContent$txtNumAccion"] != null && collection["ctl00$MainContent$txtNumAccion"] != "")
                                nuevaAccion.numaccion = int.Parse(collection["ctl00$MainContent$txtNumAccion"].ToString());
                            if (collection["ctl00$MainContent$txtFechaFinAccion"] != null && collection["ctl00$MainContent$txtFechaFinAccion"] != "")
                                nuevaAccion.fecha_fin = DateTime.Parse(collection["ctl00$MainContent$txtFechaFinAccion"].ToString());
                            if (collection["ctl00$MainContent$txtFechaCierreAccion"] != null && collection["ctl00$MainContent$txtFechaCierreAccion"] != "")
                                nuevaAccion.fecha_cierre = DateTime.Parse(collection["ctl00$MainContent$txtFechaCierreAccion"].ToString());
                            nuevaAccion.idaccionmejora = id;

                            if (collection["ctl00$MainContent$hdnIdAccion"] != null && collection["ctl00$MainContent$hdnIdAccion"] != "0")
                            {
                                nuevaAccion.id = int.Parse(collection["ctl00$MainContent$hdnIdAccion"]);
                                Datos.ModificarAccionAccionMejora(nuevaAccion);
                            }
                            else
                            {
                                Datos.InsertarAccionAccionMejora(nuevaAccion);
                            }

                            if (nuevaAccion.id == 0)
                                Session["EdicionAccionMejoraMensaje"] = "Acción añadida correctamente";
                            else
                                Session["EdicionAccionMejoraMensaje"] = "Acción actualizada correctamente";
                            #endregion
                        }
                        else
                        {
                            #region recarga
                            accionesmejora acc = Datos.GetDatosAccionMejora(id);
                            ViewData["accionmejora"] = acc;
                            if (acc != null && acc.antecedente != 0)
                            {
                                ViewData["referencias"] = ListadoReferencias(acc.antecedente, centroseleccionado.id);
                            }
                            ViewData["tiposaccionmejora"] = Datos.ListarTiposAccionMejora();
                            ViewData["responsables"] = Datos.ListarUsuariosCentral(idCentral);
                            ViewData["ambitos"] = Datos.ListarAmbitos();
                            ViewData["referencialesAsignables"] = Datos.ListarReferencialesAsignablesAccMejora(id);
                            ViewData["referencialesAsignadas"] = Datos.ListarReferencialesAsignadosAccmejora(id);      
                            ViewData["modulos"] = Datos.ListarModulos();
                            ViewData["procesos"] = Datos.ListarProcesos();
                            ViewData["acciones"] = Datos.ListarAccionesAccionesMejora(id);
                            ViewData["documentosaccionesmejora"] = Datos.ListarDocumentosAccionMejora(id);
                            Session["EdicionObjetivoError"] = "Se ha producido un error, compruebe que los datos son válidos y han sido cumplimentados";

                            return View();
                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        #region exception
                        accionesmejora acc = Datos.GetDatosAccionMejora(id);
                        ViewData["accionmejora"] = acc;
                        if (acc != null && acc.antecedente != 0)
                        {
                            ViewData["referencias"] = ListadoReferencias(acc.antecedente, centroseleccionado.id);
                        }
                        ViewData["tiposaccionmejora"] = Datos.ListarTiposAccionMejora();
                        ViewData["responsables"] = Datos.ListarUsuariosCentral(idCentral);
                        ViewData["ambitos"] = Datos.ListarAmbitos();
                        ViewData["referencialesAsignables"] = Datos.ListarReferencialesAsignablesAccMejora(id);
                        ViewData["referencialesAsignadas"] = Datos.ListarReferencialesAsignadosAccmejora(id);      
                        ViewData["modulos"] = Datos.ListarModulos();
                        ViewData["procesos"] = Datos.ListarProcesos();
                        ViewData["acciones"] = Datos.ListarAccionesAccionesMejora(id);
                        ViewData["documentosaccionesmejora"] = Datos.ListarDocumentosAccionMejora(id);
                        Session["EdicionObjetivoError"] = "Se ha producido un error, compruebe que los datos son válidos y han sido cumplimentados";

                        return View();
                        #endregion
                    }
                    #region recarga
                    accionesmejora acci = Datos.GetDatosAccionMejora(id);
                    ViewData["accionmejora"] = acci;
                    if (acci != null && acci.antecedente != 0)
                    {
                        ViewData["referencias"] = ListadoReferencias(acci.antecedente, centroseleccionado.id);
                    }
                    ViewData["tiposaccionmejora"] = Datos.ListarTiposAccionMejora();
                    ViewData["responsables"] = Datos.ListarUsuariosCentral(idCentral);
                    ViewData["ambitos"] = Datos.ListarAmbitos();
                    ViewData["referencialesAsignables"] = Datos.ListarReferencialesAsignablesAccMejora(id);
                    ViewData["referencialesAsignadas"] = Datos.ListarReferencialesAsignadosAccmejora(id);      
                    ViewData["modulos"] = Datos.ListarModulos();
                    ViewData["procesos"] = Datos.ListarProcesos();
                    ViewData["acciones"] = Datos.ListarAccionesAccionesMejora(id);
                    ViewData["documentosaccionesmejora"] = Datos.ListarDocumentosAccionMejora(id);
                    #endregion

                    return View();
                }
                catch (Exception ex)
                {
                    #region exception
                    accionesmejora acci = Datos.GetDatosAccionMejora(id);
                    ViewData["accionmejora"] = acci;
                    if (acci != null && acci.antecedente != 0)
                    {
                        ViewData["referencias"] = ListadoReferencias(acci.antecedente, centroseleccionado.id);
                    }
                    ViewData["tiposaccionmejora"] = Datos.ListarTiposAccionMejora();
                    ViewData["responsables"] = Datos.ListarUsuariosCentral(idCentral);
                    ViewData["ambitos"] = Datos.ListarAmbitos();
                    ViewData["referencialesAsignables"] = Datos.ListarReferencialesAsignablesAccMejora(id);
                    ViewData["referencialesAsignadas"] = Datos.ListarReferencialesAsignadosAccmejora(id);      
                    ViewData["modulos"] = Datos.ListarModulos();
                    ViewData["procesos"] = Datos.ListarProcesos();
                    ViewData["acciones"] = Datos.ListarAccionesAccionesMejora(id);
                    ViewData["documentosaccionesmejora"] = Datos.ListarDocumentosAccionMejora(id);
                    return View();
                    #endregion
                }
                #endregion
            }

            if (formulario == "btnImprimir")
            {
                #region imprimir
                string sourceFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaPlantillaWord"] + "FichaAccionMejora.docx");
                string destinationFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaGeneracionWord"] + "FichaAccionMejora_" + DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + ".docx");

                Session["source"] = sourceFile;
                Session["destino"] = destinationFile;
                // Create a copy of the template file and open the copy 
                System.IO.File.Copy(sourceFile, destinationFile, true);

                #region impresion
                Vista_AccionMejoraFicha actualizar = Datos.GetDatosAccionMejoraFicha(id);


                // create key value pair, key represents words to be replace and 
                //values represent values in document in place of keys.
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add("T_Central", centroseleccionado.siglas.Replace("\r\n", "<w:br/>"));
                keyValues.Add("T_Codigo", actualizar.codigo.Replace("\r\n", "<w:br/>"));
                keyValues.Add("T_Asunto", actualizar.asunto.Replace("\r\n", "<w:br/>"));
                if (actualizar.fecha_apertura != null)
                    keyValues.Add("T_FechaApertura", actualizar.fecha_apertura.ToString().Replace(" 0:00:00", "").Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_FechaApertura", "");
                keyValues.Add("T_Tipo", actualizar.tipo.Replace("\r\n", "<w:br/>"));
                if (actualizar.antecedente != null)
                    keyValues.Add("T_Antecedente", actualizar.antecedente.Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_Antecedente", "");

                accionesmejora acci = Datos.GetDatosAccionMejora(id);
                ItemDesplegable refer = ListadoReferencias(acci.antecedente, centroseleccionado.id).Where(x=>x.id == actualizar.referencia).FirstOrDefault();
                if (actualizar.referencia != null)
                    keyValues.Add("T_Referencia", refer.nombre.ToString().Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_Referencia", actualizar.referencianoconforme);

                if (actualizar.contratista == 1)
                    keyValues.Add("T_Contratista", "Sí");
                else
                    keyValues.Add("T_Contratista", "No");
                keyValues.Add("T_Detectada", actualizar.detectadopor.Replace("\r\n", "<w:br/>"));
                
                if (actualizar.proceso != null)
                    keyValues.Add("T_Proceso", actualizar.proceso.Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_Proceso", string.Empty);
                keyValues.Add("T_Causas", actualizar.causas.Replace("\r\n", "<w:br/>"));

                if (actualizar.ai_descripcion != null)
                    keyValues.Add("T_DescripcionAI", actualizar.ai_descripcion.Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_DescripcionAI", "");

                if (actualizar.descripcion != null)
                    keyValues.Add("T_Descripcion", actualizar.descripcion.Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_Descripcion", "");

                if (actualizar.personasinv != null)
                    keyValues.Add("T_PersonasInv", actualizar.personasinv.Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_PersonasInv", "");

                if (actualizar.ambito == 0)
                {
                    keyValues.Add("T_Ambito", "General");
                }
                else
                {
                    ambitos amb = Datos.ObtenerAmbito(actualizar.ambito);
                    keyValues.Add("T_Ambito", amb.nombre_ambito);
                }

                

                if (actualizar.ai_responsable != null)
                    keyValues.Add("T_ResponsableAI", actualizar.ai_responsable.Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_ResponsableAI", "");

                if (actualizar.ai_ffin_prevista != null)
                    keyValues.Add("T_FechaFinAi", actualizar.ai_ffin_prevista.ToString().Replace(" 0:00:00", "").Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_FechaFinAi", "");

                if (actualizar.ai_fcierre != null)
                    keyValues.Add("T_FechaCierreAI", actualizar.ai_fcierre.ToString().Replace(" 0:00:00", "").Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_FechaCierreAI", "");

                switch (actualizar.ai_estado)
                {
                    case 0:
                        keyValues.Add("T_EstadoAI", "No ejecutada");
                        break;
                    case 1:
                        keyValues.Add("T_EstadoAI", "Ejecutada");
                        break;
                    case 2:
                        keyValues.Add("T_EstadoAI", "En ejecucion");
                        break;
                    default:
                        keyValues.Add("T_EstadoAI", "");
                        break;
                }
                    

                if (actualizar.ai_comentario != null)
                    keyValues.Add("T_ComentarioAI", actualizar.ai_comentario.Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_ComentarioAI", "");

                SearchAndReplace(destinationFile, keyValues);

                WordprocessingDocument doc = WordprocessingDocument.Open(destinationFile, true);

                //LISTADO DESPLIEGUE
                List<VISTA_ListarAccionesMejoraFicha> despliegue = Datos.ListarAccionesMejoraFicha(id);
                

                if (despliegue.Count > 1)
                {
                    for (int i = 1; i < despliegue.Count; i++)
                    {
                        DocumentFormat.OpenXml.Wordprocessing.Table clon = (DocumentFormat.OpenXml.Wordprocessing.Table)doc.MainDocumentPart.Document.Body.Descendants<DocumentFormat.OpenXml.Wordprocessing.Table>().ElementAt(3).Clone();

                        doc.MainDocumentPart.Document.Body.Append(clon);
                        doc.MainDocumentPart.Document.Body.Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph());

                    }

                }

                if (despliegue.Count > 0)
                {
                    for (int i = 0; i < despliegue.Count; i++)
                    {
                        DocumentFormat.OpenXml.Wordprocessing.Table tabladespliegue = doc.MainDocumentPart.Document.Body.Descendants<DocumentFormat.OpenXml.Wordprocessing.Table>().ElementAt(i + 3);

                        var runPropertiesDespliegue1 = new DocumentFormat.OpenXml.Wordprocessing.RunProperties();
                        runPropertiesDespliegue1.AppendChild(new RunFonts() { Ascii = "Calibri" });
                        runPropertiesDespliegue1.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "22" });

                        var filadespliegue1 = tabladespliegue.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>().ElementAt(1);

                        DocumentFormat.OpenXml.Wordprocessing.TableCell celdanaccion = filadespliegue1.Elements<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(0);
                        var parrafo = celdanaccion.Elements<DocumentFormat.OpenXml.Wordprocessing.Paragraph>().ElementAt(0);
                        var runprocesos = parrafo.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Run());
                        runprocesos.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesDespliegue1);
                        runprocesos.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Text(despliegue[i].descripcion));

                        var filadespliegue3 = tabladespliegue.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>().ElementAt(3);

                        var runPropertiesDespliegue2 = new DocumentFormat.OpenXml.Wordprocessing.RunProperties();
                        runPropertiesDespliegue2.AppendChild(new RunFonts() { Ascii = "Calibri" });
                        runPropertiesDespliegue2.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "22" });

                        DocumentFormat.OpenXml.Wordprocessing.TableCell celdacomentario = filadespliegue3.Elements<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(0);
                        var parrafocomentario = celdacomentario.Elements<DocumentFormat.OpenXml.Wordprocessing.Paragraph>().ElementAt(0);
                        var runprocesoscomentario = parrafocomentario.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Run());
                        runprocesoscomentario.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesDespliegue2);
                        runprocesoscomentario.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Text(despliegue[i].comentario));


                        var filadespliegue2 = tabladespliegue.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>().ElementAt(5);

                        var runPropertiesDespliegue3 = new DocumentFormat.OpenXml.Wordprocessing.RunProperties();
                        runPropertiesDespliegue3.AppendChild(new RunFonts() { Ascii = "Calibri" });
                        runPropertiesDespliegue3.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "22" });

                        DocumentFormat.OpenXml.Wordprocessing.TableCell celdaestado = filadespliegue2.Elements<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(0);
                        var parrafoestado = celdaestado.Elements<DocumentFormat.OpenXml.Wordprocessing.Paragraph>().ElementAt(0);
                        var runestado = parrafoestado.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Run());
                        runestado.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesDespliegue3);
                        switch (despliegue[i].estado)
                        {
                            case 0:
                                runestado.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Text("Ejecutado"));
                                break;
                            case 1:
                                runestado.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Text("No Ejecutado"));
                                break;
                            case 2:
                                runestado.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Text("En ejecucion"));
                                break;
                        }


                        var runPropertiesDespliegue4 = new DocumentFormat.OpenXml.Wordprocessing.RunProperties();
                        runPropertiesDespliegue4.AppendChild(new RunFonts() { Ascii = "Calibri" });
                        runPropertiesDespliegue4.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "22" });

                        DocumentFormat.OpenXml.Wordprocessing.TableCell celdaresponsable = filadespliegue2.Elements<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(1);
                        var parraforesponsable = celdaresponsable.Elements<DocumentFormat.OpenXml.Wordprocessing.Paragraph>().ElementAt(0);
                        var runresponsable = parraforesponsable.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Run());
                        runresponsable.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesDespliegue4);
                        runresponsable.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Text(despliegue[i].responsable));


                        var runPropertiesDespliegue5 = new DocumentFormat.OpenXml.Wordprocessing.RunProperties();
                        runPropertiesDespliegue5.AppendChild(new RunFonts() { Ascii = "Calibri" });
                        runPropertiesDespliegue5.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "22" });

                        DocumentFormat.OpenXml.Wordprocessing.TableCell celdafechafin = filadespliegue2.Elements<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(2);
                        var parrafofechafin = celdafechafin.Elements<DocumentFormat.OpenXml.Wordprocessing.Paragraph>().ElementAt(0);
                        var runfechafin = parrafofechafin.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Run());
                        runfechafin.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesDespliegue5);
                        runfechafin.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Text(despliegue[i].fecha_fin.ToString().Replace(" 0:00:00", "")));


                        var runPropertiesDespliegue6 = new DocumentFormat.OpenXml.Wordprocessing.RunProperties();
                        runPropertiesDespliegue6.AppendChild(new RunFonts() { Ascii = "Calibri" });
                        runPropertiesDespliegue6.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "22" });

                        DocumentFormat.OpenXml.Wordprocessing.TableCell celdafechacierre = filadespliegue2.Elements<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(3);
                        var parrafofechacierre = celdafechacierre.Elements<DocumentFormat.OpenXml.Wordprocessing.Paragraph>().ElementAt(0);
                        var runfechacierre = parrafofechacierre.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Run());
                        runfechacierre.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesDespliegue6);
                        runfechacierre.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Text(despliegue[i].fecha_cierre.ToString().Replace(" 0:00:00", "")));
                                              

                    }
                }

                doc.MainDocumentPart.Document.Save();

                doc.Close();

                Session["nombreArchivo"] = destinationFile;
                #endregion

                
                ViewData["accionmejora"] = acci;
                if (acci != null && acci.antecedente != 0)
                {
                    ViewData["referencias"] = ListadoReferencias(acci.antecedente, centroseleccionado.id);
                }
                ViewData["tiposaccionmejora"] = Datos.ListarTiposAccionMejora();
                ViewData["responsables"] = Datos.ListarUsuariosCentral(idCentral);
                ViewData["ambitos"] = Datos.ListarAmbitos();
                ViewData["referencialesAsignables"] = Datos.ListarReferencialesAsignablesAccMejora(id);
                ViewData["referencialesAsignadas"] = Datos.ListarReferencialesAsignadosAccmejora(id);      
                ViewData["modulos"] = Datos.ListarModulos();
                ViewData["procesos"] = Datos.ListarProcesos();
                ViewData["acciones"] = Datos.ListarAccionesAccionesMejora(id);
                ViewData["documentosaccionesmejora"] = Datos.ListarDocumentosAccionMejora(id);
                return Redirect(Url.RouteUrl(new { controller = "AccionMejora", action = "detalle_accion", id = id }));
                #endregion
            }

            if (formulario == "btnImprimirNC")
            {
                #region imprimir
                string sourceFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaPlantillaWord"] + "FormatoNC.docx");
                string destinationFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaGeneracionWord"] + "InformeNC_" + DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + ".docx");

                Session["source"] = sourceFile;
                Session["destino"] = destinationFile;
                // Create a copy of the template file and open the copy 
                System.IO.File.Copy(sourceFile, destinationFile, true);

                #region impresion
                Vista_AccionMejoraFicha actualizar = Datos.GetDatosAccionMejoraFicha(id);
                List<VISTA_AccionMejora_Accion> actualizarAccion = Datos.ListarAccionesAccionesMejora(id);
                List<accionmejora_documento> docs = Datos.ListarDocumentosAccionMejora(id);

                

                // create key value pair, key represents words to be replace and 
                //values represent values in document in place of keys.
                Dictionary<string, string> keyValues = new Dictionary<string, string>();

                //if (actualizar.codigo != null)
                //    keyValues.Add("T_CodAccion", actualizar.codigo.Replace("\r\n", "<w:br/>"));
                //else
                //    keyValues.Add("T_CodAccion", string.Empty);


                keyValues.Add("T_Centro", centroseleccionado.siglas.Replace("\r\n", "<w:br/>"));
                if (actualizar.detectadopor != null)
                    keyValues.Add("T_DetectadoPor", actualizar.detectadopor.Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_DetectadoPor", string.Empty);
                if (actualizar.descripcion != null)
                    keyValues.Add("T_Descripcion", actualizar.descripcion.Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_Descripcion", string.Empty);
                if (actualizar.fecha_apertura != null)
                    keyValues.Add("T_Fapertura", actualizar.fecha_apertura.ToString().Replace(" 0:00:00", "").Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_Fapertura", "");

                if (actualizar.ai_responsable != null)
                    keyValues.Add("T_Causas", actualizar.causas.Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_Causas", string.Empty);

                //if (actualizar. != null)
                //    keyValues.Add("T_Causas", actualizar.causas.Replace("\r\n", "<w:br/>"));
               // else
                 //   keyValues.Add("T_Causas", string.Empty);

                if (actualizar.ai_descripcion != null)
                    keyValues.Add("T_AIDescripcion", actualizar.ai_descripcion.Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_AIDescripcion", string.Empty);

                if (actualizar.ai_descripcion != null)
                    keyValues.Add("T_AIFinPrev", actualizar.ai_ffin_prevista.ToString().Replace(" 0:00:00", "").Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_AIFinPrev", string.Empty);

                if (actualizar.ai_ffin_prevista != null)
                    keyValues.Add("T_ResponsableAI", actualizar.ai_responsable.Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_ResponsableAI", string.Empty);

                if (actualizar.ai_fcierre != null)
                    keyValues.Add("T_FFin", actualizar.ai_fcierre.ToString().Replace(" 0:00:00", "").Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_FFin", string.Empty);


                int count = 1;
                while (count < 2)
                {
                    int countAccion = 1;
                    foreach (VISTA_AccionMejora_Accion accion in actualizarAccion)
                    {

                        if (accion.comentario != null)
                            keyValues.Add("T_" + countAccion + "_Desc", accion.comentario.Replace("\r\n", "<w:br/>"));
                        else
                            keyValues.Add("T_" + countAccion + "_Desc", string.Empty);

                        if (accion.nombre != null)
                            keyValues.Add("T_" +countAccion + "_Responsable", accion.nombre.Replace("\r\n", "<w:br/>"));
                        else
                            keyValues.Add("T_" +countAccion + "_Responsable", string.Empty);

                        if (accion.fecha_fin != null)
                            keyValues.Add("T_" +countAccion + "_FFinAlt", accion.fecha_fin.ToString().Replace(" 0:00:00", "").Replace("\r\n", "<w:br/>"));
                        else
                            keyValues.Add("T_" + countAccion + "_FFinAlt", string.Empty);

                        if (accion.fecha_cierre != null)
                            keyValues.Add("T_" + countAccion + "_FcierreAlt", accion.fecha_cierre.ToString().Replace(" 0:00:00", "").Replace("\r\n", "<w:br/>"));
                        else
                            keyValues.Add("T_" + countAccion + "_FcierreAlt", string.Empty);

                        countAccion++;
                    }
                    count++;
                }

                try
                {
                    keyValues.Add("T_1_Desc", string.Empty);
                    keyValues.Add("T_1_Responsable", string.Empty);
                    keyValues.Add("T_1_FFinAlt", string.Empty);
                    keyValues.Add("T_1_FcierreAlt", string.Empty);
                }
                catch (Exception ex)
                {

                }

                try
                {
                    keyValues.Add("T_2_Desc", string.Empty);
                    keyValues.Add("T_2_Responsable", string.Empty);
                    keyValues.Add("T_2_FFinAlt", string.Empty);
                    keyValues.Add("T_2_FcierreAlt", string.Empty);
                }
                catch (Exception ex)
                {

                }


                try
                {
                    keyValues.Add("T_3_Desc", string.Empty);
                    keyValues.Add("T_3_Responsable", string.Empty);
                    keyValues.Add("T_3_FFinAlt", string.Empty);
                    keyValues.Add("T_3_FcierreAlt", string.Empty);
                }
                catch (Exception ex)
                {

                }


                try
                {
                    keyValues.Add("T_4_Desc", string.Empty);
                    keyValues.Add("T_4_Responsable", string.Empty);
                    keyValues.Add("T_4_FFinAlt", string.Empty);
                    keyValues.Add("T_4_FcierreAlt", string.Empty);
                }
                catch (Exception ex)
                {

                }


                try
                {
                    keyValues.Add("T_5_Desc", string.Empty);
                    keyValues.Add("T_5_Responsable", string.Empty);
                    keyValues.Add("T_5_FFinAlt", string.Empty);
                    keyValues.Add("T_5_FcierreAlt", string.Empty);
                }
                catch (Exception ex)
                {

                }

                count = 0;
                if (docs.Count() > 0)
                {
                    string documentos = string.Empty;
                    foreach (accionmejora_documento doc in docs)
                    {
                        documentos = documentos + doc.nombre + "<w:br/>";
                    }
                    keyValues.Add("T_Documentos", documentos.Replace(" ", "-")) ;
                }
                else
                    keyValues.Add("T_Documentos", string.Empty);

                SearchAndReplace(destinationFile, keyValues);

                #region cabecera
                WordprocessingDocument docu = WordprocessingDocument.Open(destinationFile, true);
                foreach (HeaderPart header in docu.MainDocumentPart.HeaderParts)
                {
                    string headerText = null;
                    using (StreamReader sr = new StreamReader(header.GetStream()))
                    {
                        headerText = sr.ReadToEnd();
                    }

                    headerText = headerText.Replace("T_CodAccion", actualizar.codigo.Replace("\r\n", "<w:br/>"));

                    using (StreamWriter sw = new StreamWriter(header.GetStream(FileMode.Create)))
                    {
                        sw.Write(headerText);
                    }

                    //Save Header
                    header.Header.Save();

                }
                docu.MainDocumentPart.Document.Save();

                docu.Close();
                #endregion

                accionesmejora acci = Datos.GetDatosAccionMejora(id);

                Session["nombreArchivo"] = destinationFile;
                #endregion


                ViewData["accionmejora"] = acci;
                if (acci != null && acci.antecedente != 0)
                {
                    ViewData["referencias"] = ListadoReferencias(acci.antecedente, centroseleccionado.id);
                }
                ViewData["tiposaccionmejora"] = Datos.ListarTiposAccionMejora();
                ViewData["responsables"] = Datos.ListarUsuariosCentral(idCentral);
                ViewData["ambitos"] = Datos.ListarAmbitos();
                ViewData["referencialesAsignables"] = Datos.ListarReferencialesAsignablesAccMejora(id);
                ViewData["referencialesAsignadas"] = Datos.ListarReferencialesAsignadosAccmejora(id);
                ViewData["modulos"] = Datos.ListarModulos();
                ViewData["procesos"] = Datos.ListarProcesos();
                ViewData["acciones"] = Datos.ListarAccionesAccionesMejora(id);
                ViewData["documentosaccionesmejora"] = Datos.ListarDocumentosAccionMejora(id);
                return Redirect(Url.RouteUrl(new { controller = "AccionMejora", action = "detalle_accion", id = id }));
                #endregion
            }
            else
            {
                #region recarga
                accionesmejora acc = new accionesmejora();

                acc.asunto = collection["ctl00$MainContent$txtAsunto"];
                acc.ambito = int.Parse(collection["ctl00$MainContent$ddlAmbito"]);
                if (collection["ctl00$MainContent$txtFApertura"] != "")
                    acc.fecha_apertura = DateTime.Parse(collection["ctl00$MainContent$txtFApertura"]);
                if (collection["ctl00$MainContent$txtFCierre"] != "")
                    acc.fecha_cierre = DateTime.Parse(collection["ctl00$MainContent$txtFCierre"]);
                if (collection["ctl00$MainContent$ddlResponsable"] != null && collection["ctl00$MainContent$ddlResponsable"] != "0")
                    acc.responsable = int.Parse(collection["ctl00$MainContent$ddlResponsable"]);
                if (collection["ctl00$MainContent$ddlEstado"] != null && collection["ctl00$MainContent$ddlEstado"] != "0")
                    acc.estado = int.Parse(collection["ctl00$MainContent$ddlEstado"]);
                if (collection["ctl00$MainContent$ddlTipo"] != null && collection["ctl00$MainContent$ddlTipo"] != "0")
                    acc.tipo = int.Parse(collection["ctl00$MainContent$ddlTipo"]);
                if (collection["ctl00$MainContent$ddlProceso"] != null && collection["ctl00$MainContent$ddlProceso"] != "0")
                    acc.proceso = int.Parse(collection["ctl00$MainContent$ddlProceso"]);
                if (collection["ctl00$MainContent$ddlContratista"] != null && collection["ctl00$MainContent$ddlContratista"] != "0")
                    acc.contratista = int.Parse(collection["ctl00$MainContent$ddlContratista"]);
                acc.detectadopor = collection["ctl00$MainContent$txtDetectado"];
                acc.antecedente = int.Parse(collection["ctl00$MainContent$ddlAntecedente"]);
                //if (acc.antecedente != 0)
                //    acc.referencia = int.Parse(collection["ctl00$MainContent$ddlReferencia"]);
                //else
                //    acc.referencianoconforme = collection["ctl00$MainContent$txtReferencia"];
                acc.descripcion = collection["ctl00$MainContent$txtDescripcion"];
                acc.personasinv = collection["ctl00$MainContent$txtPersonasInvolucradas"];
                acc.causas = collection["ctl00$MainContent$txtCausa"];

                acc.ai_descripcion = collection["ctl00$MainContent$txtDescripcionAI"];
                if (collection["ctl00$MainContent$ddlResponsableAI"] != null && collection["ctl00$MainContent$ddlResponsableAI"] != "0")
                    acc.ai_responsable = int.Parse(collection["ctl00$MainContent$ddlResponsableAI"]);
                if (collection["ctl00$MainContent$txtFFinAI"] != "")
                    acc.ai_ffin_prevista = DateTime.Parse(collection["ctl00$MainContent$txtFFinAI"]);
                if (collection["ctl00$MainContent$txtFCierreAI"] != "")
                    acc.ai_fcierre = DateTime.Parse(collection["ctl00$MainContent$txtFCierreAI"]);

                if (collection["ctl00$MainContent$ddlEstadoAI"] != null && collection["ctl00$MainContent$ddlEstadoAI"] != "0")
                    acc.ai_estado = int.Parse(collection["ctl00$MainContent$ddlEstadoAI"]);
                acc.ai_comentario = collection["ctl00$MainContent$txtComentarioAI"];
                
                ViewData["accionmejora"] = acc;
                if (acc != null && acc.antecedente != 0)
                {
                    ViewData["referencias"] = ListadoReferencias(acc.antecedente, centroseleccionado.id);
                }
                ViewData["tiposaccionmejora"] = Datos.ListarTiposAccionMejora();
                ViewData["responsables"] = Datos.ListarUsuariosCentral(idCentral);
                ViewData["ambitos"] = Datos.ListarAmbitos();
                ViewData["referencialesAsignables"] = Datos.ListarReferencialesAsignablesAccMejora(id);
                ViewData["referencialesAsignadas"] = Datos.ListarReferencialesAsignadosAccmejora(id);      
                if (acc.antecedente != 0)
                {
                    ViewData["referencias"] = ListadoReferencias(acc.antecedente, centroseleccionado.id);
                }
                ViewData["modulos"] = Datos.ListarModulos();
                ViewData["procesos"] = Datos.ListarProcesos();
                ViewData["acciones"] = Datos.ListarAccionesAccionesMejora(id);
                ViewData["documentosaccionesmejora"] = Datos.ListarDocumentosAccionMejora(id);
                return View();
                #endregion
            }
        }

        public ActionResult eliminar_accionmejora(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            accionesmejora accmejora = Datos.GetDatosAccionMejora(id);
            int idreferencia = 0;
            string modulo = "0";

            if (Session["modulo"] != null)
                modulo = Session["modulo"].ToString();

            if (modulo != "0")
                idreferencia = int.Parse(accmejora.referencia.ToString());

            Datos.EliminarAccionMejora(id);
            Session["EdicionAccionMejoraMensaje"] = "Acción eliminada";

            string pagina = "accionesmejora";
            string controlador = "AccionMejora";

            switch (modulo)
            {
                case "0":
                    pagina = "accionesmejora";
                    controlador = "AccionMejora";
                    break;
                case "1":
                    pagina = "detalle_objetivo/" + idreferencia;
                    controlador = "Objetivos";
                    break;
                case "2":
                    pagina = "detalle_auditoria/" + idreferencia;
                    controlador = "Auditorias";
                    break;
                case "3":
                    pagina = "detalle_comunicacion/" + idreferencia;
                    controlador = "Comunicacion";
                    break;
                case "4":
                    pagina = "detalle_formacion/" + idreferencia;
                    controlador = "Formacion";
                    break;
                case "5":
                    pagina = "detalle_emergencia/" + idreferencia;
                    controlador = "Emergencias";
                    break;
                case "6":
                    pagina = "detalle_satisfaccion/" + idreferencia;
                    controlador = "Satisfaccion";
                    break;
                case "7":
                    pagina = "detalle_revision/" + idreferencia;
                    controlador = "RevEnergetica";
                    break;
                case "8":
                    pagina = "detalle_requisito/" + idreferencia;
                    controlador = "Requisitos";
                    break;
                case "9":
                    pagina = "detalle_indicador/" + idreferencia;
                    controlador = "Indicadores";
                    break;
                case "10":
                    pagina = "detalle_aspecto/" + idreferencia;
                    controlador = "Aspectos";
                    break;
                case "11":
                    pagina = "detalle_riesgo/" + idreferencia;
                    controlador = "Riesgos";
                    break;
                case "12":
                    pagina = "detalle_reunion/" + idreferencia;
                    controlador = "ActasReunion";
                    break;
                case "13":
                    pagina = "detalle_evento_amb/" + idreferencia;
                    controlador = "Comunicacion";
                    break;
                case "14":
                    pagina = "detalle_evento_cal/" + idreferencia;
                    controlador = "Comunicacion";
                    break;
                case "15":
                    pagina = "detalle_evento_seg/" + idreferencia;
                    controlador = "Comunicacion";
                    break;
                default:
                    pagina = "accionesmejora";
                    controlador = "AccionMejora";
                    break;
            }

            return RedirectToAction(pagina, controlador);
        }

        public ActionResult eliminar_accion(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            int accionmejora = Datos.EliminarAccionAccionMejora(id);
            Session["EdicionAccionMejoraMensaje"] = "Acción eliminada";
            return RedirectToAction("detalle_accion/" + accionmejora, "AccionMejora");
        }

        #region cargar desplegable referencias
        public List<ItemDesplegable> ListadoReferencias(int idAntecedente, int idCentral)
        {
            List<ItemDesplegable> listado = new List<ItemDesplegable>();
            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            switch (idAntecedente)
            {
                //Objetivos
                case 1:
                    
                    if (centroseleccionado.id == 25)
                        idCentral = 0;
                    List<objetivos> listaObj = Datos.ListarObjetivosAccionMejora(idCentral);
                    foreach (objetivos obj in listaObj)
                    {
                        ItemDesplegable nuevoItem = new ItemDesplegable();
                        nuevoItem.id = obj.id;
                        nuevoItem.nombre = obj.Codigo + " - " + obj.Nombre;
                        listado.Add(nuevoItem);
                    }
                    break;
                //Auditorias
                case 2:
                    List<VISTA_Auditorias> listaAud = Datos.ListarAuditorias(idCentral);
                    foreach (VISTA_Auditorias aud in listaAud)
                    {
                        ItemDesplegable nuevoItem = new ItemDesplegable();
                        nuevoItem.id = aud.id;
                        if (aud.fechainicio != null)
                        {
                            nuevoItem.nombre = aud.nombre + " - " + DateTime.Parse(aud.fechainicio.ToString()).Date.ToShortDateString();
                        }
                        else
                        {
                            nuevoItem.nombre = aud.nombre;
                        }
                        listado.Add(nuevoItem);
                    }
                    break;
                //Comunicación
                case 3:
                    List<VISTA_Comunicaciones> listaCom = Datos.ListarComunicaciones(idCentral);
                    foreach (VISTA_Comunicaciones com in listaCom)
                    {
                        ItemDesplegable nuevoItem = new ItemDesplegable();
                        nuevoItem.id = com.id;
                        if (com.fechainicio != null)
                        {
                            nuevoItem.nombre = com.idcomunicacion + " - " + com.Clasificacion + " - " + DateTime.Parse(com.fechainicio.ToString()).Date.ToShortDateString();
                        }
                        else
                        {
                            nuevoItem.nombre = com.idcomunicacion + " - " + com.Clasificacion;
                        }
                        
                        listado.Add(nuevoItem);
                    }
                    break;
                //Formación
                case 4:
                    if (centroseleccionado.tipo == 4)
                        idCentral = 0;
                    List<formacion> listaFor = Datos.ListarFormacion(idCentral);
                    foreach (formacion form in listaFor)
                    {
                        ItemDesplegable nuevoItem = new ItemDesplegable();
                        nuevoItem.id = form.id;
                        nuevoItem.nombre = form.codigo + " - " + form.denominacion;
                        listado.Add(nuevoItem);
                    }
                    break;
                //Emergencias
                case 5:
                    List<VISTA_ListarEmergencias> listaEme = Datos.ListarEmergencias(idCentral);
                    foreach (VISTA_ListarEmergencias eme in listaEme)
                    {
                        ItemDesplegable nuevoItem = new ItemDesplegable();
                        nuevoItem.id = eme.id;
                        nuevoItem.nombre = eme.codigo + " - " + eme.descripcion;
                        listado.Add(nuevoItem);
                    }
                    break;
                //Satisfacción
                case 6:
                    List<VISTA_ListarSatisfaccion> listaSat = Datos.ListarSatisfaccion(idCentral);
                    
                    foreach (VISTA_ListarSatisfaccion sat in listaSat)
                    {
                        ItemDesplegable nuevoItem = new ItemDesplegable();
                        nuevoItem.id = sat.id;
                        satisfaccion satisf = Datos.GetDatosSatisfaccion(sat.id);
                        nuevoItem.nombre = sat.codigo + " - " + Datos.ObtenerStakeholderN3(satisf.stakeholder).denominacion;
                        listado.Add(nuevoItem);
                    }
                    break;
                //Revisión energética
                case 7:
                    List<VISTA_ListarRevisionesEnergeticas> listaRev = Datos.ListarRevisiones(idCentral);
                    foreach (VISTA_ListarRevisionesEnergeticas rev in listaRev)
                    {
                        ItemDesplegable nuevoItem = new ItemDesplegable();
                        nuevoItem.id = rev.id;
                        if (rev.fechaplanificacion != null)
                        {
                            nuevoItem.nombre = rev.codigo + " - " + DateTime.Parse(rev.fechaplanificacion.ToString()).Date.ToShortDateString();
                        }
                        else
                        {
                            nuevoItem.nombre = rev.codigo;
                        }
                        listado.Add(nuevoItem);
                    }
                    break;
                //Req. Legales
                case 8:
                    List<VISTA_RequisitosLegales> listaReq = Datos.ListarRequisitos(idCentral);
                    foreach (VISTA_RequisitosLegales req in listaReq)
                    {
                        ItemDesplegable nuevoItem = new ItemDesplegable();
                        nuevoItem.id = req.id;
                        nuevoItem.nombre = req.codigo + " - " + req.denominacion;
                        listado.Add(nuevoItem);
                    }
                    break;
                //Indicadores
                case 9:
                    break;
                //Aspectos ambientales
                case 10:
                    List <VISTA_AspectosValoracion> listaAspectos = Datos.ListarAspectosValoracion(centroseleccionado.id, 0);
                    foreach (VISTA_AspectosValoracion asp in listaAspectos)
                    {
                        ItemDesplegable nuevoItem = new ItemDesplegable();
                        nuevoItem.id = asp.id;
                        nuevoItem.nombre = asp.Expr1 + "-" + asp.Nombre;
                        listado.Add(nuevoItem);
                    }
                    List <VISTA_AspectosValoracion> listaAspectosF = Datos.ListarAspectosValoracion(centroseleccionado.id, 1);
                    foreach (VISTA_AspectosValoracion asp in listaAspectosF)
                    {
                        ItemDesplegable nuevoItem = new ItemDesplegable();
                        nuevoItem.id = asp.id;
                        nuevoItem.nombre = asp.Expr1 + "-" + asp.nombrefoco;
                        listado.Add(nuevoItem);
                    }
                    break;
                //Riesgos y oportunidades
                case 11:
                    List<VISTA_Riesgo> listaRie = Datos.ListarRiesgosFicha(centroseleccionado.id);
                    foreach (VISTA_Riesgo reu in listaRie)
                    {
                        ItemDesplegable nuevoItem = new ItemDesplegable();
                        nuevoItem.id = reu.Id;
                        nuevoItem.nombre = reu.CodigoRiesgo;

                        listado.Add(nuevoItem);
                    }
                    break;
                case 12:
                    List<reuniones> listaReu = Datos.ListarReuniones(centroseleccionado.id);
                    foreach (reuniones reu in listaReu)
                    {
                        ItemDesplegable nuevoItem = new ItemDesplegable();
                        nuevoItem.id = reu.id;
                        if (reu.fecha_convocatoria != null)
                        {
                            nuevoItem.nombre = reu.cod_reunion + " - " + DateTime.Parse(reu.fecha_convocatoria.ToString()).Date.ToShortDateString();
                        }
                        else
                        {
                            nuevoItem.nombre = reu.cod_reunion;
                        }
                        listado.Add(nuevoItem);
                    }
                    break;
                case 13:
                    List<VISTA_ListarEventosAmbientales> listaEv = Datos.ListarEventosAmbientales(centroseleccionado.id);
                    foreach (VISTA_ListarEventosAmbientales ev in listaEv)
                    {
                        ItemDesplegable nuevoItem = new ItemDesplegable();
                        nuevoItem.id = ev.id;
                        if (ev.fechaevento != null)
                        {

                            nuevoItem.nombre = ev.tipo + " - " + ev.matrizprincipal + " - " + DateTime.Parse(ev.fechaevento.ToString()).Date.ToShortDateString();
                        }
                        else
                        {
                            nuevoItem.nombre = ev.tipo + " - " + ev.matrizprincipal;
                        }
                        listado.Add(nuevoItem);
                    }
                    break;
                case 14:
                    List<VISTA_ListarEventosCalidad> listaEvCal = Datos.ListarEventosCalidad(centroseleccionado.id);
                    foreach (VISTA_ListarEventosCalidad ev in listaEvCal)
                    {
                        ItemDesplegable nuevoItem = new ItemDesplegable();
                        nuevoItem.id = ev.id;
                        if (ev.fechacomienzo != null)
                        {

                            nuevoItem.nombre = DateTime.Parse(ev.fechacomienzo.ToString()).Date.ToShortDateString() + " - " + ev.asunto;
                        }
                        else
                        {
                            nuevoItem.nombre = ev.asunto;
                        }
                        listado.Add(nuevoItem);
                    }
                    break;
                case 15:
                    List<VISTA_Eventos_Seguridad> listaEvSeg = Datos.ListarEventosSeguridad(centroseleccionado.id);
                    foreach (VISTA_Eventos_Seguridad ev in listaEvSeg)
                    {
                        ItemDesplegable nuevoItem = new ItemDesplegable();
                        nuevoItem.id = ev.id;
                        if (ev.fecha != null)
                        {

                            nuevoItem.nombre = DateTime.Parse(ev.fecha.ToString()).Date.ToShortDateString() + " - " + ev.tipo + " - " + ev.severidad;
                        }
                        else
                        {
                            nuevoItem.nombre = ev.tipo + " - " + ev.severidad;
                        }
                        listado.Add(nuevoItem);
                    }
                    break;
                case 16:
                    List<partes> listaPartes = Datos.ListarPartes(centroseleccionado.id);
                    foreach (partes ev in listaPartes)
                    {
                        ItemDesplegable nuevoItem = new ItemDesplegable();
                        nuevoItem.id = ev.id;
                        if (ev.cumplimentadofecha != null)
                        {

                            nuevoItem.nombre = DateTime.Parse(ev.cumplimentadofecha.ToString()).Date.ToShortDateString() + " - " + ev.idcomunicacion + " - " + ev.empresa;
                        }
                        else
                        {
                            nuevoItem.nombre = ev.idcomunicacion + " - " + ev.empresa;
                        }
                        listado.Add(nuevoItem);
                    }
                    break;
            }

            return listado;
        }
        #endregion

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
