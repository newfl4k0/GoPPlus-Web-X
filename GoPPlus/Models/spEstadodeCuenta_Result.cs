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
    
    public partial class spEstadodeCuenta_Result
    {
        public int ID_Conductor { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int ID_Calle { get; set; }
        public int Numero_Exterior { get; set; }
        public string Numero_Interior { get; set; }
        public string Telefono { get; set; }
        public string Celular { get; set; }
        public string RFC { get; set; }
        public bool Habilitado { get; set; }
        public System.DateTime Fecha_Nacimiento { get; set; }
        public string NoLicencia { get; set; }
        public int ID_Turno { get; set; }
        public System.DateTime Fecha_Creacion { get; set; }
        public Nullable<System.DateTime> Fecha_Actualizacion { get; set; }
        public string UserID { get; set; }
        public int ID_Flota { get; set; }
        public string UserID_Conductor { get; set; }
        public string Contrasena { get; set; }
        public string Usuario { get; set; }
        public string token { get; set; }
        public string NoTarjeton { get; set; }
        public int Vigencia_Licencia { get; set; }
        public int Vigencia_Tarjeton { get; set; }
        public bool Liquido { get; set; }
        public string Archivo_CartaFianza { get; set; }
        public string Archivo_INE { get; set; }
        public string Archivo_Domicilio { get; set; }
        public string Archivo_Tarjeton { get; set; }
        public string Archivo_Licencia { get; set; }
        public string Archivo_Antidoping { get; set; }
        public string Archivo_NoAntecedentes { get; set; }
    }
}
