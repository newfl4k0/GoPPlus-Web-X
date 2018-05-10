using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
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
    public class VehiculosController : Controller
    {
        private GoPSEntities db = new GoPSEntities();
        DBServicios serv = new DBServicios();
        DBValidaciones valid = new DBValidaciones();
        Utilities util = new Utilities();

        // GET: Vehiculos
        [HasPermission("Vehiculos_Visualizacion")]
        public ActionResult Index()
        {
            if (TempData["Delete"] != null)
            {
                ViewBag.Delete = true;
                TempData.Remove("Delete");
            }
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            var vehiculos = db.Vehiculos.Where(o => ID_Afiliados.Contains(o.Flotas.ID_Afiliado)).Include(v => v.Ciudades).Include(v => v.Flotas).Include(v => v.Modelos).Include(v => v.Seguros).Include(v => v.TiposVehiculos).Include(v => v.Colores);

            VehiculosViewModel vehiculosViewModel = new VehiculosViewModel(vehiculos.ToList());

            return View(vehiculosViewModel);
        }

        // GET: Vehiculos/Details/5
         
        [HasPermission("Vehiculos_Visualizacion")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehiculos vehiculos = db.Vehiculos.Find(id);
            if (vehiculos == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
            }
            return View(vehiculos);
        }

        // GET: Vehiculos/Create
        [HasPermission("Vehiculos_Edicion")]
        public ActionResult Create()
        {
            ObtenerGeografiaSelectList();
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.ID_Flota = new SelectList(db.Flotas.Where(o => ID_Afiliados.Contains(o.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_Flota", "Nombre");
            ObtenerModeloMarcaSelectList();
            ViewBag.ID_Seguro = new SelectList(db.Seguros.OrderBy(o => o.Nombre), "ID_Seguro", "Nombre");
            ViewBag.ID_TipoVehiculo = new SelectList(db.TiposVehiculos.OrderBy(o => o.Nombre), "ID_TipoVehiculo", "Nombre");
            ViewBag.ID_Color = new SelectList(db.Colores.OrderBy(o => o.Nombre), "ID_Color", "Nombre");
            return View(new Vehiculos() { Habilitado = true });
        }

        private void ObtenerModeloMarcaSelectList()
        {
            ViewBag.ID_Marca = new SelectList(db.Marcas.OrderBy(o => o.Nombre), "ID_Marca", "Nombre");
            ViewBag.ID_Modelo = new SelectList(db.Modelos.OrderBy(o => o.Nombre), "ID_Modelo", "Nombre");
        }

        private void ObtenerGeografiaSelectList()
        {
            ViewBag.ID_Estado = new SelectList(db.Estados.OrderBy(o => o.Nombre), "ID_Estado", "Nombre");
            ViewBag.ID_Ciudad = new SelectList(db.Ciudades.OrderBy(o => o.Poblacion).Take(0), "ID_Ciudad", "Poblacion");
        }

        // POST: Vehiculos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Vehiculos_Edicion")]
        public ActionResult Create([Bind(Include = "ID_Vehiculo,Matricula,Fecha_Alta,Fecha_Baja,Telefono_Seguro,Telefono_Seguro_EsCelular,Poliza,Telefono,Serie,Habilitado,NoLicencia,NoTarjeton,RevistaMecanica,KmInicial,KmFinal,Version,ID_Flota,ID_Ciudad,ID_Modelo,ID_Seguro,ID_TipoVehiculo,ID_Color,NoPermiso,Vigencia_Permiso,NoTransporte,Externo,Fecha_Creacion,Fecha_Actualizacion,UserID")] Vehiculos vehiculos)
        {
            valid.ValidarVehiculo(ModelState, vehiculos);
            if (ModelState.IsValid)
            {
                vehiculos.Fecha_Alta = util.ConvertToMexicanDate(DateTime.Now);
                vehiculos.Fecha_Creacion = util.ConvertToMexicanDate(DateTime.Now);
                vehiculos.UserID = User.Identity.GetUserId();
                db.Vehiculos.Add(vehiculos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ObtenerGeografiaSelectList(vehiculos);
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.ID_Flota = new SelectList(db.Flotas.Where(o => ID_Afiliados.Contains(o.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_Flota", "Nombre", vehiculos.ID_Flota);
            ObtenerModeloMarcaSelectList(vehiculos);
            ViewBag.ID_Seguro = new SelectList(db.Seguros.OrderBy(o => o.Nombre), "ID_Seguro", "Nombre", vehiculos.ID_Seguro);
            ViewBag.ID_TipoVehiculo = new SelectList(db.TiposVehiculos.OrderBy(o => o.Nombre), "ID_TipoVehiculo", "Nombre", vehiculos.ID_TipoVehiculo);
            ViewBag.ID_Color = new SelectList(db.Colores.OrderBy(o => o.Nombre), "ID_Color", "Nombre", vehiculos.ID_Color);
            return View(vehiculos);
        }

        // GET: Vehiculos/Edit/5
         
        [HasPermission("Vehiculos_Edicion")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehiculos vehiculos = db.Vehiculos.Find(id);
            if (vehiculos == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
            }
            ObtenerGeografiaSelectList(vehiculos);
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.ID_Flota = new SelectList(db.Flotas.Where(o => ID_Afiliados.Contains(o.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_Flota", "Nombre", vehiculos.ID_Flota);
            ViewBag.ID_Flota = new SelectList(db.Flotas.OrderBy(o => o.Nombre), "ID_Flota", "Nombre", vehiculos.ID_Flota);
            ObtenerModeloMarcaSelectList(vehiculos);
            ViewBag.ID_Seguro = new SelectList(db.Seguros.OrderBy(o => o.Nombre), "ID_Seguro", "Nombre", vehiculos.ID_Seguro);
            ViewBag.ID_TipoVehiculo = new SelectList(db.TiposVehiculos.OrderBy(o => o.Nombre), "ID_TipoVehiculo", "Nombre", vehiculos.ID_TipoVehiculo);
            ViewBag.ID_Color = new SelectList(db.Colores.OrderBy(o => o.Nombre), "ID_Color", "Nombre", vehiculos.ID_Color);
            return View(vehiculos);
        }

        // POST: Vehiculos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Vehiculos_Edicion")]
        public ActionResult Edit([Bind(Include = "ID_Vehiculo,Matricula,Fecha_Alta,Fecha_Baja,Telefono_Seguro,Telefono_Seguro_EsCelular,Poliza,Telefono,Serie,Habilitado,NoLicencia,NoTarjeton,RevistaMecanica,KmInicial,KmFinal,Version,ID_Flota,ID_Ciudad,ID_Modelo,ID_Seguro,ID_TipoVehiculo,ID_Color,NoPermiso,Vigencia_Permiso,NoTransporte,Externo,Fecha_Creacion,Fecha_Actualizacion,UserID")] Vehiculos vehiculos)
        {
            valid.ValidarVehiculo(ModelState, vehiculos);
            if (ModelState.IsValid)
            {
                vehiculos.Fecha_Actualizacion = util.ConvertToMexicanDate(DateTime.Now);
                vehiculos.UserID = User.Identity.GetUserId();
                db.Entry(vehiculos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ObtenerGeografiaSelectList(vehiculos);
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.ID_Flota = new SelectList(db.Flotas.Where(o => ID_Afiliados.Contains(o.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_Flota", "Nombre", vehiculos.ID_Flota);
            ViewBag.ID_Flota = new SelectList(db.Flotas.OrderBy(o => o.Nombre), "ID_Flota", "Nombre", vehiculos.ID_Flota);
            ObtenerModeloMarcaSelectList(vehiculos);
            ViewBag.ID_Seguro = new SelectList(db.Seguros.OrderBy(o => o.Nombre), "ID_Seguro", "Nombre", vehiculos.ID_Seguro);
            ViewBag.ID_TipoVehiculo = new SelectList(db.TiposVehiculos.OrderBy(o => o.Nombre), "ID_TipoVehiculo", "Nombre", vehiculos.ID_TipoVehiculo);
            ViewBag.ID_Color = new SelectList(db.Colores.OrderBy(o => o.Nombre), "ID_Color", "Nombre", vehiculos.ID_Color);
            return View(vehiculos);
        }

        // GET: Vehiculos/Delete/5
         
        [HasPermission("Vehiculos_Edicion")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehiculos vehiculos = db.Vehiculos.Find(id);
            if (vehiculos == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
            }
            return View(vehiculos);
        }

        // POST: Vehiculos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [HasPermission("Vehiculos_Edicion")]
        public ActionResult DeleteConfirmed(int id)
        {
            serv.EliminarVehiculo(id);
            TempData["Delete"] = true;
            return RedirectToAction("Index");
        }

        private void ObtenerModeloMarcaSelectList(Vehiculos vehiculos)
        {
            if (vehiculos.ID_Modelo == 0)
                ObtenerModeloMarcaSelectList();
            else
            {
                Marcas marca = db.Modelos.Find(vehiculos.ID_Modelo).Marcas;
                ViewBag.ID_Marca = new SelectList(db.Marcas.OrderBy(o => o.Nombre), "ID_Marca", "Nombre", marca.ID_Marca);
                ViewBag.ID_Modelo = new SelectList(marca.Modelos.OrderBy(o => o.Nombre), "ID_Modelo", "Nombre", vehiculos.ID_Modelo);
            }
        }

        private void ObtenerGeografiaSelectList(Vehiculos vehiculos)
        {
            if (vehiculos.ID_Ciudad == 0)
                ObtenerGeografiaSelectList();
            else
            {
                Estados estado = db.Ciudades.Find(vehiculos.ID_Ciudad).Estados;
                ViewBag.ID_Estado = new SelectList(db.Estados.OrderBy(o => o.Nombre), "ID_Estado", "Nombre", estado.ID_Estado);
                ViewBag.ID_Ciudad = new SelectList(estado.Ciudades.OrderBy(o => o.Poblacion), "ID_Ciudad", "Poblacion", vehiculos.ID_Ciudad);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
