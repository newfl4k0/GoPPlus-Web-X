using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GoPS.Models
{
    [MetadataType(typeof(TiposSancionesMetadata))]
    public partial class TiposSanciones
    {
    }

    public class TiposSancionesMetadata
    {
        [ScaffoldColumn(false)]
        public int ID_TipoSancion { get; set; }
        [Required(ErrorMessage = "El campo Nombre es requerido")]
        [StringLength(50, ErrorMessage = "El campo Nombre debe tener una longitud máxima de 50 caracteres")]
        [DisplayName("Nombre")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El campo Horas de Penalización es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "El campo Horas de Penalización debe ser mayor a cero")]
        [DisplayName("Horas de Penalización")]
        public int Horas_Penalizacion { get; set; }
        [ScaffoldColumn(false)]
        public System.DateTime Fecha_Creacion { get; set; }
        [ScaffoldColumn(false)]
        public Nullable<System.DateTime> Fecha_Actualizacion { get; set; }
        [ScaffoldColumn(false)]
        public string UserID { get; set; }
    }
}