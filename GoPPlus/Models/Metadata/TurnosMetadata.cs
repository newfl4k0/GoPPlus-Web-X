using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoPS.Models
{
    [MetadataType(typeof(TurnosMetadata))]
    public partial class Turnos
    {
        public string Hora_I_Format
        {
            get
            {
                return DateTime.Today.Add(new TimeSpan(Hora_Inicio.Hours, Hora_Inicio.Minutes, 00)).ToString("hh:mm tt");
            }
        }

        public string Hora_F_Format
        {
            get
            {
                return DateTime.Today.Add(new TimeSpan(Hora_Fin.Hours, Hora_Fin.Minutes, 00)).ToString("hh:mm tt");
            }
        }

        public List<CheckBoxListItem> diasList { get; set; }
    }

    public class TurnosMetadata
    {
        [ScaffoldColumn(false)]
        public int ID_Turno { get; set; }
        [Required(ErrorMessage = "El campo Nombre es requerido")]
        [StringLength(50, ErrorMessage = "El campo Nombre debe tener una longitud máxima de 50 caracteres")]
        [DisplayName("Nombre")]
        public string Nombre { get; set; }
        [DisplayName("¿Habilitado?")]
        public bool Habilitado { get; set; }
        [Required(ErrorMessage = "El campo Hora de Inicio es requerido")]
        [RegularExpression(@"((([0-1][0-9])|(2[0-3]))(:[0-5][0-9])(:[0-5][0-9])?)", ErrorMessage = "El formato del campo Hora de Inicio no es válido")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:hh\\:mm tt}")]
        [Display(Name = "Hora de Inicio")]
        public System.TimeSpan Hora_Inicio { get; set; }
        [Required(ErrorMessage = "El campo Hora de Finalización es requerido")]
        [RegularExpression(@"((([0-1][0-9])|(2[0-3]))(:[0-5][0-9])(:[0-5][0-9])?)", ErrorMessage = "El formato del campo Hora de Finalización no es válido")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:hh\\:mm tt}")]
        [Display(Name = "Hora de Finalización")]
        public System.TimeSpan Hora_Fin { get; set; }
        [Required(ErrorMessage = "El campo Días es requerido")]
        [StringLength(10, ErrorMessage = "El campo Días debe tener una longitud máxima de 10 caracteres")]
        [DisplayName("Días")]
        public string Dias { get; set; }
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