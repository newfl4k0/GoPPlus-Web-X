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
    [MetadataType(typeof(OperadoresMetadata))]
    public partial class Operadores
    {

        public virtual AspNetUsers AspNetUsers
        {
            get
            {
                return new GoPSEntities().AspNetUsers.Where(e => e.Id == UserID_Operador).FirstOrDefault();
            }
        }

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

    public class OperadoresMetadata
    {
        [ScaffoldColumn(false)]
        public int ID_Operador { get; set; }
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
        [StringLength(10, ErrorMessage = "El campo Número Interior debe tener una longitud máxima de 10 caracteres")]
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "No especificado")]
        [DisplayName("Número Interior")]
        public string Numero_Interior { get; set; }
        [Required(ErrorMessage = "El campo Código Postal es requerido")]
        //[StringLength(5, MinimumLength = 5, ErrorMessage = "El campo Código Postal debe tener una longitud de 5 caracteres")]
        [Range(1, int.MaxValue, ErrorMessage = "El campo Código Postal debe ser mayor a cero")]
        [DisplayName("Código Postal")]
        public int Codigo_Postal { get; set; }
        [Required(ErrorMessage = "El campo Teléfono es requerido")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(\+?\d{1,3}[ ]?)?([0-9]{3})?[ ]?([0-9]{3})[ ]?([0-9]{4})$", ErrorMessage = "El formato del campo Teléfono no es válido")]
        [DisplayName("Teléfono")]
        public string Telefono { get; set; }
        [Required(ErrorMessage = "El campo RFC es requerido")]
        [StringLength(14, MinimumLength = 13, ErrorMessage = "El campo RFC debe tener una longitud de 13 o 14 caracteres")]
        [DisplayName("RFC")]
        public string RFC { get; set; }
        [DisplayName("¿Habilitado?")]
        public bool Habilitado { get; set; }
        [Required(ErrorMessage = "El campo Objetivo Mensual es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "El campo Objetivo Mensual debe ser mayor a cero")]
        [DisplayName("Objetivo Mensual")]
        public int Objetivo_Mensual { get; set; }
        [Required(ErrorMessage = "El campo Afiliado es requerido")]
        [DisplayName("Afiliado")]
        public int ID_Afiliado { get; set; }
        [Required(ErrorMessage = "El campo Turno es requerido")]
        [DisplayName("Turno")]
        public int ID_Turno { get; set; }
        [Required(ErrorMessage = "El campo Usuario es requerido")]
        [DisplayName("Usuario")]
        public int UserID_Operador { get; set; }
        [ScaffoldColumn(false)]
        public System.DateTime Fecha_Creacion { get; set; }
        [ScaffoldColumn(false)]
        public Nullable<System.DateTime> Fecha_Actualizacion { get; set; }
        [ScaffoldColumn(false)]
        public string UserID { get; set; }
    }
}