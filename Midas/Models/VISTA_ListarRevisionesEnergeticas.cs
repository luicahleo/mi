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
    
    public partial class VISTA_ListarRevisionesEnergeticas
    {
        public int id { get; set; }
        public string codigo { get; set; }
        public Nullable<System.DateTime> fechaplanificacion { get; set; }
        public string planificacionenergetica { get; set; }
        public Nullable<System.DateTime> fecharevision { get; set; }
        public string revisionenergetica { get; set; }
        public string nombre { get; set; }
        public string conclusiones { get; set; }
        public int idcentral { get; set; }
        public string personasinv { get; set; }
    }
}
