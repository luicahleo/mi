using MIDAS.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MIDAS.Validations.Configuracion
{
    public class Validation_editar_medida_general
    {
        //public FormCollection formCollection { get; set; }

        public Validation_editar_medida_general()
        {
            //formCollection = collection;
        }

        public List<string> validaFormInsertar(FormCollection collection,
                                                   HttpPostedFileBase seleccionArchivoIcono)
        {
            var apartados = int.Parse(collection["ctl00$MainContent$ddlApartados"]);
            string descripcion = collection["ctl00$MainContent$txtNombre"];
            string chkAmbos = collection["ctl00$MainContent$muestraAmbosChkId"];
            var file = seleccionArchivoIcono;
            List<string> listaErrores = new List<string>();

            //validamos la collection
            if (apartados == 0)
            {
                listaErrores.Add("Error: El campo apartados deber ser rellenado.");
            }
            if (descripcion == null || descripcion == "")
            {
                listaErrores.Add("Error: El campo descripcion deber ser rellenado.");
            }

            //if (file != null && file.ContentLength > 0)
            //{
                //existe la imagen?
                //obtenemos la ruta de la BD
                //string ruta = "../Content/images/medidas/medidasgenerales/" + seleccionArchivoIcono.FileName;
                //var existeRutaImagen = Datos.GetExisteRutaImagenMedidasGenerales(ruta);
                //if (existeRutaImagen)
                //{
                //    listaErrores.Add("Error: Ruta de icono imagen existente!");
                //}

                ////obtenemos la ruta de la carpeta para verificar si existe el fichero
                //string rutaServer = System.Web.Hosting.HostingEnvironment.MapPath("~/");
                //ruta = ruta.Replace("..", "");
                //string rutaFichero = Path.Combine(rutaServer + ruta);
                //if (File.Exists(rutaFichero))
                //{
                //    listaErrores.Add("Error: Fichero de imagen icono existente!");
                //}

                //listaErrores.Add("contenido de Icono");

            //}
            return listaErrores;
        }

        public List<string> validaFormActualizar(FormCollection collection,
                                                   HttpPostedFileBase seleccionArchivoIcono)
        {
            var apartados = int.Parse(collection["ctl00$MainContent$ddlApartados"]);
            string descripcion = collection["ctl00$MainContent$txtNombre"];
            string chkAmbos = collection["ctl00$MainContent$muestraAmbosChkId"];
            var file = seleccionArchivoIcono;
            List<string> listaErrores = new List<string>();

            //validamos la collection
            if (apartados == 0)
            {
                listaErrores.Add("Error: El campo apartados deber ser rellenado.");
            }
            if (descripcion == null || descripcion == "")
            {
                listaErrores.Add("Error: El campo descripcion deber ser rellenado.");
            }

            //if (file != null && file.ContentLength > 0)
            //{
                //existe la imagen?
                //obtenemos la ruta de la BD
                //string ruta = "../Content/images/medidas/medidasgenerales/" + seleccionArchivoIcono.FileName;
                //var existeRutaImagen = Datos.GetExisteRutaImagenMedidasGenerales(ruta);
                //if (existeRutaImagen)
                //{
                //    listaErrores.Add("Error: Ruta de icono imagen existente!");
                //}

                ////obtenemos la ruta de la carpeta para verificar si existe el fichero
                //string rutaServer = System.Web.Hosting.HostingEnvironment.MapPath("~/");
                //ruta = ruta.Replace("..", "");
                //string rutaFichero = Path.Combine(rutaServer + ruta);
                //if (File.Exists(rutaFichero))
                //{
                //    listaErrores.Add("Error: Fichero de imagen icono existente!");
                //}

                //listaErrores.Add("contenido de Icono");

            //}
            return listaErrores;
        }


    }
}