using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.VariantTypes;

using OfficeOpenXml.Utils.TypeConversion;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MIDAS.Models
{
    public class Datos
    {
        public static bool SignIn(string user, string pass)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            int contador = (from u in bd.VISTA_ObtenerUsuario
                            where u.nombre == user && u.password == pass
                            select u).Count();

            if (contador > 0)
            {



                log insertarlog = new log();
                insertarlog.usuario = user;
                insertarlog.fecha = DateTime.Now;
                insertarlog.evento = "Acceso a la aplicación";

                bd.log.Add(insertarlog);
                bd.SaveChanges();


                return true;

            }
            else
            {
                log insertarlog = new log();
                insertarlog.usuario = user;
                insertarlog.fecha = DateTime.Now;
                insertarlog.evento = "Login erróneo";

                bd.log.Add(insertarlog);
                bd.SaveChanges();

                return false;
            }
        }

        public static VISTA_ObtenerUsuario ObtenerUsuarioActual(string user)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            VISTA_ObtenerUsuario nuevo = new VISTA_ObtenerUsuario();

            var registro = (from u in bd.VISTA_ObtenerUsuario
                            where u.nombre == user
                            select u).FirstOrDefault();

            if (registro != null)
            {
                nuevo.idUsuario = registro.idUsuario;
                nuevo.nombre = registro.nombre;
                nuevo.password = registro.password;
                nuevo.mail = registro.mail;
                nuevo.telefono = registro.telefono;
                nuevo.puesto = registro.puesto;
                nuevo.perfil = registro.perfil;
                nuevo.baja = registro.baja;
                nuevo.idUnidad = registro.idUnidad;
            }
            else
                nuevo = null;


            return nuevo;
        }

        public static bool ComprobarCodigoDocumento(string coddoc, int idDoc)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            documentacion nuevo = new documentacion();

            var registro = (from u in bd.documentacion
                            where u.cod_fichero == coddoc && u.idFichero != idDoc
                            select u).FirstOrDefault();

            if (registro != null)
            {
                return true;
            }
            else
                return false;
        }

        public static centros ObtenerCentroPorNombre(string centro)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            centros nuevo = new centros();

            var registro = (from u in bd.centros
                            where u.nombre == centro
                            select u).FirstOrDefault();

            if (registro != null)
            {
                nuevo.id = registro.id;
                nuevo.nombre = registro.nombre;
                nuevo.siglas = registro.siglas;
                nuevo.tipo = registro.tipo;
                nuevo.ubicacion = registro.ubicacion;
            }
            else
                nuevo = null;


            return nuevo;
        }

        public static pais ObtenerPais(int? id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            pais nuevo = new pais();

            var registro = (from u in bd.pais
                            where u.id_pais == id
                            select u).First();

            nuevo = registro;


            return nuevo;
        }
        public static int? DameVersionSistema(int idSistema)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            int? nuevo = null;


            int? registro = (from u in bd.matriz_centro
                             where u.id_areanivel2 == idSistema
                             select u.version).Max();

            nuevo = registro;


            return nuevo;
        }
        public static int? DameVersionSistemaCritico(int idSistema)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            int? nuevo = null;

            int? registro = (from u in bd.matriz_centro_critico
                             where u.id_areanivel2 == idSistema
                             select u.version).Max();

            nuevo = registro;

            return nuevo;
        }
        public static int? DameVersionArea(int idArea)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            int? nuevo = null;


            int? registro = (from u in bd.matriz_centro
                             where u.id_areanivel1 == idArea
                             select u.version).Max();

            nuevo = registro;

            return nuevo;
        }
        public static int? DameVersionAreaCritico(int idArea)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            int? nuevo = null;


            int? registro = (from u in bd.matriz_centro_critico
                             where u.id_areanivel1 == idArea
                             select u.version).Max();

            nuevo = registro;

            return nuevo;
        }
        public static int ObtenerAreaPadre(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();


            int nuevo = 0;
            int registro = (from u in bd.areanivel2
                            where u.id == id
                            select u.id_areanivel1).First();
            if (registro != null)
            {
                nuevo = registro;
            }


            return nuevo;
        }


        public static string ObtenerImagenNivel(int identificadorNivel)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var registro = (from u in bd.areas_imagenes
                            where u.id_areanivel1 == identificadorNivel
                            select u.rutaImagen).First();
            if (registro == null)
            {
                registro = "";
            }
            return registro.ToString();
        }
        public static string ObtenerImagenNivel2(int identificadorNivel)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var registro = (from u in bd.areas2_imagenes
                            where u.id_areanivel2 == identificadorNivel
                            select u.rutaImagen).First();
            if (registro == null)
            {
                registro = "";
            }
            return registro.ToString();
        }
        public static string ObtenerImagenNivel3(int identificadorNivel)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var registro = (from u in bd.equipos_imagenes
                            where u.id_areanivel3 == identificadorNivel
                            select u.rutaImagen).First();
            if (registro == null)
            {
                registro = "";
            }
            return registro.ToString();
        }
        public static string ObtenerImagenNivel4(int identificadorNivel)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var registro = (from u in bd.areas4_imagenes
                            where u.id_areanivel4 == identificadorNivel
                            select u.rutaImagen).First();
            if (registro == null)
            {
                registro = "";
            }
            return registro.ToString();
        }

        public static int ObtenerSistemaPadre(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            int nuevo = 0;
            var registro = (from u in bd.areanivel3
                            where u.id == id
                            select u.id_areanivel2).First();
            if (registro != null)
            {
                nuevo = registro;
            }
            return nuevo;
        }

        public static int ObtenerEquipoPadre(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            int nuevo = 0;
            var registro = (from u in bd.areanivel4
                            where u.id == id
                            select u.id_areanivel3).First();
            if (registro != null)
            {
                nuevo = registro;
            }
            return nuevo;
        }
        public static List<comunidad_autonoma> ObtenerCCAA(int? id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            List<comunidad_autonoma> nuevo = new List<comunidad_autonoma>();

            var registro = (from u in bd.comunidad_autonoma
                            where u.id_comunidad_autonoma == id
                            select u).ToList();

            nuevo = registro;


            return nuevo;
        }

        public static List<VISTA_ListarUsuarios> ListarUsuarios()
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var registros = from v in bd.VISTA_ListarUsuarios orderby v.idUsuario descending select v;

            List<VISTA_ListarUsuarios> listaArchivosCargados = registros.ToList();

            return listaArchivosCargados;

        }

        public static List<VISTA_ListarTipoDocumento> ListarTiposDoc()
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var registros = from v in bd.VISTA_ListarTipoDocumento orderby v.id descending select v;

            List<VISTA_ListarTipoDocumento> listaArchivosCargados = registros.ToList();

            return listaArchivosCargados;

        }

        public static List<referenciales> ListarReferenciales()
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var registros = from v in bd.referenciales orderby v.id descending select v;

            List<referenciales> listaReferenciales = registros.ToList();

            return listaReferenciales;

        }

        public static List<auditores> ListarAuditores()
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var registros = from v in bd.auditores orderby v.id descending select v;

            List<auditores> listaAuditores = registros.ToList();

            return listaAuditores;

        }

        public static List<riesgos_categorias> ListarCategoriasRiesgo()
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var registros = from v in bd.riesgos_categorias orderby v.id descending select v;

            List<riesgos_categorias> listaAuditores = registros.ToList();

            return listaAuditores;

        }

        public static List<riesgos_tipologias> ListarTipologiasRiesgo()
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var registros = from v in bd.riesgos_tipologias orderby v.id descending select v;

            List<riesgos_tipologias> listaAuditores = registros.ToList();

            return listaAuditores;

        }

        public static List<ambitos> ListarAmbitos()
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var registros = from v in bd.ambitos orderby v.id ascending select v;

            List<ambitos> listaAmbitos = registros.ToList();


            return listaAmbitos;

        }


        public static List<ambitos> ListarAmbitosOrderByNombreAmbito()
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var registros = from v in bd.ambitos orderby v.nombre_ambito ascending select v;

            List<ambitos> listaAmbitos = registros.ToList();


            return listaAmbitos;

        }

        public static List<informesseguridad_elaboradopor> ListarInformesElaborado()
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var registros = from v in bd.informesseguridad_elaboradopor orderby v.id ascending select v;

            List<informesseguridad_elaboradopor> listaAmbitos = registros.ToList();

            return listaAmbitos;

        }

        public static List<informesseguridad_tipodoc> ListarInformesTipodocs()
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var registros = from v in bd.informesseguridad_tipodoc orderby v.id ascending select v;

            List<informesseguridad_tipodoc> listaAmbitos = registros.ToList();

            return listaAmbitos;

        }

        public static List<materialdivulgativo_riesgos> ListarMaterialRiesgos()
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var registros = from v in bd.materialdivulgativo_riesgos orderby v.id ascending select v;

            List<materialdivulgativo_riesgos> listaAmbitos = registros.ToList();

            return listaAmbitos;

        }

        public static List<materialdivulgativo_tipodoc> ListarMaterialTipodocs()
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var registros = from v in bd.materialdivulgativo_tipodoc orderby v.id ascending select v;

            List<materialdivulgativo_tipodoc> listaAmbitos = registros.ToList();

            return listaAmbitos;

        }

        public static List<evaluacionriesgos_tipodoc> ListarEvaluacionTipodocs()
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var registros = from v in bd.evaluacionriesgos_tipodoc orderby v.id ascending select v;

            List<evaluacionriesgos_tipodoc> listaAmbitos = registros.ToList();

            return listaAmbitos;

        }

        public static List<evaluacionriesgos_elaboradopor> ListarEvaluacionElaborado()
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var registros = from v in bd.evaluacionriesgos_elaboradopor orderby v.id ascending select v;

            List<evaluacionriesgos_elaboradopor> listaAmbitos = registros.ToList();

            return listaAmbitos;

        }

        public static List<usuarios> ListarUsuariosAsignar(int idAuditoria)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var registros = from v in bd.usuarios
                            where !(from ar in bd.auditorias_observador
                                    where ar.idAuditoria == idAuditoria
                                    select ar.idUsuario).Contains(v.idUsuario)
                            orderby v.nombre ascending
                            select v;

            List<usuarios> listaUsuarios = registros.ToList();

            return listaUsuarios;

        }

        public static List<auditores> ListarAuditoresAsignar(int idAuditoria)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var registros = from v in bd.auditores
                            where !(from ar in bd.auditorias_auditores
                                    where ar.idAuditoria == idAuditoria
                                    select ar.idAuditor).Contains(v.id)
                            orderby v.nombre ascending
                            select v;

            List<auditores> listaUsuarios = registros.ToList();

            return listaUsuarios;

        }


        public static int ActualizarDescripcion(descripcion_centro objeto)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.descripcion_centro
                              where u.id_centro == objeto.id_centro
                              select u).FirstOrDefault();

            if (/*objeto.id != 0 && */actualizar != null)
            {
                actualizar.descripcion = objeto.descripcion;
                actualizar.descripcion_texto = objeto.descripcion_texto;
                actualizar.fechaModificacion = DateTime.Now;
                bd.SaveChanges();
                return actualizar.id;
            }
            else
            {
                descripcion_centro insertar = new descripcion_centro();
                insertar.id_centro = objeto.id_centro;
                insertar.descripcion = objeto.descripcion;
                insertar.descripcion_texto = objeto.descripcion_texto;
                insertar.fechaModificacion = DateTime.Now;
                bd.descripcion_centro.Add(insertar);
                bd.SaveChanges();

                return insertar.id;
            }
        }
        public static int ActualizarDescripcionGeneral(descripcion_general objeto)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.descripcion_general
                              where u.id == objeto.id
                              select u).FirstOrDefault();

            if (/*objeto.id != 0 && */actualizar != null)
            {
                actualizar.descripcion = objeto.descripcion;
                actualizar.descripcionTexto = objeto.descripcionTexto;
                bd.SaveChanges();
                return actualizar.id;
            }
            else
            {
                descripcion_general insertar = new descripcion_general();
                insertar.codigo = objeto.codigo;
                insertar.descripcion = objeto.descripcion;
                insertar.descripcionTexto = objeto.descripcionTexto;
                bd.descripcion_general.Add(insertar);
                bd.SaveChanges();

                return insertar.id;
            }
        }
        public static int ActualizarSistema(areanivel2 oSistema)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.areanivel2
                              where u.id == oSistema.id
                              select u).FirstOrDefault();

            if (oSistema.id != 0 && actualizar != null)
            {
                actualizar.codigo = oSistema.codigo;
                actualizar.nombre = oSistema.nombre;
                bd.SaveChanges();
                return actualizar.id;
            }
            else
            {
                areanivel2 insertar = new areanivel2();
                insertar.id_areanivel1 = oSistema.id_areanivel1;
                insertar.codigo = oSistema.codigo;
                insertar.nombre = oSistema.nombre;
                bd.areanivel2.Add(insertar);
                bd.SaveChanges();

                return insertar.id;
            }
        }


        public static int ActualizarRiesgo(tipos_riesgos oArea)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.tipos_riesgos
                              where u.id == oArea.id
                              select u).FirstOrDefault();

            if (oArea.id != 0 && actualizar != null)
            {
                //actualizar.codigo = oArea.codigo;
                actualizar.definicion = oArea.definicion;
                bd.SaveChanges();
                return actualizar.id;
            }
            else
            {
                return 0;
            }
        }

        public static int ActualizarArea(areanivel1 oArea)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.areanivel1
                              where u.id == oArea.id
                              select u).FirstOrDefault();

            if (oArea.id != 0 && actualizar != null)
            {
                actualizar.codigo = oArea.codigo;
                actualizar.nombre = oArea.nombre;
                actualizar.id_tecnologia = oArea.id_tecnologia;
                bd.SaveChanges();
                return actualizar.id;
            }
            else
            {
                areanivel1 insertar = new areanivel1();
                insertar.id_centro = oArea.id_centro;
                insertar.codigo = oArea.codigo;
                insertar.nombre = oArea.nombre;
                insertar.id_tecnologia = oArea.id_tecnologia;
                bd.areanivel1.Add(insertar);
                bd.SaveChanges();

                return insertar.id;
            }
        }
        public static int ActualizarMedidaGeneralRiesgo(riesgos_medidas oMedidas)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.riesgos_medidas
                              where u.id == oMedidas.id
                              select u).FirstOrDefault();

            if (oMedidas.id != 0 && actualizar != null)
            {
                actualizar.codigo = oMedidas.codigo;
                actualizar.descripcion = oMedidas.descripcion;
                actualizar.id_tecnologia = oMedidas.id_tecnologia;
                actualizar.id_apartado = oMedidas.id_apartado;
                actualizar.id_riesgo = oMedidas.id_riesgo;
                actualizar.imagen = oMedidas.imagen;
                actualizar.imagen_grande = oMedidas.imagen_grande;

                bd.SaveChanges();
                return actualizar.id;
            }
            else
            {
                riesgos_medidas insertar = new riesgos_medidas();
                insertar.id_centro = 0; //administrador
                insertar.codigo = oMedidas.codigo;
                insertar.descripcion = oMedidas.descripcion;
                insertar.id_tecnologia = oMedidas.id_tecnologia;
                insertar.id_apartado = oMedidas.id_apartado;
                insertar.id_riesgo = oMedidas.id_riesgo;
                insertar.imagen = oMedidas.imagen;
                insertar.imagen_grande = oMedidas.imagen_grande;
                bd.riesgos_medidas.Add(insertar);
                bd.SaveChanges();

                return insertar.id;
            }
        }
        public static int ActualizarMedidaGeneralRiesgoJson(riesgos_medidas oMedidas)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.riesgos_medidas
                              where u.id == oMedidas.id
                              select u).FirstOrDefault();

            if (oMedidas.id != 0 && actualizar != null)
            {
                actualizar.codigo = oMedidas.codigo;
                actualizar.descripcion = oMedidas.descripcion;
                actualizar.id_tecnologia = oMedidas.id_tecnologia;
                actualizar.id_apartado = oMedidas.id_apartado;
                actualizar.id_riesgo = oMedidas.id_riesgo;
                actualizar.imagen = oMedidas.imagen;
                actualizar.imagen_grande = oMedidas.imagen_grande;

                bd.SaveChanges();
                return actualizar.id;
            }
            else
            {
                riesgos_medidas insertar = new riesgos_medidas();
                insertar.id_centro = oMedidas.id_centro;
                insertar.codigo = oMedidas.codigo;
                insertar.descripcion = oMedidas.descripcion;
                insertar.id_tecnologia = oMedidas.id_tecnologia;
                insertar.id_apartado = oMedidas.id_apartado;
                insertar.id_riesgo = oMedidas.id_riesgo;
                insertar.imagen = oMedidas.imagen;
                insertar.imagen_grande = oMedidas.imagen_grande;
                bd.riesgos_medidas.Add(insertar);
                bd.SaveChanges();

                return insertar.id;
            }
        }

        public static int ActualizarMedidaGeneralRiesgoV2(riesgos_medidas oMedidas, string nombreArchivo, string chkIC, string chkIG, HttpPostedFileBase archivoIC, HttpPostedFileBase archivoIG)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.riesgos_medidas
                              where u.id == oMedidas.id
                              select u).FirstOrDefault();

            if (oMedidas.id != 0 && actualizar != null)
            {
                actualizar.codigo = oMedidas.codigo;
                actualizar.descripcion = oMedidas.descripcion;
                actualizar.id_tecnologia = oMedidas.id_tecnologia;
                actualizar.id_riesgo = oMedidas.id_riesgo;
                actualizar.id_apartado = oMedidas.id_apartado;
                //actualizar.imagen = nombreArchivo != null ? "../Content/images/medidas/" + nombreArchivo : "";
                //actualizar.imagen_grande = chkIC != null ? 0 : 1;

                if (nombreArchivo == null && chkIC == null && chkIG == null)
                {
                    actualizar.imagen = null;
                    actualizar.imagen_grande = null;
                }
                else if (chkIC != null && archivoIC != null)
                {
                    string rutaServer = System.Web.HttpContext.Current.Server.MapPath("~/");
                    //var ruta = oMedidas.imagen.Replace("..", "");
                    var ruta = "/Content/images/medidas/";
                    archivoIC.SaveAs(rutaServer + ruta + archivoIC.FileName);
                    actualizar.imagen_grande = 0;
                    actualizar.imagen = "../Content/images/medidas/" + nombreArchivo;
                }
                else if (chkIG != null && archivoIG != null)
                {
                    string rutaServer = System.Web.HttpContext.Current.Server.MapPath("~/");
                    //var ruta = oMedidas.imagen.Replace("..", "");
                    var ruta = "/Content/images/medidas/";
                    archivoIG.SaveAs(rutaServer + ruta + archivoIG.FileName);
                    actualizar.imagen_grande = 1;
                    actualizar.imagen = "../Content/images/medidas/" + nombreArchivo;
                }
                bd.SaveChanges();
                return actualizar.id;
            }
            else
            {
                riesgos_medidas insertar = new riesgos_medidas();
                insertar.codigo = oMedidas.codigo;
                insertar.descripcion = oMedidas.descripcion;
                insertar.id_tecnologia = oMedidas.id_tecnologia;
                insertar.id_riesgo = oMedidas.id_riesgo;
                insertar.id_apartado = oMedidas.id_apartado;
                insertar.id_centro = 0;

                if (nombreArchivo == null && chkIC == null && chkIG == null)
                {
                    insertar.imagen = null;
                    insertar.imagen_grande = null;
                }
                else if (chkIC != null && archivoIC != null)
                {
                    string rutaServer = System.Web.HttpContext.Current.Server.MapPath("~/");
                    //var ruta = oMedidas.imagen.Replace("..", "");
                    var ruta = "/Content/images/medidas/";
                    archivoIC.SaveAs(rutaServer + ruta + archivoIC.FileName);
                    insertar.imagen_grande = 0;
                    insertar.imagen = "../Content/images/medidas/" + nombreArchivo;
                }
                else if (chkIG != null)
                {
                    string rutaServer = System.Web.HttpContext.Current.Server.MapPath("~/");
                    //var ruta = oMedidas.imagen.Replace("..", "");
                    var ruta = "/Content/images/medidas/";
                    archivoIG.SaveAs(rutaServer + ruta + archivoIG.FileName);
                    insertar.imagen_grande = 1;
                    insertar.imagen = "../Content/images/medidas/" + nombreArchivo;
                }

                bd.riesgos_medidas.Add(insertar);
                bd.SaveChanges();

                return insertar.id;
            }
        }

        public static int ActualizarMedidaPreventivaImagen(int id, string nombreArchivo, string chkIC, string chkIG, HttpPostedFileBase archivoIC, HttpPostedFileBase archivoIG)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.medidaspreventivas_imagenes
                              where u.id_medida == id
                              select u).FirstOrDefault();

            if (id != 0 && actualizar != null)
            {
                actualizar.id_medida = id;

                if (nombreArchivo == null && chkIC == null && chkIG == null)
                {
                    actualizar.rutaImagen = null;
                    actualizar.tamano = null;
                }
                else if (chkIC != null && archivoIC != null)
                {
                    string rutaServer = System.Web.HttpContext.Current.Server.MapPath("~/");
                    //var ruta = oMedidas.imagen.Replace("..", "");
                    var ruta = "/Content/images/medidas/medidaspreventivas/";
                    archivoIC.SaveAs(rutaServer + ruta + archivoIC.FileName);
                    actualizar.tamano = false;
                    actualizar.rutaImagen = "../Content/images/medidas/medidaspreventivas/" + nombreArchivo;
                }
                else if (chkIG != null && archivoIG != null)
                {
                    string rutaServer = System.Web.HttpContext.Current.Server.MapPath("~/");
                    //var ruta = oMedidas.imagen.Replace("..", "");
                    var ruta = "/Content/images/medidas/medidaspreventivas/";
                    archivoIG.SaveAs(rutaServer + ruta + archivoIG.FileName);
                    actualizar.tamano = true;
                    actualizar.rutaImagen = "../Content/images/medidas/medidaspreventivas/" + nombreArchivo;
                }
                bd.SaveChanges();
                return actualizar.id;
            }
            else
            {
                medidaspreventivas_imagenes insertar = new medidaspreventivas_imagenes();
                insertar.id_medida = id;
                insertar.id_centro = 0;

                if (nombreArchivo == null && chkIC == null && chkIG == null)
                {
                    insertar.rutaImagen = null;
                    insertar.tamano = null;
                }
                else if (chkIC != null && archivoIC != null)
                {
                    string rutaServer = System.Web.HttpContext.Current.Server.MapPath("~/");
                    var ruta = "/Content/images/medidas/medidaspreventivas/";
                    archivoIC.SaveAs(rutaServer + ruta + archivoIC.FileName);
                    insertar.tamano = false;
                    insertar.rutaImagen = "../Content/images/medidas/medidaspreventivas/" + nombreArchivo;
                }
                else if (chkIG != null && archivoIG != null)
                {
                    string rutaServer = System.Web.HttpContext.Current.Server.MapPath("~/");
                    var ruta = "/Content/images/medidas/medidaspreventivas/";
                    archivoIG.SaveAs(rutaServer + ruta + archivoIG.FileName);
                    insertar.tamano = true;
                    insertar.rutaImagen = "../Content/images/medidas/medidaspreventivas/" + nombreArchivo;
                }

                bd.medidaspreventivas_imagenes.Add(insertar);
                bd.SaveChanges();

                return insertar.id;
            }
        }


        public static int ActualizarMedidaPreventivaV2(medidas_preventivas oMedidas)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.medidas_preventivas
                              where u.id == oMedidas.id
                              select u).FirstOrDefault();

            if (oMedidas.id != 0 && actualizar != null)
            {
                actualizar.codigo = oMedidas.codigo;
                actualizar.descripcion = oMedidas.descripcion;
                actualizar.id_situacion = oMedidas.id_situacion;
                actualizar.id_centro = 0;


                bd.SaveChanges();
                return actualizar.id;
            }
            else
            {
                medidas_preventivas insertar = new medidas_preventivas();
                insertar.codigo = oMedidas.codigo;
                insertar.descripcion = oMedidas.descripcion;
                insertar.id_situacion = oMedidas.id_situacion;
                insertar.id_centro = 0;

                bd.medidas_preventivas.Add(insertar);
                bd.SaveChanges();

                return insertar.id;
            }
        }


        public static int ActualizarMedidaGeneral(medidas_generales oMedidas)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.medidas_generales
                              where u.id == oMedidas.id
                              select u).FirstOrDefault();

            if (oMedidas.id != 0 && actualizar != null)
            {
                actualizar.descripcion = oMedidas.descripcion;
                actualizar.id_centro = null;
                actualizar.id_tecnologia = null;
                actualizar.codigo = oMedidas.codigo;
                actualizar.id_apartado_generales = oMedidas.id_apartado_generales;
                bd.SaveChanges();
                return actualizar.id;
            }
            else
            {
                medidas_generales insertar = new medidas_generales();
                insertar.descripcion = oMedidas.descripcion;
                insertar.id_centro = null;
                insertar.id_tecnologia = null;
                insertar.codigo = oMedidas.codigo;
                insertar.id_apartado_generales = oMedidas.id_apartado_generales;
                bd.medidas_generales.Add(insertar);
                bd.SaveChanges();

                return insertar.id;
            }
        }
        public static int ActualizarParametricaMedidas(parametrica_medidas oMedidas)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.parametrica_medidas
                              where u.id_medida == oMedidas.id_medida && u.id_centro == oMedidas.id_centro
                              select u).FirstOrDefault();

            if (actualizar != null)
            {
                actualizar.id_riesgo = oMedidas.id_riesgo;
                actualizar.id_tecnologia = oMedidas.id_tecnologia;
                actualizar.id_medida = oMedidas.id_medida;
                actualizar.id_situacion = oMedidas.id_situacion;
                actualizar.id_centro = oMedidas.id_centro;
                actualizar.activo = oMedidas.activo;
                bd.SaveChanges();
                return actualizar.id;
            }
            else
            {
                parametrica_medidas insertar = new parametrica_medidas();
                insertar.id_riesgo = oMedidas.id_riesgo;
                insertar.id_tecnologia = oMedidas.id_tecnologia;
                insertar.id_medida = oMedidas.id_medida;
                insertar.id_situacion = oMedidas.id_situacion;
                insertar.id_centro = oMedidas.id_centro;
                insertar.activo = oMedidas.activo;
                bd.parametrica_medidas.Add(insertar);
                bd.SaveChanges();
                return insertar.id;
            }
        }

        public static int ActualizarCheckSituacion(parametrica_medidas oMedidas)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.parametrica_medidas
                              where u.id_centro == oMedidas.id_centro && u.id_situacion == oMedidas.id_situacion && u.id_medida == null
                              select u).FirstOrDefault();

            if (actualizar != null)
            {
                actualizar.activo = oMedidas.activo;
                bd.SaveChanges();

                var medidas = ListarMedidas().Where(x => x.id_situacion == oMedidas.id_situacion);
                foreach (medidas_preventivas item in medidas)
                {
                    parametrica_medidas parMEdida = new parametrica_medidas();
                    parMEdida.id_medida = item.id;
                    parMEdida.id_centro = oMedidas.id_centro;
                    parMEdida.activo = oMedidas.activo;
                    ActualizarCheckMedida(parMEdida);
                }
                return actualizar.id;
            }
            else
            {
                parametrica_medidas insertar = new parametrica_medidas();
                insertar.id_riesgo = oMedidas.id_riesgo;
                insertar.id_tecnologia = oMedidas.id_tecnologia;
                insertar.id_medida = oMedidas.id_medida;
                insertar.id_situacion = oMedidas.id_situacion;
                insertar.id_centro = oMedidas.id_centro;
                insertar.activo = oMedidas.activo;
                bd.parametrica_medidas.Add(insertar);
                bd.SaveChanges();

                var medidas = ListarMedidas().Where(x => x.id_situacion == oMedidas.id_situacion);
                foreach (medidas_preventivas item in medidas)
                {
                    parametrica_medidas parMEdida = new parametrica_medidas();
                    parMEdida.id_medida = item.id;
                    parMEdida.id_centro = oMedidas.id_centro;
                    parMEdida.activo = oMedidas.activo;
                    ActualizarCheckMedida(parMEdida);
                }
                return insertar.id;
            }

        }
        public static int ActualizarSetCheckSituacion(parametrica_medidas oMedidas)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.parametrica_medidas
                              where u.id_centro == oMedidas.id_centro && u.id_situacion == oMedidas.id_situacion && u.id_medida == null
                              select u).FirstOrDefault();

            if (actualizar != null)
            {
                actualizar.activo = oMedidas.activo;
                bd.SaveChanges();

                return actualizar.id;
            }
            else
            {
                parametrica_medidas insertar = new parametrica_medidas();
                insertar.id_riesgo = oMedidas.id_riesgo;
                insertar.id_tecnologia = oMedidas.id_tecnologia;
                insertar.id_medida = oMedidas.id_medida;
                insertar.id_situacion = oMedidas.id_situacion;
                insertar.id_centro = oMedidas.id_centro;
                insertar.activo = oMedidas.activo;
                bd.parametrica_medidas.Add(insertar);
                bd.SaveChanges();

                return insertar.id;
            }

        }
        public static int ActualizarCheckMedida(parametrica_medidas oMedidas)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.parametrica_medidas
                              where u.id_centro == oMedidas.id_centro && u.id_medida == oMedidas.id_medida
                              select u).FirstOrDefault();

            if (actualizar != null)
            {
                actualizar.activo = oMedidas.activo;
                bd.SaveChanges();
                return actualizar.id;
            }
            else
            {
                parametrica_medidas insertar = new parametrica_medidas();
                insertar.id_riesgo = oMedidas.id_riesgo;
                insertar.id_tecnologia = oMedidas.id_tecnologia;
                insertar.id_medida = oMedidas.id_medida;
                insertar.id_situacion = oMedidas.id_situacion;
                insertar.id_centro = oMedidas.id_centro;
                insertar.activo = oMedidas.activo;
                bd.parametrica_medidas.Add(insertar);
                bd.SaveChanges();

            }
            return 0;
        }

        public static int ActualizarMedidaPreventiva(medidas_preventivas oMedidas)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.medidas_preventivas
                              where u.id == oMedidas.id
                              select u).FirstOrDefault();

            if (oMedidas.id != 0 && actualizar != null)
            {
                actualizar.codigo = oMedidas.codigo;
                actualizar.descripcion = oMedidas.descripcion;
                actualizar.id_situacion = oMedidas.id_situacion;
                actualizar.id_centro = oMedidas.id_centro;
                actualizar.id_situacion = actualizar.id_situacion;
                bd.SaveChanges();
                return actualizar.id;
            }
            else
            {
                medidas_preventivas insertar = new medidas_preventivas();
                insertar.id_situacion = oMedidas.id_situacion;
                insertar.codigo = oMedidas.codigo;
                insertar.descripcion = oMedidas.descripcion;
                insertar.id_centro = oMedidas.id_centro;
                bd.medidas_preventivas.Add(insertar);
                bd.SaveChanges();
                return insertar.id;
            }
        }
        public static int GuardarMedidaAjax(medidas_preventivas oMedidas)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.medidas_preventivas
                              where u.id == oMedidas.id
                              select u).FirstOrDefault();

            if (oMedidas.id != 0 && actualizar != null)
            {
                actualizar.descripcion = oMedidas.descripcion;
                actualizar.id_centro = oMedidas.id_centro;
                bd.SaveChanges();
                return actualizar.id;
            }
            else
            {
                medidas_preventivas insertar = new medidas_preventivas();
                insertar.descripcion = oMedidas.descripcion;
                insertar.id_centro = oMedidas.id_centro;
                bd.medidas_preventivas.Add(insertar);
                bd.SaveChanges();
                return insertar.id;
            }
        }
        public static int GuardarRiesgoMedidaAjax(riesgos_medidas oMedidas)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.riesgos_medidas
                              where u.id == oMedidas.id
                              select u).FirstOrDefault();

            if (oMedidas.id != 0 && actualizar != null)
            {
                actualizar.descripcion = oMedidas.descripcion;
                actualizar.id_centro = oMedidas.id_centro;
                bd.SaveChanges();
                return actualizar.id;
            }
            else
            {
                riesgos_medidas insertar = new riesgos_medidas();
                insertar.descripcion = oMedidas.descripcion;
                insertar.id_centro = oMedidas.id_centro;
                bd.riesgos_medidas.Add(insertar);
                bd.SaveChanges();
                return insertar.id;
            }
        }

        public static int ActualizarSubMedidasPreventivas(submedidas_preventivas oMedidas)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.submedidas_preventivas
                              where u.id == oMedidas.id
                              select u).FirstOrDefault();

            if (!(oMedidas.id != 0 && actualizar != null))
            {

                submedidas_preventivas insertar = new submedidas_preventivas();
                insertar.descripcion = oMedidas.descripcion;
                insertar.codigo = oMedidas.codigo;
                insertar.id_medida_preventiva = oMedidas.id_medida_preventiva;
                bd.submedidas_preventivas.Add(insertar);
                bd.SaveChanges();
                return insertar.id;
            }
            return 0;
        }
        public static int ActualizarSituacion(riesgos_situaciones oMedidas)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.riesgos_situaciones
                              where u.id == oMedidas.id
                              select u).FirstOrDefault();

            if (oMedidas.id != 0 && actualizar != null)
            {
                actualizar.codigo = oMedidas.codigo;
                actualizar.descripcion = oMedidas.descripcion;
                actualizar.id_tipo_riesgo = oMedidas.id_tipo_riesgo;
                bd.SaveChanges();
                return actualizar.id;
            }
            else
            {
                riesgos_situaciones insertar = new riesgos_situaciones();
                insertar.id_tipo_riesgo = oMedidas.id_tipo_riesgo;
                insertar.codigo = oMedidas.codigo;
                insertar.descripcion = oMedidas.descripcion;
                bd.riesgos_situaciones.Add(insertar);
                bd.SaveChanges();
                return insertar.id;
            }
        }
        public static int ActualizarFechaUsuarioMatriz(int idVersion, string usuario)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            List<version_matriz> registros = (from u in bd.version_matriz
                                              where u.id == idVersion
                                              select u).ToList();

            foreach (var actualizar in registros)
            {
                if (idVersion != 0 && actualizar != null)
                {
                    actualizar.usuario = usuario;
                    actualizar.fechaModificacion = DateTime.Now;
                    bd.SaveChanges();
                    return actualizar.id;
                }
            }


            return 0;

        }

        public static int ActualizarVersionMatriz(int idCentro, string usuario)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            List<version_matriz> registros = (from u in bd.version_matriz
                                              where u.id_centro == idCentro
                                              select u).ToList();

            foreach (var actualizar in registros)
            {
                if (idCentro != 0 && actualizar != null)
                {
                    actualizar.estado = 0;
                    //actualizar.usuario = usuario;
                    //actualizar.fechaModificacion = DateTime.Now;
                    bd.SaveChanges();
                }
            }


            int? version = (from u in bd.version_matriz
                            where u.id_centro == idCentro
                            select u.version).Max();

            version_matriz insertar = new version_matriz();
            insertar.id_centro = idCentro;
            insertar.estado = 1;
            insertar.fechaCreacion = DateTime.Now;
            insertar.fechaModificacion = DateTime.Now;
            insertar.version = (version == null) ? 0 : version + 1;
            insertar.usuario = usuario;

            bd.version_matriz.Add(insertar);
            bd.SaveChanges();

            return insertar.id;

        }

        public static List<usuarios> ListarParticipantesAsignar(int idReunion)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var registros = from v in bd.usuarios
                            where !(from ar in bd.reuniones_participantes
                                    where ar.idreunion == idReunion
                                    select ar.idUsuario).Contains(v.idUsuario)
                            orderby v.nombre ascending
                            select v;

            List<usuarios> listaUsuarios = registros.ToList();

            return listaUsuarios;

        }

        public static List<referenciales> ListarReferencialesAsignar(int idAuditoria)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var registros = from v in bd.referenciales
                            where !(from ar in bd.auditorias_referenciales
                                    where ar.idAuditoria == idAuditoria
                                    select ar.idReferencial).Contains(v.id)
                            orderby v.id descending
                            select v;

            List<referenciales> listaReferenciales = registros.ToList();

            return listaReferenciales;

        }

        public static List<VISTA_AuditoriaReferenciales> ListarReferencialesAsignados(int idAuditoria)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var registros = from v in bd.VISTA_AuditoriaReferenciales
                            where v.idAuditoria == idAuditoria
                            orderby v.idReferencial
                            select v;

            List<VISTA_AuditoriaReferenciales> listaReferenciales = registros.ToList();

            return listaReferenciales;

        }

        public static List<VISTA_AuditoriaObservadores> ListarUsuariosAsignados(int idAuditoria)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var registros = from v in bd.VISTA_AuditoriaObservadores
                            where v.idAuditoria == idAuditoria
                            orderby v.id
                            select v;

            List<VISTA_AuditoriaObservadores> listaObservadores = registros.ToList();

            return listaObservadores;

        }

        public static List<VISTA_ReunionParticipantes> ListarParticipantesAsignados(int idReunion)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var registros = from v in bd.VISTA_ReunionParticipantes
                            where v.idreunion == idReunion
                            orderby v.id
                            select v;

            List<VISTA_ReunionParticipantes> listaParticipantes = registros.ToList();

            return listaParticipantes;

        }

        public static List<VISTA_Auditores> ListarAuditores(int idAuditoria)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var registros = from v in bd.VISTA_Auditores
                            where v.idAuditoria == idAuditoria
                            orderby v.id
                            select v;

            List<VISTA_Auditores> listaAuditores = registros.ToList();

            return listaAuditores;

        }

        public static List<log> ListarLog()
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var registros = from v in bd.log orderby v.fecha descending select v;

            List<log> listaArchivosCargados = registros.ToList();

            return listaArchivosCargados;

        }

        public static List<VISTA_ListarUsuarios> ListarUsuarios(int idOrganizacion)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var registros = from v in bd.VISTA_ListarUsuarios join cen in bd.usuario_centros on v.idUsuario equals cen.idusuario where cen.idcentro == idOrganizacion orderby v.idUsuario descending select v;

            List<VISTA_ListarUsuarios> listaArchivosCargados = registros.ToList();

            return listaArchivosCargados;

        }

        public static List<VISTA_ListarUsuarios> ListarUsuariosCentral(int idOrganizacion)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            centros cent = ObtenerCentroPorID(idOrganizacion);

            List<VISTA_ListarUsuarios> listadoUsuariosCentral = new List<VISTA_ListarUsuarios>();
            if (cent.tipo == 4)
            {
                listadoUsuariosCentral = (from v in bd.VISTA_ListarUsuarios where v.perfil == 1 || v.perfil == 2 orderby v.nombre ascending select v).ToList();
                int cont = 0;
                foreach (var item in listadoUsuariosCentral)
                {
                    listadoUsuariosCentral[cont].nombre = item.nombre.ToUpper();
                    cont++;
                }
            }
            else
            {
                listadoUsuariosCentral = (from v in bd.VISTA_ListarUsuarios join cen in bd.usuario_centros on v.idUsuario equals cen.idusuario where cen.idcentro == idOrganizacion orderby v.nombre ascending select v).ToList();
                int cont = 0;
                foreach (var item in listadoUsuariosCentral)
                {
                    listadoUsuariosCentral[cont].nombre = item.nombre.ToUpper();
                    cont++;
                }

                List<VISTA_ListarUsuarios> listadoUsuariosAdmin = new List<VISTA_ListarUsuarios>();

                listadoUsuariosAdmin = (from v in bd.VISTA_ListarUsuarios where v.perfil == 1 orderby v.nombre ascending select v).ToList();
                cont = 0;
                foreach (var item in listadoUsuariosAdmin)
                {
                    listadoUsuariosAdmin[cont].nombre = item.nombre.ToUpper();
                    cont++;
                }
                listadoUsuariosCentral.AddRange(listadoUsuariosAdmin);

                listadoUsuariosCentral = listadoUsuariosCentral.OrderBy(VISTA_ListarUsuarios => VISTA_ListarUsuarios.nombre).ToList();


            }

            return listadoUsuariosCentral;

        }

        public static List<tipo_accionesmejora> ListarTiposAccionMejora()
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            List<tipo_accionesmejora> listadoTipos = new List<tipo_accionesmejora>();

            listadoTipos = (from v in bd.tipo_accionesmejora select v).ToList();

            return listadoTipos;

        }

        public static List<modulos> ListarModulos()
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            List<modulos> listadoModulos = new List<modulos>();

            listadoModulos = (from v in bd.modulos
                              where v.id != 3 && v.id != 4 && v.id != 5 && v.id != 6 && v.id != 7 && v.id != 8
                              && v.id != 10 && v.id != 12 && v.id != 13 && v.id != 14 && v.id != 15
                              select v).ToList();

            return listadoModulos;

        }

        public static List<noticias> ListarNoticias(int idOrganizacion)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var registros = (from v in bd.noticias where (v.organizacion == idOrganizacion || v.organizacion == null) && (v.fechaexp > DateTime.Now || v.fechaexp == null) && v.validada == 1 orderby v.id descending select v).Take(10);

            List<noticias> listaArchivosCargados = registros.ToList();

            return listaArchivosCargados;

        }

        public static List<noticias> ListarNoticiasGrid(int idOrganizacion)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var registros = from v in bd.noticias where (v.organizacion == idOrganizacion || v.organizacion == null) && v.validada == 1 orderby v.id descending select v;

            List<noticias> listaArchivosCargados = registros.ToList();

            return listaArchivosCargados;

        }

        public static List<noticias> ListarNoticias()
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var registros = (from v in bd.noticias where v.organizacion == null && (v.fechaexp > DateTime.Now || v.fechaexp == null) && v.validada == 1 orderby v.id descending select v).Take(10);

            List<noticias> listaArchivosCargados = registros.ToList();

            return listaArchivosCargados;

        }

        public static List<noticias> ListarNoticiasGenerales()
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var registros = (from v in bd.noticias where v.organizacion == null && (v.fechaexp > DateTime.Now || v.fechaexp == null) && v.validada == 1 orderby v.id descending select v).Take(10);

            List<noticias> listaArchivosCargados = registros.ToList();

            return listaArchivosCargados;

        }

        public static List<noticias> ListarNoticiasCentral(int idCentral)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var registros = (from v in bd.noticias where v.organizacion == idCentral && (v.fechaexp > DateTime.Now || v.fechaexp == null) && v.validada == 1 orderby v.id descending select v).Take(10);

            List<noticias> listaArchivosCargados = registros.ToList();

            return listaArchivosCargados;

        }

        public static List<noticias> ListarNoticiasGrid()
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var registros = from v in bd.noticias where v.organizacion == null && v.validada == 1 orderby v.id descending select v;

            List<noticias> listaArchivosCargados = registros.ToList();

            return listaArchivosCargados;

        }

        public static List<log> ListarLog(int idCentro)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var registros = from v in bd.log join u in bd.usuarios on v.usuario equals u.nombre join c in bd.usuario_centros on u.idUsuario equals c.idusuario where c.idcentro == idCentro orderby v.fecha descending select v;

            List<log> listaArchivosCargados = registros.ToList();

            return listaArchivosCargados;

        }

        public static List<documentacion_hist> ListarLogDocumentacion(int idFichero)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            List<documentacion_hist> listaArchivosCargados = new List<documentacion_hist>();
            if ((from v in bd.documentacion where v.idFichero == idFichero select v).Count() > 0)
            {
                var registros = from v in bd.documentacion_hist where v.idVigente == idFichero orderby v.id descending select v;

                listaArchivosCargados = registros.ToList();
            }

            return listaArchivosCargados;

        }
        public static VISTA_DocumentacionVersion ListarVersionDoc(int idDoc)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            VISTA_DocumentacionVersion nuevo = new VISTA_DocumentacionVersion();

            nuevo = (from u in bd.VISTA_DocumentacionVersion
                     where u.idFichero == idDoc
                     select u).FirstOrDefault();

            return nuevo;
        }

        public static List<stakeholders_nivel1> ListarStakeholdersN1()
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var registro = (from v in bd.stakeholders_nivel1 orderby v.id select v).ToList();

            return registro;

        }

        public static List<VISTA_StakeholdersN2> ListarStakeholdersN2()
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var registro = (from v in bd.VISTA_StakeholdersN2 orderby v.id select v).ToList();

            return registro;

        }

        public static List<VISTA_StakeholdersN3> ListarStakeholdersN3()
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var registro = (from v in bd.VISTA_StakeholdersN3 orderby v.parteinteresada descending, v.id ascending select v).ToList();

            return registro;

        }

        public static List<VISTA_StakeholdersN4> ListarStakeholdersN4(int idcentral)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var registro = (from v in bd.VISTA_StakeholdersN4 where v.idcentral == idcentral orderby v.parteinteresada descending, v.id ascending select v).ToList();

            return registro;

        }

        public static List<riesgos_categorias> ListarCategorias()
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var registro = (from v in bd.riesgos_categorias orderby v.categoria ascending, v.id ascending select v).ToList();

            return registro;

        }

        public static List<riesgos_tipologias> ListarTipologias()
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var registro = (from v in bd.riesgos_tipologias orderby v.tipologia ascending, v.id ascending select v).ToList();

            return registro;

        }

        public static VISTA_ObtenerDocumentacion ListarDocumentacion(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var registro = (from v in bd.VISTA_ObtenerDocumentacion where v.idFichero == id orderby v.idFichero descending select v).FirstOrDefault();

            return registro;

        }

        public static List<VISTA_ObtenerDocumentacion> ListarDocumentacion(int nivel, centros centroseleccionado)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            List<VISTA_ObtenerDocumentacion> registros = new List<VISTA_ObtenerDocumentacion>();

            switch (nivel)
            {
                case 1:
                    registros = (from v in bd.VISTA_ObtenerDocumentacion where v.nivel == nivel orderby v.idFichero descending select v).ToList();
                    break;
                case 2:
                    if (centroseleccionado.tipo == 4)
                        registros = (from v in bd.VISTA_ObtenerDocumentacion where v.nivel == nivel orderby v.idFichero descending select v).ToList();
                    else
                        registros = (from v in bd.VISTA_ObtenerDocumentacion where v.nivel == nivel && v.tipocentral == centroseleccionado.tipo orderby v.idFichero descending select v).ToList();
                    break;
                case 4:
                    registros = (from v in bd.VISTA_ObtenerDocumentacion where v.nivel == nivel orderby v.idFichero descending select v).ToList();
                    break;
                default:
                    registros = (from v in bd.VISTA_ObtenerDocumentacion where v.nivel == nivel && v.idcentro == centroseleccionado.id orderby v.idFichero descending select v).ToList();
                    break;
            }


            return registros;

        }

        public static Cell ConstructCell(string value, CellValues dataType)
        {
            return new Cell()
            {
                CellValue = new CellValue(value),
                DataType = new EnumValue<CellValues>(dataType)
            };
        }

        private static void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;

            }
            finally
            {
                GC.Collect();
            }

        }

        public static int ActualizarUsuario(VISTA_ObtenerUsuario oUsuario)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.usuarios
                              where u.idUsuario == oUsuario.idUsuario
                              select u).FirstOrDefault();

            if (oUsuario.idUsuario != 0 && actualizar != null)
            {
                actualizar.login = oUsuario.nombre;
                actualizar.nombre = oUsuario.nombreap;
                actualizar.password = System.Text.Encoding.UTF8.GetBytes(oUsuario.password);
                actualizar.perfil = oUsuario.perfil;
                actualizar.mail = oUsuario.mail;
                actualizar.telefono = oUsuario.telefono;
                actualizar.puesto = oUsuario.puesto;
                actualizar.idUnidad = oUsuario.idUnidad;

                actualizar.baja = null;
                bd.SaveChanges();

                return actualizar.idUsuario;
            }
            else
            {

                usuarios insertar = new usuarios();
                insertar.login = oUsuario.nombre;
                insertar.password = System.Text.Encoding.UTF8.GetBytes(oUsuario.password);
                insertar.perfil = oUsuario.perfil;
                insertar.nombre = oUsuario.nombreap;
                insertar.mail = oUsuario.mail;
                insertar.telefono = oUsuario.telefono;
                insertar.puesto = oUsuario.puesto;
                insertar.fecha_registro = DateTime.Now.Date;
                bd.usuarios.Add(insertar);
                bd.SaveChanges();

                return insertar.idUsuario;
            }
        }

        public static int ActualizarImagenNivel(areas_imagenes area)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.areas_imagenes
                              where u.id_areanivel1 == area.id_areanivel1
                              select u).FirstOrDefault();
            if (actualizar != null)
            {
                actualizar.rutaImagen = area.rutaImagen;
                actualizar.id_areanivel1 = area.id_areanivel1;
                bd.SaveChanges();
                return actualizar.id;
            }
            else
            {
                areas_imagenes insertar = new areas_imagenes();
                insertar.rutaImagen = area.rutaImagen;
                insertar.id_areanivel1 = area.id_areanivel1;
                bd.areas_imagenes.Add(insertar);
                bd.SaveChanges();
                return insertar.id;
            }

        }
        public static int ActualizarImagenNivel2(areas2_imagenes area)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.areas2_imagenes
                              where u.id_areanivel2 == area.id_areanivel2
                              select u).FirstOrDefault();
            if (actualizar != null)
            {
                actualizar.rutaImagen = area.rutaImagen;
                actualizar.id_areanivel2 = area.id_areanivel2;
                bd.SaveChanges();
                return actualizar.id;
            }
            else
            {
                areas2_imagenes insertar = new areas2_imagenes();
                insertar.rutaImagen = area.rutaImagen;
                insertar.id_areanivel2 = area.id_areanivel2;
                bd.areas2_imagenes.Add(insertar);
                bd.SaveChanges();
                return insertar.id;
            }

        }
        public static int ActualizarImagenNivel3(equipos_imagenes area)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.equipos_imagenes
                              where u.id_areanivel3 == area.id_areanivel3
                              select u).FirstOrDefault();
            if (actualizar != null)
            {
                actualizar.rutaImagen = area.rutaImagen;
                actualizar.id_areanivel3 = area.id_areanivel3;
                bd.SaveChanges();
                return actualizar.id;
            }
            else
            {
                equipos_imagenes insertar = new equipos_imagenes();
                insertar.rutaImagen = area.rutaImagen;
                insertar.id_areanivel3 = area.id_areanivel3;
                bd.equipos_imagenes.Add(insertar);
                bd.SaveChanges();
                return insertar.id;
            }

        }
        public static int ActualizarImagenNivel4(areas4_imagenes area)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.areas4_imagenes
                              where u.id_areanivel4 == area.id_areanivel4
                              select u).FirstOrDefault();
            if (actualizar != null)
            {
                actualizar.rutaImagen = area.rutaImagen;
                actualizar.id_areanivel4 = area.id_areanivel4;
                bd.SaveChanges();
                return actualizar.id;
            }
            else
            {
                areas4_imagenes insertar = new areas4_imagenes();
                insertar.rutaImagen = area.rutaImagen;
                insertar.id_areanivel4 = area.id_areanivel4;
                bd.areas4_imagenes.Add(insertar);
                bd.SaveChanges();
                return insertar.id;
            }
        }
        public static int ActualizarImagenRutaMedidaGeneralRiesgo(riesgos_medidas medida)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.riesgos_medidas
                              where u.id == medida.id
                              select u).FirstOrDefault();
            actualizar.imagen = medida.imagen;
            actualizar.imagen_grande = medida.imagen_grande;
            bd.SaveChanges();
            return actualizar.id;
        }

        public static string ObtenerImagenSituacionMedida(int medida)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.medidaspreventivas_imagenes
                              where u.id_medida == medida
                              select u).FirstOrDefault();
            if (actualizar != null)
            {
                if (actualizar.rutaImagen == null)
                {
                    return "";
                }
                else
                {
                    return actualizar.rutaImagen;
                }

            }
            else
            {
                return "";
            }

        }
        public static bool EsImagenGrandeMedida(int medida)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.medidaspreventivas_imagenes
                              where u.id_medida == medida
                              select u).FirstOrDefault();
            if (actualizar != null)
            {
                if (actualizar.tamano != null)
                {
                    if (actualizar.tamano == true)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

        }
        public static bool EsImagenGrandeMedidaGeneral(int medida)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.medidas_generales_imagenes
                              where u.id_medida_general == medida
                              select u).FirstOrDefault();
            if (actualizar != null)
            {
                if (actualizar.tamano == null)
                {
                    return false;
                }
                else
                {
                    if (actualizar.tamano == true)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

            }
            else
            {
                return false;
            }

        }

        public static string ObtenerImagenRiesgo(int riesgo)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.tipos_riesgos
                              where u.id == riesgo
                              select u).FirstOrDefault();
            if (actualizar != null)
            {
                if (actualizar.rutaImagen == null)
                {
                    return "";
                }
                else
                {
                    return actualizar.rutaImagen;
                }

            }
            else
            {
                return "";
            }

        }

        public static int ActualizarImagenRutaMedidaSituacionRiesgo(medidaspreventivas_imagenes medida)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.medidaspreventivas_imagenes
                              where u.id_medida == medida.id_medida
                              select u).FirstOrDefault();

            if (actualizar != null)
            {
                actualizar.rutaImagen = medida.rutaImagen;
                actualizar.id_medida = medida.id_medida;
                actualizar.id_centro = medida.id_centro;
                actualizar.tamano = medida.tamano;
                bd.SaveChanges();
                return actualizar.id;
            }
            else
            {
                medidaspreventivas_imagenes insertar = new medidaspreventivas_imagenes();
                insertar.rutaImagen = medida.rutaImagen;
                insertar.id_medida = medida.id_medida;
                insertar.id_centro = medida.id_centro;
                insertar.tamano = medida.tamano;
                bd.medidaspreventivas_imagenes.Add(insertar);
                bd.SaveChanges();
                return insertar.id;
            }

            //return actualizar.id;
        }


        public static int ActualizarCentro(centros oCentro, centros_zonas oCentZona, centros_agrupacion oCentAgru)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.centros
                              where u.id == oCentro.id
                              select u).FirstOrDefault();

            if (oCentro.id != 0 && actualizar != null)
            {
                actualizar.nombre = oCentro.nombre;
                actualizar.siglas = oCentro.siglas;
                actualizar.tipo = oCentro.tipo;


                //actualizar.provincia = oCentro.provincia;
                actualizar.direccion = oCentro.direccion;
                actualizar.coordenadas = oCentro.coordenadas;

                if (oCentro.rutaImagen != null)
                {
                    //eliminamos imagen anterior
                    //string rutaServer = System.Web.HttpContext.Current.Server.MapPath("~/");
                    //var ruta = actualizar.rutaImagen.Replace("..", "");
                    //if (System.IO.File.Exists(rutaServer + ruta))
                    //{
                    //    System.IO.File.Delete(rutaServer + ruta);
                    //}
                    actualizar.rutaImagen = oCentro.rutaImagen;
                }
                if (oCentro.rutaImagenLogo != null)
                {
                    //eliminamos imagen anterior
                    //string rutaServer = System.Web.HttpContext.Current.Server.MapPath("~/");
                    //var ruta = actualizar.rutaImagenLogo.Replace("..", "");
                    //if (System.IO.File.Exists(rutaServer + ruta))
                    //{
                    //    System.IO.File.Delete(rutaServer + ruta);
                    //}
                    actualizar.rutaImagenLogo = oCentro.rutaImagenLogo;
                }


                //provincia prov = (from u in bd.provincia
                //                  where u.id_provincia == oCentro.provincia
                //                  select u).FirstOrDefault();

                //actualizar.ubicacion = prov.id_comunidad_autonoma;

                // Insercción código - Rafael Ortega
                var tecnologiaCentro = (from t in bd.tecnologias_centros
                                        where t.id_centro == oCentro.id
                                        select t).FirstOrDefault();

                tecnologiaCentro.id_tecnologia = (int)oCentro.tipo;

                if (oCentZona.id_zona != 0 && oCentZona.id_zona != null)
                {
                    oCentZona.id_centro = oCentro.id;
                    bd.centros_zonas.Add(oCentZona);
                }

                if (oCentAgru.id_agrupacion != 0 && oCentAgru.id_agrupacion != null)
                {
                    oCentAgru.id_centro = oCentro.id;
                    bd.centros_agrupacion.Add(oCentAgru);
                }
                // Insercción código - Rafael Ortega

                bd.SaveChanges();

                return actualizar.id;
            }
            else
            {

                centros insertar = new centros();

                insertar.nombre = oCentro.nombre;
                insertar.siglas = oCentro.siglas;
                insertar.tipo = oCentro.tipo;
                //insertar.provincia = oCentro.provincia;
                insertar.rutaImagen = oCentro.rutaImagen;
                insertar.direccion = oCentro.direccion;
                insertar.coordenadas = oCentro.coordenadas;
                insertar.rutaImagenLogo = oCentro.rutaImagenLogo;

                //if (insertar.provincia != 0)
                //{
                //    provincia prov = (from u in bd.provincia
                //                      where u.id_provincia == oCentro.provincia
                //                      select u).FirstOrDefault();

                //    insertar.ubicacion = prov.id_comunidad_autonoma;
                //}
                //else
                //{
                //    insertar.ubicacion = 0;
                //}

                bd.centros.Add(insertar);
                bd.SaveChanges();

                // Insercción código - Rafael Ortega
                tecnologias_centros insertarTecnoCentro = new tecnologias_centros();

                var centroLast = (from c in bd.centros
                                  select c).ToList().Last();

                insertarTecnoCentro.id_centro = centroLast.id;
                insertarTecnoCentro.id_tecnologia = (int)oCentro.tipo;

                bd.tecnologias_centros.Add(insertarTecnoCentro);

                if (oCentZona.id_zona != 0 && oCentZona.id_zona != null)
                {
                    oCentZona.id_centro = centroLast.id;
                    bd.centros_zonas.Add(oCentZona);
                }

                if (oCentAgru.id_agrupacion != 0 && oCentAgru.id_agrupacion != null)
                {
                    oCentAgru.id_centro = centroLast.id;
                    bd.centros_agrupacion.Add(oCentAgru);
                }

                bd.SaveChanges();
                // Insercción código - Rafael Ortega

                return insertar.id;
            }



        }
        public static int ActualizarImagenCentro(centros oCentro)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.centros
                              where u.id == oCentro.id
                              select u).FirstOrDefault();

            if (oCentro.id != 0 && actualizar != null)
            {

                actualizar.rutaImagen = oCentro.rutaImagen;
                bd.SaveChanges();

                return actualizar.id;
            }

            return 0;

        }
        public static int ActualizarImagenMedidaGeneral(medidas_generales_imagenes oCentro)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.medidas_generales_imagenes
                              where u.id_medida_general == oCentro.id
                              select u).FirstOrDefault();

            if (oCentro.id != 0 && actualizar != null)
            {

                actualizar.rutaImagen = oCentro.rutaImagen;
                actualizar.tamano = oCentro.tamano;
                bd.SaveChanges();

                return actualizar.id;
            }
            else
            {
                medidas_generales_imagenes insertar = new medidas_generales_imagenes();
                insertar.id_medida_general = oCentro.id_medida_general;
                insertar.rutaImagen = oCentro.rutaImagen;
                insertar.tamano = oCentro.tamano;
                bd.medidas_generales_imagenes.Add(insertar);
                bd.SaveChanges();
            }

            return 0;

        }
        public static int ActualizarImagenCentroLogo(centros oCentro)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.centros
                              where u.id == oCentro.id
                              select u).FirstOrDefault();

            if (oCentro.id != 0 && actualizar != null)
            {

                actualizar.rutaImagenLogo = oCentro.rutaImagenLogo;
                bd.SaveChanges();

                return actualizar.id;
            }

            return 0;

        }
        public static int ActualizarTipoDoc(tipodocumento oTipoDoc)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.tipodocumento
                              where u.id == oTipoDoc.id
                              select u).FirstOrDefault();

            if (oTipoDoc.id != 0 && actualizar != null)
            {
                actualizar.tipo = oTipoDoc.tipo;
                actualizar.nivel = oTipoDoc.nivel;
                actualizar.tecnologia = oTipoDoc.tecnologia;

                bd.SaveChanges();

                return actualizar.id;
            }
            else
            {
                tipodocumento insertar = new tipodocumento();
                insertar.tipo = oTipoDoc.tipo;
                insertar.nivel = oTipoDoc.nivel;
                insertar.tecnologia = oTipoDoc.tecnologia;

                bd.tipodocumento.Add(insertar);
                bd.SaveChanges();

                return insertar.id;
            }
        }

        public static int ActualizarStakeholderN1(stakeholders_nivel1 oStakeholder)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.stakeholders_nivel1
                              where u.id == oStakeholder.id
                              select u).FirstOrDefault();

            if (oStakeholder.id != 0 && actualizar != null)
            {
                actualizar.denominacion = oStakeholder.denominacion;
                bd.SaveChanges();
                return actualizar.id;
            }
            else
            {
                stakeholders_nivel1 insertar = new stakeholders_nivel1();
                insertar.denominacion = oStakeholder.denominacion;
                bd.stakeholders_nivel1.Add(insertar);
                bd.SaveChanges();

                return insertar.id;
            }
        }

        public static int ActualizarStakeholderN2(stakeholders_nivel2 oStakeholder)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.stakeholders_nivel2
                              where u.id == oStakeholder.id
                              select u).FirstOrDefault();

            if (oStakeholder.id != 0 && actualizar != null)
            {
                actualizar.denominacion = oStakeholder.denominacion;
                actualizar.idnivel1 = oStakeholder.idnivel1;
                bd.SaveChanges();
                return actualizar.id;
            }
            else
            {
                stakeholders_nivel2 insertar = new stakeholders_nivel2();
                insertar.denominacion = oStakeholder.denominacion;
                insertar.idnivel1 = oStakeholder.idnivel1;
                bd.stakeholders_nivel2.Add(insertar);
                bd.SaveChanges();

                return insertar.id;
            }
        }

        public static int ActualizarStakeholderN3(stakeholders_nivel3 oStakeholder)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.stakeholders_nivel3
                              where u.id == oStakeholder.id
                              select u).FirstOrDefault();

            if (oStakeholder.id != 0 && actualizar != null)
            {
                actualizar.denominacion = oStakeholder.denominacion;
                actualizar.idnivel2 = oStakeholder.idnivel2;
                actualizar.parteinteresada = oStakeholder.parteinteresada;
                actualizar.necesidades = oStakeholder.necesidades;
                bd.SaveChanges();
                return actualizar.id;
            }
            else
            {
                stakeholders_nivel3 insertar = new stakeholders_nivel3();
                insertar.denominacion = oStakeholder.denominacion;
                insertar.idnivel2 = oStakeholder.idnivel2;
                insertar.parteinteresada = oStakeholder.parteinteresada;
                insertar.necesidades = oStakeholder.necesidades;
                bd.stakeholders_nivel3.Add(insertar);
                bd.SaveChanges();

                return insertar.id;
            }
        }

        public static int ActualizarStakeholderN4(stakeholders_nivel4 oStakeholder)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.stakeholders_nivel4
                              where u.id == oStakeholder.id
                              select u).FirstOrDefault();

            if (oStakeholder.id != 0 && actualizar != null)
            {
                actualizar.denominacion = oStakeholder.denominacion;
                actualizar.idnivel3 = oStakeholder.idnivel3;
                actualizar.requisitosrel = oStakeholder.requisitosrel;
                actualizar.necesidades = oStakeholder.necesidades;
                actualizar.idcentral = oStakeholder.idcentral;
                bd.SaveChanges();
                return actualizar.id;
            }
            else
            {
                stakeholders_nivel4 insertar = new stakeholders_nivel4();
                insertar.denominacion = oStakeholder.denominacion;
                insertar.idnivel3 = oStakeholder.idnivel3;
                insertar.requisitosrel = oStakeholder.requisitosrel;
                insertar.necesidades = oStakeholder.necesidades;
                insertar.idcentral = oStakeholder.idcentral;
                bd.stakeholders_nivel4.Add(insertar);
                bd.SaveChanges();

                return insertar.id;
            }
        }



        public static int ActualizarReferencial(referenciales oReferencial)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.referenciales
                              where u.id == oReferencial.id
                              select u).FirstOrDefault();

            if (oReferencial.id != 0 && actualizar != null)
            {
                actualizar.nombre = oReferencial.nombre;
                bd.SaveChanges();
                return actualizar.id;
            }
            else
            {
                referenciales insertar = new referenciales();
                insertar.nombre = oReferencial.nombre;
                bd.referenciales.Add(insertar);
                bd.SaveChanges();

                return insertar.id;
            }
        }

        public static int ActualizarTipoComunicacion(comunicacion_tipos oTipoCom)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.comunicacion_tipos
                              where u.id == oTipoCom.id
                              select u).FirstOrDefault();

            if (oTipoCom.id != 0 && actualizar != null)
            {
                actualizar.tipo = oTipoCom.tipo;
                bd.SaveChanges();
                return actualizar.id;
            }
            else
            {
                comunicacion_tipos insertar = new comunicacion_tipos();
                insertar.tipo = oTipoCom.tipo;
                bd.comunicacion_tipos.Add(insertar);
                bd.SaveChanges();

                return insertar.id;
            }
        }

        public static int ActualizarClasifComunicacion(comunicacion_clasificacion oTipoCom)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.comunicacion_clasificacion
                              where u.id == oTipoCom.id
                              select u).FirstOrDefault();

            if (oTipoCom.id != 0 && actualizar != null)
            {
                actualizar.tipo = oTipoCom.tipo;
                bd.SaveChanges();
                return actualizar.id;
            }
            else
            {
                comunicacion_clasificacion insertar = new comunicacion_clasificacion();
                insertar.tipo = oTipoCom.tipo;
                bd.comunicacion_clasificacion.Add(insertar);
                bd.SaveChanges();

                return insertar.id;
            }
        }

        public static int ActualizarCanalComunicacion(comunicacion_canales oTipoCom)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.comunicacion_canales
                              where u.id == oTipoCom.id
                              select u).FirstOrDefault();

            if (oTipoCom.id != 0 && actualizar != null)
            {
                actualizar.canal = oTipoCom.canal;
                bd.SaveChanges();
                return actualizar.id;
            }
            else
            {
                comunicacion_canales insertar = new comunicacion_canales();
                insertar.canal = oTipoCom.canal;
                bd.comunicacion_canales.Add(insertar);
                bd.SaveChanges();

                return insertar.id;
            }
        }

        public static int ActualizarTipoEventoAmbiental(evento_ambiental_tipo oTipoCom)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.evento_ambiental_tipo
                              where u.id == oTipoCom.id
                              select u).FirstOrDefault();

            if (oTipoCom.id != 0 && actualizar != null)
            {
                actualizar.tipo = oTipoCom.tipo;
                bd.SaveChanges();
                return actualizar.id;
            }
            else
            {
                evento_ambiental_tipo insertar = new evento_ambiental_tipo();
                insertar.tipo = oTipoCom.tipo;
                bd.evento_ambiental_tipo.Add(insertar);
                bd.SaveChanges();

                return insertar.id;
            }
        }

        public static int ActualizarCatriesgo(riesgos_categorias oTipoCom)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.riesgos_categorias
                              where u.id == oTipoCom.id
                              select u).FirstOrDefault();

            if (oTipoCom.id != 0 && actualizar != null)
            {
                actualizar.categoria = oTipoCom.categoria;
                bd.SaveChanges();
                return actualizar.id;
            }
            else
            {
                riesgos_categorias insertar = new riesgos_categorias();
                insertar.categoria = oTipoCom.categoria;
                bd.riesgos_categorias.Add(insertar);
                bd.SaveChanges();

                return insertar.id;
            }
        }

        public static int ActualizarTipriesgo(riesgos_tipologias oTipoCom)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.riesgos_tipologias
                              where u.id == oTipoCom.id
                              select u).FirstOrDefault();

            if (oTipoCom.id != 0 && actualizar != null)
            {
                actualizar.tipologia = oTipoCom.tipologia;
                bd.SaveChanges();
                return actualizar.id;
            }
            else
            {
                riesgos_tipologias insertar = new riesgos_tipologias();
                insertar.tipologia = oTipoCom.tipologia;
                bd.riesgos_tipologias.Add(insertar);
                bd.SaveChanges();

                return insertar.id;
            }
        }

        public static int ActualizarMatrizEventoAmbiental(evento_ambiental_matriz oTipoCom)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.evento_ambiental_matriz
                              where u.id == oTipoCom.id
                              select u).FirstOrDefault();

            if (oTipoCom.id != 0 && actualizar != null)
            {
                actualizar.matriz = oTipoCom.matriz;
                bd.SaveChanges();
                return actualizar.id;
            }
            else
            {
                evento_ambiental_matriz insertar = new evento_ambiental_matriz();
                insertar.matriz = oTipoCom.matriz;
                bd.evento_ambiental_matriz.Add(insertar);
                bd.SaveChanges();

                return insertar.id;
            }
        }

        public static int ActualizarAuditor(auditores oAuditor)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.auditores
                              where u.id == oAuditor.id
                              select u).FirstOrDefault();

            if (oAuditor.id != 0 && actualizar != null)
            {
                actualizar.nombre = oAuditor.nombre;
                actualizar.empresa = oAuditor.empresa;
                bd.SaveChanges();
                return actualizar.id;
            }
            else
            {
                auditores insertar = new auditores();
                insertar.nombre = oAuditor.nombre;
                insertar.empresa = oAuditor.empresa;
                bd.auditores.Add(insertar);
                bd.SaveChanges();

                return insertar.id;
            }
        }

        public static int ActualizarTipoAccMejora(tipo_accionesmejora oTipoAccMejora)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.tipo_accionesmejora
                              where u.id == oTipoAccMejora.id
                              select u).FirstOrDefault();

            if (oTipoAccMejora.id != 0 && actualizar != null)
            {
                actualizar.nombre = oTipoAccMejora.nombre;
                bd.SaveChanges();
                return actualizar.id;
            }
            else
            {
                tipo_accionesmejora insertar = new tipo_accionesmejora();
                insertar.nombre = oTipoAccMejora.nombre;
                bd.tipo_accionesmejora.Add(insertar);
                bd.SaveChanges();

                return insertar.id;
            }
        }

        public static int ActualizarEquipo(areanivel3 oEquipo)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.areanivel3
                              where u.id == oEquipo.id
                              select u).FirstOrDefault();

            if (oEquipo.id != 0 && actualizar != null)
            {
                actualizar.codigo = oEquipo.codigo;
                actualizar.nombre = oEquipo.nombre;
                bd.SaveChanges();
                return actualizar.id;
            }
            else
            {
                areanivel3 insertar = new areanivel3();
                insertar.id_areanivel2 = oEquipo.id_areanivel2;
                insertar.codigo = oEquipo.codigo;
                insertar.nombre = oEquipo.nombre;
                bd.areanivel3.Add(insertar);
                bd.SaveChanges();

                return insertar.id;
            }
        }
        public static int ActualizarNivelCuatro(areanivel4 oEquipo)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.areanivel4
                              where u.id == oEquipo.id
                              select u).FirstOrDefault();

            if (oEquipo.id != 0 && actualizar != null)
            {
                actualizar.codigo = oEquipo.codigo;
                actualizar.nombre = oEquipo.nombre;
                bd.SaveChanges();
                return actualizar.id;
            }
            else
            {
                areanivel4 insertar = new areanivel4();
                insertar.id_areanivel3 = oEquipo.id_areanivel3;
                insertar.codigo = oEquipo.codigo;
                insertar.nombre = oEquipo.nombre;
                bd.areanivel4.Add(insertar);
                bd.SaveChanges();

                return insertar.id;
            }
        }
        public static int ActualizarAmbito(ambitos oAmbito)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.ambitos
                              where u.id == oAmbito.id
                              select u).FirstOrDefault();

            if (oAmbito.id != 0 && actualizar != null)
            {
                actualizar.nombre_ambito = oAmbito.nombre_ambito;
                bd.SaveChanges();
                return actualizar.id;
            }
            else
            {
                ambitos insertar = new ambitos();
                insertar.nombre_ambito = oAmbito.nombre_ambito;
                bd.ambitos.Add(insertar);
                bd.SaveChanges();

                return insertar.id;
            }
        }

        public static bool AsociarParametroCentro(int idParametro, int idCentro)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.indicadores_hojadedatos_valores
                              where u.idcentral == idCentro && u.CodIndiHojaDatos == idParametro
                              select u).FirstOrDefault();

            indicadores_hojadedatos_valores insertarParaCen = new indicadores_hojadedatos_valores();
            if (actualizar == null)
            {
                insertarParaCen.CodIndiHojaDatos = idParametro;
                insertarParaCen.idcentral = idCentro;
                insertarParaCen.anio = 2018;

                bd.indicadores_hojadedatos_valores.Add(insertarParaCen);
                bd.SaveChanges();

                return true;
            }
            else
            {
                actualizar.baja = null;
                bd.SaveChanges();
            }

            return false;

        }

        public static bool AsociarUsuarioCentro(int idUsuario, int idCentro, bool Permiso)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.usuario_centros
                              where u.idcentro == idCentro && u.idusuario == idUsuario
                              select u).FirstOrDefault();

            usuario_centros insertarUsuCen = new usuario_centros();
            if (actualizar == null)
            {
                insertarUsuCen.idusuario = idUsuario;
                insertarUsuCen.idcentro = idCentro;
                insertarUsuCen.permiso = Permiso;

                bd.usuario_centros.Add(insertarUsuCen);
                bd.SaveChanges();

                return true;
            }

            return false;

        }

        public static bool AsociarAuditoriaReferencial(int idAuditoria, int idReferencial)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.auditorias_referenciales
                              where u.idReferencial == idReferencial && u.idAuditoria == idAuditoria
                              select u).FirstOrDefault();

            auditorias_referenciales insertarAudRef = new auditorias_referenciales();
            if (actualizar == null)
            {
                insertarAudRef.idAuditoria = idAuditoria;
                insertarAudRef.idReferencial = idReferencial;

                bd.auditorias_referenciales.Add(insertarAudRef);
                bd.SaveChanges();

                return true;
            }

            return false;

        }

        public static VISTA_UltimasMediciones obtenerUltimasMediciones(int idIndicador, int anio, int idCentral)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            VISTA_UltimasMediciones ultimaMedicion = (from u in bd.VISTA_UltimasMediciones
                                                      where u.IdIndicador == idIndicador && u.idcentral == idCentral && u.anio == anio
                                                      orderby u.anio descending
                                                      select u).FirstOrDefault();

            return ultimaMedicion;
        }

        public static void CrearPlanificacionIndicador(int idIndicador, centros centroseleccionado)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            indicadores_planificacion ultimaform = (from u in bd.indicadores_planificacion
                                                    where u.idCentral == centroseleccionado.id && u.anio == DateTime.Now.Year
                                                    orderby u.Id descending
                                                    select u).FirstOrDefault();





            indicadores_planificacion indic = new indicadores_planificacion();
            indic.IdIndicador = idIndicador;
            indic.idCentral = centroseleccionado.id;
            indic.anio = DateTime.Now.Year;


            bd.indicadores_planificacion.Add(indic);
            bd.SaveChanges();

            int idconsecutivo = 1;

            if (ultimaform != null && ultimaform.idconsecutivo != null)
                idconsecutivo = int.Parse(ultimaform.idconsecutivo.ToString()) + 1;

            indic.CodIndicador = centroseleccionado.siglas + "/" + idconsecutivo.ToString();
            indic.idconsecutivo = idconsecutivo;

            bd.SaveChanges();
        }

        public static void CrearParametroIndicador(int idParam, int anio, int idcentral)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            indicadores_hojadedatos_valores param = new indicadores_hojadedatos_valores();
            param.CodIndiHojaDatos = idParam;
            param.idcentral = idcentral;
            param.anio = anio;


            bd.indicadores_hojadedatos_valores.Add(param);
            bd.SaveChanges();
        }

        public static void CrearValoracionIndicador(int idIndicador, int anio, int idcentral)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            indicadores_imputacion param = new indicadores_imputacion();
            param.IdIndicador = idIndicador;
            param.idcentral = idcentral;
            param.anio = anio;


            bd.indicadores_imputacion.Add(param);
            bd.SaveChanges();
        }


        public static void CrearPlanificacionIndicador(int idPlanificacion, int idIndicador, int anio)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            indicadores_imputacion indic = new indicadores_imputacion();
            indic.IdIndicador = idIndicador;
            indic.IdPlanificacionIndicador = idPlanificacion;
            indic.anio = anio;

            bd.indicadores_imputacion.Add(indic);
            bd.SaveChanges();
        }

        public static void CrearValoracionAspecto(int idAspecto, int idCentral, int foco)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            aspecto_valoracion ultimoreq = (from u in bd.aspecto_valoracion
                                            where u.idCentral == idCentral
                                            && u.foco == foco
                                            orderby u.idconsecutivo descending
                                            select u).FirstOrDefault();

            aspecto_valoracion indic = new aspecto_valoracion();
            indic.idAspecto = idAspecto;
            indic.idCentral = idCentral;
            indic.foco = foco;
            bd.aspecto_valoracion.Add(indic);
            bd.SaveChanges();

            int idconsecutivo = 1;

            if (ultimoreq != null)
                idconsecutivo = ultimoreq.idconsecutivo + 1;

            centros cent = ObtenerCentroPorID(idCentral);

            if (foco == 1)
                indic.codigo = cent.siglas + "-F-" + idconsecutivo.ToString();
            else
                indic.codigo = cent.siglas + "-P-" + idconsecutivo.ToString();
            indic.idconsecutivo = idconsecutivo;

            bd.SaveChanges();
        }

        public static void CrearValoracionAspecto(int idAspecto, int idCentral, int foco, string nombrefoco, int continuo)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            aspecto_valoracion ultimoreq = (from u in bd.aspecto_valoracion
                                            where u.idCentral == idCentral
                                            && u.foco == foco
                                            orderby u.idconsecutivo descending
                                            select u).FirstOrDefault();

            aspecto_valoracion indic = new aspecto_valoracion();
            indic.idAspecto = idAspecto;
            indic.idCentral = idCentral;
            indic.nombrefoco = nombrefoco;
            indic.continuo = continuo;
            indic.foco = foco;
            bd.aspecto_valoracion.Add(indic);
            bd.SaveChanges();

            int idconsecutivo = 1;

            if (ultimoreq != null)
                idconsecutivo = ultimoreq.idconsecutivo + 1;

            centros cent = ObtenerCentroPorID(idCentral);

            if (foco == 1)
                indic.codigo = cent.siglas + "-F-" + idconsecutivo.ToString();
            else
                indic.codigo = cent.siglas + "-P-" + idconsecutivo.ToString();
            indic.idconsecutivo = idconsecutivo;

            bd.SaveChanges();
        }

        public static void CrearValoracionParametro(int idAspecto, int idParametro, string nombreParametro)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            aspecto_valoracion aspecto_val = new aspecto_valoracion();
            aspecto_val = (from u in bd.aspecto_valoracion
                           where u.id == idAspecto
                           select u).FirstOrDefault();

            aspecto_tipo aspecto_tipo = new aspecto_tipo();
            aspecto_tipo = (from u in bd.aspecto_tipo
                            where u.id == aspecto_val.idAspecto
                            select u).FirstOrDefault();

            aspecto_parametro_valoracion indic = new aspecto_parametro_valoracion();
            indic.id_aspecto = idAspecto;
            indic.id_parametro = idParametro;
            indic.nombre = nombreParametro;

            if (aspecto_tipo.Grupo == 6 || aspecto_tipo.Grupo == 13 || aspecto_tipo.Grupo == 14)
            {
                aspecto_parametro_valoracion param_val = (from u in bd.aspecto_parametro_valoracion
                                                          where u.id_aspecto == idAspecto
                                                          orderby u.id ascending
                                                          select u).FirstOrDefault();

                if (param_val != null)
                {
                    indic.magnitud = param_val.magnitud;
                    indic.origen = param_val.origen;

                    if (aspecto_tipo.Grupo == 13 || aspecto_tipo.Grupo == 14)
                    {
                        indic.RU_DiaRef = param_val.RU_DiaRef;
                        indic.RU_TardeRef = param_val.RU_TardeRef;
                        indic.RU_NocheRef = param_val.RU_NocheRef;
                        indic.RU_Dia = param_val.RU_Dia;
                        indic.RU_Tarde = param_val.RU_Tarde;
                        indic.RU_Noche = param_val.RU_Noche;
                    }
                }
            }

            bd.aspecto_parametro_valoracion.Add(indic);
            bd.SaveChanges();
        }

        public static bool AsociarAuditoriaObservador(int idAuditoria, int idObservador)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.auditorias_observador
                              where u.idUsuario == idObservador && u.idAuditoria == idAuditoria
                              select u).FirstOrDefault();

            auditorias_observador insertarAudObs = new auditorias_observador();
            if (actualizar == null)
            {
                insertarAudObs.idAuditoria = idAuditoria;
                insertarAudObs.idUsuario = idObservador;

                bd.auditorias_observador.Add(insertarAudObs);
                bd.SaveChanges();

                return true;
            }

            return false;

        }

        public static bool AsociarReunionParticipante(int idReunion, int idParticipante)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.reuniones_participantes
                              where u.idUsuario == idParticipante && u.idreunion == idReunion
                              select u).FirstOrDefault();

            reuniones_participantes insertarReuPar = new reuniones_participantes();
            if (actualizar == null)
            {
                insertarReuPar.idreunion = idReunion;
                insertarReuPar.idUsuario = idParticipante;

                bd.reuniones_participantes.Add(insertarReuPar);
                bd.SaveChanges();

                return true;
            }

            return false;

        }

        public static bool AgregarAuditoriaAuditor(int idAuditoria, int idAuditor)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            auditorias_auditores insertarAudObs = new auditorias_auditores();

            insertarAudObs.idAuditoria = idAuditoria;
            insertarAudObs.idAuditor = idAuditor;

            bd.auditorias_auditores.Add(insertarAudObs);
            bd.SaveChanges();

            return true;

        }

        public static auditores GetDatosAuditor(int idAuditor)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            auditores auditor = (from u in bd.auditores
                                 where u.id == idAuditor
                                 select u).FirstOrDefault();


            return auditor;

        }

        public static bool AsociarObjetivoCentro(int idObjetivo, int idCentro)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.objetivo_centrales
                              where u.idCentro == idCentro && u.idObjetivo == idObjetivo
                              select u).FirstOrDefault();

            objetivo_centrales insertarObjCen = new objetivo_centrales();
            if (actualizar == null)
            {
                insertarObjCen.idObjetivo = idObjetivo;
                insertarObjCen.idCentro = idCentro;

                bd.objetivo_centrales.Add(insertarObjCen);
                bd.SaveChanges();

                return true;
            }

            return false;

        }

        public static bool AsociarRiesgoStakeholder(int idRiesgo, int idStakeholder)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.riesgos_stakeholders
                              where u.idstakeholder == idStakeholder && u.idriesgo == idRiesgo
                              select u).FirstOrDefault();

            riesgos_stakeholders insertarRieSta = new riesgos_stakeholders();
            if (actualizar == null)
            {
                insertarRieSta.idriesgo = idRiesgo;
                insertarRieSta.idstakeholder = idStakeholder;

                bd.riesgos_stakeholders.Add(insertarRieSta);
                bd.SaveChanges();

                return true;
            }

            return false;

        }

        public static bool AsociarIndicadorCentro(int idIndicador, int idCentro)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.indicador_centrales
                              where u.idCentro == idCentro && u.idIndicador == idIndicador
                              select u).FirstOrDefault();

            indicador_centrales insertarObjCen = new indicador_centrales();
            if (actualizar == null)
            {
                insertarObjCen.idIndicador = idIndicador;
                insertarObjCen.idCentro = idCentro;

                bd.indicador_centrales.Add(insertarObjCen);
                bd.SaveChanges();

                return true;
            }

            return false;

        }

        public static bool AsociarAccMejoraReferencial(int idAccMejora, int idRef)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.accionmejora_referencial
                              where u.idaccionmejora == idAccMejora && u.idreferencial == idRef
                              select u).FirstOrDefault();

            accionmejora_referencial insertarAccRef = new accionmejora_referencial();
            if (actualizar == null)
            {
                insertarAccRef.idaccionmejora = idAccMejora;
                insertarAccRef.idreferencial = idRef;

                bd.accionmejora_referencial.Add(insertarAccRef);
                bd.SaveChanges();

                return true;
            }

            return false;

        }


        public static bool AsociarComunicacionProceso(int idComunicacion, int idProceso)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.comunicacion_procesos
                              where u.idproceso == idProceso && u.idcomunicacion == idComunicacion
                              select u).FirstOrDefault();

            comunicacion_procesos insertarComProc = new comunicacion_procesos();
            if (actualizar == null)
            {
                insertarComProc.idcomunicacion = idComunicacion;
                insertarComProc.idproceso = idProceso;

                bd.comunicacion_procesos.Add(insertarComProc);
                bd.SaveChanges();

                return true;
            }

            return false;

        }

        public static bool AsociarObjetivoTecnologia(int idObjetivo, int idTecnologia)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.objetivo_tecnologias
                              where u.idTecnologia == idTecnologia && u.idObjetivo == idObjetivo
                              select u).FirstOrDefault();

            objetivo_tecnologias insertarObjTec = new objetivo_tecnologias();
            if (actualizar == null)
            {
                insertarObjTec.idObjetivo = idObjetivo;
                insertarObjTec.idTecnologia = idTecnologia;

                bd.objetivo_tecnologias.Add(insertarObjTec);
                bd.SaveChanges();

                return true;
            }

            return false;

        }

        public static bool InsertarProceso(procesos insertar)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            //var actualizar = (from u in bd.procesos
            //                  where u.organizacion == insertar.organizacion
            //                  select u).OrderByDescending(x => x.cod_proceso).FirstOrDefault();

            procesos insertarPro = new procesos();
            if (insertar.id == 0)
            {
                insertarPro.nivel = insertar.nivel;
                insertarPro.tipo = insertar.tipo;
                insertarPro.padre = insertar.padre;
                insertarPro.nombre = insertar.nombre;
                insertarPro.organizacion = insertar.organizacion;
                insertarPro.cod_proceso = insertar.cod_proceso;
                insertarPro.fedicion = insertar.fedicion;
                insertarPro.tecnologia = insertar.tecnologia;
                bd.procesos.Add(insertarPro);
                bd.SaveChanges();

                return true;
            }

            return false;

        }

        public static bool InsertarObjetivo(objetivos insertar)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            objetivos insertarObj = new objetivos();
            if (insertar.id == 0)
            {
                insertarObj.Tipo = insertar.Tipo;
                insertarObj.Nombre = insertar.Nombre;
                insertarObj.idorganizacion = insertar.idorganizacion;
                bd.objetivos.Add(insertarObj);
                bd.SaveChanges();

                return true;
            }

            return false;

        }

        public static bool ActualizarProceso(procesos oProceso)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.procesos
                              where u.id == oProceso.id
                              select u).FirstOrDefault();

            actualizar.cod_proceso = oProceso.cod_proceso;
            actualizar.nombre = oProceso.nombre;
            actualizar.descripcion = oProceso.descripcion;
            actualizar.objetivos = oProceso.objetivos;
            actualizar.alcance = oProceso.alcance;
            actualizar.tecnologia = oProceso.tecnologia;

            bd.SaveChanges();

            return true;
        }

        public static int ActualizarComunicacion(comunicacion oComunicacion)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            if (oComunicacion.id != 0)
            {

                var actualizar = (from u in bd.comunicacion
                                  where u.id == oComunicacion.id
                                  select u).FirstOrDefault();

                actualizar.idcomunicacion = oComunicacion.idcomunicacion;
                actualizar.clasificacion = oComunicacion.clasificacion;
                actualizar.tipo = oComunicacion.tipo;
                actualizar.stakeholder = oComunicacion.stakeholder;
                actualizar.fechainicio = oComunicacion.fechainicio;
                actualizar.canal = oComunicacion.canal;
                actualizar.asunto = oComunicacion.asunto;
                actualizar.remitente = oComunicacion.remitente;
                actualizar.fechafin = oComunicacion.fechafin;
                actualizar.descripcion = oComunicacion.descripcion;
                actualizar.descripcionres = oComunicacion.descripcionres;
                bd.SaveChanges();
                return actualizar.id;
            }
            else
            {
                comunicacion ultimoobj = (from u in bd.comunicacion
                                          where u.idcentral == oComunicacion.idcentral
                                          orderby u.idconsecutivo descending
                                          select u).FirstOrDefault();

                bd.comunicacion.Add(oComunicacion);
                bd.SaveChanges();
                centros cent = ObtenerCentroPorID(oComunicacion.idcentral);
                if (oComunicacion.idcentral != 0)
                {
                    oComunicacion.idcomunicacion = cent.siglas + "/";
                }
                else
                {
                    oComunicacion.idcomunicacion = "GTI" + "/";
                }

                int idconsecutivo = 1;

                if (ultimoobj != null)
                    idconsecutivo = ultimoobj.idconsecutivo + 1;

                oComunicacion.idcomunicacion = oComunicacion.idcomunicacion + idconsecutivo.ToString() + "/" + DateTime.Now.Year.ToString();
                oComunicacion.idconsecutivo = idconsecutivo;

                bd.SaveChanges();

                return oComunicacion.id;
            }


        }

        public static int ActualizarParte(partes oParte)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            if (oParte.id != 0)
            {

                var actualizar = (from u in bd.partes
                                  where u.id == oParte.id
                                  select u).FirstOrDefault();

                actualizar.empresa = oParte.empresa;
                actualizar.instalacion = oParte.instalacion;
                actualizar.trabajo = oParte.trabajo;
                actualizar.detalle = oParte.detalle;
                actualizar.accionescorrectoras = oParte.accionescorrectoras;
                actualizar.accionesprevistas = oParte.accionesprevistas;
                actualizar.cumplimentadopor = oParte.cumplimentadopor;
                actualizar.cumplimentadofecha = oParte.cumplimentadofecha;
                actualizar.entregadopor = oParte.entregadopor;
                actualizar.entregadofecha = oParte.entregadofecha;
                actualizar.recibidounidadorg = oParte.recibidounidadorg;
                actualizar.recibidofecha = oParte.recibidofecha;
                actualizar.resueltopor = oParte.resueltopor;
                actualizar.resueltofecha = oParte.resueltofecha;
                actualizar.observaciones = oParte.observaciones;
                actualizar.desest = oParte.desest;
                actualizar.desestfecha = oParte.desestfecha;
                actualizar.asunto = oParte.asunto;
                bd.SaveChanges();
                return actualizar.id;
            }
            else
            {
                partes ultimoobj = (from u in bd.partes
                                    where u.idcentral == oParte.idcentral
                                    orderby u.idconsecutivo descending
                                    select u).FirstOrDefault();

                bd.partes.Add(oParte);
                bd.SaveChanges();
                centros cent = ObtenerCentroPorID(oParte.idcentral);
                if (oParte.idcentral != 0)
                {
                    oParte.idcomunicacion = cent.siglas + "/";
                }
                else
                {
                    oParte.idcomunicacion = "GTI" + "/";
                }

                int idconsecutivo = 1;

                if (ultimoobj != null)
                    idconsecutivo = ultimoobj.idconsecutivo + 1;

                oParte.idcomunicacion = oParte.idcomunicacion + idconsecutivo.ToString() + "/" + DateTime.Now.Year.ToString();
                oParte.idconsecutivo = idconsecutivo;

                bd.SaveChanges();

                return oParte.id;
            }


        }


        public static int ActualizarReunion(reuniones oReunion)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            if (oReunion.id != 0)
            {

                var actualizar = (from u in bd.reuniones
                                  where u.id == oReunion.id
                                  select u).FirstOrDefault();

                actualizar.fecha_convocatoria = oReunion.fecha_convocatoria;
                actualizar.horainicio = oReunion.horainicio;
                actualizar.horafin = oReunion.horafin;
                actualizar.estado = oReunion.estado;
                actualizar.ordendeldia = oReunion.ordendeldia;
                actualizar.resumen = oReunion.resumen;
                actualizar.personasinv = oReunion.personasinv;
                actualizar.asunto = oReunion.asunto;

                bd.SaveChanges();
                return actualizar.id;
            }
            else
            {
                reuniones ultimoobj = (from u in bd.reuniones
                                       where u.idcentral == oReunion.idcentral
                                       orderby u.idconsecutivo descending
                                       select u).FirstOrDefault();

                bd.reuniones.Add(oReunion);
                bd.SaveChanges();
                centros cent = ObtenerCentroPorID(oReunion.idcentral);

                oReunion.cod_reunion = cent.siglas + "/";

                int idconsecutivo = 1;

                if (ultimoobj != null)
                    idconsecutivo = ultimoobj.idconsecutivo + 1;

                oReunion.cod_reunion = oReunion.cod_reunion + idconsecutivo.ToString() + "/" + DateTime.Now.Year.ToString();
                oReunion.idconsecutivo = idconsecutivo;

                bd.SaveChanges();

                return oReunion.id;
            }


        }

        public static int ActualizarEventoAmb(evento_ambiental oEvento)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            if (oEvento.id != 0)
            {

                var actualizar = (from u in bd.evento_ambiental
                                  where u.id == oEvento.id
                                  select u).FirstOrDefault();

                actualizar.fechaevento = oEvento.fechaevento;
                actualizar.tipo = oEvento.tipo;
                actualizar.matrizprincipal = oEvento.matrizprincipal;
                actualizar.matrizsecundaria = oEvento.matrizsecundaria;
                actualizar.unidadnegocio = oEvento.unidadnegocio;
                actualizar.companiainvolucrada = oEvento.companiainvolucrada;
                actualizar.empresacontratista = oEvento.empresacontratista;

                if (actualizar.tipo == 1 || actualizar.tipo == 2 || actualizar.tipo == 3)
                {
                    actualizar.claseevento_sec2 = oEvento.claseevento_sec2;
                    actualizar.extension_sec2 = oEvento.extension_sec2;
                    actualizar.impacto_sec2 = oEvento.impacto_sec2;
                    actualizar.localizacion_sec2 = oEvento.localizacion_sec2;
                    actualizar.descripcion_sec2 = oEvento.descripcion_sec2;
                    actualizar.causa_sec2 = oEvento.causa_sec2;
                    actualizar.accionesinmediatas_sec2 = oEvento.accionesinmediatas_sec2;
                    actualizar.infoadicional = oEvento.infoadicional;
                }

                if (actualizar.tipo == 4 || actualizar.tipo == 5)
                {
                    actualizar.descripcion_sec3 = oEvento.descripcion_sec3;
                    actualizar.demandante_sec3 = oEvento.demandante_sec3;
                    actualizar.tipodemantante_sec3 = oEvento.tipodemantante_sec3;
                    actualizar.tipocriticidad_sec3 = oEvento.tipocriticidad_sec3;
                    actualizar.infoadicional = oEvento.infoadicional;
                }

                bd.SaveChanges();
                return actualizar.id;
            }
            else
            {
                bd.evento_ambiental.Add(oEvento);
                bd.SaveChanges();

                return oEvento.id;
            }


        }

        public static int ActualizarEventoSeg(evento_seguridad oEvento)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            if (oEvento.id != 0)
            {

                var actualizar = (from u in bd.evento_seguridad
                                  where u.id == oEvento.id
                                  select u).FirstOrDefault();

                actualizar.fecha = oEvento.fecha;
                actualizar.tipo = oEvento.tipo;
                actualizar.severidad = oEvento.severidad;
                actualizar.personalafectado = oEvento.personalafectado;
                actualizar.organizacion = oEvento.organizacion;
                actualizar.hora = oEvento.hora;
                actualizar.unidadnegocio = oEvento.unidadnegocio;
                actualizar.idcentral = oEvento.idcentral;
                actualizar.compania = oEvento.compania;
                actualizar.baja = oEvento.baja;
                actualizar.fechabaja = oEvento.fechabaja;
                actualizar.horaaccidente = oEvento.horaaccidente;

                actualizar.ie_region = oEvento.ie_region;
                actualizar.ie_localizacion = oEvento.ie_localizacion;
                actualizar.ie_descripcion = oEvento.ie_descripcion;

                actualizar.pa_nombre = oEvento.pa_nombre;
                actualizar.pa_apellido = oEvento.pa_apellido;
                actualizar.pa_genero = oEvento.pa_genero;
                actualizar.pa_edad = oEvento.pa_edad;
                actualizar.pa_puesto = oEvento.pa_puesto;
                actualizar.pa_nacionalidad = oEvento.pa_nacionalidad;
                actualizar.pa_antiguedadempresa = oEvento.pa_antiguedadempresa;
                actualizar.pa_antiguedadcategoria = oEvento.pa_antiguedadcategoria;
                actualizar.pa_tipocontrato = oEvento.pa_tipocontrato;
                actualizar.pa_accidentesant = oEvento.pa_accidentesant;
                actualizar.pa_numacc = oEvento.pa_numacc;
                actualizar.pa_diasbaja = oEvento.pa_diasbaja;
                actualizar.pa_fechamodformacion = oEvento.pa_fechamodformacion;
                actualizar.pa_nombreultformacion = oEvento.pa_nombreultformacion;

                actualizar.id_informemedico = oEvento.id_informemedico;
                actualizar.id_diasconv_primerpron = oEvento.id_diasconv_primerpron;
                actualizar.id_reqasistencia = oEvento.id_reqasistencia;
                actualizar.id_asistenciaen = oEvento.id_asistenciaen;
                actualizar.id_personalsanitario = oEvento.id_personalsanitario;
                actualizar.id_naturalezalesion = oEvento.id_naturalezalesion;
                actualizar.id_localizacionanatomica = oEvento.id_localizacionanatomica;
                actualizar.id_agentelesion = oEvento.id_agentelesion;
                actualizar.id_envmutua = oEvento.id_envmutua;
                actualizar.id_mutua = oEvento.id_mutua;
                actualizar.id_localidadmutua = oEvento.id_localidadmutua;
                actualizar.id_envcs = oEvento.id_envcs;
                actualizar.id_centrosanitario = oEvento.id_centrosanitario;
                actualizar.id_localidadcs = oEvento.id_localidadcs;
                actualizar.id_mandodirecto = oEvento.id_mandodirecto;
                actualizar.id_testigo1 = oEvento.id_testigo1;
                actualizar.id_testigo2 = oEvento.id_testigo2;
                actualizar.id_testigo3 = oEvento.id_testigo3;

                actualizar.te_tipo = oEvento.te_tipo;
                actualizar.te_subtipo = oEvento.te_subtipo;
                actualizar.te_categorizacion = oEvento.te_categorizacion;
                actualizar.te_causa = oEvento.te_causa;
                actualizar.te_accionesinm = oEvento.te_accionesinm;

                actualizar.ic_nombreempresa = oEvento.ic_nombreempresa;
                actualizar.ic_actividad = oEvento.ic_actividad;
                actualizar.ic_personaref = oEvento.ic_personaref;
                actualizar.ic_telefono = oEvento.ic_telefono;
                actualizar.ic_email = oEvento.ic_email;
                actualizar.ic_empcontratista = oEvento.ic_empcontratista;
                actualizar.ic_subcontrata = oEvento.ic_subcontrata;
                actualizar.ic_personalsanit = oEvento.ic_personalsanit;
                actualizar.ic_domicilio = oEvento.ic_domicilio;
                actualizar.ic_cif = oEvento.ic_cif;
                actualizar.ic_localidad = oEvento.ic_localidad;

                actualizar.ia_horario = oEvento.ia_horario;
                actualizar.ia_desde = oEvento.ia_desde;
                actualizar.ia_hacia = oEvento.ia_hacia;
                actualizar.ia_lugar = oEvento.ia_lugar;
                actualizar.ia_medio = oEvento.ia_medio;
                actualizar.ia_propiedadmediotransporte = oEvento.ia_propiedadmediotransporte;
                actualizar.ia_causa = oEvento.ia_causa;

                bd.SaveChanges();
                return actualizar.id;
            }
            else
            {
                bd.evento_seguridad.Add(oEvento);
                bd.SaveChanges();

                return oEvento.id;
            }


        }

        public static int ActualizarEventoCal(evento_calidad oEvento)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            if (oEvento.id != 0)
            {

                var actualizar = (from u in bd.evento_calidad
                                  where u.id == oEvento.id
                                  select u).FirstOrDefault();

                actualizar.asunto = oEvento.asunto;
                actualizar.compania = oEvento.compania;
                actualizar.pais = oEvento.pais;
                actualizar.tecnologia = oEvento.tecnologia;
                actualizar.idcentral = oEvento.idcentral;
                actualizar.unidad = oEvento.unidad;
                actualizar.fechacomienzo = oEvento.fechacomienzo;
                actualizar.fechafin = oEvento.fechafin;
                actualizar.evento = oEvento.evento;
                actualizar.descripcion = oEvento.descripcion;
                actualizar.impacto = oEvento.impacto;
                actualizar.cargo = oEvento.cargo;
                actualizar.persona = oEvento.persona;

                bd.SaveChanges();
                return actualizar.id;
            }
            else
            {
                bd.evento_calidad.Add(oEvento);
                bd.SaveChanges();

                return oEvento.id;
            }


        }

        public static int ActualizarObjetivo(objetivos oObjetivo)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            if (oObjetivo.id != 0)
            {

                var actualizar = (from u in bd.objetivos
                                  where u.id == oObjetivo.id
                                  select u).FirstOrDefault();

                actualizar.Nombre = oObjetivo.Nombre;
                actualizar.Descripcion = oObjetivo.Descripcion;
                actualizar.FechaEstimada = oObjetivo.FechaEstimada;
                actualizar.FechaReal = oObjetivo.FechaReal;
                actualizar.Responsable = oObjetivo.Responsable;
                actualizar.metodomedicion = oObjetivo.metodomedicion;
                actualizar.idAspecto = oObjetivo.idAspecto;
                actualizar.Coste = oObjetivo.Coste;
                actualizar.Medios = oObjetivo.Medios;
                actualizar.Seguimiento = oObjetivo.Seguimiento;
                actualizar.especifico = oObjetivo.especifico;
                actualizar.estado = oObjetivo.estado;
                actualizar.ambito = oObjetivo.ambito;
                actualizar.personasinv = oObjetivo.personasinv;
                //actualizar.Comentarios = oObjetivo.Comentarios;
                bd.SaveChanges();
                return actualizar.id;
            }
            else
            {
                string anio = DateTime.Now.Year.ToString();
                objetivos ultimoobj = (from u in bd.objetivos
                                       where u.idorganizacion == oObjetivo.idorganizacion && u.Tipo == oObjetivo.Tipo
                                       && u.Codigo.Contains(anio)
                                       orderby u.idconsecutivo descending
                                       select u).FirstOrDefault();

                bd.objetivos.Add(oObjetivo);
                bd.SaveChanges();


                centros cent = ObtenerCentroPorID(oObjetivo.idorganizacion);
                if (oObjetivo.idorganizacion != 0)
                {
                    oObjetivo.Codigo = cent.siglas + "/";
                }
                else
                {
                    oObjetivo.Codigo = "SPM" + "/";
                }

                int idconsecutivo = 1;

                if (ultimoobj != null)
                    idconsecutivo = ultimoobj.idconsecutivo + 1;

                oObjetivo.Codigo = oObjetivo.Codigo + idconsecutivo.ToString() + "/" + DateTime.Now.Year.ToString();
                oObjetivo.idconsecutivo = idconsecutivo;

                bd.SaveChanges();


                return oObjetivo.id;
            }


        }

        public static int ActualizarFormacion(formacion oFormacion)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            if (oFormacion.id != 0)
            {

                var actualizar = (from u in bd.formacion
                                  where u.id == oFormacion.id
                                  select u).FirstOrDefault();

                actualizar.denominacion = oFormacion.denominacion;

                actualizar.analisiscausasnorealiz = oFormacion.analisiscausasnorealiz;
                actualizar.analisiscausasnoefectivas = oFormacion.analisiscausasnoefectivas;
                actualizar.observaciones = oFormacion.observaciones;

                actualizar.valoracion_eficacia = oFormacion.valoracion_eficacia;
                actualizar.fecha_registro_inicio = oFormacion.fecha_registro_inicio;
                actualizar.fecha_registro_ejecutado = oFormacion.fecha_registro_ejecutado;

                actualizar.actividadesplanificadas = oFormacion.actividadesplanificadas;
                actualizar.actividadesejecutadas = oFormacion.actividadesejecutadas;
                actualizar.horascalidad = oFormacion.horascalidad;
                actualizar.horasmedioambiente = oFormacion.horasmedioambiente;
                actualizar.horasseguridadsalud = oFormacion.horasseguridadsalud;
                actualizar.horasotrasareas = oFormacion.horasotrasareas;

                bd.SaveChanges();
                return actualizar.id;
            }
            else
            {
                formacion ultimaform = (from u in bd.formacion
                                        where u.anio == oFormacion.anio
                                        && u.idcentral == oFormacion.idcentral
                                        orderby u.idconsecutivo descending
                                        select u).FirstOrDefault();

                oFormacion.codigo = string.Empty;
                bd.formacion.Add(oFormacion);
                bd.SaveChanges();

                int idconsecutivo = 1;

                if (ultimaform != null)
                    idconsecutivo = ultimaform.idconsecutivo + 1;

                oFormacion.codigo = DateTime.Now.Year.ToString() + "-" + idconsecutivo.ToString();
                oFormacion.idconsecutivo = idconsecutivo;

                bd.SaveChanges();

                return oFormacion.id;
            }


        }

        public static int ActualizarRiesgo(Riesgos oRiesgo)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            if (oRiesgo.Id != 0)
            {

                var actualizar = (from u in bd.Riesgos
                                  where u.Id == oRiesgo.Id
                                  select u).FirstOrDefault();

                actualizar.Tipo = oRiesgo.Tipo;
                actualizar.idCadenaValor = oRiesgo.idCadenaValor;
                actualizar.idMacroproceso = oRiesgo.idMacroproceso;
                actualizar.idProceso = oRiesgo.idProceso;
                actualizar.Descripcion = oRiesgo.Descripcion;
                actualizar.Categoria = oRiesgo.Categoria;
                actualizar.Tipologia = oRiesgo.Tipologia;
                actualizar.fechaCreacion = oRiesgo.fechaCreacion;
                actualizar.fechaModificacion = oRiesgo.fechaModificacion;
                actualizar.vigente = oRiesgo.vigente;

                actualizar.RI_ProbabilidadOcurrencia = oRiesgo.RI_ProbabilidadOcurrencia;
                actualizar.RI_ImpactoObjetivos = oRiesgo.RI_ImpactoObjetivos;
                actualizar.RI_ImpactoEconomico = oRiesgo.RI_ImpactoEconomico;
                actualizar.RI_ImpactoProcesosNegocio = oRiesgo.RI_ImpactoProcesosNegocio;
                actualizar.RI_ImpactoReputacional = oRiesgo.RI_ImpactoReputacional;
                actualizar.RI_ImpactoCumplimiento = oRiesgo.RI_ImpactoCumplimiento;
                actualizar.RI_ImpactoGeneral = oRiesgo.RI_ImpactoGeneral;
                actualizar.RI_RelevanciaRiesgo = oRiesgo.RI_RelevanciaRiesgo;
                actualizar.RI_ValorRelevanciaRiesgo = oRiesgo.RI_ValorRelevanciaRiesgo;
                actualizar.RI_GestionRiesgo = oRiesgo.RI_GestionRiesgo;

                actualizar.LimitOOE = oRiesgo.LimitOOE;
                actualizar.LimitOO = oRiesgo.LimitOO;
                actualizar.LimitE = oRiesgo.LimitE;
                actualizar.SinLimit = oRiesgo.SinLimit;
                actualizar.SinEfectos = oRiesgo.SinEfectos;
                actualizar.EfectD = oRiesgo.EfectD;
                actualizar.EfectDI = oRiesgo.EfectDI;
                actualizar.ValoracionOportunidad = oRiesgo.ValoracionOportunidad;
                actualizar.GestionOportunidad = oRiesgo.GestionOportunidad;

                actualizar.DescripcionControl = oRiesgo.DescripcionControl;
                actualizar.PropietarioControl = oRiesgo.PropietarioControl;

                actualizar.RR_ProbabilidadOcurrencia = oRiesgo.RR_ProbabilidadOcurrencia;
                actualizar.RR_ImpactoObjetivos = oRiesgo.RR_ImpactoObjetivos;
                actualizar.RR_ImpactoEconomico = oRiesgo.RR_ImpactoEconomico;
                actualizar.RR_ImpactoProcesosNegocio = oRiesgo.RR_ImpactoProcesosNegocio;
                actualizar.RR_ImpactoReputacional = oRiesgo.RR_ImpactoReputacional;
                actualizar.RR_ImpactoCumplimiento = oRiesgo.RR_ImpactoCumplimiento;
                actualizar.RR_ImpactoGeneral = oRiesgo.RR_ImpactoGeneral;
                actualizar.RR_RelevanciaRiesgo = oRiesgo.RR_RelevanciaRiesgo;
                actualizar.RR_ValorRelevanciaRiesgo = oRiesgo.RR_ValorRelevanciaRiesgo;


                bd.SaveChanges();
                return actualizar.Id;
            }
            else
            {
                Riesgos ultimaform = (from u in bd.Riesgos
                                      where u.idCentral == oRiesgo.idCentral
                                      orderby u.Id descending
                                      select u).FirstOrDefault();

                oRiesgo.CodigoRiesgo = string.Empty;
                bd.Riesgos.Add(oRiesgo);
                bd.SaveChanges();


                centros cent = ObtenerCentroPorID(int.Parse(oRiesgo.idCentral.ToString()));
                if (oRiesgo.idCentral != 0)
                {
                    oRiesgo.CodigoRiesgo = cent.siglas + "-";
                }
                else
                {
                    oRiesgo.CodigoRiesgo = "GTI" + "-";
                }

                int idconsecutivo = 1;

                if (ultimaform != null)
                    idconsecutivo = int.Parse(ultimaform.idconsecutivo.ToString()) + 1;

                oRiesgo.CodigoRiesgo = oRiesgo.CodigoRiesgo + idconsecutivo.ToString();
                oRiesgo.idconsecutivo = idconsecutivo;

                bd.SaveChanges();




                return oRiesgo.Id;
            }


        }

        public static int ActualizarEmergencia(emergencias oEmergencia)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            if (oEmergencia.id != 0)
            {

                var actualizar = (from u in bd.emergencias
                                  where u.id == oEmergencia.id
                                  select u).FirstOrDefault();

                actualizar.descripcion = oEmergencia.descripcion;
                actualizar.responsable = oEmergencia.responsable;
                actualizar.fechaplanificada = oEmergencia.fechaplanificada;
                actualizar.fecharealizacion = oEmergencia.fecharealizacion;

                actualizar.personalimplicado = oEmergencia.personalimplicado;
                actualizar.mediosempleados = oEmergencia.mediosempleados;
                actualizar.escenarioplanteado = oEmergencia.escenarioplanteado;
                actualizar.objetivos = oEmergencia.objetivos;
                actualizar.conclusiones = oEmergencia.conclusiones;

                bd.SaveChanges();
                return actualizar.id;
            }
            else
            {
                emergencias ultimaform = (from u in bd.emergencias
                                          where u.anio == oEmergencia.anio
                                          && u.idcentral == oEmergencia.idcentral
                                          orderby u.idconsecutivo descending
                                          select u).FirstOrDefault();

                oEmergencia.codigo = string.Empty;
                bd.emergencias.Add(oEmergencia);
                bd.SaveChanges();

                int idconsecutivo = 1;

                if (ultimaform != null)
                    idconsecutivo = ultimaform.idconsecutivo + 1;

                oEmergencia.codigo = DateTime.Now.Year.ToString() + "-" + idconsecutivo.ToString();
                oEmergencia.idconsecutivo = idconsecutivo;

                bd.SaveChanges();

                return oEmergencia.id;
            }


        }

        public static int ActualizarNorma(normas oNorma)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            if (oNorma.id != 0)
            {

                var actualizar = (from u in bd.normas
                                  where u.id == oNorma.id
                                  select u).FirstOrDefault();

                actualizar.codigo = oNorma.codigo;
                actualizar.nombre_norma = oNorma.nombre_norma;
                actualizar.edicion_norma = oNorma.edicion_norma;

                bd.SaveChanges();
                return actualizar.id;
            }
            else
            {

                oNorma.descargas = 0;
                bd.normas.Add(oNorma);
                bd.SaveChanges();

                return oNorma.id;
            }


        }

        public static int ActualizarEnlace(enlaces oEnlace)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            if (oEnlace.id != 0)
            {

                var actualizar = (from u in bd.enlaces
                                  where u.id == oEnlace.id
                                  select u).FirstOrDefault();

                actualizar.titulo = oEnlace.titulo;
                actualizar.url = oEnlace.url;
                actualizar.ambito = oEnlace.ambito;

                bd.SaveChanges();
                return actualizar.id;
            }
            else
            {

                bd.enlaces.Add(oEnlace);
                bd.SaveChanges();

                return oEnlace.id;
            }


        }

        public static int ActualizarMaterial(materialdivulgativo oMaterial)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            if (oMaterial.id != 0)
            {
                var actualizar = (from u in bd.materialdivulgativo
                                  where u.id == oMaterial.id
                                  select u).FirstOrDefault();

                actualizar.codigo = oMaterial.codigo;
                actualizar.titulo = oMaterial.titulo;
                actualizar.tipodoc = oMaterial.tipodoc;
                actualizar.fechapub = oMaterial.fechapub;
                actualizar.riesgoasoc = oMaterial.riesgoasoc;
                actualizar.idcentral = oMaterial.idcentral;

                bd.SaveChanges();
                return actualizar.id;
            }
            else
            {

                oMaterial.descargas = 0;
                bd.materialdivulgativo.Add(oMaterial);
                bd.SaveChanges();

                return oMaterial.id;
            }
        }

        public static int ActualizarEvaluacionRiesgo(evaluacionriesgos oEvaluacion)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            if (oEvaluacion.id != 0)
            {
                var actualizar = (from u in bd.evaluacionriesgos
                                  where u.id == oEvaluacion.id
                                  select u).FirstOrDefault();

                actualizar.codigo = oEvaluacion.codigo;
                actualizar.titulo = oEvaluacion.titulo;
                actualizar.tipodoc = oEvaluacion.tipodoc;
                actualizar.fechapub = oEvaluacion.fechapub;
                actualizar.elaboradopor = oEvaluacion.elaboradopor;
                actualizar.empresa = oEvaluacion.empresa;
                actualizar.idcentral = oEvaluacion.idcentral;

                bd.SaveChanges();
                return actualizar.id;
            }
            else
            {
                oEvaluacion.descargas = 0;
                bd.evaluacionriesgos.Add(oEvaluacion);
                bd.SaveChanges();

                return oEvaluacion.id;
            }
        }

        public static int ActualizarInforme(informesseguridad oInforme)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            if (oInforme.id != 0)
            {
                var actualizar = (from u in bd.informesseguridad
                                  where u.id == oInforme.id
                                  select u).FirstOrDefault();

                actualizar.codigo = oInforme.codigo;
                actualizar.titulo = oInforme.titulo;
                actualizar.tipodoc = oInforme.tipodoc;
                actualizar.fechapub = oInforme.fechapub;
                actualizar.elaboradopor = oInforme.elaboradopor;
                actualizar.mes = oInforme.mes;
                actualizar.anio = oInforme.anio;

                bd.SaveChanges();
                return actualizar.id;
            }
            else
            {
                oInforme.descargas = 0;
                bd.informesseguridad.Add(oInforme);
                bd.SaveChanges();

                return oInforme.id;
            }
        }

        public static int ActualizarAccionMejora(accionesmejora oAccion)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            if (oAccion.id != 0)
            {

                var actualizar = (from u in bd.accionesmejora
                                  where u.id == oAccion.id
                                  select u).FirstOrDefault();

                actualizar.asunto = oAccion.asunto;
                actualizar.tipo = oAccion.tipo;
                actualizar.fecha_apertura = oAccion.fecha_apertura;
                actualizar.fecha_cierre = oAccion.fecha_cierre;
                actualizar.ambito = oAccion.ambito;
                actualizar.responsable = oAccion.responsable;
                actualizar.estado = oAccion.estado;
                actualizar.proceso = oAccion.proceso;
                actualizar.antecedente = oAccion.antecedente;
                actualizar.detectadopor = oAccion.detectadopor;
                actualizar.causas = oAccion.causas;
                actualizar.descripcion = oAccion.descripcion;
                actualizar.personasinv = oAccion.personasinv;
                if (actualizar.antecedente == 0)
                {
                    actualizar.referencianoconforme = oAccion.referencianoconforme;
                }
                else
                {
                    actualizar.referencia = oAccion.referencia;
                }
                actualizar.especifico = oAccion.especifico;
                actualizar.ai_descripcion = oAccion.ai_descripcion;
                actualizar.ai_responsable = oAccion.ai_responsable;
                actualizar.ai_ffin_prevista = oAccion.ai_ffin_prevista;
                actualizar.ai_fcierre = oAccion.ai_fcierre;
                actualizar.ai_estado = oAccion.ai_estado;
                actualizar.ai_comentario = oAccion.ai_comentario;
                actualizar.contratista = oAccion.contratista;

                bd.SaveChanges();
                return actualizar.id;
            }
            else
            {
                accionesmejora ultimaform = (from u in bd.accionesmejora
                                             where u.anio == oAccion.anio
                                          && u.idcentral == oAccion.idcentral
                                             orderby u.idconsecutivo descending
                                             select u).FirstOrDefault();

                oAccion.codigo = string.Empty;
                bd.accionesmejora.Add(oAccion);
                bd.SaveChanges();

                int idconsecutivo = 1;

                if (ultimaform != null)
                    idconsecutivo = ultimaform.idconsecutivo + 1;

                oAccion.codigo = oAccion.anio.ToString() + "-" + idconsecutivo.ToString();
                oAccion.idconsecutivo = idconsecutivo;

                bd.SaveChanges();

                return oAccion.id;
            }


        }

        public static int ActualizarSatisfaccion(satisfaccion oSatisfaccion)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            if (oSatisfaccion.id != 0)
            {

                var actualizar = (from u in bd.satisfaccion
                                  where u.id == oSatisfaccion.id
                                  select u).FirstOrDefault();

                actualizar.responsable = oSatisfaccion.responsable;
                actualizar.stakeholder = oSatisfaccion.stakeholder;
                actualizar.fecharealizacion = oSatisfaccion.fecharealizacion;

                actualizar.conclusiones = oSatisfaccion.conclusiones;
                actualizar.personasinv = oSatisfaccion.personasinv;

                bd.SaveChanges();
                return actualizar.id;
            }
            else
            {
                satisfaccion ultimaform = (from u in bd.satisfaccion
                                           where u.anio == oSatisfaccion.anio
                                           && u.idcentral == oSatisfaccion.idcentral
                                           orderby u.idconsecutivo descending
                                           select u).FirstOrDefault();

                oSatisfaccion.codigo = string.Empty;
                bd.satisfaccion.Add(oSatisfaccion);
                bd.SaveChanges();

                int idconsecutivo = 1;

                if (ultimaform != null)
                    idconsecutivo = ultimaform.idconsecutivo + 1;

                oSatisfaccion.codigo = DateTime.Now.Year.ToString() + "-" + idconsecutivo.ToString();
                oSatisfaccion.idconsecutivo = idconsecutivo;

                bd.SaveChanges();

                return oSatisfaccion.id;
            }


        }

        public static int ActualizarRevision(revision_energetica oRevision)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            if (oRevision.id != 0)
            {

                var actualizar = (from u in bd.revision_energetica
                                  where u.id == oRevision.id
                                  select u).FirstOrDefault();

                actualizar.responsable = oRevision.responsable;
                actualizar.fechaplanificacion = oRevision.fechaplanificacion;
                actualizar.fecharevision = oRevision.fecharevision;
                actualizar.conclusiones = oRevision.conclusiones;
                actualizar.personasinv = oRevision.personasinv;

                bd.SaveChanges();
                return actualizar.id;
            }
            else
            {
                revision_energetica ultimaform = (from u in bd.revision_energetica
                                                  where u.anio == oRevision.anio
                                                  && u.idcentral == oRevision.idcentral
                                                  orderby u.idconsecutivo descending
                                                  select u).FirstOrDefault();

                oRevision.codigo = string.Empty;
                bd.revision_energetica.Add(oRevision);
                bd.SaveChanges();

                int idconsecutivo = 1;

                if (ultimaform != null)
                    idconsecutivo = ultimaform.idconsecutivo + 1;

                oRevision.codigo = DateTime.Now.Year.ToString() + "-" + idconsecutivo.ToString();
                oRevision.idconsecutivo = idconsecutivo;

                bd.SaveChanges();

                return oRevision.id;
            }


        }

        public static int ActualizarRequisito(requisitoslegales oRequisito)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            if (oRequisito.id != 0)
            {

                var actualizar = (from u in bd.requisitoslegales
                                  where u.id == oRequisito.id
                                  select u).FirstOrDefault();

                actualizar.denominacion = oRequisito.denominacion;
                actualizar.fecharegistro = oRequisito.fecharegistro;

                actualizar.numrequisitos = oRequisito.numrequisitos;
                actualizar.cumple = oRequisito.cumple;
                actualizar.tramite = oRequisito.tramite;
                actualizar.nocumple = oRequisito.nocumple;
                actualizar.observacion = oRequisito.observacion;
                actualizar.noprocede = oRequisito.noprocede;
                actualizar.noverificado = oRequisito.noverificado;
                actualizar.ambito = oRequisito.ambito;

                bd.SaveChanges();
                return actualizar.id;
            }
            else
            {
                requisitoslegales ultimoreq = (from u in bd.requisitoslegales
                                               where u.anio == oRequisito.anio
                                               orderby u.idconsecutivo descending
                                               select u).FirstOrDefault();

                oRequisito.codigo = string.Empty;
                bd.requisitoslegales.Add(oRequisito);
                bd.SaveChanges();

                int idconsecutivo = 1;

                if (ultimoreq != null)
                    idconsecutivo = ultimoreq.idconsecutivo + 1;

                oRequisito.codigo = DateTime.Now.Year.ToString() + "-" + idconsecutivo.ToString();
                oRequisito.idconsecutivo = idconsecutivo;

                bd.SaveChanges();

                return oRequisito.id;
            }


        }

        public static int ActualizarAuditoria(auditorias oAuditoria)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            if (oAuditoria.id != 0)
            {

                var actualizar = (from u in bd.auditorias
                                  where u.id == oAuditoria.id
                                  select u).FirstOrDefault();

                actualizar.programa = oAuditoria.programa;
                actualizar.tipo = oAuditoria.tipo;
                actualizar.fechainicio = oAuditoria.fechainicio;
                actualizar.fechafin = oAuditoria.fechafin;
                actualizar.informe = oAuditoria.informe;
                actualizar.comentario = oAuditoria.comentario;

                bd.SaveChanges();
                return actualizar.id;
            }
            else
            {
                bd.auditorias.Add(oAuditoria);
                bd.SaveChanges();

                return oAuditoria.id;
            }


        }

        public static int ActualizarPlanificacion(indicadores_planificacion oPlanificacion)
        {
            DIMASSTEntities bd = new DIMASSTEntities();



            var actualizar = (from u in bd.indicadores_planificacion
                              where u.Id == oPlanificacion.Id
                              select u).FirstOrDefault();

            actualizar.FuenteInformacion = oPlanificacion.FuenteInformacion;
            actualizar.Operación = oPlanificacion.Operación;

            bd.SaveChanges();
            return actualizar.Id;


        }

        public static int ActualizarImputacion(indicadores_imputacion oImputacion)
        {
            DIMASSTEntities bd = new DIMASSTEntities();



            var actualizar = (from u in bd.indicadores_imputacion
                              where u.Id == oImputacion.Id && u.anio == oImputacion.anio
                              select u).FirstOrDefault();

            if (actualizar != null)
            {
                actualizar.Valoracion1 = oImputacion.Valoracion1;
                actualizar.Valoracion2 = oImputacion.Valoracion2;
                actualizar.Valoracion3 = oImputacion.Valoracion3;
                actualizar.Valoracion4 = oImputacion.Valoracion4;
                actualizar.Valoracion5 = oImputacion.Valoracion5;
                actualizar.Valoracion6 = oImputacion.Valoracion6;
                actualizar.Valoracion7 = oImputacion.Valoracion7;
                actualizar.Valoracion8 = oImputacion.Valoracion8;
                actualizar.Valoracion9 = oImputacion.Valoracion9;
                actualizar.Valoracion10 = oImputacion.Valoracion10;
                actualizar.Valoracion11 = oImputacion.Valoracion11;
                actualizar.Valoracion12 = oImputacion.Valoracion12;

                actualizar.ValorReferencia1 = oImputacion.ValorReferencia1;
                actualizar.ValorReferencia2 = oImputacion.ValorReferencia2;
                actualizar.ValorReferencia3 = oImputacion.ValorReferencia3;
                actualizar.ValorReferencia4 = oImputacion.ValorReferencia4;
                actualizar.ValorReferencia5 = oImputacion.ValorReferencia5;
                actualizar.ValorReferencia6 = oImputacion.ValorReferencia6;
                actualizar.ValorReferencia7 = oImputacion.ValorReferencia7;
                actualizar.ValorReferencia8 = oImputacion.ValorReferencia8;
                actualizar.ValorReferencia9 = oImputacion.ValorReferencia9;
                actualizar.ValorReferencia10 = oImputacion.ValorReferencia10;
                actualizar.ValorReferencia11 = oImputacion.ValorReferencia11;
                actualizar.ValorReferencia12 = oImputacion.ValorReferencia12;

                actualizar.ValorCalculado1 = oImputacion.ValorCalculado1;
                actualizar.ValorCalculado2 = oImputacion.ValorCalculado2;
                actualizar.ValorCalculado3 = oImputacion.ValorCalculado3;
                actualizar.ValorCalculado4 = oImputacion.ValorCalculado4;
                actualizar.ValorCalculado5 = oImputacion.ValorCalculado5;
                actualizar.ValorCalculado6 = oImputacion.ValorCalculado6;
                actualizar.ValorCalculado7 = oImputacion.ValorCalculado7;
                actualizar.ValorCalculado8 = oImputacion.ValorCalculado8;
                actualizar.ValorCalculado9 = oImputacion.ValorCalculado9;
                actualizar.ValorCalculado10 = oImputacion.ValorCalculado10;
                actualizar.ValorCalculado11 = oImputacion.ValorCalculado11;
                actualizar.ValorCalculado12 = oImputacion.ValorCalculado12;

                actualizar.Operacion = oImputacion.Operacion;

                bd.SaveChanges();
                return actualizar.Id;
            }
            else
            {
                actualizar.anio = oImputacion.anio;
                actualizar.Valoracion1 = oImputacion.Valoracion1;
                actualizar.Valoracion2 = oImputacion.Valoracion2;
                actualizar.Valoracion3 = oImputacion.Valoracion3;
                actualizar.Valoracion4 = oImputacion.Valoracion4;
                actualizar.Valoracion5 = oImputacion.Valoracion5;
                actualizar.Valoracion6 = oImputacion.Valoracion6;
                actualizar.Valoracion7 = oImputacion.Valoracion7;
                actualizar.Valoracion8 = oImputacion.Valoracion8;
                actualizar.Valoracion9 = oImputacion.Valoracion9;
                actualizar.Valoracion10 = oImputacion.Valoracion10;
                actualizar.Valoracion11 = oImputacion.Valoracion11;
                actualizar.Valoracion12 = oImputacion.Valoracion12;

                actualizar.ValorReferencia1 = oImputacion.ValorReferencia1;
                actualizar.ValorReferencia2 = oImputacion.ValorReferencia2;
                actualizar.ValorReferencia3 = oImputacion.ValorReferencia3;
                actualizar.ValorReferencia4 = oImputacion.ValorReferencia4;
                actualizar.ValorReferencia5 = oImputacion.ValorReferencia5;
                actualizar.ValorReferencia6 = oImputacion.ValorReferencia6;
                actualizar.ValorReferencia7 = oImputacion.ValorReferencia7;
                actualizar.ValorReferencia8 = oImputacion.ValorReferencia8;
                actualizar.ValorReferencia9 = oImputacion.ValorReferencia9;
                actualizar.ValorReferencia10 = oImputacion.ValorReferencia10;
                actualizar.ValorReferencia11 = oImputacion.ValorReferencia11;
                actualizar.ValorReferencia12 = oImputacion.ValorReferencia12;

                actualizar.ValorCalculado1 = oImputacion.ValorCalculado1;
                actualizar.ValorCalculado2 = oImputacion.ValorCalculado2;
                actualizar.ValorCalculado3 = oImputacion.ValorCalculado3;
                actualizar.ValorCalculado4 = oImputacion.ValorCalculado4;
                actualizar.ValorCalculado5 = oImputacion.ValorCalculado5;
                actualizar.ValorCalculado6 = oImputacion.ValorCalculado6;
                actualizar.ValorCalculado7 = oImputacion.ValorCalculado7;
                actualizar.ValorCalculado8 = oImputacion.ValorCalculado8;
                actualizar.ValorCalculado9 = oImputacion.ValorCalculado9;
                actualizar.ValorCalculado10 = oImputacion.ValorCalculado10;
                actualizar.ValorCalculado11 = oImputacion.ValorCalculado11;
                actualizar.ValorCalculado12 = oImputacion.ValorCalculado12;

                actualizar.IdIndicador = oImputacion.IdIndicador;
                actualizar.IdPlanificacionIndicador = oImputacion.IdPlanificacionIndicador;

                bd.indicadores_imputacion.Add(actualizar);
                return actualizar.Id;
            }


        }

        public static int ActualizarImputacion(VISTA_IndicadoresAfectadosParametro oImputacion)
        {
            DIMASSTEntities bd = new DIMASSTEntities();



            var actualizar = (from u in bd.indicadores_imputacion
                              where u.Id == oImputacion.Id && u.anio == oImputacion.anio
                              select u).FirstOrDefault();

            if (actualizar != null)
            {
                actualizar.Valoracion1 = oImputacion.Valoracion1;
                actualizar.Valoracion2 = oImputacion.Valoracion2;
                actualizar.Valoracion3 = oImputacion.Valoracion3;
                actualizar.Valoracion4 = oImputacion.Valoracion4;
                actualizar.Valoracion5 = oImputacion.Valoracion5;
                actualizar.Valoracion6 = oImputacion.Valoracion6;
                actualizar.Valoracion7 = oImputacion.Valoracion7;
                actualizar.Valoracion8 = oImputacion.Valoracion8;
                actualizar.Valoracion9 = oImputacion.Valoracion9;
                actualizar.Valoracion10 = oImputacion.Valoracion10;
                actualizar.Valoracion11 = oImputacion.Valoracion11;
                actualizar.Valoracion12 = oImputacion.Valoracion12;

                actualizar.ValorReferencia1 = oImputacion.ValorReferencia1;
                actualizar.ValorReferencia2 = oImputacion.ValorReferencia2;
                actualizar.ValorReferencia3 = oImputacion.ValorReferencia3;
                actualizar.ValorReferencia4 = oImputacion.ValorReferencia4;
                actualizar.ValorReferencia5 = oImputacion.ValorReferencia5;
                actualizar.ValorReferencia6 = oImputacion.ValorReferencia6;
                actualizar.ValorReferencia7 = oImputacion.ValorReferencia7;
                actualizar.ValorReferencia8 = oImputacion.ValorReferencia8;
                actualizar.ValorReferencia9 = oImputacion.ValorReferencia9;
                actualizar.ValorReferencia10 = oImputacion.ValorReferencia10;
                actualizar.ValorReferencia11 = oImputacion.ValorReferencia11;
                actualizar.ValorReferencia12 = oImputacion.ValorReferencia12;

                actualizar.ValorCalculado1 = oImputacion.ValorCalculado1;
                actualizar.ValorCalculado2 = oImputacion.ValorCalculado2;
                actualizar.ValorCalculado3 = oImputacion.ValorCalculado3;
                actualizar.ValorCalculado4 = oImputacion.ValorCalculado4;
                actualizar.ValorCalculado5 = oImputacion.ValorCalculado5;
                actualizar.ValorCalculado6 = oImputacion.ValorCalculado6;
                actualizar.ValorCalculado7 = oImputacion.ValorCalculado7;
                actualizar.ValorCalculado8 = oImputacion.ValorCalculado8;
                actualizar.ValorCalculado9 = oImputacion.ValorCalculado9;
                actualizar.ValorCalculado10 = oImputacion.ValorCalculado10;
                actualizar.ValorCalculado11 = oImputacion.ValorCalculado11;
                actualizar.ValorCalculado12 = oImputacion.ValorCalculado12;

                actualizar.Operacion = oImputacion.Operacion;

                bd.SaveChanges();
                return actualizar.Id;
            }
            else
            {
                actualizar.anio = oImputacion.anio;
                actualizar.Valoracion1 = oImputacion.Valoracion1;
                actualizar.Valoracion2 = oImputacion.Valoracion2;
                actualizar.Valoracion3 = oImputacion.Valoracion3;
                actualizar.Valoracion4 = oImputacion.Valoracion4;
                actualizar.Valoracion5 = oImputacion.Valoracion5;
                actualizar.Valoracion6 = oImputacion.Valoracion6;
                actualizar.Valoracion7 = oImputacion.Valoracion7;
                actualizar.Valoracion8 = oImputacion.Valoracion8;
                actualizar.Valoracion9 = oImputacion.Valoracion9;
                actualizar.Valoracion10 = oImputacion.Valoracion10;
                actualizar.Valoracion11 = oImputacion.Valoracion11;
                actualizar.Valoracion12 = oImputacion.Valoracion12;

                actualizar.ValorReferencia1 = oImputacion.ValorReferencia1;
                actualizar.ValorReferencia2 = oImputacion.ValorReferencia2;
                actualizar.ValorReferencia3 = oImputacion.ValorReferencia3;
                actualizar.ValorReferencia4 = oImputacion.ValorReferencia4;
                actualizar.ValorReferencia5 = oImputacion.ValorReferencia5;
                actualizar.ValorReferencia6 = oImputacion.ValorReferencia6;
                actualizar.ValorReferencia7 = oImputacion.ValorReferencia7;
                actualizar.ValorReferencia8 = oImputacion.ValorReferencia8;
                actualizar.ValorReferencia9 = oImputacion.ValorReferencia9;
                actualizar.ValorReferencia10 = oImputacion.ValorReferencia10;
                actualizar.ValorReferencia11 = oImputacion.ValorReferencia11;
                actualizar.ValorReferencia12 = oImputacion.ValorReferencia12;

                actualizar.ValorCalculado1 = oImputacion.ValorCalculado1;
                actualizar.ValorCalculado2 = oImputacion.ValorCalculado2;
                actualizar.ValorCalculado3 = oImputacion.ValorCalculado3;
                actualizar.ValorCalculado4 = oImputacion.ValorCalculado4;
                actualizar.ValorCalculado5 = oImputacion.ValorCalculado5;
                actualizar.ValorCalculado6 = oImputacion.ValorCalculado6;
                actualizar.ValorCalculado7 = oImputacion.ValorCalculado7;
                actualizar.ValorCalculado8 = oImputacion.ValorCalculado8;
                actualizar.ValorCalculado9 = oImputacion.ValorCalculado9;
                actualizar.ValorCalculado10 = oImputacion.ValorCalculado10;
                actualizar.ValorCalculado11 = oImputacion.ValorCalculado11;
                actualizar.ValorCalculado12 = oImputacion.ValorCalculado12;

                actualizar.IdIndicador = oImputacion.IdIndicador;
                actualizar.IdPlanificacionIndicador = oImputacion.IdPlanificacionIndicador;

                bd.indicadores_imputacion.Add(actualizar);
                return actualizar.Id;
            }


        }

        public static int ActualizarParametroInd(indicadores_hojadedatos_valores oImputacion)
        {
            DIMASSTEntities bd = new DIMASSTEntities();



            var actualizar = (from u in bd.indicadores_hojadedatos_valores
                              where u.Id == oImputacion.Id
                              select u).FirstOrDefault();


            actualizar.valor1 = oImputacion.valor1;
            actualizar.valor2 = oImputacion.valor2;
            actualizar.valor3 = oImputacion.valor3;
            actualizar.valor4 = oImputacion.valor4;
            actualizar.valor5 = oImputacion.valor5;
            actualizar.valor6 = oImputacion.valor6;
            actualizar.valor7 = oImputacion.valor7;
            actualizar.valor8 = oImputacion.valor8;
            actualizar.valor9 = oImputacion.valor9;
            actualizar.valor10 = oImputacion.valor10;
            actualizar.valor11 = oImputacion.valor11;
            actualizar.valor12 = oImputacion.valor12;

            bd.SaveChanges();
            return actualizar.Id;



        }

        public static int ActualizarParametro(indicadores_hojadedatos oParametro)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.indicadores_hojadedatos
                              where u.id == oParametro.id
                              select u).FirstOrDefault();

            if (actualizar != null)
            {

                actualizar.idproceso = oParametro.idproceso;
                actualizar.indicador = oParametro.indicador;
                actualizar.unidad = oParametro.unidad;
                actualizar.periodicidad = oParametro.periodicidad;

                bd.SaveChanges();
            }
            else
            {
                actualizar = oParametro;
                bd.indicadores_hojadedatos.Add(oParametro);
                bd.SaveChanges();
            }
            return actualizar.id;
        }

        public static int ActualizarValoracion(aspecto_valoracion oValoracion)
        {
            DIMASSTEntities bd = new DIMASSTEntities();



            var actualizar = (from u in bd.aspecto_valoracion
                              where u.id == oValoracion.id
                              select u).FirstOrDefault();

            if (actualizar != null)
            {
                actualizar.identificacion = oValoracion.identificacion;
                actualizar.nombrefoco = oValoracion.nombrefoco;

                actualizar.anio1 = oValoracion.anio1;
                actualizar.anio2 = oValoracion.anio2;
                actualizar.anio3 = oValoracion.anio3;
                actualizar.anio4 = oValoracion.anio4;
                actualizar.anio5 = oValoracion.anio5;
                actualizar.anio6 = oValoracion.anio6;

                actualizar.naturaleza = oValoracion.naturaleza;
                actualizar.origen = oValoracion.origen;
                actualizar.magnitud = oValoracion.magnitud;

                actualizar.referencia = oValoracion.referencia;
                actualizar.datoabsoluto = oValoracion.datoabsoluto;
                actualizar.variacion = oValoracion.variacion;
                actualizar.acercamiento = oValoracion.acercamiento;
                actualizar.RU_Dia = oValoracion.RU_Dia;
                actualizar.RU_DiaRef = oValoracion.RU_DiaRef;
                actualizar.RU_Tarde = oValoracion.RU_Tarde;
                actualizar.RU_TardeRef = oValoracion.RU_TardeRef;
                actualizar.RU_Noche = oValoracion.RU_Noche;
                actualizar.RU_NocheRef = oValoracion.RU_NocheRef;

                actualizar.IN_Aspecto = oValoracion.IN_Aspecto;
                actualizar.IN_Proveedor = oValoracion.IN_Proveedor;
                actualizar.IN_ServicioPrestado = oValoracion.IN_ServicioPrestado;
                actualizar.IN_TipoActividad = oValoracion.IN_TipoActividad;

                actualizar.resmagnitud = oValoracion.resmagnitud;
                actualizar.resnaturaleza = oValoracion.resnaturaleza;
                actualizar.resorigen = oValoracion.resorigen;
                actualizar.significancia6 = oValoracion.significancia6;

                actualizar.descripcion = oValoracion.descripcion;
                actualizar.quejaobs = oValoracion.quejaobs;
                actualizar.queja = oValoracion.queja;
                actualizar.idqueja = oValoracion.idqueja;

                bd.SaveChanges();
                return actualizar.id;
            }
            else
            {
                return 0;
            }
        }

        public static int ActualizarValoracion(aspecto_parametro_valoracion oValoracion)
        {
            DIMASSTEntities bd = new DIMASSTEntities();



            var actualizar = (from u in bd.aspecto_parametro_valoracion
                              where u.id == oValoracion.id
                              select u).FirstOrDefault();

            if (actualizar != null)
            {
                actualizar.observaciones = oValoracion.observaciones;

                actualizar.mes1 = oValoracion.mes1;
                actualizar.mes2 = oValoracion.mes2;
                actualizar.mes3 = oValoracion.mes3;
                actualizar.mes4 = oValoracion.mes4;
                actualizar.mes5 = oValoracion.mes5;
                actualizar.mes6 = oValoracion.mes6;
                actualizar.mes7 = oValoracion.mes7;
                actualizar.mes8 = oValoracion.mes8;
                actualizar.mes9 = oValoracion.mes9;
                actualizar.mes10 = oValoracion.mes10;
                actualizar.mes11 = oValoracion.mes11;
                actualizar.mes12 = oValoracion.mes12;

                actualizar.anio1 = oValoracion.anio1;
                actualizar.anio2 = oValoracion.anio2;
                actualizar.anio3 = oValoracion.anio3;
                actualizar.anio4 = oValoracion.anio4;
                actualizar.anio5 = oValoracion.anio5;
                actualizar.anio6 = oValoracion.anio6;

                actualizar.naturaleza = oValoracion.naturaleza;
                actualizar.origen = oValoracion.origen;
                actualizar.magnitud = oValoracion.magnitud;

                actualizar.referencia = oValoracion.referencia;
                actualizar.referenciasup = oValoracion.referenciasup;

                actualizar.variacion = oValoracion.variacion;
                actualizar.acercamiento = oValoracion.acercamiento;
                actualizar.RU_Dia = oValoracion.RU_Dia;
                actualizar.RU_DiaRef = oValoracion.RU_DiaRef;
                actualizar.RU_Tarde = oValoracion.RU_Tarde;
                actualizar.RU_TardeRef = oValoracion.RU_TardeRef;
                actualizar.RU_Noche = oValoracion.RU_Noche;
                actualizar.RU_NocheRef = oValoracion.RU_NocheRef;


                actualizar.resmagnitud = oValoracion.resmagnitud;
                actualizar.resnaturaleza = oValoracion.resnaturaleza;
                actualizar.resorigen = oValoracion.resorigen;
                actualizar.significancia6 = oValoracion.significancia6;


                bd.SaveChanges();
                return actualizar.id;
            }
            else
            {
                return 0;
            }
        }

        public static int ActualizarSignificanciaAspecto(aspecto_valoracion oValoracion)
        {
            DIMASSTEntities bd = new DIMASSTEntities();



            var actualizar = (from u in bd.aspecto_valoracion
                              where u.id == oValoracion.id
                              select u).FirstOrDefault();

            if (actualizar != null)
            {
                actualizar.resmagnitud = oValoracion.resmagnitud;
                actualizar.resnaturaleza = oValoracion.resnaturaleza;
                actualizar.resorigen = oValoracion.resorigen;
                actualizar.significancia6 = oValoracion.significancia6;


                bd.SaveChanges();
                return actualizar.id;
            }
            else
            {
                return 0;
            }
        }

        public static int ActualizarNoticia(noticias oNoticia)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.noticias
                              where u.id == oNoticia.id
                              select u).FirstOrDefault();

            actualizar.texto = oNoticia.texto;
            actualizar.titulo = oNoticia.titulo;
            actualizar.fecha = oNoticia.fecha;
            actualizar.fechaexp = oNoticia.fechaexp;
            actualizar.validada = oNoticia.validada;
            actualizar.cabecera = oNoticia.cabecera;
            bd.SaveChanges();

            return actualizar.id;
        }

        public static int InsertarNoticia(noticias oNoticia)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            bd.noticias.Add(oNoticia);

            bd.SaveChanges();

            return oNoticia.id;
        }

        public static bool InsertarObjetivoProceso(objetivos_procesos oAsociacion)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            bd.objetivos_procesos.Add(oAsociacion);

            bd.SaveChanges();

            return true;
        }

        public static bool InsertarObjetivoIndicadorProceso(objetivo_indicadorproceso oAsociacion)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            bd.objetivo_indicadorproceso.Add(oAsociacion);

            bd.SaveChanges();

            return true;
        }


        public static int InsertarAccionObjetivo(despliegue oAccion)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            despliegue ultimaacc = (from d in bd.despliegue
                                    where d.idObjetivo == oAccion.idObjetivo
                                    orderby d.idconsecutivo descending
                                    select d).FirstOrDefault();

            bd.despliegue.Add(oAccion);

            bd.SaveChanges();

            if (ultimaacc != null)
            {
                oAccion.idconsecutivo = ultimaacc.idconsecutivo + 1;
                oAccion.NumeroAccionDespliegue = oAccion.NumeroAccionDespliegue + "/" + oAccion.idconsecutivo.ToString();
            }
            else
            {
                oAccion.idconsecutivo = 1;
                oAccion.NumeroAccionDespliegue = oAccion.NumeroAccionDespliegue + "/" + "1";
            }

            bd.SaveChanges();

            return oAccion.id;
        }

        public static int InsertarAccionAccionMejora(accionmejora_accion oAccion)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            bd.accionmejora_accion.Add(oAccion);

            bd.SaveChanges();

            return oAccion.id;
        }

        public static bool ModificarAccionAccionMejora(accionmejora_accion oAccion)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.accionmejora_accion
                              where u.id == oAccion.id
                              select u).FirstOrDefault();

            if (oAccion.id != 0 && actualizar != null)
            {
                actualizar.descripcion = oAccion.descripcion;
                actualizar.fecha_fin = oAccion.fecha_fin;
                actualizar.fecha_cierre = oAccion.fecha_cierre;
                actualizar.responsable = oAccion.responsable;
                actualizar.estado = oAccion.estado;
                actualizar.comentario = oAccion.comentario;
                actualizar.numaccion = oAccion.numaccion;
                bd.SaveChanges();

                return true;
            }
            else
                return false;
        }

        public static bool ModificarAccionObjetivo(despliegue oAccion)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.despliegue
                              where u.id == oAccion.id
                              select u).FirstOrDefault();

            if (oAccion.id != 0 && actualizar != null)
            {
                //actualizar.NumeroAccionDespliegue = oAccion.NumeroAccionDespliegue;
                actualizar.Nombre = oAccion.Nombre;
                actualizar.Descripcion = oAccion.Descripcion;
                actualizar.FechaEstimada = oAccion.FechaEstimada;
                actualizar.FechaReal = oAccion.FechaReal;
                actualizar.Responsable = oAccion.Responsable;
                actualizar.Estado = oAccion.Estado;
                actualizar.Recursos = oAccion.Recursos;
                actualizar.Comentarios = oAccion.Comentarios;
                actualizar.Porcentaje = oAccion.Porcentaje;
                bd.SaveChanges();

                return true;
            }
            else
                return false;
        }

        public static int ActualizarTipoAspecto(aspecto_tipo oAspecto)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.aspecto_tipo
                              where u.id == oAspecto.id
                              select u).FirstOrDefault();

            if (oAspecto.id != 0 && actualizar != null)
            {
                actualizar.Grupo = oAspecto.Grupo;
                actualizar.Nombre = oAspecto.Nombre;
                actualizar.Descripcion = oAspecto.Descripcion;
                actualizar.Unidad = oAspecto.Unidad;
                actualizar.RE_Peligroso = oAspecto.RE_Peligroso;
                actualizar.relativoMwhb = oAspecto.relativoMwhb;
                actualizar.relativohAnioGE = oAspecto.relativohAnioGE;
                actualizar.relativokmAnio = oAspecto.relativokmAnio;
                actualizar.relativom3Hora = oAspecto.relativom3Hora;
                actualizar.relativohfuncAnio = oAspecto.relativohfuncAnio;
                actualizar.Impacto = oAspecto.Impacto;
                bd.SaveChanges();

                return oAspecto.id;
            }
            else
            {
                aspecto_grupo grupo = (from u in bd.aspecto_grupo
                                       where u.id == oAspecto.Grupo
                                       select u).FirstOrDefault();

                VISTA_AspectosTipo ultimaform = (from u in bd.VISTA_AspectosTipo
                                                 where u.siglas == grupo.siglas
                                                 orderby u.id descending
                                                 select u).FirstOrDefault();

                aspecto_tipo insertarInd = new aspecto_tipo();
                insertarInd.Grupo = oAspecto.Grupo;
                insertarInd.Nombre = oAspecto.Nombre;
                insertarInd.Descripcion = oAspecto.Descripcion;
                insertarInd.Unidad = oAspecto.Unidad;
                insertarInd.RE_Peligroso = oAspecto.RE_Peligroso;
                insertarInd.relativoMwhb = oAspecto.relativoMwhb;
                insertarInd.relativohAnioGE = oAspecto.relativohAnioGE;
                insertarInd.relativokmAnio = oAspecto.relativokmAnio;
                insertarInd.relativom3Hora = oAspecto.relativom3Hora;
                insertarInd.relativohfuncAnio = oAspecto.relativohfuncAnio;
                insertarInd.Activo = true;
                bd.aspecto_tipo.Add(insertarInd);
                bd.SaveChanges();

                int idconsecutivo = 1;

                if (ultimaform != null && ultimaform.idconsecutivo != null)
                    idconsecutivo = int.Parse(ultimaform.idconsecutivo.ToString()) + 1;

                insertarInd.Codigo = grupo.siglas.Trim() + "-" + idconsecutivo.ToString();
                insertarInd.idconsecutivo = idconsecutivo;

                bd.SaveChanges();

                return insertarInd.id;
            }



        }

        public static int ActualizarIndicador(indicadores oIndicador)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.indicadores
                              where u.Id == oIndicador.Id
                              select u).FirstOrDefault();

            if (oIndicador.Id != 0 && actualizar != null)
            {
                actualizar.Nombre = oIndicador.Nombre;
                actualizar.Descripcion = oIndicador.Descripcion;
                actualizar.MetodoMedicion = oIndicador.MetodoMedicion;
                actualizar.Unidad = oIndicador.Unidad;
                actualizar.ProcesoAsociado = oIndicador.ProcesoAsociado;
                actualizar.tecnologia = oIndicador.tecnologia;
                actualizar.Periodicidad = oIndicador.Periodicidad;
                actualizar.tendencia = oIndicador.tendencia;
                actualizar.ValorNumerico = oIndicador.ValorNumerico;
                actualizar.Operador1 = oIndicador.Operador1;
                actualizar.Operador2 = oIndicador.Operador2;
                actualizar.Operador3 = oIndicador.Operador3;
                actualizar.Operacion1 = oIndicador.Operacion1;
                actualizar.Operacion2 = oIndicador.Operacion2;
                actualizar.Operador1Constante = oIndicador.Operador1Constante;
                actualizar.Operador2Constante = oIndicador.Operador2Constante;
                actualizar.Operador3Constante = oIndicador.Operador3Constante;

                actualizar.especifico = oIndicador.especifico;

                bd.SaveChanges();

                return oIndicador.Id;
            }
            else
            {
                indicadores insertarInd = new indicadores();
                insertarInd.Nombre = oIndicador.Nombre;
                insertarInd.Descripcion = oIndicador.Descripcion;
                insertarInd.MetodoMedicion = oIndicador.MetodoMedicion;
                insertarInd.Unidad = oIndicador.Unidad;
                insertarInd.ProcesoAsociado = oIndicador.ProcesoAsociado;
                insertarInd.Activo = true;
                insertarInd.tecnologia = oIndicador.tecnologia;
                insertarInd.Periodicidad = oIndicador.Periodicidad;
                insertarInd.ValorNumerico = oIndicador.ValorNumerico;
                insertarInd.Operador1 = oIndicador.Operador1;
                insertarInd.Operador2 = oIndicador.Operador2;
                insertarInd.Operador3 = oIndicador.Operador3;
                insertarInd.Operacion1 = oIndicador.Operacion1;
                insertarInd.Operacion2 = oIndicador.Operacion2;
                insertarInd.tendencia = oIndicador.tendencia;
                insertarInd.Operador1Constante = oIndicador.Operador1Constante;
                insertarInd.Operador2Constante = oIndicador.Operador2Constante;
                insertarInd.Operador3Constante = oIndicador.Operador3Constante;

                insertarInd.especifico = oIndicador.especifico;

                bd.indicadores.Add(insertarInd);
                bd.SaveChanges();

                return insertarInd.Id;
            }



        }

        public static int ActualizarParametros(aspecto_parametros oParametros)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.aspecto_parametros
                              where u.idCentral == oParametros.idCentral
                              select u).FirstOrDefault();

            if (actualizar != null)
            {
                actualizar.Mwhb1 = oParametros.Mwhb1;
                actualizar.Mwhb2 = oParametros.Mwhb2;
                actualizar.Mwhb3 = oParametros.Mwhb3;
                actualizar.Mwhb4 = oParametros.Mwhb4;
                actualizar.Mwhb5 = oParametros.Mwhb5;
                actualizar.Mwhb6 = oParametros.Mwhb6;

                actualizar.hAnioGE1 = oParametros.hAnioGE1;
                actualizar.hAnioGE2 = oParametros.hAnioGE2;
                actualizar.hAnioGE3 = oParametros.hAnioGE3;
                actualizar.hAnioGE4 = oParametros.hAnioGE4;
                actualizar.hAnioGE5 = oParametros.hAnioGE5;
                actualizar.hAnioGE6 = oParametros.hAnioGE6;

                actualizar.kmAnio1 = oParametros.kmAnio1;
                actualizar.kmAnio2 = oParametros.kmAnio2;
                actualizar.kmAnio3 = oParametros.kmAnio3;
                actualizar.kmAnio4 = oParametros.kmAnio4;
                actualizar.kmAnio5 = oParametros.kmAnio5;
                actualizar.kmAnio6 = oParametros.kmAnio6;

                actualizar.m3Hora1 = oParametros.m3Hora1;
                actualizar.m3Hora2 = oParametros.m3Hora2;
                actualizar.m3Hora3 = oParametros.m3Hora3;
                actualizar.m3Hora4 = oParametros.m3Hora4;
                actualizar.m3Hora5 = oParametros.m3Hora5;
                actualizar.m3Hora6 = oParametros.m3Hora6;

                actualizar.hfuncAnio1 = oParametros.hfuncAnio1;
                actualizar.hfuncAnio2 = oParametros.hfuncAnio2;
                actualizar.hfuncAnio3 = oParametros.hfuncAnio3;
                actualizar.hfuncAnio4 = oParametros.hfuncAnio4;
                actualizar.hfuncAnio5 = oParametros.hfuncAnio5;
                actualizar.hfuncAnio6 = oParametros.hfuncAnio6;

                actualizar.numtrabAnio1 = oParametros.numtrabAnio1;
                actualizar.numtrabAnio2 = oParametros.numtrabAnio2;
                actualizar.numtrabAnio3 = oParametros.numtrabAnio3;
                actualizar.numtrabAnio4 = oParametros.numtrabAnio4;
                actualizar.numtrabAnio5 = oParametros.numtrabAnio5;
                actualizar.numtrabAnio6 = oParametros.numtrabAnio6;

                actualizar.m3aguadesaladaAnio1 = oParametros.m3aguadesaladaAnio1;
                actualizar.m3aguadesaladaAnio2 = oParametros.m3aguadesaladaAnio2;
                actualizar.m3aguadesaladaAnio3 = oParametros.m3aguadesaladaAnio3;
                actualizar.m3aguadesaladaAnio4 = oParametros.m3aguadesaladaAnio4;
                actualizar.m3aguadesaladaAnio5 = oParametros.m3aguadesaladaAnio5;
                actualizar.m3aguadesaladaAnio6 = oParametros.m3aguadesaladaAnio6;

                actualizar.trabcanteraAnio1 = oParametros.trabcanteraAnio1;
                actualizar.trabcanteraAnio2 = oParametros.trabcanteraAnio2;
                actualizar.trabcanteraAnio3 = oParametros.trabcanteraAnio3;
                actualizar.trabcanteraAnio4 = oParametros.trabcanteraAnio4;
                actualizar.trabcanteraAnio5 = oParametros.trabcanteraAnio5;
                actualizar.trabcanteraAnio6 = oParametros.trabcanteraAnio6;

                bd.SaveChanges();

                return actualizar.id;
            }
            else
            {
                aspecto_parametros insertarParam = new aspecto_parametros();
                insertarParam = oParametros;

                bd.aspecto_parametros.Add(insertarParam);
                bd.SaveChanges();

                return insertarParam.id;
            }



        }

        public static documentacion ActualizarEstadosFichero(int idFichero)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.documentacion
                              where u.idFichero == idFichero
                              select u).FirstOrDefault();

            if (actualizar != null)
            {
                actualizar.estado = 0;
                bd.SaveChanges();

                return actualizar;
            }

            return actualizar;
        }

        public static bool ActualizarPais(pais oPais)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.pais
                              where u.id_pais == oPais.id_pais
                              select u).FirstOrDefault();

            if (oPais.id_pais != 0 && actualizar != null)
            {

                actualizar.nombre_pais = oPais.nombre_pais;

                bd.SaveChanges();

                return true;
            }
            else
            {
                pais insertarpais = new pais();

                insertarpais.nombre_pais = oPais.nombre_pais;

                bd.pais.Add(insertarpais);
                bd.SaveChanges();



                return true;
            }



        }

        public static bool ActualizarCCAA(comunidad_autonoma oCCAA)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.comunidad_autonoma
                              where u.id_comunidad_autonoma == oCCAA.id_comunidad_autonoma
                              select u).FirstOrDefault();

            if (oCCAA.id_comunidad_autonoma != 0 && actualizar != null)
            {

                actualizar.id_pais = oCCAA.id_pais;
                actualizar.nombre_comunidad = oCCAA.nombre_comunidad;

                bd.SaveChanges();

                return true;
            }
            else
            {
                comunidad_autonoma insertarCCAA = new comunidad_autonoma();

                insertarCCAA.id_pais = oCCAA.id_pais;
                insertarCCAA.nombre_comunidad = oCCAA.nombre_comunidad;


                bd.comunidad_autonoma.Add(insertarCCAA);
                bd.SaveChanges();



                return true;
            }



        }

        public static bool ActualizarPassword(usuarios oUsuario)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.usuarios
                              where u.idUsuario == oUsuario.idUsuario
                              select u).FirstOrDefault();

            if (oUsuario != null && actualizar != null)
            {
                //actualizar.id = oPedido.id;
                //actualizar.nombre = oUsuario.nombre;
                actualizar.password = oUsuario.password;
                //actualizar.perfil = 1;


                bd.SaveChanges();

                return true;
            }
            else
            {
                return false;
            }



        }

        public static usuarios ObtenerUsuario(int? idUsuario)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            usuarios nuevo = new usuarios();

            var registro = (from u in bd.usuarios
                            where u.idUsuario == idUsuario
                            select u).FirstOrDefault();


            if (registro != null)
            {
                nuevo.idUsuario = registro.idUsuario;
                nuevo.nombre = registro.nombre;
                nuevo.password = registro.password;
                nuevo.perfil = registro.perfil;
                nuevo.mail = registro.mail;
                nuevo.telefono = registro.telefono;
                nuevo.puesto = registro.puesto;
            }
            else
                nuevo = null;


            return nuevo;
        }

        public static VISTA_ObtenerUsuario ObtenerUsuarioVista(int idUsuario)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            VISTA_ObtenerUsuario nuevo = new VISTA_ObtenerUsuario();

            var registro = (from u in bd.VISTA_ObtenerUsuario
                            where u.idUsuario == idUsuario
                            select u).FirstOrDefault();


            if (registro != null)
            {
                nuevo.idUsuario = registro.idUsuario;
                nuevo.nombre = registro.nombre;
                nuevo.password = registro.password;
                nuevo.perfil = registro.perfil;
                nuevo.nombreap = registro.nombreap;
                nuevo.mail = registro.mail;
                nuevo.telefono = registro.telefono;
                nuevo.puesto = registro.puesto;
                nuevo.fecha_registro = registro.fecha_registro;
                nuevo.idUnidad = registro.idUnidad;

            }
            else
                nuevo = null;


            return nuevo;
        }
        public static int ObtenerCentroPorIdArea(int idArea)
        {

            DIMASSTEntities bd = new DIMASSTEntities();


            int registro = (from u in bd.areanivel1
                            where u.id == idArea
                            select u.id_centro).FirstOrDefault();
            return registro;
        }

        public static centros ObtenerCentro(int? idCentro)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            centros nuevo = new centros();

            var registro = (from u in bd.centros
                            where u.id == idCentro
                            select u).FirstOrDefault();


            if (registro != null)
            {
                nuevo.id = registro.id;
                nuevo.siglas = registro.siglas;
                nuevo.nombre = registro.nombre;
                nuevo.tipo = registro.tipo;
            }
            else
                nuevo = null;


            return nuevo;
        }

        public static centros ObtenerCentro(string siglas)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            centros nuevo = new centros();

            var registro = (from u in bd.centros
                            where u.siglas == siglas
                            select u).FirstOrDefault();


            if (registro != null)
            {
                nuevo.id = registro.id;
                nuevo.siglas = registro.siglas;
                nuevo.nombre = registro.nombre;

            }
            else
                nuevo = null;


            return nuevo;
        }
        public static List<comunidad_autonoma> ListarComunidades()
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<comunidad_autonoma> listaProvincias = new List<comunidad_autonoma>();

            var registros = (from u in bd.comunidad_autonoma
                             select u).ToList();


            foreach (comunidad_autonoma registro in registros)
            {
                comunidad_autonoma nuevaProvincia = new comunidad_autonoma();
                nuevaProvincia.id_comunidad_autonoma = registro.id_comunidad_autonoma;
                nuevaProvincia.nombre_comunidad = registro.nombre_comunidad;

                listaProvincias.Add(nuevaProvincia);
            }



            return listaProvincias;
        }


        public static List<provincia> ListarProvincias()
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<provincia> listaProvincias = new List<provincia>();

            var registros = (from u in bd.provincia
                             select u).ToList();


            foreach (provincia registro in registros)
            {
                provincia nuevaProvincia = new provincia();
                nuevaProvincia.id_provincia = registro.id_provincia;
                nuevaProvincia.nombre_provincia = registro.nombre_provincia;

                listaProvincias.Add(nuevaProvincia);
            }



            return listaProvincias;
        }

        public static List<VISTA_ListarComunidadesEdicion> ListarComunidadesEdicion()
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<VISTA_ListarComunidadesEdicion> listaComunidades = new List<VISTA_ListarComunidadesEdicion>();

            listaComunidades = (from u in bd.VISTA_ListarComunidadesEdicion
                                select u
                             ).OrderBy(x => x.nombre_comunidad).ToList();

            return listaComunidades;
        }

        public static async Task<List<VISTA_ListarCentrales>> ListarCentrosAsync()
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<VISTA_ListarCentrales> listaCentros = new List<VISTA_ListarCentrales>();

            var registros = await (from u in bd.VISTA_ListarCentrales
                                   select u).OrderBy(x => x.nombre).ToListAsync();


            foreach (VISTA_ListarCentrales registro in registros)
            {
                VISTA_ListarCentrales nuevoCentro = new VISTA_ListarCentrales();
                nuevoCentro.id = registro.id;
                nuevoCentro.nombre = registro.nombre;
                nuevoCentro.siglas = registro.siglas;
                nuevoCentro.tipo = registro.tipo;
                nuevoCentro.nombre_comunidad = registro.nombre_comunidad;
                nuevoCentro.nombre_provincia = registro.nombre_provincia;

                listaCentros.Add(nuevoCentro);
            }

            return listaCentros;
        }
        public static List<VISTA_ListarCentrales> ListarCentros()
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<VISTA_ListarCentrales> listaCentros = new List<VISTA_ListarCentrales>();

            var registros = (from u in bd.VISTA_ListarCentrales
                             select u).OrderBy(x => x.nombre).ToList();


            foreach (VISTA_ListarCentrales registro in registros)
            {
                VISTA_ListarCentrales nuevoCentro = new VISTA_ListarCentrales();
                nuevoCentro.id = registro.id;
                nuevoCentro.nombre = registro.nombre;
                nuevoCentro.siglas = registro.siglas;
                nuevoCentro.tipo = registro.tipo;
                nuevoCentro.nombre_comunidad = registro.nombre_comunidad;
                nuevoCentro.nombre_provincia = registro.nombre_provincia;

                listaCentros.Add(nuevoCentro);
            }

            return listaCentros;
        }

        public static List<VISTA_ListaCentroTecnologia> VISTAListaCentroTecnologia()
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<VISTA_ListaCentroTecnologia> registros = (from u in bd.VISTA_ListaCentroTecnologia
                                                           select u).ToList();

            return registros;
        }
        public static List<VISTA_ListaCentroZona> VistaListarCentrosZonas()
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<VISTA_ListaCentroZona> registros = (from u in bd.VISTA_ListaCentroZona
                                                     select u).ToList();

            return registros;
        }
        public static List<VISTA_ListaCentroZonaAgrupacion> VISTAListaCentroZonaAgrupacion()
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<VISTA_ListaCentroZonaAgrupacion> registros = (from u in bd.VISTA_ListaCentroZonaAgrupacion
                                                               select u).ToList();

            return registros;
        }

        public static List<centros> ListarCentrosPorAgrupacion(int agrupacion)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<centros> listaCentros = new List<centros>();

            var registros = (from u in bd.centros
                             join ca in bd.centros_agrupacion on u.id equals ca.id_centro
                             where ca.id_agrupacion == agrupacion
                             select u).OrderBy(x => x.nombre).ToList();



            foreach (centros registro in registros)
            {
                centros nuevoCentro = new centros();
                nuevoCentro.id = registro.id;
                nuevoCentro.nombre = registro.nombre;
                nuevoCentro.siglas = registro.siglas;
                nuevoCentro.tipo = registro.tipo;
                nuevoCentro.ubicacion = registro.ubicacion;
                nuevoCentro.provincia = registro.provincia;

                listaCentros.Add(nuevoCentro);
            }

            return listaCentros;
        }

        public static List<areanivel3> ListarEquipos(int sistema)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<areanivel3> listaEquipos = new List<areanivel3>();

            var registros = (from u in bd.areanivel3
                             select u).Where(y => y.id_areanivel2 == sistema).OrderBy(x => x.nombre).ToList();


            foreach (areanivel3 registro in registros)
            {
                areanivel3 nuevoArea = new areanivel3();
                nuevoArea.id = registro.id;
                nuevoArea.nombre = registro.nombre;
                nuevoArea.codigo = registro.codigo;
                nuevoArea.id_areanivel2 = registro.id_areanivel2;


                listaEquipos.Add(nuevoArea);
            }

            return listaEquipos;
        }

        public static List<areanivel4> ListarNivelescuatro(int equipo)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<areanivel4> listaEquipos = new List<areanivel4>();

            var registros = (from u in bd.areanivel4
                             select u).Where(y => y.id_areanivel3 == equipo).OrderBy(x => x.nombre).ToList();
            foreach (areanivel4 registro in registros)
            {
                areanivel4 nuevoArea = new areanivel4();
                nuevoArea.id = registro.id;
                nuevoArea.nombre = registro.nombre;
                nuevoArea.codigo = registro.codigo;
                nuevoArea.id_areanivel3 = registro.id_areanivel3;
                listaEquipos.Add(nuevoArea);
            }
            return listaEquipos;
        }



        public static List<areanivel4> ListarNivelescuatro()
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<areanivel4> listaEquipos = new List<areanivel4>();

            var registros = (from u in bd.areanivel4
                             select u).OrderBy(x => x.id).ToList();


            foreach (areanivel4 registro in registros)
            {
                areanivel4 nuevoArea = new areanivel4();
                nuevoArea.id = registro.id;
                nuevoArea.nombre = registro.nombre;
                nuevoArea.codigo = registro.codigo;
                nuevoArea.id_areanivel3 = registro.id_areanivel3;


                listaEquipos.Add(nuevoArea);
            }

            return listaEquipos;
        }

        public static List<areanivel3> ListarEquipos()
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<areanivel3> listaEquipos = new List<areanivel3>();

            var registros = (from u in bd.areanivel3
                             select u).OrderBy(x => x.id).ToList();


            foreach (areanivel3 registro in registros)
            {
                areanivel3 nuevoArea = new areanivel3();
                nuevoArea.id = registro.id;
                nuevoArea.nombre = registro.nombre;
                nuevoArea.codigo = registro.codigo;
                nuevoArea.id_areanivel2 = registro.id_areanivel2;


                listaEquipos.Add(nuevoArea);
            }

            return listaEquipos;
        }

        public static List<matriz_centro> ListarNiveltresMatriz(int idCentro, int idVersion)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<areanivel3> listaEquipos = new List<areanivel3>();

            List<matriz_centro> registros = (from u in bd.matriz_centro
                                             where u.id_centro == idCentro &&
                                             u.version == idVersion &&
                                             u.id_areanivel3 != null
                                             select u).OrderBy(x => x.id).ToList();

            return registros;
        }


        public static List<areas_imagenes> ListarImagenesAreas()
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<areas_imagenes> listaAreas = new List<areas_imagenes>();

            var registros = (from u in bd.areas_imagenes
                             select u).OrderBy(x => x.id).ToList();


            foreach (areas_imagenes registro in registros)
            {
                areas_imagenes nuevoArea = new areas_imagenes();
                nuevoArea.id = registro.id;
                nuevoArea.id_areanivel1 = registro.id_areanivel1;
                nuevoArea.rutaImagen = registro.rutaImagen;
                listaAreas.Add(nuevoArea);
            }

            return listaAreas;
        }

        public static List<areas2_imagenes> ListarImagenesSistemas()
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<areas2_imagenes> listaAreas = new List<areas2_imagenes>();

            var registros = (from u in bd.areas2_imagenes
                             select u).OrderBy(x => x.id).ToList();


            foreach (areas2_imagenes registro in registros)
            {
                areas2_imagenes nuevoArea = new areas2_imagenes();
                nuevoArea.id = registro.id;
                nuevoArea.id_areanivel2 = registro.id_areanivel2;
                nuevoArea.rutaImagen = registro.rutaImagen;
                listaAreas.Add(nuevoArea);
            }

            return listaAreas;
        }
        public static List<equipos_imagenes> ListarImagenesEquipos()
        {

            DIMASSTEntities bd = new DIMASSTEntities();
            List<equipos_imagenes> listaAreas = new List<equipos_imagenes>();
            var registros = (from u in bd.equipos_imagenes
                             select u).OrderBy(x => x.id).ToList();


            foreach (equipos_imagenes registro in registros)
            {
                equipos_imagenes nuevoArea = new equipos_imagenes();
                nuevoArea.id = registro.id;
                nuevoArea.id_areanivel3 = registro.id_areanivel3;
                nuevoArea.rutaImagen = registro.rutaImagen;
                listaAreas.Add(nuevoArea);
            }

            return listaAreas;
        }

        public static List<areas4_imagenes> ListarImagenesAreasCuatro()
        {

            DIMASSTEntities bd = new DIMASSTEntities();
            List<areas4_imagenes> listaAreas = new List<areas4_imagenes>();
            var registros = (from u in bd.areas4_imagenes
                             select u).OrderBy(x => x.id).ToList();


            foreach (areas4_imagenes registro in registros)
            {
                areas4_imagenes nuevoArea = new areas4_imagenes();
                nuevoArea.id = registro.id;
                nuevoArea.id_areanivel4 = registro.id_areanivel4;
                nuevoArea.rutaImagen = registro.rutaImagen;
                listaAreas.Add(nuevoArea);
            }

            return listaAreas;
        }


        public static List<areanivel2> ListarSistema()
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<areanivel2> listaCentros = new List<areanivel2>();

            var registros = (from u in bd.areanivel2
                             select u).OrderBy(x => x.id).ToList();


            foreach (areanivel2 registro in registros)
            {
                areanivel2 nuevoArea = new areanivel2();
                nuevoArea.id = registro.id;
                nuevoArea.nombre = registro.nombre;
                nuevoArea.codigo = registro.codigo;
                nuevoArea.id_areanivel1 = registro.id_areanivel1;


                listaCentros.Add(nuevoArea);
            }

            return listaCentros;
        }

        public static List<areanivel2> ListarSistemaPorIDArea(int id)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<areanivel2> listaCentros = new List<areanivel2>();

            var registros = (from u in bd.areanivel2
                             where u.areanivel1.id == id
                             select u).OrderBy(x => x.nombre).ToList();


            foreach (areanivel2 registro in registros)
            {
                areanivel2 nuevoArea = new areanivel2();
                nuevoArea.id = registro.id;
                nuevoArea.nombre = registro.nombre;
                nuevoArea.codigo = registro.codigo;
                nuevoArea.id_areanivel1 = registro.id_areanivel1;


                listaCentros.Add(nuevoArea);
            }

            return listaCentros;
        }
        public static List<areanivel3> ListarEquiposPorIDSistema(int id)
        {

            DIMASSTEntities bd = new DIMASSTEntities();
            List<areanivel3> listaCentros = new List<areanivel3>();
            var registros = (from u in bd.areanivel3
                             where u.areanivel2.id == id
                             select u).OrderBy(x => x.nombre).ToList();

            foreach (areanivel3 registro in registros)
            {
                areanivel3 nuevoArea = new areanivel3();
                nuevoArea.id = registro.id;
                nuevoArea.nombre = registro.nombre;
                nuevoArea.codigo = registro.codigo;
                nuevoArea.id_areanivel2 = registro.id_areanivel2;
                listaCentros.Add(nuevoArea);
            }

            return listaCentros;
        }



        public static List<areanivel1> ListarAreas()
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<areanivel1> listaCentros = new List<areanivel1>();

            var registros = (from u in bd.areanivel1
                             select u).OrderBy(x => x.id).ToList();


            foreach (areanivel1 registro in registros)
            {
                areanivel1 nuevoArea = new areanivel1();
                nuevoArea.id = registro.id;
                nuevoArea.nombre = registro.nombre;
                nuevoArea.codigo = registro.codigo;
                nuevoArea.id_centro = registro.id_centro;
                nuevoArea.id_tecnologia = registro.id_tecnologia;


                listaCentros.Add(nuevoArea);
            }

            return listaCentros;
        }


        public static List<areanivel1> ListarAreas(int version)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<areanivel1> listaAreas = new List<areanivel1>();

            var ids = (from ju in bd.matriz_centro
                       where ju.version == version
                       select ju.id_areanivel1).ToList();

            var registros = (from u in bd.areanivel1
                             where ids.Contains(u.id)
                             select u).OrderBy(x => x.id).ToList();


            foreach (areanivel1 registro in registros)
            {
                areanivel1 nuevoArea = new areanivel1();
                nuevoArea.id = registro.id;
                nuevoArea.nombre = registro.nombre;
                nuevoArea.codigo = registro.codigo;
                nuevoArea.id_centro = registro.id_centro;
                nuevoArea.id_tecnologia = registro.id_tecnologia;

                listaAreas.Add(nuevoArea);
            }

            return listaAreas;
        }

        public static List<areanivel1> ListarAreasPorVersionYTecnologia(int version, int tecnologia)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<areanivel1> listaAreas = new List<areanivel1>();

            var ids = (from ju in bd.matriz_centro
                       where ju.version == version && ju.id_tecnologia == tecnologia
                       select ju.id_areanivel1).ToList();

            var registros = (from u in bd.areanivel1
                             where ids.Contains(u.id)
                             select u).OrderBy(x => x.id).ToList();


            foreach (areanivel1 registro in registros)
            {
                areanivel1 nuevoArea = new areanivel1();
                nuevoArea.id = registro.id;
                nuevoArea.nombre = registro.nombre;
                nuevoArea.codigo = registro.codigo;
                nuevoArea.id_centro = registro.id_centro;
                nuevoArea.id_tecnologia = registro.id_tecnologia;

                listaAreas.Add(nuevoArea);
            }

            return listaAreas;
        }
        public static List<areanivel1> ListarAreasMaestro(int version, int centro)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<areanivel1> listaAreas = new List<areanivel1>();

            var ids = (from ju in bd.matriz_centro
                       where ju.version == version
                       select ju.id_areanivel1).ToList();

            var registros = (from u in bd.areanivel1
                             where ids.Contains(u.id)
                             select u).OrderBy(x => x.id).ToList();


            foreach (areanivel1 registro in registros)
            {
                areanivel1 nuevoArea = new areanivel1();
                nuevoArea.id = registro.id;
                nuevoArea.nombre = registro.nombre;
                nuevoArea.codigo = registro.codigo;
                nuevoArea.id_centro = registro.id_centro;
                nuevoArea.id_tecnologia = registro.id_tecnologia;

                listaAreas.Add(nuevoArea);
            }

            return listaAreas;
        }
        public static List<procesos> ListarProcesosAsignablesComunicacion(int idCom)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<procesos> listaProcesos = new List<procesos>();

            List<procesos> registros = new List<procesos>();

            registros = (from u in bd.procesos
                         where !(from uc in bd.comunicacion_procesos
                                 where uc.idcomunicacion == idCom
                                 select uc.idproceso).Contains(u.id)
                         select u).OrderBy(x => x.nombre).ToList();


            foreach (procesos registro in registros)
            {
                procesos nuevoProceso = new procesos();
                nuevoProceso = registro;
                nuevoProceso.nombre = nuevoProceso.cod_proceso + " - " + nuevoProceso.nombre;
                listaProcesos.Add(nuevoProceso);
            }

            return listaProcesos;
        }

        public static List<centros> ListarCentrosAsignablesIndicador(int idObj)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<centros> listaCentros = new List<centros>();

            List<centros> registros = new List<centros>();

            if (idObj != 0)
            {
                registros = (from u in bd.centros
                             where !(from uc in bd.indicador_centrales
                                     where uc.idIndicador == idObj
                                     select uc.idCentro).Contains(u.id)
                             select u).OrderBy(x => x.nombre).ToList();
            }
            else
            {
                registros = (from u in bd.centros
                             select u).OrderBy(x => x.nombre).ToList();
            }


            foreach (centros registro in registros)
            {
                centros nuevoCentro = new centros();
                nuevoCentro.id = registro.id;
                nuevoCentro.nombre = registro.nombre;
                nuevoCentro.siglas = registro.siglas;

                listaCentros.Add(nuevoCentro);
            }

            return listaCentros;
        }

        public static List<centros> ListarCentrosAsignablesIndicadorNuevo()
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<centros> listaCentros = new List<centros>();

            List<centros> registros = new List<centros>();

            registros = (from u in bd.centros
                         where !(from uc in bd.indicador_centrales
                                 where uc.idIndicador == 5
                                 select uc.idCentro).Contains(u.id)
                         select u).OrderBy(x => x.nombre).ToList();


            foreach (centros registro in registros)
            {
                centros nuevoCentro = new centros();
                nuevoCentro.id = registro.id;
                nuevoCentro.nombre = registro.nombre;
                nuevoCentro.siglas = registro.siglas;

                listaCentros.Add(nuevoCentro);
            }

            return listaCentros;
        }
        public static List<centros> ListarCentrosAsignadosIndicador(int idObj)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<centros> listaCentros = new List<centros>();

            List<centros> registros = new List<centros>();

            if (idObj != 0)
            {

                registros = (from u in bd.centros
                             where (from uc in bd.indicador_centrales
                                    where uc.idIndicador == idObj
                                    select uc.idCentro).Contains(u.id)
                             select u).OrderBy(x => x.nombre).ToList();


                foreach (centros registro in registros)
                {
                    centros nuevoCentro = new centros();
                    nuevoCentro.id = registro.id;
                    nuevoCentro.nombre = registro.nombre;
                    nuevoCentro.siglas = registro.siglas;

                    listaCentros.Add(nuevoCentro);
                }
            }
            return listaCentros;
        }

        public static List<centros> ListarCentrosAsignablesObjetivo(int idObj)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<centros> listaCentros = new List<centros>();

            List<centros> registros = new List<centros>();

            registros = (from u in bd.centros
                         where !(from uc in bd.objetivo_centrales
                                 where uc.idObjetivo == idObj
                                 select uc.idCentro).Contains(u.id)
                         select u).OrderBy(x => x.nombre).ToList();


            foreach (centros registro in registros)
            {
                centros nuevoCentro = new centros();
                nuevoCentro.id = registro.id;
                nuevoCentro.nombre = registro.nombre;
                nuevoCentro.siglas = registro.siglas;

                listaCentros.Add(nuevoCentro);
            }

            return listaCentros;
        }

        public static List<VISTA_StakeholdersN2> ListarStakeholdersAsignablesRiesgo(int idRiesgo)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<VISTA_StakeholdersN2> listaCentros = new List<VISTA_StakeholdersN2>();

            List<VISTA_StakeholdersN2> registros = new List<VISTA_StakeholdersN2>();

            registros = (from u in bd.VISTA_StakeholdersN2
                         where !(from uc in bd.riesgos_stakeholders
                                 where uc.idriesgo == idRiesgo
                                 select uc.idstakeholder).Contains(u.id)
                         select u).OrderBy(x => x.denominacionn2).ToList();


            foreach (VISTA_StakeholdersN2 registro in registros)
            {
                VISTA_StakeholdersN2 nuevoCentro = new VISTA_StakeholdersN2();
                nuevoCentro.id = registro.id;
                nuevoCentro.denominacionn2 = registro.denominacionn2;

                listaCentros.Add(nuevoCentro);
            }

            return listaCentros;
        }

        public static List<VISTA_StakeholdersN2> ListarStakeholdersAsignadosRiesgo(int idRiesgo)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<VISTA_StakeholdersN2> listaCentros = new List<VISTA_StakeholdersN2>();

            List<VISTA_StakeholdersN2> registros = new List<VISTA_StakeholdersN2>();

            registros = (from u in bd.VISTA_StakeholdersN2
                         where (from uc in bd.riesgos_stakeholders
                                where uc.idriesgo == idRiesgo
                                select uc.idstakeholder).Contains(u.id)
                         select u).OrderBy(x => x.denominacionn2).ToList();


            foreach (VISTA_StakeholdersN2 registro in registros)
            {
                VISTA_StakeholdersN2 nuevoCentro = new VISTA_StakeholdersN2();
                nuevoCentro.id = registro.id;
                nuevoCentro.denominacionn2 = registro.denominacionn2;

                listaCentros.Add(nuevoCentro);
            }

            return listaCentros;
        }

        public static List<referenciales> ListarReferencialesAsignablesAccMejora(int idAcc)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<referenciales> listaReferenciales = new List<referenciales>();

            List<referenciales> registros = new List<referenciales>();

            registros = (from u in bd.referenciales
                         where !(from uc in bd.accionmejora_referencial
                                 where uc.idaccionmejora == idAcc
                                 select uc.idreferencial).Contains(u.id)
                                 && u.id != 4
                         select u).OrderBy(x => x.id).ToList();


            foreach (referenciales registro in registros)
            {
                referenciales nuevoReferencial = new referenciales();
                nuevoReferencial.id = registro.id;
                nuevoReferencial.nombre = registro.nombre;

                listaReferenciales.Add(nuevoReferencial);
            }

            return listaReferenciales;
        }

        public static List<referenciales> ListarReferencialesAsignadosAccmejora(int idAcc)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<referenciales> listaReferenciales = new List<referenciales>();

            List<referenciales> registros = new List<referenciales>();

            registros = (from u in bd.referenciales
                         where (from uc in bd.accionmejora_referencial
                                where uc.idaccionmejora == idAcc
                                select uc.idreferencial).Contains(u.id)
                                 && u.id != 4
                         select u).OrderBy(x => x.id).ToList();


            foreach (referenciales registro in registros)
            {
                referenciales nuevoReferencial = new referenciales();
                nuevoReferencial.id = registro.id;
                nuevoReferencial.nombre = registro.nombre;

                listaReferenciales.Add(nuevoReferencial);
            }

            return listaReferenciales;
        }

        public static List<tipocentral> ListarTecnologiasAsignablesObjetivo(int idObj)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<tipocentral> listaTecnologias = new List<tipocentral>();

            List<tipocentral> registros = new List<tipocentral>();

            registros = (from u in bd.tipocentral
                         where !(from uc in bd.objetivo_tecnologias
                                 where uc.idObjetivo == idObj
                                 select uc.idTecnologia).Contains(u.id)
                                 && u.id != 4
                         select u).OrderBy(x => x.id).ToList();


            foreach (tipocentral registro in registros)
            {
                tipocentral nuevaTecnologia = new tipocentral();
                nuevaTecnologia.id = registro.id;
                nuevaTecnologia.nombre = registro.nombre;

                listaTecnologias.Add(nuevaTecnologia);
            }

            return listaTecnologias;
        }

        public static List<tipocentral> ListarTecnologiasAsignadasObjetivo(int idObj)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<tipocentral> listaTecnologias = new List<tipocentral>();

            List<tipocentral> registros = new List<tipocentral>();

            registros = (from u in bd.tipocentral
                         where (from uc in bd.objetivo_tecnologias
                                where uc.idObjetivo == idObj
                                select uc.idTecnologia).Contains(u.id)
                                 && u.id != 4
                         select u).OrderBy(x => x.id).ToList();


            foreach (tipocentral registro in registros)
            {
                tipocentral nuevaTecnologia = new tipocentral();
                nuevaTecnologia.id = registro.id;
                nuevaTecnologia.nombre = registro.nombre;

                listaTecnologias.Add(nuevaTecnologia);
            }

            return listaTecnologias;
        }

        public static List<centros> ListarCentrosAsignadosObjetivo(int idObj)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<centros> listaCentros = new List<centros>();

            List<centros> registros = new List<centros>();

            registros = (from u in bd.centros
                         where (from uc in bd.objetivo_centrales
                                where uc.idObjetivo == idObj
                                select uc.idCentro).Contains(u.id)
                         select u).OrderBy(x => x.nombre).ToList();


            foreach (centros registro in registros)
            {
                centros nuevoCentro = new centros();
                nuevoCentro.id = registro.id;
                nuevoCentro.nombre = registro.nombre;
                nuevoCentro.siglas = registro.siglas;

                listaCentros.Add(nuevoCentro);
            }

            return listaCentros;
        }

        public static List<procesos> ListarProcesosAsignadosComunicacion(int idCom)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<procesos> listaProcesos = new List<procesos>();

            List<procesos> registros = new List<procesos>();

            registros = (from u in bd.procesos
                         where (from uc in bd.comunicacion_procesos
                                where uc.idcomunicacion == idCom
                                select uc.idproceso).Contains(u.id)
                         select u).OrderBy(x => x.nombre).ToList();


            foreach (procesos registro in registros)
            {
                procesos nuevoProceso = new procesos();
                nuevoProceso = registro;
                nuevoProceso.nombre = nuevoProceso.cod_proceso + " - " + nuevoProceso.nombre;
                listaProcesos.Add(nuevoProceso);
            }

            return listaProcesos;
        }

        public static List<comunicacion_documentos> ListarDocumentosComunicacion(int idCom)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<comunicacion_documentos> listaDocumentos = new List<comunicacion_documentos>();

            List<comunicacion_documentos> registros = new List<comunicacion_documentos>();

            registros = (from u in bd.comunicacion_documentos
                         where u.idcomunicacion == idCom
                         select u).OrderBy(x => x.id).ToList();


            foreach (comunicacion_documentos registro in registros)
            {
                comunicacion_documentos nuevoDocumento = new comunicacion_documentos();
                nuevoDocumento = registro;

                listaDocumentos.Add(nuevoDocumento);
            }

            return listaDocumentos;
        }

        public static List<accionmejora_documento> ListarDocumentosAccionMejora(int idCom)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<accionmejora_documento> listaDocumentos = new List<accionmejora_documento>();

            List<accionmejora_documento> registros = new List<accionmejora_documento>();

            registros = (from u in bd.accionmejora_documento
                         where u.idaccionmejora == idCom
                         select u).OrderBy(x => x.id).ToList();


            foreach (accionmejora_documento registro in registros)
            {
                accionmejora_documento nuevoDocumento = new accionmejora_documento();
                nuevoDocumento = registro;

                listaDocumentos.Add(nuevoDocumento);
            }

            return listaDocumentos;
        }

        public static List<reuniones_documentos> ListarDocumentosReunion(int idReu)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<reuniones_documentos> listaDocumentos = new List<reuniones_documentos>();

            List<reuniones_documentos> registros = new List<reuniones_documentos>();

            registros = (from u in bd.reuniones_documentos
                         where u.idreunion == idReu
                         select u).OrderBy(x => x.id).ToList();


            foreach (reuniones_documentos registro in registros)
            {
                reuniones_documentos nuevoDocumento = new reuniones_documentos();
                nuevoDocumento = registro;

                listaDocumentos.Add(nuevoDocumento);
            }

            return listaDocumentos;
        }

        public static List<evento_ambiental_documentos> ListarDocumentosEventoAmb(int idEv)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<evento_ambiental_documentos> listaDocumentos = new List<evento_ambiental_documentos>();

            List<evento_ambiental_documentos> registros = new List<evento_ambiental_documentos>();

            registros = (from u in bd.evento_ambiental_documentos
                         where u.ideventoamb == idEv
                         select u).OrderBy(x => x.id).ToList();


            foreach (evento_ambiental_documentos registro in registros)
            {
                evento_ambiental_documentos nuevoDocumento = new evento_ambiental_documentos();
                nuevoDocumento = registro;

                listaDocumentos.Add(nuevoDocumento);
            }

            return listaDocumentos;
        }

        public static List<evento_ambiental_foto> ListarFotosEventoAmb(int idEv)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<evento_ambiental_foto> listaDocumentos = new List<evento_ambiental_foto>();

            List<evento_ambiental_foto> registros = new List<evento_ambiental_foto>();

            registros = (from u in bd.evento_ambiental_foto
                         where u.idEventoAmbiental == idEv
                         select u).OrderBy(x => x.id).ToList();


            foreach (evento_ambiental_foto registro in registros)
            {
                evento_ambiental_foto nuevoDocumento = new evento_ambiental_foto();
                nuevoDocumento = registro;

                listaDocumentos.Add(nuevoDocumento);
            }

            return listaDocumentos;
        }

        public static List<evento_seguridad_foto> ListarFotosEventoSeg(int idEv)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<evento_seguridad_foto> listaDocumentos = new List<evento_seguridad_foto>();

            List<evento_seguridad_foto> registros = new List<evento_seguridad_foto>();

            registros = (from u in bd.evento_seguridad_foto
                         where u.idEventoSeg == idEv
                         select u).OrderBy(x => x.id).ToList();


            foreach (evento_seguridad_foto registro in registros)
            {
                evento_seguridad_foto nuevoDocumento = new evento_seguridad_foto();
                nuevoDocumento = registro;

                listaDocumentos.Add(nuevoDocumento);
            }

            return listaDocumentos;
        }

        public static List<evento_seguridad_documentos> ListarArchivosEventoSeg(int idEv)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<evento_seguridad_documentos> listaDocumentos = new List<evento_seguridad_documentos>();

            List<evento_seguridad_documentos> registros = new List<evento_seguridad_documentos>();

            registros = (from u in bd.evento_seguridad_documentos
                         where u.idSeg == idEv
                         select u).OrderBy(x => x.id).ToList();


            foreach (evento_seguridad_documentos registro in registros)
            {
                evento_seguridad_documentos nuevoDocumento = new evento_seguridad_documentos();
                nuevoDocumento = registro;

                listaDocumentos.Add(nuevoDocumento);
            }

            return listaDocumentos;
        }

        public static List<centros> ListarCentrosAsignablesParametro(int idParametro)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<centros> listaCentros = new List<centros>();

            List<centros> registros = new List<centros>();


            registros = (from u in bd.centros
                         where !(from uc in bd.indicadores_hojadedatos_valores
                                 where uc.CodIndiHojaDatos == idParametro &&
                                 uc.anio == 2018 && uc.baja != true
                                 select uc.idcentral).Contains(u.id)
                         select u).OrderBy(x => x.nombre).ToList();



            foreach (centros registro in registros)
            {
                centros nuevoCentro = new centros();
                nuevoCentro.id = registro.id;
                nuevoCentro.nombre = registro.nombre;
                nuevoCentro.siglas = registro.siglas;

                listaCentros.Add(nuevoCentro);
            }

            return listaCentros;
        }

        public static List<centros> ListarCentrosAsignadosParametro(int idParametro)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<centros> listaCentros = new List<centros>();

            var registros = (from u in bd.centros
                             where (from uc in bd.indicadores_hojadedatos_valores
                                    where uc.CodIndiHojaDatos == idParametro &&
                                    uc.anio == 2018 && uc.baja != true
                                    select uc.idcentral).Contains(u.id)
                             select u).OrderBy(x => x.nombre).ToList();


            foreach (centros registro in registros)
            {
                centros nuevoCentro = new centros();
                nuevoCentro.id = registro.id;
                nuevoCentro.nombre = registro.nombre;
                nuevoCentro.siglas = registro.siglas;
                nuevoCentro.tipo = registro.tipo;
                listaCentros.Add(nuevoCentro);
            }

            return listaCentros;
        }


        public static List<centros> ListarCentrosAsignables(int idUsuario, VISTA_ObtenerUsuario usuarioadm, centros cent)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<centros> listaCentros = new List<centros>();

            List<centros> registros = new List<centros>();

            if (usuarioadm.perfil == 1)
            {
                registros = (from u in bd.centros
                             where !(from uc in bd.usuario_centros
                                     where uc.idusuario == idUsuario
                                     select uc.idcentro).Contains(u.id)
                             select u).OrderBy(x => x.nombre).ToList();
            }
            else
            {

                registros = (from u in bd.centros
                             where !(from uc in bd.usuario_centros
                                     where uc.idusuario == idUsuario
                                     select uc.idcentro).Contains(u.id)
                                     && u.id == cent.id
                             select u).OrderBy(x => x.nombre).ToList();
            }


            foreach (centros registro in registros)
            {
                centros nuevoCentro = new centros();
                nuevoCentro.id = registro.id;
                nuevoCentro.nombre = registro.nombre;
                nuevoCentro.siglas = registro.siglas;

                listaCentros.Add(nuevoCentro);
            }

            return listaCentros;
        }

        public static List<centros> ListarCentrosAsignados()
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<centros> listaCentros = new List<centros>();

            var registros = (from u in bd.centros
                             where u.tipo != 4
                             select u).OrderBy(x => x.nombre).ToList();


            foreach (centros registro in registros)
            {
                centros nuevoCentro = new centros();
                nuevoCentro.id = registro.id;
                nuevoCentro.nombre = registro.nombre;
                nuevoCentro.siglas = registro.siglas;

                listaCentros.Add(nuevoCentro);
            }

            return listaCentros;
        }

        public static List<VISTA_ListarCentrosSede> ListarCentrosSedeCentral()
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<VISTA_ListarCentrosSede> listaCentros = new List<VISTA_ListarCentrosSede>();

            var registros = (from u in bd.VISTA_ListarCentrosSede
                             select u).ToList();


            foreach (VISTA_ListarCentrosSede registro in registros)
            {
                VISTA_ListarCentrosSede nuevoCentro = new VISTA_ListarCentrosSede();
                nuevoCentro.id = registro.id;
                nuevoCentro.nombre = registro.nombre;
                nuevoCentro.siglas = registro.siglas;

                listaCentros.Add(nuevoCentro);
            }

            return listaCentros;
        }

        public static List<VISTA_ListarCentrosAsignados> ListarCentrosAsignados(int idUsuario)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<VISTA_ListarCentrosAsignados> listaCentros = new List<VISTA_ListarCentrosAsignados>();

            var registros = (from u in bd.VISTA_ListarCentrosAsignados
                             where u.idusuario == idUsuario
                             select u).OrderBy(x => x.nombre).ToList();


            foreach (VISTA_ListarCentrosAsignados registro in registros)
            {
                VISTA_ListarCentrosAsignados nuevoCentro = new VISTA_ListarCentrosAsignados();
                nuevoCentro.id = registro.id;
                nuevoCentro.nombre = registro.nombre;
                nuevoCentro.siglas = registro.siglas;
                nuevoCentro.tipo = registro.tipo;
                nuevoCentro.nombre_comunidad = registro.nombre_comunidad;
                nuevoCentro.permiso = registro.permiso;
                listaCentros.Add(nuevoCentro);
            }

            return listaCentros;
        }

        public static noticias GetNoticia(int idNoticia)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            noticias nuevo = new noticias();

            var registro = (from u in bd.noticias
                            where u.id == idNoticia
                            select u).FirstOrDefault();


            if (registro != null)
            {
                nuevo.id = registro.id;
                nuevo.texto = registro.texto;
                nuevo.titulo = registro.titulo;
                nuevo.fecha = registro.fecha;
                nuevo.fechaexp = registro.fechaexp;
                nuevo.organizacion = registro.organizacion;
                nuevo.cabecera = registro.cabecera;
            }
            else
                nuevo = null;


            return nuevo;
        }

        public static procesos GetDatosProceso(int idProceso)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            procesos nuevo = new procesos();

            var registro = (from u in bd.procesos
                            where u.id == idProceso
                            select u).FirstOrDefault();


            if (registro != null)
            {
                nuevo.id = registro.id;
                nuevo.cod_proceso = registro.cod_proceso;
                nuevo.nombre = registro.nombre;
                nuevo.descripcion = registro.descripcion;
                nuevo.alcance = registro.alcance;
                nuevo.objetivos = registro.objetivos;
                nuevo.tipo = registro.tipo;
                nuevo.tecnologia = registro.tecnologia;
            }
            else
                nuevo = null;


            return nuevo;
        }

        public static objetivos GetDatosObjetivo(int idObjetivo)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            objetivos nuevo = new objetivos();

            var registro = (from u in bd.objetivos
                            where u.id == idObjetivo
                            select u).FirstOrDefault();


            if (registro != null)
            {
                nuevo = registro;
            }
            else
                nuevo = null;


            return nuevo;
        }

        public static comunicacion GetDatosComunicacion(int idComunicacion)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            comunicacion nuevo = new comunicacion();

            var registro = (from u in bd.comunicacion
                            where u.id == idComunicacion
                            select u).FirstOrDefault();


            if (registro != null)
            {
                nuevo = registro;
            }
            else
                nuevo = null;


            return nuevo;
        }

        public static partes GetDatosParte(int idParte)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            partes nuevo = new partes();

            var registro = (from u in bd.partes
                            where u.id == idParte
                            select u).FirstOrDefault();


            if (registro != null)
            {
                nuevo = registro;
            }
            else
                nuevo = null;


            return nuevo;
        }

        public static evento_ambiental GetDatosEventoAmb(int idEvento)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            evento_ambiental nuevo = new evento_ambiental();

            var registro = (from u in bd.evento_ambiental
                            where u.id == idEvento
                            select u).FirstOrDefault();


            if (registro != null)
            {
                nuevo = registro;
            }
            else
                nuevo = null;


            return nuevo;
        }


        public static evento_seguridad GetDatosEventoSeg(int idEvento)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            evento_seguridad nuevo = new evento_seguridad();

            var registro = (from u in bd.evento_seguridad
                            where u.id == idEvento
                            select u).FirstOrDefault();


            if (registro != null)
            {
                nuevo = registro;
            }
            else
                nuevo = null;


            return nuevo;
        }

        public static List<evento_seguridad_tipo> GetDatosEventoSegTipos()
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<evento_seguridad_tipo> nuevo = new List<evento_seguridad_tipo>();

            var registro = (from u in bd.evento_seguridad_tipo
                            select u).ToList();


            if (registro != null)
            {
                nuevo = registro;
            }
            else
                nuevo = null;


            return nuevo;
        }

        public static List<evento_seguridad_severidad> GetDatosEventoSegSeveridades()
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<evento_seguridad_severidad> nuevo = new List<evento_seguridad_severidad>();

            var registro = (from u in bd.evento_seguridad_severidad
                            orderby u.severidad ascending
                            select u).ToList();


            if (registro != null)
            {
                nuevo = registro;
            }
            else
                nuevo = null;


            return nuevo;
        }

        public static List<evento_seguridad_tipoevento> GetDatosEventoSegTiposEven()
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<evento_seguridad_tipoevento> nuevo = new List<evento_seguridad_tipoevento>();

            var registro = (from u in bd.evento_seguridad_tipoevento
                            select u).ToList();


            if (registro != null)
            {
                nuevo = registro;
            }
            else
                nuevo = null;


            return nuevo;
        }

        public static List<evento_seguridad_subtipoevento> GetDatosEventoSegSubtiposEven()
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<evento_seguridad_subtipoevento> nuevo = new List<evento_seguridad_subtipoevento>();

            var registro = (from u in bd.evento_seguridad_subtipoevento
                            select u).ToList();


            if (registro != null)
            {
                nuevo = registro;
            }
            else
                nuevo = null;


            return nuevo;
        }

        public static List<evento_ambiental_tipo> GetDatosEventoAmbTipos()
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<evento_ambiental_tipo> nuevo = new List<evento_ambiental_tipo>();

            var registro = (from u in bd.evento_ambiental_tipo
                            select u).ToList();


            if (registro != null)
            {
                nuevo = registro;
            }
            else
                nuevo = null;


            return nuevo;
        }

        public static List<evento_ambiental_matriz> GetDatosEventoAmbMatrices()
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<evento_ambiental_matriz> nuevo = new List<evento_ambiental_matriz>();

            var registro = (from u in bd.evento_ambiental_matriz
                            select u).ToList();


            if (registro != null)
            {
                nuevo = registro;
            }
            else
                nuevo = null;


            return nuevo;
        }

        public static evento_calidad GetDatosEventoCal(int idEvento)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            evento_calidad nuevo = new evento_calidad();

            var registro = (from u in bd.evento_calidad
                            where u.id == idEvento
                            select u).FirstOrDefault();


            if (registro != null)
            {
                nuevo = registro;
            }
            else
                nuevo = null;


            return nuevo;
        }

        public static formacion GetDatosFormacion(int idFormacion)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            formacion nuevo = new formacion();

            var registro = (from u in bd.formacion
                            where u.id == idFormacion
                            select u).FirstOrDefault();

            if (registro != null)
            {
                nuevo = registro;
            }
            else
                nuevo = null;

            return nuevo;
        }

        public static revision_energetica GetDatosRevision(int idRevision)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            revision_energetica nuevo = new revision_energetica();

            var registro = (from u in bd.revision_energetica
                            where u.id == idRevision
                            select u).FirstOrDefault();

            if (registro != null)
            {
                nuevo = registro;
            }
            else
                nuevo = null;

            return nuevo;
        }

        public static emergencias GetDatosEmergencia(int idFormacion)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            emergencias nuevo = new emergencias();

            var registro = (from u in bd.emergencias
                            where u.id == idFormacion
                            select u).FirstOrDefault();

            if (registro != null)
            {
                nuevo = registro;
            }
            else
                nuevo = null;

            return nuevo;
        }

        public static normas GetDatosNorma(int idNorma)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            normas nuevo = new normas();

            var registro = (from u in bd.normas
                            where u.id == idNorma
                            select u).FirstOrDefault();

            if (registro != null)
            {
                nuevo = registro;
            }
            else
                nuevo = null;

            return nuevo;
        }

        public static enlaces GetDatosEnlace(int idEnlace)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            enlaces nuevo = new enlaces();

            var registro = (from u in bd.enlaces
                            where u.id == idEnlace
                            select u).FirstOrDefault();

            if (registro != null)
            {
                nuevo = registro;
            }
            else
                nuevo = null;

            return nuevo;
        }

        public static materialdivulgativo GetDatosMaterial(int idEnlace)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            materialdivulgativo nuevo = new materialdivulgativo();

            var registro = (from u in bd.materialdivulgativo
                            where u.id == idEnlace
                            select u).FirstOrDefault();

            if (registro != null)
            {
                nuevo = registro;
            }
            else
                nuevo = null;

            return nuevo;
        }

        public static informesseguridad GetDatosInforme(int idEnlace)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            informesseguridad nuevo = new informesseguridad();

            var registro = (from u in bd.informesseguridad
                            where u.id == idEnlace
                            select u).FirstOrDefault();

            if (registro != null)
            {
                nuevo = registro;
            }
            else
                nuevo = null;

            return nuevo;
        }

        public static evaluacionriesgos GetDatosEvaluaciones(int idEnlace)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            evaluacionriesgos nuevo = new evaluacionriesgos();

            var registro = (from u in bd.evaluacionriesgos
                            where u.id == idEnlace
                            select u).FirstOrDefault();

            if (registro != null)
            {
                nuevo = registro;
            }
            else
                nuevo = null;

            return nuevo;
        }

        public static accionesmejora GetDatosAccionMejora(int idAccion)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            accionesmejora nuevo = new accionesmejora();

            var registro = (from u in bd.accionesmejora
                            where u.id == idAccion
                            select u).FirstOrDefault();

            if (registro != null)
            {
                nuevo = registro;
            }
            else
                nuevo = null;

            return nuevo;
        }

        public static Vista_AccionMejoraFicha GetDatosAccionMejoraFicha(int idAccion)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            Vista_AccionMejoraFicha nuevo = new Vista_AccionMejoraFicha();

            var registro = (from u in bd.Vista_AccionMejoraFicha
                            where u.id == idAccion
                            select u).FirstOrDefault();

            if (registro != null)
            {
                nuevo = registro;
            }
            else
                nuevo = null;

            return nuevo;
        }

        public static satisfaccion GetDatosSatisfaccion(int idSatisfaccion)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            satisfaccion nuevo = new satisfaccion();

            var registro = (from u in bd.satisfaccion
                            where u.id == idSatisfaccion
                            select u).FirstOrDefault();

            if (registro != null)
            {
                nuevo = registro;
            }
            else
                nuevo = null;

            return nuevo;
        }

        public static Riesgos GetDatosRiesgo(int idRiesgo)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            Riesgos nuevo = new Riesgos();

            var registro = (from u in bd.Riesgos
                            where u.Id == idRiesgo
                            select u).FirstOrDefault();

            if (registro != null)
            {
                nuevo = registro;
            }
            else
                nuevo = null;

            return nuevo;
        }

        public static VISTA_ListarEmergencias GetDatosEmergenciaFicha(int idEmergencia)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            VISTA_ListarEmergencias nuevo = new VISTA_ListarEmergencias();

            var registro = (from u in bd.VISTA_ListarEmergencias
                            where u.id == idEmergencia
                            select u).FirstOrDefault();

            if (registro != null)
            {
                nuevo = registro;
            }
            else
                nuevo = null;

            return nuevo;
        }

        public static VISTA_ListarSatisfaccion GetDatosSatisfaccionFicha(int idSatisfaccion)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            VISTA_ListarSatisfaccion nuevo = new VISTA_ListarSatisfaccion();

            var registro = (from u in bd.VISTA_ListarSatisfaccion
                            where u.id == idSatisfaccion
                            select u).FirstOrDefault();

            if (registro != null)
            {
                nuevo = registro;
            }
            else
                nuevo = null;

            return nuevo;
        }

        public static VISTA_ListarRevisionesEnergeticas GetDatosRevisionFicha(int idRevision)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            VISTA_ListarRevisionesEnergeticas nuevo = new VISTA_ListarRevisionesEnergeticas();

            var registro = (from u in bd.VISTA_ListarRevisionesEnergeticas
                            where u.id == idRevision
                            select u).FirstOrDefault();

            if (registro != null)
            {
                nuevo = registro;
            }
            else
                nuevo = null;

            return nuevo;
        }

        public static requisitoslegales GetDatosRequisito(int idRequisito)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            requisitoslegales nuevo = new requisitoslegales();

            var registro = (from u in bd.requisitoslegales
                            where u.id == idRequisito
                            select u).FirstOrDefault();

            if (registro != null)
            {
                nuevo = registro;
            }
            else
                nuevo = null;

            return nuevo;
        }

        public static comunicacion_documentos GetDatosDocComunicacion(int idDocComunicacion)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            comunicacion_documentos nuevo = new comunicacion_documentos();

            var registro = (from u in bd.comunicacion_documentos
                            where u.id == idDocComunicacion
                            select u).FirstOrDefault();

            if (registro != null)
            {
                nuevo = registro;
            }
            else
                nuevo = null;

            return nuevo;
        }

        public static accionmejora_documento GetDatosDocAccMejora(int idDocAccMejora)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            accionmejora_documento nuevo = new accionmejora_documento();

            var registro = (from u in bd.accionmejora_documento
                            where u.id == idDocAccMejora
                            select u).FirstOrDefault();

            if (registro != null)
            {
                nuevo = registro;
            }
            else
                nuevo = null;

            return nuevo;
        }

        public static reuniones_documentos GetDatosDocReunion(int idDocReunion)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            reuniones_documentos nuevo = new reuniones_documentos();

            var registro = (from u in bd.reuniones_documentos
                            where u.id == idDocReunion
                            select u).FirstOrDefault();

            if (registro != null)
            {
                nuevo = registro;
            }
            else
                nuevo = null;

            return nuevo;
        }

        public static evento_ambiental_documentos GetDatosDocEventoAmb(int idDocEventoAmb)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            evento_ambiental_documentos nuevo = new evento_ambiental_documentos();

            var registro = (from u in bd.evento_ambiental_documentos
                            where u.id == idDocEventoAmb
                            select u).FirstOrDefault();

            if (registro != null)
            {
                nuevo = registro;
            }
            else
                nuevo = null;

            return nuevo;
        }

        public static evento_seguridad_documentos GetDatosDocEventoSeg(int idDocEventoSeg)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            evento_seguridad_documentos nuevo = new evento_seguridad_documentos();

            var registro = (from u in bd.evento_seguridad_documentos
                            where u.id == idDocEventoSeg
                            select u).FirstOrDefault();

            if (registro != null)
            {
                nuevo = registro;
            }
            else
                nuevo = null;

            return nuevo;
        }

        public static cualificaciones GetDatosCualificacion(int idCualificacion)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            cualificaciones nuevo = new cualificaciones();

            var registro = (from u in bd.cualificaciones
                            where u.id == idCualificacion
                            select u).FirstOrDefault();

            if (registro != null)
            {
                nuevo = registro;
            }
            else
                nuevo = null;

            return nuevo;
        }

        public static auditorias GetDatosAuditoria(int idAuditoria)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            auditorias nuevo = new auditorias();

            var registro = (from u in bd.auditorias
                            where u.id == idAuditoria
                            select u).FirstOrDefault();

            if (registro != null)
            {
                nuevo = registro;
            }
            else
                nuevo = null;

            return nuevo;
        }

        public static auditorias_programa GetProgramaAuditoria(int idPrograma)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            auditorias_programa nuevo = new auditorias_programa();

            var registro = (from u in bd.auditorias_programa
                            where u.id == idPrograma
                            select u).FirstOrDefault();

            if (registro != null)
            {
                nuevo = registro;
            }
            else
                nuevo = null;

            return nuevo;
        }

        public static documentos_programa GetProgramaDocumentos(int idPrograma)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            documentos_programa nuevo = new documentos_programa();

            var registro = (from u in bd.documentos_programa
                            where u.id == idPrograma
                            select u).FirstOrDefault();

            if (registro != null)
            {
                nuevo = registro;
            }
            else
                nuevo = null;

            return nuevo;
        }


        public static int InsertarDocumentoHistorico(documento_historico objeto)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            if (objeto != null)
            {
                // set new value 
                bd.documento_historico.Add(objeto);
                // save changeds
                bd.SaveChanges();
            }
            return objeto.id;

            //DIMASSTEntities bd = new DIMASSTEntities();

            //documento_historico actualizar = new documento_historico();

            ////1 = Definitivo
            //if (objeto.descarga == 1)
            //{
            //    actualizar = (from u in bd.documento_historico
            //                  where u.id_centro == objeto.id_centro && u.descarga == 1
            //                  select u).OrderByDescending(u => u.id).FirstOrDefault();
            //}
            //else
            //{
            //    actualizar = (from u in bd.documento_historico
            //                  where u.id_centro == objeto.id_centro && u.descarga == 0
            //                  select u).OrderByDescending(u => u.id).FirstOrDefault();

            //}

            //if (actualizar != null)
            //{
            //    actualizar.nombre = objeto.nombre;
            //    actualizar.tipo = objeto.tipo;
            //    actualizar.version = objeto.version;
            //    actualizar.estado = objeto.estado;
            //    actualizar.fechaUltimaModificacion = DateTime.Now;
            //    actualizar.usuario = objeto.usuario;
            //    actualizar.descarga = objeto.descarga;
            //    actualizar.ruta = objeto.ruta;
            //    actualizar.id_centro = objeto.id_centro;

            //    bd.SaveChanges();

            //    return actualizar.id;
            //}
            //else
            //{
            //    documento_historico insertar = new documento_historico();
            //    insertar.nombre = objeto.nombre;
            //    insertar.tipo = objeto.tipo;
            //    insertar.version = objeto.version;
            //    insertar.estado = objeto.estado;
            //    insertar.fechaUltimaModificacion = DateTime.Now;
            //    insertar.usuario = objeto.usuario;
            //    insertar.descarga = objeto.descarga;
            //    insertar.ruta = objeto.ruta;
            //    insertar.id_centro = objeto.id_centro;

            //    bd.documento_historico.Add(insertar);
            //    bd.SaveChanges();

            //    return insertar.id;
            //}

            //    documento_historico insertar = new documento_historico();
            //    insertar.nombre = objeto.nombre;
            //    insertar.tipo = objeto.tipo;
            //    insertar.version = objeto.version;
            //    insertar.estado = objeto.estado;
            //    insertar.fechaUltimaModificacion = DateTime.Now;
            //    insertar.usuario = objeto.usuario;
            //    insertar.descarga = objeto.descarga;
            //    insertar.ruta = objeto.ruta;
            //    insertar.id_centro = objeto.id_centro;

            //    bd.documento_historico.Add(insertar);
            //    bd.SaveChanges();

            //    return insertar.id;
            //}

        }
        public static List<aspecto_tipo> getAspectosTipo()
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            List<aspecto_tipo> registro = (from u in bd.aspecto_tipo
                                           join uu in bd.aspecto_parametro on u.id equals uu.id_aspecto
                                           where u.id == 14 || u.id == 15 || u.id == 16
                                           select u).ToList();

            return registro;
        }

        public static List<VISTA_ListarParametrosTipoAspecto> getParametrosAmbientalesInv()
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            List<VISTA_ListarParametrosTipoAspecto> registro =
                (from u in bd.VISTA_ListarParametrosTipoAspecto
                 where u.id_aspecto == 1 || u.id_aspecto == 6 || u.id_aspecto == 14 || u.id_aspecto == 15 || u.id_aspecto == 16 || u.id_aspecto == 17 || u.id_aspecto == 18
                 select u).ToList();

            return registro;
        }
        public static aspecto_parametro getParametrosAmbientalesInv(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            aspecto_parametro registro =
                (from u in bd.aspecto_parametro where u.id_parametro == id select u).FirstOrDefault();

            return registro;
        }
        public static string GeneraNumeroRandom()
        {
            var guid = Guid.NewGuid();
            var justNumbers = new String(guid.ToString().Where(Char.IsDigit).ToArray());
            var seed = int.Parse(justNumbers.Substring(0, 4));

            return seed.ToString();
        }

        public static int ActualizarParametroAmbiental(aspecto_parametro oParametro)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.aspecto_parametro
                              where u.id_parametro == oParametro.id_parametro
                              select u).FirstOrDefault();

            if (actualizar != null)
            {

                actualizar.id_aspecto = oParametro.id_aspecto;
                actualizar.nombre = oParametro.nombre;

                bd.SaveChanges();
            }
            else
            {
                actualizar = oParametro;
                bd.aspecto_parametro.Add(oParametro);
                bd.SaveChanges();
            }
            return actualizar.id_parametro;
        }

        public static void eliminarParametroAmbiental(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var AspectoTipo = bd.aspecto_parametro.Where(x => x.id_parametro == id).FirstOrDefault();

            if (AspectoTipo != null)
            {
                bd.aspecto_parametro.Remove(AspectoTipo);
                bd.SaveChanges();
            }


        }

        public static indicadores_planificacion GetDatosPlanificacion(int idIndicador)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            indicadores_planificacion nuevo = new indicadores_planificacion();

            var registro = (from u in bd.indicadores_planificacion
                            where u.Id == idIndicador
                            select u).FirstOrDefault();

            if (registro != null)
            {
                nuevo = registro;
            }
            else
                nuevo = null;

            return nuevo;
        }

        public static aspecto_valoracion GetDatosValoracion(int idValoracion)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            aspecto_valoracion nuevo = new aspecto_valoracion();

            var registro = (from u in bd.aspecto_valoracion
                            where u.id == idValoracion
                            select u).FirstOrDefault();

            if (registro != null)
            {
                nuevo = registro;
            }
            else
                nuevo = null;

            return nuevo;
        }

        public static aspecto_parametro_valoracion GetDatosParametro(int idValoracion)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            aspecto_parametro_valoracion nuevo = new aspecto_parametro_valoracion();

            var registro = (from u in bd.aspecto_parametro_valoracion
                            where u.id == idValoracion
                            select u).FirstOrDefault();

            if (registro != null)
            {
                nuevo = registro;
            }
            else
                nuevo = null;

            return nuevo;
        }

        public static List<VISTA_ListadoIndicadores> GetInformeIndicador(int idCentral, int anio)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            List<VISTA_ListadoIndicadores> nuevo = new List<VISTA_ListadoIndicadores>();

            nuevo = (from u in bd.VISTA_ListadoIndicadores
                     where u.idcentral == idCentral && u.anio == anio
                     select u).ToList();

            return nuevo;
        }

        public static reuniones GetDatosReunion(int idReunion)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            reuniones nuevo = new reuniones();

            var registro = (from u in bd.reuniones
                            where u.id == idReunion
                            select u).FirstOrDefault();

            if (registro != null)
            {
                nuevo = registro;
            }
            else
                nuevo = null;

            return nuevo;
        }

        public static bool comprobarReferencias(int id, int idCentral)
        {
            bool asignado = false;

            DIMASSTEntities bd = new DIMASSTEntities();

            List<objetivos> registros = new List<objetivos>();

            registros = (from u in bd.objetivos
                         where u.idorganizacion == idCentral && u.idReferencia == id
                         select u).ToList();

            if (registros.Count > 0)
            {
                asignado = true;
            }

            return asignado;

        }


        public static List<VISTA_Objetivos> ListarObjetivos(int tipo, int idCentral)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<VISTA_Objetivos> listaObjetivos = new List<VISTA_Objetivos>();

            List<VISTA_Objetivos> registros = new List<VISTA_Objetivos>();

            centros cent = ObtenerCentroPorID(idCentral);

            int tipocentral = int.Parse(cent.tipo.ToString());

            if (tipo == 0 && cent.tipo != 4)
            {
                registros = (from u in bd.VISTA_Objetivos
                             where u.Tipo == tipo && u.idorganizacion == 0
                             && (((u.especifico == 1 || u.especifico == 2) && ((from oc in bd.objetivo_centrales where oc.idObjetivo == u.id select oc.idCentro).Contains(idCentral)) || ((from oc in bd.objetivo_tecnologias where oc.idObjetivo == u.id select oc.idTecnologia).Contains(tipocentral)))
                             || ((u.especifico == null)))
                             orderby u.id descending
                             select u).ToList();
            }
            else
            {
                if (cent.id == 25)
                {
                    registros = (from u in bd.VISTA_Objetivos
                                 where u.Tipo == 0 && (u.idorganizacion == 0 || u.idorganizacion == idCentral)
                                 orderby u.id descending
                                 select u).ToList();
                }
                else
                {
                    registros = (from u in bd.VISTA_Objetivos
                                 where u.Tipo == tipo && (u.idorganizacion == 0 || u.idorganizacion == idCentral)
                                 orderby u.id descending
                                 select u).ToList();
                }

            }

            if (tipo == 2)
            {
                registros = (from u in bd.VISTA_Objetivos
                             where u.siglas == cent.siglas
                             orderby u.id descending
                             select u).ToList();
            }


            foreach (VISTA_Objetivos registro in registros)
            {
                VISTA_Objetivos nuevoObjetivo = registro;
                listaObjetivos.Add(nuevoObjetivo);
            }

            return listaObjetivos;
        }

        public static List<VISTA_Objetivos> ListarObjetivosFechas(int idCentral, string anio)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<VISTA_Objetivos> listaObjetivos = new List<VISTA_Objetivos>();

            List<VISTA_Objetivos> registros = new List<VISTA_Objetivos>();

            centros cent = ObtenerCentroPorID(idCentral);

            int tipocentral = int.Parse(cent.tipo.ToString());

            registros = (from u in bd.VISTA_Objetivos
                         where u.idorganizacion == idCentral &&
                         u.Codigo.Contains(anio)
                         && u.nombre_ambito == "Planificación actividad preventiva"
                         orderby u.id ascending
                         select u).ToList();

            foreach (VISTA_Objetivos registro in registros)
            {
                VISTA_Objetivos nuevoObjetivo = registro;
                listaObjetivos.Add(nuevoObjetivo);
            }

            return listaObjetivos;
        }

        public static List<VISTA_Riesgos> ListarRiesgos(int idCentral)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<VISTA_Riesgos> listaRiesgos = new List<VISTA_Riesgos>();

            List<VISTA_Riesgos> registros = new List<VISTA_Riesgos>();

            centros cent = ObtenerCentroPorID(idCentral);

            int tipocentral = int.Parse(cent.tipo.ToString());

            registros = (from u in bd.VISTA_Riesgos
                         where u.idCentral == idCentral
                         select u).ToList();



            foreach (VISTA_Riesgos registro in registros)
            {
                VISTA_Riesgos nuevoRiesgo = registro;
                listaRiesgos.Add(nuevoRiesgo);
            }

            return listaRiesgos;
        }

        public static List<riesgos_medidas> ListarRiesgosMedidas()
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<riesgos_medidas> listaRiesgos = new List<riesgos_medidas>();

            List<riesgos_medidas> registros = new List<riesgos_medidas>();

            registros = (from u in bd.riesgos_medidas
                         select u).ToList();

            foreach (riesgos_medidas registro in registros)
            {
                riesgos_medidas nuevoRiesgo = registro;
                listaRiesgos.Add(nuevoRiesgo);
            }

            return listaRiesgos;
        }

        public static List<tipos_riesgos> ListarRiesgos()
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<tipos_riesgos> listaRiesgos = new List<tipos_riesgos>();

            List<tipos_riesgos> registros = new List<tipos_riesgos>();

            registros = (from u in bd.tipos_riesgos
                         select u).ToList();



            foreach (tipos_riesgos registro in registros)
            {
                tipos_riesgos nuevoRiesgo = registro;
                listaRiesgos.Add(nuevoRiesgo);
            }

            return listaRiesgos;
        }
        public static tipos_riesgos DameRiesgo(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            tipos_riesgos registros = new tipos_riesgos();
            registros = (from u in bd.tipos_riesgos
                         where u.id == id
                         select u).FirstOrDefault();
            return registros;
        }

        public static async Task<List<VISTA_tipos_riesgos>> ListarTiposRiesgosAsync()
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            //List<VISTA_tipos_riesgos> listaRiesgos = new List<VISTA_tipos_riesgos>();

            List<VISTA_tipos_riesgos> registros = new List<VISTA_tipos_riesgos>();
            registros = await (from u in bd.VISTA_tipos_riesgos select u).ToListAsync();

            //foreach (VISTA_tipos_riesgos registro in registros)
            //{
            //    VISTA_tipos_riesgos nuevoRiesgo = registro;
            //    listaRiesgos.Add(nuevoRiesgo);
            //}

            return registros;
        }
        public static List<VISTA_tipos_riesgos> ListarTiposRiesgos()
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            //List<VISTA_tipos_riesgos> listaRiesgos = new List<VISTA_tipos_riesgos>();

            List<VISTA_tipos_riesgos> registros = new List<VISTA_tipos_riesgos>();
            registros = (from u in bd.VISTA_tipos_riesgos select u).ToList();

            //foreach (VISTA_tipos_riesgos registro in registros)
            //{
            //    VISTA_tipos_riesgos nuevoRiesgo = registro;
            //    listaRiesgos.Add(nuevoRiesgo);
            //}

            return registros;
        }
        public static List<VISTA_tipo_riesgos_critico> ListarTiposRiesgosCriticosVista()
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            //List<VISTA_tipos_riesgos> listaRiesgos = new List<VISTA_tipos_riesgos>();

            List<VISTA_tipo_riesgos_critico> registros = new List<VISTA_tipo_riesgos_critico>();
            registros = (from u in bd.VISTA_tipo_riesgos_critico select u).ToList();

            //foreach (VISTA_tipos_riesgos registro in registros)
            //{
            //    VISTA_tipos_riesgos nuevoRiesgo = registro;
            //    listaRiesgos.Add(nuevoRiesgo);
            //}

            return registros;
        }
        public static List<VISTA_tipo_riesgos_critico> ListarVistaTiposRiesgosCriticos()
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            //List<VISTA_tipos_riesgos> listaRiesgos = new List<VISTA_tipos_riesgos>();

            List<VISTA_tipo_riesgos_critico> registros = new List<VISTA_tipo_riesgos_critico>();
            registros = (from u in bd.VISTA_tipo_riesgos_critico select u).ToList();

            //foreach (VISTA_tipos_riesgos registro in registros)
            //{
            //    VISTA_tipos_riesgos nuevoRiesgo = registro;
            //    listaRiesgos.Add(nuevoRiesgo);
            //}

            return registros;
        }
        public static List<tecnologias_tiposRiesgosCriticos> ObtenerListaTecnologias_TiposRiesgosCriticos(int tecno)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<tecnologias_tiposRiesgosCriticos> registros = new List<tecnologias_tiposRiesgosCriticos>();
            registros = (from u in bd.tecnologias_tiposRiesgosCriticos where u.id_tecnologia == tecno select u).ToList();


            return registros;
        }


        public static List<tipo_riesgos_critico> ListarTiposRiesgosCriticos()
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<tipo_riesgos_critico> registros = new List<tipo_riesgos_critico>();
            registros = (from u in bd.tipo_riesgos_critico select u).ToList();


            return registros;
        }

        public static int obtenerUltimaVersionCentro(int idCentro)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            int? version = (from u in bd.version_matriz
                            where u.id_centro == idCentro
                            select u.id).Max();

            if (version == null)
            {
                return 0;
            }
            else
            {
                return (int)version;
            }

        }
        public static int obtenerUltimaVersionMatrizFinalizadaCentro(int idCentro)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            //List<version_matriz> version_Matriz = new List<version_matriz>();
            version_matriz version_Matriz = new version_matriz();

            version_Matriz = (from u in bd.version_matriz
                              orderby u.id descending
                              where u.id_centro == idCentro
                              select u).ToList().FirstOrDefault();

            var idVersion = 0;
            if (version_Matriz != null)
            {
                idVersion = version_Matriz.estado == 0 ? version_Matriz.id : 0;
            }

            return idVersion;
        }


        public static bool EstaVersionEsBorrador(int centro)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            var versionBD = (from u in bd.version_matriz
                             where u.id_centro == centro
                             && u.estado == 1
                             select u).FirstOrDefault();


            if (versionBD == null)
            {
                return false;
            }
            else
            {
                if (versionBD.estado == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }




        public static List<tipos_riesgos> ListarTiposRiesgosMedidas(int idCentro)
        {

            DIMASSTEntities bd = new DIMASSTEntities();
            List<tipos_riesgos> riesgos = (from r in bd.tipos_riesgos
                                           select r).ToList();
            List<tipos_riesgos> listaRiesgos = new List<tipos_riesgos>();

            List<int> registros = new List<int>();
            registros = (from u in bd.matriz_centro
                         where u.id_centro == idCentro && u.activo == true && u.version == (from v in bd.matriz_centro where v.id_centro == idCentro select v.version).Max()
                         select u.id_riesgo).Distinct().ToList();

            foreach (tipos_riesgos riesgo in riesgos)
            {
                if (registros.Contains(riesgo.id))
                    listaRiesgos.Add(riesgo);
            }

            return listaRiesgos;
        }

        public static List<tipo_riesgos_critico> ListarTiposRiesgosInerentesMedidasCriticos(int idCentro)
        {

            DIMASSTEntities bd = new DIMASSTEntities();
            List<tipo_riesgos_critico> riesgos = (from r in bd.tipo_riesgos_critico
                                                  where r.id < 29
                                                  select r).ToList();
            List<tipo_riesgos_critico> listaRiesgos = new List<tipo_riesgos_critico>();

            List<int> registros = new List<int>();
            registros = (from u in bd.matriz_centro
                         where u.id_centro == idCentro && u.activo == true && u.version == (from v in bd.matriz_centro where v.id_centro == idCentro select v.version).Max()
                         select u.id_riesgo).Distinct().ToList();

            foreach (tipo_riesgos_critico riesgo in riesgos)
            {
                if (registros.Contains(riesgo.id))
                    listaRiesgos.Add(riesgo);
            }

            return listaRiesgos;
        }

        public static List<tipos_riesgos> ListarTiposRiesgosInerentesMedidas(int idCentro)
        {

            DIMASSTEntities bd = new DIMASSTEntities();
            List<tipos_riesgos> riesgos = (from r in bd.tipos_riesgos
                                           where r.id < 29
                                           select r).ToList();
            List<tipos_riesgos> listaRiesgos = new List<tipos_riesgos>();

            List<int> registros = new List<int>();
            registros = (from u in bd.matriz_centro
                         where u.id_centro == idCentro && u.activo == true && u.version == (from v in bd.matriz_centro where v.id_centro == idCentro select v.version).Max()
                         select u.id_riesgo).Distinct().ToList();

            foreach (tipos_riesgos riesgo in riesgos)
            {
                if (registros.Contains(riesgo.id))
                    listaRiesgos.Add(riesgo);
            }

            return listaRiesgos;
        }

        public static List<tipo_riesgos_critico> ListarTiposRiesgosCriticosMedidasCriticos(int idCentro)
        {

            DIMASSTEntities bd = new DIMASSTEntities();
            List<tipo_riesgos_critico> riesgos = (from r in bd.tipo_riesgos_critico
                                                  where r.id > 28
                                                  select r).ToList();
            List<tipo_riesgos_critico> listaRiesgos = new List<tipo_riesgos_critico>();

            List<int> registros = new List<int>();
            registros = (from u in bd.matriz_centro
                         where u.id_centro == idCentro && u.activo == true && u.version == (from v in bd.matriz_centro where v.id_centro == idCentro select v.version).Max()
                         select u.id_riesgo).Distinct().ToList();

            foreach (tipo_riesgos_critico riesgo in riesgos)
            {
                if (registros.Contains(riesgo.id))
                    listaRiesgos.Add(riesgo);
            }

            return listaRiesgos;
        }

        public static List<tipos_riesgos> ListarTiposRiesgosCriticosMedidas(int idCentro)
        {

            DIMASSTEntities bd = new DIMASSTEntities();
            List<tipos_riesgos> riesgos = (from r in bd.tipos_riesgos
                                           where r.id > 28
                                           select r).ToList();
            List<tipos_riesgos> listaRiesgos = new List<tipos_riesgos>();

            List<int> registros = new List<int>();
            registros = (from u in bd.matriz_centro
                         where u.id_centro == idCentro && u.activo == true && u.version == (from v in bd.matriz_centro where v.id_centro == idCentro select v.version).Max()
                         select u.id_riesgo).Distinct().ToList();

            foreach (tipos_riesgos riesgo in riesgos)
            {
                if (registros.Contains(riesgo.id))
                    listaRiesgos.Add(riesgo);
            }

            return listaRiesgos;
        }

        public static List<int> ListarTiposRiesgosMedidasEntero(int idCentro)
        {

            DIMASSTEntities bd = new DIMASSTEntities();
            List<tipos_riesgos> riesgos = (from r in bd.tipos_riesgos
                                           select r).ToList();
            List<int> listaRiesgos = new List<int>();

            List<int> registros = new List<int>();
            registros = (from u in bd.matriz_centro
                         where u.id_centro == idCentro && u.activo == true && u.version == (from v in bd.matriz_centro where v.id_centro == idCentro select v.version).Max()
                         select u.id_riesgo).Distinct().ToList();

            foreach (tipos_riesgos riesgo in riesgos)
            {
                if (registros.Contains(riesgo.id))
                    listaRiesgos.Add(riesgo.id);
            }

            return listaRiesgos;
        }
        public static List<riesgos_situaciones> ListarSituaciones()
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<riesgos_situaciones> listaRiesgos = new List<riesgos_situaciones>();

            List<riesgos_situaciones> registros = new List<riesgos_situaciones>();
            registros = (from u in bd.riesgos_situaciones
                         select u).ToList();

            foreach (riesgos_situaciones registro in registros)
            {
                riesgos_situaciones nuevoRiesgo = registro;
                listaRiesgos.Add(nuevoRiesgo);
            }

            return listaRiesgos;
        }
        public static List<medidas_preventivas> ListarMedidasPorID_CENTRO(int id_centro)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<medidas_preventivas> listaRiesgos = new List<medidas_preventivas>();

            List<medidas_preventivas> registros = new List<medidas_preventivas>();
            registros = (from u in bd.medidas_preventivas
                         where u.id_centro == id_centro
                         select u).ToList();

            foreach (medidas_preventivas registro in registros)
            {
                medidas_preventivas nuevoRiesgo = registro;
                listaRiesgos.Add(nuevoRiesgo);
            }

            return listaRiesgos;
        }
        public static List<riesgos_medidas> ListarRiesgosMedidasPorID_CENTRO(int id_centro)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<riesgos_medidas> listaRiesgos = new List<riesgos_medidas>();

            List<riesgos_medidas> registros = new List<riesgos_medidas>();
            registros = (from u in bd.riesgos_medidas
                         where u.id_centro == id_centro
                         select u).ToList();

            foreach (riesgos_medidas registro in registros)
            {
                riesgos_medidas nuevoRiesgo = registro;
                listaRiesgos.Add(nuevoRiesgo);
            }

            return listaRiesgos;
        }
        public static List<medidas_generales> ListarMedidasGeneralesPorID_CENTRO(int id_centro)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<medidas_generales> listaRiesgos = new List<medidas_generales>();

            List<medidas_generales> registros = new List<medidas_generales>();
            registros = (from u in bd.medidas_generales
                         where u.id_centro == id_centro
                         select u).ToList();

            foreach (medidas_generales registro in registros)
            {
                medidas_generales nuevoRiesgo = registro;
                listaRiesgos.Add(nuevoRiesgo);
            }

            return listaRiesgos;
        }
        public static List<matriz_centro> ListarMatrizCentroPorID_CENTRO(int id_centro)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<matriz_centro> listaMatriz = new List<matriz_centro>();

            List<matriz_centro> registros = new List<matriz_centro>();
            registros = (from u in bd.matriz_centro
                         where u.id_centro == id_centro
                         select u).ToList();

            foreach (matriz_centro registro in registros)
            {
                matriz_centro nuevoRiesgo = registro;
                listaMatriz.Add(nuevoRiesgo);
            }

            return listaMatriz;
        }

        public static List<medidas_preventivas> ListarMedidas()
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<medidas_preventivas> listaRiesgos = new List<medidas_preventivas>();

            List<medidas_preventivas> registros = new List<medidas_preventivas>();
            registros = (from u in bd.medidas_preventivas
                         select u).ToList();

            foreach (medidas_preventivas registro in registros)
            {
                medidas_preventivas nuevoRiesgo = registro;
                listaRiesgos.Add(nuevoRiesgo);
            }

            return listaRiesgos;
        }
        public static List<submedidas_preventivas> ListarSubMedidas(int idMedida)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<submedidas_preventivas> listaMedidas = new List<submedidas_preventivas>();

            List<submedidas_preventivas> registros = new List<submedidas_preventivas>();
            registros = (from u in bd.submedidas_preventivas
                         where u.id_medida_preventiva == idMedida
                         select u).ToList();

            foreach (submedidas_preventivas registro in registros)
            {
                submedidas_preventivas nuevoRiesgo = registro;
                listaMedidas.Add(nuevoRiesgo);
            }

            return listaMedidas;
        }
        public static List<submedidas_preventivas> ListarSubMedidas()
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<submedidas_preventivas> listaMedidas = new List<submedidas_preventivas>();

            List<submedidas_preventivas> registros = new List<submedidas_preventivas>();
            registros = (from u in bd.submedidas_preventivas
                         select u).ToList();

            foreach (submedidas_preventivas registro in registros)
            {
                submedidas_preventivas nuevoRiesgo = registro;
                listaMedidas.Add(nuevoRiesgo);
            }

            return listaMedidas;
        }

        public static List<riesgos_medidas> ListarMedidasRiesgo(int idCentro)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<riesgos_medidas> listaRiesgos = new List<riesgos_medidas>();

            List<riesgos_medidas> registros = new List<riesgos_medidas>();
            registros = (from u in bd.riesgos_medidas
                         where u.id_centro == idCentro || u.id_centro == 0
                         select u).ToList();

            foreach (riesgos_medidas registro in registros)
            {
                riesgos_medidas nuevoRiesgo = registro;
                listaRiesgos.Add(nuevoRiesgo);
            }

            return listaRiesgos;
        }
        public static List<riesgos_medidas> ListarMedidasRiesgo()
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<riesgos_medidas> listaRiesgos = new List<riesgos_medidas>();

            List<riesgos_medidas> registros = new List<riesgos_medidas>();
            registros = (from u in bd.riesgos_medidas
                         select u).ToList();

            foreach (riesgos_medidas registro in registros)
            {
                riesgos_medidas nuevoRiesgo = registro;
                listaRiesgos.Add(nuevoRiesgo);
            }

            return listaRiesgos;
        }

        //public static List<medidas_apartados> ListarApartados()
        //{

        //    DIMASSTEntities bd = new DIMASSTEntities();

        //    List<medidas_apartados> registros = new List<medidas_apartados>();
        //    registros = (from u in bd.medidas_apartados
        //                 select u).ToList();

        //    return registros;
        //}
        public static List<medidas_apartadosV2> ListarApartadosV2()
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<medidas_apartadosV2> registros = new List<medidas_apartadosV2>();
            registros = (from u in bd.medidas_apartadosV2
                         select u).ToList();

            return registros;
        }
        public static List<medidas_apartados_generales> ListarApartadosGenerales()
        {

            DIMASSTEntities bd = new DIMASSTEntities();


            List<medidas_apartados_generales> registros = new List<medidas_apartados_generales>();
            registros = (from u in bd.medidas_apartados_generales
                         select u).ToList();


            return registros;
        }
        public static List<medidas_generales> ListarMedidasGenerales(int centro, int tecnologia)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<medidas_generales> listaMedidas = new List<medidas_generales>();

            List<medidas_generales> registros = new List<medidas_generales>();
            registros = (from u in bd.medidas_generales
                         where u.id_centro == centro && u.id_tecnologia == tecnologia
                         select u).ToList();

            foreach (medidas_generales registro in registros)
            {
                medidas_generales nuevaMedida = registro;
                listaMedidas.Add(nuevaMedida);
            }

            return listaMedidas;

        }

        public static string obtenerNombreApartadoGenerales(int id_apartado)
        {
            string resultado = "";

            DIMASSTEntities bd = new DIMASSTEntities();

            string registros = "";
            registros = (from u in bd.medidas_apartados_generales
                         where u.id == id_apartado
                         select u.descripcion).First();
            if (registros != null)
            {
                resultado = registros;
            }

            return resultado;
        }
        public static string obtenerNombreApartadoMedidasRiesgo(int id_apartado)
        {
            string resultado = "";

            DIMASSTEntities bd = new DIMASSTEntities();

            var registros = (from u in bd.medidas_apartados
                             where u.id == id_apartado
                             select u.nombre);
            if (registros != null && registros.Count() > 0)
            {
                resultado = registros.First();
            }


            return resultado;
        }
        public static string obtenerNombreApartadoMedidasRiesgoV2(int id_apartado)
        {
            string resultado = "";

            DIMASSTEntities bd = new DIMASSTEntities();

            var registros = (from u in bd.medidas_apartadosV2
                             where u.id == id_apartado
                             select u.nombre);
            if (registros != null && registros.Count() > 0)
            {
                resultado = registros.First();
            }


            return resultado;
        }

        public static medidas_apartados obtenerMedidaApartado(int id_apartado)
        {
            medidas_apartados resultado = new medidas_apartados();

            DIMASSTEntities bd = new DIMASSTEntities();

            var registros = (from u in bd.medidas_apartados
                             where u.id == id_apartado
                             select u);
            if (registros != null && registros.Count() > 0)
            {
                resultado = registros.First();
            }

            return resultado;
        }
        public static string obtenerImagenMedidasGenerales(int id_medida)
        {
            string resultado = "";

            DIMASSTEntities bd = new DIMASSTEntities();

            List<string> registros = (from u in bd.medidas_generales_imagenes
                                      where u.id_medida_general == id_medida
                                      select u.rutaImagen).ToList();


            if (registros != null && registros.Count() > 0)
            {
                resultado = registros.First();
            }

            return resultado;
        }

        public static medidas_generales_imagenes obtenerImagenMedidasGeneralesObjeto(int id_medida)
        {
            medidas_generales_imagenes resultado = new medidas_generales_imagenes();

            DIMASSTEntities bd = new DIMASSTEntities();

            var registros = (from u in bd.medidas_generales_imagenes
                             where u.id_medida_general == id_medida
                             select u);
            if (registros != null && registros.Count() > 0)
            {
                resultado = registros.First();
            }

            return resultado;
        }
        public static medidaspreventivas_imagenes obtenerMedidasPreventivasImagenes(int id_medida)
        {
            medidaspreventivas_imagenes resultado = new medidaspreventivas_imagenes();

            DIMASSTEntities bd = new DIMASSTEntities();

            var registros = (from u in bd.medidaspreventivas_imagenes
                             where u.id_medida == id_medida
                             select u);

            if (registros != null && registros.Count() > 0)
            {
                resultado = registros.First();
            }
            return resultado;
        }

        public static medidaspreventivas_imagenes ObtenerMedidaPorIdImagenMedida(int id_imagen)
        {
            medidaspreventivas_imagenes resultado = new medidaspreventivas_imagenes();

            DIMASSTEntities bd = new DIMASSTEntities();

            var registros = (from u in bd.medidaspreventivas_imagenes
                             where u.id == id_imagen
                             select u);

            if (registros != null && registros.Count() > 0)
            {
                resultado = registros.First();
            }
            return resultado;
        }

        public static medidas_generales_imagenes getMedidasGeneralesImagenesById(int id_medida)
        {

            DIMASSTEntities bd = new DIMASSTEntities();
            medidas_generales_imagenes nuevo = new medidas_generales_imagenes();

            var registro = (from u in bd.medidas_generales_imagenes
                            where u.id_medida_general == id_medida
                            select u).FirstOrDefault();

            if (registro != null)
            {
                nuevo.id = registro.id;
                nuevo.id_medida_general = registro.id_medida_general;
                nuevo.rutaImagen = registro.rutaImagen;
                nuevo.tamano = registro.tamano;
            }
            else
                nuevo = null;



            return nuevo;
        }

        public static List<medidas_generales_imagenes> GetListMedidasGeneralesImagenes()
        {
            StringBuilder errorMessages = new StringBuilder();

            DIMASSTEntities bd = new DIMASSTEntities();

            List<medidas_generales_imagenes> lista = new List<medidas_generales_imagenes>();

            try
            {

                var registros = (from u in bd.medidas_generales_imagenes select u).ToList();

                if (registros != null)
                {
                    foreach (medidas_generales_imagenes item in registros)
                    {
                        lista.Add(item);
                    }
                }

                return lista;
            }
            catch (SqlException ex)
            {
                for (int i = 0; i < ex.Errors.Count; i++)
                {
                    errorMessages.Append("Index #" + i + "\n" +
                        "Message: " + ex.Errors[i].Message + "\n" +
                        "LineNumber: " + ex.Errors[i].LineNumber + "\n" +
                        "Source: " + ex.Errors[i].Source + "\n" +
                        "Procedure: " + ex.Errors[i].Procedure + "\n");
                }
                Console.WriteLine(errorMessages.ToString());
                return null;
            }
        }

        public static List<medidaspreventivas_imagenes> GetListMedidasPreventivasImagenes()
        {
            StringBuilder errorMessages = new StringBuilder();

            DIMASSTEntities bd = new DIMASSTEntities();

            List<medidaspreventivas_imagenes> lista = new List<medidaspreventivas_imagenes>();

            try
            {

                var registros = (from u in bd.medidaspreventivas_imagenes select u).ToList();

                if (registros != null)
                {
                    foreach (medidaspreventivas_imagenes item in registros)
                    {
                        lista.Add(item);
                    }
                }

                return lista;
            }
            catch (SqlException ex)
            {
                for (int i = 0; i < ex.Errors.Count; i++)
                {
                    errorMessages.Append("Index #" + i + "\n" +
                        "Message: " + ex.Errors[i].Message + "\n" +
                        "LineNumber: " + ex.Errors[i].LineNumber + "\n" +
                        "Source: " + ex.Errors[i].Source + "\n" +
                        "Procedure: " + ex.Errors[i].Procedure + "\n");
                }
                Console.WriteLine(errorMessages.ToString());
                return null;
            }
        }
        //public medidas_generales_imagenes  EsImagenGrandeMedidaGeneral(int id_medida)
        //{
        //    medidas_generales_imagenes resultado = new medidas_generales_imagenes();

        //    DIMASSTEntities bd = new DIMASSTEntities();

        //    var registros = (from u in bd.medidas_generales_imagenes
        //                              where u.id_medida_general == id_medida
        //                              select u).ToList();


        //    if (registros != null && registros.Count() > 0)
        //    {
        //        resultado = registros.First();
        //    }

        //    return resultado;
        //}
        //public static bool obtenerTamanoMedidasGenerales(int id_medida)
        //{
        //    bool resultado = false;

        //    DIMASSTEntities bd = new DIMASSTEntities();

        //    var registros = (from u in bd.medidas_generales_imagenes
        //                 where u.id == id_medida
        //                 select u.tamano).First();
        //    if (registros != null)
        //    {
        //        resultado = registros;
        //    }

        //    return resultado;
        //}
        public static List<medidas_generales> ListarMedidasGenerales()
        {

            DIMASSTEntities bd = new DIMASSTEntities();
            List<medidas_generales> listaMedidas = new List<medidas_generales>();
            List<medidas_generales> registros = new List<medidas_generales>();
            registros = (from u in bd.medidas_generales
                         select u).OrderBy(c => c.id).ToList();
            foreach (medidas_generales registro in registros)
            {
                medidas_generales nuevaMedida = registro;
                listaMedidas.Add(nuevaMedida);
            }
            return listaMedidas;
        }
        public static List<personas> ListarPersonas()
        {

            DIMASSTEntities bd = new DIMASSTEntities();
            List<personas> listaPersonas = new List<personas>();
            List<personas> registros = new List<personas>();
            registros = (from u in bd.personas
                         select u).OrderBy(c => c.Id).ToList();
            foreach (personas registro in registros)
            {
                personas nuevaPersona = registro;
                listaPersonas.Add(nuevaPersona);
            }
            return listaPersonas;

        }

        public static List<medidas_generales_imagenes> ListarMedidasGeneralesImagenes()
        {

            DIMASSTEntities bd = new DIMASSTEntities();
            List<medidas_generales_imagenes> listaMedidas = new List<medidas_generales_imagenes>();
            List<medidas_generales_imagenes> registros = new List<medidas_generales_imagenes>();
            registros = (from u in bd.medidas_generales_imagenes
                         select u).OrderBy(c => c.id).ToList();
            foreach (medidas_generales_imagenes registro in registros)
            {
                if (registro.rutaImagen != null && registro.rutaImagen != "")
                {
                    medidas_generales_imagenes nuevaMedida = registro;
                    listaMedidas.Add(nuevaMedida);
                }
            }
            return listaMedidas;
        }

        public static List<banco_icono> ListarBancoIcono()
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var registro = (from u in bd.banco_icono
                            select u).ToList();

            return registro;
        }

        public static List<riesgos_medidas> ListarRiesgosMedidasImagenes()
        {

            DIMASSTEntities bd = new DIMASSTEntities();
            List<riesgos_medidas> listaMedidas = new List<riesgos_medidas>();
            List<riesgos_medidas> registros = new List<riesgos_medidas>();
            registros = (from u in bd.riesgos_medidas
                         select u).OrderBy(c => c.id).ToList();
            foreach (riesgos_medidas registro in registros)
            {
                if (registro.imagen != null && registro.imagen != "")
                {
                    riesgos_medidas nuevaMedida = registro;
                    listaMedidas.Add(nuevaMedida);
                }
            }
            return listaMedidas;
        }

        public static List<medidaspreventivas_imagenes> ListarMedidasPreventivasImagenes()
        {

            DIMASSTEntities bd = new DIMASSTEntities();
            List<medidaspreventivas_imagenes> listaMedidas = new List<medidaspreventivas_imagenes>();
            List<medidaspreventivas_imagenes> registros = new List<medidaspreventivas_imagenes>();
            registros = (from u in bd.medidaspreventivas_imagenes
                         select u).OrderBy(c => c.id).ToList();
            foreach (medidaspreventivas_imagenes registro in registros)
            {
                if (registro.rutaImagen != null && registro.rutaImagen != "")
                {
                    medidaspreventivas_imagenes nuevaMedida = registro;
                    listaMedidas.Add(nuevaMedida);
                }
            }
            return listaMedidas;
        }


        public static List<parametrica_medidas> ListarParametricaMedidas()
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<parametrica_medidas> listaMedidas = new List<parametrica_medidas>();

            List<parametrica_medidas> registros = new List<parametrica_medidas>();
            registros = (from u in bd.parametrica_medidas
                         select u).ToList();

            foreach (parametrica_medidas registro in registros)
            {
                parametrica_medidas nuevaMedida = registro;
                listaMedidas.Add(nuevaMedida);
            }

            return listaMedidas;

        }



        public static List<VISTA_Riesgo> ListarRiesgosFicha(int idCentral)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<VISTA_Riesgo> listaRiesgos = new List<VISTA_Riesgo>();

            List<VISTA_Riesgo> registros = new List<VISTA_Riesgo>();

            centros cent = ObtenerCentroPorID(idCentral);

            int tipocentral = int.Parse(cent.tipo.ToString());

            registros = (from u in bd.VISTA_Riesgo
                         where u.idCentral == idCentral
                         select u).ToList();



            foreach (VISTA_Riesgo registro in registros)
            {
                VISTA_Riesgo nuevoRiesgo = registro;
                listaRiesgos.Add(nuevoRiesgo);
            }

            return listaRiesgos;
        }

        public static List<procesos> ListarProcesos(int idOrg, string tipoProceso)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<procesos> listaProcesos = new List<procesos>();

            List<procesos> registros = new List<procesos>();

            if (tipoProceso != "T")
            {
                registros = (from u in bd.procesos
                             where u.organizacion == idOrg && u.tipo == tipoProceso
                             && u.padre == null
                             select u).OrderBy(x => x.nivel).OrderBy(x => x.orden).ToList();
            }
            else
            {
                registros = (from u in bd.procesos
                             where u.organizacion == idOrg && u.padre != null
                             select u).OrderBy(x => x.nivel).OrderBy(x => x.orden).ToList();
            }


            foreach (procesos registro in registros)
            {
                procesos nuevoProceso = new procesos();
                nuevoProceso.id = registro.id;
                nuevoProceso.nombre = registro.nombre;
                nuevoProceso.tipo = registro.tipo;
                nuevoProceso.nivel = registro.nivel;
                nuevoProceso.padre = registro.padre;
                nuevoProceso.organizacion = registro.organizacion;
                nuevoProceso.cod_proceso = registro.cod_proceso;
                listaProcesos.Add(nuevoProceso);
            }

            return listaProcesos;
        }

        public static List<procesos> ListarProcesosPorNivel(int idOrg, string nivel)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<procesos> listaProcesos = new List<procesos>();

            List<procesos> registros = new List<procesos>();


            registros = (from u in bd.procesos
                         where u.organizacion == idOrg && u.nivel == nivel
                         select u).OrderBy(x => x.nivel).OrderBy(x => x.orden).ToList();


            foreach (procesos registro in registros)
            {
                procesos nuevoProceso = new procesos();
                nuevoProceso.id = registro.id;
                nuevoProceso.nombre = registro.nombre;
                nuevoProceso.tipo = registro.tipo;
                nuevoProceso.nivel = registro.nivel;
                nuevoProceso.padre = registro.padre;
                nuevoProceso.organizacion = registro.organizacion;
                nuevoProceso.cod_proceso = registro.cod_proceso;
                listaProcesos.Add(nuevoProceso);
            }

            return listaProcesos;
        }


        public static List<procesos> ListarPadres(int idOrg, string tipoProceso, string nivel)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<procesos> listaProcesos = new List<procesos>();

            List<procesos> registros = new List<procesos>();

            if (nivel == "S")
            {
                registros = (from u in bd.procesos
                             where u.organizacion == idOrg && u.nivel == "M" && u.tipo == tipoProceso
                             select u).OrderBy(x => x.nivel).OrderBy(x => x.orden).ToList();
            }
            else if (nivel == "F")
            {
                registros = (from u in bd.procesos
                             where u.organizacion == idOrg && (u.nivel == "S" || u.nivel == "M") && u.tipo == tipoProceso
                             select u).OrderBy(x => x.nivel).OrderBy(x => x.orden).ToList();
            }


            foreach (procesos registro in registros)
            {
                procesos nuevoProceso = new procesos();
                nuevoProceso.id = registro.id;
                nuevoProceso.nombre = registro.nombre;
                nuevoProceso.tipo = registro.tipo;
                nuevoProceso.nivel = registro.nivel;
                nuevoProceso.padre = registro.padre;
                nuevoProceso.organizacion = registro.organizacion;
                nuevoProceso.cod_proceso = registro.cod_proceso;
                listaProcesos.Add(nuevoProceso);
            }

            return listaProcesos;
        }

        public static List<procesos> ListarPadresbytecnologias(int idOrg, string tipoProceso, string nivel, int tecnologia)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<procesos> listaProcesos = new List<procesos>();

            List<procesos> registros = new List<procesos>();

            if (nivel == "S")
            {
                registros = (from u in bd.procesos
                             where u.organizacion == idOrg && u.nivel == "M" && u.tipo == tipoProceso && u.tecnologia == tecnologia
                             select u).OrderBy(x => x.nivel).OrderBy(x => x.orden).ToList();
            }
            else if (nivel == "F")
            {
                registros = (from u in bd.procesos
                             where u.organizacion == idOrg && (u.nivel == "S" || u.nivel == "M") && u.tipo == tipoProceso && u.tecnologia == tecnologia
                             select u).OrderBy(x => x.nivel).OrderBy(x => x.orden).ToList();
            }


            foreach (procesos registro in registros)
            {
                procesos nuevoProceso = new procesos();
                nuevoProceso.id = registro.id;
                nuevoProceso.nombre = registro.nombre;
                nuevoProceso.tipo = registro.tipo;
                nuevoProceso.nivel = registro.nivel;
                nuevoProceso.padre = registro.padre;
                nuevoProceso.organizacion = registro.organizacion;
                nuevoProceso.cod_proceso = registro.cod_proceso;
                listaProcesos.Add(nuevoProceso);
            }

            return listaProcesos;
        }

        public static List<VISTA_ObtenerDocumentacion> ListarDocumentosProceso(int id, int idCentral)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            centros centralElegida = ObtenerCentroPorID(idCentral);

            List<VISTA_ObtenerDocumentacion> listaProcesos = new List<VISTA_ObtenerDocumentacion>();

            var registros = (from u in bd.VISTA_ObtenerDocumentacion
                             where u.idproceso == id && (u.idcentro == null || u.idcentro == idCentral) && (u.tipocentral == null || u.tipocentral == centralElegida.tipo)
                             select u).OrderBy(x => x.nivel).OrderBy(x => x.idFichero).ToList();

            foreach (VISTA_ObtenerDocumentacion registro in registros)
            {

                listaProcesos.Add(registro);
            }

            return listaProcesos;
        }

        public static List<procesos> ListarMacroprocesos(int idOrg)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<procesos> listaProcesos = new List<procesos>();

            var registros = (from u in bd.procesos
                             where u.organizacion == idOrg && u.nivel == "M"
                             select u).OrderBy(x => x.nivel).ToList();


            foreach (procesos registro in registros)
            {
                procesos nuevoProceso = new procesos();
                nuevoProceso.id = registro.id;
                nuevoProceso.nombre = registro.nombre;
                nuevoProceso.tipo = registro.tipo;
                nuevoProceso.nivel = registro.nivel;
                nuevoProceso.padre = registro.padre;
                nuevoProceso.organizacion = registro.organizacion;
                listaProcesos.Add(nuevoProceso);
            }

            return listaProcesos;
        }

        public static areanivel4 ObtenerNivel4PorID(int idCentro)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            areanivel4 nuevo = new areanivel4();

            var registro = (from u in bd.areanivel4
                            where u.id == idCentro
                            select u).FirstOrDefault();

            if (registro != null)
            {
                nuevo.id = registro.id;
                nuevo.codigo = registro.codigo;
                nuevo.nombre = registro.nombre;
                nuevo.id_areanivel3 = registro.id_areanivel3;
            }
            else
            {
                nuevo = null;
            }
            return nuevo;
        }

        public static areanivel3 ObtenerEquipoPorID(int idCentro)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            areanivel3 nuevo = new areanivel3();


            var registro = (from u in bd.areanivel3
                            where u.id == idCentro
                            select u).FirstOrDefault();


            if (registro != null)
            {
                nuevo.id = registro.id;
                nuevo.codigo = registro.codigo;
                nuevo.nombre = registro.nombre;
                nuevo.id_areanivel2 = registro.id_areanivel2;

            }
            else
                nuevo = null;


            return nuevo;
        }

        public static areanivel2 ObtenerSistemaPorID(int idCentro)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            areanivel2 nuevo = new areanivel2();


            var registro = (from u in bd.areanivel2
                            where u.id == idCentro
                            select u).FirstOrDefault();


            if (registro != null)
            {
                nuevo.id = registro.id;
                nuevo.codigo = registro.codigo;
                nuevo.nombre = registro.nombre;
                nuevo.id_areanivel1 = registro.id_areanivel1;

            }
            else
                nuevo = null;


            return nuevo;
        }

        public static areanivel2 ObtenerAreaPorIDSistema(int idSistema)
        {
            DIMASSTEntities db = new DIMASSTEntities();
            areanivel2 area = new areanivel2();

            var registro = (from u in db.areanivel2
                            where u.id == idSistema
                            select u.areanivel1).FirstOrDefault();

            if (registro != null)
            {
                area.id = registro.id;
                area.codigo = registro.codigo;
                area.nombre = registro.nombre;

            }
            else
                area = null;

            return area;
        }

        public static areanivel1 ObtenerAreaPorID(int idCentro)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            areanivel1 nuevo = new areanivel1();


            var registro = (from u in bd.areanivel1
                            where u.id == idCentro
                            select u).FirstOrDefault();


            if (registro != null)
            {
                nuevo.id = registro.id;
                nuevo.codigo = registro.codigo;
                nuevo.nombre = registro.nombre;
                nuevo.id_centro = registro.id_centro;

            }
            else
                nuevo = null;


            return nuevo;
        }
        public static areanivel1 ObtenerAreaNivel1PorId(int idArea)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            areanivel1 nuevo = new areanivel1();


            var registro = (from u in bd.areanivel1
                            where u.id == idArea
                            select u).FirstOrDefault();


            if (registro != null)
            {
                nuevo.id = registro.id;
                nuevo.codigo = registro.codigo;
                nuevo.nombre = registro.nombre;
                nuevo.id_centro = registro.id_centro;
                nuevo.id_tecnologia = registro.id_tecnologia;
                nuevo.areanivel2 = registro.areanivel2;

            }
            else
                nuevo = null;


            return nuevo;
        }

        public static List<centros> ListarTodosCentros()
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<centros> listaCentros = new List<centros>();

            var registros = (from u in bd.centros
                             select u).OrderBy(x => x.nombre).ToList();
            //var registros = (from u in bd.centros                 
            //                 where u.tipo == tipo
            //                 select u).OrderBy(x => x.nombre).ToList();

            foreach (centros registro in registros)
            {
                centros nuevoCentro = new centros();
                nuevoCentro.id = registro.id;
                nuevoCentro.nombre = registro.nombre;
                nuevoCentro.siglas = registro.siglas;
                nuevoCentro.tipo = registro.tipo;
                nuevoCentro.ubicacion = registro.ubicacion;
                nuevoCentro.provincia = registro.provincia;
                nuevoCentro.rutaImagen = registro.rutaImagen;
                nuevoCentro.direccion = registro.direccion;
                nuevoCentro.coordenadas = registro.coordenadas;
                nuevoCentro.rutaImagenLogo = registro.rutaImagenLogo;
                listaCentros.Add(nuevoCentro);
            }

            return listaCentros;
        }
        public static List<centros> ListarCentros(int tipo)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<centros> listaCentros = new List<centros>();

            var registros = (from u in bd.centros
                             join tg in bd.tecnologias_centros on u.id equals tg.id_centro
                             where tg.id_tecnologia == tipo
                             select u).OrderBy(x => x.nombre).ToList();
            //var registros = (from u in bd.centros                 
            //                 where u.tipo == tipo
            //                 select u).OrderBy(x => x.nombre).ToList();

            foreach (centros registro in registros)
            {
                centros nuevoCentro = new centros();
                nuevoCentro.id = registro.id;
                nuevoCentro.nombre = registro.nombre;
                nuevoCentro.siglas = registro.siglas;
                nuevoCentro.tipo = registro.tipo;
                nuevoCentro.ubicacion = registro.ubicacion;
                nuevoCentro.provincia = registro.provincia;

                listaCentros.Add(nuevoCentro);
            }

            return listaCentros;
        }

        //public static List<centros> ListarCentrosTiposZonas(int tipo, int zona)
        //{

        //    DIMASSTEntities bd = new DIMASSTEntities();

        //    List<centros> listaCentros = new List<centros>();

        //    var registros = (from u in bd.centros
        //                     join tg in bd.tecnologias_centros on u.id equals tg.id_centro
        //                     join cz in bd.centros_zonas on u.id equals cz.id_centro
        //                     where tg.id_tecnologia == tipo && cz.id_zona == zona
        //                     select u).OrderBy(x => x.nombre).ToList();
        //    //var registros = (from u in bd.centros                 
        //    //                 where u.tipo == tipo
        //    //                 select u).OrderBy(x => x.nombre).ToList();

        //    foreach (centros registro in registros)
        //    {
        //        centros nuevoCentro = new centros();
        //        nuevoCentro.id = registro.id;
        //        nuevoCentro.nombre = registro.nombre;
        //        nuevoCentro.siglas = registro.siglas;
        //        nuevoCentro.tipo = registro.tipo;
        //        nuevoCentro.ubicacion = registro.ubicacion;
        //        nuevoCentro.provincia = registro.provincia;

        //        listaCentros.Add(nuevoCentro);
        //    }

        //    return listaCentros;
        //}
        public static List<centros> ListarCentrosTiposZonasV2(int tipo, int zona)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<centros> listaCentros = new List<centros>();

            var registros = (from u in bd.centros
                             join cz in bd.centros_zonas on u.id equals cz.id_centro
                             where u.tipo == tipo && cz.id_zona == zona
                             select u).OrderBy(x => x.nombre).ToList();


            foreach (centros registro in registros)
            {
                centros nuevoCentro = new centros();
                nuevoCentro.id = registro.id;
                nuevoCentro.nombre = registro.nombre;
                nuevoCentro.siglas = registro.siglas;
                nuevoCentro.tipo = registro.tipo;
                nuevoCentro.ubicacion = registro.ubicacion;
                nuevoCentro.provincia = registro.provincia;

                listaCentros.Add(nuevoCentro);
            }

            return listaCentros;
        }
        public static List<zonas> ListarZonas()
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<zonas> listarZonas = new List<zonas>();

            var registros = (from u in bd.zonas
                             select u).OrderBy(x => x.nombre).ToList();


            foreach (zonas registro in registros)
            {
                zonas nuevaZona = new zonas();
                nuevaZona.id = registro.id;
                nuevaZona.nombre = registro.nombre;


                listarZonas.Add(nuevaZona);
            }

            return listarZonas;
        }
        public static List<zonas> ListarZonas(int tipo)
        {

            DIMASSTEntities bd = new DIMASSTEntities();
            List<zonas> listarZonas = new List<zonas>();

            var registros = (from u in bd.zonas
                             join zt in bd.zonas_tipotecnologia on u.id equals zt.id_zona
                             where zt.id_tecnologia == tipo
                             select u).OrderBy(x => x.nombre).ToList();


            foreach (zonas registro in registros)
            {
                zonas nuevaZona = new zonas();
                nuevaZona.id = registro.id;
                nuevaZona.nombre = registro.nombre;


                listarZonas.Add(nuevaZona);
            }

            return listarZonas;
        }

        public static List<agrupacion> ListarAgrupaciones()
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<agrupacion> listaAgrupacion = new List<agrupacion>();

            var registros = (from u in bd.agrupacion
                             select u).OrderBy(x => x.nombre).ToList();


            foreach (agrupacion registro in registros)
            {
                agrupacion agrupacion = new agrupacion();
                agrupacion.id = registro.id;
                agrupacion.nombre = registro.nombre;


                listaAgrupacion.Add(agrupacion);
            }

            return listaAgrupacion;
        }
        public static List<centros_zonas> ListarCentrosZonas()
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<centros_zonas> lista_centros_zonas = new List<centros_zonas>();

            var registros = (from u in bd.centros_zonas
                             select u).OrderBy(x => x.id_zona).ToList();


            foreach (centros_zonas registro in registros)
            {
                centros_zonas zonas = new centros_zonas();
                zonas.id = registro.id;
                zonas.id_zona = registro.id_zona;


                lista_centros_zonas.Add(zonas);
            }

            return lista_centros_zonas;
        }

        public static List<agrupacion> ListarAgrupacionesPorZonas(int idZona)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<agrupacion> listaAgrupacion = new List<agrupacion>();

            var registros = (from u in bd.agrupacion
                             where u.id_zona == idZona
                             select u).OrderBy(x => x.nombre).ToList();


            foreach (agrupacion registro in registros)
            {
                agrupacion agrupacion = new agrupacion();
                agrupacion.id = registro.id;
                agrupacion.nombre = registro.nombre;


                listaAgrupacion.Add(agrupacion);
            }

            return listaAgrupacion;
        }

        public static centros ObtenerCentroPorID(int idCentro)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            centros nuevo = new centros();


            var registro = (from u in bd.centros
                            where u.id == idCentro
                            select u).FirstOrDefault();


            if (registro != null)
            {
                nuevo.id = registro.id;
                nuevo.siglas = registro.siglas;
                nuevo.nombre = registro.nombre;
                nuevo.tipo = registro.tipo;
                nuevo.ubicacion = registro.ubicacion;
                nuevo.provincia = registro.provincia;
                nuevo.rutaImagen = registro.rutaImagen;
                nuevo.direccion = registro.direccion;
                nuevo.coordenadas = registro.coordenadas;
                nuevo.rutaImagenLogo = registro.rutaImagenLogo;

            }
            else
                nuevo = null;


            return nuevo;
        }
        public static medidas_generales ObtenerMedidaGeneralID(int idMedida)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            medidas_generales nuevo = new medidas_generales();


            var registro = (from u in bd.medidas_generales
                            where u.id == idMedida
                            select u).FirstOrDefault();


            if (registro != null)
            {
                nuevo = registro;

            }
            else
                nuevo = null;


            return nuevo;
        }

        public static riesgos_situaciones ObtenerSituacionRiesgoporID(int idSituacion)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            riesgos_situaciones nuevo = new riesgos_situaciones();


            var registro = (from u in bd.riesgos_situaciones
                            where u.id == idSituacion
                            select u).FirstOrDefault();


            if (registro != null)
            {
                nuevo = registro;

            }
            else
                nuevo = null;


            return nuevo;
        }
        public static medidas_generales ObtenerMedidaporId(int id_medida)
        {

            DIMASSTEntities bd = new DIMASSTEntities();
            medidas_generales nuevo = new medidas_generales();
            var registro = (from u in bd.medidas_generales
                            where u.id == id_medida
                            select u).FirstOrDefault();

            if (registro != null)
            {
                nuevo.id = registro.id;
                nuevo.descripcion = registro.descripcion;
                nuevo.id_apartado_generales = registro.id_apartado_generales;

            }
            else
            {
                nuevo = null;
            }
            return nuevo;
        }

        public static medidas_preventivas ObtenerMedidaPreventivaId(int id_medida)
        {

            DIMASSTEntities bd = new DIMASSTEntities();
            medidas_preventivas nuevo = new medidas_preventivas();
            var registro = (from u in bd.medidas_preventivas
                            where u.id == id_medida
                            select u).FirstOrDefault();

            if (registro != null)
            {
                nuevo.id = registro.id;
                nuevo.descripcion = registro.descripcion;
                nuevo.id_centro = registro.id_centro;
                nuevo.id_situacion = registro.id_situacion;

            }
            else
            {
                nuevo = null;
            }
            return nuevo;
        }
        public static personas ObtenerPersonaId(int id)
        {

            DIMASSTEntities bd = new DIMASSTEntities();
            personas persona = new personas();
            var registro = (from u in bd.personas
                            where u.Id == id
                            select u).FirstOrDefault();

            if (registro != null)
            {
                persona.Id = registro.Id;
                persona.Nº_Empleado = registro.Nº_Empleado;
                persona.Perfil_de_riesgo = registro.Perfil_de_riesgo;
                persona.Actividad_Funcional = registro.Actividad_Funcional;
                persona.DNI = registro.DNI;
                persona.Nombre = registro.Nombre;
                persona.Empresa = registro.Empresa;
                persona.Centro_de_trabajo = registro.Centro_de_trabajo;
                persona.Actividad = registro.Actividad;
                persona.Unidad_Organizativa = registro.Unidad_Organizativa;
                persona.Jefe_Directo = registro.Jefe_Directo;
                persona.Posicion = registro.Posicion;
                persona.Ocupacion = registro.Ocupacion;
                persona.Activo = registro.Activo;

            }
            else
            {
                persona = null;
            }
            return persona;
        }
        public static riesgos_medidas ObtenerRiesgoMedidaporId(int id_medida)
        {

            DIMASSTEntities bd = new DIMASSTEntities();
            riesgos_medidas nuevo = new riesgos_medidas();
            var registro = (from u in bd.riesgos_medidas
                            where u.id == id_medida
                            select u).FirstOrDefault();

            if (registro != null)
            {
                nuevo.id = registro.id;
                nuevo.descripcion = registro.descripcion;
                nuevo.id_apartado = registro.id_apartado;
                nuevo.imagen = registro.imagen;
                nuevo.imagen_grande = registro.imagen_grande;
                nuevo.id_riesgo = registro.id_riesgo;
                nuevo.id_centro = registro.id_centro;

            }
            else
            {
                nuevo = null;
            }
            return nuevo;
        }

        public static void EliminarRiesgoMedida(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var Eliminar = bd.riesgos_medidas.Where(x => x.id == id).FirstOrDefault();

            if (Eliminar != null)
            {
                bd.riesgos_medidas.Remove(Eliminar);
                bd.SaveChanges();
            }
        }


        public static void EliminarRutaImagenPorId_Y_TipoImagen(int id, int tipoImagen)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var Eliminar = bd.riesgos_medidas.Where(x => x.id == id && x.imagen_grande == tipoImagen).FirstOrDefault();

            if (Eliminar != null)
            {
                Eliminar.imagen = null;
                //bd.riesgos_medidas.Remove(Eliminar);
                bd.SaveChanges();
            }
        }





        public static int GuardarApartado(string apartado)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            medidas_apartados insertar = new medidas_apartados();

            insertar.nombre = apartado;
            bd.medidas_apartados.Add(insertar);
            bd.SaveChanges();

            return insertar.id;
        }
        public static bool GuardarListaPersonas(List<personas> listaPersonas)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            //List<personas> insertar = new List<personas>();

            var all = from c in bd.personas select c;
            bd.personas.RemoveRange(all);
            bd.SaveChanges();

            //bd.BulkInsert(listaPersonas.ToList());

            //foreach (personas item in listaPersonas)
            //{
            bd.personas.AddRange(listaPersonas);
            bd.SaveChanges();
            //}

            //insertar.nombre = apartado;
            //bd.medidas_apartados.Add(insertar);
            //bd.SaveChanges();

            return true;
        }

        public static int GuardarMedidasGeneralesImagenes(int idMedidaGeneral, string nombreArchivo)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            medidas_generales_imagenes insertar = new medidas_generales_imagenes();

            insertar.id_medida_general = idMedidaGeneral;
            insertar.rutaImagen = "../Content/images/medidas/medidasgenerales/" + nombreArchivo;
            insertar.tamano = false;

            bd.medidas_generales_imagenes.Add(insertar);
            bd.SaveChanges();


            return insertar.id;
        }
        public static int GuardarMedidasPreventivasImagenes(int idMedidaRiesgo, string nombreArchivo)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            medidaspreventivas_imagenes insertar = new medidaspreventivas_imagenes();

            insertar.id_medida = idMedidaRiesgo;
            insertar.rutaImagen = "../Content/images/medidas/medidaspreventivas/" + nombreArchivo;
            insertar.tamano = false;

            bd.medidaspreventivas_imagenes.Add(insertar);
            bd.SaveChanges();


            return insertar.id;
        }

        public static int ActualizarMedidasGeneralesImagenes(int idMedidaGeneral, string nombreArchivo)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            StringBuilder errorMessages = new StringBuilder();

            try
            {
                var actualizar = (from u in bd.medidas_generales_imagenes
                                  where u.id_medida_general == idMedidaGeneral
                                  select u).FirstOrDefault();

                if (actualizar != null)
                {
                    actualizar.id_medida_general = idMedidaGeneral;
                    actualizar.rutaImagen = "../Content/images/medidas/medidasgenerales/" + nombreArchivo;
                    actualizar.tamano = false;

                    bd.SaveChanges();
                }
                else
                {
                    var id_guardado = Datos.GuardarMedidasGeneralesImagenes(idMedidaGeneral, nombreArchivo);
                    return id_guardado;
                }
                return actualizar.id;
            }
            catch (SqlException ex)
            {
                for (int i = 0; i < ex.Errors.Count; i++)
                {
                    errorMessages.Append("Index #" + i + "\n" +
                        "Message: " + ex.Errors[i].Message + "\n" +
                        "LineNumber: " + ex.Errors[i].LineNumber + "\n" +
                        "Source: " + ex.Errors[i].Source + "\n" +
                        "Procedure: " + ex.Errors[i].Procedure + "\n");
                }
                Console.WriteLine(errorMessages.ToString());
                return 0;
            }
        }
        public static int ActualizarMedidasGeneralesImagenesPorObjeto(medidas_generales_imagenes medidas_Generales_Imagenes, int idMedidaGeneral)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            StringBuilder errorMessages = new StringBuilder();
            medidas_generales_imagenes insertar = new medidas_generales_imagenes();

            try
            {
                var actualizar = (from u in bd.medidas_generales_imagenes
                                  where u.id_medida_general == idMedidaGeneral
                                  select u).FirstOrDefault();

                if (actualizar != null)
                {
                    actualizar.id_medida_general = idMedidaGeneral;
                    actualizar.rutaImagen = medidas_Generales_Imagenes.rutaImagen;
                    actualizar.tamano = false;

                    bd.SaveChanges();
                }
                else
                {
                    insertar.id_medida_general = medidas_Generales_Imagenes.id_medida_general;
                    insertar.rutaImagen = medidas_Generales_Imagenes.rutaImagen;
                    insertar.tamano = medidas_Generales_Imagenes.tamano;

                    var id_guardado = bd.medidas_generales_imagenes.Add(insertar);
                    bd.SaveChanges();

                    return id_guardado.id;
                }
                return actualizar.id;
            }
            catch (SqlException ex)
            {
                for (int i = 0; i < ex.Errors.Count; i++)
                {
                    errorMessages.Append("Index #" + i + "\n" +
                        "Message: " + ex.Errors[i].Message + "\n" +
                        "LineNumber: " + ex.Errors[i].LineNumber + "\n" +
                        "Source: " + ex.Errors[i].Source + "\n" +
                        "Procedure: " + ex.Errors[i].Procedure + "\n");
                }
                Console.WriteLine(errorMessages.ToString());
                return 0;
            }
        }


        public static string ObtenerApartadoPorNombre(string nombre)
        {
            string resultado = "";
            DIMASSTEntities bd = new DIMASSTEntities();

            centros nuevo = new centros();


            var registro = (from u in bd.medidas_apartados
                            where u.nombre.ToLower() == nombre.ToLower()
                            select u.id).FirstOrDefault();

            if (registro != null)
            {
                resultado = registro.ToString();
            }

            return resultado;
        }
        public static string ObtenerApartadoPorNombreV2(string nombre)
        {
            string resultado = "";
            DIMASSTEntities bd = new DIMASSTEntities();

            centros nuevo = new centros();

            var registro = (from u in bd.medidas_apartadosV2
                            where u.nombre.ToLower() == nombre.ToLower()
                            select u.id).FirstOrDefault();

            if (registro != null)
            {
                resultado = registro.ToString();
            }

            return resultado;
        }
        public static usuario_centros ObtenerPermisos(int idUsuario, int idCentro)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            usuario_centros nuevo = new usuario_centros();

            nuevo = (from u in bd.usuario_centros
                     where u.idcentro == idCentro && u.idusuario == idUsuario
                     select u).FirstOrDefault();

            return nuevo;
        }

        public static tipodocumento ObtenerTipoDoc(int id)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            tipodocumento nuevo = new tipodocumento();


            var registro = (from u in bd.tipodocumento
                            where u.id == id
                            select u).FirstOrDefault();


            if (registro != null)
            {
                nuevo.id = registro.id;
                nuevo.tipo = registro.tipo;
                nuevo.nivel = registro.nivel;
                nuevo.tecnologia = registro.tecnologia;
            }
            else
                nuevo = null;


            return nuevo;
        }

        public static referenciales ObtenerReferencial(int id)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            referenciales nuevo = new referenciales();


            var registro = (from u in bd.referenciales
                            where u.id == id
                            select u).FirstOrDefault();


            if (registro != null)
            {
                nuevo.id = registro.id;
                nuevo.nombre = registro.nombre;
            }
            else
                nuevo = null;


            return nuevo;
        }

        public static comunicacion_tipos ObtenerTipoComunicacion(int id)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            comunicacion_tipos nuevo = new comunicacion_tipos();


            var registro = (from u in bd.comunicacion_tipos
                            where u.id == id
                            select u).FirstOrDefault();


            if (registro != null)
            {
                nuevo.id = registro.id;
                nuevo.tipo = registro.tipo;
            }
            else
                nuevo = null;


            return nuevo;
        }

        public static comunicacion_clasificacion ObtenerClasifComunicacion(int id)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            comunicacion_clasificacion nuevo = new comunicacion_clasificacion();


            var registro = (from u in bd.comunicacion_clasificacion
                            where u.id == id
                            select u).FirstOrDefault();


            if (registro != null)
            {
                nuevo.id = registro.id;
                nuevo.tipo = registro.tipo;
            }
            else
                nuevo = null;


            return nuevo;
        }

        public static comunicacion_canales ObtenerCanalComunicacion(int id)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            comunicacion_canales nuevo = new comunicacion_canales();


            var registro = (from u in bd.comunicacion_canales
                            where u.id == id
                            select u).FirstOrDefault();


            if (registro != null)
            {
                nuevo.id = registro.id;
                nuevo.canal = registro.canal;
            }
            else
                nuevo = null;


            return nuevo;
        }

        public static evento_ambiental_tipo ObtenerTipoEventoAmbiental(int id)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            evento_ambiental_tipo nuevo = new evento_ambiental_tipo();


            var registro = (from u in bd.evento_ambiental_tipo
                            where u.id == id
                            select u).FirstOrDefault();


            if (registro != null)
            {
                nuevo.id = registro.id;
                nuevo.tipo = registro.tipo;
            }
            else
                nuevo = null;


            return nuevo;
        }

        public static riesgos_categorias ObtenerCategoriaRiesgo(int id)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            riesgos_categorias nuevo = new riesgos_categorias();


            var registro = (from u in bd.riesgos_categorias
                            where u.id == id
                            select u).FirstOrDefault();


            if (registro != null)
            {
                nuevo.id = registro.id;
                nuevo.categoria = registro.categoria;
            }
            else
                nuevo = null;


            return nuevo;
        }

        public static riesgos_tipologias ObtenerTipologiaRiesgo(int id)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            riesgos_tipologias nuevo = new riesgos_tipologias();


            var registro = (from u in bd.riesgos_tipologias
                            where u.id == id
                            select u).FirstOrDefault();


            if (registro != null)
            {
                nuevo.id = registro.id;
                nuevo.tipologia = registro.tipologia;
            }
            else
                nuevo = null;


            return nuevo;
        }

        public static tipos_riesgos ObtenerTiposRiesgos(int id)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            tipos_riesgos nuevo = new tipos_riesgos();


            var registro = (from u in bd.tipos_riesgos
                            where u.id == id
                            select u).FirstOrDefault();


            if (registro != null)
            {
                nuevo.id = registro.id;
                nuevo.codigo = registro.codigo;
                nuevo.definicion = registro.definicion;
                nuevo.riesgos_situaciones = registro.riesgos_situaciones;
                nuevo.rutaImagen = registro.rutaImagen;
            }
            else
                nuevo = null;


            return nuevo;
        }

        public static evento_ambiental_matriz ObtenerMatrizEventoAmbiental(int id)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            evento_ambiental_matriz nuevo = new evento_ambiental_matriz();


            var registro = (from u in bd.evento_ambiental_matriz
                            where u.id == id
                            select u).FirstOrDefault();


            if (registro != null)
            {
                nuevo.id = registro.id;
                nuevo.matriz = registro.matriz;
            }
            else
                nuevo = null;


            return nuevo;
        }

        public static aspecto_tipo ObtenerTipoAspecto(int id)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            aspecto_tipo nuevo = (from u in bd.aspecto_tipo
                                  where u.id == id
                                  select u).FirstOrDefault();


            return nuevo;
        }

        public static aspecto_grupo ObtenerGrupoAspecto(int id)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            aspecto_grupo nuevo = (from u in bd.aspecto_grupo
                                   where u.id == id
                                   select u).FirstOrDefault();


            return nuevo;
        }

        public static tipo_accionesmejora ObtenerTipoAccMejora(int id)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            tipo_accionesmejora nuevo = new tipo_accionesmejora();


            var registro = (from u in bd.tipo_accionesmejora
                            where u.id == id
                            select u).FirstOrDefault();


            if (registro != null)
            {
                nuevo.id = registro.id;
                nuevo.nombre = registro.nombre;
            }
            else
                nuevo = null;


            return nuevo;
        }


        public static stakeholders_nivel1 ObtenerStakeholderN1(int id)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            stakeholders_nivel1 nuevo = new stakeholders_nivel1();


            var registro = (from u in bd.stakeholders_nivel1
                            where u.id == id
                            select u).FirstOrDefault();


            if (registro != null)
            {
                nuevo.id = registro.id;
                nuevo.denominacion = registro.denominacion;
            }
            else
                nuevo = null;


            return nuevo;
        }


        public static stakeholders_nivel2 ObtenerStakeholderN2(int id)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            stakeholders_nivel2 nuevo = new stakeholders_nivel2();


            var registro = (from u in bd.stakeholders_nivel2
                            where u.id == id
                            select u).FirstOrDefault();


            if (registro != null)
            {
                nuevo.id = registro.id;
                nuevo.denominacion = registro.denominacion;
                nuevo.idnivel1 = registro.idnivel1;
            }
            else
                nuevo = null;


            return nuevo;
        }


        public static stakeholders_nivel3 ObtenerStakeholderN3(int id)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            stakeholders_nivel3 nuevo = new stakeholders_nivel3();


            var registro = (from u in bd.stakeholders_nivel3
                            where u.id == id
                            select u).FirstOrDefault();


            if (registro != null)
            {
                nuevo.id = registro.id;
                nuevo.denominacion = registro.denominacion;
                nuevo.idnivel2 = registro.idnivel2;
                nuevo.parteinteresada = registro.parteinteresada;
                nuevo.necesidades = registro.necesidades;
            }
            else
                nuevo = null;


            return nuevo;
        }

        public static stakeholders_nivel4 ObtenerStakeholderN4(int id)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            stakeholders_nivel4 nuevo = new stakeholders_nivel4();


            var registro = (from u in bd.stakeholders_nivel4
                            where u.id == id
                            select u).FirstOrDefault();


            if (registro != null)
            {
                nuevo.id = registro.id;
                nuevo.denominacion = registro.denominacion;
                nuevo.idnivel3 = registro.idnivel3;
                nuevo.necesidades = registro.necesidades;
                nuevo.requisitosrel = registro.requisitosrel;
            }
            else
                nuevo = null;


            return nuevo;
        }

        public static ambitos ObtenerAmbito(int id)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            ambitos nuevo = new ambitos();


            var registro = (from u in bd.ambitos
                            where u.id == id
                            select u).FirstOrDefault();


            if (registro != null)
            {
                nuevo.id = registro.id;
                nuevo.nombre_ambito = registro.nombre_ambito;
            }
            else
                nuevo = null;


            return nuevo;
        }

        public static void EliminarCentro(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var CentroAEliminar = bd.centros.Where(x => x.id == id).FirstOrDefault();

            // Insercción código - Rafael Ortega - 22/07/2022
            var tecnologiaCentroElim = bd.tecnologias_centros.Where(x => x.id_centro == id).FirstOrDefault();

            var centZonaElim = bd.centros_zonas.Where(x => x.id_centro == id).FirstOrDefault();

            var centAgrupElim = bd.centros_agrupacion.Where(x => x.id_centro == id).FirstOrDefault();
            // Insercción código - Rafael Ortega - 22/07/2022

            if (CentroAEliminar != null)
            {
                bd.centros.Remove(CentroAEliminar);

                // Insercción código - Rafael Ortega - 22/07/2022
                bd.tecnologias_centros.Remove(tecnologiaCentroElim);

                if (centZonaElim != null)
                {
                    bd.centros_zonas.Remove(centZonaElim);
                }

                if (centAgrupElim != null)
                {
                    bd.centros_agrupacion.Remove(centAgrupElim);
                }
                // Insercción código - Rafael Ortega - 22/07/2022

                bd.SaveChanges();
            }
        }
        public static void EliminarMedidaGeneral(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var medidaEliminar = bd.medidas_generales.Where(x => x.id == id).FirstOrDefault();

            if (medidaEliminar != null)
            {

                bd.medidas_generales.Remove(medidaEliminar);


                bd.SaveChanges();
            }
        }

        public static void EliminarMatrizCentroPorIdCentro(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var medidaEliminar = bd.matriz_centro.Where(x => x.id_centro == id).ToList();

            if (medidaEliminar != null)
            {
                bd.matriz_centro.RemoveRange(medidaEliminar);

                bd.SaveChanges();
            }
        }

        public static string EliminarMedidaPreventivaImagen(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var imagenemedidaEliminar = bd.medidaspreventivas_imagenes.Where(x => x.id == id).FirstOrDefault();

            if (imagenemedidaEliminar != null)
            {
                bd.medidaspreventivas_imagenes.Remove(imagenemedidaEliminar);
                bd.SaveChanges();

                return imagenemedidaEliminar.rutaImagen;
            }

            return null;

        }

        public static string EliminarMedidaGeneralImagenes(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var imagenemedidaEliminar = bd.medidas_generales_imagenes.Where(x => x.id_medida_general == id).FirstOrDefault();

            if (imagenemedidaEliminar != null)
            {
                bd.medidas_generales_imagenes.Remove(imagenemedidaEliminar);
                bd.SaveChanges();

                return imagenemedidaEliminar.rutaImagen;
            }

            return null;

        }
        public static void EliminarEquipo(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var EquipoAELiminar = bd.areanivel3.Where(x => x.id == id).FirstOrDefault();

            if (ListarNivelescuatro().Where(x => x.id_areanivel3 == id).Count() > 0)
            {
                foreach (areanivel4 item in ListarNivelescuatro().Where(x => x.id_areanivel3 == id))
                {
                    EliminarNivel4(item.id);
                }
            }


            if (EquipoAELiminar != null)
            {
                bd.areanivel3.Remove(EquipoAELiminar);
                bd.SaveChanges();
            }
            List<matriz_centro> datosArea = bd.matriz_centro.Where(x => x.id_areanivel3 == id).ToList();

            foreach (matriz_centro item in datosArea)
            {
                bd.matriz_centro.Remove(item);
                bd.SaveChanges();

            }


        }

        public static void EliminarNivel4(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var EquipoAELiminar = bd.areanivel4.Where(x => x.id == id).FirstOrDefault();

            if (EquipoAELiminar != null)
            {
                bd.areanivel4.Remove(EquipoAELiminar);
                bd.SaveChanges();
            }
            List<matriz_centro> datosArea = bd.matriz_centro.Where(x => x.id_areanivel4 == id).ToList();

            foreach (matriz_centro item in datosArea)
            {
                bd.matriz_centro.Remove(item);
                bd.SaveChanges();

            }


        }
        public static void EliminarSistema(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var SistemaAEliminar = bd.areanivel2.Where(x => x.id == id).FirstOrDefault();
            if (SistemaAEliminar.areanivel3.Count > 0)
            {
                foreach (areanivel3 item in SistemaAEliminar.areanivel3)
                {
                    EliminarEquipo(item.id);
                }
            }
            if (SistemaAEliminar != null)
            {
                bd = new DIMASSTEntities();
                SistemaAEliminar = bd.areanivel2.Where(x => x.id == id).FirstOrDefault();
                bd.areanivel2.Remove(SistemaAEliminar);
                bd.SaveChanges();
            }
            List<matriz_centro> datosArea = bd.matriz_centro.Where(x => x.id_areanivel2 == id).ToList();

            foreach (matriz_centro item in datosArea)
            {
                bd.matriz_centro.Remove(item);
                bd.SaveChanges();

            }


        }
        public static int EliminarArea(int id)
        {
            int resultado = -1;
            try
            {
                DIMASSTEntities bd = new DIMASSTEntities();

                var AreaEliminar = bd.areanivel1.Where(x => x.id == id).FirstOrDefault();
                if (AreaEliminar.areanivel2.Count > 0)
                {
                    foreach (areanivel2 item in AreaEliminar.areanivel2)
                    {
                        EliminarSistema(item.id);
                    }
                }
                if (AreaEliminar != null)
                {
                    bd = new DIMASSTEntities();
                    AreaEliminar = bd.areanivel1.Where(x => x.id == id).FirstOrDefault();
                    bd.areanivel1.Remove(AreaEliminar);
                    bd.SaveChanges();
                }

                List<matriz_centro> datosArea = bd.matriz_centro.Where(x => x.id_areanivel1 == id).ToList();

                foreach (matriz_centro item in datosArea)
                {
                    bd.matriz_centro.Remove(item);
                    bd.SaveChanges();
                    resultado = 1;
                }
                return resultado;
            }
            catch (Exception ex)
            {
                return resultado;
            }

        }
        public static void EliminarSituacion(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var AreaEliminar = bd.riesgos_situaciones.Where(x => x.id == id).FirstOrDefault();

            if (AreaEliminar != null)
            {
                bd.riesgos_situaciones.Remove(AreaEliminar);
                bd.SaveChanges();
            }
        }


        public static int EliminarMedidaSituacion(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            medidas_preventivas AreaEliminar = bd.medidas_preventivas.Where(x => x.id == id).FirstOrDefault();



            if (AreaEliminar != null)
            {
                bd.medidas_preventivas.Remove(AreaEliminar);
                bd.SaveChanges();
            }

            else
            {
                return 0;
            }
            if (AreaEliminar != null)
            {
                return AreaEliminar.id;
            }
            else
            {
                return 0;
            }

        }

        public static int EliminarPersona(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            personas persona = bd.personas.Where(x => x.Id == id).FirstOrDefault();

            if (persona != null)
            {
                bd.personas.Remove(persona);
                bd.SaveChanges();
            }

            else
            {
                return 0;
            }
            if (persona != null)
            {
                return persona.Id;
            }
            else
            {
                return 0;
            }
        }

        public static void EliminarSubMedidas(int idMedida)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            List<submedidas_preventivas> listamedidas = bd.submedidas_preventivas.Where(x => x.id_medida_preventiva == idMedida).ToList();

            foreach (submedidas_preventivas item in listamedidas)
            {
                bd.submedidas_preventivas.Remove(item);
                bd.SaveChanges();
            }
        }
        public static void EliminarMEdida(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var AreaEliminar = bd.medidas_generales.Where(x => x.id == id).FirstOrDefault();

            if (AreaEliminar != null)
            {
                bd.medidas_generales.Remove(AreaEliminar);
                bd.SaveChanges();
            }
        }
        public static void EliminarMedidaPreventiva(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var AreaEliminar = bd.medidas_preventivas.Where(x => x.id == id).FirstOrDefault();

            if (AreaEliminar != null)
            {
                bd.medidas_preventivas.Remove(AreaEliminar);
                bd.SaveChanges();
            }
        }

        public static void EliminarTipoDoc(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var TipoDocAEliminar = bd.tipodocumento.Where(x => x.id == id).FirstOrDefault();

            if (TipoDocAEliminar != null)
            {
                bd.tipodocumento.Remove(TipoDocAEliminar);
                bd.SaveChanges();
            }


        }

        public static void EliminarReferencial(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var ReferencialAEliminar = bd.referenciales.Where(x => x.id == id).FirstOrDefault();

            if (ReferencialAEliminar != null)
            {
                bd.referenciales.Remove(ReferencialAEliminar);
                bd.SaveChanges();
            }


        }

        public static void EliminarAuditor(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            List<auditorias_auditores> AuditoresAsociadosAEliminar = bd.auditorias_auditores.Where(x => x.idAuditor == id).ToList();

            foreach (auditorias_auditores auditorasoc in AuditoresAsociadosAEliminar)
            {
                bd.auditorias_auditores.Remove(auditorasoc);
                bd.SaveChanges();
            }

            var AuditorAEliminar = bd.auditores.Where(x => x.id == id).FirstOrDefault();

            if (AuditorAEliminar != null)
            {
                bd.auditores.Remove(AuditorAEliminar);
                bd.SaveChanges();
            }


        }

        public static void EliminarTipoComunicacion(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var TipoAEliminar = bd.comunicacion_tipos.Where(x => x.id == id).FirstOrDefault();

            if (TipoAEliminar != null)
            {
                bd.comunicacion_tipos.Remove(TipoAEliminar);
                bd.SaveChanges();
            }


        }

        public static void EliminarClasifComunicacion(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var ClasifAEliminar = bd.comunicacion_clasificacion.Where(x => x.id == id).FirstOrDefault();

            if (ClasifAEliminar != null)
            {
                bd.comunicacion_clasificacion.Remove(ClasifAEliminar);
                bd.SaveChanges();
            }


        }

        public static void EliminarTecnologiaCentroVersion(int id_tecnologia, int id_centro, int id_version)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            var registros = (from u in bd.matriz_centro
                             where u.id_tecnologia == id_tecnologia &&
                                    u.id_centro == id_centro &&
                                    u.version == id_version
                             select u);
            foreach (matriz_centro item in registros)
            {
                bd.matriz_centro.Remove(item);

            }
            bd.SaveChanges();
        }

        public static void EliminarCanalComunicacion(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var CanalAEliminar = bd.comunicacion_canales.Where(x => x.id == id).FirstOrDefault();

            if (CanalAEliminar != null)
            {
                bd.comunicacion_canales.Remove(CanalAEliminar);
                bd.SaveChanges();
            }


        }

        public static void EliminarTipoEventoAmbiental(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var CanalAEliminar = bd.evento_ambiental_tipo.Where(x => x.id == id).FirstOrDefault();

            if (CanalAEliminar != null)
            {
                bd.evento_ambiental_tipo.Remove(CanalAEliminar);
                bd.SaveChanges();
            }
        }

        public static void EliminarCatRiesgo(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var CanalAEliminar = bd.riesgos_categorias.Where(x => x.id == id).FirstOrDefault();

            if (CanalAEliminar != null)
            {
                bd.riesgos_categorias.Remove(CanalAEliminar);
                bd.SaveChanges();
            }
        }

        public static void EliminarTipRiesgo(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var CanalAEliminar = bd.riesgos_tipologias.Where(x => x.id == id).FirstOrDefault();

            if (CanalAEliminar != null)
            {
                bd.riesgos_tipologias.Remove(CanalAEliminar);
                bd.SaveChanges();
            }
        }

        public static void EliminarMatrizEventoAmbiental(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var MatrizAEliminar = bd.evento_ambiental_matriz.Where(x => x.id == id).FirstOrDefault();

            if (MatrizAEliminar != null)
            {
                bd.evento_ambiental_matriz.Remove(MatrizAEliminar);
                bd.SaveChanges();
            }
        }

        public static void EliminarRiesgo(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var RiesgoAEliminar = bd.Riesgos.Where(x => x.Id == id).FirstOrDefault();

            if (RiesgoAEliminar != null)
            {
                bd.Riesgos.Remove(RiesgoAEliminar);
                bd.SaveChanges();
            }


        }

        public static void EliminarTipoAccMejora(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var TipoAccMejoraAEliminar = bd.tipo_accionesmejora.Where(x => x.id == id).FirstOrDefault();

            if (TipoAccMejoraAEliminar != null)
            {
                bd.tipo_accionesmejora.Remove(TipoAccMejoraAEliminar);
                bd.SaveChanges();
            }


        }

        public static void EliminarArenaN1(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var AreaAEliminar = bd.areanivel1.Where(x => x.id == id).FirstOrDefault();

            if (AreaAEliminar != null)
            {
                bd.areanivel1.Remove(AreaAEliminar);
                bd.SaveChanges();
            }


        }

        public static void EliminarStakeholderN1(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var AreaAEliminar = bd.areanivel1.Where(x => x.id == id).FirstOrDefault();

            if (AreaAEliminar != null)
            {
                bd.areanivel1.Remove(AreaAEliminar);
                bd.SaveChanges();
            }


        }

        public static void EliminarStakeholderN2(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var StakeholderAEliminar = bd.stakeholders_nivel2.Where(x => x.id == id).FirstOrDefault();

            if (StakeholderAEliminar != null)
            {
                bd.stakeholders_nivel2.Remove(StakeholderAEliminar);
                bd.SaveChanges();
            }


        }

        public static void EliminarStakeholderN3(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var StakeholderAEliminar = bd.stakeholders_nivel3.Where(x => x.id == id).FirstOrDefault();

            if (StakeholderAEliminar != null)
            {
                bd.stakeholders_nivel3.Remove(StakeholderAEliminar);
                bd.SaveChanges();
            }


        }

        public static void EliminarAmbito(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var AmbitoAEliminar = bd.ambitos.Where(x => x.id == id).FirstOrDefault();

            if (AmbitoAEliminar != null)
            {
                bd.ambitos.Remove(AmbitoAEliminar);
                bd.SaveChanges();
            }


        }

        public static void EliminarUsuario(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var usuarioAEliminar = bd.usuarios.Where(x => x.idUsuario == id).FirstOrDefault();

            if (usuarioAEliminar != null)
            {
                usuarioAEliminar.baja = true;
                bd.SaveChanges();
            }


        }

        public static void HabilitarUsuario(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var usuarioAEliminar = bd.usuarios.Where(x => x.idUsuario == id).FirstOrDefault();

            if (usuarioAEliminar != null)
            {
                usuarioAEliminar.baja = false;
                bd.SaveChanges();
            }


        }

        public static int EliminarProceso(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            List<procesos> procesosAEliminar = bd.procesos.Where(x => x.padre == id).ToList();

            if (procesosAEliminar.Count > 0)
            {
                foreach (procesos proc in procesosAEliminar)
                {
                    EliminarProceso(proc.id);
                }
            }

            procesos proceso = bd.procesos.Where(x => x.id == id).FirstOrDefault();

            bd.procesos.Remove(proceso);
            bd.SaveChanges();

            return id;
        }

        public static int EliminarObjetivo(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            objetivos objetivo = bd.objetivos.Where(x => x.id == id).FirstOrDefault();

            bd.objetivos.Remove(objetivo);
            bd.SaveChanges();

            return id;
        }

        public static int EliminarRequisito(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            requisitoslegales requisito = bd.requisitoslegales.Where(x => x.id == id).FirstOrDefault();

            bd.requisitoslegales.Remove(requisito);
            bd.SaveChanges();

            return id;
        }

        public static int EliminarFormacion(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            formacion form = bd.formacion.Where(x => x.id == id).FirstOrDefault();

            bd.formacion.Remove(form);
            bd.SaveChanges();

            return id;
        }

        public static int EliminarEmergencia(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            emergencias form = bd.emergencias.Where(x => x.id == id).FirstOrDefault();

            bd.emergencias.Remove(form);
            bd.SaveChanges();

            return id;
        }

        public static int EliminarIndicadorPlanificado(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            indicadores_planificacion form = bd.indicadores_planificacion.Where(x => x.Id == id).FirstOrDefault();

            bd.indicadores_planificacion.Remove(form);
            bd.SaveChanges();

            return id;
        }

        public static int EliminarAspectoValoracion(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            aspecto_valoracion form = bd.aspecto_valoracion.Where(x => x.id == id).FirstOrDefault();

            bd.aspecto_valoracion.Remove(form);
            bd.SaveChanges();

            return id;
        }

        public static int EliminarParametroValoracion(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            aspecto_parametro_valoracion form = bd.aspecto_parametro_valoracion.Where(x => x.id == id).FirstOrDefault();

            int idAspecto = form.id_aspecto;

            bd.aspecto_parametro_valoracion.Remove(form);
            bd.SaveChanges();

            return idAspecto;
        }

        public static int EliminarRevision(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            revision_energetica form = bd.revision_energetica.Where(x => x.id == id).FirstOrDefault();

            bd.revision_energetica.Remove(form);
            bd.SaveChanges();

            return id;
        }

        public static int EliminarSatisfaccion(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            satisfaccion form = bd.satisfaccion.Where(x => x.id == id).FirstOrDefault();

            bd.satisfaccion.Remove(form);
            bd.SaveChanges();

            return id;
        }

        public static int EliminarStakeholderN4(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            stakeholders_nivel4 form = bd.stakeholders_nivel4.Where(x => x.id == id).FirstOrDefault();

            bd.stakeholders_nivel4.Remove(form);
            bd.SaveChanges();

            return id;
        }

        public static int EliminarComunicacion(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            comunicacion comu = bd.comunicacion.Where(x => x.id == id).FirstOrDefault();

            bd.comunicacion.Remove(comu);
            bd.SaveChanges();

            return id;
        }

        public static int EliminarParte(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            partes comu = bd.partes.Where(x => x.id == id).FirstOrDefault();

            bd.partes.Remove(comu);
            bd.SaveChanges();

            return id;
        }

        public static int EliminarEventoAmb(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            evento_ambiental comu = bd.evento_ambiental.Where(x => x.id == id).FirstOrDefault();

            bd.evento_ambiental.Remove(comu);
            bd.SaveChanges();

            return id;
        }

        public static int EliminarEventoSeg(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            evento_seguridad comu = bd.evento_seguridad.Where(x => x.id == id).FirstOrDefault();

            bd.evento_seguridad.Remove(comu);
            bd.SaveChanges();

            return id;
        }

        public static int EliminarEventoCal(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            evento_calidad comu = bd.evento_calidad.Where(x => x.id == id).FirstOrDefault();

            bd.evento_calidad.Remove(comu);
            bd.SaveChanges();

            return id;
        }

        public static void EliminarPais(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var paisAEliminar = bd.pais.Where(x => x.id_pais == id).FirstOrDefault();

            if (paisAEliminar != null)
            {
                bd.pais.Remove(paisAEliminar);
                bd.SaveChanges();
            }


        }

        public static void EliminarCCAA(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var CCAAAEliminar = bd.comunidad_autonoma.Where(x => x.id_comunidad_autonoma == id).FirstOrDefault();

            if (CCAAAEliminar != null)
            {
                bd.comunidad_autonoma.Remove(CCAAAEliminar);
                bd.SaveChanges();
            }


        }

        public static void EliminarFichero(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var ficheroAEliminar = bd.ficheros.Where(x => x.id == id).FirstOrDefault();

            if (ficheroAEliminar != null)
            {
                bd.ficheros.Remove(ficheroAEliminar);
                bd.SaveChanges();
            }


        }

        public static void EliminarNorma(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var ficheroAEliminar = bd.normas.Where(x => x.id == id).FirstOrDefault();

            if (ficheroAEliminar != null)
            {
                bd.normas.Remove(ficheroAEliminar);
                bd.SaveChanges();
            }


        }

        public static void EliminarMaterial(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var ficheroAEliminar = bd.materialdivulgativo.Where(x => x.id == id).FirstOrDefault();

            if (ficheroAEliminar != null)
            {
                bd.materialdivulgativo.Remove(ficheroAEliminar);
                bd.SaveChanges();
            }


        }

        public static void EliminarInformeSeguridad(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var ficheroAEliminar = bd.informesseguridad.Where(x => x.id == id).FirstOrDefault();

            if (ficheroAEliminar != null)
            {
                bd.informesseguridad.Remove(ficheroAEliminar);
                bd.SaveChanges();
            }


        }

        public static void EliminarEvaluacionesRiesgos(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var ficheroAEliminar = bd.evaluacionriesgos.Where(x => x.id == id).FirstOrDefault();

            if (ficheroAEliminar != null)
            {
                bd.evaluacionriesgos.Remove(ficheroAEliminar);
                bd.SaveChanges();
            }


        }

        public static void EliminarEnlace(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var ficheroAEliminar = bd.enlaces.Where(x => x.id == id).FirstOrDefault();

            if (ficheroAEliminar != null)
            {
                bd.enlaces.Remove(ficheroAEliminar);
                bd.SaveChanges();
            }


        }

        public static void EliminarDocumentacion(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var ficheroAEliminar = bd.documentacion.Where(x => x.idFichero == id).FirstOrDefault();

            if (ficheroAEliminar != null)
            {
                bd.documentacion.Remove(ficheroAEliminar);
                bd.SaveChanges();
            }


        }


        public static int EliminarEvidencia(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            evidencias ficheroAEliminar = bd.evidencias.Where(x => x.id == id).FirstOrDefault();

            int idAccion = 0;
            if (ficheroAEliminar != null)
            {
                idAccion = ficheroAEliminar.idaccion;
                bd.evidencias.Remove(ficheroAEliminar);
                bd.SaveChanges();
            }

            return idAccion;
        }

        public static void EliminarDocumentacionHist(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var ficheroAEliminar = bd.documentacion_hist.Where(x => x.id == id).FirstOrDefault();

            if (ficheroAEliminar != null)
            {
                bd.documentacion_hist.Remove(ficheroAEliminar);
                bd.SaveChanges();
            }


        }

        public static void EliminarUsuarioCentro(int idUsuario, int idCentro)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var ficheroAEliminar = bd.usuario_centros.Where(x => x.idcentro == idCentro && x.idusuario == idUsuario).FirstOrDefault();

            if (ficheroAEliminar != null)
            {
                bd.usuario_centros.Remove(ficheroAEliminar);
                bd.SaveChanges();
            }


        }

        public static int EliminarParametroCentro(int idParametro, int idCentral)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            int idReturn = 0;
            indicadores_hojadedatos_valores ficheroAEliminar = bd.indicadores_hojadedatos_valores.Where(x => x.anio == 2018 && x.CodIndiHojaDatos == idParametro && x.idcentral == idCentral).FirstOrDefault();

            if (ficheroAEliminar != null)
            {
                idReturn = ficheroAEliminar.CodIndiHojaDatos;
                ficheroAEliminar.baja = true;
                bd.SaveChanges();
                return idReturn;
            }
            else
            {
                return 0;
            }


        }

        public static void EliminarIndicador(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var indicadorAEliminar = bd.indicadores.Where(x => x.Id == id).FirstOrDefault();

            if (indicadorAEliminar != null)
            {
                indicadorAEliminar.Activo = false;
                bd.SaveChanges();
            }
        }

        public static void EliminarAspectoTipo(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var aspectoAEliminar = bd.aspecto_tipo.Where(x => x.id == id).FirstOrDefault();

            if (aspectoAEliminar != null)
            {
                aspectoAEliminar.Activo = false;
                bd.SaveChanges();
            }
        }

        public static void ActivarIndicador(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var indicadorAActivar = bd.indicadores.Where(x => x.Id == id).FirstOrDefault();

            if (indicadorAActivar != null)
            {
                indicadorAActivar.Activo = true;
                bd.SaveChanges();
            }
        }

        public static int EliminarAccionObjetivo(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var accionAEliminar = bd.despliegue.Where(x => x.id == id).FirstOrDefault();
            int objetivo = accionAEliminar.idObjetivo;
            if (accionAEliminar != null)
            {
                bd.despliegue.Remove(accionAEliminar);
                bd.SaveChanges();
            }

            return objetivo;
        }

        public static void EliminarAccionMejora(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var accionAEliminar = bd.accionesmejora.Where(x => x.id == id).FirstOrDefault();
            if (accionAEliminar != null)
            {
                bd.accionesmejora.Remove(accionAEliminar);
                bd.SaveChanges();
            }

        }

        public static int EliminarAccionAccionMejora(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var accionAEliminar = bd.accionmejora_accion.Where(x => x.id == id).FirstOrDefault();
            int accionmejora = accionAEliminar.idaccionmejora;
            if (accionAEliminar != null)
            {
                bd.accionmejora_accion.Remove(accionAEliminar);
                bd.SaveChanges();
            }

            return accionmejora;
        }

        public static void EliminarAsociacionComunicacionProcesos(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            List<comunicacion_procesos> asociacionComunicacionProcesos = bd.comunicacion_procesos.Where(x => x.idcomunicacion == id).ToList();

            foreach (comunicacion_procesos comproc in asociacionComunicacionProcesos)
            {
                bd.comunicacion_procesos.Remove(comproc);
                bd.SaveChanges();
            }
        }

        public static void EliminarAsociacionIndicadorCentros(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            List<indicador_centrales> asociacionIndicadorCentrales = bd.indicador_centrales.Where(x => x.idIndicador == id).ToList();

            foreach (indicador_centrales objcen in asociacionIndicadorCentrales)
            {
                bd.indicador_centrales.Remove(objcen);
                bd.SaveChanges();
            }
        }

        public static void EliminarAsociacionObjetivoCentros(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            List<objetivo_centrales> asociacionObjetivosCentrales = bd.objetivo_centrales.Where(x => x.idObjetivo == id).ToList();

            foreach (objetivo_centrales objcen in asociacionObjetivosCentrales)
            {
                bd.objetivo_centrales.Remove(objcen);
                bd.SaveChanges();
            }
        }

        public static void EliminarAsociacionRiesgoStakeholders(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            List<riesgos_stakeholders> asociacionRiesgoStakeholders = bd.riesgos_stakeholders.Where(x => x.idriesgo == id).ToList();

            foreach (riesgos_stakeholders riesta in asociacionRiesgoStakeholders)
            {
                bd.riesgos_stakeholders.Remove(riesta);
                bd.SaveChanges();
            }
        }

        public static void EliminarAsociacionAccMejoraReferencial(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            List<accionmejora_referencial> asociacionAccMejoraReferencial = bd.accionmejora_referencial.Where(x => x.idaccionmejora == id).ToList();

            foreach (accionmejora_referencial accmref in asociacionAccMejoraReferencial)
            {
                bd.accionmejora_referencial.Remove(accmref);
                bd.SaveChanges();
            }
        }

        public static void EliminarAsociacionObjetivoTecnologias(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            List<objetivo_tecnologias> asociacionObjetivosTecnologias = bd.objetivo_tecnologias.Where(x => x.idObjetivo == id).ToList();

            foreach (objetivo_tecnologias objcen in asociacionObjetivosTecnologias)
            {
                bd.objetivo_tecnologias.Remove(objcen);
                bd.SaveChanges();
            }
        }

        public static int EliminarReferencialAsociado(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var referencialAEliminar = bd.auditorias_referenciales.Where(x => x.id == id).FirstOrDefault();
            auditorias_referenciales valoracion = bd.auditorias_referenciales.Where(x => x.id == referencialAEliminar.id).FirstOrDefault();
            int idAuditoria = valoracion.idAuditoria;
            if (referencialAEliminar != null)
            {
                bd.auditorias_referenciales.Remove(referencialAEliminar);
                bd.SaveChanges();
            }

            return idAuditoria;
        }

        public static int EliminarObservadorAsociado(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var observadorAEliminar = bd.auditorias_observador.Where(x => x.id == id).FirstOrDefault();
            auditorias_observador valoracion = bd.auditorias_observador.Where(x => x.id == observadorAEliminar.id).FirstOrDefault();
            int idAuditoria = valoracion.idAuditoria;
            if (observadorAEliminar != null)
            {
                bd.auditorias_observador.Remove(observadorAEliminar);
                bd.SaveChanges();
            }

            return idAuditoria;
        }

        public static int EliminarAuditorAsociado(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var auditorAEliminar = bd.auditorias_auditores.Where(x => x.id == id).FirstOrDefault();
            auditorias_auditores valoracion = bd.auditorias_auditores.Where(x => x.id == auditorAEliminar.id).FirstOrDefault();
            int idAuditoria = valoracion.idAuditoria;
            if (auditorAEliminar != null)
            {
                bd.auditorias_auditores.Remove(auditorAEliminar);
                bd.SaveChanges();
            }

            return idAuditoria;
        }

        public static int EliminarNoticia(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var noticiaAEliminar = bd.noticias.Where(x => x.id == id).FirstOrDefault();
            noticias valoracion = bd.noticias.Where(x => x.id == noticiaAEliminar.id).FirstOrDefault();
            int noticia = valoracion.id;
            if (noticiaAEliminar != null)
            {
                bd.noticias.Remove(noticiaAEliminar);
                bd.SaveChanges();
            }

            return noticia;
        }

        public static int EliminarCualificacion(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var cualificacionAEliminar = bd.cualificaciones.Where(x => x.id == id).FirstOrDefault();
            cualificaciones cualificacion = bd.cualificaciones.Where(x => x.id == cualificacionAEliminar.id).FirstOrDefault();
            int idAuditoria = cualificacion.idAuditoria;
            if (cualificacionAEliminar != null)
            {
                bd.cualificaciones.Remove(cualificacionAEliminar);
                bd.SaveChanges();
            }

            return idAuditoria;
        }

        public static int EliminarDocComunicacion(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var cualificacionAEliminar = bd.comunicacion_documentos.Where(x => x.id == id).FirstOrDefault();
            comunicacion_documentos cualificacion = bd.comunicacion_documentos.Where(x => x.id == cualificacionAEliminar.id).FirstOrDefault();
            int idAuditoria = cualificacion.idcomunicacion;
            if (cualificacionAEliminar != null)
            {
                bd.comunicacion_documentos.Remove(cualificacionAEliminar);
                bd.SaveChanges();
            }

            return idAuditoria;
        }

        public static int EliminarDocAccMejora(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var cualificacionAEliminar = bd.accionmejora_documento.Where(x => x.id == id).FirstOrDefault();
            accionmejora_documento cualificacion = bd.accionmejora_documento.Where(x => x.id == cualificacionAEliminar.id).FirstOrDefault();
            int idAuditoria = cualificacion.idaccionmejora;
            if (cualificacionAEliminar != null)
            {
                bd.accionmejora_documento.Remove(cualificacionAEliminar);
                bd.SaveChanges();
            }

            return idAuditoria;
        }

        public static int EliminarReunion(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var reunionAEliminar = bd.reuniones.Where(x => x.id == id).FirstOrDefault();
            reuniones reunion = bd.reuniones.Where(x => x.id == reunionAEliminar.id).FirstOrDefault();
            int idReunion = reunion.id;
            if (reunionAEliminar != null)
            {
                bd.reuniones.Remove(reunionAEliminar);
                bd.SaveChanges();
            }

            return idReunion;
        }

        public static int EliminarDocReunion(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var documentoAEliminar = bd.reuniones_documentos.Where(x => x.id == id).FirstOrDefault();
            reuniones_documentos documento = bd.reuniones_documentos.Where(x => x.id == documentoAEliminar.id).FirstOrDefault();
            int idReunion = documento.idreunion;
            if (documentoAEliminar != null)
            {
                bd.reuniones_documentos.Remove(documentoAEliminar);
                bd.SaveChanges();
            }

            return idReunion;
        }

        public static int EliminarDocEventoAmb(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var documentoAEliminar = bd.evento_ambiental_documentos.Where(x => x.id == id).FirstOrDefault();
            evento_ambiental_documentos documento = bd.evento_ambiental_documentos.Where(x => x.id == documentoAEliminar.id).FirstOrDefault();
            int idEventoAmb = documento.ideventoamb;
            if (documentoAEliminar != null)
            {
                bd.evento_ambiental_documentos.Remove(documentoAEliminar);
                bd.SaveChanges();
            }

            return idEventoAmb;
        }

        public static int EliminarDocSeg(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var documentoAEliminar = bd.evento_seguridad_documentos.Where(x => x.id == id).FirstOrDefault();
            evento_seguridad_documentos documento = bd.evento_seguridad_documentos.Where(x => x.id == documentoAEliminar.id).FirstOrDefault();
            int idEventoAmb = documento.idSeg;
            if (documentoAEliminar != null)
            {
                bd.evento_seguridad_documentos.Remove(documentoAEliminar);
                bd.SaveChanges();
            }

            return idEventoAmb;
        }

        public static int EliminarFotoEventoAmb(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var documentoAEliminar = bd.evento_ambiental_foto.Where(x => x.id == id).FirstOrDefault();
            evento_ambiental_foto documento = bd.evento_ambiental_foto.Where(x => x.id == documentoAEliminar.id).FirstOrDefault();
            int idEventoAmb = documento.idEventoAmbiental;
            if (documentoAEliminar != null)
            {
                bd.evento_ambiental_foto.Remove(documentoAEliminar);
                bd.SaveChanges();
            }

            return idEventoAmb;
        }

        public static int EliminarFotoEventoSeg(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var documentoAEliminar = bd.evento_ambiental_foto.Where(x => x.id == id).FirstOrDefault();
            evento_seguridad_foto documento = bd.evento_seguridad_foto.Where(x => x.id == documentoAEliminar.id).FirstOrDefault();
            int idEventoSeg = documento.idEventoSeg;
            if (documentoAEliminar != null)
            {
                bd.evento_ambiental_foto.Remove(documentoAEliminar);
                bd.SaveChanges();
            }

            return idEventoSeg;
        }

        public static void EliminarAuditoria(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var auditoriaAEliminar = bd.auditorias.Where(x => x.id == id).FirstOrDefault();
            if (auditoriaAEliminar != null)
            {
                bd.auditorias.Remove(auditoriaAEliminar);
                bd.SaveChanges();
            }
        }

        public static List<indicadores> ListarIndicadoresAplicables(centros central)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<indicadores> listaIndicadores = new List<indicadores>();

            listaIndicadores = (from u in bd.indicadores
                                where (((u.tecnologia == null || u.tecnologia == 0 || u.tecnologia == central.tipo) && u.especifico != 1)
                                || ((u.especifico == 1) && ((from oc in bd.indicador_centrales where oc.idIndicador == u.Id select oc.idCentro).Contains(central.id))))
                                && u.Activo == true
                                select u).ToList();

            return listaIndicadores;
        }

        public static List<VISTA_ParametrosInd> ListarParametrosInd()
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<VISTA_ParametrosInd> listaParametros = new List<VISTA_ParametrosInd>();

            listaParametros = (from u in bd.VISTA_ParametrosInd
                               select u).ToList();

            return listaParametros;
        }

        public static List<VISTA_ParametrosInd> ListarParametrosInd(int idCentral)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<VISTA_ParametrosInd> listaParametros = new List<VISTA_ParametrosInd>();

            listaParametros = (from u in bd.VISTA_ParametrosInd
                               where (from val in bd.indicadores_hojadedatos_valores
                                      where val.idcentral == idCentral && val.anio == 2018
                                      && val.baja != true
                                      select val.CodIndiHojaDatos).Contains(u.id)
                               select u).ToList();

            return listaParametros;
        }

        public static List<VISTA_AspectoTipo> ListarGruposAplicables(int Foco)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<VISTA_AspectoTipo> listaAspectos = new List<VISTA_AspectoTipo>();

            if (Foco == 1)
            {
                listaAspectos = (from u in bd.VISTA_AspectoTipo
                                 where u.Grupo == 1 || u.Grupo == 6 || (u.Grupo == 12 && u.Codigo != "AD-CO-4")
                                 || u.Grupo == 13 || u.Grupo == 14
                                 orderby u.Grupo
                                 select u).ToList();
            }
            else
            {
                listaAspectos = (from u in bd.VISTA_AspectoTipo
                                 where u.Grupo != 1 && u.Grupo != 6
                                 && u.Grupo != 13 && u.Grupo != 14
                                 && u.Codigo != "AD-CO-5" && u.Codigo != "AD-CO-6" && u.Codigo != "AD-CO-7"
                                 orderby u.Grupo
                                 select u).ToList();
            }

            return listaAspectos;
        }

        public static List<VISTA_AspectosDesplegable> ListarIndicadoresAplicables()
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<VISTA_AspectosDesplegable> listaAspectos = new List<VISTA_AspectosDesplegable>();

            listaAspectos = (from u in bd.VISTA_AspectosDesplegable
                             where u.Activo == true
                             select u).ToList();

            return listaAspectos;
        }

        public static List<VISTA_MaestroAspectos> ListarAspectos()
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<VISTA_MaestroAspectos> listaIndicadores = new List<VISTA_MaestroAspectos>();

            listaIndicadores = (from u in bd.VISTA_MaestroAspectos
                                select u).ToList();

            return listaIndicadores;
        }

        public static List<VISTA_Indicadores> ListarIndicadores()
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<VISTA_Indicadores> listaIndicadores = new List<VISTA_Indicadores>();

            listaIndicadores = (from u in bd.VISTA_Indicadores
                                select u).ToList();

            foreach (var data in listaIndicadores)
            {

                List<VISTA_Indicadores_Centros> centralesAsignadas = (from u in bd.VISTA_Indicadores_Centros
                                                                      where u.idIndicador == data.Id
                                                                      select u).ToList();
                foreach (VISTA_Indicadores_Centros centro in centralesAsignadas)
                {
                    if (centralesAsignadas.Count == 1)
                    {
                        if (data.listadoCentrales == String.Empty || data.listadoCentrales == null || data.listadoCentrales == "")
                            data.listadoCentrales = data.listadoCentrales + " " + centro.nombre;
                    }
                }
            }

            listaIndicadores = (from u in listaIndicadores
                                where u.Activo == "Activo"
                                select u).ToList();

            return listaIndicadores;
        }

        public static List<VISTA_IndicadoresFichaDireccion> ListarIndicadoresPlanificados(int idCentral, int anio)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<VISTA_IndicadoresFichaDireccion> listaIndicadores = new List<VISTA_IndicadoresFichaDireccion>();

            listaIndicadores = (from u in bd.VISTA_IndicadoresFichaDireccion
                                where u.idcentral == idCentral && u.anio == anio
                                select u).ToList();

            return listaIndicadores;
        }

        public static List<VISTA_AspectosValoracion> ListarAspectosValoracion(int idCentral)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<VISTA_AspectosValoracion> listaAspectos = new List<VISTA_AspectosValoracion>();

            listaAspectos = (from u in bd.VISTA_AspectosValoracion
                             where u.idCentral == idCentral
                             select u).ToList();

            return listaAspectos;
        }

        public static List<VISTA_AspectosValoracion> ListarAspectosValoracion(int idCentral, string anio)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<VISTA_AspectosValoracion> listaAspectos = new List<VISTA_AspectosValoracion>();

            listaAspectos = (from u in bd.VISTA_AspectosValoracion
                             where u.idCentral == idCentral && u.Codigo.Contains(anio)
                             select u).ToList();

            return listaAspectos;
        }

        public static List<VISTA_ListarLogEvaluacionesParametro> ListarLogEvaluacionesParametro(int idCentral, int anio)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<VISTA_ListarLogEvaluacionesParametro> listaAspectos = new List<VISTA_ListarLogEvaluacionesParametro>();

            listaAspectos = (from u in bd.VISTA_ListarLogEvaluacionesParametro
                             where u.idCentral == idCentral && u.anio == anio
                             select u).ToList();

            return listaAspectos;
        }

        public static List<VISTA_ListarLogEvaluacionesFoco> ListarLogEvaluacionesFoco(int idCentral, int anio)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<VISTA_ListarLogEvaluacionesFoco> listaAspectos = new List<VISTA_ListarLogEvaluacionesFoco>();

            listaAspectos = (from u in bd.VISTA_ListarLogEvaluacionesFoco
                             where u.idCentral == idCentral && u.anio == anio
                             select u).ToList();

            return listaAspectos;
        }

        public static List<VISTA_ListarLogParametrosFocos> ListarLogParametrosFocos(int idCentral, int anio)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<VISTA_ListarLogParametrosFocos> listaAspectos = new List<VISTA_ListarLogParametrosFocos>();

            listaAspectos = (from u in bd.VISTA_ListarLogParametrosFocos
                             where u.idCentral == idCentral && u.anio == anio
                             select u).ToList();

            return listaAspectos;
        }

        public static List<VISTA_AspectosValoracion> ListarAspectosValoracion(int idCentral, int Foco)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<VISTA_AspectosValoracion> listaAspectos = new List<VISTA_AspectosValoracion>();

            listaAspectos = (from u in bd.VISTA_AspectosValoracion
                             where u.idCentral == idCentral
                             && u.foco == Foco
                             select u).ToList();

            return listaAspectos;
        }

        public static List<aspecto_valoracion> ListarAspectosResiduos(int idCentral)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<aspecto_valoracion> listaAspectos = new List<aspecto_valoracion>();

            listaAspectos = (from u in bd.aspecto_valoracion
                             join
    at in bd.aspecto_tipo on u.idAspecto equals at.id
                             where u.idCentral == idCentral && at.Grupo == 8
                             select u).ToList();

            return listaAspectos;
        }

        public static List<aspecto_valoracion> ListarAspectosCombSust(int idCentral)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<aspecto_valoracion> listaAspectos = new List<aspecto_valoracion>();

            listaAspectos = (from u in bd.aspecto_valoracion
                             join
                                 at in bd.aspecto_tipo on u.idAspecto equals at.id
                             where u.idCentral == idCentral && (at.Grupo == 9 || at.Grupo == 12)
                             select u).ToList();

            return listaAspectos;
        }

        public static List<aspecto_parametro_valoracion> ListarParametrosCombSust(int idCentral)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<aspecto_parametro_valoracion> listaAspectos = new List<aspecto_parametro_valoracion>();

            listaAspectos = (from u in bd.aspecto_parametro_valoracion
                             join av in bd.aspecto_valoracion on u.id_aspecto equals av.id
                             join at in bd.aspecto_tipo on av.idAspecto equals at.id
                             where av.idCentral == idCentral && (at.Grupo == 9 || at.Grupo == 12)
                             select u).ToList();

            return listaAspectos;
        }

        public static decimal CalcularTotalResiduosCentral(int idCentral)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            decimal totalaspectos = 0;

            try
            {

                totalaspectos = decimal.Parse((from u in bd.aspecto_valoracion
                                               join
                                               a in bd.aspecto_tipo on u.idAspecto equals a.id
                                               where u.idCentral == idCentral && a.Grupo == 8
                                               select u.anio6).Sum().ToString());
            }
            catch (Exception ex)
            {
                totalaspectos = 0;
            }

            return totalaspectos;
        }

        public static decimal CalcularTotalConsumoCombustibleCentral(int idCentral)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            decimal totalaspectos = 0;
            decimal totalparametros = 0;
            try
            {

                try
                {
                    totalaspectos = decimal.Parse((from u in bd.aspecto_valoracion
                                                   join
                                                   a in bd.aspecto_tipo on u.idAspecto equals a.id
                                                   where u.idCentral == idCentral && (a.Grupo == 9 || a.Grupo == 12) && u.foco != 1
                                                   select u.anio6).Sum().ToString());
                }
                catch (Exception ex)
                {
                    totalaspectos = 0;
                }


                try
                {
                    totalparametros = decimal.Parse((from p in bd.aspecto_parametro_valoracion
                                                     join
               u in bd.aspecto_valoracion on p.id_aspecto equals u.id
                                                     join
                                                     a in bd.aspecto_tipo on u.idAspecto equals a.id
                                                     where u.idCentral == idCentral && (a.Grupo == 9 || a.Grupo == 12)
                                                     select p.anio6).Sum().ToString());
                }
                catch (Exception ex)
                {
                    totalparametros = 0;
                }

                totalaspectos = totalaspectos + totalparametros;
            }
            catch (Exception ex)
            {
                totalaspectos = 0;
            }

            return totalaspectos;
        }

        public static decimal CalcularTotalConsumoSustanciasCentral(int idCentral)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            decimal totalaspectos = 0;
            decimal totalparametros = 0;

            try
            {

                totalaspectos = decimal.Parse((from u in bd.aspecto_valoracion
                                               join
                                               a in bd.aspecto_tipo on u.idAspecto equals a.id
                                               where u.idCentral == idCentral && (a.Grupo == 9 || a.Grupo == 12) && u.foco != 1
                                               select u.anio6).Sum().ToString());
            }
            catch (Exception ex)
            {
                totalaspectos = 0;
            }

            try
            {
                totalparametros = decimal.Parse((from p in bd.aspecto_parametro_valoracion
                                                 join
                                                     u in bd.aspecto_valoracion on p.id_aspecto equals u.id
                                                 join
                                                 a in bd.aspecto_tipo on u.idAspecto equals a.id
                                                 where u.idCentral == idCentral && (a.Grupo == 9 || a.Grupo == 12)
                                                 select p.anio6).Sum().ToString());
            }
            catch (Exception ex)
            {
                totalparametros = 0;
            }

            totalaspectos = totalaspectos + totalparametros;

            return totalaspectos;
        }

        public static List<objetivos> ListarObjetivos(int idOrganizacion)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<objetivos> listaObjetivos = new List<objetivos>();

            listaObjetivos = (from u in bd.objetivos
                              where u.idorganizacion == idOrganizacion
                              select u).ToList();

            return listaObjetivos;
        }

        public static List<objetivos> ListarObjetivos(int idOrganizacion, string anio)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<objetivos> listaObjetivos = new List<objetivos>();

            listaObjetivos = (from u in bd.objetivos
                              where u.idorganizacion == idOrganizacion && u.Codigo.Contains(anio)
                              select u).ToList();

            return listaObjetivos;
        }

        public static List<objetivos> ListarObjetivosAccionMejora(int idOrganizacion)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<objetivos> listaObjetivos = new List<objetivos>();
            string anio = DateTime.Now.Year.ToString();
            string anioanterior = (DateTime.Now.Year - 1).ToString();

            listaObjetivos = (from u in bd.objetivos
                              where u.idorganizacion == idOrganizacion
                              where u.Codigo.Contains(anio) || u.Codigo.Contains(anioanterior)
                              select u).ToList();

            return listaObjetivos;
        }

        public static List<VISTA_Comunicaciones> ListarComunicaciones(int idCentral)
        {

            DIMASSTEntities bd = new DIMASSTEntities();
            List<VISTA_Comunicaciones> listaComunicaciones = new List<VISTA_Comunicaciones>();

            listaComunicaciones = (from u in bd.VISTA_Comunicaciones
                                   where u.idcentral == idCentral
                                   select u).ToList();

            return listaComunicaciones;
        }

        public static List<VISTA_Comunicaciones> ListarComunicaciones(int idCentral, string anio)
        {

            DIMASSTEntities bd = new DIMASSTEntities();
            List<VISTA_Comunicaciones> listaComunicaciones = new List<VISTA_Comunicaciones>();

            listaComunicaciones = (from u in bd.VISTA_Comunicaciones
                                   where u.idcentral == idCentral
                                   && u.idcomunicacion.Contains(anio)
                                   select u).ToList();

            return listaComunicaciones;
        }

        public static List<VISTA_Comunicaciones> ListarQuejas(int idCentral)
        {

            DIMASSTEntities bd = new DIMASSTEntities();
            List<VISTA_Comunicaciones> listaComunicaciones = new List<VISTA_Comunicaciones>();

            listaComunicaciones = (from u in bd.VISTA_Comunicaciones
                                   where u.idcentral == idCentral && u.Clasificacion == "Queja"
                                   select u).ToList();

            return listaComunicaciones;
        }

        public static List<VISTA_Comunicaciones> ListarComunicaciones(int idCentral, DateTime fechaInicio, DateTime fechaFin)
        {

            DIMASSTEntities bd = new DIMASSTEntities();
            List<VISTA_Comunicaciones> listaComunicaciones = new List<VISTA_Comunicaciones>();

            listaComunicaciones = (from u in bd.VISTA_Comunicaciones
                                   where u.idcentral == idCentral &&
                                   u.fechainicio >= fechaInicio && u.fechainicio <= fechaFin
                                   select u).ToList();

            return listaComunicaciones;
        }

        public static List<partes> ListarPartes(int idCentral)
        {

            DIMASSTEntities bd = new DIMASSTEntities();
            List<partes> listaPartes = new List<partes>();

            listaPartes = (from u in bd.partes
                           where u.idcentral == idCentral
                           select u).ToList();

            return listaPartes;
        }

        public static List<VISTA_ListarEventosAmbientales> ListarEventosAmbientales(int idCentral)
        {

            DIMASSTEntities bd = new DIMASSTEntities();
            List<VISTA_ListarEventosAmbientales> listaEventos = new List<VISTA_ListarEventosAmbientales>();

            listaEventos = (from u in bd.VISTA_ListarEventosAmbientales
                            where u.idcentral == idCentral
                            select u).ToList();

            return listaEventos;
        }

        public static List<VISTA_ListarEventosCalidad> ListarEventosCalidad(int idCentral)
        {

            DIMASSTEntities bd = new DIMASSTEntities();
            List<VISTA_ListarEventosCalidad> listaEventos = new List<VISTA_ListarEventosCalidad>();

            listaEventos = (from u in bd.VISTA_ListarEventosCalidad
                            where u.idcentral == idCentral
                            select u).ToList();

            return listaEventos;
        }

        public static List<VISTA_Eventos_Seguridad> ListarEventosSeguridad(int idCentral)
        {

            DIMASSTEntities bd = new DIMASSTEntities();
            List<VISTA_Eventos_Seguridad> listaEventos = new List<VISTA_Eventos_Seguridad>();

            listaEventos = (from u in bd.VISTA_Eventos_Seguridad
                            where u.idcentral == idCentral
                            select u).ToList();

            return listaEventos;
        }

        public static VISTA_Comunicaciones ListarComunicacion(int idComunicacion)
        {

            DIMASSTEntities bd = new DIMASSTEntities();
            VISTA_Comunicaciones listaComunicaciones = new VISTA_Comunicaciones();


            listaComunicaciones = (from u in bd.VISTA_Comunicaciones
                                   where u.id == idComunicacion
                                   select u).FirstOrDefault();

            return listaComunicaciones;
        }

        public static List<VISTA_ListarEmergencias> ListarEmergencias(int idCentral)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            List<VISTA_ListarEmergencias> listaEmergencias = new List<VISTA_ListarEmergencias>();

            listaEmergencias = (from u in bd.VISTA_ListarEmergencias
                                where u.idcentral == idCentral
                                select u).ToList();

            return listaEmergencias;
        }

        public static List<VISTA_ListarEmergencias> ListarEmergencias(int idCentral, DateTime fechaInicio, DateTime fechaFin)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            List<VISTA_ListarEmergencias> listaEmergencias = new List<VISTA_ListarEmergencias>();

            listaEmergencias = (from u in bd.VISTA_ListarEmergencias
                                where u.idcentral == idCentral
                                && u.fechaplanificada >= fechaInicio && u.fechaplanificada <= fechaFin
                                select u).ToList();

            return listaEmergencias;
        }

        public static List<VISTA_ListarEmergencias> ListarEmergencias(int idCentral, string anio)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            List<VISTA_ListarEmergencias> listaEmergencias = new List<VISTA_ListarEmergencias>();

            listaEmergencias = (from u in bd.VISTA_ListarEmergencias
                                where u.idcentral == idCentral
                                && u.codigo.Contains(anio)
                                select u).ToList();

            return listaEmergencias;
        }

        public static List<VISTA_ListarSatisfaccion> ListarSatisfaccion(int idCentral)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            List<VISTA_ListarSatisfaccion> listaSatisfaccion = new List<VISTA_ListarSatisfaccion>();

            listaSatisfaccion = (from u in bd.VISTA_ListarSatisfaccion
                                 where u.idcentral == idCentral
                                 select u).ToList();

            return listaSatisfaccion;
        }

        public static List<VISTA_ListarSatisfaccion> ListarSatisfaccion(int idCentral, DateTime fechaInicio, DateTime fechaFin)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            List<VISTA_ListarSatisfaccion> listaSatisfaccion = new List<VISTA_ListarSatisfaccion>();

            listaSatisfaccion = (from u in bd.VISTA_ListarSatisfaccion
                                 where u.idcentral == idCentral
                                 && u.fecharealizacion >= fechaInicio && u.fecharealizacion <= fechaFin
                                 select u).ToList();

            return listaSatisfaccion;
        }

        public static List<VISTA_ListarSatisfaccion> ListarSatisfaccion(int idCentral, string anio)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            List<VISTA_ListarSatisfaccion> listaSatisfaccion = new List<VISTA_ListarSatisfaccion>();

            listaSatisfaccion = (from u in bd.VISTA_ListarSatisfaccion
                                 where u.idcentral == idCentral
                                 && u.codigo.Contains(anio)
                                 select u).ToList();

            return listaSatisfaccion;
        }

        public static List<VISTA_ListarRevisionesEnergeticas> ListarRevisiones(int idCentral)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            List<VISTA_ListarRevisionesEnergeticas> listaFormacion = new List<VISTA_ListarRevisionesEnergeticas>();

            listaFormacion = (from u in bd.VISTA_ListarRevisionesEnergeticas
                              where u.idcentral == idCentral
                              select u).ToList();

            return listaFormacion;
        }

        public static List<reuniones> ListarReuniones(int idCentral)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            List<reuniones> listaReuniones = new List<reuniones>();

            listaReuniones = (from u in bd.reuniones
                              where u.idcentral == idCentral
                              select u).ToList();

            return listaReuniones;
        }

        public static List<reuniones> ListarReuniones(int idCentral, DateTime fechaInicio, DateTime fechaFin)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            List<reuniones> listaReuniones = new List<reuniones>();

            listaReuniones = (from u in bd.reuniones
                              where u.idcentral == idCentral
                              && u.fecha_convocatoria >= fechaInicio && u.fecha_convocatoria <= fechaFin
                              select u).ToList();

            return listaReuniones;
        }

        public static List<VISTA_ListarAccionesMejora> ListarAccionesMejora(int idCentral)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            List<VISTA_ListarAccionesMejora> listaAcciones = new List<VISTA_ListarAccionesMejora>();

            listaAcciones = (from u in bd.VISTA_ListarAccionesMejora
                             where u.idcentral == idCentral
                             select u).ToList();

            return listaAcciones;
        }

        public static List<documento_historico> ListaDocumentoHistorico(int idCentral)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            List<documento_historico> listaDocumentos = new List<documento_historico>();

            listaDocumentos = (from u in bd.documento_historico
                               where u.id_centro == idCentral && u.descarga == 0
                               orderby u.version ascending
                               select u).ToList();

            return listaDocumentos;
        }
        public static List<documentos_riesgos> ListaDocumentoRiesgosPorIdCentro(int idCentral)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            List<documentos_riesgos> listaDocumentos = new List<documentos_riesgos>();

            listaDocumentos = (from u in bd.documentos_riesgos
                               where u.id_centro == idCentral
                               select u).ToList();

            return listaDocumentos;
        }

        public static List<documento_historico_criticos> ListaDocumentoHistoricoDefinitivoCritico(int idCentral)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            List<documento_historico_criticos> listaDocumentos = new List<documento_historico_criticos>();

            listaDocumentos = (from u in bd.documento_historico_criticos
                               where u.id_centro == idCentral && u.descarga == 1
                               orderby u.version ascending
                               select u).ToList();

            return listaDocumentos;
        }

        public static List<documento_historico> ListaDocumentoHistoricoDefinitivo(int idCentral)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            List<documento_historico> listaDocumentos = new List<documento_historico>();

            listaDocumentos = (from u in bd.documento_historico
                               where u.id_centro == idCentral && u.descarga == 1
                               orderby u.version ascending
                               select u).ToList();

            return listaDocumentos;
        }

        public static medidaspreventivas_imagenes ObtenerMedidapreventivaImagen(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            medidaspreventivas_imagenes listaDocumentos = new medidaspreventivas_imagenes();

            listaDocumentos = (from u in bd.medidaspreventivas_imagenes
                               where u.id == id
                               select u).ToList().FirstOrDefault();

            return listaDocumentos;
        }

        public static documento_historico ObtenerUltimoDocumentoHistorico(int idCentral)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            documento_historico listaDocumentos = new documento_historico();

            listaDocumentos = (from u in bd.documento_historico
                               where u.id_centro == idCentral && u.descarga == 1
                               orderby u.version ascending
                               select u).ToList().LastOrDefault();

            return listaDocumentos;
        }

        public static documento_historico_criticos ObtenerUltimoDocumentoHistoricoCriticos(int idCentral)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            documento_historico_criticos listaDocumentos = new documento_historico_criticos();

            listaDocumentos = (from u in bd.documento_historico_criticos
                               where u.id_centro == idCentral && u.descarga == 1
                               orderby u.version ascending
                               select u).ToList().LastOrDefault();

            return listaDocumentos;
        }

        public static List<VISTA_ListarAccionesMejora> ListarAccionesMejora(int idCentral, string anio)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            List<VISTA_ListarAccionesMejora> listaAcciones = new List<VISTA_ListarAccionesMejora>();

            listaAcciones = (from u in bd.VISTA_ListarAccionesMejora
                             where u.idcentral == idCentral &&
                             (u.codigo.Contains(anio) || u.estado == 0)
                             select u).ToList();

            return listaAcciones;
        }

        public static List<VISTA_ListarAccionesMejora> ListarAccionesMejoraFechas(int idCentral, string anio)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            List<VISTA_ListarAccionesMejora> listaAcciones = new List<VISTA_ListarAccionesMejora>();

            listaAcciones = (from u in bd.VISTA_ListarAccionesMejora
                             where u.idcentral == idCentral &&
                             (u.codigo.Contains(anio) || u.estado == 0)
                             && u.ambito == 2
                             select u).ToList();

            return listaAcciones;
        }

        public static List<VISTA_ListarAccionesMejora> ListarAccionesMejora(int idCentral, DateTime fechaInicio, DateTime fechaFin)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            List<VISTA_ListarAccionesMejora> listaAcciones = new List<VISTA_ListarAccionesMejora>();

            listaAcciones = (from u in bd.VISTA_ListarAccionesMejora
                             where u.idcentral == idCentral
                             && u.fecha_apertura >= fechaInicio && u.fecha_apertura <= fechaFin
                             select u).ToList();

            return listaAcciones;
        }

        public static List<VISTA_ListarAccionesMejoraFicha> ListarAccionesMejoraFicha(int idAccionMejora)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            List<VISTA_ListarAccionesMejoraFicha> listaAcciones = new List<VISTA_ListarAccionesMejoraFicha>();

            listaAcciones = (from u in bd.VISTA_ListarAccionesMejoraFicha
                             where u.idaccionmejora == idAccionMejora
                             select u).ToList();

            return listaAcciones;
        }

        public static List<VISTA_ListarAccionesMejora> ListarAccionesMejora(int idCentral, int antecedente, int referencia)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            List<VISTA_ListarAccionesMejora> listaAcciones = new List<VISTA_ListarAccionesMejora>();

            listaAcciones = (from u in bd.VISTA_ListarAccionesMejora
                             where u.idcentral == idCentral
                             && u.antecedente == antecedente
                             && u.referencia == referencia
                             select u).ToList();

            return listaAcciones;
        }

        public static List<formacion> ListarFormacion(int idCentral)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            List<formacion> listaFormacion = new List<formacion>();

            listaFormacion = (from u in bd.formacion
                              where u.idcentral == idCentral
                              select u).ToList();

            return listaFormacion;
        }

        public static List<formacion> ListarFormacion(int idCentral, DateTime fechaInicio, DateTime fechaFin)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            List<formacion> listaFormacion = new List<formacion>();

            listaFormacion = (from u in bd.formacion
                              where u.idcentral == idCentral &&
                              u.fecha_registro_inicio >= fechaInicio && u.fecha_registro_inicio <= fechaFin
                              select u).ToList();

            return listaFormacion;
        }

        public static List<formacion> ListarFormacion(int idCentral, string anio)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            List<formacion> listaFormacion = new List<formacion>();

            listaFormacion = (from u in bd.formacion
                              where u.idcentral == idCentral &&
                              u.codigo.Contains(anio)
                              select u).ToList();

            return listaFormacion;
        }

        public static List<formacion> ListarFormacionInforme(int idCentral)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            List<formacion> listaFormacion = new List<formacion>();

            listaFormacion = (from u in bd.formacion
                              where u.idcentral == idCentral || u.idcentral == 0
                              select u).ToList();

            return listaFormacion;
        }

        public static List<VISTA_RequisitosLegales> ListarRequisitos(int idCentral)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            List<VISTA_RequisitosLegales> listaRequisitos = new List<VISTA_RequisitosLegales>();

            listaRequisitos = (from u in bd.VISTA_RequisitosLegales
                               where u.idcentral == idCentral
                               orderby u.id descending
                               select u).ToList();

            return listaRequisitos;
        }

        public static List<VISTA_RequisitosLegales> ListarRequisitos(int idCentral, string anio)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            List<VISTA_RequisitosLegales> listaRequisitos = new List<VISTA_RequisitosLegales>();

            listaRequisitos = (from u in bd.VISTA_RequisitosLegales
                               where u.idcentral == idCentral
                               && u.codigo.Contains(anio)
                               orderby u.id descending
                               select u).ToList();

            return listaRequisitos;
        }

        public static List<VISTA_Auditorias> ListarAuditorias()
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            List<VISTA_Auditorias> listaFormacion = new List<VISTA_Auditorias>();

            listaFormacion = (from u in bd.VISTA_Auditorias
                              select u).ToList();

            return listaFormacion;
        }

        public static List<VISTA_Auditorias> ListarAuditorias(int idCentral)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            List<VISTA_Auditorias> listaAuditorias = new List<VISTA_Auditorias>();
            List<VISTA_Auditorias> listaRegistros = new List<VISTA_Auditorias>();

            listaRegistros = (from u in bd.VISTA_Auditorias
                              where u.idCentral == idCentral
                              select u).ToList();

            foreach (VISTA_Auditorias audit in listaRegistros)
            {
                string cadenareferenciales = string.Empty;
                List<auditorias_referenciales> auditreferenciales = (from v in bd.auditorias_referenciales
                                                                     where v.idAuditoria == audit.id
                                                                     select v
                                                              ).ToList();

                foreach (auditorias_referenciales auditref in auditreferenciales)
                {
                    referenciales refe = (from r in bd.referenciales
                                          where r.id == auditref.idReferencial
                                          select r).FirstOrDefault();

                    if (cadenareferenciales != string.Empty)
                        cadenareferenciales = cadenareferenciales + ", " + refe.nombre;
                    else
                        cadenareferenciales = refe.nombre;
                }
                audit.referenciales = cadenareferenciales;

                listaAuditorias.Add(audit);
            }

            return listaAuditorias;
        }

        public static List<VISTA_Auditorias> ListarAuditorias(int idCentral, string anio)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            List<VISTA_Auditorias> listaFormacion = new List<VISTA_Auditorias>();

            DateTime fechaInicio = DateTime.Parse("01/01/" + anio);
            DateTime fechaFin = DateTime.Parse("31/12/" + anio);

            listaFormacion = (from u in bd.VISTA_Auditorias
                              where u.idCentral == idCentral
                              && u.fechainicio >= fechaInicio && u.fechainicio <= fechaFin
                              select u).ToList();

            return listaFormacion;
        }

        public static VISTA_Auditorias ListarAuditoria(int idAuditoria)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            VISTA_Auditorias listaFormacion = new VISTA_Auditorias();

            listaFormacion = (from u in bd.VISTA_Auditorias
                              where u.id == idAuditoria
                              select u).FirstOrDefault();

            return listaFormacion;
        }

        public static List<VISTA_AuditoriaObservadores> ListarObservadores(int idAuditoria)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            List<VISTA_AuditoriaObservadores> listaAuditores = new List<VISTA_AuditoriaObservadores>();

            listaAuditores = (from u in bd.VISTA_AuditoriaObservadores
                              where u.idAuditoria == idAuditoria
                              select u).ToList();

            return listaAuditores;
        }

        public static List<procesos> ListarProcesosAsociados(int idObjetivo)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<procesos> listaProcesos = new List<procesos>();

            listaProcesos = (from u in bd.procesos
                             join
           op in bd.objetivos_procesos on u.id equals op.idProceso
                             where op.idObjetivo == idObjetivo
                             select u).ToList();

            return listaProcesos;
        }

        public static List<VISTA_AccionMejora_Accion> ListarAccionesAccionesMejora(string anio)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<VISTA_AccionMejora_Accion> listaAcciones = new List<VISTA_AccionMejora_Accion>();

            listaAcciones = (from u in bd.VISTA_AccionMejora_Accion
                             join
    acm in bd.accionesmejora on u.idaccionmejora equals acm.id
                             where acm.codigo.Contains(anio) || acm.estado == 0
                             select u).ToList();

            return listaAcciones;
        }

        public static List<VISTA_AccionMejora_Accion> ListarAccionesAccionesMejora(int idAccionMejora)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<VISTA_AccionMejora_Accion> listaAcciones = new List<VISTA_AccionMejora_Accion>();

            listaAcciones = (from u in bd.VISTA_AccionMejora_Accion
                             where u.idaccionmejora == idAccionMejora
                             select u).ToList();

            return listaAcciones;
        }

        public static List<VISTA_Despliegue> ListarAccionesObjetivo(int idObjetivo)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<VISTA_Despliegue> listaAcciones = new List<VISTA_Despliegue>();

            listaAcciones = (from u in bd.VISTA_Despliegue
                             where u.idObjetivo == idObjetivo
                             select u).ToList();

            return listaAcciones;
        }

        public static usuarios obtenerUsuarioAccionMejora(int idAccionMejora)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            usuarios responsable = new usuarios();

            responsable = (from a in bd.accionesmejora
                           join u in bd.usuarios on a.responsable equals u.idUsuario
                           where a.id == idAccionMejora
                           select u).FirstOrDefault();
            return responsable;

        }

        public static List<VISTA_Despliegue> ListarAccionesObjetivo(List<VISTA_Objetivos> listaObjetivos)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<VISTA_Despliegue> despliegue = new List<VISTA_Despliegue>();

            foreach (VISTA_Objetivos obj in listaObjetivos)
            {
                List<VISTA_Despliegue> listaAcciones = new List<VISTA_Despliegue>();

                listaAcciones = (from u in bd.VISTA_Despliegue
                                 where u.idObjetivo == obj.id
                                 select u).ToList();

                despliegue.AddRange(listaAcciones);
            }

            return despliegue;
        }

        public static List<comunicacion_clasificacion> ListarClasifComunicacion()
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            List<comunicacion_clasificacion> listaTipos = new List<comunicacion_clasificacion>();

            listaTipos = (from u in bd.comunicacion_clasificacion
                          select u).ToList();

            return listaTipos;
        }


        public static List<comunicacion_tipos> ListarTiposComunicacion()
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            List<comunicacion_tipos> listaTipos = new List<comunicacion_tipos>();

            listaTipos = (from u in bd.comunicacion_tipos
                          select u).ToList();

            return listaTipos;
        }


        public static List<comunicacion_canales> ListarCanalesComunicacion()
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            List<comunicacion_canales> listaCanales = new List<comunicacion_canales>();

            listaCanales = (from u in bd.comunicacion_canales
                            select u).ToList();

            return listaCanales;
        }

        public static List<evento_ambiental_tipo> ListarTiposEventoAmbiental()
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            List<evento_ambiental_tipo> listaCanales = new List<evento_ambiental_tipo>();

            listaCanales = (from u in bd.evento_ambiental_tipo
                            select u).ToList();

            return listaCanales;
        }

        public static despliegue ObtenerAccionPorID(int idAccion)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            despliegue accion = new despliegue();

            accion = (from u in bd.despliegue
                      where u.id == idAccion
                      select u).FirstOrDefault();

            return accion;
        }

        public static auditorias_auditores ObtenerAuditorPorID(int idAuditor)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            auditorias_auditores auditor = new auditorias_auditores();

            auditor = (from u in bd.auditorias_auditores
                       where u.id == idAuditor
                       select u).FirstOrDefault();

            return auditor;
        }

        public static List<evidencias> ObtenerEvidenciasAccion(int idAccion)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<evidencias> listaEvidencias = new List<evidencias>();

            listaEvidencias = (from u in bd.evidencias
                               where u.idaccion == idAccion
                               select u).ToList();

            return listaEvidencias;
        }

        public static List<cualificaciones> ObtenerCualificacionAuditor(int idAuditor)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<cualificaciones> listaCualificaciones = new List<cualificaciones>();

            listaCualificaciones = (from u in bd.cualificaciones
                                    where u.idAuditor == idAuditor
                                    select u).ToList();

            return listaCualificaciones;
        }

        public static indicadores ObtenerIndicador(int id)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            indicadores listaIndicadores = new indicadores();

            listaIndicadores = (from u in bd.indicadores
                                where u.Id == id
                                select u).FirstOrDefault();

            return listaIndicadores;
        }

        public static VISTA_IndicadoresPlanificados ObtenerIndicadorPlanificado(int id)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            VISTA_IndicadoresPlanificados listaIndicadores = new VISTA_IndicadoresPlanificados();

            listaIndicadores = (from u in bd.VISTA_IndicadoresPlanificados
                                where u.Id == id
                                select u).FirstOrDefault();

            return listaIndicadores;
        }


        public static indicadores_imputacion ObtenerImputacionIndicador(int idPlanificacion, int anio)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            indicadores_imputacion listaIndicadores = new indicadores_imputacion();

            listaIndicadores = (from u in bd.indicadores_imputacion
                                where u.IdPlanificacionIndicador == idPlanificacion && u.anio == anio
                                select u).FirstOrDefault();

            return listaIndicadores;
        }

        public static aspecto_valoracion ObtenerValoracionAspecto(int id)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            aspecto_valoracion listaValoraciones = new aspecto_valoracion();

            listaValoraciones = (from u in bd.aspecto_valoracion
                                 where u.id == id
                                 select u).FirstOrDefault();

            return listaValoraciones;
        }

        public static aspecto_parametro ObtenerParametro(int id)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            aspecto_parametro listaParametros = new aspecto_parametro();

            listaParametros = (from u in bd.aspecto_parametro
                               where u.id_parametro == id
                               select u).FirstOrDefault();

            return listaParametros;
        }

        public static aspecto_parametros ObtenerParametrosCentral(int idCentral)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            aspecto_parametros listaParametros = new aspecto_parametros();

            listaParametros = (from u in bd.aspecto_parametros
                               where u.idCentral == idCentral
                               select u).FirstOrDefault();

            return listaParametros;
        }

        public static descripcion_centro ObtenerInformacionCentral(int idCentral)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            descripcion_centro listaParametros = new descripcion_centro();

            listaParametros = (from u in bd.descripcion_centro
                               where u.id_centro == idCentral
                               select u).FirstOrDefault();

            return listaParametros;
        }
        public static descripcion_general ObtenerDescripcionGeneral(int idDescripcion)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            descripcion_general listaParametros = new descripcion_general();

            listaParametros = (from u in bd.descripcion_general
                               where u.id == idDescripcion
                               select u).FirstOrDefault();

            return listaParametros;
        }



        public static aspecto_parametros_log ObtenerParametrosLogCentral(int idCentral, int anio)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            aspecto_parametros_log listaParametros = new aspecto_parametros_log();

            listaParametros = (from u in bd.aspecto_parametros_log
                               where u.idCentral == idCentral &&
                               u.anio == anio
                               select u).FirstOrDefault();

            return listaParametros;
        }

        public static aspecto_parametro_valoracion ObtenerParametroAspecto(int idParametro)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            aspecto_parametro_valoracion listaParametros = new aspecto_parametro_valoracion();

            listaParametros = (from u in bd.aspecto_parametro_valoracion
                               where u.id == idParametro
                               select u).FirstOrDefault();

            return listaParametros;
        }


        public static indicadores_hojadedatos ObtenerParametroInd(int id)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            indicadores_hojadedatos Parametro = new indicadores_hojadedatos();

            Parametro = (from u in bd.indicadores_hojadedatos
                         where u.id == id
                         select u).FirstOrDefault();

            return Parametro;
        }

        public static indicadores_hojadedatos_valores ObtenerParametroInd(int idParam, int anio, int idcentral)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            indicadores_hojadedatos_valores Parametro = new indicadores_hojadedatos_valores();

            Parametro = (from u in bd.indicadores_hojadedatos_valores
                         where u.CodIndiHojaDatos == idParam && u.anio == anio && u.idcentral == idcentral
                         select u).FirstOrDefault();

            return Parametro;
        }

        public static List<VISTA_IndicadoresAfectadosParametro> ObtenerIndicadoresAfectados(int idParam, int anio, int idcentral)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<VISTA_IndicadoresAfectadosParametro> listaIndicadores = new List<VISTA_IndicadoresAfectadosParametro>();

            listaIndicadores = (from u in bd.VISTA_IndicadoresAfectadosParametro
                                where (u.Operador1 == idParam || u.Operador2 == idParam || u.Operador3 == idParam) && u.anio == anio && u.idcentral == idcentral
                                select u).ToList();

            return listaIndicadores;
        }

        public static indicadores_imputacion ObtenerValoracionInd(int idIndicador, int anio, int idcentral)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            indicadores_imputacion valoracion = new indicadores_imputacion();

            valoracion = (from u in bd.indicadores_imputacion
                          where u.IdIndicador == idIndicador && u.anio == anio && u.idcentral == idcentral
                          select u).FirstOrDefault();

            return valoracion;
        }

        public static int ObtenerLicenciaActiva(int idusuario)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            cuestionario licenciaactual = new cuestionario();

            licenciaactual = (from u in bd.cuestionario
                              join
    cen in bd.usuario_centros on u.idOrganizacion equals cen.idcentro
                              join
    usu in bd.usuarios on cen.idusuario equals usu.idUsuario
                              where usu.idUsuario == idusuario && u.fechavalidez >= DateTime.Now
                              orderby u.fechavalidez ascending
                              select u).FirstOrDefault();

            return licenciaactual.id;
        }

        public static cuestionario ObtenerLicencia(int idusuario)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            cuestionario licenciaactual = new cuestionario();

            licenciaactual = (from u in bd.cuestionario
                              join
    cen in bd.usuario_centros on u.idOrganizacion equals cen.idcentro
                              join
    usu in bd.usuarios on cen.idusuario equals usu.idUsuario
                              where usu.idUsuario == idusuario && u.fechavalidez >= DateTime.Now
                              orderby u.fechavalidez ascending
                              select u).FirstOrDefault();

            return licenciaactual;
        }

        public static List<cuestionario> ObtenerLicencias(int idusuario)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<cuestionario> licenciaactual = new List<cuestionario>();

            licenciaactual = (from u in bd.cuestionario
                              join
                              cen in bd.usuario_centros on u.idOrganizacion equals cen.idcentro
                              join
                         usu in bd.usuarios on cen.idusuario equals usu.idUsuario
                              where usu.idUsuario == idusuario
                              orderby u.fechavalidez ascending
                              select u).ToList();

            return licenciaactual;
        }

        public static void ValidarOrganizacion(cuestionario licencia)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            cuestionario licenciaactual = (from u in bd.cuestionario
                                           where u.id == licencia.id
                                           select u).FirstOrDefault();

            if (licenciaactual != null)
            {
                licenciaactual.organizacionvalidada = true;
                bd.SaveChanges();
            }


        }

        public static void ValidarCuestionario(cuestionario licencia)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            cuestionario licenciaactual = (from u in bd.cuestionario
                                           where u.id == licencia.id
                                           select u).FirstOrDefault();

            if (licenciaactual != null)
            {
                licenciaactual.cuestionariovalidado = true;
                licenciaactual.anio = licencia.anio;
                bd.SaveChanges();
            }


        }

        public static List<cuestionario> ObtenerCuestionariosExportar(int? idOrganizacion)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<cuestionario> licencias = new List<cuestionario>();

            licencias = (from u in bd.cuestionario
                         join
                    cen in bd.usuario_centros on u.idOrganizacion equals cen.idcentro
                         join
                         usu in bd.usuarios on cen.idusuario equals usu.idUsuario
                         where cen.idcentro == idOrganizacion && u.cuestionariovalidado == true
                         orderby u.fechavalidez ascending
                         select u).ToList();

            return licencias;
        }

        public static List<cuestionario> ObtenerCuestionariosExportarPorOrganizacion(int? idOrganizacion)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            List<cuestionario> licencias = new List<cuestionario>();

            licencias = (from u in bd.cuestionario
                         join
                    cen in bd.usuario_centros on u.idOrganizacion equals cen.idcentro
                         join
                         usu in bd.usuarios on cen.idusuario equals usu.idUsuario
                         where cen.idcentro == idOrganizacion && u.cuestionariovalidado == true
                         orderby u.fechavalidez ascending
                         select u).ToList();

            return licencias;
        }


        public static List<documentacion> ListarFicherosDoc(int idProceso, int tipo)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var registros = from v in bd.documentacion where v.tipo == tipo && v.estado == 1 orderby v.idFichero descending select v;

            List<documentacion> listaArchivosCargados = registros.ToList();

            return listaArchivosCargados;

        }

        public static List<tipodocumento> ListarTipoDocumento(int? nivel, int? tecnologia)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var registros = from v in bd.tipodocumento
                            where (v.nivel == null || v.nivel == nivel) && (v.tecnologia == tecnologia || v.tecnologia == null)
                            orderby v.id descending
                            select v;

            List<tipodocumento> listaTipos = registros.ToList();

            return listaTipos;

        }

        public static tecnologias ObtenerTecnologia(int? id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            tecnologias registros = new tecnologias();

            registros = (from v in bd.tecnologias where v.id == id select v).FirstOrDefault();

            return registros;

        }

        public static List<tipocentral> ObtenerTecnologiaCentral(int? id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            List<tipocentral> registros = new List<tipocentral>();

            if (id == 4 || id == 5)
            {
                registros = (from v in bd.tipocentral orderby v.id ascending select v).ToList();
            }
            else
            {
                registros = (from v in bd.tipocentral where v.id == id orderby v.id ascending select v).ToList();
            }
            return registros;

        }

        public static List<tipocentral> ListarTipos()
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var registros = from v in bd.tipocentral orderby v.id ascending select v;

            List<tipocentral> listaTipos = registros.ToList();

            return listaTipos;

        }
        public static List<tecnologias> ListarTecnologias()
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            var registros = from v in bd.tecnologias orderby v.id ascending select v;
            List<tecnologias> listaTipos = registros.OrderBy(x => x.nombre).ToList();
            return listaTipos;

        }
        public static List<string> ListarTecnologiasDeMatrizCentroPorVersion(int version)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            var registros = (from v in bd.matriz_centro where v.version == version select v.id_tecnologia).Distinct().ToList();

            List<int> lista = registros;

            //convertir la lista de int a string.
            List<string> listaString = lista.ConvertAll<string>(x => x.ToString());

            return listaString;
        }


        public static List<tecnologias> ListarTecnologiasPorCentro(int idCentro)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var registros = from v in bd.tecnologias join vc in bd.tecnologias_centros on v.id equals vc.id_tecnologia where vc.id_centro == idCentro orderby v.id ascending select v;

            List<tecnologias> listaTipos = registros.OrderBy(x => x.nombre).ToList();

            return listaTipos;

        }
        //public static List<tecnologias> ListarTecnologiasPorVersion(int idCentro, int version)
        //{
        //    DIMASSTEntities bd = new DIMASSTEntities();
        //    var registros = from v in bd.tecnologias join vc in bd.matriz_centro on v.id equals vc.id_tecnologia where vc.id_centro == idCentro && vc.version == version orderby v.id ascending select v;
        //    //var registros = from v in bd.tecnologias join vc in bd.tecnologias_centros on v.id equals vc.id_tecnologia where vc.id_centro == idCentro orderby v.id ascending select v;

        //    List<tecnologias> tecnologias = registros.OrderBy(x => x.nombre).Distinct().ToList();

        //    return tecnologias;

        //}       
        public static List<tecnologias> ListarTecnologiasPorVersion(int idCentro, int version)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            //var registros = from v in bd.matriz_centro
            //                where v.id_centro == idCentro && v.version == version
            //                select v;
            //var registros = from v in bd.matriz_centro_critico where v.id_centro == idCentro && v.version == version select v;

            List<tecnologias> registros = (from v in bd.tecnologias join vc in bd.matriz_centro on v.id equals vc.id_tecnologia where vc.id_centro == idCentro && vc.version == version orderby v.id ascending select v).Distinct().ToList();
            //var registros = from v in bd.tecnologias join vc in bd.tecnologias_centros on v.id equals vc.id_tecnologia where vc.id_centro == idCentro orderby v.id ascending select v;

            //List<tecnologias> tecnologias = registros;
            //List<tecnologias> tecnologias1 = registros;
            //matriz.GroupBy(x => x.id_tecnologia).Select(grp => grp.First()).OrderByDescending(y => y.id);
            List<tecnologias> listaTecnologias = registros;
            return registros;

        }
        public static List<tecnologias> ListarTecnologiasid(List<string> tecnologiasSelec)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var registros = from v in bd.tecnologias where tecnologiasSelec.Contains(v.id.ToString()) orderby v.id ascending select v;

            List<tecnologias> listaTipos = registros.OrderBy(x => x.nombre).ToList();

            return listaTipos;

        }

        public static tecnologias ObtenerTecnologiaCentro(int idCentro)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var registro = from t in bd.tecnologias join tc in bd.tecnologias_centros on t.id equals tc.id_tecnologia where tc.id_centro == idCentro select t;

            if (registro.Any())
            {
                return registro.FirstOrDefault();
            }
            else
            {
                tecnologias tec = new tecnologias
                {
                    nombre = "sin tecnologia asignada",
                    rutaImagen = "sin ruta de imagen asignada"
                };
                return tec;
            }
        }

        public static List<documentos_riesgos> ObtenerDocumentosRiesgos(int centro)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            List<documentos_riesgos> registros = (from v in bd.documentos_riesgos where v.id_centro == centro select v).ToList();

            return registros;

        }

        public static List<documento_historico> ObtenerDocumentoHistoricoFinal(int centro)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            List<documento_historico> registros = (from v in bd.documento_historico where v.id_centro == centro && v.descarga == 1 select v).ToList();

            return registros;

        }
        public static documento_historico ObtenerDocumentoHistoricoPor_id_centro_descarga(int id_documento, int id_centro)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var registros = (from v in bd.documento_historico where v.id == id_documento && v.id_centro == id_centro && v.descarga == 1 select v).FirstOrDefault();

            return registros;

        }

        public static List<documento_historico_criticos> ObtenerDocumentoHistoricoFinalCritico(int centro)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            List<documento_historico_criticos> registros = (from v in bd.documento_historico_criticos where v.id_centro == centro && v.descarga == 1 select v).ToList();

            return registros;

        }

        public static List<documento_historico> ObtenerDocumentoHistoricoBorrador(int centro)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            List<documento_historico> registros = (from v in bd.documento_historico where v.id_centro == centro && v.descarga == 0 select v).ToList();

            return registros;

        }

        public static List<documento_historico_criticos> ObtenerDocumentoHistoricoBorradorCritico(int centro)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            List<documento_historico_criticos> registros = (from v in bd.documento_historico_criticos where v.id_centro == centro && v.descarga == 0 select v).ToList();

            return registros;

        }

        public static string ObtenerUltimaversionTextoDocumento(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.documentos_riesgos
                              where u.id == id
                              select u.descripcion).FirstOrDefault();
            if (actualizar != null)
            {
                return actualizar;
            }
            else
            {
                return "";
            }

        }

        public static string ObtenerUltimaversionTextoDocumentoCriticos(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.documentos_riesgos_criticos
                              where u.id == id
                              select u.descripcion).FirstOrDefault();
            if (actualizar != null)
            {
                return actualizar;
            }
            else
            {
                return "";
            }

        }

        public static int ObtenerUltimaversionDocumento(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.documentos_riesgos
                              where u.id_centro == id
                              select u).OrderByDescending(u => u.id).FirstOrDefault();
            if (actualizar != null)
            {
                return actualizar.id;
            }
            else
            {
                return 0;
            }

        }
        public static documentos_riesgos ObtenerDocumentosRiesgosPorId(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.documentos_riesgos
                              where u.id == id
                              select u).FirstOrDefault();
            if (actualizar != null)
            {
                return actualizar;
            }
            else
            {
                return null;
            }
        }

        public static int ObtenerUltimaversionDocumentoCriticos(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.documentos_riesgos_criticos
                              where u.id_centro == id
                              select u).OrderByDescending(u => u.id).FirstOrDefault();
            if (actualizar != null)
            {
                return actualizar.id;
            }
            else
            {
                return 0;
            }

        }

        public static int ActualizarDocumentoRiesgos(documentos_riesgos objeto, bool esborrador)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.documentos_riesgos
                              where u.id_centro == objeto.id_centro && u.esborrador == true
                              select u).OrderByDescending(u => u.id).FirstOrDefault();

            if (objeto.id != 0 && actualizar != null)
            {
                actualizar.descripcion = objeto.descripcion;
                actualizar.id_centro = objeto.id_centro;
                actualizar.fechageneracion = DateTime.Now;
                actualizar.rutaDescarga = objeto.rutaDescarga;
                actualizar.revision = objeto.revision;
                actualizar.esborrador = esborrador;
                bd.SaveChanges();
                return actualizar.id;
            }
            else
            {
                documentos_riesgos insertar = new documentos_riesgos();
                insertar.descripcion = objeto.descripcion;
                insertar.id_centro = objeto.id_centro;
                insertar.fechageneracion = DateTime.Now;
                insertar.rutaDescarga = objeto.rutaDescarga;
                insertar.revision = objeto.revision + 1;
                insertar.esborrador = esborrador;
                bd.documentos_riesgos.Add(insertar);
                bd.SaveChanges();

                return insertar.id;
            }
        }
        //public static int ActualizarDocumentoRiesgos_Revision(int revision, )
        //{
        //    DIMASSTEntities bd = new DIMASSTEntities();

        //    var actualizar = (from u in bd.documentos_riesgos
        //                      where u.id_centro == objeto.id_centro
        //                      select u).OrderByDescending(u => u.id).FirstOrDefault();


        //    if (objeto.id != 0 && actualizar != null)
        //    {
        //        actualizar.descripcion = objeto.descripcion;
        //        actualizar.id_centro = objeto.id_centro;
        //        actualizar.fechageneracion = DateTime.Now;
        //        actualizar.rutaDescarga = objeto.rutaDescarga;
        //        bd.SaveChanges();
        //        return actualizar.id;
        //    }
        //    else
        //    {
        //        documentos_riesgos insertar = new documentos_riesgos();
        //        insertar.descripcion = objeto.descripcion;
        //        insertar.id_centro = objeto.id_centro;
        //        insertar.fechageneracion = DateTime.Now;
        //        insertar.rutaDescarga = objeto.rutaDescarga;
        //        bd.documentos_riesgos.Add(insertar);
        //        bd.SaveChanges();

        //        return insertar.id;
        //    }
        //}

        public static List<matriz_centro> listarMatrizCentroUltimaVersion(int idCentro)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            int? maxVersion = (from v in bd.matriz_centro where v.id_centro == idCentro select v.version).Max();

            List<matriz_centro> listaTipos;
            if (maxVersion != 0 && maxVersion != null)
            {
                var registros = from v in bd.matriz_centro where v.id_centro == idCentro && v.version == maxVersion select v;

                listaTipos = registros.ToList();
            }
            else
            {
                var registros = from v in bd.matriz_centro where v.id_centro == idCentro select v;
                listaTipos = registros.ToList();
            }

            return listaTipos;

        }

        public static List<matriz_centro_critico> listarMatrizCentroUltimaVersionCritico(int idCentro)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            int? maxVersion = (from v in bd.matriz_centro_critico where v.id_centro == idCentro select v.version).Max();

            List<matriz_centro_critico> listaTipos;
            if (maxVersion != 0 && maxVersion != null)
            {
                var registros = from v in bd.matriz_centro_critico where v.id_centro == idCentro && v.version == maxVersion select v;

                listaTipos = registros.ToList();
            }
            else
            {
                var registros = from v in bd.matriz_centro_critico where v.id_centro == idCentro select v;
                listaTipos = registros.ToList();
            }

            return listaTipos;

        }

        public static matriz_centro ObtenerMatrizCentro(int idCentro, int idversion)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            matriz_centro Parametro = new matriz_centro();

            Parametro = (from u in bd.matriz_centro
                         where u.id_centro == idCentro && u.version == idversion
                         select u).FirstOrDefault();

            return Parametro;

        }
        public static List<matriz_centro> ObtenerListaMatrizCentro(int idCentro, int idversion)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            List<matriz_centro> Parametro = new List<matriz_centro>();

            Parametro = (from u in bd.matriz_centro
                         where u.id_centro == idCentro && u.version == idversion
                         select u).ToList();

            return Parametro;

        }

        public static bool InsertarMatrizCritico(List<matriz_centro_critico> conjunto_matrices)
        {
            bool resultado = false;
            try
            {
                DIMASSTEntities bd = new DIMASSTEntities();
                matriz_centro_critico mcc = new matriz_centro_critico();
                foreach (matriz_centro_critico matriz in conjunto_matrices)
                {
                    mcc.id_matriz_centro = matriz.id;
                    mcc.id_tecnologia = matriz.id_tecnologia;
                    mcc.id_centro = matriz.id_centro;
                    mcc.id_riesgoCritico = matriz.id_riesgoCritico;
                    mcc.id_areanivel1 = matriz.id_areanivel1;
                    mcc.id_areanivel2 = matriz.id_areanivel2;
                    mcc.id_areanivel3 = matriz.id_areanivel3;
                    mcc.id_areanivel4 = matriz.id_areanivel4;
                    mcc.activo = matriz.activo;
                    mcc.version = matriz.version;

                    bd.matriz_centro_critico.Add(mcc);

                    bd.SaveChanges();
                }
                resultado = true;
            }
            catch (Exception ex)
            {
                resultado = false;
            }

            return resultado;
        }

        public static List<matriz_centro> listarMatrizCentro(int idCentro)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            var registros = from v in bd.matriz_centro where v.id_centro == idCentro select v;
            List<matriz_centro> listaTipos = registros.ToList();


            return listaTipos;

        }
        public static List<matriz_centro> listarMatrizCentro(int idCentro, int version)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            var registros = from v in bd.matriz_centro where v.id_centro == idCentro && v.version == version select v;
            List<matriz_centro> listaTipos = registros.ToList();


            return listaTipos;

        }
        public static List<matriz_centro_critico> listarMatrizCriticoCentro(int idCentro, int version)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            var registros = from v in bd.matriz_centro_critico where v.id_centro == idCentro && v.version == version select v;
            List<matriz_centro_critico> listaTipos = registros.ToList();


            return listaTipos;

        }
        public static List<matriz_centro_critico> listarMatrizCentroCritico(int idCentro, int version)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            var registros = from v in bd.matriz_centro_critico where v.id_centro == idCentro && v.version == version select v;
            List<matriz_centro_critico> listaTipos = registros.ToList();


            return listaTipos;

        }
        public static List<areanivel2> ObtenerHijosNivel2(int identificador)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            List<areanivel2> registros = (from v in bd.areanivel2 where v.id_areanivel1 == identificador select v).ToList();
            return registros;
        }
        public static async Task<List<areanivel2>> ObtenerHijosNivel2Async(int identificador)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            List<areanivel2> registros = await (from v in bd.areanivel2 where v.id_areanivel1 == identificador select v).ToListAsync();
            return registros;
        }
        public static async Task<List<areanivel3>> ObtenerHijosNivel3Async(int identificador)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            List<areanivel3> registros = await (from v in bd.areanivel3 where v.id_areanivel2 == identificador select v).ToListAsync();
            return registros;

        }
        public static List<areanivel3> ObtenerHijosNivel3(int identificador)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            List<areanivel3> registros = (from v in bd.areanivel3 where v.id_areanivel2 == identificador select v).ToList();
            return registros;

        }
        public static async Task<List<areanivel4>> ObtenerHijosNivel4Async(int identificador)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            List<areanivel4> registros = await (from v in bd.areanivel4 where v.id_areanivel3 == identificador select v).ToListAsync();
            return registros;

        }
        public static List<areanivel4> ObtenerHijosNivel4(int identificador)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            List<areanivel4> registros = (from v in bd.areanivel4 where v.id_areanivel3 == identificador select v).ToList();
            return registros;

        }


        public static void RecalcularMatrizVersion(int idCentro, int version)
        {
            DIMASSTEntities bddd = new DIMASSTEntities();
            List<matriz_centro> registrosnivelcuatro = (from v in bddd.matriz_centro where v.id_centro == idCentro && v.version == version && v.id_areanivel4 != null select v).ToList();
            List<matriz_centro> registrosniveltres = (from v in bddd.matriz_centro where v.id_centro == idCentro && v.version == version && v.id_areanivel3 != null select v).ToList();
            foreach (VISTA_tipos_riesgos riesgo in Datos.ListarTiposRiesgos())
            {
                List<matriz_centro> registrosRiesgo = registrosniveltres.Where(x => x.id_riesgo == riesgo.id).ToList();

                foreach (matriz_centro r3 in registrosRiesgo)
                {
                    List<matriz_centro> registros = registrosRiesgo.Where(x => x.id_areanivel3 == r3.id_areanivel3).ToList();
                    bool algunhijoactivo = false;
                    if (r3.id_areanivel3 != null)
                    {
                        foreach (areanivel4 item in ObtenerHijosNivel4((int)r3.id_areanivel3))
                        {
                            matriz_centro regHijo = registrosnivelcuatro.Where(x => x.id_riesgo == riesgo.id && x.id_centro == idCentro && x.version == version && x.id_areanivel4 == item.id).FirstOrDefault();
                            if (regHijo != null)
                            {
                                if (regHijo.activo)
                                {
                                    algunhijoactivo = true;
                                    break;
                                }
                            }
                        }
                        if (ObtenerHijosNivel4((int)r3.id_areanivel3).Count > 0)
                        {
                            matriz_centro actualizar = registros.FirstOrDefault();
                            if (actualizar != null)
                            {
                                if (algunhijoactivo)
                                {
                                    actualizar.activo = true;
                                    break;
                                }
                                else
                                {
                                    actualizar.activo = false;
                                }
                            }
                        }
                    }

                }
            }

            bddd.SaveChanges();

            DIMASSTEntities bd = new DIMASSTEntities();
            registrosnivelcuatro = (from v in bd.matriz_centro where v.id_centro == idCentro && v.version == version && v.id_areanivel4 != null select v).ToList();
            registrosniveltres = (from v in bd.matriz_centro where v.id_centro == idCentro && v.version == version && v.id_areanivel3 != null select v).ToList();
            List<matriz_centro> registrosniveldos = (from v in bd.matriz_centro where v.id_centro == idCentro && v.version == version && v.id_areanivel2 != null select v).ToList();
            List<matriz_centro> registrosniveluno = (from v in bd.matriz_centro where v.id_centro == idCentro && v.version == version && v.id_areanivel1 != null select v).ToList();

            //Por cada riesgo
            foreach (VISTA_tipos_riesgos riesgo in Datos.ListarTiposRiesgos())
            {
                //Obtengo todos los revistros
                //List< matriz_centro> registrosRiesgo = (from v in bd.matriz_centro where v.id_centro == idCentro && v.version == version && v.id_riesgo == riesgo.id select v).ToList();
                List<matriz_centro> registrosRiesgo = registrosniveldos.Where(x => x.id_riesgo == riesgo.id).ToList();
                foreach (matriz_centro r2 in registrosRiesgo)
                {
                    //filtro los registros de areanivel 2 del bucle
                    List<matriz_centro> registros = registrosRiesgo.Where(x => x.id_areanivel2 == r2.id_areanivel2).ToList();
                    //se activa este bool cuando encuentre algun hijo
                    bool algunhijoactivo = false;
                    if (r2.id_areanivel2 != null)
                    {
                        //obtengo sus hijos
                        foreach (areanivel3 item in ObtenerHijosNivel3((int)r2.id_areanivel2))
                        {
                            //por cada hijo, busco su correspondencia en la tabla matriz, y  compruebo si está activo.
                            matriz_centro regHijo = registrosniveltres.Where(x => x.id_riesgo == riesgo.id && x.id_centro == idCentro && x.version == version && x.id_areanivel3 == item.id).FirstOrDefault();
                            if (regHijo != null)
                            {
                                if (regHijo.activo)
                                {
                                    algunhijoactivo = true;
                                    break;
                                }
                            }
                        }
                        if (ObtenerHijosNivel3((int)r2.id_areanivel2).Count > 0)
                        {
                            matriz_centro actualizar = registros.FirstOrDefault();
                            if (actualizar != null)
                            {
                                if (algunhijoactivo)
                                {
                                    actualizar.activo = true;
                                    break;
                                }
                                else
                                {
                                    actualizar.activo = false;
                                }
                            }
                        }
                    }

                }
            }
            bd.SaveChanges();
            DIMASSTEntities bdd = new DIMASSTEntities();
            //registrosniveltres = (from v in bdd.matriz_centro where v.id_centro == idCentro && v.version == version && v.id_areanivel3 != null select v).ToList();
            registrosniveldos = (from v in bdd.matriz_centro where v.id_centro == idCentro && v.version == version && v.id_areanivel2 != null select v).ToList();
            registrosniveluno = (from v in bdd.matriz_centro where v.id_centro == idCentro && v.version == version && v.id_areanivel1 != null select v).ToList();
            foreach (VISTA_tipos_riesgos riesgo in Datos.ListarTiposRiesgos())
            {
                //var registrosRiesgo = (from v in bdd.matriz_centro where v.id_centro == idCentro && v.version == version  && v.id_riesgo == riesgo.id select v);
                List<matriz_centro> registrosRiesgo = registrosniveluno.Where(x => x.id_riesgo == riesgo.id).ToList();
                foreach (matriz_centro r1 in registrosRiesgo)
                {
                    List<matriz_centro> registros = registrosRiesgo.Where(x => x.id_areanivel1 == r1.id_areanivel1).ToList();
                    bool algunhijoactivon1 = false;
                    if (r1.id_areanivel1 != null)
                    {
                        foreach (areanivel2 item in ObtenerHijosNivel2((int)r1.id_areanivel1))
                        {
                            matriz_centro regHijo = registrosniveldos.Where(x => x.id_riesgo == riesgo.id && x.id_centro == idCentro && x.version == version && x.id_areanivel2 == item.id).FirstOrDefault();
                            if (regHijo != null)
                            {
                                if (regHijo.activo)
                                {
                                    algunhijoactivon1 = true;
                                    break;
                                }

                            }

                        }
                        if (ObtenerHijosNivel2((int)r1.id_areanivel1).Count > 0)
                        {
                            // var registros = (from v in bdd.matriz_centro where v.id_centro == idCentro && v.version == version && v.id_areanivel1 == r1.id_areanivel1 && v.id_riesgo == riesgo.id select v);
                            if (registros != null)
                            {
                                matriz_centro actualizar = registros.FirstOrDefault();
                                if (algunhijoactivon1)
                                {
                                    actualizar.activo = true;
                                    break;
                                    //   bdd.SaveChanges();
                                }
                                else
                                {
                                    actualizar.activo = false;
                                    //   bdd.SaveChanges();
                                }
                            }
                        }

                    }

                }

            }
            bdd.SaveChanges();
        }



        public static async Task RecalcularMatrizVersionFilaAsync(int idCentro, int version, int idArea)
        {
            DIMASSTEntities bddd = new DIMASSTEntities();
            List<matriz_centro> registrosnivelcuatro = await (from v in bddd.matriz_centro where v.id_centro == idCentro && v.version == version && v.id_areanivel4 != null select v).ToListAsync();
            List<matriz_centro> registrosniveltres = await (from v in bddd.matriz_centro where v.id_centro == idCentro && v.version == version && v.id_areanivel3 != null select v).ToListAsync();

            var tiposRiesgos = await Datos.ListarTiposRiesgosAsync();
            foreach (VISTA_tipos_riesgos riesgo in tiposRiesgos)
            {
                var obtenerHijosNivel2 = await ObtenerHijosNivel2Async(idArea);
                List<matriz_centro> registrosRiesgo = registrosniveltres.Where(x => x.id_riesgo == riesgo.id).ToList();
                foreach (areanivel2 itemnivel2 in obtenerHijosNivel2)
                {
                    var obtenerHijosNivel3 = await ObtenerHijosNivel3Async(itemnivel2.id);
                    foreach (areanivel3 itemnivel3 in obtenerHijosNivel3)
                    {

                        List<matriz_centro> registros = registrosRiesgo.Where(x => x.id_areanivel3 == itemnivel3.id).ToList();
                        bool algunhijoactivo = false;

                        var obtenerHijosNivel4 = await ObtenerHijosNivel4Async(itemnivel3.id);
                        foreach (areanivel4 item in obtenerHijosNivel4)
                        {
                            matriz_centro regHijo = registrosnivelcuatro.Where(x => x.id_riesgo == riesgo.id && x.id_centro == idCentro && x.version == version && x.id_areanivel4 == item.id).FirstOrDefault();
                            if (regHijo != null)
                            {
                                if (regHijo.activo)
                                {
                                    algunhijoactivo = true;
                                    break;
                                }
                            }
                        }
                        if (obtenerHijosNivel4.Count > 0)
                        {
                            matriz_centro actualizar = registros.FirstOrDefault();
                            if (actualizar != null)
                            {
                                if (algunhijoactivo)
                                {
                                    actualizar.activo = true;
                                    break;
                                }
                                else
                                {
                                    actualizar.activo = false;
                                }
                            }
                        }
                    }
                }
            }
            bddd.SaveChanges();

            DIMASSTEntities bd = new DIMASSTEntities();
            registrosniveltres = await (from v in bd.matriz_centro where v.id_centro == idCentro && v.version == version && v.id_areanivel3 != null select v).ToListAsync();
            List<matriz_centro> registrosniveldos = await (from v in bd.matriz_centro where v.id_centro == idCentro && v.version == version && v.id_areanivel2 != null select v).ToListAsync();

            foreach (VISTA_tipos_riesgos riesgo in tiposRiesgos)
            {
                List<matriz_centro> registrosRiesgo = registrosniveldos.Where(x => x.id_riesgo == riesgo.id).ToList();

                var obtenerHijosNivel2 = await ObtenerHijosNivel2Async(idArea);
                foreach (areanivel2 itemnivel2 in obtenerHijosNivel2)
                {
                    List<matriz_centro> registros = registrosRiesgo.Where(x => x.id_areanivel2 == itemnivel2.id).ToList();
                    bool algunhijoactivo = false;

                    var obtenerHijosNivel3 = await ObtenerHijosNivel3Async(itemnivel2.id);
                    foreach (areanivel3 item in obtenerHijosNivel3)
                    {
                        matriz_centro regHijo = registrosniveltres.Where(x => x.id_riesgo == riesgo.id && x.id_centro == idCentro && x.version == version && x.id_areanivel3 == item.id).FirstOrDefault();
                        if (regHijo != null)
                        {
                            if (regHijo.activo)
                            {
                                algunhijoactivo = true;
                                break;
                            }
                        }
                    }
                    if (obtenerHijosNivel3.Count > 0)
                    {
                        matriz_centro actualizar = registros.FirstOrDefault();
                        if (actualizar != null)
                        {
                            if (algunhijoactivo)
                            {
                                actualizar.activo = true;
                                break;
                            }
                            else
                            {
                                actualizar.activo = false;
                            }
                        }
                    }
                }
            }
            bd.SaveChanges();
            DIMASSTEntities bdd = new DIMASSTEntities();
            registrosniveldos = await (from v in bdd.matriz_centro where v.id_centro == idCentro && v.version == version && v.id_areanivel2 != null select v).ToListAsync();
            List<matriz_centro> registrosniveluno = await (from v in bdd.matriz_centro where v.id_centro == idCentro && v.version == version && v.id_areanivel1 == idArea select v).ToListAsync();
            foreach (VISTA_tipos_riesgos riesgo in tiposRiesgos)
            {
                //var registrosRiesgo = (from v in bdd.matriz_centro where v.id_centro == idCentro && v.version == version  && v.id_riesgo == riesgo.id select v);
                List<matriz_centro> registrosRiesgo = registrosniveluno.Where(x => x.id_riesgo == riesgo.id).ToList();


                List<matriz_centro> registros = registrosRiesgo.Where(x => x.id_areanivel1 == idArea).ToList();
                bool algunhijoactivon1 = false;

                var obtenerHijosNivel2 = await ObtenerHijosNivel2Async(idArea);
                foreach (areanivel2 item in obtenerHijosNivel2)
                {
                    matriz_centro regHijo = registrosniveldos.Where(x => x.id_riesgo == riesgo.id && x.id_centro == idCentro && x.version == version && x.id_areanivel2 == item.id).FirstOrDefault();
                    if (regHijo != null)
                    {
                        if (regHijo.activo)
                        {
                            algunhijoactivon1 = true;
                            break;
                        }
                    }
                }
                if (obtenerHijosNivel2.Count > 0)
                {
                    if (registros != null)
                    {
                        matriz_centro actualizar = registros.FirstOrDefault();
                        if (algunhijoactivon1)
                        {
                            actualizar.activo = true;
                        }
                        else
                        {
                            actualizar.activo = false;
                        }
                    }
                }
            }
            bdd.SaveChanges();
        }
        //public static void RecalcularMatrizVersionFila(int idCentro, int version, int idArea)
        //{
        //    DIMASSTEntities bddd = new DIMASSTEntities();
        //    List<matriz_centro> registrosnivelcuatro = (from v in bddd.matriz_centro where v.id_centro == idCentro && v.version == version && v.id_areanivel4 != null select v).ToList();
        //    List<matriz_centro> registrosniveltres = (from v in bddd.matriz_centro where v.id_centro == idCentro && v.version == version && v.id_areanivel3 != null select v).ToList();

        //    foreach (VISTA_tipos_riesgos riesgo in Datos.ListarTiposRiesgos())
        //    {
        //        List<matriz_centro> registrosRiesgo = registrosniveltres.Where(x => x.id_riesgo == riesgo.id).ToList();

        //        var hN2 = ObtenerHijosNivel2(idArea);
        //        foreach (areanivel2 itemnivel2 in hN2)
        //        {
        //            var hN3 = ObtenerHijosNivel3(itemnivel2.id);
        //            foreach (areanivel3 itemnivel3 in hN3)
        //            {
        //                List<matriz_centro> registros = registrosRiesgo.Where(x => x.id_areanivel3 == itemnivel3.id).ToList();
        //                bool algunhijoactivo = false;

        //                var hN4 = ObtenerHijosNivel4(itemnivel3.id);
        //                foreach (areanivel4 item in hN4)
        //                {
        //                    matriz_centro regHijo = registrosnivelcuatro.Where(x => x.id_riesgo == riesgo.id && x.id_centro == idCentro && x.version == version && x.id_areanivel4 == item.id).FirstOrDefault();
        //                    if (regHijo != null)
        //                    {
        //                        if (regHijo.activo)
        //                        {
        //                            algunhijoactivo = true;
        //                            break;
        //                        }
        //                    }
        //                }

        //                if (hN4.Count > 0)
        //                {
        //                    matriz_centro actualizar = registros.FirstOrDefault();
        //                    if (actualizar != null)
        //                    {
        //                        if (algunhijoactivo)
        //                        {
        //                            actualizar.activo = true;
        //                            break;
        //                        }
        //                        else
        //                        {
        //                            actualizar.activo = false;
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    bddd.SaveChanges();

        //    DIMASSTEntities bd = new DIMASSTEntities();
        //    registrosniveltres = (from v in bd.matriz_centro where v.id_centro == idCentro && v.version == version && v.id_areanivel3 != null select v).ToList();
        //    List<matriz_centro> registrosniveldos = (from v in bd.matriz_centro where v.id_centro == idCentro && v.version == version && v.id_areanivel2 != null select v).ToList();

        //    foreach (VISTA_tipos_riesgos riesgo in Datos.ListarTiposRiesgos())
        //    {
        //        List<matriz_centro> registrosRiesgo = registrosniveldos.Where(x => x.id_riesgo == riesgo.id).ToList();
        //        var hN2 = ObtenerHijosNivel2(idArea);
        //        foreach (areanivel2 itemnivel2 in hN2)
        //        {

        //            List<matriz_centro> registros = registrosRiesgo.Where(x => x.id_areanivel2 == itemnivel2.id).ToList();
        //            bool algunhijoactivo = false;

        //            var hN3 = ObtenerHijosNivel3(itemnivel2.id);
        //            foreach (areanivel3 itemnivel3 in hN3)
        //            {
        //                matriz_centro regHijo = registrosniveltres.Where(x => x.id_riesgo == riesgo.id && x.id_centro == idCentro && x.version == version && x.id_areanivel3 == itemnivel3.id).FirstOrDefault();
        //                if (regHijo != null)
        //                {
        //                    if (regHijo.activo)
        //                    {
        //                        algunhijoactivo = true;
        //                        break;
        //                    }
        //                }
        //            }
        //            if (hN3.Count > 0)
        //            {
        //                matriz_centro actualizar = registros.FirstOrDefault();
        //                if (actualizar != null)
        //                {
        //                    if (algunhijoactivo)
        //                    {
        //                        actualizar.activo = true;
        //                        break;
        //                    }
        //                    else
        //                    {
        //                        actualizar.activo = false;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    bd.SaveChanges();
        //    DIMASSTEntities bdd = new DIMASSTEntities();
        //    registrosniveldos = (from v in bdd.matriz_centro
        //                         where v.id_centro == idCentro
        //                         && v.version == version
        //                         && v.id_areanivel2 != null
        //                         select v).ToList();
        //    List<matriz_centro> registrosniveluno = (from v in bdd.matriz_centro
        //                                             where v.id_centro == idCentro
        //                                             && v.version == version
        //                                             && v.id_areanivel1 == idArea
        //                                             select v).ToList();
        //    foreach (VISTA_tipos_riesgos riesgo in Datos.ListarTiposRiesgos())
        //    {
        //        //var registrosRiesgo = (from v in bdd.matriz_centro where v.id_centro == idCentro && v.version == version  && v.id_riesgo == riesgo.id select v);
        //        List<matriz_centro> registrosRiesgo = registrosniveluno.Where(x => x.id_riesgo == riesgo.id).ToList();

        //        List<matriz_centro> registros = registrosRiesgo.Where(x => x.id_areanivel1 == idArea).ToList();
        //        bool algunhijoactivon1 = false;

        //        var hN2 = ObtenerHijosNivel2(idArea);
        //        foreach (areanivel2 item in hN2)
        //        {
        //            matriz_centro regHijo = registrosniveldos.Where(x => x.id_riesgo == riesgo.id
        //            && x.id_centro == idCentro
        //            && x.version == version
        //            && x.id_areanivel2 == item.id).FirstOrDefault();
        //            if (regHijo != null)
        //            {
        //                if (regHijo.activo)
        //                {
        //                    algunhijoactivon1 = true;
        //                    break;
        //                }
        //            }
        //        }
        //        if (hN2.Count > 0)
        //        {
        //            if (registros != null)
        //            {
        //                matriz_centro actualizar = registros.FirstOrDefault();
        //                if (algunhijoactivon1)
        //                {
        //                    actualizar.activo = true;
        //                }
        //                else
        //                {
        //                    actualizar.activo = false;
        //                }
        //            }
        //        }
        //    }
        //    bdd.SaveChanges();
        //}


        public static void RecalcularMatrizVersionFila(int idCentro, int version, int idArea)
        {
            DIMASSTEntities bddd = new DIMASSTEntities();
            List<matriz_centro> registrosnivelcuatro = (from v in bddd.matriz_centro where v.id_centro == idCentro && v.version == version && v.id_areanivel4 != null select v).ToList();
            List<matriz_centro> registrosniveltres = (from v in bddd.matriz_centro where v.id_centro == idCentro && v.version == version && v.id_areanivel3 != null select v).ToList();

            foreach (VISTA_tipos_riesgos riesgo in Datos.ListarTiposRiesgos())
            {
                List<matriz_centro> registrosRiesgo = registrosniveltres.Where(x => x.id_riesgo == riesgo.id).ToList();

                var hN2 = ObtenerHijosNivel2(idArea);
                foreach (areanivel2 itemnivel2 in hN2)
                {
                    var hN3 = ObtenerHijosNivel3(itemnivel2.id);
                    foreach (areanivel3 itemnivel3 in hN3)
                    {
                        List<matriz_centro> registros = registrosRiesgo.Where(x => x.id_areanivel3 == itemnivel3.id).ToList();
                        bool algunhijoactivo = false;

                        var hN4 = ObtenerHijosNivel4(itemnivel3.id);
                        foreach (areanivel4 item in hN4)
                        {
                            matriz_centro regHijo = registrosnivelcuatro.Where(x => x.id_riesgo == riesgo.id && x.id_centro == idCentro && x.version == version && x.id_areanivel4 == item.id).FirstOrDefault();
                            if (regHijo != null)
                            {
                                if (regHijo.activo)
                                {
                                    algunhijoactivo = true;
                                    break;
                                }
                            }
                        }

                        if (hN4.Count > 0)
                        {
                            matriz_centro actualizar = registros.FirstOrDefault();
                            if (actualizar != null)
                            {
                                if (algunhijoactivo)
                                {
                                    actualizar.activo = true;
                                    break;
                                }
                                else
                                {
                                    actualizar.activo = false;
                                }
                            }
                        }
                    }
                }
            }
            bddd.SaveChanges();

            DIMASSTEntities bd = new DIMASSTEntities();
            registrosniveltres = (from v in bd.matriz_centro where v.id_centro == idCentro && v.version == version && v.id_areanivel3 != null select v).ToList();
            List<matriz_centro> registrosniveldos = (from v in bd.matriz_centro where v.id_centro == idCentro && v.version == version && v.id_areanivel2 != null select v).ToList();

            foreach (VISTA_tipos_riesgos riesgo in Datos.ListarTiposRiesgos())
            {
                List<matriz_centro> registrosRiesgo = registrosniveldos.Where(x => x.id_riesgo == riesgo.id).ToList();
                var hN2 = ObtenerHijosNivel2(idArea);
                foreach (areanivel2 itemnivel2 in hN2)
                {

                    List<matriz_centro> registros = registrosRiesgo.Where(x => x.id_areanivel2 == itemnivel2.id).ToList();
                    bool algunhijoactivo = false;

                    var hN3 = ObtenerHijosNivel3(itemnivel2.id);
                    foreach (areanivel3 itemnivel3 in hN3)
                    {
                        matriz_centro regHijo = registrosniveltres.Where(x => x.id_riesgo == riesgo.id && x.id_centro == idCentro && x.version == version && x.id_areanivel3 == itemnivel3.id).FirstOrDefault();
                        if (regHijo != null)
                        {
                            if (regHijo.activo)
                            {
                                algunhijoactivo = true;
                                break;
                            }
                        }
                    }
                    if (hN3.Count > 0)
                    {
                        matriz_centro actualizar = registros.FirstOrDefault();
                        if (actualizar != null)
                        {
                            if (algunhijoactivo)
                            {
                                actualizar.activo = true;
                                //break;
                            }
                            else
                            {
                                actualizar.activo = false;
                            }
                        }
                    }
                }
            }
            bd.SaveChanges();
            DIMASSTEntities bdd = new DIMASSTEntities();
            registrosniveldos = (from v in bdd.matriz_centro
                                 where v.id_centro == idCentro
                                 && v.version == version
                                 && v.id_areanivel2 != null
                                 select v).ToList();
            List<matriz_centro> registrosniveluno = (from v in bdd.matriz_centro
                                                     where v.id_centro == idCentro
                                                     && v.version == version
                                                     && v.id_areanivel1 == idArea
                                                     select v).ToList();
            foreach (VISTA_tipos_riesgos riesgo in Datos.ListarTiposRiesgos())
            {
                //var registrosRiesgo = (from v in bdd.matriz_centro where v.id_centro == idCentro && v.version == version  && v.id_riesgo == riesgo.id select v);
                List<matriz_centro> registrosRiesgo = registrosniveluno.Where(x => x.id_riesgo == riesgo.id).ToList();

                List<matriz_centro> registros = registrosRiesgo.Where(x => x.id_areanivel1 == idArea).ToList();
                bool algunhijoactivon1 = false;

                var hN2 = ObtenerHijosNivel2(idArea);
                foreach (areanivel2 item in hN2)
                {
                    matriz_centro regHijo = registrosniveldos.Where(x => x.id_riesgo == riesgo.id
                    && x.id_centro == idCentro
                    && x.version == version
                    && x.id_areanivel2 == item.id).FirstOrDefault();
                    if (regHijo != null)
                    {
                        if (regHijo.activo)
                        {
                            algunhijoactivon1 = true;
                            break;
                        }
                    }
                }
                if (hN2.Count > 0)
                {
                    if (registros != null)
                    {
                        matriz_centro actualizar = registros.FirstOrDefault();
                        if (algunhijoactivon1)
                        {
                            actualizar.activo = true;
                        }
                        else
                        {
                            actualizar.activo = false;
                        }
                    }
                }
            }
            bdd.SaveChanges();
        }

        public static void RecalcularMatrizVersionFilaCritico(int idCentro, int version, int idArea)
        {
            DIMASSTEntities bddd = new DIMASSTEntities();
            List<matriz_centro_critico> registrosnivelcuatro = (from v in bddd.matriz_centro_critico where v.id_centro == idCentro && v.version == version && v.id_areanivel4 != null select v).ToList();
            List<matriz_centro_critico> registrosniveltres = (from v in bddd.matriz_centro_critico where v.id_centro == idCentro && v.version == version && v.id_areanivel3 != null select v).ToList();

            foreach (VISTA_tipo_riesgos_critico riesgo in Datos.ListarTiposRiesgosCriticosVista())
            {
                List<matriz_centro_critico> registrosRiesgo = registrosniveltres.Where(x => x.id_riesgoCritico == riesgo.id).ToList();
                foreach (areanivel2 itemnivel2 in ObtenerHijosNivel2(idArea))
                {
                    foreach (areanivel3 itemnivel3 in ObtenerHijosNivel3(itemnivel2.id))
                    {
                        List<matriz_centro_critico> registros = registrosRiesgo.Where(x => x.id_areanivel3 == itemnivel3.id).ToList();
                        bool algunhijoactivo = false;

                        foreach (areanivel4 item in ObtenerHijosNivel4(itemnivel3.id))
                        {
                            matriz_centro_critico regHijo = registrosnivelcuatro.Where(x => x.id_riesgoCritico == riesgo.id && x.id_centro == idCentro && x.version == version && x.id_areanivel4 == item.id).FirstOrDefault();
                            if (regHijo != null)
                            {
                                if (regHijo.activo)
                                {
                                    algunhijoactivo = true;
                                    break;
                                }
                            }
                        }
                        if (ObtenerHijosNivel4(itemnivel3.id).Count > 0)
                        {
                            matriz_centro_critico actualizar = registros.FirstOrDefault();
                            if (actualizar != null)
                            {
                                if (algunhijoactivo)
                                {
                                    actualizar.activo = true;
                                    break;
                                }
                                else
                                {
                                    actualizar.activo = false;
                                }
                            }
                        }
                    }
                }
            }
            bddd.SaveChanges();

            DIMASSTEntities bd = new DIMASSTEntities();
            registrosniveltres = (from v in bd.matriz_centro_critico where v.id_centro == idCentro && v.version == version && v.id_areanivel3 != null select v).ToList();
            List<matriz_centro_critico> registrosniveldos = (from v in bd.matriz_centro_critico where v.id_centro == idCentro && v.version == version && v.id_areanivel2 != null select v).ToList();

            foreach (VISTA_tipo_riesgos_critico riesgo in Datos.ListarTiposRiesgosCriticosVista())
            {
                List<matriz_centro_critico> registrosRiesgo = registrosniveldos.Where(x => x.id_riesgoCritico == riesgo.id).ToList();
                var hijosNivel2 = ObtenerHijosNivel2(idArea);
                foreach (areanivel2 itemnivel2 in hijosNivel2)
                {
                    List<matriz_centro_critico> registros = registrosRiesgo.Where(x => x.id_areanivel2 == itemnivel2.id).ToList();
                    bool algunhijoactivo = false;

                    var hijosNivel3 = ObtenerHijosNivel3(itemnivel2.id);
                    foreach (areanivel3 itemnivel3 in hijosNivel3)
                    {
                        matriz_centro_critico regHijo = registrosniveltres.Where(x => x.id_riesgoCritico == riesgo.id && x.id_centro == idCentro && x.version == version && x.id_areanivel3 == itemnivel3.id).FirstOrDefault();
                        if (regHijo != null)
                        {
                            if (regHijo.activo)
                            {
                                algunhijoactivo = true;
                                break;
                            }
                        }
                    }
                    if (hijosNivel3.Count > 0)
                    {
                        matriz_centro_critico actualizar = registros.FirstOrDefault();
                        if (actualizar != null)
                        {
                            if (algunhijoactivo)
                            {
                                actualizar.activo = true;
                                break;
                            }
                            else
                            {
                                actualizar.activo = false;
                            }
                        }
                    }
                }
            }
            bd.SaveChanges();
            DIMASSTEntities bdd = new DIMASSTEntities();
            registrosniveldos = (from v in bdd.matriz_centro_critico
                                 where v.id_centro == idCentro
                                 && v.version == version
                                 && v.id_areanivel2 != null
                                 select v).ToList();
            List<matriz_centro_critico> registrosniveluno = (from v in bdd.matriz_centro_critico
                                                             where v.id_centro == idCentro
                                                             && v.version == version
                                                             && v.id_areanivel1 == idArea
                                                             select v).ToList();
            foreach (VISTA_tipo_riesgos_critico riesgo in Datos.ListarTiposRiesgosCriticosVista())
            {
                //var registrosRiesgo = (from v in bdd.matriz_centro_critico where v.id_centro == idCentro && v.version == version  && v.id_riesgoCritico == riesgo.id select v);
                List<matriz_centro_critico> registrosRiesgo = registrosniveluno.Where(x => x.id_riesgoCritico == riesgo.id).ToList();

                List<matriz_centro_critico> registros = registrosRiesgo.Where(x => x.id_areanivel1 == idArea).ToList();
                bool algunhijoactivon1 = false;

                var hijosNivel2 = ObtenerHijosNivel2(idArea);
                foreach (areanivel2 item in hijosNivel2)
                {
                    matriz_centro_critico regHijo = registrosniveldos.Where(x => x.id_riesgoCritico == riesgo.id
                    && x.id_centro == idCentro
                    && x.version == version
                    && x.id_areanivel2 == item.id).FirstOrDefault();
                    if (regHijo != null)
                    {
                        if (regHijo.activo)
                        {
                            algunhijoactivon1 = true;
                            break;
                        }
                    }
                }
                if (hijosNivel2.Count > 0)
                {
                    if (registros != null)
                    {
                        matriz_centro_critico actualizar = registros.FirstOrDefault();
                        if (algunhijoactivon1)
                        {
                            actualizar.activo = true;
                        }
                        else
                        {
                            actualizar.activo = false;
                        }
                    }
                }
            }
            bdd.SaveChanges();
        }

        public static List<matriz_centro> matrizMaestrotoMatrizCentro(int tecnologia)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            var registros = from v in bd.matriz_inicial where v.id_tecnologia == tecnologia select v;
            List<matriz_inicial> listaMatrizInicial = registros.ToList();
            List<matriz_centro> listaMatrizCentro = new List<matriz_centro>();

            foreach (matriz_inicial matriz in listaMatrizInicial)
            {
                matriz_centro objetoCentro = new matriz_centro();
                objetoCentro.id_areanivel1 = matriz.id_areanivel1;
                objetoCentro.id_areanivel2 = matriz.id_areanivel2;
                objetoCentro.id_areanivel3 = matriz.id_areanivel3;
                objetoCentro.id_areanivel4 = matriz.id_areanivel4;
                objetoCentro.activo = matriz.activo;
                objetoCentro.id_riesgo = matriz.id_riesgo;
                objetoCentro.id_tecnologia = matriz.id_tecnologia;
                listaMatrizCentro.Add(objetoCentro);

            }
            return listaMatrizCentro;

        }


        public static List<descripcion_centro> listarDescripcionCentro(int idCentro)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            var registros = from v in bd.descripcion_centro where v.id_centro == idCentro select v;
            List<descripcion_centro> listaTipos = registros.ToList();
            return listaTipos;

        }
        public static List<version_matriz> listarMatrizVersion(int idCentro)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            var registros = from v in bd.version_matriz where v.id_centro == idCentro select v;
            List<version_matriz> listaTipos = registros.ToList();
            return listaTipos;

        }


        public static List<version_matriz> finalizarMatrizVersion(int idVersion, string usuario)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            List<version_matriz> registros = (from v in bd.version_matriz where v.id == idVersion select v).ToList();
            List<version_matriz> listaTipos = registros.ToList();
            foreach (version_matriz item in listaTipos)
            {
                item.estado = 0;
                item.usuario = usuario;
                item.fechaModificacion = DateTime.Now;

                bd.SaveChanges();
            }

            return registros;
        }

        public static List<version_matriz> InsertarMatrizVersion(int idVersion, string usuario)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            List<version_matriz> registros = (from v in bd.version_matriz where v.id == idVersion select v).ToList();
            //var aux = (from v in bd.matriz_centro where v.id_centro == idVersion select v).ToList()

            List<version_matriz> listaTipos = registros.ToList();
            foreach (version_matriz item in listaTipos)
            {
                var estado = item.estado;

                if (estado == 1)
                {
                    item.estado = 0;
                    item.usuario = usuario;
                    item.fechaModificacion = DateTime.Now;

                    bd.SaveChanges();
                }
                else
                {
                    item.estado = 0;
                    item.usuario = usuario;
                    item.fechaModificacion = DateTime.Now;
                    item.version += 1;

                    bd.version_matriz.Add(item);
                    bd.SaveChanges();
                }
            }

            return registros;
        }




        public static List<matriz_centro> listarMatrizCentroAgrupados(int idCentro)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            var registros = from v in bd.matriz_centro where v.id_centro == idCentro select v;
            List<matriz_centro> listaTipos = registros.ToList();
            return listaTipos;

        }
        public static List<version_matriz> eliminarMatrizVersion(int idVersion)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            //solo la utilizamos para devolver los registros eliminados
            List<version_matriz> registros = (from v in bd.version_matriz where v.id == idVersion select v).ToList();

            var versionM = bd.version_matriz.Where(vm => vm.id == idVersion);
            bd.version_matriz.RemoveRange(versionM);

            bd.SaveChanges();

            DIMASSTEntities bdd = new DIMASSTEntities();
            List<matriz_centro> registrosMatriz = (from v in bdd.matriz_centro where v.version == idVersion select v).ToList();

            var MatrizC = bd.matriz_centro.Where(mc => mc.version == idVersion);
            bd.matriz_centro.RemoveRange(MatrizC);

            bd.SaveChanges();


            return registros;
        }
        public static bool eliminarDescripcionCentro(int idCentro)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            StringBuilder errorMessages = new StringBuilder();
            try
            {
                List<descripcion_centro> registros = (from v in bd.descripcion_centro where v.id_centro == idCentro select v).ToList();
                List<descripcion_centro> listaTipos = registros.ToList();
                if (registros != null)
                {
                    foreach (descripcion_centro item in listaTipos)
                    {
                        bd.descripcion_centro.Remove(item);
                        bd.SaveChanges();
                    }
                    return true;
                }
                else { return false; }
            }
            catch (SqlException ex)
            {
                for (int i = 0; i < ex.Errors.Count; i++)
                {
                    errorMessages.Append("Index #" + i + "\n" +
                        "Message: " + ex.Errors[i].Message + "\n" +
                        "LineNumber: " + ex.Errors[i].LineNumber + "\n" +
                        "Source: " + ex.Errors[i].Source + "\n" +
                        "Procedure: " + ex.Errors[i].Procedure + "\n");
                }
                Console.WriteLine(errorMessages.ToString());
                return false;
            }
        }

        public static bool eliminarDocumentoHistorico(int idDoc)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            StringBuilder errorMessages = new StringBuilder();
            try
            {
                List<documento_historico> registros = (from v in bd.documento_historico where v.id == idDoc select v).ToList();
                List<documento_historico> listaTipos = registros.ToList();
                if (registros != null)
                {
                    foreach (documento_historico item in listaTipos)
                    {
                        bd.documento_historico.Remove(item);
                        bd.SaveChanges();

                        //borramos el fichero 
                        var rutaFichero = item.ruta.Replace("..", "");
                        var nombreFichero = item.nombre;
                        string rutaServer = System.Web.HttpContext.Current.Server.MapPath("~/");
                        var rutaFinal = rutaServer + rutaFichero + nombreFichero;
                        if (System.IO.File.Exists(rutaFinal))
                        {
                            System.IO.File.Delete(rutaFinal);
                        }
                    }
                    return true;
                }
                else { return false; }
            }
            catch (SqlException ex)
            {
                for (int i = 0; i < ex.Errors.Count; i++)
                {
                    errorMessages.Append("Index #" + i + "\n" +
                        "Message: " + ex.Errors[i].Message + "\n" +
                        "LineNumber: " + ex.Errors[i].LineNumber + "\n" +
                        "Source: " + ex.Errors[i].Source + "\n" +
                        "Procedure: " + ex.Errors[i].Procedure + "\n");
                }
                Console.WriteLine(errorMessages.ToString());
                return false;
            }
        }
        public static bool EliminarDocumentoRiesgoPorIdCentro(int id_centro)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            StringBuilder errorMessages = new StringBuilder();
            try
            {
                List<documentos_riesgos> registros = (from v in bd.documentos_riesgos where v.id_centro == id_centro select v).ToList();
                if (registros != null)
                {
                    bd.documentos_riesgos.RemoveRange(registros);
                    bd.SaveChanges();

                    return true;
                }
                else { return false; }
            }
            catch (SqlException ex)
            {
                for (int i = 0; i < ex.Errors.Count; i++)
                {
                    errorMessages.Append("Index #" + i + "\n" +
                        "Message: " + ex.Errors[i].Message + "\n" +
                        "LineNumber: " + ex.Errors[i].LineNumber + "\n" +
                        "Source: " + ex.Errors[i].Source + "\n" +
                        "Procedure: " + ex.Errors[i].Procedure + "\n");
                }
                Console.WriteLine(errorMessages.ToString());
                return false;
            }
        }

        public static bool EliminarParametricaMedidasPorIdCentro(int id_centro)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            StringBuilder errorMessages = new StringBuilder();
            try
            {
                List<parametrica_medidas> registros = (from v in bd.parametrica_medidas where v.id_centro == id_centro select v).ToList();
                if (registros != null)
                {
                    bd.parametrica_medidas.RemoveRange(registros);
                    bd.SaveChanges();

                    return true;
                }
                else { return false; }
            }
            catch (SqlException ex)
            {
                for (int i = 0; i < ex.Errors.Count; i++)
                {
                    errorMessages.Append("Index #" + i + "\n" +
                        "Message: " + ex.Errors[i].Message + "\n" +
                        "LineNumber: " + ex.Errors[i].LineNumber + "\n" +
                        "Source: " + ex.Errors[i].Source + "\n" +
                        "Procedure: " + ex.Errors[i].Procedure + "\n");
                }
                Console.WriteLine(errorMessages.ToString());
                return false;
            }
        }

        public static List<matriz_centro> eliminarMatrizRiesgoArea(int id_seleccionado, int tecnologia)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            List<matriz_centro> registros = (from v in bd.matriz_centro where v.id_areanivel1 == id_seleccionado && v.id_tecnologia == tecnologia select v).ToList();
            List<matriz_centro> listaTipos = registros.ToList();
            foreach (matriz_centro item in listaTipos)
            {
                bd.matriz_centro.Remove(item);
                bd.SaveChanges();
            }
            return registros;
        }

        public static List<matriz_centro> eliminarMatrizRiesgoSistema(int id_seleccionado, int tecnologia)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            var registros = (from v in bd.matriz_centro where v.id_areanivel2 == id_seleccionado && v.id_tecnologia == tecnologia select v).ToList();

            bd.matriz_centro.RemoveRange(registros);
            bd.SaveChanges();
            return registros;
        }
        public static List<matriz_centro> eliminarMatrizRiesgoEquipo(int id_seleccionado, int tecnologia)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            var registros = (from v in bd.matriz_centro where v.id_areanivel3 == id_seleccionado && v.id_tecnologia == tecnologia select v).ToList();

            bd.matriz_centro.RemoveRange(registros);
            bd.SaveChanges();
            return registros;

        }

        public static async Task<List<matriz_centro>> eliminarMatrizRiesgoSistemaAsync(int id_seleccionado, int tecnologia)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            var registros = await (from v in bd.matriz_centro where v.id_areanivel2 == id_seleccionado && v.id_tecnologia == tecnologia select v).ToListAsync();

            bd.matriz_centro.RemoveRange(registros);
            bd.SaveChanges();
            return registros;
        }

        public static async Task<List<matriz_centro>> eliminarMatrizRiesgoEquipoAsync(int id_seleccionado, int tecnologia)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            var registros = await (from v in bd.matriz_centro where v.id_areanivel3 == id_seleccionado && v.id_tecnologia == tecnologia select v).ToListAsync();

            bd.matriz_centro.RemoveRange(registros);
            bd.SaveChanges();
            return registros;

        }
        public static async Task<List<matriz_centro>> eliminarMatrizRiesgoNivel4Async(int id_seleccionado, int tecnologia)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            var registros = await (from v in bd.matriz_centro where v.id_areanivel4 == id_seleccionado && v.id_tecnologia == tecnologia select v).ToListAsync();

            bd.matriz_centro.RemoveRange(registros);
            bd.SaveChanges();
            return registros;
        }
        public static List<matriz_centro> eliminarMatrizRiesgoNivel4(int id_seleccionado, int tecnologia)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            var registros = (from v in bd.matriz_centro where v.id_areanivel4 == id_seleccionado && v.id_tecnologia == tecnologia select v).ToList();

            bd.matriz_centro.RemoveRange(registros);
            bd.SaveChanges();
            return registros;
        }
        public static List<matriz_centro_critico> eliminarMatrizRiesgoAreaCritico(int id_seleccionado, int tecnologia)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            List<matriz_centro_critico> registros = (from v in bd.matriz_centro_critico where v.id_areanivel1 == id_seleccionado && v.id_tecnologia == tecnologia select v).ToList();
            List<matriz_centro_critico> listaTipos = registros.ToList();
            foreach (matriz_centro_critico item in listaTipos)
            {
                bd.matriz_centro_critico.Remove(item);
                bd.SaveChanges();
            }
            return registros;
        }
        public static async Task<List<matriz_centro_critico>> eliminarMatrizRiesgoSistemaCriticoAsync(int id_seleccionado, int tecnologia)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            var registros = await (from v in bd.matriz_centro_critico where v.id_areanivel2 == id_seleccionado && v.id_tecnologia == tecnologia select v).ToListAsync();

            bd.matriz_centro_critico.RemoveRange(registros);
            bd.SaveChanges();
            return registros;
        }
        public static async Task<List<matriz_centro_critico>> eliminarMatrizRiesgoEquipoCriticoAsync(int id_seleccionado, int tecnologia)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            var registros = await (from v in bd.matriz_centro_critico where v.id_areanivel3 == id_seleccionado && v.id_tecnologia == tecnologia select v).ToListAsync();

            bd.matriz_centro_critico.RemoveRange(registros);
            bd.SaveChanges();
            return registros;

        }
        public static async Task<List<matriz_centro_critico>> eliminarMatrizRiesgoNivel4CriticoAsync(int id_seleccionado, int tecnologia)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            var registros = await (from v in bd.matriz_centro_critico where v.id_areanivel4 == id_seleccionado && v.id_tecnologia == tecnologia select v).ToListAsync();

            bd.matriz_centro_critico.RemoveRange(registros);
            bd.SaveChanges();
            return registros;
        }



        public static List<matriz_centro> actualizarMatrizEquipo(Hashtable matrizEquipo, int idEquipo)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            List<matriz_centro> registros = (from v in bd.matriz_centro where v.id_areanivel3 == idEquipo select v).ToList();
            foreach (matriz_centro item in registros)
            {
                if (matrizEquipo.Contains(item.id_riesgo))
                {
                    item.activo = (bool)matrizEquipo[item.id_riesgo];
                }
                bd.SaveChanges();
            }
            return registros;
        }
        public static List<matriz_centro> actualizarMatriznivel4(Hashtable matrizEquipo, int idEquipo)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            List<matriz_centro> registros = (from v in bd.matriz_centro where v.id_areanivel4 == idEquipo select v).ToList();
            foreach (matriz_centro item in registros)
            {
                if (matrizEquipo.Contains(item.id_riesgo))
                {
                    item.activo = (bool)matrizEquipo[item.id_riesgo];
                }
                bd.SaveChanges();
            }
            return registros;
        }




        public static List<matriz_inicial> listarMatrizInicial()
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            var registros = from v in bd.matriz_inicial select v;
            List<matriz_inicial> listaTipos = registros.ToList();
            return listaTipos;

        }
        public static List<matriz_inicial> listarMatrizInicial(List<string> lista)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            var registros = from v in bd.matriz_inicial where lista.Contains(v.id_tecnologia.ToString()) && v.activo == true select v;
            List<matriz_inicial> listaTipos = registros.ToList();
            return listaTipos;

        }
        public static List<areanivel1> listarAreasInicial(int tecnologia)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            var registros = from v in bd.areanivel1 where v.id_tecnologia == tecnologia && v.id_centro == 0 select v;
            List<areanivel1> listaTipos = registros.ToList();
            return listaTipos;

        }
        public static List<areanivel1> listarAreasInicialAPartirDeMatrizCentro(int version)
        {
            DIMASSTEntities bd = new DIMASSTEntities();
            var registros = (from v in bd.matriz_centro where v.version == version select v.id_areanivel1).Distinct();

            List<areanivel1> listaNivel1 = new List<areanivel1>();
            foreach (var item in registros)
            {
                if (item != null)
                {
                    listaNivel1.Add(ObtenerAreaNivel1PorId(item.Value));
                }
            }

            return listaNivel1;


        }

        public static List<tipocentral> ListarTecnologiasProcesos(int tecnologia)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var registros = from v in bd.tipocentral where v.id == tecnologia orderby v.id ascending select v;

            List<tipocentral> listaTipos = registros.ToList();

            return listaTipos;

        }

        public static List<procesos> ListarProcesosBytecnologia(int idOrg, string tipoProceso, int tecnologia, int url)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            List<procesos> listaProcesos = new List<procesos>();

            List<procesos> registros = new List<procesos>();
            List<procesos> registrosaux = new List<procesos>();
            // proceso padre
            if (tipoProceso != "T")
            {
                if (tecnologia == 4)
                {
                    if (url == 1)
                    {
                        registros = (from u in bd.procesos
                                     where u.organizacion == idOrg && u.tipo == tipoProceso
                                     && u.padre == null
                                     select u).OrderBy(x => x.nivel).OrderBy(x => x.orden).ToList();
                    }
                    else
                    {
                        registros = (from u in bd.procesos
                                     where u.organizacion == idOrg && u.tipo == tipoProceso
                                     && u.padre == null && u.tecnologia == null
                                     select u).OrderBy(x => x.nivel).OrderBy(x => x.orden).ToList();

                        registrosaux = (from u in bd.procesos
                                        where u.organizacion == idOrg && u.tipo == tipoProceso
                                        && u.padre == null && u.tecnologia == tecnologia
                                        select u).OrderBy(x => x.nivel).OrderBy(x => x.orden).ToList();
                        var resultado = registros.Union(registrosaux).ToList();
                        if (resultado != null)
                        {
                            registros = resultado;
                        }
                    }

                }
                if (tecnologia != 4)
                {
                    registros = (from u in bd.procesos
                                 where u.organizacion == idOrg && u.tipo == tipoProceso
                                 && u.padre == null && u.tecnologia == null
                                 select u).OrderBy(x => x.nivel).OrderBy(x => x.orden).ToList();

                    registrosaux = (from u in bd.procesos
                                    where u.organizacion == idOrg && u.tipo == tipoProceso
                                    && u.padre == null && u.tecnologia == tecnologia
                                    select u).OrderBy(x => x.nivel).OrderBy(x => x.orden).ToList();
                    var resultado = registros.Union(registrosaux).ToList();
                    if (resultado != null)
                    {
                        registros = resultado;
                    }
                }

            }
            // procesos hijos
            else
            {
                registros = (from u in bd.procesos
                             where u.organizacion == idOrg && u.padre != null
                             select u).OrderBy(x => x.nivel).OrderBy(x => x.orden).ToList();
            }



            foreach (procesos registro in registros)
            {
                procesos nuevoProceso = new procesos();
                nuevoProceso.id = registro.id;
                nuevoProceso.nombre = registro.nombre;
                nuevoProceso.tipo = registro.tipo;
                nuevoProceso.nivel = registro.nivel;
                nuevoProceso.padre = registro.padre;
                nuevoProceso.organizacion = registro.organizacion;
                nuevoProceso.cod_proceso = registro.cod_proceso;
                listaProcesos.Add(nuevoProceso);
            }

            return listaProcesos;

        }

        public static List<aspecto_parametro> ListarParametros(int idAspecto)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var registros = from v in bd.aspecto_parametro where v.id_aspecto == idAspecto orderby v.nombre ascending select v;

            List<aspecto_parametro> listaTipos = registros.ToList();

            return listaTipos;

        }

        public static List<aspecto_parametro_valoracion> ListarParametrosAsignados(int idAspecto)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var registros = from v in bd.aspecto_parametro_valoracion where v.id_aspecto == idAspecto orderby v.id_parametro ascending select v;

            List<aspecto_parametro_valoracion> listaTipos = registros.ToList();

            return listaTipos;

        }

        public static tipocentral ListarTecnologia(int? id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var registros = from v in bd.tipocentral where v.id == id orderby v.id ascending select v;

            tipocentral listaTipos = registros.FirstOrDefault();

            return listaTipos;

        }

        public static List<procesos> ListarProcesos()
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var registros = from v in bd.procesos orderby v.id ascending select v;

            List<procesos> listaProcesos = new List<procesos>();

            foreach (procesos registro in registros)
            {
                procesos nuevoProceso = new procesos();
                nuevoProceso.id = registro.id;
                nuevoProceso.cod_proceso = registro.cod_proceso;
                nuevoProceso.nombre = registro.cod_proceso + "-" + registro.nombre;
                listaProcesos.Add(nuevoProceso);
            }

            return listaProcesos;

        }

        public static List<normas> ListarNormas()
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var registros = from v in bd.normas orderby v.id descending select v;

            List<normas> listaNormasCargadas = registros.ToList();

            return listaNormasCargadas;

        }

        public static List<VISTA_ListarEnlaces> ListarEnlaces()
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var registros = from v in bd.VISTA_ListarEnlaces orderby v.id descending select v;

            List<VISTA_ListarEnlaces> listaEnlacesCargados = registros.ToList();

            return listaEnlacesCargados;

        }

        public static List<VISTA_ListarMateriales> ListarMateriales()
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var registros = from v in bd.VISTA_ListarMateriales orderby v.id descending select v;

            List<VISTA_ListarMateriales> listaApoyosCargados = registros.ToList();

            return listaApoyosCargados;

        }

        public static List<VISTA_ListarInfSeg> ListarInformesSeguridad()
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var registros = from v in bd.VISTA_ListarInfSeg orderby v.id descending select v;

            List<VISTA_ListarInfSeg> listaApoyosCargados = registros.ToList();

            return listaApoyosCargados;

        }

        public static List<VISTA_EvalRiesgos> ListarEvaluacionesRiesgo()
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var registros = from v in bd.VISTA_EvalRiesgos orderby v.id descending select v;

            List<VISTA_EvalRiesgos> listaApoyosCargados = registros.ToList();

            return listaApoyosCargados;

        }

        public static List<ficheros> ListarFicheros(int tipo)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var registros = from v in bd.ficheros where v.tipo == tipo orderby v.id descending select v;

            List<ficheros> listaArchivosCargados = registros.ToList();

            return listaArchivosCargados;

        }

        public static List<imagenes> ListarImagenes(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var registros = from v in bd.imagenes where v.idnoticia == id orderby v.id descending select v;

            List<imagenes> listaArchivosCargados = registros.ToList();

            return listaArchivosCargados;

        }

        public static int InsertFichero(ficheros oFichero)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            if (oFichero != null)
            {
                // set new value 
                bd.ficheros.Add(oFichero);
                // save changeds
                bd.SaveChanges();
            }
            return oFichero.id;
        }

        public static int InsertNorma(normas oNorma)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            if (oNorma != null)
            {
                // set new value 
                bd.normas.Add(oNorma);
                // save changeds
                bd.SaveChanges();
            }
            return oNorma.id;
        }
        public static int InsertarRegistroMatriz(matriz_centro matriz)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            if (matriz != null)
            {
                // set new value 
                bd.matriz_centro.Add(matriz);
                // save changeds
                bd.SaveChanges();
            }
            return matriz.id;
        }

        public static bool actualizarPadres(matriz_centro matriz, int nivel)
        {
            bool resultado = false;
            try
            {
                DIMASSTEntities bd = new DIMASSTEntities();
                if (nivel == 3)
                {
                    areanivel3 padre = (from v in bd.areanivel3 where v.id == matriz.id_areanivel3 select v).FirstOrDefault();
                    matriz_centro registros = (from v in bd.matriz_centro
                                               where
                    v.id_areanivel2 == padre.id_areanivel2 &&
                    v.version == matriz.version &&
                    v.id_tecnologia == matriz.id_tecnologia
                    && v.id_riesgo == matriz.id_riesgo
                                               select v).FirstOrDefault();

                    if (registros != null)
                    {
                        registros.activo = true;
                        bd.SaveChanges();
                        actualizarPadres(registros, 2);
                    }


                }
                else if (nivel == 2)
                {
                    areanivel2 padre = (from v in bd.areanivel2 where v.id == matriz.id_areanivel2 select v).FirstOrDefault();
                    matriz_centro registros = (from v in bd.matriz_centro
                                               where
                    v.id_areanivel1 == padre.id_areanivel1 &&
                    v.version == matriz.version &&
                    v.id_tecnologia == matriz.id_tecnologia
                    && v.id_riesgo == matriz.id_riesgo
                                               select v).FirstOrDefault();
                    if (registros != null)
                    {
                        registros.activo = true;
                        bd.SaveChanges();
                    }
                }



                resultado = true;

            }
            catch (Exception ex)
            {
                resultado = false;
            }



            return resultado;
        }
        public static bool InsertarMatriz(List<matriz_centro> conjunto_matrices)
        {
            bool resultado = false;
            try
            {
                DIMASSTEntities bd = new DIMASSTEntities();
                foreach (matriz_centro matriz in conjunto_matrices)
                {
                    if (matriz != null)
                    {
                        // set new value 
                        bd.matriz_centro.Add(matriz);
                        // save changeds

                    }
                }
                bd.SaveChanges();
                resultado = true;

            }
            catch (Exception ex)
            {
                resultado = false;
            }



            return resultado;
        }
        public static bool InsertarMatrizCriticoConjunto(List<matriz_centro_critico> conjunto_matrices)
        {
            bool resultado = false;
            try
            {
                DIMASSTEntities bd = new DIMASSTEntities();
                foreach (matriz_centro_critico matriz in conjunto_matrices)
                {
                    if (matriz != null)
                    {
                        // set new value 
                        bd.matriz_centro_critico.Add(matriz);
                        // save changeds

                    }
                }
                bd.SaveChanges();
                resultado = true;

            }
            catch (Exception ex)
            {
                resultado = false;
            }



            return resultado;
        }
        public static bool ActualizarFilaMatrizNivel4(List<matriz_centro> conjunto_matrices, int id_seleccionado)
        {
            bool resultado = false;
            try
            {
                DIMASSTEntities bd = new DIMASSTEntities();

                List<matriz_centro> registros = (from u in bd.matriz_centro
                                                 where u.id_areanivel4 == id_seleccionado
                                                 select u).ToList();

                for (var i = 0; i < registros.Count; i++)
                {
                    registros[i].activo = conjunto_matrices[i].activo;
                }

                bd.SaveChanges();
                resultado = true;
            }
            catch (Exception ex)
            {
                resultado = false;
            }

            return resultado;
        }
        public static bool ActualizarFilaMatrizNivel4Critico(List<matriz_centro_critico> conjunto_matrices, int id_seleccionado)
        {
            bool resultado = false;
            try
            {
                DIMASSTEntities bd = new DIMASSTEntities();

                List<matriz_centro_critico> registros = (from u in bd.matriz_centro_critico
                                                         where u.id_areanivel4 == id_seleccionado
                                                         select u).ToList();

                for (var i = 0; i < registros.Count; i++)
                {
                    registros[i].activo = conjunto_matrices[i].activo;
                }

                bd.SaveChanges();
                resultado = true;
            }
            catch (Exception ex)
            {
                resultado = false;
            }

            return resultado;
        }
        public static bool ActualizarFilaMatrizNivel3(List<matriz_centro> conjunto_matrices, int id_seleccionado)
        {
            bool resultado = false;
            try
            {
                DIMASSTEntities bd = new DIMASSTEntities();

                List<matriz_centro> registros = (from u in bd.matriz_centro
                                                 where u.id_areanivel3 == id_seleccionado
                                                 select u).ToList();

                for (var i = 0; i < registros.Count; i++)
                {
                    registros[i].activo = conjunto_matrices[i].activo;
                }

                bd.SaveChanges();
                resultado = true;
            }
            catch (Exception ex)
            {
                resultado = false;
            }

            return resultado;
        }
        public static bool ActualizarFilaMatrizNivel3Critico(List<matriz_centro_critico> conjunto_matrices, int id_seleccionado)
        {
            bool resultado = false;
            try
            {
                DIMASSTEntities bd = new DIMASSTEntities();

                List<matriz_centro_critico> registros = (from u in bd.matriz_centro_critico
                                                         where u.id_areanivel3 == id_seleccionado
                                                         select u).ToList();

                for (var i = 0; i < registros.Count; i++)
                {
                    registros[i].activo = conjunto_matrices[i].activo;
                }

                bd.SaveChanges();
                resultado = true;
            }
            catch (Exception ex)
            {
                resultado = false;
            }

            return resultado;
        }
        public static bool ActualizarFilaMatrizNivel2(List<matriz_centro> conjunto_matrices, int id_seleccionado)
        {
            bool resultado = false;
            try
            {
                DIMASSTEntities bd = new DIMASSTEntities();

                List<matriz_centro> registros = (from u in bd.matriz_centro
                                                 where u.id_areanivel2 == id_seleccionado
                                                 select u).ToList();

                for (var i = 0; i < registros.Count; i++)
                {
                    registros[i].activo = conjunto_matrices[i].activo;
                }

                bd.SaveChanges();
                resultado = true;

            }
            catch (Exception ex)
            {
                resultado = false;
            }



            return resultado;
        }
        public static bool ActualizarFilaMatrizNivel2Critico(List<matriz_centro_critico> conjunto_matrices, int id_seleccionado)
        {
            bool resultado = false;
            try
            {
                DIMASSTEntities bd = new DIMASSTEntities();

                List<matriz_centro_critico> registros = (from u in bd.matriz_centro_critico
                                                         where u.id_areanivel2 == id_seleccionado
                                                         select u).ToList();

                for (var i = 0; i < registros.Count; i++)
                {
                    registros[i].activo = conjunto_matrices[i].activo;
                }

                bd.SaveChanges();
                resultado = true;
            }
            catch (Exception ex)
            {
                resultado = false;
            }

            return resultado;
        }

        public static int InsertEvidencia(evidencias oFichero)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            if (oFichero != null)
            {
                // set new value 
                bd.evidencias.Add(oFichero);
                // save changeds
                bd.SaveChanges();
            }
            return oFichero.id;
        }

        public static int InsertCualificacion(cualificaciones oFichero)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            if (oFichero != null)
            {
                // set new value 
                bd.cualificaciones.Add(oFichero);
                // save changeds
                bd.SaveChanges();
            }
            return oFichero.id;
        }

        public static int InsertFotoEventoAmbiental(evento_ambiental_foto oFichero)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            if (oFichero != null)
            {
                // set new value 
                bd.evento_ambiental_foto.Add(oFichero);
                // save changeds
                bd.SaveChanges();
            }
            return oFichero.id;
        }

        public static int InsertFotoEventoSeguridad(evento_seguridad_foto oFichero)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            if (oFichero != null)
            {
                // set new value 
                bd.evento_seguridad_foto.Add(oFichero);
                // save changeds
                bd.SaveChanges();
            }
            return oFichero.id;
        }

        public static int InsertDocComunicacion(comunicacion_documentos oFichero)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            if (oFichero != null)
            {
                // set new value 
                bd.comunicacion_documentos.Add(oFichero);
                // save changeds
                bd.SaveChanges();
            }
            return oFichero.id;
        }

        public static int InserDocEventoSeguridad(evento_seguridad_documentos oFichero)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            if (oFichero != null)
            {
                // set new value 
                bd.evento_seguridad_documentos.Add(oFichero);
                // save changeds
                bd.SaveChanges();
            }
            return oFichero.id;
        }

        public static int InsertDocAccMejora(accionmejora_documento oFichero)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            if (oFichero != null)
            {
                // set new value 
                bd.accionmejora_documento.Add(oFichero);
                // save changeds
                bd.SaveChanges();
            }
            return oFichero.id;
        }

        public static int InsertDocReunion(reuniones_documentos oFichero)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            if (oFichero != null)
            {
                // set new value 
                bd.reuniones_documentos.Add(oFichero);
                // save changeds
                bd.SaveChanges();
            }
            return oFichero.id;
        }

        public static int InsertDocEventoAmb(evento_ambiental_documentos oFichero)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            if (oFichero != null)
            {
                // set new value 
                bd.evento_ambiental_documentos.Add(oFichero);
                // save changeds
                bd.SaveChanges();
            }
            return oFichero.id;
        }

        public static int InsertDocEventoSeg(evento_seguridad_documentos oFichero)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            if (oFichero != null)
            {
                // set new value 
                bd.evento_seguridad_documentos.Add(oFichero);
                // save changeds
                bd.SaveChanges();
            }
            return oFichero.id;
        }

        public static int InsertImagen(imagenes oFichero)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            if (oFichero != null)
            {
                // set new value 
                bd.imagenes.Add(oFichero);
                // save changeds
                bd.SaveChanges();
            }
            return oFichero.id;
        }

        public static void InsertarEnLog(log registro)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            bd.log.Add(registro);
            bd.SaveChanges();

        }

        public static void ActualizarRutaNorma(int id, string path)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            normas oNorma = (from v in bd.normas where v.id == id select v).FirstOrDefault();

            oNorma.enlace = path;
            bd.SaveChanges();

        }

        public static void ActualizarDescargas(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            normas oNorma = (from v in bd.normas where v.id == id select v).FirstOrDefault();

            oNorma.descargas = oNorma.descargas + 1;
            bd.SaveChanges();

        }

        public static void ActualizarDescargasMaterial(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            materialdivulgativo omat = (from v in bd.materialdivulgativo where v.id == id select v).FirstOrDefault();

            omat.descargas = omat.descargas + 1;
            bd.SaveChanges();

        }

        public static void ActualizarDescargasInfSeguridad(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            informesseguridad oinf = (from v in bd.informesseguridad where v.id == id select v).FirstOrDefault();

            oinf.descargas = oinf.descargas + 1;
            bd.SaveChanges();

        }

        public static void ActualizarDescargasEvalRiesgos(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            evaluacionriesgos oeval = (from v in bd.evaluacionriesgos where v.id == id select v).FirstOrDefault();

            oeval.descargas = oeval.descargas + 1;
            bd.SaveChanges();

        }

        public static int InsertDocumentacion(documentacion oFichero)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            if (oFichero != null)
            {
                // set new value 
                bd.documentacion.Add(oFichero);
                // save changeds
                bd.SaveChanges();
            }
            return oFichero.idFichero;
        }

        public static int InsertHistorico(documentacion_hist oFichero)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            if (oFichero != null)
            {
                // set new value 
                bd.documentacion_hist.Add(oFichero);
                // save changeds
                bd.SaveChanges();
            }
            return oFichero.id;
        }

        public static void UpdateDocumentacionEnlace(documentacion oFichero)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.documentacion
                              where u.idFichero == oFichero.idFichero
                              select u).FirstOrDefault();

            actualizar.enlace = oFichero.enlace;
            actualizar.nombre_fichero = oFichero.nombre_fichero;
            actualizar.cod_fichero = oFichero.cod_fichero;
            bd.SaveChanges();
        }

        public static void UpdateProgramaAuditoriaEnlace(auditorias oAuditoria)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.auditorias
                              where u.id == oAuditoria.id
                              select u).FirstOrDefault();

            actualizar.programa = oAuditoria.programa;
            bd.SaveChanges();
        }

        public static void UpdateProgramaAuditoriaGeneral(auditorias_programa oPrograma)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.auditorias_programa
                              where u.id == 1
                              select u).FirstOrDefault();

            actualizar.rutaFichero = oPrograma.rutaFichero;
            actualizar.nombrefichero = oPrograma.nombrefichero;
            bd.SaveChanges();
        }

        public static void UpdateProgramaDocumentacionGeneral(documentos_programa oPrograma)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.documentos_programa
                              where u.id == 1
                              select u).FirstOrDefault();

            actualizar.rutaFichero = oPrograma.rutaFichero;
            actualizar.nombrefichero = oPrograma.nombrefichero;
            bd.SaveChanges();
        }

        public static void UpdateCualificacionAuditor(auditores oAuditor)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.auditores
                              where u.id == oAuditor.id
                              select u).FirstOrDefault();

            actualizar.cv = oAuditor.cv;
            bd.SaveChanges();
        }

        public static void UpdateInformeAuditoriaEnlace(auditorias oAuditoria)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.auditorias
                              where u.id == oAuditoria.id
                              select u).FirstOrDefault();

            actualizar.informe = oAuditoria.informe;
            bd.SaveChanges();
        }

        public static void UpdatePlanificacionRev(revision_energetica oRevision)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.revision_energetica
                              where u.id == oRevision.id
                              select u).FirstOrDefault();

            actualizar.planificacionenergetica = oRevision.planificacionenergetica;
            bd.SaveChanges();
        }

        public static void UpdateRevisionRev(revision_energetica oRevision)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.revision_energetica
                              where u.id == oRevision.id
                              select u).FirstOrDefault();

            actualizar.revisionenergetica = oRevision.revisionenergetica;
            bd.SaveChanges();
        }

        public static void UpdatePlanInicialEnlace(formacion oFormacion)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.formacion
                              where u.id == oFormacion.id
                              select u).FirstOrDefault();

            actualizar.planificacion_inicial = oFormacion.planificacion_inicial;
            bd.SaveChanges();
        }

        public static void UpdateInformeEnlace(emergencias oEmergencia)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.emergencias
                              where u.id == oEmergencia.id
                              select u).FirstOrDefault();

            actualizar.informe = oEmergencia.informe;
            bd.SaveChanges();
        }

        public static void UpdateEnlaceNorma(normas oNorma)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.normas
                              where u.id == oNorma.id
                              select u).FirstOrDefault();

            actualizar.enlace = oNorma.enlace;
            bd.SaveChanges();
        }

        public static void UpdateEnlaceEnlace(enlaces oEnlace)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.enlaces
                              where u.id == oEnlace.id
                              select u).FirstOrDefault();

            actualizar.enlace = oEnlace.enlace;
            bd.SaveChanges();
        }

        public static void UpdateEnlaceMaterial(materialdivulgativo oEnlace)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.materialdivulgativo
                              where u.id == oEnlace.id
                              select u).FirstOrDefault();

            actualizar.enlace = oEnlace.enlace;
            bd.SaveChanges();
        }

        public static void UpdateEnlaceEvaluacion(evaluacionriesgos oEnlace)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.evaluacionriesgos
                              where u.id == oEnlace.id
                              select u).FirstOrDefault();

            actualizar.enlace = oEnlace.enlace;
            bd.SaveChanges();
        }

        public static void UpdateEnlaceInformeSeg(informesseguridad oEnlace)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.informesseguridad
                              where u.id == oEnlace.id
                              select u).FirstOrDefault();

            actualizar.enlace = oEnlace.enlace;
            bd.SaveChanges();
        }

        public static void UpdateInformeSatisfaccion(satisfaccion oSatisfaccion)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.satisfaccion
                              where u.id == oSatisfaccion.id
                              select u).FirstOrDefault();

            actualizar.informe = oSatisfaccion.informe;
            bd.SaveChanges();
        }

        public static void UpdateInformeEvaluacionEnlace(requisitoslegales oRequisito)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.requisitoslegales
                              where u.id == oRequisito.id
                              select u).FirstOrDefault();

            actualizar.informeevaluacion = oRequisito.informeevaluacion;
            bd.SaveChanges();
        }

        public static void UpdatePlanEjecutadoEnlace(formacion oFormacion)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.formacion
                              where u.id == oFormacion.id
                              select u).FirstOrDefault();

            actualizar.planificacion_ejecutada = oFormacion.planificacion_ejecutada;
            bd.SaveChanges();
        }

        public static void UpdateDocumentacion(documentacion oFichero)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.documentacion
                              where u.idFichero == oFichero.idFichero
                              select u).FirstOrDefault();

            actualizar.titulo = oFichero.titulo;
            actualizar.fecha_aprobacion = oFichero.fecha_aprobacion;
            actualizar.fecha_publicacion = oFichero.fecha_publicacion;
            actualizar.idproceso = oFichero.idproceso;
            actualizar.cod_fichero = oFichero.cod_fichero;
            actualizar.version = oFichero.version;
            actualizar.tipo = oFichero.tipo;
            bd.SaveChanges();
        }

        public static void UpdateHistorico(documentacion_hist oFichero)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var actualizar = (from u in bd.documentacion_hist
                              where u.id == oFichero.id
                              select u).FirstOrDefault();

            actualizar.enlace = oFichero.enlace;
            bd.SaveChanges();
        }

        public static normas ObtenerNormaPorID(int idFichero)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            normas nuevo = new normas();

            var registro = (from u in bd.normas
                            where u.id == idFichero
                            select u).FirstOrDefault();


            if (registro != null)
            {
                nuevo.id = registro.id;
                nuevo.nombre_norma = registro.nombre_norma;
                nuevo.descargas = registro.descargas;
                nuevo.enlace = registro.enlace;
            }
            else
                nuevo = null;


            return nuevo;
        }

        public static enlaces ObtenerEnlacePorID(int idFichero)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            enlaces nuevo = new enlaces();

            var registro = (from u in bd.enlaces
                            where u.id == idFichero
                            select u).FirstOrDefault();


            if (registro != null)
            {
                nuevo.id = registro.id;
                nuevo.titulo = registro.titulo;
                nuevo.enlace = registro.enlace;
            }
            else
                nuevo = null;


            return nuevo;
        }

        public static materialdivulgativo ObtenerMaterialPorID(int idFichero)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            materialdivulgativo nuevo = new materialdivulgativo();

            var registro = (from u in bd.materialdivulgativo
                            where u.id == idFichero
                            select u).FirstOrDefault();


            if (registro != null)
            {
                nuevo.id = registro.id;
                nuevo.titulo = registro.titulo;
                nuevo.enlace = registro.enlace;
            }
            else
                nuevo = null;


            return nuevo;
        }

        public static informesseguridad ObtenerInformeSeguridadPorID(int idFichero)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            informesseguridad nuevo = new informesseguridad();

            var registro = (from u in bd.informesseguridad
                            where u.id == idFichero
                            select u).FirstOrDefault();


            if (registro != null)
            {
                nuevo.id = registro.id;
                nuevo.titulo = registro.titulo;
                nuevo.enlace = registro.enlace;
            }
            else
                nuevo = null;


            return nuevo;
        }

        public static evaluacionriesgos ObtenerEvaluacionRiesgosPorID(int idFichero)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            evaluacionriesgos nuevo = new evaluacionriesgos();

            var registro = (from u in bd.evaluacionriesgos
                            where u.id == idFichero
                            select u).FirstOrDefault();


            if (registro != null)
            {
                nuevo.id = registro.id;
                nuevo.titulo = registro.titulo;
                nuevo.enlace = registro.enlace;
            }
            else
                nuevo = null;


            return nuevo;
        }

        public static ficheros ObtenerFicheroPorID(int idFichero)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            ficheros nuevo = new ficheros();

            var registro = (from u in bd.ficheros
                            where u.id == idFichero
                            select u).FirstOrDefault();


            if (registro != null)
            {
                nuevo.id = registro.id;
                nuevo.nombre_fichero = registro.nombre_fichero;
                nuevo.tipo = registro.tipo;
                nuevo.enlace = registro.enlace;
            }
            else
                nuevo = null;


            return nuevo;
        }

        public static ficheros ObtenerDocumento(int idDoc)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            ficheros nuevo = new ficheros();

            var registro = (from u in bd.documentacion
                            where u.idFichero == idDoc
                            select u).FirstOrDefault();


            if (registro != null)
            {
                nuevo.id = registro.idFichero;
                nuevo.nombre_fichero = registro.nombre_fichero;
                nuevo.enlace = registro.enlace;
            }
            else
                nuevo = null;


            return nuevo;
        }

        public static ficheros ObtenerDocumentoPorID(int idDoc)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            ficheros nuevo = new ficheros();

            var registro = (from u in bd.documentacion
                            where u.idFichero == idDoc
                            select u).FirstOrDefault();


            if (registro != null)
            {
                nuevo.id = registro.idFichero;
                nuevo.nombre_fichero = registro.idFichero + "\\";
                nuevo.enlace = registro.enlace;
            }
            else
                nuevo = null;


            return nuevo;
        }

        public static documento_historico ObtenerDocumentoHistoricoRutaDescarga(int id_centro)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            documento_historico nuevo = new documento_historico();

            var registro = (from u in bd.documento_historico
                            where u.id_centro == id_centro && u.descarga == 1
                            select u).FirstOrDefault();

            if (registro != null)
            {
                nuevo.nombre = registro.nombre;
                nuevo.tipo = registro.tipo;
                nuevo.version = registro.version;
                nuevo.estado = registro.estado;
                nuevo.fechaUltimaModificacion = registro.fechaUltimaModificacion;
                nuevo.usuario = registro.usuario;
                nuevo.descarga = registro.descarga;
                nuevo.ruta = registro.ruta;
                nuevo.id_centro = registro.id_centro;

            }
            else
                nuevo = null;


            return nuevo;
        }

        public static ficheros ObtenerCriteriosEvaluacion(string codigo)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            ficheros nuevo = new ficheros();

            var registro = (from u in bd.documentacion
                            where u.cod_fichero == codigo
                            select u).FirstOrDefault();


            if (registro != null)
            {
                nuevo.id = registro.idFichero;
                nuevo.nombre_fichero = registro.idFichero + "\\";
                nuevo.enlace = registro.enlace;
            }
            else
                nuevo = null;


            return nuevo;
        }

        public static ficheros ObtenerEvidenciaPorID(int idDoc)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            ficheros nuevo = new ficheros();

            var registro = (from u in bd.evidencias
                            where u.id == idDoc
                            select u).FirstOrDefault();


            if (registro != null)
            {
                nuevo.id = registro.id;
                nuevo.nombre_fichero = registro.idaccion + "\\";
                nuevo.enlace = registro.enlace;
            }
            else
                nuevo = null;


            return nuevo;
        }

        public static ficheros ObtenerDocumentoHistPorID(int idDoc)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            ficheros nuevo = new ficheros();

            var registro = (from u in bd.documentacion_hist
                            where u.id == idDoc
                            select u).FirstOrDefault();


            if (registro != null)
            {
                nuevo.id = registro.id;
                nuevo.enlace = registro.enlace;
                nuevo.nombre_fichero = registro.idVigente + "\\" + registro.id + "\\";
            }
            else
                nuevo = null;


            return nuevo;
        }

        public static documentacion ObtenerFicheroDocPorID(int idFichero)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            documentacion nuevo = new documentacion();

            var registro = (from u in bd.documentacion
                            where u.idFichero == idFichero
                            select u).FirstOrDefault();


            if (registro != null)
            {
                nuevo.idFichero = registro.idFichero;
                nuevo.nombre_fichero = registro.nombre_fichero;
                nuevo.tipo = registro.tipo;
                nuevo.enlace = registro.enlace;
                nuevo.fecha_publicacion = registro.fecha_publicacion;
                nuevo.fecha_aprobacion = registro.fecha_aprobacion;
                nuevo.titulo = registro.titulo;
                nuevo.nivel = registro.nivel;
                nuevo.tipocentral = registro.tipocentral;
                nuevo.idproceso = registro.idproceso;
                nuevo.cod_fichero = registro.cod_fichero;
                nuevo.version = registro.version;
            }
            else
                nuevo = null;


            return nuevo;
        }

        public static bool GetExisteRutaImagenMedidasRiesgoIcono(string dato)
        {
            //medidaspreventivas_imagenes resultado;

            DIMASSTEntities bd = new DIMASSTEntities();

            var registro = (from u in bd.medidaspreventivas_imagenes
                            where u.rutaImagen == dato
                            select u).FirstOrDefault();

            if (registro != null)
            {
                return true;
            }

            return false;
        }
        public static bool ExisteRutaImagenCentro(string dato)
        {
            //medidaspreventivas_imagenes resultado;

            DIMASSTEntities bd = new DIMASSTEntities();

            var registro = (from u in bd.centros
                            where u.rutaImagen == dato
                            select u).FirstOrDefault();

            if (registro != null)
            {
                return true;
            }

            return false;
        }
        public static bool ExisteMatrizCritico(int version)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            var registro = (from u in bd.matriz_centro_critico
                            where u.version == version
                            select u).FirstOrDefault();

            if (registro != null)
            {
                return true;
            }

            return false;
        }
        public static bool ExisteRutaImagenIcono(string dato)
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            var registro = (from u in bd.riesgos_medidas
                            where u.imagen == dato
                            select u).FirstOrDefault();

            if (registro != null)
            {
                return true;
            }

            return false;
        }
        public static bool ExisteRutaImagenCentroLogo(string dato)
        {
            //medidaspreventivas_imagenes resultado;

            DIMASSTEntities bd = new DIMASSTEntities();

            var registro = (from u in bd.centros
                            where u.rutaImagenLogo == dato
                            select u).FirstOrDefault();

            if (registro != null)
            {
                return true;
            }

            return false;
        }
        //public static bool ExisteRutaImagen(string claveRuta, string nombreArchivo)
        //{
        //    List<string> listaErroresDiccionario = new List<string>();
        //    DIMASSTEntities bd = new DIMASSTEntities();

        //    Dictionary<string, string> diccionarioRutas = new Dictionary<string, string>()
        //    {
        //        {"imagenCentros", "../Content/images/centros/" },
        //        {"imagenCentrosLogo", "../Content/images/centros/logos/" },
        //        {"imagenIcono", "../Content/images/medidas/medidasgenerales/" }
        //    };

        //    if (diccionarioRutas.TryGetValue(claveRuta, out string resultadoRuta))
        //    {
        //        var registro = (from u in bd.centros
        //                        where u.rutaImagen == resultadoRuta + nombreArchivo
        //                        select u).FirstOrDefault();

        //        if (registro != null)
        //        {
        //            return true;
        //        }
        //    }else if()

        //    else
        //    {
        //        listaErroresDiccionario.Add("Error, no existe la clave de la ruta");
        //    }

        //    return false;
        //}



        public static bool GetExisteRutaImagenMedidasGenerales(string dato)
        {
            //medidaspreventivas_imagenes resultado;

            DIMASSTEntities bd = new DIMASSTEntities();

            var registro = (from u in bd.medidas_generales_imagenes
                            where u.rutaImagen == dato
                            select u).FirstOrDefault();

            if (registro != null)
            {
                return true;
            }

            return false;
        }
        public static bool GetExisteRutaImagenMedidasRiesgoIG(string dato)
        {
            //medidaspreventivas_imagenes resultado;

            DIMASSTEntities bd = new DIMASSTEntities();

            var registro = (from u in bd.riesgos_medidas
                            where (u.imagen == dato && u.imagen_grande == 1)
                            select u).FirstOrDefault();

            if (registro != null)
            {
                return true;
            }

            return false;
        }

        //public static tipocentral ObtenerUnidad(int id)
        //{
        //    DIMASSTEntities bd = new DIMASSTEntities();

        //    tipocentral unidad = new tipocentral();

        //    var registro = (from u in bd.tipocentral
        //                    where u.id == id
        //                    select u).FirstOrDefault();

        //    if (registro != null)
        //    {
        //        unidad.id = registro.id;
        //        unidad.nombre = registro.nombre;
        //    }
        //    else { 
        //        unidad = null;
        //    }

        //    return unidad;
        //}

        public static string ObtenerDimensionesIG(HttpPostedFileBase seleccionArchivoGrande)
        {

            System.IO.Stream stream = seleccionArchivoGrande.InputStream;
            System.Drawing.Image image = System.Drawing.Image.FromStream(stream);
            int height = image.Height;
            int width = image.Width;

            string dimensiones = "Alto: " + height + "; Ancho: " + width;

            return dimensiones;
        }


        #region Personas
        public static bool ExisteRegistroEnTablaPersonas()
        {

            DIMASSTEntities bd = new DIMASSTEntities();

            var registro = (from u in bd.personas
                            select u).FirstOrDefault();

            if (registro != null)
            {
                return true;
            }

            return false;
        }

        public static List<personas> ListaPersonas()
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var registro = (from u in bd.personas
                            select u).ToList();

            return registro;
        }
        public static List<string> ListaPersonas_Actividad()
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var registro = (from u in bd.personas
                            select u.Actividad).Distinct().ToList();

            return registro;
        }
        public static List<string> ListaPersonas_CentroDeTrabajo()
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var registro = (from u in bd.personas
                            select u.Centro_de_trabajo).Distinct().ToList();

            return registro;
        }
        public static List<string> ListaPersonas_PerfilDeRiesgo()
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var registro = (from u in bd.personas
                            select u.Perfil_de_riesgo).Distinct().ToList();

            return registro;
        }

        public static List<personas> ListaPersonasPorNumeroEmpleado(string numeroEmpleado)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var registro = (from u in bd.personas
                            where u.Nº_Empleado == numeroEmpleado
                            select u).ToList();

            return registro;
        }
        public static int GuardarPersonaPorNumeroEmpleado(string numeroEmpleado, int usuarioId)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var persona = (from v in bd.personas
                           where v.Nº_Empleado == numeroEmpleado
                           select v).FirstOrDefault();

            var actualizar = (from u in bd.lista_final_personas
                              where u.Nº_Empleado == numeroEmpleado && u.Usuario_Propietario_Lista == usuarioId
                              select u).FirstOrDefault();



            if (actualizar != null && persona != null)
            {
                actualizar.Nº_Empleado = numeroEmpleado;
                actualizar.Usuario_Propietario_Lista = usuarioId;

                bd.SaveChanges();
                return actualizar.Id;
            }
            else
            {
                lista_final_personas insertar = new lista_final_personas();

                insertar.Id_Personas = persona.Id;
                insertar.Nº_Empleado = numeroEmpleado;
                insertar.Perfil_de_riesgo = persona.Perfil_de_riesgo;
                insertar.Actividad_Funcional = persona.Actividad_Funcional;
                insertar.DNI = persona.DNI;
                insertar.Nombre = persona.Nombre;
                insertar.Usuario_Propietario_Lista = usuarioId;

                bd.lista_final_personas.Add(insertar);
                bd.SaveChanges();
                return insertar.Id;
            }
        }

        #endregion
        #region lista_final
        public static List<lista_final_personas> Listar_ListaFinalPersonas()
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var registro = (from u in bd.lista_final_personas
                            select u).ToList();

            return registro;
        }

        public static List<lista_final_personas> Listar_ListaFinalPersonas_PorIdUsuario(int idUsuario)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var registro = (from u in bd.lista_final_personas
                            where u.Usuario_Propietario_Lista == idUsuario
                            select u).ToList();

            return registro;
        }
        public static bool EliminarPersonaDeListaFinalPorId(int id)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var persona = bd.lista_final_personas.Where(x => x.Id == id).FirstOrDefault();

            if (persona != null)
            {
                bd.lista_final_personas.Remove(persona);
                bd.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool EliminarPersonaDeListaFinalPorNumEmpleado_IdUsuario(string numEmpleado, int idUsario)
        {
            DIMASSTEntities bd = new DIMASSTEntities();

            var persona = (from u in bd.lista_final_personas
                           where u.Nº_Empleado == numEmpleado && u.Usuario_Propietario_Lista == idUsario
                           select u).FirstOrDefault();

            if (persona != null)
            {
                bd.lista_final_personas.Remove(persona);
                bd.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }



        #endregion







    }
}
