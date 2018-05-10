using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GoPS.Models
{
    [MetadataType(typeof(ChatsMetadata))]
    public partial class Chat
    {
    }

    public class ChatsMetadata
    {
        [ScaffoldColumn(false)]
        public int ID_Chat { get; set; }
        [Required(ErrorMessage = "El campo Mensaje es requerido")]
        [StringLength(200, ErrorMessage = "El campo Mensaje debe tener una longitud máxima de 200 caracteres")]
        [DataType(DataType.MultilineText)]
        [DisplayName("Mensaje")]
        public string Mensaje { get; set; }
        [Required(ErrorMessage = "El campo Conductor es requerido")]
        [DisplayName("Conductor")]
        public int ID_Conductor { get; set; }
        [DisplayName("Operador")]
        public Nullable<int> ID_Operador { get; set; }
        [DisplayName("Despacho")]
        public Nullable<int> ID_Despacho { get; set; }
        [Required(ErrorMessage = "El campo Fecha es requerido")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Fecha")]
        public System.DateTime Fecha { get; set; }
        [UIHint("DisplayBoolean")]
        [DisplayName("¿Es Conductor?")]
        public bool Es_Conductor { get; set; }
    }
}