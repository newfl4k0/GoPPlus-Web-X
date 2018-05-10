using GoPS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
    public class CiudadesController : _GeneralController
    {
        public JsonResult ObtenerCiudades(int ID_Estado)
        {
            var ciudades = new SelectList(db.Ciudades.Where(c => c.ID_Estado == ID_Estado).OrderBy(o => o.Poblacion), "ID_Ciudad", "Poblacion");
            return Json(ciudades, JsonRequestBehavior.AllowGet);
        }
    }
}