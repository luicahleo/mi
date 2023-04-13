using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

using MIDAS.Models;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MIDAS.Controllers
{
    public class RiesgosController : BaseController
    {
        //
        // GET: /Riesgos/

        public ActionResult menu_riesgos()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            if (Session["CentralElegida"] != null)
            {
                centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));

                List<tecnologias> tecnologiasCentro = Datos.ListarTecnologiasPorCentro(centroseleccionado.id);
                ViewData["tecnologias"] = tecnologiasCentro.OrderBy(x => x.id);
            }
            else
                return RedirectToAction("LogOn", "Account");

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post), ValidateInput(false)]
        public ActionResult editar_informacion(FormCollection collection, string submit)
        {
            try
            {
                if (Session["usuario"] == null)
                {
                    return RedirectToAction("LogOn", "Account");
                }

                if (!string.IsNullOrEmpty(collection["txt_desc"].ToString()))
                {
                    centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
                    descripcion_centro descripcion = new descripcion_centro();
                    descripcion.descripcion = collection["txt_desc"].ToString();
                    descripcion.id_centro = centroseleccionado.id;
                    FreeTextBoxControls.FreeTextBox pruebad = new FreeTextBoxControls.FreeTextBox();
                    pruebad.Text = collection["txt_desc"].ToString();
                    string cadenna = pruebad.HtmlStrippedText;
                    descripcion.descripcion_texto = cadenna;

                    var id_descripcion = Datos.ActualizarDescripcion(descripcion);
                    if (id_descripcion != 0)
                    {
                        Session["DescripcionCentro"] = "existe";
                    }
                }
            }
            catch (Exception ex)
            {

                RedirectToAction("Principal", "Home");

            }
            return RedirectToAction("Principal", "Home");
        }


        public ActionResult editar_informacion()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            descripcion_centro desc = Datos.ObtenerInformacionCentral(centroseleccionado.id);
            if (desc != null)
            {
                ViewData["informacion_centro"] = desc.descripcion;
            }



            return View();
        }

        public ActionResult editar_persona()
        {
            //if (Session["usuario"] == null)
            //{
            //    return RedirectToAction("LogOn", "Account");
            //}

            //centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            //descripcion_centro desc = Datos.ObtenerInformacionCentral(centroseleccionado.id);
            //if (desc != null)
            //{
            //    ViewData["informacion_centro"] = desc.descripcion;
            //}



            return View();
        }

        public ActionResult editor_summernote()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            descripcion_centro desc = Datos.ObtenerInformacionCentral(centroseleccionado.id);
            if (desc != null)
            {
                ViewData["informacion_centro"] = desc.descripcion;
            }

            return View();
        }
        public ActionResult seleccionar_tecnologia()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            if (Session["CentralElegida"] != null)
            {
                centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));

                List<tecnologias> tecnologiasCentro = Datos.ListarTecnologiasPorCentro(centroseleccionado.id);
                ViewData["tecnologias_centro"] = tecnologiasCentro.OrderBy(x => x.nombre);

                List<tecnologias> todasTecnologias = Datos.ListarTecnologias();
                //todasTecnologias.RemoveAt(4);                                            //quitamos la tecnologia GENERAL de la lista
                ViewData["tecnologias"] = todasTecnologias.OrderBy(x => x.id);
            }
            else
                return RedirectToAction("LogOn", "Account");

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult seleccionar_tecnologia(FormCollection form)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }
            var prueba = form["ctl00$MainContent$1"];
            if (Session["CentralElegida"] != null)
            {
                List<int> lista = new List<int>();
                lista.Add(1);
                lista.Add(2);
                return RedirectToAction("matriz_riesgos", "Riesgos", new { id = 0, prueba = lista });
            }

            //habilitamos el boton de Medidas Preventivas 

            var listaVersionMatriz = (string)Session["CentralElegida"];
            //var listaVersionMatriz = MIDAS.Models.Datos.listarMatrizVersion(int.Parse(collection["ctl00$MainContent$ddlCentros"]));

            //if (listaVersionMatriz.Count > 0)
            //{
            //    Session["ExisteMatriz"] = "existe";
            //}


            return View();
        }

        public ActionResult medidas_preventivas()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            if (Session["CentralElegida"] != null)
            {
                centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));

                var versionUltima = Datos.listarMatrizVersion(centroseleccionado.id).FirstOrDefault();

                //para obtener la version, obtener la que es borrador, y luego utilizarla para buscar la matriz
                List<matriz_centro> matrices = Datos.listarMatrizCentro(centroseleccionado.id, 0);
                //var agrupadosPorCentro = matrices.GroupBy(x => x.version).Distinct();

                List<tecnologias> listaTecnologias = new List<tecnologias>();
                tecnologias inicial = new tecnologias();
                inicial.id = 0;
                inicial.nombre = "Seleccione Tecnología";
                listaTecnologias.Add(inicial);
                listaTecnologias.AddRange(Datos.ListarTecnologias());
                ViewData["tecnologias"] = listaTecnologias;
                //List<medidas_generales> listaGenerales = Datos.ListarMedidasGenerales(centroseleccionado.id, 0);
                List<medidas_generales> listaGenerales = Datos.ListarMedidasGenerales();
                List<parametrica_medidas> listaParametricaMedidas = Datos.ListarParametricaMedidas();

                ViewData["parametricaMedidas"] = listaParametricaMedidas.Where(x => x.id_centro == centroseleccionado.id);
                ViewData["medidasRiesgosGenerales"] = listaGenerales;
                ViewData["apartadosRiesgosGenerales"] = Datos.ListarApartadosGenerales();
                List<tipos_riesgos> riesgosMedidas = Datos.ListarTiposRiesgosMedidas(centroseleccionado.id);
                ViewData["riesgos"] = riesgosMedidas;


                ViewData["matriz"] = matrices;

                //if (Datos.EstaVersionEsBorrador(centroseleccionado.id))
                //{

                //}
                //else
                //{
                //    ViewData["riesgos"] = null;
                //}
                ViewData["situaciones"] = Datos.ListarSituaciones();
                // List<medidas_preventivas> sa= Datos.ListarMedidas().Where(x => x.id_centro == 0 || x.id_centro == centroseleccionado.id).ToList();
                ViewData["medidas"] = Datos.ListarMedidas().Where(x => x.id_centro == 0 || x.id_centro == centroseleccionado.id).ToList();

                //tecnologia 1
                string tecnologia = "1";
                if (Session["TecnologiaElegida"] != null)
                {
                    tecnologia = Session["TecnologiaElegida"].ToString();
                }

                ViewData["tecnologia"] = tecnologia;


                //Obtener los apartados de las medidas asociadas a los riesgos 
                List<riesgos_medidas> riesgo_medida = Datos.ListarMedidasRiesgo(centroseleccionado.id);
                List<riesgos_medidas> riesgo_medida_tipo1 = riesgo_medida.Where(x => x.imagen_grande == null || x.imagen_grande == 0).ToList();
                List<riesgos_medidas> riesgo_medida_tipo2 = riesgo_medida.Where(x => x.imagen_grande == 1).ToList();
                ViewData["medidasRiesgos"] = riesgo_medida_tipo1;
                ViewData["medidasRiesgosImagen"] = riesgo_medida_tipo2;
                List<medidas_apartadosV2> apartados = Datos.ListarApartadosV2();
                var apartadosMedidas = riesgo_medida.Select(x => x.id_apartado).ToList().Distinct();

                List<medidas_apartadosV2> apartadosRiesgo = new List<medidas_apartadosV2>();
                foreach (medidas_apartadosV2 item in apartados)
                {
                    if (apartadosMedidas.Contains(item.id))
                    {
                        apartadosRiesgo.Add(item);
                    }
                }
                ViewData["centroSeleccionado"] = centroseleccionado.id;
                ViewData["apartadosRiesgos"] = apartadosRiesgo;
            }
            else
            {
                //Devolver mensaje de que no hay matrices
                return RedirectToAction("LogOn", "Account");
            }

            return View();
        }


        public ActionResult lista_matrices()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            if (Session["CentralElegida"] != null)
            {
                centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
                List<version_matriz> matrices = Datos.listarMatrizVersion(centroseleccionado.id);

                ViewData["versiones"] = matrices;

                if (matrices.Count > 0)
                {
                    Session["VersionMatriz"] = "existe";
                }
            }
            else
            {
                //Devolver mensaje de que no hay matrices
                return RedirectToAction("LogOn", "Account");
            }


            return View();
            //return RedirectToAction("Principal", "Home");

        }
        public ActionResult gestion_riesgos()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            int idCentral = centroseleccionado.id;
            ViewData["riesgos"] = Datos.ListarRiesgos(idCentral);

            #region generacion fichero
            if (Session["nombreArchivo"] != null && Session["nombreArchivo"].ToString() != "")
            {
                if (Session["nombreArchivo"].ToString().Contains("xlsx"))
                {
                    Response.ContentType = "application/msexcel";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=ExportacionRiesgos.xlsx");
                    Response.TransmitFile(Session["nombreArchivo"].ToString());
                    Response.End();
                }
                Session["nombreArchivo"] = "";
            }
            #endregion

            return View();
        }

        //[HttpPost]
        //public ActionResult matriz_riesgos(List<int> id)
        //{
        //    if (Session["usuario"] == null)
        //    {
        //        return RedirectToAction("LogOn", "Account");
        //    }

        //    return View();
        //}
        //[HttpPost]
        //public ActionResult matriz_riesgos(int id)
        //{
        //    if (Session["usuario"] == null)
        //    {

        //        return RedirectToAction("LogOn", "Account");
        //    }
        //    //string[] recibe = (string[])TempData["Create"];
        //    string[] recibe = null;
        //    if (Session["tecnologiasSeleccionadas"] != null)
        //    {
        //        recibe = (string[])Session["tecnologiasSeleccionadas"];
        //        Session["tecnologiasSeleccionadas"] = null;
        //    }

        //    int version = 0;
        //    if (id != null)
        //    {
        //        version = (int)id;
        //    }
        //    if (version == 0)
        //    {


        //        centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));

        //        List<tecnologias> tecnologiasCentro = Datos.ListarTecnologiasPorCentro(centroseleccionado.id);

        //        List<areanivel1> areasVersion = Datos.ListarAreasMaestro(version, 0);
        //        List<areanivel1> areas = Datos.ListarAreas();
        //        areasVersion = areasVersion.Where(x => x.id_centro == centroseleccionado.id).OrderBy(x => x.id).ToList();
        //        List<areanivel2> sistemas = Datos.ListarSistema();
        //        List<areanivel3> equipos = Datos.ListarEquipos();

        //        List<areanivel1> areasPrede = new List<areanivel1>();



        //        //COMPROBAR QUE TENEMOS LOS DATOS DE TECNOLOGIAS
        //        List<string> tecnologiasSelec = new List<string>();
        //        if (recibe != null)
        //        {
        //            foreach (string item in recibe)
        //            {
        //                tecnologiasSelec.Add(item);
        //            }
        //        }



        //        List<tecnologias> tecnologias = Datos.ListarTecnologiasid(tecnologiasSelec).OrderBy(x => x.id).ToList();

        //        //string[] tecnologiasSeleccionadas = { "1","2", "6" };
        //        if (tecnologias.Count() > 0)
        //        {
        //            foreach (tecnologias item in tecnologias)
        //            {
        //                int tecnologia = item.id;
        //                List<areanivel1> ar = Datos.listarAreasInicial(tecnologia);
        //                if (ar != null)
        //                {
        //                    areasPrede.AddRange(ar);
        //                }
        //            }

        //        }
        //        //else
        //        //{
        //        //    //if (version == 0)
        //        //    //{
        //        //    //    int idVersionNueva = Datos.ActualizarVersionMatriz(centroseleccionado.id);
        //        //    //    return RedirectToAction("matriz_riesgos", new { id = idVersionNueva });
        //        //    //}

        //        //    //ViewData["matriz_inicial"] = Datos.listarMatrizInicial();
        //        //    //areasVersion = Datos.ListarAreas(version);
        //        //    //areas = Datos.ListarAreas();
        //        //    //sistemas = Datos.ListarSistema();
        //        //    //equipos = Datos.ListarEquipos();
        //        //}

        //        int idVersionNueva = Datos.ActualizarVersionMatriz(centroseleccionado.id);
        //        ViewData["areas"] = areasPrede;
        //        //ViewData["tecnologias"] = tecnologiasCentro.OrderBy(x => x.id);

        //        ViewData["tecnologias"] = tecnologias;
        //        areasVersion = areasVersion.Where(x => x.id_centro == centroseleccionado.id).OrderBy(x => x.id).ToList();
        //        ViewData["tipos_riesgos"] = Datos.ListarTiposRiesgos();
        //        ViewData["matriz_centro"] = Datos.listarMatrizCentro(centroseleccionado.id, version);

        //        //ViewData["areas"] = areasVersion;
        //        ViewData["sistemas"] = sistemas;
        //        ViewData["equipos"] = equipos;
        //        ViewData["version"] = idVersionNueva;


        //        List<matriz_centro> matrizNueva = new List<matriz_centro>();

        //        foreach (tecnologias tecno in tecnologias)
        //        {
        //            matriz_centro mc = new matriz_centro();
        //            foreach (areanivel1 area in areasPrede)
        //            {
        //                if (area.id_tecnologia == tecno.id)
        //                {
        //                    areanivel1 copiaArea = new areanivel1();
        //                    copiaArea.nombre = area.nombre;
        //                    copiaArea.codigo = area.codigo;
        //                    copiaArea.id_centro = centroseleccionado.id;
        //                    copiaArea.id_tecnologia = tecno.id;
        //                    int id_area_seleccionada = Datos.ActualizarArea(copiaArea);
        //                    copiaArea.id = id_area_seleccionada;

        //                    foreach (VISTA_tipos_riesgos riesgo in Datos.ListarTiposRiesgos())
        //                    {

        //                        matriz_centro matriz = new matriz_centro();
        //                        matriz.id_centro = centroseleccionado.id;
        //                        matriz.id_areanivel1 = copiaArea.id;
        //                        matriz.id_areanivel2 = null;
        //                        matriz.id_areanivel3 = null;
        //                        matriz.id_tecnologia = tecno.id;
        //                        matriz.id_riesgo = riesgo.id;
        //                        matriz.activo = false;
        //                        matriz.version = idVersionNueva;
        //                        matrizNueva.Add(matriz);

        //                    }
        //                    if (area.id_tecnologia == tecno.id)
        //                    {
        //                        Datos.InsertarMatriz(matrizNueva);
        //                        matrizNueva.Clear();
        //                    }

        //                    foreach (areanivel2 sistema in area.areanivel2)
        //                    {
        //                        areanivel2 copiaSistema = new areanivel2();
        //                        copiaSistema.nombre = sistema.nombre;
        //                        copiaSistema.codigo = sistema.codigo;
        //                        copiaSistema.id_areanivel1 = copiaArea.id;
        //                        int id_sistema_seleccionado = Datos.ActualizarSistema(copiaSistema);
        //                        copiaSistema.id = id_sistema_seleccionado;

        //                        foreach (VISTA_tipos_riesgos riesgo in Datos.ListarTiposRiesgos())
        //                        {

        //                            matriz_centro matriz = new matriz_centro();
        //                            matriz.id_centro = centroseleccionado.id;
        //                            matriz.id_areanivel1 = null;
        //                            matriz.id_areanivel2 = copiaSistema.id;
        //                            matriz.id_areanivel3 = null;
        //                            matriz.id_tecnologia = tecno.id;
        //                            matriz.id_riesgo = riesgo.id;
        //                            matriz.activo = false;
        //                            matriz.version = idVersionNueva;
        //                            matrizNueva.Add(matriz);

        //                        }
        //                        if (area.id_tecnologia == tecno.id)
        //                        {
        //                            Datos.InsertarMatriz(matrizNueva);
        //                            matrizNueva.Clear();
        //                        }
        //                        foreach (areanivel3 equipo in sistema.areanivel3)
        //                        {
        //                            areanivel3 copiaEquipo = new areanivel3();
        //                            copiaEquipo.nombre = equipo.nombre;
        //                            copiaEquipo.codigo = equipo.codigo;
        //                            copiaEquipo.id_areanivel2 = copiaSistema.id;

        //                            int id_equipo_seleccionado = Datos.ActualizarEquipo(copiaEquipo);
        //                            copiaSistema.id = id_equipo_seleccionado;

        //                            foreach (VISTA_tipos_riesgos riesgo in Datos.ListarTiposRiesgos())
        //                            {

        //                                matriz_centro matriz = new matriz_centro();
        //                                matriz.id_centro = centroseleccionado.id;
        //                                matriz.id_areanivel1 = null;
        //                                matriz.id_areanivel2 = null;
        //                                matriz.id_areanivel3 = copiaEquipo.id;
        //                                matriz.id_tecnologia = tecno.id;
        //                                matriz.id_riesgo = riesgo.id;
        //                                matriz.activo = false;
        //                                matriz.version = idVersionNueva;
        //                                matrizNueva.Add(matriz);

        //                            }
        //                            if (area.id_tecnologia == tecno.id)
        //                            {
        //                                Datos.InsertarMatriz(matrizNueva);
        //                                matrizNueva.Clear();
        //                            }
        //                        }
        //                    }
        //                }

        //            }

        //        }
        //        RedirectToAction("matriz_riesgos/" + idVersionNueva, "Riesgos");

        //    }
        //    else
        //    {



        //        ViewData["tipos_riesgos"] = Datos.ListarTiposRiesgos();
        //        centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));

        //        if (version == 0)
        //        {
        //            int idVersionNueva = Datos.ActualizarVersionMatriz(centroseleccionado.id);
        //            return RedirectToAction("matriz_riesgos", new { id = idVersionNueva });
        //        }

        //        //ViewData["matriz_inicial"] = Datos.listarMatrizInicial();
        //        ViewData["matriz_centro"] = Datos.listarMatrizCentro(centroseleccionado.id, version);

        //        List<tecnologias> tecnologiasCentro = Datos.ListarTecnologiasPorCentro(centroseleccionado.id);
        //        ViewData["tecnologias"] = tecnologiasCentro.OrderBy(x => x.id);


        //        List<areanivel1> areasVersion = Datos.ListarAreas(version);
        //        List<areanivel1> areas = Datos.ListarAreas();
        //        areasVersion = areasVersion.Where(x => x.id_centro == centroseleccionado.id).OrderBy(x => x.id).ToList();
        //        List<areanivel2> sistemas = Datos.ListarSistema();
        //        List<areanivel3> equipos = Datos.ListarEquipos();

        //        ViewData["areas"] = areasVersion;
        //        ViewData["sistemas"] = sistemas;
        //        ViewData["equipos"] = equipos;
        //        ViewData["version"] = version;
        //    }
        //    return View();
        //}


        //[HttpGet]
        //public ActionResult matriz_riesgos_criticos(int? id)
        //{
        //	if (Session["usuario"] == null)
        //	{

        //		return RedirectToAction("LogOn", "Account");
        //	}
        //	//string[] recibe = (string[])TempData["Create"];
        //	string[] recibe = null;
        //	string versionDesde = null;
        //	if (Session["tecnologiasSeleccionadas"] != null)
        //	{
        //		recibe = (string[])Session["tecnologiasSeleccionadas"];
        //		Session["tecnologiasSeleccionadas"] = null;
        //	}

        //	if (Session["versionDesde"] != null)
        //	{
        //		versionDesde = Session["versionDesde"].ToString();
        //		Session["versionDesde"] = null;
        //	}
        //	centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
        //	VISTA_ObtenerUsuario user = Datos.ObtenerUsuarioActual(Session["usuario"].ToString());
        //	int version = 0;
        //	if (id != null)
        //	{
        //		version = (int)id;
        //	}
        //	if (version == 0 || versionDesde != null)
        //	{
        //		if (versionDesde != null)
        //		{

        //			int versionNueva = Datos.ActualizarVersionMatriz(centroseleccionado.id, Session["usuario"].ToString());

        //			List<tecnologias> tecnologiasVersionAnterior = Datos.ListarTecnologiasPorVersion(centroseleccionado.id, int.Parse(versionDesde));
        //			List<areanivel1> areasVersionAnterior = Datos.ListarAreas(int.Parse(versionDesde));

        //			//FILTRAR SOLO LOS DE LA VERSION ANTERIOR PARA MAS EFICIENCIA
        //			//List<areanivel2> sistemasVersionAnterior = Datos.ListarSistema();
        //			//List<areanivel3> equiposVersionAnterior = Datos.ListarEquipos();

        //			copiarMatriz(tecnologiasVersionAnterior, areasVersionAnterior, centroseleccionado.id, versionNueva, Datos.listarMatrizCentro(centroseleccionado.id, int.Parse(versionDesde)));
        //			Datos.RecalcularMatrizVersion(centroseleccionado.id, versionNueva);

        //			return RedirectToAction("matriz_riesgos_criticos/" + versionNueva);
        //		}
        //		else if (version == 0)
        //		{

        //			List<tecnologias> tecnologiasCentro = Datos.ListarTecnologiasPorCentro(centroseleccionado.id);
        //			List<areanivel1> areasVersion = Datos.ListarAreasMaestro(version, 0);
        //			areasVersion = areasVersion.Where(x => x.id_centro == centroseleccionado.id).OrderBy(x => x.id).ToList();
        //			List<areanivel2> sistemas = Datos.ListarSistema();
        //			List<areanivel3> equipos = Datos.ListarEquipos();
        //			List<areanivel4> nivelescuatro = Datos.ListarNivelescuatro();
        //			List<areanivel1> areasPrede = new List<areanivel1>();

        //			//COMPROBAR QUE TENEMOS LOS DATOS DE TECNOLOGIAS
        //			List<string> tecnologiasSelec = new List<string>();
        //			if (recibe != null)
        //			{
        //				foreach (string item in recibe)
        //				{
        //					tecnologiasSelec.Add(item);
        //				}
        //			}
        //			//PARA MAS ADELANTE, HACER LA BUSQUEDA POR ENTEROS
        //			List<tecnologias> tecnologias = Datos.ListarTecnologiasid(tecnologiasSelec).OrderBy(x => x.id).ToList();

        //			//string[] tecnologiasSeleccionadas = { "1","2", "6" };
        //			if (tecnologias.Count() > 0)
        //			{
        //				foreach (tecnologias item in tecnologias)
        //				{
        //					int tecnologia = item.id;
        //					List<areanivel1> ar = Datos.listarAreasInicial(tecnologia);
        //					if (ar != null)
        //					{
        //						areasPrede.AddRange(ar);
        //					}
        //				}
        //			}

        //			//PARA MAS ADELANTE, HACER LA BUSQUEDA POR ENTEROS
        //			List<matriz_inicial> listaMatricesMaestro = Datos.listarMatrizInicial(tecnologiasSelec);

        //			int idVersionNueva = Datos.ActualizarVersionMatriz(centroseleccionado.id, Session["usuario"].ToString());

        //			List<matriz_centro> matrizNueva = new List<matriz_centro>();

        //			foreach (tecnologias tecno in tecnologias)
        //			{
        //				matriz_centro mc = new matriz_centro();
        //				foreach (areanivel1 area in areasPrede)
        //				{
        //					if (area.id_tecnologia == tecno.id)
        //					{
        //						areanivel1 copiaArea = new areanivel1();
        //						copiaArea.nombre = area.nombre;
        //						copiaArea.codigo = area.codigo;
        //						copiaArea.id_centro = centroseleccionado.id;
        //						copiaArea.id_tecnologia = tecno.id;
        //						int id_area_seleccionada = Datos.ActualizarArea(copiaArea);
        //						copiaArea.id = id_area_seleccionada;


        //						foreach (tipo_riesgos_critico riesgo in Datos.ListarTiposRiesgosCriticos())
        //						{

        //							matriz_centro matriz = new matriz_centro();
        //							matriz.id_centro = centroseleccionado.id;
        //							matriz.id_areanivel1 = copiaArea.id;
        //							matriz.id_areanivel2 = null;
        //							matriz.id_areanivel3 = null;
        //							matriz.id_tecnologia = tecno.id;
        //							matriz.id_riesgo = riesgo.id;
        //							matriz.activo = listaMatricesMaestro.Where(z => z.id_areanivel1 == area.id && z.id_riesgo == matriz.id_riesgo).Select(x => x.activo).DefaultIfEmpty(false).First();

        //							matriz.version = idVersionNueva;
        //							matrizNueva.Add(matriz);

        //						}
        //						if (area.id_tecnologia == tecno.id)
        //						{
        //							Datos.InsertarMatriz(matrizNueva);
        //							matrizNueva.Clear();
        //						}

        //						foreach (areanivel2 sistema in area.areanivel2)
        //						{
        //							areanivel2 copiaSistema = new areanivel2();
        //							copiaSistema.nombre = sistema.nombre;
        //							copiaSistema.codigo = sistema.codigo;
        //							copiaSistema.id_areanivel1 = copiaArea.id;
        //							int id_sistema_seleccionado = Datos.ActualizarSistema(copiaSistema);
        //							copiaSistema.id = id_sistema_seleccionado;
        //							copiaArea.areanivel2.Add(copiaSistema);
        //							Datos.ActualizarArea(copiaArea);
        //							foreach (tipo_riesgos_critico riesgo in Datos.ListarTiposRiesgosCriticos())
        //							{

        //								matriz_centro matriz = new matriz_centro();
        //								matriz.id_centro = centroseleccionado.id;
        //								matriz.id_areanivel1 = null;
        //								matriz.id_areanivel2 = copiaSistema.id;
        //								matriz.id_areanivel3 = null;
        //								matriz.id_tecnologia = tecno.id;
        //								matriz.id_riesgo = riesgo.id;
        //								matriz.activo = false;
        //								matriz.activo = listaMatricesMaestro.Where(z => z.id_areanivel2 == sistema.id && z.id_riesgo == matriz.id_riesgo).Select(x => x.activo).DefaultIfEmpty(false).First();
        //								matriz.version = idVersionNueva;
        //								matrizNueva.Add(matriz);

        //							}
        //							if (area.id_tecnologia == tecno.id)
        //							{
        //								Datos.InsertarMatriz(matrizNueva);
        //								matrizNueva.Clear();
        //							}
        //							foreach (areanivel3 equipo in sistema.areanivel3)
        //							{
        //								areanivel3 copiaEquipo = new areanivel3();
        //								copiaEquipo.nombre = equipo.nombre;
        //								copiaEquipo.codigo = equipo.codigo;
        //								copiaEquipo.id_areanivel2 = copiaSistema.id;

        //								int id_equipo_seleccionado = Datos.ActualizarEquipo(copiaEquipo);
        //								copiaEquipo.id = id_equipo_seleccionado;
        //								copiaSistema.areanivel3.Add(copiaEquipo);
        //								Datos.ActualizarSistema(copiaSistema);
        //								foreach (tipo_riesgos_critico riesgo in Datos.ListarTiposRiesgosCriticos())
        //								{

        //									matriz_centro matriz = new matriz_centro();
        //									matriz.id_centro = centroseleccionado.id;
        //									matriz.id_areanivel1 = null;
        //									matriz.id_areanivel2 = null;
        //									matriz.id_areanivel3 = copiaEquipo.id;
        //									matriz.id_tecnologia = tecno.id;
        //									matriz.id_riesgo = riesgo.id;
        //									//matriz.activo = false;
        //									matriz.activo = listaMatricesMaestro.Where(z => z.id_areanivel3 == equipo.id && z.id_riesgo == matriz.id_riesgo).Select(x => x.activo).DefaultIfEmpty(false).First();
        //									matriz.version = idVersionNueva;
        //									matrizNueva.Add(matriz);

        //								}
        //								if (area.id_tecnologia == tecno.id)
        //								{
        //									Datos.InsertarMatriz(matrizNueva);
        //									matrizNueva.Clear();
        //								}
        //								foreach (areanivel4 nivelcuatro in Datos.ListarNivelescuatro().Where(x => x.id_areanivel3 == equipo.id))
        //								{
        //									areanivel4 copianivelCuatro = new areanivel4();
        //									copianivelCuatro.nombre = nivelcuatro.nombre;
        //									copianivelCuatro.codigo = nivelcuatro.codigo;
        //									copianivelCuatro.id_areanivel3 = copiaEquipo.id;

        //									int id_nivelcuatro_seleccionado = Datos.ActualizarNivelCuatro(copianivelCuatro);
        //									copianivelCuatro.id = id_nivelcuatro_seleccionado;

        //									foreach (tipo_riesgos_critico riesgo in Datos.ListarTiposRiesgosCriticos())
        //									{

        //										matriz_centro matriz = new matriz_centro();
        //										matriz.id_centro = centroseleccionado.id;
        //										matriz.id_areanivel1 = null;
        //										matriz.id_areanivel2 = null;
        //										matriz.id_areanivel3 = null;
        //										matriz.id_areanivel4 = copianivelCuatro.id;
        //										matriz.id_tecnologia = tecno.id;
        //										matriz.id_riesgo = riesgo.id;
        //										matriz.activo = false;
        //										matriz.activo = listaMatricesMaestro.Where(z => z.id_areanivel4 == nivelcuatro.id && z.id_riesgo == matriz.id_riesgo).Select(x => x.activo).DefaultIfEmpty(false).First();
        //										matriz.version = idVersionNueva;
        //										matrizNueva.Add(matriz);

        //									}
        //									if (area.id_tecnologia == tecno.id)
        //									{
        //										Datos.InsertarMatriz(matrizNueva);
        //										matrizNueva.Clear();
        //									}
        //								}
        //							}
        //						}
        //					}

        //				}

        //			}
        //			Datos.RecalcularMatrizVersion(centroseleccionado.id, idVersionNueva);

        //			return RedirectToAction("matriz_riesgos_criticos/" + idVersionNueva);
        //		}
        //	}
        //	else
        //	{



        //		ViewData["tipos_riesgos"] = Datos.ListarTiposRiesgosCriticos();


        //		if (version == 0)
        //		{
        //			int idVersionNueva = Datos.ActualizarVersionMatriz(centroseleccionado.id, Session["usuario"].ToString());
        //			return RedirectToAction("matriz_riesgos_criticos", new { id = idVersionNueva });
        //		}

        //		//ViewData["matriz_inicial"] = Datos.listarMatrizInicial();
        //		ViewData["matriz_centro"] = Datos.listarMatrizCentro(centroseleccionado.id, version);

        //		List<tecnologias> tecnologiasCentro = Datos.ListarTecnologiasPorVersion(centroseleccionado.id, version);
        //		if (tecnologiasCentro.Count == 0)
        //		{
        //			tecnologiasCentro = Datos.ListarTecnologiasPorCentro(centroseleccionado.id);
        //		}
        //		ViewData["tecnologias"] = tecnologiasCentro.OrderBy(x => x.id);


        //		List<areanivel1> areasVersion = Datos.ListarAreas(version);
        //		areasVersion = areasVersion.Where(x => x.id_centro == centroseleccionado.id).OrderBy(x => x.id).ToList();
        //		List<areanivel2> sistemas = Datos.ListarSistema();
        //		List<areanivel3> equipos = Datos.ListarEquipos();
        //		List<areanivel4> nivelescuatro = Datos.ListarNivelescuatro();


        //		ViewData["areas"] = areasVersion;
        //		ViewData["sistemas"] = sistemas;
        //		ViewData["equipos"] = equipos;
        //		ViewData["nivelescuatro"] = nivelescuatro;
        //		ViewData["imagenesAreas"] = Datos.ListarImagenesAreas();
        //		ViewData["imagenesSistemas"] = Datos.ListarImagenesSistemas();
        //		ViewData["imagenesEquipos"] = Datos.ListarImagenesEquipos();
        //		ViewData["imagenesareascuatro"] = Datos.ListarImagenesAreasCuatro();
        //		ViewData["version"] = version;
        //	}

        //	var listaVersionMatriz = MIDAS.Models.Datos.listarMatrizVersion(int.Parse(Session["CentralElegida"].ToString()));

        //	if (listaVersionMatriz.Count > 0)
        //	{
        //		Session["ExisteMatriz"] = "existe";
        //	}
        //	return View();
        //}



        [HttpGet]
        public ActionResult matriz_riesgos(int? id)
        {
            if (Session["usuario"] == null)
            {

                return RedirectToAction("LogOn", "Account");
            }
            //string[] recibe = (string[])TempData["Create"];
            string[] recibe = null;
            string versionDesde = null;
            if (Session["tecnologiasSeleccionadas"] != null)
            {
                recibe = (string[])Session["tecnologiasSeleccionadas"];
                Session["tecnologiasSeleccionadas"] = null;
            }

            if (Session["versionDesde"] != null)
            {
                versionDesde = Session["versionDesde"].ToString();
                //Session["versionDesde"] = null;
                Session.Remove("versionDesde");
            }
            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            VISTA_ObtenerUsuario user = Datos.ObtenerUsuarioActual(Session["usuario"].ToString());
            int version = 0;
            if (id != null)
            {
                version = (int)id;
            }
            if (version == 0 || versionDesde != null)
            {
                if (versionDesde != null)
                {
                    var listaVerMat = Datos.listarMatrizVersion(centroseleccionado.id);
                    var versionActual = listaVerMat.Where(x => x.id == version).FirstOrDefault();
                    var finalizado = versionActual.estado;
                    int versionNueva = 0;
                    if (finalizado != 1)
                    {
                        versionNueva = Datos.ActualizarVersionMatriz(centroseleccionado.id, Session["usuario"].ToString());
                        List<tecnologias> tecnologiasVersionAnterior = Datos.ListarTecnologiasPorVersion(centroseleccionado.id, int.Parse(versionDesde));
                        List<areanivel1> areasVersionAnterior = Datos.ListarAreas(int.Parse(versionDesde));

                        //FILTRAR SOLO LOS DE LA VERSION ANTERIOR PARA MAS EFICIENCIA
                        //List<areanivel2> sistemasVersionAnterior = Datos.ListarSistema();
                        //List<areanivel3> equiposVersionAnterior = Datos.ListarEquipos();

                        copiarMatriz(tecnologiasVersionAnterior, areasVersionAnterior, centroseleccionado.id, versionNueva, Datos.listarMatrizCentro(centroseleccionado.id, int.Parse(versionDesde)));
                        Datos.RecalcularMatrizVersion(centroseleccionado.id, versionNueva);
                    }
                    else
                    {
                        versionNueva = versionActual.id;
                    }

                    return RedirectToAction("matriz_riesgos/" + versionNueva);
                }
                else if (version == 0)
                {



                    List<tecnologias> tecnologiasCentro = Datos.ListarTecnologiasPorCentro(centroseleccionado.id);
                    List<areanivel1> areasVersion = Datos.ListarAreasMaestro(version, 0);
                    areasVersion = areasVersion.Where(x => x.id_centro == centroseleccionado.id).OrderBy(x => x.id).ToList();
                    List<areanivel2> sistemas = Datos.ListarSistema();
                    List<areanivel3> equipos = Datos.ListarEquipos();
                    List<areanivel4> nivelescuatro = Datos.ListarNivelescuatro();
                    List<areanivel1> areasPrede = new List<areanivel1>();

                    //COMPROBAR QUE TENEMOS LOS DATOS DE TECNOLOGIAS
                    List<string> tecnologiasSelec = new List<string>();
                    if (recibe != null)
                    {
                        foreach (string item in recibe)
                        {
                            tecnologiasSelec.Add(item);
                        }
                    }
                    //PARA MAS ADELANTE, HACER LA BUSQUEDA POR ENTEROS
                    List<tecnologias> tecnologias = Datos.ListarTecnologiasid(tecnologiasSelec).OrderBy(x => x.id).ToList();

                    //string[] tecnologiasSeleccionadas = { "1","2", "6" };
                    if (tecnologias.Count() > 0)
                    {
                        foreach (tecnologias item in tecnologias)
                        {
                            int tecnologia = item.id;
                            List<areanivel1> ar = Datos.listarAreasInicial(tecnologia);
                            if (ar != null)
                            {
                                areasPrede.AddRange(ar);
                            }
                        }
                    }

                    //PARA MAS ADELANTE, HACER LA BUSQUEDA POR ENTEROS
                    List<matriz_inicial> listaMatricesMaestro = Datos.listarMatrizInicial(tecnologiasSelec);

                    int idVersionNueva = Datos.ActualizarVersionMatriz(centroseleccionado.id, Session["usuario"].ToString());

                    List<matriz_centro> matrizNueva = new List<matriz_centro>();

                    foreach (tecnologias tecno in tecnologias)
                    {
                        matriz_centro mc = new matriz_centro();
                        foreach (areanivel1 area in areasPrede)
                        {
                            if (area.id_tecnologia == tecno.id)
                            {
                                areanivel1 copiaArea = new areanivel1();
                                copiaArea.nombre = area.nombre;
                                copiaArea.codigo = area.codigo;
                                copiaArea.id_centro = centroseleccionado.id;
                                copiaArea.id_tecnologia = tecno.id;
                                int id_area_seleccionada = Datos.ActualizarArea(copiaArea);
                                copiaArea.id = id_area_seleccionada;


                                foreach (VISTA_tipos_riesgos riesgo in Datos.ListarTiposRiesgos())
                                {

                                    matriz_centro matriz = new matriz_centro();
                                    matriz.id_centro = centroseleccionado.id;
                                    matriz.id_areanivel1 = copiaArea.id;
                                    matriz.id_areanivel2 = null;
                                    matriz.id_areanivel3 = null;
                                    matriz.id_tecnologia = tecno.id;
                                    matriz.id_riesgo = riesgo.id;
                                    matriz.activo = listaMatricesMaestro.Where(z => z.id_areanivel1 == area.id && z.id_riesgo == matriz.id_riesgo).Select(x => x.activo).DefaultIfEmpty(false).First();

                                    matriz.version = idVersionNueva;
                                    matrizNueva.Add(matriz);

                                }
                                if (area.id_tecnologia == tecno.id)
                                {
                                    Datos.InsertarMatriz(matrizNueva);
                                    matrizNueva.Clear();
                                }

                                foreach (areanivel2 sistema in area.areanivel2)
                                {
                                    areanivel2 copiaSistema = new areanivel2();
                                    copiaSistema.nombre = sistema.nombre;
                                    copiaSistema.codigo = sistema.codigo;
                                    copiaSistema.id_areanivel1 = copiaArea.id;
                                    int id_sistema_seleccionado = Datos.ActualizarSistema(copiaSistema);
                                    copiaSistema.id = id_sistema_seleccionado;
                                    copiaArea.areanivel2.Add(copiaSistema);
                                    Datos.ActualizarArea(copiaArea);
                                    foreach (VISTA_tipos_riesgos riesgo in Datos.ListarTiposRiesgos())
                                    {

                                        matriz_centro matriz = new matriz_centro();
                                        matriz.id_centro = centroseleccionado.id;
                                        matriz.id_areanivel1 = null;
                                        matriz.id_areanivel2 = copiaSistema.id;
                                        matriz.id_areanivel3 = null;
                                        matriz.id_tecnologia = tecno.id;
                                        matriz.id_riesgo = riesgo.id;
                                        matriz.activo = false;
                                        matriz.activo = listaMatricesMaestro.Where(z => z.id_areanivel2 == sistema.id && z.id_riesgo == matriz.id_riesgo).Select(x => x.activo).DefaultIfEmpty(false).First();
                                        matriz.version = idVersionNueva;
                                        matrizNueva.Add(matriz);

                                    }
                                    if (area.id_tecnologia == tecno.id)
                                    {
                                        Datos.InsertarMatriz(matrizNueva);
                                        matrizNueva.Clear();
                                    }
                                    foreach (areanivel3 equipo in sistema.areanivel3)
                                    {
                                        areanivel3 copiaEquipo = new areanivel3();
                                        copiaEquipo.nombre = equipo.nombre;
                                        copiaEquipo.codigo = equipo.codigo;
                                        copiaEquipo.id_areanivel2 = copiaSistema.id;

                                        int id_equipo_seleccionado = Datos.ActualizarEquipo(copiaEquipo);
                                        copiaEquipo.id = id_equipo_seleccionado;
                                        copiaSistema.areanivel3.Add(copiaEquipo);
                                        Datos.ActualizarSistema(copiaSistema);
                                        foreach (VISTA_tipos_riesgos riesgo in Datos.ListarTiposRiesgos())
                                        {

                                            matriz_centro matriz = new matriz_centro();
                                            matriz.id_centro = centroseleccionado.id;
                                            matriz.id_areanivel1 = null;
                                            matriz.id_areanivel2 = null;
                                            matriz.id_areanivel3 = copiaEquipo.id;
                                            matriz.id_tecnologia = tecno.id;
                                            matriz.id_riesgo = riesgo.id;
                                            //matriz.activo = false;
                                            matriz.activo = listaMatricesMaestro.Where(z => z.id_areanivel3 == equipo.id && z.id_riesgo == matriz.id_riesgo).Select(x => x.activo).DefaultIfEmpty(false).First();
                                            matriz.version = idVersionNueva;
                                            matrizNueva.Add(matriz);

                                        }
                                        if (area.id_tecnologia == tecno.id)
                                        {
                                            Datos.InsertarMatriz(matrizNueva);
                                            matrizNueva.Clear();
                                        }
                                        foreach (areanivel4 nivelcuatro in Datos.ListarNivelescuatro().Where(x => x.id_areanivel3 == equipo.id))
                                        {
                                            areanivel4 copianivelCuatro = new areanivel4();
                                            copianivelCuatro.nombre = nivelcuatro.nombre;
                                            copianivelCuatro.codigo = nivelcuatro.codigo;
                                            copianivelCuatro.id_areanivel3 = copiaEquipo.id;

                                            int id_nivelcuatro_seleccionado = Datos.ActualizarNivelCuatro(copianivelCuatro);
                                            copianivelCuatro.id = id_nivelcuatro_seleccionado;

                                            foreach (VISTA_tipos_riesgos riesgo in Datos.ListarTiposRiesgos())
                                            {

                                                matriz_centro matriz = new matriz_centro();
                                                matriz.id_centro = centroseleccionado.id;
                                                matriz.id_areanivel1 = null;
                                                matriz.id_areanivel2 = null;
                                                matriz.id_areanivel3 = null;
                                                matriz.id_areanivel4 = copianivelCuatro.id;
                                                matriz.id_tecnologia = tecno.id;
                                                matriz.id_riesgo = riesgo.id;
                                                matriz.activo = false;
                                                matriz.activo = listaMatricesMaestro.Where(z => z.id_areanivel4 == nivelcuatro.id && z.id_riesgo == matriz.id_riesgo).Select(x => x.activo).DefaultIfEmpty(false).First();
                                                matriz.version = idVersionNueva;
                                                matrizNueva.Add(matriz);

                                            }
                                            if (area.id_tecnologia == tecno.id)
                                            {
                                                Datos.InsertarMatriz(matrizNueva);
                                                matrizNueva.Clear();
                                            }
                                        }
                                    }
                                }
                            }

                        }

                    }
                    Datos.RecalcularMatrizVersion(centroseleccionado.id, idVersionNueva);

                    return RedirectToAction("matriz_riesgos/" + idVersionNueva);
                }
            }
            else
            {



                ViewData["tipos_riesgos"] = Datos.ListarTiposRiesgos();


                if (version == 0)
                {
                    int idVersionNueva = Datos.ActualizarVersionMatriz(centroseleccionado.id, Session["usuario"].ToString());
                    return RedirectToAction("matriz_riesgos", new { id = idVersionNueva });
                }

                //ViewData["matriz_inicial"] = Datos.listarMatrizInicial();
                ViewData["matriz_centro"] = Datos.listarMatrizCentro(centroseleccionado.id, version);

                List<tecnologias> tecnologiasCentro = Datos.ListarTecnologiasPorVersion(centroseleccionado.id, version);
                if (tecnologiasCentro.Count == 0)
                {
                    tecnologiasCentro = Datos.ListarTecnologiasPorCentro(centroseleccionado.id);
                }
                ViewData["tecnologias"] = tecnologiasCentro.OrderByDescending(x => x.id);


                List<areanivel1> areasVersion = Datos.ListarAreas(version);
                areasVersion = areasVersion.Where(x => x.id_centro == centroseleccionado.id).OrderBy(x => x.id).ToList();
                List<areanivel2> sistemas = Datos.ListarSistema();
                List<areanivel3> equipos = Datos.ListarEquipos();
                List<areanivel4> nivelescuatro = Datos.ListarNivelescuatro();


                ViewData["areas"] = areasVersion;
                ViewData["sistemas"] = sistemas;
                ViewData["equipos"] = equipos;
                ViewData["nivelescuatro"] = nivelescuatro;
                ViewData["imagenesAreas"] = Datos.ListarImagenesAreas();
                ViewData["imagenesSistemas"] = Datos.ListarImagenesSistemas();
                ViewData["imagenesEquipos"] = Datos.ListarImagenesEquipos();
                ViewData["imagenesareascuatro"] = Datos.ListarImagenesAreasCuatro();
                ViewData["version"] = version;
            }

            var listaVersionMatriz = MIDAS.Models.Datos.listarMatrizVersion(int.Parse(Session["CentralElegida"].ToString()));

            if (listaVersionMatriz.Count > 0)
            {
                Session["ExisteMatriz"] = "existe";
            }
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult gestion_riesgos(FormCollection collection)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            VISTA_ObtenerUsuario user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());

            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));

            int idCentral = centroseleccionado.id;

            List<VISTA_Riesgo> listadoRiesgos = Datos.ListarRiesgosFicha(centroseleccionado.id);

            #region generacion fichero
            string sourceFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaPlantillaWord"] + "ExportacionRiesgos.xlsx");
            string destinationFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaGeneracionWord"] + "ExportacionRiesgos_" + DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + ".xlsx");

            Session["source"] = sourceFile;
            Session["destino"] = destinationFile;
            // Create a copy of the template file and open the copy 
            System.IO.File.Copy(sourceFile, destinationFile, true);

            #region impresion

            using (SpreadsheetDocument document = SpreadsheetDocument.Open(destinationFile, true))
            {
                WorksheetPart worksheetPart = GetWorksheetPartByName(document, "Riesgos y Oportunidades");
                uint indiceFila = 12;
                if (worksheetPart != null)
                {
                    foreach (VISTA_Riesgo rie in listadoRiesgos)
                    {
                        Row row = new Row();

                        #region inserción en celdas de excel
                        if (rie.CodigoRiesgo != null)
                        {
                            Cell codigo = GetCell(worksheetPart.Worksheet, "A", indiceFila);
                            codigo.CellValue = new CellValue(rie.CodigoRiesgo);
                            codigo.DataType = new EnumValue<CellValues>(CellValues.Number);
                        }

                        if (rie.Tipo != null)
                        {
                            Cell tipo = GetCell(worksheetPart.Worksheet, "B", indiceFila);
                            if (rie.Tipo == 1)
                                tipo.CellValue = new CellValue("Riesgo");
                            else
                                tipo.CellValue = new CellValue("Oportunidad");
                            tipo.DataType = new EnumValue<CellValues>(CellValues.String);
                        }

                        if (rie.Descripcion != null)
                        {
                            Cell descripcion = GetCell(worksheetPart.Worksheet, "C", indiceFila);
                            descripcion.CellValue = new CellValue(rie.Descripcion);
                            descripcion.DataType = new EnumValue<CellValues>(CellValues.String);
                        }

                        if (rie.vigente != null)
                        {
                            Cell vigencia = GetCell(worksheetPart.Worksheet, "D", indiceFila);
                            if (rie.vigente == 0)
                            {
                                vigencia.CellValue = new CellValue("Sí");
                                vigencia.DataType = new EnumValue<CellValues>(CellValues.String);
                            }
                            else
                            {
                                vigencia.CellValue = new CellValue("No");
                                vigencia.DataType = new EnumValue<CellValues>(CellValues.String);
                            }

                        }

                        if (rie.idCadenaValor != null && rie.idCadenaValor != 0)
                        {
                            Cell cadenavalor = GetCell(worksheetPart.Worksheet, "E", indiceFila);
                            cadenavalor.CellValue = new CellValue(Datos.GetDatosProceso(int.Parse(rie.idCadenaValor.ToString())).nombre);
                            cadenavalor.DataType = new EnumValue<CellValues>(CellValues.String);
                        }

                        if (rie.idMacroproceso != null && rie.idMacroproceso != 0)
                        {
                            Cell macroproceso = GetCell(worksheetPart.Worksheet, "F", indiceFila);
                            macroproceso.CellValue = new CellValue(Datos.GetDatosProceso(int.Parse(rie.idMacroproceso.ToString())).nombre);
                            macroproceso.DataType = new EnumValue<CellValues>(CellValues.String);
                        }

                        if (rie.idProceso != null && rie.idProceso != 0)
                        {
                            Cell proceso = GetCell(worksheetPart.Worksheet, "G", indiceFila);
                            proceso.CellValue = new CellValue(Datos.GetDatosProceso(int.Parse(rie.idProceso.ToString())).nombre);
                            proceso.DataType = new EnumValue<CellValues>(CellValues.String);
                        }

                        if (rie.categoria != null)
                        {
                            Cell categoria = GetCell(worksheetPart.Worksheet, "H", indiceFila);
                            categoria.CellValue = new CellValue(rie.categoria);
                            categoria.DataType = new EnumValue<CellValues>(CellValues.String);
                        }

                        if (rie.tipologia != null)
                        {
                            Cell tipologia = GetCell(worksheetPart.Worksheet, "I", indiceFila);
                            tipologia.CellValue = new CellValue(rie.tipologia);
                            tipologia.DataType = new EnumValue<CellValues>(CellValues.String);
                        }

                        List<VISTA_StakeholdersN2> listaStakeholders = Datos.ListarStakeholdersAsignadosRiesgo(rie.Id);
                        string stakeholders = string.Empty;
                        foreach (VISTA_StakeholdersN2 shn3 in listaStakeholders)
                        {
                            stakeholders = stakeholders + "-" + shn3.denominacionn2 + "\n\r";
                        }
                        if (stakeholders != string.Empty)
                        {
                            Cell sh = GetCell(worksheetPart.Worksheet, "J", indiceFila);
                            sh.CellValue = new CellValue(stakeholders);
                            sh.DataType = new EnumValue<CellValues>(CellValues.String);
                        }

                        if (rie.RI_ProbabilidadOcurrencia != null)
                        {
                            Cell probabilidadRI = GetCell(worksheetPart.Worksheet, "K", indiceFila);
                            probabilidadRI.CellValue = new CellValue(rie.RI_ProbabilidadOcurrencia);
                            probabilidadRI.DataType = new EnumValue<CellValues>(CellValues.String);
                        }

                        if (rie.RI_ImpactoObjetivos != null)
                        {
                            Cell impactoobjetivosRI = GetCell(worksheetPart.Worksheet, "L", indiceFila);
                            impactoobjetivosRI.CellValue = new CellValue(rie.RI_ImpactoObjetivos);
                            impactoobjetivosRI.DataType = new EnumValue<CellValues>(CellValues.String);
                        }

                        if (rie.RI_ImpactoEconomico != null)
                        {
                            Cell impactoeconomicoRI = GetCell(worksheetPart.Worksheet, "M", indiceFila);
                            impactoeconomicoRI.CellValue = new CellValue(rie.RI_ImpactoEconomico);
                            impactoeconomicoRI.DataType = new EnumValue<CellValues>(CellValues.String);
                        }

                        if (rie.RI_ImpactoProcesosNegocio != null)
                        {
                            Cell impactoprocesosRI = GetCell(worksheetPart.Worksheet, "N", indiceFila);
                            impactoprocesosRI.CellValue = new CellValue(rie.RI_ImpactoProcesosNegocio);
                            impactoprocesosRI.DataType = new EnumValue<CellValues>(CellValues.String);
                        }

                        if (rie.RI_ImpactoReputacional != null)
                        {
                            Cell impactoreputacionalRI = GetCell(worksheetPart.Worksheet, "O", indiceFila);
                            impactoreputacionalRI.CellValue = new CellValue(rie.RI_ImpactoReputacional);
                            impactoreputacionalRI.DataType = new EnumValue<CellValues>(CellValues.String);
                        }

                        if (rie.RI_ImpactoCumplimiento != null)
                        {
                            Cell impactocumplimientoRI = GetCell(worksheetPart.Worksheet, "P", indiceFila);
                            impactocumplimientoRI.CellValue = new CellValue(rie.RI_ImpactoCumplimiento);
                            impactocumplimientoRI.DataType = new EnumValue<CellValues>(CellValues.String);
                        }

                        if (rie.RI_ImpactoGeneral != null)
                        {
                            Cell impactogeneralRI = GetCell(worksheetPart.Worksheet, "Q", indiceFila);
                            impactogeneralRI.CellValue = new CellValue(rie.RI_ImpactoGeneral);
                            impactogeneralRI.DataType = new EnumValue<CellValues>(CellValues.String);
                        }

                        if (rie.RI_RelevanciaRiesgo != null)
                        {
                            Cell relevanciariesgoRI = GetCell(worksheetPart.Worksheet, "R", indiceFila);
                            relevanciariesgoRI.CellValue = new CellValue(rie.RI_RelevanciaRiesgo);
                            relevanciariesgoRI.DataType = new EnumValue<CellValues>(CellValues.String);
                        }

                        if (rie.RI_ValorRelevanciaRiesgo != null)
                        {
                            Cell relevancevalueRI = GetCell(worksheetPart.Worksheet, "S", indiceFila);
                            relevancevalueRI.CellValue = new CellValue(rie.RI_ValorRelevanciaRiesgo);
                            relevancevalueRI.DataType = new EnumValue<CellValues>(CellValues.String);
                        }

                        if (rie.RI_GestionRiesgo != null)
                        {
                            Cell gestionriesgo = GetCell(worksheetPart.Worksheet, "T", indiceFila);
                            gestionriesgo.CellValue = new CellValue(rie.RI_GestionRiesgo);
                            gestionriesgo.DataType = new EnumValue<CellValues>(CellValues.String);
                        }

                        if (rie.LimitOOE != null && rie.LimitOOE == true)
                        {
                            Cell limitOOE = GetCell(worksheetPart.Worksheet, "U", indiceFila);
                            limitOOE.CellValue = new CellValue("X");
                            limitOOE.DataType = new EnumValue<CellValues>(CellValues.String);
                        }

                        if (rie.LimitOO != null && rie.LimitOO == true)
                        {
                            Cell limitOO = GetCell(worksheetPart.Worksheet, "V", indiceFila);
                            limitOO.CellValue = new CellValue("X");
                            limitOO.DataType = new EnumValue<CellValues>(CellValues.String);
                        }

                        if (rie.LimitE != null && rie.LimitE == true)
                        {
                            Cell limitE = GetCell(worksheetPart.Worksheet, "W", indiceFila);
                            limitE.CellValue = new CellValue("X");
                            limitE.DataType = new EnumValue<CellValues>(CellValues.String);
                        }

                        if (rie.SinLimit != null && rie.SinLimit == true)
                        {
                            Cell sinlimit = GetCell(worksheetPart.Worksheet, "X", indiceFila);
                            sinlimit.CellValue = new CellValue("X");
                            sinlimit.DataType = new EnumValue<CellValues>(CellValues.String);
                        }

                        if (rie.SinEfectos != null && rie.SinEfectos == true)
                        {
                            Cell sinefectos = GetCell(worksheetPart.Worksheet, "Y", indiceFila);
                            sinefectos.CellValue = new CellValue("X");
                            sinefectos.DataType = new EnumValue<CellValues>(CellValues.String);
                        }

                        if (rie.EfectD != null && rie.EfectD == true)
                        {
                            Cell efectd = GetCell(worksheetPart.Worksheet, "Z", indiceFila);
                            efectd.CellValue = new CellValue("X");
                            efectd.DataType = new EnumValue<CellValues>(CellValues.String);
                        }

                        if (rie.EfectDI != null && rie.EfectDI == true)
                        {
                            Cell efectdi = GetCell(worksheetPart.Worksheet, "AA", indiceFila);
                            efectdi.CellValue = new CellValue("X");
                            efectdi.DataType = new EnumValue<CellValues>(CellValues.String);
                        }

                        if (rie.ValoracionOportunidad != null)
                        {
                            Cell valoracionoportunidad = GetCell(worksheetPart.Worksheet, "AB", indiceFila);
                            valoracionoportunidad.CellValue = new CellValue(rie.ValoracionOportunidad);
                            valoracionoportunidad.DataType = new EnumValue<CellValues>(CellValues.String);
                        }

                        if (rie.GestionOportunidad != null)
                        {
                            Cell gestionoportunidad = GetCell(worksheetPart.Worksheet, "AC", indiceFila);
                            gestionoportunidad.CellValue = new CellValue(rie.GestionOportunidad);
                            gestionoportunidad.DataType = new EnumValue<CellValues>(CellValues.String);
                        }

                        if (rie.DescripcionControl != null)
                        {
                            Cell descripcioncontrol = GetCell(worksheetPart.Worksheet, "AD", indiceFila);
                            descripcioncontrol.CellValue = new CellValue(rie.DescripcionControl);
                            descripcioncontrol.DataType = new EnumValue<CellValues>(CellValues.String);
                        }

                        if (rie.PropietarioControl != null)
                        {
                            Cell propietariocontrol = GetCell(worksheetPart.Worksheet, "AE", indiceFila);
                            propietariocontrol.CellValue = new CellValue(rie.PropietarioControl);
                            propietariocontrol.DataType = new EnumValue<CellValues>(CellValues.String);
                        }

                        if (rie.RR_ProbabilidadOcurrencia != null)
                        {
                            Cell probabilidadRR = GetCell(worksheetPart.Worksheet, "AF", indiceFila);
                            probabilidadRR.CellValue = new CellValue(rie.RR_ProbabilidadOcurrencia);
                            probabilidadRR.DataType = new EnumValue<CellValues>(CellValues.String);
                        }

                        if (rie.RR_ImpactoObjetivos != null)
                        {
                            Cell impactoobjetivosRR = GetCell(worksheetPart.Worksheet, "AG", indiceFila);
                            impactoobjetivosRR.CellValue = new CellValue(rie.RR_ImpactoObjetivos);
                            impactoobjetivosRR.DataType = new EnumValue<CellValues>(CellValues.String);
                        }

                        if (rie.RR_ImpactoEconomico != null)
                        {
                            Cell impactoeconomicoRR = GetCell(worksheetPart.Worksheet, "AH", indiceFila);
                            impactoeconomicoRR.CellValue = new CellValue(rie.RR_ImpactoEconomico);
                            impactoeconomicoRR.DataType = new EnumValue<CellValues>(CellValues.String);
                        }

                        if (rie.RR_ImpactoProcesosNegocio != null)
                        {
                            Cell impactoprocesosRR = GetCell(worksheetPart.Worksheet, "AI", indiceFila);
                            impactoprocesosRR.CellValue = new CellValue(rie.RR_ImpactoProcesosNegocio);
                            impactoprocesosRR.DataType = new EnumValue<CellValues>(CellValues.String);
                        }

                        if (rie.RR_ImpactoReputacional != null)
                        {
                            Cell impactoreputacionalRR = GetCell(worksheetPart.Worksheet, "AJ", indiceFila);
                            impactoreputacionalRR.CellValue = new CellValue(rie.RR_ImpactoReputacional);
                            impactoreputacionalRR.DataType = new EnumValue<CellValues>(CellValues.String);
                        }

                        if (rie.RR_ImpactoCumplimiento != null)
                        {
                            Cell impactocumplimientoRR = GetCell(worksheetPart.Worksheet, "AK", indiceFila);
                            impactocumplimientoRR.CellValue = new CellValue(rie.RR_ImpactoCumplimiento);
                            impactocumplimientoRR.DataType = new EnumValue<CellValues>(CellValues.String);
                        }

                        if (rie.RR_ImpactoGeneral != null)
                        {
                            Cell impactogeneralRR = GetCell(worksheetPart.Worksheet, "AL", indiceFila);
                            impactogeneralRR.CellValue = new CellValue(rie.RR_ImpactoGeneral);
                            impactogeneralRR.DataType = new EnumValue<CellValues>(CellValues.String);
                        }

                        if (rie.RR_RelevanciaRiesgo != null)
                        {
                            Cell relevanciariesgoRR = GetCell(worksheetPart.Worksheet, "AM", indiceFila);
                            relevanciariesgoRR.CellValue = new CellValue(rie.RR_RelevanciaRiesgo);
                            relevanciariesgoRR.DataType = new EnumValue<CellValues>(CellValues.String);
                        }

                        if (rie.RR_ValorRelevanciaRiesgo != null)
                        {
                            Cell relevancevalueRR = GetCell(worksheetPart.Worksheet, "AN", indiceFila);
                            relevancevalueRR.CellValue = new CellValue(rie.RR_ValorRelevanciaRiesgo);
                            relevancevalueRR.DataType = new EnumValue<CellValues>(CellValues.String);
                        }
                        indiceFila++;
                        #endregion
                    }
                }

                // save worksheet
                document.WorkbookPart.WorksheetParts.First().Worksheet.Save();

                document.WorkbookPart.Workbook.Save();

                Session["nombreArchivo"] = destinationFile;
            }
            #endregion
            #endregion
            ViewData["accionesmejora"] = Datos.ListarAccionesMejora(idCentral);

            return RedirectToAction("gestion_riesgos", "Riesgos");
        }

        public JsonResult EliminarTecnologiaCentroVersion(string version, string tecnologia)
        {
            bool datos = false;
            try
            {

                centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
                Datos.EliminarTecnologiaCentroVersion(int.Parse(tecnologia), centroseleccionado.id, int.Parse(version));

                if (Datos.ListarTecnologiasPorVersion(centroseleccionado.id, int.Parse(version)).Count == 0)
                {
                    Datos.eliminarMatrizVersion(int.Parse(version));
                    datos = true;
                }

                /*si  el centro se queda sin matriz, se debe hacer lo mismo que cuando se borra el borrador*/
            }
            catch (Exception ex)
            {
            }
            return Json(datos);
        }
        public JsonResult ObtenerActivosFila(string version, string nivel, string idArea)
        {
            List<matriz_centro> datos = null;
            try
            {

                centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
                datos = Datos.listarMatrizCentro(centroseleccionado.id, int.Parse(version));
                //Datos.RecalcularMatrizVersionFila(centroseleccionado.id, int.Parse(version), int.Parse(idArea));
                Datos.RecalcularMatrizVersionFilaAsync(centroseleccionado.id, int.Parse(version), int.Parse(idArea));
                //datos =datos.Where(x => x.activo == true).ToList();              

            }
            catch (Exception ex)
            {

            }

            return Json(datos, JsonRequestBehavior.AllowGet);
        }



        public JsonResult checkSituacion(string situacion, int activo)
        {
            try
            {
                centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
                parametrica_medidas parametrica = new parametrica_medidas();
                parametrica.id_situacion = int.Parse(situacion);
                parametrica.id_centro = centroseleccionado.id;
                parametrica.activo = activo == 1 ? true : false;
                int datos = Datos.ActualizarCheckSituacion(parametrica);
                if (datos == null) datos = 0;
                return Json(datos, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult SetCheckSituacion(string situacion, int activo)
        {
            try
            {
                centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
                parametrica_medidas parametrica = new parametrica_medidas();
                parametrica.id_situacion = int.Parse(situacion);
                parametrica.id_centro = centroseleccionado.id;
                parametrica.activo = activo == 1 ? true : false;
                int datos = Datos.ActualizarSetCheckSituacion(parametrica);
                if (datos == null) datos = 0;
                return Json(datos, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult SetCheckElemento(string situacion, string tipoRiesgo, int activo)
        {
            try
            {
                centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
                parametrica_medidas parametrica = new parametrica_medidas();
                parametrica.id_situacion = int.Parse(situacion);
                parametrica.id_centro = centroseleccionado.id;
                parametrica.activo = activo == 1 ? true : false;
                int datos = Datos.ActualizarSetCheckSituacion(parametrica);
                if (datos == null) datos = 0;
                return Json(datos, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GuardarDicMedidas(Dictionary<string, bool> dicMedidas, string situacion)
        {
            try
            {
                centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
                parametrica_medidas parametrica = new parametrica_medidas();

                int datos = 0;

                foreach (var item in dicMedidas)
                {
                    var medida = item.Key.Replace("chk_", "");
                    parametrica.id_medida = int.Parse(medida);
                    parametrica.id_situacion = int.Parse(situacion);
                    parametrica.id_centro = centroseleccionado.id;
                    parametrica.activo = item.Value;
                    datos = Datos.ActualizarParametricaMedidas(parametrica);
                }
                if (datos == null) datos = 0;
                return Json(datos, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult checkMedida(string medida, int activo)
        {
            try
            {
                parametrica_medidas parametrica = new parametrica_medidas();
                centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
                parametrica.id_centro = centroseleccionado.id;
                parametrica.id_medida = int.Parse(medida);
                parametrica.activo = activo == 1 ? true : false;
                int datos = Datos.ActualizarCheckMedida(parametrica);
                if (datos == null) datos = 0;
                return Json(datos);
            }
            catch (Exception ex)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult obtenerMedidaPorIdAjax(int medida)
        {
            try
            {
                medidas_preventivas medidas_Preventivas = Datos.ObtenerMedidaPreventivaId(medida);

                return Json(medidas_Preventivas);
            }
            catch (Exception ex)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult obtenerRiesgoMedidaPorIdAjax(int medida)
        {
            try
            {
                riesgos_medidas riesgoMedidas = Datos.ObtenerRiesgoMedidaporId(medida);

                return Json(riesgoMedidas);
            }
            catch (Exception ex)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult obtenerListaRiesgoMedidaAjax()
        {
            try
            {
                List<VISTA_tipos_riesgos> listaRiesgoMedidas = Datos.ListarTiposRiesgos();

                return Json(listaRiesgoMedidas);
            }
            catch (Exception ex)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult guardarMedidaAjax(string textoDesc, int medida)
        {
            try
            {
                medidas_preventivas medidas_Preventivas = new medidas_preventivas();
                centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
                medidas_Preventivas.id_centro = centroseleccionado.id;
                medidas_Preventivas.id = medida;
                medidas_Preventivas.descripcion = textoDesc;
                //parametrica.activo = activo == 1 ? true : false;
                int datos = Datos.GuardarMedidaAjax(medidas_Preventivas);
                if (datos == null) datos = 0;
                return Json(datos);
            }
            catch (Exception ex)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult guardarRiesgoMedidaAjax(string textoDesc, int medida)
        {
            try
            {
                riesgos_medidas riesgos_Medidas = new riesgos_medidas();
                centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
                riesgos_Medidas.id_centro = centroseleccionado.id;
                riesgos_Medidas.id = medida;
                riesgos_Medidas.descripcion = textoDesc;
                //parametrica.activo = activo == 1 ? true : false;
                int datos = Datos.GuardarRiesgoMedidaAjax(riesgos_Medidas);
                if (datos == null) datos = 0;
                return Json(datos);
            }
            catch (Exception ex)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult ObtenerActivos(string version)
        {
            List<matriz_centro> datos = null;
            try
            {
                centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
                //  Datos.RecalcularMatrizVersion(centroseleccionado.id, int.Parse(version));
                datos = Datos.listarMatrizCentro(centroseleccionado.id, int.Parse(version));


                //Datos.RecalcularMatrizVersion(centroseleccionado.id, int.Parse(version));
                datos = datos.Where(x => x.activo == true).ToList();

            }
            catch (Exception ex)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }

            return Json(datos, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SeleccionarTodos()
        {

            try
            {
                centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
                List<riesgos_situaciones> listasituaciones = Datos.ListarSituaciones();
                // List<medidas_preventivas> sa= Datos.ListarMedidas().Where(x => x.id_centro == 0 || x.id_centro == centroseleccionado.id).ToList();
                List<medidas_preventivas> listaMedidas = Datos.ListarMedidas().Where(x => x.id_centro == 0 || x.id_centro == centroseleccionado.id).ToList();
                List<int> conjuntoRiesgos = Datos.ListarTiposRiesgosMedidasEntero(centroseleccionado.id);
                foreach (riesgos_situaciones situacion in listasituaciones)
                {
                    if (conjuntoRiesgos.Contains(situacion.id_tipo_riesgo))
                    {

                        parametrica_medidas parametrica = new parametrica_medidas();
                        parametrica.id_situacion = situacion.id;
                        parametrica.id_centro = centroseleccionado.id;
                        parametrica.activo = true;
                        int datos = Datos.ActualizarCheckSituacion(parametrica);
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return RedirectToAction("medidas_preventivas", "Riesgos");
        }
        public ActionResult DesSeleccionarTodos()
        {

            try
            {
                centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
                List<riesgos_situaciones> listasituaciones = Datos.ListarSituaciones();
                // List<medidas_preventivas> sa= Datos.ListarMedidas().Where(x => x.id_centro == 0 || x.id_centro == centroseleccionado.id).ToList();
                List<medidas_preventivas> listaMedidas = Datos.ListarMedidas().Where(x => x.id_centro == 0 || x.id_centro == centroseleccionado.id).ToList();

                foreach (riesgos_situaciones situacion in listasituaciones)
                {
                    parametrica_medidas parametrica = new parametrica_medidas();
                    parametrica.id_situacion = situacion.id;
                    parametrica.id_centro = centroseleccionado.id;
                    parametrica.activo = false;
                    int datos = Datos.ActualizarCheckSituacion(parametrica);
                }
            }
            catch (Exception ex)
            {

            }

            return RedirectToAction("medidas_preventivas", "Riesgos");
        }



        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CrearMatrizMaestros(string[] matrizMaestro)
        {
            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            bool datos = false;
            if (matrizMaestro.Length > 0)
            {
                Session["tecnologiasSeleccionadas"] = matrizMaestro;
                //    return Redirect("matriz_riesgos");
                //}
                //else
                //{
                //    return Json(datos);
            }

            return Json(datos);

        }

        public ActionResult CrearMatrizDesde(int id)
        {


            Session["versionDesde"] = id;
            return RedirectToAction("matriz_riesgos/" + id);
        }

        public JsonResult AddTecnologiaVersion(string version, string tecnologia)
        {
            bool datos = true;

            try
            {
                centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));

                List<string> listatecnologias = new List<string>() { tecnologia };
                if (!string.IsNullOrEmpty(tecnologia))
                {
                    List<matriz_inicial> matrizMaestroTecnologia = Datos.listarMatrizInicial(listatecnologias);
                }
                copiarMatriz(Datos.ListarTecnologiasid(listatecnologias),
                    Datos.listarAreasInicial(int.Parse(tecnologia)),
                    centroseleccionado.id, int.Parse(version),/*Datos.listarMatrizCentro(centroseleccionado.id, int.Parse(version)*/Datos.matrizMaestrotoMatrizCentro(int.Parse(tecnologia)));

                Datos.RecalcularMatrizVersion(centroseleccionado.id, int.Parse(version));

            }
            catch (Exception ex)
            {
                return Json(false);
            }

            return Json(datos);
        }





        public ActionResult eliminar_version(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            var Centro = Datos.eliminarMatrizVersion(id);


            var listaVersionMatriz = MIDAS.Models.Datos.listarMatrizVersion(int.Parse(Session["CentralElegida"].ToString()));

            if (listaVersionMatriz.Count == 0)
            {
                Session["VersionMatriz"] = null;
            }

            Session["EliminarSistema"] = "Borrador eliminado";

            return RedirectToAction("lista_matrices", "Riesgos");
            //Session["NoMatrizGenerada"] = "No existe matriz de riesgo generada";
            //return RedirectToAction("Principal", "Home");
        }
        public JsonResult ObtenerApartados()
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
                SelectListItem ep = new SelectListItem();
                ep.Text = "EQUIPOS DE PROTECCION";
                ep.Value = "2";
                datos.Add(ep);
                SelectListItem mg = new SelectListItem();
                mg.Text = "MEDIDAS GENERALES";
                mg.Value = "3";
                datos.Add(mg);




                //foreach (medidas_apartados item in listado)
                //{
                //    SelectListItem it = new SelectListItem();
                //    it.Text = item.nombre;
                //    it.Value = item.id.ToString();
                //    datos.Add(it);
                //}

            }
            catch (Exception ex)
            {

            }
            return Json(datos);
        }

        public JsonResult ObtenerTecnologias(string idVersion)
        {

            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            List<SelectListItem> datos = null;
            try
            {
                List<tecnologias> listado = Datos.ListarTecnologias();
                //listado.RemoveAt(4);
                List<tecnologias> yaExistentes = Datos.ListarTecnologiasPorVersion(centroseleccionado.id, int.Parse(idVersion));

                datos = new List<SelectListItem>();
                SelectListItem inicial = new SelectListItem();
                inicial.Text = "Seleccione Tecnologia";
                inicial.Value = "0";
                datos.Add(inicial);
                foreach (tecnologias item in listado)
                {
                    if (!((yaExistentes.Where(x => x.id == item.id).Count()) > 0))
                    {
                        SelectListItem it = new SelectListItem();
                        it.Text = item.nombre;
                        it.Value = item.id.ToString();
                        datos.Add(it);
                    }

                }

            }
            catch (Exception ex)
            {

            }
            return Json(datos);
        }



        public ActionResult finalizar_version(string id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            var Centro = Datos.finalizarMatrizVersion(int.Parse(id), Session["usuario"].ToString());

            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));



            Session["EliminarSistema"] = "Se ha finalizado la evaluación";

            return RedirectToAction("lista_matrices", "Riesgos");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GuardarAreaRiesgos(string[][] matrizRiesgos, string tecnologia, string nombre, string nivel, string version)
        {
            bool datos = false;


            try
            {


                centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));

                List<matriz_centro> conjunto = new List<matriz_centro>();
                //CREAR AREA
                areanivel1 area = new areanivel1();
                area.id_centro = centroseleccionado.id;
                area.nombre = nombre;

                //int posicionpunto = nombre.IndexOf(".");
                //if (posicionpunto != -1)
                //{
                //    string codigoObtenido = nombre.Substring(0, posicionpunto);                   
                //    area.codigo = codigoObtenido;
                //    area.nombre = nombre.Substring(posicionpunto);
                //}
                //else
                //{
                //    area.codigo = "";

                //}


                area.id_tecnologia = int.Parse(tecnologia);

                int id_area_seleccionada = Datos.ActualizarArea(area);
                //INSERTAR AREA CREADA EN MATRIZ

                foreach (var item in matrizRiesgos)
                {
                    matriz_centro matriz = new matriz_centro();

                    matriz.id_centro = centroseleccionado.id;
                    matriz.id_areanivel1 = id_area_seleccionada;
                    matriz.id_areanivel2 = null;
                    matriz.id_areanivel3 = null;
                    matriz.id_tecnologia = int.Parse(tecnologia);
                    matriz.id_riesgo = int.Parse(item[0]);
                    matriz.activo = item[1] == "true" ? true : false;
                    matriz.version = int.Parse(version);

                    conjunto.Add(matriz);
                }

                Datos.InsertarMatriz(conjunto);
                //matriz_centro matriz = new matriz_centro();
                //Datos.InsertarRegistroMatriz(matriz);
                Datos.ActualizarFechaUsuarioMatriz(int.Parse(version), Session["usuario"].ToString());

                //Datos.RecalcularMatrizVersionFila(centroseleccionado.id, int.Parse(version), id_area_seleccionada);
                Datos.RecalcularMatrizVersionFila(centroseleccionado.id, int.Parse(version), id_area_seleccionada);

            }
            catch (Exception ex)
            {
                DIMASSTEntities bd = new DIMASSTEntities();
                log insertarlog = new log();
                insertarlog.usuario = Session["usuario"].ToString();
                insertarlog.fecha = DateTime.Now;
                insertarlog.evento = "Error al guardar area en la mariz: " + ex.Message;
                bd.log.Add(insertarlog);
                bd.SaveChanges();
            }
            return Json(datos);
        }


        public JsonResult GuardarSistemaRiesgos(string[][] matrizRiesgos, string tecnologia, string nombre, string area, string nivel)
        {
            bool datos = false;


            try
            {

                centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));

                List<matriz_centro> conjunto = new List<matriz_centro>();
                //CREAR SISTEMA
                areanivel2 sistema = new areanivel2();
                int areaEntero = 0;
                int.TryParse(area, out areaEntero);
                sistema.id_areanivel1 = areaEntero;
                sistema.nombre = nombre;
                int id_sistema_seleccionado = Datos.ActualizarSistema(sistema);
                //INSERTAR SISTEMA CREADO EN MATRIZ

                foreach (var item in matrizRiesgos)
                {
                    matriz_centro matriz = new matriz_centro();

                    matriz.id_centro = centroseleccionado.id;
                    matriz.id_areanivel1 = int.Parse(area);
                    matriz.id_areanivel2 = id_sistema_seleccionado;
                    matriz.id_areanivel3 = null;
                    matriz.id_tecnologia = int.Parse(tecnologia);
                    matriz.id_riesgo = int.Parse(item[0]);
                    matriz.version = matriz.version = Datos.DameVersionArea(Datos.ObtenerAreaPadre(id_sistema_seleccionado));
                    matriz.activo = item[1] == "true" ? true : false;
                    conjunto.Add(matriz);
                }
                Datos.InsertarMatriz(conjunto);
                int? version = Datos.DameVersionArea(Datos.ObtenerAreaPadre(id_sistema_seleccionado));
                if (version != null)
                {
                    Datos.ActualizarFechaUsuarioMatriz((int)version, Session["usuario"].ToString());
                    //Datos.RecalcularMatrizVersionFila(centroseleccionado.id, (int)version, Datos.ObtenerAreaPadre(id_sistema_seleccionado));
                    Datos.RecalcularMatrizVersionFila(centroseleccionado.id, (int)version, Datos.ObtenerAreaPadre(id_sistema_seleccionado));
                }

            }
            catch (Exception ex)
            {
                DIMASSTEntities bd = new DIMASSTEntities();
                log insertarlog = new log();
                insertarlog.usuario = Session["usuario"].ToString();
                insertarlog.fecha = DateTime.Now;
                insertarlog.evento = "Error al guardar sistema en la mariz: " + ex.Message;
                bd.log.Add(insertarlog);
                bd.SaveChanges();
            }
            return Json(datos);
        }


        public JsonResult GuardarNivelCuatroRiesgos(string[][] matrizRiesgos, string tecnologia, string nombre, string area, string sistema, string equipo, string nivel)
        {
            bool datos = false;

            try
            {

                centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));

                List<matriz_centro> conjunto = new List<matriz_centro>();
                //CREAR SISTEMA
                areanivel4 nivelcuatro = new areanivel4();
                int equipoEntero = 0;
                int.TryParse(equipo, out equipoEntero);
                nivelcuatro.id_areanivel3 = equipoEntero;
                nivelcuatro.nombre = nombre;
                int id_nivelcuatro_seleccionado = Datos.ActualizarNivelCuatro(nivelcuatro);

                //INSERTAR SISTEMA CREADO EN MATRIZ

                foreach (var item in matrizRiesgos)
                {
                    matriz_centro matriz = new matriz_centro();

                    matriz.id_centro = centroseleccionado.id;
                    matriz.id_areanivel1 = int.Parse(area);
                    matriz.id_areanivel2 = int.Parse(sistema);
                    matriz.id_areanivel3 = nivelcuatro.id_areanivel3;
                    matriz.id_areanivel4 = id_nivelcuatro_seleccionado;
                    matriz.id_tecnologia = int.Parse(tecnologia);
                    matriz.id_riesgo = int.Parse(item[0]);
                    matriz.version = matriz.version = Datos.DameVersionSistema(Datos.ObtenerSistemaPadre(Datos.ObtenerEquipoPadre(id_nivelcuatro_seleccionado)));
                    matriz.activo = item[1] == "true" ? true : false;
                    conjunto.Add(matriz);
                }
                Datos.InsertarMatriz(conjunto);
                int? version = Datos.DameVersionSistema(Datos.ObtenerSistemaPadre(Datos.ObtenerEquipoPadre(id_nivelcuatro_seleccionado)));
                if (version != null)
                {
                    Datos.ActualizarFechaUsuarioMatriz((int)version, Session["usuario"].ToString());
                    //Datos.RecalcularMatrizVersionFila(centroseleccionado.id, (int)version, Datos.ObtenerAreaPadre(Datos.ObtenerSistemaPadre(Datos.ObtenerEquipoPadre(id_nivelcuatro_seleccionado))));
                    Datos.RecalcularMatrizVersionFila(centroseleccionado.id, (int)version, Datos.ObtenerAreaPadre(Datos.ObtenerSistemaPadre(Datos.ObtenerEquipoPadre(id_nivelcuatro_seleccionado))));
                }
            }
            catch (Exception ex)
            {
                DIMASSTEntities bd = new DIMASSTEntities();
                log insertarlog = new log();
                insertarlog.usuario = Session["usuario"].ToString();
                insertarlog.fecha = DateTime.Now;
                insertarlog.evento = "Error al guardar sistema en la mariz: " + ex.Message;
                bd.log.Add(insertarlog);
                bd.SaveChanges();
            }
            return Json(datos);
        }


        public JsonResult GuardarEquipoRiesgos(string[][] matrizRiesgos, string tecnologia, string nombre, string area, string sistema, string nivel)
        {
            bool datos = false;

            try
            {

                centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));

                List<matriz_centro> conjunto = new List<matriz_centro>();
                //CREAR SISTEMA
                areanivel3 equipo = new areanivel3();
                int sistemaEntero = 0;
                int.TryParse(sistema, out sistemaEntero);
                equipo.id_areanivel2 = sistemaEntero;
                equipo.nombre = nombre;
                int id_equipo_seleccionado = Datos.ActualizarEquipo(equipo);

                //INSERTAR SISTEMA CREADO EN MATRIZ

                foreach (var item in matrizRiesgos)
                {
                    matriz_centro matriz = new matriz_centro();

                    matriz.id_centro = centroseleccionado.id;
                    matriz.id_areanivel1 = int.Parse(area);
                    matriz.id_areanivel2 = equipo.id_areanivel2;
                    matriz.id_areanivel3 = id_equipo_seleccionado; ;
                    matriz.id_tecnologia = int.Parse(tecnologia);
                    matriz.id_riesgo = int.Parse(item[0]);
                    matriz.version = matriz.version = Datos.DameVersionSistema(Datos.ObtenerSistemaPadre(id_equipo_seleccionado));
                    matriz.activo = item[1] == "true" ? true : false;
                    conjunto.Add(matriz);
                }
                Datos.InsertarMatriz(conjunto);
                int? version = Datos.DameVersionSistema(Datos.ObtenerSistemaPadre(id_equipo_seleccionado));
                if (version != null)
                {
                    Datos.ActualizarFechaUsuarioMatriz((int)version, Session["usuario"].ToString());
                    //Datos.RecalcularMatrizVersionFila(centroseleccionado.id, (int)version, Datos.ObtenerAreaPadre(Datos.ObtenerSistemaPadre(id_equipo_seleccionado)));
                    //Datos.RecalcularMatrizVersionFilaAsyc(centroseleccionado.id, (int)version, Datos.ObtenerAreaPadre(Datos.ObtenerSistemaPadre(id_equipo_seleccionado)));
                    Datos.RecalcularMatrizVersionFila(centroseleccionado.id, (int)version, Datos.ObtenerAreaPadre(Datos.ObtenerSistemaPadre(id_equipo_seleccionado)));
                }
            }
            catch (Exception ex)
            {
                DIMASSTEntities bd = new DIMASSTEntities();
                log insertarlog = new log();
                insertarlog.usuario = Session["usuario"].ToString();
                insertarlog.fecha = DateTime.Now;
                insertarlog.evento = "Error al guardar sistema en la mariz: " + ex.Message;
                bd.log.Add(insertarlog);
                bd.SaveChanges();
            }
            return Json(datos);
        }




        public JsonResult GuardarImagen()
        {
            string datos = "";
            List<riesgos_medidas> response = new List<riesgos_medidas>();
            try
            {
                HttpPostedFileBase img2 = Request.Files["photo"];
                if (Request.Params["idRiesgo"] != null)
                {
                    string idRiesgo = Request.Params["idRiesgo"].ToString();
                    var ruta = System.IO.Path.Combine(Server.MapPath("~/Content/images/medidas"), img2.FileName);
                    string rutabd = "../Content/images/medidas/" + img2.FileName;
                    img2.SaveAs(ruta);
                    centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
                    riesgos_medidas medidaa_general_riesgo = new riesgos_medidas();
                    medidaa_general_riesgo.id = int.Parse(idRiesgo);
                    medidaa_general_riesgo.imagen = rutabd;
                    medidaa_general_riesgo.imagen_grande = 0;
                    Datos.ActualizarImagenRutaMedidaGeneralRiesgo(medidaa_general_riesgo);
                    List<matriz_centro> conjunto = new List<matriz_centro>();
                    datos = rutabd;
                    // var prueba= data;

                    riesgos_medidas ri = new riesgos_medidas();
                    ri.id = int.Parse(idRiesgo);
                    ri.imagen = rutabd;
                    response.Add(ri);
                }

                if (Request.Params["idmdSituacion"] != null)
                {
                    string idMedida = Request.Params["idmdSituacion"].ToString();
                    var ruta = System.IO.Path.Combine(Server.MapPath("~/Content/images/medidas/medidaspreventivas"), img2.FileName);
                    string rutabd = "../Content/images/medidas/medidaspreventivas/" + img2.FileName;
                    img2.SaveAs(ruta);
                    centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
                    medidaspreventivas_imagenes medidapreventiva = new medidaspreventivas_imagenes();
                    medidapreventiva.id_medida = int.Parse(idMedida);
                    medidapreventiva.rutaImagen = rutabd;
                    medidapreventiva.id_centro = centroseleccionado.id;
                    medidapreventiva.tamano = false;
                    Datos.ActualizarImagenRutaMedidaSituacionRiesgo(medidapreventiva);

                    datos = rutabd;
                    // var prueba= data;

                    riesgos_medidas ri = new riesgos_medidas();
                    ri.id = int.Parse(idMedida);
                    ri.imagen = rutabd;
                    response.Add(ri);
                }         
                if (Request.Params["idmdSituacionGrande"] != null)
                {
                    string idMedida = Request.Params["idmdSituacionGrande"].ToString();
                    var ruta = System.IO.Path.Combine(Server.MapPath("~/Content/images/medidas/medidaspreventivas"), img2.FileName);
                    string rutabd = "../Content/images/medidas/medidaspreventivas/" + img2.FileName;
                    img2.SaveAs(ruta);
                    centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
                    medidaspreventivas_imagenes medidapreventiva = new medidaspreventivas_imagenes();
                    medidapreventiva.id_medida = int.Parse(idMedida);
                    medidapreventiva.rutaImagen = rutabd;
                    medidapreventiva.id_centro = centroseleccionado.id;
                    medidapreventiva.tamano = true;
                    Datos.ActualizarImagenRutaMedidaSituacionRiesgo(medidapreventiva);

                    datos = rutabd;
                    // var prueba= data;

                    riesgos_medidas ri = new riesgos_medidas();
                    ri.id = int.Parse(idMedida);
                    ri.imagen = rutabd;
                    response.Add(ri);
                }





            }
            catch (Exception ex)
            {

            }
            return Json(response);
        }

        public JsonResult GuardarImagenGrande()
        {
            string datos = "";
            List<riesgos_medidas> response = new List<riesgos_medidas>();
            try
            {
                HttpPostedFileBase img2 = Request.Files["photo"];
                if (Request.Params["idRiesgo"] != null)
                {
                    string idRiesgo = Request.Params["idRiesgo"].ToString();
                    var ruta = System.IO.Path.Combine(Server.MapPath("~/Content/images/medidas"), img2.FileName);
                    string rutabd = "../Content/images/medidas/" + img2.FileName;
                    img2.SaveAs(ruta);
                    centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
                    riesgos_medidas medidaa_general_riesgo = new riesgos_medidas();
                    medidaa_general_riesgo.id = int.Parse(idRiesgo);
                    medidaa_general_riesgo.imagen = rutabd;
                    medidaa_general_riesgo.imagen_grande = 1;
                    Datos.ActualizarImagenRutaMedidaGeneralRiesgo(medidaa_general_riesgo);
                    List<matriz_centro> conjunto = new List<matriz_centro>();
                    datos = rutabd;
                    // var prueba= data;

                    riesgos_medidas ri = new riesgos_medidas();
                    ri.id = int.Parse(idRiesgo);
                    ri.imagen = rutabd;
                    response.Add(ri);
                }

                if (Request.Params["idmdSituacion"] != null)
                {
                    string idMedida = Request.Params["idmdSituacion"].ToString();
                    var ruta = System.IO.Path.Combine(Server.MapPath("~/Content/images/medidas/medidaspreventivas"), img2.FileName);
                    string rutabd = "../Content/images/medidas/medidaspreventivas/" + img2.FileName;
                    img2.SaveAs(ruta);
                    centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
                    medidaspreventivas_imagenes medidapreventiva = new medidaspreventivas_imagenes();
                    medidapreventiva.id_medida = int.Parse(idMedida);
                    medidapreventiva.rutaImagen = rutabd;
                    medidapreventiva.id_centro = centroseleccionado.id;
                    medidapreventiva.tamano = false;
                    Datos.ActualizarImagenRutaMedidaSituacionRiesgo(medidapreventiva);

                    datos = rutabd;
                    // var prueba= data;

                    riesgos_medidas ri = new riesgos_medidas();
                    ri.id = int.Parse(idMedida);
                    ri.imagen = rutabd;
                    response.Add(ri);
                }

            }
            catch (Exception ex)
            {

            }
            return Json(response);
        }



        public JsonResult ObtenerImagenNivel(string identificadorArea, string nivel)
        {
            string datos = "";
            List<areas_imagenes> response = new List<areas_imagenes>();
            try
            {
                if (nivel == "AREA")
                {
                    datos = Datos.ObtenerImagenNivel(int.Parse(identificadorArea));
                }
                else if (nivel == "SISTEMA")
                {
                    datos = Datos.ObtenerImagenNivel2(int.Parse(identificadorArea));
                }
                else if (nivel == "EQUIPO")
                {
                    datos = Datos.ObtenerImagenNivel3(int.Parse(identificadorArea));
                }
                else if (nivel == "NIVELCUATRO")
                {
                    datos = Datos.ObtenerImagenNivel4(int.Parse(identificadorArea));
                }

            }
            catch (Exception ex)
            {

            }
            return Json(datos);
        }

        public JsonResult GuardarImagenNivel()
        {
            string datos = "";

            try
            {
                HttpPostedFileBase img2 = Request.Files["photo"];
                string idNivel = Request.Params["idNivel"].ToString();
                string nivel = Request.Params["nivel"].ToString();

                centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));

                if (nivel == "AREA")
                {
                    var ruta = System.IO.Path.Combine(Server.MapPath("~/Content/images/areas"), img2.FileName);
                    string rutabd = "../Content/images/areas/" + img2.FileName;
                    img2.SaveAs(ruta);
                    List<areas_imagenes> response = new List<areas_imagenes>();
                    areas_imagenes oA_imagen = new areas_imagenes();
                    oA_imagen.id_areanivel1 = int.Parse(idNivel);
                    oA_imagen.rutaImagen = rutabd;
                    Datos.ActualizarImagenNivel(oA_imagen);
                    datos = rutabd;
                    areas_imagenes ri = new areas_imagenes();
                    ri.id = int.Parse(idNivel);
                    ri.rutaImagen = rutabd;
                    response.Add(ri);
                    return Json(response);
                }
                else if (nivel == "SISTEMA")
                {
                    var ruta = System.IO.Path.Combine(Server.MapPath("~/Content/images/sistemas"), img2.FileName);
                    string rutabd = "../Content/images/sistemas/" + img2.FileName;
                    img2.SaveAs(ruta);
                    List<areas2_imagenes> response = new List<areas2_imagenes>();
                    areas2_imagenes oA_imagen = new areas2_imagenes();
                    oA_imagen.id_areanivel2 = int.Parse(idNivel);
                    oA_imagen.rutaImagen = rutabd;
                    Datos.ActualizarImagenNivel2(oA_imagen);
                    datos = rutabd;
                    areas2_imagenes ri = new areas2_imagenes();
                    ri.id = int.Parse(idNivel);
                    ri.rutaImagen = rutabd;
                    response.Add(ri);
                    return Json(response);

                }
                else if (nivel == "EQUIPO")
                {
                    var ruta = System.IO.Path.Combine(Server.MapPath("~/Content/images/equipos"), img2.FileName);
                    string rutabd = "../Content/images/equipos/" + img2.FileName;
                    img2.SaveAs(ruta);
                    List<equipos_imagenes> response = new List<equipos_imagenes>();
                    equipos_imagenes oA_imagen = new equipos_imagenes();
                    oA_imagen.id_areanivel3 = int.Parse(idNivel);
                    oA_imagen.rutaImagen = rutabd;
                    Datos.ActualizarImagenNivel3(oA_imagen);
                    datos = rutabd;
                    equipos_imagenes ri = new equipos_imagenes();
                    ri.id = int.Parse(idNivel);
                    ri.rutaImagen = rutabd;
                    response.Add(ri);
                    return Json(response);
                }
                else if (nivel == "NIVELCUATRO")
                {
                    var ruta = System.IO.Path.Combine(Server.MapPath("~/Content/images/nivelcuatro"), img2.FileName);
                    string rutabd = "../Content/images/nivelcuatro/" + img2.FileName;
                    img2.SaveAs(ruta);
                    List<areas4_imagenes> response = new List<areas4_imagenes>();
                    areas4_imagenes oA_imagen = new areas4_imagenes();
                    oA_imagen.id_areanivel4 = int.Parse(idNivel);
                    oA_imagen.rutaImagen = rutabd;
                    Datos.ActualizarImagenNivel4(oA_imagen);
                    datos = rutabd;
                    areas4_imagenes ri = new areas4_imagenes();
                    ri.id = int.Parse(idNivel);
                    ri.rutaImagen = rutabd;
                    response.Add(ri);
                    return Json(response);
                }
                else
                {
                    return null;
                }


            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public JsonResult EliminaArea(string tecnologia, string area)
        {
            bool datos = false;

            try
            {

                centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
                List<matriz_centro> conjunto = new List<matriz_centro>();
                Datos.EliminarArea(int.Parse(area));
                Datos.RecalcularMatrizVersionFila(centroseleccionado.id, (int)Datos.DameVersionArea(int.Parse(area)), int.Parse(area));
                //ELIMINAR DATOS DE MATRIZ CENRO
                // Datos.EliminarDatosMatriz();

            }
            catch (Exception ex)
            {
                DIMASSTEntities bd = new DIMASSTEntities();
                log insertarlog = new log();
                insertarlog.usuario = Session["usuario"].ToString();
                insertarlog.fecha = DateTime.Now;
                insertarlog.evento = "Error al eliminar area: " + ex.Message;
                bd.log.Add(insertarlog);
                bd.SaveChanges();
            }
            return Json(datos);
        }
        public JsonResult EliminaSistema(string tecnologia, string area, string sistema)
        {
            bool datos = false;

            try
            {

                centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
                List<matriz_centro> conjunto = new List<matriz_centro>();
                int AreaEntero = Datos.ObtenerAreaPadre(int.Parse(sistema));
                int VersionEntero = (int)Datos.DameVersionSistema(int.Parse(sistema));

                Datos.EliminarSistema(int.Parse(sistema));
                Datos.RecalcularMatrizVersionFila(centroseleccionado.id, VersionEntero, AreaEntero);
                //Datos.RecalcularMatrizVersionFilaAsync(centroseleccionado.id, VersionEntero, AreaEntero);
            }
            catch (Exception ex)
            {
                DIMASSTEntities bd = new DIMASSTEntities();
                log insertarlog = new log();
                insertarlog.usuario = Session["usuario"].ToString();
                insertarlog.fecha = DateTime.Now;
                insertarlog.evento = "Error al eliminar area: " + ex.Message;
                bd.log.Add(insertarlog);
                bd.SaveChanges();
            }
            return Json(datos);
        }

        public JsonResult EliminaEquipo(string tecnologia, string area, string sistema, string equipo)
        {
            bool datos = false;

            try
            {

                centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
                List<matriz_centro> conjunto = new List<matriz_centro>();
                int AreaEntero = Datos.ObtenerAreaPadre(Datos.ObtenerSistemaPadre(int.Parse(equipo)));
                //int VersionEntero = Datos.ObtenerAreaPadre(Datos.ObtenerSistemaPadre(int.Parse(equipo)));
                int VersionEntero = (int)Datos.DameVersionArea(AreaEntero);
                Datos.EliminarEquipo(int.Parse(equipo));
                Datos.RecalcularMatrizVersionFila(centroseleccionado.id, VersionEntero, AreaEntero);
                //Datos.RecalcularMatrizVersion(centroseleccionado.id, VersionEntero);
                //Datos.RecalcularMatrizVersionFilaAsync(centroseleccionado.id, VersionEntero, AreaEntero);

            }
            catch (Exception ex)
            {
                DIMASSTEntities bd = new DIMASSTEntities();
                log insertarlog = new log();
                insertarlog.usuario = Session["usuario"].ToString();
                insertarlog.fecha = DateTime.Now;
                insertarlog.evento = "Error al eliminar area: " + ex.Message;
                bd.log.Add(insertarlog);
                bd.SaveChanges();
            }
            return Json(datos);
        }
        public JsonResult EliminaNivelCuatro(string tecnologia, string area, string sistema, string equipo, string nivelcuatro)
        {
            bool datos = false;
            try
            {
                centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
                List<matriz_centro> conjunto = new List<matriz_centro>();
                int AreaEntero = Datos.ObtenerAreaPadre(Datos.ObtenerSistemaPadre(Datos.ObtenerEquipoPadre(int.Parse(nivelcuatro))));
                int VersionEntero = (int)Datos.DameVersionArea(AreaEntero);
                Datos.EliminarNivel4(int.Parse(nivelcuatro));
                Datos.RecalcularMatrizVersionFila(centroseleccionado.id, VersionEntero, AreaEntero);
                //Datos.RecalcularMatrizVersionFilaAsync(centroseleccionado.id, VersionEntero, AreaEntero);

            }
            catch (Exception ex)
            {

            }
            return Json(datos);
        }
        public JsonResult GuardarMedidasCompuestas(string situacion, string descripcion)
        {
            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            bool datos = false;
            string[] stringSeparators = new string[] { "\n" };
            string[] lines = descripcion.Split(stringSeparators, StringSplitOptions.None);
            medidas_preventivas medidas = new medidas_preventivas();
            // medidas.codigo = codigo;

            medidas.id_situacion = int.Parse(situacion);
            medidas.descripcion = lines.First();
            medidas.id_centro = centroseleccionado.id;
            int id = Datos.ActualizarMedidaPreventiva(medidas);

            int contador = 1;
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

            return Json(datos);
        }

        public JsonResult GuardarVariasMedidas(string situacion, string descripcion)
        {
            bool datos = false;

            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));

            string[] stringSeparators = new string[] { "\n" };
            string[] lines = descripcion.Split(stringSeparators, StringSplitOptions.None);
            foreach (string s in lines)
            {
                if (!string.IsNullOrEmpty(s))
                {

                    medidas_preventivas medidas = new medidas_preventivas();
                    medidas.id_situacion = int.Parse(situacion);
                    medidas.descripcion = CleanInput(s);
                    medidas.id_centro = centroseleccionado.id;
                    int id = Datos.ActualizarMedidaPreventiva(medidas);
                }
            }
            return Json(datos);
        }

        public JsonResult GuardarMedidaSituacion(string situacion, string descripcion)
        {
            bool datos = false;
            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            medidas_preventivas medidas = new medidas_preventivas();
            // medidas.codigo = codigo;
            medidas.id_situacion = int.Parse(situacion);
            medidas.descripcion = descripcion;
            medidas.id_centro = centroseleccionado.id;
            int id = Datos.ActualizarMedidaPreventiva(medidas);
            return Json(datos);
        }

        public JsonResult GuardarSituacion(string riesgo, string descripcion)
        {
            bool datos = false;
            riesgos_situaciones medidas = new riesgos_situaciones();
            //medidas.codigo = codigo;
            medidas.id_tipo_riesgo = int.Parse(riesgo);
            medidas.descripcion = descripcion;
            int id = Datos.ActualizarSituacion(medidas);
            return Json(datos);
        }

        public JsonResult ActualizarRegistro(string[][] matrizRiesgos, string id, string nombre, string tecnologia, string version, string nivel)
        {
            string datos = "";
            Hashtable ht = new Hashtable();
            foreach (var item in matrizRiesgos)
            {
                ht.Add(int.Parse(item[0]), item[1] == "true" ? true : false);
            }
            try
            {
                centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
                int sistemaEntero = 0;
                int equipoEntero = 0;
                int id_seleccionado = 0;
                int tecnologiaEnt = 0;
                int.TryParse(tecnologia, out tecnologiaEnt);
                List<matriz_centro> conjunto = new List<matriz_centro>();



                switch (nivel)
                {

                    case "AREA":
                        areanivel1 area = new areanivel1();

                        int.TryParse(id, out sistemaEntero);
                        area.id = sistemaEntero;
                        area.nombre = nombre;
                        datos = nombre;
                        area.id_tecnologia = int.Parse(tecnologia);
                        id_seleccionado = Datos.ActualizarArea(area);
                        Datos.eliminarMatrizRiesgoArea(id_seleccionado, tecnologiaEnt);

                        foreach (var item in matrizRiesgos)
                        {
                            matriz_centro matriz = new matriz_centro();

                            matriz.id_centro = centroseleccionado.id;
                            matriz.id_areanivel1 = id_seleccionado;
                            matriz.id_areanivel2 = null;
                            matriz.id_areanivel3 = null;
                            matriz.id_tecnologia = int.Parse(tecnologia);
                            matriz.id_riesgo = int.Parse(item[0]);
                            matriz.activo = item[1] == "true" ? true : false;
                            matriz.version = int.Parse(version);
                            conjunto.Add(matriz);
                        }
                        Datos.InsertarMatriz(conjunto);
                        Datos.ActualizarFechaUsuarioMatriz(int.Parse(version), Session["usuario"].ToString());
                        break;

                    case "SISTEMA":
                        areanivel2 sistema = new areanivel2();

                        int.TryParse(id, out sistemaEntero);
                        sistema.id = sistemaEntero;
                        sistema.nombre = nombre;
                        datos = nombre;
                        id_seleccionado = Datos.ActualizarSistema(sistema);

                        //Datos.eliminarMatrizRiesgoSistemaAsync(id_seleccionado, tecnologiaEnt);
                        Datos.eliminarMatrizRiesgoSistema(id_seleccionado, tecnologiaEnt);

                        foreach (var item in matrizRiesgos)
                        {
                            matriz_centro matriz = new matriz_centro();

                            matriz.id_centro = centroseleccionado.id;
                            matriz.id_areanivel1 = null;
                            matriz.id_areanivel2 = id_seleccionado;
                            matriz.id_areanivel3 = null;
                            matriz.id_tecnologia = int.Parse(tecnologia);
                            matriz.id_riesgo = int.Parse(item[0]);
                            var areaPadre = Datos.ObtenerAreaPadre(id_seleccionado);
                            matriz.version = Datos.DameVersionArea(areaPadre);
                            matriz.activo = item[1] == "true" ? true : false;

                            conjunto.Add(matriz);


                        }
                        Datos.ActualizarFilaMatrizNivel2(conjunto, id_seleccionado);
                        Datos.InsertarMatriz(conjunto);
                        var padre1 = Datos.ObtenerAreaPadre(id_seleccionado);
                        int? versionsistema = Datos.DameVersionArea(padre1);
                        if (versionsistema != null)
                        {
                            Datos.ActualizarFechaUsuarioMatriz((int)versionsistema, Session["usuario"].ToString());
                            //Stopwatch st = new Stopwatch();
                            //st.Start();
                            Datos.RecalcularMatrizVersionFila(centroseleccionado.id, (int)versionsistema, padre1);
                            //Datos.RecalcularMatrizVersionFilaAsync(centroseleccionado.id, (int)versionsistema, padre);

                            //st.Stop();
                            //var tiempo = st.Elapsed.TotalMilliseconds;
                        }
                        break;

                    case "EQUIPO":

                        areanivel3 equipo = new areanivel3();

                        int.TryParse(id, out sistemaEntero);
                        equipo.id = sistemaEntero;
                        equipo.nombre = nombre;
                        datos = nombre;
                        id_seleccionado = Datos.ActualizarEquipo(equipo);
                        //List<matriz_centro> conjunto=Datos.eliminarMatrizRiesgoEquipo(id_seleccionado);
                        Datos.eliminarMatrizRiesgoEquipo(id_seleccionado, tecnologiaEnt);
                        //Datos.eliminarMatrizRiesgoEquipoAsync(id_seleccionado, tecnologiaEnt);
                        // conjunto = Datos.actualizarMatrizEquipo(ht,id_seleccionado);
                        //if (conjunto.Count == 0)
                        //{
                        foreach (var item in matrizRiesgos)
                        {
                            matriz_centro matriz = new matriz_centro();

                            matriz.id_centro = centroseleccionado.id;
                            matriz.id_areanivel1 = null;
                            matriz.id_areanivel2 = null;
                            matriz.id_areanivel3 = id_seleccionado;
                            matriz.id_tecnologia = int.Parse(tecnologia);
                            matriz.id_riesgo = int.Parse(item[0]);
                            var sistemaPadre = Datos.ObtenerSistemaPadre(id_seleccionado);
                            matriz.version = Datos.DameVersionSistema(sistemaPadre);
                            matriz.activo = item[1] == "true" ? true : false;
                            conjunto.Add(matriz);

                        }
                        Datos.InsertarMatriz(conjunto);
                        Datos.ActualizarFilaMatrizNivel3(conjunto, id_seleccionado);
                        var padre2 = Datos.ObtenerSistemaPadre(id_seleccionado);

                        int? versionequipo = Datos.DameVersionSistema(padre2);
                        if (versionequipo != null)
                        {
                            Datos.ActualizarFechaUsuarioMatriz((int)versionequipo, Session["usuario"].ToString());
                            var areaPadre = Datos.ObtenerAreaPadre(padre2);
                            Datos.RecalcularMatrizVersionFila(centroseleccionado.id, (int)versionequipo, areaPadre);
                            //Datos.RecalcularMatrizVersionFilaAsync(centroseleccionado.id, (int)versionequipo, Datos.ObtenerAreaPadre(Datos.ObtenerSistemaPadre(id_seleccionado)));
                        }
                        // }
                        break;
                    case "NIVELCUATRO":
                        areanivel4 areanivelcuatro = new areanivel4();

                        int.TryParse(id, out equipoEntero);
                        areanivelcuatro.id = equipoEntero;
                        areanivelcuatro.nombre = nombre;
                        datos = nombre;
                        id_seleccionado = Datos.ActualizarNivelCuatro(areanivelcuatro);
                        //Datos.eliminarMatrizRiesgoNivel4Async(id_seleccionado, tecnologiaEnt);
                        Datos.eliminarMatrizRiesgoNivel4(id_seleccionado, tecnologiaEnt);

                        foreach (var item in matrizRiesgos)
                        {
                            matriz_centro matriz = new matriz_centro();

                            matriz.id_centro = centroseleccionado.id;
                            matriz.id_areanivel1 = null;
                            matriz.id_areanivel2 = null;
                            matriz.id_areanivel3 = null;
                            matriz.id_areanivel4 = id_seleccionado;
                            matriz.id_tecnologia = int.Parse(tecnologia);
                            matriz.id_riesgo = int.Parse(item[0]);
                            var equipoPadre = Datos.ObtenerEquipoPadre(id_seleccionado);
                            var sistemaPadre = Datos.ObtenerSistemaPadre(equipoPadre);
                            matriz.version = Datos.DameVersionSistema(sistemaPadre);
                            matriz.activo = item[1] == "true" ? true : false;
                            conjunto.Add(matriz);

                        }
                        Datos.InsertarMatriz(conjunto);
                        Datos.ActualizarFilaMatrizNivel4(conjunto, id_seleccionado);
                        var padre3 = Datos.ObtenerEquipoPadre(id_seleccionado);

                        var sistPadre = Datos.ObtenerSistemaPadre(padre3);
                        int? versionnivel4 = Datos.DameVersionSistema(sistPadre);
                        if (versionnivel4 != null)
                        {
                            Datos.ActualizarFechaUsuarioMatriz((int)versionnivel4, Session["usuario"].ToString());
                            Datos.RecalcularMatrizVersionFila(centroseleccionado.id, (int)versionnivel4, Datos.ObtenerAreaPadre(Datos.ObtenerSistemaPadre(padre3)));
                            //Datos.RecalcularMatrizVersionFilaAsync(centroseleccionado.id, (int)versionnivel4, Datos.ObtenerAreaPadre(Datos.ObtenerSistemaPadre(padre3)));
                        }
                        break;
                    default:
                        break;

                }

                //foreach (matriz_centro item in conjunto.Where(x => x.id_areanivel3 != null))
                //{

                //    if (item.activo)
                //    {

                //        Datos.actualizarPadres(item, 3);
                //    }
                //}


                //foreach (matriz_centro item in conjunto.Where(x => x.id_areanivel2 != null))
                //{
                //    if (item.activo)
                //    {
                //        Datos.actualizarPadres(item, 2);
                //    }
                //}


            }
            catch (Exception ex)
            {
                DIMASSTEntities bd = new DIMASSTEntities();
                log insertarlog = new log();
                insertarlog.usuario = Session["usuario"].ToString();
                insertarlog.fecha = DateTime.Now;
                insertarlog.evento = "Error al guardar sistema en la mariz: " + ex.Message;
                bd.log.Add(insertarlog);
                bd.SaveChanges();
            }
            return Json(datos);
        }

        #region métodos para insertar en excel
        private static WorksheetPart
             GetWorksheetPartByName(SpreadsheetDocument document,
             string sheetName)
        {
            IEnumerable<Sheet> sheets =
               document.WorkbookPart.Workbook.GetFirstChild<Sheets>().
               Elements<Sheet>().Where(s => s.Name == sheetName);

            if (sheets.Count() == 0)
            {
                // The specified worksheet does not exist.

                return null;
            }

            string relationshipId = sheets.First().Id.Value;
            WorksheetPart worksheetPart = (WorksheetPart)
                 document.WorkbookPart.GetPartById(relationshipId);
            return worksheetPart;

        }

        // Given a worksheet, a column name, and a row index, 
        // gets the cell at the specified column and 
        private static Cell GetCell(Worksheet worksheet,
                  string columnName, uint rowIndex)
        {
            Row row = GetRow(worksheet, rowIndex);

            if (row == null)
                return null;

            return row.Elements<Cell>().Where(c => string.Compare
                   (c.CellReference.Value, columnName +
                   rowIndex, true) == 0).First();
        }


        // Given a worksheet and a row index, return the row.
        private static Row GetRow(Worksheet worksheet, uint rowIndex)
        {
            return worksheet.GetFirstChild<SheetData>().
              Elements<Row>().Where(r => r.RowIndex == rowIndex).First();
        }
        #endregion

        public ActionResult detalle_riesgo(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            Session["modulo"] = "11";

            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            int idCentral = centroseleccionado.id;

            if (Session["CentralElegida"] != null)
            {
                int centralElegida = int.Parse(Session["CentralElegida"].ToString());

                centros central = Datos.ObtenerCentroPorID(centralElegida);

                ViewData["accionesmejora"] = Datos.ListarAccionesMejora(centralElegida, 11, id);
            }

            Riesgos rie = Datos.GetDatosRiesgo(id);
            ViewData["riesgo"] = rie;
            ViewData["stakeholdersAsignables"] = Datos.ListarStakeholdersAsignablesRiesgo(id);
            ViewData["stakeholdersAsignadas"] = Datos.ListarStakeholdersAsignadosRiesgo(id);
            List<riesgos_categorias> listarCategorias = Datos.ListarCategorias();
            ViewData["categorias"] = listarCategorias;
            List<riesgos_tipologias> listarTipologias = Datos.ListarTipologias();
            ViewData["tipologias"] = listarTipologias;
            List<procesos> listarVC = Datos.ListarProcesosPorNivel(1, "M");
            ViewData["valuechain"] = listarVC;
            List<procesos> listarMP = Datos.ListarProcesosPorNivel(1, "S");
            ViewData["macroprocesos"] = listarMP;
            List<procesos> listarP = Datos.ListarProcesosPorNivel(1, "F");
            ViewData["procesos"] = listarP;

            #region generacion fichero
            if (Session["nombreArchivo"] != null && Session["nombreArchivo"].ToString() != "")
            {
                if (Session["nombreArchivo"].ToString().Contains("xlsx"))
                {
                    Response.ContentType = "application/msexcel";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=ExportacionRiesgos.xlsx");
                    Response.TransmitFile(Session["nombreArchivo"].ToString());
                    Response.End();
                }
                Session["nombreArchivo"] = "";
            }
            #endregion

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult detalle_riesgo(int id, FormCollection collection)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }
            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            int idCentral = centroseleccionado.id;

            formacion proce = new formacion();
            string formulario = collection["hdFormularioEjecutado"];

            if (formulario == "GuardarRiesgo")
            {
                #region guardar
                try
                {
                    if (id != 0)
                    {
                        #region actualizar
                        Riesgos actualizar = Datos.GetDatosRiesgo(id);

                        if (collection["ctl00$MainContent$ddlTipo"] != "")
                            actualizar.Tipo = int.Parse(collection["ctl00$MainContent$ddlTipo"]);
                        if (collection["ctl00$MainContent$ddlValueChain"] != "")
                            actualizar.idCadenaValor = int.Parse(collection["ctl00$MainContent$ddlValueChain"]);
                        if (collection["ctl00$MainContent$ddlMacroproceso"] != "")
                            actualizar.idMacroproceso = int.Parse(collection["ctl00$MainContent$ddlMacroproceso"]);
                        if (collection["ctl00$MainContent$ddlProceso"] != "")
                            actualizar.idProceso = int.Parse(collection["ctl00$MainContent$ddlProceso"]);
                        actualizar.Descripcion = collection["ctl00$MainContent$txtDescripcion"];
                        actualizar.Categoria = int.Parse(collection["ctl00$MainContent$ddlCategoria"]);
                        actualizar.Tipologia = int.Parse(collection["ctl00$MainContent$ddlTipologia"]);
                        if (collection["ctl00$MainContent$txtFechaCreacion"] != "")
                            actualizar.fechaCreacion = DateTime.Parse(collection["ctl00$MainContent$txtFechaCreacion"]);
                        else
                            actualizar.fechaCreacion = null;
                        if (collection["ctl00$MainContent$txtFechaModificacion"] != "")
                            actualizar.fechaModificacion = DateTime.Parse(collection["ctl00$MainContent$txtFechaModificacion"]);
                        else
                            actualizar.fechaModificacion = null;
                        actualizar.vigente = int.Parse(collection["ctl00$MainContent$ddlVigente"]);

                        if (collection["ctl00$MainContent$ddlProbabilidadRI"] != "")
                            actualizar.RI_ProbabilidadOcurrencia = collection["ctl00$MainContent$ddlProbabilidadRI"];
                        if (collection["ctl00$MainContent$ddlImpactoObjetivosRI"] != "")
                            actualizar.RI_ImpactoObjetivos = collection["ctl00$MainContent$ddlImpactoObjetivosRI"];
                        if (collection["ctl00$MainContent$ddlImpactoEconomicoRI"] != "")
                            actualizar.RI_ImpactoEconomico = collection["ctl00$MainContent$ddlImpactoEconomicoRI"];
                        if (collection["ctl00$MainContent$ddlImpactoProcesosRI"] != "")
                            actualizar.RI_ImpactoProcesosNegocio = collection["ctl00$MainContent$ddlImpactoProcesosRI"];
                        if (collection["ctl00$MainContent$ddlImpactoReputacionalRI"] != "")
                            actualizar.RI_ImpactoReputacional = collection["ctl00$MainContent$ddlImpactoReputacionalRI"];
                        if (collection["ctl00$MainContent$ddlImpactoCumplimientoRI"] != "")
                            actualizar.RI_ImpactoCumplimiento = collection["ctl00$MainContent$ddlImpactoCumplimientoRI"];

                        actualizar.RI_ImpactoGeneral = collection["ctl00$MainContent$txtImpactoGeneralRI"];
                        actualizar.RI_RelevanciaRiesgo = collection["ctl00$MainContent$txtRelevanciaRI"];
                        actualizar.RI_ValorRelevanciaRiesgo = collection["ctl00$MainContent$txtRelevanceValueRI"];
                        if (collection["ctl00$MainContent$ddlGestionRiesgoRI"] != "")
                            actualizar.RI_GestionRiesgo = collection["ctl00$MainContent$ddlGestionRiesgoRI"];

                        if (collection["ctl00$MainContent$chkLimitOOE"] != null)
                            actualizar.LimitOOE = true;
                        else
                            actualizar.LimitOOE = false;
                        if (collection["ctl00$MainContent$chkLimitOO"] != null)
                            actualizar.LimitOO = true;
                        else
                            actualizar.LimitOO = false;
                        if (collection["ctl00$MainContent$chkLimitE"] != null)
                            actualizar.LimitE = true;
                        else
                            actualizar.LimitE = false;
                        if (collection["ctl00$MainContent$chkSinLimit"] != null)
                            actualizar.SinLimit = true;
                        else
                            actualizar.SinLimit = false;
                        if (collection["ctl00$MainContent$chkSinEfectos"] != null)
                            actualizar.SinEfectos = true;
                        else
                            actualizar.SinEfectos = false;
                        if (collection["ctl00$MainContent$chkEfectD"] != null)
                            actualizar.EfectD = true;
                        else
                            actualizar.EfectD = false;
                        if (collection["ctl00$MainContent$chkEfectDI"] != null)
                            actualizar.EfectDI = true;
                        else
                            actualizar.EfectDI = false;

                        actualizar.ValoracionOportunidad = collection["ctl00$MainContent$txtValoracionOportunidad"];
                        actualizar.GestionOportunidad = collection["ctl00$MainContent$txtGestionOportunidad"];

                        actualizar.DescripcionControl = collection["ctl00$MainContent$txtDescripcionControl"];
                        actualizar.PropietarioControl = collection["ctl00$MainContent$txtPropietarioControl"];

                        if (collection["ctl00$MainContent$ddlProbabilidadRR"] != "")
                            actualizar.RR_ProbabilidadOcurrencia = collection["ctl00$MainContent$ddlProbabilidadRR"];
                        if (collection["ctl00$MainContent$ddlImpactoObjetivosRR"] != "")
                            actualizar.RR_ImpactoObjetivos = collection["ctl00$MainContent$ddlImpactoObjetivosRR"];
                        if (collection["ctl00$MainContent$ddlImpactoEconomicoRR"] != "")
                            actualizar.RR_ImpactoEconomico = collection["ctl00$MainContent$ddlImpactoEconomicoRR"];
                        if (collection["ctl00$MainContent$ddlImpactoProcesosRR"] != "")
                            actualizar.RR_ImpactoProcesosNegocio = collection["ctl00$MainContent$ddlImpactoProcesosRR"];
                        if (collection["ctl00$MainContent$ddlImpactoReputacionalRR"] != "")
                            actualizar.RR_ImpactoReputacional = collection["ctl00$MainContent$ddlImpactoReputacionalRR"];
                        if (collection["ctl00$MainContent$ddlImpactoCumplimientoRR"] != "")
                            actualizar.RR_ImpactoCumplimiento = collection["ctl00$MainContent$ddlImpactoCumplimientoRR"];

                        actualizar.RR_ImpactoGeneral = collection["ctl00$MainContent$txtImpactoGeneralRR"];
                        actualizar.RR_RelevanciaRiesgo = collection["ctl00$MainContent$txtRelevanciaRR"];
                        actualizar.RR_ValorRelevanciaRiesgo = collection["ctl00$MainContent$txtRelevanceValueRR"];

                        if (actualizar.Descripcion != string.Empty)
                        {
                            Datos.ActualizarRiesgo(actualizar);

                            string hdnStakeholders = collection["ctl00$MainContent$hdnPartesInteresadasSeleccionadas"].ToString();

                            string[] arraystakeholders = hdnStakeholders.Split(new char[] { ';' });

                            Datos.EliminarAsociacionRiesgoStakeholders(id);

                            if (arraystakeholders.Count() > 0)
                            {
                                for (int i = 0; i < arraystakeholders.Count() - 1; i++)
                                {
                                    Datos.AsociarRiesgoStakeholder(id, int.Parse(arraystakeholders[i]));
                                }
                            }

                            if (Session["CentralElegida"] != null)
                            {
                                int centralElegida = int.Parse(Session["CentralElegida"].ToString());

                                centros central = Datos.ObtenerCentroPorID(centralElegida);

                                ViewData["accionesmejora"] = Datos.ListarAccionesMejora(centralElegida, 11, id);
                            }

                            Session["EdicionRiesgoMensaje"] = "Riesgo actualizado correctamente";
                            Riesgos rie = Datos.GetDatosRiesgo(id);
                            ViewData["riesgo"] = rie;
                            ViewData["stakeholdersAsignables"] = Datos.ListarStakeholdersAsignablesRiesgo(id);
                            ViewData["stakeholdersAsignadas"] = Datos.ListarStakeholdersAsignadosRiesgo(id);
                            List<riesgos_categorias> listarCategorias = Datos.ListarCategorias();
                            ViewData["categorias"] = listarCategorias;
                            List<riesgos_tipologias> listarTipologias = Datos.ListarTipologias();
                            ViewData["tipologias"] = listarTipologias;
                            List<procesos> listarVC = Datos.ListarProcesosPorNivel(1, "M");
                            ViewData["valuechain"] = listarVC;
                            List<procesos> listarMP = Datos.ListarProcesosPorNivel(1, "S");
                            ViewData["macroprocesos"] = listarMP;
                            List<procesos> listarP = Datos.ListarProcesosPorNivel(1, "F");
                            ViewData["procesos"] = listarP;
                            return View();

                        }
                        else
                        {
                            Session["EdicionRiesgoError"] = "El campo descripción es obligatorio.";

                            if (Session["CentralElegida"] != null)
                            {
                                int centralElegida = int.Parse(Session["CentralElegida"].ToString());

                                centros central = Datos.ObtenerCentroPorID(centralElegida);

                                ViewData["accionesmejora"] = Datos.ListarAccionesMejora(centralElegida, 11, id);
                            }

                            Riesgos rie = Datos.GetDatosRiesgo(id);
                            ViewData["riesgo"] = rie;
                            ViewData["stakeholdersAsignables"] = Datos.ListarStakeholdersAsignablesRiesgo(id);
                            ViewData["stakeholdersAsignadas"] = Datos.ListarStakeholdersAsignadosRiesgo(id);
                            List<riesgos_categorias> listarCategorias = Datos.ListarCategorias();
                            ViewData["categorias"] = listarCategorias;
                            List<riesgos_tipologias> listarTipologias = Datos.ListarTipologias();
                            ViewData["tipologias"] = listarTipologias;
                            List<procesos> listarVC = Datos.ListarProcesosPorNivel(1, "M");
                            ViewData["valuechain"] = listarVC;
                            List<procesos> listarMP = Datos.ListarProcesosPorNivel(1, "S");
                            ViewData["macroprocesos"] = listarMP;
                            List<procesos> listarP = Datos.ListarProcesosPorNivel(1, "F");
                            ViewData["procesos"] = listarP;
                            return View();
                        }
                        #endregion
                    }
                    else
                    {
                        #region insertar
                        Riesgos insertar = new Riesgos();
                        if (collection["ctl00$MainContent$ddlTipo"] != "")
                            insertar.Tipo = int.Parse(collection["ctl00$MainContent$ddlTipo"]);
                        if (collection["ctl00$MainContent$ddlValueChain"] != "")
                            insertar.idCadenaValor = int.Parse(collection["ctl00$MainContent$ddlValueChain"]);
                        if (collection["ctl00$MainContent$ddlMacroproceso"] != "")
                            insertar.idMacroproceso = int.Parse(collection["ctl00$MainContent$ddlMacroproceso"]);
                        if (collection["ctl00$MainContent$ddlProceso"] != "")
                            insertar.idProceso = int.Parse(collection["ctl00$MainContent$ddlProceso"]);
                        insertar.Descripcion = collection["ctl00$MainContent$txtDescripcion"];
                        insertar.Categoria = int.Parse(collection["ctl00$MainContent$ddlCategoria"]);
                        insertar.Tipologia = int.Parse(collection["ctl00$MainContent$ddlTipologia"]);
                        if (collection["ctl00$MainContent$txtFechaCreacion"] != "")
                            insertar.fechaCreacion = DateTime.Parse(collection["ctl00$MainContent$txtFechaCreacion"]);
                        else
                            insertar.fechaCreacion = null;
                        if (collection["ctl00$MainContent$txtFechaModificacion"] != "")
                            insertar.fechaModificacion = DateTime.Parse(collection["ctl00$MainContent$txtFechaModificacion"]);
                        else
                            insertar.fechaModificacion = null;
                        insertar.vigente = int.Parse(collection["ctl00$MainContent$ddlVigente"]);

                        if (collection["ctl00$MainContent$ddlProbabilidadRI"] != "")
                            insertar.RI_ProbabilidadOcurrencia = collection["ctl00$MainContent$ddlProbabilidadRI"];
                        if (collection["ctl00$MainContent$ddlImpactoObjetivosRI"] != "")
                            insertar.RI_ImpactoObjetivos = collection["ctl00$MainContent$ddlImpactoObjetivosRI"];
                        if (collection["ctl00$MainContent$ddlImpactoEconomicoRI"] != "")
                            insertar.RI_ImpactoEconomico = collection["ctl00$MainContent$ddlImpactoEconomicoRI"];
                        if (collection["ctl00$MainContent$ddlImpactoProcesosRI"] != "")
                            insertar.RI_ImpactoProcesosNegocio = collection["ctl00$MainContent$ddlImpactoProcesosRI"];
                        if (collection["ctl00$MainContent$ddlImpactoReputacionalRI"] != "")
                            insertar.RI_ImpactoReputacional = collection["ctl00$MainContent$ddlImpactoReputacionalRI"];
                        if (collection["ctl00$MainContent$ddlImpactoCumplimientoRI"] != "")
                            insertar.RI_ImpactoCumplimiento = collection["ctl00$MainContent$ddlImpactoCumplimientoRI"];

                        insertar.RI_ImpactoGeneral = collection["ctl00$MainContent$txtImpactoGeneralRI"];
                        insertar.RI_RelevanciaRiesgo = collection["ctl00$MainContent$txtRelevanciaRI"];
                        insertar.RI_ValorRelevanciaRiesgo = collection["ctl00$MainContent$txtRelevanceValueRI"];
                        if (collection["ctl00$MainContent$ddlGestionRiesgoRI"] != "")
                            insertar.RI_GestionRiesgo = collection["ctl00$MainContent$ddlGestionRiesgoRI"];

                        if (collection["ctl00$MainContent$chkLimitOOE"] != null)
                            insertar.LimitOOE = true;
                        else
                            insertar.LimitOOE = false;
                        if (collection["ctl00$MainContent$chkLimitOO"] != null)
                            insertar.LimitOO = true;
                        else
                            insertar.LimitOO = false;
                        if (collection["ctl00$MainContent$chkLimitE"] != null)
                            insertar.LimitE = true;
                        else
                            insertar.LimitE = false;
                        if (collection["ctl00$MainContent$chkSinLimit"] != null)
                            insertar.SinLimit = true;
                        else
                            insertar.SinLimit = false;
                        if (collection["ctl00$MainContent$chkSinEfectos"] != null)
                            insertar.SinEfectos = true;
                        else
                            insertar.SinEfectos = false;
                        if (collection["ctl00$MainContent$chkEfectD"] != null)
                            insertar.EfectD = true;
                        else
                            insertar.EfectD = false;
                        if (collection["ctl00$MainContent$chkEfectDI"] != null)
                            insertar.EfectDI = true;
                        else
                            insertar.EfectDI = false;

                        insertar.ValoracionOportunidad = collection["ctl00$MainContent$txtValoracionOportunidad"];
                        insertar.GestionOportunidad = collection["ctl00$MainContent$txtGestionOportunidad"];

                        insertar.DescripcionControl = collection["ctl00$MainContent$txtDescripcionControl"];
                        insertar.PropietarioControl = collection["ctl00$MainContent$txtPropietarioControl"];

                        if (collection["ctl00$MainContent$ddlProbabilidadRR"] != "")
                            insertar.RR_ProbabilidadOcurrencia = collection["ctl00$MainContent$ddlProbabilidadRR"];
                        if (collection["ctl00$MainContent$ddlImpactoObjetivosRR"] != "")
                            insertar.RR_ImpactoObjetivos = collection["ctl00$MainContent$ddlImpactoObjetivosRR"];
                        if (collection["ctl00$MainContent$ddlImpactoEconomicoRR"] != "")
                            insertar.RR_ImpactoEconomico = collection["ctl00$MainContent$ddlImpactoEconomicoRR"];
                        if (collection["ctl00$MainContent$ddlImpactoProcesosRR"] != "")
                            insertar.RR_ImpactoProcesosNegocio = collection["ctl00$MainContent$ddlImpactoProcesosRR"];
                        if (collection["ctl00$MainContent$ddlImpactoReputacionalRR"] != "")
                            insertar.RR_ImpactoReputacional = collection["ctl00$MainContent$ddlImpactoReputacionalRR"];
                        if (collection["ctl00$MainContent$ddlImpactoCumplimientoRR"] != "")
                            insertar.RR_ImpactoCumplimiento = collection["ctl00$MainContent$ddlImpactoCumplimientoRR"];

                        insertar.RR_ImpactoGeneral = collection["ctl00$MainContent$txtImpactoGeneralRR"];
                        insertar.RR_RelevanciaRiesgo = collection["ctl00$MainContent$txtRelevanciaRR"];
                        insertar.RR_ValorRelevanciaRiesgo = collection["ctl00$MainContent$txtRelevanceValueRR"];

                        insertar.idCentral = centroseleccionado.id;

                        if (insertar.Descripcion != string.Empty)
                        {
                            int idForm = Datos.ActualizarRiesgo(insertar);

                            string hdnStakeholders = collection["ctl00$MainContent$hdnPartesInteresadasSeleccionadas"].ToString();

                            string[] arraystakeholders = hdnStakeholders.Split(new char[] { ';' });

                            if (arraystakeholders.Count() > 0)
                            {
                                for (int i = 0; i < arraystakeholders.Count() - 1; i++)
                                {
                                    Datos.AsociarRiesgoStakeholder(idForm, int.Parse(arraystakeholders[i]));
                                }
                            }

                            Session["EdicionRiesgoMensaje"] = "Riesgo actualizado correctamente";

                            if (Session["CentralElegida"] != null)
                            {
                                int centralElegida = int.Parse(Session["CentralElegida"].ToString());

                                centros central = Datos.ObtenerCentroPorID(centralElegida);

                                ViewData["accionesmejora"] = Datos.ListarAccionesMejora(centralElegida, 11, id);
                            }

                            Riesgos rie = Datos.GetDatosRiesgo(id);
                            ViewData["riesgo"] = rie;
                            ViewData["stakeholdersAsignables"] = Datos.ListarStakeholdersAsignablesRiesgo(id);
                            ViewData["stakeholdersAsignadas"] = Datos.ListarStakeholdersAsignadosRiesgo(id);
                            List<riesgos_categorias> listarCategorias = Datos.ListarCategorias();
                            ViewData["categorias"] = listarCategorias;
                            List<riesgos_tipologias> listarTipologias = Datos.ListarTipologias();
                            ViewData["tipologias"] = listarTipologias;
                            List<procesos> listarVC = Datos.ListarProcesosPorNivel(1, "M");
                            ViewData["valuechain"] = listarVC;
                            List<procesos> listarMP = Datos.ListarProcesosPorNivel(1, "S");
                            ViewData["macroprocesos"] = listarMP;
                            List<procesos> listarP = Datos.ListarProcesosPorNivel(1, "F");
                            ViewData["procesos"] = listarP;
                            return Redirect(Url.RouteUrl(new { controller = "Riesgos", action = "detalle_riesgo", id = idForm }));
                        }
                        else
                        {
                            Session["EdicionRiesgoError"] = "El campo Descripción es obligatorio.";

                            if (Session["CentralElegida"] != null)
                            {
                                int centralElegida = int.Parse(Session["CentralElegida"].ToString());

                                centros central = Datos.ObtenerCentroPorID(centralElegida);

                                ViewData["accionesmejora"] = Datos.ListarAccionesMejora(centralElegida, 11, id);
                            }

                            Riesgos rie = Datos.GetDatosRiesgo(id);
                            ViewData["riesgo"] = rie;
                            ViewData["stakeholdersAsignables"] = Datos.ListarStakeholdersAsignablesRiesgo(id);
                            ViewData["stakeholdersAsignadas"] = Datos.ListarStakeholdersAsignadosRiesgo(id);
                            List<riesgos_categorias> listarCategorias = Datos.ListarCategorias();
                            ViewData["categorias"] = listarCategorias;
                            List<riesgos_tipologias> listarTipologias = Datos.ListarTipologias();
                            ViewData["tipologias"] = listarTipologias;
                            List<procesos> listarVC = Datos.ListarProcesosPorNivel(1, "M");
                            ViewData["valuechain"] = listarVC;
                            List<procesos> listarMP = Datos.ListarProcesosPorNivel(1, "S");
                            ViewData["macroprocesos"] = listarMP;
                            List<procesos> listarP = Datos.ListarProcesosPorNivel(1, "F");
                            ViewData["procesos"] = listarP;
                            return View();
                        }
                        #endregion
                    }
                }
                catch (Exception ex)
                {
                    if (Session["CentralElegida"] != null)
                    {
                        int centralElegida = int.Parse(Session["CentralElegida"].ToString());

                        centros central = Datos.ObtenerCentroPorID(centralElegida);

                        ViewData["accionesmejora"] = Datos.ListarAccionesMejora(centralElegida, 11, id);
                    }
                    Riesgos rie = Datos.GetDatosRiesgo(id);
                    ViewData["riesgo"] = rie;
                    ViewData["stakeholdersAsignables"] = Datos.ListarStakeholdersAsignablesRiesgo(id);
                    ViewData["stakeholdersAsignadas"] = Datos.ListarStakeholdersAsignadosRiesgo(id);
                    List<riesgos_categorias> listarCategorias = Datos.ListarCategorias();
                    ViewData["categorias"] = listarCategorias;
                    List<riesgos_tipologias> listarTipologias = Datos.ListarTipologias();
                    ViewData["tipologias"] = listarTipologias;
                    List<procesos> listarVC = Datos.ListarProcesosPorNivel(1, "M");
                    ViewData["valuechain"] = listarVC;
                    List<procesos> listarMP = Datos.ListarProcesosPorNivel(1, "S");
                    ViewData["macroprocesos"] = listarMP;
                    List<procesos> listarP = Datos.ListarProcesosPorNivel(1, "F");
                    ViewData["procesos"] = listarP;
                    return View();
                }
                #endregion
            }

            if (formulario == "btnImprimir")
            {
                VISTA_ObtenerUsuario user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());

                List<VISTA_Riesgo> listadoRiesgos = Datos.ListarRiesgosFicha(centroseleccionado.id);
                listadoRiesgos = listadoRiesgos.Where(x => x.Id == id).ToList();

                #region generacion fichero
                string sourceFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaPlantillaWord"] + "ExportacionRiesgos.xlsx");
                string destinationFile = this.Server.MapPath(ConfigurationManager.AppSettings["RutaGeneracionWord"] + "ExportacionRiesgos_" + DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + ".xlsx");

                Session["source"] = sourceFile;
                Session["destino"] = destinationFile;
                // Create a copy of the template file and open the copy 
                System.IO.File.Copy(sourceFile, destinationFile, true);

                #region impresion

                using (SpreadsheetDocument document = SpreadsheetDocument.Open(destinationFile, true))
                {
                    WorksheetPart worksheetPart = GetWorksheetPartByName(document, "Riesgos y Oportunidades");
                    uint indiceFila = 12;
                    if (worksheetPart != null)
                    {
                        foreach (VISTA_Riesgo ries in listadoRiesgos)
                        {
                            Row row = new Row();
                            #region inserción campos en excel
                            if (ries.CodigoRiesgo != null)
                            {
                                Cell codigo = GetCell(worksheetPart.Worksheet, "A", indiceFila);
                                codigo.CellValue = new CellValue(ries.CodigoRiesgo);
                                codigo.DataType = new EnumValue<CellValues>(CellValues.Number);
                            }

                            if (ries.Tipo != null)
                            {
                                Cell parteinteresada = GetCell(worksheetPart.Worksheet, "B", indiceFila);
                                if (ries.Tipo == 1)
                                    parteinteresada.CellValue = new CellValue("Riesgo");
                                else
                                    parteinteresada.CellValue = new CellValue("Oportunidad");
                                parteinteresada.DataType = new EnumValue<CellValues>(CellValues.String);
                            }

                            if (ries.Descripcion != null)
                            {
                                Cell descripcion = GetCell(worksheetPart.Worksheet, "C", indiceFila);
                                descripcion.CellValue = new CellValue(ries.Descripcion);
                                descripcion.DataType = new EnumValue<CellValues>(CellValues.String);
                            }

                            if (ries.idCadenaValor != null && ries.idCadenaValor != 0)
                            {
                                Cell cadenavalor = GetCell(worksheetPart.Worksheet, "D", indiceFila);
                                cadenavalor.CellValue = new CellValue(Datos.GetDatosProceso(int.Parse(ries.idCadenaValor.ToString())).nombre);
                                cadenavalor.DataType = new EnumValue<CellValues>(CellValues.String);
                            }

                            if (ries.idMacroproceso != null && ries.idMacroproceso != 0)
                            {
                                Cell macroproceso = GetCell(worksheetPart.Worksheet, "E", indiceFila);
                                macroproceso.CellValue = new CellValue(Datos.GetDatosProceso(int.Parse(ries.idMacroproceso.ToString())).nombre);
                                macroproceso.DataType = new EnumValue<CellValues>(CellValues.String);
                            }

                            if (ries.idProceso != null && ries.idProceso != 0)
                            {
                                Cell proceso = GetCell(worksheetPart.Worksheet, "F", indiceFila);
                                proceso.CellValue = new CellValue(Datos.GetDatosProceso(int.Parse(ries.idProceso.ToString())).nombre);
                                proceso.DataType = new EnumValue<CellValues>(CellValues.String);
                            }

                            if (ries.categoria != null)
                            {
                                Cell categoria = GetCell(worksheetPart.Worksheet, "G", indiceFila);
                                categoria.CellValue = new CellValue(ries.categoria);
                                categoria.DataType = new EnumValue<CellValues>(CellValues.String);
                            }

                            if (ries.tipologia != null)
                            {
                                Cell tipologia = GetCell(worksheetPart.Worksheet, "H", indiceFila);
                                tipologia.CellValue = new CellValue(ries.tipologia);
                                tipologia.DataType = new EnumValue<CellValues>(CellValues.String);
                            }

                            if (ries.RI_ProbabilidadOcurrencia != null)
                            {
                                Cell probabilidadRI = GetCell(worksheetPart.Worksheet, "I", indiceFila);
                                probabilidadRI.CellValue = new CellValue(ries.RI_ProbabilidadOcurrencia);
                                probabilidadRI.DataType = new EnumValue<CellValues>(CellValues.String);
                            }

                            if (ries.RI_ImpactoObjetivos != null)
                            {
                                Cell impactoobjetivosRI = GetCell(worksheetPart.Worksheet, "J", indiceFila);
                                impactoobjetivosRI.CellValue = new CellValue(ries.RI_ImpactoObjetivos);
                                impactoobjetivosRI.DataType = new EnumValue<CellValues>(CellValues.String);
                            }

                            if (ries.RI_ImpactoEconomico != null)
                            {
                                Cell impactoeconomicoRI = GetCell(worksheetPart.Worksheet, "K", indiceFila);
                                impactoeconomicoRI.CellValue = new CellValue(ries.RI_ImpactoEconomico);
                                impactoeconomicoRI.DataType = new EnumValue<CellValues>(CellValues.String);
                            }

                            if (ries.RI_ImpactoProcesosNegocio != null)
                            {
                                Cell impactoprocesosRI = GetCell(worksheetPart.Worksheet, "L", indiceFila);
                                impactoprocesosRI.CellValue = new CellValue(ries.RI_ImpactoProcesosNegocio);
                                impactoprocesosRI.DataType = new EnumValue<CellValues>(CellValues.String);
                            }

                            if (ries.RI_ImpactoReputacional != null)
                            {
                                Cell impactoreputacionalRI = GetCell(worksheetPart.Worksheet, "M", indiceFila);
                                impactoreputacionalRI.CellValue = new CellValue(ries.RI_ImpactoReputacional);
                                impactoreputacionalRI.DataType = new EnumValue<CellValues>(CellValues.String);
                            }

                            if (ries.RI_ImpactoCumplimiento != null)
                            {
                                Cell impactocumplimientoRI = GetCell(worksheetPart.Worksheet, "N", indiceFila);
                                impactocumplimientoRI.CellValue = new CellValue(ries.RI_ImpactoCumplimiento);
                                impactocumplimientoRI.DataType = new EnumValue<CellValues>(CellValues.String);
                            }

                            if (ries.RI_ImpactoGeneral != null)
                            {
                                Cell impactogeneralRI = GetCell(worksheetPart.Worksheet, "O", indiceFila);
                                impactogeneralRI.CellValue = new CellValue(ries.RI_ImpactoGeneral);
                                impactogeneralRI.DataType = new EnumValue<CellValues>(CellValues.String);
                            }

                            if (ries.RI_RelevanciaRiesgo != null)
                            {
                                Cell relevanciariesgoRI = GetCell(worksheetPart.Worksheet, "P", indiceFila);
                                relevanciariesgoRI.CellValue = new CellValue(ries.RI_RelevanciaRiesgo);
                                relevanciariesgoRI.DataType = new EnumValue<CellValues>(CellValues.String);
                            }

                            if (ries.RI_ValorRelevanciaRiesgo != null)
                            {
                                Cell relevancevalueRI = GetCell(worksheetPart.Worksheet, "Q", indiceFila);
                                relevancevalueRI.CellValue = new CellValue(ries.RI_ValorRelevanciaRiesgo);
                                relevancevalueRI.DataType = new EnumValue<CellValues>(CellValues.String);
                            }

                            if (ries.RI_GestionRiesgo != null)
                            {
                                Cell gestionriesgo = GetCell(worksheetPart.Worksheet, "R", indiceFila);
                                gestionriesgo.CellValue = new CellValue(ries.RI_GestionRiesgo);
                                gestionriesgo.DataType = new EnumValue<CellValues>(CellValues.String);
                            }

                            if (ries.LimitOOE != null && ries.LimitOOE == true)
                            {
                                Cell limitOOE = GetCell(worksheetPart.Worksheet, "S", indiceFila);
                                limitOOE.CellValue = new CellValue("X");
                                limitOOE.DataType = new EnumValue<CellValues>(CellValues.String);
                            }

                            if (ries.LimitOO != null && ries.LimitOO == true)
                            {
                                Cell limitOO = GetCell(worksheetPart.Worksheet, "T", indiceFila);
                                limitOO.CellValue = new CellValue("X");
                                limitOO.DataType = new EnumValue<CellValues>(CellValues.String);
                            }

                            if (ries.LimitE != null && ries.LimitE == true)
                            {
                                Cell limitE = GetCell(worksheetPart.Worksheet, "U", indiceFila);
                                limitE.CellValue = new CellValue("X");
                                limitE.DataType = new EnumValue<CellValues>(CellValues.String);
                            }

                            if (ries.SinLimit != null && ries.SinLimit == true)
                            {
                                Cell sinlimit = GetCell(worksheetPart.Worksheet, "V", indiceFila);
                                sinlimit.CellValue = new CellValue("X");
                                sinlimit.DataType = new EnumValue<CellValues>(CellValues.String);
                            }

                            if (ries.SinEfectos != null && ries.SinEfectos == true)
                            {
                                Cell sinefectos = GetCell(worksheetPart.Worksheet, "W", indiceFila);
                                sinefectos.CellValue = new CellValue("X");
                                sinefectos.DataType = new EnumValue<CellValues>(CellValues.String);
                            }

                            if (ries.EfectD != null && ries.EfectD == true)
                            {
                                Cell efectd = GetCell(worksheetPart.Worksheet, "X", indiceFila);
                                efectd.CellValue = new CellValue("X");
                                efectd.DataType = new EnumValue<CellValues>(CellValues.String);
                            }

                            if (ries.EfectDI != null && ries.EfectDI == true)
                            {
                                Cell efectdi = GetCell(worksheetPart.Worksheet, "Y", indiceFila);
                                efectdi.CellValue = new CellValue("X");
                                efectdi.DataType = new EnumValue<CellValues>(CellValues.String);
                            }

                            if (ries.ValoracionOportunidad != null)
                            {
                                Cell valoracionoportunidad = GetCell(worksheetPart.Worksheet, "Z", indiceFila);
                                valoracionoportunidad.CellValue = new CellValue(ries.ValoracionOportunidad);
                                valoracionoportunidad.DataType = new EnumValue<CellValues>(CellValues.String);
                            }

                            if (ries.GestionOportunidad != null)
                            {
                                Cell gestionoportunidad = GetCell(worksheetPart.Worksheet, "AA", indiceFila);
                                gestionoportunidad.CellValue = new CellValue(ries.GestionOportunidad);
                                gestionoportunidad.DataType = new EnumValue<CellValues>(CellValues.String);
                            }

                            if (ries.DescripcionControl != null)
                            {
                                Cell descripcioncontrol = GetCell(worksheetPart.Worksheet, "AB", indiceFila);
                                descripcioncontrol.CellValue = new CellValue(ries.DescripcionControl);
                                descripcioncontrol.DataType = new EnumValue<CellValues>(CellValues.String);
                            }

                            if (ries.PropietarioControl != null)
                            {
                                Cell propietariocontrol = GetCell(worksheetPart.Worksheet, "AC", indiceFila);
                                propietariocontrol.CellValue = new CellValue(ries.PropietarioControl);
                                propietariocontrol.DataType = new EnumValue<CellValues>(CellValues.String);
                            }

                            if (ries.RR_ProbabilidadOcurrencia != null)
                            {
                                Cell probabilidadRR = GetCell(worksheetPart.Worksheet, "AD", indiceFila);
                                probabilidadRR.CellValue = new CellValue(ries.RR_ProbabilidadOcurrencia);
                                probabilidadRR.DataType = new EnumValue<CellValues>(CellValues.String);
                            }

                            if (ries.RR_ImpactoObjetivos != null)
                            {
                                Cell impactoobjetivosRR = GetCell(worksheetPart.Worksheet, "AE", indiceFila);
                                impactoobjetivosRR.CellValue = new CellValue(ries.RR_ImpactoObjetivos);
                                impactoobjetivosRR.DataType = new EnumValue<CellValues>(CellValues.String);
                            }

                            if (ries.RR_ImpactoEconomico != null)
                            {
                                Cell impactoeconomicoRR = GetCell(worksheetPart.Worksheet, "AF", indiceFila);
                                impactoeconomicoRR.CellValue = new CellValue(ries.RR_ImpactoEconomico);
                                impactoeconomicoRR.DataType = new EnumValue<CellValues>(CellValues.String);
                            }

                            if (ries.RR_ImpactoProcesosNegocio != null)
                            {
                                Cell impactoprocesosRR = GetCell(worksheetPart.Worksheet, "AG", indiceFila);
                                impactoprocesosRR.CellValue = new CellValue(ries.RR_ImpactoProcesosNegocio);
                                impactoprocesosRR.DataType = new EnumValue<CellValues>(CellValues.String);
                            }

                            if (ries.RR_ImpactoReputacional != null)
                            {
                                Cell impactoreputacionalRR = GetCell(worksheetPart.Worksheet, "AH", indiceFila);
                                impactoreputacionalRR.CellValue = new CellValue(ries.RR_ImpactoReputacional);
                                impactoreputacionalRR.DataType = new EnumValue<CellValues>(CellValues.String);
                            }

                            if (ries.RR_ImpactoCumplimiento != null)
                            {
                                Cell impactocumplimientoRR = GetCell(worksheetPart.Worksheet, "AI", indiceFila);
                                impactocumplimientoRR.CellValue = new CellValue(ries.RR_ImpactoCumplimiento);
                                impactocumplimientoRR.DataType = new EnumValue<CellValues>(CellValues.String);
                            }

                            if (ries.RR_ImpactoGeneral != null)
                            {
                                Cell impactogeneralRR = GetCell(worksheetPart.Worksheet, "AJ", indiceFila);
                                impactogeneralRR.CellValue = new CellValue(ries.RR_ImpactoGeneral);
                                impactogeneralRR.DataType = new EnumValue<CellValues>(CellValues.String);
                            }

                            if (ries.RR_RelevanciaRiesgo != null)
                            {
                                Cell relevanciariesgoRR = GetCell(worksheetPart.Worksheet, "AK", indiceFila);
                                relevanciariesgoRR.CellValue = new CellValue(ries.RR_RelevanciaRiesgo);
                                relevanciariesgoRR.DataType = new EnumValue<CellValues>(CellValues.String);
                            }

                            if (ries.RR_ValorRelevanciaRiesgo != null)
                            {
                                Cell relevancevalueRR = GetCell(worksheetPart.Worksheet, "AL", indiceFila);
                                relevancevalueRR.CellValue = new CellValue(ries.RR_ValorRelevanciaRiesgo);
                                relevancevalueRR.DataType = new EnumValue<CellValues>(CellValues.String);
                            }
                            #endregion
                            indiceFila++;
                        }
                    }

                    // save worksheet
                    document.WorkbookPart.WorksheetParts.First().Worksheet.Save();

                    document.WorkbookPart.Workbook.Save();

                    Session["nombreArchivo"] = destinationFile;
                }
                #endregion
                #endregion

                if (Session["CentralElegida"] != null)
                {
                    int centralElegida = int.Parse(Session["CentralElegida"].ToString());

                    centros central = Datos.ObtenerCentroPorID(centralElegida);

                    ViewData["accionesmejora"] = Datos.ListarAccionesMejora(centralElegida, 11, id);
                }
                Riesgos rie = Datos.GetDatosRiesgo(id);
                ViewData["riesgo"] = rie;
                ViewData["stakeholdersAsignables"] = Datos.ListarStakeholdersAsignablesRiesgo(id);
                ViewData["stakeholdersAsignadas"] = Datos.ListarStakeholdersAsignadosRiesgo(id);
                List<riesgos_categorias> listarCategorias = Datos.ListarCategorias();
                ViewData["categorias"] = listarCategorias;
                List<riesgos_tipologias> listarTipologias = Datos.ListarTipologias();
                ViewData["tipologias"] = listarTipologias;
                List<procesos> listarVC = Datos.ListarProcesosPorNivel(1, "M");
                ViewData["valuechain"] = listarVC;
                List<procesos> listarMP = Datos.ListarProcesosPorNivel(1, "S");
                ViewData["macroprocesos"] = listarMP;
                List<procesos> listarP = Datos.ListarProcesosPorNivel(1, "F");
                ViewData["procesos"] = listarP;
                return Redirect(Url.RouteUrl(new { controller = "Riesgos", action = "detalle_riesgo", id = id }));
            }
            else
            {
                if (Session["CentralElegida"] != null)
                {
                    int centralElegida = int.Parse(Session["CentralElegida"].ToString());

                    centros central = Datos.ObtenerCentroPorID(centralElegida);

                    ViewData["accionesmejora"] = Datos.ListarAccionesMejora(centralElegida, 11, id);
                }
                Riesgos rie = Datos.GetDatosRiesgo(id);
                ViewData["riesgo"] = rie;
                ViewData["stakeholdersAsignables"] = Datos.ListarStakeholdersAsignablesRiesgo(id);
                ViewData["stakeholdersAsignadas"] = Datos.ListarStakeholdersAsignadosRiesgo(id);
                List<riesgos_categorias> listarCategorias = Datos.ListarCategorias();
                ViewData["categorias"] = listarCategorias;
                List<riesgos_tipologias> listarTipologias = Datos.ListarTipologias();
                ViewData["tipologias"] = listarTipologias;
                List<procesos> listarVC = Datos.ListarProcesosPorNivel(1, "M");
                ViewData["valuechain"] = listarVC;
                List<procesos> listarMP = Datos.ListarProcesosPorNivel(1, "S");
                ViewData["macroprocesos"] = listarMP;
                List<procesos> listarP = Datos.ListarProcesosPorNivel(1, "F");
                ViewData["procesos"] = listarP;
                return View();
            }
        }



        //public void recalcularChecksMatriz(int centro, int version)
        //{
        //    List<matriz_centro> matriz = Datos.listarMatrizCentro(centro, version);
        //    List<areanivel1> areas = Datos.ListarAreas(version).Where(x=>x.id_centro==centro).ToList();

        //    //recorrer todas las areas solo si tienen hijos
        //    foreach (areanivel3 area in areas)
        //    {   
        //        foreach (areanivel2 area2 in Datos.ListarSistemaPorIDArea(area.id).Where(x => x.areanivel3.Count > 0))
        //        {                    
        //            foreach (areanivel3 area3 in Datos.ListarEquipos(area2.id))
        //            {
        //                bool algunTercerNivelActivo = false;
        //                foreach (matriz_centro item in matriz.Where(x => x.id_areanivel3 == area3.id))
        //                {                            
        //                    if (item.activo)
        //                    {
        //                        algunTercerNivelActivo = true;
        //                        matriz_centro padre= matriz.Where(x => x.id_riesgo == item.id_riesgo && x.version == item.version &&  x.id_areanivel2==area3.id_areanivel2).First();

        //                        //Datos.ActualizarMatriz();
        //                        padre.activo = true;
        //                    }

        //                }                       
        //            }



        //        }

        //    }
        //}

        public bool copiarMatriz(List<tecnologias> tec, List<areanivel1> areas, int centro, int versionDestino, List<matriz_centro> listaMatricesMaestro)
        {
            bool correcto = true;
            try
            {
                List<matriz_centro> matrizNueva = new List<matriz_centro>();
                foreach (tecnologias tecno in tec)
                {
                    matriz_centro mc = new matriz_centro();
                    foreach (areanivel1 area in areas)
                    {
                        if (area.id_tecnologia == tecno.id)
                        {
                            areanivel1 copiaArea = new areanivel1();
                            copiaArea.nombre = area.nombre;
                            copiaArea.codigo = area.codigo;
                            copiaArea.id_centro = centro;
                            copiaArea.id_tecnologia = tecno.id;
                            int id_area_seleccionada = Datos.ActualizarArea(copiaArea);
                            copiaArea.id = id_area_seleccionada;

                            foreach (VISTA_tipos_riesgos riesgo in Datos.ListarTiposRiesgos())
                            {

                                matriz_centro matriz = new matriz_centro();
                                matriz.id_centro = centro;
                                matriz.id_areanivel1 = copiaArea.id;
                                matriz.id_areanivel2 = null;
                                matriz.id_areanivel3 = null;
                                matriz.id_tecnologia = tecno.id;
                                matriz.id_riesgo = riesgo.id;
                                matriz.activo = listaMatricesMaestro.Where(z => z.id_areanivel1 == area.id && z.id_riesgo == matriz.id_riesgo).Select(x => x.activo).DefaultIfEmpty(false).First();

                                matriz.version = versionDestino;
                                matrizNueva.Add(matriz);

                            }
                            if (area.id_tecnologia == tecno.id)
                            {
                                Datos.InsertarMatriz(matrizNueva);
                                matrizNueva.Clear();
                            }

                            foreach (areanivel2 sistema in Datos.ListarSistema().Where(x => x.id_areanivel1 == area.id))
                            {
                                areanivel2 copiaSistema = new areanivel2();
                                copiaSistema.nombre = sistema.nombre;
                                copiaSistema.codigo = sistema.codigo;
                                copiaSistema.id_areanivel1 = copiaArea.id;
                                int id_sistema_seleccionado = Datos.ActualizarSistema(copiaSistema);
                                copiaSistema.id = id_sistema_seleccionado;

                                foreach (VISTA_tipos_riesgos riesgo in Datos.ListarTiposRiesgos())
                                {

                                    matriz_centro matriz = new matriz_centro();
                                    matriz.id_centro = centro;
                                    matriz.id_areanivel1 = null;
                                    matriz.id_areanivel2 = copiaSistema.id;
                                    matriz.id_areanivel3 = null;
                                    matriz.id_tecnologia = tecno.id;
                                    matriz.id_riesgo = riesgo.id;
                                    matriz.activo = false;
                                    matriz.activo = listaMatricesMaestro.Where(z => z.id_areanivel2 == sistema.id && z.id_riesgo == matriz.id_riesgo).Select(x => x.activo).DefaultIfEmpty(false).First();

                                    matriz.version = versionDestino;
                                    matrizNueva.Add(matriz);

                                }
                                if (area.id_tecnologia == tecno.id)
                                {
                                    Datos.InsertarMatriz(matrizNueva);
                                    matrizNueva.Clear();
                                }
                                foreach (areanivel3 equipo in Datos.ListarEquipos().Where(x => x.id_areanivel2 == sistema.id))
                                {
                                    areanivel3 copiaEquipo = new areanivel3();
                                    copiaEquipo.nombre = equipo.nombre;
                                    copiaEquipo.codigo = equipo.codigo;
                                    copiaEquipo.id_areanivel2 = copiaSistema.id;

                                    int id_equipo_seleccionado = Datos.ActualizarEquipo(copiaEquipo);
                                    copiaEquipo.id = id_equipo_seleccionado;

                                    foreach (VISTA_tipos_riesgos riesgo in Datos.ListarTiposRiesgos())
                                    {

                                        matriz_centro matriz = new matriz_centro();
                                        matriz.id_centro = centro;
                                        matriz.id_areanivel1 = null;
                                        matriz.id_areanivel2 = null;
                                        matriz.id_areanivel3 = copiaEquipo.id;
                                        matriz.id_tecnologia = tecno.id;
                                        matriz.id_riesgo = riesgo.id;
                                        matriz.activo = false;
                                        matriz.activo = listaMatricesMaestro.Where(z => z.id_areanivel3 == equipo.id && z.id_riesgo == matriz.id_riesgo).Select(x => x.activo).DefaultIfEmpty(false).First();
                                        matriz.version = versionDestino;
                                        matrizNueva.Add(matriz);

                                    }
                                    if (area.id_tecnologia == tecno.id)
                                    {
                                        Datos.InsertarMatriz(matrizNueva);
                                        matrizNueva.Clear();
                                    }
                                    foreach (areanivel4 nivelcuatro in Datos.ListarNivelescuatro().Where(x => x.id_areanivel3 == equipo.id))
                                    {
                                        areanivel4 copianivelCuatro = new areanivel4();
                                        copianivelCuatro.nombre = nivelcuatro.nombre;
                                        copianivelCuatro.codigo = nivelcuatro.codigo;
                                        copianivelCuatro.id_areanivel3 = copiaEquipo.id;

                                        int id_nivelcuatro_seleccionado = Datos.ActualizarNivelCuatro(copianivelCuatro);
                                        copianivelCuatro.id = id_nivelcuatro_seleccionado;

                                        foreach (VISTA_tipos_riesgos riesgo in Datos.ListarTiposRiesgos())
                                        {

                                            matriz_centro matriz = new matriz_centro();
                                            matriz.id_centro = centro;
                                            matriz.id_areanivel1 = null;
                                            matriz.id_areanivel2 = null;
                                            matriz.id_areanivel3 = null;
                                            matriz.id_areanivel4 = copianivelCuatro.id;
                                            matriz.id_tecnologia = tecno.id;
                                            matriz.id_riesgo = riesgo.id;
                                            matriz.activo = false;
                                            matriz.activo = listaMatricesMaestro.Where(z => z.id_areanivel4 == nivelcuatro.id && z.id_riesgo == matriz.id_riesgo).Select(x => x.activo).DefaultIfEmpty(false).First();
                                            matriz.version = versionDestino;
                                            matrizNueva.Add(matriz);

                                        }
                                        if (area.id_tecnologia == tecno.id)
                                        {
                                            Datos.InsertarMatriz(matrizNueva);
                                            matrizNueva.Clear();
                                        }
                                    }
                                }
                            }
                        }

                    }

                }
                return correcto;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public ActionResult eliminar_riesgo(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            Datos.EliminarRiesgo(id);
            Session["EditarRiesgosResultado"] = "Riesgo eliminado";
            return RedirectToAction("gestion_riesgos", "Riesgos");
        }


        public JsonResult EliminarRiesgoMedida(string idMedida)
        {

            bool res = false;
            Datos.EliminarRiesgoMedida(int.Parse(idMedida));
            return Json(res);
        }

        public JsonResult GuardarMedidaGeneralRiesgoImagen()
        {
            bool datos = false;

            HttpPostedFileBase imagenRelativa = Request.Files["photo"];
            if (imagenRelativa != null)
            {
                var ruta = System.IO.Path.Combine(Server.MapPath("~/Content/images/medidas"), imagenRelativa.FileName);

                HttpPostedFileBase img2 = Request.Files["photo"];
                string riesgo = Request.Params["idRiesgo"].ToString();
                string apartado = Request.Params["idapartado"].ToString();
                string descripcion = Request.Params["descripcion"].ToString();
                descripcion = descripcion.Replace("•\t", "");


                descripcion = descripcion.Replace("\n", "[SALTO]");
                // string path = @"C: \\..\..\..\Users\jose.pinto\Documents\REPOSITORIO\REPOSITORIO_NOVOTEC\DIMASST\Midas\Content\images\medidas\" + img2.FileName;
                string rutabd = "../Content/images/medidas/" + img2.FileName;
                img2.SaveAs(ruta);
                var apartadoResultado = Datos.ObtenerApartadoPorNombreV2(apartado);
                try
                {
                    centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
                    riesgos_medidas medida = new riesgos_medidas();

                    //medida.id_tecnologia = 9;
                    medida.imagen_grande = 1;
                    medida.imagen = rutabd;
                    medida.id_centro = centroseleccionado.id;
                    medida.descripcion = descripcion;
                    if (apartadoResultado != "0")
                    {
                        medida.id_apartado = int.Parse(apartadoResultado);
                    }
                    else
                    {
                        int nuevoid = Datos.GuardarApartado(apartado);
                        medida.id_apartado = nuevoid;
                    }

                    medida.id_riesgo = int.Parse(riesgo);
                    Datos.ActualizarMedidaGeneralRiesgoJson(medida);
                }
                catch (Exception ex)
                {
                    RedirectToAction("LogOn", "Account");
                }
            }
            return Json(datos);
        }

        public JsonResult GuardarMedidaPreventivaRiesgoImagenGrande()
        {
            bool datos = false;

            HttpPostedFileBase imagenRelativa = Request.Files["photo"];
            if (imagenRelativa != null)
            {

                centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
                var ruta = System.IO.Path.Combine(Server.MapPath("~/Content/images/medidas"), imagenRelativa.FileName);

                HttpPostedFileBase img2 = Request.Files["photo"];
                string riesgo = Request.Params["idRiesgo"].ToString();
                string situacion = Request.Params["situacion"].ToString();
                string descripcion = Request.Params["descripcion"].ToString();
                descripcion = descripcion.Replace("\n", "[SALTO]");
                // string path = @"C: \\..\..\..\Users\jose.pinto\Documents\REPOSITORIO\REPOSITORIO_NOVOTEC\DIMASST\Midas\Content\images\medidas\" + img2.FileName;
                string rutabd = "../Content/images/medidas/" + img2.FileName;
                img2.SaveAs(ruta);

                try
                {

                    string[] stringSeparators = new string[] { "\n" };
                    string[] lines = descripcion.Split(stringSeparators, StringSplitOptions.None);
                    medidas_preventivas medidas = new medidas_preventivas();
                    // medidas.codigo = codigo;
                    medidas.id_situacion = int.Parse(situacion);
                    medidas.descripcion = lines.First();
                    medidas.id_centro = centroseleccionado.id;
                    int id = Datos.ActualizarMedidaPreventiva(medidas);

                    int contador = 1;
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

                    medidaspreventivas_imagenes medidapreventiva = new medidaspreventivas_imagenes();
                    medidapreventiva.id_medida = id;
                    medidapreventiva.rutaImagen = rutabd;
                    medidapreventiva.id_centro = centroseleccionado.id;
                    medidapreventiva.tamano = true;
                    Datos.ActualizarImagenRutaMedidaSituacionRiesgo(medidapreventiva);
                }
                catch (Exception ex)
                {
                    RedirectToAction("LogOn", "Account");
                }
            }
            return Json(datos);
        }


        public JsonResult GuardarMedidaGeneralRiesgo(string descripcion, string tecnologia, string riesgo, string apartado)
        {
            bool datos = false;
            descripcion = descripcion.Replace("•\t", "");
            descripcion = descripcion.Replace("\n", "[SALTO]");

            //var apartadoResultado = Datos.ObtenerApartadoPorNombre(apartado); ////esta linea es para cuando se agregue la mejora de agregar mas apartados, en la tabla medidas_apartados
            var apartadoResultado = Datos.ObtenerApartadoPorNombreV2(apartado);
            try
            {
                centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
                riesgos_medidas medida = new riesgos_medidas();
                if (tecnologia != "")
                {
                    medida.id_tecnologia = int.Parse(tecnologia);
                }

                //medida.codigo = int.Parse(codigo);
                medida.descripcion = descripcion;
                medida.id_centro = centroseleccionado.id;
                medida.imagen_grande = null;
                if (apartadoResultado != "0")
                {
                    medida.id_apartado = int.Parse(apartadoResultado);
                }
                else
                {
                    int nuevoid = Datos.GuardarApartado(apartado);
                    medida.id_apartado = nuevoid;
                }

                medida.id_riesgo = int.Parse(riesgo);
                Datos.ActualizarMedidaGeneralRiesgoJson(medida);
            }
            catch (Exception ex)
            {
                RedirectToAction("LogOn", "Account");
            }
            return Json(datos);
        }
        public JsonResult GuardarMedidaGeneralRiesgoVarias(string descripcion, string tecnologia, string riesgo, string apartado)
        {

            //METODO PROVISONAL PARA CORREGIR MEDIDAS, BORRAR ESTE METODO
            bool datos = false;
            //descripcion = descripcion.Replace("•\t", "");
            //descripcion = descripcion.Replace("\n", "[SALTO]");

            var apartadoResultado = Datos.ObtenerApartadoPorNombre(apartado);
            //var apartadoResultado = Datos.ObtenerApartadoPorNombreV2(apartado);
            try
            {
                centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));

                string[] stringSeparators = new string[] { "\n" };
                string[] lines = descripcion.Split(stringSeparators, StringSplitOptions.None);
                foreach (string s in lines)
                {
                    if (!string.IsNullOrEmpty(s))
                    {

                        riesgos_medidas medidas = new riesgos_medidas();
                        medidas.id_riesgo = int.Parse(riesgo);
                        medidas.descripcion = CleanInput(s);
                        medidas.id_centro = 79;
                        medidas.imagen_grande = 0;
                        if (apartadoResultado != "0")
                        {
                            medidas.id_apartado = int.Parse(apartadoResultado);
                        }
                        else
                        {
                            int nuevoid = Datos.GuardarApartado(apartado);
                            medidas.id_apartado = nuevoid;
                        }
                        int id = Datos.ActualizarMedidaGeneralRiesgoJson(medidas);
                    }
                }



            }
            catch (Exception ex)
            {
                RedirectToAction("LogOn", "Account");
            }
            return Json(datos);
        }

        public JsonResult GuardarMedidaGeneral(string tecnologia, string descripcion)
        {
            bool datos = false;
            try
            {
                centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
                medidas_generales medida = new medidas_generales();

                medida.id_tecnologia = int.Parse(tecnologia);
                //medida.codigo = int.Parse(codigo);
                medida.descripcion = descripcion;
                medida.id_centro = centroseleccionado.id;
                Datos.ActualizarMedidaGeneral(medida);
            }
            catch (Exception ex)
            {
                RedirectToAction("LogOn", "Account");
            }
            return Json(datos);
        }



        public JsonResult EliminarSituacion(string idSituacion)
        {
            bool datos = false;
            try
            {
                Datos.EliminarSituacion(int.Parse(idSituacion));
            }
            catch (Exception ex)
            {
                RedirectToAction("LogOn", "Account");
            }
            return Json(datos);
        }
        public JsonResult EliminarMedidaSituacion(string idMedida)
        {
            bool datos = false;
            try
            {
                int id = Datos.EliminarMedidaSituacion(int.Parse(idMedida));
                if (id != 0)
                {
                    Datos.EliminarSubMedidas(id);

                }

                if (id == 0)
                {
                    datos = false;
                }
                else
                {
                    datos = true;
                }

            }
            catch (Exception ex)
            {
                RedirectToAction("LogOn", "Account");
            }
            return Json(datos);
        }
        public JsonResult EliminarMedidaGeneral(string idMedida)
        {
            bool datos = false;
            try
            {
                Datos.EliminarMEdida(int.Parse(idMedida));
            }
            catch (Exception ex)
            {
                RedirectToAction("LogOn", "Account");
            }
            return Json(datos);
        }
        public JsonResult ObtenerMedidasGenerales(string idTipo)
        {


            int tipo = 0;

            List<medidas_generales> datos = null;
            try
            {
                List<medidas_generales> listado = null;
                tipo = int.Parse(idTipo);
                centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
                //listado = Datos.ListarMedidasGenerales(centroseleccionado.id, tipo);
                listado = Datos.ListarMedidasGenerales();
                datos = new List<medidas_generales>();


                foreach (medidas_generales item in listado)
                {
                    medidas_generales it = new medidas_generales();
                    it.id = item.id;
                    it.descripcion = item.descripcion;
                    it.codigo = item.codigo;
                    datos.Add(it);
                }

            }
            catch (Exception ex)
            {
                RedirectToAction("LogOn", "Account");
            }
            return Json(datos);
        }

        public FileResult ObtenerCriterios()
        {
            try
            {
                ficheros IF = Datos.ObtenerCriteriosEvaluacion("EG-SIG-PGE-013");
                byte[] fileBytes = System.IO.File.ReadAllBytes(IF.enlace);
                string fileName = IF.enlace.Replace(Server.MapPath("~/Documentacion") + "\\" + IF.nombre_fichero, "");
                fileName = fileName.Replace(IF.nombre_fichero, "");
                fileName = fileName.Replace("á", "a");
                fileName = fileName.Replace("é", "e");
                fileName = fileName.Replace("í", "i");
                fileName = fileName.Replace("ó", "o");
                fileName = fileName.Replace("ú", "u");
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
