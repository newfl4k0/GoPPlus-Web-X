using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GoPS.Models
{
    [MetadataType(typeof(AfiliadosMetadata))]
    public partial class Afiliados
    {
        private List<string> CiudadesGuadalajara
        {
            get
            {
                return new List<string>() { "Guadalajara", "Tlaquepaque", "Zapopan", "Tonala" };
            }
        }

        public string estatus { get; set; }

        public List<CheckBoxListItem> estatusList { get; set; }

        public string tiposvehiculos { get; set; }

        public List<CheckBoxListItem> tiposvehiculosList { get; set; }

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

        public string NombreRFC
        {
            get
            {
                return Nombre + " - " + 
                    (CiudadesGuadalajara.Contains(Calles.Colonias.Ciudades.Poblacion.Trim()) ? "Guadalajara"
                    : Calles.Colonias.Ciudades.Poblacion.Trim());
            }
        }

        public string EmpresaNombreRFC
        {
            get
            {
                return "Cliente: " + Empresas.Nombre + ": " + Nombre + " - " +
                    (CiudadesGuadalajara.Contains(Calles.Colonias.Ciudades.Poblacion.Trim()) ? "Guadalajara"
                    : Calles.Colonias.Ciudades.Poblacion.Trim());
            }
        }
    }

    public class AfiliadosMetadata
    {
        [ScaffoldColumn(false)]
        public int ID_Afiliado { get; set; }
        [Required(ErrorMessage = "El campo Nombre es requerido")]
        [StringLength(50, ErrorMessage = "El campo Nombre debe tener una longitud máxima de 50 caracteres")]
        [DisplayName("Nombre")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El campo RFC es requerido")]
        [StringLength(13, MinimumLength = 12, ErrorMessage = "El campo RFC debe tener una longitud de 12 o 13 caracteres")]
        [DisplayName("RFC")]
        public string RFC { get; set; }
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
        [Required(ErrorMessage = "El campo Código Postal es requerido")]
        //[StringLength(5, MinimumLength = 5, ErrorMessage = "El campo Código Postal debe tener una longitud de 5 caracteres")]
        [Range(1, int.MaxValue, ErrorMessage = "El campo Código Postal debe ser mayor a cero")]
        [DisplayName("Código Postal")]
        public int Codigo_Postal { get; set; }
        [Required(ErrorMessage = "El campo Horas de Trabajo del Conductor es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "El campo Horas de Trabajo del Conductor debe ser mayor a cero")]
        [DisplayName("Horas de Trabajo del Conductor")]
        public int Horas_Conductor { get; set; }
        //[Required(ErrorMessage = "El campo Cuota del Conductor es requerido")]
        [RegularExpression(@"-?(?:\d*[\,\.])?\d+", ErrorMessage = "El valor del campo Cuota del Conductor es inválido")]
        [Range(typeof(decimal), "0.01", "79228162514264337593543950335", ErrorMessage = "El valor del campo Cuota del Conductor debe ser mayor a 0")]
        [DataType(DataType.Currency)]
        [DisplayName("Cuota del Conductor")]
        public decimal Cuota_Conductor { get; set; }
        //[Required(ErrorMessage = "El campo Porcentaje de Comisión del Conductor es requerido")]
        [RegularExpression(@"-?(?:\d*[\,\.])?\d+", ErrorMessage = "El valor del campo Porcentaje de Comisión del Conductor es inválido")]
        [Range(typeof(decimal), "0.01", "100", ErrorMessage = "El valor del campo Porcentaje de Comisión del Conductor debe ser mayor a 0 y menor o igual a 100")]
        [DataType(DataType.Currency)]
        [DisplayName("Porcentaje de Comisión del Conductor")]
        public decimal Porcentaje_Conductor { get; set; }
        [Required(ErrorMessage = "El campo Tipo de Pago es requerido")]
        [DisplayName("Tipo de Pago")]
        public int ID_TipoPago { get; set; }
        [Required(ErrorMessage = "El campo Frecuencia de Pago es requerido")]
        [DisplayName("Frecuencia de Pago")]
        public int ID_FrecuenciaPago { get; set; }
        [Required(ErrorMessage = "El campo Tipo de Servicio es requerido")]
        [DisplayName("Tipo de Servicio")]
        public int ID_TipoServicio { get; set; }
        [Required(ErrorMessage = "El campo Empresa es requerido")]
        [DisplayName("Empresa")]
        public int ID_Empresa { get; set; }
        [Required(ErrorMessage = "El campo Estatus es requerido")]
        [DisplayName("Estatus")]
        public string estatus { get; set; }
        [Required(ErrorMessage = "El campo Tipos de Vehículos es requerido")]
        [DisplayName("Tipos de Vehículos")]
        public string tiposvehiculos { get; set; }
        [ScaffoldColumn(false)]
        public System.DateTime Fecha_Creacion { get; set; }
        [ScaffoldColumn(false)]
        public Nullable<System.DateTime> Fecha_Actualizacion { get; set; }
        [ScaffoldColumn(false)]
        public string UserID { get; set; }
    }
}