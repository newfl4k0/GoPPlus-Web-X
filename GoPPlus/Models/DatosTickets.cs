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
    
    public partial class DatosTickets
    {
        public int ID_DatoTicket { get; set; }
        public string Texto_1 { get; set; }
        public string Texto_2 { get; set; }
        public string Texto_3 { get; set; }
        public string Texto_4 { get; set; }
        public string Texto_5 { get; set; }
        public string Texto_6 { get; set; }
        public string Texto_7 { get; set; }
        public string Texto_8 { get; set; }
        public string Texto_9 { get; set; }
        public string Texto_10 { get; set; }
        public string Imagen { get; set; }
        public bool Fecha_Bordo { get; set; }
        public bool Fecha_Finalizacion { get; set; }
        public bool No_Taxi { get; set; }
        public bool Placas { get; set; }
        public bool Nombre_Cliente { get; set; }
        public bool Origen { get; set; }
        public bool Destino { get; set; }
        public bool Importe { get; set; }
        public bool Observaciones { get; set; }
        public bool Despacho { get; set; }
        public bool Codigo { get; set; }
        public int ID_Afiliado { get; set; }
        public System.DateTime Fecha_Creacion { get; set; }
        public Nullable<System.DateTime> Fecha_Actualizacion { get; set; }
        public string UserID { get; set; }
    
        public virtual Afiliados Afiliados { get; set; }
    }
}