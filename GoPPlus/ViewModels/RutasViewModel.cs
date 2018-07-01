using GoPS.Classes;
using GoPS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GoPS.ViewModels
{
    public class RutasViewModel
    {
        public List<Vehiculos> Unidades;
        public List<Conductores> Conductores;
        public DateTime FechaInicio;
        public DateTime FechaFin;
        
    }
}