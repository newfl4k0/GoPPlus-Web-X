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
    
    public partial class Llamadas
    {
        public int ID_Llamada { get; set; }
        public string Numero { get; set; }
        public int Tiempo { get; set; }
        public System.DateTime Fecha { get; set; }
        public int ID_Operador { get; set; }
        public System.DateTime Fecha_Creacion { get; set; }
        public Nullable<System.DateTime> Fecha_Actualizacion { get; set; }
        public string UserID { get; set; }
        public bool Eliminado { get; set; }
    
        public virtual Operadores Operadores { get; set; }
    }
}
