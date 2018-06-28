using GoPS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GoPS.ViewModels
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required(ErrorMessage = "El campo Email es requerido.")]
        [EmailAddress(ErrorMessage = "El formato del campo Email es inválido.")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required(ErrorMessage = "El campo Provider es requerido.")]
        [Display(Name = "Provider")]
        public string Provider { get; set; }

        [Required(ErrorMessage = "El campo Code es requerido.")]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "¿Recordar este Navegador?")]
        public bool RememberBrowser { get; set; }

        [Display(Name = "¿Recordarme?")]
        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required(ErrorMessage = "El campo Email es requerido.")]
        [EmailAddress(ErrorMessage = "El formato del campo Email es inválido.")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage = "El campo de usuario es requerido. Introduzca su email o su usuario.")]
        [Display(Name = "Email o Username")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El campo Contraseña es requerido.")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Display(Name = "¿Recordarme?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "El campo Email es requerido.")]
        [EmailAddress(ErrorMessage = "El formato del campo Email es inválido.")]
        [StringLength(256, ErrorMessage = "El campo Email debe tener una longitud máxima de 256 caracteres.")]
        [Display(Name = "Email")]
        [System.Web.Mvc.Remote("UserEmailAlreadyExistsAsync", "Account", ErrorMessage = "El usuario con este correo electrónico ya existe.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El campo Contraseña es requerido.")]
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        [MinLength(8, ErrorMessage = "La contraseña debe contener como mínimo 8 caracteres.")]
        [MaxLength(16, ErrorMessage = "La contraseña tiene como máximo 16 caracteres.")]
        // 6 is minimum length and 45 is maximum length for a password
        [RegularExpression(@"^(?=.{8,16}$)(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[^a-zA-Z0-9]).*", ErrorMessage = "La contraseña debe tener una letra en mayúscula, un caracter especial, un número, y debe tener mínimo 8 caracteres y máximo 16.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "El campo Confirmar Contraseña es requerido.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Contraseña")]
        [MinLength(8, ErrorMessage = "La contraseña debe contener como mínimo 8 caracteres.")]
        [MaxLength(16, ErrorMessage = "La contraseña tiene como máximo 16 caracteres.")]
        [Compare("Password", ErrorMessage = "La contraseña y la confirmación no coinciden.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "El campo Nombre de Usuario es requerido.")]
        [StringLength(50, ErrorMessage = "El campo Nombre de Usuario debe tener una longitud máxima de 50 caracteres.")]
        [Display(Name = "Nombre de Usuario")]
        [System.Web.Mvc.Remote("UserUsernameAlreadyExistsAsync", "Account", ErrorMessage = "Ya existe un usuario con el mismo nombre")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "El campo Teléfono es requerido.")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(\+?\d{1,3}[ ]?)?([0-9]{3})?[ ]?([0-9]{3})[ ]?([0-9]{4})$", ErrorMessage = "El formato del campo Teléfono no es válido.")]
        [Display(Name = "Teléfono")]
        public string PhoneNumber { get; set; }

        //[Required(ErrorMessage = "El campo Puesto de Trabajo es requerido.")]
        [Display(Name ="Puesto de Trabajo")]
        public Nullable<int> PositionID { get; set; }

        [Required(ErrorMessage =  "El campo Fecha de Nacimiento es requerido.")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name ="Fecha de Nacimiento")]
        public DateTime? DateOfBirth { get; set; }

        [Required(ErrorMessage = "El campo Imagen es requerido.")]
        [Display(Name = "Imagen")]
        public string PicturePath { get; set; }
        public int Peso_PicturePath { get; set; }
        public string File_PicturePath { get; set; }

        [Required(ErrorMessage = "El campo Perfil es requerido.")]
        [Display(Name = "Perfil")]
        public string role { get; set; }
        public List<RadioButtonListItem> rolesList { get; set; }

        [Required(ErrorMessage = "El campo Afiliados es requerido.")]
        [Display(Name = "Afiliados")]
        public string afiliados { get; set; }
        public List<CheckBoxListItem> afiliadosListChecks { get; set; }
        public List<RadioButtonListItem> afiliadosListRadios { get; set; }

        public string Id { get; set; }
        [Display(Name = "¿Email Confirmado?")]
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        [Display(Name = "¿Teléfono Confirmado?")]
        public bool PhoneNumberConfirmed { get; set; }
        [Display(Name = "¿Dos Factores de Confirmación Activado?")]
        public bool TwoFactorEnabled { get; set; }
        [Display(Name = "Fecha Final de Bloqueo")]
        public Nullable<System.DateTime> LockoutEndDateUtc { get; set; }
        [Display(Name = "¿Bloqueo Activado?")]
        public bool LockoutEnabled { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "El campo Cantidad de Accesos Fallidos no puede ser negativo.")]
        [Display(Name = "Cantidad de Accesos Fallidos")]
        public int AccessFailedCount { get; set; }
        public int newId { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "El campo Email es requerido.")]
        [EmailAddress(ErrorMessage = "El formato del campo Email es inválido.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        [Required(ErrorMessage = "El campo Contraseña es requerido.")]
        [MinLength(8, ErrorMessage = "La contraseña debe contener como mínimo 8 caracteres.")]
        [MaxLength(16, ErrorMessage = "La contraseña tiene como máximo 16 caracteres.")]
        [RegularExpression(@"((?=.*\d)(?=.*[A-Z])(?=.*\W).{8,16})", ErrorMessage = "La contraseña debe tener una letra en mayúscula, un caracter especial, un número, y debe tener mínimo 8 caracteres y máximo 16.")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Contraseña")]
        [MinLength(8, ErrorMessage = "La contraseña debe contener como mínimo 8 caracteres.")]
        [MaxLength(16, ErrorMessage = "La contraseña tiene como máximo 16 caracteres.")]
        [Compare("Password", ErrorMessage = "La contraseña y la confirmación no coinciden.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "El campo Email es requerido.")]
        [EmailAddress(ErrorMessage = "El formato del campo Email es inválido.")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
