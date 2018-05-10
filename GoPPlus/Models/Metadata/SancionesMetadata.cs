using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GoPS.Models
{
    [MetadataType(typeof(SancionesMetadata))]
    public partial class Sanciones
    {
        public string Nombre_Operador_Alta
        {
            get
            {
                return Operadores != null ? Operadores.Nombre : "";
            }
        }
    }

    public class SancionesMetadata
    {
        [ScaffoldColumn(false)]
        public int ID_Sancion { get; set; }
        [Required(ErrorMessage = "El campo Fecha de Inicio es requerido")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        [DisplayName("Fecha de Inicio")]
        public System.DateTime Fecha_Inicio { get; set; }
        [Required(ErrorMessage = "El campo Fecha de Finalización es requerido")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        [DisplayName("Fecha de Finalización")]
        public System.DateTime Fecha_Fin { get; set; }
        [Required(ErrorMessage = "El campo Operador de Alta es requerido")]
        [DisplayName("Operador de Alta")]
        public int ID_Operador_Alta { get; set; }
        [DisplayFormat(NullDisplayText = "No especificado")]
        [DisplayName("Operador de Baja")]
        public Nullable<int> ID_Operador_Baja { get; set; }
        [StringLength(500, ErrorMessage = "El campo Observaciones debe tener una longitud máxima de 500 caracteres")]
        [DataType(DataType.MultilineText)]
        [DisplayName("Observaciones")]
        public string Observaciones { get; set; }
        [Required(ErrorMessage = "El campo Tipo de Sanción es requerido")]
        [DisplayName("Tipo de Sanción")]
        public int ID_TipoSancion { get; set; }
        [Required(ErrorMessage = "El campo Conductor es requerido")]
        [DisplayName("Conductor")]
        public int ID_Conductor { get; set; }
        [ScaffoldColumn(false)]
        public System.DateTime Fecha_Creacion { get; set; }
        [ScaffoldColumn(false)]
        public Nullable<System.DateTime> Fecha_Actualizacion { get; set; }
        [ScaffoldColumn(false)]
        public string UserID { get; set; }
    }
}