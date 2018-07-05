using GoPS.Classes;
using GoPS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GoPS.ViewModels
{
    public class RutasViewModel
    {
        public List<Vehiculos> Unidades;
        public List<Conductores> Conductores;
        public List<Conductores> TrazaRutaConductores;
        public List<Vehiculos> TrazaRutaUnidades;
        [DisplayName("Fecha Inicio")]
        public DateTime? FechaInicio
        {
            get;

            set;
        }
        [DisplayName("Fecha Fin")]
        public DateTime? FechaFin
        {
            get;

            set;
        }

    }
}