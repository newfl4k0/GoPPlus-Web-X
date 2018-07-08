using GoPS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;


namespace GoPS.ViewModels
{
    public class RutasViewModel
    {
        public List<Vehiculos> unidades;
        public List<Conductores> conductores;
        public List<Conductores> trazaRutaConductores;
        public List<Vehiculos> trazaRutaUnidades;
        [DisplayName("Fecha Inicio")]
        public DateTime? fechaInicio
        {
            get;

            set;
        }
        [DisplayName("Fecha Fin")]
        public DateTime? fechaFin
        {
            get;

            set;
        }
        public TrazaRutaViewModel trazaRutaListas;

    }
}