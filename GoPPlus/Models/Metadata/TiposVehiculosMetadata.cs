using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GoPS.Models
{
    [MetadataType(typeof(TiposVehiculosMetadata))]
    public partial class TiposVehiculos
    {
        public int Peso_Imagen { get; set; }

        public string File_Imagen { get; set; }

        public int Peso_ImagenRed { get; set; }

        public string File_ImagenRed { get; set; }
    }

    public partial class TiposVehiculos_Afiliados
    {
        public string TipoVehiculoNombre
        {
            get
            {
                return TiposVehiculos.Nombre;
            }
        }
    }

    public class TiposVehiculosMetadata
    {
        [ScaffoldColumn(false)]
        public int ID_TipoVehiculo { get; set; }
        [Required(ErrorMessage = "El campo Nombre es requerido")]
        [StringLength(50, ErrorMessage = "El campo Nombre debe tener una longitud máxima de 50 caracteres")]
        [DisplayName("Nombre")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El campo Imagen es requerido")]
        [DisplayName("Imagen")]
        public string Imagen { get; set; }
        [Required(ErrorMessage = "El campo Tarifa es requerido")]
        [DisplayName("Tarifa")]
        public int ID_Tarifa { get; set; }
        [DisplayName("Empresa")]
        public int ID_Empresa { get; set; }
        [ScaffoldColumn(false)]
        public System.DateTime Fecha_Creacion { get; set; }
        [ScaffoldColumn(false)]
        public Nullable<System.DateTime> Fecha_Actualizacion { get; set; }
        [ScaffoldColumn(false)]
        public string UserID { get; set; }
    }
}