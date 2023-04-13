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
    public class NoticiasController : Controller
    {
        //
        // GET: /Noticias/

        public ActionResult noticias()
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

                if (Session["CentralElegida"] != null)
                {
                    centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
                }


                    if (centroseleccionado.tipo != 4)
                        ViewData["noticias"] = Datos.ListarNoticiasGrid(centroseleccionado.id);
                    else
                        ViewData["noticias"] = Datos.ListarNoticiasGrid();

            }
            else
            {
                return RedirectToAction("LogOn", "Account");
            }
            return View();
        }

        public ActionResult leer_noticia(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

           ViewData["noticia"] = Datos.GetNoticia(id);
           return View();
        }

        public ActionResult leer_noticia_popup(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            ViewData["noticia"] = Datos.GetNoticia(id);
            return View();
        }

        public ActionResult editar_noticia(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            if (id == 0)
            {
                noticias noti = new noticias();
                noti.validada = 0;
                MIDAS.Models.centros centroseleccionado = new MIDAS.Models.centros();
                if (Session["CentralElegida"] != null)
                {
                    centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
                    if (centroseleccionado.tipo != 4)
                        noti.organizacion = centroseleccionado.id;
                }
                noti.id = Datos.InsertarNoticia(noti);
                ViewData["noticia"] = Datos.GetNoticia(noti.id);
                return RedirectToAction("editar_noticia/" + noti.id, "Noticias");
            }
            else
            {
                ViewData["noticia"] = Datos.GetNoticia(id);
                return View();
            }

        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult editar_noticia(int id, FormCollection collection, HttpPostedFileBase file)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            string formulario = collection["hdFormularioEjecutado"];

            if (formulario == "GuardarNoticia")
            {
                #region guardar noticia
                try
                {
                    if (id != 0)
                    {
                        noticias actualizar = Datos.GetNoticia(id);

                        actualizar.titulo = collection["ctl00$MainContent$txtTitulo"];
                        actualizar.texto = collection["ctl00$MainContent$txtTexto"];
                        if (collection["ctl00$MainContent$txtExpira"] != null && collection["ctl00$MainContent$txtExpira"] != string.Empty)
                            actualizar.fechaexp = DateTime.Parse(collection["ctl00$MainContent$txtExpira"]);
                        actualizar.fecha = DateTime.Now;
                        actualizar.validada = 1;

                        if ((file != null && file.ContentLength > 0))
                        {
                            var fileName = System.IO.Path.GetFileName(file.FileName);

                            var path = System.IO.Path.Combine(Server.MapPath("~/Cabeceras/" + id.ToString()), fileName);
                            actualizar.cabecera = fileName;

                            if (Directory.Exists(Server.MapPath("~/Cabeceras/" + id.ToString())))
                            {
                                file.SaveAs(path);
                            }
                            else
                            {
                                Directory.CreateDirectory(Server.MapPath("~/Cabeceras/" + id.ToString()));
                                file.SaveAs(path);
                            }
                        }


                        if (actualizar.titulo != string.Empty && actualizar.texto != string.Empty)
                            Datos.ActualizarNoticia(actualizar);
                        else
                        {
                            Session["EdicionNoticiaError"] = "Los campos marcados con (*) son obligatorios.";
                            return View();
                        }

                        return RedirectToAction("noticias", "Noticias");
                    }
                    else
                    {
                        noticias insertar = new noticias();
                        insertar.titulo = collection["ctl00$MainContent$txtTitulo"];
                        insertar.texto = collection["ctl00_MainContent_txtTexto"];
                        if (collection["ctl00$MainContent$txtExpira"] != null && collection["ctl00$MainContent$txtExpira"] != string.Empty)
                            insertar.fechaexp = DateTime.Parse(collection["ctl00$MainContent$txtExpira"]);
                        insertar.fecha = DateTime.Now;
                        insertar.validada = 1;

                        MIDAS.Models.centros centroseleccionado = new MIDAS.Models.centros();
                        if (Session["CentralElegida"] != null)
                        {
                            centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
                            if (centroseleccionado.tipo != 4)
                                insertar.organizacion = centroseleccionado.id;
                        }

                        Datos.InsertarNoticia(insertar);

                        return RedirectToAction("noticias", "Noticias");
                    }

                }
                catch (Exception ex)
                {
                    ViewData["noticia"] = Datos.GetNoticia(id);
                    return View();
                }
                #endregion
            }

            ViewData["noticia"] = Datos.GetNoticia(id);

            return View();
        }

        public ActionResult imagenes_noticia()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            ViewData["imagenesnoticia"] = Datos.ListarImagenes(int.Parse(Session["noticia"].ToString()));

            return View();
        }

        [HttpPost]
        public ActionResult imagenes_noticia(HttpPostedFileBase file, FormCollection collection)
        {

            if (file != null && file.ContentLength > 0)
            {
                var fileName = System.IO.Path.GetFileName(file.FileName);


                string id = "0";

                if (Session["noticia"] != null)
                    id = Session["noticia"].ToString();

                var path = System.IO.Path.Combine(Server.MapPath("~/Imagenes"), fileName);

                if (Directory.Exists(Server.MapPath("~/Imagenes")))
                {
                    file.SaveAs(path);
                }
                else
                {
                    Directory.CreateDirectory(Server.MapPath("~/Imagenes"));
                    file.SaveAs(path);
                }


                imagenes IF = new imagenes();

                IF.link = "http://novotecsevilla.westeurope.cloudapp.azure.com/evr/Imagenes/" + fileName;
                IF.idnoticia = int.Parse(id);

                Datos.InsertImagen(IF);

            }
            ViewData["imagenesnoticia"] = Datos.ListarImagenes(int.Parse(Session["noticia"].ToString()));
            return RedirectToAction("imagenes_noticia", "Noticias");
        }

        public ActionResult eliminar_noticia(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            int carpeta = Datos.EliminarNoticia(id);

            if (Directory.Exists(Server.MapPath("~/Cabeceras/" + carpeta.ToString())))
            {
                Directory.Delete(Server.MapPath("~/Cabeceras/" + carpeta.ToString()), true);
            }
            return RedirectToAction("noticias", "noticias");
        }
    }
}
