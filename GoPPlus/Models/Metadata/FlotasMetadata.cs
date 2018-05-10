using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GoPS.Models
{
    [MetadataType(typeof(FlotasMetadata))]
    public partial class Flotas
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

    public class FlotasMetadata
    {
        [ScaffoldColumn(false)]
        public int ID_Flota { get; set; }
        [Required(ErrorMessage = "El campo Nombre es requerido")]
        [StringLength(50, ErrorMessage = "El campo Nombre debe tener una longitud máxima de 50 caracteres")]
        [DisplayName("Nombre")]
        public string Nombre { get; set; }
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
        [Required(ErrorMessage = "El campo Teléfono es requerido")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(\+?\d{1,3}[ ]?)?([0-9]{3})?[ ]?([0-9]{3})[ ]?([0-9]{4})$", ErrorMessage = "El formato del campo Teléfono no es válido")]
        [DisplayName("Teléfono")]
        public string Telefono { get; set; }
        [Required(ErrorMessage = "El campo Celular es requerido")]
        [RegularExpression(@"^(\+?\d{1,3}[ ]?)?([0-9]{3})?[ ]?([0-9]{3})[ ]?([0-9]{4})$", ErrorMessage = "El formato del campo Celular no es válido")]
        [DisplayName("Celular")]
        public string Celular { get; set; }
        [Required(ErrorMessage = "El campo RFC es requerido")]
        [StringLength(13, MinimumLength = 12, ErrorMessage = "El campo RFC debe tener una longitud de 13 o 14 caracteres")]
        [DisplayName("RFC")]
        public string RFC { get; set; }
        [Required(ErrorMessage = "El campo Banco es requerido")]
        [DisplayName("Banco")]
        public int ID_Banco { get; set; }
        [Required(ErrorMessage = "El campo Clabe es requerido")]
        [StringLength(18, ErrorMessage = "El campo Clabe debe tener una longitud máxima de 18 caracteres")]
        [DisplayName("Clabe")]
        public string Clabe { get; set; }
        [Required(ErrorMessage = "El campo Email es requerido")]
        [EmailAddress(ErrorMessage = "El formato del campo Email es inválido")]
        [DisplayName("Email")]
        public string Email { get; set; }
        [DisplayName("¿Habilitado?")]
        public bool Habilitado { get; set; }
        [DisplayName("¿Reportes?")]
        public bool Reportes { get; set; }
        [Required(ErrorMessage = "El campo Forma de Pago es requerido")]
        [DisplayName("Forma de Pago")]
        public int ID_FormaPago { get; set; }
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