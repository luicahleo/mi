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
    
    public partial class riesgos_situaciones
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public riesgos_situaciones()
        {
            this.medidas_preventivas = new HashSet<medidas_preventivas>();
        }
    
        public int id { get; set; }
        public string codigo { get; set; }
        public string descripcion { get; set; }
        public int id_tipo_riesgo { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<medidas_preventivas> medidas_preventivas { get; set; }
        public virtual tipos_riesgos tipos_riesgos { get; set; }
    }
}
