//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GoPS.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Ubicaciones
    {
        public int ID_Ubicacion { get; set; }
        public System.DateTime Fecha_Ubicacion { get; set; }
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
        public int Estatus { get; set; }
        public int ID_Conexion { get; set; }
    
        public virtual Estatus_Reserva Estatus_Reserva { get; set; }
        public virtual Conexiones Conexiones { get; set; }
    }
}
