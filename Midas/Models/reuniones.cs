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
    
    public partial class reuniones
    {
        public int id { get; set; }
        public string cod_reunion { get; set; }
        public Nullable<System.DateTime> fecha_convocatoria { get; set; }
        public string ordendeldia { get; set; }
        public Nullable<System.TimeSpan> horainicio { get; set; }
        public Nullable<System.TimeSpan> horafin { get; set; }
        public string resumen { get; set; }
        public int estado { get; set; }
        public int idcentral { get; set; }
        public int anio { get; set; }
        public int idconsecutivo { get; set; }
        public string personasinv { get; set; }
        public string asunto { get; set; }
    }
}