using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GoPS.Models
{
    [MetadataType(typeof(EstatusMetadata))]
    public partial class Estatus
    {
        public int Peso_Imagen { get; set; }

        public string File_Imagen { get; set; }
    }

    public partial class Estatus_Afiliados
    {
        public string EstatusNombre
        {
            get
            {
                return Estatus.Nombre;
            }
        }
    }

    public class EstatusMetadata
    {
        [ScaffoldColumn(false)]
        public int ID_Estatus { get; set; }
        [Required(ErrorMessage = "El campo Nombre es requerido")]
        [StringLength(50, ErrorMessage = "El campo Nombre debe tener una longitud máxima de 50 caracteres")]
        [DisplayName("Nombre")]
        public string Nombre { get; set; }
        [DisplayName("Imagen")]
        public string Imagen { get; set; }
        [Required(ErrorMessage = "El campo Empresa es requerido")]
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