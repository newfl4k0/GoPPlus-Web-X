using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GoPS.Models
{
    [MetadataType(typeof(EstatusReservasMetadata))]
    public partial class Estatus_Reserva
    {
        
    }

    public class EstatusReservasMetadata
    {
        [ScaffoldColumn(false)]
        public int ID_Estatus_Reserva { get; set; }
        [Required(ErrorMessage = "El campo Nombre es requerido")]
        [StringLength(20, ErrorMessage = "El campo Nombre debe tener una longitud máxima de 20 caracteres")]
        [DisplayName("Nombre")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El campo Descripción es requerido")]
        [StringLength(100, ErrorMessage = "El campo Descripción debe tener una longitud máxima de 100 caracteres")]
        [DataType(DataType.MultilineText)]
        [DisplayName("Descripción")]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "El campo Orden es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "El campo Orden debe ser mayor a cero")]
        [DisplayName("Orden")]
        public int Orden { get; set; }
        [ScaffoldColumn(false)]
        public System.DateTime Fecha_Creacion { get; set; }
        [ScaffoldColumn(false)]
        public Nullable<System.DateTime> Fecha_Actualizacion { get; set; }
        [ScaffoldColumn(false)]
        public string UserID { get; set; }
    }
}