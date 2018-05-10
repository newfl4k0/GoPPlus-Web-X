using GoPS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GoPS.ViewModels
{
    public class VehiculosViewModel
    {
        private GoPSEntities db = new GoPSEntities();
        public IList<GoPS.Models.Vehiculos> vehiculos;

        public VehiculosViewModel(IList<GoPS.Models.Vehiculos> vehiculos)
        {
            this.vehiculos = vehiculos;
        }

        public string ObtenerServiciosMensuales(int id)
        {
            int cant = db.ObtenerDespachosMensualesPorVehiculo(id);
            return cant > 0 ? cant.ToString() : "0";
        }

        public string ObtenerServiciosAnuales(int id)
        {
            int cant = db.ObtenerDespachosAnualesPorVehiculo(id);
            return cant > 0 ? cant.ToString() : "0";
        }

    }
}