using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GoPS.Models.Metadata
{
    public class ParametrosEdoCta
    {
        [Required(ErrorMessage = "El campo Fecha Inicio es requerido")]
        [DisplayName("Fecha Inicio")]
        public DateTime? Fecha_Inicio
        {
            get;

            set;
        }
        [Required(ErrorMessage = "El campo Fecha Fin es requerido")]
        [DisplayName("Fecha Fin")]
        public DateTime? Fecha_Fin
        {
            get;

            set;
        }
        [Required(ErrorMessage = "El campo RFC es requerido")]
        [DisplayName("RFC")]
        public string RFC
        {
            get;

            set;
        }
        [Required(ErrorMessage = "El campo Afiliado es requerido")]
        [DisplayName("Afiliado")]
        public int? ID_Afiliado
        {
            get;

            set;
        }
        public string Formato
        {
            get;

            set;
        }

    }

    public class ParametrosServColonia
    {
        [Required(ErrorMessage = "El campo Fecha Inicio es requerido")]
        [DisplayName("Fecha Inicio")]
        public DateTime? Fecha_Inicio
        {
            get;

            set;
        }
        [Required(ErrorMessage = "El campo Fecha Fin es requerido")]
        [DisplayName("Fecha Fin")]
        public DateTime? Fecha_Fin
        {
            get;

            set;
        }
        [Required(ErrorMessage = "El campo Colonia es requerido")]
        [DisplayName("Colonia")]
        public int? ID_Colonia
        {
            get;

            set;
        }
        [Required(ErrorMessage = "El campo Afiliado es requerido")]
        [DisplayName("Afiliado")]
        public int? ID_Afiliado
        {
            get;

            set;
        }
        [DisplayName("Estado")]
        public int? ID_Estado
        {
            get;

            set;
        }
        [DisplayName("Ciudad")]
        public int? ID_Ciudad
        {
            get;

            set;
        }        
        public string Formato
        {
            get;

            set;
        }

    }

    public class ParametrosCH
    {
        [Required(ErrorMessage = "El campo Fecha Inicio es requerido")]
        [DisplayName("Fecha Inicio")]
        public DateTime? Fecha_Inicio
        {
            get;

            set;
        }
        [Required(ErrorMessage = "El campo Fecha Fin es requerido")]
        [DisplayName("Fecha Fin")]
        public DateTime? Fecha_Fin
        {
            get;

            set;
        }
        [Required(ErrorMessage = "El campo Afiliado es requerido")]
        [DisplayName("Afiliado")]
        public int? ID_Afiliado
        {
            get;

            set;
        }
        public string Formato
        {
            get;

            set;
        }

    }

    public class ParametrosCA
    {
        [Required(ErrorMessage = "El campo Fecha Inicio es requerido")]
        [DisplayName("Fecha Inicio")]
        public DateTime? Fecha_Inicio
        {
            get;

            set;
        }
        [Required(ErrorMessage = "El campo Fecha Fin es requerido")]
        [DisplayName("Fecha Fin")]
        public DateTime? Fecha_Fin
        {
            get;

            set;
        }
        [Required(ErrorMessage = "El campo Afiliado es requerido")]
        [DisplayName("Afiliado")]
        public int? ID_Afiliado
        {
            get;

            set;
        }
        public string Formato
        {
            get;

            set;
        }

    }

    public class DespachosMetadata
    {
    }
}