using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace GoPS.Classes
{
    public class StringValidationAttribute : ValidationAttribute, IMetadataAware
    {

        private bool _uppercase;

        public StringValidationAttribute(bool uppercase = false)
        {
            _uppercase = uppercase;
        }

        public void OnMetadataCreated(ModelMetadata metadata)
        {
            metadata.AdditionalValues["Uppercase"] = _uppercase;
        }
    }
}

/*public class StringBinder : IModelBinder
{
    public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
    {
       string valorResult="";

       ValueProviderResult result = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

        if (result == null)
            return null;

        if (bindingContext.ModelMetadata.AdditionalValues.ContainsKey("Uppercase"))
        {
            if ((bool)bindingContext.ModelMetadata.AdditionalValues["Uppercase"])
            {
                return result.AttemptedValue.ToUpper();
            }
        }
        
        string[] valores = ((IEnumerable)result.RawValue)
                                 .Cast<object>()
                                 .Select(x => x.ToString())
                                 .ToArray();

        if (valores.Length == 1)
            valorResult = valores[0];
        else
            valorResult = valores[1];

        return valorResult;
    }

    string ConvertObjectToString(object obj)
    {
        return obj?.ToString() ?? string.Empty;
    }
}*/
