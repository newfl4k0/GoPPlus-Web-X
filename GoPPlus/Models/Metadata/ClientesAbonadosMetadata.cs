using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GoPS.Models
{
    [MetadataType(typeof(ClientesAbonadosMetadata))]
    public partial class ClientesAbonados
    {
        public string Direccion
        {
            get
            {
                return Calles == null ? null :
                    Calles.Nombre + " # " + Numero_Exterior
                    + (String.IsNullOrEmpty(Numero_Interior) ? "" : " Int " + Numero_Interior)
                    + ", Col. " + Calles.Colonias.Nombre
                    + ", " + Calles.Colonias.Ciudades.Poblacion
                    + ", " + Calles.Colonias.Ciudades.Estados.Nombre;
            }
        }
    }

    public class ClientesAbonadosMetadata
    {
        [ScaffoldColumn(false)]
        public int ID_ClienteAbonado { get; set; }
        [Required(ErrorMessage = "El campo Nombre es requerido")]
        [StringLength(50, ErrorMessage = "El campo Nombre debe tener una longitud máxima de 50 caracteres")]
        [DisplayName("Nombre")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El campo Teléfono es requerido")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(\+?\d{1,3}[ ]?)?([0-9]{3})?[ ]?([0-9]{3})[ ]?([0-9]{4})$", ErrorMessage = "El formato del campo Teléfono no es válido")]
        [DisplayName("Teléfono")]
        public string Telefono { get; set; }
        [DisplayName("¿El Teléfono es Celular?")]
        public bool Telefono_EsCelular { get; set; }
        [Required(ErrorMessage = "El campo Límite de Crédito es requerido")]
        [RegularExpression(@"-?(?:\d*[\,\.])?\d+", ErrorMessage = "El valor del campo Límite de Crédito es inválido")]
        [Range(typeof(decimal), "0.01", "79228162514264337593543950335", ErrorMessage = "El valor del campo Límite de Crédito debe ser mayor a 0")]
        [DataType(DataType.Currency)]
        [DisplayName("Límite de Crédito")]
        public decimal Limite_Credito { get; set; }
        [Required(ErrorMessage = "El campo Email es requerido")]
        [EmailAddress(ErrorMessage = "El formato del campo Email es inválido")]
        [DisplayName("Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "El campo Clave Automática es requerido")]
        [StringLength(10, ErrorMessage = "El campo Clave Automática debe tener una longitud máxima de 10 caracteres")]
        [DisplayName("Clave Automática")]
        public string Clave_Automatica { get; set; }
        [StringLength(500, ErrorMessage = "El campo Observaciones debe tener una longitud máxima de 500 caracteres")]
        [DataType(DataType.MultilineText)]
        [DisplayName("Observaciones")]
        public string Observaciones { get; set; }
        [DisplayName("¿Habilitado?")]
        public bool Habilitado { get; set; }
        [DisplayName("¿Reportes?")]
        public bool Reportes { get; set; }
        [DisplayName("¿Servicios Automáticos?")]
        public bool Servicios_Automaticos { get; set; }
        [Required(ErrorMessage = "El campo Fecha de Alta es requerido")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Fecha de Alta")]
        public System.DateTime Fecha_Alta { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true, NullDisplayText = "No especificado")]
        [DisplayName("Fecha de Baja")]
        public Nullable<System.DateTime> Fecha_Baja { get; set; }
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
        [Required(ErrorMessage = "El campo CP es requerido")]
        //[StringLength(5, MinimumLength = 5, ErrorMessage = "El campo CP debe tener una longitud de 5 caracteres")]
        [Range(1, int.MaxValue, ErrorMessage = "El campo CP debe ser mayor a cero")]
        [DisplayName("CP")]
        public int CP { get; set; }
        [Required(ErrorMessage = "El campo RFC es requerido")]
        [StringLength(14, MinimumLength = 13, ErrorMessage = "El campo RFC debe tener una longitud de 13 o 14 caracteres")]
        [DisplayName("RFC")]
        public string RFC { get; set; }
        [Required(ErrorMessage = "El campo Día de Pago es requerido")]
        [DisplayName("Día de Pago")]
        public int Dia_Pago { get; set; }
        [Required(ErrorMessage = "El campo Banco es requerido")]
        [DisplayName("Banco")]
        public int ID_Banco { get; set; }
        [Required(ErrorMessage = "El campo Clabe es requerido")]
        [StringLength(18, MinimumLength = 18, ErrorMessage = "El campo Clabe debe tener una longitud de 18 caracteres")]
        [DisplayName("Clabe")]
        public string Clabe { get; set; }
        [Required(ErrorMessage = "El campo Tipo de Aviso es requerido")]
        [DisplayName("Tipo de Aviso")]
        public int ID_TipoAviso { get; set; }
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