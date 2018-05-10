using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GoPS.Models
{
    [MetadataType(typeof(MensajesPredefinidosMetadata))]
    public partial class MensajesPredefinidos
    {
    }

    public class MensajesPredefinidosMetadata
    {
        [ScaffoldColumn(false)]
        public int ID_MensajePredefinido { get; set; }
        [Required(ErrorMessage = "El campo Texto es requerido")]
        [StringLength(50, ErrorMessage = "El campo Texto debe tener una longitud máxima de 50 caracteres")]
        [DisplayName("Texto")]
        public string Texto { get; set; }
        [ScaffoldColumn(false)]
        public System.DateTime Fecha_Creacion { get; set; }
        [ScaffoldColumn(false)]
        public Nullable<System.DateTime> Fecha_Actualizacion { get; set; }
        [ScaffoldColumn(false)]
        public string UserID { get; set; }
    }
}