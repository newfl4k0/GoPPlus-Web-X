using GoPS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GoPS.ViewModels
{
    public class ClientesAbonadosViewModel
    {
        private GoPSEntities db = new GoPSEntities();
        public IList<GoPS.Models.ClientesAbonados> clientesAbonados;

        public ClientesAbonadosViewModel(IList<GoPS.Models.ClientesAbonados> clientesAbonados)
        {
            this.clientesAbonados = clientesAbonados;
        }

        public string ObtenerServiciosMensuales(int id)
        {
            int cant = db.ObtenerDespachosMensualesPorClienteAbonado(id);
            return cant > 0 ? cant.ToString() : "0";
        }

        public string ObtenerServiciosAnuales(int id)
        {
            int cant = db.ObtenerDespachosAnualesPorClienteAbonado(id);
            return cant > 0 ? cant.ToString() : "0";
        }

        public string ObtenerUltimoServicio(int id)
        {
            Despachos desp = db.ClientesAbonados.Find(id).Reservas.SelectMany(d => d.Despachos).OrderByDescending(c => c.Fecha).FirstOrDefault();
            return desp != null ? desp.Fecha.ToShortDateString() : "";
        }

    }
}