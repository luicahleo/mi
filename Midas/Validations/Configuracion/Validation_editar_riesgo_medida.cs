using MIDAS.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace MIDAS.Validations.Configuracion
{
    public class Validation_editar_riesgo_medida
    {
        public Validation_editar_riesgo_medida()
        {
        }

        public List<string> validaFormInsertar(FormCollection collection, HttpPostedFileBase seleccionArchivoIC, HttpPostedFileBase seleccionArchivoIG)
        {
            var apartados = int.Parse(collection["ctl00$MainContent$ddlApartados"]);
            var riesgos = int.Parse(collection["ctl00$MainContent$ddlRiesgos"]);
            string descripcion = collection["ctl00$MainContent$txtNombre"];

            List<string> listaErrores = new List<string>();

            //validamos la collection
            if (apartados == 0)
            {
                listaErrores.Add("Error: El campo apartados deber ser rellenado.");
            }
            if (riesgos == 0)
            {
                listaErrores.Add("Error: El campo riesgos deber ser rellenado.");
            }
            if (descripcion == null || descripcion == "")
            {
                listaErrores.Add("Error: El campo descripcion deber ser rellenado.");
            }
            if(seleccionArchivoIC != null && seleccionArchivoIG != null)
            {
                listaErrores.Add("Error: No se debe agregar Icono e Imagen Grande a la vez.");
            }

            //existe la imagen?
            //if (seleccionArchivoIC != null && seleccionArchivoIC.ContentLength > 0)
            //{
            //    //obtenemos la ruta de la BD
            //    string ruta = "../Content/images/medidas/" + seleccionArchivoIC.FileName;
            //    var existeRutaImagen = Datos.GetExisteRutaImagenMedidasRiesgoIcono(ruta);
            //    if (existeRutaImagen)
            //    {
            //        listaErrores.Add("Error: Ruta de icono imagen existente!");
            //    }

            //    //obtenemos la ruta de la carpeta para verificar si existe el fichero
            //    string rutaServer = System.Web.Hosting.HostingEnvironment.MapPath("~/");
            //    ruta = ruta.Replace("..", "");
            //    string rutaFichero = Path.Combine(rutaServer + ruta);
            //    if (File.Exists(rutaFichero))
            //    {
            //        listaErrores.Add("Error: Fichero de imagen icono existente!");
            //    }
            //}
            //if (seleccionArchivoIG != null && seleccionArchivoIG.ContentLength > 0)
            //{
            //    //obtenemos la ruta de la BD
            //    string ruta = "../Content/images/medidas/" + seleccionArchivoIG.FileName;
            //    var existeRutaImagen = Datos.GetExisteRutaImagenMedidasRiesgoIG(ruta);
            //    if (existeRutaImagen)cualqui
            //    {
            //        listaErrores.Add("Error: Ruta de imagen grande existente!");
            //    }

            //    //obtenemos la ruta de la carpeta para verificar si existe el fichero
            //    string rutaServer = System.Web.Hosting.HostingEnvironment.MapPath("~/");
            //    ruta = ruta.Replace("..", "");
            //    string rutaFichero = Path.Combine(rutaServer + ruta);
            //    if (File.Exists(rutaFichero))
            //    {
            //        listaErrores.Add("Error: Fichero de imagen grande existente!");
            //    }
            //}


            return listaErrores;
        }

        public List<string> validaFormActualizar(FormCollection collection, HttpPostedFileBase seleccionArchivoIC, HttpPostedFileBase seleccionArchivoIG)
        {
            var apartados = int.Parse(collection["ctl00$MainContent$ddlApartados"]);
            var riesgos = int.Parse(collection["ctl00$MainContent$ddlRiesgos"]);
            string descripcion = collection["ctl00$MainContent$txtNombre"];

            List<string> listaErrores = new List<string>();

            if (apartados == 0)
            {
                listaErrores.Add("Error: El campo apartados deber ser rellenado.");
            }
            if (riesgos == 0)
            {
                listaErrores.Add("Error: El campo riesgos deber ser rellenado.");
            }
            if (descripcion == null || descripcion == "")
            {
                listaErrores.Add("Error: El campo descripcion deber ser rellenado.");
            }
            if (seleccionArchivoIC != null && seleccionArchivoIG != null)
            {
                listaErrores.Add("Error: No se debe agregar Icono e Imagen Grande a la vez.");
            }
            //existe la imagen?
            //if (seleccionArchivoIC != null && seleccionArchivoIC.ContentLength > 0)
            //{
            //    //obtenemos la ruta de la BD
            //    string ruta = "../Content/images/medidas/" + seleccionArchivoIC.FileName;
            //    var existeRutaImagen = Datos.GetExisteRutaImagenMedidasRiesgoIcono(ruta);
            //    if (existeRutaImagen)
            //    {
            //        listaErrores.Add("Error: Ruta de icono imagen existente!");
            //    }

            //    //obtenemos la ruta de la carpeta para verificar si existe el fichero
            //    string rutaServer = System.Web.Hosting.HostingEnvironment.MapPath("~/");
            //    ruta = ruta.Replace("..", "");
            //    string rutaFichero = Path.Combine(rutaServer + ruta);
            //    if (File.Exists(rutaFichero))
            //    {
            //        listaErrores.Add("Error: Fichero de imagen icono existente!");
            //    }
            //}
            //else
            //{
            //    listaErrores.Add("Error: Sin contenido en el icono");
            //}

            //existe la imagen?
            //if (seleccionArchivoIG != null && seleccionArchivoIG.ContentLength > 0)
            //{
            //    //obtenemos la ruta de la BD
            //    string ruta = "../Content/images/medidas/" + seleccionArchivoIG.FileName;
            //    var existeRutaImagen = Datos.GetExisteRutaImagenMedidasRiesgoIG(ruta);
            //    if (existeRutaImagen)
            //    {
            //        listaErrores.Add("Error: Ruta de imagen grande existente!");
            //    }

            //    //obtenemos la ruta de la carpeta para verificar si existe el fichero
            //    string rutaServer = System.Web.Hosting.HostingEnvironment.MapPath("~/");
            //    ruta = ruta.Replace("..", "");
            //    string rutaFichero = Path.Combine(rutaServer + ruta);
            //    if (File.Exists(rutaFichero))
            //    {
            //        listaErrores.Add("Error: Fichero de imagen grande existente!");
            //    }
            //}
            //else
            //{
            //    listaErrores.Add("Error: Sin contenido en imagen grande");
            //}

            return listaErrores;
        }

    }
}