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
    
    public partial class VISTA_Indicadores
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string MetodoMedicion { get; set; }
        public string Unidad { get; set; }
        public Nullable<int> ProcesoAsociado { get; set; }
        public string Activo { get; set; }
        public string NombreProceso { get; set; }
        public string NombreTecnologia { get; set; }
        public Nullable<int> tecnologia { get; set; }
        public string listadoCentrales { get; set; }
    }
}