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
    
    public partial class GetServicesByVCId_Result
    {
        public int id { get; set; }
        public string origen { get; set; }
        public string destino { get; set; }
        public string nombre_cliente { get; set; }
        public string fecha_despacho { get; set; }
        public string fecha_finalizado { get; set; }
        public string estatus { get; set; }
        public string es_historial { get; set; }
        public decimal lat_origen { get; set; }
        public decimal lng_origen { get; set; }
        public decimal lat_des { get; set; }
        public decimal lng_des { get; set; }
    }
}
