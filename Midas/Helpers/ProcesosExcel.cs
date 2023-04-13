using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIDAS.Models;
using System.Drawing;
using System.Net;


namespace MIDAS.Helpers
{
    public class ProcesosExcel
    {
        
           

        public static void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to release the object(object:{0})", obj.ToString());
            }
            finally
            {
                obj = null;
                GC.Collect();
            }
        }

        public static string FiltroNulo(object valor)
        {
            if (valor == null)
                return String.Empty;
            else
                return valor.ToString();
        }

        public static string FiltroNuloExportacion(object valor)
        {
            if (valor == null)
                return "-";
            else
                return valor.ToString();
        }

        public static int FiltroNuloNumerico(string valor)
        {
            if (valor == String.Empty)
                return 0;
            else
                if (IsNumeric(valor))
                    return Convert.ToInt32(valor.ToString());
                else
                    return 0;

        }

        public static Int64 FiltroNuloNumericoLong(string valor)
        {
            if (valor == String.Empty)
                return 0;
            else
                if (IsNumeric(valor))
                    return Convert.ToInt64(valor.ToString());
                else
                    return 0;

        }

        public static int SiyNO(string valor)
        {
            return valor.ToUpper() == "SI" ? 1 : 0;
        }

        public static Boolean IsNumeric(string valor)
        {
            try
            {
                long result;
                result = Convert.ToInt64(valor);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


       

        public static string NullColor(string color)
        {
            if (color == null)
                return "FFFFFF";
            else
                return color;
        }

    }
}
