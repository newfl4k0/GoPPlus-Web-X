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
    
    public partial class DireccionesAlternativas
    {
        public int ID_DireccionAlternativa { get; set; }
        public int ID_Calle { get; set; }
        public int Numero_Exterior { get; set; }
        public string Numero_Interior { get; set; }
        public Nullable<int> ID_Cliente { get; set; }
        public Nullable<int> ID_ClienteAbonado { get; set; }
        public Nullable<int> ID_ClienteHabitual { get; set; }
        public System.DateTime Fecha_Creacion { get; set; }
    
        public virtual Calles Calles { get; set; }
        public virtual Clientes Clientes { get; set; }
        public virtual ClientesAbonados ClientesAbonados { get; set; }
        public virtual ClientesHabituales ClientesHabituales { get; set; }
    }
}