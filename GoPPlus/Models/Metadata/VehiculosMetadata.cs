using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GoPS.Models
{
    [MetadataType(typeof(VehiculosMetadata))]
    public partial class Vehiculos
    {
    }

    public class VehiculosMetadata
    {
        [ScaffoldColumn(false)]
        public int ID_Vehiculo { get; set; }
        [Required(ErrorMessage = "El campo Matrícula es requerido")]
        [StringLength(7, MinimumLength = 7, ErrorMessage = "El campo Matrícula debe tener una longitud de 7 caracteres")]
        [DisplayName("Matrícula")]
        public string Matricula { get; set; }
        [Required(ErrorMessage = "El campo Fecha de Alta es requerido")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Fecha de Alta")]
        public System.DateTime Fecha_Alta { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true, NullDisplayText = "No especificado")]
        [DisplayName("Fecha de Baja")]
        public Nullable<System.DateTime> Fecha_Baja { get; set; }
        [Required(ErrorMessage = "El campo Teléfono Seguro es requerido")]
        [RegularExpression(@"^(\+?\d{1,3}[ ]?)?([0-9]{3})?[ ]?([0-9]{3})[ ]?([0-9]{4})$", ErrorMessage = "El formato del campo Teléfono Seguro no es válido")]
        [DisplayName("Teléfono Seguro")]
        public string Telefono_Seguro { get; set; }
        [DisplayName("¿El Teléfono Seguro es Celular?")]
        public bool Telefono_Seguro_EsCelular { get; set; }
        [Required(ErrorMessage = "El campo Póliza es requerido")]
        [StringLength(15, ErrorMessage = "El campo Póliza debe tener una longitud máxima de 15 caracteres")]
        [DisplayName("Póliza")]
        public string Poliza { get; set; }
        [Required(ErrorMessage = "El campo Teléfono es requerido")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(\+?\d{1,3}[ ]?)?([0-9]{3})?[ ]?([0-9]{3})[ ]?([0-9]{4})$", ErrorMessage = "El formato del campo Teléfono no es válido")]
        [DisplayName("Teléfono")]
        public string Telefono { get; set; }
        [Required(ErrorMessage = "El campo Serie es requerido")]
        [StringLength(18, MinimumLength = 18, ErrorMessage = "El campo Serie debe tener una longitud de 18 caracteres")]
        [DisplayName("Serie")]
        public string Serie { get; set; }
        [DisplayName("¿Habilitado?")]
        public bool Habilitado { get; set; }
        [DisplayName("¿Externo?")]
        public bool Externo { get; set; }
        [Required(ErrorMessage = "El campo Número de Licencia es requerido")]
        [StringLength(13, MinimumLength = 12, ErrorMessage = "El campo Número de Licencia debe tener una longitud de 12 o 13 caracteres")]
        [DisplayName("Número de Licencia")]
        public string NoLicencia { get; set; }
        [Required(ErrorMessage = "El campo Número de Tarjetón es requerido")]
        [StringLength(10, MinimumLength = 9, ErrorMessage = "El campo Número de Tarjetón debe tener una longitud de 9 o 10 caracteres")]
        [DisplayName("Número de Tarjetón")]
        public string NoTarjeton { get; set; }
        [Required(ErrorMessage = "El campo Versión es requerido")]
        [StringLength(5, ErrorMessage = "El campo Versión debe tener una longitud máxima de 5 caracteres")]
        [DisplayName("Versión")]
        public string Version { get; set; }
        [Required(ErrorMessage = "El campo Revista Mecánica es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "El campo Revista Mecánica debe ser mayor a cero")]
        [DisplayName("Revista Mecánica")]
        public int RevistaMecanica { get; set; }
        [Required(ErrorMessage = "El campo Kilometraje Inicial es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "El campo Kilometraje Inicial debe ser mayor a cero")]
        [DisplayName("Kilometraje Inicial")]
        public int KmInicial { get; set; }
        [Required(ErrorMessage = "El campo Kilometraje Final es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "El campo Kilometraje Final debe ser mayor a cero")]
        [DisplayName("Kilometraje Final")]
        public int KmFinal { get; set; }
        [Required(ErrorMessage = "El campo Número de Permiso es requerido")]
        [StringLength(16, MinimumLength = 10, ErrorMessage = "El campo Número de Permiso debe tener una longitud de 10 a 16 caracteres")]
        [DisplayName("Número de Permiso")]
        public string NoPermiso { get; set; }
        [Required(ErrorMessage = "El campo Vigencia de Permiso es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "El campo Vigencia de Permiso debe ser mayor a cero")]
        [DisplayName("Vigencia de Permiso")]
        public int Vigencia_Permiso { get; set; }
        [Required(ErrorMessage = "El campo Número de Transporte Ejecutivo es requerido")]
        [StringLength(12, MinimumLength = 10, ErrorMessage = "El campo Número de Transporte Ejecutivo debe tener una longitud de 10 a 12 caracteres")]
        [DisplayName("Número de Transporte Ejecutivo")]
        public string NoTransporte { get; set; }
        [Required(ErrorMessage = "El campo Flota es requerido")]
        [DisplayName("Flota")]
        public int ID_Flota { get; set; }
        [Required(ErrorMessage = "El campo Ciudad es requerido")]
        [DisplayName("Ciudad")]
        public int ID_Ciudad { get; set; }
        [Required(ErrorMessage = "El campo Modelo es requerido")]
        [DisplayName("Modelo")]
        public int ID_Modelo { get; set; }
        [Required(ErrorMessage = "El campo Seguro es requerido")]
        [DisplayName("Seguro")]
        public int ID_Seguro { get; set; }
        [Required(ErrorMessage = "El campo Tipo de Vehículo es requerido")]
        [DisplayName("Tipo de Vehículo")]
        public int ID_TipoVehiculo { get; set; }
        [Required(ErrorMessage = "El campo Color es requerido")]
        [DisplayName("Color")]
        public int ID_Color { get; set; }
        [ScaffoldColumn(false)]
        public System.DateTime Fecha_Creacion { get; set; }
        [ScaffoldColumn(false)]
        public Nullable<System.DateTime> Fecha_Actualizacion { get; set; }
        [ScaffoldColumn(false)]
        public string UserID { get; set; }
    }
}