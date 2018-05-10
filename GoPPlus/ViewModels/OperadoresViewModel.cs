using GoPS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GoPS.ViewModels
{
    public class OperadoresViewModel
    {
        //private Entities ent = new Entities();
        private GoPSEntities db = new GoPSEntities();
        public IList<GoPS.Models.Operadores> operadores;

        public OperadoresViewModel(IList<GoPS.Models.Operadores> operadores)
        {
            this.operadores = operadores;
        }

        public string ObtenerUsuario(string user)
        {
            return db.AspNetUsers.Find(user).UserName;
        }

        public string ObtenerEstatusUsuario(string user)
        {
            bool? log = db.AspNetUsers.Find(user).IsLoged_in;
            return log.HasValue && log.Value ? "Conectado" : "Desconectado";
        }

        public string ObtenerUltimoLoginUsuario(string user)
        {
            DateTime? date = db.AspNetUsers.Find(user).LastLoginDate;
            return date.HasValue ? date.Value.ToShortDateString() : "" ;
        }

        public string ObtenerUltimoLogoutUsuario(string user)
        {
            DateTime? date = db.AspNetUsers.Find(user).LastLogoutDate;
            return date.HasValue ? date.Value.ToShortDateString() : "";
        }

        public string ObtenerResMensuales(int id)
        {
            int cant = db.ObtenerReservasMensualesPorOperador(id);
            return cant > 0 ? cant.ToString() : "0";
        }

        public string ObtenerResAnuales(int id)
        {
            int cant = db.ObtenerReservasAnualesPorOperador(id);
            return cant > 0 ? cant.ToString() : "0";
        }

        public string ObtenerLlamadasMes(int id)
        {
            int cant = db.ObtenerLlamadasMensualesPorOperador(id);
            return cant > 0 ? cant.ToString() : "0";
        }

    }
}