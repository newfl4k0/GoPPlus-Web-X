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
    
    public partial class Payment_GetTransaccionCancelar_Result
    {
        public Nullable<int> m { get; set; }
        public int id_transaccion { get; set; }
        public int cliente { get; set; }
        public string referencia { get; set; }
        public string numero_control { get; set; }
        public string codigo_autorizacion { get; set; }
        public Nullable<decimal> monto { get; set; }
        public string nombre { get; set; }
        public string numero { get; set; }
        public string exp { get; set; }
        public string cvv { get; set; }
    }
}