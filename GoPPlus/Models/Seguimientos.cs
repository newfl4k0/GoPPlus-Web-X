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
    
    public partial class Seguimientos
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Seguimientos()
        {
            this.CambiosEstatus = new HashSet<CambiosEstatus>();
        }
    
        public int ID_Seguimiento { get; set; }
        public Nullable<int> ID_Vehiculo_Conductor { get; set; }
        public decimal Latitud_Actual { get; set; }
        public decimal Longitud_Actual { get; set; }
        public Nullable<decimal> Latitud_Origen { get; set; }
        public Nullable<decimal> Longitud_Origen { get; set; }
        public Nullable<decimal> Latitud_Destino { get; set; }
        public Nullable<decimal> Longitud_Destino { get; set; }
        public Nullable<int> ID_Estatus_Afiliado { get; set; }
        public System.DateTime Fecha_Creacion { get; set; }
        public Nullable<System.DateTime> Fecha_Actualizacion { get; set; }
        public string UserID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CambiosEstatus> CambiosEstatus { get; set; }
        public virtual Estatus_Afiliados Estatus_Afiliados { get; set; }
        public virtual Vehiculos_Conductores Vehiculos_Conductores { get; set; }
    }
}
