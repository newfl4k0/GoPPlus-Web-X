using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using GoPS.Classes;

namespace GoPS.Models
{
    [MetadataType(typeof(UsersMetadata))]
    public partial class AspNetUsers
    {
        public string roles
        {
            get
            {
                return string.Join(",", AspNetUserRoles.Select(p => p.AspNetRoles).Select(a => a.Id).ToArray());
            }
        }

        public List<RadioButtonListItem> rolesList
        {
            get
            {
                return new Utilities().ObtenerRadioButtonRolesList(roles, "");
            }
        }

        public string afiliados
        {
            get
            {
                return string.Join(",", AspNetUserRoles.SelectMany(p => p.Afiliados).Select(a => a.ID_Afiliado).ToArray());
            }
        }

        public List<CheckBoxListItem> afiliadosList
        {
            get
            {
                return new Utilities().ObtenerCheckBoxAfiliadosList(afiliados, new List<int>(), "");
            }
        }
    }

    public class UsersMetadata
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }
        [DisplayName("Nombre de Usuario")]
        public string UserName { get; set; }
        [DisplayName("Email")]
        public string Email { get; set; }
        [DisplayName("¿Email Confirmado?")]
        public bool EmailConfirmed { get; set; }
        [DisplayName("Teléfono")]
        public string PhoneNumber { get; set; }
        [DisplayName("¿Teléfono Confirmado?")]
        public bool PhoneNumberConfirmed { get; set; }
        [DisplayName("¿Dos Factores de Confirmación Activado?")]
        public bool TwoFactorEnabled { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "No especificado")]
        [DisplayName("Fecha Final de Bloqueo")]
        public Nullable<System.DateTime> LockoutEndDateUtc { get; set; }
        [DisplayName("¿Bloqueo Activado?")]
        public bool LockoutEnabled { get; set; }
        [DisplayName("Cantidad de Accesos Fallidos")]
        public int AccessFailedCount { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "No especificado")]
        [DisplayName("Fecha de Nacimiento")]
        public System.DateTime DateOfBirth { get; set; }
        [DisplayName("Imagen")]
        public string PicturePath { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "No especificado")]
        [DisplayName("Puesto de Trabajo")]
        public Nullable<int> PositionID { get; set; }
        public Nullable<int> newId { get; set; }
    }
}