using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GoPS.Models;
using GoPS.Classes;
 
using Microsoft.AspNet.Identity;
using GoPS.CustomFilters;

namespace GoPS.Controllers
{
    [RedirectOnError]
    [OutputCache(NoStore = true, Duration = 0)]
    [ValidateInput(false)]
    public class TiposPuntosInteresController : Controller
    {
        private GoPSEntities db = new GoPSEntities();
        DBServicios serv = new DBServicios();
        DBValidaciones valid = new DBValidaciones();
        Utilities util = new Utilities();

        // GET: TiposPuntosInteres
        [HasPermission("Configuraciones_Visualizacion")]
        public ActionResult Index()
        {
            if (TempData["Delete"] != null)
            {
                ViewBag.Delete = true;
                TempData.Remove("Delete");
            }
            return View(db.TiposPuntosInteres.ToList());
        }

        // GET: TiposPuntosInteres/Details/5
        [HasPermission("Configuraciones_Visualizacion")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TiposPuntosInteres tiposPuntosInteres = db.TiposPuntosInteres.Find(id);
            if (tiposPuntosInteres == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
            }
            return View(tiposPuntosInteres);
        }

        // GET: TiposPuntosInteres/Create
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: TiposPuntosInteres/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Create([Bind(Include = "ID_TipoPuntoInteres,Nombre,Fecha_Creacion,Fecha_Actualizacion,UserID")] TiposPuntosInteres tiposPuntosInteres)
        {
            valid.ValidarTipoPuntoInteres(ModelState, tiposPuntosInteres);
            if (ModelState.IsValid)
            {
                tiposPuntosInteres.Fecha_Creacion = util.ConvertToMexicanDate(DateTime.Now);
                tiposPuntosInteres.UserID = User.Identity.GetUserId();
                db.TiposPuntosInteres.Add(tiposPuntosInteres);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tiposPuntosInteres);
        }

        // GET: TiposPuntosInteres/Edit/5
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TiposPuntosInteres tiposPuntosInteres = db.TiposPuntosInteres.Find(id);
            if (tiposPuntosInteres == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
            }
            return View(tiposPuntosInteres);
        }

        // POST: TiposPuntosInteres/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Edit([Bind(Include = "ID_TipoPuntoInteres,Nombre,Fecha_Creacion,Fecha_Actualizacion,UserID")] TiposPuntosInteres tiposPuntosInteres)
        {
            valid.ValidarTipoPuntoInteres(ModelState, tiposPuntosInteres);
            if (ModelState.IsValid)
            {
                tiposPuntosInteres.Fecha_Actualizacion = util.ConvertToMexicanDate(DateTime.Now);
                tiposPuntosInteres.UserID = User.Identity.GetUserId();
                db.Entry(tiposPuntosInteres).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tiposPuntosInteres);
        }

        // GET: TiposPuntosInteres/Delete/5
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TiposPuntosInteres tiposPuntosInteres = db.TiposPuntosInteres.Find(id);
            if (tiposPuntosInteres == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
            }
            return View(tiposPuntosInteres);
        }

        // POST: TiposPuntosInteres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult DeleteConfirmed(int id)
        {
            serv.EliminarTipoPuntoInteres(id);
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
