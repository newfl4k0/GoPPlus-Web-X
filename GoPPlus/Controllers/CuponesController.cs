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
using System.Web.Mvc;
using System.Web.Routing;
using GoPS.Filters;

namespace GoPS.Controllers
{
    [RedirectOnError]
    [OutputCache(NoStore = true, Duration = 0)]
    [EncryptedActionParameter]
    [ValidateInput(false)]
    public class CuponesController : _GeneralController 
    {
        // GET: Cupones
        [HasPermission("Configuraciones_Visualizacion")]
        [RedirectOnError]
        public ActionResult Index()
        {
            if (TempData["Delete"] != null)
            {
                ViewBag.Delete = true;
                TempData.Remove("Delete");
            }
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            var cupones = db.Cupones.Where(r => ID_Afiliados.Contains(r.ID_Afiliado)).Include(d => d.Afiliados);
            return View(cupones.ToList());
        }

        // GET: Cupones/Details/5
        [HasPermission("Configuraciones_Visualizacion")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cupones cupones = db.Cupones.Find(id);
            if (cupones == null)
            {
                TempData["Mess"] = MensajeNotFound;
                TempData["NavBar"] = "NavBar_CatCupones";
                TempData["BackLink"] = "Index";

                return RedirectToAction("ItemNotFound");
            }
            return View(cupones);
        }

        // GET: Cupones/Create
        [HasPermission("Configuraciones_Edicion")]
        [RedirectOnError]
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult Create()
        {
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.MostrarAfiliados = ID_Afiliados.Count > 1;
            int ID_Afiliado = ID_Afiliados.Count == 1 ? ID_Afiliados.FirstOrDefault() : 0;
            ViewBag.ID_Afiliado = new SelectList(db.Afiliados.Where(r => ID_Afiliados.Contains(r.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_Afiliado", "Nombre", ID_Afiliado);
            return View();
        }

        // POST: Cupones/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [RedirectOnError]
        [OutputCache(NoStore = true, Duration = 0)]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Create([Bind(Include = "ID_Cupon,Descripcion,Codigo,Cantidad,Fecha_Inicio,Fecha_Fin,Descuento,Primer_Servicio,ID_Afiliado,Fecha_Creacion,Fecha_Actualizacion,UserID")] Cupones cupones)
        {
            valid.ValidarCupon(ModelState, cupones);
            if (ModelState.IsValid)
            {
                cupones.Fecha_Creacion = util.ConvertToMexicanDate(DateTime.Now);
                cupones.UserID = User.Identity.GetUserId();
                db.Cupones.Add(cupones);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.MostrarAfiliados = ID_Afiliados.Count > 1;
            ViewBag.ID_Afiliado = new SelectList(db.Afiliados.Where(r => ID_Afiliados.Contains(r.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_Afiliado", "Nombre", cupones.ID_Afiliado);
            return View(cupones);
        }

        // GET: Cupones/Edit/5
        [HasPermission("Configuraciones_Edicion")]
        [RedirectOnError]
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cupones cupones = db.Cupones.Find(id);
            if (cupones == null)
            {
                TempData["Mess"] = MensajeNotFound;
                TempData["NavBar"] = "NavBar_CatCupones";
                TempData["BackLink"] = "Index";

                return RedirectToAction("ItemNotFound");
            }
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.MostrarAfiliados = ID_Afiliados.Count > 1;
            ViewBag.ID_Afiliado = new SelectList(db.Afiliados.Where(r => ID_Afiliados.Contains(r.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_Afiliado", "Nombre", cupones.ID_Afiliado);
            return View(cupones);
        }

        // POST: Cupones/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [RedirectOnError]
        [OutputCache(NoStore = true, Duration = 0)]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Edit([Bind(Include = "ID_Cupon,Descripcion,Codigo,Cantidad,Fecha_Inicio,Fecha_Fin,Descuento,Primer_Servicio,ID_Afiliado,Fecha_Creacion,Fecha_Actualizacion,UserID")] Cupones cupones)
        {
            
            valid.ValidarCupon(ModelState, cupones, "edit");
            if (ModelState.IsValid)
            {
                cupones.Fecha_Actualizacion = util.ConvertToMexicanDate(DateTime.Now);
                cupones.UserID = User.Identity.GetUserId();
                db.Entry(cupones).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.MostrarAfiliados = ID_Afiliados.Count > 1;
            ViewBag.ID_Afiliado = new SelectList(db.Afiliados.Where(r => ID_Afiliados.Contains(r.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_Afiliado", "Nombre", cupones.ID_Afiliado);
            return View(cupones);
        }

        // GET: Cupones/Delete/5
        [HasPermission("Configuraciones_Edicion")]
        [RedirectOnError]
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cupones cupones = db.Cupones.Find(id);
            if (cupones == null)
            {
                TempData["Mess"] = MensajeNotFound;
                TempData["NavBar"] = "NavBar_CatCupones";
                TempData["BackLink"] = "Index";

                return RedirectToAction("ItemNotFound");
            }
            ViewBag.Mess = MensajeDelete;
            return View(cupones);
        }

        // POST: Cupones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult DeleteConfirmed(int id)
        {
            serv.EliminarCupon(id);
            TempData["Delete"] = true;
            return RedirectToAction("Index");
        }
    }
}
