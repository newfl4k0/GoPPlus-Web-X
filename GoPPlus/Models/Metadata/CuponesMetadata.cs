using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GoPS.Models
{
    [MetadataType(typeof(CuponesMetadata))]
    public partial class Cupones
    {

    }

    public class CuponesMetadata
    {
        [ScaffoldColumn(false)]
        public int ID_Cupon { get; set; }
        [Required(ErrorMessage = "El campo Descripción es requerido")]
        [StringLength(100, ErrorMessage = "El campo Descripción debe tener una longitud máxima de 100 caracteres")]
        [DataType(DataType.MultilineText)]
        [DisplayName("Descripción")]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "El campo Clave Automática es requerido")]
        [StringLength(10, ErrorMessage = "El campo Clave Automática debe tener una longitud máxima de 10 caracteres")]
        [DisplayName("Clave Automática")]
        public string Codigo { get; set; }
        [Required(ErrorMessage = "El campo No. de Cupones es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "El campo No. de Cupones debe ser mayor a cero, número entero y no mayor a 2,147,483,647.")]
        [DisplayName("No. de Cupones")]
        public int Cantidad { get; set; }
        [Required(ErrorMessage = "El campo Fecha de Inicio es requerido")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Fecha de Inicio")]
        public System.DateTime Fecha_Inicio { get; set; }
        [Required(ErrorMessage = "El campo Fecha de Finalización es requerido")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Fecha de Finalización")]
        public System.DateTime Fecha_Fin { get; set; }


        //[Range(typeof(int), "1", "100", ErrorMessage = "El valor del campo Descuento debe ser un número entero mayor a 0 y menor o igual a 100.")]
        [Required(ErrorMessage = "El campo Descuento es requerido")]
        [DisplayName("Descuento")]
        public decimal Descuento { get; set; }
        [DisplayName("¿Primer Servicio?")]
        public bool Primer_Servicio { get; set; }
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