using GoPS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GoPS.ViewModels
{
    public class ConfiguracionesViewModel
    {
        [Required(ErrorMessage = "El campo Offset UTC es requerido.")]
        [RegularExpression(@"^(0[0-9]|1[0-2]):00$", ErrorMessage = "El formato del campo Offset UTC no es válido.")]
        [Display(Name = "Offset UTC")]
        public string OffsetUTC { get; set; }

        [Required(ErrorMessage = "El campo Tiempo de Confirmación es requerido.")]
        [Range(1, int.MaxValue, ErrorMessage = "El campo Tiempo de Confirmación debe ser mayor a cero.")]
        [Display(Name = "Tiempo de Confirmación")]
        public int tiempoConfirmacion { get; set; }

        [Required(ErrorMessage = "El campo Teléfono Base es requerido.")]
        [RegularExpression(@"^(\+?\d{1,3}[ ]?)?([0-9]{3})?[ ]?([0-9]{3})[ ]?([0-9]{4})$", ErrorMessage = "El formato del campo Teléfono Base no es válido.")]
        [Display(Name = "Teléfono Base")]
        public string telefonoBase { get; set; }

        [Required(ErrorMessage = "El campo Horas UTC es requerido.")]
        [Range(1, int.MaxValue, ErrorMessage = "El campo Horas UTC debe ser mayor a cero.")]
        [Display(Name = "Horas UTC")]
        public int horasUTC { get; set; }

        [Required(ErrorMessage = "El campo Empresa es requerido.")]
        [StringLength(100, ErrorMessage = "El campo Empresa debe tener una longitud máxima de 100 caracteres.")]
        [Display(Name = "Empresa")]
        public string Empresa { get; set; }

        [Required(ErrorMessage = "El campo Tiempo de Sincronización es requerido.")]
        [Range(1, int.MaxValue, ErrorMessage = "El campo Tiempo de Sincronización debe ser mayor a cero.")]
        [Display(Name = "Tiempo de Sincronización")]
        public int TiempoSync { get; set; }

        [Required(ErrorMessage = "El campo Logo es requerido.")]
        [StringLength(100, ErrorMessage = "El campo Logo debe tener una longitud máxima de 100 caracteres.")]
        [Display(Name = "Logo")]
        public string Logo { get; set; }
        public int Peso_Logo { get; set; }
        public string File_Logo { get; set; }

        [Required(ErrorMessage = "El campo Color es requerido.")]
        [RegularExpression("^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$", ErrorMessage = "El formato del campo Color no es válido.")]
        [Display(Name = "Color")]
        public string Color { get; set; }

        [Required(ErrorMessage = "El campo IVA es requerido.")]
        [Range(1, int.MaxValue, ErrorMessage = "El campo IVA debe ser mayor a cero.")]
        [Display(Name = "IVA")]
        public int IVA { get; set; }

        [Required(ErrorMessage = "El campo Correo Contacto es requerido.")]
        [StringLength(100, ErrorMessage = "El campo Correo Contacto debe tener una longitud máxima de 100 caracteres.")]
        [Display(Name = "Correo Contacto")]
        public string correoContacto { get; set; }

        [Required(ErrorMessage = "El campo Correo Factura es requerido.")]
        [StringLength(100, ErrorMessage = "El campo Correo Factura debe tener una longitud máxima de 100 caracteres.")]
        [Url(ErrorMessage = "El formato del campo Correo Factura no es válido.")]
        [Display(Name = "Correo Factura")]
        public string correoFacturaUrl { get; set; }

        [Required(ErrorMessage = "El campo EndPoint App Cliente es requerido.")]
        [StringLength(100, ErrorMessage = "El campo EndPoint App Cliente debe tener una longitud máxima de 100 caracteres.")]
        [Url(ErrorMessage = "El formato del campo EndPoint App Cliente no es válido.")]
        [Display(Name = "EndPoint App Cliente")]
        public string endpointAppCliente { get; set; }

        [Required(ErrorMessage = "El campo Url Términos y Condicioneses requerido.")]
        [StringLength(100, ErrorMessage = "El campo Url Términos y Condiciones debe tener una longitud máxima de 100 caracteres.")]
        [Url(ErrorMessage = "El formato del campo Url Términos y Condiciones no es válido.")]
        [Display(Name = "Url Términos y Condiciones")]
        public string urlTerminosCondiciones { get; set; }

        public ConfiguracionesViewModel(List<Configuraciones> variables)
        {
            OffsetUTC = variables.Where(v => v.Atributo.ToLower() == "offsetutc").FirstOrDefault().Valor;
            tiempoConfirmacion = int.Parse(variables.Where(v => v.Atributo.ToLower() == "tiempoconfirmacion").FirstOrDefault().Valor);
            telefonoBase = variables.Where(v => v.Atributo.ToLower() == "telefonobase").FirstOrDefault().Valor;
            horasUTC = int.Parse(variables.Where(v => v.Atributo.ToLower() == "horasutc").FirstOrDefault().Valor);
            Empresa = variables.Where(v => v.Atributo.ToLower() == "empresa").FirstOrDefault().Valor;
            TiempoSync = int.Parse(variables.Where(v => v.Atributo.ToLower() == "tiemposync").FirstOrDefault().Valor);
            Logo = variables.Where(v => v.Atributo.ToLower() == "logo").FirstOrDefault().Valor;
            Color = variables.Where(v => v.Atributo.ToLower() == "color").FirstOrDefault().Valor;
            IVA = int.Parse(variables.Where(v => v.Atributo.ToLower() == "iva").FirstOrDefault().Valor);
            correoContacto = variables.Where(v => v.Atributo.ToLower() == "correocontacto").FirstOrDefault().Valor;
            correoFacturaUrl = variables.Where(v => v.Atributo.ToLower() == "correofacturaurl").FirstOrDefault().Valor;
            endpointAppCliente = variables.Where(v => v.Atributo.ToLower() == "endpointappcliente").FirstOrDefault().Valor;
            urlTerminosCondiciones = variables.Where(v => v.Atributo.ToLower() == "urlterminoscondiciones").FirstOrDefault().Valor;
        }

    }
}