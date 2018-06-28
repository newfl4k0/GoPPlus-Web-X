using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace GoPS.ViewModels
{
    public class IndexViewModel
    {
        public bool HasPassword { get; set; }
        public IList<UserLoginInfo> Logins { get; set; }
        public string PhoneNumber { get; set; }
        public bool TwoFactor { get; set; }
        public bool BrowserRemembered { get; set; }
    }

    public class ManageLoginsViewModel
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; }
        public IList<AuthenticationDescription> OtherLogins { get; set; }
    }

    public class FactorViewModel
    {
        public string Purpose { get; set; }
    }

    public class SetPasswordViewModel
    {
        [DataType(DataType.Password)]
        [Display(Name = "Nueva Contraseña")]
        [Required(ErrorMessage = "El campo Nueva Contraseña es requerido.")]
        [RegularExpression(@"((?=.*\d)(?=.*[A-Z])(?=.*\W).{6,45})", ErrorMessage = "La nueva contraseña debe tene una letra en mayúscula, un caracter especial y un número.")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Nueva Contraseña")]
        [Compare("NewPassword", ErrorMessage = "La nueva contraseña y la confirmación no coinciden.")]
        public string ConfirmPassword { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña Actual")]
        [Required(ErrorMessage = "El campo Contraseña Actual es requerido.")]
        public string OldPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Nueva Contraseña")]
        [MinLength(8, ErrorMessage ="La contraseña debe contener como mínimo 8 caracteres.")]
        [MaxLength(16, ErrorMessage ="La contraseña tiene como máximo 16 caracteres.")]
        [Required(ErrorMessage = "El campo Nueva Contraseña es requerido.")]
        [RegularExpression(@"((?=.*\d)(?=.*[A-Z])(?=.*\W).{6,45})", ErrorMessage = "La nueva contraseña debe tener mínimo una letra minúscula, una letra mayúscula, un caracter especial y un número.")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Nueva Contraseña")]
        [MinLength(8, ErrorMessage = "La contraseña debe contener como mínimo 8 caracteres.")]
        [MaxLength(16, ErrorMessage = "La contraseña tiene como máximo 16 caracteres.")]
        [Compare("NewPassword", ErrorMessage = "La nueva contraseña y la confirmación no coinciden.")]
        public string ConfirmPassword { get; set; }
    }

    public class AddPhoneNumberViewModel
    {
        [Required(ErrorMessage = "El campo Teléfono es requerido.")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(\+?\d{1,3}[ ]?)?([0-9]{3})?[ ]?([0-9]{3})[ ]?([0-9]{4})$", ErrorMessage = "El formato del campo Teléfono no es válido.")]
        [Display(Name = "Teléfono")]
        public string Number { get; set; }
    }

    public class VerifyPhoneNumberViewModel
    {
        [Required(ErrorMessage = "El campo Código es requerido.")]
        [Display(Name = "Código")]
        public string Code { get; set; }

        [Required(ErrorMessage = "El campo Teléfono es requerido.")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(\+?\d{1,3}[ ]?)?([0-9]{3})?[ ]?([0-9]{3})[ ]?([0-9]{4})$", ErrorMessage = "El formato del campo Teléfono no es válido.")]
        [Display(Name = "Teléfono")]
        public string PhoneNumber { get; set; }
    }

    public class ConfigureTwoFactorViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
    }
}