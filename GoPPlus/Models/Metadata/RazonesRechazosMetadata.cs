using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GoPS.Models
{
    [MetadataType(typeof(RazonesRechazosMetadata))]
    public partial class RazonesRechazos
    {
    }

    public class RazonesRechazosMetadata
    {
        [ScaffoldColumn(false)]
        public int ID_RazonRechazo { get; set; }
        [Required(ErrorMessage = "El campo Nombre es requerido")]
        [StringLength(50, ErrorMessage = "El campo Nombre debe tener una longitud máxima de 50 caracteres")]
        [DisplayName("Nombre")]
        public string Nombre { get; set; }
        [DisplayName("¿Activo?")]
        public bool Activo { get; set; }
        [Required(ErrorMessage = "El campo Tipo de Rechazo es requerido")]
        [DisplayName("Tipo de Rechazo")]
        public int ID_TipoRechazo { get; set; }
        [ScaffoldColumn(false)]
        public System.DateTime Fecha_Creacion { get; set; }
        [ScaffoldColumn(false)]
        public Nullable<System.DateTime> Fecha_Actualizacion { get; set; }
        [ScaffoldColumn(false)]
        public string UserID { get; set; }
    }
}