﻿using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GoPS.Models
{
    [MetadataType(typeof(FrecuenciasPagoMetadata))]
    public partial class FrecuenciasPago
    {
    }

    public class FrecuenciasPagoMetadata
    {
        [ScaffoldColumn(false)]
        public int ID_FrecuenciaPago { get; set; }
        [Required(ErrorMessage = "El campo Nombre es requerido")]
        [StringLength(50, ErrorMessage = "El campo Nombre debe tener una longitud máxima de 50 caracteres")]
        [DisplayName("Nombre")]
        public string Nombre { get; set; }
        [ScaffoldColumn(false)]
        public DateTime Fecha_Creacion { get; set; }
        [ScaffoldColumn(false)]
        public Nullable<System.DateTime> Fecha_Actualizacion { get; set; }
        [ScaffoldColumn(false)]
        public string UserID { get; set; }
    }
}