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
    
    public partial class Rutas
    {
        public int ID_Ruta { get; set; }
        public string Nombre { get; set; }
        public bool Habilitado { get; set; }
        public int Precio { get; set; }
        public int ID_Calle_Origen { get; set; }
        public int Numero_Exterior_Origen { get; set; }
        public string Numero_Interior_Origen { get; set; }
        public int ID_Calle_Destino { get; set; }
        public int Numero_Exterior_Destino { get; set; }
        public string Numero_Interior_Destino { get; set; }
        public int ID_UsuarioAbonado { get; set; }
        public System.DateTime Fecha_Creacion { get; set; }
        public Nullable<System.DateTime> Fecha_Actualizacion { get; set; }
        public string UserID { get; set; }
    
        public virtual Calles Calles { get; set; }
        public virtual Calles Calles1 { get; set; }
        public virtual UsuariosAbonados UsuariosAbonados { get; set; }
    }
}
