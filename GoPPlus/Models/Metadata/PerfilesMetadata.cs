using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GoPS.Models
{
    [MetadataType(typeof(AspNetRolesMetadata))]
    public partial class AspNetRoles
    {
        public string permisos { get; set; }

        public List<CheckBoxListItem> permisosList { get; set; }
    }

    public class AspNetRolesMetadata
    {
        [ScaffoldColumn(false)]
        public string Id { get; set; }
        [Required(ErrorMessage = "El campo Nombre es requerido")]
        [StringLength(50, ErrorMessage = "El campo Nombre debe tener una longitud máxima de 50 caracteres")]
        [DisplayName("Nombre")]
        public string Name { get; set; }
        [Required(ErrorMessage = "El campo Permisos es requerido")]
        [DisplayName("Permisos")]
        public string permisos { get; set; }
        [DisplayName("¿Perfil por Afiliado?")]
        public bool Afiliado { get; set; }
    }
}