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
    
    public partial class ch_AplicarDescuento_Result
    {
        public Nullable<int> status { get; set; }
        public string message { get; set; }
        public string motivo_descuento { get; set; }
        public Nullable<double> descuento_codigo { get; set; }
        public decimal creditos_cliente { get; set; }
        public Nullable<int> id_usuario { get; set; }
        public decimal nuevo_abono_usuario { get; set; }
    }
}