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
    
    public partial class emergencias
    {
        public int id { get; set; }
        public string codigo { get; set; }
        public string descripcion { get; set; }
        public Nullable<System.DateTime> fechaplanificada { get; set; }
        public Nullable<System.DateTime> fecharealizacion { get; set; }
        public string personalimplicado { get; set; }
        public int responsable { get; set; }
        public string mediosempleados { get; set; }
        public string escenarioplanteado { get; set; }
        public string objetivos { get; set; }
        public string informe { get; set; }
        public string conclusiones { get; set; }
        public int idcentral { get; set; }
        public int anio { get; set; }
        public int idconsecutivo { get; set; }
    }
}
