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
using GoPS.CustomFilters;
using GoPS.Filters;

namespace GoPS.Controllers
{
    [RedirectOnError]
    [OutputCache(NoStore = true, Duration = 0)]
    [EncryptedActionParameter]
    [ValidateInput(false)]
    public class TarifasController : Controller
    {
        private GoPSEntities db = new GoPSEntities();
        DBServicios serv = new DBServicios();
        DBValidaciones valid = new DBValidaciones();
        Utilities util = new Utilities();

        // GET: Tarifas
        [HasPermission("Vehiculos_Visualizacion")]
        public ActionResult Index()
        {
            if (TempData["Delete"] != null)
            {
                ViewBag.Delete = true;
                TempData.Remove("Delete");
            }
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            var tarifas = db.Tarifas.Where(r => ID_Afiliados.Contains(r.ID_Afiliado)).Include(d => d.Afiliados);
            return View(tarifas.ToList());
        }

        // GET: Tarifas/Details/5
         
        [HasPermission("Vehiculos_Visualizacion")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tarifas tarifas = db.Tarifas.Find(id);
            if (tarifas == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
            }
            return View(tarifas);
        }

        // GET: Tarifas/Create
        [HasPermission("Vehiculos_Edicion")]
        public ActionResult Create()
        {
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.MostrarAfiliados = ID_Afiliados.Count > 1;
            int ID_Afiliado = ID_Afiliados.Count == 1 ? ID_Afiliados.FirstOrDefault() : 0;
            ViewBag.ID_Afiliado = new SelectList(db.Afiliados.Where(r => ID_Afiliados.Contains(r.ID_Afiliado)).ToList().OrderBy(o => o.Nombre), "ID_Afiliado", "Nombre", ID_Afiliado);
            return View(new Tarifas() { Activa = true });
        }

        // POST: Tarifas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Vehiculos_Edicion")]
        public ActionResult Create([Bind(Include = "ID_Tarifa,Nombre,Descripcion,Tarifa_Minima,Precio_Base,Precio_Km,Precio_Min,ID_Afiliado,Activa,Fecha_Creacion,Fecha_Actualizacion,UserID")] Tarifas tarifas)
        {
            valid.ValidarTarifa(ModelState, tarifas);
            if (ModelState.IsValid)
            {
                if(tarifas.Activa)
                    CambiarTarifaActivaPorAfiliado(tarifas);
                tarifas.Fecha_Creacion = util.ConvertToMexicanDate(DateTime.Now);
                tarifas.UserID = User.Identity.GetUserId();
                db.Tarifas.Add(tarifas);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.MostrarAfiliados = ID_Afiliados.Count > 1;
            ViewBag.ID_Afiliado = new SelectList(db.Afiliados.Where(r => ID_Afiliados.Contains(r.ID_Afiliado)).ToList().OrderBy(o => o.Nombre), "ID_Afiliado", "Nombre", tarifas.ID_Afiliado);
            return View(tarifas);
        }

        private void CambiarTarifaActivaPorAfiliado(Tarifas tarifas)
        {
            Tarifas activa = db.Tarifas.AsNoTracking().Where(t => t.ID_Afiliado == tarifas.ID_Afiliado && t.Activa).FirstOrDefault();
            if (activa != null)
            {
                activa.Activa = false;
                db.Entry(activa).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        // GET: Tarifas/Edit/5
        [HasPermission("Vehiculos_Edicion")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tarifas tarifas = db.Tarifas.Find(id);
            if (tarifas == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
            }
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.MostrarAfiliados = ID_Afiliados.Count > 1;
            ViewBag.ID_Afiliado = new SelectList(db.Afiliados.Where(r => ID_Afiliados.Contains(r.ID_Afiliado)).ToList().OrderBy(o => o.Nombre), "ID_Afiliado", "Nombre", tarifas.ID_Afiliado);
            return View(tarifas);
        }

        // POST: Tarifas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Vehiculos_Edicion")]
        public ActionResult Edit([Bind(Include = "ID_Tarifa,Nombre,Descripcion,Tarifa_Minima,Precio_Base,Precio_Km,Precio_Min,ID_Afiliado,Activa,Fecha_Creacion,Fecha_Actualizacion,UserID")] Tarifas tarifas)
        {
            valid.ValidarTarifa(ModelState, tarifas);
            if (ModelState.IsValid)
            {
                if (tarifas.Activa)
                    CambiarTarifaActivaPorAfiliado(tarifas);
                tarifas.Fecha_Actualizacion = util.ConvertToMexicanDate(DateTime.Now);
                tarifas.UserID = User.Identity.GetUserId();
                db.Entry(tarifas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.MostrarAfiliados = ID_Afiliados.Count > 1;
            ViewBag.ID_Afiliado = new SelectList(db.Afiliados.Where(r => ID_Afiliados.Contains(r.ID_Afiliado)).ToList().OrderBy(o => o.Nombre), "ID_Afiliado", "Nombre", tarifas.ID_Afiliado);
            return View(tarifas);
        }

        // GET: Tarifas/Delete/5
         
        [HasPermission("Vehiculos_Edicion")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tarifas tarifas = db.Tarifas.Find(id);
            if (tarifas == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
            }
            return View(tarifas);
        }

        // POST: Tarifas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [HasPermission("Vehiculos_Edicion")]
        public ActionResult DeleteConfirmed(int id)
        {
            serv.EliminarTarifa(id);
            TempData["Delete"] = true;
            return RedirectToAction("Index");
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
