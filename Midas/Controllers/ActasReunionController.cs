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
using DocumentFormat.OpenXml.Wordprocessing;

namespace MIDAS.Controllers
{
    public class ActasReunionController : Controller
    {
        //
        // GET: /ActasReunion/

        public ActionResult gestionreuniones()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            int idCentral = centroseleccionado.id;

            ViewData["reuniones"] = Datos.ListarReuniones(idCentral);

            #region generacion fichero
            if (Session["nombreArchivo"] != null && Session["nombreArchivo"].ToString() != "")
            {
                if (Session["nombreArchivo"].ToString().Contains("docx"))
                {
                    Response.ContentType = "application/msword";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=ExportacionReuniones.docx");
                    Response.TransmitFile(Session["nombreArchivo"].ToString());
                    Response.End();
                }
                Session["nombreArchivo"] = "";
            }
            #endregion

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult gestionreuniones(FormCollection collection)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            VISTA_ObtenerUsuario user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());

            string sourceFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaPlantillaWord"] + "FichaActaReunion.docx");
            string destinationFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaGeneracionWord"] + "FichaActaReunion_" + DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + ".docx");

            Session["source"] = sourceFile;
            Session["destino"] = destinationFile;
            // Create a copy of the template file and open the copy 
            System.IO.File.Copy(sourceFile, destinationFile, true);

            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            int idCentral = centroseleccionado.id;
            centros actualizar = Datos.ObtenerCentroPorID(idCentral);

            #region impresionlistado

            if (actualizar != null)
            {
                // create key value pair, key represents words to be replace and 
                //values represent values in document in place of keys.
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add("T_Codigo", actualizar.siglas.Replace("\r\n", "<w:br/>"));

                SearchAndReplace(destinationFile, keyValues);
            }

            WordprocessingDocument doc = WordprocessingDocument.Open(destinationFile, true);

            //LISTADO REUNIONES
            List<reuniones> form = new List<reuniones>();

            form = Datos.ListarReuniones(idCentral);

            DocumentFormat.OpenXml.Wordprocessing.Table tabladespliegue = doc.MainDocumentPart.Document.Body.Descendants<DocumentFormat.OpenXml.Wordprocessing.Table>().ElementAt(1);

            if (form.Count > 0)
            {
                for (int i = 0; i < form.Count; i++)
                {
                    var runPropertiesDespliegue1 = new DocumentFormat.OpenXml.Wordprocessing.RunProperties();
                    runPropertiesDespliegue1.AppendChild(new RunFonts() { Ascii = "Calibri" });
                    runPropertiesDespliegue1.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "16" });

                    tabladespliegue.Append(new DocumentFormat.OpenXml.Wordprocessing.TableRow());
                    var filadespliegue = tabladespliegue.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>().ElementAt(i+ 1);

                    filadespliegue.Append(new DocumentFormat.OpenXml.Wordprocessing.TableCell());
                    DocumentFormat.OpenXml.Wordprocessing.TableCell celdanaccion = filadespliegue.Elements<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(0);
                    var parrafo = celdanaccion.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph());
                    var runprocesos = parrafo.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Run());
                    runprocesos.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesDespliegue1);
                    runprocesos.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Text(form[i].cod_reunion));

                    var runPropertiesDespliegue2 = new DocumentFormat.OpenXml.Wordprocessing.RunProperties();
                    runPropertiesDespliegue2.AppendChild(new RunFonts() { Ascii = "Calibri" });
                    runPropertiesDespliegue2.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "16" });

                    filadespliegue.Append(new DocumentFormat.OpenXml.Wordprocessing.TableCell());
                    DocumentFormat.OpenXml.Wordprocessing.TableCell celdafase = filadespliegue.Elements<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(1);
                    parrafo = celdafase.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph());
                    runprocesos = parrafo.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Run());
                    runprocesos.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesDespliegue2);
                    if (form[i].fecha_convocatoria != null)
                        runprocesos.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Text(form[i].fecha_convocatoria.ToString().Replace(" 0:00:00", "")));
                    else
                        runprocesos.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Text(""));

                    var runPropertiesDespliegue3 = new DocumentFormat.OpenXml.Wordprocessing.RunProperties();
                    runPropertiesDespliegue3.AppendChild(new RunFonts() { Ascii = "Calibri" });
                    runPropertiesDespliegue3.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "16" });

                    filadespliegue.Append(new DocumentFormat.OpenXml.Wordprocessing.TableCell());
                    DocumentFormat.OpenXml.Wordprocessing.TableCell planificacioninicial = filadespliegue.Elements<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(2);
                    parrafo = planificacioninicial.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph());
                    runprocesos = parrafo.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Run());
                    runprocesos.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesDespliegue3);
                    List<VISTA_ReunionParticipantes> listaParticipantes = Datos.ListarParticipantesAsignados(form[i].id);
                    foreach (VISTA_ReunionParticipantes obs in listaParticipantes)
                    {
                        runprocesos.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Text("-" + obs.nombre));
                        runprocesos.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Break());
                    }

                    var runPropertiesDespliegue4 = new DocumentFormat.OpenXml.Wordprocessing.RunProperties();
                    runPropertiesDespliegue4.AppendChild(new RunFonts() { Ascii = "Calibri" });
                    runPropertiesDespliegue4.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "16" });

                    filadespliegue.Append(new DocumentFormat.OpenXml.Wordprocessing.TableCell());
                    DocumentFormat.OpenXml.Wordprocessing.TableCell celdafechainicial = filadespliegue.Elements<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(3);
                    parrafo = celdafechainicial.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph());
                    runprocesos = parrafo.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Run());
                    runprocesos.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesDespliegue4);
                    runprocesos.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Text(form[i].ordendeldia));


                    var runPropertiesDespliegueRef = new DocumentFormat.OpenXml.Wordprocessing.RunProperties();
                    runPropertiesDespliegueRef.AppendChild(new RunFonts() { Ascii = "Calibri" });
                    runPropertiesDespliegueRef.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "16" });

                    filadespliegue.Append(new DocumentFormat.OpenXml.Wordprocessing.TableCell());
                    DocumentFormat.OpenXml.Wordprocessing.TableCell celdaRef = filadespliegue.Elements<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(4);
                    parrafo = celdaRef.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph());
                    runprocesos = parrafo.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Run());
                    runprocesos.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesDespliegueRef);
                    if (form[i].horainicio != null)
                        runprocesos.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Text((DateTime.Parse(form[i].horainicio.ToString()).ToString("HH:mm"))));
                    else
                        runprocesos.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Text(""));


                    var runPropertiesDespliegue5 = new DocumentFormat.OpenXml.Wordprocessing.RunProperties();
                    runPropertiesDespliegue5.AppendChild(new RunFonts() { Ascii = "Calibri" });
                    runPropertiesDespliegue5.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "16" });

                    filadespliegue.Append(new DocumentFormat.OpenXml.Wordprocessing.TableCell());
                    DocumentFormat.OpenXml.Wordprocessing.TableCell celdafechaejecutado = filadespliegue.Elements<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(5);
                    parrafo = celdafechaejecutado.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph());
                    runprocesos = parrafo.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Run());
                    runprocesos.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesDespliegue5);
                    if (form[i].horafin != null)
                        runprocesos.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Text((DateTime.Parse(form[i].horafin.ToString()).ToString("HH:mm"))));
                    else
                        runprocesos.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Text(""));

                    var runPropertiesDespliegueEquipo = new DocumentFormat.OpenXml.Wordprocessing.RunProperties();
                    runPropertiesDespliegueEquipo.AppendChild(new RunFonts() { Ascii = "Calibri" });
                    runPropertiesDespliegueEquipo.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "16" });

                    filadespliegue.Append(new DocumentFormat.OpenXml.Wordprocessing.TableCell());
                    DocumentFormat.OpenXml.Wordprocessing.TableCell celdaEquipo = filadespliegue.Elements<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(6);
                    parrafo = celdaEquipo.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph());
                    runprocesos = parrafo.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Run());
                    runprocesos.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesDespliegueEquipo);
                    runprocesos.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Text(form[i].resumen));

                    var runPropertiesDespliegueObservadores = new DocumentFormat.OpenXml.Wordprocessing.RunProperties();
                    runPropertiesDespliegueObservadores.AppendChild(new RunFonts() { Ascii = "Calibri" });
                    runPropertiesDespliegueObservadores.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "16" });
                    filadespliegue.Append(new DocumentFormat.OpenXml.Wordprocessing.TableCell());
                    DocumentFormat.OpenXml.Wordprocessing.TableCell celdaObservadores = filadespliegue.Elements<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(7);
                    parrafo = celdaObservadores.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph());
                    runprocesos = parrafo.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Run());
                    runprocesos.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesDespliegueObservadores);
                    if (form[i].estado != 0)
                        runprocesos.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Text("En seguimiento"));
                    else
                        runprocesos.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Text("Cerrada"));

                    var runPropertiesDespliegue6 = new DocumentFormat.OpenXml.Wordprocessing.RunProperties();
                    runPropertiesDespliegue6.AppendChild(new RunFonts() { Ascii = "Calibri" });
                    runPropertiesDespliegue6.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "16" });

                    filadespliegue.Append(new DocumentFormat.OpenXml.Wordprocessing.TableCell());
                    DocumentFormat.OpenXml.Wordprocessing.TableCell planificacionejecutada = filadespliegue.Elements<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(8);
                    parrafo = planificacionejecutada.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph());
                    runprocesos = parrafo.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Run());
                    runprocesos.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesDespliegue6);
                    List<reuniones_documentos> listaDocumentos = Datos.ListarDocumentosReunion(form[i].id);
                    foreach (reuniones_documentos obs in listaDocumentos)
                    {
                        runprocesos.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Text("-" + obs.nombre));
                        runprocesos.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Break());
                    }

                    var runPropertiesDespliegue7 = new DocumentFormat.OpenXml.Wordprocessing.RunProperties();
                    runPropertiesDespliegue7.AppendChild(new RunFonts() { Ascii = "Calibri" });
                    runPropertiesDespliegue7.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "16" });

                    filadespliegue.Append(new DocumentFormat.OpenXml.Wordprocessing.TableCell());
                    DocumentFormat.OpenXml.Wordprocessing.TableCell celdaestado = filadespliegue.Elements<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(9);
                    parrafo = celdaestado.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph());
                    runprocesos = parrafo.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Run());
                    runprocesos.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesDespliegue7);
                    List<VISTA_ListarAccionesMejora> listadoAccionesMejora = Datos.ListarAccionesMejora(idCentral, 12, form[i].id);
                    foreach (VISTA_ListarAccionesMejora acc in listadoAccionesMejora)
                    {
                        runprocesos.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Text("-" + acc.asunto));
                        runprocesos.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Break());
                    }

                }
            }



            doc.MainDocumentPart.Document.Save();

            doc.Close();

            #endregion

            Session["nombreArchivo"] = destinationFile;

            ViewData["reuniones"] = Datos.ListarReuniones(idCentral);

            return RedirectToAction("gestionreuniones", "ActasReunion");

        }

        public ActionResult detalle_reunion(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            Session["modulo"] = "12";

            reuniones reu = Datos.GetDatosReunion(id);
            ViewData["reunion"] = reu;
            if (Session["CentralElegida"] != null)
            {
                int centralElegida = int.Parse(Session["CentralElegida"].ToString());

                centros central = Datos.ObtenerCentroPorID(centralElegida);

                ViewData["accionesmejora"] = Datos.ListarAccionesMejora(centralElegida, 12, id);
            }

            List<usuarios> usus = Datos.ListarParticipantesAsignar(id);
            ViewData["usuariosasignar"] = usus;
            List<VISTA_ReunionParticipantes> ususasig = Datos.ListarParticipantesAsignados(id);
            ViewData["usuariosasignados"] = ususasig;
            ViewData["documentosreunion"] = Datos.ListarDocumentosReunion(id);

            #region generacion fichero
            if (Session["nombreArchivo"] != null && Session["nombreArchivo"].ToString() != "")
            {
                if (Session["nombreArchivo"].ToString().Contains("docx"))
                {
                    Response.ContentType = "application/msword";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=FichaReunion.docx");
                    Response.TransmitFile(Session["nombreArchivo"].ToString());
                    Response.End();
                }
                Session["nombreArchivo"] = "";
            }
            #endregion

            return View();
        }

        public FileResult ObtenerDocReunion(int id)
        {
            try
            {
                reuniones_documentos IF = Datos.GetDatosDocReunion(id);
                byte[] fileBytes = System.IO.File.ReadAllBytes(IF.enlace);
                string fileName = IF.enlace.Replace(Server.MapPath("~/Reuniones") + "\\" + IF.id + "\\", "");
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ActionResult eliminar_reunion(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            int idReunion = Datos.EliminarReunion(id);
            Session["EditarReunionesResultado"] = "Reunión eliminada";
            return RedirectToAction("gestionreuniones", "ActasReunion");
        }

        public ActionResult eliminar_docreunion(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            int idReunion = Datos.EliminarDocReunion(id);
            Session["EdicionReunionMensaje"] = "Documentación eliminada";
            return RedirectToAction("detalle_reunion/" + idReunion, "ActasReunion");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult detalle_reunion(int id, FormCollection collection, HttpPostedFileBase file)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }
            reuniones reun = new reuniones();
            string formulario = collection["hdFormularioEjecutado"];

            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            int idCentral = centroseleccionado.id;

            if (formulario == "GuardarReunion")
            {
                try
                {

                    if (id != 0)
                    {
                        #region actualizar
                        reuniones actualizar = Datos.GetDatosReunion(id);
                        if (collection["ctl00$MainContent$txtFechaConvocatoria"] != "")
                            actualizar.fecha_convocatoria = DateTime.Parse(collection["ctl00$MainContent$txtFechaConvocatoria"]);
                        if (collection["ctl00$MainContent$txtHoraInicio"] != "")
                            actualizar.horainicio = DateTime.Parse(collection["ctl00$MainContent$txtHoraInicio"]).TimeOfDay;
                        if (collection["ctl00$MainContent$txtHoraFin"] != "")
                            actualizar.horafin = DateTime.Parse(collection["ctl00$MainContent$txtHoraFin"]).TimeOfDay;
                        actualizar.estado = int.Parse(collection["ctl00$MainContent$ddlEstado"]);
                        actualizar.ordendeldia = collection["ctl00$MainContent$txtOrden"];
                        actualizar.resumen = collection["ctl00$MainContent$txtResumenAcuerdos"];
                        actualizar.personasinv = collection["ctl00$MainContent$txtPersonasInvolucradas"];
                        actualizar.asunto = collection["ctl00$MainContent$txtAsunto"];

                        if (actualizar.fecha_convocatoria != null)
                        {
                            Datos.ActualizarReunion(actualizar);

                            Session["EdicionReunionMensaje"] = "Información actualizada correctamente";

                            reuniones reu = Datos.GetDatosReunion(id);
                            ViewData["reunion"] = reu;
                        }
                        else
                        {
                            Session["EdicionReunionError"] = "Los campos marcados con (*) son obligatorios.";
                            ViewData["reunion"] = actualizar;
                        }
                        if (Session["CentralElegida"] != null)
                        {
                            int centralElegida = int.Parse(Session["CentralElegida"].ToString());

                            centros central = Datos.ObtenerCentroPorID(centralElegida);

                            ViewData["accionesmejora"] = Datos.ListarAccionesMejora(centralElegida, 12, id);
                        }

                        List<usuarios> usus = Datos.ListarParticipantesAsignar(id);
                        ViewData["usuariosasignar"] = usus;
                        List<VISTA_ReunionParticipantes> ususasig = Datos.ListarParticipantesAsignados(id);
                        ViewData["usuariosasignados"] = ususasig;
                        ViewData["documentosreunion"] = Datos.ListarDocumentosReunion(id);

                        return View();
                        #endregion
                    }
                    else
                    {
                        #region insertar
                        reuniones insertar = new reuniones();
                        if (collection["ctl00$MainContent$txtFechaConvocatoria"] != "")
                            insertar.fecha_convocatoria = DateTime.Parse(collection["ctl00$MainContent$txtFechaConvocatoria"]);
                        if (collection["ctl00$MainContent$txtHoraInicio"] != "")
                            insertar.horainicio = DateTime.Parse(collection["ctl00$MainContent$txtHoraInicio"]).TimeOfDay;
                        if (collection["ctl00$MainContent$txtHoraFin"] != "")
                            insertar.horafin = DateTime.Parse(collection["ctl00$MainContent$txtHoraFin"]).TimeOfDay;
                        insertar.estado = int.Parse(collection["ctl00$MainContent$ddlEstado"]);
                        insertar.ordendeldia = collection["ctl00$MainContent$txtOrden"];
                        insertar.resumen = collection["ctl00$MainContent$txtResumenAcuerdos"];
                        insertar.idcentral = centroseleccionado.id;
                        insertar.personasinv = collection["ctl00$MainContent$txtPersonasInvolucradas"];
                        insertar.asunto = collection["ctl00$MainContent$txtAsunto"];

                        if (insertar.fecha_convocatoria != null)
                        {
                            int idCom = Datos.ActualizarReunion(insertar);

                            Session["EdicionReunionMensaje"] = "Información actualizada correctamente";
                            return Redirect(Url.RouteUrl(new { controller = "ActasReunion", action = "detalle_reunion", id = idCom }));
                        }
                        else
                        {
                            Session["EdicionReunionError"] = "Los campos marcados con (*) son obligatorios.";
                            reuniones reu = insertar;
                            ViewData["reunion"] = reu;
                            if (Session["CentralElegida"] != null)
                            {
                                int centralElegida = int.Parse(Session["CentralElegida"].ToString());

                                centros central = Datos.ObtenerCentroPorID(centralElegida);

                                ViewData["accionesmejora"] = Datos.ListarAccionesMejora(centralElegida, 12, id);
                            }

                            List<usuarios> usus = Datos.ListarParticipantesAsignar(id);
                            ViewData["usuariosasignar"] = usus;
                            List<VISTA_ReunionParticipantes> ususasig = Datos.ListarParticipantesAsignados(id);
                            ViewData["usuariosasignados"] = ususasig;
                            ViewData["documentosreunion"] = Datos.ListarDocumentosReunion(id);
                        }


                        #endregion
                    }
                }
                catch (Exception ex)
                {
                    #region exception
                    reuniones reu = Datos.GetDatosReunion(id);
                    ViewData["reunion"] = reu;
                    if (Session["CentralElegida"] != null)
                    {
                        int centralElegida = int.Parse(Session["CentralElegida"].ToString());

                        centros central = Datos.ObtenerCentroPorID(centralElegida);

                        ViewData["accionesmejora"] = Datos.ListarAccionesMejora(centralElegida, 12, id);
                    }

                    List<usuarios> usus = Datos.ListarParticipantesAsignar(id);
                    ViewData["usuariosasignar"] = usus;
                    List<VISTA_ReunionParticipantes> ususasig = Datos.ListarParticipantesAsignados(id);
                    ViewData["usuariosasignados"] = ususasig;
                    ViewData["documentosreunion"] = Datos.ListarDocumentosReunion(id);
                    return View();
                    #endregion
                }
            }

            if (formulario == "btnAddObservador")
            {
                #region añadir observador
                if (collection["ctl00$MainContent$ddlObservadores"] != null)
                {
                    int idObservador = int.Parse(collection["ctl00$MainContent$ddlObservadores"]);
                    Datos.AsociarReunionParticipante(id, idObservador);
                }


                reun = Datos.GetDatosReunion(id);
                ViewData["reunion"] = reun;
                if (Session["CentralElegida"] != null)
                {
                    int centralElegida = int.Parse(Session["CentralElegida"].ToString());

                    centros central = Datos.ObtenerCentroPorID(centralElegida);

                    ViewData["accionesmejora"] = Datos.ListarAccionesMejora(centralElegida, 12, id);
                }

                List<usuarios> usus = Datos.ListarParticipantesAsignar(id);
                ViewData["usuariosasignar"] = usus;
                List<VISTA_ReunionParticipantes> ususasig = Datos.ListarParticipantesAsignados(id);
                ViewData["usuariosasignados"] = ususasig;
                ViewData["documentosreunion"] = Datos.ListarDocumentosReunion(id);

                Session["EdicionReunionMensaje"] = "Participante asignado correctamente";
                return View();
                #endregion
            }

            if (formulario == "btnAddDocumento")
            {
                #region add documento
                if (file != null && file.ContentLength > 0 && collection["ctl00$MainContent$txtNombreDoc"] != null)
                {
                    #region guardar fichero cualificacion
                    var fileName = System.IO.Path.GetFileName(file.FileName);

                    var path = System.IO.Path.Combine(Server.MapPath("~/Reuniones/" + id), fileName);

                    if (Directory.Exists(Server.MapPath("~/Reuniones/" + id)))
                    {
                        file.SaveAs(path);
                    }
                    else
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Reuniones/" + id));
                        file.SaveAs(path);
                    }

                    reuniones_documentos IF = new reuniones_documentos();
                    IF.nombre = collection["ctl00$MainContent$txtNombreDoc"];
                    IF.nombrefichero = file.FileName;
                    IF.enlace = path;
                    IF.idreunion = id;
                    IF.fecha = DateTime.Now.Date;
                    Datos.InsertDocReunion(IF);
                    #endregion
                }

                reuniones reu = Datos.GetDatosReunion(id);
                ViewData["reunion"] = reu;
                if (Session["CentralElegida"] != null)
                {
                    int centralElegida = int.Parse(Session["CentralElegida"].ToString());

                    centros central = Datos.ObtenerCentroPorID(centralElegida);

                    ViewData["accionesmejora"] = Datos.ListarAccionesMejora(centralElegida, 12, id);
                }

                List<usuarios> usus = Datos.ListarParticipantesAsignar(id);
                ViewData["usuariosasignar"] = usus;
                List<VISTA_ReunionParticipantes> ususasig = Datos.ListarParticipantesAsignados(id);
                ViewData["usuariosasignados"] = ususasig;
                ViewData["documentosreunion"] = Datos.ListarDocumentosReunion(id);
                return View();
                #endregion
            }

            if (formulario == "btnImprimir")
            {
                #region imprimir
                string sourceFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaPlantillaWord"] + "FichaActa.docx");
                string destinationFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaGeneracionWord"] + "FichaActaReunion_" + DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + ".docx");

                Session["source"] = sourceFile;
                Session["destino"] = destinationFile;
                // Create a copy of the template file and open the copy 
                System.IO.File.Copy(sourceFile, destinationFile, true);


                centros actualizar = Datos.ObtenerCentroPorID(idCentral);
                reuniones form = new reuniones();
                form = Datos.GetDatosReunion(id);
                #region impresionlistado
                if (actualizar != null)
                {
                    // create key value pair, key represents words to be replace and 
                    //values represent values in document in place of keys.
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add("T_Codigo", form.cod_reunion.Replace("\r\n", "<w:br/>"));
                    keyValues.Add("T_Fecha", form.fecha_convocatoria.ToString().Replace("0:00:00", "").Replace("\r\n", "<w:br/>"));
                    keyValues.Add("T_HoraInicio", (DateTime.Parse(form.horainicio.ToString()).ToString("HH:mm")).Replace("\r\n", "<w:br/>"));
                    keyValues.Add("T_HoraFin", (DateTime.Parse(form.horafin.ToString()).ToString("HH:mm")).Replace("\r\n", "<w:br/>"));
                    if (form.estado != 0)
                        keyValues.Add("T_Estado", "En seguimiento");
                    else
                        keyValues.Add("T_Estado", "Cerrada");

                    List<VISTA_ReunionParticipantes> participantes = Datos.ListarParticipantesAsignados(id);
                    string cadenaParticipantes = string.Empty;
                    foreach (VISTA_ReunionParticipantes evi in participantes)
                    {
                        cadenaParticipantes = cadenaParticipantes + evi.nombre + "<w:br/>";
                    }
                    keyValues.Add("T_Participantes", cadenaParticipantes);
                    keyValues.Add("T_PersonasInv", form.personasinv.Replace("\r\n", "<w:br/>"));
                    keyValues.Add("T_OrdenDia", form.ordendeldia.Replace("\r\n", "<w:br/>"));
                    keyValues.Add("T_ResumenAcuerdos", form.resumen.Replace("\r\n", "<w:br/>"));

                    List<reuniones_documentos> documentos = Datos.ListarDocumentosReunion(id);
                    string cadenaDocumentos = string.Empty;
                    foreach (reuniones_documentos doc in documentos)
                    {
                        cadenaDocumentos = cadenaDocumentos + doc.nombre + "<w:br/>";
                    }
                    keyValues.Add("T_Documentos", cadenaDocumentos);

                    int centralElegida = int.Parse(Session["CentralElegida"].ToString());
                    List<VISTA_ListarAccionesMejora> accionesmejora = Datos.ListarAccionesMejora(centralElegida, 12, id);
                    string cadenaAccionesMejora = string.Empty;
                    foreach (VISTA_ListarAccionesMejora accMejora in accionesmejora)
                    {
                        cadenaAccionesMejora = cadenaAccionesMejora + accMejora.asunto + "<w:br/>";
                    }
                    keyValues.Add("T_AccionesMejora", cadenaAccionesMejora);
                    SearchAndReplace(destinationFile, keyValues);
                }
                #endregion

                #region session y viewstate

                Session["nombreArchivo"] = destinationFile;

                reun = Datos.GetDatosReunion(id);
                ViewData["reunion"] = reun;
                if (Session["CentralElegida"] != null)
                {
                    int centralElegida = int.Parse(Session["CentralElegida"].ToString());

                    centros central = Datos.ObtenerCentroPorID(centralElegida);

                    ViewData["accionesmejora"] = Datos.ListarAccionesMejora(centralElegida, 12, id);
                }

                List<usuarios> usus = Datos.ListarParticipantesAsignar(id);
                ViewData["usuariosasignar"] = usus;
                List<VISTA_ReunionParticipantes> ususasig = Datos.ListarParticipantesAsignados(id);
                ViewData["usuariosasignados"] = ususasig;
                ViewData["documentosreunion"] = Datos.ListarDocumentosReunion(id);

                Session["EdicionReunionMensaje"] = "Participante asignado correctamente";
                #endregion

                return Redirect(Url.RouteUrl(new { controller = "ActasReunion", action = "detalle_reunion", id = id }));
                #endregion
            }
            else
            {
                #region recarga
                reuniones reu = Datos.GetDatosReunion(id);
                ViewData["reunion"] = reu;
                if (Session["CentralElegida"] != null)
                {
                    int centralElegida = int.Parse(Session["CentralElegida"].ToString());

                    centros central = Datos.ObtenerCentroPorID(centralElegida);

                    ViewData["accionesmejora"] = Datos.ListarAccionesMejora(centralElegida, 12, id);
                }

                List<usuarios> usus = Datos.ListarParticipantesAsignar(id);
                ViewData["usuariosasignar"] = usus;
                List<VISTA_ReunionParticipantes> ususasig = Datos.ListarParticipantesAsignados(id);
                ViewData["usuariosasignados"] = ususasig;
                ViewData["documentosreunion"] = Datos.ListarDocumentosReunion(id);
                return View();
                #endregion
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

    }
}
