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
    
    public partial class documento_historico
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string tipo { get; set; }
        public string version { get; set; }
        public Nullable<int> estado { get; set; }
        public string fechaUltimaModificacion { get; set; }
        public string usuario { get; set; }
        public Nullable<int> descarga { get; set; }
        public string ruta { get; set; }
        public Nullable<int> id_centro { get; set; }
        public Nullable<int> revision { get; set; }
        public string descripcion { get; set; }
    }
}
