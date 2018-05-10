using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GoPS.Models
{
    [MetadataType(typeof(CiudadesMetadata))]
    public partial class Ciudades
    {
    }

    public class CiudadesMetadata
    {
        [ScaffoldColumn(false)]
        public int ID_Ciudad { get; set; }
        [Required(ErrorMessage = "El campo Población es requerido")]
        [StringLength(50, ErrorMessage = "El campo Población debe tener una longitud máxima de 50 caracteres")]
        [DisplayName("Población")]
        public string Poblacion { get; set; }
        [Required(ErrorMessage = "El campo Estado es requerido")]
        [DisplayName("Estado")]
        public int ID_Estado { get; set; }
        [ScaffoldColumn(false)]
        public System.DateTime Fecha_Creacion { get; set; }
        [ScaffoldColumn(false)]
        public Nullable<System.DateTime> Fecha_Actualizacion { get; set; }
        [ScaffoldColumn(false)]
        public string UserID { get; set; }
    }
}