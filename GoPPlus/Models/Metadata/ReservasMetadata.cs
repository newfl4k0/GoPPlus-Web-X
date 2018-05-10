using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GoPS.Models
{
    [MetadataType(typeof(ReservasMetadata))]
    public partial class Reservas
    {
        public string DireccionOrigen
        {
            get
            {
                return Calles1.Nombre + " # " + Numero_Exterior_Origen
                    + (String.IsNullOrEmpty(Numero_Interior_Origen) ? "" : " Int " + Numero_Interior_Origen)
                    + ", Col. " + Calles1.Colonias.Nombre
                    + ", " + Calles1.Colonias.Ciudades.Poblacion
                    + ", " + Calles1.Colonias.Ciudades.Estados.Nombre;
            }
        }
        public string DireccionDestino
        {
            get
            {
                return Calles.Nombre + " # " + Numero_Exterior_Destino
                    + (String.IsNullOrEmpty(Numero_Interior_Destino) ? "" : " Int " + Numero_Interior_Destino)
                    + ", Col. " + Calles.Colonias.Nombre
                    + ", " + Calles.Colonias.Ciudades.Poblacion
                    + ", " + Calles.Colonias.Ciudades.Estados.Nombre;
            }
        }
    }

    public class ReservasMetadata
    {
        [ScaffoldColumn(false)]
        public int ID_Reserva { get; set; }
        [Required(ErrorMessage = "El campo Fecha es requerido")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Fecha")]
        public System.DateTime Fecha { get; set; }
        [Required(ErrorMessage = "El campo Operador es requerido")]
        [DisplayName("Operador")]
        public int ID_Operador { get; set; }
        [DisplayName("Latitud Origen")]
        public Nullable<decimal> Latitud_Origen { get; set; }
        [DisplayName("Longitud Origen")]
        public Nullable<decimal> Longitud_Origen { get; set; }
        [DisplayName("Latitud Destino")]
        public Nullable<decimal> Latitud_Destino { get; set; }
        [DisplayName("Longitud Destino")]
        public Nullable<decimal> Longitud_Destino { get; set; }
        [DisplayName("Cliente")]
        public Nullable<int> ID_Cliente { get; set; }
        [DisplayName("Cliente Habitual")]
        public Nullable<int> ID_ClienteHabitual { get; set; }
        [DisplayName("Cliente Abonado")]
        public Nullable<int> ID_ClienteAbonado { get; set; }
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
        //[Required(ErrorMessage = "El campo Estatus de Reserva es requerido")]
        [DisplayName("Estatus")]
        public Nullable<int> ID_Estatus_Reserva { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Fecha de Finalización")]
        public Nullable<System.DateTime> Fecha_Finalizacion { get; set; }
        [Required(ErrorMessage = "El campo Tipo de Vehículo es requerido")]
        [DisplayName("Tipo de Vehículo")]
        public int ID_TipoVehiculo { get; set; }
        [DisplayName("Observaciones")]
        public string Observaciones { get; set; }
        [ScaffoldColumn(false)]
        public System.DateTime Fecha_Creacion { get; set; }
        [ScaffoldColumn(false)]
        public Nullable<System.DateTime> Fecha_Actualizacion { get; set; }
        [ScaffoldColumn(false)]
        public string UserID { get; set; }
    }
}