using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GoPS.Models
{
    [MetadataType(typeof(PuntosInteresMetadata))]
    public partial class PuntosInteres
    {
        public string Direccion
        {
            get
            {
                return Calles.Nombre + " # " + Numero
                    + ", Col. " + Calles.Colonias.Nombre
                    + ", " + Calles.Colonias.Ciudades.Poblacion
                    + ", " + Calles.Colonias.Ciudades.Estados.Nombre;
            }
        }
    }

    public class PuntosInteresMetadata
    {
        [ScaffoldColumn(false)]
        public int ID_PuntoInteres { get; set; }
        [Required(ErrorMessage = "El campo Nombre es requerido")]
        [StringLength(50, ErrorMessage = "El campo Nombre debe tener una longitud máxima de 50 caracteres")]
        [DisplayName("Nombre")]
        public string Nombre { get; set; }
        [DisplayName("¿Habilitado?")]
        public bool Habilitado { get; set; }
        [Required(ErrorMessage = "El campo Calle es requerido")]
        [DisplayName("Calle")]
        public int ID_Calle { get; set; }
        [Required(ErrorMessage = "El campo Número es requerido")]
        [DisplayName("Número")]
        public int Numero { get; set; }
        [Required(ErrorMessage = "El campo Latitud es requerido")]
        [DisplayName("Latitud")]
        public decimal Latitud { get; set; }
        [Required(ErrorMessage = "El campo Longitud es requerido")]
        [DisplayName("Longitud")]
        public decimal Longitud { get; set; }
        [Required(ErrorMessage = "El campo Tipo de Punto de Interés es requerido")]
        [DisplayName("Tipo de Punto de Interés")]
        public int ID_TipoPuntoInteres { get; set; }
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