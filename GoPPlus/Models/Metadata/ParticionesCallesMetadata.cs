using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GoPS.Models
{
    [MetadataType(typeof(ParticionesCallesMetadata))]
    public partial class ParticionesCalles
    {
        public string Direccion
        {
            get
            {
                return Calles.Nombre
                    + ", Col. " + Calles.Colonias.Nombre
                    + ", " + Calles.Colonias.Ciudades.Poblacion
                    + ", " + Calles.Colonias.Ciudades.Estados.Nombre;
            }
        }
    }

    public class ParticionesCallesMetadata
    {
        [ScaffoldColumn(false)]
        public int ID_ParticionCalle { get; set; }
        [Required(ErrorMessage = "El campo Calle es requerido")]
        [DisplayName("Calle")]
        public int ID_Calle { get; set; }
        [Required(ErrorMessage = "El campo Calle es requerido")]
           
        public decimal Latitud { get; set; }
        [Required(ErrorMessage = "El campo Latitud es requerido")]
        
        public decimal Longitud { get; set; }
        [Required(ErrorMessage = "El campo Longitud es requerido")]
        
        public int Numero { get; set; }
        [ScaffoldColumn(false)]
        [Required(ErrorMessage = "El campo Numero es requerido")]
   
        public System.DateTime Fecha_Creacion { get; set; }
        [ScaffoldColumn(false)]
        public Nullable<System.DateTime> Fecha_Actualizacion { get; set; }
        [ScaffoldColumn(false)]
        public string UserID { get; set; }
    }
}