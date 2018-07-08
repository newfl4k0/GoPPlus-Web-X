using GoPS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;


namespace GoPS.ViewModels
{
    public class TrazaRutaViewModel
    {
        public Vehiculos unidades;
        public Conductores conductores;
        public List<Seguimientos> seguimientos;
        public List<Seguimiento_Despacho> seguimientos_Despacho;
        public List<List<Seguimiento_Despacho_Detalle>> seguimientos_Despacho_Detalle;
        public List<Despachos> despachos;
        public List<Conexiones> conexiones;
        public List<Estatus> estatus;
        public List<Reservas> reservas;
        public List<Ubicaciones> ubicaciones;
        
    }
}