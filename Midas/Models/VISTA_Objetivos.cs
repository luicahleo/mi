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
    
    public partial class VISTA_Objetivos
    {
        public int id { get; set; }
        public int idorganizacion { get; set; }
        public int Tipo { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public Nullable<System.DateTime> FechaEstimada { get; set; }
        public Nullable<System.DateTime> FechaReal { get; set; }
        public string Responsable { get; set; }
        public int idconsecutivo { get; set; }
        public string Coste { get; set; }
        public string Medios { get; set; }
        public string Seguimiento { get; set; }
        public Nullable<int> especifico { get; set; }
        public Nullable<int> estado { get; set; }
        public string Comentarios { get; set; }
        public Nullable<int> idResponsable { get; set; }
        public string nombre_ambito { get; set; }
        public Nullable<int> idUnidad { get; set; }
        public string siglas { get; set; }
    }
}
