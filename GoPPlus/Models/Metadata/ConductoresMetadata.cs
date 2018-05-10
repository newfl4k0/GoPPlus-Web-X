using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GoPS.Models
{
    [MetadataType(typeof(ConductoresMetadata))]
    public partial class Conductores
    {
        public virtual AspNetUsers AspNetUsers
        {
            get
            {
                return new GoPSEntities().AspNetUsers.Where(e => e.Id == UserID_Conductor).FirstOrDefault();
            }
        }

        public string NombreCompleto
        {
            get
            {
                return Nombre + " " + Apellido;
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

        public int Peso_Archivo_CartaFianza { get; set; }
        public int Peso_Archivo_INE { get; set; }
        public int Peso_Archivo_Domicilio { get; set; }
        public int Peso_Archivo_Tarjeton { get; set; }
        public int Peso_Archivo_Licencia { get; set; }
        public int Peso_Archivo_Antidoping { get; set; }
        public int Peso_Archivo_NoAntecedentes { get; set; }

        public string File_Archivo_CartaFianza { get; set; }
        public string File_Archivo_INE { get; set; }
        public string File_Archivo_Domicilio { get; set; }
        public string File_Archivo_Tarjeton { get; set; }
        public string File_Archivo_Licencia { get; set; }
        public string File_Archivo_Antidoping { get; set; }
        public string File_Archivo_NoAntecedentes { get; set; }
    }

    public class ConductoresMetadata
    {
        [ScaffoldColumn(false)]
        public int ID_Conductor { get; set; }
        [Required(ErrorMessage = "El campo Nombre es requerido")]
        [StringLength(50, ErrorMessage = "El campo Nombre debe tener una longitud máxima de 50 caracteres")]
        [DisplayName("Nombre")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El campo Apellido es requerido")]
        [StringLength(50, ErrorMessage = "El campo Apellido debe tener una longitud máxima de 50 caracteres")]
        [DisplayName("Apellido")]
        public string Apellido { get; set; }
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
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(\+?\d{1,3}[ ]?)?([0-9]{3})?[ ]?([0-9]{3})[ ]?([0-9]{4})$", ErrorMessage = "El formato del campo Celular no es válido")]
        [DisplayName("Celular")]
        public string Celular { get; set; }
        [Required(ErrorMessage = "El campo RFC es requerido")]
        [StringLength(13, MinimumLength = 12, ErrorMessage = "El campo RFC debe tener una longitud de 13 o 14 caracteres")]
        [DisplayName("RFC")]
        public string RFC { get; set; }
        [UIHint("DisplayBoolean")]
        [DisplayName("¿Habilitado?")]
        public bool Habilitado { get; set; }
        [Required(ErrorMessage = "El campo Fecha de Nacimiento es requerido")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Fecha de Nacimiento")]
        public System.DateTime Fecha_Nacimiento { get; set; }
        [Required(ErrorMessage = "El campo Número de Licencia es requerido")]
        [StringLength(13, MinimumLength = 12, ErrorMessage = "El campo Número de Licencia debe tener una longitud de 12 o 13 caracteres")]
        [DisplayName("Número de Licencia")]
        public string NoLicencia { get; set; }
        [Required(ErrorMessage = "El campo Número de Tarjetón es requerido")]
        [StringLength(11, MinimumLength = 10, ErrorMessage = "El campo Número de Tarjetón debe tener una longitud de 10 u 11 caracteres")]
        [DisplayName("Número de Tarjetón")]
        public string NoTarjeton { get; set; }
        [Required(ErrorMessage = "El campo Vigencia de Licencia es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "El campo Vigencia de Licencia debe ser mayor a cero")]
        [DisplayName("Vigencia de Licencia")]
        public int Vigencia_Licencia { get; set; }
        [Required(ErrorMessage = "El campo Vigencia de Tarjetón es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "El campo Vigencia de Tarjetón debe ser mayor a cero")]
        [DisplayName("Vigencia de Tarjetón")]
        public int Vigencia_Tarjeton { get; set; }
        [Required(ErrorMessage = "El campo Archivo Fianza es requerido")]
        [DisplayName("Archivo Carta Fianza")]
        public string Archivo_CartaFianza { get; set; }
        [Required(ErrorMessage = "El campo Archivo INE es requerido")]
        [DisplayName("Archivo INE")]
        public string Archivo_INE { get; set; }
        [Required(ErrorMessage = "El campo Archivo Domicilio es requerido")]
        [DisplayName("Archivo Domicilio")]
        public string Archivo_Domicilio { get; set; }
        [Required(ErrorMessage = "El campo Archivo No. Tarjetón es requerido")]
        [DisplayName("Archivo No. Tarjetón")]
        public string Archivo_Tarjeton { get; set; }
        [Required(ErrorMessage = "El campo Archivo No. Licencia es requerido")]
        [DisplayName("Archivo No. Licencia")]
        public string Archivo_Licencia { get; set; }
        [Required(ErrorMessage = "El campo Archivo Antidoping es requerido")]
        [DisplayName("Archivo Antidoping")]
        public string Archivo_Antidoping { get; set; }
        [Required(ErrorMessage = "El campo Archivo No Antecedentes es requerido")]
        [DisplayName("Archivo No Antecedentes")]
        public string Archivo_NoAntecedentes { get; set; }
        [Required(ErrorMessage = "El campo Turno es requerido")]
        [DisplayName("Turno")]
        public int ID_Turno { get; set; }
        [Required(ErrorMessage = "El campo Flota es requerido")]
        [DisplayName("Flota")]
        public int ID_Flota { get; set; }
        [Required(ErrorMessage = "El campo Usuario es requerido")]
        [DisplayName("Usuario")]
        public int UserID_Conductor { get; set; }
        [ScaffoldColumn(false)]
        public System.DateTime Fecha_Creacion { get; set; }
        [ScaffoldColumn(false)]
        public Nullable<System.DateTime> Fecha_Actualizacion { get; set; }
        [ScaffoldColumn(false)]
        public string UserID { get; set; }
    }
}