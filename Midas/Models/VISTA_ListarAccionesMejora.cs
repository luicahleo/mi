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
    
    public partial class VISTA_ListarAccionesMejora
    {
        public int id { get; set; }
        public string codigo { get; set; }
        public string tipo { get; set; }
        public string asunto { get; set; }
        public Nullable<System.DateTime> fecha_apertura { get; set; }
        public Nullable<System.DateTime> fecha_cierre { get; set; }
        public string EstadoEscrito { get; set; }
        public int estado { get; set; }
        public string Responsable { get; set; }
        public int idcentral { get; set; }
        public int antecedente { get; set; }
        public Nullable<int> referencia { get; set; }
        public Nullable<int> especifico { get; set; }
        public int ambito { get; set; }
        public int tipoid { get; set; }
        public int contratista { get; set; }
        public string nombre_ambito { get; set; }
        public string descripcion { get; set; }
    }
}