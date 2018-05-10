﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GoPS.Models
{
    [MetadataType(typeof(ColoniasMetadata))]
    public partial class Colonias
    {
    }

    public class ColoniasMetadata
    {
        [ScaffoldColumn(false)]
        public int ID_Colonia { get; set; }
        [Required(ErrorMessage = "El campo Nombre es requerido")]
        [StringLength(50, ErrorMessage = "El campo Nombre debe tener una longitud máxima de 50 caracteres")]
        [DisplayName("Nombre")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El campo Ciudad es requerido")]
        [DisplayName("Ciudad")]
        public int ID_Ciudad { get; set; }
        [Required(ErrorMessage = "El campo Latitud es requerido")]
        [DisplayName("Latitud")]
        public decimal Latitud { get; set; }
        [Required(ErrorMessage = "El campo Longitud es requerido")]
        [DisplayName("Longitud")]
        public decimal Longitud { get; set; }
        [ScaffoldColumn(false)]
        public System.DateTime Fecha_Creacion { get; set; }
        [ScaffoldColumn(false)]
        public Nullable<System.DateTime> Fecha_Actualizacion { get; set; }
        [ScaffoldColumn(false)]
        public string UserID { get; set; }
    }
}