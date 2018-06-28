using GoPS.CustomFilters;
using GoPS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using GoPS.Classes;
using GoPS.Filters;

namespace GoPS.Controllers
{
    [RedirectOnError]
    [OutputCache(NoStore = true, Duration = 0)]
    [EncryptedActionParameter]
    [ValidateInput(false)]
    public class ConfiguracionController : _GeneralController 
    {
        // GET: Configuracion
        [HasPermission("VariablesConfig_Edicion")]
        public ActionResult Variables()
        {
            List<Configuraciones> variables = db.Configuraciones.ToList();
            foreach (Configuraciones config in variables)
            {
                if (config.TipoDato.ToLower() == "color")
                    config.Valor = config.Valor.Substring(1);
                TempData.Remove(config.Atributo);
            }
            return View(variables);
        }

        [HttpPost]
        [HasPermission("VariablesConfig_Edicion")]
        public ActionResult Variables(FormCollection collection)
        {
            bool valid = true;
            List<Configuraciones> variables = db.Configuraciones.ToList();
            foreach (Configuraciones config in variables)
            {
                string form_value = collection[config.Atributo].ToString();
                config.Valor = form_value;
                config.EsConfiguracionChofer = (Boolean.Parse(collection["Chofer_" + config.Atributo].ToString().Split(',')[0]) ? 1 : 0);
                bool error = false;
                if (String.IsNullOrEmpty(config.Valor) && config.TipoDato.ToLower() != "imagen")
                {
                    TempData[config.Atributo] = "El campo " + config.Atributo + " es requerido.";
                    error = true;
                }
                else
                {
                    if (config.TipoDato.ToLower() == "número")
                    {
                        int valor;
                        bool isInt = int.TryParse(config.Valor, out valor);
                        if(isInt)
                        {
                            if (valor > int.MaxValue)
                            {
                                TempData[config.Atributo] = "El campo " + config.Atributo + " es inválido.";
                                error = true;
                            }
                        }
                    }
                    if (config.TipoDato.ToLower() == "decimal")
                    {
                        decimal valor;
                        bool isDecimal = decimal.TryParse(config.Valor, out valor);
                        if (isDecimal)
                        {
                            if (valor > decimal.Parse("79228162514264337593543950335"))
                            {
                                TempData[config.Atributo] = "El campo " + config.Atributo + " es inválido.";
                                error = true;
                            }
                        }
                    }
                    if (config.TipoDato.ToLower() == "color")
                    {
                        Regex regexColor = new Regex("^([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$");
                        if (!regexColor.IsMatch(config.Valor))
                        {
                            TempData[config.Atributo] = "El formato del campo " + config.Atributo + " es inválido.";
                            error = true;
                        }
                    }
                    if (config.TipoDato.ToLower() == "hora")
                    {
                        Regex regexHora = new Regex("^(0[0-9]|1[0-2]):00$");
                        if (!regexHora.IsMatch(config.Valor))
                        {
                            TempData[config.Atributo] = "El formato del campo " + config.Atributo + " es inválido.";
                            error = true;
                        }
                    }
                    if (config.TipoDato.ToLower() == "teléfono")
                    {
                        Regex regexTelefono = new Regex("^(\\+?\\d{1,3}[ ]?)?([0-9]{3})?[ ]?([0-9]{3})[ ]?([0-9]{4})$");
                        if (!regexTelefono.IsMatch(config.Valor))
                        {
                            TempData[config.Atributo] = "El formato del campo " + config.Atributo + " es inválido.";
                            error = true;
                        }
                    }
                    if (config.TipoDato.ToLower() == "fecha")
                    {
                        DateTime valor = Convert.ToDateTime(config.Valor);
                        if (valor == new DateTime())
                        {
                            TempData[config.Atributo] = "El campo " + config.Atributo + " es inválido.";
                            error = true;
                        }
                    }
                    if (config.TipoDato.ToLower() == "url")
                    {
                        Regex regexUrl = new Regex("^(http:\\/\\/www\\.|https:\\/\\/www\\.|http:\\/\\/|https:\\/\\/)?[a-z0-9]+([\\-\\.]{1}[a-z0-9]+)*\\.[a-z]{2,5}(:[0-9]{1,5})?(\\/.*)?$");
                        if (!regexUrl.IsMatch(config.Valor))
                        {
                            TempData[config.Atributo] = "El formato del campo " + config.Atributo + " es inválido.";
                            error = true;
                        }
                    }
                    if (config.TipoDato.ToLower() == "email")
                    {
                        Regex regexEmail = new Regex("^[a-zA-ZñÑ0-9._-]+@[a-zA-ZñÑ0-9._-]+\\.[a-zA-Z]{2,4}$");
                        if (!regexEmail.IsMatch(config.Valor))
                        {
                            TempData[config.Atributo] = "El formato del campo " + config.Atributo + " es inválido.";
                            error = true;
                        }
                    }
                    if (config.TipoDato.ToLower() == "texto")
                    {
                        Regex regexFraseComaPunto = new Regex("^[a-zA-ZáéíóúñÁÉÍÓÚÑ,. ]*$");
                        if (!regexFraseComaPunto.IsMatch(config.Valor))
                        {
                            TempData[config.Atributo] = "El campo " + config.Atributo + " no permite números, caracteres especiales ni signos de puntuación.";
                            error = true;
                        }
                        config.Valor = System.Text.RegularExpressions.Regex.Replace(config.Valor, @"\s+", " ");
                        config.Valor = config.Valor.TrimStart();
                    }
                    if (config.TipoDato.ToLower() == "string")
                    {
                        
                       
                        config.Valor = config.Valor.TrimStart();
                    }
                    if (config.TipoDato.ToLower() == "imagen")
                    {
                        HttpPostedFileBase imagen = Request.Files["File_" + config.Atributo];
                        if (imagen != null && !String.IsNullOrEmpty(imagen.FileName))
                        {
                            int peso = imagen != null ? imagen.ContentLength : 1000;
                            List<string> extensions = new List<string>() { ".jpg", ".png", ".jpeg" };
                            bool ImagenEsValida = extensions.Contains(Path.GetExtension(config.Valor).ToLower());
                            bool PesoImagenEsValido = (peso > 0) && (peso <= (1024 * 1024 * 1024));
                            GetTempFileNames(config, imagen);
                            if (!ImagenEsValida)
                            {
                                TempData[config.Atributo] = "La extensión de la imagen " + config.Atributo + " debe ser .jpg, .jpeg o .png.";
                                error = true;
                            }
                            if (!PesoImagenEsValido)
                            {
                                TempData[config.Atributo] = "El peso de la imagen " + config.Atributo + " no es válido.";
                                error = true;
                            }
                        }
                        else if (String.IsNullOrEmpty(config.Valor))
                        {
                            TempData[config.Atributo] = "El campo " + config.Atributo + " es requerido.";
                            error = true;
                        }
                    }
                }
                valid = valid && !error;
            }

            if (valid)
            {
                foreach (Configuraciones config in variables)
                {
                    if (config.TipoDato.ToLower() == "color")
                        config.Valor = "#" + config.Valor;
                    if(config.TipoDato.ToLower() == "imagen")
                    {
                        HttpPostedFileBase imagen = Request.Files["File_" + config.Atributo];
                        GetFileNames(config, imagen);
                    }
                    db.Entry(config).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            ViewBag.Success = valid;
            ViewBag.Error = !valid;
            return View(variables);
        }

        private void GetFileNames(Configuraciones variable, HttpPostedFileBase Imagen)
        {
            variable.Valor = util.SaveOrMoveFile(variable.Valor, Imagen, true);
        }

        private void GetTempFileNames(Configuraciones variable, HttpPostedFileBase Imagen)
        {
            string filename = variable.Valor;
            variable.Valor = util.SaveNewTempFile(filename, Imagen, true);
        }
    }
}