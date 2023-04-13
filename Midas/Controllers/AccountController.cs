using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;

using MIDAS.Models;

namespace MIDAS.Controllers
{

    [HandleError]
    public class AccountController : Controller
    {

        // This constructor is used by the MVC framework to instantiate the controller using
        // the default forms authentication and membership providers.

        public AccountController()
        {
        }

        public ActionResult LogOn()
        {

            return View();
        }

        public ActionResult SeleccionCentral(string idTipo, string idCentral)
        {
            int idUsuario;

            if (Session["idUsuario"] != null)
            {
                idUsuario = int.Parse(Session["idUsuario"].ToString());
                int perfil = int.Parse(Session["perfil"].ToString());

                //List<SelectListItem> datos = new List<SelectListItem>();
                //foreach (tipocentral item in Datos.ListarTipos())
                //{
                //    SelectListItem it = new SelectListItem();
                //    it.Text = item.nombre;
                //    it.Value = item.id.ToString();
                //    datos.Add(it);
                //}

                List<tecnologias> listaTecnologias = new List<tecnologias>();
                tecnologias inicial = new tecnologias();
                inicial.id = 0;
                inicial.nombre = "Seleccione Tecnología";
                listaTecnologias.Add(inicial);
                listaTecnologias.AddRange(Datos.ListarTecnologias());

                ViewData["tecnologias"] = listaTecnologias;
                ViewData["agrupaciones"] = Datos.ListarAgrupaciones();
                ViewData["zonas"] = Datos.ListarZonas();

                if (!string.IsNullOrEmpty(idTipo))
                {
                    int tipo = int.Parse(idTipo);
                    ViewData["centrosasignados"] = Datos.ListarCentros(tipo);
                    ViewData["tiposCentros"] = Datos.ListarCentros(tipo);

                }

                //ViewData["centrosasignados"] = Datos.ListarCentrosSedeCentral();
                ViewData["tiposCentros"] = Datos.ListarTipos();
                /*CODIGO COMENTADO PORQUE ANTES SE LISTABAN LOS CENTROS DEPENDIENDO DE USUARIO. PREGUNTAR A JESUS O ANDRES
                //if (perfil == 1)
                //{
                    
                //    
                //}
                //else
                //{                    
                //   ViewData["centrosasignados"] = Datos.ListarCentrosAsignados(idUsuario);                    
                //}
                */
            }

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings",
            Justification = "Needs to take same parameter type as Controller.Redirect()")]
        public ActionResult LogOn(string userName, string password, string returnUrl)
        {
            if (!ValidateLogOn(userName, password))
            {
                return View();
            }

            if (!String.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                int perfil = int.Parse(Session["perfil"].ToString());

                int idUsuario = int.Parse(Session["idUsuario"].ToString());
                List<VISTA_ListarCentrosAsignados> centros = Datos.ListarCentrosAsignados(idUsuario);

                if (centros.Count > 1 || (perfil == 1 || perfil == 2))
                {
                    //return RedirectToAction("SeleccionCentral", "Account");
                    return RedirectToAction("Principal", "Home");
                }

                if (centros.Count == 1)
                {
                    Session["CentralElegida"] = centros[0].id;
                    return RedirectToAction("Principal", "Home");
                }
                else
                {
                    return RedirectToAction("LogOn", "Account");
                }

            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SeleccionCentral(FormCollection collection, string idTipo, string idCentral)
        {
            //if (string.IsNullOrEmpty(idTipo) || string.IsNullOrEmpty(idCentral))
            //{
            //    int tipo = int.Parse(idTipo);
            //    ViewData["centrosportipo"] = Datos.ListarCentros(tipo);
            //    return RedirectToAction("Account","SeleccionCentral",new {idCentral=idCentral, idTipo=idTipo });
            //    //return View();
            //}
            //else
            {
                Session["CentralElegida"] = collection["ctl00$MainContent$ddlCentros"];
                Session["TecnologiaElegida"] = collection["ctl00$MainContent$ddlTecnologias"];

                var listaDescripcionCentro = MIDAS.Models.Datos.listarDescripcionCentro(int.Parse(collection["ctl00$MainContent$ddlCentros"]));
                var listaVersionMatriz = MIDAS.Models.Datos.listarMatrizVersion(int.Parse(collection["ctl00$MainContent$ddlCentros"]));

                if (Session["DescripcionCentro"] != null)
                    Session.Remove("DescripcionCentro");
                if (Session["VersionMatriz"] != null)
                    Session.Remove("VersionMatriz");

                if (listaVersionMatriz.Count > 0)
                {
                    Session["VersionMatriz"] = "existe";
                }
                if (listaDescripcionCentro.Count > 0)
                {
                    Session["DescripcionCentro"] = "existe";
                }
                //return Redirect(Url.RouteUrl(new { controller = "home", action = "Principal", id = int.Parse(collection["ctl00$MainContent$ddlCentros"]) }));
                return RedirectToAction("Principal", "Home");
            }
        }

        public JsonResult ObtenerCentros(string idTipo)
        {

            int tipo = 0;

            List<SelectListItem> datos = null;
            try
            {
                tipo = int.Parse(idTipo);
                datos = new List<SelectListItem>();
                foreach (centros item in Datos.ListarCentros(tipo))
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

        public JsonResult ObtenerCentrosZonas(string idTipo, string idZona)
        {

            int tipo = 0;
            int zona = 0;
            List<SelectListItem> datos = null;
            try
            {
                tipo = int.Parse(idTipo);
                zona = int.Parse(idZona);
                datos = new List<SelectListItem>();
                foreach (centros item in Datos.ListarCentrosTiposZonasV2(tipo, zona))
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
        public JsonResult ObtenerCentrosAgrupacion(string idTipo, string idAgrupacion)
        {

            int tipo = 0;
            int Idagrupacion = 0;


            List<SelectListItem> datos = null;
            try
            {
                tipo = int.Parse(idTipo);
                Idagrupacion = int.Parse(idAgrupacion);
                datos = new List<SelectListItem>();
                foreach (centros item in Datos.ListarCentrosPorAgrupacion(Idagrupacion))
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
                if (tipo == 7 || tipo == 8 || tipo == 9)
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
        public JsonResult ObtenerCentrosPorTecnologias(string idTipo)
        {

            int tipo = 0;

            List<SelectListItem> datos = null;
            try
            {
                tipo = int.Parse(idTipo);
                datos = new List<SelectListItem>();
                foreach (centros item in Datos.ListarCentros(tipo))
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

        public ActionResult LogOff()
        {

            Session["usuario"] = null;

            return RedirectToAction("LogOn", "Account");
        }

        #region Validation Methods


        private bool ValidateLogOn(string userName, string password)
        {
            if (String.IsNullOrEmpty(userName))
            {
                ModelState.AddModelError("username", "Se debe ingresar Usuario.");
            }
            if (String.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("password", "Se debe ingresar Contraseña.");
                Session["usuario"] = null;
                return ModelState.IsValid;
            }

            if (Datos.SignIn(userName, password))
            {
                Session["usuario"] = userName;
                VISTA_ObtenerUsuario user = ((VISTA_ObtenerUsuario)Datos.ObtenerUsuarioActual(userName));
                Session["idUsuario"] = user.idUsuario;
                Session["perfil"] = user.perfil;

                //cambiar
                centros org = Datos.ObtenerCentro(1);

                usuarios permisosUser = Datos.ObtenerUsuario(user.idUsuario);

                if (user.baja == true)
                {
                    Session["usuario"] = null;
                    ModelState.AddModelError("_FORM", "El usuario se encuentra desactivado. Por favor, contacte con el administrador de la herramienta.");
                }

            }
            else
            {
                Session["usuario"] = null;
                ModelState.AddModelError("_FORM", "Usuario o Contraseña Incorrectos.");
            }

            return ModelState.IsValid;
        }




        #endregion
    }



}
