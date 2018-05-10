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
    
    public partial class Vehiculos_Conductores
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Vehiculos_Conductores()
        {
            this.Conexiones = new HashSet<Conexiones>();
            this.Despachos = new HashSet<Despachos>();
            this.Seguimientos = new HashSet<Seguimientos>();
        }
    
        public int ID_Vehiculo_Conductor { get; set; }
        public int ID_Vehiculo { get; set; }
        public int ID_Conductor { get; set; }
        public System.DateTime Fecha_Asignacion { get; set; }
        public bool Activo { get; set; }
        public System.DateTime Fecha_Creacion { get; set; }
        public Nullable<System.DateTime> Fecha_Actualizacion { get; set; }
        public string UserID { get; set; }
        public Nullable<int> ID_Estatus { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Conexiones> Conexiones { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Despachos> Despachos { get; set; }
        public virtual Estatus Estatus { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Seguimientos> Seguimientos { get; set; }
        public virtual Vehiculos Vehiculos { get; set; }
        public virtual Conductores Conductores { get; set; }
    }
}