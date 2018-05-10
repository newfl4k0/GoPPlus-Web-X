using GoPS.Classes;
using GoPS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GoPS.ViewModels
{
    public class TurnosViewModel
    {
        Utilities util = new Utilities();
        public IList<GoPS.Models.Turnos> turnos;

        public TurnosViewModel(IList<GoPS.Models.Turnos> turnos)
        {
            this.turnos = turnos;
        }

        public List<CheckBoxListItem> ObtenerDiasList(string dias)
        {
            return util.ObtenerDiasList(dias, true);
        }

    }
}