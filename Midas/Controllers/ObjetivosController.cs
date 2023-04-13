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
    public class ObjetivosController : Controller
    {
        //
        // GET: /Objetivos/

        public ActionResult gestion_objetivos()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            int idCentral = centroseleccionado.id;
            ViewData["objetivosGenericos"] = Datos.ListarObjetivos(0, idCentral);
            ViewData["objetivosEspecificos"] = Datos.ListarObjetivos(1, idCentral);
            ViewData["objetivosUnidad"] = Datos.ListarObjetivos(2, idCentral);


            #region generacion fichero
            if (Session["nombreArchivo"] != null && Session["nombreArchivo"].ToString() != "")
            {
                if (Session["nombreArchivo"].ToString().Contains("xlsx"))
                {
                    Response.ContentType = "application/msexcel";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=ExportacionObjetivos.xlsx");
                    Response.TransmitFile(Session["nombreArchivo"].ToString());
                    Response.End();
                }
                Session["nombreArchivo"] = "";
            }
            #endregion

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult gestion_objetivos(FormCollection collection)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            VISTA_ObtenerUsuario user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());

            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));

            int idCentral = centroseleccionado.id;

            List<VISTA_Objetivos> genericos = Datos.ListarObjetivos(0, idCentral);
            List<VISTA_Objetivos> especificos = Datos.ListarObjetivos(1, idCentral);

            int anioFiltro = 0;
            if (collection["ctl00$MainContent$ddlAnio"] != "0")
                anioFiltro = int.Parse(collection["ctl00$MainContent$ddlAnio"]);

            if (anioFiltro != 0)
            {
                genericos = genericos.Where(x => x.Codigo.Contains(anioFiltro.ToString())).ToList();
                especificos = especificos.Where(x => x.Codigo.Contains(anioFiltro.ToString())).ToList();
            }

            #region generacion fichero
            string sourceFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaPlantillaWord"] + "ExportacionObjetivos.xlsx");
            string destinationFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaGeneracionWord"] + "ExportacionObjetivos_" + DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + ".xlsx");

            Session["source"] = sourceFile;
            Session["destino"] = destinationFile;
            // Create a copy of the template file and open the copy 
            System.IO.File.Copy(sourceFile, destinationFile, true);

            #region impresion

            using (SpreadsheetDocument document = SpreadsheetDocument.Open(destinationFile, true))
            {
                SheetData sheetData = document.WorkbookPart.WorksheetParts.ElementAt(1).Worksheet.Elements<SheetData>().First();
                foreach (VISTA_Objetivos obj in genericos)
                {
                    Row row = new Row();

                    string cod_objetivo = string.Empty;
                    string titulo = string.Empty;
                    string fecha_estimada = string.Empty;
                    string fecha_real = string.Empty;
                    string responsable = string.Empty;
                    string estado = string.Empty;
                    string nombre_ambito = string.Empty;

                    if (obj.Codigo == null)
                        cod_objetivo = string.Empty;
                    else
                        cod_objetivo = obj.Codigo;
                    if (obj.Nombre == null)
                        titulo = string.Empty;
                    else
                        titulo = obj.Nombre;
                    if (obj.Responsable == null)
                        responsable = string.Empty;
                    else
                    {
                        responsable = obj.Responsable;
                    }
                    if (obj.nombre_ambito == null)
                        nombre_ambito = string.Empty;
                    else
                    {
                        nombre_ambito = obj.nombre_ambito;
                    }
                    switch (obj.estado)
                    {
                        case 1:
                            estado = "Alcanza resultados esperados";
                            break;
                        case 2:
                            estado = "No alcanza resultados esperados";
                            break;
                        case 3:
                            estado = "Desestimado";
                            break;
                        default:
                            estado = "En seguimiento";
                            break;
                    }
                    if (obj.FechaEstimada == null)
                        fecha_estimada = string.Empty;
                    else
                        fecha_estimada = obj.FechaEstimada.ToString().Replace(" 0:00:00", "");
                    if (obj.FechaReal == null)
                        fecha_real = string.Empty;
                    else
                        fecha_real = obj.FechaReal.ToString().Replace(" 0:00:00", "");

                    row.Append(
                        Datos.ConstructCell(cod_objetivo.ToString(), CellValues.String),
                        Datos.ConstructCell(titulo, CellValues.String),
                        Datos.ConstructCell(fecha_estimada, CellValues.String),
                        Datos.ConstructCell(fecha_real, CellValues.String),
                        Datos.ConstructCell(responsable, CellValues.String),
                        Datos.ConstructCell(estado, CellValues.String),
                        Datos.ConstructCell(nombre_ambito, CellValues.String)
                        );

                    sheetData.AppendChild(row);
                }

                SheetData sheetData2 = document.WorkbookPart.WorksheetParts.ElementAt(0).Worksheet.Elements<SheetData>().First();
                foreach (VISTA_Objetivos obj in especificos)
                {
                    Row row = new Row();

                    string cod_objetivo = string.Empty;
                    string titulo = string.Empty;
                    string fecha_estimada = string.Empty;
                    string fecha_real = string.Empty;
                    string responsable = string.Empty;
                    string estado = string.Empty;
                    string nombre_ambito = string.Empty;

                    if (obj.Codigo == null)
                        cod_objetivo = string.Empty;
                    else
                        cod_objetivo = obj.Codigo;
                    if (obj.Nombre == null)
                        titulo = string.Empty;
                    else
                        titulo = obj.Nombre;
                    if (obj.Responsable == null)
                        responsable = string.Empty;
                    else
                    {
                        responsable = obj.Responsable;
                    }
                    if (obj.nombre_ambito == null)
                        nombre_ambito = string.Empty;
                    else
                    {
                        nombre_ambito = obj.nombre_ambito;
                    }
                    switch (obj.estado)
                    {
                        case 1:
                            estado = "Alcanza resultados esperados";
                            break;
                        case 2:
                            estado = "No alcanza resultados esperados";
                            break;
                        case 3:
                            estado = "Desestimado";
                            break;
                        default:
                            estado = "En seguimiento";
                            break;
                    }
                    if (obj.FechaEstimada == null)
                        fecha_estimada = string.Empty;
                    else
                        fecha_estimada = obj.FechaEstimada.ToString().Replace(" 0:00:00", "");
                    if (obj.FechaReal == null)
                        fecha_real = string.Empty;
                    else
                        fecha_real = obj.FechaReal.ToString().Replace(" 0:00:00", "");

                    row.Append(
                        Datos.ConstructCell(cod_objetivo.ToString(), CellValues.String),
                        Datos.ConstructCell(titulo, CellValues.String),
                        Datos.ConstructCell(fecha_estimada, CellValues.String),
                        Datos.ConstructCell(fecha_real, CellValues.String),
                        Datos.ConstructCell(responsable, CellValues.String),
                        Datos.ConstructCell(estado, CellValues.String),
                        Datos.ConstructCell(nombre_ambito, CellValues.String)
                        );

                    sheetData2.AppendChild(row);
                }

                // save worksheet
                document.WorkbookPart.WorksheetParts.First().Worksheet.Save();

                document.WorkbookPart.Workbook.Save();

                Session["nombreArchivo"] = destinationFile;
            }
            #endregion
            #endregion
            ViewData["objetivosGenericos"] = Datos.ListarObjetivos(0, idCentral);
            ViewData["objetivosEspecificos"] = Datos.ListarObjetivos(1, idCentral);

            return RedirectToAction("gestion_objetivos", "Objetivos");
        }

        public ActionResult asignar_objetivo(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            VISTA_ObtenerUsuario user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());

            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));

            int idCentral = centroseleccionado.id;

            objetivos objPrincipal = Datos.GetDatosObjetivo(id);

            objetivos objEspecifico = new objetivos();
            objEspecifico.idorganizacion = centroseleccionado.id;
            objEspecifico.Tipo = 1;
            objEspecifico.Codigo = objPrincipal.Codigo + "/" + centroseleccionado.siglas;
            objEspecifico.Nombre = objPrincipal.Nombre;
            objEspecifico.Descripcion = objPrincipal.Descripcion;
            objEspecifico.FechaEstimada = objPrincipal.FechaEstimada;
            objEspecifico.FechaReal = objPrincipal.FechaReal;
            objEspecifico.Coste = objPrincipal.Coste;
            objEspecifico.Medios = objPrincipal.Medios;
            objEspecifico.Seguimiento = objPrincipal.Seguimiento;
            objEspecifico.estado = objPrincipal.estado;
            objEspecifico.Comentarios = objPrincipal.Comentarios;
            objEspecifico.idReferencia = objPrincipal.id;
            objEspecifico.metodomedicion = objPrincipal.metodomedicion;
            Datos.ActualizarObjetivo(objEspecifico);

            Session["EditarObjetivosResultado"] = "Se ha asignado el objetivo a su central";

            ViewData["objetivosGenericos"] = Datos.ListarObjetivos(0, idCentral);
            ViewData["objetivosEspecificos"] = Datos.ListarObjetivos(1, idCentral);

            return RedirectToAction("gestion_objetivos", "Objetivos");
        }

        public ActionResult detalle_objetivo(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            Session["modulo"] = "1";

            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            int idCentral = centroseleccionado.id;  

            objetivos obj = Datos.GetDatosObjetivo(id);
            ViewData["objetivo"] = obj;
            ViewData["despliegue"] = Datos.ListarAccionesObjetivo(id);
            ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 1, id);
            ViewData["responsables"] = Datos.ListarUsuariosCentral(idCentral);
            ViewData["centralesAsignables"] = Datos.ListarCentrosAsignablesObjetivo(id);
            ViewData["centralesAsignadas"] = Datos.ListarCentrosAsignadosObjetivo(id);
            ViewData["aspectos"] = Datos.ListarAspectos();
            ViewData["ambitos"] = Datos.ListarAmbitosOrderByNombreAmbito();
            ViewData["tecnologiasAsignables"] = Datos.ListarTecnologiasAsignablesObjetivo(id);
            ViewData["tecnologiasAsignadas"] = Datos.ListarTecnologiasAsignadasObjetivo(id);

            #region generacion fichero
            if (Session["nombreArchivo"] != null && Session["nombreArchivo"].ToString() != "")
            {
                if (Session["nombreArchivo"].ToString().Contains("docx"))
                {
                    Response.ContentType = "application/msword";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=FichaControl.docx");
                    Response.TransmitFile(Session["nombreArchivo"].ToString());
                    Response.End();
                }
                Session["nombreArchivo"] = "";
            }
            #endregion

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult detalle_objetivo(int id, FormCollection collection)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            int idCentral = centroseleccionado.id;  

            objetivos proce = new objetivos();
            string formulario = collection["hdFormularioEjecutado"];

            if (formulario == "GuardarObjetivo")
            {
                #region guardar
                try
                {

                    if (id != 0)
                    {
                        #region actualizar
                        objetivos actualizar = Datos.GetDatosObjetivo(id);
                        actualizar.ambito = int.Parse(collection["ctl00$MainContent$ddlAmbito"]);
                        actualizar.Nombre = collection["ctl00$MainContent$txtNombre"];
                        actualizar.personasinv = collection["ctl00$MainContent$txtPersonasInvolucradas"];
                        actualizar.Descripcion = collection["ctl00$MainContent$txtDescripcionObjetivo"];
                        actualizar.Responsable = int.Parse(collection["ctl00$MainContent$ddlResponsable"]);
                        actualizar.metodomedicion = collection["ctl00$MainContent$txtMetodoMedicion"];
                        actualizar.idAspecto = int.Parse(collection["ctl00$MainContent$ddlAspecto"]);
                        actualizar.Coste = collection["ctl00$MainContent$txtCoste"];
                        actualizar.Medios = collection["ctl00$MainContent$txtMedios"];
                        actualizar.Seguimiento = collection["ctl00$MainContent$txtSeguimiento"];
                        //actualizar.Comentarios = collection["ctl00$MainContent$txtComentarios"];
                        if (collection["ctl00$MainContent$ddlEstadoObjetivo"] != null)
                            actualizar.estado = int.Parse(collection["ctl00$MainContent$ddlEstadoObjetivo"].ToString());
                        if (collection["ctl00$MainContent$txtFEstimada"] != "")
                            actualizar.FechaEstimada = DateTime.Parse(collection["ctl00$MainContent$txtFEstimada"]);
                        if (collection["ctl00$MainContent$txtFReal"] != "")
                            actualizar.FechaReal = DateTime.Parse(collection["ctl00$MainContent$txtFReal"]);
                        if (collection["ctl00$MainContent$ddlEspecifico"] != null)
                            actualizar.especifico = int.Parse(collection["ctl00$MainContent$ddlEspecifico"]);

                        if (actualizar.Nombre != string.Empty  && actualizar.Descripcion != string.Empty && actualizar.Coste != string.Empty && actualizar.Medios != string.Empty && actualizar.FechaEstimada != null)
                        {
                            Datos.ActualizarObjetivo(actualizar);

                            if (actualizar.especifico == 1)
                            {
                                string hdnCentros = collection["ctl00$MainContent$hdnCentrosSeleccionados"].ToString();

                                string[] arraycentros = hdnCentros.Split(new char[] { ';' });

                                Datos.EliminarAsociacionObjetivoCentros(id);

                                if (arraycentros.Count() > 0)
                                {
                                    for (int i = 0; i < arraycentros.Count() - 1; i++)
                                    {
                                        Datos.AsociarObjetivoCentro(id, int.Parse(arraycentros[i]));
                                    }
                                }
                            }
                            if (actualizar.especifico == 2)
                            {
                                string hdnTecnologias = collection["ctl00$MainContent$hdnTecnologiasSeleccionadas"].ToString();

                                string[] arraytecnologias = hdnTecnologias.Split(new char[] { ';' });

                                Datos.EliminarAsociacionObjetivoTecnologias(id);

                                if (arraytecnologias.Count() > 0)
                                {
                                    for (int i = 0; i < arraytecnologias.Count() - 1; i++)
                                    {
                                        Datos.AsociarObjetivoTecnologia(id, int.Parse(arraytecnologias[i]));
                                    }
                                }
                            }

                            Session["EdicionObjetivoMensaje"] = "Información actualizada correctamente";
                        }
                        else
                        {
                            Session["EdicionObjetivoError"] = "Los campos marcados con (*) son obligatorios.";
                            proce = actualizar;
                            ViewData["objetivo"] = proce;
                            ViewData["despliegue"] = Datos.ListarAccionesObjetivo(id);
                            ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 1, id);
                            ViewData["centralesAsignables"] = Datos.ListarCentrosAsignablesObjetivo(id);
                            ViewData["centralesAsignadas"] = Datos.ListarCentrosAsignadosObjetivo(id);
                            ViewData["responsables"] = Datos.ListarUsuariosCentral(idCentral);
                            ViewData["tecnologiasAsignables"] = Datos.ListarTecnologiasAsignablesObjetivo(id);
                            ViewData["tecnologiasAsignadas"] = Datos.ListarTecnologiasAsignadasObjetivo(id);
                            ViewData["aspectos"] = Datos.ListarAspectos();
                            ViewData["indicadores"] = Datos.ListarIndicadoresAplicables(centroseleccionado);
                            ViewData["ambitos"] = Datos.ListarAmbitos();
                            return View();
                        }
                        #endregion
                    }
                    else
                    {
                        #region insertar
                        objetivos insertar = new objetivos();
                        insertar.ambito = int.Parse(collection["ctl00$MainContent$ddlAmbito"]);
                        insertar.Nombre = collection["ctl00$MainContent$txtNombre"];
                        insertar.personasinv = collection["ctl00$MainContent$txtPersonasInvolucradas"];
                        insertar.Descripcion = collection["ctl00$MainContent$txtDescripcionObjetivo"];
                        insertar.Responsable = int.Parse(collection["ctl00$MainContent$ddlResponsable"]);
                        insertar.metodomedicion = collection["ctl00$MainContent$txtMetodoMedicion"];
                        insertar.idAspecto = int.Parse(collection["ctl00$MainContent$ddlAspecto"]);
                        insertar.Coste = collection["ctl00$MainContent$txtCoste"];
                        insertar.Medios = collection["ctl00$MainContent$txtMedios"];
                        insertar.Seguimiento = collection["ctl00$MainContent$txtSeguimiento"];
                        insertar.siglas = centroseleccionado.siglas;

                        //insertar.Comentarios = collection["ctl00$MainContent$txtComentarios"];
                        if (collection["ctl00$MainContent$ddlEstadoObjetivo"] != null)
                            insertar.estado = int.Parse(collection["ctl00$MainContent$ddlEstadoObjetivo"].ToString());
                        if (collection["ctl00$MainContent$txtFEstimada"] != "")
                            insertar.FechaEstimada = DateTime.Parse(collection["ctl00$MainContent$txtFEstimada"]);
                        if (collection["ctl00$MainContent$txtFReal"] != "")
                            insertar.FechaReal = DateTime.Parse(collection["ctl00$MainContent$txtFReal"]);
                        if (collection["ctl00$MainContent$ddlEspecifico"] != null && collection["ctl00$MainContent$ddlEspecifico"] != "0")
                            insertar.especifico = int.Parse(collection["ctl00$MainContent$ddlEspecifico"]);

                        if (Session["CentralElegida"] != null)
                        {
                            if (centroseleccionado.id != 25)
                            {
                                insertar.Tipo = 1;
                                insertar.idorganizacion = centroseleccionado.id;
                            }
                            else
                            {
                                insertar.Tipo = 0;
                            }
                        }

                        //if (Session["CentralElegida"] != null)
                        //{
                        //    if (centroseleccionado.id != 25)
                        //    {
                        //        insertar.Tipo = 1;
                        //        insertar.idorganizacion = centroseleccionado.id;
                        //    }
                        //    else
                        //    {
                        //        insertar.Tipo = 1;
                        //    }
                        //}

                        if (insertar.Nombre != string.Empty && insertar.Descripcion != string.Empty && insertar.Coste != string.Empty && insertar.Medios != string.Empty && insertar.FechaEstimada != null)
                        {

                        int idObj = Datos.ActualizarObjetivo(insertar);

                        if (insertar.especifico == 1)
                        {
                            string hdnCentros = collection["ctl00$MainContent$hdnCentrosSeleccionados"].ToString();

                            string[] arraycentros = hdnCentros.Split(new char[] { ';' });

                            Datos.EliminarAsociacionObjetivoCentros(idObj);

                            if (arraycentros.Count() > 0)
                            {
                                for (int i = 0; i < arraycentros.Count() - 1; i++)
                                {
                                    Datos.AsociarObjetivoCentro(idObj, int.Parse(arraycentros[i]));
                                }
                            }
                        }
                        if (insertar.especifico == 2)
                        {
                            string hdnTecnologias = collection["ctl00$MainContent$hdnTecnologiasSeleccionadas"].ToString();

                            string[] arraytecnologias = hdnTecnologias.Split(new char[] { ';' });

                            Datos.EliminarAsociacionObjetivoTecnologias(idObj);

                            if (arraytecnologias.Count() > 0)
                            {
                                for (int i = 0; i < arraytecnologias.Count() - 1; i++)
                                {
                                    Datos.AsociarObjetivoTecnologia(idObj, int.Parse(arraytecnologias[i]));
                                }
                            }
                        }

                        Session["EdicionObjetivoMensaje"] = "Información actualizada correctamente";

                        objetivos obje = Datos.GetDatosObjetivo(idObj);
                        ViewData["objetivo"] = obje;
                        ViewData["despliegue"] = Datos.ListarAccionesObjetivo(idObj);
                        ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 1, id);
                        ViewData["centralesAsignables"] = Datos.ListarCentrosAsignablesObjetivo(id);
                        ViewData["centralesAsignadas"] = Datos.ListarCentrosAsignadosObjetivo(id);
                        ViewData["responsables"] = Datos.ListarUsuariosCentral(idCentral);
                        ViewData["tecnologiasAsignables"] = Datos.ListarTecnologiasAsignablesObjetivo(id);
                        ViewData["tecnologiasAsignadas"] = Datos.ListarTecnologiasAsignadasObjetivo(id);
                        ViewData["aspectos"] = Datos.ListarAspectos();
                        ViewData["indicadores"] = Datos.ListarIndicadoresAplicables(centroseleccionado);
                        ViewData["ambitos"] = Datos.ListarAmbitos();
                        return Redirect(Url.RouteUrl(new { controller = "Objetivos", action = "detalle_objetivo", id = idObj }));

                        }
                        else
                        {
                            Session["EdicionObjetivoError"] = "Los campos marcados con (*) son obligatorios.";
                            proce = insertar;
                            ViewData["objetivo"] = proce;
                            ViewData["despliegue"] = Datos.ListarAccionesObjetivo(id);
                            ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 1, id);
                            ViewData["centralesAsignables"] = Datos.ListarCentrosAsignablesObjetivo(id);
                            ViewData["centralesAsignadas"] = Datos.ListarCentrosAsignadosObjetivo(id);
                            ViewData["responsables"] = Datos.ListarUsuariosCentral(idCentral);
                            ViewData["tecnologiasAsignables"] = Datos.ListarTecnologiasAsignablesObjetivo(id);
                            ViewData["tecnologiasAsignadas"] = Datos.ListarTecnologiasAsignadasObjetivo(id);
                            ViewData["aspectos"] = Datos.ListarAspectos();
                            ViewData["indicadores"] = Datos.ListarIndicadoresAplicables(centroseleccionado);
                            ViewData["ambitos"] = Datos.ListarAmbitos();
                            return View();
                        }

                        
                        #endregion
                    }
                }
                catch (Exception ex)
                {
                    #region exception
                    proce = Datos.GetDatosObjetivo(id);
                    ViewData["objetivo"] = proce;
                    ViewData["despliegue"] = Datos.ListarAccionesObjetivo(id);
                    ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 1, id);
                    ViewData["centralesAsignables"] = Datos.ListarCentrosAsignablesObjetivo(id);
                    ViewData["centralesAsignadas"] = Datos.ListarCentrosAsignadosObjetivo(id);
                    ViewData["responsables"] = Datos.ListarUsuariosCentral(idCentral);
                    ViewData["tecnologiasAsignables"] = Datos.ListarTecnologiasAsignablesObjetivo(id);
                    ViewData["tecnologiasAsignadas"] = Datos.ListarTecnologiasAsignadasObjetivo(id);
                    ViewData["aspectos"] = Datos.ListarAspectos();
                    ViewData["indicadores"] = Datos.ListarIndicadoresAplicables(centroseleccionado);
                    ViewData["aspectos"] = Datos.ListarAspectos();
                    ViewData["indicadores"] = Datos.ListarIndicadoresAplicables(centroseleccionado);
                    ViewData["ambitos"] = Datos.ListarAmbitos();
                    return View();
                    #endregion
                }
                #endregion
            }

            if (formulario == "btnNuevaAccion")
            {
                #region nueva accion
                despliegue nuevaAccion = new despliegue();
                try
                {
                    try
                    {
                        if (collection["ctl00$MainContent$txtActividad"] != null)
                        {
                            #region alta y modificacion
                            

                            objetivos obj = new objetivos();
                            obj = Datos.GetDatosObjetivo(id);
                            nuevaAccion.NumeroAccionDespliegue = obj.Codigo;

                            nuevaAccion.Nombre = collection["ctl00$MainContent$txtActividad"];
                            nuevaAccion.Responsable = int.Parse(collection["ctl00$MainContent$ddlResponsableAccion"]);
                            nuevaAccion.Descripcion = collection["ctl00$MainContent$txtDescripcion"];
                            nuevaAccion.Descripcion = nuevaAccion.Descripcion.Replace("'", "");
                            nuevaAccion.Recursos = collection["ctl00$MainContent$txtRecursos"];
                            nuevaAccion.Recursos = nuevaAccion.Recursos.Replace("'", "");
                            nuevaAccion.Porcentaje = int.Parse(collection["ctl00$MainContent$txtConsecucion"]);
                            nuevaAccion.Comentarios = collection["ctl00$MainContent$txtComentariosAccion"];
                            nuevaAccion.Comentarios = nuevaAccion.Comentarios.Replace("'", "");
                            nuevaAccion.Estado = int.Parse(collection["ctl00$MainContent$ddlEstadoAccion"]);
                            if (collection["ctl00$MainContent$txtFechaEstimada"] != null && collection["ctl00$MainContent$txtFechaEstimada"] != "")
                                nuevaAccion.FechaEstimada = DateTime.Parse(collection["ctl00$MainContent$txtFechaEstimada"].ToString());
                            if (collection["ctl00$MainContent$txtFechaReal"] != null && collection["ctl00$MainContent$txtFechaReal"] != "")
                                nuevaAccion.FechaReal = DateTime.Parse(collection["ctl00$MainContent$txtFechaReal"].ToString());
                            nuevaAccion.idObjetivo = id;

                            if (collection["ctl00$MainContent$hdnIdAccion"] != null && collection["ctl00$MainContent$hdnIdAccion"] != "0")
                            {
                                nuevaAccion.id = int.Parse(collection["ctl00$MainContent$hdnIdAccion"]);
                                if (nuevaAccion.Nombre != string.Empty && nuevaAccion.Descripcion != string.Empty && nuevaAccion.FechaEstimada != null && nuevaAccion.Recursos != string.Empty)
                                {
                                    Datos.ModificarAccionObjetivo(nuevaAccion);
                                    Session["EdicionObjetivoMensaje"] = "Acción actualizada correctamente";
                                }
                                else
                                {
                                    Session["EdicionObjetivoError"] = "Los campos marcados con (*) son obligatorios.";
                                }
                            }
                            else
                            {
                                if (nuevaAccion.Nombre != string.Empty && nuevaAccion.Descripcion != string.Empty && nuevaAccion.FechaEstimada != null && nuevaAccion.Recursos != string.Empty)
                                {
                                    Datos.InsertarAccionObjetivo(nuevaAccion);
                                    Session["EdicionObjetivoMensaje"] = "Acción añadida correctamente";
                                }
                                else
                                {
                                    Session["accionFallida"] = nuevaAccion;

                                    Session["EdicionObjetivoError"] = "Los campos marcados con (*) son obligatorios.";
                                }
                            }

                               
                            #endregion
                        }
                        else
                        {
                            #region recarga
                            proce = Datos.GetDatosObjetivo(id);
                            ViewData["objetivo"] = proce;
                            ViewData["despliegue"] = Datos.ListarAccionesObjetivo(id);
                            ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 1, id);
                            ViewData["centralesAsignables"] = Datos.ListarCentrosAsignablesObjetivo(id);
                            ViewData["centralesAsignadas"] = Datos.ListarCentrosAsignadosObjetivo(id);
                            ViewData["responsables"] = Datos.ListarUsuariosCentral(idCentral);
                            ViewData["tecnologiasAsignables"] = Datos.ListarTecnologiasAsignablesObjetivo(id);
                            ViewData["tecnologiasAsignadas"] = Datos.ListarTecnologiasAsignadasObjetivo(id);
                            ViewData["aspectos"] = Datos.ListarAspectos();
                            ViewData["indicadores"] = Datos.ListarIndicadoresAplicables(centroseleccionado);
                            ViewData["ambitos"] = Datos.ListarAmbitos();
                            Session["EdicionObjetivoError"] = "Se ha producido un error, compruebe que los datos son válidos y han sido cumplimentados";

                            return View();
                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        #region exception
                        Session["accionFallida"] = nuevaAccion;

                        proce = Datos.GetDatosObjetivo(id);
                        ViewData["objetivo"] = proce;
                        ViewData["despliegue"] = Datos.ListarAccionesObjetivo(id);
                        ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 1, id);
                        ViewData["centralesAsignables"] = Datos.ListarCentrosAsignablesObjetivo(id);
                        ViewData["centralesAsignadas"] = Datos.ListarCentrosAsignadosObjetivo(id);
                        ViewData["responsables"] = Datos.ListarUsuariosCentral(idCentral);
                        ViewData["tecnologiasAsignables"] = Datos.ListarTecnologiasAsignablesObjetivo(id);
                        ViewData["tecnologiasAsignadas"] = Datos.ListarTecnologiasAsignadasObjetivo(id);
                        ViewData["aspectos"] = Datos.ListarAspectos();
                        ViewData["indicadores"] = Datos.ListarIndicadoresAplicables(centroseleccionado);
                        ViewData["ambitos"] = Datos.ListarAmbitos();
                        Session["EdicionObjetivoError"] = "Se ha producido un error, compruebe que los datos son válidos y han sido cumplimentados";

                        return View();
                        #endregion
                    }
                    #region recarga
                    proce = Datos.GetDatosObjetivo(id);
                    proce.ambito = int.Parse(collection["ctl00$MainContent$ddlAmbito"]);
                    proce.Nombre = collection["ctl00$MainContent$txtNombre"];
                    proce.personasinv = collection["ctl00$MainContent$txtPersonasInvolucradas"];
                    proce.Descripcion = collection["ctl00$MainContent$txtDescripcionObjetivo"];
                    proce.Responsable = int.Parse(collection["ctl00$MainContent$ddlResponsable"]);
                    proce.metodomedicion = collection["ctl00$MainContent$txtMetodoMedicion"];
                    proce.idAspecto = int.Parse(collection["ctl00$MainContent$ddlAspecto"]);
                    proce.Coste = collection["ctl00$MainContent$txtCoste"];
                    proce.Medios = collection["ctl00$MainContent$txtMedios"];
                    proce.Seguimiento = collection["ctl00$MainContent$txtSeguimiento"];
                    //actualizar.Comentarios = collection["ctl00$MainContent$txtComentarios"];
                    if (collection["ctl00$MainContent$ddlEstadoObjetivo"] != null)
                        proce.estado = int.Parse(collection["ctl00$MainContent$ddlEstadoObjetivo"].ToString());
                    if (collection["ctl00$MainContent$txtFEstimada"] != "")
                        proce.FechaEstimada = DateTime.Parse(collection["ctl00$MainContent$txtFEstimada"]);
                    if (collection["ctl00$MainContent$txtFReal"] != "")
                        proce.FechaReal = DateTime.Parse(collection["ctl00$MainContent$txtFReal"]);
                    if (collection["ctl00$MainContent$ddlEspecifico"] != null)
                        proce.especifico = int.Parse(collection["ctl00$MainContent$ddlEspecifico"]);
                    ViewData["objetivo"] = proce;
                    ViewData["despliegue"] = Datos.ListarAccionesObjetivo(id);
                    ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 1, id);
                    ViewData["centralesAsignables"] = Datos.ListarCentrosAsignablesObjetivo(id);
                    ViewData["centralesAsignadas"] = Datos.ListarCentrosAsignadosObjetivo(id);
                    ViewData["responsables"] = Datos.ListarUsuariosCentral(idCentral);
                    ViewData["tecnologiasAsignables"] = Datos.ListarTecnologiasAsignablesObjetivo(id);
                    ViewData["tecnologiasAsignadas"] = Datos.ListarTecnologiasAsignadasObjetivo(id);
                    ViewData["aspectos"] = Datos.ListarAspectos();
                    ViewData["indicadores"] = Datos.ListarIndicadoresAplicables(centroseleccionado);
                    ViewData["ambitos"] = Datos.ListarAmbitos();
                    #endregion

                    return View();
                }
                catch (Exception ex)
                {
                    #region exception
                    proce = Datos.GetDatosObjetivo(id);
                    ViewData["objetivo"] = proce;
                    ViewData["despliegue"] = Datos.ListarAccionesObjetivo(id);
                    ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 1, id);
                    ViewData["centralesAsignables"] = Datos.ListarCentrosAsignablesObjetivo(id);
                    ViewData["centralesAsignadas"] = Datos.ListarCentrosAsignadosObjetivo(id);
                    ViewData["responsables"] = Datos.ListarUsuariosCentral(idCentral);
                    ViewData["tecnologiasAsignables"] = Datos.ListarTecnologiasAsignablesObjetivo(id);
                    ViewData["tecnologiasAsignadas"] = Datos.ListarTecnologiasAsignadasObjetivo(id);
                    ViewData["aspectos"] = Datos.ListarAspectos();
                    ViewData["indicadores"] = Datos.ListarIndicadoresAplicables(centroseleccionado);
                    ViewData["ambitos"] = Datos.ListarAmbitos();
                    return View();
                    #endregion
                }
                #endregion
            }

            if (formulario == "btnImprimirFC")
            {
                #region imprimir
                string sourceFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaPlantillaWord"] + "FormatoFC.docx");
                string destinationFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaGeneracionWord"] + "FormatoFC_" + DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + ".docx");

                Session["source"] = sourceFile;
                Session["destino"] = destinationFile;
                // Create a copy of the template file and open the copy 
                System.IO.File.Copy(sourceFile, destinationFile, true);

                #region impresion

                Dictionary<string, string> keyValues = new Dictionary<string, string>();

                objetivos datosObj = Datos.GetDatosObjetivo(id);

                if (datosObj != null)
                {
                    keyValues.Add("T_CodObjetivo", datosObj.Codigo.Replace("\r\n", "<w:br/>"));
                    keyValues.Add("T_TituloObjetivo", datosObj.Nombre.Replace("\r\n", "<w:br/>"));
                    string anio = datosObj.Codigo.Split('/')[2];
                    keyValues.Add("T_Anio", anio);   
                }

                SearchAndReplace(destinationFile, keyValues);

                WordprocessingDocument doc = WordprocessingDocument.Open(destinationFile, true);

                var runPropertiesDespliegue8 = new DocumentFormat.OpenXml.Wordprocessing.RunProperties();
                runPropertiesDespliegue8.AppendChild(new RunFonts() { Ascii = "Calibri" });
                runPropertiesDespliegue8.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "22" });


                DocumentFormat.OpenXml.Wordprocessing.Table tablaPrincipal = doc.MainDocumentPart.Document.Body.Descendants<DocumentFormat.OpenXml.Wordprocessing.Table>().ElementAt(1);


                //LISTADO DESPLIEGUE
                List<VISTA_Despliegue> despliegue = Datos.ListarAccionesObjetivo(id);


                if (despliegue.Count > 1)
                {
                    DocumentFormat.OpenXml.Wordprocessing.Table tabladespliegue = (DocumentFormat.OpenXml.Wordprocessing.Table)doc.MainDocumentPart.Document.Body.Descendants<DocumentFormat.OpenXml.Wordprocessing.Table>().ElementAt(1);
                    for (int i = 1; i < despliegue.Count; i++)
                    {
                        var primerafila = (DocumentFormat.OpenXml.Wordprocessing.TableRow)tabladespliegue.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>().ElementAt(1).Clone();
                        tabladespliegue.Append(primerafila);
                    }

                }

                if (despliegue.Count > 0)
                {
                    for (int i = 0; i < despliegue.Count; i++)
                    {
                        DocumentFormat.OpenXml.Wordprocessing.Table tabladespliegue = doc.MainDocumentPart.Document.Body.Descendants<DocumentFormat.OpenXml.Wordprocessing.Table>().ElementAt(1);

                        var runPropertiesDespliegue1 = new DocumentFormat.OpenXml.Wordprocessing.RunProperties();
                        runPropertiesDespliegue1.AppendChild(new RunFonts() { Ascii = "Calibri" });
                        runPropertiesDespliegue1.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "22" });

                        var primerafila = tabladespliegue.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>().ElementAt(i + 1);

                        DocumentFormat.OpenXml.Wordprocessing.TableCell celdanaccion = primerafila.Elements<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(0);
                        celdanaccion.RemoveAllChildren();
                        var parrafo = celdanaccion.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph());
                        var runprocesos = parrafo.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Run());
                        runprocesos.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesDespliegue1);
                        runprocesos.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Text(despliegue[i].NumeroAccionDespliegue));

                        var runPropertiesDespliegue2 = new DocumentFormat.OpenXml.Wordprocessing.RunProperties();
                        runPropertiesDespliegue2.AppendChild(new RunFonts() { Ascii = "Calibri" });
                        runPropertiesDespliegue2.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "22" });


                        DocumentFormat.OpenXml.Wordprocessing.TableCell celdafase = primerafila.Elements<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(1);
                        celdafase.RemoveAllChildren();
                        parrafo = celdafase.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph());
                        runprocesos = parrafo.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Run());
                        runprocesos.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesDespliegue2);
                        runprocesos.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Text(despliegue[i].Nombre));

                        var runPropertiesDespliegue4 = new DocumentFormat.OpenXml.Wordprocessing.RunProperties();
                        runPropertiesDespliegue4.AppendChild(new RunFonts() { Ascii = "Calibri" });
                        runPropertiesDespliegue4.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "22" });


                        DocumentFormat.OpenXml.Wordprocessing.TableCell celdaResponsable = primerafila.Elements<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(2);
                        celdaResponsable.RemoveAllChildren();
                        parrafo = celdaResponsable.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph());
                        runprocesos = parrafo.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Run());
                        runprocesos.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesDespliegue4);
                        runprocesos.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Text(despliegue[i].NombreResponsable));

                        var runPropertiesDespliegue3 = new DocumentFormat.OpenXml.Wordprocessing.RunProperties();
                        runPropertiesDespliegue3.AppendChild(new RunFonts() { Ascii = "Calibri" });
                        runPropertiesDespliegue3.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "22" });


                        DocumentFormat.OpenXml.Wordprocessing.TableCell celdadescripcion = primerafila.Elements<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(6);
                        celdadescripcion.RemoveAllChildren();
                        parrafo = celdadescripcion.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph());
                        runprocesos = parrafo.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Run());
                        runprocesos.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesDespliegue3);
                        runprocesos.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Text(despliegue[i].FechaReal.ToString().Replace(" 0:00:00", "")));
                        //keyValues.Add("T_Plazo" + (i + 1).ToString(), despliegue[i].FechaEstimada.ToString().Replace(" 0:00:00", "").Replace("\r\n", "<w:br/>"));

                        var runPropertiesDespliegueConsecucion = new DocumentFormat.OpenXml.Wordprocessing.RunProperties();
                        runPropertiesDespliegueConsecucion.AppendChild(new RunFonts() { Ascii = "Calibri" });
                        runPropertiesDespliegueConsecucion.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "22" });

                        DocumentFormat.OpenXml.Wordprocessing.TableCell celdaConsecucion = primerafila.Elements<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(7);
                        celdaConsecucion.RemoveAllChildren();
                        parrafo = celdaConsecucion.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph());
                        runprocesos = parrafo.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Run());
                        runprocesos.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesDespliegueConsecucion);
                        runprocesos.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Text(despliegue[i].Porcentaje.ToString()));


                        var runPropertiesDespliegueRecursos = new DocumentFormat.OpenXml.Wordprocessing.RunProperties();
                        runPropertiesDespliegueRecursos.AppendChild(new RunFonts() { Ascii = "Calibri" });
                        runPropertiesDespliegueRecursos.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "22" });

                        DocumentFormat.OpenXml.Wordprocessing.TableCell celdaRecursos = primerafila.Elements<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(8);
                        celdaRecursos.RemoveAllChildren();
                        parrafo = celdaRecursos.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph());
                        runprocesos = parrafo.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Run());
                        runprocesos.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesDespliegueRecursos);
                        runprocesos.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Text(despliegue[i].Comentarios));                     
                                               

                    }
                }


                doc.MainDocumentPart.Document.Save();

                doc.Close();

                accionesmejora acci = Datos.GetDatosAccionMejora(id);

                Session["nombreArchivo"] = destinationFile;
                #endregion


                objetivos objetivo = Datos.GetDatosObjetivo(id);
                ViewData["objetivo"] = objetivo;
                ViewData["despliegue"] = Datos.ListarAccionesObjetivo(id);
                ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 1, id);
                ViewData["centralesAsignables"] = Datos.ListarCentrosAsignablesObjetivo(id);
                ViewData["centralesAsignadas"] = Datos.ListarCentrosAsignadosObjetivo(id);
                ViewData["responsables"] = Datos.ListarUsuariosCentral(idCentral);
                ViewData["tecnologiasAsignables"] = Datos.ListarTecnologiasAsignablesObjetivo(id);
                ViewData["tecnologiasAsignadas"] = Datos.ListarTecnologiasAsignadasObjetivo(id);
                ViewData["aspectos"] = Datos.ListarAspectos();
                ViewData["indicadores"] = Datos.ListarIndicadoresAplicables(centroseleccionado);
                ViewData["ambitos"] = Datos.ListarAmbitos();
                return RedirectToAction("detalle_objetivo/" + id, "Objetivos");
                #endregion
            }

            if (formulario == "btnImprimir")
            {
                #region imprimir
                string sourceFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaPlantillaWord"] + "FichaObjetivo.docx");
                string destinationFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaGeneracionWord"] + "FichaObjetivo_" + DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + ".docx");

                Session["source"] = sourceFile;
                Session["destino"] = destinationFile;
                // Create a copy of the template file and open the copy 
                System.IO.File.Copy(sourceFile, destinationFile, true);

                #region impresion
                objetivos actualizar = Datos.GetDatosObjetivo(id);

                if (actualizar.Nombre != null)
                {
                    // create key value pair, key represents words to be replace and 
                    //values represent values in document in place of keys.
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add("T_Codigo", actualizar.Codigo.Replace("\r\n", "<w:br/>"));
                    keyValues.Add("T_Nombre", actualizar.Nombre.Replace("\r\n", "<w:br/>"));
                    keyValues.Add("T_PersonasInv", actualizar.personasinv.Replace("\r\n", "<w:br/>").Replace("&", "&amp;"));
                    string ambito = string.Empty;
                    if (actualizar.ambito == 0)
                    {
                        ambito = "General";
                    }
                    else
                    {
                        ambito = Datos.ListarAmbitos().Where(x => x.id == actualizar.ambito).FirstOrDefault().nombre_ambito;
                    }
                    keyValues.Add("T_Ambito", ambito);
                    if (actualizar.FechaEstimada != null)
                        keyValues.Add("T_FechaEstimada", actualizar.FechaEstimada.ToString().Replace(" 0:00:00", "").Replace("\r\n", "<w:br/>"));
                    else
                        keyValues.Add("T_FechaEstimada", "");
                    if (actualizar.FechaReal != null)
                        keyValues.Add("T_FechaReal", actualizar.FechaReal.ToString().Replace(" 0:00:00", "").Replace("\r\n", "<w:br/>"));
                    else
                        keyValues.Add("T_FechaReal", "");
                    if (actualizar.Medios != null)
                        keyValues.Add("T_Medios", actualizar.Medios.Replace("\r\n", "<w:br/>"));
                    else
                        keyValues.Add("T_Medios", "");

                    if (actualizar.Coste != null)
                        keyValues.Add("T_Coste", actualizar.Coste.Replace("\r\n", "<w:br/>"));
                    else
                        keyValues.Add("T_Coste", "");

                    if (actualizar.Responsable != null)
                    {
                        usuarios usu =  Datos.ObtenerUsuario(actualizar.Responsable);                       
                        keyValues.Add("T_Responsable", usu.nombre.Replace("\r\n", "<w:br/>"));
                    }
                    else
                        keyValues.Add("T_Responsable", "");

                    if (actualizar.idAspecto != null && actualizar.idAspecto !=0)
                    {
                        aspecto_tipo aspectoObj = Datos.ObtenerTipoAspecto(int.Parse(actualizar.idAspecto.ToString()));
                        aspecto_grupo agrupo = Datos.ObtenerGrupoAspecto(aspectoObj.Grupo);
                        string aspecto = aspectoObj.Codigo + "\\" + agrupo.grupo + "\\" + aspectoObj.Nombre;
                        if (aspecto != null)
                        {
                            keyValues.Add("T_Aspecto", aspecto.Replace("\r\n", "<w:br/>"));
                        }
                    }
                    else
                        keyValues.Add("T_Aspecto", "");

                    if (actualizar.metodomedicion != null)
                        keyValues.Add("T_Indicador", actualizar.metodomedicion.Replace("\r\n", "<w:br/>"));
                    else
                        keyValues.Add("T_Indicador", "");

                    if (actualizar.Seguimiento != null)
                        keyValues.Add("T_Seguimiento", actualizar.Seguimiento.Replace("\r\n", "<w:br/>").Replace("&", "&amp;"));
                    else
                        keyValues.Add("T_Seguimiento", "");

                    if (actualizar.Descripcion != null)
                        keyValues.Add("T_Descripcion", actualizar.Descripcion.Replace("\r\n", "<w:br/>").Replace("&", "&amp;"));
                    else
                        keyValues.Add("T_Descripcion", "");

                    //if (actualizar.Comentarios != null)
                    //    keyValues.Add("T_Comentarios", actualizar.Comentarios.Replace("\r\n", "<w:br/>"));
                    //else
                    //    keyValues.Add("T_Comentarios", "");

                    SearchAndReplace(destinationFile, keyValues);

                    WordprocessingDocument doc = WordprocessingDocument.Open(destinationFile, true);

                    var runPropertiesDespliegue8 = new DocumentFormat.OpenXml.Wordprocessing.RunProperties();
                    runPropertiesDespliegue8.AppendChild(new RunFonts() { Ascii = "Calibri" });
                    runPropertiesDespliegue8.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "22" });


                    DocumentFormat.OpenXml.Wordprocessing.Table tablaPrincipal = doc.MainDocumentPart.Document.Body.Descendants<DocumentFormat.OpenXml.Wordprocessing.Table>().ElementAt(1);

                    var filadespliegue = tablaPrincipal.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>().ElementAt(9);

                    DocumentFormat.OpenXml.Wordprocessing.TableCell celdaaccionesmejora = filadespliegue.Elements<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(0);
                    var parrafoAccionesMejora = celdaaccionesmejora.Elements<DocumentFormat.OpenXml.Wordprocessing.Paragraph>().ElementAt(0);
                    var runAccionesMejora = parrafoAccionesMejora.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Run());
                    runAccionesMejora.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesDespliegue8);
                    //ACCIONES DE MEJORA
                    List<VISTA_ListarAccionesMejora> listadoAccionesMejora = Datos.ListarAccionesMejora(idCentral, 1, actualizar.id);
                    foreach (VISTA_ListarAccionesMejora acc in listadoAccionesMejora)
                    {
                        runAccionesMejora.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Text("-" + acc.asunto));
                        runAccionesMejora.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Break());
                    }


                    //LISTADO DESPLIEGUE
                    List<VISTA_Despliegue> despliegue = Datos.ListarAccionesObjetivo(id);


                    if (despliegue.Count > 1)
                    {
                        for (int i = 1; i < despliegue.Count; i++)
                        {
                            DocumentFormat.OpenXml.Wordprocessing.Table clon = (DocumentFormat.OpenXml.Wordprocessing.Table)doc.MainDocumentPart.Document.Body.Descendants<DocumentFormat.OpenXml.Wordprocessing.Table>().ElementAt(2).Clone();
                            
                            doc.MainDocumentPart.Document.Body.Append(clon);
                            doc.MainDocumentPart.Document.Body.Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph());
                            
                        }

                    }

                    if (despliegue.Count > 0)
                    {
                        for (int i = 0; i < despliegue.Count; i++)
                        {
                            DocumentFormat.OpenXml.Wordprocessing.Table tabladespliegue = doc.MainDocumentPart.Document.Body.Descendants<DocumentFormat.OpenXml.Wordprocessing.Table>().ElementAt(2 + i);

                            var runPropertiesDespliegue1 = new DocumentFormat.OpenXml.Wordprocessing.RunProperties();
                            runPropertiesDespliegue1.AppendChild(new RunFonts() { Ascii = "Calibri" });
                            runPropertiesDespliegue1.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "22" });

                            var primerafila = tabladespliegue.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>().ElementAt(1);
                            var segundafila = tabladespliegue.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>().ElementAt(3);
                            var tercerafila = tabladespliegue.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>().ElementAt(5);

                            DocumentFormat.OpenXml.Wordprocessing.TableCell celdanaccion = primerafila.Elements<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(0);
                            celdanaccion.RemoveAllChildren();
                            var parrafo = celdanaccion.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph());
                            var runprocesos = parrafo.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Run());
                            runprocesos.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesDespliegue1);
                            runprocesos.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Text(despliegue[i].NumeroAccionDespliegue));

                            var runPropertiesDespliegue2 = new DocumentFormat.OpenXml.Wordprocessing.RunProperties();
                            runPropertiesDespliegue2.AppendChild(new RunFonts() { Ascii = "Calibri" });
                            runPropertiesDespliegue2.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "22" });


                            DocumentFormat.OpenXml.Wordprocessing.TableCell celdafase = tercerafila.Elements<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(0);
                            celdafase.RemoveAllChildren();
                            parrafo = celdafase.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph());                            
                            runprocesos = parrafo.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Run());
                            runprocesos.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesDespliegue2);
                            runprocesos.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Text(despliegue[i].Nombre));

                            var runPropertiesDespliegue3 = new DocumentFormat.OpenXml.Wordprocessing.RunProperties();
                            runPropertiesDespliegue3.AppendChild(new RunFonts() { Ascii = "Calibri" });
                            runPropertiesDespliegue3.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "22" });


                            DocumentFormat.OpenXml.Wordprocessing.TableCell celdadescripcion = primerafila.Elements<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(1);
                            celdadescripcion.RemoveAllChildren();
                            parrafo = celdadescripcion.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph());
                            runprocesos = parrafo.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Run());
                            runprocesos.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesDespliegue3);
                            runprocesos.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Text(despliegue[i].FechaEstimada.ToString().Replace(" 0:00:00", "")));
                            //keyValues.Add("T_Plazo" + (i + 1).ToString(), despliegue[i].FechaEstimada.ToString().Replace(" 0:00:00", "").Replace("\r\n", "<w:br/>"));

                            var runPropertiesDespliegue4 = new DocumentFormat.OpenXml.Wordprocessing.RunProperties();
                            runPropertiesDespliegue4.AppendChild(new RunFonts() { Ascii = "Calibri" });
                            runPropertiesDespliegue4.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "22" });


                            DocumentFormat.OpenXml.Wordprocessing.TableCell celdaResponsable = primerafila.Elements<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(2);
                            celdaResponsable.RemoveAllChildren();
                            parrafo = celdaResponsable.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph());
                            runprocesos = parrafo.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Run());
                            runprocesos.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesDespliegue4);
                            runprocesos.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Text(despliegue[i].NombreResponsable));

                            var runPropertiesDespliegueRecursos = new DocumentFormat.OpenXml.Wordprocessing.RunProperties();
                            runPropertiesDespliegueRecursos.AppendChild(new RunFonts() { Ascii = "Calibri" });
                            runPropertiesDespliegueRecursos.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "22" });
                            
                            DocumentFormat.OpenXml.Wordprocessing.TableCell celdaRecursos = segundafila.Elements<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(0);
                            celdaRecursos.RemoveAllChildren();
                            parrafo = celdaRecursos.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph());
                            runprocesos = parrafo.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Run());
                            runprocesos.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesDespliegueRecursos);
                            runprocesos.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Text(despliegue[i].Recursos));

                            var runPropertiesDespliegueComentarios = new DocumentFormat.OpenXml.Wordprocessing.RunProperties();
                            runPropertiesDespliegueComentarios.AppendChild(new RunFonts() { Ascii = "Calibri" });
                            runPropertiesDespliegueComentarios.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "22" });

                            DocumentFormat.OpenXml.Wordprocessing.TableCell celdaComentarios = tercerafila.Elements<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(2);
                            celdaComentarios.RemoveAllChildren();
                            parrafo = celdaComentarios.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph());
                            runprocesos = parrafo.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Run());
                            runprocesos.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesDespliegueComentarios);
                            runprocesos.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Text(despliegue[i].Comentarios));

                            var runPropertiesDespliegueEvidencias = new DocumentFormat.OpenXml.Wordprocessing.RunProperties();
                            runPropertiesDespliegueEvidencias.AppendChild(new RunFonts() { Ascii = "Calibri" });
                            runPropertiesDespliegueEvidencias.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "22" });

                            DocumentFormat.OpenXml.Wordprocessing.TableCell celdaEvidencias = tercerafila.Elements<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(1);
                            celdaEvidencias.RemoveAllChildren();
                            parrafo = celdaEvidencias.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph());
                            runprocesos = parrafo.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Run());
                            runprocesos.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesDespliegueEvidencias);
                            List<evidencias> listaEvidencias = Datos.ObtenerEvidenciasAccion(despliegue[i].id);
                            foreach (evidencias evi in listaEvidencias)
                            {
                                runprocesos.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Text("-" + evi.nombre + " (" + evi.nombrefichero + ")"));
                                runprocesos.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Break());
                            }

                            var runPropertiesDespliegue5 = new DocumentFormat.OpenXml.Wordprocessing.RunProperties();
                            runPropertiesDespliegue5.AppendChild(new RunFonts() { Ascii = "Calibri" });
                            runPropertiesDespliegue5.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "22" });


                            DocumentFormat.OpenXml.Wordprocessing.TableCell celdaEstado = segundafila.Elements<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(1);
                            celdaEstado.RemoveAllChildren();
                            parrafo = celdaEstado.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph());
                            runprocesos = parrafo.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Run());
                            runprocesos.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesDespliegue5);
                            switch (despliegue[i].Estado)
                            {
                                case 2:
                                    runprocesos.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Text("Ejecutado"));
                                    break;
                                case 1:
                                    runprocesos.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Text("Pendiente de ejecutar"));
                                    break;
                                default:
                                    runprocesos.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Text("No ejecutado"));
                                    break;
                            }

                            var runPropertiesDespliegueConsecucion = new DocumentFormat.OpenXml.Wordprocessing.RunProperties();
                            runPropertiesDespliegueConsecucion.AppendChild(new RunFonts() { Ascii = "Calibri" });
                            runPropertiesDespliegueConsecucion.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "22" });

                            DocumentFormat.OpenXml.Wordprocessing.TableCell celdaConsecucion = segundafila.Elements<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(2);
                            celdaConsecucion.RemoveAllChildren();
                            parrafo = celdaConsecucion.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph());
                            runprocesos = parrafo.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Run());
                            runprocesos.PrependChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>(runPropertiesDespliegueConsecucion);
                            runprocesos.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Text(despliegue[i].Porcentaje.ToString()));

                        }
                    }



                    doc.MainDocumentPart.Document.Save();

                    doc.Close();

                    SearchAndReplace(destinationFile, keyValues);

                    Session["nombreArchivo"] = destinationFile;

                }
                else
                {
                    Session["EdicionObjetivoError"] = "Es necesario informar una fecha de inicio y fin del objetivo antes de generar la ficha.";
                }
                #endregion
                objetivos objetivo = Datos.GetDatosObjetivo(id);
                ViewData["objetivo"] = objetivo;
                ViewData["despliegue"] = Datos.ListarAccionesObjetivo(id);
                ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 1, id);
                ViewData["centralesAsignables"] = Datos.ListarCentrosAsignablesObjetivo(id);
                ViewData["centralesAsignadas"] = Datos.ListarCentrosAsignadosObjetivo(id);
                ViewData["responsables"] = Datos.ListarUsuariosCentral(idCentral);
                ViewData["tecnologiasAsignables"] = Datos.ListarTecnologiasAsignablesObjetivo(id);
                ViewData["tecnologiasAsignadas"] = Datos.ListarTecnologiasAsignadasObjetivo(id);
                ViewData["aspectos"] = Datos.ListarAspectos();
                ViewData["indicadores"] = Datos.ListarIndicadoresAplicables(centroseleccionado);
                ViewData["ambitos"] = Datos.ListarAmbitos();
                return RedirectToAction("detalle_objetivo/" + id, "Objetivos");
                #endregion
            }

            #region recarga
            objetivos objet = Datos.GetDatosObjetivo(id);
            ViewData["objetivo"] = objet;
            ViewData["despliegue"] = Datos.ListarAccionesObjetivo(id);
            ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral, 1, id);
            ViewData["centralesAsignables"] = Datos.ListarCentrosAsignablesObjetivo(id);
            ViewData["centralesAsignadas"] = Datos.ListarCentrosAsignadosObjetivo(id);
            ViewData["responsables"] = Datos.ListarUsuariosCentral(idCentral);
            ViewData["tecnologiasAsignables"] = Datos.ListarTecnologiasAsignablesObjetivo(id);
            ViewData["tecnologiasAsignadas"] = Datos.ListarTecnologiasAsignadasObjetivo(id);
            ViewData["aspectos"] = Datos.ListarAspectos();
            ViewData["indicadores"] = Datos.ListarIndicadoresAplicables(centroseleccionado);
            ViewData["ambitos"] = Datos.ListarAmbitos();
            #endregion

            return View();
        }

        public ActionResult Eliminar_Objetivo(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            int idProceso = Datos.EliminarObjetivo(id);
            Session["EditarObjetivosResultado"] = "Objetivo eliminado";
            return RedirectToAction("gestion_objetivos", "Objetivos");
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

        #region evidencias
        public ActionResult evidencias(int id)
        {
            if (Session["usuario"] == null || Session["idObjetivo"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            ViewData["accion"] = Datos.ObtenerAccionPorID(id);

            //Contenido del grid
            ViewData["evidenciasaccion"] = Datos.ObtenerEvidenciasAccion(id);

            return View();
        }

        [HttpPost]
        public ActionResult evidencias(int id, FormCollection collection, HttpPostedFileBase file)
        {

            if (file != null && file.ContentLength > 0 && collection["txtNombre"] != null)
            {
                #region guardar evidencia
                var fileName = System.IO.Path.GetFileName(file.FileName);


                var path = System.IO.Path.Combine(Server.MapPath("~/Evidencias/" + id), fileName);

                if (Directory.Exists(Server.MapPath("~/Evidencias/" + id)))
                {
                    file.SaveAs(path);
                }
                else
                {
                    Directory.CreateDirectory(Server.MapPath("~/Evidencias/" + id));
                    file.SaveAs(path);
                }

                evidencias IF = new evidencias();
                IF.nombre = collection["txtNombre"];
                IF.nombrefichero = file.FileName;
                IF.enlace = path;
                IF.idaccion = id;
                IF.fecha = DateTime.Now.Date;
                Datos.InsertEvidencia(IF);
                #endregion
            }
            else
            {
                Session["error"] = 1;
            }
            return RedirectToAction("evidencias/" + id, "Objetivos");
        }

        public FileResult ObtenerEvidencia(int id)
        {
            try
            {
                ficheros IF = Datos.ObtenerEvidenciaPorID(id);
                byte[] fileBytes = System.IO.File.ReadAllBytes(IF.enlace);
                string fileName = IF.enlace.Replace(Server.MapPath("~/Evidencias") + "\\" + IF.nombre_fichero, "");
                fileName = fileName.Replace(IF.nombre_fichero, "");
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ActionResult eliminar_evidencia(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            int idAccion = Datos.EliminarEvidencia(id);
            return RedirectToAction("evidencias/" + idAccion, "Objetivos");
        }

        #endregion

        #region acciones

        public ActionResult eliminar_accion(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            int objetivo = Datos.EliminarAccionObjetivo(id);
            Session["EdicionObjetivoMensaje"] = "Acción eliminada";
            return RedirectToAction("detalle_objetivo/" + objetivo, "Objetivos");
        }

        #endregion
    }
}
