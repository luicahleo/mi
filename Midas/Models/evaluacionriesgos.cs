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
    
    public partial class evaluacionriesgos
    {
        public int id { get; set; }
        public string codigo { get; set; }
        public string titulo { get; set; }
        public int tipodoc { get; set; }
        public Nullable<System.DateTime> fechapub { get; set; }
        public int elaboradopor { get; set; }
        public string empresa { get; set; }
        public Nullable<int> idcentral { get; set; }
        public string enlace { get; set; }
        public int descargas { get; set; }
    }
}
