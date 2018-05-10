﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GoPS.Models;
using GoPS.ViewModels;
using GoPS.Classes;
using GoPS.CustomFilters;
using GoPS.Filters;

namespace GoPS.Controllers
{
    [RedirectOnError]
    [OutputCache(NoStore = true, Duration = 0)]
    [EncryptedActionParameter]
    [ValidateInput(false)]
    public class DespachosController : Controller
    {
        private GoPSEntities db = new GoPSEntities();
        private Utilities util = new Utilities();

        // GET: Despachos
        [HasPermission("Monitoreo_Visualizacion")]
        public ActionResult Index()
        {
            string estatus = "0";
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.MostrarAfiliados = ID_Afiliados.Count > 1;
            int ID_Afiliado = ID_Afiliados.Count == 1 ? ID_Afiliados.FirstOrDefault() : 0;
            ViewBag.ID_Afiliado = new SelectList(db.Afiliados.ToList().Where(c => ID_Afiliados.Contains(c.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_Afiliado", "Nombre", ID_Afiliado);
            DespachosViewModel despachosViewModel = LoadDespachos(ID_Afiliado, estatus, "");
            return View(despachosViewModel);
        }

        [HasPermission("Monitoreo_Visualizacion")]
        [HttpPost]
        public ActionResult Index(FormCollection collection)
        {
            string search = collection["search-table"].ToString();
            string estatus = collection["estatusVehiculo"].ToString();
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.MostrarAfiliados = ID_Afiliados.Count > 1;
            int ID_Afiliado = String.IsNullOrEmpty(collection["ID_Afiliado"].ToString()) ? 0 : Int32.Parse(collection["ID_Afiliado"].ToString());
            ViewBag.ID_Afiliado = new SelectList(db.Afiliados.ToList().Where(c => ID_Afiliados.Contains(c.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_Afiliado", "Nombre", ID_Afiliado);
            DespachosViewModel despachosViewModel = LoadDespachos(ID_Afiliado, estatus, search);
            return View(despachosViewModel);
        }

        private DespachosViewModel LoadDespachos(int ID_Afiliado, string estatus, string search)
        {
            int? ID_estatus = Int32.Parse(estatus);
            List<ObtenerDespachosPorEstatus_Result> despachos = db.ObtenerDespachosPorEstatus(ID_Afiliado, ID_estatus == 0 ? null : ID_estatus).ToList();
            DespachosViewModel despachosViewModel = new DespachosViewModel();
            List<DespachosObject> despachosObject = despachos.Select(d => new DespachosObject
            {
                Abordo = (d.Abordo.HasValue ? d.Abordo.Value : DateTime.Now).ToString("dd-MM-yyyy hh:mm"),
                Finalizacion = (d.Finalizacion.HasValue ? d.Finalizacion.Value : DateTime.Now).ToString("dd-MM-yyyy hh:mm"),
                Costo = d.Costo.HasValue ? d.Costo.Value.ToString() : "",
                Calificacion = d.Calificacion.HasValue ? d.Calificacion.Value.ToString() : "",
                Transporte_Ejecutivo = d.Transporte_Ejecutivo,
                Cancelacion = d.Cancelacion,
                Origen = d.Origen,
                Estatus = d.Estatus,
                Vehiculo = d.Vehiculo,
                Conductor = d.Conductor,
                ID_Conductor = d.ID_Conductor,
                Atraso = d.Atraso.HasValue ? d.Atraso.Value : 0,
                Cliente = d.Cliente,
                Observaciones = d.Observaciones,
                ClassName = ((!d.Atraso.HasValue || d.Atraso <= 5) ? "atraso-white" : (d.Atraso > 5 && d.Atraso < 15 ? "atraso-yellow" : "atraso-red"))
                
            }).ToList();
            despachosViewModel.search = search;
            despachosViewModel.estatusVehiculo = estatus;
            despachosViewModel.despachos = despachosObject;
            despachosViewModel.estatusVehiculosList = util.ObtenerRadioButtonEstatusList(despachosViewModel.estatusVehiculo);
            return despachosViewModel;
        }

        // GET: Despachos/MoreDetails/5
         
        public ActionResult MoreDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Despachos despachos = db.Despachos.Find(id);
            if (despachos == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
            }
            return PartialView("MoreDetails", despachos);
        }
    }
}