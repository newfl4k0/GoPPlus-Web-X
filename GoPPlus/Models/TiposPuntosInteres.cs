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
    
    public partial class TiposPuntosInteres
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TiposPuntosInteres()
        {
            this.PuntosInteres = new HashSet<PuntosInteres>();
        }
    
        public int ID_TipoPuntoInteres { get; set; }
        public string Nombre { get; set; }
        public System.DateTime Fecha_Creacion { get; set; }
        public Nullable<System.DateTime> Fecha_Actualizacion { get; set; }
        public string UserID { get; set; }
        public bool Eliminado { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PuntosInteres> PuntosInteres { get; set; }
    }
}
