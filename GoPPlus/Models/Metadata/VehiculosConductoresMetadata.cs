using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GoPS.Models
{
    [MetadataType(typeof(VehiculosConductoresMetadata))]
    public partial class Vehiculos_Conductores
    {
    }

    public class VehiculosConductoresMetadata
    {
        [ScaffoldColumn(false)]
        public int ID_Vehiculo_Conductor { get; set; }
        [Required(ErrorMessage = "El campo Vehiculoo es requerido")]
        [DisplayName("Vehiculo")]
        public int ID_Vehiculo { get; set; }
        [Required(ErrorMessage = "El campo Conductor es requerido")]
        [DisplayName("Conductor")]
        public int ID_Conductor { get; set; }
        [Required(ErrorMessage = "El campo Fecha de Asignación es requerido")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Fecha de Asignación")]
        public System.DateTime Fecha_Asignacion { get; set; }
        [DisplayName("¿Activo?")]
        public bool Activo { get; set; }
        [ScaffoldColumn(false)]
        public System.DateTime Fecha_Creacion { get; set; }
        [ScaffoldColumn(false)]
        public Nullable<System.DateTime> Fecha_Actualizacion { get; set; }
        [ScaffoldColumn(false)]
        public string UserID { get; set; }
    }
}