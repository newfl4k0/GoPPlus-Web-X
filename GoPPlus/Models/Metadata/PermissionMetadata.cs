using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GoPS.Models
{
    [MetadataType(typeof(PermissionsMetadata))]
    public partial class Permissions
    {
        public string roles { get; set; }

        public List<CheckBoxListItem> rolesList { get; set; }
    }

    public class PermissionsMetadata
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo Nombre es requerido")]
        [StringLength(50, ErrorMessage = "El campo Nombre debe tener una longitud máxima de 50 caracteres")]
        [DisplayName("Nombre")]
        public string Name { get; set; }
        [Required(ErrorMessage = "El campo Descripción es requerido")]
        [StringLength(100, ErrorMessage = "El campo Descripción debe tener una longitud máxima de 100 caracteres")]
        [DisplayName("Descripción")]
        public string Description { get; set; }
        [DisplayName("¿Habilitado?")]
        public bool Able { get; set; }
        [DisplayName("Perfiles")]
        public string roles { get; set; }
    }
}