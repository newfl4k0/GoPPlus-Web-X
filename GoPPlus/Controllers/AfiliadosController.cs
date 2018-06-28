using GoPS.Classes;
using GoPS.CustomFilters;
using GoPS.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using GoPS.Filters;

namespace GoPS.Controllers
{
    [RedirectOnError]
    [OutputCache(NoStore = true, Duration = 0)]
    [EncryptedActionParameter]
    [ValidateInput(false)]
    public class AfiliadosController : _GeneralController 
    {
        
        // GET: Afiliados
        [HasPermission("Clientes_Visualizacion")]
        public ActionResult Index()
        {
            if (TempData["Delete"] != null)
            {
                ViewBag.Delete = true;
                TempData.Remove("Delete");
            }
            var afiliados = db.Afiliados.Include(a => a.TiposServicios);
            return View(afiliados.ToList());
        }

        // GET: Afiliados/Details/5
        [HasPermission("Clientes_Visualizacion")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Afiliados afiliados = db.Afiliados.Find(id);
            if (afiliados == null)
            {
                TempData["Mess"] = MensajeNotFound;
                TempData["NavBar"] = "NavBar_CatAfiliados";
                TempData["BackLink"] = "Index";

                return RedirectToAction("ItemNotFound");
            }
            List<int> ID_Empresas = RouteData.Values["ID_Empresas"] as List<int>;
            bool isSAdmin = Boolean.Parse(RouteData.Values["SAdministrador"].ToString());
            ViewBag.MostrarEmpresas = ID_Empresas.Count() > 1 && isSAdmin;
            return View(afiliados);
        }

        // GET: Afiliados/Create
        [HasPermission("Clientes_Edicion")]
        public ActionResult Create()
        {
            ViewBag.ID_TipoServicio = new SelectList(db.TiposServicios.OrderBy(o => o.Nombre), "ID_TipoServicio", "Nombre");
            ViewBag.ID_TipoPago = new SelectList(db.TiposPagos.OrderBy(o => o.Nombre), "ID_TipoPago", "Nombre");
            ViewBag.ID_FrecuenciaPago = new SelectList(db.FrecuenciasPago.OrderBy(o => o.Nombre), "ID_FrecuenciaPago", "Nombre");
            ObtenerGeografiaSelectList();
            List<int> ID_Empresas = RouteData.Values["ID_Empresas"] as List<int>;
            ViewBag.MostrarEmpresas = ID_Empresas.Count() > 1;
            int ID_Empresa = ID_Empresas.Count == 1 ? ID_Empresas.FirstOrDefault() : 0;
            Empresas empresa = db.Empresas.Find(ID_Empresa);
            ViewBag.ID_Empresa = new SelectList(db.Empresas.Where(e => ID_Empresas.Contains(e.ID_Empresa)).OrderBy(o => o.Nombre), "ID_Empresa", "Nombre", ID_Empresa);
            Afiliados afiliados = new Afiliados();
            ViewBag.DisableAfiliado = db.Afiliados.Where(a => a.ID_Empresa == ID_Empresa).ToList().Count > 1;
            afiliados.Nombre = empresa != null ? empresa.Afiliados.FirstOrDefault().Nombre : afiliados.Nombre;
            afiliados.RFC = empresa != null ? empresa.Afiliados.FirstOrDefault().RFC : afiliados.RFC;
            afiliados.estatusList = util.ObtenerEstatusList(afiliados.estatus);
            afiliados.tiposvehiculosList = util.ObtenerTiposVehiculosList(afiliados.tiposvehiculos);
            return View(afiliados);
        }

        private void ObtenerGeografiaSelectList()
        {
            ViewBag.ID_Estado = new SelectList(db.Estados.OrderBy(o => o.Nombre), "ID_Estado", "Nombre");
            ViewBag.ID_Ciudad = new SelectList(db.Ciudades.OrderBy(o => o.Poblacion).Take(0), "ID_Ciudad", "Poblacion");
            ViewBag.ID_Colonia = new SelectList(db.Colonias.OrderBy(o => o.Nombre).Take(0), "ID_Colonia", "Nombre");
            ViewBag.ID_Calle = new SelectList(db.Calles.OrderBy(o => o.Nombre).Take(0), "ID_Calle", "Nombre");
        }

        // POST: Afiliados/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Clientes_Edicion")]
        public ActionResult Create([Bind(Include = "ID_Afiliado,Nombre,RFC,ID_Calle,Numero_Exterior,Numero_Interior,Codigo_Postal,Horas_Conductor,Cuota_Conductor,Porcentaje_Conductor,ID_TipoPago,ID_FrecuenciaPago,ID_TipoServicio,ID_Empresa,Fecha_Creacion,Fecha_Actualizacion,UserID,estatus,tiposvehiculos")] Afiliados afiliados)
        {

            valid.ValidarAfiliado(ModelState, afiliados);
            if (ModelState.IsValid)
            {
                afiliados.Fecha_Creacion = util.ConvertToMexicanDate(DateTime.Now);
                afiliados.UserID = User.Identity.GetUserId();
                db.Afiliados.Add(afiliados);
                db.SaveChanges();
                AgregarEstatus(afiliados);
                AgregarTiposVehiculos(afiliados);
                return RedirectToAction("Index");
            }
            ViewBag.ID_TipoServicio = new SelectList(db.TiposServicios.OrderBy(o => o.Nombre), "ID_TipoServicio", "Nombre");
            ViewBag.ID_TipoPago = new SelectList(db.TiposPagos.OrderBy(o => o.Nombre), "ID_TipoPago", "Nombre");
            ViewBag.ID_FrecuenciaPago = new SelectList(db.FrecuenciasPago.OrderBy(o => o.Nombre), "ID_FrecuenciaPago", "Nombre");
            ObtenerGeografiaSelectList(afiliados);
            List<int> ID_Empresas = RouteData.Values["ID_Empresas"] as List<int>;
            ViewBag.MostrarEmpresas = ID_Empresas.Count() > 1;
            int ID_Empresa = ID_Empresas.Count == 1 ? ID_Empresas.FirstOrDefault() : 0;
            Empresas empresa = db.Empresas.Find(ID_Empresa);
            ViewBag.ID_Empresa = new SelectList(db.Empresas.Where(e => ID_Empresas.Contains(e.ID_Empresa)).OrderBy(o => o.Nombre), "ID_Empresa", "Nombre", ID_Empresa);
            ViewBag.DisableAfiliado = db.Afiliados.Where(a => a.ID_Empresa == ID_Empresa).ToList().Count > 1;
            afiliados.Nombre = empresa != null ? empresa.Afiliados.FirstOrDefault().Nombre : afiliados.Nombre;
            afiliados.RFC = empresa != null ? empresa.Afiliados.FirstOrDefault().RFC : afiliados.RFC;
            afiliados.estatusList = util.ObtenerEstatusList(afiliados.estatus);
            afiliados.tiposvehiculosList = util.ObtenerTiposVehiculosList(afiliados.tiposvehiculos);
            return View(afiliados);
        }

        private void AgregarEstatus(Afiliados afiliados)
        {
            List<int> estatusChecked = String.IsNullOrEmpty(afiliados.estatus) ? new List<int>() : afiliados.estatus.Split(',').Select(Int32.Parse).ToList();
            List<int> estatusDelete = db.Estatus_Afiliados.Where(a => a.ID_Afiliado == afiliados.ID_Afiliado && !estatusChecked.Contains(a.ID_Estatus)).Select(a => a.ID_Estatus).ToList();
            foreach (int est in estatusChecked)
            {
                Estatus_Afiliados est_afi = db.Estatus_Afiliados.Where(a => a.ID_Afiliado == afiliados.ID_Afiliado && a.ID_Estatus == est).FirstOrDefault();
                if (est_afi != null)
                {
                    est_afi.Fecha_Actualizacion = util.ConvertToMexicanDate(DateTime.Now);
                    est_afi.UserID = User.Identity.GetUserId();
                    db.Entry(est_afi).State = EntityState.Modified;
                }
                else
                {
                    est_afi = new Estatus_Afiliados();
                    est_afi.ID_Estatus = est;
                    est_afi.ID_Afiliado = afiliados.ID_Afiliado;
                    est_afi.Fecha_Creacion = util.ConvertToMexicanDate(DateTime.Now);
                    est_afi.UserID = User.Identity.GetUserId();
                    db.Estatus_Afiliados.Add(est_afi);
                }
                db.SaveChanges();
            }
            if (estatusDelete.Count > 0)
            {
                db.Estatus_Afiliados.RemoveRange(db.Estatus_Afiliados.Where(a => a.ID_Afiliado == afiliados.ID_Afiliado && estatusDelete.Contains(a.ID_Estatus)));
                db.SaveChanges();
            }
        }

        private void AgregarTiposVehiculos(Afiliados afiliados)
        {
            List<int> tiposvehiculosChecked = String.IsNullOrEmpty(afiliados.tiposvehiculos) ? new List<int>() : afiliados.tiposvehiculos.Split(',').Select(Int32.Parse).ToList();
            List<int> tiposvehiculosDelete = db.TiposVehiculos_Afiliados.Where(a => a.ID_Afiliado == afiliados.ID_Afiliado && !tiposvehiculosChecked.Contains(a.ID_TipoVehiculo)).Select(a => a.ID_TipoVehiculo).ToList();
            foreach (int tipo in tiposvehiculosChecked)
            {
                TiposVehiculos_Afiliados tipo_afi = db.TiposVehiculos_Afiliados.Where(a => a.ID_Afiliado == afiliados.ID_Afiliado && a.ID_TipoVehiculo == tipo).FirstOrDefault();
                if (tipo_afi != null)
                {
                    tipo_afi.Fecha_Actualizacion = util.ConvertToMexicanDate(DateTime.Now);
                    tipo_afi.UserID = User.Identity.GetUserId();
                    db.Entry(tipo_afi).State = EntityState.Modified;
                }
                else
                {
                    tipo_afi = new TiposVehiculos_Afiliados();
                    tipo_afi.ID_TipoVehiculo = tipo;
                    tipo_afi.ID_Afiliado = afiliados.ID_Afiliado;
                    tipo_afi.Fecha_Creacion = util.ConvertToMexicanDate(DateTime.Now);
                    tipo_afi.UserID = User.Identity.GetUserId();
                    db.TiposVehiculos_Afiliados.Add(tipo_afi);
                }
                db.SaveChanges();
            }
            if (tiposvehiculosDelete.Count > 0)
            {
                db.TiposVehiculos_Afiliados.RemoveRange(db.TiposVehiculos_Afiliados.Where(a => a.ID_Afiliado == afiliados.ID_Afiliado && tiposvehiculosDelete.Contains(a.ID_TipoVehiculo)));
                db.SaveChanges();
            }
        }

        // GET: Afiliados/Edit/5
        [HasPermission("Clientes_Edicion")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Afiliados afiliados = db.Afiliados.Find(id);
            if (afiliados == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID_TipoServicio = new SelectList(db.TiposServicios.OrderBy(o => o.Nombre), "ID_TipoServicio", "Nombre", afiliados.ID_TipoServicio);
            ViewBag.ID_TipoPago = new SelectList(db.TiposPagos.OrderBy(o => o.Nombre), "ID_TipoPago", "Nombre", afiliados.ID_TipoPago);
            ViewBag.ID_FrecuenciaPago = new SelectList(db.FrecuenciasPago.OrderBy(o => o.Nombre), "ID_FrecuenciaPago", "Nombre", afiliados.ID_FrecuenciaPago);
            ObtenerGeografiaSelectList(afiliados);
            List<int> ID_Empresas = RouteData.Values["ID_Empresas"] as List<int>;
            ViewBag.MostrarEmpresas = ID_Empresas.Count() > 1;
            ViewBag.DisableAfiliado = db.Afiliados.Where(a => a.ID_Empresa == afiliados.ID_Empresa).ToList().Count > 1;
            ViewBag.ID_Empresa = new SelectList(db.Empresas.Where(e => ID_Empresas.Contains(e.ID_Empresa)).OrderBy(o => o.Nombre), "ID_Empresa", "Nombre", afiliados.ID_Empresa);
            afiliados.estatus = string.Join(",", afiliados.Estatus_Afiliados.Select(a => a.ID_Estatus).Select(a => a.ToString()).ToArray());
            afiliados.estatusList = util.ObtenerEstatusList(afiliados.estatus);
            afiliados.tiposvehiculos = string.Join(",", afiliados.TiposVehiculos_Afiliados.Select(a => a.ID_TipoVehiculo).Select(a => a.ToString()).ToArray());
            afiliados.tiposvehiculosList = util.ObtenerTiposVehiculosList(afiliados.tiposvehiculos);
            return View(afiliados);
        }

        // POST: Afiliados/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Clientes_Edicion")]
        public ActionResult Edit([Bind(Include = "ID_Afiliado,Nombre,RFC,ID_Calle,Numero_Exterior,Numero_Interior,Codigo_Postal,Horas_Conductor,Cuota_Conductor,Porcentaje_Conductor,ID_TipoPago,ID_FrecuenciaPago,ID_TipoServicio,ID_Empresa,Fecha_Creacion,Fecha_Actualizacion,UserID,estatus,tiposvehiculos")] Afiliados afiliados)
        {
            bool isSAdmin = Boolean.Parse(RouteData.Values["SAdministrador"].ToString());
            valid.ValidarAfiliado(ModelState, afiliados);
            if (ModelState.IsValid)
            {
                afiliados.Fecha_Actualizacion = util.ConvertToMexicanDate(DateTime.Now);
                afiliados.UserID = User.Identity.GetUserId();
                db.Entry(afiliados).State = EntityState.Modified;
                db.SaveChanges();
                AgregarEstatus(afiliados);
                AgregarTiposVehiculos(afiliados);
                return RedirectToAction("Index");
            }
            ViewBag.ID_TipoServicio = new SelectList(db.TiposServicios.OrderBy(o => o.Nombre), "ID_TipoServicio", "Nombre", afiliados.ID_TipoServicio);
            ViewBag.ID_TipoPago = new SelectList(db.TiposPagos.OrderBy(o => o.Nombre), "ID_TipoPago", "Nombre", afiliados.ID_TipoPago);
            ViewBag.ID_FrecuenciaPago = new SelectList(db.FrecuenciasPago.OrderBy(o => o.Nombre), "ID_FrecuenciaPago", "Nombre", afiliados.ID_FrecuenciaPago);
            ObtenerGeografiaSelectList(afiliados);
            List<int> ID_Empresas = RouteData.Values["ID_Empresas"] as List<int>;
            ViewBag.MostrarEmpresas = ID_Empresas.Count() > 1;
            ViewBag.DisableAfiliado = db.Afiliados.Where(a => a.ID_Empresa == afiliados.ID_Empresa).ToList().Count > 1;
            ViewBag.ID_Empresa = new SelectList(db.Empresas.Where(e => ID_Empresas.Contains(e.ID_Empresa)).OrderBy(o => o.Nombre), "ID_Empresa", "Nombre", afiliados.ID_Empresa);
            afiliados.estatusList = util.ObtenerEstatusList(afiliados.estatus);
            afiliados.tiposvehiculosList = util.ObtenerTiposVehiculosList(afiliados.tiposvehiculos);
            return View(afiliados);
        }

        // GET: Afiliados/Delete/5
        [HasPermission("Clientes_Edicion")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Afiliados afiliados = db.Afiliados.Find(id);
            if (afiliados == null)
            {
                TempData["Mess"] = MensajeNotFound;
                TempData["NavBar"] = "NavBar_CatAfiliados";
                TempData["BackLink"] = "Index";

                return RedirectToAction("ItemNotFound");
            }
            ViewBag.Mess = MensajeDelete;
            List<int> ID_Empresas = RouteData.Values["ID_Empresas"] as List<int>;
            bool isSAdmin = Boolean.Parse(RouteData.Values["SAdministrador"].ToString());
            ViewBag.MostrarEmpresas = ID_Empresas.Count() > 1 && isSAdmin;
            ViewBag.Eliminar = db.Conductores.Where(c => c.Flotas.ID_Afiliado == afiliados.ID_Afiliado).Count() == 0
                                && db.Conductores.Where(c => c.Turnos.ID_Afiliado == afiliados.ID_Afiliado).Count() == 0;
            return View(afiliados);
        }

        // POST: Afiliados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [HasPermission("Clientes_Edicion")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await serv.EliminarAfiliado(id);
            TempData["Delete"] = true;
            return RedirectToAction("Index");
        }

        //[HasPermission("Monitoreo_Visualizacion")]
        //public JsonResult ObtenerAfiliados(int ID_Empresa)
        //{
        //    List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
        //    ViewBag.MostrarAfiliados = ID_Afiliados.Count > 1;
        //    int ID_Afiliado = ID_Afiliados.Count == 1 ? ID_Afiliados.FirstOrDefault() : 0;
        //    var afiliados = new SelectList(db.Afiliados.Where(c => ID_Afiliados.Contains(c.ID_Afiliado) && c.ID_Empresa == ID_Empresa).OrderBy(o => o.NombreRFC), "ID_Afiliado", "NombreRFC");
        //    return Json(afiliados, JsonRequestBehavior.AllowGet);
        //}

        private void ObtenerGeografiaSelectList(Afiliados afiliados)
        {
            if (afiliados.ID_Calle == 0)
                ObtenerGeografiaSelectList();
            else
            {
                Colonias colonia = db.Calles.Find(afiliados.ID_Calle).Colonias;
                Ciudades ciudad = colonia.Ciudades;
                Estados estado = ciudad.Estados;
                ViewBag.ID_Estado = new SelectList(db.Estados.OrderBy(o => o.Nombre), "ID_Estado", "Nombre", estado.ID_Estado);
                ViewBag.ID_Ciudad = new SelectList(estado.Ciudades.OrderBy(o => o.Poblacion), "ID_Ciudad", "Poblacion", ciudad.ID_Ciudad);
                ViewBag.ID_Colonia = new SelectList(ciudad.Colonias.OrderBy(o => o.Nombre), "ID_Colonia", "Nombre", colonia.ID_Colonia);
                ViewBag.ID_Calle = new SelectList(colonia.Calles.OrderBy(o => o.Nombre), "ID_Calle", "Nombre", afiliados.ID_Calle);
            }
        }

        public JsonResult ObtenerCoordenadasPorAfiliado(int ID_Afiliado)
        {
            int id_calle = db.Afiliados.Find(ID_Afiliado).ID_Calle;
            var coordenadas = db.Calles.Where(c => c.ID_Calle == id_calle).Select(x => new { x.Latitud, x.Longitud }).FirstOrDefault();
            return Json(coordenadas, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ObtenerNombreRFCPorEmpresa(int ID_Empresa)
        {
            List<Afiliados> afiliados = db.Afiliados.Where(a => a.ID_Empresa == ID_Empresa).ToList();
            int count = afiliados.Count;
            var nombre = count > 0 ? afiliados.FirstOrDefault().Nombre : "";
            var rfc = count > 0 ? afiliados.FirstOrDefault().RFC : "";
            return Json(new { count, nombre, rfc }, JsonRequestBehavior.AllowGet);
        }
        
    }
}
