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
    
    public partial class Vehiculos
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Vehiculos()
        {
            this.Vehiculos_Conductores = new HashSet<Vehiculos_Conductores>();
        }
    
        public int ID_Vehiculo { get; set; }
        public string Matricula { get; set; }
        public System.DateTime Fecha_Alta { get; set; }
        public Nullable<System.DateTime> Fecha_Baja { get; set; }
        public string Telefono_Seguro { get; set; }
        public bool Telefono_Seguro_EsCelular { get; set; }
        public string Poliza { get; set; }
        public string Telefono { get; set; }
        public string Serie { get; set; }
        public bool Habilitado { get; set; }
        public string NoLicencia { get; set; }
        public string Version { get; set; }
        public int ID_Flota { get; set; }
        public int ID_Ciudad { get; set; }
        public int ID_Modelo { get; set; }
        public int ID_Seguro { get; set; }
        public int ID_TipoVehiculo { get; set; }
        public System.DateTime Fecha_Creacion { get; set; }
        public Nullable<System.DateTime> Fecha_Actualizacion { get; set; }
        public string UserID { get; set; }
        public int ID_Color { get; set; }
        public string NoTarjeton { get; set; }
        public int RevistaMecanica { get; set; }
        public int KmInicial { get; set; }
        public int KmFinal { get; set; }
        public string NoPermiso { get; set; }
        public int Vigencia_Permiso { get; set; }
        public string NoTransporte { get; set; }
        public bool Externo { get; set; }
    
        public virtual Ciudades Ciudades { get; set; }
        public virtual Flotas Flotas { get; set; }
        public virtual Modelos Modelos { get; set; }
        public virtual Seguros Seguros { get; set; }
        public virtual TiposVehiculos TiposVehiculos { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Vehiculos_Conductores> Vehiculos_Conductores { get; set; }
        public virtual Colores Colores { get; set; }
    }
}
