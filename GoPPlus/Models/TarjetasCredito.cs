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
    
    public partial class TarjetasCredito
    {
        public int ID_TarjetaCredito { get; set; }
        public string Nombre { get; set; }
        public System.DateTime Fecha_Creacion { get; set; }
        public Nullable<System.DateTime> Fecha_Actualizacion { get; set; }
        public string UserID { get; set; }
        public Nullable<int> ID_Cliente { get; set; }
        public string Numero { get; set; }
        public string Exp { get; set; }
        public string Cvv { get; set; }
        public Nullable<int> Validado { get; set; }
        public string Control { get; set; }
    }
}
