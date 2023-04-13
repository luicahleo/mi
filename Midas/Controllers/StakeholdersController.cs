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
    public class StakeholdersController : Controller
    {
        public ActionResult stakeholders()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }
            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            ViewData["stakeholdersN4"] = Datos.ListarStakeholdersN4(centroseleccionado.id);

            #region generacion fichero
            if (Session["nombreArchivo"] != null && Session["nombreArchivo"].ToString() != "")
            {
                if (Session["nombreArchivo"].ToString().Contains("xlsx"))
                {
                    Response.ContentType = "application/msexcel";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=ExportacionStakeholders.xlsx");
                    Response.TransmitFile(Session["nombreArchivo"].ToString());
                    Response.End();
                }
                Session["nombreArchivo"] = "";
            }
            #endregion

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult stakeholders(FormCollection collection)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            VISTA_ObtenerUsuario user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());

            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));

                #region generacion fichero
                string sourceFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaPlantillaWord"] + "ExportacionStakeholders.xlsx");
                string destinationFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaGeneracionWord"] + "ExportacionStakeholders_" + DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + ".xlsx");

                Session["source"] = sourceFile;
                Session["destino"] = destinationFile;
                // Create a copy of the template file and open the copy 
                System.IO.File.Copy(sourceFile, destinationFile, true);

                using (SpreadsheetDocument document = SpreadsheetDocument.Open(destinationFile, true))
                {
                    #region nivel1
                    List<stakeholders_nivel1> nivel1 = Datos.ListarStakeholdersN1();
                    SheetData sheetData = document.WorkbookPart.WorksheetParts.ElementAt(2).Worksheet.Elements<SheetData>().First();
                    foreach (stakeholders_nivel1 sh in nivel1)
                    {
                        Row row = new Row();

                        string titulo = string.Empty;

                        if (sh.denominacion == null)
                            titulo = string.Empty;
                        else
                            titulo = sh.denominacion;
                        
                        row.Append(
                            Datos.ConstructCell(titulo, CellValues.String));

                        sheetData.AppendChild(row);
                    }
                    #endregion

                    #region nivel2
                    List<VISTA_StakeholdersN2> nivel2 = Datos.ListarStakeholdersN2();
                    SheetData sheetData2 = document.WorkbookPart.WorksheetParts.ElementAt(1).Worksheet.Elements<SheetData>().First();
                    foreach (VISTA_StakeholdersN2 sh2 in nivel2)
                    {
                        Row row = new Row();

                        string nivel1denominacion = string.Empty;
                        string titulo = string.Empty;

                        if (sh2.denominacion == null)
                            nivel1denominacion = string.Empty;
                        else
                            nivel1denominacion = sh2.denominacion;
                        if (sh2.denominacionn2 == null)
                            titulo = string.Empty;
                        else
                            titulo = sh2.denominacionn2;
                        
                        row.Append(
                            Datos.ConstructCell(nivel1denominacion.ToString(), CellValues.String),
                            Datos.ConstructCell(titulo, CellValues.String));

                        sheetData2.AppendChild(row);
                    }
                    #endregion

                    #region nivel3

                    List<VISTA_StakeholdersN3> nivel3 = Datos.ListarStakeholdersN3();
                    SheetData sheetData3 = document.WorkbookPart.WorksheetParts.ElementAt(0).Worksheet.Elements<SheetData>().First();
                    foreach (VISTA_StakeholdersN3 sh3 in nivel3)
                    {
                        Row row = new Row();

                        string nivel1denominacion = string.Empty;
                        string nivel2denominacion = string.Empty;
                        string titulo = string.Empty;

                        if (sh3.denominacion == null)
                            nivel1denominacion = string.Empty;
                        else
                            nivel1denominacion = sh3.denominacion;
                        if (sh3.denominacionn2 == null)
                            nivel2denominacion = string.Empty;
                        else
                            nivel2denominacion = sh3.denominacionn2;
                        if (sh3.denominacionn3 == null)
                            titulo = string.Empty;
                        else
                            titulo = sh3.denominacionn3;
                        
                        row.Append(
                            Datos.ConstructCell(nivel1denominacion.ToString(), CellValues.String),
                            Datos.ConstructCell(nivel2denominacion, CellValues.String),
                            Datos.ConstructCell(titulo, CellValues.String));

                        sheetData3.AppendChild(row);
                    }                    

                    #endregion

                    // save worksheet
                    document.WorkbookPart.WorksheetParts.First().Worksheet.Save();

                    document.WorkbookPart.Workbook.Save();

                    Session["nombreArchivo"] = destinationFile;
                }
                #endregion

            return RedirectToAction("Stakeholders", "stakeholders");
        }

        public ActionResult editar_stakeholdern4(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            ViewData["idStakeholderN4"] = id;

            ViewData["listashn3"] = Datos.ListarStakeholdersN3();
            stakeholders_nivel4 buscarStakeholder = Datos.ObtenerStakeholderN4(id);
            ViewData["EditarStakeholderN4"] = buscarStakeholder;
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult editar_stakeholdern4(int id, FormCollection collection)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            ViewData["idStakeholderN4"] = id;

            try
            {
                #region añadir stakeholder
                stakeholders_nivel4 actualizar = new stakeholders_nivel4();
                actualizar.id = id;

                if (collection["ctl00$MainContent$txtNombre"] != null)
                {
                    centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));

                    if (collection["ctl00$MainContent$hdnIdDocumento"] != null && collection["ctl00$MainContent$hdnIdDocumento"] != "")
                        actualizar.id = int.Parse(collection["ctl00$MainContent$hdnIdDocumento"]);
                    actualizar.denominacion = collection["ctl00$MainContent$txtNombre"];
                    actualizar.idnivel3 = int.Parse(collection["ctl00$MainContent$ddlSHN3"].ToString());
                    actualizar.necesidades = collection["ctl00$MainContent$txtNecesidades"];
                    actualizar.requisitosrel = collection["ctl00$MainContent$txtRequisitos"];
                    actualizar.idcentral = centroseleccionado.id;
                    if (actualizar.denominacion == string.Empty)
                    {
                        ViewData["listashn3"] = Datos.ListarStakeholdersN3();
                        stakeholders_nivel4 buscarStakeholder = Datos.ObtenerStakeholderN4(id);
                        ViewData["EditarStakeholderN4"] = buscarStakeholder;
                        Session["EdicionStakeholderN4Error"] = "Debe introducir un nombre para el stakeholder";
                        return View();
                    }

                    int idRef = Datos.ActualizarStakeholderN4(actualizar);

                    ViewData["listashn3"] = Datos.ListarStakeholdersN3();
                    stakeholders_nivel4 buscarshn4 = Datos.ObtenerStakeholderN4(idRef);
                    ViewData["EditarStakeholderN4"] = buscarshn4;

                    Session["EdicionStakeholderN4Mensaje"] = "Los datos han sido modificados correctamente";
                    return RedirectToAction("editar_stakeholdern4/" + idRef, "Stakeholders");
                }
                else
                {
                    ViewData["listashn3"] = Datos.ListarStakeholdersN3();
                    stakeholders_nivel4 buscarStakeholder = Datos.ObtenerStakeholderN4(id);
                    ViewData["EditarStakeholderN4"] = buscarStakeholder;
                    return View();
                }
                #endregion

            }
            catch (Exception ex)
            {
                #region exception
                Session["EdicionStakeholderN4Error"] = "FALLA" + ";" + ex.Message;
                ViewData["listashn3"] = Datos.ListarStakeholdersN3();
                stakeholders_nivel4 buscarStakeholder = Datos.ObtenerStakeholderN4(id);
                ViewData["EditarStakeholderN4"] = buscarStakeholder;
                return RedirectToAction("stakeholders", "stakeholders");
                #endregion
            }

        }

        public ActionResult eliminar_stakeholdern4(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            Datos.EliminarStakeholderN4(id);
            Session["EditarStakeholder4Resultado"] = "Registro eliminado";
            return RedirectToAction("stakeholders", "Stakeholders");
        }

    }

    
}
