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
    
    public partial class CoordenadasGeocerca
    {
        public int ID_CoordenadaGeocerca { get; set; }
        public decimal Longitud { get; set; }
        public decimal Latitud { get; set; }
        public int ID_Geocerca { get; set; }
    
        public virtual Geocerca Geocerca { get; set; }
    }
}
