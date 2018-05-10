using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Linq.Expressions;

namespace GoPS.Models
{
    public partial class Seguimientos
    {
        public bool MostrarEnMapa
        {
            get {
                return Estatus_Afiliados.Estatus.Nombre.ToUpper() != "DESCONECTADO"
                    && Estatus_Afiliados.Estatus.Nombre.ToUpper() != "ASIGNADO";
            }
        }

    }
}