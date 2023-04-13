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
    public class FormacionController : Controller
    {
        //
        // GET: /Formacion/

        public ActionResult plan_formacion()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            int idCentral = centroseleccionado.id;

            ViewData["planformacion"] = Datos.ListarFormacion(0);
            ViewData["planformacionespecifico"] = Datos.ListarFormacion(idCentral);

            #region generacion fichero
            if (Session["nombreArchivo"] != null && Session["nombreArchivo"].ToString() != "")
            {
                if (Session["nombreArchivo"].ToString().Contains("xlsx"))
                {
                    Response.ContentType = "application/msexcel";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=ExportacionFormacion.xlsx");
                    Response.TransmitFile(Session["nombreArchivo"].ToString());
                    Response.End();
                }
                Session["nombreArchivo"] = "";
            }
            #endregion

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult plan_formacion(FormCollection collection)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            VISTA_ObtenerUsuario user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());

            #region generacion fichero
            string sourceFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaPlantillaWord"] + "ExportacionFormacion.xlsx");
            string destinationFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaGeneracionWord"] + "ExportacionFormacion_" + DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + ".xlsx");

            Session["source"] = sourceFile;
            Session["destino"] = destinationFile;
            // Create a copy of the template file and open the copy 
            System.IO.File.Copy(sourceFile, destinationFile, true);

            #region impresion
            //LISTADO FORMACION
            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            int idCentral = centroseleccionado.id;

            using (SpreadsheetDocument document = SpreadsheetDocument.Open(destinationFile, true))
            {
                
                List<formacion> form = Datos.ListarFormacionInforme(idCentral);
                SheetData sheetData = document.WorkbookPart.WorksheetParts.ElementAt(0).Worksheet.Elements<SheetData>().First();
                foreach (formacion fo in form)
                {
                    Row row = new Row();

                    string anio = string.Empty;
                    string denominacion = string.Empty;
                    string planifinicial = string.Empty;
                    string fechainicio = string.Empty;
                    string fechaejec = string.Empty;
                    string planifejec = string.Empty;
                    string valoracion = string.Empty;
                    string accionesmejora = string.Empty;

                    if (fo.anio == null)
                        anio = string.Empty;
                    else
                        anio = fo.anio.ToString();

                    if (fo.denominacion == null)
                        denominacion = string.Empty;
                    else
                        denominacion = fo.denominacion;

                    if (fo.planificacion_inicial == null)
                        planifinicial = string.Empty;
                    else
                        planifinicial = fo.planificacion_inicial.Replace(Server.MapPath("~/Formacion") + "\\" + fo.id + "\\PlanInicial\\", "");

                    if (fo.planificacion_ejecutada == null)
                        planifejec = string.Empty;
                    else
                        planifejec = fo.planificacion_ejecutada.Replace(Server.MapPath("~/Formacion") + "\\" + fo.id + "\\PlanEjecutada\\", "");

                    if (fo.fecha_registro_inicio == null)
                        fechainicio = string.Empty;
                    else
                        fechainicio = fo.fecha_registro_inicio.ToString().Replace(" 0:00:00", "");

                    if (fo.fecha_registro_ejecutado == null)
                        fechaejec = string.Empty;
                    else
                        fechaejec = fo.fecha_registro_ejecutado.ToString().Replace(" 0:00:00", "");

                    if (fo.valoracion_eficacia == null)
                        valoracion = string.Empty;
                    else
                    {
                        switch (fo.valoracion_eficacia)
                        {
                            case 0:
                                valoracion = "";
                                break;
                            case 1:
                                valoracion = "Eficaz";
                                break;
                            case 2:
                                valoracion = "No eficaz";
                                break;
                        }
                    }

                    List<VISTA_ListarAccionesMejora> listaacciones = Datos.ListarAccionesMejora(idCentral, 4, fo.id);

                    foreach (VISTA_ListarAccionesMejora acc in listaacciones)
                    {
                        accionesmejora = accionesmejora + acc.codigo + "/" + acc.asunto + "\r\n";
                    }

                    row.Append(
                        Datos.ConstructCell(anio, CellValues.String),
                        Datos.ConstructCell(denominacion, CellValues.String),
                        Datos.ConstructCell(planifinicial, CellValues.String),
                        Datos.ConstructCell(fechainicio, CellValues.String),
                        Datos.ConstructCell(fechaejec, CellValues.String),
                        Datos.ConstructCell(planifejec, CellValues.String),
                        Datos.ConstructCell(valoracion, CellValues.String),
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
            Session["nombreArchivo"] = destinationFile;

            ViewData["planformacion"] = Datos.ListarFormacion(0);
            ViewData["planformacionespecifico"] = Datos.ListarFormacion(idCentral);

            return RedirectToAction("plan_formacion", "Formacion");

        }

        public FileResult ObtenerPlanInicial(int id)
        {
            try
            {
                formacion IF = Datos.GetDatosFormacion(id);
                byte[] fileBytes = System.IO.File.ReadAllBytes(IF.planificacion_inicial);
                string fileName = IF.planificacion_inicial.Replace(Server.MapPath("~/Formacion") + "\\" + IF.id + "\\PlanInicial\\", "");
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public FileResult ObtenerPlanEjecutada(int id)
        {
            try
            {
                formacion IF = Datos.GetDatosFormacion(id);
                byte[] fileBytes = System.IO.File.ReadAllBytes(IF.planificacion_ejecutada);
                string fileName = IF.planificacion_ejecutada.Replace(Server.MapPath("~/Formacion") + "\\" + IF.id + "\\PlanEjecutada\\", "");
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ActionResult detalle_formacion(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            Session["modulo"] = "4";

            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            int idCentral = centroseleccionado.id;  

            formacion obj = Datos.GetDatosFormacion(id);
            ViewData["formacion"] = obj;
            ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 4, id);

            #region generacion fichero
            if (Session["nombreArchivo"] != null && Session["nombreArchivo"].ToString() != "")
            {
                if (Session["nombreArchivo"].ToString().Contains("docx"))
                {
                    Response.ContentType = "application/msword";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=FichaFormacion.docx");
                    Response.TransmitFile(Session["nombreArchivo"].ToString());
                    Response.End();
                }
                Session["nombreArchivo"] = "";
            }
            #endregion

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult detalle_formacion(int id, FormCollection collection, HttpPostedFileBase[] file)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }
            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            int idCentral = centroseleccionado.id;  

            formacion proce = new formacion();
            string formulario = collection["hdFormularioEjecutado"];

            if (formulario == "GuardarFormacion")
            {
                #region guardar
                try
                {
                    if (id != 0)
                    {
                        #region actualizar
                        formacion actualizar = Datos.GetDatosFormacion(id);
                        actualizar.denominacion = collection["ctl00$MainContent$txtDenominacion"];

                        actualizar.analisiscausasnorealiz = collection["ctl00$MainContent$txtCausasNoRealiz"];
                        actualizar.analisiscausasnoefectivas = collection["ctl00$MainContent$txtCausasNoEfectivas"];
                        actualizar.observaciones = collection["ctl00$MainContent$txtObservaciones"];

                        if (collection["ctl00$MainContent$txtActEjecutadas"] != "")
                            actualizar.actividadesejecutadas = int.Parse(collection["ctl00$MainContent$txtActEjecutadas"]);
                        else
                            actualizar.actividadesejecutadas = 0;

                        if (collection["ctl00$MainContent$txtActPlanificadas"] != "")
                            actualizar.actividadesplanificadas = int.Parse(collection["ctl00$MainContent$txtActPlanificadas"]);
                        else
                            actualizar.actividadesplanificadas = 0;

                        if (collection["ctl00$MainContent$txtHorasCalidad"] != "")
                            actualizar.horascalidad = int.Parse(collection["ctl00$MainContent$txtHorasCalidad"]);
                        else
                            actualizar.horascalidad = 0;

                        if (collection["ctl00$MainContent$txtHorasMedioambiente"] != "")
                            actualizar.horasmedioambiente = int.Parse(collection["ctl00$MainContent$txtHorasMedioambiente"]);
                        else
                            actualizar.horasmedioambiente = 0;

                        if (collection["ctl00$MainContent$txtHorasSegySalud"] != "")
                            actualizar.horasseguridadsalud = int.Parse(collection["ctl00$MainContent$txtHorasSegySalud"]);
                        else
                            actualizar.horasseguridadsalud = 0;

                        if (collection["ctl00$MainContent$txtHorasOtrasAreas"] != "")
                            actualizar.horasotrasareas = int.Parse(collection["ctl00$MainContent$txtHorasOtrasAreas"]);
                        else
                            actualizar.horasotrasareas = 0;

                        if (collection["ctl00$MainContent$txtFRegInicio"] != "")
                            actualizar.fecha_registro_inicio = DateTime.Parse(collection["ctl00$MainContent$txtFRegInicio"]);
                        if (collection["ctl00$MainContent$txtFRegEjecutado"] != "")
                            actualizar.fecha_registro_ejecutado = DateTime.Parse(collection["ctl00$MainContent$txtFRegEjecutado"]);
                        if (collection["ctl00$MainContent$ddlValoracion"] != null)
                            actualizar.valoracion_eficacia = int.Parse(collection["ctl00$MainContent$ddlValoracion"]);

                        if (actualizar.denominacion != string.Empty)
                        {
                            Datos.ActualizarFormacion(actualizar);

                            if ((file[0] != null && file[0].ContentLength > 0))
                            {
                                var fileName = System.IO.Path.GetFileName(file[0].FileName);

                                var path = System.IO.Path.Combine(Server.MapPath("~/Formacion/" + id.ToString() + "/PlanInicial"), fileName);
                                actualizar.planificacion_inicial = path;

                                Datos.UpdatePlanInicialEnlace(actualizar);

                                if (Directory.Exists(Server.MapPath("~/Formacion/" + id.ToString() + "/PlanInicial")))
                                {
                                    file[0].SaveAs(path);
                                }
                                else
                                {
                                    Directory.CreateDirectory(Server.MapPath("~/Formacion/" + id.ToString() + "/PlanInicial"));
                                    file[0].SaveAs(path);
                                }
                            }

                            if ((file[1] != null && file[1].ContentLength > 0))
                            {
                                var fileName = System.IO.Path.GetFileName(file[1].FileName);

                                var path = System.IO.Path.Combine(Server.MapPath("~/Formacion/" + id.ToString() + "/PlanEjecutada"), fileName);
                                actualizar.planificacion_ejecutada = path;

                                Datos.UpdatePlanEjecutadoEnlace(actualizar);

                                if (Directory.Exists(Server.MapPath("~/Formacion/" + id.ToString() + "/PlanEjecutada")))
                                {
                                    file[1].SaveAs(path);
                                }
                                else
                                {
                                    Directory.CreateDirectory(Server.MapPath("~/Formacion/" + id.ToString() + "/PlanEjecutada"));
                                    file[1].SaveAs(path);
                                }
                            }

                            Session["EdicionFormacionMensaje"] = "Información actualizada correctamente";
                            proce = Datos.GetDatosFormacion(id);
                            ViewData["formacion"] = proce;
                            ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 4, id);
                            return View();

                        }
                        else
                        {
                            Session["EdicionFormacionError"] = "Los campos marcados con (*) son obligatorios.";

                            ViewData["formacion"] = actualizar;
                            ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 4, id);
                            return View();                            
                        }
                        #endregion
                    }
                    else
                    {
                        #region insertar
                        formacion insertar = new formacion();
                        insertar.denominacion = collection["ctl00$MainContent$txtDenominacion"];

                        insertar.analisiscausasnorealiz = collection["ctl00$MainContent$txtCausasNoRealiz"];
                        insertar.analisiscausasnoefectivas = collection["ctl00$MainContent$txtCausasNoEfectivas"];
                        insertar.observaciones = collection["ctl00$MainContent$txtObservaciones"];

                        if (collection["ctl00$MainContent$txtActEjecutadas"] != "")
                            insertar.actividadesejecutadas = int.Parse(collection["ctl00$MainContent$txtActEjecutadas"]);
                        else
                            insertar.actividadesejecutadas = 0;

                        if (collection["ctl00$MainContent$txtActPlanificadas"] != "")
                            insertar.actividadesplanificadas = int.Parse(collection["ctl00$MainContent$txtActPlanificadas"]);
                        else
                            insertar.actividadesplanificadas = 0;

                        if (collection["ctl00$MainContent$txtHorasCalidad"] != "")
                            insertar.horascalidad = int.Parse(collection["ctl00$MainContent$txtHorasCalidad"]);
                        else
                            insertar.horascalidad = 0;

                        if (collection["ctl00$MainContent$txtHorasMedioambiente"] != "")
                            insertar.horasmedioambiente = int.Parse(collection["ctl00$MainContent$txtHorasMedioambiente"]);
                        else
                            insertar.horasmedioambiente = 0;

                        if (collection["ctl00$MainContent$txtHorasSegySalud"] != "")
                            insertar.horasseguridadsalud = int.Parse(collection["ctl00$MainContent$txtHorasSegySalud"]);
                        else
                            insertar.horasseguridadsalud = 0;

                        if (collection["ctl00$MainContent$txtHorasOtrasAreas"] != "")
                            insertar.horasotrasareas = int.Parse(collection["ctl00$MainContent$txtHorasOtrasAreas"]);
                        else
                            insertar.horasotrasareas = 0;

                        insertar.anio = int.Parse(collection["ctl00$MainContent$ddlAnio"]);
                        if (collection["ctl00$MainContent$txtFRegInicio"] != "")
                            insertar.fecha_registro_inicio = DateTime.Parse(collection["ctl00$MainContent$txtFRegInicio"]);
                        if (collection["ctl00$MainContent$txtFRegEjecutado"] != "")
                            insertar.fecha_registro_ejecutado = DateTime.Parse(collection["ctl00$MainContent$txtFRegEjecutado"]);
                        if (collection["ctl00$MainContent$ddlValoracion"] != null && collection["ctl00$MainContent$ddlValoracion"] != "0")
                            insertar.valoracion_eficacia = int.Parse(collection["ctl00$MainContent$ddlValoracion"]);
                        if (Session["CentralElegida"] != null)
                        {
                            if (centroseleccionado.tipo != 4)
                            {
                                insertar.idcentral = centroseleccionado.id;
                            }
                            else
                                insertar.idcentral = 0;
                        }


                        if (insertar.denominacion != string.Empty)
                        {
                            int idForm = Datos.ActualizarFormacion(insertar);

                            if ((file[0] != null && file[0].ContentLength > 0))
                            {
                                var fileName = System.IO.Path.GetFileName(file[0].FileName);

                                var path = System.IO.Path.Combine(Server.MapPath("~/Formacion/" + idForm.ToString() + "/PlanInicial"), fileName);
                                insertar.planificacion_inicial = path;

                                Datos.UpdatePlanInicialEnlace(insertar);

                                if (Directory.Exists(Server.MapPath("~/Formacion/" + idForm.ToString() + "/PlanInicial")))
                                {
                                    file[0].SaveAs(path);
                                }
                                else
                                {
                                    Directory.CreateDirectory(Server.MapPath("~/Formacion/" + idForm.ToString() + "/PlanInicial"));
                                    file[0].SaveAs(path);
                                }
                            }

                            if ((file[1] != null && file[1].ContentLength > 0))
                            {
                                var fileName = System.IO.Path.GetFileName(file[1].FileName);

                                var path = System.IO.Path.Combine(Server.MapPath("~/Formacion/" + idForm.ToString() + "/PlanEjecutada"), fileName);
                                insertar.planificacion_ejecutada = path;

                                Datos.UpdatePlanEjecutadoEnlace(insertar);

                                if (Directory.Exists(Server.MapPath("~/Formacion/" + idForm.ToString() + "/PlanEjecutada")))
                                {
                                    file[1].SaveAs(path);
                                }
                                else
                                {
                                    Directory.CreateDirectory(Server.MapPath("~/Formacion/" + idForm.ToString() + "/PlanEjecutada"));
                                    file[1].SaveAs(path);
                                }
                            }



                            Session["EdicionFormacionMensaje"] = "Información actualizada correctamente";

                            formacion form = Datos.GetDatosFormacion(idForm);
                            ViewData["formacion"] = form;
                            ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 4, id);
                            return Redirect(Url.RouteUrl(new { controller = "Formacion", action = "detalle_formacion", id = idForm }));
                        }
                        else
                        {
                            Session["EdicionFormacionError"] = "Los campos marcados con (*) son obligatorios.";

                            ViewData["formacion"] = insertar;
                            ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 4, id);
                            return View();     
                        }
                        #endregion
                    }

                }
                catch (Exception ex)
                {
                    proce = Datos.GetDatosFormacion(id);
                    ViewData["formacion"] = proce;
                    ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 4, id);
                    return View();
                }
                #endregion
            }

            if (formulario == "btnImprimir")
            {
                #region imprimir
                string sourceFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaPlantillaWord"] + "FichaFormacion.docx");
                string destinationFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaGeneracionWord"] + "FichaFormacion_" + DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + ".docx");

                Session["source"] = sourceFile;
                Session["destino"] = destinationFile;
                // Create a copy of the template file and open the copy 
                System.IO.File.Copy(sourceFile, destinationFile, true);

                #region impresion
                //LISTADO FORMACION
                formacion actualizar = Datos.GetDatosFormacion(id);

                // create key value pair, key represents words to be replace and 
                //values represent values in document in place of keys.
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add("T_Codigo", actualizar.codigo.Replace("\r\n", "<w:br/>"));
                keyValues.Add("T_Denominacion", actualizar.denominacion.Replace("\r\n", "<w:br/>"));
                if (actualizar.valoracion_eficacia == 1)
                    keyValues.Add("T_Valoracion", "Eficaz");
                if (actualizar.valoracion_eficacia == 2)
                    keyValues.Add("T_Valoracion", "No eficaz");
                keyValues.Add("T_ActEjecutadas", actualizar.actividadesejecutadas.ToString().Replace("\r\n", "<w:br/>"));
                keyValues.Add("T_ActPlanificadas", actualizar.actividadesplanificadas.ToString().Replace("\r\n", "<w:br/>"));
                if (actualizar.fecha_registro_inicio != null)
                    keyValues.Add("T_FechaInicio", actualizar.fecha_registro_inicio.ToString().Replace(" 0:00:00", "").Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_FechaInicio", "");
                if (actualizar.fecha_registro_ejecutado != null)
                    keyValues.Add("T_FechaEjecutado", actualizar.fecha_registro_ejecutado.ToString().Replace(" 0:00:00", "").Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_FechaEjecutado", "");
                keyValues.Add("T_Calidad", actualizar.horascalidad.ToString().Replace("\r\n", "<w:br/>"));
                keyValues.Add("T_Medioambiente", actualizar.horasmedioambiente.ToString().Replace("\r\n", "<w:br/>"));
                keyValues.Add("T_Seguridad", actualizar.horasseguridadsalud.ToString().Replace("\r\n", "<w:br/>"));
                keyValues.Add("T_Otros", actualizar.horasotrasareas.ToString().Replace("\r\n", "<w:br/>"));
                if (actualizar.planificacion_inicial != null)
                    keyValues.Add("T_PlanificacionInicial", actualizar.planificacion_inicial.Replace(Server.MapPath("~/Formacion") + "\\" + actualizar.id + "\\PlanInicial\\", ""));
                else
                    keyValues.Add("T_PlanificacionInicial", "");
                if (actualizar.planificacion_ejecutada != null)
                    keyValues.Add("T_PlanificacionEjecutada", actualizar.planificacion_ejecutada.Replace(Server.MapPath("~/Formacion") + "\\" + actualizar.id + "\\PlanEjecutada\\", ""));
                else
                    keyValues.Add("T_PlanificacionEjecutada", "");

                keyValues.Add("T_CausasNoRealiz", actualizar.analisiscausasnorealiz.ToString().Replace("\r\n", "<w:br/>").Replace("&", "&amp;"));
                keyValues.Add("T_CausasNoEfectivas", actualizar.analisiscausasnoefectivas.ToString().Replace("\r\n", "<w:br/>").Replace("&", "&amp;"));
                keyValues.Add("T_Observaciones", actualizar.observaciones.ToString().Replace("\r\n", "<w:br/>").Replace("&", "&amp;"));


                List<VISTA_ListarAccionesMejora> listaAcciones = Datos.ListarAccionesMejora(idCentral, 4, id);
                string accionesmejora = string.Empty;
                foreach (VISTA_ListarAccionesMejora acc in listaAcciones)
                {
                    accionesmejora = accionesmejora + acc.codigo + "/" + acc.asunto + "\r\n";
                }
                keyValues.Add("T_AccionesMejora", accionesmejora.Replace("\r\n", "<w:br/>"));
                
                SearchAndReplace(destinationFile, keyValues);

                Session["nombreArchivo"] = destinationFile;

                #endregion



                proce = Datos.GetDatosFormacion(id);
                ViewData["formacion"] = proce;
                ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 4, id);
                return Redirect(Url.RouteUrl(new { controller = "Formacion", action = "detalle_formacion", id = id }));
                #endregion
            }
            else
            {
                proce = Datos.GetDatosFormacion(id);
                ViewData["formacion"] = proce;
                ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 4, id);
                return View();
            }
        }

        public ActionResult Eliminar_Planformacion(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            int idProceso = Datos.EliminarFormacion(id);
            Session["EditarFormacionResultado"] = "ELIMINADOFORMACION";
            return RedirectToAction("plan_formacion", "Formacion");
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
