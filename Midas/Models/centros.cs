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
    
    public partial class centros
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public centros()
        {
            this.areanivel1 = new HashSet<areanivel1>();
        }
    
        public int id { get; set; }
        public string siglas { get; set; }
        public string nombre { get; set; }
        public Nullable<int> tipo { get; set; }
        public Nullable<int> ubicacion { get; set; }
        public Nullable<int> provincia { get; set; }
        public string rutaImagen { get; set; }
        public string direccion { get; set; }
        public string coordenadas { get; set; }
        public string rutaImagenLogo { get; set; }
        public Nullable<int> activo { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<areanivel1> areanivel1 { get; set; }
    }
}