using System;
using System.IO;
using System.Web.Mvc;

namespace MIDAS.Helpers
{
     public class EscribirLog
    {
        public string mensajeLog { get; set; }
        public Boolean mostrarConsola { get; set; }

        //Constructor si se pasa el mensaje por parámetro en la creación de la clase
        public EscribirLog(string mensajeEnviar, Boolean mostrarConsola, string controller, string actionResult)
        {
            mensajeLog = mensajeEnviar;
            if (mostrarConsola)
                monstrarMensajeConsola();
            escribirLineaFichero(controller, actionResult);
        }

        //Constructor si se pasa el mensaje por setter tras la creación de la clase
        public EscribirLog(string controller, string actionResult)
        {
            if (mostrarConsola)
                monstrarMensajeConsola();
            escribirLineaFichero(controller, actionResult);
        }

        public void monstrarMensajeConsola()
        {
            //Quitar posibles saltos de línea del mensaje
            mensajeLog = mensajeLog.Replace(Environment.NewLine, " | ");
            mensajeLog = mensajeLog.Replace("\r\n", " | ").Replace("\n", " | ").Replace("\r", " | ");
            Console.WriteLine(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + " " + mensajeLog);
        }

        //Escribe el mensaje de la propiedad mensajeLog en un fichero en la carpeta del ejecutable
        public void escribirLineaFichero(string controller, string actionResult)
        {
            try
            {
                FileStream fs = new FileStream(@AppDomain.CurrentDomain.BaseDirectory +
                    "estado.log", FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter m_streamWriter = new StreamWriter(fs);
                m_streamWriter.BaseStream.Seek(0, SeekOrigin.End);
                //Quitar posibles saltos de línea del mensaje
                mensajeLog = mensajeLog.Replace(Environment.NewLine, " | ");
                mensajeLog = mensajeLog.Replace("\r\n", " | ").Replace("\n", " | ").Replace("\r", " | ");
                m_streamWriter.WriteLine(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + " " + mensajeLog 
                                        + "[Controller: " + controller + ", ActionResult: " + actionResult + "]." );
                m_streamWriter.Flush();
                m_streamWriter.Close();
            }
            catch
            {
                //Silenciosa
            }
        }
    }
}