using MIDAS.Helpers;
using MIDAS.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MIDAS.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ActionResult About()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }
            return View();
        }

        public ActionResult Principal(int? id)
        {
            log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(Server.MapPath("~/log4net.config")));

            try
            {
                if (Session["usuario"] == null)
                {
                    return RedirectToAction("LogOn", "Account");
                }

                MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
                MIDAS.Models.centros centroseleccionado = new MIDAS.Models.centros();

                if (Session["usuario"] != null)
                {

                    user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());

                    //if (Session["CentralElegida"] != null)
                    //{
                    //    centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
                    //}
                    //else
                    //    return RedirectToAction("LogOn", "Account");

                    ViewData["noticiasCentral"] = Datos.ListarNoticiasCentral(centroseleccionado.id);
                    ViewData["noticiasGenerales"] = Datos.ListarNoticiasGenerales();

                    //Esta parte busca si el esta en la tabla lista_final_personas, para crear una variable de session para mostrar una opcion de listaFinal
                    var listaFinalPersonas = Datos.Listar_ListaFinalPersonas();

                    var existeUsuario = listaFinalPersonas.Where(x => x.Usuario_Propietario_Lista.Equals(user.idUsuario)).ToList();

                    if(existeUsuario.Count > 0)
                    {
                        Session["ListaFinal"] = existeUsuario;
                    }
                    else
                    {
                        Session.Remove("ListaFinal");
                    }


                    return View();
                }
                else
                {
                    return RedirectToAction("LogOn", "Account");
                }
            }
            catch (Exception ex)
            {
                new EscribirLog("Error al conectar al servidor de BD " +
                            ex.Message, true, this.ToString(), "Principal");
                return RedirectToAction("LogOn", "Account");
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Principal(FormCollection collection)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }
            return View();
        }

        public ActionResult datos_centro(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            usuarios user = new usuarios();
            user = Datos.ObtenerUsuario(int.Parse(Session["idUsuario"].ToString()));

            //ViewData["clientes"] = Datos.ListarClientes();


            ViewData["EditarOrganizacion"] = Datos.ObtenerCentroPorID(id);

            centros org = (centros)ViewData["EditarOrganizacion"];

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult datos_centro(int id, FormCollection collection, HttpPostedFileBase file)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            string formulario = collection["hdFormularioEjecutado"];

            if (formulario == "GuardarOrganizacion")
            {
                #region guardar organizacion
                centros org = new centros();
                org = Datos.ObtenerCentroPorID(id);

                org.siglas = collection["ctl00$MainContent$txtSiglas"];
                org.nombre = collection["ctl00$MainContent$txtNombre"];

                //Datos.ActualizarCentro(org);

                ViewData["EditarOrganizacion"] = Datos.ObtenerCentroPorID(id);

                centros orga = (centros)ViewData["EditarOrganizacion"];
                #endregion
            }
            else
            {
                #region recarga
                centros org = Datos.ObtenerCentroPorID(id);
                //ViewData["clientes"] = Datos.ListarClientes();
                ViewData["EditarOrganizacion"] = org;
                ViewData["comunidades"] = Datos.ListarProvincias();
                return RedirectToAction("Principal", "Home");
                #endregion
            }
            return View();
        }

        public ActionResult cambiar_password(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            Session["idUsuario"] = id;

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult cambiar_password(int id, FormCollection collection)
        {
            usuarios actualizar = new usuarios();
            actualizar.idUsuario = id;

            if (collection["txtPassword"] != null && (collection["txtPassword"] != "" && collection["txtRepetir"] != "") && (collection["txtPassword"] == collection["txtRepetir"]))
            {
                Session["passwordchanged"] = "1";
                actualizar.password = System.Text.Encoding.UTF8.GetBytes(collection["txtRepetir"]);
                Datos.ActualizarPassword(actualizar);
            }
            else
            {
                if (collection["txtPassword"] == "")
                {
                    Session["passwordchanged"] = "2";
                }
                else
                {
                    Session["passwordchanged"] = "3";
                }
            }
            return View();
        }

        public ActionResult utilidades()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }
            ViewData["ficherosAdjuntos"] = Datos.ListarFicheros(1);
            return View();
        }

        [HttpPost]
        public ActionResult Utilidades(HttpPostedFileBase file, FormCollection collection)
        {

            if (file != null && file.ContentLength > 0 && collection["ctl00$MainContent$txtNombre"] != null)
            {
                var fileName = System.IO.Path.GetFileName(file.FileName);
                string id = "1";

                var path = System.IO.Path.Combine(Server.MapPath("~/Ficheros"), fileName);

                if (Directory.Exists(Server.MapPath("~/Ficheros")))
                {
                    file.SaveAs(path);
                }
                else
                {
                    Directory.CreateDirectory(Server.MapPath("~/Ficheros"));
                    file.SaveAs(path);
                }

                ficheros IF = new ficheros();
                IF.nombre_fichero = collection["ctl00$MainContent$txtNombre"];
                IF.tipo = int.Parse(id);
                IF.enlace = path;

                Datos.InsertFichero(IF);

                log insertarlog = new log();
                insertarlog.usuario = Session["usuario"].ToString();
                insertarlog.fecha = DateTime.Now;
                insertarlog.evento = "Agregado fichero a utilidades";

                Datos.InsertarEnLog(insertarlog);
            }
            else
            {
                Session["error"] = 1;
            }
            return RedirectToAction("utilidades/" + Session["idFicheros"], "Home");
        }

        public FileResult ObtenerFichero(int id)
        {
            try
            {
                ficheros IF = Datos.ObtenerFicheroPorID(id);
                byte[] fileBytes = System.IO.File.ReadAllBytes(IF.enlace);
                string fileName = IF.enlace.Replace(Server.MapPath("~/Ficheros") + "\\", "");
                fileName = fileName.Replace("á", "a");
                fileName = fileName.Replace("é", "e");
                fileName = fileName.Replace("í", "i");
                fileName = fileName.Replace("ó", "o");
                fileName = fileName.Replace("ú", "u");
                fileName = fileName.Replace("Á", "A");
                fileName = fileName.Replace("É", "É");
                fileName = fileName.Replace("Í", "I");
                fileName = fileName.Replace("Ó", "O");
                fileName = fileName.Replace("Ú", "U");
                //Datos.InsertFichero(IF);

                log insertarlog = new log();
                insertarlog.usuario = Session["usuario"].ToString();
                insertarlog.fecha = DateTime.Now;
                insertarlog.evento = "Descargado el fichero: " + fileName;

                Datos.InsertarEnLog(insertarlog);
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ActionResult eliminar_fichero(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            Datos.EliminarFichero(id);
            return RedirectToAction("utilidades", "Home");
        }


        public FileResult ObtenerFicheroRep(int id)
        {
            try
            {
                documentacion IF = Datos.ObtenerFicheroDocPorID(id);
                byte[] fileBytes = System.IO.File.ReadAllBytes(IF.enlace);
                string fileName = IF.nombre_fichero.Replace(Server.MapPath("~/Documentacion") + "\\", "");
                fileName = fileName.Replace("á", "a");
                fileName = fileName.Replace("é", "e");
                fileName = fileName.Replace("í", "i");
                fileName = fileName.Replace("ó", "o");
                fileName = fileName.Replace("ú", "u");
                fileName = fileName.Replace("Á", "A");
                fileName = fileName.Replace("É", "É");
                fileName = fileName.Replace("Í", "I");
                fileName = fileName.Replace("Ó", "O");
                fileName = fileName.Replace("Ú", "U");
                log insertarlog = new log();
                insertarlog.usuario = Session["usuario"].ToString();
                insertarlog.fecha = DateTime.Now;
                insertarlog.evento = "Descargado el fichero: " + fileName;

                Datos.InsertarEnLog(insertarlog);

                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #region enlaces

        public ActionResult eliminar_enlace(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            Datos.EliminarEnlace(id);
            return RedirectToAction("enlaces", "Home");
        }

        public ActionResult enlaces()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }
            ViewData["enlaces"] = Datos.ListarEnlaces();
            return View();
        }


        public ActionResult detalle_enlace(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }
            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            int idCentral = centroseleccionado.id;

            enlaces obj = Datos.GetDatosEnlace(id);
            ViewData["enlace"] = obj;
            ViewData["ambitos"] = Datos.ListarAmbitos();

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult detalle_enlace(int id, FormCollection collection, HttpPostedFileBase file)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            enlaces enlac = new enlaces();
            string formulario = collection["hdFormularioEjecutado"];

            if (formulario == "GuardarEnlace")
            {
                #region guardar
                try
                {
                    if (id != 0)
                    {
                        #region actualizar
                        enlaces actualizar = Datos.GetDatosEnlace(id);
                        actualizar.titulo = collection["ctl00$MainContent$txtNombre"];
                        actualizar.url = collection["ctl00$MainContent$txtURL"];
                        actualizar.ambito = int.Parse(collection["ctl00$MainContent$ddlAmbito"]);

                        if (actualizar.titulo != null)
                        {
                            Datos.ActualizarEnlace(actualizar);

                            if ((file != null && file.ContentLength > 0))
                            {
                                var fileName = System.IO.Path.GetFileName(file.FileName);

                                var path = System.IO.Path.Combine(Server.MapPath("~/Enlaces/" + id.ToString()), fileName);
                                actualizar.enlace = path;

                                Datos.UpdateEnlaceEnlace(actualizar);

                                if (Directory.Exists(Server.MapPath("~/Enlaces/" + id.ToString())))
                                {
                                    file.SaveAs(path);
                                }
                                else
                                {
                                    Directory.CreateDirectory(Server.MapPath("~/Enlaces/" + id.ToString()));
                                    file.SaveAs(path);
                                }
                            }


                            Session["EdicionEnlaceMensaje"] = "Información actualizada correctamente";
                            enlac = Datos.GetDatosEnlace(id);
                            ViewData["enlace"] = enlac;
                            ViewData["ambitos"] = Datos.ListarAmbitos();
                        }
                        else
                        {
                            Session["EdicionEnlaceError"] = "Los campos marcados con (*) son obligatorios.";
                            enlac = Datos.GetDatosEnlace(id);
                            ViewData["enlace"] = enlac;
                            ViewData["ambitos"] = Datos.ListarAmbitos();
                        }
                        return View();
                        #endregion
                    }
                    else
                    {
                        #region insertar
                        enlaces insertar = new enlaces();
                        insertar.titulo = collection["ctl00$MainContent$txtNombre"];
                        insertar.url = collection["ctl00$MainContent$txtURL"];
                        insertar.ambito = int.Parse(collection["ctl00$MainContent$ddlAmbito"]);

                        if (insertar.titulo != string.Empty)
                        {
                            int idForm = Datos.ActualizarEnlace(insertar);

                            if ((file != null && file.ContentLength > 0))
                            {
                                var fileName = System.IO.Path.GetFileName(file.FileName);

                                var path = System.IO.Path.Combine(Server.MapPath("~/Enlaces/" + idForm.ToString()), fileName);
                                insertar.enlace = path;

                                Datos.UpdateEnlaceEnlace(insertar);

                                if (Directory.Exists(Server.MapPath("~/Enlaces/" + idForm.ToString())))
                                {
                                    file.SaveAs(path);
                                }
                                else
                                {
                                    Directory.CreateDirectory(Server.MapPath("~/Enlaces/" + idForm.ToString()));
                                    file.SaveAs(path);
                                }
                            }

                            Session["EdicionEnlaceMensaje"] = "Información actualizada correctamente";

                            enlac = Datos.GetDatosEnlace(id);
                            ViewData["enlace"] = enlac;
                            ViewData["ambitos"] = Datos.ListarAmbitos();
                            return Redirect(Url.RouteUrl(new { controller = "home", action = "detalle_enlace", id = idForm }));
                        }
                        else
                        {
                            Session["EdicionEnlaceError"] = "Los campos marcados con (*) son obligatorios.";
                            enlac = insertar;
                            ViewData["enlace"] = enlac;
                            ViewData["ambitos"] = Datos.ListarAmbitos();
                            return View();
                        }
                        #endregion
                    }

                }
                catch (Exception ex)
                {
                    enlac = Datos.GetDatosEnlace(id);
                    ViewData["enlace"] = enlac;
                    ViewData["ambitos"] = Datos.ListarAmbitos();
                    return View();
                }
                #endregion
            }
            else
            {
                enlac = Datos.GetDatosEnlace(id);
                ViewData["enlace"] = enlac;
                ViewData["ambitos"] = Datos.ListarAmbitos();
                return View();
            }
        }

        public FileResult ObtenerEnlace(int id)
        {
            try
            {
                enlaces IF = Datos.ObtenerEnlacePorID(id);
                byte[] fileBytes = System.IO.File.ReadAllBytes(IF.enlace);
                string fileName = IF.enlace.Replace(Server.MapPath("~/Enlaces/" + id) + "\\", "");
                fileName = fileName.Replace("á", "a");
                fileName = fileName.Replace("é", "e");
                fileName = fileName.Replace("í", "i");
                fileName = fileName.Replace("ó", "o");
                fileName = fileName.Replace("ú", "u");
                fileName = fileName.Replace("Á", "A");
                fileName = fileName.Replace("É", "É");
                fileName = fileName.Replace("Í", "I");
                fileName = fileName.Replace("Ó", "O");
                fileName = fileName.Replace("Ú", "U");
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion

        #region apoyoPRL

        public ActionResult apoyoprl()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            ViewData["materiales"] = Datos.ListarMateriales();
            ViewData["informes"] = Datos.ListarInformesSeguridad();
            ViewData["evaluaciones"] = Datos.ListarEvaluacionesRiesgo();

            return View();
        }

        #region material divulgativo

        public ActionResult detalle_material(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }
            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            int idCentral = centroseleccionado.id;

            materialdivulgativo obj = Datos.GetDatosMaterial(id);
            ViewData["material"] = obj;
            ViewData["riesgos"] = Datos.ListarMaterialRiesgos();
            ViewData["tipodocs"] = Datos.ListarMaterialTipodocs();
            ViewData["centros"] = Datos.ListarCentros();

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult detalle_material(int id, FormCollection collection, HttpPostedFileBase file)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            enlaces enlac = new enlaces();
            string formulario = collection["hdFormularioEjecutado"];

            if (formulario == "GuardarMaterial")
            {
                #region guardar
                try
                {
                    if (id != 0)
                    {
                        #region actualizar
                        materialdivulgativo actualizar = Datos.GetDatosMaterial(id);
                        actualizar.codigo = collection["ctl00$MainContent$txtCodigo"];
                        actualizar.titulo = collection["ctl00$MainContent$txtNombre"];
                        actualizar.tipodoc = int.Parse(collection["ctl00$MainContent$ddlTipoDoc"]);
                        if (collection["ctl00$MainContent$txtFechaPublicacion"] != null)
                            actualizar.fechapub = DateTime.Parse(collection["ctl00$MainContent$txtFechaPublicacion"]);
                        if (collection["ctl00$MainContent$ddlRiesgos"] != null && actualizar.tipodoc == 1)
                            actualizar.riesgoasoc = int.Parse(collection["ctl00$MainContent$ddlRiesgos"]);
                        else
                            actualizar.riesgoasoc = null;
                        if (collection["ctl00$MainContent$ddlCentro"] != null && actualizar.tipodoc == 4)
                            actualizar.idcentral = int.Parse(collection["ctl00$MainContent$ddlCentro"]);
                        else
                            actualizar.idcentral = null;

                        if (actualizar.titulo != null)
                        {
                            Datos.ActualizarMaterial(actualizar);

                            if ((file != null && file.ContentLength > 0))
                            {
                                var fileName = System.IO.Path.GetFileName(file.FileName);

                                var path = System.IO.Path.Combine(Server.MapPath("~/Materiales/" + id.ToString()), fileName);
                                actualizar.enlace = path;

                                Datos.UpdateEnlaceMaterial(actualizar);

                                if (Directory.Exists(Server.MapPath("~/Materiales/" + id.ToString())))
                                {
                                    file.SaveAs(path);
                                }
                                else
                                {
                                    Directory.CreateDirectory(Server.MapPath("~/Materiales/" + id.ToString()));
                                    file.SaveAs(path);
                                }
                            }


                            Session["EdicionMaterialMensaje"] = "Información actualizada correctamente";
                            materialdivulgativo obj = Datos.GetDatosMaterial(id);
                            ViewData["material"] = obj;
                            ViewData["riesgos"] = Datos.ListarMaterialRiesgos();
                            ViewData["tipodocs"] = Datos.ListarMaterialTipodocs();
                            ViewData["centros"] = Datos.ListarCentros();
                        }
                        else
                        {
                            Session["EdicionMaterialError"] = "Los campos marcados con (*) son obligatorios.";
                            materialdivulgativo obj = Datos.GetDatosMaterial(id);
                            ViewData["material"] = obj;
                            ViewData["riesgos"] = Datos.ListarMaterialRiesgos();
                            ViewData["tipodocs"] = Datos.ListarMaterialTipodocs();
                            ViewData["centros"] = Datos.ListarCentros();
                        }
                        return View();
                        #endregion
                    }
                    else
                    {
                        #region insertar
                        materialdivulgativo insertar = new materialdivulgativo();
                        insertar.codigo = collection["ctl00$MainContent$txtCodigo"];
                        insertar.titulo = collection["ctl00$MainContent$txtNombre"];
                        insertar.tipodoc = int.Parse(collection["ctl00$MainContent$ddlTipoDoc"]);
                        if (collection["ctl00$MainContent$txtFechaPublicacion"] != null)
                            insertar.fechapub = DateTime.Parse(collection["ctl00$MainContent$txtFechaPublicacion"]);
                        if (collection["ctl00$MainContent$ddlRiesgos"] != null && insertar.tipodoc == 1)
                            insertar.riesgoasoc = int.Parse(collection["ctl00$MainContent$ddlRiesgos"]);
                        else
                            insertar.riesgoasoc = null;
                        if (collection["ctl00$MainContent$ddlCentro"] != null && insertar.tipodoc == 4)
                            insertar.idcentral = int.Parse(collection["ctl00$MainContent$ddlCentro"]);
                        else
                            insertar.idcentral = null;

                        if (insertar.titulo != string.Empty)
                        {
                            int idForm = Datos.ActualizarMaterial(insertar);

                            if ((file != null && file.ContentLength > 0))
                            {
                                var fileName = System.IO.Path.GetFileName(file.FileName);

                                var path = System.IO.Path.Combine(Server.MapPath("~/Materiales/" + idForm.ToString()), fileName);
                                insertar.enlace = path;

                                Datos.UpdateEnlaceMaterial(insertar);

                                if (Directory.Exists(Server.MapPath("~/Materiales/" + idForm.ToString())))
                                {
                                    file.SaveAs(path);
                                }
                                else
                                {
                                    Directory.CreateDirectory(Server.MapPath("~/Materiales/" + idForm.ToString()));
                                    file.SaveAs(path);
                                }
                            }

                            Session["EdicionMaterialMensaje"] = "Información actualizada correctamente";

                            materialdivulgativo obj = Datos.GetDatosMaterial(id);
                            ViewData["material"] = obj;
                            ViewData["riesgos"] = Datos.ListarMaterialRiesgos();
                            ViewData["tipodocs"] = Datos.ListarMaterialTipodocs();
                            ViewData["centros"] = Datos.ListarCentros();
                            return Redirect(Url.RouteUrl(new { controller = "home", action = "detalle_material", id = idForm }));
                        }
                        else
                        {
                            Session["EdicionMaterialError"] = "Los campos marcados con (*) son obligatorios.";
                            materialdivulgativo obj = insertar;
                            ViewData["material"] = obj;
                            ViewData["riesgos"] = Datos.ListarMaterialRiesgos();
                            ViewData["tipodocs"] = Datos.ListarMaterialTipodocs();
                            ViewData["centros"] = Datos.ListarCentros();
                            return View();
                        }
                        #endregion
                    }

                }
                catch (Exception ex)
                {
                    materialdivulgativo obj = Datos.GetDatosMaterial(id);
                    ViewData["material"] = obj;
                    ViewData["riesgos"] = Datos.ListarMaterialRiesgos();
                    ViewData["tipodocs"] = Datos.ListarMaterialTipodocs();
                    ViewData["centros"] = Datos.ListarCentros();
                    return View();
                }
                #endregion
            }
            else
            {
                materialdivulgativo obj = Datos.GetDatosMaterial(id);
                ViewData["material"] = obj;
                ViewData["riesgos"] = Datos.ListarMaterialRiesgos();
                ViewData["tipodocs"] = Datos.ListarMaterialTipodocs();
                ViewData["centros"] = Datos.ListarCentros();
                return View();
            }
        }

        public FileResult ObtenerMaterial(int id)
        {
            try
            {
                materialdivulgativo IF = Datos.ObtenerMaterialPorID(id);
                byte[] fileBytes = System.IO.File.ReadAllBytes(IF.enlace);
                string fileName = IF.enlace.Replace(Server.MapPath("~/Materiales/" + id) + "\\", "");
                fileName = fileName.Replace("á", "a");
                fileName = fileName.Replace("é", "e");
                fileName = fileName.Replace("í", "i");
                fileName = fileName.Replace("ó", "o");
                fileName = fileName.Replace("ú", "u");
                fileName = fileName.Replace("Á", "A");
                fileName = fileName.Replace("É", "É");
                fileName = fileName.Replace("Í", "I");
                fileName = fileName.Replace("Ó", "O");
                fileName = fileName.Replace("Ú", "U");

                Datos.ActualizarDescargasMaterial(IF.id);
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ActionResult eliminar_material(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            Datos.EliminarMaterial(id);
            return RedirectToAction("apoyoprl", "Home");
        }

        #endregion

        #region informes de seguridad

        public ActionResult detalle_informe(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }
            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            int idCentral = centroseleccionado.id;

            informesseguridad obj = Datos.GetDatosInforme(id);
            ViewData["informe"] = obj;
            ViewData["elaborado"] = Datos.ListarInformesElaborado();
            ViewData["tipodocs"] = Datos.ListarInformesTipodocs();

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult detalle_informe(int id, FormCollection collection, HttpPostedFileBase file)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            string formulario = collection["hdFormularioEjecutado"];

            if (formulario == "GuardarInforme")
            {
                #region guardar
                try
                {
                    if (id != 0)
                    {
                        #region actualizar
                        informesseguridad actualizar = Datos.GetDatosInforme(id);
                        actualizar.codigo = collection["ctl00$MainContent$txtCodigo"];
                        actualizar.titulo = collection["ctl00$MainContent$txtNombre"];
                        actualizar.tipodoc = int.Parse(collection["ctl00$MainContent$ddlTipoDoc"]);
                        actualizar.elaboradopor = int.Parse(collection["ctl00$MainContent$ddlElaboradoPor"]);
                        if (collection["ctl00$MainContent$txtFechaPublicacion"] != null)
                            actualizar.fechapub = DateTime.Parse(collection["ctl00$MainContent$txtFechaPublicacion"]);
                        actualizar.mes = int.Parse(collection["ctl00$MainContent$ddlMes"]);
                        actualizar.anio = int.Parse(collection["ctl00$MainContent$ddlAnio"]);

                        if (actualizar.titulo != null)
                        {
                            Datos.ActualizarInforme(actualizar);

                            if ((file != null && file.ContentLength > 0))
                            {
                                var fileName = System.IO.Path.GetFileName(file.FileName);

                                var path = System.IO.Path.Combine(Server.MapPath("~/InformesSeguridad/" + id.ToString()), fileName);
                                actualizar.enlace = path;

                                Datos.UpdateEnlaceInformeSeg(actualizar);

                                if (Directory.Exists(Server.MapPath("~/InformesSeguridad/" + id.ToString())))
                                {
                                    file.SaveAs(path);
                                }
                                else
                                {
                                    Directory.CreateDirectory(Server.MapPath("~/InformesSeguridad/" + id.ToString()));
                                    file.SaveAs(path);
                                }
                            }


                            Session["EdicionInformeMensaje"] = "Información actualizada correctamente";
                            informesseguridad obj = Datos.GetDatosInforme(id);
                            ViewData["informe"] = obj;
                            ViewData["elaborado"] = Datos.ListarInformesElaborado();
                            ViewData["tipodocs"] = Datos.ListarInformesTipodocs();
                        }
                        else
                        {
                            Session["EdicionInformeError"] = "Los campos marcados con (*) son obligatorios.";
                            informesseguridad obj = Datos.GetDatosInforme(id);
                            ViewData["informe"] = obj;
                            ViewData["elaborado"] = Datos.ListarInformesElaborado();
                            ViewData["tipodocs"] = Datos.ListarInformesTipodocs();
                        }
                        return View();
                        #endregion
                    }
                    else
                    {
                        #region insertar
                        informesseguridad insertar = new informesseguridad();
                        insertar.codigo = collection["ctl00$MainContent$txtCodigo"];
                        insertar.titulo = collection["ctl00$MainContent$txtNombre"];
                        insertar.tipodoc = int.Parse(collection["ctl00$MainContent$ddlTipoDoc"]);
                        insertar.elaboradopor = int.Parse(collection["ctl00$MainContent$ddlElaboradoPor"]);
                        if (collection["ctl00$MainContent$txtFechaPublicacion"] != null)
                            insertar.fechapub = DateTime.Parse(collection["ctl00$MainContent$txtFechaPublicacion"]);
                        insertar.mes = int.Parse(collection["ctl00$MainContent$ddlMes"]);
                        insertar.anio = int.Parse(collection["ctl00$MainContent$ddlAnio"]);

                        if (insertar.titulo != string.Empty)
                        {
                            int idForm = Datos.ActualizarInforme(insertar);

                            if ((file != null && file.ContentLength > 0))
                            {
                                var fileName = System.IO.Path.GetFileName(file.FileName);

                                var path = System.IO.Path.Combine(Server.MapPath("~/InformesSeguridad/" + idForm.ToString()), fileName);
                                insertar.enlace = path;

                                Datos.UpdateEnlaceInformeSeg(insertar);

                                if (Directory.Exists(Server.MapPath("~/InformesSeguridad/" + idForm.ToString())))
                                {
                                    file.SaveAs(path);
                                }
                                else
                                {
                                    Directory.CreateDirectory(Server.MapPath("~/InformesSeguridad/" + idForm.ToString()));
                                    file.SaveAs(path);
                                }
                            }

                            Session["EdicionInformeMensaje"] = "Información actualizada correctamente";

                            informesseguridad obj = Datos.GetDatosInforme(id);
                            ViewData["informe"] = obj;
                            ViewData["elaborado"] = Datos.ListarInformesElaborado();
                            ViewData["tipodocs"] = Datos.ListarInformesTipodocs();
                            return Redirect(Url.RouteUrl(new { controller = "home", action = "detalle_informe", id = idForm }));
                        }
                        else
                        {
                            Session["EdicionInformeError"] = "Los campos marcados con (*) son obligatorios.";
                            informesseguridad obj = Datos.GetDatosInforme(id);
                            ViewData["informe"] = obj;
                            ViewData["elaborado"] = Datos.ListarInformesElaborado();
                            ViewData["tipodocs"] = Datos.ListarInformesTipodocs();
                            return View();
                        }
                        #endregion
                    }

                }
                catch (Exception ex)
                {
                    informesseguridad obj = Datos.GetDatosInforme(id);
                    ViewData["informe"] = obj;
                    ViewData["elaborado"] = Datos.ListarInformesElaborado();
                    ViewData["tipodocs"] = Datos.ListarInformesTipodocs();
                    return View();
                }
                #endregion
            }
            else
            {
                informesseguridad obj = Datos.GetDatosInforme(id);
                ViewData["informe"] = obj;
                ViewData["elaborado"] = Datos.ListarInformesElaborado();
                ViewData["tipodocs"] = Datos.ListarInformesTipodocs();
                return View();
            }
        }

        public FileResult ObtenerInformeSeguridad(int id)
        {
            try
            {
                informesseguridad IF = Datos.ObtenerInformeSeguridadPorID(id);
                byte[] fileBytes = System.IO.File.ReadAllBytes(IF.enlace);
                string fileName = IF.enlace.Replace(Server.MapPath("~/InformesSeguridad/" + id) + "\\", "");
                fileName = fileName.Replace("á", "a");
                fileName = fileName.Replace("é", "e");
                fileName = fileName.Replace("í", "i");
                fileName = fileName.Replace("ó", "o");
                fileName = fileName.Replace("ú", "u");
                fileName = fileName.Replace("Á", "A");
                fileName = fileName.Replace("É", "É");
                fileName = fileName.Replace("Í", "I");
                fileName = fileName.Replace("Ó", "O");
                fileName = fileName.Replace("Ú", "U");

                Datos.ActualizarDescargasInfSeguridad(IF.id);
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ActionResult eliminar_informeseguridad(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            Datos.EliminarInformeSeguridad(id);
            return RedirectToAction("apoyoprl", "Home");
        }

        #endregion

        #region evaluaciones de riesgos

        public ActionResult detalle_evaluacion(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }
            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            int idCentral = centroseleccionado.id;

            evaluacionriesgos obj = Datos.GetDatosEvaluaciones(id);
            ViewData["evaluacion"] = obj;
            ViewData["elaborado"] = Datos.ListarEvaluacionElaborado();
            ViewData["tipodocs"] = Datos.ListarEvaluacionTipodocs();
            ViewData["centros"] = Datos.ListarCentros();

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult detalle_evaluacion(int id, FormCollection collection, HttpPostedFileBase file)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            string formulario = collection["hdFormularioEjecutado"];

            if (formulario == "GuardarEvaluacion")
            {
                #region guardar
                try
                {
                    if (id != 0)
                    {
                        #region actualizar
                        evaluacionriesgos actualizar = Datos.GetDatosEvaluaciones(id);
                        actualizar.codigo = collection["ctl00$MainContent$txtCodigo"];
                        actualizar.titulo = collection["ctl00$MainContent$txtNombre"];
                        actualizar.tipodoc = int.Parse(collection["ctl00$MainContent$ddlTipoDoc"]);
                        actualizar.elaboradopor = int.Parse(collection["ctl00$MainContent$ddlElaboradoPor"]);
                        if (collection["ctl00$MainContent$txtFechaPublicacion"] != null)
                            actualizar.fechapub = DateTime.Parse(collection["ctl00$MainContent$txtFechaPublicacion"]);
                        if (collection["ctl00$MainContent$ddlElaboradoPor"] != null && actualizar.elaboradopor != 1)
                            actualizar.empresa = collection["ctl00$MainContent$txtEmpresa"];
                        else
                            actualizar.empresa = null;
                        actualizar.idcentral = int.Parse(collection["ctl00$MainContent$ddlCentro"]);

                        if (actualizar.titulo != null)
                        {
                            Datos.ActualizarEvaluacionRiesgo(actualizar);

                            if ((file != null && file.ContentLength > 0))
                            {
                                var fileName = System.IO.Path.GetFileName(file.FileName);

                                var path = System.IO.Path.Combine(Server.MapPath("~/EvaluacionRiesgos/" + id.ToString()), fileName);
                                actualizar.enlace = path;

                                Datos.UpdateEnlaceEvaluacion(actualizar);

                                if (Directory.Exists(Server.MapPath("~/EvaluacionRiesgos/" + id.ToString())))
                                {
                                    file.SaveAs(path);
                                }
                                else
                                {
                                    Directory.CreateDirectory(Server.MapPath("~/EvaluacionRiesgos/" + id.ToString()));
                                    file.SaveAs(path);
                                }
                            }


                            Session["EdicionEvaluacionMensaje"] = "Información actualizada correctamente";
                            evaluacionriesgos obj = Datos.GetDatosEvaluaciones(id);
                            ViewData["evaluacion"] = obj;
                            ViewData["elaborado"] = Datos.ListarEvaluacionElaborado();
                            ViewData["tipodocs"] = Datos.ListarEvaluacionTipodocs();
                            ViewData["centros"] = Datos.ListarCentros();
                        }
                        else
                        {
                            Session["EdicionEvaluacionError"] = "Los campos marcados con (*) son obligatorios.";
                            evaluacionriesgos obj = Datos.GetDatosEvaluaciones(id);
                            ViewData["evaluacion"] = obj;
                            ViewData["elaborado"] = Datos.ListarEvaluacionElaborado();
                            ViewData["tipodocs"] = Datos.ListarEvaluacionTipodocs();
                            ViewData["centros"] = Datos.ListarCentros();
                        }
                        return View();
                        #endregion
                    }
                    else
                    {
                        #region insertar
                        evaluacionriesgos insertar = new evaluacionriesgos();
                        insertar.codigo = collection["ctl00$MainContent$txtCodigo"];
                        insertar.titulo = collection["ctl00$MainContent$txtNombre"];
                        insertar.tipodoc = int.Parse(collection["ctl00$MainContent$ddlTipoDoc"]);
                        insertar.elaboradopor = int.Parse(collection["ctl00$MainContent$ddlElaboradoPor"]);
                        if (collection["ctl00$MainContent$txtFechaPublicacion"] != null)
                            insertar.fechapub = DateTime.Parse(collection["ctl00$MainContent$txtFechaPublicacion"]);
                        if (collection["ctl00$MainContent$ddlElaboradoPor"] != null && insertar.elaboradopor != 1)
                            insertar.empresa = collection["ctl00$MainContent$txtEmpresa"];
                        else
                            insertar.empresa = null;
                        insertar.idcentral = int.Parse(collection["ctl00$MainContent$ddlCentro"]);

                        if (insertar.titulo != string.Empty)
                        {
                            int idForm = Datos.ActualizarEvaluacionRiesgo(insertar);

                            if ((file != null && file.ContentLength > 0))
                            {
                                var fileName = System.IO.Path.GetFileName(file.FileName);

                                var path = System.IO.Path.Combine(Server.MapPath("~/EvaluacionRiesgos/" + idForm.ToString()), fileName);
                                insertar.enlace = path;

                                Datos.UpdateEnlaceEvaluacion(insertar);

                                if (Directory.Exists(Server.MapPath("~/EvaluacionRiesgos/" + idForm.ToString())))
                                {
                                    file.SaveAs(path);
                                }
                                else
                                {
                                    Directory.CreateDirectory(Server.MapPath("~/EvaluacionRiesgos/" + idForm.ToString()));
                                    file.SaveAs(path);
                                }
                            }

                            Session["EdicionEvaluacionMensaje"] = "Información actualizada correctamente";

                            evaluacionriesgos obj = Datos.GetDatosEvaluaciones(id);
                            ViewData["evaluacion"] = obj;
                            ViewData["elaborado"] = Datos.ListarEvaluacionElaborado();
                            ViewData["tipodocs"] = Datos.ListarEvaluacionTipodocs();
                            ViewData["centros"] = Datos.ListarCentros();
                            return Redirect(Url.RouteUrl(new { controller = "home", action = "detalle_evaluacion", id = idForm }));
                        }
                        else
                        {
                            Session["EdicionEvaluacionError"] = "Los campos marcados con (*) son obligatorios.";
                            evaluacionriesgos obj = Datos.GetDatosEvaluaciones(id);
                            ViewData["evaluacion"] = obj;
                            ViewData["elaborado"] = Datos.ListarEvaluacionElaborado();
                            ViewData["tipodocs"] = Datos.ListarEvaluacionTipodocs();
                            ViewData["centros"] = Datos.ListarCentros();
                            return View();
                        }
                        #endregion
                    }

                }
                catch (Exception ex)
                {
                    evaluacionriesgos obj = Datos.GetDatosEvaluaciones(id);
                    ViewData["evaluacion"] = obj;
                    ViewData["elaborado"] = Datos.ListarEvaluacionElaborado();
                    ViewData["tipodocs"] = Datos.ListarEvaluacionTipodocs();
                    ViewData["centros"] = Datos.ListarCentros();
                    return View();
                }
                #endregion
            }
            else
            {
                evaluacionriesgos obj = Datos.GetDatosEvaluaciones(id);
                ViewData["evaluacion"] = obj;
                ViewData["elaborado"] = Datos.ListarEvaluacionElaborado();
                ViewData["tipodocs"] = Datos.ListarEvaluacionTipodocs();
                ViewData["centros"] = Datos.ListarCentros();
                return View();
            }
        }

        public FileResult ObtenerEvaluacionRiesgos(int id)
        {
            try
            {
                evaluacionriesgos IF = Datos.ObtenerEvaluacionRiesgosPorID(id);
                byte[] fileBytes = System.IO.File.ReadAllBytes(IF.enlace);
                string fileName = IF.enlace.Replace(Server.MapPath("~/EvaluacionRiesgos/" + id) + "\\", "");
                fileName = fileName.Replace("á", "a");
                fileName = fileName.Replace("é", "e");
                fileName = fileName.Replace("í", "i");
                fileName = fileName.Replace("ó", "o");
                fileName = fileName.Replace("ú", "u");
                fileName = fileName.Replace("Á", "A");
                fileName = fileName.Replace("É", "É");
                fileName = fileName.Replace("Í", "I");
                fileName = fileName.Replace("Ó", "O");
                fileName = fileName.Replace("Ú", "U");

                Datos.ActualizarDescargasEvalRiesgos(IF.id);
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ActionResult eliminar_evaluacionriesgos(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            Datos.EliminarEvaluacionesRiesgos(id);
            return RedirectToAction("apoyoprl", "Home");
        }
        #endregion

        #endregion

        #region catalogonormas

        public ActionResult eliminar_norma(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            Datos.EliminarNorma(id);
            return RedirectToAction("catalogo", "Home");
        }

        public ActionResult catalogo()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }
            ViewData["catalogonormas"] = Datos.ListarNormas();
            return View();
        }

        public FileResult ObtenerNorma(int id)
        {
            try
            {
                normas IF = Datos.ObtenerNormaPorID(id);
                byte[] fileBytes = System.IO.File.ReadAllBytes(IF.enlace);
                string fileName = IF.enlace.Replace(Server.MapPath("~/Normas/" + id) + "\\", "");
                fileName = fileName.Replace("á", "a");
                fileName = fileName.Replace("é", "e");
                fileName = fileName.Replace("í", "i");
                fileName = fileName.Replace("ó", "o");
                fileName = fileName.Replace("ú", "u");
                fileName = fileName.Replace("Á", "A");
                fileName = fileName.Replace("É", "É");
                fileName = fileName.Replace("Í", "I");
                fileName = fileName.Replace("Ó", "O");
                fileName = fileName.Replace("Ú", "U");
                //Datos.InsertFichero(IF);        

                Datos.ActualizarDescargas(IF.id);
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ActionResult detalle_norma(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }
            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            int idCentral = centroseleccionado.id;

            normas obj = Datos.GetDatosNorma(id);
            ViewData["norma"] = obj;

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult detalle_norma(int id, FormCollection collection, HttpPostedFileBase file)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }
            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            int idCentral = centroseleccionado.id;
            List<VISTA_ListarUsuarios> listarResponsables = new List<VISTA_ListarUsuarios>();

            normas emer = new normas();
            string formulario = collection["hdFormularioEjecutado"];

            if (formulario == "GuardarNorma")
            {
                #region guardar
                try
                {
                    if (id != 0)
                    {
                        #region actualizar
                        normas actualizar = Datos.GetDatosNorma(id);
                        actualizar.nombre_norma = collection["ctl00$MainContent$txtNombre"];
                        actualizar.codigo = collection["ctl00$MainContent$txtCodigo"];
                        actualizar.edicion_norma = collection["ctl00$MainContent$txtEdicion"];

                        if (actualizar.codigo != string.Empty && actualizar.nombre_norma != null && actualizar.edicion_norma != null)
                        {
                            Datos.ActualizarNorma(actualizar);

                            if ((file != null && file.ContentLength > 0))
                            {
                                var fileName = System.IO.Path.GetFileName(file.FileName);

                                var path = System.IO.Path.Combine(Server.MapPath("~/Normas/" + id.ToString()), fileName);
                                actualizar.enlace = path;

                                Datos.UpdateEnlaceNorma(actualizar);

                                if (Directory.Exists(Server.MapPath("~/Normas/" + id.ToString())))
                                {
                                    file.SaveAs(path);
                                }
                                else
                                {
                                    Directory.CreateDirectory(Server.MapPath("~/Normas/" + id.ToString()));
                                    file.SaveAs(path);
                                }
                            }


                            Session["EdicionNormaMensaje"] = "Información actualizada correctamente";
                            emer = Datos.GetDatosNorma(id);
                            ViewData["norma"] = emer;
                        }
                        else
                        {
                            Session["EdicionNormaError"] = "Los campos marcados con (*) son obligatorios.";
                            emer = Datos.GetDatosNorma(id);
                            ViewData["norma"] = emer;
                        }
                        return Redirect(Url.RouteUrl(new { controller = "home", action = "catalogo" }));
                        //return View();
                        #endregion
                    }
                    else
                    {
                        #region insertar
                        normas insertar = new normas();
                        insertar.nombre_norma = collection["ctl00$MainContent$txtNombre"];
                        insertar.codigo = collection["ctl00$MainContent$txtCodigo"];
                        insertar.edicion_norma = collection["ctl00$MainContent$txtEdicion"];

                        if (insertar.nombre_norma != string.Empty && insertar.codigo != null && insertar.edicion_norma != null)
                        {
                            int idForm = Datos.ActualizarNorma(insertar);

                            if ((file != null && file.ContentLength > 0))
                            {
                                var fileName = System.IO.Path.GetFileName(file.FileName);

                                var path = System.IO.Path.Combine(Server.MapPath("~/Normas/" + idForm.ToString()), fileName);
                                insertar.enlace = path;

                                Datos.UpdateEnlaceNorma(insertar);

                                if (Directory.Exists(Server.MapPath("~/Normas/" + idForm.ToString())))
                                {
                                    file.SaveAs(path);
                                }
                                else
                                {
                                    Directory.CreateDirectory(Server.MapPath("~/Normas/" + idForm.ToString()));
                                    file.SaveAs(path);
                                }
                            }

                            Session["EdicionNormaMensaje"] = "Información actualizada correctamente";

                            emer = Datos.GetDatosNorma(id);
                            ViewData["norma"] = emer;
                            return Redirect(Url.RouteUrl(new { controller = "home", action = "catalogo" }));
                            //return Redirect(Url.RouteUrl(new { controller = "home", action = "detalle_norma", id = idForm }));
                        }
                        else
                        {
                            Session["EdicionNormaError"] = "Los campos marcados con (*) son obligatorios.";
                            emer = insertar;
                            ViewData["norma"] = emer;
                            return Redirect(Url.RouteUrl(new { controller = "home", action = "catalogo"}));
                            //return View();
                        }
                        #endregion
                    }

                }
                catch (Exception ex)
                {
                    emer = Datos.GetDatosNorma(id);
                    ViewData["norma"] = emer;
                    return Redirect(Url.RouteUrl(new { controller = "home", action = "catalogo" }));
                    //return View();
                }
                #endregion
            }
            else
            {
                emer = Datos.GetDatosNorma(id);
                ViewData["norma"] = emer;
                return Redirect(Url.RouteUrl(new { controller = "home", action = "catalogo" }));
                //return View();
            }
        }

        #endregion
    }
}
