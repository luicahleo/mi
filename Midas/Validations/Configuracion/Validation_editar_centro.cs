using MIDAS.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MIDAS.Validations.Configuracion
{
    public class Validation_editar_centro
    {
        public Validation_editar_centro()
        {
        }

        public List<string> validaFormInsertar(int id, FormCollection collection, HttpPostedFileBase seleccionArchivoIC, HttpPostedFileBase seleccionArchivoIL)
        {
            var nombre = collection["ctl00$MainContent$txtNombre"];
            var tipo = int.Parse(collection["ctl00$MainContent$ddlTipo"]);
            //var provincia = int.Parse(collection["ctl00$MainContent$ddlProvincia"]);
            var rutaImagen = "../Content/images/centros/";
            var direccion = collection["ctl00$MainContent$direccion"];
            var coordenadas = collection["ctl00$MainContent$coordenadas"];
            var rutaImagenLogo = "../Content/images/centros/logos/";

            List<string> listaErrores = new List<string>();

            //validamos la collection
            if (nombre == null || nombre == "")
            {
                listaErrores.Add("Error: El campo nombre deber ser rellenado");
            }
            if (tipo == 0)
            {
                listaErrores.Add("Error: Debe seleccionar una tecnologia");
            }
            //if (provincia == 0)
            //{
            //    listaErrores.Add("Error: Debe seleccionar una provincia");
            //}
            //if (direccion == null || direccion == "")
            //{
            //    listaErrores.Add("Error: El campo direccion deber ser rellenado");
            //}
            //if (coordenadas == null || coordenadas == "")
            //{
            //    listaErrores.Add("Error: El campo coordenadas deber ser rellenado");
            //}

            //validamos la imagen

            if (seleccionArchivoIC != null && seleccionArchivoIC.ContentLength > 0)
            {
                //obtenemos la ruta de la BD
                //string ruta = rutaImagen + seleccionArchivoIC.FileName;
                //var existeRutaImagen = Datos.ExisteRutaImagenCentro(ruta);
                //if (existeRutaImagen)
                //{
                //    listaErrores.Add("Error: Ruta de imagen centro existente!");
                //}
                //else
                //{
                    //obtenemos la ruta de la carpeta para verificar si existe el fichero
                    //string rutaServer = System.Web.Hosting.HostingEnvironment.MapPath("~/");
                    //ruta = ruta.Replace("..", "");
                    //string rutaFichero = Path.Combine(rutaServer + ruta);
                    //if (File.Exists(rutaFichero))
                    //{
                    //    listaErrores.Add("Error: Fichero de imagen centro existente!");
                    //}
                //}
            }
            else
            {
                listaErrores.Add("Error: Sin contenido en la imagen centro");
            }
            if (seleccionArchivoIL != null && seleccionArchivoIL.ContentLength > 0)
            {
                //DIMASSTEntities bd = new DIMASSTEntities();

                ////obtenemos la ruta de la BD
                //string ruta = rutaImagenLogo + seleccionArchivoIL.FileName;
                //var existeRutaImagen = Datos.ExisteRutaImagenCentroLogo(ruta);
                ////var existeRutaImagen = from rutaLogo in bd.centros where rutaLogo.rutaImagenLogo == ruta select rutaLogo;
                ////centros existeRutaImagen = bd.centros.First(il => il.rutaImagenLogo.Equals(ruta));
                //if (existeRutaImagen)
                //{
                //    listaErrores.Add("Error: Ruta de imagen logo existente!");
                //}
                //else
                //{
                //    //obtenemos la ruta de la carpeta para verificar si existe el fichero
                //    string rutaServer = System.Web.Hosting.HostingEnvironment.MapPath("~/");
                //    ruta = ruta.Replace("..", "");
                //    string rutaFichero = Path.Combine(rutaServer + ruta);
                //    if (File.Exists(rutaFichero))
                //    {
                //        listaErrores.Add("Error: Fichero de imagen logo existente!");
                //    }
                //}
            }
            else
            {
                listaErrores.Add("Error: Sin contenido en la imagen logo");
            }

            return listaErrores;
        }

        public List<string> validaFormActualizar(int id, FormCollection collection, HttpPostedFileBase seleccionArchivoIC, HttpPostedFileBase seleccionArchivoIL, string chkIC, string chkILL)
        {
            var nombre = collection["ctl00$MainContent$txtNombre"];
            var tipo = int.Parse(collection["ctl00$MainContent$ddlTipo"]);
            //var provincia = int.Parse(collection["ctl00$MainContent$ddlProvincia"]);
            var rutaImagen = "../Content/images/centros/";
            var direccion = collection["ctl00$MainContent$direccion"];
            var coordenadas = collection["ctl00$MainContent$coordenadas"];
            var rutaImagenLogo = "../Content/images/centros/logos/";

            List<string> listaErrores = new List<string>();

            //validamos la collection
            if (nombre == null || nombre == "")
            {
                listaErrores.Add("Error: El campo nombre deber ser rellenado");
            }
            if (tipo == 0)
            {
                listaErrores.Add("Error: Debe seleccionar una tecnologia");
            }
            //if (provincia == 0)
            //{
            //    listaErrores.Add("Error: Debe seleccionar una provincia");
            //}
            //if (direccion == null || direccion == "")
            //{
            //    listaErrores.Add("Error: El campo direccion deber ser rellenado");
            //}
            //if (coordenadas == null || coordenadas == "")
            //{
            //    listaErrores.Add("Error: El campo coordenadas deber ser rellenado");
            //}

            //validamos la imagen
            //if (chkIC != null)
            //{
            //    if (seleccionArchivoIC != null && seleccionArchivoIC.ContentLength > 0)
            //    {
            //        //obtenemos la ruta de la BD
            //        string ruta = rutaImagen + seleccionArchivoIC.FileName;
            //        var existeRutaImagen = Datos.ExisteRutaImagenCentro(ruta);
            //        if (existeRutaImagen)
            //        {
            //            listaErrores.Add("Error: Ruta de imagen centro existente!");
            //        }
            //        else
            //        {
            //            //obtenemos la ruta de la carpeta para verificar si existe el fichero
            //            string rutaServer = System.Web.Hosting.HostingEnvironment.MapPath("~/");
            //            ruta = ruta.Replace("..", "");
            //            string rutaFichero = Path.Combine(rutaServer + ruta);
            //            if (File.Exists(rutaFichero))
            //            {
            //                listaErrores.Add("Error: Fichero de imagen centro existente!");
            //            }
            //        }
            //    }
            //    else
            //    {
            //        listaErrores.Add("Error: Sin contenido en la imagen centro");
            //    }
            //}
            //if (chkILL != null)
            //{
            //    if (seleccionArchivoIL != null && seleccionArchivoIL.ContentLength > 0)
            //    {
            //        DIMASSTEntities bd = new DIMASSTEntities();

            //        //obtenemos la ruta de la BD
            //        string ruta = rutaImagenLogo + seleccionArchivoIL.FileName;
            //        var existeRutaImagen = Datos.ExisteRutaImagenCentroLogo(ruta);
            //        //var existeRutaImagen = from rutaLogo in bd.centros where rutaLogo.rutaImagenLogo == ruta select rutaLogo;
            //        //centros existeRutaImagen = bd.centros.First(il => il.rutaImagenLogo.Equals(ruta));
            //        if (existeRutaImagen)
            //        {
            //            listaErrores.Add("Error: Ruta de imagen logo existente!");
            //        }
            //        else
            //        {
            //            //obtenemos la ruta de la carpeta para verificar si existe el fichero
            //            string rutaServer = System.Web.Hosting.HostingEnvironment.MapPath("~/");
            //            ruta = ruta.Replace("..", "");
            //            string rutaFichero = Path.Combine(rutaServer + ruta);
            //            if (File.Exists(rutaFichero))
            //            {
            //                listaErrores.Add("Error: Fichero de imagen logo existente!");
            //            }
            //        }
            //    }
            //    else
            //    {
            //        listaErrores.Add("Error: Sin contenido en la imagen logo");
            //    }
            //}

            return listaErrores;
        }

    }
}