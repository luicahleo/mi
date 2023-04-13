using DocumentFormat.OpenXml.Wordprocessing;

using MIDAS.Classes.Personas;
using MIDAS.Helpers;
using MIDAS.Models;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Script.Serialization;

using static MIDAS.Models.Enum;

namespace MIDAS.Controllers
{
    public class PersonasController : BaseController
    {
        #region analisis_datos
        [HttpGet]
        public ActionResult analisis_datos()
        {

            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            if (ViewData["filtrosTotales"] != null)
            {
                return View();
            }

            bool existeRegistros = Datos.ExisteRegistroEnTablaPersonas();
            if (existeRegistros)
            {
                Session["analizarPersonas"] = true;
            }
            else
            {
                Alert("No puede analizar datos si la tabla maestro Personas esta vacia", NotificationType.error, 2000);
                Session["analizarPersonas"] = false;
                return View();
            }

            List<personas> listaPersonas = Datos.ListaPersonas();



            List<string> listaActividad = Datos.ListaPersonas_Actividad();
            List<string> listaCentroDeTrabajo = Datos.ListaPersonas_CentroDeTrabajo();

            ViewData["listaActividad"] = listaActividad;
            ViewData["listaCentroTrabajo"] = listaCentroDeTrabajo;



            return View();
        }

        [HttpPost]
        public JsonResult FiltrosDatos(string filtrosJson)
        {
            log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(Server.MapPath("~/log4net.config")));

            try
            {


                var serializer = new JavaScriptSerializer();

                Filtros filtros = JsonConvert.DeserializeObject<Filtros>(filtrosJson);


                List<personas> listaPersonas = Datos.ListaPersonas();

                List<personas> listaPerfilRiesgoPersonalizada = new List<personas>();

                if (filtros.actividad.Count > 0 && filtros.centroTrabajo.Count == 0)
                {
                    var listaActividades = listaPersonas.Where(persona => filtros.actividad.Contains(persona.Actividad)).ToList();
                    listaPerfilRiesgoPersonalizada = listaActividades.GroupBy(x => x.Perfil_de_riesgo).Select(group => group.First()).ToList();


                }
                else if (filtros.actividad.Count == 0 && filtros.centroTrabajo.Count > 0)
                {
                    var listaCentroTrabajo = listaPersonas.Where(persona => filtros.centroTrabajo.Contains(persona.Centro_de_trabajo)).ToList();
                    listaPerfilRiesgoPersonalizada = listaCentroTrabajo.GroupBy(x => x.Perfil_de_riesgo).Select(group => group.First()).ToList();
                }
                else
                {
                    listaPerfilRiesgoPersonalizada = listaPersonas.Where(persona => filtros.actividad.Contains(persona.Actividad) && filtros.centroTrabajo.Contains(persona.Centro_de_trabajo)).ToList();
                }
                Session["listaPersonasFiltradas"] = listaPerfilRiesgoPersonalizada;

                string listaFinalJson = serializer.Serialize(listaPerfilRiesgoPersonalizada);

                return Json(listaFinalJson);
            }
            catch (Exception ex)
            {
                new EscribirLog("Error en " +
                            ex.Message, true, this.ToString(), "FiltrosDatos");
                return Json(null);

            }
        }

        [HttpPost]
        public JsonResult GuardarElementosSeleccionados(List<string> listaNumeroEmpleadoSeleccionados)
        {
            if (Session["usuario"] == null)
            {
                //return RedirectToAction("LogOn", "Account");
            }
            var serializer = new JavaScriptSerializer();

            VISTA_ObtenerUsuario user = Datos.ObtenerUsuarioActual(Session["usuario"].ToString());


            List<int> listaIdGuardado = new List<int>();

            if (listaNumeroEmpleadoSeleccionados != null)
            {

                foreach (string numeroEmpleado in listaNumeroEmpleadoSeleccionados)
                {
                    int idGuardado = Datos.GuardarPersonaPorNumeroEmpleado(numeroEmpleado, user.idUsuario);
                    listaIdGuardado.Add(idGuardado);
                }

            }
            else
            {
                listaIdGuardado.Add(-1); // este valor solo representa un espacio en el array de respuesta en JS, para visualizar el mensaje de guardado
            }
            string listaIdGuardadoJson = serializer.Serialize(listaIdGuardado);


            return Json(listaIdGuardadoJson);
        }

        #endregion analisis_datos

        #region lista_final
        [HttpGet]
        public ActionResult lista_final()
        {
            log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(Server.MapPath("~/log4net.config")));

            try
            {
                if (Session["usuario"] == null)
                {
                    return RedirectToAction("LogOn", "Account");
                }

                VISTA_ObtenerUsuario user = Datos.ObtenerUsuarioActual(Session["usuario"].ToString());

                List<lista_final_personas> listaFinal = Datos.Listar_ListaFinalPersonas_PorIdUsuario(user.idUsuario);
                listaFinal.Reverse();

                ViewData["listaFinal"] = listaFinal;

                return View();
            }
            catch (Exception ex)
            {
                new EscribirLog("Error en " +
                            ex.Message, true, this.ToString(), "lista_final");
                return View();

            }
        }
        [HttpGet]
        public ActionResult eliminar_persona_listaFinal(int id)
        {
            log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(Server.MapPath("~/log4net.config")));
            try
            {
                if (Session["usuario"] == null)
                {
                    return RedirectToAction("LogOn", "Account");
                }

                var registro = Datos.EliminarPersonaDeListaFinalPorId(id);
                if (registro)
                {
                    Alert("Registro Persona eliminada de lista final", NotificationType.success, 2000);
                }
                else
                {
                    Alert("No se pudo eliminar el registro de lista final", NotificationType.error, 2000);
                }

                return RedirectToAction("lista_final", "Personas");
            }
            catch (Exception ex)
            {
                new EscribirLog("Error en " +
                            ex.Message, true, this.ToString(), "eliminar_persona_listaFinal");
                return View();

            }
        }

        [HttpPost]
        public JsonResult ListaFinalPorUsuario()
        {
            if (Session["usuario"] == null)
            {
                //return RedirectToAction("LogOn", "Account");
            }
            var serializer = new JavaScriptSerializer();

            VISTA_ObtenerUsuario user = Datos.ObtenerUsuarioActual(Session["usuario"].ToString());

            var lista_final_personas = Datos.Listar_ListaFinalPersonas_PorIdUsuario(user.idUsuario);

            var listaFinalPersonasPorNumeroEmpleado = lista_final_personas.Select(x => x.Nº_Empleado).ToList();
            string listaFinalJson = serializer.Serialize(listaFinalPersonasPorNumeroEmpleado);


            return Json(listaFinalJson);
        }

        [HttpPost]
        public JsonResult BorrarPersonaDeListaFinal(string valorParaBorrar)
        {
            if (Session["usuario"] == null)
            {
                //return RedirectToAction("LogOn", "Account");
            }
            var serializer = new JavaScriptSerializer();

            VISTA_ObtenerUsuario user = Datos.ObtenerUsuarioActual(Session["usuario"].ToString());

            bool lista_final_personas = Datos.EliminarPersonaDeListaFinalPorNumEmpleado_IdUsuario(valorParaBorrar, user.idUsuario);

            string listaFinalJson = serializer.Serialize(lista_final_personas);


            return Json(listaFinalJson);
        }


        #endregion lista_final



    }
}