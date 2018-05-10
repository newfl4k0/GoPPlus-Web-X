using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GoPS.Models
{
    [MetadataType(typeof(ClientesHabitualesMetadata))]
    public partial class ClientesHabituales
    {
        public string Direccion
        {
            get
            {
                return Calles.Nombre + " # " + Numero_Exterior 
                    + (String.IsNullOrEmpty(Numero_Interior) ? "" : " Int " + Numero_Interior)
                    + ", Col. " + Calles.Colonias.Nombre 
                    + ", " + Calles.Colonias.Ciudades.Poblacion
                    + ", " + Calles.Colonias.Ciudades.Estados.Nombre;
            }
        }
    }

    public class ClientesHabitualesMetadata
    {
        [ScaffoldColumn(false)]
        public int ID_ClienteHabitual { get; set; }
        [Required(ErrorMessage = "El campo Nombre es requerido")]
        [StringLength(50, ErrorMessage = "El campo Nombre debe tener una longitud máxima de 50 caracteres")]
        [DisplayName("Nombre")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El campo Teléfono es requerido")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(\+?\d{1,3}[ ]?)?([0-9]{3})?[ ]?([0-9]{3})[ ]?([0-9]{4})$", ErrorMessage = "El formato del campo Teléfono no es válido")]
        [DisplayName("Teléfono")]
        public string Telefono { get; set; }
        [DisplayName("¿Habilitado?")]
        public bool Habilitado { get; set; }
        [DisplayName("¿Servicios Automáticos?")]
        public bool Servicios_Automaticos { get; set; }
        [Required(ErrorMessage = "El campo Tipo de Aviso es requerido")]
        [DisplayName("Tipo de Aviso")]
        public int ID_TipoAviso { get; set; }
        [Required(ErrorMessage = "El campo Calle es requerido")]
        [DisplayName("Calle")]
        public int ID_Calle { get; set; }
        [Required(ErrorMessage = "El campo Número Exterior es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "El campo Número Exterior debe ser mayor a cero")]
        [DisplayName("Número Exterior")]
        public int Numero_Exterior { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "No especificado")]
        [StringLength(10, ErrorMessage = "El campo Número Interior debe tener una longitud máxima de 10 caracteres")]
        [DisplayName("Número Interior")]
        public string Numero_Interior { get; set; }
        [Required(ErrorMessage = "El campo Fecha de Alta es requerido")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Fecha de Alta")]
        public System.DateTime Fecha_Alta { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true, NullDisplayText = "No especificado")]
        [DisplayName("Fecha de Baja")]
        public Nullable<System.DateTime> Fecha_Baja { get; set; }
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