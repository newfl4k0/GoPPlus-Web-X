using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GoPS.Models
{
    [MetadataType(typeof(RutasMetadata))]
    public partial class Rutas
    {
        public string Direccion_Origen
        {
            get
            {
                return Calles1.Nombre + " # " + Numero_Exterior_Origen
                    + (String.IsNullOrEmpty(Numero_Interior_Origen) ? "" : "Int " + Numero_Interior_Origen)
                    + ", Col. " + Calles1.Colonias.Nombre
                    + ", " + Calles1.Colonias.Ciudades.Poblacion
                    + ", " + Calles1.Colonias.Ciudades.Estados.Nombre;
            }
        }

        public string Direccion_Destino
        {
            get
            {
                return Calles.Nombre + " # " + Numero_Exterior_Destino
                    + (String.IsNullOrEmpty(Numero_Interior_Destino) ? "" : "Int " + Numero_Interior_Destino)
                    + ", Col. " + Calles.Colonias.Nombre
                    + ", " + Calles.Colonias.Ciudades.Poblacion
                    + ", " + Calles.Colonias.Ciudades.Estados.Nombre;
            }
        }
    }

    public class RutasMetadata
    {
        [ScaffoldColumn(false)]
        public int ID_Ruta { get; set; }
        [Required(ErrorMessage = "El campo Nombre es requerido")]
        [StringLength(50, ErrorMessage = "El campo Nombre debe tener una longitud máxima de 50 caracteres")]
        [DisplayName("Nombre")]
        public string Nombre { get; set; }
        [DisplayName("¿Habilitado?")]
        public bool Habilitado { get; set; }
        [Required(ErrorMessage = "El campo Precio es requerido")]
        [RegularExpression(@"-?(?:\d*[\,\.])?\d+", ErrorMessage = "El valor del campo Precio es inválido")]
        [Range(typeof(decimal), "0.01", "79228162514264337593543950335", ErrorMessage = "El valor del campo Precio debe ser mayor a 0")]
        [DataType(DataType.Currency)]
        [DisplayName("Precio")]
        public int Precio { get; set; }
        [Required(ErrorMessage = "El campo Calle Origen es requerido")]
        [DisplayName("Calle Origen")]
        public int ID_Calle_Origen { get; set; }
        [Required(ErrorMessage = "El campo Número Exterior Origen es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "El campo Número Exterior Origen debe ser mayor a cero")]
        [DisplayName("Número Exterior Origen")]
        public int Numero_Exterior_Origen { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "No especificado")]
        [DisplayName("Número Interior Origen")]
        [StringLength(10, ErrorMessage = "El campo Número Interior Origen debe tener una longitud máxima de 10 caracteres")]
        public string Numero_Interior_Origen { get; set; }
        [Required(ErrorMessage = "El campo Calle Destino es requerido")]
        [DisplayName("Calle Destino")]
        public int ID_Calle_Destino { get; set; }
        [Required(ErrorMessage = "El campo Número Exterior Destino es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "El campo Número Exterior Destino debe ser mayor a cero")]
        [DisplayName("Número Exterior Destino")]
        public int Numero_Exterior_Destino { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "No especificado")]
        [StringLength(10, ErrorMessage = "El campo Número Interior Destino debe tener una longitud máxima de 10 caracteres")]
        [DisplayName("Número Interior Destino")]
        public string Numero_Interior_Destino { get; set; }
        [Required(ErrorMessage = "El campo Usuario Abonado es requerido")]
        [DisplayName("Usuario Abonado")]
        public int ID_UsuarioAbonado { get; set; }
        [ScaffoldColumn(false)]
        public System.DateTime Fecha_Creacion { get; set; }
        [ScaffoldColumn(false)]
        public Nullable<System.DateTime> Fecha_Actualizacion { get; set; }
        [ScaffoldColumn(false)]
        public string UserID { get; set; }
    }
}