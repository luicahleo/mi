using MIDAS.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MIDAS.Validations.Configuracion
{
    public class Validation_editar_medida_preventiva
    {
        //public FormCollection formCollection { get; set; }

        public Validation_editar_medida_preventiva()
        {
            //formCollection = collection;
        }

        public List<string> validaFormInsertar(FormCollection collection,
                                                   HttpPostedFileBase seleccionArchivoIC, 
                                                   HttpPostedFileBase seleccionArchivoIG)
        {
            var situacion = int.Parse(collection["ctl00$MainContent$ddlSituaciones"]);
            string descripcion = collection["ctl00$MainContent$txtNombre"];
            var file = seleccionArchivoIC;
            List<string> listaErrores = new List<string>();

            //validamos la collection
            if (situacion == 0)
            {
                listaErrores.Add("Error: El campo situacion deber ser rellenado.");
            }
            if (descripcion == null || descripcion == "")
            {
                listaErrores.Add("Error: El campo descripcion deber ser rellenado.");
            }
            if (seleccionArchivoIC != null && seleccionArchivoIG != null)
            {
                listaErrores.Add("Error: No se debe agregar Icono e Imagen Grande a la vez.");
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
                                                   HttpPostedFileBase seleccionArchivoIC,
                                                   HttpPostedFileBase seleccionArchivoIG)
        {
            var situacion = int.Parse(collection["ctl00$MainContent$ddlSituaciones"]);
            string descripcion = collection["ctl00$MainContent$txtNombre"];
            var file = seleccionArchivoIC;
            List<string> listaErrores = new List<string>();

            //validamos la collection
            if (situacion == 0)
            {
                listaErrores.Add("Error: El campo situacion deber ser rellenado.");
            }
            if (descripcion == null || descripcion == "")
            {
                listaErrores.Add("Error: El campo descripcion deber ser rellenado.");
            }
            if (seleccionArchivoIC != null && seleccionArchivoIG != null)
            {
                listaErrores.Add("Error: No se debe agregar Icono e Imagen Grande a la vez.");
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