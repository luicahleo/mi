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
    public class ProcesosController : Controller
    {
        //
        // GET: /Procesos/

        public ActionResult impresion_procesos()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }
            int tecnologia = (int)Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString())).tipo;
            if (tecnologia != 4)
            {
                ViewData["procesosE"] = Datos.ListarProcesosBytecnologia(1, "E", tecnologia,1);
                ViewData["procesosO"] = Datos.ListarProcesosBytecnologia(1, "O", tecnologia,1);
                ViewData["procesosS"] = Datos.ListarProcesosBytecnologia(1, "S", tecnologia,1);
                ViewData["procesosT"] = Datos.ListarProcesosBytecnologia(1, "T", tecnologia,1);
            }
            else
            {
                ViewData["procesosE"] = Datos.ListarProcesos(1, "E");
                ViewData["procesosO"] = Datos.ListarProcesos(1, "O");
                ViewData["procesosS"] = Datos.ListarProcesos(1, "S");
                ViewData["procesosT"] = Datos.ListarProcesos(1, "T");
            }
            return View();
        }

        public ActionResult gestion_procesos()
        {
            string url = Request.Url.ToString();
            
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }
            string tipo = "E";
            string nivel = "M";
            int idOrg = 1;
            int tecnologia = (int)Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString())).tipo;

            ViewData["tecnologias"] =  Datos.ListarTecnologias();
            ViewData["padres"] = Datos.ListarPadres(idOrg, tipo, nivel);

                if (tecnologia != 4)
                {

                    ViewData["procesosE"] = Datos.ListarProcesosBytecnologia(1, "E", tecnologia,1);
                    ViewData["procesosO"] = Datos.ListarProcesosBytecnologia(1, "O", tecnologia,1);
                    ViewData["procesosS"] = Datos.ListarProcesosBytecnologia(1, "S", tecnologia,1);
                    ViewData["procesosT"] = Datos.ListarProcesosBytecnologia(1, "T", tecnologia,1);
                }
                else
                {
                if (!url.Contains("/0")) {
                    ViewData["procesosE"] = Datos.ListarProcesos(1, "E");
                    ViewData["procesosO"] = Datos.ListarProcesos(1, "O");
                    ViewData["procesosS"] = Datos.ListarProcesos(1, "S");
                    ViewData["procesosT"] = Datos.ListarProcesos(1, "T");
                }
                else
                {
                    ViewData["procesosE"] = Datos.ListarProcesosBytecnologia(1, "E", tecnologia,0);
                    ViewData["procesosO"] = Datos.ListarProcesosBytecnologia(1, "O", tecnologia,0);
                    ViewData["procesosS"] = Datos.ListarProcesosBytecnologia(1, "S", tecnologia,0);
                    ViewData["procesosT"] = Datos.ListarProcesosBytecnologia(1, "T", tecnologia,0);
                }
                    
                }

            
           
                
            
            
            

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult gestion_procesos(FormCollection collection)
        {
            string formulario = collection["hdFormularioEjecutado"];
            string tipo = collection["ctl00$MainContent$ddlTipo"];
            string nivel = collection["ctl00$MainContent$ddlNivel"];
            string tecnologia = collection["ctl00$MainContent$ddlTecnologia"];
            int tecnologiaint = (int)Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString())).tipo;
            int idOrg = 1;

            if (formulario == "GuardarElemento")
            {
                #region guardar proceso
                if (collection["ctl00$MainContent$txtCodigo"] != "" && collection["ctl00$MainContent$txtNombre"] != "")
                {
                    try
                    {
                        procesos actualizar = new procesos();
                        actualizar.organizacion = 1;

                        if (collection["ctl00$MainContent$txtNombre"] != null)
                        {
                            actualizar.cod_proceso = collection["ctl00$MainContent$txtCodigo"];
                            actualizar.nombre = collection["ctl00$MainContent$txtNombre"];
                            actualizar.tipo = collection["ctl00$MainContent$ddlTipo"];
                            actualizar.nivel = collection["ctl00$MainContent$ddlNivel"];
                            if (tecnologia != "0")
                                actualizar.tecnologia = int.Parse(tecnologia);
                            actualizar.fedicion = DateTime.Now.Date.ToString();
                            if (collection["ctl00$MainContent$ddlDependencia"] != null && int.Parse(collection["ctl00$MainContent$ddlDependencia"]) > 0)
                                actualizar.padre = int.Parse(collection["ctl00$MainContent$ddlDependencia"]);

                            if (actualizar.id == 0)
                            {
                                Datos.InsertarProceso(actualizar);
                                Session["EdicionGPMensaje"] = "Proceso añadido correctamente";
                                tipo = collection["ctl00$MainContent$ddlTipo"];
                                nivel = collection["ctl00$MainContent$ddlNivel"];

                                idOrg = 1;
                                Session["nivel"] = nivel;
                                Session["tipo"] = tipo;
                                ViewData["tecnologias"] = Datos.ListarTecnologias();
                                ViewData["padres"] = Datos.ListarPadres(idOrg, tipo, nivel);


                               
                                if (tecnologiaint != 4)
                                {
                                    ViewData["procesosE"] = Datos.ListarProcesosBytecnologia(1, "E", tecnologiaint,1);
                                    ViewData["procesosO"] = Datos.ListarProcesosBytecnologia(1, "O", tecnologiaint,1);
                                    ViewData["procesosS"] = Datos.ListarProcesosBytecnologia(1, "S", tecnologiaint,1);
                                    ViewData["procesosT"] = Datos.ListarProcesosBytecnologia(1, "T", tecnologiaint,1);
                                }
                                else
                                {
                                    ViewData["procesosE"] = Datos.ListarProcesos(1, "E");
                                    ViewData["procesosO"] = Datos.ListarProcesos(1, "O");
                                    ViewData["procesosS"] = Datos.ListarProcesos(1, "S");
                                    ViewData["procesosT"] = Datos.ListarProcesos(1, "T");
                                }
                                return View();
                            }
                        }
                        else
                        {
                            tipo = collection["ctl00$MainContent$ddlTipo"];
                            nivel = collection["ctl00$MainContent$ddlNivel"];
                            idOrg = 1;
                            Session["nivel"] = nivel;
                            Session["tipo"] = tipo;
                            ViewData["tecnologias"] = Datos.ListarTecnologias();
                            ViewData["padres"] = Datos.ListarPadres(idOrg, tipo, nivel);
                            if (int.Parse(tecnologia) != 4)
                            {
                                ViewData["procesosE"] = Datos.ListarProcesosBytecnologia(1, "E", int.Parse(tecnologia),1);
                                ViewData["procesosO"] = Datos.ListarProcesosBytecnologia(1, "O", int.Parse(tecnologia),1);
                                ViewData["procesosS"] = Datos.ListarProcesosBytecnologia(1, "S", int.Parse(tecnologia),1);
                                ViewData["procesosT"] = Datos.ListarProcesosBytecnologia(1, "T", int.Parse(tecnologia),1);
                            }
                            else
                            {
                                ViewData["procesosE"] = Datos.ListarProcesos(1, "E");
                                ViewData["procesosO"] = Datos.ListarProcesos(1, "O");
                                ViewData["procesosS"] = Datos.ListarProcesos(1, "S");
                                ViewData["procesosT"] = Datos.ListarProcesos(1, "T");
                            }
                            //Session["EditarUsuarioResultado"] = "Debe introducir un nombre de usuario";
                            return View();
                        }

                        tipo = collection["ctl00$MainContent$ddlTipo"];
                        nivel = collection["ctl00$MainContent$ddlNivel"];
                        idOrg = 1;
                        Session["nivel"] = nivel;
                        Session["tipo"] = tipo;
                        ViewData["tecnologias"] = Datos.ListarTecnologias();
                        ViewData["padres"] = Datos.ListarPadres(idOrg, tipo, nivel);
                        if (int.Parse(tecnologia) != 4)
                        {
                            ViewData["procesosE"] = Datos.ListarProcesosBytecnologia(1, "E", int.Parse(tecnologia),1);
                            ViewData["procesosO"] = Datos.ListarProcesosBytecnologia(1, "O", int.Parse(tecnologia),1);
                            ViewData["procesosS"] = Datos.ListarProcesosBytecnologia(1, "S", int.Parse(tecnologia),1);
                            ViewData["procesosT"] = Datos.ListarProcesosBytecnologia(1, "T", int.Parse(tecnologia),1);
                        }
                        else
                        {
                            ViewData["procesosE"] = Datos.ListarProcesos(1, "E");
                            ViewData["procesosO"] = Datos.ListarProcesos(1, "O");
                            ViewData["procesosS"] = Datos.ListarProcesos(1, "S");
                            ViewData["procesosT"] = Datos.ListarProcesos(1, "T");
                        }

                        return View();
                    }
                    catch (Exception ex)
                    {
                        tipo = collection["ctl00$MainContent$ddlTipo"];
                        nivel = collection["ctl00$MainContent$ddlNivel"];
                        idOrg = 1;
                        Session["nivel"] = nivel;
                        Session["tipo"] = tipo;
                        ViewData["tecnologias"] = Datos.ListarTecnologias();
                        ViewData["padres"] = Datos.ListarPadres(idOrg, tipo, nivel);
                        if (int.Parse(tecnologia) != 4)
                        {
                            ViewData["procesosE"] = Datos.ListarProcesosBytecnologia(1, "E", int.Parse(tecnologia),1);
                            ViewData["procesosO"] = Datos.ListarProcesosBytecnologia(1, "O", int.Parse(tecnologia),1);
                            ViewData["procesosS"] = Datos.ListarProcesosBytecnologia(1, "S", int.Parse(tecnologia),1);
                            ViewData["procesosT"] = Datos.ListarProcesosBytecnologia(1, "T", int.Parse(tecnologia),1);
                        }
                        else
                        {
                            ViewData["procesosE"] = Datos.ListarProcesos(1, "E");
                            ViewData["procesosO"] = Datos.ListarProcesos(1, "O");
                            ViewData["procesosS"] = Datos.ListarProcesos(1, "S");
                            ViewData["procesosT"] = Datos.ListarProcesos(1, "T");
                        }
                        //Session["EditarUsuarioResultado"] = "FALLA" + ";" + ex.Message;
                        return View();
                    }
                }
                else
                {
                    Session["errorGPMensaje"] = "Los campos marcados con (*) son obligatorios.";

                    tipo = collection["ctl00$MainContent$ddlTipo"];
                    nivel = collection["ctl00$MainContent$ddlNivel"];
                    idOrg = 1;
                    Session["nivel"] = nivel;
                    Session["tipo"] = tipo;
                    ViewData["tecnologias"] = Datos.ListarTecnologias();
                    ViewData["padres"] = Datos.ListarPadres(idOrg, tipo, nivel);
                    if (int.Parse(tecnologia) != 4)
                    {
                        ViewData["procesosE"] = Datos.ListarProcesosBytecnologia(1, "E", int.Parse(tecnologia),1);
                        ViewData["procesosO"] = Datos.ListarProcesosBytecnologia(1, "O", int.Parse(tecnologia),1);
                        ViewData["procesosS"] = Datos.ListarProcesosBytecnologia(1, "S", int.Parse(tecnologia),1);
                        ViewData["procesosT"] = Datos.ListarProcesosBytecnologia(1, "T", int.Parse(tecnologia),1);
                    }
                    else
                    {
                        ViewData["procesosE"] = Datos.ListarProcesos(1, "E");
                        ViewData["procesosO"] = Datos.ListarProcesos(1, "O");
                        ViewData["procesosS"] = Datos.ListarProcesos(1, "S");
                        ViewData["procesosT"] = Datos.ListarProcesos(1, "T");
                    }
                    //Session["EditarUsuarioResultado"] = "FALLA" + ";" + ex.Message;
                    return View();
                }
                #endregion
            }
            else
            {
                #region recarga
                tipo = collection["ctl00$MainContent$ddlTipo"];
                nivel = collection["ctl00$MainContent$ddlNivel"];
                idOrg = 1;
                Session["nivel"] = nivel;
                Session["tipo"] = tipo;
                ViewData["tecnologias"] = Datos.ListarTecnologias();
                ViewData["padres"] = Datos.ListarPadres(idOrg, tipo, nivel);
                if (tecnologiaint != 4)
                {
                    ViewData["procesosE"] = Datos.ListarProcesosBytecnologia(1, "E", tecnologiaint,1);
                    ViewData["procesosO"] = Datos.ListarProcesosBytecnologia(1, "O", tecnologiaint,1);
                    ViewData["procesosS"] = Datos.ListarProcesosBytecnologia(1, "S", tecnologiaint,1);
                    ViewData["procesosT"] = Datos.ListarProcesosBytecnologia(1, "T", tecnologiaint,1);
                }
                else
                {
                    ViewData["procesosE"] = Datos.ListarProcesos(1, "E");
                    ViewData["procesosO"] = Datos.ListarProcesos(1, "O");
                    ViewData["procesosS"] = Datos.ListarProcesos(1, "S");
                    ViewData["procesosT"] = Datos.ListarProcesos(1, "T");
                }
                return View();
                #endregion
            }

        }

        public ActionResult editar_proceso(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }
            procesos proc = Datos.GetDatosProceso(id);
            ViewData["proceso"] = proc;
            ViewData["dependencias"] = Datos.ListarPadres(proc.organizacion, proc.tipo, proc.nivel);

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult editar_proceso(int id, FormCollection collection)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }
            procesos proc = new procesos();
            string formulario = collection["hdFormularioEjecutado"];

            if (formulario == "GuardarProceso" || formulario == "btnDiag")
            {
                #region guardar
                try
                {
                    procesos actualizar = Datos.GetDatosProceso(id);
                    actualizar.cod_proceso = collection["ctl00$MainContent$txtCod"];
                    actualizar.nombre = collection["ctl00$MainContent$txtNombre"];
                    try
                    {
                        if (collection["ctl00$MainContent$txtOrden"] != null)
                            actualizar.orden = int.Parse(collection["ctl00$MainContent$txtOrden"]);
                    }
                    catch
                    {

                    }
                    if (collection["ctl00$MainContent$ddlDependencia"] != null && collection["ctl00$MainContent$ddlDependencia"] != "0")
                    {
                        actualizar.padre = int.Parse(collection["ctl00$MainContent$ddlDependencia"]);
                    }

                    Datos.ActualizarProceso(actualizar);
                    Session["EdicionProcesoMensaje"] = "Información actualizada correctamente";
                }
                catch (Exception ex)
                {
                    proc = Datos.GetDatosProceso(id);
                    ViewData["proceso"] = proc;
                    ViewData["dependencias"] = Datos.ListarPadres(proc.organizacion, proc.tipo, proc.nivel);
                    return View();
                }
                #endregion
            }
            proc = Datos.GetDatosProceso(id);
            ViewData["proceso"] = proc;
            ViewData["dependencias"] = Datos.ListarPadres(proc.organizacion, proc.tipo, proc.nivel);
            return View();
        }

        public ActionResult detalle_proceso(int id)
        {
            if (Session["usuario"] == null || Session["CentralElegida"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            procesos proc = Datos.GetDatosProceso(id);
            ViewData["proceso"] = proc;
            int idCentral = int.Parse(Session["CentralElegida"].ToString());
            ViewData["documentos"] = Datos.ListarDocumentosProceso(id, idCentral);
            ViewData["tecnologias"] = Datos.ListarTecnologias();

            #region generacion fichero
            if (Session["nombreArchivo"] != null && Session["nombreArchivo"].ToString() != "")
            {
                if (Session["nombreArchivo"].ToString().Contains("docx"))
                {
                    Response.ContentType = "application/msword";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=FichaProceso.docx");
                    Response.TransmitFile(Session["nombreArchivo"].ToString());
                    Response.End();
                }
                Session["nombreArchivo"] = "";
            }
            #endregion
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult detalle_proceso(int id, FormCollection collection, HttpPostedFileBase file)
        {
            if (Session["usuario"] == null || Session["CentralElegida"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }
            procesos proce = new procesos();
            string formulario = collection["hdFormularioEjecutado"];

            if (formulario == "GuardarProceso")
            {
                #region guardar proceso
                try
                {
                    procesos actualizar = Datos.GetDatosProceso(id);
                    actualizar.cod_proceso = collection["ctl00$MainContent$txtCod"];
                    actualizar.nombre = collection["ctl00$MainContent$txtNombre"];
                    actualizar.descripcion = collection["ctl00$MainContent$txtDescripcion"];
                    actualizar.objetivos = collection["ctl00$MainContent$txtEntradas"];
                    actualizar.alcance = collection["ctl00$MainContent$txtSalidas"];
                    if (collection["ctl00$MainContent$ddlTecnologia"] != null && collection["ctl00$MainContent$ddlTecnologia"] != "0")
                        actualizar.tecnologia = int.Parse(collection["ctl00$MainContent$ddlTecnologia"].ToString());

                    if (actualizar.cod_proceso != null && actualizar.cod_proceso != string.Empty && actualizar.nombre != null && actualizar.nombre != string.Empty)
                    {
                        Datos.ActualizarProceso(actualizar);
                        Session["EdicionProcesoMensaje"] = "Información actualizada correctamente";
                    }
                    else
                    {
                        Session["EdicionProcesoError"] = "Debe indicar un código y nombre para el proceso.";
                        ViewData["proceso"] = actualizar;
                        int idCentral = int.Parse(Session["CentralElegida"].ToString());
                        ViewData["documentos"] = Datos.ListarDocumentosProceso(id, idCentral);
                        ViewData["tecnologias"] = Datos.ListarTecnologias();

                        return View();
                    }
                }
                catch (Exception ex)
                {
                    procesos proc = Datos.GetDatosProceso(id);
                    ViewData["proceso"] = proc;
                    int idCentral = int.Parse(Session["CentralElegida"].ToString());
                    ViewData["documentos"] = Datos.ListarDocumentosProceso(id, idCentral);
                    ViewData["tecnologias"] = Datos.ListarTecnologias();
                    return View();
                }
                #endregion
            }

            if (formulario == "btnImprimir")
            {
                #region imprimir ficha
                string sourceFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaPlantillaWord"] + "FichaProcesos.docx");
                string destinationFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaGeneracionWord"] + "FichaProcesos_" + DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + ".docx");

                Session["source"] = sourceFile;
                Session["destino"] = destinationFile;
                // Create a copy of the template file and open the copy 
                System.IO.File.Copy(sourceFile, destinationFile, true);

                procesos actualizar = Datos.GetDatosProceso(id);

                if (collection["ctl00$MainContent$txtNombre"] != null)
                {
                    actualizar.nombre = collection["ctl00$MainContent$txtNombre"];
                    actualizar.descripcion = collection["ctl00$MainContent$txtDescripcion"];
                    actualizar.objetivos = collection["ctl00$MainContent$txtEntradas"];
                    actualizar.alcance = collection["ctl00$MainContent$txtSalidas"];
                    actualizar.cod_proceso = collection["ctl00$MainContent$txtCod"];
                    if (collection["ctl00$MainContent$ddlPerfil"] != null && collection["ctl00$MainContent$ddlPerfil"] != "0")
                        actualizar.tecnologia = int.Parse(collection["ctl00$MainContent$ddlPerfil"].ToString());
                }

                WordprocessingDocument doc = WordprocessingDocument.Open(destinationFile, true);
                DocumentFormat.OpenXml.Wordprocessing.Table tablacabecera = doc.MainDocumentPart.Document.Body.Descendants<DocumentFormat.OpenXml.Wordprocessing.Table>().First();
                DocumentFormat.OpenXml.Wordprocessing.TableRow filacabecera = tablacabecera.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>().First();
                DocumentFormat.OpenXml.Wordprocessing.TableCell celdacabecera = filacabecera.Elements<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(1);
                DocumentFormat.OpenXml.Wordprocessing.Paragraph parrafonombrecabecera = celdacabecera.Elements<DocumentFormat.OpenXml.Wordprocessing.Paragraph>().ElementAt(1);
                DocumentFormat.OpenXml.Wordprocessing.Run runNombre = parrafonombrecabecera.Elements<DocumentFormat.OpenXml.Wordprocessing.Run>().First();
                DocumentFormat.OpenXml.Wordprocessing.Text textNombre = runNombre.Elements<DocumentFormat.OpenXml.Wordprocessing.Text>().First();
                runNombre.Append(new DocumentFormat.OpenXml.Wordprocessing.Text(actualizar.nombre));
                runNombre.Elements<DocumentFormat.OpenXml.Wordprocessing.Text>().First().Remove();

                DocumentFormat.OpenXml.Wordprocessing.TableCell celdacabeceracodigo = filacabecera.Elements<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(2);
                DocumentFormat.OpenXml.Wordprocessing.Paragraph parrafocodigocabecera = celdacabeceracodigo.Elements<DocumentFormat.OpenXml.Wordprocessing.Paragraph>().ElementAt(1);
                DocumentFormat.OpenXml.Wordprocessing.Run runCodigo = parrafocodigocabecera.Elements<DocumentFormat.OpenXml.Wordprocessing.Run>().First();
                DocumentFormat.OpenXml.Wordprocessing.Text textCodigo = runCodigo.Elements<DocumentFormat.OpenXml.Wordprocessing.Text>().First();

                runCodigo.Append(new DocumentFormat.OpenXml.Wordprocessing.Text(actualizar.cod_proceso));
                runCodigo.Elements<DocumentFormat.OpenXml.Wordprocessing.Text>().First().Remove();

                doc.MainDocumentPart.Document.Save();

                doc.Close();

                // create key value pair, key represents words to be replace and 
                //values represent values in document in place of keys.
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                if (actualizar.descripcion != null)
                    keyValues.Add("T_Objeto", actualizar.descripcion.Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_Objeto", "");

                if (actualizar.objetivos != null)
                    keyValues.Add("T_Entradas", actualizar.objetivos.Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_Entradas", "");

                if (actualizar.alcance != null)
                    keyValues.Add("T_Salidas", actualizar.alcance.Replace("\r\n", "<w:br/>"));
                else
                    keyValues.Add("T_Salidas", "");

                string tipo = string.Empty;
                switch (actualizar.tipo)
                {
                    case "E":
                        tipo = "Estratégico";
                        break;
                    case "O":
                        tipo = "Operativo";
                        break;
                    case "S":
                        tipo = "Soporte";
                        break;
                }
                keyValues.Add("T_TProceso", tipo.Replace("\r\n", "<w:br/>"));

                int centralElegida = int.Parse(Session["CentralElegida"].ToString());
                List<VISTA_ObtenerDocumentacion> listadoDocumentacion = Datos.ListarDocumentosProceso(id, centralElegida);

                if (listadoDocumentacion.Count > 0)
                {
                    string cadenadocumentos = string.Empty;
                    foreach (VISTA_ObtenerDocumentacion docu in listadoDocumentacion)
                    {
                        cadenadocumentos = cadenadocumentos + docu.titulo + "<w:br/>";
                    }
                    keyValues.Add("T_Documentacion", cadenadocumentos);
                }
                else
                    keyValues.Add("T_Documentacion", string.Empty);


                SearchAndReplace(destinationFile, keyValues);


                Session["nombreArchivo"] = destinationFile;

                procesos proc = Datos.GetDatosProceso(id);
                ViewData["proceso"] = proc;
                int idCentral = int.Parse(Session["CentralElegida"].ToString());
                ViewData["documentos"] = Datos.ListarDocumentosProceso(id, idCentral);
                ViewData["tecnologias"] = Datos.ListarTecnologias();
                return RedirectToAction("detalle_proceso/" + id, "Procesos");
                #endregion
            }

            #region recarga
            procesos proceso = Datos.GetDatosProceso(id);
            ViewData["proceso"] = proceso;
            int idCentro = int.Parse(Session["CentralElegida"].ToString());
            ViewData["documentos"] = Datos.ListarDocumentosProceso(id, idCentro);
            ViewData["tecnologias"] = Datos.ListarTecnologias();
            #endregion

            return Redirect(Url.RouteUrl(new { controller = "Procesos", action = "detalle_proceso", id = id }));
        }

        public ActionResult Eliminar_Proceso(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            int idProceso = Datos.EliminarProceso(id);
            Session["EditarProcesoResultado"] = "ELIMINADOPROCESO";
            return RedirectToAction("gestion_procesos", "Procesos");
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
