using GoPS.Classes;
using GoPS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GoPS.ViewModels
{
    public class FlotasViewModel
    {
        Utilities util = new Utilities();
        public Flotas flotas;
        public Dictionary<Conductores, SelectList> vehiculosAsignados;
        public List<Conductores> conductores;
        public Dictionary<Conductores, List<Despachos>> servicios;

        public FlotasViewModel(Flotas flotas, string action)
        {
            this.flotas = flotas;
            if (action == "assign")
            {
                this.vehiculosAsignados = new Dictionary<Conductores, SelectList>();
                AsignarData();
            }
            if (action == "liquidate")
            {
                this.conductores = new List<Conductores>();
                LiquidarData();
            }
            if (action == "transfer")
            {
                this.servicios = new Dictionary<Conductores, List<Despachos>>();
                TransferirData();
            }
        }

        private void AsignarData()
        {
            foreach (Conductores conductores in this.flotas.Conductores)
            {
                int sel = conductores.Vehiculos_Conductores.Where(vc => vc.Activo).OrderByDescending(vc => vc.Fecha_Asignacion).Select(vc => vc.ID_Vehiculo).FirstOrDefault();
                SelectList select = new SelectList(this.flotas.Vehiculos, "ID_Vehiculo", "Matricula", sel);
                this.vehiculosAsignados.Add(conductores, select);
            }
        }

        private void LiquidarData()
        {
            this.conductores = this.flotas.Conductores.ToList();
        }

        private void TransferirData()
        {
            DateTime fecha_inicio, fecha_fin;
            ObtenerRangoFechas(out fecha_inicio, out fecha_fin);

            foreach (Conductores conductores in this.flotas.Conductores)
            {
                List<Despachos> despachos = new List<Despachos>();

                Vehiculos_Conductores vh = conductores.Vehiculos_Conductores.Where(vc => vc.Activo).FirstOrDefault();
                if (vh != null)
                {
                    
                }
                despachos = vh.Despachos.Where(d => d.Fecha >= fecha_inicio && d.Fecha <= fecha_fin
                                                && d.Reservas.Estatus_Reserva.Nombre.ToUpper() == "FINALIZADO"
                                                && d.Reservas.Operadores.ID_Afiliado == this.flotas.ID_Afiliado).ToList();
                if (despachos.Count > 0)
                    this.servicios.Add(conductores, despachos);
            }
        }

        private void ObtenerRangoFechas(out DateTime fecha_inicio, out DateTime fecha_fin)
        {
            DateTime hoy = util.ConvertToMexicanDate(DateTime.Now).Date;
            switch (this.flotas.Afiliados.FrecuenciasPago.Nombre.ToUpper())
            {
                case "DIARIO":
                    fecha_inicio = hoy;
                    fecha_fin = fecha_inicio.AddHours(23).AddMinutes(59);
                    break;
                case "SEMANAL":
                    fecha_inicio = StartOfWeek(hoy);
                    fecha_fin = fecha_inicio.AddDays(6).AddHours(23).AddMinutes(59);
                    break;
                case "MENSUAL":
                    fecha_inicio = new DateTime(hoy.Year, hoy.Month, 1);
                    fecha_fin = new DateTime(hoy.Year, hoy.Month,
                                    DateTime.DaysInMonth(hoy.Year, hoy.Month)).AddHours(23).AddMinutes(59);
                    break;
                case "ANUAL":
                    fecha_inicio = new DateTime(hoy.Year, 1, 1);
                    fecha_fin = new DateTime(hoy.Year, 12, 31).AddHours(23).AddMinutes(59);
                    break;
                default:
                    fecha_inicio = fecha_fin = hoy;
                    break;
            }
        }

        public static DateTime StartOfWeek(DateTime today)
        {
            System.Globalization.CultureInfo ci = System.Threading.Thread.CurrentThread.CurrentCulture;
            DayOfWeek fdow = ci.DateTimeFormat.FirstDayOfWeek;
            return today.AddDays(-(today.DayOfWeek - fdow)).Date;
        }

    }
}