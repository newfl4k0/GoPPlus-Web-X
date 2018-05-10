using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using GoPS.Models;
using GoPS.CustomFilters;
using Microsoft.AspNet.Identity;
using GoPS.Classes;

namespace GoPS.Controllers
{
    [RedirectOnError]
    [OutputCache(NoStore = true, Duration = 0)]
    [ValidateInput(false)]
    public class ServiciosController : Controller
    {
        private GoPSEntities db = new GoPSEntities();
        Utilities util = new Utilities();

        // GET: Servicios
        [HasPermission("General_Visualizacion")]
        public ActionResult Index()
        {
            var reservas = db.Reservas.Include(r => r.Clientes).Include(r => r.ClientesAbonados).Include(r => r.ClientesHabituales).Include(r => r.Estatus_Reserva).Include(r => r.Operadores).Include(r => r.TiposVehiculos);
            return View(reservas.ToList());
        }

        // GET: Servicios/Details/5
        [HasPermission("General_Visualizacion")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservas reservas = db.Reservas.Find(id);
            if (reservas == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
            }
            return View(reservas);
        }

        // GET: Servicios/Create
        [HasPermission("General_Edicion")]
        public ActionResult Create()
        {
            //decimal latitud = (decimal)20.663320000000000;
            //decimal longitud = (decimal)-103.385500000000000;
            //int radio = 1000;

            //var yourLocation = SqlGeography.Point(double.Parse(latitud.ToString()), double.Parse(longitud.ToString()), 4326);

            //var query = from ubi in db.ObtenerUltimaUbicacionLibres(3).ToList()
            //            let distance = SqlGeography
            //                          .Point(double.Parse(ubi.Latitud.ToString()), double.Parse(ubi.Longitud.ToString()), 4326)
            //                          .STDistance(yourLocation)
            //                          .Value
            //            where distance <= radio
            //            orderby distance
            //            select ubi;

            //var result = query.Distinct().ToList();

            ViewBag.ID_Cliente = new SelectList(db.Clientes.OrderBy(o => o.Nombre), "ID_Cliente", "Nombre");
            ViewBag.ID_ClienteAbonado = new SelectList(db.ClientesAbonados.OrderBy(o => o.Nombre), "ID_ClienteAbonado", "Nombre");
            ViewBag.ID_ClienteHabitual = new SelectList(db.ClientesHabituales.OrderBy(o => o.Nombre), "ID_ClienteHabitual", "Nombre");
            ViewBag.ID_TipoVehiculo = new SelectList(db.TiposVehiculos.OrderBy(o => o.Nombre), "ID_TipoVehiculo", "Nombre");
            ObtenerGeografiaOrigenSelectList();
            ObtenerGeografiaDestinoSelectList();
            return View();
        }

        // POST: Servicios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("General_Edicion")]
        public ActionResult Create([Bind(Include = "ID_Reserva,Fecha,ID_Operador,Latitud_Origen,Longitud_Origen,Latitud_Destino,Longitud_Destino,ID_Estatus_Afiliado,Fecha_Creacion,Fecha_Actualizacion,UserID,ID_Cliente,ID_ClienteHabitual,ID_ClienteAbonado,ID_Calle_Origen,ID_Calle_Destino,Observaciones,ID_Estatus_Reserva,Numero_Exterior_Origen,Numero_Exterior_Destino,Fecha_Finalizacion,ID_TipoVehiculo,Numero_Interior_Origen,Numero_Interior_Destino")] Reservas reservas)
        {
            reservas.ID_Operador = 1;
            reservas.ID_Estatus_Reserva = db.Estatus_Reserva.Where(e => e.Nombre.ToUpper() == "RESERVA").FirstOrDefault().ID_Estatus_Reserva;
            reservas.Fecha = util.ConvertToMexicanDate(DateTime.Now);

            if (ModelState.IsValid)
            {
                //var result = db.AltaServicio(reservas.ID_Operador, reservas.Latitud_Origen, reservas.Longitud_Origen,
                //                            reservas.Latitud_Destino, reservas.Longitud_Destino, reservas.ID_Estatus_Afiliado,
                //                            reservas.ID_Cliente, reservas.ID_ClienteHabitual, reservas.ID_ClienteAbonado,
                //                            reservas.ID_Calle_Origen, reservas.ID_Calle_Destino, reservas.Observaciones,
                //                            reservas.Numero_Interior_Origen, reservas.Numero_Exterior_Origen, reservas.Numero_Interior_Destino,
                //                            reservas.Numero_Exterior_Destino, reservas.ID_Estatus_Reserva, reservas.ID_TipoVehiculo, DateTime.Now);
                reservas.Latitud_Origen = db.Calles.Find(reservas.ID_Calle_Origen).Latitud;
                reservas.Longitud_Origen = db.Calles.Find(reservas.ID_Calle_Origen).Longitud;
                reservas.Latitud_Destino = db.Calles.Find(reservas.ID_Calle_Destino).Latitud;
                reservas.Longitud_Destino = db.Calles.Find(reservas.ID_Calle_Destino).Longitud;
                reservas.Fecha_Creacion = util.ConvertToMexicanDate(DateTime.Now);
                reservas.UserID = User.Identity.GetUserId();
                db.Reservas.Add(reservas);
                db.SaveChanges();
                //validar que no incluya conductores que hayan rechazado la reserva, de ser el caso
                int vehiculo_libre = db.ObtenerIDVehiculoConductorLibre(3, reservas.ID_Reserva).FirstOrDefault() ?? 0;

                if (vehiculo_libre > 0)
                {
                    try
                    {
                        //int result_despacho = db.CrearDespacho(reservas.Fecha, vehiculo_libre, 65, reservas.Observaciones,
                        //                        User.Identity.GetUserId(), reservas.ID_Reserva, null, null,
                        //                        1, null).FirstOrDefault() ?? 0;
                        ViewBag.Success = true;
                    }
                    catch(Exception)
                    {
                        reservas.ID_Estatus_Reserva = 6;
                        db.SaveChanges();
                        ViewBag.Error = true;
                    }
                }
                else
                {
                    reservas.ID_Estatus_Reserva = 6;
                    db.SaveChanges();
                    ViewBag.Error = true;
                }
                ViewBag.ID_Cliente = new SelectList(db.Clientes.OrderBy(o => o.Nombre), "ID_Cliente", "Nombre");
                ViewBag.ID_ClienteAbonado = new SelectList(db.ClientesAbonados.OrderBy(o => o.Nombre), "ID_ClienteAbonado", "Nombre");
                ViewBag.ID_ClienteHabitual = new SelectList(db.ClientesHabituales.OrderBy(o => o.Nombre), "ID_ClienteHabitual", "Nombre");
                ViewBag.ID_TipoVehiculo = new SelectList(db.TiposVehiculos.OrderBy(o => o.Nombre), "ID_TipoVehiculo", "Nombre");
                ObtenerGeografiaOrigenSelectList();
                ObtenerGeografiaDestinoSelectList();
                return View();
            }
            else
            {
                ViewBag.ID_Cliente = new SelectList(db.Clientes.OrderBy(o => o.Nombre), "ID_Cliente", "Nombre", reservas.ID_Cliente);
                ViewBag.ID_ClienteAbonado = new SelectList(db.ClientesAbonados.OrderBy(o => o.Nombre), "ID_ClienteAbonado", "Nombre", reservas.ID_ClienteAbonado);
                ViewBag.ID_ClienteHabitual = new SelectList(db.ClientesHabituales.OrderBy(o => o.Nombre), "ID_ClienteHabitual", "Nombre", reservas.ID_ClienteHabitual);
                ViewBag.ID_TipoVehiculo = new SelectList(db.TiposVehiculos.OrderBy(o => o.Nombre), "ID_TipoVehiculo", "Nombre", reservas.ID_TipoVehiculo);
                ObtenerGeografiaSelectList(reservas);
                return View(reservas);
            }
        }

        private void ObtenerGeografiaSelectList(Reservas reservas)
        {
            if (reservas.ID_Calle_Origen == 0)
                ObtenerGeografiaOrigenSelectList();
            else
            {
                Colonias colonia_origen = db.Calles.Find(reservas.ID_Calle_Origen).Colonias;
                Ciudades ciudad_origen = colonia_origen.Ciudades;
                Estados estado_origen = ciudad_origen.Estados;
                ViewBag.ID_Estado_Origen = new SelectList(db.Estados.OrderBy(o => o.Nombre), "ID_Estado", "Nombre", estado_origen.ID_Estado);
                ViewBag.ID_Ciudad_Origen = new SelectList(estado_origen.Ciudades.OrderBy(o => o.Poblacion), "ID_Ciudad", "Poblacion", ciudad_origen.ID_Ciudad);
                ViewBag.ID_Colonia_Origen = new SelectList(ciudad_origen.Colonias.OrderBy(o => o.Nombre), "ID_Colonia", "Nombre", colonia_origen.ID_Colonia);
                ViewBag.ID_Calle_Origen = new SelectList(colonia_origen.Calles.OrderBy(o => o.Nombre), "ID_Calle", "Nombre", reservas.ID_Calle_Origen);
            }
            if (reservas.ID_Calle_Destino == 0)
                ObtenerGeografiaDestinoSelectList();
            else
            {
                Colonias colonia_destino = db.Calles.Find(reservas.ID_Calle_Destino).Colonias;
                Ciudades ciudad_destino = colonia_destino.Ciudades;
                Estados estado_destino = ciudad_destino.Estados;
                ViewBag.ID_Estado_Destino = new SelectList(db.Estados.OrderBy(o => o.Nombre), "ID_Estado", "Nombre", estado_destino.ID_Estado);
                ViewBag.ID_Ciudad_Destino = new SelectList(estado_destino.Ciudades.OrderBy(o => o.Poblacion), "ID_Ciudad", "Poblacion", ciudad_destino.ID_Ciudad);
                ViewBag.ID_Colonia_Destino = new SelectList(ciudad_destino.Colonias.OrderBy(o => o.Nombre), "ID_Colonia", "Nombre", colonia_destino.ID_Colonia);
                ViewBag.ID_Calle_Destino = new SelectList(colonia_destino.Calles.OrderBy(o => o.Nombre), "ID_Calle", "Nombre", reservas.ID_Calle_Destino);
            }
        }

        private void ObtenerGeografiaOrigenSelectList()
        {
            ViewBag.ID_Estado_Origen = new SelectList(db.Estados.OrderBy(o => o.Nombre), "ID_Estado", "Nombre");
            ViewBag.ID_Ciudad_Origen = new SelectList(db.Ciudades.OrderBy(o => o.Poblacion).Take(0), "ID_Ciudad", "Poblacion");
            ViewBag.ID_Colonia_Origen = new SelectList(db.Colonias.OrderBy(o => o.Nombre).Take(0), "ID_Colonia", "Nombre");
            ViewBag.ID_Calle_Origen = new SelectList(db.Calles.OrderBy(o => o.Nombre).Take(0), "ID_Calle", "Nombre");
        }

        private void ObtenerGeografiaDestinoSelectList()
        {
            ViewBag.ID_Estado_Destino = new SelectList(db.Estados.OrderBy(o => o.Nombre), "ID_Estado", "Nombre");
            ViewBag.ID_Ciudad_Destino = new SelectList(db.Ciudades.OrderBy(o => o.Poblacion).Take(0), "ID_Ciudad", "Poblacion");
            ViewBag.ID_Colonia_Destino = new SelectList(db.Colonias.OrderBy(o => o.Nombre).Take(0), "ID_Colonia", "Nombre");
            ViewBag.ID_Calle_Destino = new SelectList(db.Calles.OrderBy(o => o.Nombre).Take(0), "ID_Calle", "Nombre");
        }

    }
    public class MyObject
    {
        public bool status { get; set; }
        public string message { get; set; }
    }
}
