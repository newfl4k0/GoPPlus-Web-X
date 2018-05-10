using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GoPS.Models
{
    [MetadataType(typeof(DatosTicketsMetadata))]
    public partial class DatosTickets
    {
        public int Peso_Imagen { get; set; }

        public string File_Imagen { get; set; }
    }

    public class DatosTicketsMetadata
    {
        [ScaffoldColumn(false)]
        public int ID_DatoTicket { get; set; }
        [StringLength(10, ErrorMessage = "El campo Texto No. 1 debe tener una longitud máxima de 10 caracteres")]
        [DisplayName("Texto No. 1")]
        public string Texto_1 { get; set; }
        [StringLength(10, ErrorMessage = "El campo Texto No. 2 debe tener una longitud máxima de 10 caracteres")]
        [DisplayName("Texto No. 2")]
        public string Texto_2 { get; set; }
        [StringLength(10, ErrorMessage = "El campo Texto No. 3 debe tener una longitud máxima de 10 caracteres")]
        [DisplayName("Texto No. 3")]
        public string Texto_3 { get; set; }
        [StringLength(10, ErrorMessage = "El campo Texto No. 4 debe tener una longitud máxima de 10 caracteres")]
        [DisplayName("Texto No. 4")]
        public string Texto_4 { get; set; }
        [StringLength(10, ErrorMessage = "El campo Texto No. 5 debe tener una longitud máxima de 10 caracteres")]
        [DisplayName("Texto No. 5")]
        public string Texto_5 { get; set; }
        [StringLength(10, ErrorMessage = "El campo Texto No. 6 debe tener una longitud máxima de 10 caracteres")]
        [DisplayName("Texto No. 6")]
        public string Texto_6 { get; set; }
        [StringLength(10, ErrorMessage = "El campo Texto No. 7 debe tener una longitud máxima de 10 caracteres")]
        [DisplayName("Texto No. 7")]
        public string Texto_7 { get; set; }
        [StringLength(10, ErrorMessage = "El campo Texto No. 8 debe tener una longitud máxima de 10 caracteres")]
        [DisplayName("Texto No. 8")]
        public string Texto_8 { get; set; }
        [StringLength(10, ErrorMessage = "El campo Texto No. 9 debe tener una longitud máxima de 10 caracteres")]
        [DisplayName("Texto No. 9")]
        public string Texto_9 { get; set; }
        [StringLength(10, ErrorMessage = "El campo Texto No. 10 debe tener una longitud máxima de 10 caracteres")]
        [DisplayName("Texto No. 10")]
        public string Texto_10 { get; set; }
        [StringLength(50, ErrorMessage = "El campo Imagen debe tener una longitud máxima de 50 caracteres")]
        [DisplayName("Imagen")]
        public string Imagen { get; set; }
        [DisplayName("¿Fecha Bordo?")]
        public bool Fecha_Bordo { get; set; }
        [DisplayName("¿Fecha Finalización?")]
        public bool Fecha_Finalizacion { get; set; }
        [DisplayName("¿No. Taxi?")]
        public bool No_Taxi { get; set; }
        [DisplayName("¿Placas?")]
        public bool Placas { get; set; }
        [DisplayName("¿Nombre de Cliente?")]
        public bool Nombre_Cliente { get; set; }
        [DisplayName("¿Origen?")]
        public bool Origen { get; set; }
        [DisplayName("¿Destino?")]
        public bool Destino { get; set; }
        [DisplayName("¿Importe?")]
        public bool Importe { get; set; }
        [DisplayName("¿Observaciones?")]
        public bool Observaciones { get; set; }
        [DisplayName("¿Despacho?")]
        public bool Despacho { get; set; }
        [DisplayName("¿Código?")]
        public bool Codigo { get; set; }
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