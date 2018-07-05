using GoPS.Classes;
using GoPS.CustomFilters;
using GoPS.Models;
using GoPS.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using GoPS.Filters;

namespace GoPS.Controllers
{
    [RedirectOnError]
    [OutputCache(NoStore = true, Duration = 0)]
    [EncryptedActionParameter]
    [ValidateInput(false)]
    public class HomeController : _GeneralController
    {
        

        public List<DashboardGrafico_Result> GetGraphicData()
        {
            List<DashboardGrafico_Result> data = db.DashboardGrafico().ToList();
            return data; 
        }

        public List<DashboardGrafico2_Result> GetGraphicData2()
        {
            List<DashboardGrafico2_Result> data = db.DashboardGrafico2().ToList();
            return data;
        }

        [HasPermission("Dashboard")]
        public ActionResult Index()
        {
            var nuevos_servicios = (db.DashboardNuevosServicios().FirstOrDefault() ?? 0).ToString();
            ViewBag.Nuevos_Servicios = nuevos_servicios;
            var servicios_finalizados = (db.DashboardServiciosFinalizados().FirstOrDefault() ?? 0).ToString();
            ViewBag.Servicios_Finalizados = servicios_finalizados;
            var servicios_cancelados = (db.DashboardServiciosCancelados().FirstOrDefault() ?? 0).ToString();
            ViewBag.Servicios_Cancelados = servicios_cancelados;
            var unidades_activas = (db.DashboardUnidadesActivas_0().FirstOrDefault() ?? 0).ToString();
            ViewBag.Unidades_Activas = unidades_activas;
            var calificacion_servicios = (db.DashboardCalificacionServicios().FirstOrDefault() ?? 0).ToString();
            ViewBag.Calificacion_Servicios = calificacion_servicios;
            var comentarios_recibidos =  (db.DashboardComentariosRecibidos().FirstOrDefault() ?? 0).ToString();
            ViewBag.Comentarios_Recibidos = comentarios_recibidos;
            var usuarios_frecuentes = (db.DashboardUsuariosFrecuentes().FirstOrDefault() ?? 0).ToString();
            ViewBag.Usuarios_Frecuentes = usuarios_frecuentes;
            var usuarios_esporadicos = (db.DashboardUsuariosEsporadicos().FirstOrDefault() ?? 0).ToString();
            ViewBag.Usuarios_Esporadicos = usuarios_esporadicos;
            var ocupados = (db.DashboardUnidadesOcupadas().FirstOrDefault() ?? 0).ToString(); 
            ViewBag.Unidades_Ocupadas = ocupados;
            var ausentes = (db.DashboardUnidadesAusentes().FirstOrDefault() ?? 0).ToString();
            ViewBag.Unidades_Ausentes = ausentes;
            var asignados = (db.DashboardUnidadesAsignadas().FirstOrDefault() ?? 0).ToString();
            ViewBag.Unidades_Asignadas = asignados;
            var deslibres = (db.DashboardServiciosLibres().FirstOrDefault() ?? 0).ToString();            
            ViewBag.Servicios_Libres = deslibres;
			List<DashboardGrafico_Result> res = GetGraphicData();
            List<DashboardGrafico2_Result> res2 = GetGraphicData2();
            StoreFormat4Graphic(res);
            StoreFormat4Graphic2(res2);
            return View();
 }

		public void StoreFormat4Graphic(List<DashboardGrafico_Result> res)
        {            
            string ejey = "";
            string meses = "";
            for (int a = 0; a < res.Count(); a++)
            {
            
                ejey = (a == 0 ? "" : ejey + ",") + res[a].Servicios;
                meses = (a==0 ? "" : meses + ",") + res[a].Mes;
            }
            
            ViewBag.data1 = ejey;
            ViewBag.data0 = meses;

        }

        public void StoreFormat4Graphic2(List<DashboardGrafico2_Result> res)
        {

            string ejey = "";
            string meses = "";
            for (int a = 0; a < res.Count(); a++)
            {

                ejey = (a == 0 ? "" : ejey + ",") + res[a].Servicios;
                meses = (a == 0 ? "" : meses + ",") + res[a].Mes;
            }

            ViewBag.data2 = ejey;

        }

        [HasPermission("Mapas_Visualizacion")]
        public ActionResult About()
        {
            ViewBag.Message = "Página de descripción de la aplicación";
            return View();
        }

        [HasPermission("Clientes_Visualizacion")]
        public ActionResult Contact()
        {
            ViewBag.Message = "Página de Contacto";
            return View();
        }

        public JsonResult GetTableTiposVehiculos(int id_afiliado, string id_tiposveh)
        {
            if (id_afiliado > 0)
            {
                var Table = db.ObtenerTablaSegTiposVehiculos(id_afiliado, id_tiposveh).ToList();
                return Json(Table, JsonRequestBehavior.AllowGet);
            }
            else
                return null;
        }

        public JsonResult GetTableLocalizacion(int id_afiliado, string vehiculo, DateTime fecha)
        {
            if (id_afiliado > 0)
            {
                var Table = db.ObtenerTablaSegLocalizacion(id_afiliado, 
                    String.IsNullOrEmpty(vehiculo) ? null : vehiculo, fecha).ToList();
                return Json(Table, JsonRequestBehavior.AllowGet);
            }
            else
                return null;
        }

        public JsonResult GetTableHistorial(int id_afiliado, string vehiculo, DateTime fecha, string hora_desde, string hora_hasta, int? despacho, int? colonia, int? km)
        {
            if (id_afiliado > 0)
            {
                var Table = db.ObtenerTablaSegHistorialAvanzado(id_afiliado,
                    String.IsNullOrEmpty(vehiculo) ? null : vehiculo, fecha,
                    String.IsNullOrEmpty(hora_desde) ? null : hora_desde,
                    String.IsNullOrEmpty(hora_hasta) ? null : hora_hasta,
                    despacho,
                    km.HasValue ? colonia : null,
                    km).ToList();
                return Json(Table, JsonRequestBehavior.AllowGet);
            }
            else
                return null;
        }

        public JsonResult GetLocationsVehiculo(int id_afiliado, string vehiculo, DateTime fecha, string hora_desde, string hora_hasta)
        {
            if (id_afiliado > 0)
            {
                List<ObtenerUbicacionesVehiculo_Result> Locations = db.ObtenerUbicacionesVehiculo(id_afiliado, vehiculo, fecha,
                    String.IsNullOrEmpty(hora_desde) ? null : hora_desde,
                    String.IsNullOrEmpty(hora_hasta) ? null : hora_hasta).ToList();
                Locations.Select(l => { l.Icon = Path.Combine(Url.Content("~/images/Uploads/"), l.Icon); return l; }).ToList();
                return Json(Locations, JsonRequestBehavior.AllowGet);
            }
            else
                return null;
        }

        public JsonResult GetLocation(bool ocultar, int id_afiliado, string id_tiposveh, string vehiculo)
        {
            if (id_afiliado > 0)
            {
                List<ObtenerListadoSeguimientosFiltrado_Result> Locations = db.ObtenerListadoSeguimientosFiltrado(ocultar, id_afiliado, id_tiposveh, String.IsNullOrEmpty(vehiculo) ? null : vehiculo).ToList();
                Locations.Select(l => { l.Icon = Path.Combine(Url.Content("~/images/Uploads/"), l.Icon); return l; }).ToList();
                return Json(Locations, JsonRequestBehavior.AllowGet);
            }
            else
                return null;
        }

        public JsonResult GetRoutes(int id_afiliado, string id_tiposveh, string vehiculo)
        {
            if (id_afiliado > 0)
            {
                var Locations = db.ObtenerRutasSeguimientosFiltrado(id_afiliado, id_tiposveh,
                String.IsNullOrEmpty(vehiculo) ? null : vehiculo).Select(
                    x => new
                    {
                        x.Latitud_Destino,
                        x.Longitud_Destino,
                        x.Longitud_Origen,
                        x.Latitud_Origen,
                        x.Ubicaciones
                    }).ToList();
                return Json(Locations, JsonRequestBehavior.AllowGet);
            }
            else
                return null;
        }

        public JsonResult GetEstatus(int? id_afiliado)
        {
            if (id_afiliado.HasValue && id_afiliado.Value > 0)
            {
                Dictionary<string, string> estatus = db.Estatus_Afiliados.Where(ea => ea.ID_Afiliado == id_afiliado.Value)
                                                        .ToDictionary(ea => ea.Estatus.Nombre,
                                                                        ea => Path.Combine(Url.Content("~/images/Uploads/"), ea.Estatus.Imagen));
                return Json(estatus, JsonRequestBehavior.AllowGet);
            }
            else
                return null;
        }

        [HasPermission("Vehiculos_Visualizacion")]
        public ActionResult Vehiculos()
        {
            ViewBag.Message = "Página de Vehículos";
            return View();
        }

        [HasPermission("Monitoreo_Visualizacion")]
        public ActionResult Monitoreo()
        {
            ViewBag.Message = "Página de Monitoreo";
            return View();
        }

        [HasPermission("Monitoreo_Visualizacion")]
        public ActionResult Unidades()
        {
            Utilities util = new Utilities();
            UnidadesViewModel viewModel = new UnidadesViewModel();
            ViewBag.Message = "Página de Unidades y Servicios";
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.MostrarAfiliados = ID_Afiliados.Count > 1;
            int ID_Afiliado = ID_Afiliados.Count == 1 ? ID_Afiliados.FirstOrDefault() : 0;
            ViewBag.ID_Afiliado = new SelectList(db.Afiliados.ToList().Where(c => ID_Afiliados.Contains(c.ID_Afiliado)).OrderBy(o => o.NombreRFC), "ID_Afiliado", "Nombre", ID_Afiliado);
            if (ID_Afiliado > 0)
            {
                Calles calle = db.Afiliados.Find(ID_Afiliado).Calles;
                ViewBag.lat = calle.Latitud;
                ViewBag.lon = calle.Longitud;
                int ID_Ciudad = calle.Colonias.Ciudades.ID_Ciudad;
                ViewBag.hist_colonia = new SelectList(db.Colonias.Where(c => c.ID_Ciudad == ID_Ciudad).OrderBy(o => o.Nombre), "ID_Colonia", "Nombre");
            }
            else
                ViewBag.hist_colonia = new SelectList(Enumerable.Empty<SelectListItem>());
            viewModel.tiposVehiculosList = util.ObtenerTiposVehiculosList();
            return View(viewModel);
        }

        [HasPermission("Monitoreo_Visualizacion")]
        public ActionResult TrazaRuta()
        {
            Utilities util = new Utilities();
            List<Conductores> conductors;
            List<Vehiculos> unidads;
            RutasViewModel viewModel = new RutasViewModel();
            ViewBag.Message = "Vista para trazado de rutas.";
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.MostrarAfiliados = ID_Afiliados.Count > 1;
            int ID_Afiliado = ID_Afiliados.Count == 1 ? ID_Afiliados.FirstOrDefault() : 0;
            ViewBag.ID_Afiliado = new SelectList(db.Afiliados.ToList().Where(c => ID_Afiliados.Contains(c.ID_Afiliado)).OrderBy(o => o.NombreRFC), "ID_Afiliado", "Nombre", ID_Afiliado);
            if (ID_Afiliado > 0)
            {
                Calles calle = db.Afiliados.Find(ID_Afiliado).Calles;
                ViewBag.lat = calle.Latitud;
                ViewBag.lon = calle.Longitud;
                int ID_Ciudad = calle.Colonias.Ciudades.ID_Ciudad;
                ViewBag.hist_colonia = new SelectList(db.Colonias.Where(c => c.ID_Ciudad == ID_Ciudad).OrderBy(o => o.Nombre), "ID_Colonia", "Nombre");
            }
            else
            {
                ViewBag.hist_colonia = new SelectList(Enumerable.Empty<SelectListItem>());                
            }
                
            viewModel.Conductores = db.Conductores.Where(c => c.Habilitado == true).ToList();
            //Render Modelo para Seleccionar Conductores:
            if (ID_Afiliado>0)
            {                
                conductors = (db.Conductores.Include("Vehiculos_Conductores").Include("Flotas").Where(c => c.Habilitado == true)).ToList();
                conductors = conductors.Where(i => i.Flotas.ID_Afiliado == (ID_Afiliado > 0 ? ID_Afiliado : i.Flotas.ID_Afiliado)).ToList();
                viewModel.TrazaRutaConductores = conductors.ToList();
                //Fin Conductores
                //Render Modelo para Seleccionar una Unidad
                unidads = db.Vehiculos.Include("Flotas").Include("Vehiculos_Conductores").ToList();
                unidads = unidads.Where(uu => uu.Habilitado == true && uu.Flotas.ID_Afiliado == (ID_Afiliado > 0 ? ID_Afiliado : uu.Flotas.ID_Afiliado)).ToList();
                viewModel.TrazaRutaUnidades = unidads;
            }
            else
            {
                viewModel.TrazaRutaConductores = new List<Conductores>();
                viewModel.TrazaRutaUnidades = new List<Vehiculos>();
            }
            
            return View(viewModel);
        }

        [HasPermission("Monitoreo_Visualizacion")]
        public ActionResult SelUni(int? idv=0, int? idc=0)
        {
            Utilities util = new Utilities();
            List<Conductores> conductors=new List<Conductores>();
            List <Vehiculos> unidads = new List<Vehiculos>();
            Vehiculos vh = new Vehiculos();
            Conductores co = new Conductores();
            RutasViewModel viewModel = new RutasViewModel();
            if (idv > 0)
            {
                int ID_V = idv.Value;
                vh = db.Vehiculos.Where(ve => ve.Habilitado == true && ve.ID_Vehiculo == ID_V).FirstOrDefault();                
            }
            if (idc > 0)
            {
                int ID_C = idc.Value;
                co = db.Conductores.Where(c => c.ID_Conductor == ID_C).FirstOrDefault();
            }
            conductors.Add(co);
            unidads.Add(vh);
            viewModel.Conductores = conductors;
            viewModel.Unidades = unidads;

            ViewBag.Message = "Vista para trazado de rutas.";
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.MostrarAfiliados = ID_Afiliados.Count > 1;
            int ID_Afiliado = ID_Afiliados.Count == 1 ? ID_Afiliados.FirstOrDefault() : 0;
            ViewBag.ID_Afiliado = new SelectList(db.Afiliados.ToList().Where(c => ID_Afiliados.Contains(c.ID_Afiliado)).OrderBy(o => o.NombreRFC), "ID_Afiliado", "Nombre", ID_Afiliado);
            if (ID_Afiliado > 0)
            {
                Calles calle = db.Afiliados.Find(ID_Afiliado).Calles;
                ViewBag.lat = calle.Latitud;
                ViewBag.lon = calle.Longitud;
                int ID_Ciudad = calle.Colonias.Ciudades.ID_Ciudad;
                ViewBag.hist_colonia = new SelectList(db.Colonias.Where(c => c.ID_Ciudad == ID_Ciudad).OrderBy(o => o.Nombre), "ID_Colonia", "Nombre");
            }
            else
            {
                ViewBag.hist_colonia = new SelectList(Enumerable.Empty<SelectListItem>());
            }
            return View(viewModel);
        }

        // POST: TrazaRuta/Index
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Monitoreo_Visualización")]
        public ActionResult TrazaRuta(int? id)
        {
            RutasViewModel viewModel = new RutasViewModel();

            //copia
            ViewBag.Message = "Vista para trazado de rutas.";
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.MostrarAfiliados = ID_Afiliados.Count > 1;
            int ID_Afiliado = ID_Afiliados.Count == 1 ? ID_Afiliados.FirstOrDefault() : 0;
            ViewBag.ID_Afiliado = new SelectList(db.Afiliados.ToList().Where(c => ID_Afiliados.Contains(c.ID_Afiliado)).OrderBy(o => o.NombreRFC), "ID_Afiliado", "Nombre", ID_Afiliado);
            if (ID_Afiliado > 0)
            {
                Calles calle = db.Afiliados.Find(ID_Afiliado).Calles;
                ViewBag.lat = calle.Latitud;
                ViewBag.lon = calle.Longitud;
                int ID_Ciudad = calle.Colonias.Ciudades.ID_Ciudad;
                ViewBag.hist_colonia = new SelectList(db.Colonias.Where(c => c.ID_Ciudad == ID_Ciudad).OrderBy(o => o.Nombre), "ID_Colonia", "Nombre");
                viewModel.Conductores = db.Conductores.Where(c => c.Habilitado == true).ToList();
                ViewBag.Cond = viewModel.Conductores;
                //Render Modelo para Seleccionar Conductores:
                List<Conductores> conductors;
                conductors = (db.Conductores.Include("Vehiculos_Conductores").Include("Flotas").Where(c => c.Habilitado == true)).ToList();
                conductors = conductors.Where(i => i.Flotas.ID_Afiliado == (ID_Afiliado > 0 ? ID_Afiliado : i.Flotas.ID_Afiliado)).ToList();
                viewModel.TrazaRutaConductores = conductors.ToList();
                //Fin Conductores
                //Render Modelo para Seleccionar una Unidad
                List<Vehiculos> unidads = db.Vehiculos.Include("Flotas").Include("Vehiculos_Conductores").ToList();
                unidads = unidads.Where(uu => uu.Habilitado == true && uu.Flotas.ID_Afiliado == (ID_Afiliado > 0 ? ID_Afiliado : uu.Flotas.ID_Afiliado)).ToList();
                viewModel.TrazaRutaUnidades = unidads;
            }            
            //copia

            return View(viewModel);
        }

        [HasPermission("Configuraciones_Visualizacion")]
        public ActionResult Configuraciones()
        {
            ViewBag.Message = "Página de Configuraciones";
            return View();
        }
    }
}