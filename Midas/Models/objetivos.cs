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
    
    public partial class objetivos
    {
        public int id { get; set; }
        public int idorganizacion { get; set; }
        public int Tipo { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public Nullable<System.DateTime> FechaEstimada { get; set; }
        public Nullable<System.DateTime> FechaReal { get; set; }
        public Nullable<int> Responsable { get; set; }
        public int idconsecutivo { get; set; }
        public string Coste { get; set; }
        public string Medios { get; set; }
        public string Seguimiento { get; set; }
        public Nullable<int> especifico { get; set; }
        public Nullable<int> estado { get; set; }
        public string Comentarios { get; set; }
        public Nullable<int> idReferencia { get; set; }
        public Nullable<int> idAspecto { get; set; }
        public string metodomedicion { get; set; }
        public Nullable<int> ambito { get; set; }
        public string personasinv { get; set; }
        public string siglas { get; set; }
    }
}
