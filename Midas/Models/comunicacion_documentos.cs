//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MIDAS.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class comunicacion_documentos
    {
        public int id { get; set; }
        public int idcomunicacion { get; set; }
        public string nombre { get; set; }
        public string nombrefichero { get; set; }
        public string enlace { get; set; }
        public Nullable<System.DateTime> fecha { get; set; }
    }
}
