using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

using MIDAS.Helpers;
using MIDAS.Models;
using MIDAS.Validations.Configuracion;

using OfficeOpenXml;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

using static MIDAS.Models.Enum;

namespace MIDAS.Controllers
{
    public class ConfiguracionController : BaseController
    {
        #region menu


        public ActionResult menu()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            //if (Session["CentralElegida"] != null)
            //{
            //    centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));

            //}
            //else
            //{

            //    return RedirectToAction("LogOn", "Account");
            //}

            return View();
        }

        #endregion

        public ActionResult centros()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            List<provincia> listaprovincias = Datos.ListarProvincias();
            List<comunidad_autonoma> comunidades_autonomas = Datos.ListarComunidades();
            List<centros> centros = Datos.ListarTodosCentros();
            centros.Reverse();
            List<zonas> listaZonas = Datos.ListarZonas();
            List<agrupacion> listaAgrupaciones = Datos.ListarAgrupaciones();
            List<VISTA_ListaCentroZona> listaCentroZona = Datos.VistaListarCentrosZonas();
            List<VISTA_ListaCentroZonaAgrupacion> listaCentroZonaAgrupacion = Datos.VISTAListaCentroZonaAgrupacion();
            //List<tecnologias> tecCentros = Datos.ListarTecnologiasPorCentro();

            ViewData["comunidades"] = comunidades_autonomas;
            ViewData["centrales"] = centros;
            ViewData["provincias"] = listaprovincias;
            ViewData["listaZonas"] = listaZonas;
            ViewData["listaAgrupaciones"] = listaAgrupaciones;
            ViewData["listaCentrosZonas"] = listaCentroZona;
            ViewData["listaCentrosZonasAgrupacion"] = listaCentroZonaAgrupacion;
            //ViewData["tecCentros"] = tecCentros;
            //ViewData["centrales"] = Datos.ListarCentros();
            return View();
        }

        [HttpGet]
        public ActionResult personas()
        {
            //if (Session["usuario"] == null)
            //{
            //    return RedirectToAction("LogOn", "Account");
            //}
            List<personas> listarPersonas = Datos.ListarPersonas();

            ViewData["personas"] = listarPersonas;

            return View();
        }

        [HttpPost]
        public ActionResult personas(HttpPostedFileBase fileExcel)
        {

            if (fileExcel == null || fileExcel.ContentLength == 0)
            {
                ModelState.AddModelError("fileExcel", "Se requiere un archivo de Excel válido");
                return RedirectToAction("personas", "Configuracion");
            }

            if (!Path.GetExtension(fileExcel.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError("fileExcel", "El formato de archivo debe ser .xlsx");
                return RedirectToAction("personas", "Configuracion");
            }
            else
            {
                List<personas> listaPersonas = new List<personas>();

                using (var package = new ExcelPackage(fileExcel.InputStream))
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                    var currentSheet = package.Workbook.Worksheets;
                    var workSheet = currentSheet.First();
                    var noOfCol = workSheet.Dimension.End.Column;
                    var noOfRow = workSheet.Dimension.End.Row;
                    for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                    {
                        var persona = new personas();

                        if (workSheet.Cells[rowIterator, 1].Value.ToString() != null)
                        {
                            persona.Nº_Empleado = workSheet.Cells[rowIterator, 1].Value.ToString();
                            persona.Perfil_de_riesgo = workSheet.Cells[rowIterator, 2].Value != null ? workSheet.Cells[rowIterator, 2].Value.ToString() : "";
                            persona.Actividad_Funcional = workSheet.Cells[rowIterator, 3].Value != null ? workSheet.Cells[rowIterator, 3].Value.ToString() : "";
                            persona.DNI = workSheet.Cells[rowIterator, 4].Value != null ? workSheet.Cells[rowIterator, 4].Value.ToString() : "";
                            persona.Nombre = workSheet.Cells[rowIterator, 5].Value != null ? workSheet.Cells[rowIterator, 5].Value.ToString() : "";
                            persona.Empresa = workSheet.Cells[rowIterator, 6].Value != null ? workSheet.Cells[rowIterator, 6].Value.ToString() : "";
                            persona.Centro_de_trabajo = workSheet.Cells[rowIterator, 7].Value != null ? workSheet.Cells[rowIterator, 7].Value.ToString() : "";
                            persona.Actividad = workSheet.Cells[rowIterator, 8].Value != null ? workSheet.Cells[rowIterator, 8].Value.ToString() : "";
                            persona.Unidad_Organizativa = workSheet.Cells[rowIterator, 9].Value != null ? workSheet.Cells[rowIterator, 9].Value.ToString() : "";
                            persona.Jefe_Directo = workSheet.Cells[rowIterator, 10].Value != null ? workSheet.Cells[rowIterator, 10].Value.ToString() : "";
                            persona.Posicion = workSheet.Cells[rowIterator, 11].Value != null ? workSheet.Cells[rowIterator, 11].Value.ToString() : "";
                            persona.Ocupacion = workSheet.Cells[rowIterator, 12].Value != null ? workSheet.Cells[rowIterator, 12].Value.ToString() : "";
                            persona.FechaRegistro = DateTime.Now;
                            persona.Activo = true;
                            listaPersonas.Add(persona);
                        }
                        else
                        {
                            break;
                        }
                    }

                    Datos.GuardarListaPersonas(listaPersonas);
                }
            }

            return RedirectToAction("personas", "Configuracion");
        }

        public ActionResult medidas_generales()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }
            List<medidas_generales> listarMedidas = Datos.ListarMedidasGenerales();

            listarMedidas.Reverse();

            //ViewData["medidas"] = Datos.ListarMedidasGenerales();
            ViewData["medidas"] = listarMedidas;

            //ViewData["centrales"] = Datos.ListarCentros();
            return View();
        }
        public ActionResult riesgos_medidas()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            List<riesgos_medidas> medidas = Datos.ListarMedidasRiesgo().ToList();
            medidas.Reverse();
            ViewData["medidas"] = medidas;

            return View();
        }

        public ActionResult eliminar_riesgo_medida(int id)
        {

            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            //preguntamos si esta medida tiene icono o imagen, eso lo vemos en riesgos_medidas[imagen_grande]
            //si es null, es solo una medida
            var oMedida = Datos.ObtenerRiesgoMedidaporId(id);

            if (oMedida.imagen_grande == null)
            {
                Datos.EliminarRiesgoMedida(id);
            }
            else
            {
                //var imagen = oMedida.imagen;
                //imagen = imagen.Replace("..", "");
                //string rutaServer = Server.MapPath("~/");
                //string rutaImagen = Path.Combine(rutaServer + imagen);
                ////borramos el archivo
                //if ((System.IO.File.Exists(rutaImagen)))
                //{
                //System.IO.File.Delete(rutaImagen);
                Datos.EliminarRiesgoMedida(id);
                //}
            }
            Session["EditarCentralesResultado"] = "Medida asociada a riesgo eliminada";
            return RedirectToAction("riesgos_medidas", "Configuracion");
        }

        public ActionResult eliminar_riesgo_medida_rutaYFichero(int id)
        {

            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            //preguntamos si esta medida tiene icono o imagen, eso lo vemos en riesgos_medidas[imagen_grande]
            //si es null, es solo una medida
            var oMedida = Datos.ObtenerRiesgoMedidaporId(id);

            if (oMedida.imagen != null)
            {
                //Datos.EliminarRiesgoMedida(id);
                var imagen = oMedida.imagen;
                if (imagen != null)
                {
                    imagen = imagen.Replace("..", "");
                    string rutaServer = Server.MapPath("~/");
                    string rutaImagen = Path.Combine(rutaServer + imagen);
                    //borramos el archivo
                    if ((System.IO.File.Exists(rutaImagen)))
                    {
                        System.IO.File.Delete(rutaImagen);
                        oMedida.imagen = null;
                        Datos.ActualizarMedidaGeneralRiesgo(oMedida);
                    }


                }
            }

            //Session["EditarCentralesResultado"] = "Medida asociada a riesgo eliminada";
            return RedirectToAction("riesgos_medidas", "Configuracion");
        }


        public ActionResult medidas_preventivas()
        {
            log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(Server.MapPath("~/log4net.config")));

            try
            {
                if (Session["usuario"] == null)
                {
                    return RedirectToAction("LogOn", "Account");
                }

                List<medidas_preventivas> medidas = Datos.ListarMedidas();
                medidas.Reverse();
                ViewData["medidas"] = medidas;

                ViewData["SubMedidas"] = Datos.ListarSubMedidas();

                //ViewData["centrales"] = Datos.ListarCentros();
                return View();
            }

            catch (Exception ex)
            {
                new EscribirLog("Error: " +
                            ex.Message, true, this.ToString(), "medidas_preventivas");
                return RedirectToAction("menu", "Configuracion");
            }

        }



        public ActionResult editar_equipo(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }
            int idSistema = -1;
            if (Session["idSistema"] != null)
            {
                idSistema = (int)Session["idSistema"];
            }


            areanivel3 buscarEquipo = Datos.ObtenerEquipoPorID(id);
            areanivel2 buscarSistema = Datos.ObtenerSistemaPorID(idSistema);
            areanivel1 buscarArea = Datos.ObtenerAreaPorID(buscarSistema.id_areanivel1);
            centros buscarCentro = Datos.ObtenerCentroPorID(buscarArea.id_centro);


            ViewData["idCentral"] = buscarCentro.id;
            ViewData["idArea"] = buscarArea.id;
            ViewData["idSistema"] = buscarSistema.id;
            ViewData["EditarCentro"] = buscarCentro;
            ViewData["EditarArea"] = buscarArea;
            ViewData["EditarSistema"] = buscarSistema;
            ViewData["EditarEquipo"] = buscarEquipo;

            ViewData["sistema"] = Datos.ListarSistema();
            ViewData["ubicaciones"] = Datos.ListarProvincias();
            ViewData["unidades"] = Datos.ListarTecnologias();

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult editar_equipo(int id, FormCollection collection)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }
            int idRef = 0;
            ViewData["idReferencial"] = id;

            int idSistema = -1;
            if (Session["idSistema"] != null)
            {
                idSistema = (int)Session["idSistema"];
            }
            try
            {
                #region añadir referencial

                areanivel3 actualizar = new areanivel3();
                actualizar.id = id;

                if (!string.IsNullOrEmpty(collection["ctl00$MainContent$equiponombre"]) && !string.IsNullOrEmpty(collection["ctl00$MainContent$equipocodigo"]))
                {

                    actualizar.nombre = collection["ctl00$MainContent$equiponombre"];
                    actualizar.codigo = collection["ctl00$MainContent$equipocodigo"];
                    actualizar.id_areanivel2 = idSistema;

                    areanivel3 buscarEquipo = new areanivel3();

                    if (actualizar.codigo == string.Empty || actualizar.nombre == string.Empty)
                    {

                        Session["EdicionEquipoError"] = "Faltan datos";
                        Session["EditarEquiposResultado"] = "Faltan datos";
                        return View();
                    }

                    idRef = Datos.ActualizarEquipo(actualizar);

                    buscarEquipo = Datos.ObtenerEquipoPorID(idRef);
                    ViewData["EditarEquipo"] = buscarEquipo;
                    Session["EditarEquiposResultado"] = "Equipo añadido";
                    //Session["EdicionEquipoMensaje"] = "Los datos han sido modificados correctamente";
                    return RedirectToAction("editar_sistema/" + idSistema, "Configuracion");
                }
                else
                {
                    areanivel3 buscarSistema = Datos.ObtenerEquipoPorID(id);
                    ViewData["EditarSistema"] = buscarSistema;
                    return View();
                }
                #endregion

            }
            catch (Exception ex)
            {
                #region exception
                Session["EdicionSistemaError"] = "FALLA" + ";" + ex.Message;
                areanivel3 buscarEquipo = Datos.ObtenerEquipoPorID(id);
                ViewData["EditarEquipo"] = buscarEquipo;
                return RedirectToAction("editar_sistema/" + idRef, "Configuracion");
                #endregion
            }
        }
        public ActionResult editar_sistema(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }
            Session["idSistema"] = id;
            ViewData["equipos"] = Datos.ListarEquipos(id);
            areanivel2 buscarSistema = Datos.ObtenerSistemaPorID(id);
            areanivel1 buscarArea = Datos.ObtenerAreaPorID(buscarSistema.id_areanivel1);
            centros buscarCentro = Datos.ObtenerCentroPorID(buscarArea.id_centro);
            ViewData["idCentral"] = buscarCentro.id;
            ViewData["idArea"] = buscarArea.id;
            ViewData["idSistema"] = buscarSistema.id;
            ViewData["EditarCentro"] = buscarCentro;
            ViewData["EditarArea"] = buscarArea;
            ViewData["EditarSistema"] = buscarSistema;
            ViewData["sistema"] = Datos.ListarSistema();
            ViewData["ubicaciones"] = Datos.ListarProvincias();
            ViewData["unidades"] = Datos.ListarTecnologias();

            return View();
        }
        [HttpPost]
        public ActionResult editar_sistema(int id, FormCollection collection, string submit)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }
            int idRef = 0;
            ViewData["idReferencial"] = id;

            if (submit == "Añadir Equipo")
            {
                if (!string.IsNullOrEmpty(collection["ctl00$MainContent$equiponombre"]) && !string.IsNullOrEmpty(collection["ctl00$MainContent$equipocodigo"]))
                {
                    return RedirectToAction("guardar_equipo", "Configuracion", new { id = id, nombre = collection["ctl00$MainContent$equiponombre"], codigo = collection["ctl00$MainContent$equipocodigo"] });
                }
            }

            try
            {
                #region añadir referencial

                areanivel2 actualizar = new areanivel2();
                actualizar.id = 0;
                if (!string.IsNullOrEmpty(collection["ctl00$MainContent$sistemacodigo"]) && !string.IsNullOrEmpty(collection["ctl00$MainContent$sistemanombre"]))
                {

                    actualizar.nombre = collection["ctl00$MainContent$sistemanombre"];
                    actualizar.codigo = collection["ctl00$MainContent$sistemacodigo"];
                    actualizar.id = id;
                    areanivel2 buscarEquipo = new areanivel2();
                    idRef = Datos.ActualizarSistema(actualizar);
                    // buscarEquipo = Datos.ObtenerEquipoPorID(idRef);
                    ViewData["EditarEquipo"] = buscarEquipo;

                    Session["EditarSistemaResultado"] = "Sistema añadido correctamente";
                    Session["EdicionSistemaMensaje"] = "Los datos han sido modificados correctamente";
                    return RedirectToAction("editar_sistema/" + id, "Configuracion");
                }
                else
                {
                    string cadenaError = string.Empty;

                    if (string.IsNullOrEmpty(collection["ctl00$MainContent$sistemanombre"])) cadenaError += " [NOMBRE] ";
                    if (string.IsNullOrEmpty(collection["ctl00$MainContent$sistemacodigo"])) cadenaError += " [CODIGO] ";
                    Session["EditarSistemaResultado"] = "Falta por informar : " + cadenaError;

                    return RedirectToAction("editar_sistema/" + id, "Configuracion");
                }
                #endregion

            }
            catch (Exception ex)
            {
                #region exception
                Session["EditarSistemaResultado"] = "FALLA" + ";" + ex.Message;
                areanivel3 buscarEquipo = Datos.ObtenerEquipoPorID(id);
                ViewData["EditarSistemaResultado"] = buscarEquipo;
                return RedirectToAction("editar_sistema/" + idRef, "Configuracion");
                #endregion
            }
        }

        public ActionResult guardar_equipo(int id, string codigo, string nombre)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }
            int idRef = 0;
            ViewData["idReferencial"] = id;

            try
            {
                #region añadir referencial
                areanivel3 actualizar = new areanivel3();
                actualizar.id = 0;
                if (!string.IsNullOrEmpty(nombre) && !string.IsNullOrEmpty(codigo))
                {

                    actualizar.nombre = nombre;
                    actualizar.codigo = codigo;
                    actualizar.id_areanivel2 = id;

                    idRef = Datos.ActualizarEquipo(actualizar);


                    Session["EditarEquiposResultado"] = "Equipo añadido correctamente";
                    Session["EdicionEquipoMensaje"] = "Los datos han sido modificados correctamente";
                    return RedirectToAction("editar_sistema/" + id, "Configuracion");
                }
                else
                {
                    string cadenaError = string.Empty;

                    if (string.IsNullOrEmpty(nombre)) cadenaError += " [NOMBRE] ";
                    if (string.IsNullOrEmpty(codigo)) cadenaError += " [CODIGO] ";
                    Session["EditarEquiposResultado"] = "Falta por informar : " + cadenaError;

                    return RedirectToAction("editar_sistema/" + id, "Configuracion");
                }

                #endregion

            }
            catch (Exception ex)
            {
                #region exception
                Session["EditarSistemaResultado"] = "FALLA" + ";" + ex.Message;

                return RedirectToAction("editar_sistema/" + idRef, "Configuracion");
                #endregion
            }
        }

        public ActionResult editar_area(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            //ViewData["sistema"] = Datos.ListarSistema();
            ViewData["sistema"] = Datos.ListarSistemaPorIDArea(id);

            areanivel1 buscarArea = Datos.ObtenerAreaPorID(id);
            centros buscarCentro = Datos.ObtenerCentroPorID(buscarArea.id_centro);
            ViewData["idCentral"] = buscarCentro.id;
            ViewData["idArea"] = buscarArea.id;
            ViewData["EditarCentro"] = buscarCentro;
            ViewData["EditarArea"] = buscarArea;
            ViewData["ubicaciones"] = Datos.ListarProvincias();
            ViewData["unidades"] = Datos.ListarTecnologias();

            return View();
        }
        public ActionResult editar_tipos_riesgos(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            //ViewData["sistema"] = Datos.ListarSistema();
            tipos_riesgos riesgo = Datos.DameRiesgo(id);
            if (riesgo != null)
            {
                ViewData["riesgos"] = riesgo;
            }


            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult editar_tipos_riesgos(int id, FormCollection collection, string submit)
        {
            log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(Server.MapPath("~/log4net.config")));

            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }
            try
            {
                //if (collection["ctl00$MainContent$txtDescripcionRiesgo"] != null && collection["ctl00$MainContent$txtNombreRiesgo"] != null)
                if (collection["ctl00$MainContent$txtDescripcionRiesgo"] != null)
                {
                    tipos_riesgos actualizar = new tipos_riesgos();
                    actualizar.id = id;
                    //actualizar.codigo = collection["ctl00$MainContent$txtNombreRiesgo"];
                    actualizar.definicion = collection["ctl00$MainContent$txtDescripcionRiesgo"];
                    Datos.ActualizarRiesgo(actualizar);

                }
                else
                {
                    Alert("No esta permitido modificar el contenido de Tipos de Riesgos ", NotificationType.error, 1000);
                }

                return RedirectToAction("tipos_riesgos", "Configuracion");
            }
            catch (Exception ex)
            {
                new EscribirLog("Error: " +
                            ex.Message, true, this.ToString(), "editar_tipos_riesgos");
                return RedirectToAction("tipos_riesgos", "Configuracion");
            }
        }

        [HttpPost]
        public ActionResult editar_area(int id, FormCollection collection, string submit)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }
            int idRef = 0;
            ViewData["idReferencial"] = id;


            if (submit == "Añadir Sistema")
            {
                if (!string.IsNullOrEmpty(collection["ctl00$MainContent$sistemanombre"]) && !string.IsNullOrEmpty(collection["ctl00$MainContent$sistemacodigo"]))
                {
                    return RedirectToAction("guardar_sistema", "Configuracion", new { id = id, nombre = collection["ctl00$MainContent$sistemanombre"], codigo = collection["ctl00$MainContent$sistemacodigo"] });
                }
            }

            try
            {
                #region añadir referencial
                areanivel1 actualizar = new areanivel1();
                actualizar.id = 0;


                if (!string.IsNullOrEmpty(collection["ctl00$MainContent$CodigoArea"]) && !string.IsNullOrEmpty(collection["ctl00$MainContent$NombreArea"]))
                {

                    actualizar.nombre = collection["ctl00$MainContent$NombreArea"].ToString();
                    actualizar.codigo = collection["ctl00$MainContent$CodigoArea"].ToString();
                    actualizar.id = id;

                    idRef = Datos.ActualizarArea(actualizar);


                    Session["EditarAreaResultado"] = "Area modificada correctamente";
                    Session["EdicionAreaMensaje"] = "Los datos han sido modificados correctamente";
                    return RedirectToAction("editar_area/" + id, "Configuracion");
                }
                else
                {
                    string cadenaError = string.Empty;

                    if (string.IsNullOrEmpty(collection["ctl00$MainContent$NombreArea"].ToString())) cadenaError += " [NOMBRE] ";
                    if (string.IsNullOrEmpty(collection["ctl00$MainContent$CodigoArea"].ToString())) cadenaError += " [CODIGO] ";
                    Session["EditarAreaResultado"] = "Falta por informar : " + cadenaError;

                    return RedirectToAction("editar_area/" + id, "Configuracion");
                }
                #endregion

            }
            catch (Exception ex)
            {
                #region exception
                Session["EditarAreaResultado"] = "FALLA" + ";" + ex.Message;

                return RedirectToAction("editar_area/" + idRef, "Configuracion");
                #endregion
            }
        }

        public ActionResult guardar_sistema(int id, string codigo, string nombre)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }
            int idRef = 0;
            ViewData["idReferencial"] = id;

            try
            {
                #region añadir referencial
                areanivel2 actualizar = new areanivel2();
                actualizar.id = 0;
                if (!string.IsNullOrEmpty(nombre) && !string.IsNullOrEmpty(codigo))
                {

                    actualizar.nombre = nombre;
                    actualizar.codigo = codigo;
                    actualizar.id_areanivel1 = id;

                    idRef = Datos.ActualizarSistema(actualizar);


                    Session["EditarSistemaResultado"] = "Sistema añadido correctamente";
                    Session["EdicionSistemaMensaje"] = "Los datos han sido modificados correctamente";
                    return RedirectToAction("editar_area/" + id, "Configuracion");
                }
                else
                {
                    string cadenaError = string.Empty;

                    if (string.IsNullOrEmpty(nombre)) cadenaError += " [NOMBRE] ";
                    if (string.IsNullOrEmpty(codigo)) cadenaError += " [CODIGO] ";
                    Session["EditarSistemaResultado"] = "Falta por informar : " + cadenaError;

                    return RedirectToAction("editar_area/" + id, "Configuracion");
                }

                #endregion

            }
            catch (Exception ex)
            {
                #region exception
                Session["EditarSistemaResultado"] = "FALLA" + ";" + ex.Message;

                return RedirectToAction("editar_area/" + idRef, "Configuracion");
                #endregion
            }
        }

        // Insercción código - Rafael Ortega - 25/07/2022                                
        public JsonResult ObtenerAgrupacion(string idTipo, string idZona)
        {

            int tipo = 0;
            int zona = 0;
            List<SelectListItem> datos = null;
            try
            {
                tipo = int.Parse(idTipo);
                zona = int.Parse(idZona);
                datos = new List<SelectListItem>();
                SelectListItem inicial = new SelectListItem();
                inicial.Text = "Seleccione Agrupación";
                inicial.Value = "0";
                datos.Add(inicial);
                foreach (agrupacion item in Datos.ListarAgrupacionesPorZonas(zona))
                {
                    SelectListItem it = new SelectListItem();
                    it.Text = item.nombre;
                    it.Value = item.id.ToString();
                    datos.Add(it);
                }

            }
            catch (Exception ex)
            {

            }
            return Json(datos);
        }
        public JsonResult ObtenerZonas(string idTipo)
        {

            int tipo = 0;

            List<SelectListItem> datos = null;
            try
            {
                List<zonas> listado = null;
                tipo = int.Parse(idTipo);
                if (tipo == 7 || tipo == 8 || tipo == 9)
                {
                    listado = Datos.ListarZonas(tipo);
                }
                else
                {
                    listado = Datos.ListarZonas();
                }

                datos = new List<SelectListItem>();
                SelectListItem inicial = new SelectListItem();
                if (tipo == 7)
                {
                    inicial.Text = "Seleccione Zona";
                }
                else
                {
                    inicial.Text = "Seleccione Centro";
                }

                inicial.Value = "0";
                datos.Add(inicial);
                foreach (zonas item in listado)
                {
                    SelectListItem it = new SelectListItem();
                    it.Text = item.nombre;
                    it.Value = item.id.ToString();
                    datos.Add(it);
                }

            }
            catch (Exception ex)
            {

            }
            return Json(datos);
        }
        // Insercción código - Rafael Ortega - 25/07/2022

        public ActionResult editar_centro(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            ViewData["idCentral"] = id;

            ViewData["areas"] = Datos.ListarAreas();

            centros buscarCentro = Datos.ObtenerCentroPorID(id);
            ViewData["EditarCentro"] = buscarCentro;
            ViewData["ubicaciones"] = Datos.ListarProvincias();
            ViewData["unidades"] = Datos.ListarTecnologias();
            ViewData["VISTAListaCentroTecnologia"] = Datos.VISTAListaCentroTecnologia();
            ViewData["VistaListarCentrosZonas"] = Datos.VistaListarCentrosZonas();
            ViewData["VISTAListaCentroZonaAgrupacion"] = Datos.VISTAListaCentroZonaAgrupacion();

            List<agrupacion> listaAgrupacion = new List<agrupacion>();
            agrupacion agrupacionInicial = new agrupacion();
            agrupacionInicial.id = 0;
            agrupacionInicial.nombre = "Seleccione Agrupación";
            listaAgrupacion.Add(agrupacionInicial);
            listaAgrupacion.AddRange(Datos.ListarAgrupaciones());
            ViewData["agrupaciones"] = listaAgrupacion;

            List<zonas> listaZonas = new List<zonas>();
            zonas zonaInicial = new zonas();
            zonaInicial.id = 0;
            zonaInicial.nombre = "Seleccione Zona";
            listaZonas.Add(zonaInicial);
            listaZonas.AddRange(Datos.ListarZonas());
            ViewData["zonas"] = listaZonas;

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult editar_medida_general(int id,
                                                    FormCollection collection,
                                                    HttpPostedFileBase seleccionArchivoIcono,
                                                    HttpPostedFileBase seleccionArchivoGrande)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }
            try
            {
                string apartados = collection["ctl00$MainContent$ddlApartados"];
                string descripcion = collection["ctl00$MainContent$txtNombre"];
                string nombreIcono = collection["ctl00$MainContent$nombreIcono"];
                
                //Crear
                if (id == 0)
                {
                    medidas_generales actualizar = new medidas_generales();
                    descripcion = collection["ctl00$MainContent$txtNombre"];
                    descripcion = descripcion.Replace("•\t", "");
                    descripcion = descripcion.Replace("\n", "[SALTO]");
                    descripcion = descripcion.Replace("-", "");
                    actualizar.descripcion = descripcion;
                    actualizar.id_apartado_generales = int.Parse(collection["ctl00$MainContent$ddlApartados"]);
                    actualizar.id_centro = 0;

                    #region validacion del form y guardar imagen en crear
                    Validation_editar_medida_general validar = new Validation_editar_medida_general();
                    List<string> listaE = validar.validaFormInsertar(collection, seleccionArchivoIcono);

                    if (listaE.Count > 0)
                    {
                        Session["ErrorForm"] = "Error al completar el formulario: " + listaE[0];

                        var apartado = int.Parse(collection["ctl00$MainContent$ddlApartados"]);
                        ViewData["apartado"] = apartado;
                        ViewData["descripcion"] = collection["ctl00$MainContent$txtNombre"];

                        //return RedirectToAction("editar_riesgo_medida", "Configuracion");
                        return View();
                    }

                    int idMedidaGeneral = Datos.ActualizarMedidaGeneral(actualizar);

                    //guardamos la ruta de la imagen mediante la codificacion
                    if (!string.IsNullOrEmpty(nombreIcono))
                    {
                        medidas_generales_imagenes medidas_Generales_Imagenes = new medidas_generales_imagenes()
                        {
                            id = 0,
                            id_medida_general = idMedidaGeneral,
                            rutaImagen = "../Content/images/banco_icono/" + nombreIcono + ".png",
                            tamano = false,
                        };

                        int id_guardado = Datos.ActualizarMedidasGeneralesImagenesPorObjeto(medidas_Generales_Imagenes, idMedidaGeneral);
                    }

                    Alert("Medida General guardada", NotificationType.success, 2000);
                    return RedirectToAction("medidas_generales", "Configuracion");

                    #endregion
                }

                //actualizar 
                else
                {
                    medidas_generales actualizar = new medidas_generales();
                    actualizar.id = id;
                    descripcion = collection["ctl00$MainContent$txtNombre"];
                    descripcion = descripcion.Replace("•\t", "");
                    descripcion = descripcion.Replace("\n", "[SALTO]");
                    descripcion = descripcion.Replace("-", "");
                    actualizar.descripcion = descripcion;
                    actualizar.id_apartado_generales = int.Parse(collection["ctl00$MainContent$ddlApartados"]);

                    actualizar.id_centro = 0;
                    //actualizar.id_tecnologia = int.Parse(Session["TecnologiaElegida"].ToString());

                    #region validacion del form y guardar imagen en actualizar
                    Validation_editar_medida_general validar = new Validation_editar_medida_general();
                    List<string> listaE = validar.validaFormActualizar(collection, seleccionArchivoIcono);

                    if (listaE.Count > 0)
                    {
                        Session["ErrorForm"] = "Error al completar el formulario: " + listaE[0];

                        var apartado = int.Parse(collection["ctl00$MainContent$ddlApartados"]);
                        ViewData["apartado"] = apartado;
                        ViewData["descripcion"] = collection["ctl00$MainContent$txtNombre"];

                        //return RedirectToAction("editar_riesgo_medida", "Configuracion");
                        return View();
                    }
                   
                    int idMedidaGeneral = Datos.ActualizarMedidaGeneral(actualizar);

                    //guardamos la ruta de la imagen mediante la codificacion
                    if (!string.IsNullOrEmpty(nombreIcono))
                    {
                        medidas_generales_imagenes medidas_Generales_Imagenes = new medidas_generales_imagenes()
                        {
                            id_medida_general = idMedidaGeneral,
                            rutaImagen = "../Content/images/banco_icono/" + nombreIcono + ".png",
                            tamano = false,
                        };

                        int id_guardado = Datos.ActualizarMedidasGeneralesImagenesPorObjeto(medidas_Generales_Imagenes, idMedidaGeneral);
                    }
                }
                #endregion
                return RedirectToAction("medidas_generales", "Configuracion");

            }

            catch (Exception ex)
            {
                return RedirectToAction("medidas_generales", "Configuracion");
            }
        }
        [HttpGet]
        public ActionResult editar_medida_general(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            ViewData["idCentral"] = id;
            if (id != 0)
            {
                medidas_generales medida = Datos.ObtenerMedidaporId(id);
                ViewData["EditarMedida"] = medida;
                ViewData["Apartado"] = medida.id_apartado_generales;
                ViewData["EditarImagen"] = Datos.obtenerImagenMedidasGeneralesObjeto(medida.id);

            }

            ViewData["apartados"] = Datos.ListarApartadosGenerales().OrderBy(x => x.id);
            ViewData["medidasImagenes"] = Datos.ListarMedidasGeneralesImagenes();
            ViewData["imagenesGenerales"] = Datos.ListarMedidasGeneralesImagenes();
            ViewData["banco_icono"] = Datos.ListarBancoIcono();


            return View();
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult editar_riesgo_medida(int id, FormCollection collection, string submit,
                                                    HttpPostedFileBase seleccionArchivoIC, HttpPostedFileBase seleccionArchivoIG)
        {

            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }
            try
            {

                riesgos_medidas actualizar = new riesgos_medidas();
                actualizar.id = id;

                ViewData["apartados"] = Datos.ListarApartadosV2().OrderBy(x => x.id);
                ViewData["riesgos"] = Datos.ListarRiesgos();
                //crear
                if (id == 0)
                {
                    Validation_editar_riesgo_medida validar = new Validation_editar_riesgo_medida();
                    List<string> listaE = validar.validaFormInsertar(collection, seleccionArchivoIC, seleccionArchivoIG);

                    if (listaE.Count > 0)
                    {
                        Session["ErrorForm"] = "Error al completar el formulario: " + listaE[0];

                        var apartado = int.Parse(collection["ctl00$MainContent$ddlApartados"]);

                        ViewData["apartado"] = apartado;
                        ViewData["riesgo"] = int.Parse(collection["ctl00$MainContent$ddlRiesgos"]);
                        ViewData["descripcion"] = collection["ctl00$MainContent$txtNombre"];

                        return View();
                    }
                    string descripcion = collection["ctl00$MainContent$txtNombre"];
                    descripcion = descripcion.Replace("•\t", "");
                    descripcion = descripcion.Replace("\n", "[SALTO]");
                    descripcion = descripcion.Replace("-", "");
                    actualizar.descripcion = descripcion;
                    //actualizar.otroapartado = collection["ctl00$MainContent$apartadoOtro"];
                    actualizar.id_apartado = int.Parse(collection["ctl00$MainContent$ddlApartados"]);
                    actualizar.id_centro = 0;
                    //actualizar.id_tecnologia = int.Parse(Session["TecnologiaElegida"].ToString());
                    actualizar.id_tecnologia = null;
                    actualizar.id_riesgo = int.Parse(collection["ctl00$MainContent$ddlRiesgos"]);

                    #region validacion del form y guardar imagen en crear

                    var archivoIC = seleccionArchivoIC;
                    var archivoIG = seleccionArchivoIG;
                    //string rutaSitio = Server.MapPath("~/");
                    //var rutaImagenBD = "../Content/images/medidas/";
                    //var rutaImagenReal = "/Content/images/medidas/";

                    //string rutaImagenFinal = Path.Combine(rutaSitio + rutaImagenReal);
                    int idMedidaRiesgo = 0;

                    if (archivoIC != null)
                    {
                        idMedidaRiesgo = Datos.ActualizarMedidaGeneralRiesgoV2(actualizar, archivoIC.FileName, "on", null, archivoIC, null);
                    }
                    else if (archivoIG != null)
                    {
                        idMedidaRiesgo = Datos.ActualizarMedidaGeneralRiesgoV2(actualizar, archivoIG.FileName, null, "on", null, archivoIG);
                    }
                    else
                    {
                        idMedidaRiesgo = Datos.ActualizarMedidaGeneralRiesgoV2(actualizar, null, null, null, null, null);
                    }
                }

                //actualizar
                else
                {

                    string descripcion = collection["ctl00$MainContent$txtNombre"];
                    // string otroapartado = descripcion = collection["ctl00$MainContent$apartadoOtro"];
                    descripcion = descripcion.Replace("•\t", "");
                    descripcion = descripcion.Replace("\n", "[SALTO]");
                    descripcion = descripcion.Replace("-", "");
                    actualizar.descripcion = descripcion;
                    actualizar.id_apartado = int.Parse(collection["ctl00$MainContent$ddlApartados"]);
                    actualizar.id_centro = 0;
                    //actualizar.id_tecnologia = int.Parse(Session["TecnologiaElegida"].ToString());
                    actualizar.id_tecnologia = null;
                    actualizar.id_riesgo = int.Parse(collection["ctl00$MainContent$ddlRiesgos"]);

                    var riesgosMedida = Datos.ObtenerRiesgoMedidaporId(id);

                    #region validacion del form y guardar imagen en actualizar

                    Validation_editar_riesgo_medida validar = new Validation_editar_riesgo_medida();
                    List<string> listaE = validar.validaFormActualizar(collection, seleccionArchivoIC, seleccionArchivoIG);

                    if (listaE.Count > 0)
                    {
                        Session["ErrorForm"] = "Error al completar el formulario: " + listaE[0];
                        return RedirectToAction("editar_riesgo_medida", "Configuracion");
                    }
                    else
                    {
                        var archivoIC = seleccionArchivoIC;
                        var archivoIG = seleccionArchivoIG;
                        //string rutaSitio = Server.MapPath("~/");
                        //var rutaImagenBD = "../Content/images/medidas/";
                        //var rutaImagenReal = "/Content/images/medidas/";

                        //string rutaImagenFinal = Path.Combine(rutaSitio + rutaImagenReal);
                        int idMedidaRiesgo = 0;

                        if (archivoIC != null)
                        {
                            idMedidaRiesgo = Datos.ActualizarMedidaGeneralRiesgoV2(actualizar, archivoIC.FileName, "on", null, archivoIC, null);
                        }
                        else if (archivoIG != null)
                        {
                            idMedidaRiesgo = Datos.ActualizarMedidaGeneralRiesgoV2(actualizar, archivoIG.FileName, null, "on", null, archivoIG);
                        }
                        else
                        {
                            if (riesgosMedida.imagen != null && riesgosMedida.imagen != "")
                            {
                                if (riesgosMedida.imagen_grande == 0)
                                {
                                    var nombreArchivo = riesgosMedida.imagen.Replace("../Content/images/medidas/", "");
                                    idMedidaRiesgo = Datos.ActualizarMedidaGeneralRiesgoV2(actualizar, nombreArchivo, "on", null, null, null);
                                }
                                else if (riesgosMedida.imagen_grande == 1)
                                {
                                    var nombreArchivo = riesgosMedida.imagen.Replace("../Content/images/medidas/", "");
                                    idMedidaRiesgo = Datos.ActualizarMedidaGeneralRiesgoV2(actualizar, nombreArchivo, null, "on", null, null);
                                }
                            }
                            else
                            {
                                idMedidaRiesgo = Datos.ActualizarMedidaGeneralRiesgoV2(actualizar, null, null, null, null, null);
                            }

                            #endregion

                        }
                    }
                }
                ViewData["medidas"] = Datos.ListarRiesgosMedidas();
                return RedirectToAction("riesgos_medidas", "Configuracion");
            }

            #endregion

            catch (Exception ex)
            {
                return RedirectToAction("riesgos_medidas", "Configuracion");
            }
        }

        public ActionResult editar_riesgo_medida(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            if (id != 0)
            {
                riesgos_medidas medida = Datos.ObtenerRiesgoMedidaporId(id);
                ViewData["EditarMedida"] = medida;

                if (medida.id_apartado != null)
                {
                    ViewData["apartadoSeleccionado"] = Datos.obtenerMedidaApartado((int)medida.id_apartado);
                }
                if (medida.id_riesgo != null)
                {
                    ViewData["tipoRiesgoSeleccionado"] = Datos.ObtenerTiposRiesgos((int)medida.id_riesgo);
                }

            }
            ViewData["riesgos"] = Datos.ListarRiesgos();
            //ViewData["apartados"] = Datos.ListarApartados().OrderBy(x => x.id);
            ViewData["apartados"] = Datos.ListarApartadosV2().OrderBy(x => x.id);
            ViewData["comboApartados"] = ObtenerApartados();
            ViewData["imagenesRiesgos"] = Datos.ListarRiesgosMedidasImagenes();


            return View();
        }

        public List<SelectListItem> ObtenerApartados()
        {
            List<SelectListItem> datos = null;
            try
            {
                List<medidas_apartadosV2> listado = Datos.ListarApartadosV2();
                datos = new List<SelectListItem>();
                SelectListItem inicial = new SelectListItem();
                inicial.Text = "Seleccione Apartado";
                inicial.Value = "0";
                datos.Add(inicial);
                SelectListItem iyf = new SelectListItem();
                iyf.Text = "INFORMACION Y FORMACION";
                iyf.Value = "1";
                datos.Add(iyf);
                SelectListItem mg = new SelectListItem();
                mg.Text = "MEDIDAS GENERALES";
                mg.Value = "2";
                datos.Add(mg);
                SelectListItem ep = new SelectListItem();
                ep.Text = "EQUIPOS DE PROTECCION";
                ep.Value = "3";
                datos.Add(ep);
            }
            catch (Exception ex)
            {

            }
            return datos;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult editar_medida_preventiva(int id, FormCollection collection, HttpPostedFileBase seleccionArchivoIC, HttpPostedFileBase seleccionArchivoIG)
        {
            log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(Server.MapPath("~/log4net.config")));

            if (Session["usuario"] == null)

                if (Session["usuario"] == null)
                {
                    return RedirectToAction("LogOn", "Account");
                }
            try
            {
                medidas_preventivas actualizar = new medidas_preventivas();
                actualizar.id = id;

                //crear
                if (id == 0)
                {
                    Validation_editar_medida_preventiva validar = new Validation_editar_medida_preventiva();
                    List<string> listaE = validar.validaFormInsertar(collection, seleccionArchivoIC, seleccionArchivoIG);

                    if (listaE.Count > 0)
                    {
                        Session["ErrorForm"] = "Error al completar el formulario: " + listaE[0];

                        var medida = Datos.ListarSituaciones();
                        ViewData["situaciones"] = medida;

                        var situacion = int.Parse(collection["ctl00$MainContent$ddlSituaciones"]);

                        ViewData["situacion"] = situacion;
                        ViewData["descripcion"] = collection["ctl00$MainContent$txtNombre"];

                        return View();
                    }
                    string descripcion = collection["ctl00$MainContent$txtNombre"];
                    string situacionString = collection["ctl00$MainContent$ddlSituaciones"];
                    actualizar.descripcion = descripcion;
                    actualizar.id_situacion = int.Parse(collection["ctl00$MainContent$ddlSituaciones"]);
                    //actualizar.id_centro = int.Parse(Session["CentralElegida"].ToString());
                    actualizar.id_centro = 0;

                    var archivoIC = seleccionArchivoIC;
                    var archivoIG = seleccionArchivoIG;

                    int idMedida = 0;
                    int idMedidaImagen = 0;

                    if (archivoIC != null)
                    {

                        //idMedida = Datos.ActualizarMedidaPreventivaV2(actualizar);
                        idMedida = GuardarMedidasCompuestasMedidasPreventivas(idMedida, situacionString, descripcion);
                        idMedidaImagen = Datos.ActualizarMedidaPreventivaImagen(idMedida, archivoIC.FileName, "on", null, archivoIC, null);
                    }
                    else if (archivoIG != null)
                    {
                        idMedida = GuardarMedidasCompuestasMedidasPreventivas(idMedida, situacionString, descripcion);
                        idMedidaImagen = Datos.ActualizarMedidaPreventivaImagen(idMedida, archivoIG.FileName, null, "on", null, archivoIG);
                    }
                    else
                    {
                        idMedida = GuardarMedidasCompuestasMedidasPreventivas(idMedida, situacionString, descripcion);
                    }
                    Alert("Nueva medida agregada", NotificationType.success, 2000);
                }

                //actualizar
                else
                {

                    Validation_editar_medida_preventiva validar = new Validation_editar_medida_preventiva();
                    List<string> listaE = validar.validaFormActualizar(collection, seleccionArchivoIC, seleccionArchivoIG);

                    if (listaE.Count > 0)
                    {
                        Session["ErrorForm"] = "Error al completar el formulario: " + listaE[0];

                        var medida = Datos.ListarSituaciones();
                        ViewData["situaciones"] = medida;

                        var situacion = int.Parse(collection["ctl00$MainContent$ddlSituaciones"]);

                        ViewData["situacion"] = situacion;
                        ViewData["descripcion"] = collection["ctl00$MainContent$txtNombre"];

                        return View();
                    }
                    string descripcion = collection["ctl00$MainContent$txtNombre"];
                    string situacionString = collection["ctl00$MainContent$ddlSituaciones"];

                    actualizar.descripcion = descripcion;
                    actualizar.id_situacion = int.Parse(collection["ctl00$MainContent$ddlSituaciones"]);
                    //actualizar.id_centro = int.Parse(Session["CentralElegida"].ToString());
                    actualizar.id_centro = 0;

                    var archivoIC = seleccionArchivoIC;
                    var archivoIG = seleccionArchivoIG;

                    int idMedida = id;
                    var idImagen = Datos.obtenerMedidasPreventivasImagenes(id);

                    if (archivoIC != null)
                    {

                        idMedida = GuardarMedidasCompuestasMedidasPreventivas(idMedida, situacionString, descripcion);
                        var idMedidaImagen = Datos.ActualizarMedidaPreventivaImagen(idMedida, archivoIC.FileName, "on", null, archivoIC, null);
                    }
                    else if (archivoIG != null)
                    {
                        idMedida = GuardarMedidasCompuestasMedidasPreventivas(idMedida, situacionString, descripcion);
                        var idMedidaImagen = Datos.ActualizarMedidaPreventivaImagen(idMedida, archivoIG.FileName, null, "on", null, archivoIG);
                    }
                    else
                    {
                        idMedida = GuardarMedidasCompuestasMedidasPreventivas(idMedida, situacionString, descripcion);
                    }
                    Alert("Medida editada", NotificationType.success, 2000);
                }
                ViewData["medidas"] = Datos.ListarRiesgosMedidas();
                return RedirectToAction("medidas_preventivas", "Configuracion");
            }

            catch (Exception ex)
            {
                new EscribirLog("Error al conectar al servidor de BD " +
                            ex.Message, true, this.ToString(), "Principal");
                return RedirectToAction("medidas_preventivas", "Configuracion");
            }
        }

        public int GuardarMedidasCompuestasMedidasPreventivas(int idMedida, string situacion, string descripcion)
        {
            //centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));

            centros centroseleccionado = new centros();
            bool datos = false;
            string[] stringSeparators = new string[] { "\n" };
            string[] lines = descripcion.Split(stringSeparators, StringSplitOptions.None);

            medidas_preventivas medidas = new medidas_preventivas();

            medidas.id = idMedida;
            medidas.id_situacion = int.Parse(situacion);
            medidas.descripcion = lines.First();
            medidas.id_centro = 0;
            int id = Datos.ActualizarMedidaPreventiva(medidas);

            int contador = 1;

            //borramos las submedidas, para cargar las nuevas submedidas
            if (medidas.id != 0)
            {
                Datos.EliminarSubMedidas(medidas.id);
            }

            foreach (string s in lines)
            {
                if (contador > 1)
                {
                    if (!string.IsNullOrEmpty(s))
                    {
                        submedidas_preventivas submedidas = new submedidas_preventivas();
                        string cadena = s.Replace("-", "");
                        submedidas.descripcion = CleanInput(cadena);
                        submedidas.id_medida_preventiva = id;
                        Datos.ActualizarSubMedidasPreventivas(submedidas);
                    }
                }

                contador++;
            }

            return id;



        }

        public ActionResult editar_medida_preventiva(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            //ViewData["idCentral"] = id;
            if (id != 0)
            {
                medidas_preventivas medida = Datos.ObtenerMedidaPreventivaId(id);
                medidaspreventivas_imagenes imagenes = Datos.obtenerMedidasPreventivasImagenes(id);
                ViewData["EditarMedida"] = medida;
                ViewData["EditarImagen"] = imagenes;
                ViewData["Centro"] = medida.id_centro;

            }

            List<riesgos_situaciones> listaModificada = Datos.ListarSituaciones();
            foreach (riesgos_situaciones item in listaModificada)
            {
                item.descripcion = "Riesgo:" + item.id_tipo_riesgo + " - " + item.descripcion;
            }
            ViewData["situaciones"] = listaModificada;

            return View();
        }

        [HttpGet]
        public ActionResult editar_persona(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            if (id != 0)
            {
                personas persona = Datos.ObtenerPersonaId(id);
                ViewData["persona"] = persona;
            }

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult editar_persona(int id, FormCollection collection, HttpPostedFileBase seleccionArchivoIC, HttpPostedFileBase seleccionArchivoIL)
        {
            log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(Server.MapPath("~/log4net.config")));


            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            ViewData["idCentral"] = id;

            //try
            //{

            //    #region agregar central
            //    centros actualizar = new centros();
            //    actualizar.id = id;

            //    if (actualizar.id == 0)
            //    {
            //        Validation_editar_centro validar = new Validation_editar_centro();
            //        List<string> listaE = validar.validaFormInsertar(id, collection, seleccionArchivoIC, seleccionArchivoIL);

            //        if (listaE.Count > 0)
            //        {
            //            Session["RepoblarCampos"] = collection;
            //            Session["ErrorForm"] = "Error al completar el formulario: " + listaE[0];

            //            ViewData["ubicaciones"] = Datos.ListarProvincias();
            //            ViewData["unidades"] = Datos.ListarTecnologias();
            //            ViewData["listadoZonas"] = Datos.ListarZonas(int.Parse(collection["ctl00$MainContent$ddlTipo"]));
            //            ViewData["listadoAgrupacion"] = Datos.ListarAgrupacionesPorZonas(int.Parse(collection["ctl00$MainContent$ddlZonas"]));

            //            return View();
            //            //return RedirectToAction("editar_centro", "Configuracion");
            //        }

            //        var archivoIC = seleccionArchivoIC;
            //        var archivoIL = seleccionArchivoIL;
            //        var rutaImagenCentro = "../Content/images/centros/";
            //        var rutaImagenLogo = "../Content/images/centros/logos/";
            //        centros_zonas centZona = new centros_zonas();
            //        centros_agrupacion centAgrup = new centros_agrupacion();

            //        actualizar.siglas = null;
            //        actualizar.nombre = collection["ctl00$MainContent$txtNombre"];
            //        actualizar.tipo = int.Parse(collection["ctl00$MainContent$ddlTipo"]);
            //        //actualizar.provincia = int.Parse(collection["ctl00$MainContent$ddlProvincia"]);
            //        actualizar.rutaImagen = rutaImagenCentro + archivoIC.FileName;
            //        actualizar.direccion = collection["ctl00$MainContent$direccion"];
            //        actualizar.coordenadas = collection["ctl00$MainContent$coordenadas"];
            //        actualizar.rutaImagenLogo = rutaImagenLogo + archivoIL.FileName;

            //        if (actualizar.tipo == 7 || actualizar.tipo == 8 || actualizar.tipo == 9)
            //        {
            //            centZona.id_zona = int.Parse(collection["ctl00$MainContent$ddlZonas"]);
            //            centAgrup.id_agrupacion = int.Parse(collection["ctl00$MainContent$ddlAgrupacion"]);
            //        }
            //        int idCentral = Datos.ActualizarCentro(actualizar, centZona, centAgrup);

            //        //guardamos el fichero imagen
            //        Datos.ActualizarImagenCentro(actualizar);
            //        string rutaSitio = Server.MapPath("~/");
            //        string rutaImagenCompletaIC = Path.Combine(rutaSitio + "/Content/images/centros/" + archivoIC.FileName);
            //        archivoIC.SaveAs(rutaImagenCompletaIC);
            //        string rutaImagenCompletaIL = Path.Combine(rutaSitio + "/Content/images/centros/logos/" + archivoIL.FileName);
            //        archivoIL.SaveAs(rutaImagenCompletaIL);

            //        centros Centro = Datos.ObtenerCentroPorID(id);
            //        ViewData["EditarCentro"] = Centro;
            //        ViewData["ubicaciones"] = Datos.ListarProvincias();
            //        ViewData["unidades"] = Datos.ListarTecnologias();

            //        Session["EdicionCentroMensaje"] = "Los datos han sido modificados correctamente";
            //        //borramos la session
            //        Session["RepoblarCampos"] = null;

            //        return RedirectToAction("centros", "Configuracion");
            //    }
            //    else //Actualizar
            //    {
            //        var chkIC = collection["ctl00$MainContent$chkIC"];
            //        var chkIL = collection["ctl00$MainContent$chkIL"];
            //        Validation_editar_centro validar = new Validation_editar_centro();
            //        List<string> listaE = validar.validaFormActualizar(id, collection, seleccionArchivoIC, seleccionArchivoIL, chkIC, chkIL);

            //        if (listaE.Count > 0)
            //        {
            //            Session["RepoblarCampos"] = collection;
            //            Session["ErrorForm"] = "Error al completar el formulario: " + listaE[0];

            //            ViewData["ubicaciones"] = Datos.ListarProvincias();
            //            ViewData["unidades"] = Datos.ListarTecnologias();
            //            ViewData["listadoZonas"] = Datos.ListarZonas(int.Parse(collection["ctl00$MainContent$ddlTipo"]));
            //            ViewData["listadoAgrupacion"] = Datos.ListarAgrupacionesPorZonas(int.Parse(collection["ctl00$MainContent$ddlZonas"]));

            //            return View();
            //        }

            //        var archivoIC = seleccionArchivoIC;
            //        var archivoIL = seleccionArchivoIL;
            //        var rutaImagenCentro = "../Content/images/centros/";
            //        var rutaImagenLogo = "../Content/images/centros/logos/";
            //        centros_zonas centZona = new centros_zonas();
            //        centros_agrupacion centAgrup = new centros_agrupacion();

            //        actualizar.siglas = null;
            //        actualizar.nombre = collection["ctl00$MainContent$txtNombre"];
            //        actualizar.tipo = int.Parse(collection["ctl00$MainContent$ddlTipo"]);
            //        //actualizar.provincia = int.Parse(collection["ctl00$MainContent$ddlProvincia"]);
            //        actualizar.rutaImagen = archivoIC != null ? rutaImagenCentro + archivoIC.FileName : null;
            //        actualizar.direccion = collection["ctl00$MainContent$direccion"];
            //        actualizar.coordenadas = collection["ctl00$MainContent$coordenadas"];
            //        actualizar.rutaImagenLogo = archivoIL != null ? rutaImagenLogo + archivoIL.FileName : null;

            //        if (actualizar.tipo == 7 || actualizar.tipo == 8 || actualizar.tipo == 9)
            //        {
            //            centZona.id_zona = int.Parse(collection["ctl00$MainContent$ddlZonas"]);
            //            centAgrup.id_agrupacion = int.Parse(collection["ctl00$MainContent$ddlAgrupacion"]);
            //        }
            //        int idCentral = Datos.ActualizarCentro(actualizar, centZona, centAgrup);

            //        //guardamos el fichero imagen
            //        // Datos.ActualizarImagenCentro(actualizar);
            //        string rutaSitio = Server.MapPath("~/");
            //        if (archivoIC != null)
            //        {
            //            string rutaImagenCompletaIC = Path.Combine(rutaSitio + "/Content/images/centros/" + archivoIC.FileName);
            //            archivoIC.SaveAs(rutaImagenCompletaIC);
            //        }
            //        if (archivoIL != null)
            //        {
            //            string rutaImagenCompletaIL = Path.Combine(rutaSitio + "/Content/images/centros/logos/" + archivoIL.FileName);
            //            archivoIL.SaveAs(rutaImagenCompletaIL);
            //        }

            //        centros Centro = Datos.ObtenerCentroPorID(id);
            //        ViewData["EditarCentro"] = Centro;
            //        ViewData["ubicaciones"] = Datos.ListarProvincias();
            //        ViewData["unidades"] = Datos.ListarTecnologias();

            //        Session["EdicionCentroMensaje"] = "Los datos han sido modificados correctamente";
            //        //borramos la session
            //        Session["RepoblarCampos"] = null;

            return RedirectToAction("personas", "Configuracion");
            //    }
            //    #endregion
            //}
            //catch (Exception ex)
            //{
            //    new EscribirLog("Error: " +
            //                ex.Message, true, this.ToString(), "editar_centro");
            //    Session["EdicionCentroError"] = "FALLA" + ";" + ex.Message;
            //    return RedirectToAction("centros", "Configuracion");
            //}

        }


        public ActionResult editar_central(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            ViewData["idCentral"] = id;

            ViewData["areas"] = Datos.ListarAreas();

            centros buscarCentro = Datos.ObtenerCentroPorID(id);
            ViewData["EditarCentro"] = buscarCentro;
            ViewData["ubicaciones"] = Datos.ListarProvincias();
            ViewData["unidades"] = Datos.ListarTecnologias();

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult editar_central(int id, FormCollection collection, string submit)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }
            if (submit == "Añadir Área")
            {
                if (!string.IsNullOrEmpty(collection["ctl00$MainContent$areanombre"]) && !string.IsNullOrEmpty(collection["ctl00$MainContent$areacodigo"]))
                {
                    return RedirectToAction("guardar_area", "Configuracion", new { id = id, nombre = collection["ctl00$MainContent$areanombre"], codigo = collection["ctl00$MainContent$areacodigo"] });
                }
            }

            ViewData["idCentral"] = id;

            try
            {
                #region editar central
                centros actualizar = new centros();
                actualizar.id = id;


                if (collection["ctl00$MainContent$txtNombre"] != null)
                {
                    //   actualizar.id = int.Parse(collection["ctl00$MainContent$hdnIdCentro"]);
                    actualizar.siglas = null;
                    actualizar.nombre = collection["ctl00$MainContent$txtNombre"];
                    actualizar.tipo = int.Parse(collection["ctl00$MainContent$ddlTipo"]);
                    actualizar.provincia = int.Parse(collection["ctl00$MainContent$ddlProvincia"]);

                    if (actualizar.id == 0)
                    {
                        centros comprobarUsuario = Datos.ObtenerCentroPorNombre(actualizar.nombre);
                        if (comprobarUsuario != null)
                        {
                            centros buscarCentro = Datos.ObtenerCentroPorID(id);
                            ViewData["EditarCentro"] = buscarCentro;
                            ViewData["ubicaciones"] = Datos.ListarProvincias();
                            ViewData["unidades"] = Datos.ListarTecnologias();
                            Session["EdicionCentroError"] = "La central ya existe";
                            Session["CentroErroneo"] = actualizar;
                            return View();
                        }
                    }

                    if (actualizar.nombre == string.Empty)
                    {
                        centros buscarCentro = Datos.ObtenerCentroPorID(id);
                        ViewData["EditarCentro"] = buscarCentro;
                        ViewData["ubicaciones"] = Datos.ListarProvincias();
                        ViewData["unidades"] = Datos.ListarTecnologias();
                        Session["EdicionCentroError"] = "Debe introducir un nombre para la central";
                        Session["CentroErroneo"] = actualizar;

                        return View();
                    }

                    // Insercción código - Rafael Ortega -- SOLO PARA PRUEBAS, HAY QUE BORRARLO POSTERIORMENTE
                    centros_zonas centZona = new centros_zonas();
                    centros_agrupacion centAgrup = new centros_agrupacion();
                    // Insercción código - Rafael Ortega -- SOLO PARA PRUEBAS, HAY QUE BORRARLO POSTERIORMENTE

                    int idCentral = Datos.ActualizarCentro(actualizar, centZona, centAgrup);

                    centros Centro = Datos.ObtenerCentroPorID(id);
                    ViewData["EditarCentro"] = Centro;
                    ViewData["ubicaciones"] = Datos.ListarProvincias();
                    ViewData["unidades"] = Datos.ListarTecnologias();

                    Session["EdicionCentroMensaje"] = "Los datos han sido modificados correctamente";

                    return RedirectToAction("editar_central/" + idCentral, "Configuracion");
                }
                else
                {
                    actualizar.siglas = null;
                    actualizar.nombre = collection["ctl00$MainContent$txtNombre"];
                    actualizar.tipo = int.Parse(collection["ctl00$MainContent$ddlTipo"]);
                    actualizar.provincia = int.Parse(collection["ctl00$MainContent$ddlProvincia"]);
                    Session["CentroErroneo"] = actualizar;
                    Session["EdicionCentroError"] = "Debe introducir un nombre de centro";

                    return View();
                }
                #endregion
            }
            catch (Exception ex)
            {
                Session["EdicionCentroError"] = "FALLA" + ";" + ex.Message;
                return RedirectToAction("centros", "Configuracion");
            }

        }

        public static int ColumnIndex(string reference)
        {
            int ci = 0;
            reference = reference.ToUpper();
            for (int ix = 0; ix < reference.Length && reference[ix] >= 'A'; ix++)
                ci = (ci * 26) + ((int)reference[ix] - 64);
            return ci;
        }
        public static SharedStringItem GetSharedStringItemById(WorkbookPart workbookPart, int id)
        {
            return workbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(id);
        }
        private string GetCellText(Cell c, string[] saSST)
        {
            string val = "";

            if ((c.DataType != null) && (c.DataType == CellValues.SharedString))
            {
                int ssid;
                if (int.TryParse(c.CellValue.Text, out ssid))
                {
                    if (saSST != null && ssid >= 0 && ssid < saSST.Length)
                    {
                        val = saSST[ssid];
                    }
                }
            }
            else if ((c.DataType != null) && c.DataType == CellValues.InlineString)
            {
                val = c.InnerText;
            }
            else if (c.CellValue != null)
            {
                val = c.CellValue.Text;
            }

            if (val == null)
                val = "";

            return val;

        }

        static void ReadExcelFileDOM(string fileName, int tecnologia)
        {
            StreamWriter sw = new StreamWriter("C:\\Users\\jose.pinto\\Test.txt");
            try
            {


                using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
                {
                    WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                    WorksheetPart worksheetPart = workbookPart.WorksheetParts.First();
                    SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();
                    string text;

                    SharedStringTablePart sstpart = workbookPart.GetPartsOfType<SharedStringTablePart>().First();
                    SharedStringTable sst = sstpart.SharedStringTable;


                    int ultimoniveluno = 0;
                    int ultimoniveldos = 0;
                    int ultimoniveltres = 0;
                    int ultimonivelcuatro = 0;
                    foreach (Row r in sheetData.Elements<Row>())
                    {
                        if (r.RowIndex > 1)
                        {
                            int nivel = 0;
                            string nombre = "";
                            areanivel1 nivel1 = new areanivel1();
                            areanivel2 nivel2 = new areanivel2();
                            areanivel3 nivel3 = new areanivel3();
                            areanivel4 nivel4 = new areanivel4();
                            bool[] tabla = new bool[28];
                            foreach (Cell c in r.Elements<Cell>())
                            {
                                if (c != null)
                                {


                                    int columna = ColumnIndex(c.CellReference.ToString());
                                    if (c.CellValue != null)
                                    {


                                        if (!string.IsNullOrEmpty(c.CellValue.Text.Trim()))
                                        {
                                            if (columna == 2)
                                            {
                                                if ((c.DataType != null) && (c.DataType == CellValues.SharedString))
                                                {
                                                    int ssid = int.Parse(c.CellValue.Text);
                                                    nombre = sst.ChildElements[ssid].InnerText;
                                                }

                                            }
                                            if (columna == 1)
                                            {
                                                nivel = int.Parse(c.CellValue.Text);
                                            }

                                            if (columna >= 3)
                                            {

                                                tabla[columna - 3] = true;
                                            }

                                        }
                                        else
                                        {
                                            if (columna >= 3)
                                            {
                                                tabla[columna - 3] = true;
                                            }
                                        }
                                    }
                                }

                            }
                            int identificador = 0;

                            if (nivel == 1)
                            {
                                nivel1.id_centro = 0;
                                nivel1.id_tecnologia = tecnologia;
                                nivel1.nombre = nombre;
                                ultimoniveluno = Datos.ActualizarArea(nivel1);
                                identificador = ultimoniveluno;
                                //    sw.WriteLine(nivel + "|" + nombre);
                            }
                            if (nivel == 2)
                            {
                                nivel2.id_areanivel1 = ultimoniveluno;
                                nivel2.nombre = nombre;
                                ultimoniveldos = Datos.ActualizarSistema(nivel2);
                                identificador = ultimoniveldos;
                                // sw.WriteLine(nivel + "|" + nombre);

                            }
                            if (nivel == 3)
                            {
                                nivel3.id_areanivel2 = ultimoniveldos;
                                nivel3.nombre = nombre;
                                ultimoniveltres = Datos.ActualizarEquipo(nivel3);
                                identificador = ultimoniveltres;
                                // sw.WriteLine(nivel + "|" + nombre);

                            }
                            if (nivel == 4)
                            {
                                nivel4.id_areanivel3 = ultimoniveltres;
                                nivel4.nombre = nombre;
                                ultimonivelcuatro = Datos.ActualizarNivelCuatro(nivel4);
                                identificador = ultimonivelcuatro;
                                //sw.WriteLine(nivel + "|" + nombre);
                            }

                            int contador = 0;
                            if (identificador != 0)
                            {
                                foreach (bool item in tabla)
                                {
                                    contador++;
                                    sw.WriteLine(cadenaInsert(nivel, identificador, item == true ? 1 : 0, contador, tecnologia));
                                }

                            }


                            //Close the file


                        }

                    }
                    sw.Close();

                }
            }
            catch (Exception ex)
            {
                sw.Close();
            }
        }

        public static string cadenaInsert(int nivel, int identificador, int activo, int y, int tecnologia)
        {
            string resultado = "";
            if (nivel == 1)
            {
                resultado = "INSERT INTO [dbo].[matriz_inicial] ([id_tecnologia],[id_riesgo],[id_areanivel1],[id_areanivel2],[id_areanivel3],[id_areanivel4],[activo]) VALUES (" + tecnologia + "," + y + "," + identificador + ",null,null,null," + activo + ")";
            }
            else if (nivel == 2)
            {
                resultado = "INSERT INTO [dbo].[matriz_inicial] ([id_tecnologia],[id_riesgo],[id_areanivel1],[id_areanivel2],[id_areanivel3],[id_areanivel4],[activo]) VALUES (" + tecnologia + "," + y + ",null," + identificador + ",null," + "null," + activo + ")";

            }
            else if (nivel == 3)
            {
                resultado = "INSERT INTO [dbo].[matriz_inicial] ([id_tecnologia],[id_riesgo],[id_areanivel1],[id_areanivel2],[id_areanivel3],[id_areanivel4],[activo]) VALUES (" + tecnologia + "," + y + ",null,null," + identificador + "," + "null," + activo + ")";

            }
            else if (nivel == 4)
            {
                resultado = "INSERT INTO [dbo].[matriz_inicial] ([id_tecnologia],[id_riesgo],[id_areanivel1],[id_areanivel2],[id_areanivel3],[id_areanivel4],[activo]) VALUES (" + tecnologia + "," + y + ",null,null,null," + identificador + "," + activo + ")";

            }
            return resultado;
        }

        public ActionResult eliminarTodoAreas()
        {

            //PONER A TRUE PARA QUE FUNCIONE
            bool debug = false;
            try
            {
                if (debug)
                {
                    List<areanivel1> lista = Datos.ListarAreas();
                    lista = lista.Where(x => x.id_centro != 0).ToList();

                    foreach (areanivel1 item in lista)
                    {
                        Datos.EliminarArea(item.id);
                    }
                }

            }
            catch (Exception ex)
            {

                RedirectToAction("Principal", "Home");

            }
            return RedirectToAction("Principal", "Home");
        }
        public ActionResult carga_maestros(FormCollection collection, string submit)
        {

            try
            {
                if (Session["usuario"] == null)
                {
                    return RedirectToAction("LogOn", "Account");
                }
                //PONER DEBUG A TRUE PARA QUE ESTO FUNCIONE.
                bool debug = true;
                if (debug)
                {
                    //CREA UN FICHERO EXCEL PLANTILLA, PEGA LOS DATOS Y PON AQUI LA RUTA, PRIMERA COLUMNA NIVELES(1,2,3, o 4), SEGUNDA COLUMNA NOMBRE DEL AREA, TERCERA HASTA EL FINAL, CHECK DEL 1 al 28
                    //ESPECIFICAR LA RUTA DE LA PLANTILLA, Y LA TECNOLOGIA. ESTAR ATENTO YA QUE LO PRIMERO QUE SE HACE ES BORRAR TODOS LOS REGISTROS DE LA TECNOLOGIA INDICADA, PARA LUEGO CREARLOS.
                    String fileName = @"C:\Users\jose.pinto\plantilla.xlsx";

                    /*ESPECIFICA AQUI EL CODIGO DE TECNOLOGIA, Y LUEGO EJECUTA EL CARGA_MAESTROS*/
                    int tecnologia = 1;
                    /*ESPECIFICA AQUI EL CODIGO DE TECNOLOGIA*/
                    List<areanivel1> lista = Datos.ListarAreas();
                    lista = lista.Where(x => x.id_centro == 0 && x.id_tecnologia == tecnologia).ToList();

                    foreach (areanivel1 item in lista)
                    {
                        Datos.EliminarArea(item.id);
                    }
                    ReadExcelFileDOM(fileName, tecnologia);    // DOM

                }



            }
            catch (Exception ex)
            {

                RedirectToAction("Principal", "Home");

            }
            return RedirectToAction("Principal", "Home");
        }


        [AcceptVerbs(HttpVerbs.Post), ValidateInput(false)]
        public ActionResult editar_informacion_general(FormCollection collection, string submit)
        {
            try
            {
                if (Session["usuario"] == null)
                {
                    return RedirectToAction("LogOn", "Account");
                }

                if (!string.IsNullOrEmpty(collection["txt_desc"].ToString()))
                {
                    //centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
                    descripcion_general descripcion = new descripcion_general();
                    descripcion.id = 1;
                    descripcion.descripcion = collection["txt_desc"].ToString();
                    FreeTextBoxControls.FreeTextBox pruebad = new FreeTextBoxControls.FreeTextBox();
                    pruebad.Text = collection["txt_desc"].ToString();
                    string cadenna = pruebad.HtmlStrippedText;
                    descripcion.descripcionTexto = cadenna;
                    Datos.ActualizarDescripcionGeneral(descripcion);

                }
            }
            catch (Exception ex)
            {

                RedirectToAction("Principal", "Home");

            }
            return RedirectToAction("editar_informacion_general", "Configuracion");
        }


        public ActionResult editar_informacion_general()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            //centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            descripcion_general desc = Datos.ObtenerDescripcionGeneral(1);
            if (desc != null)
            {
                ViewData["descripcionGeneral"] = desc.descripcion;
            }



            return View();
        }



        public JsonResult GuardarImagenLogo()
        {
            string datos = "";
            List<centros> response = new List<centros>();
            try
            {

                HttpPostedFileBase img2 = Request.Files["photo"];
                string hdnIdCentral = Request.Params["hdnIdCentral"].ToString();
                var ruta = System.IO.Path.Combine(Server.MapPath("~/Content/images/centros/logos"), img2.FileName);
                string rutabd = "../Content/images/centros/logos/" + img2.FileName;
                img2.SaveAs(ruta);
                centros centrosE = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(hdnIdCentral));
                centros ri = new centros();
                ri.id = centrosE.id;
                ri.rutaImagenLogo = rutabd;
                Datos.ActualizarImagenCentroLogo(ri);
                datos = rutabd;
                // var prueba= data;
                response.Add(ri);
            }
            catch (Exception ex)
            {

            }
            return Json(response);
        }
        public JsonResult ExisteImagenCentro()
        {

            string response = null;
            try
            {
                HttpPostedFileBase img2 = Request.Files["photo"];
                string hdnIdCentral = Request.Params["hdnIdCentral"].ToString();

                string rutaServer = System.Web.Hosting.HostingEnvironment.MapPath("~/");
                var rutaBD = "../Content/images/centros/";

                //obtenemos la ruta de la BD
                var existeRutaImagen = Datos.ExisteRutaImagenCentro(rutaBD + img2.FileName);
                if (existeRutaImagen)
                {
                    response = "Error: Ruta de imagen centro existente!";
                }
                else
                {
                    //obtenemos la ruta de la carpeta para verificar si existe el fichero
                    rutaBD = rutaBD.Replace("..", "");
                    string rutaFichero = Path.Combine(rutaServer + rutaBD + img2.FileName);
                    if (System.IO.File.Exists(rutaFichero))
                    {
                        response = "Error: Fichero de imagen centro existente!";
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return Json(response);
        }
        public JsonResult ExisteImagenIcono()
        {
            string response = null;
            try
            {
                HttpPostedFileBase img2 = Request.Files["photo"];
                //string hdIdMedida = Request.Params["hdIdMedida"].ToString();

                string rutaServer = System.Web.Hosting.HostingEnvironment.MapPath("~/");
                var rutaBD = "../Content/images/medidas/";

                //obtenemos la ruta de la BD
                var existeRutaImagen = Datos.ExisteRutaImagenIcono(rutaBD + img2.FileName);
                if (existeRutaImagen)
                {
                    response = "Error: Ruta de imagen icono existente!";
                }
                else
                {
                    //obtenemos la ruta de la carpeta para verificar si existe el fichero
                    rutaBD = rutaBD.Replace("..", "");
                    string rutaFichero = Path.Combine(rutaServer + rutaBD + img2.FileName);
                    if (System.IO.File.Exists(rutaFichero))
                    {
                        response = "Error: Fichero de imagen icono existente!";
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return Json(response);
        }
        public JsonResult ExisteImagenGrande()
        {
            string response = null;
            try
            {
                HttpPostedFileBase img2 = Request.Files["photo"];
                //string hdIdMedida = Request.Params["hdIdMedida"].ToString();

                string rutaServer = System.Web.Hosting.HostingEnvironment.MapPath("~/");
                var rutaBD = "../Content/images/medidas/";

                //obtenemos la ruta de la BD
                var existeRutaImagen = Datos.ExisteRutaImagenIcono(rutaBD + img2.FileName);
                if (existeRutaImagen)
                {
                    response = "Error: Ruta de imagen grande existente!";
                }
                else
                {
                    //obtenemos la ruta de la carpeta para verificar si existe el fichero
                    rutaBD = rutaBD.Replace("..", "");
                    string rutaFichero = Path.Combine(rutaServer + rutaBD + img2.FileName);
                    if (System.IO.File.Exists(rutaFichero))
                    {
                        response = "Error: Fichero de imagen grande existente!";
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return Json(response);
        }
        public JsonResult ExisteImagenLogo()
        {
            string datos = "";


            string response = null;
            try
            {
                HttpPostedFileBase img2 = Request.Files["photo"];
                string hdnIdCentral = Request.Params["hdnIdCentral"].ToString();

                string rutaServer = System.Web.Hosting.HostingEnvironment.MapPath("~/");
                var rutaBD = "../Content/images/centros/logos/";

                //obtenemos la ruta de la BD
                var existeRutaImagen = Datos.ExisteRutaImagenCentro(rutaBD + img2.FileName);
                if (existeRutaImagen)
                {
                    response = "Error: Ruta de imagen logo existente!";
                }
                else
                {
                    //obtenemos la ruta de la carpeta para verificar si existe el fichero
                    rutaBD = rutaBD.Replace("..", "");
                    string rutaFichero = Path.Combine(rutaServer + rutaBD + img2.FileName);
                    if (System.IO.File.Exists(rutaFichero))
                    {
                        response = "Error: Fichero de imagen logo existente!";
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return Json(response);
        }

        public JsonResult GuardarImagenCentro()
        {
            string datos = "";
            List<centros> response = new List<centros>();
            try
            {

                HttpPostedFileBase img2 = Request.Files["photo"];
                string hdnIdCentral = Request.Params["hdnIdCentral"].ToString();
                var ruta = System.IO.Path.Combine(Server.MapPath("~/Content/images/centros"), img2.FileName);
                string rutabd = "../Content/images/centros/" + img2.FileName;
                img2.SaveAs(ruta);
                centros centrosE = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(hdnIdCentral));
                centros ri = new centros();
                ri.id = centrosE.id;
                ri.rutaImagen = rutabd;
                Datos.ActualizarImagenCentro(ri);
                datos = rutabd;
                // var prueba= data;
                response.Add(ri);
            }
            catch (Exception ex)
            {

            }
            return Json(response);
        }
        public JsonResult GuardarImageMedidaGeneral()
        {
            string datos = "";
            List<medidas_generales_imagenes> response = new List<medidas_generales_imagenes>();
            try
            {

                HttpPostedFileBase img2 = Request.Files["photo"];
                string hdnIdCentral = Request.Params["hdnIdMedida"].ToString();
                string tamano = Request.Params["tamano"].ToString();
                var ruta = System.IO.Path.Combine(Server.MapPath("~/Content/images/medidas/medidasgenerales/"), img2.FileName);
                string rutabd = "../Content/images/medidas/medidasgenerales/" + img2.FileName;
                img2.SaveAs(ruta);
                medidas_generales medidasE = MIDAS.Models.Datos.ObtenerMedidaGeneralID(int.Parse(hdnIdCentral));
                medidas_generales_imagenes ri = new medidas_generales_imagenes();
                ri.id = int.Parse(hdnIdCentral);
                ri.rutaImagen = rutabd;
                ri.id_medida_general = medidasE.id;
                if (tamano == "true") ri.tamano = true;
                Datos.ActualizarImagenMedidaGeneral(ri);
                datos = rutabd;
                // var prueba= data;
                response.Add(ri);
            }
            catch (Exception ex)
            {

            }
            return Json(response);
        }
        public JsonResult GuardarImageMedidaPreventiva()
        {
            string datos = "";
            List<medidaspreventivas_imagenes> response = new List<medidaspreventivas_imagenes>();
            try
            {

                HttpPostedFileBase img2 = Request.Files["photo"];
                string hdnIdCentral = Request.Params["hdnIdMedida"].ToString();
                string tamano = Request.Params["tamano"].ToString();
                var ruta = System.IO.Path.Combine(Server.MapPath("~/Content/images/medidas/medidaspreventivas/"), img2.FileName);
                string rutabd = "../Content/images/medidas/medidaspreventivas/" + img2.FileName;
                img2.SaveAs(ruta);
                medidas_preventivas medidasE = MIDAS.Models.Datos.ObtenerMedidaPreventivaId(int.Parse(hdnIdCentral));
                medidaspreventivas_imagenes ri = new medidaspreventivas_imagenes();
                ri.id = int.Parse(hdnIdCentral);
                ri.rutaImagen = rutabd;
                ri.id_medida = medidasE.id;
                if (tamano == "true") ri.tamano = true;
                Datos.ActualizarImagenRutaMedidaSituacionRiesgo(ri);
                datos = rutabd;
                // var prueba= data;
                response.Add(ri);
            }
            catch (Exception ex)
            {

            }
            return Json(response);
        }

        public JsonResult GuardarImageMedidaRiesgo()
        {
            string datos = "";
            List<riesgos_medidas> response = new List<riesgos_medidas>();
            try
            {

                HttpPostedFileBase img2 = Request.Files["photo"];
                string hdnIdCentral = Request.Params["hdnIdMedida"].ToString();
                string tamano = Request.Params["tamano"].ToString();
                var ruta = System.IO.Path.Combine(Server.MapPath("~/Content/images/medidas/"), img2.FileName);
                string rutabd = "../Content/images/medidas/" + img2.FileName;
                img2.SaveAs(ruta);
                riesgos_medidas medidasE = MIDAS.Models.Datos.ObtenerRiesgoMedidaporId(int.Parse(hdnIdCentral));
                riesgos_medidas ri = new riesgos_medidas();
                ri.id = int.Parse(hdnIdCentral);
                ri.imagen = rutabd;
                if (tamano == "true") ri.imagen_grande = 1;
                //ACTUALIZAR ESTO
                ri.imagen_grande = 1;

                Datos.ActualizarImagenRutaMedidaGeneralRiesgo(ri);
                datos = rutabd;
                // var prueba= data;
                response.Add(ri);
            }
            catch (Exception ex)
            {

            }
            return Json(response);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult editar_centro(int id, FormCollection collection, HttpPostedFileBase seleccionArchivoIC, HttpPostedFileBase seleccionArchivoIL)
        {
            log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(Server.MapPath("~/log4net.config")));


            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            ViewData["idCentral"] = id;

            try
            {

                #region agregar central
                centros actualizar = new centros();
                actualizar.id = id;

                if (actualizar.id == 0)
                {
                    Validation_editar_centro validar = new Validation_editar_centro();
                    List<string> listaE = validar.validaFormInsertar(id, collection, seleccionArchivoIC, seleccionArchivoIL);

                    if (listaE.Count > 0)
                    {
                        Session["RepoblarCampos"] = collection;
                        Session["ErrorForm"] = "Error al completar el formulario: " + listaE[0];

                        ViewData["ubicaciones"] = Datos.ListarProvincias();
                        ViewData["unidades"] = Datos.ListarTecnologias();
                        ViewData["listadoZonas"] = Datos.ListarZonas(int.Parse(collection["ctl00$MainContent$ddlTipo"]));
                        ViewData["listadoAgrupacion"] = Datos.ListarAgrupacionesPorZonas(int.Parse(collection["ctl00$MainContent$ddlZonas"]));

                        return View();
                        //return RedirectToAction("editar_centro", "Configuracion");
                    }

                    var archivoIC = seleccionArchivoIC;
                    var archivoIL = seleccionArchivoIL;
                    var rutaImagenCentro = "../Content/images/centros/";
                    var rutaImagenLogo = "../Content/images/centros/logos/";
                    centros_zonas centZona = new centros_zonas();
                    centros_agrupacion centAgrup = new centros_agrupacion();

                    actualizar.siglas = null;
                    actualizar.nombre = collection["ctl00$MainContent$txtNombre"];
                    actualizar.tipo = int.Parse(collection["ctl00$MainContent$ddlTipo"]);
                    //actualizar.provincia = int.Parse(collection["ctl00$MainContent$ddlProvincia"]);
                    actualizar.rutaImagen = rutaImagenCentro + archivoIC.FileName;
                    actualizar.direccion = collection["ctl00$MainContent$direccion"];
                    actualizar.coordenadas = collection["ctl00$MainContent$coordenadas"];
                    actualizar.rutaImagenLogo = rutaImagenLogo + archivoIL.FileName;

                    if (actualizar.tipo == 7 || actualizar.tipo == 8 || actualizar.tipo == 9)
                    {
                        centZona.id_zona = int.Parse(collection["ctl00$MainContent$ddlZonas"]);
                        centAgrup.id_agrupacion = int.Parse(collection["ctl00$MainContent$ddlAgrupacion"]);
                    }
                    int idCentral = Datos.ActualizarCentro(actualizar, centZona, centAgrup);

                    //guardamos el fichero imagen
                    Datos.ActualizarImagenCentro(actualizar);
                    string rutaSitio = Server.MapPath("~/");
                    string rutaImagenCompletaIC = Path.Combine(rutaSitio + "/Content/images/centros/" + archivoIC.FileName);
                    archivoIC.SaveAs(rutaImagenCompletaIC);
                    string rutaImagenCompletaIL = Path.Combine(rutaSitio + "/Content/images/centros/logos/" + archivoIL.FileName);
                    archivoIL.SaveAs(rutaImagenCompletaIL);

                    centros Centro = Datos.ObtenerCentroPorID(id);
                    ViewData["EditarCentro"] = Centro;
                    ViewData["ubicaciones"] = Datos.ListarProvincias();
                    ViewData["unidades"] = Datos.ListarTecnologias();

                    Session["EdicionCentroMensaje"] = "Los datos han sido modificados correctamente";
                    //borramos la session
                    Session["RepoblarCampos"] = null;

                    return RedirectToAction("centros", "Configuracion");
                }
                else //Actualizar
                {
                    var chkIC = collection["ctl00$MainContent$chkIC"];
                    var chkIL = collection["ctl00$MainContent$chkIL"];
                    Validation_editar_centro validar = new Validation_editar_centro();
                    List<string> listaE = validar.validaFormActualizar(id, collection, seleccionArchivoIC, seleccionArchivoIL, chkIC, chkIL);

                    if (listaE.Count > 0)
                    {
                        Session["RepoblarCampos"] = collection;
                        Session["ErrorForm"] = "Error al completar el formulario: " + listaE[0];

                        ViewData["ubicaciones"] = Datos.ListarProvincias();
                        ViewData["unidades"] = Datos.ListarTecnologias();
                        ViewData["listadoZonas"] = Datos.ListarZonas(int.Parse(collection["ctl00$MainContent$ddlTipo"]));
                        ViewData["listadoAgrupacion"] = Datos.ListarAgrupacionesPorZonas(int.Parse(collection["ctl00$MainContent$ddlZonas"]));

                        return View();
                    }

                    var archivoIC = seleccionArchivoIC;
                    var archivoIL = seleccionArchivoIL;
                    var rutaImagenCentro = "../Content/images/centros/";
                    var rutaImagenLogo = "../Content/images/centros/logos/";
                    centros_zonas centZona = new centros_zonas();
                    centros_agrupacion centAgrup = new centros_agrupacion();

                    actualizar.siglas = null;
                    actualizar.nombre = collection["ctl00$MainContent$txtNombre"];
                    actualizar.tipo = int.Parse(collection["ctl00$MainContent$ddlTipo"]);
                    //actualizar.provincia = int.Parse(collection["ctl00$MainContent$ddlProvincia"]);
                    actualizar.rutaImagen = archivoIC != null ? rutaImagenCentro + archivoIC.FileName : null;
                    actualizar.direccion = collection["ctl00$MainContent$direccion"];
                    actualizar.coordenadas = collection["ctl00$MainContent$coordenadas"];
                    actualizar.rutaImagenLogo = archivoIL != null ? rutaImagenLogo + archivoIL.FileName : null;

                    if (actualizar.tipo == 7 || actualizar.tipo == 8 || actualizar.tipo == 9)
                    {
                        centZona.id_zona = int.Parse(collection["ctl00$MainContent$ddlZonas"]);
                        centAgrup.id_agrupacion = int.Parse(collection["ctl00$MainContent$ddlAgrupacion"]);
                    }
                    int idCentral = Datos.ActualizarCentro(actualizar, centZona, centAgrup);

                    //guardamos el fichero imagen
                    // Datos.ActualizarImagenCentro(actualizar);
                    string rutaSitio = Server.MapPath("~/");
                    if (archivoIC != null)
                    {
                        string rutaImagenCompletaIC = Path.Combine(rutaSitio + "/Content/images/centros/" + archivoIC.FileName);
                        archivoIC.SaveAs(rutaImagenCompletaIC);
                    }
                    if (archivoIL != null)
                    {
                        string rutaImagenCompletaIL = Path.Combine(rutaSitio + "/Content/images/centros/logos/" + archivoIL.FileName);
                        archivoIL.SaveAs(rutaImagenCompletaIL);
                    }

                    centros Centro = Datos.ObtenerCentroPorID(id);
                    ViewData["EditarCentro"] = Centro;
                    ViewData["ubicaciones"] = Datos.ListarProvincias();
                    ViewData["unidades"] = Datos.ListarTecnologias();

                    Session["EdicionCentroMensaje"] = "Los datos han sido modificados correctamente";
                    //borramos la session
                    Session["RepoblarCampos"] = null;

                    return RedirectToAction("centros", "Configuracion");
                }
                #endregion
            }
            catch (Exception ex)
            {
                new EscribirLog("Error: " +
                            ex.Message, true, this.ToString(), "editar_centro");
                Session["EdicionCentroError"] = "FALLA" + ";" + ex.Message;
                return RedirectToAction("centros", "Configuracion");
            }
        }

        public ActionResult guardar_area(int id, string codigo, string nombre)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }
            int idRef = 0;
            ViewData["idReferencial"] = id;

            try
            {
                #region añadir referencial

                areanivel1 actualizar = new areanivel1();
                actualizar.id = 0;
                if (!string.IsNullOrEmpty(nombre) && !string.IsNullOrEmpty(codigo))
                {

                    actualizar.nombre = nombre;
                    actualizar.codigo = codigo;
                    actualizar.id_centro = id;

                    idRef = Datos.ActualizarArea(actualizar);


                    Session["EditarAreaResultado"] = "Area añadida correctamente";
                    Session["EdicionAreaMensaje"] = "Los datos han sido modificados correctamente";
                    return RedirectToAction("editar_central/" + id, "Configuracion");
                }
                else
                {
                    string cadenaError = string.Empty;

                    if (string.IsNullOrEmpty(nombre)) cadenaError += " [NOMBRE] ";
                    if (string.IsNullOrEmpty(codigo)) cadenaError += " [CODIGO] ";
                    Session["EditarSistemaResultado"] = "Falta por informar : " + cadenaError;

                    return RedirectToAction("editar_central/" + id, "Configuracion");
                }
                #endregion

            }
            catch (Exception ex)
            {
                #region exception
                Session["EditarAreaResultado"] = "FALLA" + ";" + ex.Message;

                return RedirectToAction("editar_central/" + idRef, "Configuracion");
                #endregion
            }
        }

        public ActionResult eliminar_centro(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            centros centro = new centros();
            centro = Datos.ObtenerCentroPorID(id);
            string rutaSitio = Server.MapPath("~/");

            try
            {

                if (centro.rutaImagen != null && centro.rutaImagen != "")
                {
                    var rutaImagen = centro.rutaImagen.Replace("..", "");
                    if (centro.rutaImagen != null)
                    {
                        if ((System.IO.File.Exists(rutaSitio + rutaImagen)))
                        {
                            System.IO.File.Delete(rutaSitio + rutaImagen);
                        }
                    }
                }
                if (centro.rutaImagenLogo != null && centro.rutaImagenLogo != "")
                {
                    var rutaLogo = centro.rutaImagenLogo.Replace("..", "");

                    if ((System.IO.File.Exists(rutaSitio + rutaLogo)))
                    {
                        System.IO.File.Delete(rutaSitio + rutaLogo);
                    }
                }


                Datos.EliminarCentro(id);
                //borramos las versiones matriz de la tabla version_matriz y matriz_centro
                List<version_matriz> versiones = Datos.listarMatrizVersion(id);
                foreach (version_matriz version in versiones)
                {
                    Datos.eliminarMatrizVersion(version.id);
                }

                //borramos las descripcion_centro 
                var descCentro = Datos.eliminarDescripcionCentro(id);

                //borramos los documentos_historicos 
                var listDocFinales = Datos.ListaDocumentoHistorico(id);
                foreach (documento_historico doc in listDocFinales)
                {
                    Datos.eliminarDocumentoHistorico(doc.id);
                }
                //borramos los documentos_riesgos 
                Datos.EliminarDocumentoRiesgoPorIdCentro(id);

                //borramos los medidas_preventivas 
                var listaMedidasPreventivas = Datos.ListarMedidasPorID_CENTRO(id);
                foreach (medidas_preventivas medidas in listaMedidasPreventivas)
                {
                    Datos.EliminarMedidaPreventiva(medidas.id);
                }
                //borramos los riesgo_medidas 
                var listaRiesgosMedidas = Datos.ListarRiesgosMedidasPorID_CENTRO(id);
                foreach (riesgos_medidas medidas in listaRiesgosMedidas)
                {
                    Datos.EliminarRiesgoMedida(medidas.id);
                }
                //borramos los riesgo_medidas 
                var listaMedidasGenerales = Datos.ListarMedidasGeneralesPorID_CENTRO(id);
                foreach (medidas_generales medidas in listaMedidasGenerales)
                {
                    Datos.EliminarMedidaGeneral(medidas.id);
                }
                //borramos los Matriz_centro 
                Datos.EliminarMatrizCentroPorIdCentro(id);

                //borramos los parametrica_medidas 
                Datos.EliminarParametricaMedidasPorIdCentro(id);

                Session["EditarCentralesResultado"] = "Central eliminada";
                return RedirectToAction("centros", "Configuracion");

            }
            catch (Exception e)
            {
                return null;
            }
        }

        public ActionResult eliminarImagenMedidas(string id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }


            Datos.EliminarMedidaGeneralImagenes(int.Parse(id));


            return RedirectToAction("editar_medida_general/" + id, "Configuracion");
        }

        public ActionResult eliminarImagenPrevia(string id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            medidaspreventivas_imagenes idmedida = Datos.ObtenerMedidaPorIdImagenMedida(int.Parse(id));
            Datos.EliminarMedidaPreventivaImagen(int.Parse(id));

            ViewData["imagenMedidaEliminada"] = idmedida.id_medida;

            return RedirectToAction("editar_medida_preventiva/" + idmedida.id_medida, "Configuracion");
            //return RedirectToAction("editar_medida_preventiva/", "Configuracion", new {id});

        }

        public ActionResult eliminar_icono(string idmedida)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            if (Session["idmedida"] != null)
            {
                idmedida = Session["idmedida"].ToString();
            }

            var tipoI = 0;

            Datos.EliminarRutaImagenPorId_Y_TipoImagen(int.Parse(idmedida), tipoI);

            Session.Remove("idmedida");

            return RedirectToAction("editar_riesgo_medida/" + idmedida, "Configuracion");
        }

        public ActionResult eliminar_ImagenGrande(string idmedida)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            if (Session["idmedida"] != null)
            {
                idmedida = Session["idmedida"].ToString();
            }

            var tipoI = 1;

            Datos.EliminarRutaImagenPorId_Y_TipoImagen(int.Parse(idmedida), tipoI);

            Session.Remove("idmedida");

            return RedirectToAction("editar_riesgo_medida/" + idmedida, "Configuracion");
        }


        public ActionResult eliminar_medida_general(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            //eliminar_imagen_medida_general(id);
            Datos.EliminarMedidaGeneral(id);

            Session["EditarMedidaResultado"] = "Medida eliminada";
            return RedirectToAction("medidas_generales", "Configuracion");
        }

        public void eliminar_imagen_medida_general(int id)
        {
            //tiene imagen?
            var rutaParaBorrarContent = Datos.EliminarMedidaGeneralImagenes(id);

            if (rutaParaBorrarContent != null)
            {

                rutaParaBorrarContent = rutaParaBorrarContent.Replace("..", "");

                string rutaSitio = Server.MapPath("~/");
                string rutaImagen = Path.Combine(rutaSitio + rutaParaBorrarContent);
                if ((System.IO.File.Exists(rutaImagen)))
                {
                    System.IO.File.Delete(rutaImagen);
                }
            }
            Session["ImagenEliminada"] = "imagenEliminada";
        }



        public ActionResult eliminar_medida_preventiva(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            Datos.EliminarMedidaSituacion(id);
            Session["EditarMedidaResultado"] = "Medida eliminada";
            return RedirectToAction("medidas_preventivas", "Configuracion");
        }

        public ActionResult eliminar_persona(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            Datos.EliminarPersona(id);

            Alert("Registro Persona eliminada", NotificationType.success, 2000);

            //Session["EditarMedidaResultado"] = "Medida eliminada";
            return RedirectToAction("personas", "Configuracion");
        }


        public ActionResult eliminar_equipo(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            var Sistema = Datos.ObtenerEquipoPorID(id);
            Datos.EliminarEquipo(id);

            Session["EditarEquiposResultado"] = "Equipo eliminado";

            return RedirectToAction("editar_sistema/" + Sistema.id_areanivel2, "Configuracion");
        }

        public ActionResult eliminar_area(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            var Centro = Datos.ObtenerCentroPorIdArea(id);
            Datos.EliminarArea(id);

            Session["EditarSistemaResultado"] = "Area eliminado";

            return RedirectToAction("editar_central/" + Centro, "Configuracion");
        }

        public ActionResult eliminar_sistema(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            var Area = Datos.ObtenerAreaPorID(id);
            Datos.EliminarSistema(id);

            Session["EditarEquiposResultado"] = "Equipo eliminado";

            return RedirectToAction("editar_area/" + Area.id_centro, "Configuracion");
        }

        [HttpPost]
        public ActionResult nuevo_equipo(string codigo, string nombre)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }
            int ids = (int)ViewData["idSistema"];
            var Sistema = Datos.ObtenerEquipoPorID(ids);
            areanivel3 actualizar = new areanivel3();
            actualizar.id_areanivel2 = ids;
            actualizar.nombre = "nombre";
            actualizar.codigo = "codigo";
            Datos.ActualizarEquipo(actualizar);

            Session["EditarEquiposResultado"] = "Equipo eliminado";

            return RedirectToAction("editar_sistema/" + Sistema.id_areanivel2, "Configuracion");
        }

        public string CleanInput(string strIn)
        {
            // Replace invalid characters with empty strings.
            try
            {
                return Regex.Replace(strIn, @"[^\w\.@-]", " ",
                                     RegexOptions.None, TimeSpan.FromSeconds(1.5));
            }
            // If we timeout when replacing invalid characters,
            // we should return Empty.
            catch (RegexMatchTimeoutException)
            {
                return String.Empty;
            }
        }

        #region usuarios

        public ActionResult usuarios()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            if (Session["CentralElegida"] != null)
            {
                centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
                //if (centroseleccionado.tipo != 4)
                //  ViewData["usuarios"] = Datos.ListarUsuarios(int.Parse(Session["CentralElegida"].ToString()));
                //  else
            }
            ViewData["usuarios"] = Datos.ListarUsuarios();

            return View();
        }

        public ActionResult editar_usuario(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            ViewData["idUsuario"] = id;
            Session["idUsuarioEdicion"] = id;
            VISTA_ObtenerUsuario buscarUsuario = Datos.ObtenerUsuarioVista(id);
            ViewData["EditarUsuario"] = buscarUsuario;
            VISTA_ObtenerUsuario user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());
            centros centroseleccionado = new MIDAS.Models.centros();
            //centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            //ViewData["centrosasignables"] = Datos.ListarCentrosAsignables(id, user, centroseleccionado);
            ViewData["centrosasignados"] = Datos.ListarCentrosAsignados(id);
            ViewData["unidades"] = Datos.ListarTecnologias();
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult editar_usuario(int id, FormCollection collection)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            ViewData["idUsuario"] = id;
            string formulario = collection["hdFormularioEjecutado"];

            if (formulario == "GuardarUsuario")
            {
                #region guardar usuario
                try
                {
                    VISTA_ObtenerUsuario actualizar = new VISTA_ObtenerUsuario();
                    actualizar.idUsuario = id;

                    if (collection["ctl00$MainContent$txtLogin"] != null)
                    {
                        actualizar.idUsuario = int.Parse(collection["ctl00$MainContent$hdnIdUsuario"]);
                        actualizar.nombre = collection["ctl00$MainContent$txtLogin"];
                        actualizar.password = collection["ctl00$MainContent$txtPassword"];
                        actualizar.perfil = int.Parse(collection["ctl00$MainContent$ddlPerfil"]);
                        actualizar.nombreap = collection["ctl00$MainContent$txtNombre"];
                        actualizar.mail = collection["ctl00$MainContent$txtMail"];
                        actualizar.telefono = collection["ctl00$MainContent$txtTelefono"];
                        actualizar.puesto = collection["ctl00$MainContent$txtPuesto"];
                        actualizar.idUnidad = int.Parse(collection["ctl00$MainContent$ddlTipo"]);

                        if (actualizar.idUsuario == 0)
                        {
                            VISTA_ObtenerUsuario comprobarUsuario = Datos.ObtenerUsuarioActual(actualizar.nombre);
                            if (comprobarUsuario != null)
                            {
                                VISTA_ObtenerUsuario user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());
                                centros centroseleccionado = new MIDAS.Models.centros();
                                centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
                                ViewData["centrosasignables"] = Datos.ListarCentrosAsignables(id, user, centroseleccionado);
                                ViewData["centrosasignados"] = Datos.ListarCentrosAsignados(id);
                                ViewData["unidades"] = Datos.ListarTecnologias();
                                Session["EdicionUsuarioError"] = "El usuario ya existe";
                                Session["UsuarioErroneo"] = actualizar;
                                return View();
                            }
                        }

                        if (actualizar.nombre == string.Empty)
                        {
                            VISTA_ObtenerUsuario user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());
                            centros centroseleccionado = new MIDAS.Models.centros();
                            centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
                            ViewData["centrosasignables"] = Datos.ListarCentrosAsignables(id, user, centroseleccionado);
                            ViewData["centrosasignados"] = Datos.ListarCentrosAsignados(id);
                            ViewData["unidades"] = Datos.ListarTecnologias();
                            Session["EdicionUsuarioError"] = "Debe introducir un nombre de usuario";
                            Session["UsuarioErroneo"] = actualizar;
                            return View();
                        }

                        if (collection["ctl00$MainContent$txtRepetir"] == null || actualizar.password != collection["ctl00$MainContent$txtRepetir"])
                        {
                            VISTA_ObtenerUsuario user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());
                            centros centroseleccionado = new MIDAS.Models.centros();
                            centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
                            ViewData["centrosasignables"] = Datos.ListarCentrosAsignables(id, user, centroseleccionado);
                            ViewData["centrosasignados"] = Datos.ListarCentrosAsignados(id);
                            ViewData["unidades"] = Datos.ListarTecnologias();
                            Session["EdicionUsuarioError"] = "Compruebe que las contraseñas coinciden";
                            Session["UsuarioErroneo"] = actualizar;
                            return View();
                        }

                        if (actualizar.password == string.Empty)
                        {
                            VISTA_ObtenerUsuario user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());
                            centros centroseleccionado = new MIDAS.Models.centros();
                            centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
                            ViewData["centrosasignables"] = Datos.ListarCentrosAsignables(id, user, centroseleccionado);
                            ViewData["centrosasignados"] = Datos.ListarCentrosAsignados(id);
                            ViewData["unidades"] = Datos.ListarTecnologias();
                            Session["EdicionUsuarioError"] = "Debe introducir una contraseña";
                            Session["UsuarioErroneo"] = actualizar;
                            return View();
                        }
                        int idUsuario = Datos.ActualizarUsuario(actualizar);

                        Session["EdicionUsuarioMensaje"] = "Los datos han sido modificados correctamente";
                        return RedirectToAction("editar_usuario/" + idUsuario, "Configuracion");
                    }
                    else
                    {
                        actualizar.nombre = collection["ctl00$MainContent$txtLogin"];
                        actualizar.password = collection["ctl00$MainContent$txtPassword"];
                        actualizar.perfil = int.Parse(collection["ctl00$MainContent$ddlPerfil"]);
                        actualizar.nombreap = collection["ctl00$MainContent$txtNombre"];
                        actualizar.mail = collection["ctl00$MainContent$txtMail"];
                        actualizar.telefono = collection["ctl00$MainContent$txtTelefono"];
                        actualizar.puesto = collection["ctl00$MainContent$txtPuesto"];
                        actualizar.idUnidad = int.Parse(collection["ctl00$MainContent$ddlTipo"]);
                        Session["UsuarioErroneo"] = actualizar;
                        Session["EdicionUsuarioError"] = "Debe introducir un nombre de usuario";
                        ViewData["unidades"] = Datos.ListarTecnologias();
                        return View();
                    }
                }
                catch (Exception ex)
                {
                    Session["EdicionUsuarioError"] = "FALLA" + ";" + ex.Message;
                    return RedirectToAction("usuarios", "Configuracion");
                }
                #endregion
            }
            if (formulario == "AsignarCentro")
            {
                #region asignar centro
                VISTA_ObtenerUsuario actualizar = new VISTA_ObtenerUsuario();
                actualizar.idUsuario = id;
                actualizar.nombre = collection["ctl00$MainContent$txtLogin"];
                actualizar.password = collection["ctl00$MainContent$txtPassword"];
                actualizar.perfil = int.Parse(collection["ctl00$MainContent$ddlPerfil"]);
                actualizar.nombreap = collection["ctl00$MainContent$txtNombre"];
                actualizar.mail = collection["ctl00$MainContent$txtMail"];
                actualizar.telefono = collection["ctl00$MainContent$txtTelefono"];
                actualizar.puesto = collection["ctl00$MainContent$txtPuesto"];
                actualizar.idUnidad = int.Parse(collection["ctl00$MainContent$ddlTipo"]);

                if (collection["ctl00$MainContent$txtPuesto"] != null)
                {
                    if (collection["ctl00$MainContent$ddlCentros"] != null)
                    {
                        int idCentro = int.Parse(collection["ctl00$MainContent$ddlCentros"]);
                        int Permiso = int.Parse(collection["ctl00$MainContent$ddlPermiso"]);
                        if (Permiso == 1)
                            Datos.AsociarUsuarioCentro(id, idCentro, true);
                        else
                            Datos.AsociarUsuarioCentro(id, idCentro, false);
                    }
                }

                Session["UsuarioErroneo"] = actualizar;
                VISTA_ObtenerUsuario user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());
                centros centroseleccionado = new MIDAS.Models.centros();
                centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
                ViewData["centrosasignables"] = Datos.ListarCentrosAsignables(id, user, centroseleccionado);
                ViewData["centrosasignados"] = Datos.ListarCentrosAsignados(id);
                ViewData["unidades"] = Datos.ListarTecnologias();
                Session["EdicionUsuarioMensaje"] = "Centro asignado correctamente";
                return View();
                #endregion
            }
            else
            {
                #region recarga
                VISTA_ObtenerUsuario actualizar = new VISTA_ObtenerUsuario();
                actualizar.idUsuario = id;
                actualizar.nombre = collection["ctl00$MainContent$txtLogin"];
                actualizar.password = collection["ctl00$MainContent$txtPassword"];
                actualizar.perfil = int.Parse(collection["ctl00$MainContent$ddlPerfil"]);
                actualizar.nombreap = collection["ctl00$MainContent$txtNombre"];
                actualizar.mail = collection["ctl00$MainContent$txtMail"];
                actualizar.telefono = collection["ctl00$MainContent$txtTelefono"];
                actualizar.puesto = collection["ctl00$MainContent$txtPuesto"];
                VISTA_ObtenerUsuario user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());
                centros centroseleccionado = new MIDAS.Models.centros();
                centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
                ViewData["centrosasignables"] = Datos.ListarCentrosAsignables(id, user, centroseleccionado);
                ViewData["centrosasignados"] = Datos.ListarCentrosAsignados(id);
                ViewData["unidades"] = Datos.ListarTecnologias();
                Session["UsuarioErroneo"] = actualizar;
                return View();
                #endregion
            }
        }

        public ActionResult eliminar_usuario(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            Datos.EliminarUsuario(id);
            Session["EditarUsuarioResultado"] = "ELIMINADOUSUARIO";
            return RedirectToAction("usuarios", "Configuracion");
        }

        public ActionResult eliminar_usuariocentro(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }
            int idUsuario = int.Parse(Session["idUsuarioEdicion"].ToString());
            Datos.EliminarUsuarioCentro(idUsuario, id);
            Session["EdicionUsuarioMensaje"] = "Centro eliminado correctamente";
            return RedirectToAction("editar_usuario/" + idUsuario, "Configuracion");
        }

        public ActionResult eliminar_parametrocentro(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }
            int idParametro = int.Parse(Session["idParametroEdicion"].ToString());
            Datos.EliminarParametroCentro(idParametro, id);
            Session["EdicionParametroMensaje"] = "Centro eliminado correctamente";
            return RedirectToAction("editar_parametro/" + idParametro, "Configuracion");
        }

        public ActionResult hab_usuario(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            Datos.HabilitarUsuario(id);
            Session["EditarUsuarioResultado"] = "HABILITADOUSUARIO";
            return RedirectToAction("usuarios", "Configuracion");
        }

        #endregion

        #region tiposdocumento

        public ActionResult documentos()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }
            ViewData["tiposdoc"] = Datos.ListarTiposDoc();
            return View();
        }

        public ActionResult editar_tipodoc(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            ViewData["idTipoDoc"] = id;

            tipodocumento buscarTipoDoc = Datos.ObtenerTipoDoc(id);
            ViewData["EditarTipoDoc"] = buscarTipoDoc;
            ViewData["tecnologias"] = Datos.ListarTecnologias();
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult editar_tipodoc(int id, FormCollection collection)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            ViewData["idTipoDoc"] = id;

            try
            {
                #region editar tipo documento
                tipodocumento actualizar = new tipodocumento();
                actualizar.id = id;

                if (collection["ctl00$MainContent$txtNombre"] != null)
                {
                    actualizar.id = int.Parse(collection["ctl00$MainContent$hdnIdDocumento"]);
                    actualizar.tipo = collection["ctl00$MainContent$txtNombre"];
                    if (collection["ctl00$MainContent$ddlNivel"].ToString() != "0")
                        actualizar.nivel = int.Parse(collection["ctl00$MainContent$ddlNivel"]);
                    if (collection["ctl00$MainContent$ddlTecnologia"].ToString() != "0")
                        actualizar.tecnologia = int.Parse(collection["ctl00$MainContent$ddlTecnologia"]);

                    if (actualizar.tipo == string.Empty)
                    {
                        tipodocumento buscarTipoDoc = Datos.ObtenerTipoDoc(id);
                        ViewData["EditarTipoDoc"] = buscarTipoDoc;
                        ViewData["tecnologias"] = Datos.ListarTecnologias();
                        Session["EdicionTipoDocError"] = "Debe introducir un nombre para el tipo de documento";
                        Session["TipoDocErroneo"] = actualizar;
                        return View();
                    }

                    int idTipoDoc = Datos.ActualizarTipoDoc(actualizar);

                    tipodocumento tipoDoc = Datos.ObtenerTipoDoc(id);
                    ViewData["EditarTipoDoc"] = tipoDoc;
                    ViewData["tecnologias"] = Datos.ListarTecnologias();

                    Session["EdicionTipoDocMensaje"] = "Los datos han sido modificados correctamente";
                    return RedirectToAction("editar_tipodoc/" + idTipoDoc, "Configuracion");
                }
                else
                {
                    #region recarga
                    actualizar.id = int.Parse(collection["ctl00$MainContent$hdnIdDocumento"]);
                    actualizar.tipo = collection["ctl00$MainContent$txtNombre"];
                    actualizar.nivel = int.Parse(collection["ctl00$MainContent$ddlNivel"]);
                    actualizar.tecnologia = int.Parse(collection["ctl00$MainContent$ddlTecnologia"]);
                    Session["TipoDocErroneo"] = actualizar;
                    Session["EdicionTipoDocError"] = "Debe introducir un nombre para el tipo de documento";
                    return View();
                    #endregion
                }
                #endregion
            }
            catch (Exception ex)
            {
                Session["EdicionTipoDocError"] = "FALLA" + ";" + ex.Message;
                return RedirectToAction("documentos", "Configuracion");
            }

        }

        public ActionResult eliminar_tipodoc(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            Datos.EliminarTipoDoc(id);
            Session["EditarTiposDocResultado"] = "Tipo de documento eliminado";
            return RedirectToAction("documentos", "Configuracion");
        }

        #endregion

        #region referenciales

        public ActionResult referenciales()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            ViewData["referenciales"] = Datos.ListarReferenciales();

            return View();
        }

        public ActionResult editar_referencial(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            ViewData["idReferencial"] = id;

            referenciales buscarReferencial = Datos.ObtenerReferencial(id);
            ViewData["EditarReferencial"] = buscarReferencial;
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult editar_referencial(int id, FormCollection collection)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            ViewData["idReferencial"] = id;

            try
            {
                #region añadir referencial
                referenciales actualizar = new referenciales();
                actualizar.id = id;

                if (collection["ctl00$MainContent$txtNombre"] != null)
                {
                    if (collection["ctl00$MainContent$hdnIdDocumento"] != null && collection["ctl00$MainContent$hdnIdDocumento"] != "")
                        actualizar.id = int.Parse(collection["ctl00$MainContent$hdnIdDocumento"]);
                    actualizar.nombre = collection["ctl00$MainContent$txtNombre"];

                    if (actualizar.nombre == string.Empty)
                    {
                        referenciales buscarTipoDoc = Datos.ObtenerReferencial(id);
                        ViewData["EditarReferencial"] = buscarTipoDoc;
                        Session["EdicionReferencialError"] = "Debe introducir un nombre para el referencial";
                        return View();
                    }

                    int idRef = Datos.ActualizarReferencial(actualizar);

                    referenciales buscarReferencial = Datos.ObtenerReferencial(idRef);
                    ViewData["EditarReferencial"] = buscarReferencial;

                    Session["EdicionReferencialMensaje"] = "Los datos han sido modificados correctamente";
                    return RedirectToAction("editar_referencial/" + idRef, "Configuracion");
                }
                else
                {
                    referenciales buscarReferencial = Datos.ObtenerReferencial(id);
                    ViewData["EditarReferencial"] = buscarReferencial;
                    return View();
                }
                #endregion

            }
            catch (Exception ex)
            {
                #region exception
                Session["EdicionReferencialError"] = "FALLA" + ";" + ex.Message;
                referenciales buscarTipoDoc = Datos.ObtenerReferencial(id);
                ViewData["EditarReferencial"] = buscarTipoDoc;
                return RedirectToAction("referenciales", "Configuracion");
                #endregion
            }

        }

        public ActionResult eliminar_referencial(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            Datos.EliminarReferencial(id);
            Session["EditarReferencialesResultado"] = "Referencial eliminado";
            return RedirectToAction("referenciales", "Configuracion");
        }

        #endregion

        #region stakeholders

        public ActionResult stakeholders()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }
            ViewData["stakeholdersN1"] = Datos.ListarStakeholdersN1();
            ViewData["stakeholdersN2"] = Datos.ListarStakeholdersN2();
            ViewData["stakeholdersN3"] = Datos.ListarStakeholdersN3();

            return View();
        }

        public ActionResult editar_stakeholdern3(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            ViewData["idStakeholderN3"] = id;

            ViewData["listashn2"] = Datos.ListarStakeholdersN2();
            stakeholders_nivel3 buscarStakeholder = Datos.ObtenerStakeholderN3(id);
            ViewData["EditarStakeholderN3"] = buscarStakeholder;
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult editar_stakeholdern3(int id, FormCollection collection)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            ViewData["idStakeholderN3"] = id;

            try
            {
                #region añadir stakeholder
                stakeholders_nivel3 actualizar = new stakeholders_nivel3();
                actualizar.id = id;

                if (collection["ctl00$MainContent$txtNombre"] != null)
                {

                    if (collection["ctl00$MainContent$hdnIdDocumento"] != null && collection["ctl00$MainContent$hdnIdDocumento"] != "")
                        actualizar.id = int.Parse(collection["ctl00$MainContent$hdnIdDocumento"]);
                    actualizar.denominacion = collection["ctl00$MainContent$txtNombre"];
                    actualizar.idnivel2 = int.Parse(collection["ctl00$MainContent$ddlSHN2"].ToString());
                    actualizar.necesidades = collection["ctl00$MainContent$txtNecesidades"];
                    actualizar.parteinteresada = int.Parse(collection["ctl00$MainContent$ddlRelevante"].ToString());

                    if (actualizar.denominacion == string.Empty)
                    {
                        ViewData["listashn2"] = Datos.ListarStakeholdersN2();
                        stakeholders_nivel3 buscarStakeholder = Datos.ObtenerStakeholderN3(id);
                        ViewData["EditarStakeholderN3"] = buscarStakeholder;
                        Session["EdicionStakeholderN3Error"] = "Debe introducir un nombre para el stakeholder";
                        return View();
                    }

                    int idRef = Datos.ActualizarStakeholderN3(actualizar);

                    ViewData["listashn2"] = Datos.ListarStakeholdersN2();
                    stakeholders_nivel3 buscarshn3 = Datos.ObtenerStakeholderN3(idRef);
                    ViewData["EditarStakeholderN3"] = buscarshn3;

                    Session["EdicionStakeholderN3Mensaje"] = "Los datos han sido modificados correctamente";
                    return RedirectToAction("editar_stakeholdern3/" + idRef, "Configuracion");
                }
                else
                {
                    ViewData["listashn2"] = Datos.ListarStakeholdersN2();
                    stakeholders_nivel3 buscarStakeholder = Datos.ObtenerStakeholderN3(id);
                    ViewData["EditarStakeholderN3"] = buscarStakeholder;
                    return View();
                }
                #endregion

            }
            catch (Exception ex)
            {
                #region exception
                Session["EdicionStakeholderN3Error"] = "FALLA" + ";" + ex.Message;
                ViewData["listashn2"] = Datos.ListarStakeholdersN2();
                stakeholders_nivel3 buscarStakeholder = Datos.ObtenerStakeholderN3(id);
                ViewData["EditarStakeholderN3"] = buscarStakeholder;
                return RedirectToAction("stakeholders", "Configuracion");
                #endregion
            }

        }

        public ActionResult editar_stakeholdern2(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            ViewData["idStakeholderN2"] = id;

            ViewData["listashn1"] = Datos.ListarStakeholdersN1();

            stakeholders_nivel2 buscarStakeholder = Datos.ObtenerStakeholderN2(id);
            ViewData["EditarStakeholderN2"] = buscarStakeholder;
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult editar_stakeholdern2(int id, FormCollection collection)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            ViewData["idStakeholderN2"] = id;

            try
            {
                #region añadir stakeholder
                stakeholders_nivel2 actualizar = new stakeholders_nivel2();
                actualizar.id = id;

                if (collection["ctl00$MainContent$txtNombre"] != null)
                {

                    if (collection["ctl00$MainContent$hdnIdDocumento"] != null && collection["ctl00$MainContent$hdnIdDocumento"] != "")
                        actualizar.id = int.Parse(collection["ctl00$MainContent$hdnIdDocumento"]);
                    actualizar.denominacion = collection["ctl00$MainContent$txtNombre"];
                    actualizar.idnivel1 = int.Parse(collection["ctl00$MainContent$ddlSHN1"].ToString());

                    if (actualizar.denominacion == string.Empty)
                    {
                        ViewData["listashn1"] = Datos.ListarStakeholdersN1();
                        stakeholders_nivel2 buscarStakeholder = Datos.ObtenerStakeholderN2(id);
                        ViewData["EditarStakeholderN2"] = buscarStakeholder;
                        Session["EdicionStakeholderN2Error"] = "Debe introducir un nombre para el stakeholder";
                        return View();
                    }

                    int idRef = Datos.ActualizarStakeholderN2(actualizar);

                    ViewData["listashn1"] = Datos.ListarStakeholdersN1();
                    stakeholders_nivel2 buscarshn2 = Datos.ObtenerStakeholderN2(idRef);
                    ViewData["EditarStakeholderN2"] = buscarshn2;

                    Session["EdicionStakeholderN2Mensaje"] = "Los datos han sido modificados correctamente";
                    return RedirectToAction("editar_stakeholdern2/" + idRef, "Configuracion");
                }
                else
                {
                    ViewData["listashn1"] = Datos.ListarStakeholdersN1();
                    stakeholders_nivel2 buscarStakeholder = Datos.ObtenerStakeholderN2(id);
                    ViewData["EditarStakeholderN2"] = buscarStakeholder;
                    return View();
                }
                #endregion

            }
            catch (Exception ex)
            {
                #region exception
                Session["EdicionStakeholderN2Error"] = "FALLA" + ";" + ex.Message;
                stakeholders_nivel2 buscarStakeholder = Datos.ObtenerStakeholderN2(id);
                ViewData["EditarStakeholderN2"] = buscarStakeholder;
                return RedirectToAction("stakeholders", "Configuracion");
                #endregion
            }

        }


        public ActionResult editar_stakeholdern1(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            ViewData["idStakeholderN1"] = id;

            stakeholders_nivel1 buscarStakeholder = Datos.ObtenerStakeholderN1(id);
            ViewData["EditarStakeholderN1"] = buscarStakeholder;
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult editar_stakeholdern1(int id, FormCollection collection)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            ViewData["idStakeholderN1"] = id;

            try
            {
                #region añadir stakeholder
                stakeholders_nivel1 actualizar = new stakeholders_nivel1();
                actualizar.id = id;

                if (collection["ctl00$MainContent$txtNombre"] != null)
                {
                    if (collection["ctl00$MainContent$hdnIdDocumento"] != null && collection["ctl00$MainContent$hdnIdDocumento"] != "")
                        actualizar.id = int.Parse(collection["ctl00$MainContent$hdnIdDocumento"]);
                    actualizar.denominacion = collection["ctl00$MainContent$txtNombre"];

                    if (actualizar.denominacion == string.Empty)
                    {
                        stakeholders_nivel1 buscarStakeholder = Datos.ObtenerStakeholderN1(id);
                        ViewData["EditarStakeholderN1"] = buscarStakeholder;
                        Session["EdicionStakeholderN1Error"] = "Debe introducir un nombre para el stakeholder";
                        return View();
                    }

                    int idRef = Datos.ActualizarStakeholderN1(actualizar);

                    stakeholders_nivel1 buscarReferencial = Datos.ObtenerStakeholderN1(idRef);
                    ViewData["EditarStakeholderN1"] = buscarReferencial;

                    Session["EdicionStakeholderN1Mensaje"] = "Los datos han sido modificados correctamente";
                    return RedirectToAction("editar_stakeholdern1/" + idRef, "Configuracion");
                }
                else
                {
                    stakeholders_nivel1 buscarStakeholder = Datos.ObtenerStakeholderN1(id);
                    ViewData["EditarStakeholderN1"] = buscarStakeholder;
                    return View();
                }
                #endregion

            }
            catch (Exception ex)
            {
                #region exception
                Session["EdicionStakeholderN1Error"] = "FALLA" + ";" + ex.Message;

                stakeholders_nivel1 buscarStakeholder = Datos.ObtenerStakeholderN1(id);
                ViewData["EditarStakeholderN1"] = buscarStakeholder;
                return RedirectToAction("stakeholders", "Configuracion");
                #endregion
            }

        }

        public ActionResult eliminar_stakeholdern1(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            Datos.EliminarStakeholderN1(id);
            Session["EditarStakeholderN1Resultado"] = "Stakeholder eliminado";
            return RedirectToAction("stakeholders", "Configuracion");
        }

        public ActionResult eliminar_stakeholdern2(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            Datos.EliminarStakeholderN2(id);
            Session["EditarStakeholderN2Resultado"] = "Stakeholder eliminado";
            return RedirectToAction("stakeholders", "Configuracion");
        }

        public ActionResult eliminar_stakeholdern3(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            Datos.EliminarStakeholderN3(id);
            Session["EditarStakeholderN3Resultado"] = "Stakeholder eliminado";
            return RedirectToAction("stakeholders", "Configuracion");
        }

        #endregion

        #region ambitos

        public ActionResult ambitos()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            ViewData["ambitos"] = Datos.ListarAmbitos();

            return View();
        }

        public ActionResult editar_ambito(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            ViewData["idAmbito"] = id;

            ambitos buscarAmbito = Datos.ObtenerAmbito(id);
            ViewData["EditarAmbito"] = buscarAmbito;
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult editar_ambito(int id, FormCollection collection)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            ViewData["idReferencial"] = id;

            try
            {
                #region añadir referencial
                ambitos actualizar = new ambitos();
                actualizar.id = id;

                if (collection["ctl00$MainContent$txtNombre"] != null)
                {
                    if (collection["ctl00$MainContent$hdnIdDocumento"] != null && collection["ctl00$MainContent$hdnIdDocumento"] != "")
                        actualizar.id = int.Parse(collection["ctl00$MainContent$hdnIdDocumento"]);
                    actualizar.nombre_ambito = collection["ctl00$MainContent$txtNombre"];

                    ambitos buscarAmbito = new ambitos();

                    if (actualizar.nombre_ambito == string.Empty)
                    {
                        buscarAmbito = Datos.ObtenerAmbito(id);
                        ViewData["EditarAmbito"] = buscarAmbito;
                        Session["EdicionAmbitoError"] = "Debe introducir un nombre para el ámbito";
                        return View();
                    }

                    int idRef = Datos.ActualizarAmbito(actualizar);

                    buscarAmbito = Datos.ObtenerAmbito(idRef);
                    ViewData["EditarAmbito"] = buscarAmbito;

                    Session["EdicionAmbitoMensaje"] = "Los datos han sido modificados correctamente";
                    return RedirectToAction("editar_ambito/" + idRef, "Configuracion");
                }
                else
                {
                    ambitos buscarAmbito = Datos.ObtenerAmbito(id);
                    ViewData["EditarAmbito"] = buscarAmbito;
                    return View();
                }
                #endregion

            }
            catch (Exception ex)
            {
                #region exception
                Session["EdicionAmbitoError"] = "FALLA" + ";" + ex.Message;
                ambitos buscarAmbito = Datos.ObtenerAmbito(id);
                ViewData["EditarAmbito"] = buscarAmbito;
                return RedirectToAction("ambitos", "Configuracion");
                #endregion
            }

        }

        public ActionResult eliminar_ambito(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            Datos.EliminarAmbito(id);
            Session["EditarAmbitosResultado"] = "Ámbito eliminado";
            return RedirectToAction("ambitos", "Configuracion");
        }

        #endregion

        #region indicadores
        public ActionResult indicadores()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            ViewData["indicadores"] = Datos.ListarIndicadores();

            return View();
        }

        public ActionResult editar_indicador(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            ViewData["idIndicador"] = id;

            ViewData["procesos"] = Datos.ListarProcesos();
            ViewData["tecnologias"] = Datos.ListarTecnologias();
            ViewData["centralesAsignables"] = Datos.ListarCentrosAsignablesIndicador(id);
            ViewData["centralesAsignadas"] = Datos.ListarCentrosAsignadosIndicador(id);

            ViewData["parametros"] = Datos.ListarParametrosInd().OrderBy(x => x.indicador);
            indicadores buscarIndicador = Datos.ObtenerIndicador(id);
            ViewData["EditarIndicador"] = buscarIndicador;
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult editar_indicador(int id, FormCollection collection)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            ViewData["idIndicador"] = id;
            int centralElegida = int.Parse(Session["CentralElegida"].ToString());

            try
            {
                #region añadir indicador
                indicadores actualizar = new indicadores();
                actualizar.Id = id;

                if (collection["ctl00$MainContent$txtNombre"] != null)
                {

                    if (collection["ctl00$MainContent$hdnIdIndicador"] != null && collection["ctl00$MainContent$hdnIdIndicador"] != "")
                        actualizar.Id = int.Parse(collection["ctl00$MainContent$hdnIdIndicador"]);
                    actualizar.Nombre = collection["ctl00$MainContent$txtNombre"];
                    actualizar.Descripcion = collection["ctl00$MainContent$txtDescripcion"];
                    actualizar.MetodoMedicion = collection["ctl00$MainContent$txtMetodo"];
                    actualizar.Unidad = collection["ctl00$MainContent$txtUnidad"];
                    actualizar.ProcesoAsociado = 1;
                    actualizar.tecnologia = int.Parse(collection["ctl00$MainContent$ddlTecnologia"].ToString());
                    actualizar.Periodicidad = collection["ctl00$MainContent$ddlFrecuenciaExp"].ToString();
                    actualizar.tendencia = int.Parse(collection["ctl00$MainContent$ddlTendencia"].ToString());

                    if (collection["ctl00$MainContent$ddlEspecifico"] != null)
                        actualizar.especifico = int.Parse(collection["ctl00$MainContent$ddlEspecifico"]);

                    if (collection["ctl00$MainContent$ddlValorNumerico"] != null && collection["ctl00$MainContent$ddlValorNumerico"] == "0")
                    {
                        actualizar.ValorNumerico = true;
                    }
                    else
                    {
                        actualizar.ValorNumerico = false;

                        if (int.Parse(collection["ctl00$MainContent$ddlTipoOperador1"]) == 0)
                            actualizar.Operador1 = int.Parse(collection["ctl00$MainContent$ddlOperador1"].ToString());
                        else
                            actualizar.Operador1Constante = decimal.Parse(collection["ctl00$MainContent$txtOperador1"].ToString());
                        if ((collection["ctl00$MainContent$ddlOperacion1"] != null) && (collection["ctl00$MainContent$ddlOperacion1"] != "0"))
                        {
                            actualizar.Operacion1 = collection["ctl00$MainContent$ddlOperacion1"].ToString();
                            if (int.Parse(collection["ctl00$MainContent$ddlTipoOperador2"]) == 0)
                                actualizar.Operador2 = int.Parse(collection["ctl00$MainContent$ddlOperador2"].ToString());
                            else
                                actualizar.Operador2Constante = decimal.Parse(collection["ctl00$MainContent$txtOperador2"].ToString());
                        }
                        if ((collection["ctl00$MainContent$ddlOperacion2"] != null) && (collection["ctl00$MainContent$ddlOperacion2"] != "0"))
                        {
                            actualizar.Operacion2 = collection["ctl00$MainContent$ddlOperacion2"].ToString();
                            if (int.Parse(collection["ctl00$MainContent$ddlTipoOperador2"]) == 0)
                                actualizar.Operador3 = int.Parse(collection["ctl00$MainContent$ddlOperador3"].ToString());
                            else
                                actualizar.Operador3Constante = decimal.Parse(collection["ctl00$MainContent$txtOperador3"].ToString());
                        }

                    }

                    if (actualizar.Nombre == string.Empty)
                    {
                        ViewData["procesos"] = Datos.ListarProcesos();
                        ViewData["tecnologias"] = Datos.ListarTecnologias();
                        ViewData["parametros"] = Datos.ListarParametrosInd(centralElegida).OrderBy(x => x.indicador);
                        ViewData["centralesAsignables"] = Datos.ListarCentrosAsignablesIndicador(id);
                        ViewData["centralesAsignadas"] = Datos.ListarCentrosAsignadosIndicador(id);

                        indicadores buscarIndicador = actualizar;
                        ViewData["EditarIndicador"] = buscarIndicador;
                        Session["EdicionIndicadorError"] = "Debe introducir un nombre para el indicador";
                        return View();
                    }
                    else
                    {

                        int idRef = Datos.ActualizarIndicador(actualizar);

                        if (actualizar.especifico == 1)
                        {
                            string hdnCentros = collection["ctl00$MainContent$hdnCentrosSeleccionados"].ToString();

                            string[] arraycentros = hdnCentros.Split(new char[] { ';' });

                            //Datos.EliminarAsociacionIndicadorCentros(id);

                            if (arraycentros.Count() > 0)
                            {
                                for (int i = 0; i < arraycentros.Count() - 1; i++)
                                {
                                    Datos.AsociarIndicadorCentro(idRef, int.Parse(arraycentros[i]));
                                }
                            }
                        }

                        ViewData["procesos"] = Datos.ListarProcesos();
                        ViewData["tecnologias"] = Datos.ListarTecnologias();
                        ViewData["parametros"] = Datos.ListarParametrosInd(centralElegida).OrderBy(x => x.indicador);
                        ViewData["centralesAsignables"] = Datos.ListarCentrosAsignablesIndicador(id);
                        ViewData["centralesAsignadas"] = Datos.ListarCentrosAsignadosIndicador(id);

                        indicadores buscarIndicador = Datos.ObtenerIndicador(id);
                        ViewData["EditarIndicador"] = buscarIndicador;

                        Session["EdicionIndicadorMensaje"] = "Los datos han sido modificados correctamente";
                        return RedirectToAction("editar_indicador/" + idRef, "Configuracion");
                    }
                }
                else
                {
                    ViewData["procesos"] = Datos.ListarProcesos();
                    ViewData["tecnologias"] = Datos.ListarTecnologias();
                    ViewData["parametros"] = Datos.ListarParametrosInd(centralElegida).OrderBy(x => x.indicador);
                    ViewData["centralesAsignables"] = Datos.ListarCentrosAsignablesIndicador(id);
                    ViewData["centralesAsignadas"] = Datos.ListarCentrosAsignadosIndicador(id);

                    indicadores buscarIndicador = Datos.ObtenerIndicador(id);
                    ViewData["EditarIndicador"] = buscarIndicador;
                    return View();
                }
                #endregion
            }
            catch (Exception ex)
            {
                #region exception
                ViewData["procesos"] = Datos.ListarProcesos();
                ViewData["tecnologias"] = Datos.ListarTecnologias();
                ViewData["parametros"] = Datos.ListarParametrosInd(centralElegida).OrderBy(x => x.indicador);
                ViewData["centralesAsignables"] = Datos.ListarCentrosAsignablesIndicador(id);
                ViewData["centralesAsignadas"] = Datos.ListarCentrosAsignadosIndicador(id);

                indicadores buscarIndicador = Datos.ObtenerIndicador(id);
                ViewData["EditarIndicador"] = buscarIndicador;
                return RedirectToAction("indicadores", "Configuracion");
                #endregion
            }

        }

        public ActionResult eliminar_indicador(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            Datos.EliminarIndicador(id);
            Session["EditarIndicadoresResultado"] = "Indicador desactivado";
            return RedirectToAction("indicadores", "Configuracion");
        }

        public ActionResult activar_indicador(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            Datos.ActivarIndicador(id);
            Session["EditarIndicadoresResultado"] = "Indicador activado";
            return RedirectToAction("indicadores", "Configuracion");
        }
        #endregion

        #region tiposaccionmejora

        public ActionResult tiposaccmejora()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            ViewData["tiposaccmejora"] = Datos.ListarTiposAccionMejora();

            return View();
        }

        public ActionResult editar_tipoaccmejora(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            ViewData["idTipoAccMejora"] = id;

            tipo_accionesmejora buscarTipoAccionMejora = Datos.ObtenerTipoAccMejora(id);
            ViewData["EditarTipoAccMejora"] = buscarTipoAccionMejora;
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult editar_tipoaccmejora(int id, FormCollection collection)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            ViewData["idTipoAccMejora"] = id;

            try
            {
                #region añadir tipo
                tipo_accionesmejora actualizar = new tipo_accionesmejora();
                actualizar.id = id;

                if (collection["ctl00$MainContent$txtNombre"] != null)
                {
                    if (collection["ctl00$MainContent$hdnIdDocumento"] != null && collection["ctl00$MainContent$hdnIdDocumento"] != "")
                        actualizar.id = int.Parse(collection["ctl00$MainContent$hdnIdDocumento"]);
                    actualizar.nombre = collection["ctl00$MainContent$txtNombre"];

                    if (actualizar.nombre == string.Empty)
                    {
                        tipo_accionesmejora buscarTipoAccionMejora = Datos.ObtenerTipoAccMejora(id);
                        ViewData["EditarTipoAccMejora"] = buscarTipoAccionMejora;
                        Session["EdicionTipoAccMejoraError"] = "Debe introducir un nombre para el tipo";
                        return View();
                    }

                    int idTipoAcc = Datos.ActualizarTipoAccMejora(actualizar);

                    tipo_accionesmejora buscarTipo = Datos.ObtenerTipoAccMejora(idTipoAcc);
                    ViewData["EditarTipoAccMejora"] = buscarTipo;

                    Session["EdicionTipoAccMejoraMensaje"] = "Los datos han sido modificados correctamente";
                    return RedirectToAction("editar_tipoaccmejora/" + idTipoAcc, "Configuracion");
                }
                else
                {
                    tipo_accionesmejora buscarTipo = Datos.ObtenerTipoAccMejora(id);
                    ViewData["EditarTipoAccMejora"] = buscarTipo;
                    Session["EdicionTipoAccMejoraError"] = "Debe introducir un nombre para el tipo";
                    return View();
                }
                #endregion

            }
            catch (Exception ex)
            {
                #region exception
                Session["EdicionTipoAccMejoraError"] = "FALLA" + ";" + ex.Message;
                tipo_accionesmejora buscarTipo = Datos.ObtenerTipoAccMejora(id);
                ViewData["EditarTipoAccMejora"] = buscarTipo;
                return RedirectToAction("tiposaccmejora", "Configuracion");
                #endregion
            }

        }

        public ActionResult eliminar_tipoaccmejora(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            Datos.EliminarTipoAccMejora(id);
            Session["EditarTiposAccMejoraResultado"] = "Tipo eliminado";
            return RedirectToAction("tiposaccmejora", "Configuracion");
        }

        #endregion

        #region aspectos
        public ActionResult aspectos()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            ViewData["aspectos"] = Datos.ListarAspectos();

            #region generacion fichero
            if (Session["nombreArchivo"] != null && Session["nombreArchivo"].ToString() != "")
            {
                if (Session["nombreArchivo"].ToString().Contains("xlsx"))
                {
                    Response.ContentType = "application/msexcel";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=ExportacionAspectos.xlsx");
                    Response.TransmitFile(Session["nombreArchivo"].ToString());
                    Response.End();
                }
                Session["nombreArchivo"] = "";
            }
            #endregion

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult aspectos(FormCollection collection)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            List<VISTA_MaestroAspectos> aspectos = Datos.ListarAspectos();

            #region generacion fichero
            string sourceFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaPlantillaWord"] + "ExportacionAspectosMaestro.xlsx");
            string destinationFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaGeneracionWord"] + "ExportacionAspectosMaestro_" + DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + ".xlsx");

            Session["source"] = sourceFile;
            Session["destino"] = destinationFile;
            // Create a copy of the template file and open the copy 
            System.IO.File.Copy(sourceFile, destinationFile, true);

            #region impresion

            using (SpreadsheetDocument document = SpreadsheetDocument.Open(destinationFile, true))
            {
                SheetData sheetData = document.WorkbookPart.WorksheetParts.ElementAt(0).Worksheet.Elements<SheetData>().First();
                foreach (VISTA_MaestroAspectos asp in aspectos)
                {
                    #region crear campos y celdas
                    Row row = new Row();

                    #region declaracion campos
                    string grupo = string.Empty;
                    string identificacion = string.Empty;
                    string elemento = string.Empty;
                    string unidad = string.Empty;

                    #endregion
                    #region asignacion campos
                    if (asp.grupo == null)
                        grupo = string.Empty;
                    else
                        grupo = asp.grupo;
                    if (asp.Codigo == null)
                        identificacion = string.Empty;
                    else
                        identificacion = asp.Codigo;
                    if (asp.Nombre == null)
                        elemento = string.Empty;
                    else
                        elemento = asp.Nombre;
                    if (asp.Unidad == null)
                        unidad = string.Empty;
                    else
                        unidad = asp.Unidad;

                    #endregion

                    #region construccion fila
                    row.Append(
                     Datos.ConstructCell(identificacion, CellValues.String),
                    Datos.ConstructCell(grupo, CellValues.String),
                    Datos.ConstructCell(elemento, CellValues.String),
                    Datos.ConstructCell(unidad, CellValues.String));
                    #endregion
                    sheetData.AppendChild(row);
                    #endregion
                }

                // save worksheet
                document.WorkbookPart.WorksheetParts.First().Worksheet.Save();

                document.WorkbookPart.Workbook.Save();

                Session["nombreArchivo"] = destinationFile;
            }
            #endregion
            #endregion

            ViewData["aspectos"] = Datos.ListarAspectos();

            return RedirectToAction("aspectos", "Configuracion");

        }

        public ActionResult editar_tipoaspecto(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            ViewData["idAspecto"] = id;

            aspecto_tipo buscarAspecto = Datos.ObtenerTipoAspecto(id);
            ViewData["EditarAspecto"] = buscarAspecto;
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult editar_tipoaspecto(int id, FormCollection collection)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            ViewData["idAspecto"] = id;
            string formulario = collection["hdFormularioEjecutado"];
            try
            {
                if (formulario == "GuardarTipoAspecto")
                {
                    #region añadir indicador
                    aspecto_tipo actualizar = new aspecto_tipo();
                    actualizar.id = id;



                    if (collection["ctl00$MainContent$hdnIdAspecto"] != null && collection["ctl00$MainContent$hdnIdAspecto"] != "")
                        actualizar.id = int.Parse(collection["ctl00$MainContent$hdnIdAspecto"]);
                    actualizar.Grupo = int.Parse(collection["ctl00$MainContent$ddlGrupo"]);
                    if (collection["ctl00$MainContent$ddlPeligroso"] != null)
                        actualizar.RE_Peligroso = int.Parse(collection["ctl00$MainContent$ddlPeligroso"]);
                    actualizar.Nombre = collection["ctl00$MainContent$txtIdentificacion"];
                    actualizar.Impacto = collection["ctl00$MainContent$txtImpacto"];
                    actualizar.Descripcion = collection["ctl00$MainContent$txtDescripcion"];
                    actualizar.Unidad = collection["ctl00$MainContent$txtUnidad"];
                    if (collection["ctl00$MainContent$rdbRelativo"] != null)
                    {
                        string valorRelativo = collection["ctl00$MainContent$rdbRelativo"];

                        switch (valorRelativo)
                        {
                            case "Mwhb":
                                actualizar.relativoMwhb = true;
                                break;
                            case "hAnioGE":
                                actualizar.relativohAnioGE = true;
                                break;
                            case "kmAnio":
                                actualizar.relativokmAnio = true;
                                break;
                            case "m3Hora":
                                actualizar.relativom3Hora = true;
                                break;
                            case "hfuncAnio":
                                actualizar.relativohfuncAnio = true;
                                break;
                        }
                    }
                    else
                    {
                        actualizar.relativoMwhb = false;
                        actualizar.relativohAnioGE = false;
                        actualizar.relativokmAnio = false;
                        actualizar.relativom3Hora = false;
                        actualizar.relativohfuncAnio = false;
                    }

                    if (actualizar.Nombre == string.Empty)
                    {
                        aspecto_tipo buscarAspecto = actualizar;
                        ViewData["EditarAspecto"] = buscarAspecto;
                        Session["EdicionIndicadorError"] = "Debe introducir una identificación para el indicador";
                        return View();
                    }
                    else
                    {

                        int idRef = Datos.ActualizarTipoAspecto(actualizar);

                        aspecto_tipo buscarAspecto = Datos.ObtenerTipoAspecto(idRef);
                        ViewData["EditarAspecto"] = buscarAspecto;

                        Session["EdicionTipoAspectoMensaje"] = "Los datos han sido modificados correctamente";
                        return RedirectToAction("editar_tipoaspecto/" + idRef, "Configuracion");
                    }


                    #endregion
                }
                else
                {
                    #region actualizar
                    aspecto_tipo actualizar = new aspecto_tipo();
                    if (collection["ctl00$MainContent$hdnIdAspecto"] != null && collection["ctl00$MainContent$hdnIdAspecto"] != "")
                        actualizar.id = int.Parse(collection["ctl00$MainContent$hdnIdAspecto"]);
                    actualizar.Grupo = int.Parse(collection["ctl00$MainContent$ddlGrupo"]);
                    if (collection["ctl00$MainContent$ddlPeligroso"] != null)
                        actualizar.RE_Peligroso = int.Parse(collection["ctl00$MainContent$ddlPeligroso"]);
                    actualizar.Nombre = collection["ctl00$MainContent$txtIdentificacion"];
                    actualizar.Impacto = collection["ctl00$MainContent$txtImpacto"];
                    actualizar.Descripcion = collection["ctl00$MainContent$txtDescripcion"];
                    actualizar.Unidad = collection["ctl00$MainContent$txtUnidad"];
                    if (collection["ctl00$MainContent$rdbRelativo"] != null)
                    {
                        string valorRelativo = collection["ctl00$MainContent$rdbRelativo"];

                        switch (valorRelativo)
                        {
                            case "Mwhb":
                                actualizar.relativoMwhb = true;
                                break;
                            case "hAnioGE":
                                actualizar.relativohAnioGE = true;
                                break;
                            case "kmAnio":
                                actualizar.relativokmAnio = true;
                                break;
                            case "m3Hora":
                                actualizar.relativom3Hora = true;
                                break;
                            case "hfuncAnio":
                                actualizar.relativohfuncAnio = true;
                                break;
                        }
                    }
                    else
                    {
                        actualizar.relativoMwhb = false;
                        actualizar.relativohAnioGE = false;
                        actualizar.relativokmAnio = false;
                        actualizar.relativom3Hora = false;
                        actualizar.relativohfuncAnio = false;
                    }

                    aspecto_tipo buscarAspecto = actualizar;
                    ViewData["EditarAspecto"] = buscarAspecto;
                    return View();
                    #endregion
                }
            }
            catch (Exception ex)
            {
                #region exception
                aspecto_tipo buscarAspecto = Datos.ObtenerTipoAspecto(id);
                ViewData["EditarAspecto"] = buscarAspecto;
                return RedirectToAction("aspectos", "Configuracion");
                #endregion
            }

        }

        public ActionResult eliminar_aspecto(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            Datos.EliminarAspectoTipo(id);
            Session["EditarAspectosResultado"] = "Aspecto desactivado";
            return RedirectToAction("aspectos", "Configuracion");
        }

        #endregion

        #region Parametros ambientales

        public ActionResult aspectosParametros()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }
            ViewData["aspectosParametros"] = Datos.getParametrosAmbientalesInv();

            return View();
        }

        public ActionResult editar_aspectoTipo(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            ViewData["parametrosAmbientales"] = Datos.getParametrosAmbientalesInv(id);

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult editar_aspectoTipo(int id, FormCollection collection)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            string formulario = collection["hdFormularioEjecutado"];

            if (formulario == "GuardarAspectoTipo")
            {
                #region Guardar Aspecto
                try
                {

                    aspecto_parametro actualizar = Datos.getParametrosAmbientalesInv(id);

                    if (actualizar != null)
                    {
                        actualizar.id_aspecto = int.Parse(collection["ctl00$MainContent$ddlTipo"].ToString());
                        actualizar.nombre = collection["ctl00$MainContent$txtNombre"].ToString();

                        Datos.ActualizarParametroAmbiental(actualizar);

                        ViewData["parametrosAmbientales"] = Datos.getParametrosAmbientalesInv(id);
                        return View();
                    }
                    else
                    {
                        aspecto_parametro insertar = new aspecto_parametro();

                        insertar.id_aspecto = int.Parse(collection["ctl00$MainContent$ddlTipo"].ToString());
                        insertar.nombre = collection["ctl00$MainContent$txtNombre"].ToString();

                        int idAspecto = Datos.ActualizarParametroAmbiental(insertar);

                        //REDIRECCIONAR CORRECTAMENTE
                        ViewData["parametrosAmbientales"] = Datos.getParametrosAmbientalesInv(id);
                        return Redirect(Url.RouteUrl(new { controller = "Configuracion", action = "editar_aspectoTipo", id = idAspecto }));
                    }
                }
                catch (Exception ex)
                {
                    ViewData["parametrosAmbientales"] = Datos.getParametrosAmbientalesInv(id);

                    Session["EdicionParametroError"] = "No se han podido guardar los datos, compruebe que los datos introducidos son correctos.";
                    return View();
                }
            }
            return null;
            #endregion
        }

        public ActionResult eliminar_aspectoTipo(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            Datos.eliminarParametroAmbiental(id);
            Session["EditarAuditoresResultado"] = "Parametro eliminado";
            return RedirectToAction("aspectosParametros", "Configuracion");
        }



        #endregion

        #region programaauditoria

        public ActionResult editar_programaaudit()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }
            ViewData["programaauditoria"] = Datos.GetProgramaAuditoria(1);

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult editar_programaaudit(HttpPostedFileBase file)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }
            auditorias_programa IF = Datos.GetProgramaAuditoria(1);
            #region guardar programa auditoria
            try
            {
                #region actualizar

                if ((file != null && file.ContentLength > 0))
                {
                    var fileName = System.IO.Path.GetFileName(file.FileName);

                    var path = System.IO.Path.Combine(Server.MapPath("~/ProgramaAuditorias"), fileName);
                    IF.rutaFichero = path;
                    IF.nombrefichero = fileName;

                    Datos.UpdateProgramaAuditoriaGeneral(IF);

                    if (Directory.Exists(Server.MapPath("~/ProgramaAuditorias")))
                    {
                        file.SaveAs(path);
                    }
                    else
                    {
                        Directory.CreateDirectory(Server.MapPath("~/ProgramaAuditorias"));
                        file.SaveAs(path);
                    }
                }

                Session["EdicionProgramaAuditoriaMensaje"] = "Información actualizada correctamente";
                ViewData["programaauditoria"] = Datos.GetProgramaAuditoria(1);

                return View();
                #endregion
            }
            catch (Exception ex)
            {
                Session["EdicionProgramaAuditoriaError"] = "Se ha producido un error";
                ViewData["programaauditoria"] = Datos.GetProgramaAuditoria(1);

                return View();
            }
            #endregion
        }

        public FileResult ObtenerProgramaAuditoria()
        {
            try
            {
                auditorias_programa IF = Datos.GetProgramaAuditoria(1);
                byte[] fileBytes = System.IO.File.ReadAllBytes(IF.rutaFichero);
                string fileName = IF.nombrefichero;
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion

        #region programaDocs

        public ActionResult editar_programaDocs()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }
            ViewData["programaDocs"] = Datos.GetProgramaDocumentos(1);

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult editar_programaDocs(HttpPostedFileBase file)
        {

            documentos_programa IF = null;
            if (Datos.GetProgramaDocumentos(1) != null)
            {
                IF = Datos.GetProgramaDocumentos(1);
            }
            else
            {
                IF = new documentos_programa();
                IF.nombrefichero = "";
                IF.rutaFichero = "";
            }

            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }
            else

                #region guardar programa auditoria
                try
                {

                    #region actualizar

                    if ((file != null && file.ContentLength > 0))
                    {
                        var fileName = System.IO.Path.GetFileName(file.FileName);

                        var path = System.IO.Path.Combine(Server.MapPath("~/ProgramaDocumentacion/"), fileName);
                        IF.rutaFichero = path;
                        IF.nombrefichero = fileName;

                        Datos.UpdateProgramaDocumentacionGeneral(IF);

                        if (Directory.Exists(Server.MapPath("~/ProgramaDocumentacion/")))
                        {
                            file.SaveAs(path);
                        }
                        else
                        {
                            Directory.CreateDirectory(Server.MapPath("~/ProgramaDocumentacion"));
                            file.SaveAs(path);
                        }
                    }

                    Session["EdicionProgramaDocumentosMensaje"] = "Información actualizada correctamente";
                    ViewData["programaDocs"] = Datos.GetProgramaDocumentos(1);

                    return View();
                    #endregion
                }
                catch (Exception ex)
                {
                    Session["EditarProgramaDocumentosError"] = "Se ha producido un error";
                    ViewData["programaDocs"] = Datos.GetProgramaDocumentos(1);

                    return View();
                }
            #endregion
        }

        public FileResult ObtenerProgramaDocumentos()
        {
            try
            {
                documentos_programa IF = Datos.GetProgramaDocumentos(1);
                byte[] fileBytes = System.IO.File.ReadAllBytes(IF.rutaFichero);
                string fileName = IF.nombrefichero;
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion
        #region auditores
        public ActionResult auditores()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            ViewData["auditores"] = Datos.ListarAuditores();

            return View();
        }

        public ActionResult eliminar_auditor(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            Datos.EliminarAuditor(id);
            Session["EditarAuditoresResultado"] = "Referencial eliminado";
            return RedirectToAction("auditores", "Configuracion");
        }

        public ActionResult editar_auditor(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            ViewData["idAuditor"] = id;

            auditores buscarAuditor = Datos.GetDatosAuditor(id);
            ViewData["EditarAuditor"] = buscarAuditor;
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult editar_auditor(int id, FormCollection collection, HttpPostedFileBase file)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            ViewData["idAuditor"] = id;

            try
            {
                #region añadir auditor
                auditores actualizar = new auditores();
                actualizar.id = id;

                if (collection["ctl00$MainContent$txtNombre"] != null && collection["ctl00$MainContent$txtEmpresa"] != null)
                {
                    if (collection["ctl00$MainContent$hdnIdDocumento"] != null && collection["ctl00$MainContent$hdnIdDocumento"] != "")
                        actualizar.id = int.Parse(collection["ctl00$MainContent$hdnIdDocumento"]);
                    actualizar.nombre = collection["ctl00$MainContent$txtNombre"];
                    actualizar.empresa = collection["ctl00$MainContent$txtEmpresa"];

                    if (actualizar.nombre == string.Empty)
                    {
                        auditores buscarAuditor = Datos.GetDatosAuditor(id);
                        ViewData["EditarAuditor"] = buscarAuditor;
                        Session["EdicionAuditorError"] = "Debe introducir un nombre para el auditor";
                        return View();
                    }
                    else
                    {

                        int idAud = Datos.ActualizarAuditor(actualizar);

                        if ((file != null && file.ContentLength > 0))
                        {
                            var fileName = System.IO.Path.GetFileName(file.FileName);

                            var path = System.IO.Path.Combine(Server.MapPath("~/Auditores/" + idAud.ToString()), fileName);
                            actualizar.cv = path;

                            actualizar.id = idAud;
                            Datos.UpdateCualificacionAuditor(actualizar);

                            if (Directory.Exists(Server.MapPath("~/Auditores/" + idAud.ToString())))
                            {
                                file.SaveAs(path);
                            }
                            else
                            {
                                Directory.CreateDirectory(Server.MapPath("~/Auditores/" + idAud.ToString()));
                                file.SaveAs(path);
                            }
                        }

                        auditores buscarAuditor = Datos.GetDatosAuditor(idAud);
                        ViewData["EditarAuditor"] = buscarAuditor;

                        Session["EdicionAuditorMensaje"] = "Los datos han sido modificados correctamente";
                        return RedirectToAction("editar_auditor/" + idAud, "Configuracion");
                    }
                }
                else
                {
                    auditores buscarAuditor = Datos.GetDatosAuditor(id);
                    ViewData["EditarAuditor"] = buscarAuditor;
                    return View();
                }
                #endregion
            }
            catch (Exception ex)
            {
                #region exception
                Session["EdicionAuditorError"] = "FALLA" + ";" + ex.Message;
                auditores buscarAuditor = Datos.GetDatosAuditor(id);
                ViewData["EditarAuditor"] = buscarAuditor;
                return RedirectToAction("editar_auditor", "Configuracion");
                #endregion
            }
        }

        public FileResult ObtenerCualificacion(int id)
        {
            try
            {
                auditores IF = Datos.GetDatosAuditor(id);
                byte[] fileBytes = System.IO.File.ReadAllBytes(IF.cv);
                string fileName = IF.cv.Replace(Server.MapPath("~/Auditores") + "\\" + IF.id + "\\", "");
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion

        #region riesgos
        public ActionResult tipos_riesgos()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            ViewData["tipos_riesgos"] = Datos.ListarTiposRiesgos();

            return View();
        }
        public ActionResult catriesgo()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            ViewData["categorias"] = Datos.ListarCategoriasRiesgo();

            return View();
        }

        public ActionResult eliminar_catriesgo(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            Datos.EliminarCatRiesgo(id);
            Session["EditarCategoriasRiesgoResultado"] = "Categoría eliminada";
            return RedirectToAction("catriesgo", "Configuracion");
        }

        public ActionResult editar_catriesgo(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            ViewData["idCatriesgo"] = id;

            riesgos_categorias buscarCanal = Datos.ObtenerCategoriaRiesgo(id);
            ViewData["EditarCatriesgo"] = buscarCanal;
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult editar_catriesgo(int id, FormCollection collection)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            ViewData["idCatriesgo"] = id;

            try
            {
                #region añadir categoria
                riesgos_categorias actualizar = new riesgos_categorias();
                actualizar.id = id;

                if (collection["ctl00$MainContent$txtNombre"] != null)
                {
                    if (collection["ctl00$MainContent$hdnIdDocumento"] != null && collection["ctl00$MainContent$hdnIdDocumento"] != "")
                        actualizar.id = int.Parse(collection["ctl00$MainContent$hdnIdDocumento"]);
                    actualizar.categoria = collection["ctl00$MainContent$txtNombre"];

                    if (actualizar.categoria == string.Empty)
                    {
                        riesgos_categorias buscarCanal = Datos.ObtenerCategoriaRiesgo(id);
                        ViewData["EditarCatriesgo"] = buscarCanal;
                        Session["EdicionCatriesgoError"] = "Debe introducir un nombre para la categoría";
                        return View();
                    }
                    else
                    {

                        int idRef = Datos.ActualizarCatriesgo(actualizar);

                        riesgos_categorias buscarCanal = Datos.ObtenerCategoriaRiesgo(idRef);
                        ViewData["EditarCatriesgo"] = buscarCanal;

                        Session["EdicionCatriesgoMensaje"] = "Los datos han sido modificados correctamente";
                        return RedirectToAction("editar_catriesgo/" + idRef, "Configuracion");
                    }
                }
                else
                {
                    riesgos_categorias buscarCanal = Datos.ObtenerCategoriaRiesgo(id);
                    ViewData["EditarCatriesgo"] = buscarCanal;
                    return View();
                }
                #endregion

            }
            catch (Exception ex)
            {
                #region exception
                Session["EdicionCatriesgoError"] = "FALLA" + ";" + ex.Message;
                riesgos_categorias buscarCanal = Datos.ObtenerCategoriaRiesgo(id);
                ViewData["EditarCatriesgo"] = buscarCanal;
                return RedirectToAction("catriesgo", "Configuracion");
                #endregion
            }

        }

        public ActionResult tipriesgo()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            ViewData["tipologias"] = Datos.ListarTipologiasRiesgo();

            return View();
        }

        public ActionResult eliminar_tipriesgo(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            Datos.EliminarTipRiesgo(id);
            Session["EditarTipologiasRiesgoResultado"] = "Categoría eliminada";
            return RedirectToAction("tipriesgo", "Configuracion");
        }

        public ActionResult editar_tipriesgo(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            ViewData["idTipriesgo"] = id;

            riesgos_tipologias buscarCanal = Datos.ObtenerTipologiaRiesgo(id);
            ViewData["EditarTipriesgo"] = buscarCanal;
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult editar_tipriesgo(int id, FormCollection collection)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            ViewData["idTipriesgo"] = id;

            try
            {
                #region añadir tipologia
                riesgos_tipologias actualizar = new riesgos_tipologias();
                actualizar.id = id;

                if (collection["ctl00$MainContent$txtNombre"] != null)
                {
                    if (collection["ctl00$MainContent$hdnIdDocumento"] != null && collection["ctl00$MainContent$hdnIdDocumento"] != "")
                        actualizar.id = int.Parse(collection["ctl00$MainContent$hdnIdDocumento"]);
                    actualizar.tipologia = collection["ctl00$MainContent$txtNombre"];

                    if (actualizar.tipologia == string.Empty)
                    {
                        riesgos_tipologias buscarCanal = Datos.ObtenerTipologiaRiesgo(id);
                        ViewData["EditarTipriesgo"] = buscarCanal;
                        Session["EdicionTipriesgoError"] = "Debe introducir un nombre para la tipología";
                        return View();
                    }
                    else
                    {

                        int idRef = Datos.ActualizarTipriesgo(actualizar);

                        riesgos_tipologias buscarCanal = Datos.ObtenerTipologiaRiesgo(idRef);
                        ViewData["EditarTipriesgo"] = buscarCanal;

                        Session["EdicionTipriesgoMensaje"] = "Los datos han sido modificados correctamente";
                        return RedirectToAction("editar_tipriesgo/" + idRef, "Configuracion");
                    }
                }
                else
                {
                    riesgos_tipologias buscarCanal = Datos.ObtenerTipologiaRiesgo(id);
                    ViewData["EditarTipriesgo"] = buscarCanal;
                    return View();
                }
                #endregion

            }
            catch (Exception ex)
            {
                #region exception
                Session["EdicionTipriesgoError"] = "FALLA" + ";" + ex.Message;
                riesgos_tipologias buscarCanal = Datos.ObtenerTipologiaRiesgo(id);
                ViewData["EditarTipriesgo"] = buscarCanal;
                return RedirectToAction("tipriesgo", "Configuracion");
                #endregion
            }

        }

        #endregion

        #region tipos comunicacion

        public ActionResult tiposcomunicacion()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            ViewData["tiposcomunicacion"] = Datos.ListarTiposComunicacion();

            return View();
        }

        public ActionResult eliminar_tipocomunicacion(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            Datos.EliminarTipoComunicacion(id);
            Session["EditarTiposComunicacionResultado"] = "Tipo eliminado";
            return RedirectToAction("tiposcomunicacion", "Configuracion");
        }

        public ActionResult editar_tipocomunicacion(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            ViewData["idTipoComunicacion"] = id;

            comunicacion_tipos buscarTipo = Datos.ObtenerTipoComunicacion(id);
            ViewData["EditarTipoComunicacion"] = buscarTipo;
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult editar_tipocomunicacion(int id, FormCollection collection)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            ViewData["idTipoComunicacion"] = id;

            try
            {
                #region añadir tipocomunicacion
                comunicacion_tipos actualizar = new comunicacion_tipos();
                actualizar.id = id;

                if (collection["ctl00$MainContent$txtNombre"] != null)
                {
                    if (collection["ctl00$MainContent$hdnIdDocumento"] != null && collection["ctl00$MainContent$hdnIdDocumento"] != "")
                        actualizar.id = int.Parse(collection["ctl00$MainContent$hdnIdDocumento"]);
                    actualizar.tipo = collection["ctl00$MainContent$txtNombre"];

                    if (actualizar.tipo == string.Empty)
                    {
                        comunicacion_tipos buscarTipoCom = Datos.ObtenerTipoComunicacion(id);
                        ViewData["EditarTipoComunicacion"] = buscarTipoCom;
                        Session["EdicionTipoComunicacionError"] = "Debe introducir un nombre para el tipo";
                        return View();
                    }
                    else
                    {

                        int idRef = Datos.ActualizarTipoComunicacion(actualizar);

                        comunicacion_tipos buscarTipoCom = Datos.ObtenerTipoComunicacion(id);
                        ViewData["EditarTipoComunicacion"] = buscarTipoCom;

                        Session["EdicionTipoComunicacionMensaje"] = "Los datos han sido modificados correctamente";
                        return RedirectToAction("editar_tipocomunicacion/" + idRef, "Configuracion");
                    }
                }
                else
                {
                    comunicacion_tipos buscarTipoCom = Datos.ObtenerTipoComunicacion(id);
                    ViewData["EditarTipoComunicacion"] = buscarTipoCom;
                    return View();
                }
                #endregion

            }
            catch (Exception ex)
            {
                #region exception
                Session["EdicionTipoComunicacionError"] = "FALLA" + ";" + ex.Message;
                comunicacion_tipos buscarTipoCom = Datos.ObtenerTipoComunicacion(id);
                ViewData["EditarTipoComunicacion"] = buscarTipoCom;
                return RedirectToAction("tiposcomunicacion", "Configuracion");
                #endregion
            }

        }

        #endregion

        #region clasificacion comunicacion

        public ActionResult clasifcomunicacion()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            ViewData["clasifcomunicacion"] = Datos.ListarClasifComunicacion();

            return View();
        }

        public ActionResult eliminar_clasifcomunicacion(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            Datos.EliminarClasifComunicacion(id);
            Session["EditarClasificacionesComunicacionResultado"] = "Clasificación eliminada";
            return RedirectToAction("clasifcomunicacion", "Configuracion");
        }

        public ActionResult editar_clasifcomunicacion(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            ViewData["idClasifComunicacion"] = id;

            comunicacion_clasificacion buscarTipo = Datos.ObtenerClasifComunicacion(id);
            ViewData["EditarClasifComunicacion"] = buscarTipo;
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult editar_clasifcomunicacion(int id, FormCollection collection)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            ViewData["idClasifComunicacion"] = id;

            try
            {
                #region añadir clasifcomunicacion
                comunicacion_clasificacion actualizar = new comunicacion_clasificacion();
                actualizar.id = id;

                if (collection["ctl00$MainContent$txtNombre"] != null)
                {
                    if (collection["ctl00$MainContent$hdnIdDocumento"] != null && collection["ctl00$MainContent$hdnIdDocumento"] != "")
                        actualizar.id = int.Parse(collection["ctl00$MainContent$hdnIdDocumento"]);
                    actualizar.tipo = collection["ctl00$MainContent$txtNombre"];

                    if (actualizar.tipo == string.Empty)
                    {
                        comunicacion_clasificacion buscarTipoCom = Datos.ObtenerClasifComunicacion(id);
                        ViewData["EditarClasifComunicacion"] = buscarTipoCom;
                        Session["EdicionClasificacionComunicacionError"] = "Debe introducir un nombre para la clasificación";
                        return View();
                    }
                    else
                    {

                        int idRef = Datos.ActualizarClasifComunicacion(actualizar);

                        comunicacion_clasificacion buscarTipoCom = Datos.ObtenerClasifComunicacion(id);
                        ViewData["EditarClasifComunicacion"] = buscarTipoCom;

                        Session["EdicionClasificacionComunicacionMensaje"] = "Los datos han sido modificados correctamente";
                        return RedirectToAction("editar_clasifcomunicacion/" + idRef, "Configuracion");
                    }
                }
                else
                {
                    comunicacion_clasificacion buscarTipoCom = Datos.ObtenerClasifComunicacion(id);
                    ViewData["EditarClasifComunicacion"] = buscarTipoCom;
                    return View();
                }
                #endregion

            }
            catch (Exception ex)
            {
                #region exception
                Session["EdicionClasificacionComunicacionError"] = "FALLA" + ";" + ex.Message;
                comunicacion_clasificacion buscarTipoCom = Datos.ObtenerClasifComunicacion(id);
                ViewData["EditarClasifComunicacion"] = buscarTipoCom;
                return RedirectToAction("clasifcomunicacion", "Configuracion");
                #endregion
            }

        }

        #endregion

        #region canal comunicacion

        public ActionResult canalescomunicacion()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            ViewData["canalescomunicacion"] = Datos.ListarCanalesComunicacion();

            return View();
        }

        public ActionResult eliminar_canalcomunicacion(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            Datos.EliminarCanalComunicacion(id);
            Session["EditarCanalesComunicacionResultado"] = "Canal eliminado";
            return RedirectToAction("canalescomunicacion", "Configuracion");
        }

        public ActionResult editar_canalcomunicacion(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            ViewData["idCanalComunicacion"] = id;

            comunicacion_canales buscarCanal = Datos.ObtenerCanalComunicacion(id);
            ViewData["EditarCanalComunicacion"] = buscarCanal;
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult editar_canalcomunicacion(int id, FormCollection collection)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            ViewData["idCanalComunicacion"] = id;

            try
            {
                #region añadir canal comunicacion
                comunicacion_canales actualizar = new comunicacion_canales();
                actualizar.id = id;

                if (collection["ctl00$MainContent$txtNombre"] != null)
                {
                    if (collection["ctl00$MainContent$hdnIdDocumento"] != null && collection["ctl00$MainContent$hdnIdDocumento"] != "")
                        actualizar.id = int.Parse(collection["ctl00$MainContent$hdnIdDocumento"]);
                    actualizar.canal = collection["ctl00$MainContent$txtNombre"];

                    if (actualizar.canal == string.Empty)
                    {
                        comunicacion_canales buscarCanal = Datos.ObtenerCanalComunicacion(id);
                        ViewData["EditarCanalComunicacion"] = buscarCanal;
                        Session["EdicionCanalComunicacionError"] = "Debe introducir un nombre para el canal";
                        return View();
                    }
                    else
                    {

                        int idRef = Datos.ActualizarCanalComunicacion(actualizar);

                        comunicacion_canales buscarCanal = Datos.ObtenerCanalComunicacion(id);
                        ViewData["EditarCanalComunicacion"] = buscarCanal;

                        Session["EdicionCanalComunicacionMensaje"] = "Los datos han sido modificados correctamente";
                        return RedirectToAction("editar_clasifcomunicacion/" + idRef, "Configuracion");
                    }
                }
                else
                {
                    comunicacion_canales buscarCanal = Datos.ObtenerCanalComunicacion(id);
                    ViewData["EditarCanalComunicacion"] = buscarCanal;
                    return View();
                }
                #endregion

            }
            catch (Exception ex)
            {
                #region exception
                Session["EdicionCanalComunicacionError"] = "FALLA" + ";" + ex.Message;
                comunicacion_canales buscarCanal = Datos.ObtenerCanalComunicacion(id);
                ViewData["EditarCanalComunicacion"] = buscarCanal;
                return RedirectToAction("canalescomunicacion", "Configuracion");
                #endregion
            }

        }

        #endregion

        #region tipo evento ambiental

        public ActionResult tiposeventoamb()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            ViewData["tiposeventoamb"] = Datos.ListarTiposEventoAmbiental();

            return View();
        }

        public ActionResult eliminar_tipoeventoambiental(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            Datos.EliminarTipoEventoAmbiental(id);
            Session["EditarTiposEventoAmbResultado"] = "Tipo eliminado";
            return RedirectToAction("tiposeventoamb", "Configuracion");
        }

        public ActionResult editar_tipoeventoamb(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            ViewData["idTipoEventoAmb"] = id;

            evento_ambiental_tipo buscarCanal = Datos.ObtenerTipoEventoAmbiental(id);
            ViewData["EditarTipoEventoAmb"] = buscarCanal;
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult editar_tipoeventoamb(int id, FormCollection collection)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            ViewData["idTipoEventoAmb"] = id;

            try
            {
                #region añadir canal comunicacion
                evento_ambiental_tipo actualizar = new evento_ambiental_tipo();
                actualizar.id = id;

                if (collection["ctl00$MainContent$txtNombre"] != null)
                {
                    if (collection["ctl00$MainContent$hdnIdDocumento"] != null && collection["ctl00$MainContent$hdnIdDocumento"] != "")
                        actualizar.id = int.Parse(collection["ctl00$MainContent$hdnIdDocumento"]);
                    actualizar.tipo = collection["ctl00$MainContent$txtNombre"];

                    if (actualizar.tipo == string.Empty)
                    {
                        evento_ambiental_tipo buscarCanal = Datos.ObtenerTipoEventoAmbiental(id);
                        ViewData["EditarTipoEventoAmb"] = buscarCanal;
                        Session["EdicionTipoEventoAmbError"] = "Debe introducir un nombre para el tipo";
                        return View();
                    }
                    else
                    {

                        int idRef = Datos.ActualizarTipoEventoAmbiental(actualizar);

                        evento_ambiental_tipo buscarCanal = Datos.ObtenerTipoEventoAmbiental(id);
                        ViewData["EditarTipoEventoAmb"] = buscarCanal;

                        Session["EdicionTipoEventoAmbMensaje"] = "Los datos han sido modificados correctamente";
                        return RedirectToAction("editar_tipoeventoamb/" + idRef, "Configuracion");
                    }
                }
                else
                {
                    evento_ambiental_tipo buscarCanal = Datos.ObtenerTipoEventoAmbiental(id);
                    ViewData["EditarTipoEventoAmb"] = buscarCanal;
                    return View();
                }
                #endregion

            }
            catch (Exception ex)
            {
                #region exception
                Session["EdicionTipoEventoAmbError"] = "FALLA" + ";" + ex.Message;
                evento_ambiental_tipo buscarCanal = Datos.ObtenerTipoEventoAmbiental(id);
                ViewData["EditarTipoEventoAmb"] = buscarCanal;
                return RedirectToAction("tiposeventoamb", "Configuracion");
                #endregion
            }

        }

        #endregion

        #region tipo evento ambiental

        public ActionResult matriceseventoamb()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            ViewData["matriceseventoamb"] = Datos.GetDatosEventoAmbMatrices();

            return View();
        }

        public ActionResult eliminar_matrizeventoambiental(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            Datos.EliminarMatrizEventoAmbiental(id);
            Session["EditarMatricesEventoAmbResultado"] = "Matriz eliminada";
            return RedirectToAction("matriceseventoamb", "Configuracion");
        }

        public ActionResult editar_matrizeventoamb(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            ViewData["idMatrizEventoAmb"] = id;

            evento_ambiental_matriz buscarCanal = Datos.ObtenerMatrizEventoAmbiental(id);
            ViewData["EditarMatrizEventoAmb"] = buscarCanal;
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult editar_matrizeventoamb(int id, FormCollection collection)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            ViewData["idMatrizEventoAmb"] = id;

            try
            {
                #region añadir matriz comunicacion
                evento_ambiental_matriz actualizar = new evento_ambiental_matriz();
                actualizar.id = id;

                if (collection["ctl00$MainContent$txtNombre"] != null)
                {
                    if (collection["ctl00$MainContent$hdnIdDocumento"] != null && collection["ctl00$MainContent$hdnIdDocumento"] != "")
                        actualizar.id = int.Parse(collection["ctl00$MainContent$hdnIdDocumento"]);
                    actualizar.matriz = collection["ctl00$MainContent$txtNombre"];

                    if (actualizar.matriz == string.Empty)
                    {
                        evento_ambiental_matriz buscarCanal = Datos.ObtenerMatrizEventoAmbiental(id);
                        ViewData["EditarMatrizEventoAmb"] = buscarCanal;
                        Session["EdicionMatrizEventoAmbError"] = "Debe introducir un nombre para la matriz";
                        return View();
                    }
                    else
                    {

                        int idRef = Datos.ActualizarMatrizEventoAmbiental(actualizar);

                        evento_ambiental_matriz buscarCanal = Datos.ObtenerMatrizEventoAmbiental(id);
                        ViewData["EditarMatrizEventoAmb"] = buscarCanal;

                        Session["EdicionMatrizEventoAmbMensaje"] = "Los datos han sido modificados correctamente";
                        return RedirectToAction("editar_matrizeventoamb/" + idRef, "Configuracion");
                    }
                }
                else
                {
                    evento_ambiental_matriz buscarCanal = Datos.ObtenerMatrizEventoAmbiental(id);
                    ViewData["EditarMatrizEventoAmb"] = buscarCanal;
                    return View();
                }
                #endregion

            }
            catch (Exception ex)
            {
                #region exception
                Session["EdicionMatrizEventoAmbError"] = "FALLA" + ";" + ex.Message;
                evento_ambiental_matriz buscarCanal = Datos.ObtenerMatrizEventoAmbiental(id);
                ViewData["EditarMatrizEventoAmb"] = buscarCanal;
                return RedirectToAction("matriceseventoamb", "Configuracion");
                #endregion
            }

        }

        #endregion

        #region parametros

        public ActionResult parametros()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            List<VISTA_ParametrosInd> parametros = Datos.ListarParametrosInd();
            ViewData["parametros"] = parametros;

            return View();
        }

        public ActionResult editar_parametro(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            Session["idParametroEdicion"] = id;
            ViewData["procesos"] = Datos.ListarProcesos();
            indicadores_hojadedatos indHojaDatos = Datos.ObtenerParametroInd(id);
            ViewData["consultaparametro"] = indHojaDatos;
            ViewData["centrosasignables"] = Datos.ListarCentrosAsignablesParametro(id);
            ViewData["centrosasignados"] = Datos.ListarCentrosAsignadosParametro(id);

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult editar_parametro(int id, FormCollection collection)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            string formulario = collection["hdFormularioEjecutado"];

            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            int idCentral = centroseleccionado.id;

            if (formulario == "GuardarParametro")
            {
                #region Guardar Indicador
                try
                {

                    indicadores_hojadedatos actualizar = Datos.ObtenerParametroInd(id);

                    if (actualizar != null)
                    {

                        actualizar.idproceso = 1;
                        actualizar.indicador = collection["ctl00$MainContent$txtParametro"].ToString();
                        actualizar.periodicidad = collection["ctl00$MainContent$ddlPeriodicidad"].ToString();
                        actualizar.unidad = collection["ctl00$MainContent$txtUnidad"].ToString();

                        Datos.ActualizarParametro(actualizar);

                        ViewData["procesos"] = Datos.ListarProcesos();
                        indicadores_hojadedatos indHojaDatos = Datos.ObtenerParametroInd(id);
                        ViewData["consultaparametro"] = indHojaDatos;
                        ViewData["centrosasignables"] = Datos.ListarCentrosAsignablesParametro(id);
                        ViewData["centrosasignados"] = Datos.ListarCentrosAsignadosParametro(id);
                        return View();
                    }
                    else
                    {
                        indicadores_hojadedatos insertar = new indicadores_hojadedatos();

                        insertar.idproceso = 1;
                        insertar.indicador = collection["ctl00$MainContent$txtParametro"].ToString();
                        insertar.periodicidad = collection["ctl00$MainContent$ddlPeriodicidad"].ToString();
                        insertar.unidad = collection["ctl00$MainContent$txtUnidad"].ToString();

                        int idParametro = Datos.ActualizarParametro(insertar);

                        //REDIRECCIONAR CORRECTAMENTE
                        ViewData["procesos"] = Datos.ListarProcesos();
                        indicadores_hojadedatos indHojaDatos = Datos.ObtenerParametroInd(id);
                        ViewData["consultaparametro"] = indHojaDatos;
                        ViewData["centrosasignables"] = Datos.ListarCentrosAsignablesParametro(id);
                        ViewData["centrosasignados"] = Datos.ListarCentrosAsignadosParametro(id);
                        return Redirect(Url.RouteUrl(new { controller = "Configuracion", action = "editar_parametro", id = idParametro }));
                    }


                }
                catch (Exception ex)
                {
                    ViewData["procesos"] = Datos.ListarProcesos();
                    indicadores_hojadedatos indHojaDatos = Datos.ObtenerParametroInd(id);
                    ViewData["consultaparametro"] = indHojaDatos;
                    ViewData["centrosasignables"] = Datos.ListarCentrosAsignablesParametro(id);
                    ViewData["centrosasignados"] = Datos.ListarCentrosAsignadosParametro(id);

                    Session["EdicionParametroError"] = "No se han podido guardar los datos, compruebe que los datos introducidos son correctos.";
                    return View();
                }
                #endregion
            }
            if (formulario == "AsignarCentro")
            {
                #region asignar centro

                if (collection["ctl00$MainContent$ddlCentros"] != null)
                {
                    int idCentro = int.Parse(collection["ctl00$MainContent$ddlCentros"]);
                    Datos.AsociarParametroCentro(id, idCentro);
                }


                ViewData["procesos"] = Datos.ListarProcesos();
                indicadores_hojadedatos indHojaDatos = Datos.ObtenerParametroInd(id);
                ViewData["consultaparametro"] = indHojaDatos;
                ViewData["centrosasignables"] = Datos.ListarCentrosAsignablesParametro(id);
                ViewData["centrosasignados"] = Datos.ListarCentrosAsignadosParametro(id);
                return View();
                #endregion
            }
            else
            {
                ViewData["procesos"] = Datos.ListarProcesos();
                indicadores_hojadedatos indHojaDatos = Datos.ObtenerParametroInd(id);
                ViewData["consultaparametro"] = indHojaDatos;
                ViewData["centrosasignables"] = Datos.ListarCentrosAsignablesParametro(id);
                ViewData["centrosasignados"] = Datos.ListarCentrosAsignadosParametro(id);

                return View();
            }

        }


        public JsonResult EliminaImagenMedida(string idMedida, string tipoImagen)
        {

            bool res = false;

            var tipoI = tipoImagen == "icono" ? 0 : 1;

            Datos.EliminarRutaImagenPorId_Y_TipoImagen(int.Parse(idMedida), tipoI);

            return Json(res);
        }
        #endregion
    }

}
