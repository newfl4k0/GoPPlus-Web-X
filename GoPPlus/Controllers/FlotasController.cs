using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using GoPS.Models;
using Microsoft.AspNet.Identity;
using GoPS.Classes;
using GoPS.ViewModels;
using GoPS.CustomFilters;
using GoPS.Filters;

namespace GoPS.Controllers
{
    [RedirectOnError]
    [OutputCache(NoStore = true, Duration = 0)]
    [EncryptedActionParameter]
    [ValidateInput(false)]
    public class FlotasController : _GeneralController
    {
        // GET: Flotas
        [HasPermission("Vehiculos_Visualizacion")]
        public ActionResult Index()
        {
            if (TempData["Delete"] != null)
            {
                ViewBag.Delete = true;
                TempData.Remove("Delete");
            }
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            var flotas = db.Flotas.Where(r => ID_Afiliados.Contains(r.ID_Afiliado)).Include(f => f.Bancos).Include(f => f.Calles).Include(f => f.FormasPago).Include(d => d.Afiliados);
            return View(flotas.ToList());
        }

        // GET: Flotas/Details/5

        [HasPermission("Vehiculos_Visualizacion")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Flotas flotas = db.Flotas.Find(id);
            if (flotas == null)
            {
                TempData["Mess"] = MensajeNotFound;
                TempData["NavBar"] = "NavBar_CatFlotas";
                TempData["BackLink"] = "Index";

                return RedirectToAction("ItemNotFound");
            }
            return View(flotas);
        }

        // GET: Flotas/Create
        [HasPermission("Vehiculos_Edicion")]
        public ActionResult Create()
        {
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.MostrarAfiliados = ID_Afiliados.Count > 1;
            int ID_Afiliado = ID_Afiliados.Count == 1 ? ID_Afiliados.FirstOrDefault() : 0;
            ViewBag.ID_Afiliado = new SelectList(db.Afiliados.Where(r => ID_Afiliados.Contains(r.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_Afiliado", "Nombre", ID_Afiliado);
            ViewBag.ID_Banco = new SelectList(db.Bancos.OrderBy(o => o.Nombre), "ID_Banco", "Nombre");
            ObtenerGeografiaSelectList();
            ViewBag.ID_FormaPago = new SelectList(db.FormasPago.OrderBy(o => o.Nombre), "ID_FormaPago", "Nombre");
            return View(new Flotas() { Habilitado = true, Liquido = true });
        }

        private void ObtenerGeografiaSelectList()
        {
            ViewBag.ID_Estado = new SelectList(db.Estados.OrderBy(o => o.Nombre), "ID_Estado", "Nombre");
            ViewBag.ID_Ciudad = new SelectList(db.Ciudades.OrderBy(o => o.Poblacion).Take(0), "ID_Ciudad", "Poblacion");
            ViewBag.ID_Colonia = new SelectList(db.Colonias.OrderBy(o => o.Nombre).Take(0), "ID_Colonia", "Nombre");
            ViewBag.ID_Calle = new SelectList(db.Calles.OrderBy(o => o.Nombre).Take(0), "ID_Calle", "Nombre");
        }

        // POST: Flotas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Vehiculos_Edicion")]
        public ActionResult Create([Bind(Include = "ID_Flota,Nombre,ID_Calle,Numero_Exterior,Numero_Interior,Telefono,Celular,RFC,ID_Banco,Clabe,Email,Habilitado,Reportes,ID_FormaPago,ID_Afiliado,Liquido,Fecha_Creacion,Fecha_Actualizacion,UserID")] Flotas flotas)
        {
            valid.ValidarFlota(ModelState, flotas);
            if (ModelState.IsValid)
            {
                flotas.Fecha_Creacion = util.ConvertToMexicanDate(DateTime.Now);
                flotas.UserID = User.Identity.GetUserId();
                db.Flotas.Add(flotas);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.MostrarAfiliados = ID_Afiliados.Count > 1;
            ViewBag.ID_Afiliado = new SelectList(db.Afiliados.Where(r => ID_Afiliados.Contains(r.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_Afiliado", "Nombre", flotas.ID_Afiliado);
            ViewBag.ID_Banco = new SelectList(db.Bancos.OrderBy(o => o.Nombre), "ID_Banco", "Nombre", flotas.ID_Banco);
            ObtenerGeografiaSelectList(flotas);
            ViewBag.ID_FormaPago = new SelectList(db.FormasPago.OrderBy(o => o.Nombre), "ID_FormaPago", "Nombre", flotas.ID_FormaPago);
            return View(flotas);
        }

        // GET: Flotas/Edit/5

        [HasPermission("Vehiculos_Edicion")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Flotas flotas = db.Flotas.Find(id);
            if (flotas == null)
            {
                TempData["Mess"] = MensajeNotFound;
                TempData["NavBar"] = "NavBar_CatFlotas";
                TempData["BackLink"] = "Index";

                return RedirectToAction("ItemNotFound");
            }
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.MostrarAfiliados = ID_Afiliados.Count > 1;
            ViewBag.ID_Afiliado = new SelectList(db.Afiliados.Where(r => ID_Afiliados.Contains(r.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_Afiliado", "Nombre", flotas.ID_Afiliado);
            ViewBag.ID_Banco = new SelectList(db.Bancos.OrderBy(o => o.Nombre), "ID_Banco", "Nombre", flotas.ID_Banco);
            ObtenerGeografiaSelectList(flotas);
            ViewBag.ID_FormaPago = new SelectList(db.FormasPago.OrderBy(o => o.Nombre), "ID_FormaPago", "Nombre", flotas.ID_FormaPago);
            return View(flotas);
        }

        // POST: Flotas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Vehiculos_Edicion")]
        public ActionResult Edit([Bind(Include = "ID_Flota,Nombre,ID_Calle,Numero_Exterior,Numero_Interior,Telefono,Celular,RFC,ID_Banco,Clabe,Email,Habilitado,Reportes,ID_FormaPago,ID_Afiliado,Liquido,Fecha_Creacion,Fecha_Actualizacion,UserID")] Flotas flotas)
        {
            valid.ValidarFlota(ModelState, flotas);
            if (ModelState.IsValid)
            {
                flotas.Fecha_Actualizacion = util.ConvertToMexicanDate(DateTime.Now);
                flotas.UserID = User.Identity.GetUserId();
                db.Entry(flotas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.MostrarAfiliados = ID_Afiliados.Count > 1;
            ViewBag.ID_Afiliado = new SelectList(db.Afiliados.Where(r => ID_Afiliados.Contains(r.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_Afiliado", "Nombre", flotas.ID_Afiliado);
            ViewBag.ID_Banco = new SelectList(db.Bancos.OrderBy(o => o.Nombre), "ID_Banco", "Nombre", flotas.ID_Banco);
            ObtenerGeografiaSelectList(flotas);
            ViewBag.ID_FormaPago = new SelectList(db.FormasPago.OrderBy(o => o.Nombre), "ID_FormaPago", "Nombre", flotas.ID_FormaPago);
            return View(flotas);
        }


        // GET: Flotas/Assign/5

        [HasPermission("Vehiculos_Edicion")]
        public ActionResult Assign(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Flotas flotas = db.Flotas.Find(id);
            FlotasViewModel flotasViewModel = new FlotasViewModel(flotas, "assign");
            if (flotas == null)
            {
                TempData["Mess"] = MensajeNotFound;
                TempData["NavBar"] = "NavBar_CatFlotas";
                TempData["BackLink"] = "Index";

                return RedirectToAction("ItemNotFound");
            }
            return View(flotasViewModel);
        }

        // POST: Flotas/Assign/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Vehiculos_Edicion")]
        public ActionResult Assign(FormCollection collection)
        {
            int ID_Flota = int.Parse(Request["ID_Flota"].ToString());
            List<Conductores> conductoresList = db.Flotas.Find(ID_Flota).Conductores.ToList();
            string IDs = "";
            foreach (Conductores cond in conductoresList)
            {
                int i = cond.ID_Conductor;
                Vehiculos_Conductores vh = cond.Vehiculos_Conductores.Where(vc => vc.Activo).OrderByDescending(vc => vc.Fecha_Asignacion).FirstOrDefault();
                if (Request["ID_Vehiculo_" + i].ToString() != "")
                {
                    int ID_Vehiculo = Int32.Parse(Request["ID_Vehiculo_" + i].ToString());
                    int ID_Conductor = Int32.Parse(Request["ID_Conductor_" + i].ToString());
                    int sel = vh == null ? 0 : vh.ID_Vehiculo;
                    if (sel != ID_Vehiculo)
                    {
                        Vehiculos_Conductores vehiculos_Conductores = new Vehiculos_Conductores();
                        vehiculos_Conductores.ID_Vehiculo = ID_Vehiculo;
                        vehiculos_Conductores.ID_Conductor = ID_Conductor;
                        vehiculos_Conductores.Activo = true;
                        vehiculos_Conductores.Fecha_Asignacion = util.ConvertToMexicanDate(DateTime.Now);
                        vehiculos_Conductores.Fecha_Creacion = util.ConvertToMexicanDate(DateTime.Now);
                        vehiculos_Conductores.UserID = User.Identity.GetUserId();
                        db.Vehiculos_Conductores.Add(vehiculos_Conductores);
                        db.SaveChanges();
                        IDs = IDs + vehiculos_Conductores.ID_Vehiculo_Conductor + ',';
                    }
                    else
                        IDs = IDs + vh.ID_Vehiculo_Conductor + ',';
                }
            }
            db.DesactivarVehiculosConductoresPorFlota(IDs.TrimEnd(','), ID_Flota, User.Identity.GetUserId());
            return RedirectToAction("Index");
        }

        // GET: Flotas/Liquidate/5

        [HasPermission("Vehiculos_Edicion")]
        public ActionResult Liquidate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Flotas flotas = db.Flotas.Find(id);
            FlotasViewModel flotasViewModel = new FlotasViewModel(flotas, "liquidate");
            if (flotas == null)
            {
                TempData["Mess"] = MensajeNotFound;
                TempData["NavBar"] = "NavBar_CatFlotas";
                TempData["BackLink"] = "Index";

                return RedirectToAction("ItemNotFound");
            }
            return View(flotasViewModel);
        }

        // POST: Flotas/Liquidate/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Vehiculos_Edicion")]
        public ActionResult Liquidate(FormCollection collection)
        {
            TiposSanciones tipoSancion = db.TiposSanciones.Where(t => t.Nombre.ToUpper().Contains("LIQUIDACIÓN")).FirstOrDefault();
            int ID_Flota = int.Parse(Request["ID_Flota"].ToString());
            Flotas flota = db.Flotas.Find(ID_Flota);
            List<Conductores> conductoresList = flota.Conductores.ToList();
            foreach (Conductores cond in conductoresList)
            {
                int i = cond.ID_Conductor;
                bool liquido = false;
                if (Request["Liquido_" + i].ToString() != "")
                {
                    liquido = bool.Parse(Request["Liquido_" + i].ToString().Split(',')[0]);
                }
                if (liquido)
                    HabilitarConductorYVehiculoConductor(cond);
                else
                    CrearSancion(tipoSancion, flota, cond);
            }
            return RedirectToAction("Index");
        }

        private void HabilitarConductorYVehiculoConductor(Conductores conductor_sancionado)
        {
            conductor_sancionado.Habilitado = true;
            conductor_sancionado.Liquido = true;
            db.Entry(conductor_sancionado).State = EntityState.Modified;
            valid.ValidaFlotaLiquidar(ModelState, conductor_sancionado);
            if (ModelState.IsValid)
            {
                Vehiculos_Conductores veh_con = conductor_sancionado.Vehiculos_Conductores.OrderByDescending(o => o.Fecha_Asignacion).FirstOrDefault();
                if (veh_con != null)
                {
                    Vehiculos vehiculo = veh_con.Vehiculos;
                    vehiculo.Habilitado = true;
                    vehiculo.Fecha_Baja = null;
                    db.Entry(vehiculo).State = EntityState.Modified;

                    veh_con.Activo = true;
                    db.Entry(veh_con).State = EntityState.Modified;
                }
                db.SaveChanges();
            }

        }

        private void InhabilitarConductorYVehiculoConductor(Conductores conductor_sancionado)
        {
            conductor_sancionado.Habilitado = false;
            conductor_sancionado.Liquido = false;
            db.Entry(conductor_sancionado).State = EntityState.Modified;

            Vehiculos_Conductores veh_con = conductor_sancionado.Vehiculos_Conductores.Where(vc => vc.Activo).FirstOrDefault();
            if (veh_con != null)
            {
                Vehiculos vehiculo = veh_con.Vehiculos;
                vehiculo.Habilitado = false;
                vehiculo.Fecha_Baja = util.ConvertToMexicanDate(DateTime.Now);
                db.Entry(vehiculo).State = EntityState.Modified;

                veh_con.Activo = false;
                db.Entry(veh_con).State = EntityState.Modified;
            }
            db.SaveChanges();
        }

        private void CrearSancion(TiposSanciones tipoSancion, Flotas flota, Conductores cond)
        {
            Sanciones sanciones = new Sanciones();
            sanciones.Fecha_Inicio = util.ConvertToMexicanDate(DateTime.Now);
            //sanciones.Fecha_Fin = sanciones.Fecha_Inicio.AddHours(tipoSancion.Horas_Penalizacion);
            sanciones.Fecha_Fin = DateTime.Now.AddHours(tipoSancion.Horas_Penalizacion);
            sanciones.ID_Conductor = cond.ID_Conductor;
            sanciones.ID_TipoSancion = tipoSancion.ID_TipoSancion;
            sanciones.Observaciones = "Sanción creada por no pagar su cuota.";
            sanciones.ID_Operador_Alta = db.Operadores.Where(o => o.ID_Afiliado == flota.ID_Afiliado).FirstOrDefault().ID_Operador;
            sanciones.Fecha_Creacion = util.ConvertToMexicanDate(DateTime.Now);
            sanciones.UserID = User.Identity.GetUserId();
            db.Sanciones.Add(sanciones);
            db.SaveChanges();
            InhabilitarConductorYVehiculoConductor(cond);
        }

        // GET: Flotas/Transfer/5

        [HasPermission("Vehiculos_Edicion")]
        public ActionResult Transfer(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Flotas flotas = db.Flotas.Find(id);
            FlotasViewModel flotasViewModel = new FlotasViewModel(flotas, "transfer");
            if (flotas == null)
            {
                TempData["Mess"] = MensajeNotFound;
                TempData["NavBar"] = "NavBar_CatFlotas";
                TempData["BackLink"] = "Index";

                return RedirectToAction("ItemNotFound");
            }
            return View(flotasViewModel);
        }

        // POST: Flotas/Transfer/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Vehiculos_Edicion")]
        public ActionResult Transfer(FormCollection collection)
        {
            int ID_Flota = int.Parse(Request["ID_Flota"].ToString());
            Flotas flota = db.Flotas.Find(ID_Flota);
            flota.Liquido = true;
            db.Entry(flota).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Flotas/Delete/5

        [HasPermission("Vehiculos_Edicion")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Flotas flotas = db.Flotas.Find(id);
            if (flotas == null)
            {
                TempData["Mess"] = MensajeNotFound;
                TempData["NavBar"] = "NavBar_CatFlotas";
                TempData["BackLink"] = "Index";

                return RedirectToAction("ItemNotFound");
            }
            ViewBag.Eliminar = db.Conductores.Where(c => c.ID_Flota == flotas.ID_Flota).Count() == 0;
            ViewBag.Mess = MensajeDelete;
            return View(flotas);
        }

        // POST: Flotas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [HasPermission("Vehiculos_Edicion")]
        public ActionResult DeleteConfirmed(int id)
        {
            serv.EliminarFlota(id);
            TempData["Delete"] = true;
            return RedirectToAction("Index");
        }

        private void ObtenerGeografiaSelectList(Flotas flotas)
        {
            if (flotas.ID_Calle == 0)
                ObtenerGeografiaSelectList();
            else
            {
                Colonias colonia = db.Calles.Find(flotas.ID_Calle).Colonias;
                Ciudades ciudad = colonia.Ciudades;
                Estados estado = ciudad.Estados;
                ViewBag.ID_Estado = new SelectList(db.Estados.OrderBy(o => o.Nombre), "ID_Estado", "Nombre", estado.ID_Estado);
                ViewBag.ID_Ciudad = new SelectList(estado.Ciudades.OrderBy(o => o.Poblacion), "ID_Ciudad", "Poblacion", ciudad.ID_Ciudad);
                ViewBag.ID_Colonia = new SelectList(ciudad.Colonias.OrderBy(o => o.Nombre), "ID_Colonia", "Nombre", colonia.ID_Colonia);
                ViewBag.ID_Calle = new SelectList(colonia.Calles.OrderBy(o => o.Nombre), "ID_Calle", "Nombre", flotas.ID_Calle);
            }
        }

        public JsonResult ObtenerFlotas(int ID_Turno)
        {
            int ID_Afiliado = db.Turnos.Find(ID_Turno).ID_Afiliado;
            var flotas = new SelectList(db.Flotas.ToList().Where(c => c.ID_Afiliado == ID_Afiliado).OrderBy(o => o.Nombre), "ID_Flota", "Nombre");
            return Json(flotas, JsonRequestBehavior.AllowGet);
        }

    }
}
