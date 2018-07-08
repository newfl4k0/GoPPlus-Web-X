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
    
    public partial class Afiliados
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Afiliados()
        {
            this.Clientes = new HashSet<Clientes>();
            this.ClientesAbonados = new HashSet<ClientesAbonados>();
            this.ClientesHabituales = new HashSet<ClientesHabituales>();
            this.Cupones = new HashSet<Cupones>();
            this.DatosTickets = new HashSet<DatosTickets>();
            this.Estatus_Afiliados = new HashSet<Estatus_Afiliados>();
            this.Flotas = new HashSet<Flotas>();
            this.Geocerca = new HashSet<Geocerca>();
            this.PuntosInteres = new HashSet<PuntosInteres>();
            this.ServiciosAuxiliares = new HashSet<ServiciosAuxiliares>();
            this.Tarifas = new HashSet<Tarifas>();
            this.Turnos = new HashSet<Turnos>();
            this.AspNetUserRoles = new HashSet<AspNetUserRoles>();
            this.Operadores = new HashSet<Operadores>();
            this.TiposVehiculos_Afiliados = new HashSet<TiposVehiculos_Afiliados>();
            this.RutasEstatus1 = new HashSet<RutasEstatus1>();
        }
    
        public int ID_Afiliado { get; set; }
        public string Nombre { get; set; }
        public string RFC { get; set; }
        public int ID_Calle { get; set; }
        public int Numero_Exterior { get; set; }
        public string Numero_Interior { get; set; }
        public int Codigo_Postal { get; set; }
        public int ID_TipoServicio { get; set; }
        public System.DateTime Fecha_Creacion { get; set; }
        public Nullable<System.DateTime> Fecha_Actualizacion { get; set; }
        public string UserID { get; set; }
        public int ID_Empresa { get; set; }
        public int Horas_Conductor { get; set; }
        public Nullable<decimal> Cuota_Conductor { get; set; }
        public Nullable<decimal> Porcentaje_Conductor { get; set; }
        public int ID_TipoPago { get; set; }
        public int ID_FrecuenciaPago { get; set; }
    
        public virtual Calles Calles { get; set; }
        public virtual TiposServicios TiposServicios { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Clientes> Clientes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ClientesAbonados> ClientesAbonados { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ClientesHabituales> ClientesHabituales { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cupones> Cupones { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DatosTickets> DatosTickets { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Estatus_Afiliados> Estatus_Afiliados { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Flotas> Flotas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Geocerca> Geocerca { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PuntosInteres> PuntosInteres { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ServiciosAuxiliares> ServiciosAuxiliares { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tarifas> Tarifas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Turnos> Turnos { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual Empresas Empresas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Operadores> Operadores { get; set; }
        public virtual FrecuenciasPago FrecuenciasPago { get; set; }
        public virtual TiposPagos TiposPagos { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TiposVehiculos_Afiliados> TiposVehiculos_Afiliados { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RutasEstatus1> RutasEstatus1 { get; set; }
    }
}
