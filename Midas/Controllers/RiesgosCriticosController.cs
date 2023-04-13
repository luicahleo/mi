using MIDAS.Models;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using static MIDAS.Models.Enum;

namespace MIDAS.Controllers
{
    public class RiesgosCriticosController : BaseController
    {
        // GET: RiesgosCriticos
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult matriz_riesgos_criticos(int? id)
        {
            if (Session["usuario"] == null)
            {

                return RedirectToAction("LogOn", "Account");
            }

            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            VISTA_ObtenerUsuario user = Datos.ObtenerUsuarioActual(Session["usuario"].ToString());
            //obtenemos la versionFinalizada de la matriz 
            var versionFinalizada = Datos.obtenerUltimaVersionMatrizFinalizadaCentro(centroseleccionado.id);

            if (versionFinalizada > 0)
            {

                var yaExisteMatrizCritica = Datos.ExisteMatrizCritico(versionFinalizada);
                if (!yaExisteMatrizCritica)
                {
                    //primero tenemos que crear nuestra matriz nivel por nivel con sus respectivos RC
                    List<string> recibe = Datos.ListarTecnologiasDeMatrizCentroPorVersion(versionFinalizada);

                    List<tecnologias> tecnologiasCentro = Datos.ListarTecnologiasPorCentro(centroseleccionado.id);
                    List<areanivel1> areasVersion = Datos.ListarAreasMaestro(versionFinalizada, 0);
                    areasVersion = areasVersion.Where(x => x.id_centro == centroseleccionado.id).OrderBy(x => x.id).ToList();
                    List<areanivel2> sistemas = Datos.ListarSistema();
                    List<areanivel3> equipos = Datos.ListarEquipos();
                    List<areanivel4> nivelescuatro = Datos.ListarNivelescuatro();
                    List<areanivel1> areasPrede = new List<areanivel1>();

                    //COMPROBAR QUE TENEMOS LOS DATOS DE TECNOLOGIAS
                    List<string> tecnologiasSelec = new List<string>();
                    if (recibe != null)
                    {
                        foreach (var item in recibe)
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
                            //List<areanivel1> ar = Datos.listarAreasInicial(tecnologia);
                            List<areanivel1> ar = Datos.listarAreasInicialAPartirDeMatrizCentro(versionFinalizada);
                            if (ar != null)
                            {
                                areasPrede.AddRange(ar);
                            }
                        }
                    }

                    //PARA MAS ADELANTE, HACER LA BUSQUEDA POR ENTEROS
                    List<matriz_inicial> listaMatricesMaestro = Datos.listarMatrizInicial(tecnologiasSelec);

                    //int idVersionNueva = Datos.ActualizarVersionMatriz(centroseleccionado.id, Session["usuario"].ToString()); 
                    int idVersionNueva = versionFinalizada;

                    List<matriz_centro_critico> matrizNueva = new List<matriz_centro_critico>();

                    foreach (tecnologias tecno in tecnologias)
                    {
                        foreach (areanivel1 area in areasPrede)
                        {
                            if (area.id_tecnologia == tecno.id)
                            {

                                //seleccionamos solo los RC que correspondan a esa tecnologia
                                var listaTiposRiesgosCriticos = Datos.ObtenerListaTecnologias_TiposRiesgosCriticos(tecno.id);

                                foreach (tecnologias_tiposRiesgosCriticos riesgo in listaTiposRiesgosCriticos)
                                {

                                    matriz_centro_critico matriz = new matriz_centro_critico();
                                    matriz.id_centro = centroseleccionado.id;
                                    matriz.id_areanivel1 = area.id;
                                    matriz.id_areanivel2 = null;
                                    matriz.id_areanivel3 = null;
                                    matriz.id_tecnologia = tecno.id;

                                    matriz.id_riesgoCritico = riesgo.id_tipoRiesgoCritico;
                                    //matriz.activo = listaMatricesMaestro.Where(z => z.id_areanivel1 == area.id && z.id_riesgoCritico == matriz.id_riesgoCritico).Select(x => x.activo).DefaultIfEmpty(false).First();
                                    matriz.activo = true;

                                    matriz.version = idVersionNueva;
                                    matrizNueva.Add(matriz);

                                }
                                if (area.id_tecnologia == tecno.id)
                                {
                                    Datos.InsertarMatrizCritico(matrizNueva);
                                    matrizNueva.Clear();
                                }

                                foreach (areanivel2 sistema in area.areanivel2)
                                {

                                    foreach (tecnologias_tiposRiesgosCriticos riesgo in listaTiposRiesgosCriticos)
                                    {

                                        matriz_centro_critico matriz = new matriz_centro_critico();
                                        matriz.id_centro = centroseleccionado.id;
                                        matriz.id_areanivel1 = null;
                                        matriz.id_areanivel2 = sistema.id;
                                        matriz.id_areanivel3 = null;
                                        matriz.id_tecnologia = tecno.id;
                                        matriz.id_riesgoCritico = riesgo.id_tipoRiesgoCritico;
                                        matriz.activo = true;
                                        //matriz.activo = listaMatricesMaestro.Where(z => z.id_areanivel2 == sistema.id && z.id_riesgoCritico == matriz.id_riesgoCritico).Select(x => x.activo).DefaultIfEmpty(false).First();
                                        matriz.version = idVersionNueva;
                                        matrizNueva.Add(matriz);

                                    }
                                    if (area.id_tecnologia == tecno.id)
                                    {
                                        Datos.InsertarMatrizCritico(matrizNueva);
                                        matrizNueva.Clear();
                                    }
                                    foreach (areanivel3 equipo in sistema.areanivel3)
                                    {
                                        foreach (tecnologias_tiposRiesgosCriticos riesgo in listaTiposRiesgosCriticos)
                                        {

                                            matriz_centro_critico matriz = new matriz_centro_critico();
                                            matriz.id_centro = centroseleccionado.id;
                                            matriz.id_areanivel1 = null;
                                            matriz.id_areanivel2 = null;
                                            matriz.id_areanivel3 = equipo.id;
                                            matriz.id_tecnologia = tecno.id;
                                            matriz.id_riesgoCritico = riesgo.id_tipoRiesgoCritico;
                                            matriz.activo = true;
                                            //matriz.activo = listaMatricesMaestro.Where(z => z.id_areanivel3 == equipo.id && z.id_riesgoCritico == matriz.id_riesgoCritico).Select(x => x.activo).DefaultIfEmpty(false).First();
                                            matriz.version = idVersionNueva;
                                            matrizNueva.Add(matriz);

                                        }
                                        if (area.id_tecnologia == tecno.id)
                                        {
                                            Datos.InsertarMatrizCritico(matrizNueva);
                                            matrizNueva.Clear();
                                        }

                                        foreach (areanivel4 nivelcuatro in Datos.ListarNivelescuatro().Where(x => x.id_areanivel3 == equipo.id))
                                        {

                                            foreach (tecnologias_tiposRiesgosCriticos riesgo in listaTiposRiesgosCriticos)
                                            {

                                                matriz_centro_critico matriz = new matriz_centro_critico();
                                                matriz.id_centro = centroseleccionado.id;
                                                matriz.id_areanivel1 = null;
                                                matriz.id_areanivel2 = null;
                                                matriz.id_areanivel3 = null;
                                                matriz.id_areanivel4 = nivelcuatro.id;
                                                matriz.id_tecnologia = tecno.id;
                                                matriz.id_riesgoCritico = riesgo.id_tipoRiesgoCritico;
                                                matriz.activo = true;
                                                //matriz.activo = listaMatricesMaestro.Where(z => z.id_areanivel4 == nivelcuatro.id && z.id_riesgoCritico == matriz.id_riesgoCritico).Select(x => x.activo).DefaultIfEmpty(false).First();
                                                matriz.version = idVersionNueva;
                                                matrizNueva.Add(matriz);

                                            }
                                            if (area.id_tecnologia == tecno.id)
                                            {
                                                Datos.InsertarMatrizCritico(matrizNueva);
                                                matrizNueva.Clear();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    return RedirectToAction("matriz_riesgos_criticos/" + idVersionNueva);
                }
                else
                {

                    //ViewData["matriz_inicial"] = Datos.listarMatrizInicial();
                    ViewData["matriz_centro_critico"] = Datos.listarMatrizCentro(centroseleccionado.id, versionFinalizada);

                    List<tecnologias> tecnologiasCentro = Datos.ListarTecnologiasPorVersion(centroseleccionado.id, versionFinalizada);
                    if (tecnologiasCentro.Count == 0)
                    {
                        tecnologiasCentro = Datos.ListarTecnologiasPorCentro(centroseleccionado.id);
                    }
                    ViewData["tecnologias"] = tecnologiasCentro.OrderBy(x => x.id);


                    List<areanivel1> areasVersion = Datos.ListarAreas(versionFinalizada);
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
                    ViewData["version"] = versionFinalizada;
                    ViewData["tipos_riesgos_criticos"] = Datos.ListarVistaTiposRiesgosCriticos();

                }
            }
            else
            {
                Alert("Debe generar una version de matriz Finalizada", NotificationType.info, 3000);
                return RedirectToAction("GenerarDocumentoRiesgos", "DocumentoRiesgos");
            }

            var listaVersionMatriz = MIDAS.Models.Datos.listarMatrizVersion(int.Parse(Session["CentralElegida"].ToString()));

            if (listaVersionMatriz.Count > 0)
            {
                Session["ExisteMatriz"] = "existe";
            }
            return View();
        }

        public JsonResult ObtenerActivos(string version)
        {
            List<matriz_centro_critico> datos = null;
            try
            {
                centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
                //  Datos.RecalcularMatrizVersion(centroseleccionado.id, int.Parse(version));
                datos = Datos.listarMatrizCentroCritico(centroseleccionado.id, int.Parse(version));


                //Datos.RecalcularMatrizVersion(centroseleccionado.id, int.Parse(version));
                datos = datos.Where(x => x.activo == true).ToList();

            }
            catch (Exception ex)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }

            return Json(datos, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ObtenerActivosFila(string version, string nivel, string idArea)
        {
            List<matriz_centro_critico> datos = null;
            try
            {

                centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
                datos = Datos.listarMatrizCentroCritico(centroseleccionado.id, int.Parse(version));
                //Datos.RecalcularMatrizVersionFila(centroseleccionado.id, int.Parse(version), int.Parse(idArea));
                //Datos.RecalcularMatrizVersionFilaAsync(centroseleccionado.id, int.Parse(version), int.Parse(idArea));
                //datos =datos.Where(x => x.activo == true).ToList();              

            }
            catch (Exception ex)
            {

            }

            return Json(datos, JsonRequestBehavior.AllowGet);
        }


        public JsonResult ObtenerTecnologias(string idVersion)
        {

            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            List<SelectListItem> datos = null;
            try
            {
                List<tecnologias> listado = Datos.ListarTecnologias();
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

        //public bool copiarMatriz(List<tecnologias> tec, List<areanivel1> areas, int centro, int versionDestino, List<matriz_centro> listaMatricesMaestro)
        //{
        //    bool correcto = true;
        //    try
        //    {
        //        List<matriz_centro_critico> matrizNueva = new List<matriz_centro_critico>();
        //        foreach (tecnologias tecno in tec)
        //        {
        //            foreach (areanivel1 area in areas)
        //            {
        //                if (area.id_tecnologia == tecno.id)
        //                {
        //                    areanivel1 copiaArea = new areanivel1();
        //                    copiaArea.nombre = area.nombre;
        //                    copiaArea.codigo = area.codigo;
        //                    copiaArea.id_centro = centro;
        //                    copiaArea.id_tecnologia = tecno.id;
        //                    int id_area_seleccionada = Datos.ActualizarArea(copiaArea);
        //                    copiaArea.id = id_area_seleccionada;

        //                    foreach (VISTA_tipos_riesgos riesgo in Datos.ListarTiposRiesgos())
        //                    {

        //                        matriz_centro_critico matriz = new matriz_centro_critico();
        //                        matriz.id_centro = centro;
        //                        matriz.id_areanivel1 = copiaArea.id;
        //                        matriz.id_areanivel2 = null;
        //                        matriz.id_areanivel3 = null;
        //                        matriz.id_tecnologia = tecno.id;
        //                        matriz.id_riesgoCritico = riesgo.id;
        //                        matriz.activo = listaMatricesMaestro.Where(z => z.id_areanivel1 == area.id && z.id_riesgoCritico == matriz.id_riesgoCritico).Select(x => x.activo).DefaultIfEmpty(false).First();

        //                        matriz.version = versionDestino;
        //                        matrizNueva.Add(matriz);

        //                    }
        //                    if (area.id_tecnologia == tecno.id)
        //                    {
        //                        Datos.InsertarMatriz(matrizNueva);
        //                        matrizNueva.Clear();
        //                    }

        //                    foreach (areanivel2 sistema in Datos.ListarSistema().Where(x => x.id_areanivel1 == area.id))
        //                    {
        //                        areanivel2 copiaSistema = new areanivel2();
        //                        copiaSistema.nombre = sistema.nombre;
        //                        copiaSistema.codigo = sistema.codigo;
        //                        copiaSistema.id_areanivel1 = copiaArea.id;
        //                        int id_sistema_seleccionado = Datos.ActualizarSistema(copiaSistema);
        //                        copiaSistema.id = id_sistema_seleccionado;

        //                        foreach (VISTA_tipos_riesgos riesgo in Datos.ListarTiposRiesgos())
        //                        {

        //                            matriz_centro_critico matriz = new matriz_centro_critico();
        //                            matriz.id_centro = centro;
        //                            matriz.id_areanivel1 = null;
        //                            matriz.id_areanivel2 = copiaSistema.id;
        //                            matriz.id_areanivel3 = null;
        //                            matriz.id_tecnologia = tecno.id;
        //                            matriz.id_riesgoCritico = riesgo.id;
        //                            matriz.activo = false;
        //                            matriz.activo = listaMatricesMaestro.Where(z => z.id_areanivel2 == sistema.id && z.id_riesgoCritico == matriz.id_riesgoCritico).Select(x => x.activo).DefaultIfEmpty(false).First();

        //                            matriz.version = versionDestino;
        //                            matrizNueva.Add(matriz);

        //                        }
        //                        if (area.id_tecnologia == tecno.id)
        //                        {
        //                            Datos.InsertarMatriz(matrizNueva);
        //                            matrizNueva.Clear();
        //                        }
        //                        foreach (areanivel3 equipo in Datos.ListarEquipos().Where(x => x.id_areanivel2 == sistema.id))
        //                        {
        //                            areanivel3 copiaEquipo = new areanivel3();
        //                            copiaEquipo.nombre = equipo.nombre;
        //                            copiaEquipo.codigo = equipo.codigo;
        //                            copiaEquipo.id_areanivel2 = copiaSistema.id;

        //                            int id_equipo_seleccionado = Datos.ActualizarEquipo(copiaEquipo);
        //                            copiaEquipo.id = id_equipo_seleccionado;

        //                            foreach (VISTA_tipos_riesgos riesgo in Datos.ListarTiposRiesgos())
        //                            {

        //                                matriz_centro_critico matriz = new matriz_centro_critico();
        //                                matriz.id_centro = centro;
        //                                matriz.id_areanivel1 = null;
        //                                matriz.id_areanivel2 = null;
        //                                matriz.id_areanivel3 = copiaEquipo.id;
        //                                matriz.id_tecnologia = tecno.id;
        //                                matriz.id_riesgoCritico = riesgo.id;
        //                                matriz.activo = false;
        //                                matriz.activo = listaMatricesMaestro.Where(z => z.id_areanivel3 == equipo.id && z.id_riesgoCritico == matriz.id_riesgoCritico).Select(x => x.activo).DefaultIfEmpty(false).First();
        //                                matriz.version = versionDestino;
        //                                matrizNueva.Add(matriz);

        //                            }
        //                            if (area.id_tecnologia == tecno.id)
        //                            {
        //                                Datos.InsertarMatriz(matrizNueva);
        //                                matrizNueva.Clear();
        //                            }
        //                            foreach (areanivel4 nivelcuatro in Datos.ListarNivelescuatro().Where(x => x.id_areanivel3 == equipo.id))
        //                            {
        //                                areanivel4 copianivelCuatro = new areanivel4();
        //                                copianivelCuatro.nombre = nivelcuatro.nombre;
        //                                copianivelCuatro.codigo = nivelcuatro.codigo;
        //                                copianivelCuatro.id_areanivel3 = copiaEquipo.id;

        //                                int id_nivelcuatro_seleccionado = Datos.ActualizarNivelCuatro(copianivelCuatro);
        //                                copianivelCuatro.id = id_nivelcuatro_seleccionado;

        //                                foreach (VISTA_tipos_riesgos riesgo in Datos.ListarTiposRiesgos())
        //                                {

        //                                    matriz_centro_critico matriz = new matriz_centro_critico();
        //                                    matriz.id_centro = centro;
        //                                    matriz.id_areanivel1 = null;
        //                                    matriz.id_areanivel2 = null;
        //                                    matriz.id_areanivel3 = null;
        //                                    matriz.id_areanivel4 = copianivelCuatro.id;
        //                                    matriz.id_tecnologia = tecno.id;
        //                                    matriz.id_riesgoCritico = riesgo.id;
        //                                    matriz.activo = false;
        //                                    matriz.activo = listaMatricesMaestro.Where(z => z.id_areanivel4 == nivelcuatro.id && z.id_riesgoCritico == matriz.id_riesgoCritico).Select(x => x.activo).DefaultIfEmpty(false).First();
        //                                    matriz.version = versionDestino;
        //                                    matrizNueva.Add(matriz);

        //                                }
        //                                if (area.id_tecnologia == tecno.id)
        //                                {
        //                                    Datos.InsertarMatriz(matrizNueva);
        //                                    matrizNueva.Clear();
        //                                }
        //                            }
        //                        }
        //                    }
        //                }

        //            }

        //        }
        //        return correcto;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}

        [HttpPost]
        public JsonResult ObtenerActivosCriticos(string version)
        {
            List<matriz_centro_critico> datos = null;
            try
            {
                centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
                //  Datos.RecalcularMatrizVersion(centroseleccionado.id, int.Parse(version));
                datos = Datos.listarMatrizCriticoCentro(centroseleccionado.id, int.Parse(version));


                //Datos.RecalcularMatrizVersion(centroseleccionado.id, int.Parse(version));
                datos = datos.Where(x => x.activo == true).ToList();

            }
            catch (Exception ex)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }

            return Json(datos, JsonRequestBehavior.AllowGet);
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
                List<matriz_centro_critico> conjunto = new List<matriz_centro_critico>();

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
                        Datos.eliminarMatrizRiesgoAreaCritico(id_seleccionado, tecnologiaEnt);

                        foreach (var item in matrizRiesgos)
                        {
                            matriz_centro_critico matriz = new matriz_centro_critico();

                            matriz.id_centro = centroseleccionado.id;
                            matriz.id_areanivel1 = id_seleccionado;
                            matriz.id_areanivel2 = null;
                            matriz.id_areanivel3 = null;
                            matriz.id_tecnologia = int.Parse(tecnologia);
                            matriz.id_riesgoCritico = int.Parse(item[0]);
                            matriz.activo = item[1] == "true" ? true : false;
                            matriz.version = int.Parse(version);
                            conjunto.Add(matriz);
                        }
                        Datos.InsertarMatrizCriticoConjunto(conjunto);
                        Datos.ActualizarFechaUsuarioMatriz(int.Parse(version), Session["usuario"].ToString());
                        break;

                    case "SISTEMA":
                        areanivel2 sistema = new areanivel2();

                        int.TryParse(id, out sistemaEntero);
                        sistema.id = sistemaEntero;
                        sistema.nombre = nombre;
                        datos = nombre;
                        id_seleccionado = Datos.ActualizarSistema(sistema);

                        Datos.eliminarMatrizRiesgoSistemaCriticoAsync(id_seleccionado, tecnologiaEnt);

                        foreach (var item in matrizRiesgos)
                        {
                            matriz_centro_critico matriz = new matriz_centro_critico();

                            matriz.id_centro = centroseleccionado.id;
                            matriz.id_areanivel1 = null;
                            matriz.id_areanivel2 = id_seleccionado;
                            matriz.id_areanivel3 = null;
                            matriz.id_tecnologia = int.Parse(tecnologia);
                            matriz.id_riesgoCritico = int.Parse(item[0]);
                            var areaPadre = Datos.ObtenerAreaPadre(id_seleccionado);
                            matriz.version = Datos.DameVersionAreaCritico(areaPadre);
                            matriz.activo = item[1] == "true" ? true : false;

                            conjunto.Add(matriz);
                        }
                        Datos.ActualizarFilaMatrizNivel2Critico(conjunto, id_seleccionado);
                        Datos.InsertarMatrizCriticoConjunto(conjunto);
                        var padre1 = Datos.ObtenerAreaPadre(id_seleccionado);
                        int? versionsistema = Datos.DameVersionAreaCritico(padre1);
                        if (versionsistema != null)
                        {
                            Datos.ActualizarFechaUsuarioMatriz((int)versionsistema, Session["usuario"].ToString());
                            Datos.RecalcularMatrizVersionFilaCritico(centroseleccionado.id, (int)versionsistema, padre1);
                            //Datos.RecalcularMatrizVersionFilaAsync(centroseleccionado.id, (int)versionsistema, padre);

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
                        //List<matriz_centro_critico> conjunto=Datos.eliminarMatrizRiesgoEquipo(id_seleccionado);
                        Datos.eliminarMatrizRiesgoEquipoCriticoAsync(id_seleccionado, tecnologiaEnt);
                        // conjunto = Datos.actualizarMatrizEquipo(ht,id_seleccionado);
                        //if (conjunto.Count == 0)
                        //{
                        foreach (var item in matrizRiesgos)
                        {
                            matriz_centro_critico matriz = new matriz_centro_critico();

                            matriz.id_centro = centroseleccionado.id;
                            matriz.id_areanivel1 = null;
                            matriz.id_areanivel2 = null;
                            matriz.id_areanivel3 = id_seleccionado;
                            matriz.id_tecnologia = int.Parse(tecnologia);
                            matriz.id_riesgoCritico = int.Parse(item[0]);
                            var sistemaPadre = Datos.ObtenerSistemaPadre(id_seleccionado);
                            matriz.version = Datos.DameVersionSistemaCritico(sistemaPadre);
                            matriz.activo = item[1] == "true" ? true : false;
                            conjunto.Add(matriz);

                        }
                        Datos.ActualizarFilaMatrizNivel3Critico(conjunto, id_seleccionado);
                        Datos.InsertarMatrizCriticoConjunto(conjunto);
                        var padre2 = Datos.ObtenerSistemaPadre(id_seleccionado);

                        int? versionequipo = Datos.DameVersionSistemaCritico(padre2);
                        if (versionequipo != null)
                        {
                            Datos.ActualizarFechaUsuarioMatriz((int)versionequipo, Session["usuario"].ToString());
                            Datos.RecalcularMatrizVersionFilaCritico(centroseleccionado.id, (int)versionequipo, Datos.ObtenerAreaPadre(padre2));
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
                        Datos.eliminarMatrizRiesgoNivel4CriticoAsync(id_seleccionado, tecnologiaEnt);

                        foreach (var item in matrizRiesgos)
                        {
                            matriz_centro_critico matriz = new matriz_centro_critico();

                            matriz.id_centro = centroseleccionado.id;
                            matriz.id_areanivel1 = null;
                            matriz.id_areanivel2 = null;
                            matriz.id_areanivel3 = null;
                            matriz.id_areanivel4 = id_seleccionado;
                            matriz.id_tecnologia = int.Parse(tecnologia);
                            matriz.id_riesgoCritico = int.Parse(item[0]);
                            var equipoPadre = Datos.ObtenerSistemaPadre(Datos.ObtenerEquipoPadre(id_seleccionado));
                            matriz.version = Datos.DameVersionSistemaCritico(equipoPadre) ;
                            matriz.activo = item[1] == "true" ? true : false;
                            conjunto.Add(matriz);

                        }
                        Datos.ActualizarFilaMatrizNivel4Critico(conjunto, id_seleccionado);
                        Datos.InsertarMatrizCriticoConjunto(conjunto);
                        var padre3 = Datos.ObtenerEquipoPadre(id_seleccionado);

                        int? versionnivel4 = Datos.DameVersionSistemaCritico(Datos.ObtenerSistemaPadre(padre3));
                        if (versionnivel4 != null)
                        {
                            Datos.ActualizarFechaUsuarioMatriz((int)versionnivel4, Session["usuario"].ToString());
                            Datos.RecalcularMatrizVersionFilaCritico(centroseleccionado.id, (int)versionnivel4, Datos.ObtenerAreaPadre(Datos.ObtenerSistemaPadre(padre3)));
                            //Datos.RecalcularMatrizVersionFilaAsync(centroseleccionado.id, (int)versionnivel4, Datos.ObtenerAreaPadre(Datos.ObtenerSistemaPadre(padre3)));
                        }
                        break;
                    default:
                        break;

                }

                //foreach (matriz_centro_critico item in conjunto.Where(x => x.id_areanivel3 != null))
                //{

                //    if (item.activo)
                //    {

                //        Datos.actualizarPadres(item, 3);
                //    }
                //}


                //foreach (matriz_centro_critico item in conjunto.Where(x => x.id_areanivel2 != null))
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

    }
}

