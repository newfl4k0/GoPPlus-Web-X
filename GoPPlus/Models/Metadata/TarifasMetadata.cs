using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GoPS.Models
{
    [MetadataType(typeof(TarifasMetadata))]
    public partial class Tarifas
    {
    }

    public partial class TarifasMetadata
    {
        [ScaffoldColumn(false)]
        public int ID_Tarifa { get; set; }
        [Required(ErrorMessage = "El campo Nombre es requerido")]
        [StringLength(50, ErrorMessage = "El campo Nombre debe tener una longitud máxima de 50 caracteres")]
        [DisplayName("Nombre")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El campo Descripción es requerido")]
        [StringLength(500, ErrorMessage = "El campo Descripción debe tener una longitud máxima de 500 caracteres")]
        [DataType(DataType.MultilineText)]
        [DisplayName("Descripción")]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "El campo Tarifa Mínima es requerido")]
        [RegularExpression(@"-?(?:\d*[\,\.])?\d+", ErrorMessage = "El valor del campo Tarifa Mínima es inválido")]
        [Range(typeof(decimal), "0.01", "79228162514264337593543950335", ErrorMessage = "El valor del campo Tarifa Mínima debe ser mayor a 0")]
        [DataType(DataType.Currency)]
        [DisplayName("Tarifa Mínima")]
        public decimal Tarifa_Minima { get; set; }
        [Required(ErrorMessage = "El campo Precio Base es requerido")]
        [DataType(DataType.Currency)]
        [RegularExpression(@"-?(?:\d*[\,\.])?\d+", ErrorMessage = "El valor del campo Precio Base es inválido")]
        [Range(typeof(decimal), "0.01", "79228162514264337593543950335", ErrorMessage = "El valor del campo Precio Base debe ser mayor a 0")]
        [DisplayName("Precio Base")]
        public decimal Precio_Base { get; set; }
        [Required(ErrorMessage = "El campo Precio por Kilómetro es requerido")]
        [RegularExpression(@"-?(?:\d*[\,\.])?\d+", ErrorMessage = "El valor del campo Precio por Kilómetro es inválido")]
        [Range(typeof(decimal), "0.01", "79228162514264337593543950335", ErrorMessage = "El valor del campo Precio por Kilómetro debe ser mayor a 0")]
        [DataType(DataType.Currency)]
        [DisplayName("Precio por Kilómetro")]
        public decimal Precio_Km { get; set; }
        [Required(ErrorMessage = "El campo Precio por Minuto es requerido")]
        [RegularExpression(@"-?(?:\d*[\,\.])?\d+", ErrorMessage = "El valor del campo Precio por Minuto es inválido")]
        [Range(typeof(decimal), "0.01", "79228162514264337593543950335", ErrorMessage = "El valor del campo Precio por Minuto debe ser mayor a 0")]
        [DataType(DataType.Currency)]
        [DisplayName("Precio por Minuto")]
        public decimal Precio_Min { get; set; }
        [DisplayName("¿Activa?")]
        public bool Activa { get; set; }
        [Required(ErrorMessage = "El campo Afiliado es requerido")]
        [DisplayName("Afiliado")]
        public int ID_Afiliado { get; set; }
        [ScaffoldColumn(false)]
        public System.DateTime Fecha_Creacion { get; set; }
        [ScaffoldColumn(false)]
        public Nullable<System.DateTime> Fecha_Actualizacion { get; set; }
        [ScaffoldColumn(false)]
        public string UserID { get; set; }
    }
}