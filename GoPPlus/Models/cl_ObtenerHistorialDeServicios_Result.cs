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
    
    public partial class cl_ObtenerHistorialDeServicios_Result
    {
        public int id { get; set; }
        public string fecha_actualizacion { get; set; }
        public Nullable<decimal> lat_origen { get; set; }
        public Nullable<decimal> lng_origen { get; set; }
        public Nullable<decimal> lat_destino { get; set; }
        public Nullable<decimal> lng_destino { get; set; }
        public string origen { get; set; }
        public string destino { get; set; }
        public string estatus { get; set; }
        public string fecha_domicilio { get; set; }
        public string tipo_vehiculo { get; set; }
        public Nullable<int> id_conductor { get; set; }
        public string conductor { get; set; }
        public decimal monto { get; set; }
        public string fecha_ocupado { get; set; }
        public string fecha_finalizacion { get; set; }
        public int encuesta { get; set; }
        public string fecha_rechazo { get; set; }
        public string placas { get; set; }
        public string color { get; set; }
        public string modelo { get; set; }
        public string marca { get; set; }
        public string ruta { get; set; }
    }
}