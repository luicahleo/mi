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
    
    public partial class matriz_centro
    {
        public int id { get; set; }
        public int id_tecnologia { get; set; }
        public int id_centro { get; set; }
        public int id_riesgo { get; set; }
        public Nullable<int> id_areanivel1 { get; set; }
        public Nullable<int> id_areanivel2 { get; set; }
        public Nullable<int> id_areanivel3 { get; set; }
        public bool activo { get; set; }
        public Nullable<int> version { get; set; }
        public Nullable<System.DateTime> fechaCreacion { get; set; }
        public Nullable<System.DateTime> fechaModificacion { get; set; }
        public Nullable<int> id_areanivel4 { get; set; }
    }
}
