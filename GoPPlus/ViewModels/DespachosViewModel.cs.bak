using GoPS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GoPS.ViewModels
{
    public class DespachosObject
    {
        public string Abordo { get; set; }
        public string Finalizacion { get; set; }
        public string Costo { get; set; }
        public string Calificacion { get; set; }
        public string Observaciones { get; set; }
        public string Transporte_Ejecutivo { get; set; }        
        public string Cancelacion { get; set; }
        public string Origen { get; set; }
        public string Estatus { get; set; }
        public string Vehiculo { get; set; }
        public string Conductor { get; set; }
        public int ID_Conductor { get; set; }
        public int Atraso { get; set; }
        public string Cliente { get; set; }
        public string ClassName { get; set; }        
    }

    public class DespachosViewModel
    {
        //[Display(Name = "EstatusVehiculo")]
        public string search { get; set; }
        public string estatusVehiculo { get; set; }
        public List<DespachosObject> despachos { get; set; }
        public List<RadioButtonListItem> estatusVehiculosList { get; set; }
    }
}